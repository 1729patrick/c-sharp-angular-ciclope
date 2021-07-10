using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Ciclope.Models
{
  [Table("TBL_Doc_CertificadoPME")]
  public class DocCertificadoPME
  {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [DisplayName("Classificação")]
        public int IdClassificacao { get; set; }
        [ForeignKey("IdClassificacao")]
        public PMEClassificacao PMEClassificacao { get; set; }

        [Required]
        [DisplayName("Data de decisão")]
        public DateTime DataDecissao { get; set; }

        [Required]
        [DisplayName("Data de efeito")]
        public DateTime DataEfeito { get; set; }

        [Required]
        public string LocalFicheiro { get; set; }
  }
}
