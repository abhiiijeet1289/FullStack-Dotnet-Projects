using BookstoreApp.Models;
using BookstoreApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookstoreApp.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IBookService bookService, ILogger<IndexModel> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        public List<Book> Books { get; set; } = new();
        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await LoadBooksAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var success = await _bookService.DeleteBookAsync(id);
                if (success)
                {
                    TempData["Message"] = "Book deleted successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to delete book.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", id);
                TempData["Error"] = "An error occurred while deleting the book.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    Books = await _bookService.SearchBooksSafeAsync(SearchTerm);
                    TempData["Message"] = $"Found {Books.Count} books matching '{SearchTerm}'.";
                }
                else
                {
                    await LoadBooksAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books");
                TempData["Error"] = "An error occurred while searching.";
                await LoadBooksAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLoadWithReaderAsync()
        {
            try
            {
                Books = await _bookService.GetAllBooksAsync();
                TempData["Message"] = "Books loaded using SqlDataReader.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books with SqlDataReader");
                TempData["Error"] = "An error occurred while loading books.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLoadWithStoredProcedureAsync()
        {
            try
            {
                Books = await _bookService.GetBooksUsingStoredProcedureAsync();
                TempData["Message"] = "Books loaded using stored procedures.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books with stored procedures");
                TempData["Error"] = "An error occurred while loading books.";
            }

            return Page();
        }

        private async Task LoadBooksAsync()
        {
            try
            {
                Books = await _bookService.GetAllBooksAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books");
                TempData["Error"] = "An error occurred while loading books.";
                Books = new List<Book>();
            }
        }
    }
}