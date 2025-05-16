using InventoryMS.Data;
using InventoryMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryMS.Controllers
{
    public class SaleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaleController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetAvailableQuantity(int productId)
        {
            var totalPurchased = _context.Purchases
                .Where(p => p.ProductId == productId)
                .Sum(p => (int?)p.Quantity) ?? 0;

            var totalSold = _context.Sales
                .Where(s => s.ProductId == productId)
                .Sum(s => (int?)s.Quantity) ?? 0;

            return totalPurchased - totalSold;
        }


        // GET: Sale/Add
        // GET: Sale/Add
        public IActionResult AddSale()
        {
            var products = _context.Products
                .ToList() // Fetch all products into memory
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    Quantity = GetAvailableQuantity(p.Id)
                })
                .ToList();

            ViewBag.Products = products;
            return View();
        }

        // POST: Sale/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSale(AddSaleModel model)
        {
            if (!ModelState.IsValid)
            {
                var products = _context.Products
                    .ToList() // Fetch all products into memory
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        Quantity = GetAvailableQuantity(p.Id)
                    })
                    .ToList();

                ViewBag.Products = products;
                return View(model);
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("", "The selected product does not exist.");
                return View(model);
            }

            var currentStock = GetAvailableQuantity(model.ProductId);

            if (currentStock < model.Quantity)
            {
                ModelState.AddModelError("", "Insufficient stock for the selected product.");
                return View(model);
            }

            var sale = new Sale
            {
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                SaleDate = model.SaleDate
            };

            _context.Sales.Add(sale);
            _context.SaveChanges();

            return RedirectToAction("ViewSales");
        }


        // GET: Sale/View
        public IActionResult ViewSales()
        {
            var sales = _context.Sales
                .Include(s => s.Product)
                .Select(s => new
                {
                    s.Id,
                    ProductTitle = s.Product.Title,
                    s.Quantity,
                    s.SaleDate
                })
                .ToList();

            ViewBag.Sales = sales;
            return View();
        }
    }
}
