using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API ao IES
    /// </summary>
    public class IesUpdate
    {
        /// <summary>
        /// Data de Validade
        /// </summary>
        public DateTime DataValidade { get; set; }
        /// <summary>
        /// Nif
        /// </summary>
        public string Nif { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Valor a pagar
        /// </summary>
        public double Valor { get; set; }
        /// <summary>
        /// Referencia de pagamento
        /// </summary>
        public string Referencia { get; set; }
        /// <summary>
        /// Entidade
        /// </summary>
        public string Entidade { get; set; }
    }
}
