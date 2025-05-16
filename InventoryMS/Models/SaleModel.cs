using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMS.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; } // Foreign key for Product

        [Required]
        [Range(2, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        // Navigation property
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
