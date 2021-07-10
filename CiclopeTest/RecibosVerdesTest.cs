﻿using Ciclope.Controllers;
using Ciclope.Data;
using Ciclope.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class RecibosVerdesTest
    {

        #region Unit Test
        [Fact]
        public async Task Test_GetList()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Doc_RecibosVerdes.AddRange(
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);
                var controller = new RecibosVerdesController(context, manager);

                var result = await controller.Index(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Doc_RecibosVerdes>>(viewResult.ViewData.Model);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_GetDetails()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Doc_RecibosVerdes.AddRange(
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);
                var controller = new RecibosVerdesController(context, manager);

                var result = await controller.Details(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<TBL_Doc_RecibosVerdes>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
            }
        }

        [Fact]
        public async Task Test_Update()
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
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Doc_RecibosVerdes.AddRange(
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);
                var controller = new RecibosVerdesController(context, manager);

                IActionResult result = await controller.Details(1);

                ViewResult okResult = result as ViewResult;

                var viewResult = Assert.IsType<Ciclope.Models.TBL_Doc_RecibosVerdes>(okResult.Model);

                viewResult.DadosIRS = 21.2;

                await controller.Edit(1, viewResult);

                result = await controller.Details(1);

                okResult = result as ViewResult;

                viewResult = Assert.IsType<Ciclope.Models.TBL_Doc_RecibosVerdes>(okResult.Model);

                Assert.Equal(21.2, viewResult.DadosIRS);
            }
        }


        [Fact]
        public async Task Test_Delete()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Doc_RecibosVerdes.AddRange(
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" },
                            new TBL_Doc_RecibosVerdes { DadosIRS = 20.5, IdEmpresa = 1, LocalFicheiro = "/" });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);
                var controller = new RecibosVerdesController(context, manager);

                var result = await controller.DeleteConfirmed(1);

                Assert.IsType<RedirectToActionResult>(result);
            }
        }

        [Fact]
        public async Task Test_Create()
        {
            var useridTest = Guid.NewGuid().ToString();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);
                var controller = new RecibosVerdesController(context, manager);

                var result = controller.Create(1);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        #endregion

        #region Browser Test
        [Fact]
        public void Test_GetPaginaListagem()
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
            driver.ExecuteScript("document.getElementById('Verdes').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/RecibosVerdes/Index", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaDetails()
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
            driver.ExecuteScript("document.getElementById('Verdes').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('details').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/RecibosVerdes/Details/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaUpdate()
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
            driver.ExecuteScript("document.getElementById('Verdes').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('edit').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/RecibosVerdes/Edit/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaDelete()
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
            driver.ExecuteScript("document.getElementById('Verdes').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('delete').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/RecibosVerdes/Delete/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaCreate()
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
            driver.ExecuteScript("document.getElementById('Verdes').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('create').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/RecibosVerdes/Create", driver.Url);
            driver.Dispose();
        }
        #endregion

    }
}