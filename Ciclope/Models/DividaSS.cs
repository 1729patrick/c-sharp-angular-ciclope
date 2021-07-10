using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
    [Table("TBL_Divida_SS")]
    public class DividaSS
    {
        [Key]
        public int Id { get; set; }

        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        [Required]
        public DateTime Data { get; set; }

        public double Divida { get; set; }
    }
}