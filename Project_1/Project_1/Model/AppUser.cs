﻿using System.Text.Json.Serialization;
using Project_1.DAL;



namespace Project_1.Model
{
    public class AppUser
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



        // Constructor
        public AppUser(int id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;

        }

        public AppUser() { }

        public static List<AppUser> Read()
        {
            //return UsersList;
            DBservices dbs = new DBservices();
            return dbs.Read();
        }

        public int Register()

        {
            List<AppUser> users = Read();
            foreach (AppUser user in users)
            {
                if (this.Email == user.Email)
                {
                    return 0;
                }
            }

            DBservices dbs = new DBservices();
            return dbs.Register(this);

        }

        public bool Login(string Email, string Password)
        {


            DBservices dbs = new DBservices();

            foreach (AppUser tempuser in dbs.Read())
            {
                if (tempuser.Email == Email && tempuser.Password == Password)
                {
                    Console.WriteLine("user exists");
                    return true;
                }

            }
            return false;
        }
      
        public (int id, string name) GetUserIdByEmail(string Email)
        {

            DBservices dbs = new DBservices();
            return dbs.GetUserIdByEmail(Email);

        }


        public AppUser Update(AppUser user)
        {
            DBservices dbs = new DBservices();
            foreach (AppUser tempuser in dbs.Read())
            {
                if(tempuser.Id==user.Id && tempuser.Email==user.Email&&tempuser.Password==user.Password)
                {
                    return null;///משתמש לא נמצא או שיש כבר משתמש כזה
                }
                return dbs.UpdateUser(user);
            }
            return dbs.UpdateUser(user);
        }





        //public int Update()
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.Update(this);

        //}

    }
}
