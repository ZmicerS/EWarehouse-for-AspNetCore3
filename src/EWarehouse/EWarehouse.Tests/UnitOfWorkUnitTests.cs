using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using EWarehouse.Repository.Entities.Account;
using Microsoft.EntityFrameworkCore;
using EWarehouse.Repository;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace EWarehouse.Tests
{
    public class UnitOfWorkUnitTests
    {

        [Fact]
        public async void CreateAsyncMethodAddNewUser()
        {
            var logger = new Mock<ILogger>();
            var loggerFactory = new Mock<ILoggerFactory>();

            loggerFactory.Setup(l => l.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            var context = CreateNewDbContext();
            var unitOfWork = new UnitOfWork((ApplicationContext)context, loggerFactory.Object);

            User user = new User
            {
                Email = "email",
                Password = "password"
            };
            IEnumerable<User> usersInDatabase;

            await unitOfWork.Users.CreateAsync(user);
         
            await unitOfWork.SaveAsync();
            usersInDatabase = unitOfWork.Users.GetAll();

            Assert.Single(usersInDatabase);
            Assert.Equal(1, user.Id);
        }


        private static DbContext CreateNewDbContext()
        {
            var logger = new Mock<ILogger>();

            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(l => l.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .UseInternalServiceProvider(serviceProvider).Options;

            var context = new ApplicationContext(options, loggerFactory.Object);
            context.Database.EnsureDeleted();

            return context;
        }

    }
}
