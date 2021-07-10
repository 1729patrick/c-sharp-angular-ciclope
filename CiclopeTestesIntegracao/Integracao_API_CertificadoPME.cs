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
    public class Integracao_API_CertificadoPME : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Integracao_API_CertificadoPME(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        }

        [Fact]
        public async Task Test_Fuction_Get_All_CertificadoPME_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.TBL_PME_Classificacao.AddRange(
                   new TBL_PME_Classificacao { Classificacao = "Micro"},
                   new TBL_PME_Classificacao { Classificacao = "Pequena" },
                   new TBL_PME_Classificacao { Classificacao = "Média" });
                context.SaveChanges();
                context.TBL_Doc_CertificadoPME.AddRange(
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_CertificadoPME_APIController(context);
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_CertificadoPME(empresa.ApiKey);


                ///Resultados
                var viewResult = Assert.IsType<ActionResult<IEnumerable<TBL_Doc_CertificadoPME>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Doc_CertificadoPME>>(viewResult.Value);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_Fuction_Get_One_CertificadoPME_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.TBL_PME_Classificacao.AddRange(
                   new TBL_PME_Classificacao { Classificacao = "Micro" },
                   new TBL_PME_Classificacao { Classificacao = "Pequena" },
                   new TBL_PME_Classificacao { Classificacao = "Média" });
                context.SaveChanges();
                context.TBL_Doc_CertificadoPME.AddRange(
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_CertificadoPME_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_CertificadoPME(empresa.ApiKey, 2);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_CertificadoPME>>(result);
                var model = Assert.IsAssignableFrom<TBL_Doc_CertificadoPME>(viewResult.Value);
                Assert.Equal(1, model.IdEmpresa);
            }
        }

        [Fact]
        public async Task Test_Fuction_Update_CertificadoPME_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.TBL_PME_Classificacao.AddRange(
                   new TBL_PME_Classificacao { Classificacao = "Micro" },
                   new TBL_PME_Classificacao { Classificacao = "Pequena" },
                   new TBL_PME_Classificacao { Classificacao = "Média" });
                context.SaveChanges();
                context.TBL_Doc_CertificadoPME.AddRange(
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_CertificadoPME_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();
                CertificadoPMEUpdate dados = new() { DataDecissao = DateTime.Parse("Jan 2, 2009"), DataEfeito = DateTime.Parse("Jan 1, 2009"), IdClassificacao = 1 };

                ///Funcao a Testar
                var result = await controller.PutTBL_Doc_CertificadoPME(empresa.ApiKey, 2, dados);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_CertificadoPME>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_CertificadoPME>(Response.Value);
                Assert.Equal(DateTime.Parse("Jan 2, 2009"), model.DataDecissao);
                Assert.Equal(DateTime.Parse("Jan 1, 2009"), model.DataEfeito);
            }
        }

        [Fact]
        public async Task Test_Fuction_Insert_CertificadoPME_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999", ApiKey = Guid.NewGuid().ToString() });
                context.TBL_PME_Classificacao.AddRange(
                   new TBL_PME_Classificacao { Classificacao = "Micro" },
                   new TBL_PME_Classificacao { Classificacao = "Pequena" },
                   new TBL_PME_Classificacao { Classificacao = "Média" });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_CertificadoPME_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/CertidaoPME.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                var result = await controller.PostTBL_Doc_CertificadoPME(empresa.ApiKey, formFile);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_CertificadoPME>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_CertificadoPME>(Response.Value);
                Assert.Equal(DateTime.Parse("Out 22, 2020"), model.DataEfeito);
                Assert.Equal(DateTime.Parse("Out 22, 2020"), model.DataDecissao);
                Assert.Equal("Micro", model.TBL_PME_Classificacao.Classificacao);
            }
        }


        [Fact]
        public async Task Test_Fuction_Delete_CertificadoPME_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.TBL_PME_Classificacao.AddRange(
                  new TBL_PME_Classificacao { Classificacao = "Micro" },
                  new TBL_PME_Classificacao { Classificacao = "Pequena" },
                  new TBL_PME_Classificacao { Classificacao = "Média" });
                context.SaveChanges();
                context.TBL_Doc_CertificadoPME.AddRange(
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 },
                    new TBL_Doc_CertificadoPME { DataDecissao = DateTime.Now, DataEfeito = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "", IdClassificacao = 1 });
                context.SaveChanges();
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var controller = new TBL_Doc_CertificadoPME_APIController(context);

                ///Funcao a Testar
                var result = await controller.DeleteTBL_Doc_CertificadoPME(empresa.ApiKey, 1);

                ///Resultados
                NoContentResult t = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, t.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Fuction_DownloadFile_CertificadoPME_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.TBL_PME_Classificacao.AddRange(
                   new TBL_PME_Classificacao { Classificacao = "Micro" },
                   new TBL_PME_Classificacao { Classificacao = "Pequena" },
                   new TBL_PME_Classificacao { Classificacao = "Média" });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {

                var controller = new TBL_Doc_CertificadoPME_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/CertidaoPME.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                await controller.PostTBL_Doc_CertificadoPME(empresa.ApiKey, formFile);

                ///Funcao a Testar
                var result = await controller.DownloadFile(empresa.ApiKey, 1);

                ///Resultados
                FileStreamResult t = Assert.IsType<FileStreamResult>(result);
                Assert.Equal("application/pdf", t.ContentType);
            }
        }

        [Fact]
        public async Task Test_Call_Get_All_CertificadoPME_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_CertificadoPME_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Get_One_CertificadoPME_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_CertificadoPME_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/11");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Update_CertificadoPME_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            CertificadoPMEUpdate CertificadoPME = new() { DataDecissao = DateTime.Parse("Jan 2, 2009"), DataEfeito = DateTime.Parse("Jan 1, 2009"),IdClassificacao = 1 };
            var json = JsonConvert.SerializeObject(CertificadoPME);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ///Funcao a Testar
            var httpResponse = await client.PutAsync("/api/TBL_Doc_CertificadoPME_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/11", data);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Insert_CertificadoPME_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/CertidaoPME.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "CertidaoPME.pdf");
            ///Funcao a Testar
            var httpResponse = await client.PostAsync("/api/TBL_Doc_CertificadoPME_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Delete_CertificadoPME_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/CertidaoPME.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "CertidaoPME.pdf");

            var httpResponse = await client.PostAsync("/api/TBL_Doc_CertificadoPME_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());

            string content = await httpResponse.Content.ReadAsStringAsync();

            var certidaoSS = JsonConvert.DeserializeObject<TBL_Doc_CertidaoSS>(content);
            ///Funcao a Testar
            var httpResponseDelete = await client.DeleteAsync("/api/TBL_Doc_CertificadoPME_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/" + certidaoSS.Id);

            ///Resultados
            httpResponseDelete.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Test_Call_DownloadFile_CertificadoPME_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_CertificadoPME_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/Ficheiro/11");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", httpResponse.Content.Headers.ContentType.ToString());
        }
    }
}
