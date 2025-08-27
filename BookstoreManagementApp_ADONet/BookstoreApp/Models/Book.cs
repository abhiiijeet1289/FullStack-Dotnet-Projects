using System.ComponentModel.DataAnnotations;

namespace BookstoreApp.Models
{
    public class Book
    {
        public int BookId { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Author is required")]
        [StringLength(255, ErrorMessage = "Author name cannot exceed 255 characters")]
        public string Author { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(20, ErrorMessage = "ISBN cannot exceed 20 characters")]
        public string ISBN { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 9999.99, ErrorMessage = "Price must be between 0.01 and 9999.99")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Published Year is required")]
        [Range(1000, 2100, ErrorMessage = "Published Year must be between 1000 and 2100")]
        public int PublishedYear { get; set; }
        
        [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters")]
        public string? Genre { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}