using System;
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
using System.Threading;
using Microsoft.AspNetCore.Authorization;
namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocDMRController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CiclopeUser> _userManager;

        public DocDMRController(ApplicationDbContext context, UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocDMR
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
            var applicationDbContext = _context.DocDMR.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocDMR/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocDMR/Create
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

        // POST: DocDMR/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocDMR DocDMR)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocDMR.LocalFicheiro = fileName;
                DocDMR = Processador.ProcessarDMR(StorageUtils.getFile(fileName), DocDMR);

                _context.Add(DocDMR);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocDMR.IdEmpresa });
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: DocDMR/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocDMR/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,Nome,Morada,Localidade,CodigoPostal,Periodo,IdDeclaracao,DataRececaoDeclaracao,ReferenciaPagamento,LinhaOptica,ImportanciaPagar,LocalFicheiro,LocalDeclaracao")] DocDMR DocDMR)
        {
            if (id != DocDMR.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext?.Request.Form.Files;

                    if (files?.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                        Stream stream = files[0].OpenReadStream();
                        StorageUtils.uploadFile(stream, fileName);
                        DocDMR.LocalDeclaracao = fileName;

                    }
                    _context.Update(DocDMR);
                    await _context.SaveChangesAsync(); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocDMRExists(DocDMR.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocDMR.IdEmpresa });
            }
            ViewData["IdEmpresa"] = new SelectList(_context.Empresa, "Id", "Email", DocDMR.IdEmpresa);
            return View(DocDMR);
        }

        // GET: DocDMR/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocDMR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocDMR = await _context.DocDMR.FindAsync(id);
            _context.DocDMR.Remove(DocDMR);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocDMR.IdEmpresa });
        }

        private bool DocDMRExists(int id)
        {
            return _context.DocDMR.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/DMRs/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocDMR = await _context.DocDMR
                    .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocDMR.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/DMRsPdf/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var DocDMR = await _context.DocDMR
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocDMR.LocalFicheiro), "application/pdf", DocDMR.LocalFicheiro);
        }

        [HttpGet]
        [Route("/DMRcPdf/{id}")]
        public async Task<ActionResult> DownloadComprovativo(int id)
        {
            var DocDMR = await _context.DocDMR
                .FirstOrDefaultAsync(m => m.Id == id);
            try
            {
                return File(StorageUtils.getFile(DocDMR.LocalDeclaracao), "application/pdf", DocDMR.LocalDeclaracao);
            }
            catch (Exception)
            {

                return NotFound();
            }


        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocDMR = await _context.DocDMR
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocDMR == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocDMR.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocDMR);
        }

    }
}
