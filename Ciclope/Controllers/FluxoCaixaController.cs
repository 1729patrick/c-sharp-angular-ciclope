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
    public class FluxoCaixaController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;

        public FluxoCaixaController(ApplicationDbContext context,UserManager<CiclopeUser> userManager)
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

            IEnumerable<CashFlow> list = await _context.CashFlow.Where(e => e.IdEmpresa == id).OrderBy(e => e.Data).ToArrayAsync();

            double acc = 0;
            list = list.Select((cash, index) => {
                acc += cash.Valor;

                return new CashFlow
                {
                    Id = cash.Id,
                    Cliente = cash.Cliente,
                    Data = cash.Data,
                    Empresa = cash.Empresa,
                    IdEmpresa = cash.IdEmpresa,
                    IdFatura = cash.IdFatura,
                    Valor = cash.Valor,
                    Saldo = acc
                };
            }
            );

            ViewData["IdEmpresa"] = id;
            return View(list);
        }


        // GET: RecibosVerdes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ////string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }          

            var CashFlow = await _context.CashFlow
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (CashFlow == null)
            {
                return View("../Home/Error");
            }

            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }

            var fatura = _context.Faturas.Where(f => f.Id == CashFlow.IdFatura).FirstOrDefault();


            return RedirectToAction("Details", "Faturas",new { id = fatura.Id });
        }

        public async Task<IActionResult> CreateAsync(int id)
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

            IEnumerable<int> cashflows = await _context.CashFlow.Where(e => e.IdEmpresa == id).Select(e => e.IdFatura).ToArrayAsync();

            ViewData["IdEmpresa"] = id;
            ViewData["Faturas"] = new SelectList(_context.Faturas
                .Where(fatura => fatura.numero != "" && fatura.EmpresaId == id)
                .Where(fatura=> !cashflows.Contains(fatura.Id))
            , "Id", "numero");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFatura, Cliente, IdEmpresa")] CashFlow cashFlow)
        {

            Console.WriteLine(cashFlow.IdFatura);
            Console.WriteLine(cashFlow.IdEmpresa);
            Console.WriteLine(cashFlow.Cliente);
            if (ModelState.IsValid)
            {


                var fatura = await _context.Faturas
                .FirstOrDefaultAsync(m => m.Id == cashFlow.IdFatura);
                cashFlow.Valor =  fatura.Valor;
                cashFlow.Data = DateTime.Now;

                _context.Add(cashFlow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = cashFlow.IdEmpresa });
            }

            return RedirectToAction(nameof(Create));
        }


        // GET: DocIUC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return NotFound();
            }

            var CashFlow = await _context.CashFlow
                .Include(t => t.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (CashFlow == null)
            {
                return View("../Home/Error");
            }

            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == id).Any())
            {
                return View("../Home/Error");
            }

            var fatura = _context.Faturas.Where(f => f.Id == CashFlow.IdFatura).FirstOrDefault();


            return RedirectToAction("Edit", "Faturas", new { id = fatura.Id });
        }
       

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("../Home/Error");
            }

            var DocRendas = await _context.CashFlow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (DocRendas == null)
            {
                return View("../Home/Error");
            }

            return View(DocRendas);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var CashFlow = await _context.CashFlow.FindAsync(id);
            _context.CashFlow.Remove(CashFlow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = CashFlow.IdEmpresa });
        }



        private bool CashFlowExists(int id)
        {
            return _context.CashFlow.Any(e => e.Id == id);
        }


    }

}
