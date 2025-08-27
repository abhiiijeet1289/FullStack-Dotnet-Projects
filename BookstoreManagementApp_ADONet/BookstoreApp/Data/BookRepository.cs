using BookstoreApp.Models;
using System.Data;
using System.Data.SqlClient;

namespace BookstoreApp.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(IConfiguration configuration, ILogger<BookRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        #region Connected Architecture with SqlDataReader

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();
            
            const string query = @"
                SELECT BookId, Title, Author, ISBN, Price, PublishedYear, Genre, CreatedDate, UpdatedDate 
                FROM Books 
                ORDER BY Title";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    books.Add(MapReaderToBook(reader));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books");
                throw;
            }

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            const string query = @"
                SELECT BookId, Title, Author, ISBN, Price, PublishedYear, Genre, CreatedDate, UpdatedDate 
                FROM Books 
                WHERE BookId = @BookId";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookId", bookId);
                
                using var reader = await command.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    return MapReaderToBook(reader);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book with ID {BookId}", bookId);
                throw;
            }

            return null;
        }

        public async Task<int> AddBookAsync(Book book)
        {
            const string query = @"
                INSERT INTO Books (Title, Author, ISBN, Price, PublishedYear, Genre) 
                VALUES (@Title, @Author, @ISBN, @Price, @PublishedYear, @Genre);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand(query, connection);
                AddBookParameters(command, book);
                
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book: {Title}", book.Title);
                throw;
            }
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            const string query = @"
                UPDATE Books 
                SET Title = @Title, Author = @Author, ISBN = @ISBN, Price = @Price, 
                    PublishedYear = @PublishedYear, Genre = @Genre, UpdatedDate = GETDATE()
                WHERE BookId = @BookId";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand(query, connection);
                AddBookParameters(command, book);
                command.Parameters.AddWithValue("@BookId", book.BookId);
                
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID {BookId}", book.BookId);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            const string query = "DELETE FROM Books WHERE BookId = @BookId";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookId", bookId);
                
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", bookId);
                throw;
            }
        }

        #endregion

        #region Stored Procedures

        public async Task<int> AddBookUsingStoredProcedureAsync(Book book)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand("sp_AddBook", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                AddBookParameters(command, book);
                
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book using stored procedure: {Title}", book.Title);
                throw;
            }
        }

        public async Task<bool> UpdateBookUsingStoredProcedureAsync(Book book)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand("sp_UpdateBook", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                AddBookParameters(command, book);
                command.Parameters.AddWithValue("@BookId", book.BookId);
                
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book using stored procedure with ID {BookId}", book.BookId);
                throw;
            }
        }

        public async Task<bool> DeleteBookUsingStoredProcedureAsync(int bookId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand("sp_DeleteBook", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                command.Parameters.AddWithValue("@BookId", bookId);
                
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book using stored procedure with ID {BookId}", bookId);
                throw;
            }
        }

        public async Task<List<Book>> GetAllBooksUsingStoredProcedureAsync()
        {
            var books = new List<Book>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                using var command = new SqlCommand("sp_GetAllBooks", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    books.Add(MapReaderToBook(reader));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving books using stored procedure");
                throw;
            }

            return books;
        }

        #endregion

        #region DataSet and DataTable Operations

        public async Task<DataSet> GetBooksAsDataSetAsync()
        {
            var dataSet = new DataSet();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                const string query = "SELECT BookId, Title, Author, ISBN, Price, PublishedYear, Genre, CreatedDate, UpdatedDate FROM Books";
                
                using var adapter = new SqlDataAdapter(query, connection);
                await Task.Run(() => adapter.Fill(dataSet, "Books"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving books as DataSet");
                throw;
            }

            return dataSet;
        }

        public async Task<bool> UpdateBooksFromDataSetAsync(DataSet dataSet)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                const string query = "SELECT BookId, Title, Author, ISBN, Price, PublishedYear, Genre, CreatedDate, UpdatedDate FROM Books";
                
                using var adapter = new SqlDataAdapter(query, connection);
                
                // Configure CommandBuilder to generate INSERT, UPDATE, DELETE commands
                using var commandBuilder = new SqlCommandBuilder(adapter);
                
                var rowsAffected = await Task.Run(() => adapter.Update(dataSet, "Books"));
                return rowsAffected >= 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating database from DataSet");
                throw;
            }
        }

        #endregion

        #region Helper Methods

        private static Book MapReaderToBook(SqlDataReader reader)
        {
            return new Book
            {
                BookId = reader.GetInt32("BookId"),
                Title = reader.GetString("Title"),
                Author = reader.GetString("Author"),
                ISBN = reader.GetString("ISBN"),
                Price = reader.GetDecimal("Price"),
                PublishedYear = reader.GetInt32("PublishedYear"),
                Genre = reader.IsDBNull("Genre") ? null : reader.GetString("Genre"),
                CreatedDate = reader.GetDateTime("CreatedDate"),
                UpdatedDate = reader.GetDateTime("UpdatedDate")
            };
        }

        private static void AddBookParameters(SqlCommand command, Book book)
        {
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@Price", book.Price);
            command.Parameters.AddWithValue("@PublishedYear", book.PublishedYear);
            command.Parameters.AddWithValue("@Genre", book.Genre ?? (object)DBNull.Value);
        }

        #endregion
    }
}