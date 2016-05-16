using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http.Authentication;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace task2.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var issuer = "https://www.upwork.com";
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Dima", ClaimValueTypes.String, issuer));
            var userIdentity = new ClaimsIdentity("TestLogin");
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.Authentication.SignInAsync("Cookie", userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                    IsPersistent = false,
                    AllowRefresh = false
                });

            return Ok();
        }
    }
}
