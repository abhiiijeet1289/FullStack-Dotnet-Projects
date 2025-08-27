using BookstoreApp.Models;
using System.Data;

namespace BookstoreApp.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int bookId);
        Task<int> AddBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int bookId);
        
        // Demonstration methods for different ADO.NET approaches
        Task<List<Book>> GetBooksUsingStoredProcedureAsync();
        Task<DataSet> GetBooksAsDataSetAsync();
        Task<bool> UpdateBooksFromDataSetAsync(DataSet dataSet);
        
        // SQL Injection demonstration
        Task<List<Book>> SearchBooksSafeAsync(string searchTerm);
    }
}