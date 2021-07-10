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
using Microsoft.AspNetCore.Http;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class M22Controller : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public M22Controller(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
            var applicationDbContext = _context.DocM22.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocM22 DocM22)
        {

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {


                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                    Stream stream = files[0].OpenReadStream();
                    StorageUtils.uploadFile(stream, fileName);
                    DocM22.LocalFicheiro = fileName;
                    DocM22 = Processador.ProcessarM22(StorageUtils.getFile(fileName), DocM22);
                }



                _context.Add(DocM22);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocM22.IdEmpresa });
            }

            return RedirectToAction(nameof(Create));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id, IdEmpresa, IdDeclaracao, Ano, IdentDocumento, IdentificacaoFiscal, ImportanciaPagar, RefPagamento, LocalFicheiro, LocalComprovativo")] DocM22 DocM22)
        {
            if (id != DocM22.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {


                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                        Stream stream = files[0].OpenReadStream();
                        StorageUtils.uploadFile(stream, fileName);
                        DocM22.LocalComprovativo = fileName;

                    }


                    _context.Update(DocM22);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocM22Exists(DocM22.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocM22.IdEmpresa });
            }
            ViewData["IdEmpresa"] = DocM22.IdEmpresa;
            return RedirectToAction(nameof(Index), new { id = DocM22.IdEmpresa });
        }

        private bool DocM22Exists(int id)
        {
            return _context.DocM22.Any(e => e.Id == id);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocM22 = await _context.DocM22.FindAsync(id);
            _context.DocM22.Remove(DocM22);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocM22.IdEmpresa });
        }

        [HttpGet]
        [Route("/M22/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocM22 = await _context.DocM22
                 .Include(t => t.Empresa)
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocM22.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/M22Pdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocM22 = await _context.DocM22
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocM22.LocalFicheiro), "application/pdf", DocM22.LocalFicheiro);
        }

        [HttpGet]
        [Route("/M22CompPdf/{id}")]
        public async Task<FileStreamResult> DownloadComprovativo(int id)
        {
            var DocM22 = await _context.DocM22
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocM22.LocalComprovativo), "application/pdf", DocM22.LocalComprovativo);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocM22 = await _context.DocM22
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocM22 == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocM22.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocM22);
        }

    }

}
