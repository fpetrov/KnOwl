using KnOwl.Database.Contexts;
using KnOwl.Entities.Authentication;
using KnOwl.Models.Authentication;
using KnOwl.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnOwl.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Entities.Authentication.User> Add(Entities.Authentication.User entity)
        {
            if (await _context.Set<Entities.Authentication.User>().AnyAsync(u => u.Name == entity.Name))
                return null;

            _context.Set<Entities.Authentication.User>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Entities.Authentication.User> Remove(int id)
        {
            var entity = await _context.Set<Entities.Authentication.User>().FindAsync(id);

            if (entity == null)
                return null;

            _context.Set<Entities.Authentication.User>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Entities.Authentication.User> Get(int id)
        {
            return await _context.Set<Entities.Authentication.User>().Include(u => u.Articles).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Entities.Authentication.User> Get(string name)
        {
            return await _context.Set<Entities.Authentication.User>().Include(u => u.Articles).FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<List<Entities.Authentication.User>> GetAll()
        {
            return await _context.Set<Entities.Authentication.User>().Include(u => u.Articles).ToListAsync();
        }

        public async Task<Entities.Authentication.User> Update(Entities.Authentication.User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, Func<Entities.Authentication.User, bool> passwordVerification, JwtOptions options)
        {
            var user = await _context.Set<Entities.Authentication.User>().SingleOrDefaultAsync(u => u.Name == request.Name);

            if (user == null || !passwordVerification.Invoke(user))
                return null;

            var jwt = GenerateJwt(user, options);
            var refreshToken = GenerateRefreshToken(request.Fingerprint, options);

            user.RefreshTokens.Add(refreshToken);

            await Update(user);

            return new AuthenticateResponse(user, jwt, refreshToken.Token);
        }

        public async Task<bool> RevokeToken(RefreshTokenRequest request)
        {
            var user = await _context.Set<Entities.Authentication.User>().SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == request.Token);

            if (!refreshToken.IsActive && request.Fingerprint != refreshToken.Fingerprint)
                return false;

            user.RefreshTokens.Remove(refreshToken);

            await Update(user);

            return true;
        }

        public async Task<AuthenticateResponse> RefreshToken(RefreshTokenRequest request, JwtOptions options)
        {
            var user = await _context.Set<Entities.Authentication.User>().SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token));

            if (user == null)
                return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == request.Token);

            if (!refreshToken.IsActive && request.Fingerprint != refreshToken.Fingerprint)
                return null;

            var newRefreshToken = GenerateRefreshToken(request.Fingerprint, options);

            user.RefreshTokens.Remove(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);

            var jwt = GenerateJwt(user, options);

            await Update(user);

            return new AuthenticateResponse(user, jwt, newRefreshToken.Token);
        }

        #region Additional Functions.
        private string GenerateJwt(Entities.Authentication.User user, JwtOptions options)
        {
            var jwt = new JwtSecurityToken(
                    issuer: options.Issuer,
                    audience: options.Audience,
                    notBefore: DateTime.Now,
                    claims: GetClaims(user).Claims,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(options.LifeTime)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.ServerSecret)), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            return encodedJwt;
        }

        private ClaimsIdentity GetClaims(Entities.Authentication.User user)
        {
            var claims = new List<Claim>
            {
                new Claim("name", user.Name),
                new Claim("role", user.Role.ToString())
            };
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", "name", "role");

            return claimsIdentity;
        }

        private RefreshToken GenerateRefreshToken(string fingerprint, JwtOptions options)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];

                rngCryptoServiceProvider.GetBytes(randomBytes);

                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.Now.AddDays(options.RefreshTokenLifeTime),
                    Fingerprint = fingerprint,
                    Created = DateTime.Now
                };
            }
        }
        #endregion
    }
}
