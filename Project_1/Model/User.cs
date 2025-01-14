using System.Text.Json.Serialization;

namespace Project_1.Model
{
    public class User
    {
        // Properties
        [JsonPropertyName("ID")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Email")]
        public string Email { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }

        // Static list for storing users
        [JsonPropertyName("UsersList")]
        private static List<User> UsersList { get; set; } = new List<User>();

        // Constructor
        public User(int id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password; // Consider hashing the password here
        }
        public User()
        {

        }

        // Insert a user into the static list
        public bool Login(User user)
        {
            // Check for existing user with the same ID or Email
            if (UsersList.Any(u => u.Id == user.Id || u.Email == user.Email))
            {
                return false; // User already exists
            }

            return true;
        }
        public bool Register(User user)
        {
            if (user == null)
            {
                return false; // Return false if the user is null
            }

            UsersList.Add(user);  // Add the user to the list
            return true;  // Return true to indicate successful registration
        }

        // Retrieve all users
        public List<User> Read()
        {
            return UsersList;
        }

    
    }
}
