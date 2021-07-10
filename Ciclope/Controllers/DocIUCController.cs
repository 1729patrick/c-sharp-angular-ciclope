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
using Microsoft.AspNetCore.Authorization;
namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocIUCController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public DocIUCController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocIUC
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
            var applicationDbContext = _context.DocIUC.Include(t => t.Empresa).Where(e => e.EmpresaId == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocIUC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocIUC/Create
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

        // POST: DocIUC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpresaId")] DocIUC DocIUC)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {

                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                    Stream stream = files[0].OpenReadStream();
                    StorageUtils.uploadFile(stream, fileName);
                    DocIUC.LocalFicheiro = fileName;
                    DocIUC = Processador.ProcessarIUC(StorageUtils.getFile(fileName), DocIUC);
                }


                _context.Add(DocIUC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocIUC.EmpresaId });
            }
            ViewData["IdEmpresa"] = DocIUC.EmpresaId;
            return RedirectToAction(nameof(Index), new { id = DocIUC.EmpresaId });
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Matricula,Ano,Mes,IdentificacaoFicheiro,Data,IdentificacaoFiscal,Morada,DataLimite,ReferenciaPagamento,ImportanciaPagar,EmpresaId,LocalFicheiro")] DocIUC DocIUC)
        {
            if (id != DocIUC.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocIUC);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocIUCExists(DocIUC.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocIUC.EmpresaId });
            }

            return View(DocIUC);
        }

        // GET: DocIUC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocIUC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocIUC = await _context.DocIUC.FindAsync(id);
            _context.DocIUC.Remove(DocIUC);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocIUC.EmpresaId });
        }

        private bool DocIUCExists(int id)
        {
            return _context.DocIUC.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/IUC/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocIUC = await _context.DocIUC
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocIUC.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/IUCPdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocIUC = await _context.DocIUC
                .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocIUC.LocalFicheiro), "application/pdf", DocIUC.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocIUC = await _context.DocIUC
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocIUC == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocIUC.EmpresaId).Any())
            {
                return View("../Home/Error");
            }
            return View(DocIUC);
        }

    }
}
