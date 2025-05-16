using System;
using System.Collections.Generic;

namespace InventoryMS.Models
{
    public class DashboardViewModel
    {
        public decimal TodaysSales { get; set; }
        public int TransactionsToday { get; set; }
        public int ProductsInStock { get; set; }
        public int LowStockItems { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<string> RecentActivity { get; set; }
    }

    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
