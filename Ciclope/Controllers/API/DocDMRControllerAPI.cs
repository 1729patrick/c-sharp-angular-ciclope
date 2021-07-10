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
    /// Classes responsável pelo api referente ao ficheiro DMR
    /// </summary>
    [Route("api/DocDMR")]
    [ApiController]
    public class DocDMRControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocDMRControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocDMR_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocDMR>>> GetDocDMR(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocDMR.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocDMR_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Api Key For Company</param>
        /// <param name="id">File Id</param>
        [HttpGet("{apiKey}/{id}")]
        public async Task<ActionResult<DocDMR>> GetDocDMR(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocDMR = await _context.DocDMR.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocDMR == null)
            {
                return NotFound();
            }

            return DocDMR;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocDMR_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Ke</param>
        /// <param name="id">File Id</param>
        /// <param name="dmrUpdate"></param>
        /// <returns></returns>
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocDMR>> PutDocDMR(string apiKey, int id, DmrUpdate dmrUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocDMR = await _context.DocDMR.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocDMR == null)
            {
                return NotFound();
            }

            if (dmrUpdate == null)
            {
                return BadRequest();
            }
            DocDMR.Nome = dmrUpdate.Nome;
            DocDMR.Morada = dmrUpdate.Morada;
            DocDMR.Localidade = dmrUpdate.Localidade;
            DocDMR.CodigoPostal = dmrUpdate.CodigoPostal;
            DocDMR.Periodo = dmrUpdate.Periodo;
            DocDMR.IdDeclaracao = dmrUpdate.IdDeclaracao;
            DocDMR.DataRececaoDeclaracao = dmrUpdate.DataRececaoDeclaracao;
            DocDMR.ReferenciaPagamento = dmrUpdate.ReferenciaPagamento;
            DocDMR.LinhaOptica = dmrUpdate.LinhaOptica;
            DocDMR.ImportanciaPagar = dmrUpdate.ImportanciaPagar;
            _context.Entry(DocDMR).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("PutDocDMR", new { id = DocDMR.Id }, DocDMR);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocDMRExists(id))
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
        ///     POST /api/DocDMR_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="file">Pdf File</param>
        /// <returns>Data from file</returns>
        [HttpPost("{apiKey}")]
        public async Task<ActionResult<DocDMR>> PostDocDMR(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            DocDMR DocDMR = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocDMR.LocalFicheiro = fileName;
            DocDMR = Processador.ProcessarDMR(StorageUtils.getFile(fileName), DocDMR);
            DocDMR.IdEmpresa = empresa.Id;
            _context.DocDMR.Add(DocDMR);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocDMR", new { id = DocDMR.Id }, DocDMR);
        }

        /// <summary>
        /// Deletes a specific Dmr.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocDMR_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocDMR(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocDMR = await _context.DocDMR.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocDMR == null)
            {
                return NotFound();
            }

            _context.DocDMR.Remove(DocDMR);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocDMRExists(int id)
        {
            return _context.DocDMR.Any(e => e.Id == id);
        }


        /// <summary>
        /// Get File of DMR
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocDMR_API/{apiKey}/Ficheiro/{id}
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
            var DocDMR = await _context.DocDMR.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocDMR == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocDMR.LocalFicheiro), "application/pdf", DocDMR.LocalFicheiro);
        }

        /// <summary>
        /// Get Declaracao of DMR
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocDMR_API/{apiKey}/Declaracao/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/Declaracao/{id:int}")]
        public async Task<ActionResult> DownloadDec(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocDMR = await _context.DocDMR.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocDMR == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocDMR.LocalDeclaracao), "application/pdf", DocDMR.LocalDeclaracao);
        }

    }
}
