using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Свързване на категорията с продуктите (One-to-Many)
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
