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
    /// Classes responsável pelo api referente ao ficheiro IMI
    /// </summary>
    [Route("api/DocIMI")]
    [ApiController]
    public class DocIMIControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocIMIControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api//api/DocIMI_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocIMI>>> GetDocIMI(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocIMI.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocIMI_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocIMI>> GetDocIMI(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIMI = await _context.DocIMI.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocIMI == null)
            {
                return NotFound();
            }

            return DocIMI;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocIMI_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="iMIUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocIMI>> PutDocIMI(string apiKey, int id,IMIUpdate iMIUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIMI = await _context.DocIMI.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocIMI == null)
            {
                return NotFound();
            }

            if (iMIUpdate == null)
            {
                return BadRequest();
            }

            DocIMI.IdentificacaoFiscal = iMIUpdate.IdentificacaoFiscal;
            DocIMI.AnoImposto = iMIUpdate.AnoImposto;
            DocIMI.IdentificacaoDocumento = iMIUpdate.IdentificacaoDocumento;
            DocIMI.DataLiquidacao = iMIUpdate.DataLiquidacao;
            DocIMI.ReferenciaPagamento = iMIUpdate.ReferenciaPagamento;
            DocIMI.ImportanciaPagar = iMIUpdate.ImportanciaPagar;
            DocIMI.AnoPagamento = iMIUpdate.AnoPagamento;
            DocIMI.MesPagamento = iMIUpdate.MesPagamento;


            _context.Entry(DocIMI).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("PutDocIMI", new { id = DocIMI.Id }, DocIMI);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocIMIExists(id))
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
        ///     POST /api/DocIMI_API/{apiKey}
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
        public async Task<ActionResult<DocIMI>> PostDocIMI(string apiKey, IFormFile file)
        {

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocIMI DocIMI = new();
            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocIMI.LocalFicheiro = fileName;
            DocIMI = Processador.ProcessarIMI(StorageUtils.getFile(fileName), DocIMI);
            DocIMI.IdEmpresa = empresa.Id;
            _context.DocIMI.Add(DocIMI);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDocIMI", new { id = DocIMI.Id }, DocIMI);
        }

        /// <summary>
        /// Deletes a specific IMI.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocIMI_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocIMI(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIMI = await _context.DocIMI.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocIMI == null)
            {
                return NotFound();
            }

            _context.DocIMI.Remove(DocIMI);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocIMIExists(int id)
        {
            return _context.DocIMI.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of IMI
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocIMI_API/{apiKey}/Ficheiro/{id}
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
            var DocIMI = await _context.DocIMI.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocIMI == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocIMI.LocalFicheiro), "application/pdf", DocIMI.LocalFicheiro);
        }
    }
}
