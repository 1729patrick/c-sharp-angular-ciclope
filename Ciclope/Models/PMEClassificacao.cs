using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciclope.Models 
{
    [Table("TBL_PME_Classificacao")]
    public class PMEClassificacao 
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Classificacao { get; set; }

        //public IEnumerable<TBL_Doc_CertificadoPME> CertificadoPMEs { get; set; }
    }
}
