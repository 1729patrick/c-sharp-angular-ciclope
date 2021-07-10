using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API a certificado PME
    /// </summary>
    public class CertificadoPMEUpdate
    {
        /// <summary>
        /// Identificação do Documento pela autoridade que o emitiu
        /// </summary>
        public int IdClassificacao { get; set; }
        /// <summary>
        /// Data da Classificacao
        /// </summary>
        public DateTime DataDecissao { get; set; }
        /// <summary>
        /// Data de Efeito de Decissao
        /// </summary>
        public DateTime DataEfeito { get; set; }
    }
}
