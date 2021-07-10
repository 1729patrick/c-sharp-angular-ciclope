using Ciclope.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiclopeTest
{
    public static class Utils
    {

        public static UserManager<CiclopeUser> CriarUserManagerMock(string useridTest)
        {
            var mockUserManager = new Mock<UserManager<CiclopeUser>>(new Mock<IUserStore<CiclopeUser>>().Object,
               new Mock<IOptions<IdentityOptions>>().Object,
               new Mock<IPasswordHasher<CiclopeUser>>().Object,
               new IUserValidator<CiclopeUser>[0],
               new IPasswordValidator<CiclopeUser>[0],
               new Mock<ILookupNormalizer>().Object,
               new Mock<IdentityErrorDescriber>().Object,
               new Mock<IServiceProvider>().Object,
               new Mock<ILogger<UserManager<CiclopeUser>>>().Object);
         
            mockUserManager.Setup(u => u.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
            .Returns((string userId) => useridTest);

            return mockUserManager.Object;
        }






    }
}
