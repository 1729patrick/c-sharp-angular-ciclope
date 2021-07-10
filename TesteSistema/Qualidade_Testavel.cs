using Ciclope;
using Microsoft.AspNetCore.Mvc.Testing;
using OpenQA.Selenium.Chrome;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TesteSistema
{
    public class Qualidade_Testavel : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public Qualidade_Testavel(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        }



        [Fact]
        public void Test_RQ1()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            Assert.Equal("Erro", driver.FindElementByTagName("h1").Text);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ2()
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
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            Assert.Equal("1729patrick@gmail.com!", driver.FindElementsByTagName("a")[1].Text);
            driver.Dispose();
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ3()
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
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ4()
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
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ5()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ6()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[2].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ8()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            Assert.Equal("Erro", driver.FindElementByTagName("h1").Text);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ13()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            Assert.Equal("Erro", driver.FindElementByTagName("h1").Text);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ14()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            Assert.Equal("Erro", driver.FindElementByTagName("h1").Text);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ15()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://www.google.com";
            Thread.Sleep(3000);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ23()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            Assert.InRange(1000, 0, 30000);
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[2].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            Assert.InRange(1000, 0, 30000);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ24()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            Assert.InRange(1000, 0, 30000);
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[2].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            Assert.InRange(1000, 0, 30000);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ25()
        {
            var t1 = DateTime.Now;
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net/123234234";
            driver.FindElementsByTagName("a")[3].Click();
            var inputs = driver.FindElementsByTagName("input");
            var rand = new Random();
            inputs[0].SendKeys("Testeuser" + rand.Next(1000, 9999) + "@gmail.com");
            inputs[1].SendKeys("Testeuser123!");
            inputs[2].SendKeys("Testeuser123!");
            inputs[3].SendKeys("999999999");
            inputs[4].SendKeys("999999999");
            var t2 = DateTime.Now;
            Assert.InRange((t2-t1).Milliseconds, 0, 5000);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ32()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public void Test_RQ34()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://ciclope.azurewebsites.net";
            Thread.Sleep(1000);
            driver.Url = "https://ciclope.azurewebsites.net/Identity/Account/Login";
            Thread.Sleep(1000);
            driver.FindElementById("Input_Email").SendKeys("1729patrick@gmail.com");
            driver.FindElementById("Input_Password").SendKeys("Qwerty123!");
            driver.ExecuteScript("document.getElementsByTagName('Button')[1].click()");
            Thread.Sleep(3000);
            driver.ExecuteScript("document.getElementsByTagName('a')[4].click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('CAT').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/CertidaoAT/Index", driver.Url);
            driver.Dispose();
        }

        [Fact]
        public async Task Test_RQ36()
        {
            ///Inicializa��o
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_IMI_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }


        [Fact]
        public async Task Test_RQ37()
        {
            ///Inicializa��o
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_IMI_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_RQ38()
        {
            ///Inicializa��o
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_IMI_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_RQ40()
        {
            ///Inicializa��o
            var client = _factory.CreateClient();

            ///Funcao a Testar
            var httpResponse = await client.GetAsync("/api/TBL_Doc_IMI_API/31A385D3-5D25-44E8-88F3-839DB1948EB0");

            ///Resultados
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", httpResponse.Content.Headers.ContentType.ToString());
        }

    }
}
