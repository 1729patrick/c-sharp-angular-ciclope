using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Ciclope.Models
{
  [Table("TBL_Doc_RecibosVerdes")]
  public class DocRecibosVerdes
  {
    [Key]
    public int Id { get; set; }

    public int IdEmpresa { get; set; }
    [ForeignKey("IdEmpresa")]
    public Empresa Empresa { get; set; }


    [DisplayName("Transmitente Nome")]
    public string TransmitenteNome { get; set; }

     
    [DisplayName("Transmitente Atividade")]
    public string TransmitenteAtividade { get; set; }

    
    [DisplayName("Transmitente Nif")]
    public string TransmitenteNif { get; set; }

    
    [DisplayName("Transmitente Domicilio")]
    public string TransmitenteDomicilio { get; set; }

   
    [DisplayName("Adquirente Nome")]
    public string AdquirenteNome { get; set; }


    [DisplayName("Adquirente Morada")]
    public string AdquirenteMorada { get; set; }

 
    [DisplayName("Adquirente NIF")]
    public string AdquirenteNif { get; set; }

 
    [DisplayName("Data Transmissão")]
    public DateTime DadosDataTransmissao { get; set; }


    [DisplayName("Descrição")]
    public string DadosDescricao { get; set; }


    [DisplayName("Valor Base")]
    public double DadosValorBase { get; set; }

 
    [DisplayName("IVA")]
    public double DadosIva { get; set; }

  
    [DisplayName("Imposto Selo")]
    public double DadosImpostoSelo { get; set; }


    [DisplayName("IRS")]
    public double DadosIRS { get; set; }

    //[Required]
    //[DisplayName("Importancia")]
    //public string DadosImportancia { get; set; }

    //[Required]
    //[DisplayName("Importancia Título")]
    //public string DadosImportanciaTitulo { get; set; }


    [DisplayName("Fatura Recibo")]
    public string FaturaRecibo { get; set; }

 
    [DisplayName("Data Emissão")]
    public DateTime DataEmissao { get; set; }

    [Required]
    public string LocalFicheiro { get; set; }

  }
}
