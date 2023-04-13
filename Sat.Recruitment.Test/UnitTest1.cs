using System;
using System.Collections.Generic;
using System.Dynamic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Controllers;
using Moq;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Enums;
using Xunit;
using Sat.Recruitment.Api.Interfaces;
using System.Net;
using System.Security.Policy;
using System.Xml.Linq;
using System.Numerics;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
    private IFileReaderService _fileReaderService;
    private IUserService _serviceNonExisting;
    private IUserService _serviceExisting;

    public UnitTest1()
    {
      var users = new List<User>();
      var firstUserTest = new User
      {
        Name = "Marlon",
        Email = "Marlon@gmail.com",
        Address = "Av. Juan G",
        Phone = "+349 1122354215",
        UserType = UserTypeEnum.Normal,
        Money = 124
      };
      users.Add(firstUserTest);
      var secondUserTest = new User
      {
        Name = "Agustina",
        Email = "Agustina@gmail.com",
        Address = "Av. Juan G",
        Phone = "+349 1122354215",
        UserType = UserTypeEnum.SuperUser,
        Money = 124
      };
      users.Add(secondUserTest);

      var user = new User
      {
        Name = "Mike",
        Email = "mike@gmail.com",
        Address = "Av. Juan G",
        Phone = "+349 1122354215",
        UserType = UserTypeEnum.Normal,
        Money = 124
      };

      var serviceNonExistingUser = new Mock<IUserService>();
      serviceNonExistingUser.Setup(x => x.GetGiftByUserType(UserTypeEnum.Normal, 124)).Returns(3);
      serviceNonExistingUser.Setup(x => x.NormalizedEmail("mike@gmail.com")).Returns("mike@gmail.com");
      serviceNonExistingUser.Setup(x => x.ValidateErrors(user)).Returns("");
      _serviceNonExisting = serviceNonExistingUser.Object;


      var serviceExistingUser = new Mock<IUserService>();
      serviceExistingUser.Setup(x => x.GetGiftByUserType(UserTypeEnum.SuperUser, 124)).Returns(20);
      serviceExistingUser.Setup(x => x.NormalizedEmail("Agustina@gmail.com")).Returns("Agustina@gmail.com");
      serviceExistingUser.Setup(x => x.ValidateErrors(secondUserTest)).Returns("");
      _serviceExisting = serviceExistingUser.Object;

      var serviceReader = new Mock<IFileReaderService>();
      serviceReader.Setup(s => s.GetUsers()).Returns(users);
      _fileReaderService = serviceReader.Object;
    }

        [Fact]
        public void CreateUser()
        {
            var userController = new UsersController(_fileReaderService, _serviceNonExisting);
            var user = new User
            {
              Name = "Mike",
              Email = "mike@gmail.com",
              Address = "Av. Juan G",
              Phone = "+349 1122354215",
              UserType = UserTypeEnum.Normal,
              Money = 124
            };

            var result = userController.CreateUser(user).Result;


            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Message);
        }

        [Fact]
        public void ValidateUserDuplicate()
        {
          var userController = new UsersController(_fileReaderService, _serviceExisting);

          var user = new User
          {
            Name = "Agustina",
            Email = "Agustina@gmail.com",
            Address = "Av. Juan G",
            Phone = "+349 1122354215",
            UserType = UserTypeEnum.SuperUser,
            Money = 124
          };
          var result = userController.CreateUser(user).Result;


            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Message);
        }



        
          [Fact]
        public void Test3()
        {
         var users=  _fileReaderService.GetUsers();

         Assert.NotEmpty(users);
        }


  }
}
