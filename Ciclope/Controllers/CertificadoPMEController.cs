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
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Ciclope.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class CertificadoPMEController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public CertificadoPMEController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
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

            var applicationDbContext = _context.DocCertificadoPME.Include(t => t.PMEClassificacao).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecibosVerdes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
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
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocCertificadoPME DocCertificadoPME)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocCertificadoPME.LocalFicheiro = fileName;
                DocCertificadoPME = Processador.ProcessarPME(StorageUtils.getFile(fileName), DocCertificadoPME, _context);
                _context.Add(DocCertificadoPME);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocCertificadoPME.IdEmpresa });
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

            var DocCertificadoPME = await _context.DocCertificadoPME.FindAsync(id);
            _context.DocCertificadoPME.Remove(DocCertificadoPME);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocCertificadoPME.IdEmpresa });
        }


        // GET: DocIUC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Classificacoes"] = new SelectList(_context.PMEClassificacao, "Id", "Classificacao");
            return await Verificacao(id);
        }

        // POST: DocIUC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,IdClassificacao, DataDecissao, DataEfeito, LocalFicheiro")] DocCertificadoPME DocCertificadoPME)
        {

            _context.Update(DocCertificadoPME);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocCertificadoPME.IdEmpresa });
        }

        private bool DocCertificadoPMEExists(int id)
        {
            return _context.DocCertificadoPME.Any(e => e.Id == id);
        }



        [HttpGet]
        [Route("/CPme/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocCertificadoPME = await _context.DocCertificadoPME
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocCertificadoPME.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/CPmePdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocCertificadoPME = await _context.DocCertificadoPME
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocCertificadoPME.LocalFicheiro), "application/pdf", DocCertificadoPME.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            ////string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocCertificadoPME = await _context.DocCertificadoPME
            .Include(t => t.PMEClassificacao)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocCertificadoPME == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocCertificadoPME.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocCertificadoPME);
        }

    }
}