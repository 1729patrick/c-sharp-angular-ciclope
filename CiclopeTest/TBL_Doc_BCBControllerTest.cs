using Ciclope.Controllers;
using Ciclope.Data;
using Ciclope.Models;
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
    public class TBL_Doc_BCBControllerTest
    {
        #region Unit Test
        [Fact]
        public async Task Test_GetListBCB()
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
                context.TBL_Doc_BCB.AddRange(
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_BCBController(context, manager);

                var result = await controller.Index(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<TBL_Doc_BCB>>(viewResult.ViewData.Model);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_GetDetailsBCB()
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
                context.TBL_Doc_BCB.AddRange(
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_BCBController(context, manager);

                var result = await controller.Details(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<TBL_Doc_BCB>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
            }
        }

        [Fact]
        public async Task Test_UpdateBCB()
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
                context.TBL_Doc_BCB.AddRange(
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_BCBController(context, manager);

                IActionResult result = await controller.Details(1);

                ViewResult okResult = result as ViewResult;

                var viewResult = Assert.IsType<TBL_Doc_BCB>(okResult.Model);

                viewResult.LocalFicheiro = "B";

                await controller.Edit(1, viewResult);

                result = await controller.Details(1);

                okResult = result as ViewResult;

                viewResult = Assert.IsType<TBL_Doc_BCB>(okResult.Model);

                Assert.Equal("B", viewResult.LocalFicheiro);
            }
        }

        [Fact]
        public async Task Test_DeleteBCB()
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
                context.TBL_Doc_BCB.AddRange(
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 },
                    new TBL_Doc_BCB { IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_BCBController(context, manager);
                var result = await controller.DeleteConfirmed(1);
                Assert.IsType<RedirectToActionResult>(result);
            }
        }

        [Fact]
        public async Task Test_CreateBCB()
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

                var controller = new TBL_Doc_BCBController(context, manager);
                var result = controller.Create(1);
                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }
        #endregion

        #region Browser Test
        [Fact]
        public void Test_GetPaginaListagemBCB()
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
            driver.ExecuteScript("document.getElementById('BCB').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_BCB", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaDetailsBCB()
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
            driver.ExecuteScript("document.getElementById('BCB').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('details').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_BCB/Details/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaUpdateBCB()
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
            driver.ExecuteScript("document.getElementById('BCB').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementById('edit').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_BCB/Edit/", driver.Url);
            driver.Dispose();
        }
        [Fact]
        public void Test_GetPaginaDeleteBCB()
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
            driver.ExecuteScript("document.getElementById('BCB').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementById('delete').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_BCB/Delete/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaCreateBCB()
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
            driver.ExecuteScript("document.getElementById('BCB').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('main')[0].getElementsByTagName('Button')[0].click()");
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_BCB/Create", driver.Url);
            driver.Dispose();
        }
        #endregion
    }
}
