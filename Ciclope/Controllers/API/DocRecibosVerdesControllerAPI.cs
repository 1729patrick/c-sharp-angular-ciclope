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
    /// Classes responsável pelo api referente ao ficheiro Recibos Verdes
    /// </summary>
    [Route("api/DocRecibosVerdes")]
    [ApiController]
    public class DocRecibosVerdesControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocRecibosVerdesControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRecibosVerdes_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocRecibosVerdes>>> GetDocRecibosVerdes(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocRecibosVerdes.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocRecibosVerdes_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocRecibosVerdes>> GetDocRecibosVerdes(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRecibosVerdes = await _context.DocRecibosVerdes.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRecibosVerdes == null)
            {
                return NotFound();
            }

            return DocRecibosVerdes;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocRecibosVerdes_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="recibosUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocRecibosVerdes>> PutDocRecibosVerdes(string apiKey, int id, RecibosUpdate recibosUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRecibosVerdes = await _context.DocRecibosVerdes.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRecibosVerdes == null)
            {
                return NotFound();
            }

            if (recibosUpdate == null)
            {
                return BadRequest();
            }

            DocRecibosVerdes.TransmitenteNome = recibosUpdate.TransmitenteNome;
            DocRecibosVerdes.TransmitenteAtividade = recibosUpdate.TransmitenteAtividade;
            DocRecibosVerdes.TransmitenteNif = recibosUpdate.TransmitenteNif;
            DocRecibosVerdes.TransmitenteDomicilio = recibosUpdate.TransmitenteDomicilio;
            DocRecibosVerdes.AdquirenteNome = recibosUpdate.AdquirenteNome;
            DocRecibosVerdes.AdquirenteMorada = recibosUpdate.AdquirenteMorada;
            DocRecibosVerdes.AdquirenteNif = recibosUpdate.AdquirenteNif;
            DocRecibosVerdes.DadosDataTransmissao = recibosUpdate.DadosDataTransmissao;
            DocRecibosVerdes.DadosDescricao = recibosUpdate.DadosDescricao;
            DocRecibosVerdes.DadosValorBase = recibosUpdate.DadosValorBase;
            DocRecibosVerdes.DadosIva = recibosUpdate.DadosIva;
            DocRecibosVerdes.DadosImpostoSelo = recibosUpdate.DadosImpostoSelo;
            DocRecibosVerdes.DadosIRS = recibosUpdate.DadosIRS;


            _context.Entry(DocRecibosVerdes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDocRecibosVerdes", new { id = DocRecibosVerdes.Id }, DocRecibosVerdes);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocRecibosVerdesExists(id))
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
        ///     POST /api/DocRecibosVerdes_API/{apiKey}
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
        public async Task<ActionResult<DocRecibosVerdes>> PostDocRecibosVerdes(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocRecibosVerdes DocRecibosVerdes = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocRecibosVerdes.LocalFicheiro = fileName;
            DocRecibosVerdes = Processador.ProcessarRecibosVerdes(StorageUtils.getFile(fileName), DocRecibosVerdes);
            DocRecibosVerdes.IdEmpresa = empresa.Id;

            _context.DocRecibosVerdes.Add(DocRecibosVerdes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocRecibosVerdes", new { id = DocRecibosVerdes.Id }, DocRecibosVerdes);
        }

        /// <summary>
        /// Deletes a specific Recibo Verde.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocRecibosVerdes_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocRecibosVerdes(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRecibosVerdes = await _context.DocRecibosVerdes.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRecibosVerdes == null)
            {
                return NotFound();
            }

            _context.DocRecibosVerdes.Remove(DocRecibosVerdes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocRecibosVerdesExists(int id)
        {
            return _context.DocRecibosVerdes.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of Recibos Verdes
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRecibosVerdes_API/{apiKey}/Ficheiro/{id}
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
            var DocRecibosVerdes = await _context.DocRecibosVerdes.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRecibosVerdes == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRecibosVerdes.LocalFicheiro), "application/pdf", DocRecibosVerdes.LocalFicheiro);
        }
    }
}
