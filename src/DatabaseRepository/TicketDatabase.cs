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
        public User UserFind(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<User>("SELECT * FROM Users WHERE ID = @id", new { id }).ToList().First();
            }
        }

        public User UserFind(string name)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<User>("SELECT * FROM Users WHERE Username=@name", new { name }).ToList().First();
            }
        }

        public List<User> UserFindAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<User>("SELECT * FROM Users").ToList();
            }
        }

        public User UserAdd(string username, string password, string email, string firstName, string lastName, string city, string zipCode, string address, int grade)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { username, password, email, firstName, lastName, city, zipCode, address, grade };
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
                    var data = new { id, username, password, email, firstName, lastName, city, zipCode, address, grade };
                    connection.Query("UPDATE Users SET Username=@username, Password=@password, Email=@email, FirstName=@firstName, LastName=@lastName, City=@city, ZipCode=@zipCode, Address=@address, Grade=@grade WHERE ID =@id", data);
                }
                else
                {
                    var data = new { id, username, email, firstName, lastName, city, zipCode, address, grade };
                    connection.Query("UPDATE Users SET Username=@username, Email=@email, FirstName=@firstName, LastName=@lastName, City=@city, ZipCode=@zipCode, Address=@address, Grade=@grade WHERE ID = @id", data);
                }
                return connection.Query<User>("SELECT * FROM Users WHERE ID=@Id", new { Id = id }).First();
            }
        }

        public bool UserDelete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = "UPDATE Users SET DeletedUser=1 Where ID=@id";
                SqlCommand command = new SqlCommand(Qry, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection.Open();
                return Convert.ToBoolean(command.ExecuteNonQuery());
            }
        }

        public Ticket TicketFind(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ticket>("SELECT * FROM Tickets WHERE ID=@id", new { id }).ToList().First();
            }
        }

        public List<Ticket> TicketFindAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ticket>("SELECT * FROM Tickets").ToList();
            }
        }

        public List<Ticket> TicketforUserFind(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ticket>("SELECT * FROM Tickets WHERE UserID=@id", new { id }).ToList();
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

        public bool TicketDelete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = "DELETE FROM TicketsToTransactions Where TicketID=@id";
                SqlCommand command = new SqlCommand(Qry, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection.Open();
                command.ExecuteNonQuery();

                Qry = "DELETE FROM Tickets Where ID=@id";
                command.Parameters.Add(new SqlParameter("@id", id));
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

        public bool TicketToTransactionAdd(int ticketId, int transactionId)
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
                return Convert.ToBoolean(command.ExecuteNonQuery());
            }
        }

        public Flight FlightFind(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Flight>("SELECT * FROM Flights WHERE ID=@id", new { id }).ToList().First();
            }
        }

        public List<Flight> FlightFindAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Flight>("SELECT * FROM Flights").ToList();
            }
        }

        public Flight FlightAdd(DateTime departureDate, int departurePort, DateTime arrivalDate, int arrivalPort, int seats, int price)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { @departureDate = departureDate.Date, departurePort, @arrivalDate = arrivalDate.Date, arrivalPort, seats, price };
                connection.Query("INSERT INTO Flights(DepartureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats, Price) values(@departureDate, @departurePort, @arrivalDate, @arrivalPort, @seats, @price)", data);
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Flights') AS Current_Identity").First();
                return connection.Query<Flight>("SELECT * FROM Flights WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public Flight FlightModify(int id, DateTime departureDate, int departurePort, DateTime arrivalDate, int arrivalPort, int seats, int price)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { id, @departureDate = departureDate.Date, departurePort, @arrivalDate = arrivalDate.Date, arrivalPort, seats, price };
                connection.Query("UPDATE Flights SET DepatureDate=@departureDate, DeparturePort=@departurePort, ArrivalDate=@arrivalDate, ArrivalPort=@arrivalPort, Seats=@seats, Price=@price WHERE ID = @id", data);
                return connection.Query<Flight>("SELECT * FROM Flights WHERE ID=@Id", new { Id = id }).First();
            }
        }

        public bool FlightDelete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = "DELETE FROM Flights Where ID=@id";
                SqlCommand command = new SqlCommand(Qry, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection.Open();
                return Convert.ToBoolean(command.ExecuteNonQuery());
            }
        }

        public AirPort AirPortFind(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<AirPort>("SELECT * FROM AirPorts WHERE ID=@id", new { id }).ToList().First();
            }
        }

        public List<AirPort> AirPortFindAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<AirPort>("SELECT * FROM AirPorts").ToList();
            }
        }

        public AirPort AirPortAdd(string name, string country, double utcOffset)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { name, country, utcOffset };
                connection.Query("INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES (@name, @country, @utcOffset)", data);
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('AirPorts') AS Current_Identity").First();
                return connection.Query<AirPort>("SELECT * FROM AirPorts WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public AirPort AirPortModify(int id, string name, string country, double utcOffset)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { id, name, country, utcOffset };
                connection.Query("UPDATE AirPorts SET Name=@name, Country=@country, UTCOffset=@utcOffset WHERE ID = @id", data);
                return connection.Query<AirPort>("SELECT * FROM AirPorts WHERE ID=@Id", new { Id = id }).First();
            }
        }

        public bool AirPortDelete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string Qry = "DELETE FROM AirPorts Where ID=@id";
                SqlCommand command = new SqlCommand(Qry, connection);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection.Open();
                return Convert.ToBoolean(command.ExecuteNonQuery());
            }
        }

        public List<Flight> AirportDeparturesFind(int portId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Flight>("SELECT Flights.ID, DepartureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats FROM AirPorts JOIN Flights ON Airports.ID=Flights.Departureport WHERE AirPorts.ID=@portId", new { portId }).ToList();
            }
        }

        public List<Flight> AirportDeparturesAtDateFind(int portId, DateTime date)
        {
            DateTime dateEnd = date.AddDays(7);
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Flight>("SELECT Flights.ID, DepartureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats FROM AirPorts JOIN Flights ON Airports.ID=Flights.Departureport WHERE AirPorts.ID=@portId AND Flighs.DepatureDate > @date AND Flighs.DepatureDate < @dateEnd", new { portId, date, dateEnd }).ToList();
            }
        }

        public List<Flight> AirportArrivalsFind(int portId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Flight>("SELECT Flights.ID, DepartureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats FROM AirPorts JOIN Flights ON Airports.ID=Flights.ArrivalPort WHERE AirPorts.ID=@portId", new { portId }).ToList();
            }
        }

        public List<int> AvaliableSeatsFind(int flightId)
        {
            List<int> occupiedSeats = new List<int>();
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                occupiedSeats = connection.Query<int>("SELECT SeatNumber FROM Tickets JOIN Flights ON Flights.ID=FlightID WHERE FlightID=@flightId", new { flightId }).ToList();
            }
            int seats = FlightFind(flightId).Seats;
            List<int> avaliableSeats = new List<int>();
            for (int i = 1; i <= seats; i++)
            {
                if (!occupiedSeats.Contains(i))
                {
                    avaliableSeats.Add(i);
                }
            }
            return avaliableSeats;
        }
        
        public string APIKeyFind(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<string>("SELECT KeyValue FROM ApiKeys WHERE FranchiseID=@id", new { id }).First();
            }
        }

        public string APISecretFind(int franchiseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<string>("SELECT Secret FROM ApiKeys WHERE FranchiseID=@franchiseId", new { franchiseId }).First();
            }
        }

        public string APISecretFind(string apiKey)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<string>("SELECT Secret FROM ApiKeys WHERE KeyValue=@apiKey", new { apiKey }).First();
            }
        }

        public List<Transaction> TransactionFind()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Transaction>("SELECT * FROM Transactions").ToList();
            }
        }

        public Session SessionAdd(int userId, string secret, DateTime created)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var data = new { userId, secret, created };
                connection.Query("INSERT INTO Sessions (UserID , Secret , Created ) VALUES (@userId, @secret, @created)", data);
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('AirPorts') AS Current_Identity").First();
                return connection.Query<Session>("SELECT * FROM Sessions WHERE ID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public Session SessionFind(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Session>("SELECT * FROM Session WHERE ID = @id", new { id }).ToList().First();
            }
        }
    }
}
