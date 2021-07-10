using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
    [Table("TBL_Doc_BCB")]
    public class DocBCB
    {
        [Key]
        public int Id { get; set; }
        public int IdEmpresa { get; set; }

        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [DisplayName("Data Emissão")]
        public DateTime DataEmissao { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }

        [DisplayName("Data Submissão")]
        public DateTime DataSubmissao { get; set; }
    }
}
