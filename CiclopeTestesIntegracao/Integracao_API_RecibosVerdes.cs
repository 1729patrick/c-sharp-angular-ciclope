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
    public class Integracao_API_RecibosVerdes : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Integracao_API_RecibosVerdes(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        }

        [Fact]
        public async Task Test_Fuction_Get_All_RecibosVerdes_API()

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
                context.TBL_Doc_RecibosVerdes.AddRange(
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    },
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    }, new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    });

                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_RecibosVerdes_APIController(context);
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_RecibosVerdes(empresa.ApiKey);


                ///Resultados
                var viewResult = Assert.IsType<ActionResult<IEnumerable<TBL_Doc_RecibosVerdes>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Doc_RecibosVerdes>>(viewResult.Value);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_Fuction_Get_One_RecibosVerdes_API()

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
                context.TBL_Doc_RecibosVerdes.AddRange(
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    },
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    }, new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_RecibosVerdes_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Doc_RecibosVerdes(empresa.ApiKey, 2);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_RecibosVerdes>>(result);
                var model = Assert.IsAssignableFrom<TBL_Doc_RecibosVerdes>(viewResult.Value);
                Assert.Equal(1, model.IdEmpresa);
                Assert.Equal(123.2, model.DadosIRS);
            }
        }

        [Fact]
        public async Task Test_Fuction_Update_RecibosVerdes_API()

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
                context.TBL_Doc_RecibosVerdes.AddRange(
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    },
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    }, 
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Doc_RecibosVerdes_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();
                RecibosUpdate dados = new()
                {
                    TransmitenteNome = "123123",
                    TransmitenteAtividade = "asjajsd",
                    TransmitenteNif = "asdasd",
                    TransmitenteDomicilio = "asda",
                    AdquirenteNome = "asdasd",
                    AdquirenteMorada = "rua 4",
                    AdquirenteNif = "nss",
                    DadosDataTransmissao = DateTime.Now,
                    DadosDescricao = "123",
                    DadosValorBase = 100.9,
                    DadosIva = 100.4,
                    DadosImpostoSelo = 100.4,
                    DadosIRS = 100.2
                };



                ///Funcao a Testar
                var result = await controller.PutTBL_Doc_RecibosVerdes(empresa.ApiKey, 2, dados);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_RecibosVerdes>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_RecibosVerdes>(Response.Value);
                Assert.Equal(dados.AdquirenteMorada, model.AdquirenteMorada);
                Assert.Equal(dados.DadosValorBase, model.DadosValorBase);
                Assert.Equal(dados.DadosIva, model.DadosIva);
                Assert.Equal(dados.DadosImpostoSelo, model.DadosImpostoSelo);
                Assert.Equal(dados.DadosIRS, model.DadosIRS);
            }
        }

        [Fact]
        public async Task Test_Fuction_Insert_RecibosVerdes_API()

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
                var controller = new TBL_Doc_RecibosVerdes_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocRecibosVerdes.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                var result = await controller.PostTBL_Doc_RecibosVerdes(empresa.ApiKey, formFile);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Doc_RecibosVerdes>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Doc_RecibosVerdes>(Response.Value);
                Assert.Equal(400, model.DadosValorBase);
                Assert.Equal(92, model.DadosIva);
                Assert.Equal(100, model.DadosIRS);
            }
        }


        [Fact]
        public async Task Test_Fuction_Delete_RecibosVerdes_API()

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
                context.TBL_Doc_RecibosVerdes.AddRange(
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    },
                    new TBL_Doc_RecibosVerdes
                    {
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    }, 
                    new TBL_Doc_RecibosVerdes{
                        TransmitenteNome = "123123",
                        TransmitenteAtividade = "asjajsd",
                        TransmitenteNif = "asdasd",
                        TransmitenteDomicilio = "asda",
                        AdquirenteNome = "asdasd",
                        AdquirenteMorada = "rua 3",
                        AdquirenteNif = "nss",
                        DadosDataTransmissao = DateTime.Now,
                        DadosDescricao = "123",
                        DadosValorBase = 12.9,
                        DadosIva = 23.4,
                        DadosImpostoSelo = 12.4,
                        DadosIRS = 123.2,
                        LocalFicheiro = "/",
                        IdEmpresa = 1
                    });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var controller = new TBL_Doc_RecibosVerdes_APIController(context);

                ///Funcao a Testar
                var result = await controller.DeleteTBL_Doc_RecibosVerdes(empresa.ApiKey, 1);

                ///Resultados
                NoContentResult t = Assert.IsType<NoContentResult>(result);
                Assert.Equal(204, t.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Fuction_DownloadFile_RecibosVerdes_API()

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

                var controller = new TBL_Doc_RecibosVerdes_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocRecibosVerdes.pdf";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                await controller.PostTBL_Doc_RecibosVerdes(empresa.ApiKey, formFile);

                ///Funcao a Testar
                var result = await controller.DownloadFicheiro(empresa.ApiKey, 1);

                ///Resultados
                FileStreamResult t = Assert.IsType<FileStreamResult>(result);
                Assert.Equal("application/pdf", t.ContentType);
            }
        }

        [Fact]
        public async Task Test_Call_Get_All_RecibosVerdes_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_RecibosVerdes_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Get_One_RecibosVerdes_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_RecibosVerdes_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/19");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Update_RecibosVerdes_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            RecibosUpdate dados = new()
            {
                TransmitenteNome = "123123",
                TransmitenteAtividade = "asjajsd",
                TransmitenteNif = "asdasd",
                TransmitenteDomicilio = "asda",
                AdquirenteNome = "asdasd",
                AdquirenteMorada = "rua 4",
                AdquirenteNif = "nss",
                DadosDataTransmissao = DateTime.Now,
                DadosDescricao = "123",
                DadosValorBase = 100.9,
                DadosIva = 100.4,
                DadosImpostoSelo = 100.4,
                DadosIRS = 100.2
            };
            var json = JsonConvert.SerializeObject(dados);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ///Funcao a Testar
            var httpResponse = await client.PutAsync("/api/TBL_Doc_RecibosVerdes_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/19", data);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Insert_RecibosVerdes_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocRecibosVerdes.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "DocRecibosVerdes.pdf");
            ///Funcao a Testar
            var httpResponse = await client.PostAsync("/api/TBL_Doc_RecibosVerdes_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Delete_RecibosVerdes_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/DocRecibosVerdes.pdf";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "DocRecibosVerdes.pdf");

            var httpResponse = await client.PostAsync("/api/TBL_Doc_RecibosVerdes_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());

            string content = await httpResponse.Content.ReadAsStringAsync();

            var certidaoSS = JsonConvert.DeserializeObject<TBL_Doc_RecibosVerdes>(content);
            ///Funcao a Testar
            var httpResponseDelete = await client.DeleteAsync("/api/TBL_Doc_RecibosVerdes_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/" + certidaoSS.Id);

            ///Resultados
            httpResponseDelete.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Test_Call_DownloadFile_RecibosVerdes_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_RecibosVerdes_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/Ficheiro/19");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", httpResponse.Content.Headers.ContentType.ToString());
        }
    }
}
