-- Active: 1722262060181@@localhost@3306@movie_db
CREATE TABLE movie(  
    MovieID int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    Duration DATETIME COMMENT 'Duration',
    Title VARCHAR(256),
    Description VARCHAR(256),
    Language VARCHAR(256),
    ReleaseDate DATETIME COMMENT 'Release Date',
    Country VARCHAR(256),
    Genre VARCHAR(256)
) COMMENT '';

-- Active: 1722262060181@@localhost@3306@movie_db

CREATE TABLE city(
    CityID INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    Name VARCHAR(64),
    State VARCHAR(64),
    ZipCode VARCHAR(16)
) COMMENT '';
CREATE TABLE cinema(
    CinemaID INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    Name VARCHAR(64),
    TotalCinemaHalls INT,
    CityID INT,
    FOREIGN KEY (CityID) REFERENCES city(CityID)
) COMMENT '';

CREATE TABLE cinema_hall(
    CinemaHallID INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    Name VARCHAR(64),
    TotalSeats INT,
    CinemaID INT,
    FOREIGN KEY (CinemaID) REFERENCES cinema(CinemaID)
) COMMENT '';

CREATE TABLE `show`(
    ShowID INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    CinemaHallID INT COMMENT 'Foreign Key',
    Date DATETIME COMMENT 'Date',
    MovieID INT COMMENT 'Foreign Key',
    StartTime DATETIME COMMENT 'Start Time',
    EndTime DATETIME COMMENT 'End Time',
    FOREIGN KEY (CinemaHallID) REFERENCES cinema_hall(CinemaHallID),
    FOREIGN KEY (MovieID) REFERENCES movie(MovieID)
) COMMENT '';
CREATE TABLE user(
    UserID INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    Name VARCHAR(64),
    Password VARCHAR(20),
    Email VARCHAR(64),
    Phone VARCHAR(16)
) COMMENT '';

CREATE TABLE cinema_seat(
    CinemaSeatID INT NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    SeatNumber INT,
    Type INT,
    CinemaHallID INT,
    FOREIGN KEY (CinemaHallID) REFERENCES cinema_hall(CinemaHallID)
) COMMENT '';

CREATE TABLE booking(  
    BookingID int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    NumberofSeats INT,
    TimeStamp DATETIME(3) COMMENT 'Duration',
    Status INT,
    UserID INT,
    ShowID INT,
    FOREIGN KEY (UserID) REFERENCES user(UserID),
    FOREIGN KEY (ShowID) REFERENCES `show`(ShowID)
) COMMENT '';

CREATE TABLE payment(  
    PaymentID int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    TimeStamp DATETIME COMMENT 'Time Stamp',
    Amount Double,
    DiscountCouponID INT,
    RemoteTransactionID INT,
    PaymentMethod INT,
    BookingID INT,
    FOREIGN KEY (BookingID) REFERENCES booking(BookingID)
) COMMENT '';

CREATE TABLE show_seat(  
    ShowSeatID int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    Status INT,
    Price Double,
    CinemaSeatID INT,
    BookingID INT,
    ShowID INT,
    FOREIGN KEY (CinemaSeatID) REFERENCES cinema_seat(CinemaSeatID),
    FOREIGN KEY (ShowID) REFERENCES `show`(ShowID),
    FOREIGN KEY (BookingID) REFERENCES booking(BookingID)
) COMMENT '';
