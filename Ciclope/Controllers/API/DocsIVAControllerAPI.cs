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
using Ciclope.Models.UpdateClasses;

namespace Ciclope.Controllers
{
    /// <summary>
    /// Classes responsável pelo api referente ao ficheiro IVA
    /// </summary>
    [Route("api/DocsIVA")]
    [ApiController]
    public class DocsIVAControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocsIVAControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocsIVA_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocsIVA>>> GetDocsIVA(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocsIVA.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocsIVA_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocsIVA>> GetDocsIVA(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocsIVA = await _context.DocsIVA.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocsIVA == null)
            {
                return NotFound();
            }

            return DocsIVA;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocsIVA_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="dRIUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocsIVA>> PutDocsIVA(string apiKey, int id, IvaUpdate ivaUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocsIVA = await _context.DocsIVA.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocsIVA == null)
            {
                return NotFound();
            }

            if (ivaUpdate == null)
            {
                return BadRequest();
            }

            DocsIVA.Nome = ivaUpdate.Nome;
            DocsIVA.Morada = ivaUpdate.Morada;
            DocsIVA.Localidade = ivaUpdate.Localidade;
            DocsIVA.CodigoPostal = ivaUpdate.CodigoPostal;
            DocsIVA.Periodo = ivaUpdate.Periodo;
            DocsIVA.IdDeclaracao = ivaUpdate.IdDeclaracao;
            DocsIVA.DataRececaoDeclaracao = ivaUpdate.DataRececaoDeclaracao;
            DocsIVA.Referencia = ivaUpdate.Referencia;
            DocsIVA.LinhaOptica = ivaUpdate.LinhaOptica;
            DocsIVA.Valor = ivaUpdate.Valor;

            _context.Entry(DocsIVA).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDocsIVA", new { id = DocsIVA.Id }, DocsIVA);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocsIVAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Insert new Pdf File
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocsIVA_API/{apiKey}
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
        public async Task<ActionResult<DocsIVA>> PostDocsIVA(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocsIVA DocsIVA = new();
            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocsIVA.LocalFicheiro = fileName;
            DocsIVA = Processador.ProcessarIva(StorageUtils.getFile(fileName), DocsIVA);
            DocsIVA.IdEmpresa = empresa.Id;

            _context.DocsIVA.Add(DocsIVA);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocsIVA", new { id = DocsIVA.Id }, DocsIVA);
        }

        /// <summary>
        /// Deletes a specific IVA.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocsIVA_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocsIVA(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocsIVA = await _context.DocsIVA.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);
            if (DocsIVA == null)
            {
                return NotFound();
            }

            _context.DocsIVA.Remove(DocsIVA);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocsIVAExists(int id)
        {
            return _context.DocsIVA.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of IVA documento de pagamento
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocsIVA_API/{apiKey}/Ficheiro/{id}
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
        public async Task<ActionResult> DownloadFicheiro(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocsIVA = await _context.DocsIVA.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocsIVA == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocsIVA.LocalFicheiro), "application/pdf", DocsIVA.LocalFicheiro);
        }

        /// <summary>
        /// Get File of IVA Comprovativo de pagamento
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocsIVA_API/{apiKey}/Comprovativo/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{IdEmpresa}/Comprovativo/{Id}")]
        public async Task<ActionResult> DownloadCertificado(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocsIVA = await _context.DocsIVA.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocsIVA == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocsIVA.LocalComprovativo), "application/pdf", DocsIVA.LocalComprovativo);
        }
    }
}
