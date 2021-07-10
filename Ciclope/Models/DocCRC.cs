using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
    [Table("TBL_Doc_CRC")]
    public class DocCRC
    {
        [Key]
        public int Id { get; set; }
        public int IdEmpresa { get; set; }

        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }

        [DisplayName("NIF")]
        [MaxLength(9)]
        public string Nif { get; set; }

        public double Total { get; set; }

        [DisplayName("Incumprimento")]
        public double EmIncumprimento { get; set; }

        [DisplayName("Montante Potêncial")]
        public double MontantePotencial { get; set; }

        [DisplayName("Número de Produtos")]
        public int NProdutos { get; set; }

        [DisplayName("Data Emissão")]
        public DateTime DataEmissao { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }

        [DisplayName("Data Submissão")]
        public DateTime DataSubmissao { get; set; }
    }
}
