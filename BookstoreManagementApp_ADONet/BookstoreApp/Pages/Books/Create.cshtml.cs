using BookstoreApp.Models;
using BookstoreApp.Services;
using BookstoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookstoreApp.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IBookService bookService, IBookRepository bookRepository, ILogger<CreateModel> logger)
        {
            _bookService = bookService;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool useStoredProcedure = false)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                int bookId;
                
                if (useStoredProcedure)
                {
                    // Demonstrate stored procedure usage
                    bookId = await _bookRepository.AddBookUsingStoredProcedureAsync(Book);
                    TempData["Message"] = $"Book '{Book.Title}' added successfully using stored procedure!";
                }
                else
                {
                    // Use regular service method (parameterized queries)
                    bookId = await _bookService.AddBookAsync(Book);
                    TempData["Message"] = $"Book '{Book.Title}' added successfully!";
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book: {Title}", Book.Title);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the book. Please try again.");
                return Page();
            }
        }
    }
}