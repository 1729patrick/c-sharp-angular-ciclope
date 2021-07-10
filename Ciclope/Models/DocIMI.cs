using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Doc_IMI")]
    public class DocIMI
    {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [DisplayName("Identificação Fiscal")]
        public int IdentificacaoFiscal { get; set; }

        [DisplayName("Ano Imposto")]
        public int AnoImposto { get; set; }

        [DisplayName("Identificação Documento")]
        public int IdentificacaoDocumento { get; set; }

        [DisplayName("Data de Liquidação")]
        public DateTime DataLiquidacao { get; set; }

        [MaxLength(50)]
        [DisplayName("Referência de Pagamento")]
        public string ReferenciaPagamento { get; set; }


        [DisplayName("Importância")]
        public double ImportanciaPagar { get; set; }

        [DisplayName("Ano Pagamento")]
        public int AnoPagamento { get; set; }

        [DisplayName("Mês Pagamento")]
        public int MesPagamento { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }

    }
}
