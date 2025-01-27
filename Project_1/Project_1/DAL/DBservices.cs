using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Project_1.Model;





namespace Project_1.DAL
{


    public class DBservices
    {





        //
        // TODO: Add constructor logic here
        //


        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString(conString);
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }

        //--------------------------------------------------------------------------------------------------
        // This method update a student to the student table 
        //--------------------------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------------------------
        // This method inserts a user to the users table 
        //--------------------------------------------------------------------------------------------------
        public int Register(AppUser user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@UserName", user.Name);
            paramDic.Add("@PasswordUser", user.Password);

            cmd = CreateCommandWithStoredProcedureGeneral("AddNewUser", con, paramDic);          // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------


        // read from a table
        public List<AppUser> Read()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<AppUser> users = new List<AppUser>();


            cmd = CreateCommandWithStoredProcedureGeneral("GetAllUsers", con, null);

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                AppUser u = new AppUser();
                u.Id = Convert.ToInt32(dataReader["ID"]);
                u.Name = dataReader["UserName"].ToString();
                u.Email = dataReader["Email"].ToString();
                u.Password = dataReader["PasswordUser"].ToString();
                users.Add(u);

            }
            return users;

        }

        public (int id, string name, bool isActive) GetUserIdByEmail(string email)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@Email", email);

                cmd = CreateCommandWithStoredProcedureGeneral("GetUserIdByEmail", con, paramDic);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    int id = Convert.ToInt32(dataReader["id"]);
                    string name = dataReader["UserName"].ToString();
                    bool isActive = Convert.ToBoolean(dataReader["isActive"]);


                    return (id, name, isActive); // החזר Tuple עם UserId ו-Name
                }

                return (-1, null, false); // מזהה לא נמצא
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Game> ReadAllGames()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            cmd = CreateCommandWithStoredProcedureGeneral("ReadAllGames", con, null);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.ReleaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.HeaderImage = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Publisher = dataReader["Developers"].ToString(); // Assuming "Developers" aligns with "Publisher"
                    g.NumberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);

                    games.Add(g);
                }
                return games;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<Game> UserGamesListById(int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", id);

            cmd = CreateCommandWithStoredProcedureGeneral("GetGamesByUserId", con, paramDic);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.ReleaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.HeaderImage = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Publisher = dataReader["Developers"].ToString(); // Assuming "Developers" aligns with "Publisher"
                    g.NumberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    games.Add(g);
                }
                return games;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<Game> GetByPrice(int id, double Price)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", id);
            paramDic.Add("@Price", Price);


            cmd = CreateCommandWithStoredProcedureGeneral("GetGamesByPrice", con, paramDic);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.ReleaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.HeaderImage = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Publisher = dataReader["Developers"].ToString(); // Assuming "Developers" aligns with "Publisher"
                    g.NumberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    games.Add(g);
                }
                return games;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<Game> GetByminScore(int id, int minScore)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", id);
            paramDic.Add("@minScore", minScore);

            cmd = CreateCommandWithStoredProcedureGeneral("GetGamesByUserAndScore", con, paramDic);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.ReleaseDate = Convert.ToDateTime(dataReader["Release_date"]);
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.HeaderImage = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                    g.ScoreRank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Publisher = dataReader["Developers"].ToString(); // Assuming "Developers" aligns with "Publisher"
                    g.NumberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    games.Add(g);
                }
                return games;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int AddGameToList(int UserId, int GameId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", UserId);
            paramDic.Add("@GameId", GameId);


            cmd = CreateCommandWithStoredProcedureGeneral("AddGameToUserList", con, paramDic);          // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public bool DeleteGameFromList(int userid, int gameid)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", userid);
            paramDic.Add("@GameId", gameid);

            cmd = CreateCommandWithStoredProcedureGeneral("DeleteGameFromUserList", con, paramDic);          // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return (numEffected > 0);

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public AppUser UpdateUser(AppUser user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                // יצירת חיבור למסד הנתונים
                con = connect("igroup8_test2");
            }
            catch (Exception ex)
            {
                throw (ex); // כתיבת שגיאה ללוג
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", user.Id); // שם הפרמטר לפי הפורמט שלך
            paramDic.Add("@UserName", user.Name);
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@Password", user.Password);

            cmd = CreateCommandWithStoredProcedureGeneral("UpdateUser", con, paramDic);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.Read())
                {
                    // מילוי אובייקט המשתמש עם הנתונים המעודכנים
                    user.Id = Convert.ToInt32(dataReader["ID"]);
                    user.Name = dataReader["UserName"].ToString();
                    user.Email = dataReader["Email"].ToString();
                    user.Password = dataReader["PasswordUser"].ToString();
                    return user;
                }
                else
                {
                    return null; // אם לא נמצאו תוצאות
                }
            }
            catch (Exception ex)
            {
                throw (ex); // כתיבת שגיאה ללוג
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // סגירת החיבור למסד הנתונים
                }
            }
        }

        public int changeIsActive(int UserId, bool isActive)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                // יצירת חיבור למסד הנתונים
                con = connect("igroup8_test2");
            }
            catch (Exception ex)
            {
                throw (ex); // כתיבת שגיאה ללוג
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", UserId); // שם הפרמטר לפי הפורמט שלך
            paramDic.Add("@isActive", isActive);


            cmd = CreateCommandWithStoredProcedureGeneral("changeIsActive", con, paramDic);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.Read())
                {
                    // מילוי אובייקט המשתמש עם הנתונים המעודכנים
                    UserId = Convert.ToInt32(dataReader["ID"]);
                    isActive = Convert.ToBoolean(dataReader["isActive"]);


                    return 1;
                }
                else
                {
                    return 0; // אם לא נמצאו תוצאות
                }
            }
            catch (Exception ex)
            {
                throw (ex); // כתיבת שגיאה ללוג
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // סגירת החיבור למסד הנתונים
                }
            }
        }

        public List<Object> GetUserDetails()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            cmd = CreateCommandWithStoredProcedureGeneral("getUserPayAmountTable", con, paramDic);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Object> users = new List<Object>();

            while (dataReader.Read())
            {
                users.Add(new
                {
                    Id = Convert.ToInt32(dataReader["UserID"]),
                    Name = dataReader["UserName"].ToString(),
                    Numofgames = Convert.ToInt32(dataReader["Numofgames"]),
                    isActive = Convert.ToBoolean(dataReader["isActive"]),
                    Totalprice = Convert.ToInt32(dataReader["Totalprice"])





                });


            }
            return users;

        }

        public List<Object> GetGameDetails()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup8_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            cmd = CreateCommandWithStoredProcedureGeneral("getGamesDownloadsAmount", con, paramDic);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Object> games = new List<Object>();

            while (dataReader.Read())
            {
                games.Add(new
                {
                    AppID = Convert.ToInt32(dataReader["AppID"]),
                    Categories = dataReader["Categories"].ToString(),
                    numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]),
                    totalamount = Convert.ToInt32(dataReader["totalamount"]),
                    





                });


            }
            return games;

        }



    }

}

