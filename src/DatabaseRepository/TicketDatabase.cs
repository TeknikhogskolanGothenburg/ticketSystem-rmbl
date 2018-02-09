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
        public List<User> UserFind(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<User>("SELECT * FROM Users WHERE ID like '%" + query + "%' OR FirstName like '%" + query + "%' OR LastName like '%" + query + "%' OR City like '%" + query + "%' OR Address like '%" + query + "%'").ToList();
            }
        }

        public List<User> UserGroupFind(string query, string grade)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<User>("SELECT * FROM Users WHERE (ID like '%" + query + "%' OR FirstName like '%" + query + "%' OR LastName like '%" + query + "%' OR City like '%" + query + "%' OR Address like '%" + query + "%') AND ID = @grade").ToList();
            }
        }

        public User UserAdd(string username, string password, string email, string firstName, string lastName, string city, string zipCode, string address, int grade)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { @username = username, @password = password, @email = email, @firstName = firstName, @lastName = lastName, @city = city, zipCode = zipCode, @address = address, @grade = grade };
                connection.Query("INSERT INTO Users(Username, Password, Email, FirstName , LastName, City, ZipCode, Address, Grade) values(@username, @password, @email, @firstName, @lastName, @city, @zipCode, @address, @grade)", data);
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Users') AS Current_Identity").First();
                return connection.Query<User>("SELECT * FROM Users WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public User UserModify(int id, string username, string password, string email, string firstName, string lastName, string city, string zipCode, string address, int grade)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (password != null)
                {
                    var data = new { @id = id, @username = username, @password = password, @email = email, @firstName = firstName, @lastName = lastName, @city = city, zipCode = zipCode, @address = address, @grade = grade };
                    connection.Query("UPDATE Users SET Username=@username, Password=@password, Email=@email, FirstName=@firstName, LastName=@lastName, City=@city, ZipCode=@zipCode, Address=@address, Grade=@grade WHERE ID =@id", data);
                }
                else
                {
                    var data = new { @id = id, @username = username, @email = email, @firstName = firstName, @lastName = lastName, @city = city, zipCode = zipCode, @address = address, @grade = grade };
                    connection.Query("UPDATE Users SET Username=@username, Email=@email, FirstName=@firstName, LastName=@lastName, City=@city, ZipCode=@zipCode, Address=@address, Grade=@grade WHERE ID = @id", data);
                }
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<User>("SELECT * FROM Tickets WHERE ID=@Id", new { Id = addedEventQuery }).First();
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

        public List<Ticket> TicketFind(string id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ticket>("SELECT * FROM Tickets WHERE ID like '%" + id + "%'").ToList();
            }
        }

        public Ticket TicketAdd(int seatId, int userId, int bookAt)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query("INSERT INTO Tickets(SeatID, UserID, BookAt) values(@seatId, @userId, @bookAt)", new { seatId, userId, bookAt });
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Tickets') AS Current_Identity").First();
                return connection.Query<Ticket>("SELECT * FROM Tickets WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public Ticket TicketModify(int id, int seatId, int userId, int bookAt)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query("UPDATE Users SET SeatID=@seatId, UserID=@userId, BookAt=@bookAt WHERE ID = @id", new { id, seatId, userId, bookAt});
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Tickets') AS Current_Identity").First();
                return connection.Query<Ticket>("SELECT * FROM Tickets WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public bool TicketDelete(string id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = String.Format("DELETE FROM Tickets Where ID={0}", id);
                SqlCommand command = new SqlCommand(Qry, connection);
                return Convert.ToBoolean(command.ExecuteNonQuery());
            }
        }

        public Transaction TransactionAdd(string paymentStatus, string paymentReferenceId)
        {

        }
    }
}
