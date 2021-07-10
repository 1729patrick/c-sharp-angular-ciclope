using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciclope.Data;
using Ciclope.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Ciclope.Controllers
{
    [Authorize]
    public class DividaATController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public DividaATController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DividaAT
        public async Task<IActionResult> Index(int id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == 0)
            {
                return View("../Home/Error");
            }
            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }
            var applicationDbContext = _context.DividaAT.Include(t => t.Empresa).Where(e => e.EmpresaId == id).OrderBy(e=> e.Data);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        private bool DividaATExists(int id)
        {
            return _context.DividaAT.Any(e => e.Id == id);
        }

        public IActionResult Create(int id)
        {
            ////string userId = _userManager.GetUserId(this.User);
            if (id == 0)
            {
                return View("../Home/Error");
            }

            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }

            ViewData["IdEmpresa"] = id;//new SelectList(_context.Empresa, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpresaId, Data, Divida")] DividaAT dividaAT)
        {

            if (ModelState.IsValid)
            {
                _context.Add(dividaAT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = dividaAT.EmpresaId });
            }
            
            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, EmpresaId, Data, Divida")] DividaAT dividaAT)
        {
            if (id != dividaAT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dividaAT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                        throw;
                }
                
                return RedirectToAction(nameof(Index), new { id = dividaAT.EmpresaId });
            }

            
            return View(dividaAT);
        }

            public async Task<IActionResult> Delete(int? id)
            {
               return await Verificacao(id);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var dividaAT = await _context.DividaAT.FindAsync(id);
                _context.DividaAT.Remove(dividaAT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = dividaAT.EmpresaId });
            }

            public async Task<IActionResult> Details(int? id)
            {
                return await Verificacao(id);
            }

            private async Task<IActionResult> Verificacao(int? id)
            {
            if (id == null)
                {
                    return NotFound();
                }

                var dividaAt = await _context.DividaAT.FindAsync(id);
                if (dividaAt == null)
                {
                    return NotFound();
                }

                return View(dividaAt);
            }
    }
}
