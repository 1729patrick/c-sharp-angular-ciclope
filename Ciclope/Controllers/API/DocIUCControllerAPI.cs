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
    /// Classes responsável pelo api referente ao ficheiro IUC
    /// </summary>
    [Route("api/DocIU")]
    [ApiController]
    public class DocIUCControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocIUCControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocIUC_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocIUC>>> GetDocIUC(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocIUC.Where(c => c.EmpresaId == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocIUC_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocIUC>> GetDocIUC(string apiKey, int id)
        {

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIUC = await _context.DocIUC.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                            && m.Id == id);

            if (DocIUC == null)
            {
                return NotFound();
            }

            return DocIUC;
        }


        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocIUC_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="iucUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocIUC>>  PutDocIUC(string apiKey, int id,IucUpdate iucUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIUC = await _context.DocIUC.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                            && m.Id == id);

            if (DocIUC == null)
            {
                return NotFound();
            }

            if (iucUpdate == null)
            {
                return BadRequest();
            }

            DocIUC.Matricula = iucUpdate.Matricula;
            DocIUC.Ano = iucUpdate.Ano;
            DocIUC.Mes = iucUpdate.Mes;
            DocIUC.IdentificacaoFicheiro = iucUpdate.IdentificacaoFicheiro;
            DocIUC.Data = iucUpdate.Data;
            DocIUC.IdentificacaoFiscal = iucUpdate.IdentificacaoFiscal;
            DocIUC.Morada = iucUpdate.Morada;
            DocIUC.DataLimite = iucUpdate.DataLimite;
            DocIUC.ReferenciaPagamento = iucUpdate.ReferenciaPagamento;
            DocIUC.ImportanciaPagar = iucUpdate.ImportanciaPagar;

            _context.Entry(DocIUC).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDocIUC", new { id = DocIUC.Id }, DocIUC);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocIUCExists(id))
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
        ///     POST /api/DocIUC_API/{apiKey}
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
        public async Task<ActionResult<DocIUC>> PostDocIUC(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }


            DocIUC DocIUC = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocIUC.LocalFicheiro = fileName;
            DocIUC = Processador.ProcessarIUC(StorageUtils.getFile(fileName), DocIUC);
            DocIUC.EmpresaId = empresa.Id;



            _context.DocIUC.Add(DocIUC);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocIUC", new { id = DocIUC.Id }, DocIUC);
        }

        /// <summary>
        /// Deletes a specific IUC.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocIUC_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocIUC(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIUC = await _context.DocIUC.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                            && m.Id == id);

            _context.DocIUC.Remove(DocIUC);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocIUCExists(int id)
        {
            return _context.DocIUC.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of IMI
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocIUC_API/{apiKey}/Ficheiro/{id}
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
            var DocIUC = await _context.DocIUC.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.EmpresaId == empresa.Id
                && m.Id == id);
            if (DocIUC == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocIUC.LocalFicheiro), "application/pdf", DocIUC.LocalFicheiro);
        }
    }
}
