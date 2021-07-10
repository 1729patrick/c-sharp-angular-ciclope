using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Ciclope.Models {
    [Table("TBL_TrabalhadorUser")]
    public class TrabalhadorUser {

        [Key]
        public int TrabalhadorId { get; set; }

        [DisplayName("Utilizador")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public CiclopeUser User { get; set; }

        public int EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        [NotMapped]
        public string Email { get; set; }

        [DisplayName("Papel")]
        [NotMapped]
        public string RoleId { get; set; }
    }
}
