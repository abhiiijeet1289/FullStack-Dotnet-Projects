using BookstoreApp.Models;
using BookstoreApp.Services;
using BookstoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookstoreApp.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IBookService bookService, IBookRepository bookRepository, ILogger<EditModel> logger)
        {
            _bookService = bookService;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Book = await _bookService.GetBookByIdAsync(id);
                if (Book == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book for edit with ID {BookId}", id);
                TempData["Error"] = "An error occurred while loading the book.";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(bool useStoredProcedure = false)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                bool success;
                
                if (useStoredProcedure)
                {
                    // Demonstrate stored procedure usage
                    success = await _bookRepository.UpdateBookUsingStoredProcedureAsync(Book);
                    TempData["Message"] = $"Book '{Book.Title}' updated successfully using stored procedure!";
                }
                else
                {
                    // Use regular service method (parameterized queries)
                    success = await _bookService.UpdateBookAsync(Book);
                    TempData["Message"] = $"Book '{Book.Title}' updated successfully!";
                }

                if (!success)
                {
                    TempData["Error"] = "Failed to update book.";
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID {BookId}", Book.BookId);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the book. Please try again.");
                return Page();
            }
        }
    }
}