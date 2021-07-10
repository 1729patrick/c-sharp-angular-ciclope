using OpenQA.Selenium.Chrome;
using System.Threading;
using Xunit;

namespace CiclopeTest
{
    public class TBL_Divida_AT_Test
    {
        [Fact]
        public void Test_GetPaginaListagemDividaAT()
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
            driver.ExecuteScript("document.getElementById('DAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/TBL_Divida_AT/Index", driver.Url);
            driver.Dispose();
        }
    }
}
