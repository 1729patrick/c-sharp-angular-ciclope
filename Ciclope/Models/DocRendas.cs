using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Doc_Rendas")]
    public class DocRendas
    {
        [Key]
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [MaxLength(50)]
        [DisplayName("Entidade Nome")]
        public string EntidadeNome { get; set; }
        [MaxLength(50)]
        [DisplayName("Enditade NIF")]
        public string EntidadeNif { get; set; }

        [MaxLength(50)]
        [DisplayName("Locatário Nome")]
        public string LocatorioNome { get; set; }

        [MaxLength(50)]
        [DisplayName("Locatário NIF")]
        public string LocatorioNif { get; set; }

        public bool Arrendamento { get; set; }

        [MaxLength(50)]
        public string Localizacao { get; set; }

        [DisplayName("Período Renda Início")]
        public DateTime PeriodoRendaInicio { get; set; }

        [DisplayName("Período Renda Fim")]
        public DateTime PeriodoRendaFim { get; set; }

        [MaxLength(50)]
        [DisplayName("Título")]
        public string Titulo { get; set; }

        [DisplayName("Data Recebimento")]
        public DateTime DataRecebimento { get; set; }

        [DisplayName("Retenção IRS")]
        public double RetencaoIRS { get; set; }

        [DisplayName("Importância Recebida")]
        public double ImportanciaRecebida { get; set; }

        [DisplayName("Quantidade Recibos Venda")]
        public int NRecibosVenda { get; set; }

        [DisplayName("Data Emissão")]
        public DateTime DataEmissao { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }
    }
}
