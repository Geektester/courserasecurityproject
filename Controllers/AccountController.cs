using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SafeVault.Models;

namespace SafeVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SafeVaultDb");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@user AND PasswordHash=@pass", conn);
            cmd.Parameters.AddWithValue("@user", model.Username);
            cmd.Parameters.AddWithValue("@pass", HashPassword(model.Password));

            int count = (int)cmd.ExecuteScalar();
            if (count == 0)
                return Unauthorized();

            var token = JwtHelper.GenerateToken(model.Username, "User");
            return Ok(new { Token = token });
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
