using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciclope.Data;
using Ciclope.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
namespace Ciclope.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocDRIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CiclopeUser> _userManager;

        public DocDRIController(ApplicationDbContext context,  UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DocDRI
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
            var applicationDbContext = _context.DocDRI.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocDRI/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: DocDRI/Create
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

        // POST: DocDRI/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocDRI DocDRI)
        {
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                Stream stream = files[0].OpenReadStream();
                StorageUtils.uploadFile(stream, fileName);
                DocDRI.LocalFicheiro = fileName;
                DocDRI = Processador.ProcessarDri(StorageUtils.getFile(fileName), DocDRI);

                _context.Add(DocDRI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocDRI.IdEmpresa });
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: DocDRI/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DocDRI = await _context.DocDRI.FindAsync(id);
            if (DocDRI == null)
            {
                return NotFound();
            }
            ViewData["IdEmpresa"] = new SelectList(_context.Empresa, "Id", "Email", DocDRI.IdEmpresa);
            return View(DocDRI);
        }

        // POST: DocDRI/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,DataEntrega,DataRegisto,Identificador,Estado,NIdentificacaoSS,Nome,TotalRemuneracoes,TotalContribuicoes,LocalFicheiro")] DocDRI DocDRI)
        {
            if (id != DocDRI.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocDRI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocDRIExists(DocDRI.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = DocDRI.IdEmpresa });
            }
            ViewData["IdEmpresa"] = new SelectList(_context.Empresa, "Id", "Email", DocDRI.IdEmpresa);
            return View(DocDRI);
        }

        // GET: DocDRI/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocDRI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocDRI = await _context.DocDRI.FindAsync(id);
            _context.DocDRI.Remove(DocDRI);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocDRI.IdEmpresa });
        }

        private bool DocDRIExists(int id)
        {
            return _context.DocDRI.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/dri/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocDRI = await _context.DocDRI
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocDRI.LocalFicheiro), "application/pdf");
        }

        [HttpGet]
        [Route("/driPdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocDRI = await _context.DocDRI
                .FirstOrDefaultAsync(m => m.Id == id);

            return File(StorageUtils.getFile(DocDRI.LocalFicheiro), "application/pdf", DocDRI.LocalFicheiro);
        }


        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocDRI = await _context.DocDRI
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocDRI == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocDRI.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocDRI);
        }
    }
}
