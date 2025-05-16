using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryMS.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than zero.")]
        public decimal SellingPrice { get; set; } // Selling price
    }
}
