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
using Microsoft.AspNetCore.Http;

namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocRelatorioUnicoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CiclopeUser> _userManager;

        public DocRelatorioUnicoController(ApplicationDbContext context,  UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocRelatorioUnico
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
            var applicationDbContext = _context.DocRelatorioUnico.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocRelatorioUnico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocRelatorioUnico/Create
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

        // POST: DocRelatorioUnico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocRelatorioUnico DocRelatorioUnico)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocRelatorioUnico.LocalAnexo0 = fileName;
                DocRelatorioUnico = Processador.ProcessarRU(StorageUtils.getFile(fileName), DocRelatorioUnico);

                _context.Add(DocRelatorioUnico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocRelatorioUnico.IdEmpresa });

            }

            return RedirectToAction(nameof(Create));
        }

        // GET: DocRelatorioUnico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocRelatorioUnico/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,Ano,LocalAnexo0,LocalAnexoA,LocalAnexoB,LocalAnexoC,LocalAnexoD,LocalAnexoE,LocalAnexoF,LocalCertificado")] DocRelatorioUnico DocRelatorioUnico)
        {
            if (id != DocRelatorioUnico.Id)
            {
                return NotFound();
            }

            var files = HttpContext.Request.Form.Files;

            if (ModelState.IsValid)
            {
                try
                {

                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                        Stream stream = file.OpenReadStream();
                        StorageUtils.uploadFile(stream, fileName);
                        switch (file.Name)
                        {
                            case "LocalAnexoA":
                                DocRelatorioUnico.LocalAnexoA = fileName;
                                break;
                            case "LocalAnexoB":
                                DocRelatorioUnico.LocalAnexoB = fileName;
                                break;
                            case "LocalAnexoC":
                                DocRelatorioUnico.LocalAnexoC = fileName;
                                break;
                            case "LocalAnexoD":
                                DocRelatorioUnico.LocalAnexoD = fileName;
                                break;
                            case "LocalAnexoE":
                                DocRelatorioUnico.LocalAnexoE = fileName;
                                break;
                            case "LocalAnexoF":
                                DocRelatorioUnico.LocalAnexoF = fileName;
                                break;
                            case "LocalCertificado":
                                DocRelatorioUnico.LocalCertificado = fileName;
                                break;
                            default:
                                break;
                        }

                    }
                    _context.Update(DocRelatorioUnico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocRelatorioUnicoExists(DocRelatorioUnico.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocRelatorioUnico.IdEmpresa });
            }

            return View(DocRelatorioUnico);
        }

        // GET: DocRelatorioUnico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocRelatorioUnico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico.FindAsync(id);
            _context.DocRelatorioUnico.Remove(DocRelatorioUnico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocRelatorioUnico.IdEmpresa });
        }

        private bool DocRelatorioUnicoExists(int id)
        {
            return _context.DocRelatorioUnico.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                    .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexo0), "application/pdf");
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexo0), "application/pdf", DocRelatorioUnico.LocalAnexo0);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadAFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoA), "application/pdf", DocRelatorioUnico.LocalAnexoA);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadBFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoB), "application/pdf", DocRelatorioUnico.LocalAnexoB);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadCFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoC), "application/pdf", DocRelatorioUnico.LocalAnexoC);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadDFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoD), "application/pdf", DocRelatorioUnico.LocalAnexoD);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadEFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoE), "application/pdf", DocRelatorioUnico.LocalAnexoE);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoF), "application/pdf", DocRelatorioUnico.LocalAnexoF);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadCertFile(int id)
        {
            var DocRelatorioUnico = await _context.DocRelatorioUnico
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocRelatorioUnico.LocalCertificado), "application/pdf", DocRelatorioUnico.LocalCertificado);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocRelatorioUnico == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocRelatorioUnico.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocRelatorioUnico);
        }


    }
}
