using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API ao Recibos Verdes
    /// </summary>
    public class RecibosUpdate
    {
        /// <summary>
        /// Nome da entidade contratada
        /// </summary>
        public string TransmitenteNome { get; set; }
        /// <summary>
        /// Atividade Contratada
        /// </summary>
        public string TransmitenteAtividade { get; set; }
        /// <summary>
        /// Nif da Entidade contratada
        /// </summary>
        public string TransmitenteNif { get; set; }
        /// <summary>
        /// Morada da entidade Contratada
        /// </summary>
        public string TransmitenteDomicilio { get; set; }
        /// <summary>
        /// Nome da Entidade Empregadora
        /// </summary>
        public string AdquirenteNome { get; set; }
        /// <summary>
        /// Morada da entidade Empregadora
        /// </summary>
        public string AdquirenteMorada { get; set; }
        /// <summary>
        /// Nif da entidade Empregadora
        /// </summary>
        public string AdquirenteNif { get; set; }
        /// <summary>
        /// Data da Transmissao
        /// </summary>
        public DateTime DadosDataTransmissao { get; set; }
        /// <summary>
        /// Descricao dos dados
        /// </summary>
        public string DadosDescricao { get; set; }
        /// <summary>
        /// Valor base
        /// </summary>
        public double DadosValorBase { get; set; }
        /// <summary>
        /// Valor do IVA
        /// </summary>
        public double DadosIva { get; set; }
        /// <summary>
        /// Valor do imposto Selo
        /// </summary>
        public double DadosImpostoSelo { get; set; }

        /// <summary>
        /// Valor do IRS
        /// </summary>
        public double DadosIRS { get; set; }
    }
}
