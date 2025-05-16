using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMS.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; } // Foreign key for Product

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Quantity purchased

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than zero.")]
        public decimal PurchasePrice { get; set; } // Price per unit

        [Required]
        public DateTime PurchaseDate { get; set; } // Date of the purchase

        // Navigation property
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
