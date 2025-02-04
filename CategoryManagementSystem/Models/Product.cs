using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Product_Category_Management_System.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity  must be greater than 0.")]
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //[ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }
        //public Category Category { get; set; }
    }
}
