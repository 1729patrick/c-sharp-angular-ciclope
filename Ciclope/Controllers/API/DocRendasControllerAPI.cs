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
    /// Classes responsável pelo api referente ao ficheiro Dri
    /// </summary>
    [Route("api/DocRendas")]
    [ApiController]
    public class DocRendasControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocRendasControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRendas_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocRendas>>> GetDocRendas(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocRendas.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocRendas_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocRendas>> GetDocRendas(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRendas = await _context.DocRendas.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRendas == null)
            {
                return NotFound();
            }

            return DocRendas;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocRendas_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="rendaUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocRendas>> PutDocRendas(string apiKey, int id,RendaUpdate rendaUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRendas = await _context.DocRendas.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRendas == null)
            {
                return NotFound();
            }

            if (rendaUpdate == null)
            {
                return BadRequest();
            }

            DocRendas.EntidadeNif = rendaUpdate.EntidadeNif;
            DocRendas.EntidadeNome = rendaUpdate.EntidadeNome;
            DocRendas.LocatorioNome = rendaUpdate.LocatorioNome;
            DocRendas.LocatorioNif = rendaUpdate.LocatorioNif;
            DocRendas.Arrendamento = rendaUpdate.Arrendamento;
            DocRendas.Localizacao = rendaUpdate.Localizacao;
            DocRendas.PeriodoRendaInicio = rendaUpdate.PeriodoRendaInicio;
            DocRendas.PeriodoRendaFim = rendaUpdate.PeriodoRendaFim;
            DocRendas.Titulo = rendaUpdate.Titulo;
            DocRendas.DataRecebimento = rendaUpdate.DataRecebimento;
            DocRendas.RetencaoIRS = rendaUpdate.RetencaoIRS;
            DocRendas.ImportanciaRecebida = rendaUpdate.RetencaoIRS;
            DocRendas.NRecibosVenda = rendaUpdate.NRecibosVenda;
            DocRendas.DataEmissao = rendaUpdate.DataEmissao;
            _context.Entry(DocRendas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDocRendas", new { id = DocRendas.Id }, DocRendas);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocRendasExists(id))
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
        ///     POST /api/DocRendas_API/{apiKey}
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
        public async Task<ActionResult<DocRendas>> PostDocRendas(string apiKey, IFormFile file)
        {

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocRendas DocRendas = new();
            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocRendas.LocalFicheiro = fileName;
            DocRendas = Processador.ProcessarRenda(StorageUtils.getFile(fileName), DocRendas);
            DocRendas.IdEmpresa = empresa.Id;
            _context.DocRendas.Add(DocRendas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocRendas", new { id = DocRendas.Id }, DocRendas);
        }

        /// <summary>
        /// Deletes a specific Renda.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocRendas_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocRendas(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRendas = await _context.DocRendas.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRendas == null)
            {
                return NotFound();
            }
            _context.DocRendas.Remove(DocRendas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocRendasExists(int id)
        {
            return _context.DocRendas.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of DRI
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRendas_API/{apiKey}/Ficheiro/{id}
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
        public async Task<ActionResult> DownloadCertificado(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            var DocRendas = await _context.DocRendas.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRendas == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRendas.LocalFicheiro), "application/pdf", DocRendas.LocalFicheiro);
        }
    }
}
