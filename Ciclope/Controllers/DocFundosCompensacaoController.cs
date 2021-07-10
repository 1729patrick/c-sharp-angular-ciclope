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
using System.IO;
using System.Threading;
using Azure.Storage.Files.Shares;
using Azure;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Ciclope.Controllers
{
    /// <summary>
    /// Controlador responsável por Fundos Compensacao
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocFundosCompensacaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;
        /// <summary>
        /// Construtor da Classe
        /// </summary>
        /// <param name="context">Acesso a base de dados</param>
        /// <param name="userManager">Acesso ao user ativo</param>
        public DocFundosCompensacaoController(ApplicationDbContext context,UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Get: DocFundosCompensacao
        /// Retorna o ecra responsável pela listagem do Fundos
        /// </summary>
        /// <param name="id"> Id de Empresa </param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int id)
        {

            if (id == 0)
            {
                return View("../Home/Error");
            }
            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }
            var applicationDbContext = _context.DocFundosCompensacao.Include(t => t.Empresa).Where(e => e.IdEmpresa == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// GET: DocFundosCompensacao/Details/
        /// Retorna a pagina de um fundos em especifico
        /// </summary>
        /// <param name="id">Id do Fundo</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        /// <summary>
        /// GET: DocFundosCompensacao/Create/IdEmpresa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        // POST: DocFundosCompensacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa")] DocFundosCompensacao DocFundosCompensacao)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {


                    string fileName = Guid.NewGuid().ToString()+ System.IO.Path.GetExtension(files[0].FileName);
                    Stream stream = files[0].OpenReadStream();
                    StorageUtils.uploadFile(stream, fileName);
                    DocFundosCompensacao.LocalFicheiro = fileName;
                    DocFundosCompensacao = Processador.ProcessarFundos(StorageUtils.getFile(fileName), DocFundosCompensacao);
                }

                

                _context.Add(DocFundosCompensacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = DocFundosCompensacao.IdEmpresa });
            }
            ViewData["IdEmpresa"] =  DocFundosCompensacao.IdEmpresa;
            return RedirectToAction(nameof(Index), new { id = DocFundosCompensacao.IdEmpresa });
        }





        // GET: DocFundosCompensacao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocFundosCompensacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpresa,DataEmissao,PeriodoPagamentoInicio,PeriodoPagamentoFim,Nome,Niss,Valor,Entidade,Referencia,LocalFicheiro")] DocFundosCompensacao DocFundosCompensacao)
        {
            if (id != DocFundosCompensacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(DocFundosCompensacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocFundosCompensacaoExists(DocFundosCompensacao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new  { id = DocFundosCompensacao.IdEmpresa});
            }
            return View(DocFundosCompensacao);
        }

        // GET: DocFundosCompensacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: DocFundosCompensacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var DocFundosCompensacao = await _context.DocFundosCompensacao.FindAsync(id);
            _context.DocFundosCompensacao.Remove(DocFundosCompensacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = DocFundosCompensacao.IdEmpresa });
        }

        private bool DocFundosCompensacaoExists(int id)
        {
            return _context.DocFundosCompensacao.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/Fundos/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var DocFundosCompensacao = await _context.DocFundosCompensacao
                 .Include(t => t.Empresa)
                 .FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(DocFundosCompensacao.LocalFicheiro), "application/pdf");
        }


        [HttpGet]
        [Route("/FundosPdf/{id}")]
        public async Task<FileStreamResult> DownloadFile(int id)
        {
            var DocFundosCompensacao = await _context.DocFundosCompensacao
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return File(StorageUtils.getFile(DocFundosCompensacao.LocalFicheiro), "application/pdf", DocFundosCompensacao.LocalFicheiro);
        }

        /// <summary>
        /// Verificacao do id dos Fundos
        /// </summary>
        /// <param name="id">Id Ficheiro</param>
        /// <returns></returns>
        private async Task<IActionResult> Verificacao(int? id)
        {
            string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocFundosCompensacao = await _context.DocFundosCompensacao
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (DocFundosCompensacao == null)
            {
                return View("../Home/Error");
            }


            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == DocFundosCompensacao.IdEmpresa).Any())
            {
                return View("../Home/Error");
            }
            return View(DocFundosCompensacao);
        }

    }
}
