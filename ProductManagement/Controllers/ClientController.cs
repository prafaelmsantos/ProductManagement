using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Models.Enums;

namespace ProductManagement.Controllers
{
    public class ClientController : Controller
    {
        private readonly ProductManagementContext _context;

        public ClientController(ProductManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients.OrderBy(x => x.Name).AsNoTracking().ToListAsync();
            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            if (id.HasValue)
            {
                var client = await _context.Clients.FindAsync(id);
                if (client == null)
                {
                    TempData["message"] = Message.Serializar("Cliente não encontrado.", MessageType.Error);
                    return RedirectToAction("Index");
                }
                return View(client);
            }
            return View(new Client());
        }

        private bool ExistsClient(int id)
        {
            return _context.Clients.Any(x => x.UserId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, [FromForm] Client client)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (ExistsClient(id.Value))
                    {
                        _context.Clients.Update(client);
                        _context.Entry(client).Property(x => x.Password).IsModified = false;

                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["message"] = Message.Serializar("Cliente atualizado com sucesso.");
                        }
                        else
                        {
                            TempData["message"] = Message.Serializar("Erro ao alterar cliente.", MessageType.Error);
                        }
                    }
                    else
                    {
                        TempData["message"] = Message.Serializar("Cliente não encontrado.", MessageType.Error);
                    }
                }
                else
                {
                    _context.Clients.Add(client);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["message"] = Message.Serializar("Cliente criado com sucesso.");
                    }
                    else
                    {
                        TempData["message"] = Message.Serializar("Erro ao criar o cliente.", MessageType.Error);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(client);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                TempData["message"] = Message.Serializar("Cliente não informado.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                TempData["message"] = Message.Serializar("Cliente não encontrado.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);

                if (await _context.SaveChangesAsync() > 0)
                    TempData["message"] = Message.Serializar("Cliente apagado com sucesso.");
                else
                    TempData["message"] = Message.Serializar("Não foi possível apagar o cliente.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["message"] = Message.Serializar("Cliente não encontrado.", MessageType.Error);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}