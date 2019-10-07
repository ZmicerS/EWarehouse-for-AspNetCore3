using AutoMapper;
using EWarehouse.Repository.Contracts;
using EWarehouse.Repository.Entities.Account;
using EWarehouse.Services;
using EWarehouse.Services.Entities.AccountModels;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EWarehouse.Tests
{
    public class AccountServiceUnitTests
    {
        [Fact]
        public async Task AccountServiceShouldRegisterNewUser()
        {
            var registerServiceModel = new RegisterServiceModel
            {
                Email = "test3@test3.com",
                Password = "test3",
            };

            var unitOfWork = new Mock<IUnitOfWork>();

            unitOfWork.Setup(u => u.Users.CreateAsync(It.IsAny<User>()));
            unitOfWork.Setup(u => u.SaveAsync());

            var mapper = new Mock<IMapper>();
            var logger = new Mock<ILogger<AccountService>>();

            var service = new AccountService(unitOfWork.Object, mapper.Object, logger.Object);

            await service.RegisterUserAsync(registerServiceModel);

            unitOfWork.Verify(u => u.Users.CreateAsync(It.IsAny<User>()), Times.Once, ".Users.CreateAsync is fail");
            unitOfWork.Verify(u => u.SaveAsync(), Times.Once, "SaveAsync is fail");
        }
    }
}
