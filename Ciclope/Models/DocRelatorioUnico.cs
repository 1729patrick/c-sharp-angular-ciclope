using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Doc_RelatorioUnico")]
    public class DocRelatorioUnico
    {
        [Key]
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        [ForeignKey("IdEmpresa")]
        public Empresa Empresa { get; set; }

        public int Ano { get; set; }

        public string LocalAnexo0 { get; set; }

        public string LocalAnexoA { get; set; }

        public string LocalAnexoB { get; set; }

        public string LocalAnexoC { get; set; }

        public string LocalAnexoD { get; set; }

        public string LocalAnexoE { get; set; }

        public string LocalAnexoF { get; set; }

        public string LocalCertificado { get; set; }
    }
}
