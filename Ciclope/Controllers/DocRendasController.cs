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
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocRendasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public DocRendasController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocRendas
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

            var applicationDbContext = _context.DocRendas.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocRendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocRendas/Create
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

        // POST: DocRendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocRendas DocRendas)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                    Stream stream = files[0].OpenReadStream();
                    StorageUtils.uploadFile(stream, fileName);
                    DocRendas.LocalFicheiro = fileName;
                    DocRendas = Processador.ProcessarRenda(StorageUtils.getFile(fileName), DocRendas);
                }

                _context.Add(DocRendas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocRendas.IdEmpresa });
            }
            ViewData["IdEmpresa"] = DocRendas.IdEmpresa;
            return RedirectToAction(nameof(Index), new { id = DocRendas.IdEmpresa });
        }

        // GET: DocRendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocRendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,EntidadeNome,EntidadeNif,LocatorioNome,LocatorioNif,Arrendamento,Localizacao,PeriodoRendaInicio,PeriodoRendaFim,Titulo,DataRecebimento,RetencaoIRS,ImportanciaRecebida,NRecibosVenda,DataEmissao,LocalFicheiro")] DocRendas DocRendas)
        {
            if (id != DocRendas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocRendas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocRendasExists(DocRendas.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocRendas.IdEmpresa });
            }
            return View(DocRendas);
        }

        // GET: DocRendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocRendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocRendas = await _context.DocRendas.FindAsync(id);
            _context.DocRendas.Remove(DocRendas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocRendas.IdEmpresa });
        }

        private bool DocRendasExists(int id)
        {
            return _context.DocRendas.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/Rendas/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocRendas = await _context.DocRendas
                 .Include(t => t.Empresa)
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocRendas.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/RendasPdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocRendas = await _context.DocRendas
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRendas.LocalFicheiro), "application/pdf", DocRendas.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocRendas = await _context.DocRendas
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocRendas == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocRendas.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocRendas);
        }


    }
}
