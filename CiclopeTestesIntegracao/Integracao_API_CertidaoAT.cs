using Ciclope;
using Ciclope.Controllers;
using Ciclope.Data;
using Ciclope.Models;
using Ciclope.Models.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CiclopeTestesIntegracao
{
    public class Integracao_API_CertidaoAT : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Integracao_API_CertidaoAT(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        }

        [Fact]
        public async Task Test_Fuction_Get_All_CertidaoAT_API()

        {
            ///Inicialização
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999",ApiKey = Guid.NewGuid().ToString() });
                context.SaveChanges();
                context.TBL_Doc_CertidaoAT.AddRange(
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1,LocalFicheiro = "" },
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" },
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_CertidaoAT_APIController(context);
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_CertidaoAT(empresa.ApiKey);


                ///Resultados
                var viewResult = Assert.IsType<ActionResult<IEnumerable<TBL_Doc_CertidaoAT>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Doc_CertidaoAT>>(viewResult.Value);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_Fuction_Get_One_CertidaoAT_API()

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
                context.TBL_Doc_CertidaoAT.AddRange(
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" },
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" },
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_CertidaoAT_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_CertidaoAT(empresa.ApiKey,2);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_CertidaoAT>>(result);
                var model = Assert.IsAssignableFrom<TBL_Doc_CertidaoAT>(viewResult.Value);
                Assert.Equal(1, model.IdEmpresa);
            }
        }

        [Fact]
        public async Task Test_Fuction_Update_CertidaoAT_API()

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
                context.TBL_Doc_CertidaoAT.AddRange(
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" },
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" },
                    new TBL_Doc_CertidaoAT { DataValidade = DateTime.Now, IdEmpresa = 1, LocalFicheiro = "" });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_CertidaoAT_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();
                CertidaoATUpdate dados = new();
                dados.CodigoValidacao = "55555";
                dados.DataValidade = DateTime.Parse("Jan 1, 2009");

                ///Funcao a Testar
                var result = await controller.PutTBL_Doc_CertidaoAT(empresa.ApiKey, 2,dados);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_CertidaoAT>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_CertidaoAT>(Response.Value);
                Assert.Equal(DateTime.Parse("Jan 1, 2009"), model.DataValidade);
                Assert.Equal("55555", model.CodigoValidacao);
            }
        }

        [Fact]
        public async Task Test_Fuction_Insert_CertidaoAT_API()

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
                var controller = new TBL_Doc_CertidaoAT_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/Certidao_AT.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                var result = await controller.PostTBL_Doc_CertidaoAT(empresa.ApiKey, formFile);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_CertidaoAT>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_CertidaoAT>(Response.Value);
                Assert.Equal(DateTime.Parse("May 12, 2021"), model.DataValidade);
                Assert.Equal("RDWSZS1CU7F7", model.CodigoValidacao);
            }
        }


        [Fact]
        public async Task Test_Fuction_Delete_CertidaoAT_API()

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
                context.TBL_Doc_CertidaoAT.AddRange(
                            new TBL_Doc_CertidaoAT { CodigoValidacao = "2", IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_CertidaoAT { CodigoValidacao = "2", IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_CertidaoAT { CodigoValidacao = "2", IdEmpresa = 1, LocalFicheiro = "/" });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var controller = new TBL_Doc_CertidaoAT_APIController(context);

                ///Funcao a Testar
                var result = await controller.DeleteTBL_Doc_CertidaoAT(empresa.ApiKey,1);

                ///Resultados
                NoContentResult t = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, t.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Fuction_DownloadFile_CertidaoAT_API()

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

                var controller = new TBL_Doc_CertidaoAT_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/Certidao_AT.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                await controller.PostTBL_Doc_CertidaoAT(empresa.ApiKey, formFile);

                ///Funcao a Testar
                var result = await controller.DownloadFile(empresa.ApiKey, 1);

                ///Resultados
                FileStreamResult t = Assert.IsType<FileStreamResult>(result);
                Assert.Equal("application/pdf", t.ContentType);
            }
        }

        [Fact]
        public async Task Test_Call_Get_All_CertidaoAT_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_CertidaoAT_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Get_One_CertidaoAT_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_CertidaoAT_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/8");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Update_CertidaoAT_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            CertidaoATUpdate certidaoAT = new() { CodigoValidacao = "123123",DataValidade=DateTime.Now};
            var json = JsonConvert.SerializeObject(certidaoAT);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ///Funcao a Testar
            var httpResponse = await client.PutAsync("/api/TBL_Doc_CertidaoAT_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/8", data);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Insert_CertidaoAT_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/Certidao_AT.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "Certidao_AT.pdf");
            ///Funcao a Testar
            var httpResponse = await client.PostAsync("/api/TBL_Doc_CertidaoAT_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Delete_CertidaoAT_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/Certidao_AT.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "Certidao_AT.pdf");

            var httpResponse = await client.PostAsync("/api/TBL_Doc_CertidaoAT_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());

            string content = await httpResponse.Content.ReadAsStringAsync();

            var certidaoAT = JsonConvert.DeserializeObject<TBL_Doc_CertidaoAT>(content);
            ///Funcao a Testar
            var httpResponseDelete = await client.DeleteAsync("/api/TBL_Doc_CertidaoAT_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/"+ certidaoAT.Id);

            ///Resultados
            httpResponseDelete.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Test_Call_DownloadFile_CertidaoAT_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_CertidaoAT_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/Ficheiro/8");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", httpResponse.Content.Headers.ContentType.ToString());
        }
    }
}
