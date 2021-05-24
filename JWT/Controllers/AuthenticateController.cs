using System;
using JWT.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using JWT.Services;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private IAuthenticationService _service;

        public AuthenticateController(IAuthenticationService service)
        {
            _service = service;
        }        

        private async Task<UserModel> AuthenticateUser(UserModel model)
        {
            UserModel user = null;

            List<UserModel> users = new List<UserModel>();
            users.Add(new UserModel { UserName = "rakib.khan", Email = "rakib.cse.sust@gmail.com", Password = "@Aa123", DateOfBirth = new DateTime(1988, 10, 12) });
            users.Add(new UserModel { UserName = "afnan.khan", Email = "afnan.khan@gmail.com", Password = "@Aa123", DateOfBirth = new DateTime(2016, 01, 28) });

            user = users.FirstOrDefault(x => x.UserName == model.UserName);

            return await Task.FromResult(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserModel model)
        {
            var user = await AuthenticateUser(model);

            if (user != null) // && await userManager.CheckPasswordAsync(user, model.Password)
            {
                //var userRoles = await userManager.GetRolesAsync(user);

                return Ok(new
                {
                    token = _service.GenerateJWTToken(user)
                });
            }

            return Unauthorized();
        }
    }
}
