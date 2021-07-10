using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
  [Table("TBL_CertidaoPermanente")]
  public class CertidaoPermanente
  {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [Required]
        public string LocalFicheiro { get; set; }

        [Required]
        public DateTime Data { get; set; }

  }
}
