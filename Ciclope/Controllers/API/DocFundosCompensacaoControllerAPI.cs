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
    /// Classes responsável pelo api referente aos Fundos de Compensação
    /// </summary>
    [Route("api/DocFundosCompensacao")]
    [ApiController]
    public class DocFundosCompensacaoControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocFundosCompensacaoControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocFundosCompensacao_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocFundosCompensacao>>> GetDocFundosCompensacao(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocFundosCompensacao.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocFundosCompensacao_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocFundosCompensacao>> GetDocFundosCompensacao(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocFundosCompensacao = await _context.DocFundosCompensacao.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocFundosCompensacao == null)
            {
                return NotFound();
            }

            return DocFundosCompensacao;
        }

        /// <summary>
        /// Update data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/DocFundosCompensacao_API/{apiKey}/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id":1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <param name="dRIUpdate"></param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpPut("{apiKey}/{id}")]
        public async Task<ActionResult<DocFundosCompensacao>> PutDocFundosCompensacao(string apiKey, int id, FundosUpdate fundosUpdate)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocFundosCompensacao = await _context.DocFundosCompensacao.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocFundosCompensacao == null)
            {
                return NotFound();
            }

            if (fundosUpdate == null)
            {
                return BadRequest();
            }

            DocFundosCompensacao.DataEmissao = fundosUpdate.DataEmissao;
            DocFundosCompensacao.PeriodoPagamentoInicio = fundosUpdate.PeriodoPagamentoInicio;
            DocFundosCompensacao.PeriodoPagamentoFim = fundosUpdate.PeriodoPagamentoFim;
            DocFundosCompensacao.Nome = fundosUpdate.Nome;
            DocFundosCompensacao.Niss = fundosUpdate.Niss;
            DocFundosCompensacao.Valor = fundosUpdate.Valor;
            DocFundosCompensacao.Entidade = fundosUpdate.Entidade;
            DocFundosCompensacao.Referencia = fundosUpdate.Referencia;

            _context.Entry(DocFundosCompensacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("PutDocFundosCompensacao", new { id = DocFundosCompensacao.Id }, DocFundosCompensacao);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocFundosCompensacaoExists(id))
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
        ///     POST /api/DocFundosCompensacao_API/{apiKey}
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
        public async Task<ActionResult<DocFundosCompensacao>> PostDocFundosCompensacao(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }


            DocFundosCompensacao DocFundosCompensacao = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocFundosCompensacao.LocalFicheiro = fileName;
            DocFundosCompensacao = Processador.ProcessarFundos(StorageUtils.getFile(fileName), DocFundosCompensacao);
            DocFundosCompensacao.IdEmpresa = empresa.Id;

            _context.DocFundosCompensacao.Add(DocFundosCompensacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocFundosCompensacao", new { id = DocFundosCompensacao.Id }, DocFundosCompensacao);
        }

        /// <summary>
        /// Deletes a specific Fundos Compensacao.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocFundosCompensacao_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocFundosCompensacao(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocFundosCompensacao = await _context.DocFundosCompensacao.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocFundosCompensacao == null)
            {
                return NotFound();
            }

            _context.DocFundosCompensacao.Remove(DocFundosCompensacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocFundosCompensacaoExists(int id)
        {
            return _context.DocFundosCompensacao.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get File of Fundos de Compensacao
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocFundosCompensacao_API/{apiKey}/Ficheiro/{id}
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
            var DocFundosCompensacao = await _context.DocFundosCompensacao.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocFundosCompensacao == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocFundosCompensacao.LocalFicheiro), "application/pdf", DocFundosCompensacao.LocalFicheiro);
        }
    }
}
