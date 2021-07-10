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
using System.Threading;
using Ciclope.Models.UpdateClasses;
using Microsoft.AspNetCore.Cors;

namespace Ciclope.Controllers
{
    /// <summary>
    /// Classes Responsável pelo api referente a divida a autoridade tributária
    /// </summary>
    [Route("api/Cashflow")]
    [ApiController]
    public class CashflowControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicialização da classe
        /// </summary>
        /// <param name="context"></param>
        public CashflowControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all data from files
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/FluxoCaixa/{apiKey}
        ///     {
        ///        "apiKey": 31A385D3-5D25-44E8-88F3-839DB1948EB0
        ///     }
        ///
        /// </remarks>
        /// <param name="apiKey">Company Api Key</param>
        /// <returns></returns>
        /// <response code="404">If the file was not found</response>   
        [HttpGet("{apiKey}")]
        public async Task<ActionResult<IEnumerable<CashFlow>>> GetCashflow(string apiKey, string startDate, string endDate)
        {
            DateTime? startDate_ = null;

            if (startDate != null)
            {
                startDate_ = DateTime.Parse(startDate);
            }

            DateTime? endDate_ = null;

            if(endDate != null)
            {
                endDate_ = DateTime.Parse(endDate);
            }


            if (startDate_ != null && endDate != null && startDate_ > endDate_)
            {
                return BadRequest();
            }

            var empresa = await _context.Empresa.FirstOrDefaultAsync(e => e.ApiKey == apiKey);
            
            if (empresa == null)
            {
                return NotFound();
            }

            return await _context.CashFlow.Where(e => e.IdEmpresa == empresa.Id && (e.Data >= startDate_ || startDate_ == null ) && (e.Data <= endDate_ || endDate_ == null)).OrderBy(e=> e.Data).ToListAsync();
        }

    }
}
