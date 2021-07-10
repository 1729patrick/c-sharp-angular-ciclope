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
    /// Classes responsável pelo api referente ao ficheiro IES
    /// </summary>
    [Route("api/DocIES")]
    [ApiController]
    public class DocIESControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocIESControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocIES_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocIES>>> GetDocIES(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocIES.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocIES_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocIES>> GetDocIES(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIES = await _context.DocIES.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocIES == null)
            {
                return NotFound();
            }

            return DocIES;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocIES_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="iesUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocIES>> PutDocIES(string apiKey, int id, IesUpdate iesUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIES = await _context.DocIES.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocIES == null)
            {
                return NotFound();
            }

            if (DocIES == null)
            {
                return BadRequest();
            }

            DocIES.DataValidade = iesUpdate.DataValidade;
            DocIES.Nif = iesUpdate.Nif;
            DocIES.Nome = iesUpdate.Nome;
            DocIES.Valor = iesUpdate.Valor;
            DocIES.Referencia = iesUpdate.Referencia;
            DocIES.Entidade = iesUpdate.Entidade;

            _context.Entry(DocIES).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("PutDocIES", new { id = DocIES.Id }, DocIES);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocIESExists(id))
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
        ///     POST /api/DocIES_API/{apiKey}
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
        public async Task<ActionResult<DocIES>> PostDocIES(string apiKey, IFormFile file)
        {

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocIES DocIES = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocIES.LocalFicheiro = fileName;
            DocIES = Processador.ProcessarIES(StorageUtils.getFile(fileName), DocIES);
            DocIES.IdEmpresa = empresa.Id;

            _context.DocIES.Add(DocIES);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDocIES", new { id = DocIES.Id }, DocIES);
        }

        /// <summary>
        /// Deletes a specific IES.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocIES_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocIES(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIES = await _context.DocIES.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                           && m.Id == id);

            if (DocIES == null)
            {
                return NotFound();
            }

            _context.DocIES.Remove(DocIES);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocIESExists(int id)
        {
            return _context.DocIES.Any(e => e.Id == id);
        }



        /// <summary>
        /// Get File of IES
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocIES_API/{apiKey}/Ficheiro/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
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

            var DocIES = await _context.DocIES.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocIES == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocIES.LocalFicheiro), "application/pdf", DocIES.LocalFicheiro);
        }

        /// <summary>
        /// Get File of IES
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocIES_API/{apiKey}/Ficheiro/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{IdEmpresa:int}/GuiaDePagamento/{id:int}")]
        public async Task<ActionResult> DownloadGuiaDePagamento(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocIES = await _context.DocIES.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocIES == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocIES.LocalPagamento), "application/pdf", DocIES.LocalPagamento);
        }

        /// <summary>
        /// Get File of IES
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocIES_API/{apiKey}/Comprovativo/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
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


            var DocIES = await _context.DocIES.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocIES == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocIES.LocalComprovativo), "application/pdf", DocIES.LocalComprovativo);
        }
    }
}
