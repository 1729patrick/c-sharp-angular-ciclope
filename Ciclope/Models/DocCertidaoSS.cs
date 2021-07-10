using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
  [Table("TBL_Doc_CertidaoSS")]
  public class DocCertidaoSS
  {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [Required]
        [DisplayName("Data Emissão")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [DisplayName("Data Validade")]
        public DateTime DataFimValidade { get; set; }

        [Required]
        public string LocalFicheiro { get; set; }
  }
}
