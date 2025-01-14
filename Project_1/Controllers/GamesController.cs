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
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return gamesList; // Return the list of available games
        }

        // GET api/Games/GetByPrice (Filter games by price)
        [HttpGet("GetByPrice")]
        public IActionResult GetByPrice(double minPrice)
        {
            // Filter games by price greater than minPrice
            var filteredGames = myGamesList.Where(game => game.Price > minPrice).ToList();

            // If no games match, return a 404 Not Found status
            if (!filteredGames.Any())
            {
                return NotFound($"No games found with a price greater than {minPrice}.");
            }

            // Return the filtered list of games with a 200 OK status
            return Ok(filteredGames);
        }

        // GET api/Games/minScore (Filter games by score)
        [HttpGet("minScore")]
        public IActionResult GetByRankScore(int minScore)
        {
            // Filter games by score greater than minScore
            var filteredGames = myGamesList.Where(game => game.ScoreRank > minScore).ToList();

            // If no games match, return a 404 Not Found status
            if (!filteredGames.Any())
            {
                return NotFound($"No games found with a score greater than {minScore}.");
            }

            // Return the filtered list of games with a 200 OK status
            return Ok(filteredGames);
        }

        // GET api/<GamesController>/5 (Get game by ID)
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var game = gamesList.FirstOrDefault(g => g.AppID == id);
            if (game != null)
                return Ok(game);

            return NotFound($"Game with ID {id} not found.");
        }

        // POST api/Games (Add a new game)
        [HttpPost]
        public IActionResult Post([FromBody] Game newGame)
        {
            if (newGame == null)
                return BadRequest("Game data is invalid.");

            if (gamesList.Any(g => g.AppID == newGame.AppID))
                return Conflict($"A game with AppID {newGame.AppID} already exists.");

            gamesList.Add(newGame);
            return CreatedAtAction(nameof(Get), new { id = newGame.AppID }, newGame);
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
        [HttpPost("AddToMyList")]
        public IActionResult AddToMyList([FromBody] Game newGame)
        {
            if (newGame == null)
                return BadRequest("Game data is invalid.");

            // Check if the game is already in the user's list
            if (myGamesList.Any(g => g.AppID == newGame.AppID))
                return Ok(new { Message = $"The game with AppID {newGame.AppID} is already in your list." } );

            myGamesList.Add(newGame);
            return Ok(new { Message = $"The game {newGame.Name} has been added to your list." });

        }

        // GET api/Games/MyList (Returns the user's list of added games)
        [HttpGet("MyList")]
        public ActionResult<IEnumerable<Game>> GetMyList()
        {
            return Ok(myGamesList); // Return the list of games added by the user
        }
    }
}
