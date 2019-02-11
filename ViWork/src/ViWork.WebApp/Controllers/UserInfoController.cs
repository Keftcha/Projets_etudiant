using System.Collections.Generic;
using System.Threading.Tasks;
using ViWork.DAL;
using ViWork.WebApp.Authentication;
using ViWork.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViWork.WebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class UserInfoController : Controller
    {
        readonly UserGateway _userGateway;

        public UserInfoController( UserGateway userGateway)
        {
            _userGateway = userGateway;
        }

        //[HttpGet( "{email}", Name = "GetUserInfo")]
        //public async Task<IActionResult> FindByEmail(string email)
        //{
        //    Result<UserData> result = await _userGateway.FindByEmail(email);
        //    return this.CreateResult(result);
        //}
    }
}
