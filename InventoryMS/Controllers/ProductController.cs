using InventoryMS.Data;
using InventoryMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryMS.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Add
        public IActionResult AddProduct()
        {
            return View();
        }

        // POST: Product/Add
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("ViewProducts");
            }
            return View(product);
        }

        // GET: Product/ViewProducts
        public IActionResult ViewProducts()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // GET: Product/Edit/{id}
        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit
        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Title = product.Title;
                existingProduct.SellingPrice = product.SellingPrice;

                _context.SaveChanges();
                return RedirectToAction("ViewProducts");
            }
            return View(product);
        }

        // GET: Product/Delete/{id}
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewProducts");
        }
    }
}
