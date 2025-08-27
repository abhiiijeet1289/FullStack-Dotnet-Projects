using BookstoreApp.Data;
using BookstoreApp.Models;
using System.Data;

namespace BookstoreApp.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _bookRepository.GetAllBooksAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error retrieving all books");
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            try
            {
                return await _bookRepository.GetBookByIdAsync(bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error retrieving book with ID {BookId}", bookId);
                throw;
            }
        }

        public async Task<int> AddBookAsync(Book book)
        {
            try
            {
                ValidateBook(book);
                return await _bookRepository.AddBookAsync(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error adding book: {Title}", book.Title);
                throw;
            }
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            try
            {
                ValidateBook(book);
                return await _bookRepository.UpdateBookAsync(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error updating book with ID {BookId}", book.BookId);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            try
            {
                return await _bookRepository.DeleteBookAsync(bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error deleting book with ID {BookId}", bookId);
                throw;
            }
        }

        public async Task<List<Book>> GetBooksUsingStoredProcedureAsync()
        {
            try
            {
                return await _bookRepository.GetAllBooksUsingStoredProcedureAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error retrieving books using stored procedure");
                throw;
            }
        }

        public async Task<DataSet> GetBooksAsDataSetAsync()
        {
            try
            {
                return await _bookRepository.GetBooksAsDataSetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error retrieving books as DataSet");
                throw;
            }
        }

        public async Task<bool> UpdateBooksFromDataSetAsync(DataSet dataSet)
        {
            try
            {
                return await _bookRepository.UpdateBooksFromDataSetAsync(dataSet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error updating books from DataSet");
                throw;
            }
        }

        // Demonstrates safe search using parameterized queries
        public async Task<List<Book>> SearchBooksSafeAsync(string searchTerm)
        {
            try
            {
                // This method would use parameterized queries to prevent SQL injection
                var allBooks = await _bookRepository.GetAllBooksAsync();
                return allBooks.Where(b => 
                    b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error searching books with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        private static void ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title is required");
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Book author is required");
            
            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new ArgumentException("Book ISBN is required");
            
            if (book.Price <= 0)
                throw new ArgumentException("Book price must be greater than zero");
        }
    }
}