using System;

namespace Ciclope.Models.NewFolder
{
    /// <summary>
    /// Classes usada para o update API a certidao AT
    /// </summary>
    public class CertidaoATUpdate
    {
        /// <summary>
        /// Codigo de Validação da AT
        /// </summary>
        public string CodigoValidacao { get; set; }

        /// <summary>
        /// Data de Validade da Certidão
        /// </summary>
        public DateTime DataValidade { get; set; }
    }
}
