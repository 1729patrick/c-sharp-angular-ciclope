using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Doc_IUC")]
    public class DocIUC
    {

        [Key]
        public int Id { get; set; }

        [MaxLength(6)]
        [DisplayName("Matrícula")]
        public string Matricula { get; set; }

        [Required]
        public int Ano { get; set; }

        [Required]
        [DisplayName("Mês")]
        public int Mes { get; set; }

        [MaxLength(50)]
        [DisplayName("Identificação Ficheiro")]
        public string IdentificacaoFicheiro { get; set; }

        public DateTime Data { get; set; }

        [MaxLength(50)]
        [DisplayName("Identificação Fiscal")]
        public string IdentificacaoFiscal  { get; set; }

        public string Morada { get; set; }

        [Required]
        [DisplayName("Data Limite")]
        public DateTime DataLimite { get; set; }

        [MaxLength(50)]
        public string ReferenciaPagamento { get; set; }

        [DisplayName("Valor")]
        [Required]
        public double ImportanciaPagar { get; set; }

        public int EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        [MaxLength(250)]
        public string LocalFicheiro { get; set; }

        

    }
}
