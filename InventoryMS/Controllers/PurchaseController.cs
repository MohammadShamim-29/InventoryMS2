using InventoryMS.Data;
using InventoryMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryMS.Controllers
{

    [Authorize]
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Purchase/Add
        public IActionResult AddPurchase()
        {
            ViewBag.Products = _context.Products.ToList();
            return View(new AddPurchaseModel { PurchaseDate = DateTime.Today });
        }

        // POST: Purchase/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPurchase(AddPurchaseModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _context.Products.ToList();
                return View(model);
            }

            // Fetch the product to validate
            var product = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("", "The selected product does not exist.");
                ViewBag.Products = _context.Products.ToList();
                return View(model);
            }

            // Create a new Purchase entity
            var purchase = new Purchase
            {
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                PurchasePrice = model.PurchasePrice,
                PurchaseDate = model.PurchaseDate
            };

            // Add the purchase to the database
            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            return RedirectToAction("ViewPurchases");
        }

        // GET: Purchase/View
        public IActionResult ViewPurchases()
        {
            var purchases = _context.Purchases
                .Include(p => p.Product)
                .Select(p => new
                {
                    p.Id,
                    ProductTitle = p.Product.Title,
                    p.Quantity,
                    p.PurchasePrice,
                    p.PurchaseDate
                })
                .ToList();

            ViewBag.Purchases = purchases;
            return View();
        }
    }
}
