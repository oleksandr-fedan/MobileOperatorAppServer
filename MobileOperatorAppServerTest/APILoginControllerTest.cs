using MobileOperatorAppServer.Controllers.API;
using MobileOperatorAppServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileOperatorAppServer.Models;
using Microsoft.EntityFrameworkCore;

namespace MobileOperatorAppServerTest
{
    [TestClass]
    public class APILoginControllerTest : BaseTest
    {
        public LoginController controller;

        public APILoginControllerTest()
        {
            controller = new LoginController(Context);
        }

        [TestMethod]
        public void CheckEnteredPhoneNumber_ValidPhoneNumber()
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
            string result = controller.CheckEnteredPhoneNumber(number);

            //Assert
            Assert.AreNotEqual("-1", result);

            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == number).Id;
            
            var userCodesToRemove = Context.UserCodes.Include(c => c.User).Where(c => c.User.Id == userId).ToList();
            Context.UserCodes.RemoveRange(userCodesToRemove);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        [TestMethod]
        public void CheckEnteredPhoneNumber_InvalidPhoneNumber()
        {
            //Arrange
            string number = "123";
            string invalidNumber = "";
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
            string result = controller.CheckEnteredPhoneNumber(invalidNumber);

            //Assert
            Assert.AreEqual("-1", result);

            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == number).Id;

            var userCodesToRemove = Context.UserCodes.Include(c => c.User).Where(c => c.User.Id == userId).ToList();
            Context.UserCodes.RemoveRange(userCodesToRemove);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        [TestMethod]
        public void CheckEnteredCode_ValidData()
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
            string code = controller.CheckEnteredPhoneNumber(number);
            bool result = controller.CheckEnteredCode(number, code);

            //Assert
            Assert.AreEqual(true, result);

            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == number).Id;

            var userCodesToRemove = Context.UserCodes.Include(c => c.User).Where(c => c.User.Id == userId).ToList();
            Context.UserCodes.RemoveRange(userCodesToRemove);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        [TestMethod]
        public void CheckEnteredCode_InvalidCode()
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
            string code = controller.CheckEnteredPhoneNumber(number);
            bool result = controller.CheckEnteredCode(number, "");

            //Assert
            Assert.AreEqual(false, result);

            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == number).Id;

            var userCodesToRemove = Context.UserCodes.Include(c => c.User).Where(c => c.User.Id == userId).ToList();
            Context.UserCodes.RemoveRange(userCodesToRemove);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        [TestMethod]
        public void CheckEnteredCode_InvalidNumber()
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
            string code = controller.CheckEnteredPhoneNumber(number);
            bool result = controller.CheckEnteredCode("", code);

            //Assert
            Assert.AreEqual(false, result);

            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == number).Id;

            var userCodesToRemove = Context.UserCodes.Include(c => c.User).Where(c => c.User.Id == userId).ToList();
            Context.UserCodes.RemoveRange(userCodesToRemove);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }
    }
}
