using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductManagementContext _context;

        public CategoryController(ProductManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            if (id.HasValue)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            return View(new Category());
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(x => x.CategoryId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, [FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (CategoryExists(id.Value))
                    {
                        _context.Categories.Update(category);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["message"] = Message.Serializar("Categoria alterada com sucesso.");
                        }
                        else
                        {
                            TempData["message"] = Message.Serializar("Erro ao alterar categoria.", MessageType.Error);
                        }
                    }
                    else
                    {
                        TempData["message"] = Message.Serializar("Categoria não encontrada.", MessageType.Error);
                    }
                }
                else
                {
                    _context.Categories.Add(category);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["message"] = Message.Serializar("Categoria criada com sucesso.");
                    }
                    else
                    {
                        TempData["message"] = Message.Serializar("Erro ao criar categoria.", MessageType.Error);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                TempData["message"] = Message.Serializar("Categoria não encontrada.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["message"] = Message.Serializar("Categoria não encontrada.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                if (await _context.SaveChangesAsync() > 0)
                    TempData["message"] = Message.Serializar("Categoria apagada com sucesso.");
                else
                    TempData["message"] = Message.Serializar("Não foi possível apagar a categoria.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["message"] = Message.Serializar("Categoria não encontrada.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
