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
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RecibosVerdesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public RecibosVerdesController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
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
            var applicationDbContext = _context.DocRecibosVerdes.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
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
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocRecibosVerdes DocRecibosVerdes)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocRecibosVerdes.LocalFicheiro = fileName;
                DocRecibosVerdes = Processador.ProcessarRecibosVerdes(StorageUtils.getFile(fileName), DocRecibosVerdes);

                _context.Add(DocRecibosVerdes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocRecibosVerdes.IdEmpresa });
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
        public async Task<IActionResult> Edit(int id, [Bind("Id, IdEmpresa, TransmitenteNome, TransmitenteAtividade, TransmitenteNif, TransmitenteDomicilio, AdquirenteNome, AdquirenteMorada, AdquirenteNif, DadosDataTransmissao, DadosDescricao, DadosValorBase, DadosIva, DadosImpostoSelo, DadosIRS, FaturaRecibo, DataEmissao, LocalFicheiro")] DocRecibosVerdes DocRecibosVerdes)
        {
            if (id != DocRecibosVerdes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocRecibosVerdes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocRecibosVerdesExists(DocRecibosVerdes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocRecibosVerdes.IdEmpresa });
            }
            return View(DocRecibosVerdes);
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

            var DocRecibosVerdes = await _context.DocRecibosVerdes.FindAsync(id);
            _context.DocRecibosVerdes.Remove(DocRecibosVerdes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocRecibosVerdes.IdEmpresa });
        }




        private bool DocRecibosVerdesExists(int id)
        {
            return _context.DocRecibosVerdes.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/RecibosV/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocRecibosVerdes = await _context.DocRecibosVerdes
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocRecibosVerdes.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/RecibosVPdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocRecibosVerdes = await _context.DocRecibosVerdes
                .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocRecibosVerdes.LocalFicheiro), "application/pdf", DocRecibosVerdes.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocRecibosVerdes = await _context.DocRecibosVerdes
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocRecibosVerdes == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocRecibosVerdes.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocRecibosVerdes);
        }

    }
}
