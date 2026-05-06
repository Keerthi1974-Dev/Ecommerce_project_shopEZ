using System.ComponentModel.DataAnnotations;

namespace WebApplication17.DTOs
{
    public class ProductDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1, 100000)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }



    }
}
