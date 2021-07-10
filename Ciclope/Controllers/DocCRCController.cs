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
    public class DocCRCController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CiclopeUser> _userManager;

        public DocCRCController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocCRC
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
            var applicationDbContext = _context.DocCRC.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocCRC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocCRC/Create
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

        // POST: DocCRC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocCRC DocCRC)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocCRC.LocalFicheiro = fileName;
                DocCRC = Processador.ProcessarCRC(StorageUtils.getFile(fileName), DocCRC);
                DocCRC.DataSubmissao = DateTime.Now;

                _context.Add(DocCRC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocCRC.IdEmpresa });
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: DocCRC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocCRC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,Nome,Nif,Total,EmIncumprimento,MontantePotencial,NProdutos,DataEmissao,LocalFicheiro,DataSubmissao")] DocCRC DocCRC)
        {
            if (id != DocCRC.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocCRC);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocCRCExists(DocCRC.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocCRC.IdEmpresa });
            }
            ViewData["IdEmpresa"] = new SelectList(_context.Empresa, "Id", "Email", DocCRC.IdEmpresa);
            return View(DocCRC);
        }

        // GET: DocCRC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocCRC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocCRC = await _context.DocCRC.FindAsync(id);
            _context.DocCRC.Remove(DocCRC);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocCRC.IdEmpresa });
        }

        private bool DocCRCExists(int id)
        {
            return _context.DocCRC.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/CRC/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocCRC = await _context.DocCRC.FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocCRC.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/CRCpdf/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var DocCRC = await _context.DocCRC.FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocCRC.LocalFicheiro), "application/pdf", DocCRC.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocCRC = await _context.DocCRC
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocCRC == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocCRC.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocCRC);
        }

    }
}
