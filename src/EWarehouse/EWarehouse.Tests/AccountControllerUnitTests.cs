using EWarehouse.Services.Entities.AccountModels;
using EWarehouse.Web.Models.Account;
using Moq;
using Xunit;
using EWarehouse.Services.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using EWarehouse.Web.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EWarehouse.Tests
{
    public class AccountControllerUnitTests
    {
        [Fact]
        public async Task AccountControllerShouldRegisterNewUser()
        {
            var registerViewModel = new RegisterViewModel
            {
                Email = "test3@test3.com",
                Password = "test3",
                ConfirmPassword = "test3"
            };

            var registerServiceModel = new RegisterServiceModel
            {
                Email = "test3@test3.com",
                Password = "test3",
            };

            var accountServiceModel = new AccountServiceModel
            {
                Email = "test3@test3.com",
                Password = "test3"
            };

            var service = new Mock<IAccountService>();
            service.Setup(u => u.FindUserAsync(It.IsAny<AccountServiceModel>())).ReturnsAsync(false);

            service.Setup(m => m.RegisterUserAsync(registerServiceModel));

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<AccountServiceModel>(registerViewModel)).Returns(accountServiceModel);
            mapper.Setup(m => m.Map<RegisterServiceModel>(registerViewModel)).Returns(registerServiceModel);

            var authorizationService = new Mock<IAuthorizationService>();
            var logger = new Mock<ILogger<AccountController>>();


            var controller = new AccountController(mapper.Object, service.Object, authorizationService.Object, logger.Object, null);
            var result = await controller.Register(registerViewModel);

            service.Verify(u => u.RegisterUserAsync(registerServiceModel), Times.Once, "fail");
            Assert.IsType<OkResult>(result);

        }
    }
}
