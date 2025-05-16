using InventoryMS.Data;
using InventoryMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InventoryMS.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch today's sales with related products
            var todaysSales = _context.Sales
                .Where(s => s.SaleDate.Date == DateTime.Today)
                .Include(s => s.Product)
                .ToList();

            // Calculate stock levels dynamically
            var productStockLevels = _context.Products
                .Select(p => new
                {
                    Product = p,
                    TotalPurchased = _context.Purchases
                        .Where(purchase => purchase.ProductId == p.Id)
                        .Sum(purchase => purchase.Quantity), // Sum all purchases for the product
                    TotalSold = _context.Sales
                        .Where(sale => sale.ProductId == p.Id)
                        .Sum(sale => sale.Quantity) // Sum all sales for the product
                })
                .ToList();

            // Process data for the dashboard
            var model = new DashboardViewModel
            {
                TodaysSales = todaysSales.Sum(s => s.Quantity * s.Product.SellingPrice), // Use SellingPrice for sales calculation
                TransactionsToday = todaysSales.Count,
                ProductsInStock = productStockLevels.Sum(p => p.TotalPurchased - p.TotalSold), // Calculate total stock
                LowStockItems = productStockLevels.Count(p => (p.TotalPurchased - p.TotalSold) <= 5), // Example threshold for low stock
                Inventory = productStockLevels.Select(p => new InventoryItem
                {
                    Id = p.Product.Id,
                    Name = p.Product.Title,
                    Category = "General", // Replace with actual category if available
                    Quantity = p.TotalPurchased - p.TotalSold, // Calculate current stock
                    SellingPrice = p.Product.SellingPrice, // Include SellingPrice
                }).ToList(),
                RecentActivity = todaysSales.Select(s => $"Sold {s.Quantity} units of {s.Product.Title}").ToList()
            };

            return View(model);
        }

        public IActionResult LowStockItems()
        {
            // Define the low stock threshold (e.g., 5 units)
            int lowStockThreshold = 5;

            // Fetch products with stock below the threshold
            var lowStockProducts = _context.Products
                .Select(p => new
                {
                    Product = p,
                    TotalPurchased = _context.Purchases
                        .Where(purchase => purchase.ProductId == p.Id)
                        .Sum(purchase => purchase.Quantity),
                    TotalSold = _context.Sales
                        .Where(sale => sale.ProductId == p.Id)
                        .Sum(sale => sale.Quantity)
                })
                .Where(p => (p.TotalPurchased - p.TotalSold) <= lowStockThreshold)
                .Select(p => new InventoryItem
                {
                    Id = p.Product.Id,
                    Name = p.Product.Title,
                    Quantity = p.TotalPurchased - p.TotalSold,
                    SellingPrice = p.Product.SellingPrice
                })
                .ToList();

            return View(lowStockProducts);
        }

        public IActionResult TotalSales()
        {
            // Group sales by date and product, then aggregate
            var salesData = _context.Sales
                .Include(s => s.Product)
                .GroupBy(s => new { s.SaleDate.Date, s.ProductId, s.Product.Title })
                .Select(g => new
                {
                    Date = g.Key.Date,
                    ProductName = g.Key.Title,
                    TotalQuantity = g.Sum(s => s.Quantity),
                    TotalAmount = g.Sum(s => s.Quantity * s.Product.SellingPrice)
                })
                .OrderByDescending(x => x.Date)
                .ThenBy(x => x.ProductName)
                .ToList();

            // Group by date for summary
            var dailyTotals = salesData
                .GroupBy(x => x.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(x => x.TotalAmount),
                    Products = g.Select(x => new { x.ProductName, x.TotalQuantity, x.TotalAmount }).ToList()
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            ViewBag.DailyTotals = dailyTotals;
            return View();
        }
    }
}
