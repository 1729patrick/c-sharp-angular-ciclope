using OpenQA.Selenium.Chrome;
using System.Threading;
using Xunit;

namespace CiclopeTest
{
    public class TBL_Doc_FundosCompensacaoControllerTest
    {

        [Fact]
        public void Test_GetPaginaListagemFundosCompensacao()
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
            driver.ExecuteScript("document.getElementById('Fundos').click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_FundosCompensacao/Index/", driver.Url);
            driver.Dispose();
        }
        [Fact]
        public void Test_GetPaginaFundosCompensacao()
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
            driver.ExecuteScript("document.getElementById('Fundos').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[1].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_FundosCompensacao/Details/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaUpdateFundosCompensacao()
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
            driver.ExecuteScript("document.getElementById('Fundos').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[0].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_FundosCompensacao/Edit/", driver.Url);
            driver.Dispose();
        }
        [Fact]
        public void Test_GetPaginaDeleteFundosCompensacao()
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
            driver.ExecuteScript("document.getElementById('Fundos').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('table')[0].getElementsByTagName('a')[2].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_FundosCompensacao/Delete/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_GetPaginaCreateFundosCompensacao()
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
            driver.ExecuteScript("document.getElementById('Fundos').click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('main')[0].getElementsByTagName('a')[0].click()");
            Thread.Sleep(3000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Doc_FundosCompensacao/Create/", driver.Url);
            driver.Dispose();
        }
    }
}
