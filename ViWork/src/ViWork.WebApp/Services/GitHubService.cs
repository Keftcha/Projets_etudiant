using System.Collections.Generic;
using System.Threading.Tasks;
using ViWork.DAL;

namespace ViWork.WebApp.Services
{
    public class GitHubService
    {
        readonly GitHubClient _gitHubClient;
        readonly UserGateway _userGateway;

        public GitHubService( GitHubClient gitHubClient, UserGateway userGateway )
        {
            _gitHubClient = gitHubClient;
            _userGateway = userGateway;
        }
    }
}
