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
using System.Net.Http;
using System.Web;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Ciclope.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class FaturasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<CiclopeUser> _userManager;


        public FaturasController(ApplicationDbContext context,  UserManager<CiclopeUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Faturas
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
            var applicationDbContext = _context.Faturas.Include(t => t.Empresa).Where(e => e.EmpresaId == id);
            ViewData["IdEmpresa"] = id;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Faturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await Verificacao(id);
        }

        // GET: Faturas/Create
        public IActionResult Create(int id) {
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

        // POST: Faturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpresaId")] Faturas Faturas)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {

                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                    Stream stream = files[0].OpenReadStream();
                    StorageUtils.uploadFile(stream, fileName);
                    Faturas.LocalFicheiro = fileName;
                    var key = await MakeRequest(StorageUtils.getFile(fileName));
                    Thread.Sleep(10000);
                    Faturas.Valor = await MakeResponse(key);

                }
                Faturas.Data = DateTime.Now;
                _context.Add(Faturas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = Faturas.EmpresaId });
            }
            return View(Faturas);
        }

        public static async Task<string> MakeRequest(Stream file)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "09beb9b8772342f89d06449f87d43738");

            // Request parameters
            queryString["includeTextDetails"] = "true";
            var uri = "https://faturasrec.cognitiveservices.azure.com/formrecognizer/v2.1-preview.2/prebuilt/invoice/analyze?" + queryString;

            HttpResponseMessage response;

            // Request body
            //byte[] byteData = Encoding.UTF8.GetBytes(file.);
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                // act on the Base64 data
                using (var content = new ByteArrayContent(fileBytes))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    response = await client.PostAsync(uri, content);
                }
            }
           
            var headers = response.Headers;
            IEnumerable<string> values;
            if (headers.TryGetValues("apim-request-id", out values))
            {
                return  values.First();
            }
            return null;


        }

        public static async Task<double> MakeResponse(string key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(key);
            Faturas Faturas = new Faturas();
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "09beb9b8772342f89d06449f87d43738");

            var uri = "https://faturasrec.cognitiveservices.azure.com/formrecognizer/v2.1-preview.2/prebuilt/invoice/analyzeResults/" + queryString;
            while (true){
                try
                {
                    var response = await client.GetAsync(uri);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(responseBody);
                    JObject reservations = JObject.Parse(responseBody);
                    var total = Convert.ToDouble(reservations["analyzeResult"]["documentResults"][0]["fields"]["InvoiceTotal"]["text"].ToString().Replace('€', ' ').Trim());
                    return total;
                }
                catch (Exception)
                {

                    Thread.Sleep(1000);
                }
               
            }
        }

        // GET: Faturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Verificacao(id);
        }

        // POST: Faturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,numero,Descricao,Valor,Entidade,LocalFicheiro,EmpresaId,QRcode,Data")] Faturas Faturas)
        {
            if (id != Faturas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Faturas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaturasExists(Faturas.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = Faturas.EmpresaId });
            }

            return View(Faturas);
        }

        // GET: Faturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Verificacao(id);
        }

        // POST: Faturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Faturas = await _context.Faturas.FindAsync(id);
            _context.Faturas.Remove(Faturas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = Faturas.EmpresaId });
        }

        private bool FaturasExists(int id)
        {
            return _context.Faturas.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(int id)
        {
            var Faturas = await _context.Faturas.FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(Faturas.LocalFicheiro), "image/jpeg");
        }

        [HttpGet]
        public async Task<ActionResult> DownloadFile(int id)
        {
            var Faturas = await _context.Faturas.FirstOrDefaultAsync(m => m.Id == id);
            return File(StorageUtils.getFile(Faturas.LocalFicheiro), "image/jpeg", Faturas.LocalFicheiro);
        }

        private async Task<IActionResult> Verificacao(int? id)
        {
            //string userId = _userManager.GetUserId(this.User);
            if (id == null)
            {
                return View("../Home/Error");
            }

            var Faturas = await _context.Faturas
            .Include(t => t.Empresa)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (Faturas == null)
            {
                return View("../Home/Error");
            }


            string userId = _userManager.GetUserId(this.User);
            if (!_context.TrabalhadorUser.Where(user => user.UserId == userId && user.EmpresaId == Faturas.EmpresaId).Any())
            {
                return View("../Home/Error");
            }
            return View(Faturas);
        }
    }
}
