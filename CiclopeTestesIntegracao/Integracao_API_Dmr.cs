using Ciclope;
using Ciclope.Controllers;
using Ciclope.Data;
using Ciclope.Models;
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
    public class Integracao_API_Dmr : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Integracao_API_Dmr(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        }

        [Fact]
        public async Task Test_Fuction_Get_All_Dmr_API()

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
                context.TBL_Doc_Dmr.AddRange(
                    new TBL_Doc_Dmr { ReferenciaPagamento = "123123123", ImportanciaPagar = 12.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "412123123", ImportanciaPagar = 22.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "432123123", ImportanciaPagar = 32.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_Dmr_APIController(context);
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_Dmr(empresa.ApiKey);


                ///Resultados
                var viewResult = Assert.IsType<ActionResult<IEnumerable<TBL_Doc_Dmr>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Doc_Dmr>>(viewResult.Value);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_Fuction_Get_One_Dmr_API()

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
                context.TBL_Doc_Dmr.AddRange(
                    new TBL_Doc_Dmr { ReferenciaPagamento = "123123123", ImportanciaPagar = 12.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "412123123", ImportanciaPagar = 22.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "432123123", ImportanciaPagar = 32.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_Dmr_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_Dmr(empresa.ApiKey, 2);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_Dmr>>(result);
                var model = Assert.IsAssignableFrom<TBL_Doc_Dmr>(viewResult.Value);
                Assert.Equal(1, model.IdEmpresa);
            }
        }

        [Fact]
        public async Task Test_Fuction_Update_Dmr_API()

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
                context.TBL_Doc_Dmr.AddRange(
                    new TBL_Doc_Dmr { ReferenciaPagamento = "123123123", ImportanciaPagar = 12.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "412123123", ImportanciaPagar = 22.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "432123123", ImportanciaPagar = 32.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_Dmr_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();
                DmrUpdate dados = new() { Nome = "Teste",Morada = "rua 3",Localidade = "Porto",CodigoPostal = "123123-1234",
                    Periodo = "2002",IdDeclaracao=12312,LinhaOptica="3234234234234",ReferenciaPagamento = "432123123",
                    ImportanciaPagar = 32.34, DataRececaoDeclaracao = DateTime.Now };

                ///Funcao a Testar
                var result = await controller.PutTBL_Doc_Dmr(empresa.ApiKey, 2, dados);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_Dmr>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_Dmr>(Response.Value);
                Assert.Equal("rua 3", model.Morada);
                Assert.Equal("Porto", model.Localidade);
                Assert.Equal("Teste", model.Nome);
            }
        }

        [Fact]
        public async Task Test_Fuction_Insert_Dmr_API()

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
                var controller = new TBL_Doc_Dmr_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DMR.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                var result = await controller.PostTBL_Doc_Dmr(empresa.ApiKey, formFile);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_Dmr>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_Dmr>(Response.Value);
                Assert.Equal("VIRTUALNAUTA, LDA", model.Nome);
                Assert.Equal("R DR CESAR COELHO N 9 7º", model.Morada);
                Assert.Equal("2835-345 LAVRADIO", model.CodigoPostal);
            }
        }


        [Fact]
        public async Task Test_Fuction_Delete_Dmr_API()

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
                context.TBL_Doc_Dmr.AddRange(
                    new TBL_Doc_Dmr { ReferenciaPagamento = "123123123", ImportanciaPagar = 12.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "412123123", ImportanciaPagar = 22.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 },
                    new TBL_Doc_Dmr { ReferenciaPagamento = "432123123", ImportanciaPagar = 32.34, DataRececaoDeclaracao = DateTime.Now, IdEmpresa = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var controller = new TBL_Doc_Dmr_APIController(context);

                ///Funcao a Testar
                var result = await controller.DeleteTBL_Doc_Dmr(empresa.ApiKey, 1);

                ///Resultados
                NoContentResult t = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, t.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Fuction_DownloadFile_Dmr_API()

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

                var controller = new TBL_Doc_Dmr_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DMR.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                await controller.PostTBL_Doc_Dmr(empresa.ApiKey, formFile);

                ///Funcao a Testar
                var result = await controller.DownloadFile(empresa.ApiKey, 1);

                ///Resultados
                FileStreamResult t = Assert.IsType<FileStreamResult>(result);
                Assert.Equal("application/pdf", t.ContentType);
            }
        }

        [Fact]
        public async Task Test_Call_Get_All_Dmr_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_Dmr_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Get_One_Dmr_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_Dmr_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/42");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Update_Dmr_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            DmrUpdate dados = new()
            {
                Nome = "Teste",
                Morada = "rua 3",
                Localidade = "Porto",
                CodigoPostal = "123123-1234",
                Periodo = "2002",
                IdDeclaracao = 12312,
                LinhaOptica = "3234234234234",
                ReferenciaPagamento = "432123123",
                ImportanciaPagar = 32.34,
                DataRececaoDeclaracao = DateTime.Now
            };
            var json = JsonConvert.SerializeObject(dados);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ///Funcao a Testar
            var httpResponse = await client.PutAsync("/api/TBL_Doc_Dmr_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/42", data);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Insert_Dmr_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DMR.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "DMR.pdf");
            ///Funcao a Testar
            var httpResponse = await client.PostAsync("/api/TBL_Doc_Dmr_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Delete_Dmr_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DMR.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "DMR.pdf");

            var httpResponse = await client.PostAsync("/api/TBL_Doc_Dmr_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());

            string content = await httpResponse.Content.ReadAsStringAsync();

            var dmr = JsonConvert.DeserializeObject<TBL_Doc_Dmr>(content);
            ///Funcao a Testar
            var httpResponseDelete = await client.DeleteAsync("/api/TBL_Doc_Dmr_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/" + dmr.Id);

            ///Resultados
            httpResponseDelete.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Test_Call_DownloadFile_Dmr_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_Dmr_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/Ficheiro/42");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", httpResponse.Content.Headers.ContentType.ToString());
        }
    }
}
