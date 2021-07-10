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
    /// Classes responsável pelo api referente ao ficheiro Certidao SS
    /// </summary>
    [Route("api/DocCertidaoSS")]
    [ApiController]
    public class DocCertidaoSSControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocCertidaoSSControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocCertidaoSS_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocCertidaoSS>>> GetDocCertidaoSS(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocCertidaoSS.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }


        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocCertidaoSS_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        [HttpGet("{apiKey}/{id}")]
        public async Task<ActionResult<DocCertidaoSS>> GetDocCertidaoSS(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertidaoSS = await _context.DocCertidaoSS.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertidaoSS == null)
            {
                return NotFound();
            }

            return DocCertidaoSS;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocCertidaoSS_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
        /// <param name="id">File Id</param>
        /// <param name="certidaoSS"></param>
        /// <returns></returns>
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocCertidaoSS>> PutDocCertidaoSS(string apiKey, int id, CertidaoSSUpdate certidaoSS)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertidaoSS = await _context.DocCertidaoSS.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertidaoSS == null)
            {
                return NotFound();
            }

            if (certidaoSS == null)
            {
                return BadRequest();
            }

            DocCertidaoSS.DataEmissao = certidaoSS.DataEmissao;
            DocCertidaoSS.DataFimValidade = certidaoSS.DataFimValidade;

            _context.Entry(DocCertidaoSS).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("PutDocCertidaoSS", new { id = DocCertidaoSS.Id }, DocCertidaoSS);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocCertidaoSSExists(id))
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
        ///     POST /api/DocCertidaoSS_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="file">Pdf File</param>
        /// <returns>Data from file</returns>
        [HttpPost("{apiKey}")]
        public async Task<ActionResult<DocCertidaoSS>> PostDocCertidaoSS(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            DocCertidaoSS DocCertidaoSS = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocCertidaoSS.LocalFicheiro = fileName;
            DocCertidaoSS = Processador.ProcessarCertidaoSS(StorageUtils.getFile(fileName), DocCertidaoSS);
            DocCertidaoSS.IdEmpresa = empresa.Id;

            _context.Add(DocCertidaoSS);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDocCertidaoSS", new { id = DocCertidaoSS.Id }, DocCertidaoSS);
        }


        /// <summary>
        /// Deletes a specific CertidaoSS.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocCertidaoSS_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocCertidaoSS(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertidaoSS = await _context.DocCertidaoSS.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertidaoSS == null)
            {
                return NotFound();
            }

            _context.DocCertidaoSS.Remove(DocCertidaoSS);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocCertidaoSSExists(int id)
        {
            return _context.DocCertidaoSS.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of CertidaoSS
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocCertidaoSS_API/{apiKey}/Ficheiro/{id}
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
        public async Task<ActionResult> DownloadFile(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocCertidaoSS = await _context.DocCertidaoSS.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocCertidaoSS == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocCertidaoSS.LocalFicheiro), "application/pdf", DocCertidaoSS.LocalFicheiro);
        }
    }
}
