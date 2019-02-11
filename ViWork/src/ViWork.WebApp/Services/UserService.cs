using System.Collections.Generic;
using System.Threading.Tasks;
using ViWork.DAL;

namespace ViWork.WebApp.Services
{
    public class UserService
    {
        readonly UserGateway _userGateway;
        readonly PasswordHasher _passwordHasher;

        public UserService(UserGateway userGateway, PasswordHasher passwordHasher)
        {
            _userGateway = userGateway;
            _passwordHasher = passwordHasher;
        }

        public Task<Result<int>> CreatePasswordUser(string email, string password, string firstname, string lastname)
        {
            return _userGateway.CreatePasswordUser(email, _passwordHasher.HashPassword(password), firstname, lastname);
        }

        public async Task<UserData> FindUser(string email, string password)
        {
            UserData user = await _userGateway.FindByEmail(email);
            if (user != null && _passwordHasher.VerifyHashedPassword(user.Password, password) == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }

        public async Task<UserData> FindByGithubId(int githubId)
        {
            UserData user = await _userGateway.FindByGithubId(githubId);
            if (user != null)
            {
                return user;
            }

            return null;
        }

        public async Task CreateOrUpdateGithubUser(string email, int githubId, string accessToken)
        {
            await _userGateway.CreateOrUpdateGithubUser(email, githubId, accessToken);          
        }


        public async Task<IEnumerable<string>> GetAuthenticationProviders(string userId)
        {
            return await _userGateway.GetAuthenticationProviders(userId);
        }
        
        public async Task Delete (int usrID)
        {
            await _userGateway.Delete(usrID);
        }

        public async Task UpdateEmail(int userId, string email)
        {
            await _userGateway.UpdateEmail(userId, email);
        }


        public async Task UpdatePassword(int userId, byte[] password)
        {
            await _userGateway.UpdatePassword(userId, password);
        }

    }
}
