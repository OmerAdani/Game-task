using Microsoft.AspNetCore.Mvc;
using Project_1.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Steam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // Static list for games (usually, you'd use a database in production)
        private static List<Game> gamesList = new List<Game>
        {
            new Game(
                appID: 123,
                name: "Pristontale",
                releaseDate: new DateTime(2002, 2, 1),
                price: 19.99,
                description: "A classic MMORPG",
                headerImage: "https://example.com/pristontale.jpg",
                website: "https://pristontale.com",
                windows: true,
                mac: false,
                linux: false,
                scoreRank: 85,
                recommendations: "Highly recommended for MMORPG fans",
                publisher: "Publisher Name"
            )
        };

        // Static list to store the user's added games.
        private static List<Game> myGamesList = new List<Game>();

        // GET: api/<GamesController>
        [HttpGet("GetAllGames")]
        public IEnumerable<Game> GetAllGames()
        {
            return Game.ReadAllGames(); // Return the list of available games
        }

        // GET api/Games/MyList (Returns the user's list of added games)
        [HttpGet("{id}")]
        public List<Game> UserGamesListById(int id)
        {
            return Game.UserGamesListById(id);
        }


        // GET api/Games/GetByPrice (Filter games by price)
        [HttpGet("GetByPrice/{id}/{minPrice}")]
        public List<Game> GetByPrice(int id, double Price)
        {
            Game game = new Game();
            return Game.GetByPrice(id, Price);
        }

        // GET api/Games/minScore (Filter games by score)
        [HttpGet("minScore/{id}/{minScore}")]
        public List<Game> GetByminScore(int id, int minScore)
        {
            Game game = new Game();
            return Game.GetByminScore(id, minScore);
        }




        // DELETE api/Games/DeleteById (Delete a game by ID)
        [HttpDelete("DeleteById")]
        public IActionResult DeleteById(int id)
        {
            // Find the game in the user's personal list (myGamesList)
            var game = myGamesList.FirstOrDefault(g => g.AppID == id);

            if (game != null)
            {
                // Remove the game from myGamesList
                myGamesList.Remove(game);
                return Ok($"Game with ID {id} has been successfully deleted from your list.");
            }

            return NotFound($"Game with ID {id} not found in your list.");
        }


        // POST api/Games/AddToMyList (Add a game to the user's personal list)
        [HttpPost("AddGameToList")]
        public IActionResult AddGameToList(int userId, int gameId)
        {
            Game g = new Game();

            // נסה להוסיף את המשחק לרשימה
            var isGameAdded = g.AddGameToList(userId, gameId);

            // אם הפעולה הצליחה, החזר הודעת הצלחה
            if (isGameAdded)
            {
                var response = new
                {
                    Status = "Success",
                    Message = $"The game with ID {gameId} was added successfully for user {userId}."
                };
                return Ok(response);
            }

            // אם המשחק כבר קיים או התהליך נכשל, החזר הודעת שגיאה
            var errorResponse = new
            {
                Status = "Conflict",
                Message = $"Unable to add the game. It may already exist in the library or the process failed.",
                UserId = userId,
                GameId = gameId
            };
            return Conflict(errorResponse);
        }

        // DELETE api/Games/RemoveFromList (Remove a game from the user's personal list)
        [HttpDelete("DeleteGameFromList")]
        public IActionResult RemoveGameFromList(int userId, int gameId)
        {
            Game g = new Game();

            // נסה להסיר את המשחק מהרשימה
            var isGameRemoved = g.DeleteGameFromList(userId, gameId);

            // אם הפעולה הצליחה, החזר הודעת הצלחה
            if (isGameRemoved)
            {
                var response = new
                {
                    Status = "Success",
                    Message = $"The game with ID {gameId} was removed successfully for user {userId}."
                };
                return Ok(response);
            }

            // אם המשחק לא נמצא או התהליך נכשל, החזר הודעת שגיאה
            var errorResponse = new
            {
                Status = "Conflict",
                Message = $"Unable to remove the game. It may not exist in the library or the process failed.",
                UserId = userId,
                GameId = gameId
            };
            return Conflict(errorResponse);
        }







    }
}
