using KnOwl.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Repositories.User
{
    public interface IUserRepository
    {
        Task<List<Entities.Authentication.User>> GetAll();
        Task<Entities.Authentication.User> Get(int id);
        Task<Entities.Authentication.User> Get(string name);
        Task<Entities.Authentication.User> Add(Entities.Authentication.User entity);
        Task<Entities.Authentication.User> Update(Entities.Authentication.User entity);
        Task<Entities.Authentication.User> Remove(int id);

        // Authentication.
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, Func<Entities.Authentication.User, bool> passwordVerification, JwtOptions options);
        Task<bool> RevokeToken(RefreshTokenRequest request);
        Task<AuthenticateResponse> RefreshToken(RefreshTokenRequest request, JwtOptions options);
    }
}
