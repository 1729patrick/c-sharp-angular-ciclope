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
using System.Net.Http;
using Ciclope.Models.NewFolder;

namespace Ciclope.Controllers
{
    /// <summary>
    /// Classes responsável pelo api referente ao ficheiro Certidao AT
    /// </summary>
    [Route("api/DocCertidaoAT")]
    [ApiController]
    public class DocCertidaoATControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocCertidaoATControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocCertidaoAT_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocCertidaoAT>>> GetDocCertidaoAT(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocCertidaoAT.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocCertidaoAT_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Api Key For Company</param>
        /// <param name="id">File Id</param>
        [HttpGet("{apiKey}/{id}")]
        public async Task<ActionResult<DocCertidaoAT>> GetDocCertidaoAT(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertidaoAT = await _context.DocCertidaoAT.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertidaoAT == null)
            {
                return NotFound();
            }

            return DocCertidaoAT;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocCertidaoAT_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="certidaoAT"></param>
        /// <returns></returns>
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocCertidaoAT>> PutDocCertidaoAT(string apiKey, int id, CertidaoATUpdate certidaoAT)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertidaoATAtual = await _context.DocCertidaoAT.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertidaoATAtual == null)
            {
                return NotFound();
            }

            if (certidaoAT == null)
            {
                return BadRequest();
            }

            DocCertidaoATAtual.CodigoValidacao = certidaoAT.CodigoValidacao;
            DocCertidaoATAtual.DataValidade = certidaoAT.DataValidade;

            _context.Entry(DocCertidaoATAtual).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("PutDocCertidaoAT", new { id = DocCertidaoATAtual.Id }, DocCertidaoATAtual);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocCertidaoATExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/DocCertidaoAT_API/1/5
        /// <summary>
        /// Insert new Pdf File
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocCertidaoAT_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="file">Pdf File</param>
        /// <returns>Data from file</returns>
        [HttpPost("{apiKey}")]
        public async Task<ActionResult<DocCertidaoAT>> PostDocCertidaoAT(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocCertidaoAT DocCertidaoAT = new();
            string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocCertidaoAT.LocalFicheiro = fileName;
            DocCertidaoAT = Processador.ProcessarCertidaoAT(StorageUtils.getFile(fileName), DocCertidaoAT);
            DocCertidaoAT.IdEmpresa = empresa.Id;

            _context.Add(DocCertidaoAT);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDocCertidaoAT", new { id = DocCertidaoAT.Id }, DocCertidaoAT);
        }

        // DELETE: api/DocCertidaoAT_API/1/5
        /// <summary>
        /// Deletes a specific CertidaoAT.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocCertidaoAT_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocCertidaoAT(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertidaoAT = await _context.DocCertidaoAT.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertidaoAT == null)
            {
                return NotFound();
            }

            _context.DocCertidaoAT.Remove(DocCertidaoAT);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocCertidaoATExists(int id)
        {
            return _context.DocCertidaoAT.Any(e => e.Id == id);
        }

        // GET: api/DocCertidaoAT_API/1/Ficheiro/5
        /// <summary>
        /// Get File of CertidaoAT
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocCertidaoAT_API/{apiKey}/Ficheiro/{id}
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
        public async Task<ActionResult> DownloadFile(string apiKey, int id)
        {

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocCertidaoAT = await _context.DocCertidaoAT.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocCertidaoAT == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocCertidaoAT.LocalFicheiro), "application/pdf", DocCertidaoAT.LocalFicheiro);
        }



    }
}
