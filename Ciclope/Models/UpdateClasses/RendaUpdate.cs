using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models.UpdateClasses
{
    /// <summary>
    /// Classes usada para o update API ao Rendas
    /// </summary>
    public class RendaUpdate
    {
        /// <summary>
        /// Nome da entidade a arendar
        /// </summary>
        public string EntidadeNome { get; set; }
        /// <summary>
        /// Nif da entidade a arendar
        /// </summary>
        public string EntidadeNif { get; set; }


        /// <summary>
        /// Nome da Entidade a alugar
        /// </summary>
        public string LocatorioNome { get; set; }

        /// <summary>
        /// Nif da Entidade a alugar
        /// </summary>
        public string LocatorioNif { get; set; }

        /// <summary>
        /// Esta a arrendar?
        /// </summary>
        public bool Arrendamento { get; set; }
        /// <summary>
        /// Localização do imovel
        /// </summary>
        public string Localizacao { get; set; }
        /// <summary>
        /// Data de Inicio da renda
        /// </summary>
        public DateTime PeriodoRendaInicio { get; set; }
        /// <summary>
        /// Data de Fim da Renda
        /// </summary>
        public DateTime PeriodoRendaFim { get; set; }
        /// <summary>
        /// Titulo
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Data do Recebimento
        /// </summary>
        public DateTime DataRecebimento { get; set; }
        /// <summary>
        /// Valor do IRS
        /// </summary>
        public double RetencaoIRS { get; set; }
        /// <summary>
        /// Valor a pagar
        /// </summary>
        public double ImportanciaRecebida { get; set; }
        /// <summary>
        /// Numero de Recibos da Venda
        /// </summary>
        public int NRecibosVenda { get; set; }
        /// <summary>
        /// Data de Emissao da Renda
        /// </summary>
        public DateTime DataEmissao { get; set; }
    }
}
