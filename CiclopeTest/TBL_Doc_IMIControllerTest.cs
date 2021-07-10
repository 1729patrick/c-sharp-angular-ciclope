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
    public class TBL_Doc_IMIControllerTest
    {
        #region Unit Test
        [Fact]
        public async Task Test_GetListIMI()
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
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Doc_IMI.AddRange(
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 12.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 20.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 32.4, IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_IMIController(context, manager);

                var result = await controller.Index(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Ciclope.Models.TBL_Doc_IMI>>(
                    viewResult.ViewData.Model);
                Assert.Equal(3, model.Count());
            }
        }

        [Fact]
        public async Task Test_GetDetailsIMI()
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
                context.TBL_Empresa.Add(new Ciclope.Models.TBL_Empresa { Nome = "EmpresaTest", Nif = "999999999", Email = "EmpresaTest@gmail.com", Telefone = "919999999" });
                context.Users.Add(new CiclopeUser { Id = useridTest, UserName = "Joao", Email = "joao@ip.pt" });
                context.SaveChanges();
                context.TBL_Doc_IMI.AddRange(
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 12.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 20.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 32.4, IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_IMIController(context, manager);

                var result = await controller.Details(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Ciclope.Models.TBL_Doc_IMI>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
            }
        }

       

        [Fact]
        public async Task Test_CreateIMI()
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
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_IMIController(context, manager);

                var result = controller.Create(1);

                var viewResult = Assert.IsType<ViewResult>(result);
            }
        }


        [Fact]
        public async Task Test_UpdateIMI()
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
                context.TBL_Doc_IMI.AddRange(
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 12.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 20.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 32.4, IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_IMIController(context, manager);

                IActionResult result =  await controller.Details(1);

                ViewResult okResult = result as ViewResult;

                var viewResult = Assert.IsType<Ciclope.Models.TBL_Doc_IMI>(okResult.Model);

                viewResult.ImportanciaPagar = 100.25;

                await controller.Edit(1, viewResult);

                result = await controller.Details(1);

                okResult = result as ViewResult;

                viewResult = Assert.IsType<Ciclope.Models.TBL_Doc_IMI>(okResult.Model);

                Assert.Equal(100.25, viewResult.ImportanciaPagar);
            }
        }


        [Fact]
        public async Task Test_DeleteIMI()
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
                context.TBL_Doc_IMI.AddRange(
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 12.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 20.5, IdEmpresa = 1 },
                    new Ciclope.Models.TBL_Doc_IMI { ImportanciaPagar = 32.4, IdEmpresa = 1 });
                context.TBL_TrabalhadorUser.Add(new TBL_TrabalhadorUser { UserId = useridTest, EmpresaId = 1 });
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(options))
            {
                var manager = Utils.CriarUserManagerMock(useridTest);

                var controller = new TBL_Doc_IMIController(context, manager);

                var result = await controller.DeleteConfirmed(1);

                Assert.IsType<RedirectToActionResult>(result);
            }
        }
        #endregion

        #region Browser Test
        [Fact]
        public void Test_GetPaginaListagemIMI()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(3000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(3000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('IMI').click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_IMI/Index/", driver.Url);
            driver.Dispose();
        }
        [Fact]
        public void Test_GetPaginaIMI()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(3000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(3000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('IMI').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[1].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_IMI/Details/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaUpdateIMI()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(3000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(3000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('IMI').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[0].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_IMI/Edit/", driver.Url);
            driver.Dispose();
        }
        [Fact]
        public void Test_GetPaginaDeleteIMI()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(3000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(3000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('IMI').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[2].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_IMI/Delete/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaCreateIMI()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(3000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(3000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('IMI').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('main')[0].getElementsByTagName('a')[0].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_IMI/Create/", driver.Url);
            driver.Dispose();
        }
        #endregion
    }
}
