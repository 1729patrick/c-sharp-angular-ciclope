﻿using Ciclope.Controllers;
using Ciclope.Data;
using Ciclope.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CiclopeTest
{
    public class TBL_FaturasControllerTest
    {
        #region Unit Test
        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;


            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Faturas.AddRange(
                    new Ciclope.Models.TBL_Faturas { Valor = 12.5, EmpresaId = 1, numero = "1" },
                    new Ciclope.Models.TBL_Faturas { Valor = 20.5, EmpresaId = 1, numero = "2" },
                    new Ciclope.Models.TBL_Faturas { Valor = 32.4, EmpresaId = 1, numero = "3" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_FaturasController(context, manager);

                var result = await controller.Index(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Ciclope.Models.TBL_Faturas>>(
                    viewResult.ViewData.Model);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_WhenFaturaDoesntExist()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_FaturasController(context, manager);

                var result = await controller.Details(1);

                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_FaturasController(context, manager);

                var result = await controller.Details(null);

                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Details_RetunrsViewResult_WhenFaturaExists()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            var useridTest = Guid.NewGuid().ToString();
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Faturas.AddRange(
                    new Ciclope.Models.TBL_Faturas { Valor = 12.5, EmpresaId = 1, numero = "1" },
                    new Ciclope.Models.TBL_Faturas { Valor = 20.5, EmpresaId = 1, numero = "2" },
                    new Ciclope.Models.TBL_Faturas { Valor = 32.4, EmpresaId = 1, numero = "3" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_FaturasController(context, manager);

                var result = await controller.Details(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Ciclope.Models.TBL_Faturas>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
            }
        }

        [Fact]
        public async Task Test_UpdateFatura()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Faturas.AddRange(
                    new Ciclope.Models.TBL_Faturas { Valor = 12.5, EmpresaId = 1, numero = "1" },
                    new Ciclope.Models.TBL_Faturas { Valor = 20.5, EmpresaId = 1, numero = "2" },
                    new Ciclope.Models.TBL_Faturas { Valor = 32.4, EmpresaId = 1, numero = "3" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_FaturasController(context, manager);

                IActionResult result = await controller.Details(1);

                ViewResult okResult = result as ViewResult;

                var viewResult = Assert.IsType<TBL_Faturas>(okResult.Model);

                viewResult.Valor = 100.25;

                await controller.Edit(1, viewResult);

                result = await controller.Details(1);

                okResult = result as ViewResult;

                viewResult = Assert.IsType<TBL_Faturas>(okResult.Model);

                Assert.Equal(100.25, viewResult.Valor);
            }
        }

        [Fact]
        public async Task Test_DeleteFatura()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Faturas.AddRange(
                    new Ciclope.Models.TBL_Faturas { Valor = 12.5, EmpresaId = 1, numero = "1" },
                    new Ciclope.Models.TBL_Faturas { Valor = 20.5, EmpresaId = 1, numero = "2" },
                    new Ciclope.Models.TBL_Faturas { Valor = 32.4, EmpresaId = 1, numero = "3" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_FaturasController(context, manager);

                var result = await controller.DeleteConfirmed(1);

                Assert.IsType<RedirectToActionResult>(result);
            }
        }

        [Fact]
        public async Task Test_CreateFatura()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_FaturasController(context, manager);

                var result = controller.Create(1);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        #endregion

        #region Browser Test
        [Fact]
        public void Test_GetPaginaListagemFatura()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('Fatura').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Faturas", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaDetailsFatura()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('Fatura').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[1].click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Faturas/Details/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaUpdateFatura()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('Fatura').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[0].click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Faturas/Edit/", driver.Url);
            driver.Dispose();
        }
        [Fact]
        public void Test_GetPaginaDeleteFatura()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('Fatura').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[2].click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Faturas/Delete/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaCreateFatura()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('Fatura').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('main')[0].getElementsByTagName('a')[0].click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Faturas/Create", driver.Url);
            driver.Dispose();
        }
        #endregion


    }
}
