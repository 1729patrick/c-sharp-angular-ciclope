using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope.Models
{
    [Table("TBL_Empresa")]
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [DisplayName("NIF")]
        [MaxLength(9)]
        [MinLength(9)]
        [Required]
        public string Nif { get; set; }
        [MaxLength(50)]
        public string Morada { get; set; }

        [DisplayName("Código Postal")]
        [MaxLength(50)]
        public string CodigoPostal { get; set; }
        [MaxLength(50)]
        public string Cidade { get; set; }
        [MaxLength(50)]
        public string Pais { get; set; }
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(9)]
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public string Telefone { get; set; }

        public string ApiKey { get; set; }
        
        public bool Ativa { get; set; }
    }
}
