using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciclope.Data;
using Ciclope.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocsIVAController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CiclopeUser> _userManager;

        public DocsIVAController(ApplicationDbContext context,UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocsIVA
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
            var applicationDbContext = _context.DocsIVA.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocsIVA/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocsIVA/Create
        public IActionResult Create(int id)
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

            ViewData["IdEmpresa"] = id;
            return View();
        }

        // POST: DocsIVA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocsIVA DocsIVA)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocsIVA.LocalFicheiro = fileName;
                DocsIVA = Processador.ProcessarIva(StorageUtils.getFile(fileName), DocsIVA);

                _context.Add(DocsIVA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocsIVA.IdEmpresa });
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: DocsIVA/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocsIVA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,Nome,Morada,Localidade,CodigoPostal,Periodo,IdDeclaracao,DataRececaoDeclaracao,Referencia,LinhaOptica,Valor,LocalFicheiro,LocalComprovativo")] DocsIVA DocsIVA)
        {
            if (id != DocsIVA.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext?.Request.Form.Files;

                    if (files?.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                        Stream stream = files[0].OpenReadStream();
                        StorageUtils.uploadFile(stream, fileName);
                        DocsIVA.LocalComprovativo = fileName;
                    }
                    _context.Update(DocsIVA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocsIVAExists(DocsIVA.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocsIVA.IdEmpresa });
            }
            return View(DocsIVA);
        }

        // GET: DocsIVA/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocsIVA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocsIVA = await _context.DocsIVA.FindAsync(id);
            _context.DocsIVA.Remove(DocsIVA);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocsIVA.IdEmpresa });
        }

        private bool DocsIVAExists(int id)
        {
            return _context.DocsIVA.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocsIVA = await _context.DocsIVA
                    .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocsIVA.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var DocsIVA = await _context.DocsIVA
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocsIVA.LocalFicheiro), "application/pdf", DocsIVA.LocalFicheiro);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadComprovativo(int id)
        {
            var DocsIVA = await _context.DocsIVA
                .FirstOrDefaultAsync(m => m.Id == id);
            try
            {
                return File(StorageUtils.getFile(DocsIVA.LocalComprovativo), "application/pdf", DocsIVA.LocalComprovativo);
            }
            catch (Exception)
            {

                return NotFound();
            }

            
        }


        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocsIVA = await _context.DocsIVA
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocsIVA == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocsIVA.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocsIVA);
        }
    }
}
