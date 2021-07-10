using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
    [Table("TBL_Doc_DRI")]
    public class DocDRI
    {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [DisplayName("Data Entrega")]
        public DateTime DataEntrega { get; set; }

        [DisplayName("Data Registo")]
        public DateTime DataRegisto { get; set; }

        [MaxLength(50)]
        public string Identificador { get; set; }

        [MaxLength(50)]
        public string Estado { get; set; }

        [DisplayName("Identificação SS")]
        [MaxLength(50)]
        public string NIdentificacaoSS { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }

        [DisplayName("Total Remunerações")]
        public double TotalRemuneracoes { get; set; }

        [DisplayName("Total Contribuições")]
        public double TotalContribuicoes { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }
    }
}
