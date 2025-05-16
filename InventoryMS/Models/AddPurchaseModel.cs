using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryMS.Models
{
    public class AddPurchaseModel
    {
        [Required]
        public int ProductId { get; set; } // Selected product ID

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Quantity purchased

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than zero.")]
        public decimal PurchasePrice { get; set; } // Price per unit

        [Required]
        public DateTime PurchaseDate { get; set; } // Date of the purchase
    }
}
