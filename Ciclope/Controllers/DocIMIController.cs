using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciclope.Data;
using Ciclope.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocIMIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public DocIMIController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocIMI
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
            var applicationDbContext = _context.DocIMI.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocIMI/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocIMI/Create
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

        // POST: DocIMI/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocIMI DocIMI)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {

                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                    Stream stream = files[0].OpenReadStream();
                    StorageUtils.uploadFile(stream, fileName);
                    DocIMI.LocalFicheiro = fileName;
                    DocIMI = Processador.ProcessarIMI(StorageUtils.getFile(fileName), DocIMI);
                }


                _context.Add(DocIMI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocIMI.IdEmpresa });
            }
            ViewData["IdEmpresa"] = DocIMI.IdEmpresa;
            return RedirectToAction(nameof(Index), new { id = DocIMI.IdEmpresa });
        }

        // GET: DocIMI/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocIMI/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,IdentificacaoFiscal,AnoImposto,IdentificacaoDocumento,DataLiquidacao,ReferenciaPagamento,ImportanciaPagar,AnoPagamento,MesPagamento,LocalFicheiro")] DocIMI DocIMI)
        {
            if (id != DocIMI.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocIMI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocIMIExists(DocIMI.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocIMI.IdEmpresa });
            }
            ViewData["IdEmpresa"] = DocIMI.IdEmpresa;
            return View(DocIMI);
        }

        // GET: DocIMI/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocIMI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocIMI = await _context.DocIMI.FindAsync(id);
            _context.DocIMI.Remove(DocIMI);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocIMI.IdEmpresa });
        }

        private bool DocIMIExists(int id)
        {
            return _context.DocIMI.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/IMI/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocIMI = await _context.DocIMI
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocIMI.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/IMIPdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocIMI = await _context.DocIMI
                .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocIMI.LocalFicheiro), "application/pdf", DocIMI.LocalFicheiro);
        }


        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocIMI = await _context.DocIMI
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocIMI == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocIMI.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocIMI);
        }
    }
}
