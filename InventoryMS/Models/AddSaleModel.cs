using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryMS.Models
{
    public class AddSaleModel
    {
        [Required]
        public int ProductId { get; set; } // Selected product ID

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } // Quantity sold

        [Required]
        public DateTime SaleDate { get; set; } // Date of the sale
    }
}
