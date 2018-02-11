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
                return connection.Query<User>("SELECT * FROM Users WHERE ID like '%" + query + "%'").ToList();
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
                return connection.Query<User>("SELECT * FROM Users WHERE ID=@Id", new { Id = id }).First();
            }
        }

        public bool UserDelete(string id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = "DELETE FROM Users Where ID=@id";
                SqlCommand command = new SqlCommand(Qry, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection.Open();
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

        public Ticket TicketAdd(int userId, int flightId, int seatNumber, int bookAt)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query("INSERT INTO Tickets(UserID, FlightID, SeatNumber, BookAt) values(@userId, @flightId, @seatNumber, @bookAt)", new { userId, flightId, seatNumber, bookAt });
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Tickets') AS Current_Identity").First();
                return connection.Query<Ticket>("SELECT * FROM Tickets WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public Ticket TicketModify(int id, int userId, int flightId, int seatNumber, int bookAt)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query("UPDATE Users SET UserID=@userId, FlightID=@flightId, SeatNumber=@seatNumber BookAt=@bookAt WHERE ID = @id", new { id, userId, flightId, seatNumber, bookAt });
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Tickets') AS Current_Identity").First();
                return connection.Query<Ticket>("SELECT * FROM Tickets WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public bool TicketDelete(string id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = "DELETE FROM Tickets Where ID=@id";
                SqlCommand command = new SqlCommand(Qry, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection.Open();
                return Convert.ToBoolean(command.ExecuteNonQuery());
            }
        }

        public Transaction TransactionAdd(string paymentStatus, string paymentReferenceId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query("INSERT INTO Transactions(PaymentStatus, PaymentReferenceId ) values(@paymentStatus, @paymentReferenceId)", new { paymentStatus, paymentReferenceId });
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Transactions') AS Current_Identity").First();
                return connection.Query<Transaction>("SELECT * FROM Transactions WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public object TicketToTransactionAdd(int ticketId, int transactionId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = "INSERT INTO TicketsToTransactions (TicketID, TransactionID) VALUES (@ticketId, @transactionId)";
                SqlCommand command = new SqlCommand(Qry, connection);
                command.Parameters.AddRange(new SqlParameter[] {
                    new SqlParameter("@ticketId",ticketId),
                    new SqlParameter("@transactionId",transactionId)});
                command.Connection.Open();
                return command.ExecuteScalar();
            }
        }

        public List<Flight> FlightFind(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Flight>("SELECT * FROM Users WHERE ID like '%" + query + "%' OR FirstName like '%" + query + "%' OR LastName like '%" + query + "%' OR City like '%" + query + "%' OR Address like '%" + query + "%'").ToList();
            }
        }

        public Ticket FlightAdd(string depatureDate, int departurePort, string arrivalDate, int arrivalPort, int seats)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { @depa };
                connection.Query("INSERT INTO Users(Username, Password, Email, FirstName , LastName, City, ZipCode, Address, Grade) values(@username, @password, @email, @firstName, @lastName, @city, @zipCode, @address, @grade)", data);
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Users') AS Current_Identity").First();
                return connection.Query<Flight>("SELECT * FROM Users WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }
    }
}
