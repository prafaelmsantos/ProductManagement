using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models;
using ProductManagement.Models.Enums;

namespace ProductManagement.Controllers
{
    public class AddressController : Controller
    {
        private readonly ProductManagementContext _context;

        public AddressController(ProductManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? cid)
        {
            if (cid.HasValue)
            {
                var client = await _context.Clients.FindAsync(cid);
                if (client != null)
                {
                    _context.Entry(client).Collection(c => c.Addresses).Load();
                    ViewData["Client"] = client;
                    return View(client.Addresses);
                }
                else
                {
                    TempData["message"] = Message.Serializar("Cliente não encontrado.", MessageType.Error);
                    return RedirectToAction("Index", "Client");
                }
            }
            else
            {
                TempData["message"] = Message.Serializar("Só é possível mostrar endereços de um cliente específico.", MessageType.Error);
                return RedirectToAction("Index", "Client");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? cid, int? eid)
        {
            if (cid.HasValue)
            {
                var client = await _context.Clients.FindAsync(cid);
                if (client != null)
                {
                    ViewData["Client"] = client;
                    if (eid.HasValue)
                    {
                        _context.Entry(client).Collection(c => c.Addresses).Load();
                        var endereco = client.Addresses.FirstOrDefault(e => e.AddressId == eid);
                        if (endereco != null)
                        {
                            return View(endereco);
                        }
                        else
                        {
                            TempData["message"] = Message.Serializar("Endereço não encontrado.", MessageType.Error);
                        }
                    }
                    else
                    {
                        return View(new Address());
                    }
                }
                else
                {
                    TempData["message"] = Message.Serializar("Cliente não encontrado.", MessageType.Error);
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = Message.Serializar("Nenhum proprietário de endereços foi informado.", MessageType.Error);
                return RedirectToAction("Index", "Client");
            }
        }

        private bool ExistsAddress(int cid, int eid)
        {
            return _context.Clients.FirstOrDefault(c => c.UserId == cid)
                .Addresses.Any(e => e.AddressId == eid);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] int? userId,
            [FromForm] Address address)
        {
            if (userId.HasValue)
            {
                var client = await _context.Clients.FindAsync(userId);
                ViewData["Client"] = client;     

                if (ModelState.IsValid)
                {
                    if (client.Addresses.Count() == 0) address.Selected = true;

                    if (address.AddressId > 0)
                    {
                        if (address.Selected)
                            client.Addresses.ToList().ForEach(e => e.Selected = false);

                        if (ExistsAddress(userId.Value, address.AddressId))
                        {
                            var currentAddress = client.Addresses
                                .FirstOrDefault(e => e.AddressId == address.AddressId);

                            _context.Entry(currentAddress).CurrentValues.SetValues(address);

                            if (_context.Entry(currentAddress).State == EntityState.Unchanged)
                            {
                                TempData["message"] = Message.Serializar("Nenhum dado do endereço foi alterado.");
                            }
                            else
                            {
                                if (await _context.SaveChangesAsync() > 0)
                                {
                                    TempData["message"] = Message.Serializar("Endereço alterado com sucesso.");
                                }
                                else
                                {
                                    TempData["message"] = Message.Serializar("Erro ao alterar endereço.");
                                }
                            }
                        }
                        else
                        {
                            TempData["message"] = Message.Serializar("Endereço não encontrado.", MessageType.Error);
                        }
                    }
                    else
                    {
                        //var addressId = client.Addresses.Count() > 0 ? client.Addresses.Max(e => e.AddressId) + 1 : 1;
                        //address.AddressId = addressId;

                        _context.Clients.FirstOrDefault(c => c.UserId == userId).Addresses.Add(address);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["message"] = Message.Serializar("Endereço guardado com sucesso.");
                        }
                        else
                        {
                            TempData["message"] = Message.Serializar("Erro ao guardar o endereço.");
                        }
                    }
                    return RedirectToAction("Index", "Address", new { cid = userId });
                }
                else
                {
                    return View(address);
                }
            }
            else
            {
                TempData["message"] = Message.Serializar("Nenhum proprietário de endereços foi informado.", MessageType.Error);
                return RedirectToAction("Index", "Client");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? cid, int? eid)
        {
            if (!cid.HasValue)
            {
                TempData["message"] = Message.Serializar("Cliente não informado.", MessageType.Error);
                return RedirectToAction("Index", "Client");
            }

            if (!eid.HasValue)
            {
                TempData["message"] = Message.Serializar("Endereço não informado.", MessageType.Error);
                return RedirectToAction("Index", new { cid = cid });
            }

            var client = await _context.Clients.FindAsync(cid);
            var address = client.Addresses.FirstOrDefault(e => e.AddressId == eid);
            if (address == null)
            {
                TempData["message"] = Message.Serializar("Endereço não encontrado.", MessageType.Error);
                return RedirectToAction("Index", new { cid = cid });
            }

            ViewData["Client"] = client;
            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int userId, int addressId)
        {
            var client = await _context.Clients.FindAsync(userId);
            var address = client.Addresses.FirstOrDefault(e => e.AddressId == addressId);
            if (address != null)
            {
                client.Addresses.Remove(address);
                if (await _context.SaveChangesAsync() > 0)
                {
                    TempData["message"] = Message.Serializar("Endereço apagado com sucesso.");
                    if (address.Selected && client.Addresses.Count() > 0)
                    {
                        client.Addresses.FirstOrDefault().Selected = true;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                    TempData["message"] = Message.Serializar("Não foi possível apagar o endereço.", MessageType.Error);                
            }
            else
            {
                TempData["message"] = Message.Serializar("Endereço não encontrado.", MessageType.Error);                
            }
            return RedirectToAction("Index", new { cid = userId });
        }
    }
}