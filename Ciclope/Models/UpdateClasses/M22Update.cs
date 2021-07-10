using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API ao M22
    /// </summary>
    public class M22Update
    {
        /// <summary>
        /// Identificacao do documento com a entidade que o emitiu
        /// </summary>
        public int IdDeclaracao { get; set; }
        /// <summary>
        /// Ano a que o ficheiro se refere
        /// </summary>
        public int Ano { get; set; }
        /// <summary>
        /// Identificacao do documento com a entidade que o emitiu
        /// </summary>
        public int IdentDocumento { get; set; }
        /// <summary>
        /// Identificacao fiscal
        /// </summary>
        public int IdentificacaoFiscal { get; set; }
        /// <summary>
        /// Valor a pagar
        /// </summary>
        public float ImportanciaPagar { get; set; }
        /// <summary>
        /// Referencia de pagamento
        /// </summary>
        public string RefPagamento { get; set; }
    }
}
