using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API ao IES
    /// </summary>
    public class IMIUpdate
    {
        /// <summary>
        /// Identificação Fiscal
        /// </summary>
        public int IdentificacaoFiscal { get; set; }
        /// <summary>
        /// Ano de Imposto
        /// </summary>
        public int AnoImposto { get; set; }
        /// <summary>
        /// Identificacao do documento com a entidade que o emitiu
        /// </summary>
        public int IdentificacaoDocumento { get; set; }
        /// <summary>
        /// Data de Liquidacao
        /// </summary>
        public DateTime DataLiquidacao { get; set; }
        /// <summary>
        /// Referencia de Pagamento
        /// </summary>
        public string ReferenciaPagamento { get; set; }
        /// <summary>
        /// Valor a pagar
        /// </summary>
        public double ImportanciaPagar { get; set; }
        /// <summary>
        /// Ano de pagamento
        /// </summary>
        public int AnoPagamento { get; set; }
        /// <summary>
        /// Mes de pagamento
        /// </summary>
        public int MesPagamento { get; set; }

    }
}
