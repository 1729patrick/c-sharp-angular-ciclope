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
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class CertificadoPermanenteController : Controller
  {

    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;
    private UserManager<CiclopeUser> _userManager;

    public CertificadoPermanenteController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<CiclopeUser> userManager)
    {
      _context = context;
      _hostEnvironment = hostEnvironment;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
      return View(await _context.CertidaoPermanente.ToArrayAsync());    
    }

    public async Task<IActionResult> Create()
    {

      CertidaoPermanente CertidaoPermanente = new CertidaoPermanente();

      CertidaoPermanente.Data = DateTime.Today;


      return View(CertidaoPermanente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,IdEmpresa,IdClassificacao,DataDecissao,DataEfeito,LocalFicheiro")] CertidaoPermanente CertidaoPermanente)
    {
        string webRootPath = _hostEnvironment.WebRootPath;
        var files = HttpContext.Request.Form.Files;

        if (files.Count > 0)
        {
          string fileName = Guid.NewGuid().ToString();
          var uploads = Path.Combine(webRootPath, @"files");
          var extension = Path.GetExtension(files[0].FileName);



          using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
          {
            files[0].CopyTo(filesStreams);
          }
          CertidaoPermanente.LocalFicheiro = @"\files\" + fileName + extension;
          CertidaoPermanente.IdEmpresa = 1;

          _context.Add(CertidaoPermanente);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
      }

      return RedirectToAction(nameof(Create));
    }
  }
}
