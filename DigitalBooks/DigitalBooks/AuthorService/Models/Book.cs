using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorService.Models
{
    public partial class Book
    {
        public Book()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int BookId { get; set; }
        [Required]
        public string? BookName { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public string? Publisher { get; set; }
        public int? UserId { get; set; }
        
        public DateTime? PublishedDate { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Createdby { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Modifiedby { get; set; }

        public virtual Category? Category { get; set; }
        public virtual UserTable? User { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
