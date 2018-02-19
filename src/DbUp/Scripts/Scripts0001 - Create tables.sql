DROP TABLE IF EXISTS ApiKeys;
DROP TABLE IF EXISTS TicketsToTransactions;
DROP TABLE IF EXISTS Sessions;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Flights;
DROP TABLE IF EXISTS AirPorts;
DROP TABLE IF EXISTS Franchises;
DROP TABLE IF EXISTS Transactions;
DROP TABLE IF EXISTS Tickets;

CREATE TABLE Users(
	ID INT PRIMARY KEY IDENTITY,
	Username VARCHAR(50) NOT NULL,
	Password VARCHAR(255) NOT NULL,
	Email VARCHAR(255) NOT NULL,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	City VARCHAR(50) NOT NULL,
	ZipCode VARCHAR(25),
	Address VARCHAR(255) NOT NULL,
	Grade TINYINT NOT NULL DEFAULT(1),
	DeletedUser BIT NOT NULL DEFAULT(0),
	CONSTRAINT AK_Username UNIQUE(Username)
);

CREATE TABLE Franchises(
 	ID INT PRIMARY KEY IDENTITY,
 	Name VARCHAR(50) NOT NULL
);

CREATE TABLE AirPorts(
	ID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL,
	Country VARCHAR(50) NOT NULL,
	UTCOffset DECIMAL(4,2) NOT NULL
);

CREATE TABLE Transactions(
	ID INT PRIMARY KEY IDENTITY,
	PaymentStatus VARCHAR(255) NOT NULL,
	PaymentReferenceId VARCHAR(255) NOT NULL
);

CREATE TABLE Flights(
	ID INT PRIMARY KEY IDENTITY,
	DepartureDate DATETIME NOT NULL,
	DeparturePort INT NOT NULL FOREIGN KEY REFERENCES AirPorts(ID),
	ArrivalDate DATETIME NOT NULL,
	ArrivalPort INT NOT NULL FOREIGN KEY REFERENCES AirPorts(ID),
	Seats INT NOT NULL,
	Price INT NOT NULL
);

CREATE TABLE Tickets(
	ID INT PRIMARY KEY IDENTITY,
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(ID),
	FlightID INT NOT NULL FOREIGN KEY REFERENCES Flights(ID),
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
	FranchiseID INT NOT NULL FOREIGN KEY REFERENCES Franchises(ID),
	Secret VARCHAR(50) NOT NULL
);

CREATE TABLE Sessions(
	ID INT PRIMARY KEY IDENTITY,
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(ID),
	Secret VARCHAR(50) NOT NULL,
	Active BIT NOT NULL DEFAULT(1),
	Created DateTime NOT NULL
);

INSERT INTO Users VALUES ('Zilver', 'qawsedrf', 'z@example.com', 'Z', '', 'Zagreb', '10000', 'Hrvatske Bratske Zajednice', 1, 0);
INSERT INTO Franchises VALUES ('Captain Morgan');
INSERT INTO AirPorts VALUES ('Madrid', 'ES', 1.1);
INSERT INTO AirPorts VALUES ('Barcelona', 'ES', 1.1);
INSERT INTO AirPorts VALUES ('Venecia', 'IT', 1.2);
INSERT INTO AirPorts VALUES ('Japan', 'JP', 1.3);
INSERT INTO AirPorts VALUES ('Moskva', 'RU', 1.4);
INSERT INTO Flights VALUES ('1833-05-16', 1, '1833-05-17', 2, 126, 9501);
INSERT INTO Flights VALUES ('1833-05-17', 1, '1833-05-18', 2, 136, 9502);
INSERT INTO Flights VALUES ('1833-05-18', 1, '1833-05-19', 2, 146, 9503);
INSERT INTO Flights VALUES ('1833-05-16', 1, '1833-05-17', 3, 16, 9504);
INSERT INTO Flights VALUES ('1833-05-16', 1, '1833-05-17', 4, 16, 9505);
INSERT INTO Flights VALUES ('1833-05-16', 1, '1833-05-17', 5, 16, 9506);
INSERT INTO Flights VALUES ('1833-05-16', 2, '1833-05-17', 1, 16, 9507);
INSERT INTO Flights VALUES ('1833-05-16', 4, '1833-05-17', 3, 16, 9508);
INSERT INTO Flights VALUES ('1833-05-16', 2, '1833-05-17', 4, 16, 9509);
INSERT INTO Flights VALUES ('1833-05-16', 2, '1833-05-17', 5, 16, 9510);

INSERT INTO Tickets VALUES (1, 1, 16, 1)
INSERT INTO Tickets VALUES (1, 2, 16, 1)
INSERT INTO Tickets VALUES (1, 3, 16, 1)
INSERT INTO Tickets VALUES (1, 4, 16, 1)
INSERT INTO Tickets VALUES (1, 5, 16, 1)
INSERT INTO ApiKeys VALUES ('d8e18d8d-6986-497b-b999-d6f073a65045', 1, 'af9f508f-142d-47ce-83ab-cb06eb9a4701')