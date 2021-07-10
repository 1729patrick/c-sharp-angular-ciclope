using OpenQA.Selenium.Chrome;
using System.Threading;
using Xunit;

namespace CiclopeTest
{
    public class TBL_Doc_BCBControllerTest
    {


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

    }
}
