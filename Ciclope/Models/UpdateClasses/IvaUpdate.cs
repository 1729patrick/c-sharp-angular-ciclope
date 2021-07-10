using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API a Iva
    /// </summary>
    public class IvaUpdate
    {
        /// <summary>
        /// Nome da Entidade
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Morada da Entidade
        /// </summary>
        public string Morada { get; set; }

        /// <summary>
        /// Localidade
        /// </summary>
        public string Localidade { get; set; }

        /// <summary>
        /// Codigo postal
        /// </summary>
        public string CodigoPostal { get; set; }
        /// <summary>
        /// Periodo
        /// </summary>
        public string Periodo { get; set; }
        /// <summary>
        /// Identificação do Documento pela autoridade que o emitiu
        /// </summary>
        public int IdDeclaracao { get; set; }

        /// <summary>
        /// Data de recepção da Daclaracao
        /// </summary>
        public DateTime DataRececaoDeclaracao { get; set; }

        /// <summary>
        /// Referencia de pagamento
        /// </summary>
        public int Referencia { get; set; }

        /// <summary>
        /// Linha optica
        /// </summary>
        public int LinhaOptica { get; set; }
        /// <summary>
        /// Valor do pagamento
        /// </summary>
        public double Valor { get; set; }
    }
}
