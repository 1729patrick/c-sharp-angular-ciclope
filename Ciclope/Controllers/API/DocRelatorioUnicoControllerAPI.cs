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

namespace Ciclope.Controllers
{
    /// <summary>
    /// Classes responsável pelo api referente ao ficheiro Relatorio Unico
    /// </summary>
    [Route("api/DocRelatorioUnico")]
    [ApiController]
    public class DocRelatorioUnicoControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public DocRelatorioUnicoControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response> 
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<DocRelatorioUnico>>> GetDocRelatorioUnico(string apiKey)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }
            return await _context.DocRelatorioUnico.Where(c => c.IdEmpresa == empresa.Id).ToListAsync();
        }

        /// <summary>
        /// Get data from a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocRelatorioUnico_API/{apiKey}/{id}
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
        public async Task<ActionResult<DocRelatorioUnico>> GetDocRelatorioUnico(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }

            return DocRelatorioUnico;
        }



        /// <summary>
        /// Insert new Pdf File
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/DocRelatorioUnico_API/{apiKey}
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
        public async Task<ActionResult<DocRelatorioUnico>> PostDocRelatorioUnico(string apiKey, IFormFile file)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            DocRelatorioUnico DocRelatorioUnico = new();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            Stream stream = file.OpenReadStream();
            StorageUtils.uploadFile(stream, fileName);
            DocRelatorioUnico.LocalAnexo0 = fileName;
            DocRelatorioUnico = Processador.ProcessarRU(StorageUtils.getFile(fileName), DocRelatorioUnico);
            DocRelatorioUnico.IdEmpresa = empresa.Id;

            _context.DocRelatorioUnico.Add(DocRelatorioUnico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocRelatorioUnico", new { id = DocRelatorioUnico.Id }, DocRelatorioUnico);
        }

        /// <summary>
        /// Deletes a specific RelatorioUnico.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/DocRelatorioUnico_API/{apiKey}/{id}
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
        public async Task<IActionResult> DeleteDocRelatorioUnico(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.Include(t => t.Empresa).FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                            && m.Id == id);

            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }

            _context.DocRelatorioUnico.Remove(DocRelatorioUnico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocRelatorioUnicoExists(int id)
        {
            return _context.DocRelatorioUnico.Any(e => e.Id == id);
        }
        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/LocalAnexo0/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/LocalAnexo0/{Id}")]
        public async Task<ActionResult> DownloadAnexo0(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexo0), "application/pdf", DocRelatorioUnico.LocalAnexo0);
        }

        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/LocalAnexoA/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/LocalAnexoA/{Id}")]
        public async Task<ActionResult> DownloadAnexoA(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoA), "application/pdf", DocRelatorioUnico.LocalAnexoA);
        }

        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/LocalAnexoB/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/LocalAnexoB/{Id}")]
        public async Task<ActionResult> DownloadAnexoB(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoB), "application/pdf", DocRelatorioUnico.LocalAnexoB);
        }

        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/LocalAnexoC/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/LocalAnexoC/{Id}")]
        public async Task<ActionResult> DownloadAnexoC(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoC), "application/pdf", DocRelatorioUnico.LocalAnexoC);
        }

        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/LocalAnexoD/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/LocalAnexoD/{Id}")]
        public async Task<ActionResult> DownloadAnexoD(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoD), "application/pdf", DocRelatorioUnico.LocalAnexoD);
        }

        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/LocalAnexoE/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey}/LocalAnexoE/{Id}")]
        public async Task<ActionResult> DownloadAnexoE(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoE), "application/pdf", DocRelatorioUnico.LocalAnexoE);
        }

        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/LocalAnexoF/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey:int}/LocalAnexoF/{id:int}")]
        public async Task<ActionResult> DownloadAnexoF(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalAnexoF), "application/pdf", DocRelatorioUnico.LocalAnexoF);
        }

        /// <summary>
        /// Get File of RU
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/DocRelatorioUnico_API/{apiKey}/Certificado/{id}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0,
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company API Key</param>
        /// <param name="id">File Id</param>
        /// <returns>PDF File</returns>
        [HttpGet("{apiKey:int}/Certificado/{id:int}")]
        public async Task<ActionResult> DownloadCertificado(string apiKey, int id)
        {
            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            if (empresa == null)
            {
                return NotFound();
            }

            var DocRelatorioUnico = await _context.DocRelatorioUnico.FirstOrDefaultAsync(m => m.IdEmpresa == empresa.Id
                && m.Id == id);
            if (DocRelatorioUnico == null)
            {
                return NotFound();
            }
            return File(StorageUtils.getFile(DocRelatorioUnico.LocalCertificado), "application/pdf", DocRelatorioUnico.LocalCertificado);
        }
    }
}
