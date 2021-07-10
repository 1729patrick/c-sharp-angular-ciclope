using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ciclope.Data;
using Ciclope.Models;
using System.IO;
using System.Threading;
using Ciclope.Models.UpdateClasses;

namespace Ciclope.Controllers
{
    /// <summary>
    /// Classes responsável pelo api referente a Fatura
    /// </summary>
    [Route("api/Faturas")]
    [ApiController]
    public class FaturasControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public FaturasControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Faturas_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<Faturas>>> GetFaturas(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.Faturas.ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Faturas_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <param name="id">File Id</param>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}/{id}")]
        public async Task<ActionResult<Faturas>> GetFaturas(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var Faturas = await _context.Faturas.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                            && m.Id == id);

            if (Faturas == null)
            {
                return NotFound();
            }

            return Faturas;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Faturas_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="faturaUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<Faturas>> PutFaturas(string apiKey, int id,FaturaUpdate faturaUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var Faturas = await _context.Faturas.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                            && m.Id == id);

            if (Faturas == null)
            {
                return NotFound();
            }

            if (faturaUpdate == null)
            {
                return BadRequest();
            }
            Faturas.numero = faturaUpdate.Numero;
            Faturas.Descricao = faturaUpdate.Descricao;
            Faturas.Valor = faturaUpdate.Valor;
            Faturas.Entidade = faturaUpdate.Entidade;
            Faturas.Data = faturaUpdate.Data;
            Faturas.ValorIva = faturaUpdate.ValorIva;

            _context.Entry(Faturas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetFaturas", new { id = Faturas.Id }, Faturas);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaturasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Insert new Pdf File
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Faturas_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="file">Pdf File</param>
        /// <returns>Data from file</returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPost("{apiKey}")]
        public async Task<ActionResult<Faturas>> PostFaturas(string apiKey, IFormFile file)
        {

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            Faturas Faturas = new();
            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            Faturas.LocalFicheiro = fileName;
            var key = await FaturasController.MakeRequest(StorageUtils.getFile(fileName));
            Thread.Sleep(15000);
            Faturas.Valor = await FaturasController.MakeResponse(key);


            Faturas.EmpresaId = empresa.Id;
            Faturas.Data = DateTime.Now;
            _context.Faturas.Add(Faturas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFaturas", new { id = Faturas.Id }, Faturas);
        }

        /// <summary>
        /// Deletes a specific Fatura.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/Faturas_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>Nothing</returns>
        /// <response code="204">If File Was Deleted</response>
        /// <response code="404">If the file was not found</response>    
        [HttpDelete("{apiKey}/{id}")]
        public async Task<IActionResult> DeleteFaturas(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var Faturas = await _context.Faturas.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                            && m.Id == id);

            if (Faturas == null)
            {
                return NotFound();
            }

            _context.Faturas.Remove(Faturas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FaturasExists(int id)
        {
            return _context.Faturas.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of Fatura
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Faturas_API/{apiKey}/Ficheiro/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/Ficheiro/{id:int}")]
        public async Task<ActionResult> DownloadCertificado(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var Faturas = await _context.Faturas.FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                && m.Id == id);
            if (Faturas == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(Faturas.LocalFicheiro), "image/jpeg", Faturas.LocalFicheiro);
        }
    }
}
