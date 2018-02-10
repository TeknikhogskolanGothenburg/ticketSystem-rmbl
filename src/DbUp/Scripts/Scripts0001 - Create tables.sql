DROP TABLE IF EXISTS TicketsToTransactions;
DROP TABLE IF EXISTS ApiKeys;
DROP TABLE IF EXISTS Tickets;
DROP TABLE IF EXISTS Franchises;
DROP TABLE IF EXISTS Flights;
DROP TABLE IF EXISTS AirPorts;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Transactions;

CREATE TABLE Users(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Username VARCHAR(50) ,
	Password VARCHAR(255),
	Epost VARCHAR(255),
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	City VARCHAR(50),
	ZipCode VARCHAR(25),
	Address VARCHAR(255),
	Grade TINYINT NOT NULL DEFAULT(1)
	CONSTRAINT AK_Username UNIQUE(Username) 
);

CREATE TABLE Franchises(
 	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
 	Name VARCHAR(50) NOT NULL
);

CREATE TABLE AirPorts(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(50),
	Country VARCHAR(50),
	UTCOffset DECIMAL(4,2)
);

CREATE TABLE Transactions(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,	
	PaymentStatus VARCHAR(255),
	PaymentReferenceId VARCHAR(255)
);

CREATE TABLE Flights(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	DepatureDate DATETIME,
	DeparturePort INT FOREIGN KEY REFERENCES AirPorts(ID),
	ArrivalDate DATETIME,
	ArrivalPort INT FOREIGN KEY REFERENCES AirPorts(ID),
	Seats INT NOT NULL
);

CREATE TABLE Tickets(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserID INT FOREIGN KEY REFERENCES Users(ID),
	FlightID INT FOREIGN KEY REFERENCES Flights(ID),
	SeatNumber INT NOT NULL,
	BookAt INT FOREIGN KEY REFERENCES Franchises(ID)
);

CREATE TABLE TicketsToTransactions(
	TicketID INT NOT NULL FOREIGN KEY REFERENCES Tickets(ID),
	TransactionID INT NOT NULL FOREIGN KEY REFERENCES Transactions(ID),
	primary key (TicketID, TransactionID)
);

CREATE TABLE ApiKeys(
	KeyValue VARCHAR(50) NOT NULL PRIMARY KEY,
	FranchiseID INT FOREIGN KEY REFERENCES Franchises(ID),
	Secret VARCHAR(50) NOT NULL,
);

INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Arlanda', 'Sweden', 1);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Adelaide', 'Australia', 9.5);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Auckland', 'New Zealand', 12);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Colorado Springs', 'United States', -7);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Alghero', 'Italy', 1);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Roissy', 'France', 1);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Shanghai Pudong', 'China', 8);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Dubai International', 'United Arab Emirates', 4);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('John F. Kennedy International', 'United States', -5);
INSERT INTO AirPorts (Name, Country, UTCOffset) VALUES ('Aberdeen', 'United Kingdom', 0);

INSERT INTO Franchises (Name) VALUES ('Skyscanner');
INSERT INTO Franchises (Name) VALUES ('Flygresor');
INSERT INTO Franchises (Name) VALUES ('Kayak');
INSERT INTO Franchises (Name) VALUES ('Momondo');
INSERT INTO Franchises (Name) VALUES ('Google');
INSERT INTO Franchises (Name) VALUES ('Hipmunk');
INSERT INTO Franchises (Name) VALUES ('CheapOAir');
INSERT INTO Franchises (Name) VALUES ('Seatguru');
INSERT INTO Franchises (Name) VALUES ('Hotwire');
INSERT INTO Franchises (Name) VALUES ('Expedia');

INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-03 12:32', 1,'2018-05-04 02:15', 4, 120);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-04 09:10', 1,'2018-05-04 17:40', 5, 200);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-03 14:05', 2,'2018-05-03 18:10', 3, 120);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-04 06:00', 2,'2018-05-04 21:55', 8, 200);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-03 03:10', 3,'2018-05-03 15:50', 7, 120);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-04 12:32', 3,'2018-05-04 20:32', 2, 200);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-03 12:32', 4,'2018-05-03 20:32', 1, 120);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-04 12:32', 4,'2018-05-04 20:32', 5, 200);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-03 12:32', 5,'2018-05-03 20:32', 6, 120);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-04 12:32', 5,'2018-05-04 20:32', 7, 200);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-03 12:32', 6,'2018-05-03 20:32', 8, 120);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-04 12:32', 6,'2018-05-04 20:32', 9, 200);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-03 12:32', 7,'2018-05-03 20:32', 9, 120);
INSERT INTO Flights (DepatureDate, DeparturePort, ArrivalDate, ArrivalPort, Seats) 
			 VALUES ('2018-05-04 12:32', 7,'2018-05-04 20:32', 10, 200);