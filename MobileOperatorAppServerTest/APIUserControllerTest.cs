using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer.Controllers.API;
using MobileOperatorAppServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MobileOperatorAppServerTest
{
    [TestClass]
    public class APIUserControllerTest : BaseTest
    {
        public UserController controller;

        public APIUserControllerTest()
        {
            controller = new UserController(Context);
        }

        [TestMethod]
        public void GetUser_ValidData()
        {
            //Arrange
            string number = "123";
            var tariff = Context.Tariffs.FirstOrDefault();
            var user = new UserModel
            {
                PhoneNumber = number,
                Name = "Test",
                Surname = "Test",
                MiddleName = "Test",
                Balance = 0,
                ConnectionDate = DateTime.Now.Date,
                Tariff = tariff
            };

            Context.Users.Add(user);
            Context.SaveChanges();

            //Act
            var result = controller.GetUser(number);

            Context.Users.Remove(user);
            Context.SaveChanges();

            //Assert
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void UpdateTariff_ValidData()
        {
            //Arrange
            string number = "123";
            var tariff = Context.Tariffs.FirstOrDefault();
            var user = new UserModel
            {
                PhoneNumber = number,
                Name = "Test",
                Surname = "Test",
                MiddleName = "Test",
                Balance = 0,
                ConnectionDate = DateTime.Now.Date,
                Tariff = tariff
            };

            var json = "{\"phoneNumber\": \"123\"}";
            var options = new JsonDocumentOptions { AllowTrailingCommas = true };
            var requestBody = JsonDocument.Parse(json, options);

            Context.Users.Add(user);
            Context.SaveChanges();

            decimal requiredBalance = user.Balance - tariff.Price;

            //Act
            controller.UpdateTariff(requestBody);

            Context.Users.Remove(user);
            Context.SaveChanges();

            //Assert
            Assert.AreEqual(requiredBalance, user.Balance);
        }

        [TestMethod]
        public void UserConnectTariff_ValidData()
        {
            //Arrange
            string number = "123";
            var tariff = new TariffModel 
            { 
                Name = "test",
                Price = 123,
                InternetQuantity = 1,
                MinutesQuantity = 1,
                OtherMinutesQuantity = 1,
                SMSQuantity = 1,
            };

            var user = new UserModel
            {
                PhoneNumber = number,
                Name = "Test",
                Surname = "Test",
                MiddleName = "Test",
                Balance = 0,
                ConnectionDate = DateTime.Now.Date,
                Tariff = Context.Tariffs.FirstOrDefault()
            };

            Context.Tariffs.Add(tariff);
            Context.Users.Add(user);
            Context.SaveChanges();

            var json = "{\"userPhoneNumber\": \"" + number + "\", \"tariffId\": " + tariff.Id +"}";
            var options = new JsonDocumentOptions { AllowTrailingCommas = true };
            var requestBody = JsonDocument.Parse(json, options);

            decimal requiredBalance = user.Balance - tariff.Price;

            //Act
            controller.UserConnectTariff(requestBody);

            //Assert
            Assert.AreEqual(requiredBalance, user.Balance);
            Assert.AreEqual(tariff, user.Tariff);

            Context.Tariffs.Remove(tariff);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        [TestMethod]
        public void UserConnectService_ValidData()
        {
            //Arrange
            string number = "123";
            var service = new ServiceModel
            {
                Name = "test",
                Price = 123,
                InternetQuantity = 1,
                MinutesQuantity = 1,
                OtherMinutesQuantity = 1,
                SMSQuantity = 1,
            };

            var user = new UserModel
            {
                PhoneNumber = number,
                Name = "Test",
                Surname = "Test",
                MiddleName = "Test",
                Balance = 0,
                ConnectionDate = DateTime.Now.Date,
                Tariff = Context.Tariffs.FirstOrDefault()
            };

            Context.Services.Add(service);
            Context.Users.Add(user);
            Context.SaveChanges();

            var json = "{\"userPhoneNumber\": \"" + number + "\", \"serviceId\": " + service.Id + "}";
            var options = new JsonDocumentOptions { AllowTrailingCommas = true };
            var requestBody = JsonDocument.Parse(json, options);

            decimal requiredBalance = user.Balance - service.Price;

            //Act
            controller.UserConnectService(requestBody);

            int countConnectedServices = Context.UserConnectedServices.Include(s => s.User).Where(s => s.User.PhoneNumber == number).Count();

            //Assert
            Assert.AreEqual(requiredBalance, user.Balance);
            Assert.AreEqual(1, countConnectedServices);

            Context.Users.Remove(user);
            Context.Services.Remove(service);
            Context.SaveChanges();
        }

        [TestMethod]
        public void Deposit_ValidData()
        {
            //Arrange
            string number = "123";
            decimal deposit = 123;

            var user = new UserModel
            {
                PhoneNumber = number,
                Name = "Test",
                Surname = "Test",
                MiddleName = "Test",
                Balance = 0,
                ConnectionDate = DateTime.Now.Date,
                Tariff = Context.Tariffs.FirstOrDefault()
            };

            Context.Users.Add(user);
            Context.SaveChanges();

            var json = "{\"phoneNumber\": \"" + number + "\", \"deposit\": " + deposit + "}";
            var options = new JsonDocumentOptions { AllowTrailingCommas = true };
            var requestBody = JsonDocument.Parse(json, options);

            decimal requiredBalance = user.Balance + deposit;

            //Act
            controller.Deposit(requestBody);

            //Assert
            Assert.AreEqual(requiredBalance, user.Balance);

            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        /*[TestMethod]
        public void GetAwailableServices_ValidData()
        {
            //Arrange
            string number = "123";

            var user = new UserModel
            {
                PhoneNumber = number,
                Name = "Test",
                Surname = "Test",
                MiddleName = "Test",
                Balance = 0,
                ConnectionDate = DateTime.Now.Date,
                Tariff = Context.Tariffs.FirstOrDefault()
            };

            Context.Users.Add(user);
            Context.SaveChanges();

            double requiredInternet = user.Tariff.InternetQuantity;
            double requiredMinutes = user.Tariff.MinutesQuantity;
            double requiredOtherMinutes = user.Tariff.OtherMinutesQuantity;
            double requiredSMS = user.Tariff.SMSQuantity;

            //Act
            var result = controller.GetAwailableServices(number);

            // Получение содержимого ответа в виде строки JSON
            OkObjectResult okResult = result as OkObjectResult;
            var availableServices = okResult.Value;

            // Использование доступных услуг в дальнейшем коде
            double availableInternet = (double)availableServices.AvailableInternet;
            double availableMinutes = (double)availableServices.AvailableMinutes;
            double availableOtherMinutes = (double)availableServices.AvailableOtherMinutes;
            double availableSMS = (double)availableServices.AvailableSMS;

            //Assert
            Assert.AreEqual(requiredInternet, availableInternet);
            Assert.AreEqual(requiredMinutes, availableMinutes);
            Assert.AreEqual(requiredOtherMinutes, availableOtherMinutes);
            Assert.AreEqual(requiredSMS, availableSMS);

            Context.Users.Remove(user);
            Context.SaveChanges();
        }*/
    }
}
