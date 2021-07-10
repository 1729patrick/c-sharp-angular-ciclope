using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models
{
    [Table("TBL_Doc_M22")]
    public class DocM22
    {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [DisplayName("Identificação")]
        public int IdDeclaracao { get; set; }

        [DisplayName("Ano")]
        public int Ano { get; set; }

        [DisplayName("Documento")]
        public int IdentDocumento { get; set; }

        [DisplayName("Identificação Fiscal")]
        public int IdentificacaoFiscal { get; set; }

        [DisplayName("Importância")]
        public double ImportanciaPagar { get; set; }

        [DisplayName("Referência")]
        public string RefPagamento { get; set; }

        public string LocalFicheiro { get; set; }

        [DisplayName("Comprovativo")]
        public string LocalComprovativo { get; set; }
    }
}
