using Microsoft.AspNetCore.Mvc;
using ChatBackend.Data;
using ChatBackend.Models;

namespace ChatBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                return BadRequest(new { message = "Username is required." });

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }
    }
}
