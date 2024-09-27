using APIDailyPlanner.Data;
using APIDailyPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIDailyPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UsersController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //regis
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] Users user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Email and password are required.");
            }

            // Lưu mật khẩu thẳng vào cơ sở dữ liệu (Không an toàn)
            var newUser = new Users
            {
                Email = user.Email,
                Password = user.Password,
                StudentID = user.StudentID, // Nếu có thông tin này
                Name = user.Name, // Nếu có thông tin này
                ProfilePicture = user.ProfilePicture, // Nếu có thông tin này
                IsDarkMode = false, // Giá trị mặc định
                Language = "vi" // Giá trị mặc định
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered successfully!");
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Tìm người dùng theo email
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null || user.Password != request.Password) // So sánh trực tiếp
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Tạo token JWT
            var token = GenerateJwtToken(user);

            // Cập nhật thời gian đăng nhập lần cuối
            user.LastLogin = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(new
            {
                token = token,
                user = new
                {
                    user.UserID,
                    user.Email,
                    user.Name,
                    user.ProfilePicture,
                    user.IsDarkMode,
                    user.Language
                }
            });
        }

    private string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
