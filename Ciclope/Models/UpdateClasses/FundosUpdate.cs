using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API aos fundos compensação
    /// </summary>
    public class FundosUpdate
    {
        /// <summary>
        /// Data Emissao do fundo
        /// </summary>
        public DateTime DataEmissao { get; set; }
        /// <summary>
        /// Periodo Pagamento Inicio
        /// </summary>
        public DateTime PeriodoPagamentoInicio { get; set; }
        /// <summary>
        /// Periodo Pagamento Fim
        /// </summary>
        public DateTime PeriodoPagamentoFim { get; set; }
        /// <summary>
        /// Nome da Empresa
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Numero de identificação Segurança Social
        /// </summary>
        public string Niss { get; set; }
        /// <summary>
        /// Valor a pagar
        /// </summary>
        public double Valor { get; set; }
        /// <summary>
        /// Entidade do pagamento
        /// </summary>
        public string Entidade { get; set; }
        /// <summary>
        /// Referencia do pagamento
        /// </summary>
        public string Referencia { get; set; }
    }
}
