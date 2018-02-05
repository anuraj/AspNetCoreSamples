using System;
using System.ComponentModel.DataAnnotations;

namespace BooksApi.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
        [Required]
        public string Author { get; set; }
    }
}