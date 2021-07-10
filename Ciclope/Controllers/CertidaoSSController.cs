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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace Ciclope.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class CertidaoSSController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public CertidaoSSController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
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

            var applicationDbContext = _context.DocCertidaoSS.Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: DocCertidaoAT/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        public IActionResult Create(int id)
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

            ViewData["IdEmpresa"] = id;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocCertidaoSS DocCertidaoSS)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocCertidaoSS.LocalFicheiro = fileName;
                DocCertidaoSS = Processador.ProcessarCertidaoSS(StorageUtils.getFile(fileName), DocCertidaoSS);
                _context.Add(DocCertidaoSS);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocCertidaoSS.IdEmpresa });
            }
            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

      

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine(id);

            var DocCertidaoSS = await _context.DocCertidaoSS.FindAsync(id);
            _context.DocCertidaoSS.Remove(DocCertidaoSS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocCertidaoSS.IdEmpresa });
        }


        // GET: DocIUC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocIUC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, IdEmpresa, LocalFicheiro, DataEmissao, DataFimValidade")] DocCertidaoSS DocCertidaoSS)
        {
            _context.Update(DocCertidaoSS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocCertidaoSS.IdEmpresa });
        }

        private bool DocCertidaoSSExists(int id)
        {
            return _context.DocCertidaoSS.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/CertidaoSSs/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocCertidaoSS = await _context.DocCertidaoSS
                    .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocCertidaoSS.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/CertidaoSSPdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocCertidaoSS = await _context.DocCertidaoSS
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocCertidaoSS.LocalFicheiro), "application/pdf", DocCertidaoSS.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            ////string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocCertidaoSS = await _context.DocCertidaoSS
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocCertidaoSS == null)
            {
                return View("../Home/Error");
            }

            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocCertidaoSS.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocCertidaoSS);
        }
    }
}
