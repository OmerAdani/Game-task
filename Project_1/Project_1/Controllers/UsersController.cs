using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable <AppUser> GetRead()
        {
           
            return AppUser.Read();
        }



        // GET: api/Users/5 (Retrieve user by ID)
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    var user = ListUpUser.FirstOrDefault(u => u.Id == id);
        //    if (user != null)
        //    {
        //        return Ok(user);
        //    }
        //    return NotFound($"User with ID {id} not found.");
        //}

      

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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



        [HttpPost("PostLogin")]
        public int Login( string Email,string Password)
        {

            AppUser user = new AppUser();
            return user.GetUserIdByEmail(Email);
            //if (user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            //{
            //    return BadRequest(new { message = "Email and password are required." });
            //}

            //try
            //{
            //    AppUser appUser = new AppUser();
            //    bool isLoggedIn = appUser.Login(user.Email, user.Password);

            //    if (isLoggedIn)
            //    {
            //        // Retrieve user ID by email
            //        int userId = appUser.GetUserIdByEmail(user.Email);

            //        if (userId > 0)
            //        {
            //            return Ok(new
            //            {
            //                message = "Login successful",
            //                UserId = userId
            //            });
            //        }

            //        return NotFound(new { message = "User ID not found." });
            //    }

            //    return Unauthorized(new { message = "Invalid email or password." });
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new
            //    {
            //        message = "Internal server error occurred.",
            //        error = ex.Message
            //    });
            //}
        }




    }
}
