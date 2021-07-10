using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Doc_FundosCompensacao")]
    public class DocFundosCompensacao
    {
        [Key]
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        [DisplayName("Data Emissão")]
        public DateTime DataEmissao { get; set; }

        [DisplayName("Período Pagamento Início")]
        public DateTime PeriodoPagamentoInicio { get; set; }

        [DisplayName("Período Pagamento Fim")]
        public DateTime PeriodoPagamentoFim { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }
        [MaxLength(50)]
        public string Niss { get; set; }

        public double Valor { get; set; }
        [MaxLength(50)]
        public string Entidade { get; set; }

        [DisplayName("Referência")]
        [MaxLength(50)]
        public string Referencia { get; set; }
        [MaxLength(250)]
        public string LocalFicheiro { get; set; }
    }
}
