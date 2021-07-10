using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API a DRI
    /// </summary>
    public class DRIUpdate
    {
        /// <summary>
        /// Data Entrega da Dri
        /// </summary>
        public DateTime DataEntrega { get; set; }
        /// <summary>
        /// Data Registo da Dri
        /// </summary>
        public DateTime DataRegisto { get; set; }
        /// <summary>
        /// Identificacao do ficheiro pela entidade que o emitiu
        /// </summary>
        public string Identificador { get; set; }
        /// <summary>
        /// Estado do Dri
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Numero de identificação social
        /// </summary>
        public string NIdentificacaoSS { get; set; }
        /// <summary>
        /// Nome da Empresa
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Total Remunerações
        /// </summary>
        public double TotalRemuneracoes { get; set; }
        /// <summary>
        /// Total Contribuições
        /// </summary>
        public double TotalContribuicoes { get; set; }
    }
}
