using System.ComponentModel.DataAnnotations;


namespace WebApplication17.Models
{
    public class Product
    {

        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1, 1000000)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

       
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
