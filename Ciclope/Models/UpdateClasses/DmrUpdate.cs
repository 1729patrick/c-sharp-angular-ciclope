using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API a dmr
    /// </summary>
    public class DmrUpdate
    {
        /// <summary>
        /// Nome da Empresa
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Morada da Empresa
        /// </summary>
        public string Morada { get; set; }

        /// <summary>
        /// Localidade da Empresa
        /// </summary>
        public string Localidade { get; set; }

        /// <summary>
        /// Codigo postal da Empresa
        /// </summary>
        public string CodigoPostal { get; set; }
        /// <summary>
        /// Periodo
        /// </summary>
        public string Periodo { get; set; }
        /// <summary>
        /// Identificacao do ficheiro pela entidade que o emitiu
        /// </summary>
        public int IdDeclaracao { get; set; }
        /// <summary>
        /// Data da Recepção da Declaração
        /// </summary>
        public DateTime DataRececaoDeclaracao { get; set; }
        /// <summary>
        /// Referencia de Pagamento 
        /// </summary>
        public string ReferenciaPagamento { get; set; }
        /// <summary>
        /// Linha Optica
        /// </summary>
        public string LinhaOptica { get; set; }
        /// <summary>
        /// Valor a pagar
        /// </summary>
        public double ImportanciaPagar { get; set; }
    }
}
