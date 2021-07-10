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
    public class CertidaoATController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public CertidaoATController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            string userid = _userManager.GetUserId(this.User);
            if (id == 0)
            {
                return View("../Home/Error");
            }

            if (!_context.TrabalhadorUser.Where(user => user.UserId == userid && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }

            var applicationDbContext = _context.DocCertidaoAT.Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }


        
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
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocCertidaoAT DocCertidaoAT)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocCertidaoAT.LocalFicheiro = fileName;
                DocCertidaoAT = Processador.ProcessarCertidaoAT(StorageUtils.getFile(fileName), DocCertidaoAT);

                _context.Add(DocCertidaoAT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocCertidaoAT.IdEmpresa });

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

            var DocCertidaoAT = await _context.DocCertidaoAT.FindAsync(id);
            _context.DocCertidaoAT.Remove(DocCertidaoAT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocCertidaoAT.IdEmpresa });
        }




        // GET: DocIUC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, IdEmpresa, LocalFicheiro, CodigoValidacao, DataValidade")] DocCertidaoAT DocCertidaoAT)
        {
            _context.Update(DocCertidaoAT);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = DocCertidaoAT.IdEmpresa });
        }

        private bool DocCertidaoATExists(int id)
        {
            return _context.DocCertidaoAT.Any(e => e.Id == id);
        }


        [HttpGet]
        [Route("/CertidaoATs/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocCertidaoAT = await _context.DocCertidaoAT
                    .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocCertidaoAT.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/CertidaoATPdf/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var DocCertidaoAT = await _context.DocCertidaoAT
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocCertidaoAT.LocalFicheiro), "application/pdf", DocCertidaoAT.LocalFicheiro);
        }



        private async Task<IActionResult> Verificacao(int? id)
        {
            string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocCertidaoAT = await _context.DocCertidaoAT
               .FirstOrDefaultAsync(m => m.Id == id);


            if (DocCertidaoAT == null)
            {
                return View("../Home/Error");
            }


            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocCertidaoAT.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocCertidaoAT);

        }
    }
}
