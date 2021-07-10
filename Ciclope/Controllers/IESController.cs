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
    public class IESController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public IESController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
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
            var applicationDbContext = _context.DocIES.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: RecibosVerdes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DocIES = await _context.DocIES
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(DocIES);
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
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocIES DocIES)
        {

            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocIES.LocalFicheiro = fileName;
                DocIES = Processador.ProcessarIES(StorageUtils.getFile(fileName), DocIES);

                _context.Add(DocIES);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocIES.IdEmpresa });

            }

            return RedirectToAction(nameof(Create));
        }

       


        // GET: DocIUC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DocIES = await _context.DocIES.FindAsync(id);
            if (DocIES == null)
            {
                return NotFound();
            }
            return View(DocIES);
        }

        // POST: DocIUC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, IdEmpresa, Ano, Nif, Nome, LocalComprovativo, LocalFicheiro, LocalPagamento, Valor, Referencia,DataValidade,Entidade")] DocIES DocIES)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                foreach (IFormFile file in files)
                {
                    if (file.Name == "LocalPagamento")
                    {
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                        Stream stream = file.OpenReadStream();
                        StorageUtils.uploadFile(stream, fileName);
                        DocIES.LocalPagamento = fileName;
                    }
                    else
                    {
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                        Stream stream = file.OpenReadStream();
                        StorageUtils.uploadFile(stream, fileName);
                        DocIES.LocalComprovativo = fileName;
                    }
                }
            }


            _context.Update(DocIES);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index), new { id = DocIES.IdEmpresa });
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

            var DocIES = await _context.DocIES.FindAsync(id);
            _context.DocIES.Remove(DocIES);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocIES.IdEmpresa });
        }


        private bool DocIESExists(int id)
        {
            return _context.DocIES.Any(e => e.Id == id);
        }

      

        [HttpGet]
        [Route("/IesF/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocIES = await _context.DocIES
                    .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocIES.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/IesPdf/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var DocIES = await _context.DocIES
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocIES.LocalFicheiro), "application/pdf", DocIES.LocalFicheiro);
        }

        [HttpGet]
        [Route("/IesCmpPdf/{id}")]
        public async Task<ActionResult> DownloadCmp(int id)
        {
            var DocIES = await _context.DocIES
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocIES.LocalComprovativo), "application/pdf", DocIES.LocalComprovativo);
        }

        [HttpGet]
        [Route("/IesDucPdf/{id}")]
        public async Task<ActionResult> DownloadPagamento(int id)
        {
            var DocIES = await _context.DocIES
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocIES.LocalPagamento), "application/pdf", DocIES.LocalPagamento);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocIES = await _context.DocIES
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocIES == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocIES.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocIES);
        }

    }
}
