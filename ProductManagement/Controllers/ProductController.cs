using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductManagementContext _context;

        public ProductController(ProductManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context
                .Products
                .OrderBy(x => x.Name)
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            var categories = _context
                .Categories
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToList();

            var categoriesSelectList = new SelectList(categories,
                nameof(Category.CategoryId), nameof(Category.Name));

            ViewData["Categories"] = categoriesSelectList;

            if (id.HasValue)
            {
                var produto = await _context.Products.FindAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }
                return View(produto);
            }
            return View(new Product());
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(x => x.ProductId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, [FromForm] Product product)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (ProductExists(id.Value))
                    {
                        _context.Products.Update(product);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["message"] = Message.Serializar("Produto alterado com sucesso.");
                        }
                        else
                        {
                            TempData["message"] = Message.Serializar("Erro ao alterar produto.", MessageType.Error);
                        }
                    }
                    else
                    {
                        TempData["message"] = Message.Serializar("Produto não encontrado.", MessageType.Error);
                    }
                }
                else
                {
                    _context.Products.Add(product);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["message"] = Message.Serializar("Produto guardado com sucesso.");
                    }
                    else
                    {
                        TempData["message"] = Message.Serializar("Erro ao guardar o produto.", MessageType.Error);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                TempData["message"] = Message.Serializar("Produto não encontrado.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["message"] = Message.Serializar("Produto não encontrado.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                if (await _context.SaveChangesAsync() > 0)
                    TempData["message"] = Message.Serializar("Produto apagado com sucesso.");
                else
                    TempData["message"] = Message.Serializar("Não foi possível apagar o produto.", MessageType.Error);
            }
            else
            {
                TempData["message"] = Message.Serializar("Produto não encontrado.", MessageType.Error);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
