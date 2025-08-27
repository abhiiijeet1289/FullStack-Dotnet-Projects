using BookstoreApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace BookstoreApp.Pages.Books
{
    public class DataSetDemoModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly ILogger<DataSetDemoModel> _logger;

        public DataSetDemoModel(IBookService bookService, ILogger<DataSetDemoModel> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        public DataSet? DataSet { get; set; }
        public DataTable? DataTable { get; set; }

        public async Task OnGetAsync()
        {
            // Load initial data
            await LoadDataSetAsync();
        }

        public async Task<IActionResult> OnPostLoadDataSetAsync()
        {
            try
            {
                await LoadDataSetAsync();
                TempData["Message"] = "DataSet loaded successfully using SqlDataAdapter.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading DataSet");
                TempData["Error"] = "An error occurred while loading the DataSet.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateDataSetAsync()
        {
            try
            {
                // First load the current DataSet to have the changes
                await LoadDataSetAsync();
                
                if (DataSet != null && DataSet.HasChanges())
                {
                    var success = await _bookService.UpdateBooksFromDataSetAsync(DataSet);
                    if (success)
                    {
                        TempData["Message"] = "Database updated successfully from DataSet changes.";
                        // Reload to show updated state
                        await LoadDataSetAsync();
                    }
                    else
                    {
                        TempData["Error"] = "Failed to update database from DataSet.";
                    }
                }
                else
                {
                    TempData["Message"] = "No changes detected in DataSet.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating database from DataSet");
                TempData["Error"] = "An error occurred while updating the database.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddRowToDataTableAsync(string newTitle, string newAuthor, string newISBN, decimal newPrice)
        {
            try
            {
                await LoadDataSetAsync();
                
                if (DataTable != null)
                {
                    // Add a new row to demonstrate DataTable operations
                    var newRow = DataTable.NewRow();
                    newRow["Title"] = newTitle;
                    newRow["Author"] = newAuthor;
                    newRow["ISBN"] = newISBN;
                    newRow["Price"] = newPrice;
                    newRow["PublishedYear"] = DateTime.Now.Year;
                    newRow["Genre"] = "Demo";
                    newRow["CreatedDate"] = DateTime.Now;
                    newRow["UpdatedDate"] = DateTime.Now;
                    
                    DataTable.Rows.Add(newRow);
                    
                    TempData["Message"] = $"Row added to DataTable. Row State: {newRow.RowState}. Use 'Update Database' to persist changes.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding row to DataTable");
                TempData["Error"] = "An error occurred while adding the row.";
            }

            return Page();
        }

        private async Task LoadDataSetAsync()
        {
            try
            {
                DataSet = await _bookService.GetBooksAsDataSetAsync();
                if (DataSet.Tables.Count > 0)
                {
                    DataTable = DataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading DataSet in LoadDataSetAsync");
                throw;
            }
        }
    }
}