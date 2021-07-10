using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ciclope.Data;
using Ciclope.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ciclope.Controllers
{
    public class TrabalhadorUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CiclopeUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public TrabalhadorUserController(ApplicationDbContext context, UserManager<CiclopeUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: TrabalhadorUser
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
            var applicationDbContext = _context.TrabalhadorUser
                .Include(t => t.Empresa)
                .Include(t => t.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(e => e.EmpresaId == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: TrabalhadorUser/Create
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

            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            ViewData["Users"] = new SelectList(_userManager.Users, "Id", "Email");
            ViewData["IdEmpresa"] = id;
            return View();
        }

        // POST: TrabalhadorUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,UserId,EmpresaId")] TrabalhadorUser TrabalhadorUser)
        {

            if (ModelState.IsValid)
            {
                var user =  _context.Users.Where(u => u.Id == TrabalhadorUser.UserId)
                    .Include(u=> u.UserRoles)
                    .ThenInclude(ur=> ur.Role)
                    .FirstOrDefault();

                if (user == null)
                {
                    return RedirectToAction(nameof(Index), new { id = TrabalhadorUser.EmpresaId });
                }


                if (user.UserRoles != null) {
                    var roles = user.UserRoles.Select(r => r.Role.Name);
                    _userManager.RemoveFromRolesAsync(user, roles).Wait();
                }

                _userManager.AddToRoleAsync(user, TrabalhadorUser.RoleId).Wait();

                _context.Add(TrabalhadorUser);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = TrabalhadorUser.EmpresaId });
            }
            return RedirectToAction(nameof(Index), new { id = TrabalhadorUser.EmpresaId });
        }


        // GET: TrabalhadorUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }


            var TrabalhadorUser = await _context.TrabalhadorUser
                .Include(t => t.Empresa)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TrabalhadorId == id);
            if (TrabalhadorUser == null)
            {
                return NotFound();
            }

            return View(TrabalhadorUser);
        }

        // POST: TrabalhadorUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var TrabalhadorUser = await _context.TrabalhadorUser.FindAsync(id);
            _context.TrabalhadorUser.Remove(TrabalhadorUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = TrabalhadorUser.EmpresaId });
        }

        private bool TrabalhadorUserExists(int id)
        {
            return _context.TrabalhadorUser.Any(e => e.TrabalhadorId == id);
        }
    }
}
