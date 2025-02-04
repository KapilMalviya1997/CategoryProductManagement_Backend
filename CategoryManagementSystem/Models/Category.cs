using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Category_Management_System.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
