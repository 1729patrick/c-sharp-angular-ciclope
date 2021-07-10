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
    public class Integracao_API_Fatura : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Integracao_API_Fatura(WebApplicationFactory<Startup> factory)
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
                context.TBL_Faturas.AddRange(
                    new TBL_Faturas
                    {
                       numero = "12",
                       Descricao = "12",
                       Valor = 1845,
                       Entidade = "12312313",
                       LocalFicheiro = "",
                       EmpresaId = 1,
                       QRcode = "123123123123",
                       Data = DateTime.Now,
                       ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    }
                    );


                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Faturas_APIController(context);
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Faturas(empresa.ApiKey);


                ///Resultados
                var viewResult = Assert.IsType<ActionResult<IEnumerable<TBL_Faturas>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Faturas>>(viewResult.Value);
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
                context.TBL_Faturas.AddRange(
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    }
                    );

                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Faturas_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                ///Funcao a Testar
                var result = await controller.GetTBL_Faturas(empresa.ApiKey, 2);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Faturas>>(result);
                var model = Assert.IsAssignableFrom<TBL_Faturas>(viewResult.Value);
                Assert.Equal(1, model.EmpresaId);
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
                context.TBL_Faturas.AddRange(
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    }
                    );

                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new TBL_Faturas_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();
                FaturaUpdate dados = new()
                {
                    Numero = "12",
                    Descricao = "12",
                    Valor = 1845,
                    Entidade = "12312313",
                    Data = DateTime.Now,
                    ValorIva = 12.43
                };



                ///Funcao a Testar
                var result = await controller.PutTBL_Faturas(empresa.ApiKey, 2, dados);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Faturas>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Faturas>(Response.Value);
                Assert.Equal(dados.Numero, model.numero);
                Assert.Equal(dados.Descricao, model.Descricao);
                Assert.Equal(dados.Valor, model.Valor);
                Assert.Equal(dados.Entidade, model.Entidade);
                Assert.Equal(dados.ValorIva, model.ValorIva);
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
                var controller = new TBL_Faturas_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/fatura.jpg";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                ///Funcao a Testar
                var result = await controller.PostTBL_Faturas(empresa.ApiKey, formFile);

                ///Resultados
                var viewResult = Assert.IsType<ActionResult<TBL_Faturas>>(result);
                var Response = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
                var model = Assert.IsAssignableFrom<TBL_Faturas>(Response.Value);
                Assert.Equal(1845, model.Valor);

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
                context.TBL_Faturas.AddRange(
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    },
                    new TBL_Faturas
                    {
                        numero = "12",
                        Descricao = "12",
                        Valor = 1845,
                        Entidade = "12312313",
                        LocalFicheiro = "",
                        EmpresaId = 1,
                        QRcode = "123123123123",
                        Data = DateTime.Now,
                        ValorIva = 12.43
                    }
                    );

                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var controller = new TBL_Faturas_APIController(context);

                ///Funcao a Testar
                var result = await controller.DeleteTBL_Faturas(empresa.ApiKey, 1);

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

                var controller = new TBL_Faturas_APIController(context);

                var empresa = await context.TBL_Empresa.FirstOrDefaultAsync();

                var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/fatura.jpg";

                using var stream = new MemoryStream(File.ReadAllBytes(caminho).ToArray());
                var formFile = new FormFile(stream, 0, stream.Length, "streamFile", caminho.Split(@"\").Last());

                await controller.PostTBL_Faturas(empresa.ApiKey, formFile);

                ///Funcao a Testar
                var result = await controller.DownloadCertificado(empresa.ApiKey, 1);

                ///Resultados
                FileStreamResult t = Assert.IsType<FileStreamResult>(result);
                Assert.Equal("image/jpeg", t.ContentType);
            }
        }

        [Fact]
        public async Task Test_Call_Get_All_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Faturas_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

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
            var httpResponse = await client.GetAsync("/api/TBL_Faturas_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/31");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Update_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            FaturaUpdate dados = new()
            {
                Numero = "12",
                Descricao = "12",
                Valor = 1845,
                Entidade = "12312313",
                Data = DateTime.Now,
                ValorIva = 12.43
            };
            var json = JsonConvert.SerializeObject(dados);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            ///Funcao a Testar
            var httpResponse = await client.PutAsync("/api/TBL_Faturas_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/31", data);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Insert_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/fatura.jpg";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "fatura.jpg");
            ///Funcao a Testar
            var httpResponse = await client.PostAsync("/api/TBL_Faturas_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_Call_Delete_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();

            var caminho = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Ficheiros/fatura.jpg";

            var bytes = File.ReadAllBytes(caminho).ToArray();


            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "fatura.jpg");

            var httpResponse = await client.PostAsync("/api/TBL_Faturas_API/31A385D3-5D25-44E8-88F3-839DB1948EB0", form);

            httpResponse.EnsureSuccessStatusCode();

            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());

            string content = await httpResponse.Content.ReadAsStringAsync();

            var certidaoSS = JsonConvert.DeserializeObject<TBL_Faturas>(content);
            ///Funcao a Testar
            var httpResponseDelete = await client.DeleteAsync("/api/TBL_Faturas_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/" + certidaoSS.Id);

            ///Resultados
            httpResponseDelete.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Test_Call_DownloadFile_IVA_API()
        {
            ///Inicialização
            var client = _factory.CreateClient();
            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Faturas_API/31A385D3-5D25-44E8-88F3-839DB1948EB0/Ficheiro/31");
            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("image/jpeg", httpResponse.Content.Headers.ContentType.ToString());
        }
    }
}