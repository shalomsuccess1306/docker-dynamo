using Docker_Dynamo.Data;
using Docker_Dynamo.Models;
using DockerDynamo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Docker_Dynamo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthServices _authServices;
        public Auth(AppDbContext context, IAuthServices auth)
        {
          
            _context = context;
            _authServices = auth;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] UserDTO model)
        {
            var user = await  _authServices.RegisterUser(model);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
           var token = await _authServices.loginUser(model);
            return Ok(token);
        }

       

    }
}
