using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Docs_IVA")]
    public class DocsIVA
    {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }

        [MaxLength(50)]
        public string Morada { get; set; }

        [MaxLength(50)]
        public string Localidade { get; set; }

        [MaxLength(50)]
        [DisplayName("Código Postal")]
        public string CodigoPostal { get; set; }

        [MaxLength(50)]
        [DisplayName("Período")]
        public string Periodo { get; set; }

        [DisplayName("Identificação Declaração")]
        public int IdDeclaracao { get; set; }

        [DisplayName("Data de Receção")]
        public DateTime DataRececaoDeclaracao { get; set; }


        [DisplayName("Referência de Pagamento")]
        public int Referencia { get; set; }


        [DisplayName("Linha Óptica")]
        public int LinhaOptica { get; set; }

        [DisplayName("Importância")]
        public double Valor { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }

        [MaxLength(250)]
        [DisplayName("Comprovativo")]
        public string LocalComprovativo { get; set; }
    }
}
