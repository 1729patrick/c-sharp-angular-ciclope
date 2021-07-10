using Ciclope.Data;
using Ciclope.Models;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ciclope
{
    public static class Processador
    {


        public static DocFundosCompensacao ProcessarFundos(Stream stream, DocFundosCompensacao fundos)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect = new Rectangle(200, 540, 150, 50);//Niss
            Rectangle rect1 = new Rectangle(100, 400, 50, 20);//Entidade
            Rectangle rect2 = new Rectangle(120, 380, 10, 20);//Referencia
            Rectangle rect3 = new Rectangle(250, 380, 5, 20);//Valor
            Rectangle rect4 = new Rectangle(200, 550, 150, 50);//Data Emissao
            fundos.Niss = ExtractTextFromPDF(pdfDoc, rect).Split(":")[1];
            fundos.Entidade = ExtractTextFromPDF(pdfDoc, rect1);
            fundos.Referencia = ExtractTextFromPDF(pdfDoc, rect2);
            fundos.Valor = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect3));
            fundos.DataEmissao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect4), "yyyy-MM-dd", null);
            pdfDoc.Close();
            pdfReader.Close();
            return fundos;
        }

        public static DocDRI ProcessarDri(Stream stream, DocDRI dri)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect1 = new Rectangle(200, 600, 20, 20);//Data
            Rectangle rect2 = new Rectangle(200, 550, 20, 20);//estado
            Rectangle rect3 = new Rectangle(400, 500, 20, 20);//TotalRemuneracao
            Rectangle rect4 = new Rectangle(480, 500, 20, 20);//Contribuicoes
            dri.DataRegisto = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect1), "yyyy-MM-dd", null);
            dri.Estado = ExtractTextFromPDF(pdfDoc, rect2);
            dri.TotalRemuneracoes = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect3).Replace(".", ""));
            dri.TotalContribuicoes = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect4).Replace(".", ""));
            pdfDoc.Close();
            pdfReader.Close();
            return dri;
        }

        public static DocsIVA ProcessarIva(Stream stream, DocsIVA iva)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect1 = new Rectangle(400, 650, 50, 20);//Data
            Rectangle rect2 = new Rectangle(200, 530, 10, 20);//importancia
            Rectangle rect3 = new Rectangle(200, 650, 50, 20);//periodo
            iva.DataRececaoDeclaracao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect1), "yyyy-MM-dd HH:mm:ss", null);
            iva.Valor = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect2).Replace(".", "").Replace("€", ""));
            iva.Periodo = ExtractTextFromPDF(pdfDoc, rect3);
            pdfDoc.Close();
            pdfReader.Close();
            return iva;
        }

        public static DocRendas ProcessarRenda(Stream stream, DocRendas renda)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect1 = new Rectangle(550, 730, 20, 20);//Data
            Rectangle rect2 = new Rectangle(530, 400, 20, 20);//importancia
            Rectangle rect3 = new Rectangle(530, 410, 20, 20);//IRS
            renda.DataEmissao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect1), "yyyy-MM-dd", null);
            renda.ImportanciaRecebida = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect2).Replace(".", ""));
            renda.RetencaoIRS = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect3).Replace(".", ""));
            pdfDoc.Close();
            pdfReader.Close();
            return renda;
        }

        public static DocRecibosVerdes ProcessarRecibosVerdes(Stream stream, DocRecibosVerdes reciboVerde)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect1 = new Rectangle(550, 750, 20, 20);//Data Emissao
            Rectangle rect2 = new Rectangle(300, 600, 50, 20);//Data Trasmissao
            Rectangle rect3 = new Rectangle(500, 500, 40, 10);//IRS
            Rectangle rect4 = new Rectangle(500, 520, 40, 10);//IVA
            Rectangle rect5 = new Rectangle(500, 530, 40, 10);//Valor
            reciboVerde.DataEmissao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect1), "dd/MM/yyyy",null);
            reciboVerde.DadosDataTransmissao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect2), "dd/MM/yyyy", null);
            reciboVerde.DadosIRS = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect3));
            reciboVerde.DadosIva = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect4));
            reciboVerde.DadosValorBase = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect5));
            pdfDoc.Close();
            pdfReader.Close();
            return reciboVerde;
        }

        public static DocCertidaoAT ProcessarCertidaoAT(Stream stream, DocCertidaoAT certidaoAT)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect1 = new Rectangle(250, 550, 50, 20);//Data
            Rectangle rect2 = new Rectangle(150, 150, 250, 20);//Codigo
            var t = ExtractTextFromPDF(pdfDoc, rect1).Split(" ");
            DateTime date = new DateTime(Convert.ToInt32(t[t.Length - 1].Replace(".", "")), MonthToNumber(t[t.Length - 3]), Convert.ToInt32(t[t.Length - 5]));
            certidaoAT.CodigoValidacao = ExtractTextFromPDF(pdfDoc, rect2);
            certidaoAT.DataValidade = date.AddMonths(3);
            pdfDoc.Close();
            pdfReader.Close();
            return certidaoAT;
        }

        public static DocCertidaoSS ProcessarCertidaoSS(Stream stream, DocCertidaoSS certidaoSS)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect1 = new Rectangle(150, 550, 250, 50);//Data
            DateTime date = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect1), "dd-MM-yyyy", null);
            certidaoSS.DataEmissao = date;
            certidaoSS.DataFimValidade = date.AddMonths(4);
            pdfDoc.Close();
            pdfReader.Close();
            return certidaoSS;
        }

        public static DocDMR ProcessarDMR(Stream stream,  DocDMR dmr)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            Rectangle rect1 = new Rectangle(150, 700, 250, 20);//Nome
            Rectangle rect2 = new Rectangle(150, 680, 250, 20);//Morada
            Rectangle rect3 = new Rectangle(100, 660, 50, 20);//Localidade
            Rectangle rect4 = new Rectangle(400, 660, 50, 20);//CodigoPostal
            Rectangle rect5 = new Rectangle(400, 600, 150, 20);//Data
            Rectangle rect6 = new Rectangle(400, 600, 150, 20);//Perido
            Rectangle rect7 = new Rectangle(200, 540, 150, 20);//Referencia
            Rectangle rect8 = new Rectangle(200, 500, 150, 20);//LinhaOptica
            Rectangle rect9 = new Rectangle(200, 460, 150, 20);//Importancia
            dmr.Nome = ExtractTextFromPDF(pdfDoc, rect1);
            dmr.Morada = ExtractTextFromPDF(pdfDoc, rect2);
            dmr.Localidade = ExtractTextFromPDF(pdfDoc, rect3);
            dmr.CodigoPostal = ExtractTextFromPDF(pdfDoc, rect4);
            dmr.DataRececaoDeclaracao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect5), "yyyy-MM-dd HH:mm:ss", null);
            dmr.Periodo = ExtractTextFromPDF(pdfDoc, rect6);
            dmr.ReferenciaPagamento = ExtractTextFromPDF(pdfDoc, rect7);
            dmr.LinhaOptica = ExtractTextFromPDF(pdfDoc, rect8);
            dmr.ImportanciaPagar = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect9).Replace("€", ""));
            pdfDoc.Close();
            pdfReader.Close();
            return dmr;
        }

        public static DocM22 ProcessarM22(Stream stream, DocM22 DocM22)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            Rectangle rect1 = new Rectangle(500, 610, 50, 50);//Pagar 
            Rectangle rect2 = new Rectangle(350, 740, 250, 20);//Ano
            Rectangle rect3 = new Rectangle(100, 570, 50, 20);//referencia        
            DocM22.ImportanciaPagar = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect1));
            DocM22.Ano = Convert.ToInt32(ExtractTextFromPDF(pdfDoc, rect2));
            DocM22.RefPagamento = ExtractTextFromPDF(pdfDoc, rect3);

            pdfDoc.Close();
            pdfReader.Close();
            return DocM22;
        }

        public static DocIES ProcessarIES(Stream stream, DocIES DocIES)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            Rectangle rect1 = new Rectangle(290, 500, 100, 10);//Valor 
            Rectangle rect2 = new Rectangle(290, 510, 100, 10);//Ref
            Rectangle rect3 = new Rectangle(290, 530, 100, 10);//Entidade
            Rectangle rect4 = new Rectangle(290, 480, 100, 10);//Data

            DocIES.Valor = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect1).Replace("€", ""));
            DocIES.Referencia = ExtractTextFromPDF(pdfDoc, rect2);
            DocIES.Entidade = ExtractTextFromPDF(pdfDoc, rect3);
            DocIES.DataValidade = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect4).Trim(), "yyyy-MM-dd", null);
            pdfDoc.Close();
            pdfReader.Close();
            return DocIES;
        }

        public static DocRelatorioUnico ProcessarRU(Stream stream, DocRelatorioUnico tBL_Doc_RU)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            Rectangle rect1 = new Rectangle(450, 630, 50, 20);

            tBL_Doc_RU.Ano = Convert.ToInt32(ExtractTextFromPDF(pdfDoc, rect1));

            pdfDoc.Close();
            pdfReader.Close();
            return tBL_Doc_RU;
        }

        public static DocIMI ProcessarIMI(Stream stream, DocIMI DocIMI)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            //Rectangle rect1 = new Rectangle(50, 280, 100, 50);//prestacao 
            Rectangle rect1 = new Rectangle(50, 240, 100, 20);//Referencia 
            Rectangle rect2 = new Rectangle(50, 200, 100, 20);//valor 
            Rectangle rect3 = new Rectangle(50, 180, 100, 10);//mes pagamento
            Rectangle rect4 = new Rectangle(400, 580, 100, 20);//Dataliquidadacao

            DocIMI.ReferenciaPagamento = ExtractTextFromPDF(pdfDoc, rect1);
            var t = ExtractTextFromPDF(pdfDoc, rect3).Split("/");
            DocIMI.MesPagamento = MonthToNumber(t[0]);
            DocIMI.AnoPagamento = Convert.ToInt32(t[1]);
            DocIMI.ImportanciaPagar = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect2));
            DocIMI.DataLiquidacao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect4), "yyyy-MM-dd", null);
            pdfDoc.Close();
            pdfReader.Close();
            return DocIMI;
        }

        public static DocIUC ProcessarIUC(Stream stream, DocIUC DocIUC)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            Rectangle rect1 = new Rectangle(50, 460, 200, 50);//valor 
            Rectangle rect2 = new Rectangle(100, 600, 50, 20);//Data
            Rectangle rect3 = new Rectangle(100, 520, 50, 20);//Referencia

            DocIUC.ImportanciaPagar = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect1).Replace("€", ""));
            DocIUC.DataLimite = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect2).Trim(), "yyyy-MM-dd", null);
            DocIUC.ReferenciaPagamento = ExtractTextFromPDF(pdfDoc, rect3);
            pdfDoc.Close();
            pdfReader.Close();
            return DocIUC;
        }

        public static DocCertificadoPME ProcessarPME(Stream stream, DocCertificadoPME DocCertificadoPME, ApplicationDbContext context)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            Rectangle rect1 = new Rectangle(50, 380, 500, 150);//prestacao 
            Rectangle rect2 = new Rectangle(150, 320, 200, 20);//DataDecisao
            Rectangle rect3 = new Rectangle(150,300, 200, 20);//DataEfeito


            var classificacao = ExtractTextFromPDF(pdfDoc, rect1).Split("satisfaz os requisitos de")[1].Split("empresa")[0].Trim();
            PMEClassificacao PMEClassificacao = context.PMEClassificacao.FirstOrDefault(m => m.Classificacao == classificacao);
            DocCertificadoPME.IdClassificacao =  PMEClassificacao.Id;
            DocCertificadoPME.DataDecissao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect2).Split(":")[1].Trim(), "dd-MM-yyyy", null);
            DocCertificadoPME.DataEfeito = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect3).Split(":")[1].Trim(), "dd-MM-yyyy", null);         
            pdfDoc.Close();
            pdfReader.Close();
            return DocCertificadoPME;
        }

        public static DocCRC ProcessarCRC(Stream stream, DocCRC DocCRC)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            Rectangle rect1 = new Rectangle(50, 750, 100, 100);//Nome
            Rectangle rect2 = new Rectangle(150, 480, 200, 20);//Potencial 
            Rectangle rect3 = new Rectangle(250, 700, 50, 100);//Nif
            Rectangle rect4 = new Rectangle(150, 520, 50, 20);//Incumprimento 
            Rectangle rect5 = new Rectangle(150, 540, 50, 20);//Total 
            Rectangle rect6 = new Rectangle(500, 580, 50, 20);//Numero 
            Rectangle rect7 = new Rectangle(50, 0, 50, 50);//DataEmissao 

            DocCRC.Nome = ExtractTextFromPDF(pdfDoc, rect1);
            DocCRC.MontantePotencial = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect2).Replace("€", ""));
            DocCRC.Nif = ExtractTextFromPDF(pdfDoc, rect3);
            DocCRC.EmIncumprimento = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect4).Replace("€", ""));
            DocCRC.Total = Convert.ToDouble(ExtractTextFromPDF(pdfDoc, rect5).Replace("€", ""));
            DocCRC.NProdutos = Convert.ToInt32(ExtractTextFromPDF(pdfDoc, rect6));
            DocCRC.DataEmissao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect7).Split(" ")[3].Trim(), "dd-MM-yyyy", null);
            pdfDoc.Close();
            pdfReader.Close();
            return DocCRC;
        }

        public static DocBCB ProcessarBCB(Stream stream, DocBCB DocBCB)
        {
            iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            Rectangle rect1 = new Rectangle(50, 0, 50, 50);//DataEmissao 

            DocBCB.DataEmissao = DateTime.ParseExact(ExtractTextFromPDF(pdfDoc, rect1).Split(" ")[3].Trim(), "yyyy-MM-dd", null);

            pdfDoc.Close();
            pdfReader.Close();
            return DocBCB;
        }

        private static int MonthToNumber(string month)
        {
            switch (month)
            {
                case "Janeiro":
                    return 1;
                case "Fevereiro":
                    return 2;
                case "Março":
                    return 3;
                case "Abril":
                    return 4;
                case "Maio":
                    return 5;
                case "Junho":
                    return 6;
                case "Julho":
                    return 7;
                case "Agosto":
                    return 8;
                case "Setembro":
                    return 9;
                case "Outubro":
                    return 10;
                case "Novembro":
                    return 11;
                case "Dezembro":
                    return 12;
                default:
                    return 1;
            }
        }


        public static string ExtractTextFromPDF(PdfDocument pdfDoc, Rectangle rectangle)
        {
            TextRegionEventFilter regionFilter = new TextRegionEventFilter(rectangle);
            ITextExtractionStrategy strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
            string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1), strategy);
            return pageContent;
        }

        public static string ExtractTextFromPDF(string stream, Rectangle rectangle)
        {
            PdfReader pdfReader = new PdfReader(stream);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            TextRegionEventFilter regionFilter = new TextRegionEventFilter(rectangle);
            ITextExtractionStrategy strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), regionFilter);
            string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1), strategy);
            //Console.WriteLine(pageContent);
            pdfDoc.Close();
            pdfReader.Close();
            return pageContent;
        }
    }
}
