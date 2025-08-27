using BookstoreApp.Models;
using BookstoreApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookstoreApp.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IBookService bookService, ILogger<DetailsModel> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

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
                _logger.LogError(ex, "Error loading book details with ID {BookId}", id);
                TempData["Error"] = "An error occurred while loading the book details.";
                return RedirectToPage("./Index");
            }
        }
    }
}