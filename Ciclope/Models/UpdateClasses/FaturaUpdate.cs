using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API a Iva
    /// </summary>
    public class FaturaUpdate
    {
        /// <summary>
        /// Numero da Fatura
        /// </summary>
        public string Numero { get; set; }
        /// <summary>
        /// Descricao da Fatura
        /// </summary>
        public string Descricao { get; set; }
        /// <summary>
        /// Valor da Fatura
        /// </summary>
        public double Valor { get; set; }
        /// <summary>
        /// Entidade 
        /// </summary>
        public string Entidade { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// Valor Iva
        /// </summary>
        public double ValorIva { get; set; }
    }
}
