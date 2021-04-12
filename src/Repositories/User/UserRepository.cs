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
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;

        public UserRepository(ApplicationContext context, ISecurityService securityService, IConfiguration configuration)
        {
            _context = context;
            _securityService = securityService;
            _configuration = configuration;
        }

        public async Task<Entities.Authentication.User> Add(Entities.Authentication.User entity)
        {
            if (await _context.Set<Entities.Authentication.User>().AnyAsync(u => u.Name == entity.Name))
                return null;

            // Secure password for storing.
            entity.Password = _securityService.Hash(entity.Password);

            _context.Set<Entities.Authentication.User>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Entities.Authentication.User> Remove(int id)
        {
            var entity = await _context.Set<Entities.Authentication.User>().FindAsync(id);

            if (entity == null)
                return entity;

            _context.Set<Entities.Authentication.User>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Entities.Authentication.User> Get(int id)
        {
            return await _context.Set<Entities.Authentication.User>().FindAsync(id);
        }

        public async Task<Entities.Authentication.User> Get(string name)
        {
            return await _context.Set<Entities.Authentication.User>().FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<List<Entities.Authentication.User>> GetAll()
        {
            return await _context.Set<Entities.Authentication.User>().ToListAsync();
        }

        public async Task<Entities.Authentication.User> Update(Entities.Authentication.User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var user = await _context.Set<Entities.Authentication.User>().SingleOrDefaultAsync(u => u.Name == request.Name);

            if (user == null || !_securityService.Verify(request.Password, user.Password))
                return null;

            var jwt = GenerateJwt(user);
            var refreshToken = GenerateRefreshToken(request.Fingerprint);

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

        public async Task<AuthenticateResponse> RefreshToken(RefreshTokenRequest request)
        {
            var user = await _context.Set<Entities.Authentication.User>().SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token));

            if (user == null)
                return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == request.Token);

            if (!refreshToken.IsActive && request.Fingerprint != refreshToken.Fingerprint)
                return null;

            var newRefreshToken = GenerateRefreshToken(request.Fingerprint);

            user.RefreshTokens.Remove(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);

            var jwt = GenerateJwt(user);

            await Update(user);

            return new AuthenticateResponse(user, jwt, newRefreshToken.Token);
        }

        #region Additional Functions.
        private string GenerateJwt(Entities.Authentication.User user)
        {
            var jwt = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    notBefore: DateTime.Now,
                    claims: GetClaims(user).Claims,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(int.Parse(_configuration["JWT:Lifetime"]))),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ServerSecret"])), SecurityAlgorithms.HmacSha256));

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

        private RefreshToken GenerateRefreshToken(string fingerprint)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];

                rngCryptoServiceProvider.GetBytes(randomBytes);

                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.Now.AddDays(15),
                    Fingerprint = fingerprint,
                    Created = DateTime.Now
                };
            }
        }
        #endregion
    }
}
