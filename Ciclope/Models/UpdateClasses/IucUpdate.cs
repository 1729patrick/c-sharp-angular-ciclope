using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API ao IUC
    /// </summary>
    public class IucUpdate
    {
        /// <summary>
        /// Matricula do veiculo
        /// </summary>
        public string Matricula { get; set; }

        /// <summary>
        /// Ano de veiculo
        /// </summary>
        public int Ano { get; set; }

        /// <summary>
        /// Mes do veiculo
        /// </summary>
        public int Mes { get; set; }

        /// <summary>
        /// Identificacao do documento com a entidade que o emitiu
        /// </summary>
        public string IdentificacaoFicheiro { get; set; }

        /// <summary>
        /// Data de IUC
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Identificação fiscal
        /// </summary>
        public string IdentificacaoFiscal { get; set; }

        /// <summary>
        /// Morada
        /// </summary>
        public string Morada { get; set; }

        /// <summary>
        /// Data Limite de pagamento
        /// </summary>
        public DateTime DataLimite { get; set; }
        /// <summary>
        /// Referencia de pagamento
        /// </summary>
        public string ReferenciaPagamento { get; set; }

        /// <summary>
        /// Valor a pagar
        /// </summary>
        public double ImportanciaPagar { get; set; }
    }
}
