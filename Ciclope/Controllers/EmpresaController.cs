using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciclope.Data;
using Ciclope.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace Ciclope.Controllers {
    [Authorize]
    public class EmpresaController : Controller {

        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public EmpresaController(ApplicationDbContext context, UserManager<CiclopeUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        // GET: Empresa
        public async Task<IActionResult> Index() {
            string userId = _userManager.GetUserId(this.User);

            if (userId == null)
                return NotFound();

            var filterUserCompanies = _context.TrabalhadorUser.Where(user => user.UserId == userId && user.Empresa.Ativa).Select(user => user.Empresa);

            return View(await filterUserCompanies.ToListAsync());
        }            

        // GET: Empresa/Details/5
        public async Task<IActionResult> Details(int? id) {
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

            var Empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
           

            if (Empresa == null)
            {
                return NotFound();
            }

            return View(Empresa);
        }


        // GET: Empresa/Menu/5
        public async Task<IActionResult> Menu(int? id)
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

            var Empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);


            if (Empresa == null)
            {
                return NotFound();
            }

            return View(Empresa);
        }


        // GET: Empresa/Create
        public IActionResult Create(int id) {
            ViewData["Id"] = id;
            return View();
        }

        // POST: Empresa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Nif,Morada,CodigoPostal,Cidade,Pais,Email,Telefone")] Empresa Empresa)
        {
            if (ModelState.IsValid)
            {
                Empresa.ApiKey = Guid.NewGuid().ToString();
                Empresa.Ativa = true;
                _context.Add(Empresa);
                await _context.SaveChangesAsync();
                TrabalhadorUser ligacaoUserEmpresa = new TrabalhadorUser();
                ligacaoUserEmpresa.EmpresaId = Empresa.Id;
                ligacaoUserEmpresa.UserId = _userManager.GetUserId(this.User);
                _context.Add(ligacaoUserEmpresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Empresa);
        }

        // GET: Empresa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string userId = _userManager.GetUserId(this.User);
            if (id == 0)
            {
                return View("../Home/Error");
            }

            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }

            var Empresa = await _context.Empresa.FindAsync(id);
            if (Empresa == null)
            {
                return NotFound();
            }
            return View(Empresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Nif,Morada,CodigoPostal,Cidade,Pais,Email,Telefone,ApiKey,Ativa")] Empresa Empresa)
        {
            if (id != Empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(Empresa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Empresa);
        }

        // GET: Empresa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string userId = _userManager.GetUserId(this.User);
            if (id == 0)
            {
                return View("../Home/Error");
            }

            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }

            var Empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Empresa == null)
            {
                return NotFound();
            }

            return View(Empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Empresa = await _context.Empresa.FindAsync(id);
            Empresa.Ativa = false;
            _context.Update(Empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GenerateKey(int id)
        {
            var Empresa = await _context.Empresa.FindAsync(id);
            Empresa.ApiKey = Guid.NewGuid().ToString();
            _context.Update(Empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = id });
        }


        private bool EmpresaExists(int id)
        {
            return _context.Empresa.Any(e => e.Id == id);
        }
    }
}
