using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Ciclope.Models
{
    [Table("TBL_Doc_IES")]
    public class DocIES
    {
        [Key]
        public int Id { get; set; }


        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }
        
        [DisplayName("Data")]
        public DateTime DataValidade { get; set; }

        [MaxLength(9)]
        [DisplayName("NIF")]
        public string Nif { get; set; }

        [MaxLength(50)]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        public string LocalFicheiro { get; set; }

        [DisplayName("Comprovativo")]
        public string LocalComprovativo { get; set; }
        
        [DisplayName("Pagamento")]
        public string LocalPagamento { get; set; }

        public double Valor { get; set; }

        [DisplayName("Referência")]
        public string Referencia { get; set; }

        public string Entidade { get; set; }

    }
}
