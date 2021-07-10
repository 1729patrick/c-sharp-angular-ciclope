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
    public class Integracao_API_IVA : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Integracao_API_IVA(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        }

        [Fact]
        public async Task Test_Fuction_Get_All_IVA_API()

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
                context.TBL_Docs_IVA.AddRange(
                    new TBL_Docs_IVA
                    {
                        Nome = "asd",
                        Morada = "asd",
                        Localidade = "qw",
                        CodigoPostal = "1234-123",
                        Periodo = "23sed",
                        IdDeclaracao = 12312,
                        DataRececaoDeclaracao = DateTime.Now,
                        Referencia = 1231231,
                        LinhaOptica = 123123123,
                        Valor = 12.5,
                        IdEmpresa = 1,
                        LocalFicheiro = "",
                    },
                    new TBL_Docs_IVA
                    {
                        Nome = "asd",
                        Morada = "asd",
                        Localidade = "qw",
                        CodigoPostal = "1234-123",
                        Periodo = "23sed",
                        IdDeclaracao = 12312,
                        DataRececaoDeclaracao = DateTime.Now,
                        Referencia = 1231231,
                        LinhaOptica = 12312312,
                        Valor = 12.5,
                        IdEmpresa = 1,
                        LocalFicheiro = "",
                    },
                    new TBL_Docs_IVA
                    {
                        Nome = "asd",
                        Morada = "asd",
                        Localidade = "qw",
                        CodigoPostal = "1234-123",
                        Periodo = "23sed",
                        IdDeclaracao = 12312,
                        DataRececaoDeclaracao = DateTime.Now,
                        Referencia = 1231231,
                        LinhaOptica = 123123123,
                        Valor = 12.5,
                        IdEmpresa = 1,
                        LocalFicheiro = "",
                    }
                    );


                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Docs_IVA_APIController(context);
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Docs_IVA(empresa.ApiKey);


                ///Resultados
                var viewResult = Assert.IsType<ActionResult<IEnumerable<TBL_Docs_IVA>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Docs_IVA>>(viewResult.Value);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_Fuction_Get_One_IVA_API()

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
                context.TBL_Docs_IVA.AddRange(
                 new TBL_Docs_IVA
                 {
                     Nome = "asd",
                     Morada = "asd",
                     Localidade = "qw",
                     CodigoPostal = "1234-123",
                     Periodo = "23sed",
                     IdDeclaracao = 12312,
                     DataRececaoDeclaracao = DateTime.Now,
                     Referencia = 1231231,
                     LinhaOptica = 123123123,
                     Valor = 12.5,
                     IdEmpresa = 1,
                     LocalFicheiro = "",
                 },
                 new TBL_Docs_IVA
                 {
                     Nome = "asd",
                     Morada = "asd",
                     Localidade = "qw",
                     CodigoPostal = "1234-123",
                     Periodo = "23sed",
                     IdDeclaracao = 12312,
                     DataRececaoDeclaracao = DateTime.Now,
                     Referencia = 1231231,
                     LinhaOptica = 123123123,
                     Valor = 12.5,
                     IdEmpresa = 1,
                     LocalFicheiro = "",
                 },
                 new TBL_Docs_IVA
                 {
                     Nome = "asd",
                     Morada = "asd",
                     Localidade = "qw",
                     CodigoPostal = "1234-123",
                     Periodo = "23sed",
                     IdDeclaracao = 12312,
                     DataRececaoDeclaracao = DateTime.Now,
                     Referencia = 1231231,
                     LinhaOptica = 123123123,
                     Valor = 12.5,
                     IdEmpresa = 1,
                     LocalFicheiro = "",
                 }
                 );
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Docs_IVA_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Docs_IVA(empresa.ApiKey, 2);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Docs_IVA>>(result);
                var model = Assert.IsAssignableFrom<TBL_Docs_IVA>(viewResult.Value);
                Assert.Equal(1, model.IdEmpresa);
            }
        }

        [Fact]
        public async Task Test_Fuction_Update_IVA_API()

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
                context.TBL_Docs_IVA.AddRange(
                  new TBL_Docs_IVA
                  {
                      Nome = "asd",
                      Morada = "asd",
                      Localidade = "qw",
                      CodigoPostal = "1234-123",
                      Periodo = "23sed",
                      IdDeclaracao = 12312,
                      DataRececaoDeclaracao = DateTime.Now,
                      Referencia = 1231231,
                      LinhaOptica = 1231231,
                      Valor = 12.5,
                      IdEmpresa = 1,
                      LocalFicheiro = "",
                  },
                  new TBL_Docs_IVA
                  {
                      Nome = "asd",
                      Morada = "asd",
                      Localidade = "qw",
                      CodigoPostal = "1234-123",
                      Periodo = "23sed",
                      IdDeclaracao = 12312,
                      DataRececaoDeclaracao = DateTime.Now,
                      Referencia = 1231231,
                      LinhaOptica = 1231231,
                      Valor = 12.5,
                      IdEmpresa = 1,
                      LocalFicheiro = "",
                  },
                  new TBL_Docs_IVA
                  {
                      Nome = "asd",
                      Morada = "asd",
                      Localidade = "qw",
                      CodigoPostal = "1234-123",
                      Periodo = "23sed",
                      IdDeclaracao = 12312,
                      DataRececaoDeclaracao = DateTime.Now,
                      Referencia = 1231231,
                      LinhaOptica = 1231231,
                      Valor = 12.5,
                      IdEmpresa = 1,
                      LocalFicheiro = "",
                  }
                  );
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Docs_IVA_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();
                IvaUpdate dados = new()
                {
                    Nome = "asd",
                    Morada = "asd",
                    Localidade = "qw",
                    CodigoPostal = "1234-123",
                    Periodo = "23sed",
                    IdDeclaracao = 12312,
                    DataRececaoDeclaracao = DateTime.Now,
                    Referencia = 1231231,
                    LinhaOptica = 1231231,
                    Valor = 12.5,
                };



                ///Funcao a Testar
                var result = await controller.PutTBL_Docs_IVA(empresa.ApiKey, 2, dados);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Docs_IVA>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Docs_IVA>(Response.Value);
                Assert.Equal(dados.Nome, model.Nome);
                Assert.Equal(dados.Morada, model.Morada);
                Assert.Equal(dados.Localidade, model.Localidade);
                Assert.Equal(dados.Periodo, model.Periodo);
                Assert.Equal(dados.Referencia, model.Referencia);
            }
        }

        [Fact]
        public async Task Test_Fuction_Insert_IVA_API()

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
                var controller = new TBL_Docs_IVA_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/IVA.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                var result = await controller.PostTBL_Docs_IVA(empresa.ApiKey, formFile);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Docs_IVA>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Docs_IVA>(Response.Value);
                Assert.Equal(1660.45, model.Valor);
                Assert.Equal("2020 / 03T", model.Periodo);

            }
        }


        [Fact]
        public async Task Test_Fuction_Delete_IVA_API()

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
                context.TBL_Docs_IVA.AddRange(
                  new TBL_Docs_IVA
                  {
                      Nome = "asd",
                      Morada = "asd",
                      Localidade = "qw",
                      CodigoPostal = "1234-123",
                      Periodo = "23sed",
                      IdDeclaracao = 12312,
                      DataRececaoDeclaracao = DateTime.Now,
                      Referencia = 1231231,
                      LinhaOptica = 1231231,
                      Valor = 12.5,
                      IdEmpresa = 1,
                      LocalFicheiro = "",
                  },
                  new TBL_Docs_IVA
                  {
                      Nome = "asd",
                      Morada = "asd",
                      Localidade = "qw",
                      CodigoPostal = "1234-123",
                      Periodo = "23sed",
                      IdDeclaracao = 12312,
                      DataRececaoDeclaracao = DateTime.Now,
                      Referencia = 1231231,
                      LinhaOptica = 1231231,
                      Valor = 12.5,
                      IdEmpresa = 1,
                      LocalFicheiro = "",
                  },
                  new TBL_Docs_IVA
                  {
                      Nome = "asd",
                      Morada = "asd",
                      Localidade = "qw",
                      CodigoPostal = "1234-123",
                      Periodo = "23sed",
                      IdDeclaracao = 12312,
                      DataRececaoDeclaracao = DateTime.Now,
                      Referencia = 1231231,
                      LinhaOptica = 1231231,
                      Valor = 12.5,
                      IdEmpresa = 1,
                      LocalFicheiro = "",
                  }
                  );
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var controller = new TBL_Docs_IVA_APIController(context);

                ///Funcao a Testar
                var result = await controller.DeleteTBL_Docs_IVA(empresa.ApiKey, 1);

                ///Resultados
                NoContentResult t = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, t.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Fuction_DownloadFile_IVA_API()

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

                var controller = new TBL_Docs_IVA_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/IVA.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                await controller.PostTBL_Docs_IVA(empresa.ApiKey, formFile);

                ///Funcao a Testar
                var result = await controller.DownloadFicheiro(empresa.ApiKey, 1);

                ///Resultados
                FileStreamResult t = Assert.IsType<FileStreamResult>(result);
                Assert.Equal("application/pdf", t.ContentType);
            }
        }

        [Fact]
        public async Task Test_Call_Get_All_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Docs_IVA_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Get_One_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Docs_IVA_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/8");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Update_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            IvaUpdate dados = new()
            {
                Nome = "asd",
                Morada = "asd",
                Localidade = "qw",
                CodigoPostal = "1234-123",
                Periodo = "23sed",
                IdDeclaracao = 12312,
                DataRececaoDeclaracao = DateTime.Now,
                Referencia = 1231231,
                LinhaOptica = 1231231,
                Valor = 12.5,
            };
            var json = JsonConvert.SerializeObject(dados);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ///Funcao a Testar
            var httpResponse = await client.PutAsync("/api/TBL_Docs_IVA_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/8", data);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Insert_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/IVA.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "IVA.pdf");
            ///Funcao a Testar
            var httpResponse = await client.PostAsync("/api/TBL_Docs_IVA_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Delete_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/IVA.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "IVA.pdf");

            var httpResponse = await client.PostAsync("/api/TBL_Docs_IVA_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());

            string content = await httpResponse.Content.ReadAsStringAsync();

            var certidaoSS = JsonConvert.DeserializeObject<TBL_Docs_IVA>(content);
            ///Funcao a Testar
            var httpResponseDelete = await client.DeleteAsync("/api/TBL_Docs_IVA_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/" + certidaoSS.Id);

            ///Resultados
            httpResponseDelete.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Test_Call_DownloadFile_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Docs_IVA_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/Ficheiro/8");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", httpResponse.Content.Headers.ContentType.ToString());
        }
    }
}