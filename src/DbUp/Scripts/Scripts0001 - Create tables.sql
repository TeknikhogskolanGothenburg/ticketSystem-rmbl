CREATE TABLE TicketEvents (
    TicketEventID int NOT NULL PRIMARY KEY,
    EventName varchar(255),
	EventHtmlDescription VARCHAR(MAX)
	/* image */
);

CREATE TABLE Venues (
    VenueID int NOT NULL PRIMARY KEY,
    VenueName varchar(255),
    Address varchar(255),
    City varchar(255),
	Country VARCHAR(255)
);


CREATE TABLE TicketEventDates (
    TicketEventDateID INT NOT NULL PRIMARY KEY,
	TicketEventID INT FOREIGN KEY REFERENCES TicketEvents(TicketEventID),
	VenueId INT FOREIGN KEY REFERENCES Venues(VenueID),
    EventStartDateTime DATETIME
);




CREATE TABLE SeatsAtEventDate (
    SeatID INT NOT NULL PRIMARY KEY,
	TicketEventDateID INT  FOREIGN KEY REFERENCES TicketEventDates(TicketEventDateID)
);

CREATE TABLE Tickets (
    TicketID INT NOT NULL PRIMARY KEY,
	SeatID INT FOREIGN KEY REFERENCES SeatsAtEventDate(SeatID)
);

CREATE TABLE TicketTransactions (
    TransactionID int NOT NULL PRIMARY KEY,
    BuyerLastName varchar(255),
    BuyerFirstName varchar(255),
    BuyerAddress varchar(255),
    BuyerCity varchar(255),
	PaymentStatus varchar(255),
	PaymentReferenceId varchar(255)
);

CREATE TABLE TicketsToTransactions (
    TicketID INT  NOT NULL   FOREIGN KEY REFERENCES Tickets(TicketID),
	TransactionID INT  NOT NULL   FOREIGN KEY REFERENCES TicketTransactions(TransactionID),
	primary key (TicketID, TransactionID)
);

