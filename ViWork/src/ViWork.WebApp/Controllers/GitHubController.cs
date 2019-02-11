using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ViWork.DAL;
using ViWork.WebApp.Authentication;
using ViWork.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViWork.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class GitHubController : Controller
    {
        readonly GitHubService _gitHubService;

        public GitHubController( GitHubService gitHubService )
        {
            _gitHubService = gitHubService;
        }
        
    }
}
