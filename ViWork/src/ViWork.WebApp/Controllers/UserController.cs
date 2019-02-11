using System.Collections.Generic;
using System.Threading.Tasks;
using ViWork.DAL;
using ViWork.WebApp.Authentication;
using ViWork.WebApp.Models.AccountViewModels;
using ViWork.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViWork.WebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class UserController : Controller
    {
        readonly UserGateway _userGateway;

        public UserController( UserGateway userGateway)
        {
            _userGateway = userGateway;
        }

        [HttpGet("FindByEmail")]
        public async Task<IActionResult> FindByEmail(string email)
        {
            UserData result = await _userGateway.FindByEmail(email);
            return Ok(result);

        }
    }
}
