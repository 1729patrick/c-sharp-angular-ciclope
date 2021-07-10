using Ciclope.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Ciclope.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Ajuda()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Permissoes()
        {
            return View();
        }

        public IActionResult UploadFaturas()
        {
            return View();
        }

        public IActionResult ListaSAFT()
        {
            return View();
        }

        public IActionResult ListaIUC()
        {
            return View();
        }

        public IActionResult ListaFaturas()
        {
            return View();
        }

        public IActionResult ListaCertidaoPermanente()
        {
            return View();
        }

        public IActionResult CriacaoRelatoriosAutomaticos()
        {
            return View();
        }

        public IActionResult PaginaIUC()
        {
            return View();
        }

        public IActionResult ListaCertificadoIAPMEI()
        {
            return View();
        }

        public IActionResult CertidaoPermanente()
        {
            return View();
        }

        public IActionResult PaginaFatura()
        {
            return View();
        }

        public IActionResult CertificadoIAPMEI()
        {
            return View();
        }

        public IActionResult PaginaSAFT()
        {
            return View();
        }


    }
}
