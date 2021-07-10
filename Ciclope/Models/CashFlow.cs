using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
    [Table("TBL_CashFlow")]
    public class CashFlow
    {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [DisplayName("Data")]
        public DateTime Data { get; set; }

        [DisplayName("Valor")]
        public double Valor { get; set; }

        [DisplayName("Fatura")]
        public int IdFatura { get; set; }
        [ForeignKey("IdFatura")]
        public Faturas Fatura { get; set; }

        [DisplayName("É cliente?")]
        public Boolean Cliente { get; set; }

        [NotMapped]
        public double Saldo { get; set; } 
    }
}
