using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBookshelfApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        [Required]
        public string ApplicationuserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<BookGenre> BookGenres { get; set; }
    }
}
