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
    /// Classes responsável pelo api referente ao ficheiro Certificado PME
    /// </summary>
    [Route("api/DocCertificadoPME")]
    [ApiController]
    public class DocCertificadoPMEControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocCertificadoPMEControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocCertificadoPME_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocCertificadoPME>>> GetDocCertificadoPME(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocCertificadoPME.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocCertificadoPME_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Api Key For Company</param>
        /// <param name="id">File Id</param>
        [HttpGet("{apiKey}/{id}")]
        public async Task<ActionResult<DocCertificadoPME>> GetDocCertificadoPME(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocCertificadoPME = await _context.DocCertificadoPME.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);

            if (DocCertificadoPME == null)
            {
                return NotFound();
            }

            return DocCertificadoPME;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocCertificadoPME_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
        /// <param name="id">File Id</param>
        /// <param name="certificadoPME"></param>
        /// <returns></returns>
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocCertificadoPME>> PutDocCertificadoPME(string apiKey, int id, CertificadoPMEUpdate certificadoPME)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertificadoPME = await _context.DocCertificadoPME.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertificadoPME == null)
            {
                return NotFound();
            }

            if (certificadoPME == null)
            {
                return BadRequest();
            }

            DocCertificadoPME.IdClassificacao = certificadoPME.IdClassificacao;
            DocCertificadoPME.DataDecissao = certificadoPME.DataDecissao;
            DocCertificadoPME.DataEfeito = certificadoPME.DataEfeito;

            _context.Entry(DocCertificadoPME).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("PutDocCertificadoPME", new { id = DocCertificadoPME.Id }, DocCertificadoPME);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocCertificadoPMEExists(id))
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
        ///     POST /api/DocCertificadoPME_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
        /// <param name="file">Pdf File</param>
        /// <returns>Data from file</returns>
        [HttpPost("{apiKey}")]
        public async Task<ActionResult<DocCertificadoPME>> PostDocCertificadoPME(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocCertificadoPME DocCertificadoPME = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocCertificadoPME.LocalFicheiro = fileName;
            DocCertificadoPME = Processador.ProcessarPME(StorageUtils.getFile(fileName), DocCertificadoPME,_context);
            DocCertificadoPME.IdEmpresa = empresa.Id;


            _context.DocCertificadoPME.Add(DocCertificadoPME);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocCertificadoPME", new { id = DocCertificadoPME.Id }, DocCertificadoPME);
        }

        /// <summary>
        /// Deletes a specific Certificado PME.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocCertificadoPME_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
        /// <param name="id">File Id</param>
        /// <returns>Nothing</returns>
        /// <response code="204">If File Was Deleted</response>
        /// <response code="404">If the file was not found</response>    
        [HttpDelete("{apiKey}/{id}")]
        public async Task<IActionResult> DeleteDocCertificadoPME(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocCertificadoPME = await _context.DocCertificadoPME.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocCertificadoPME == null)
            {
                return NotFound();
            }

            _context.DocCertificadoPME.Remove(DocCertificadoPME);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocCertificadoPMEExists(int id)
        {
            return _context.DocCertificadoPME.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of Certificado PME
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocCertificadoPME_API/{apiKey}/Ficheiro/{id}
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

            var DocCertificadoPME = await _context.DocCertificadoPME.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocCertificadoPME == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocCertificadoPME.LocalFicheiro), "application/pdf", DocCertificadoPME.LocalFicheiro);
        }
    }
}
