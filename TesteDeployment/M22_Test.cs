﻿using OpenQA.Selenium.Chrome;
using System.Threading;
using Xunit;

namespace CiclopeTest
{
    public class M22_Test
    {

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
            driver.ExecuteScript("document.getElementById('M22').click()");
            Thread.Sleep(1000);
            Assert.Contains("https://ciclope.azurewebsites.net/M22/Index", driver.Url);
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
            driver.ExecuteScript("document.getElementById('M22').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('details').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/M22/Details/", driver.Url);
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
            driver.ExecuteScript("document.getElementById('M22').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('edit').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/M22/Edit/", driver.Url);
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
            driver.ExecuteScript("document.getElementById('M22').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('delete').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/M22/Delete/", driver.Url);
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
            driver.ExecuteScript("document.getElementById('M22').click()");
            Thread.Sleep(1000);
            driver.ExecuteScript("document.getElementById('create').click()");
            Assert.Contains("https://ciclope.azurewebsites.net/M22/Create", driver.Url);
            driver.Dispose();
        }


    }
}