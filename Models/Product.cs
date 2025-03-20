using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля, въведете име на продукта.")]
        [StringLength(100, ErrorMessage = "Името трябва да е до 100 символа.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Моля, въведете описание.")]
        [StringLength(500, ErrorMessage = "Описанието трябва да е до 500 символа.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Моля, въведете цена.")]
        [Range(0.01, 10000, ErrorMessage = "Цената трябва да е между 0.01 и 10,000 лв.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моля, изберете категория.")]
        public int CategoryId { get; set; } // външен ключ
        public Category? Category { get; set; } //мавигационно свойство ?-позволява null стойност

    }
}
