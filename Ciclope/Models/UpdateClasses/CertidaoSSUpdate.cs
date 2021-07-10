using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API a certidao SS
    /// </summary>
    public class CertidaoSSUpdate
    {
        /// <summary>
        /// Data Inicio da Validade da Certidao
        /// </summary>
        public DateTime DataEmissao { get; set; }

        /// <summary>
        /// Data Fim da Valilade da Certidao
        /// </summary>
        public DateTime DataFimValidade { get; set; }
    }
}
