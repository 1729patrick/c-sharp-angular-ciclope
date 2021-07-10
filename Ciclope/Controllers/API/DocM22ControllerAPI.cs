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
    /// Classes responsável pelo api referente ao ficheiro M22
    /// </summary>
    [Route("api/DocM22")]
    [ApiController]
    public class DocM22ControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocM22ControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocM22_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocM22>>> GetDocM22(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocM22.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocM22_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocM22>> GetDocM22(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocM22 = await _context.DocM22.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocM22 == null)
            {
                return NotFound();
            }

            return DocM22;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocM22_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="m22Update"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocM22>> PutDocM22(string apiKey, int id, M22Update m22Update)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocM22 = await _context.DocM22.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocM22 == null)
            {
                return NotFound();
            }

            if (m22Update == null)
            {
                return BadRequest();
            }

            DocM22.IdDeclaracao = m22Update.IdDeclaracao;
            DocM22.Ano = m22Update.Ano;
            DocM22.IdentDocumento = m22Update.IdentDocumento;
            DocM22.IdentificacaoFiscal = m22Update.IdentificacaoFiscal;
            DocM22.ImportanciaPagar = m22Update.ImportanciaPagar;
            DocM22.RefPagamento = m22Update.RefPagamento;

            _context.Entry(DocM22).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDocM22", new { id = DocM22.Id }, DocM22);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocM22Exists(id))
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
        ///     POST /api/DocM22_API/{apiKey}
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
        public async Task<ActionResult<DocM22>> PostDocM22(string apiKey, IFormFile file)
        {

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocM22 DocM22 = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocM22.LocalFicheiro = fileName;
            DocM22 = Processador.ProcessarM22(StorageUtils.getFile(fileName), DocM22);
            DocM22.IdEmpresa = empresa.Id;

            _context.DocM22.Add(DocM22);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDocM22", new { id = DocM22.Id }, DocM22);
        }

        /// <summary>
        /// Deletes a specific M22.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocM22_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocM22(string apiKey, int id)
        {


            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocM22 = await _context.DocM22.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);

            if (DocM22 == null)
            {
                return NotFound();
            }
            _context.DocM22.Remove(DocM22);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocM22Exists(int id)
        {
            return _context.DocM22.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of M22
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocM22_API/{apiKey}/Ficheiro/{id}
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
            var DocM22 = await _context.DocM22.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocM22 == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocM22.LocalFicheiro), "application/pdf", DocM22.LocalFicheiro);
        }

        /// <summary>
        /// Get File of M22
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocM22_API/{apiKey}/Comprovativo/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{IdEmpresa:int}/Comprovativo/{id:int}")]
        public async Task<ActionResult> DownloadComprovativo(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocM22 = await _context.DocM22.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocM22 == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocM22.LocalComprovativo), "application/pdf", DocM22.LocalComprovativo);
        }

    }
}
