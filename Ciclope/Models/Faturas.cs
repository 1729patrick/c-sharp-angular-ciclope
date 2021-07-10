using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Faturas")]
    public class Faturas
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Número")]
        [MaxLength(50)]
        public string numero { get; set; }

        [MaxLength(50)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [Required]
        public double Valor { get; set; }

        [MaxLength(50)]
        public string Entidade { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }

        public int EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        public string QRcode { get; set; }

        public DateTime Data { get; set; }

        [DisplayName("Valor IVA")]
        public double ValorIva { get; set; }

    }
}
