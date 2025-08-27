-- Step 1: Create Database
CREATE DATABASE BookstoreDB;
GO

USE BookstoreDB;
GO

-- Step 2: Create Books table
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    ISBN NVARCHAR(20) UNIQUE NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    PublishedYear INT NOT NULL,
    Genre NVARCHAR(100),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    UpdatedDate DATETIME2 DEFAULT GETDATE()
);
GO

-- Step 3: Insert sample data
INSERT INTO Books (Title, Author, ISBN, Price, PublishedYear, Genre) VALUES
('The Great Gatsby', 'F. Scott Fitzgerald', '978-0-7432-7356-5', 12.99, 1925, 'Fiction'),
('To Kill a Mockingbird', 'Harper Lee', '978-0-06-112008-4', 14.99, 1960, 'Fiction'),
('1984', 'George Orwell', '978-0-452-28423-4', 13.99, 1949, 'Dystopian Fiction'),
('Pride and Prejudice', 'Jane Austen', '978-0-14-143951-8', 11.99, 1813, 'Romance');
GO

SELECT * FROM BOOKS;

-- Step 4: Create Stored Procedures
CREATE PROCEDURE sp_AddBook
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @ISBN NVARCHAR(20),
    @Price DECIMAL(10,2),
    @PublishedYear INT,
    @Genre NVARCHAR(100)
AS
BEGIN
    INSERT INTO Books (Title, Author, ISBN, Price, PublishedYear, Genre)
    VALUES (@Title, @Author, @ISBN, @Price, @PublishedYear, @Genre);
    
    SELECT SCOPE_IDENTITY() AS NewBookId;
END
GO

CREATE PROCEDURE sp_UpdateBook
    @BookId INT,
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @ISBN NVARCHAR(20),
    @Price DECIMAL(10,2),
    @PublishedYear INT,
    @Genre NVARCHAR(100)
AS
BEGIN
    UPDATE Books 
    SET Title = @Title,
        Author = @Author,
        ISBN = @ISBN,
        Price = @Price,
        PublishedYear = @PublishedYear,
        Genre = @Genre,
        UpdatedDate = GETDATE()
    WHERE BookId = @BookId;
END
GO

CREATE PROCEDURE sp_DeleteBook
    @BookId INT
AS
BEGIN
    DELETE FROM Books WHERE BookId = @BookId;
END
GO

CREATE PROCEDURE sp_GetAllBooks
AS
BEGIN
    SELECT BookId, Title, Author, ISBN, Price, PublishedYear, Genre, CreatedDate, UpdatedDate
    FROM Books
    ORDER BY Title;
END
GO

CREATE PROCEDURE sp_GetBookById
    @BookId INT
AS
BEGIN
    SELECT BookId, Title, Author, ISBN, Price, PublishedYear, Genre, CreatedDate, UpdatedDate
    FROM Books
    WHERE BookId = @BookId;
END
GO