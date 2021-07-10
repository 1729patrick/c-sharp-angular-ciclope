using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
  [Table("TBL_Doc_CertidaoAT")]
  public class DocCertidaoAT
    {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [Required]
        public string LocalFicheiro { get; set; }


        [MaxLength(50)]
        [DisplayName("Código Validação")]
        public string CodigoValidacao { get; set; }

        [Required]
        [DisplayName("Data Validade")]
        public DateTime DataValidade { get; set; }

  }
}
