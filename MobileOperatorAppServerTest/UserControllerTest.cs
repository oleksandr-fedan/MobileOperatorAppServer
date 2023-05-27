using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer;
using MobileOperatorAppServer.Controllers;
using MobileOperatorAppServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorAppServerTest
{
    [TestClass]
    public class UserControllerTest : BaseTest
    {
        public UserController controller;

        public UserControllerTest()
        {
            controller = new UserController(Context);
        }

        [TestMethod]
        public void CreateUser_ValidPhoneNumber()
        {
            //Arrange
            string phoneNumber = "123456789";
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middleName";
            int tariffId = Context.Tariffs.FirstOrDefault().Id;

            //Act
            controller.Create(name, surname, middleName, phoneNumber, tariffId);

            var user = Context.Users.Include(u => u.Tariff).FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user != null)
            {
                Context.Users.Remove(user);
                Context.SaveChanges();
            }

            //Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(phoneNumber, user.PhoneNumber);
            Assert.AreEqual(name, user.Name);
            Assert.AreEqual(surname, user.Surname);
            Assert.AreEqual(middleName, user.MiddleName);
            Assert.AreEqual(tariffId, user.Tariff.Id);
        }

        [TestMethod]
        public void CreateUser_InvalidPhoneNumber()
        {
            //Arrange
            string phoneNumber = Context.Users.FirstOrDefault().PhoneNumber.ToString();
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middleName";
            int tariffId = Context.Tariffs.FirstOrDefault().Id;

            //Act + Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Create(name, surname, middleName, phoneNumber, tariffId));
        }

        [TestMethod]
        public void UpdateUser_ValidPhoneNumber()
        {
            //Arrange
            string phoneNumber = "123456789";
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middleName";
            string newPhoneNumber = "123456781";
            string newName = "test_name1";
            string newSurname = "test_surname1";
            string newMiddleName = "test_middleName1";
            int tariffId = Context.Tariffs.FirstOrDefault().Id;
            controller.Create(name, surname, middleName, phoneNumber, tariffId);

            //Act
            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber).Id;

            controller.Update(userId, newPhoneNumber, newName, newSurname, newMiddleName);
            var user = Context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                Context.Users.Remove(user);
                Context.SaveChanges();
            }

            //Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(newPhoneNumber, user.PhoneNumber);
            Assert.AreEqual(newName, user.Name);
            Assert.AreEqual(newSurname, user.Surname);
            Assert.AreEqual(newMiddleName, user.MiddleName);
            Assert.AreEqual(tariffId, user.Tariff.Id);
        }

        [TestMethod]
        public void UpdateUser_InvalidPhoneNumber()
        {
            //Arrange
            string phoneNumber = "123456789";
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middleName";
            string newPhoneNumber = Context.Users.FirstOrDefault().PhoneNumber.ToString();
            int tariffId = Context.Tariffs.FirstOrDefault().Id;
            controller.Create(name, surname, middleName, phoneNumber, tariffId);

            //Act
            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber).Id;

            //Act + Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Update(userId, newPhoneNumber, name, surname, middleName));

            var user = Context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                Context.Users.Remove(user);
                Context.SaveChanges();
            }
        }

        [TestMethod]
        public void DeleteUser_ValidData()
        {
            //Arrange
            string phoneNumber = "123456789";
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middleName";
            int tariffId = Context.Tariffs.FirstOrDefault().Id;
            controller.Create(name, surname, middleName, phoneNumber, tariffId);
            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber).Id;

            //Act
            controller.Delete(userId);
            var user = Context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            //Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void AddActivity_ValidData()
        {
            //Arrange
            string phoneNumber = "123456789";
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middleName";
            string quantity = "1";
            ActivityType type = ActivityType.INTERNET;
            int tariffId = Context.Tariffs.FirstOrDefault().Id;
            controller.Create(name, surname, middleName, phoneNumber, tariffId);
            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber).Id;

            //Act
            controller.AddActivity(userId, quantity, type);
            var activity = Context.Activities.Include(a => a.User).FirstOrDefault(a => a.User.Id == userId);
            controller.Delete(userId);

            //Assert
            Assert.IsNotNull(activity);
            Assert.AreEqual(quantity, activity.Quantity.ToString());
            Assert.AreEqual(type, activity.Type);
        }

        [TestMethod]
        public void AddActivity_InvalidData()
        {
            //Arrange
            string phoneNumber = "123456789";
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middleName";
            string quantity = "1000000";
            ActivityType type = ActivityType.INTERNET;
            int tariffId = Context.Tariffs.FirstOrDefault().Id;
            controller.Create(name, surname, middleName, phoneNumber, tariffId);
            int userId = Context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber).Id;
            controller.AddActivity(userId, quantity, type);

            //Act + Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.AddActivity(userId, quantity, type));

            controller.Delete(userId);
        }
    }
}
