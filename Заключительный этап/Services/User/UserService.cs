using KnOwl.Models.Authentication;
using KnOwl.Repositories.User;
using KnOwl.Services.User;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;
        private readonly JwtOptions _jwtOptions;

        public UserService(IUserRepository userRepository, ISecurityService securityService, IOptions<JwtOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _securityService = securityService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<Entities.Authentication.User> Add(Entities.Authentication.User entity)
        {
            entity.Role = Entities.Authentication.Role.Admin;

            // Secure password for storing.
            entity.Password = _securityService.Hash(entity.Password);

            return await _userRepository.Add(entity);
        }

        public async Task<Entities.Authentication.User> Remove(int id)
        {
            return await _userRepository.Remove(id);
        }

        public async Task<Entities.Authentication.User> Get(int id)
        {
            return await _userRepository.Get(id);
        }

        public async Task<Entities.Authentication.User> Get(string name)
        {
            return await _userRepository.Get(name);
        }

        public async Task<List<Entities.Authentication.User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<Entities.Authentication.User> Update(Entities.Authentication.User entity)
        {
            return await _userRepository.Update(entity);
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var response = await _userRepository.Authenticate(request, u => _securityService.Verify(request.Password, u.Password), _jwtOptions);

            return response;
        }

        public async Task<bool> RevokeToken(RefreshTokenRequest request)
        {
            return await _userRepository.RevokeToken(request);
        }

        public async Task<AuthenticateResponse> RefreshToken(RefreshTokenRequest request)
        {
            return await _userRepository.RefreshToken(request, _jwtOptions);
        }
    }
}
