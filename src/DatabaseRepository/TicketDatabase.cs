using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketSystem.DatabaseRepository.Model;

namespace TicketSystem.DatabaseRepository
{
    public class TicketDatabase : ITicketDatabase
    {
        public User UserAdd(string firstName, string lastName, string password, string salt, string city, string address, int grade)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { @firstName = firstName, @lastName = lastName, @password = password, @salt = salt, @city = city, @address = address, @grade = grade };
                connection.Query("INSERT INTO Users(FirstName , LastName, Password, Salt, City, Address, Grade) values(@firstName, @lastName, @password @salt @city, @address, @grade)", data);
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<User>("SELECT * FROM Users WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public List<User> UserFind(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // problem, how do we get users where id=1 but not all users where grade=1 ? better code needed
                return connection.Query<User>("SELECT * FROM Users WHERE ID like '%" + query + "%' OR FirstName '%" + query + "%' OR LastName like '%" + query + "%' OR Password like '%" + query + "%' OR Salt like '%" + query + "%' OR City like '%" + query + "%' OR Address like '%" + query + "%' OR Grade like '%" + query + "%'").ToList();
            }
        }

        public User UserModify(string firstName, string lastName, string password, string salt, string city, string address, int grade, string id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (password != null)
                {
                    var data = new { @firstName = firstName, @lastName = lastName, @password = password, @salt = salt, @city = city, @address = address, @grade = grade, @id = id };
                    connection.Query("UPDATE Users SET FirstName=@firstName, LastName=@lastName, Password=@password Salt=@salt City=@city, Address=@address, Grade=@grade", data);
                }
                else
                {
                    var data = new { @firstName = firstName, @lastName = lastName, @city = city, @address = address, @grade = grade, @id = id };
                    connection.Query("UPDATE Users SET FirstName=@firstName, LastName=@lastName, City=@city, Address=@address, Grade=@grade WHERE ID = @id", data);
                }
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<User>("SELECT * FROM Users WHERE ID=@Id", new { Id = addedEventQuery }).First();                
            }
        }

        public bool UserDelete(string id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = String.Format("DELETE FROM Users Where ID={0}", id);
                SqlCommand command = new SqlCommand(Qry, connection);
                return Convert.ToBoolean(command.ExecuteNonQuery());
            }
        }


        //public Venue VenueAdd(string name, string address, string city, string country)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        connection.Query("insert into Venues([VenueName],[Address],[City],[Country]) values(@Name,@Address, @City, @Country)", new { Name = name, Address = address, City = city, Country = country });
        //        var addedVenueQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Venues') AS Current_Identity").First();
        //        return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueID=@Id", new { Id = addedVenueQuery }).First();
        //    }
        //}

        //public List<Venue> VenuesFind(string query)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueName like '%" + query + "%' OR Address like '%" + query + "%' OR City like '%" + query + "%' OR Country like '%" + query + "%'").ToList();
        //    }
        //}
    }
}
