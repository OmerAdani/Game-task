using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Project_1.Model;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Steam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<AppUser> GetRead()
        {
            return AppUser.Read();
        }

        // PUT api/<UsersController>/5
        
        [HttpPut("update/{id}")]
        public IActionResult Put(int id, [FromBody] AppUser user)
        {
            if (id != user.Id)
            {
                return BadRequest(new { message = "User ID mismatch" });
            }

            AppUser updatedUser = user.Update(user);

            if (updatedUser == null)
            {
                return Conflict(new { message = "User not found or email is already in use" });
            }

            return Ok(updatedUser);
        }



        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // Method to register a new user
        [HttpPost("PostRegister")]
        public IActionResult Post([FromBody] AppUser user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid user data." });
                }

                int result = user.Register();

                if (result == 1)
                {
                    return Ok(new { message = "User registered successfully." });
                }
                else if (result == 0)
                {
                    return Conflict(new { message = "Email is already registered." });
                }
                else
                {
                    return BadRequest(new { message = "Failed to register user." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        // Login method
        [HttpPost("PostLogin")]
        public IActionResult Login(string Email, string Password)
        {
            AppUser user = new AppUser();
            bool isLogin = user.Login(Email, Password);

            if (isLogin)
            {
                var (id, name) = user.GetUserIdByEmail(Email);
                
                if (id > 0)
                {
                    return Ok(new
                    {
                        id = id,
                        name = name
                    });
                }

                return NotFound(new { message = "User ID not found" });
            }

            return Unauthorized(new { message = "Invalid email or password" });
        }
    }
}
