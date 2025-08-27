using BookstoreApp.Models;
using System.Data;

namespace BookstoreApp.Data
{
    public interface IBookRepository
    {
        // Connected Architecture Methods
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int bookId);
        Task<int> AddBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int bookId);
        
        // Stored Procedure Methods
        Task<int> AddBookUsingStoredProcedureAsync(Book book);
        Task<bool> UpdateBookUsingStoredProcedureAsync(Book book);
        Task<bool> DeleteBookUsingStoredProcedureAsync(int bookId);
        Task<List<Book>> GetAllBooksUsingStoredProcedureAsync();
        
        // Dataset/DataTable Methods
        Task<DataSet> GetBooksAsDataSetAsync();
        Task<bool> UpdateBooksFromDataSetAsync(DataSet dataSet);
    }
}