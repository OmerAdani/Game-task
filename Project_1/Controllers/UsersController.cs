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
        List<User> usersList = new List<User>();
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {


            /* usersList.Add(new User(
                 id: 123,
                 name: "Naorc",
                 email: "Naorc@gmail.com",
                 password: "naorfeigalah"
             ));*/
            User user = new User();
            return user.Read();
        }


        // GET: api/Users/5 (Retrieve user by ID)
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = usersList.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound($"User with ID {id} not found.");
        }

        // POST: api/Users (Add a new user)
        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("User data is invalid");
            }

            // Check if a user with the same ID or email already exists
            if (usersList.Any(u => u.Id == newUser.Id ))
            {
                return Conflict("A user with the same ID already exists");
            }

            usersList.Add(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

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
        public bool PostRegister([FromBody] User user)
        {
            return user.Register(user);
        }

        // Method to login an existing user
        [HttpPost("PostLogin")]
        public bool PostLogin([FromBody] User user)
        {
            return user.Login(user);
        }
    }
}
