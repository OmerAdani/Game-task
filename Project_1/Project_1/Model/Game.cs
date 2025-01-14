using Project_1.DAL;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Project_1.Model
{
    public class Game
    {
        int appID;
        string name;
        DateTime releaseDate;
        double price;
        string description;
        string headerImage;
        string website;
        bool windows;
        bool mac;
        bool linux;
        int scoreRank;
        string recommendations;
        string publisher;
        int numberOfPurchases;
        private static List<Game> gamesList;

        [JsonPropertyName("AppID")]
        public int AppID { get => appID; set => appID = value; }
        [JsonPropertyName("Name")]
        public string Name { get => name; set => name = value; }
        [JsonPropertyName("ReleaseDate")]
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }
        [JsonPropertyName("Price")]
        public double Price { get => price; set => price = value; }
        [JsonPropertyName("Description")]
        public string Description { get => description; set => description = value; }
        [JsonPropertyName("HeaderImage")]
        public string HeaderImage { get => headerImage; set => headerImage = value; }
        [JsonPropertyName("Website")]
        public string Website { get => website; set => website = value; }
        [JsonPropertyName("Windows")]
        public bool Windows { get => windows; set => windows = value; }
        [JsonPropertyName("Mac")]
        public bool Mac { get => mac; set => mac = value; }
        [JsonPropertyName("Linux")]
        public bool Linux { get => linux; set => linux = value; }
        [JsonPropertyName("ScoreRank")]
        public int ScoreRank { get => scoreRank; set => scoreRank = value; }
        [JsonPropertyName("Recommendations")]
        public string Recommendations { get => recommendations; set => recommendations = value; }
        [JsonPropertyName("Publisher")]
        public string Publisher { get => publisher; set => publisher = value; }
        [JsonPropertyName("GamesList")]
        public int NumberOfPurchases { get => numberOfPurchases; set => numberOfPurchases = value; }
        //[JsonPropertyName("GamesList")]
        //public static List<Game> GamesList { get => gamesList; set => gamesList = value; }

        public Game(int appID, string name, DateTime releaseDate, double price, string description, string headerImage,
              string website, bool windows, bool mac, bool linux, int scoreRank,
              string recommendations, string publisher)
        {
            AppID = appID;
            Name = name;
            ReleaseDate = releaseDate;
            Price = price;
            Description = description;
            HeaderImage = headerImage;
            Website = website;
            Windows = windows;
            Mac = mac;
            Linux = linux;
            ScoreRank = scoreRank;
            Recommendations = recommendations;
            Publisher = publisher;
        }
        public Game() { }
        public static bool Insert(Game game)
        {

            if (gamesList.Exists(g => g.appID == game.appID || g.name.Equals(game.name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }


            gamesList.Add(game);
            return true;
        }

        public static List<Game> ReadAllGames()
        {
            DBservices dbs = new DBservices();

            return dbs.ReadAllGames();
        }



        public static List<Game> UserGamesListById(int id)
        {
            DBservices dbs = new DBservices();

            return dbs.UserGamesListById(id);
        }


        public static List<Game> GetByPrice(int id, double Price)
        {
            DBservices dbs = new DBservices();

            return dbs.GetByPrice(id, Price);
        }

        public static List<Game> GetByminScore(int id, int minScore)
        {
            DBservices dbs = new DBservices();

            return dbs.GetByminScore(id, minScore);
        }

        public bool AddGameToList(int userId, int gameId)
        {
            // יצירת אובייקט של DBServices
            DBservices dbs = new DBservices();

            try
            {
                // שליחת בקשה להוספת המשחק למסד הנתונים
                int affectedRows = dbs.AddGameToList(userId, gameId);

                // בדיקה אם נוספו שורות
                return affectedRows > 0; //  true אם נוספה לפחות שורה אחת מחזיר
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה (לדוגמה: שמירת השגיאה ללוג)
                Console.WriteLine($"Failed to add game to user list. Error: {ex.Message}");
                return false; // במקרה של שגיאה, מחזיר false
            }
        }

        public bool DeleteGameFromList(int userId, int gameId)
        {
            DBservices dbs = new DBservices(); // יצירת אובייקט של DBServices

            return dbs.DeleteGameFromList(userId,gameId);
        }







    }

}

