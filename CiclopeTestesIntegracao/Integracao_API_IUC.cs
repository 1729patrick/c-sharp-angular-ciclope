using Ciclope;
using Ciclope.Controllers;
using Ciclope.Data;
using Ciclope.Models;
using Ciclope.Models.NewFolder;
using Ciclope.Models.UpdateClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace CiclopeTestesIntegracao
{
    public class Integracao_API_IUC : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Integracao_API_IUC(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        }

        [Fact]
        public async Task Test_Fuction_Get_All_IUC_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.SaveChanges();
                context.TBL_Doc_IUC.AddRange(
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 234.43,Ano = 2020,Mes =3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 431.43, Ano = 2020, Mes = 3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 123.43, Ano = 2020, Mes = 3 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_IUC_APIController(context);
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_IUC(empresa.ApiKey);


                ///Resultados
                var viewResult = Assert.IsType<ActionResult<IEnumerable<TBL_Doc_IUC>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Doc_IUC>>(viewResult.Value);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_Fuction_Get_One_IUC_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.SaveChanges();
                context.TBL_Doc_IUC.AddRange(
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 234.43, Ano = 2020, Mes = 3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 431.43, Ano = 2020, Mes = 3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 123.43, Ano = 2020, Mes = 3 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_IUC_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_IUC(empresa.ApiKey, 2);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_IUC>>(result);
                var model = Assert.IsAssignableFrom<TBL_Doc_IUC>(viewResult.Value);
                Assert.Equal(1, model.EmpresaId);
            }
        }

        [Fact]
        public async Task Test_Fuction_Update_IUC_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.SaveChanges();
                context.TBL_Doc_IUC.AddRange(
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 234.43, Ano = 2020, Mes = 3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 431.43, Ano = 2020, Mes = 3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 123.43, Ano = 2020, Mes = 3 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_IUC_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();
                IucUpdate dados = new() { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", 
                    ImportanciaPagar = 234.43, Ano = 2020, Mes = 3,Data = DateTime.Now,
                    IdentificacaoFicheiro = "123123" , IdentificacaoFiscal = "123", Matricula = "232323",Morada = "rua 3"};
                


                ///Funcao a Testar
                var result = await controller.PutTBL_Doc_IUC(empresa.ApiKey, 2, dados);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_IUC>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_IUC>(Response.Value);
                Assert.Equal(dados.DataLimite, model.DataLimite);
                Assert.Equal(dados.ReferenciaPagamento, model.ReferenciaPagamento);
                Assert.Equal(dados.ImportanciaPagar, model.ImportanciaPagar);
                Assert.Equal(dados.Matricula, model.Matricula);
                Assert.Equal(dados.ImportanciaPagar, model.ImportanciaPagar);
            }
        }

        [Fact]
        public async Task Test_Fuction_Insert_IUC_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_IUC_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocIUC.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                var result = await controller.PostTBL_Doc_IUC(empresa.ApiKey, formFile);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_IUC>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_IUC>(Response.Value);
                Assert.Equal(67.59, model.ImportanciaPagar);
                Assert.Equal("167 020 612 212 807", model.ReferenciaPagamento);
            }
        }


        [Fact]
        public async Task Test_Fuction_Delete_IUC_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.SaveChanges();
                context.TBL_Doc_IUC.AddRange(
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 234.43, Ano = 2020, Mes = 3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 431.43, Ano = 2020, Mes = 3 },
                    new TBL_Doc_IUC { DataLimite = DateTime.Now, ReferenciaPagamento = "123123123", EmpresaId = 1, LocalFicheiro = "", ImportanciaPagar = 123.43, Ano = 2020, Mes = 3 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var controller = new TBL_Doc_IUC_APIController(context);

                ///Funcao a Testar
                var result = await controller.DeleteTBL_Doc_IUC(empresa.ApiKey, 1);

                ///Resultados
                NoContentResult t = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, t.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Fuction_DownloadFile_IUC_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {

                var controller = new TBL_Doc_IUC_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocIUC.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                await controller.PostTBL_Doc_IUC(empresa.ApiKey, formFile);

                ///Funcao a Testar
                var result = await controller.DownloadFicheiro(empresa.ApiKey, 1);

                ///Resultados
                FileStreamResult t = Assert.IsType<FileStreamResult>(result);
                Assert.Equal("application/pdf", t.ContentType);
            }
        }

        [Fact]
        public async Task Test_Call_Get_All_IUC_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_IUC_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Get_One_IUC_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_IUC_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/9");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Update_IUC_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            IucUpdate dados = new()
            {
                DataLimite = DateTime.Now,
                ReferenciaPagamento = "123123123",
                ImportanciaPagar = 234.43,
                Ano = 2020,
                Mes = 3,
                Data = DateTime.Now,
                IdentificacaoFicheiro = "123123",
                IdentificacaoFiscal = "123",
                Matricula = "232323",
                Morada = "rua 3"
            };
            var json = JsonConvert.SerializeObject(dados);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ///Funcao a Testar
            var httpResponse = await client.PutAsync("/api/TBL_Doc_IUC_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/9", data);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Insert_IUC_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocIUC.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "DocIUC.pdf");
            ///Funcao a Testar
            var httpResponse = await client.PostAsync("/api/TBL_Doc_IUC_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Delete_IUC_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocIUC.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "DocIUC.pdf");

            var httpResponse = await client.PostAsync("/api/TBL_Doc_IUC_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());

            string content = await httpResponse.Content.ReadAsStringAsync();

            var certidaoSS = JsonConvert.DeserializeObject<TBL_Doc_IUC>(content);
            ///Funcao a Testar
            var httpResponseDelete = await client.DeleteAsync("/api/TBL_Doc_IUC_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/" + certidaoSS.Id);

            ///Resultados
            httpResponseDelete.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Test_Call_DownloadFile_IUC_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_IUC_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/Ficheiro/9");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", httpResponse.Content.Headers.ContentType.ToString());
        }
    }
}
