using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace CiclopeTest
{
    public class UnitTest1
    {
        [Fact]
        public void DriverWorking()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://www.google.com";
            Thread.Sleep(3000);
            driver.Dispose();
        }

       
    }
}
