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

        public void FranchiseAdd(string name)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO FRANCHISE VALUES('@name')", connection);
                command.ExecuteNonQuery();
                
            }
        }

        public void ApiKeyAdd(string key, string secret)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO ApiKeys(KeyValue, Secret) VALUES(@key, @secret)", connection);
                command.ExecuteNonQuery();

            }
        }
    }
}
