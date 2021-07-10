using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
    [Table("TBL_Doc_Dmr")]
    public class DocDMR
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

        [MaxLength(50)]
        [DisplayName("Referência de Pagamento")]
        public string ReferenciaPagamento { get; set; }

        [MaxLength(50)]
        [DisplayName("Linha Óptica")]
        public string LinhaOptica { get; set; }

        [DisplayName("Importância")]
        public double ImportanciaPagar { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; } // Guia de Pagamento

        [MaxLength(250)]
        public string LocalDeclaracao { get; set; } //Comprovativo
    }
}
