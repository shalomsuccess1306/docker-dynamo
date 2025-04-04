using CommonOps;
using Docker_Dynamo.Data;
using Docker_Dynamo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DockerDynamo.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthServices(AppDbContext context, IConfiguration configuration )
        {
            _configuration = configuration;
            _context = context;
            
        }
        public async Task<StandardResponse<string>> loginUser(LoginModel loginModel)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginModel.Email);
            if(user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
                return StandardResponse<string>.ErrorMessage("User Not Found");

            // Generate JWT
            var token = GenerateJwtToken(user);
            return StandardResponse<string>.SuccessMessage("Successful", token);
        }

        public async Task<StandardResponse<UserDTO>> RegisterUser(UserDTO model)
        {
            var newUser = new User();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.EmailAddress);
            if (user != null) return StandardResponse<UserDTO>.ErrorMessage("User Already Exist");

            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            newUser.Email = model.EmailAddress;
            newUser.Name = model.FullName;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return StandardResponse<UserDTO>.SuccessMessage("Successful", newUser);

        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
   
}
