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
using Microsoft.AspNetCore.Authorization;
namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocBCBController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CiclopeUser> _userManager;

        public DocBCBController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocBCB
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
            var applicationDbContext = _context.DocBCB.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocBCB/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocBCB/Create
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

        // POST: DocBCB/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocBCB DocBCB)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocBCB.LocalFicheiro = fileName;
                DocBCB = Processador.ProcessarBCB(StorageUtils.getFile(fileName), DocBCB);
                DocBCB.DataSubmissao = DateTime.Now;
                _context.Add(DocBCB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocBCB.IdEmpresa });
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: DocBCB/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocBCB/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,DataEmissao,LocalFicheiro,DataSubmissao")] DocBCB DocBCB)
        {
            if (id != DocBCB.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocBCB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocBCBExists(DocBCB.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocBCB.IdEmpresa });
            }
            return View(DocBCB);
        }

        // GET: DocBCB/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocBCB/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocBCB = await _context.DocBCB.FindAsync(id);
            _context.DocBCB.Remove(DocBCB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocBCB.IdEmpresa });
        }

        private bool DocBCBExists(int id)
        {
            return _context.DocBCB.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/BCB/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocBCB = await _context.DocBCB.FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocBCB.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/BCBpdf/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var DocBCB = await _context.DocBCB.FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocBCB.LocalFicheiro), "application/pdf", DocBCB.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocBCB = await _context.DocBCB
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocBCB == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocBCB.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocBCB);
        }

    }
}
