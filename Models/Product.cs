using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля, въведете име на продукта.")]
        [StringLength(100, ErrorMessage = "Името трябва да е до 100 символа.")]
        public required string Name { get; set; } //добавено "required" за да се избегне потенциална NullReferenceException грешка

        [Required(ErrorMessage = "Моля, въведете описание.")]
        [StringLength(500, ErrorMessage = "Описанието трябва да е до 500 символа.")]
        public required string Description { get; set; } //добавено "required" за да се избегне потенциална NullReferenceException грешка

        [Required(ErrorMessage = "Моля, въведете цена.")]//добавено "required" за да се избегне потенциална NullReferenceException грешка
        [Range(0.01, 10000, ErrorMessage = "Цената трябва да е между 0.01 и 10,000 лв.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моля, изберете категория.")]
        public int CategoryId { get; set; } // външен ключ
        public Category? Category { get; set; } //мавигационно свойство ?-позволява null стойност

        public string? ImageUrl { get; set; } // ше съдържа URL или път към изображението
    }
}
