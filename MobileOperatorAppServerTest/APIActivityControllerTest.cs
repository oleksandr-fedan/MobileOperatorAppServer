using MobileOperatorAppServer.Controllers.API;
using MobileOperatorAppServer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorAppServerTest
{
    [TestClass]
    public class APIActivityControllerTest : BaseTest
    {
        public ActivityController controller;

        public APIActivityControllerTest()
        {
            controller = new ActivityController(Context);
        }

        [TestMethod]
        public void GetInternetActivity_ValidData()
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

            double quantity = 123;
            ActivityType type = ActivityType.INTERNET;
            var activity = new ActivityModel
            {
                User = user,
                Type = type,
                Quantity = quantity,
                Date = DateTime.Now.Date,
            };

            Context.Activities.Add(activity);
            Context.SaveChanges();

            //Act
            ICollection activities = controller.GetInternetActivity(number);
            Context.Users.Remove(user);
            Context.SaveChanges();

            //Assert
            Assert.AreEqual(1, activities.Count);
        }

        [TestMethod]
        public void GetMinutesActivity_ValidData()
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

            double quantity = 111;
            ActivityType type = ActivityType.MINUTES;
            var activity = new ActivityModel
            {
                User = user,
                Type = type,
                Quantity = quantity,
                Date = DateTime.Now.Date,
            };

            Context.Activities.Add(activity);
            Context.SaveChanges();

            //Act
            ICollection activities = controller.GetMinutesActivity(number);
            Context.Users.Remove(user);
            Context.SaveChanges();

            //Assert
            Assert.AreEqual(1, activities.Count);
        }

        [TestMethod]
        public void GetOtherMinutesActivity_ValidData()
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

            double quantity = 121;
            ActivityType type = ActivityType.OTHER_MINUTES;
            var activity = new ActivityModel
            {
                User = user,
                Type = type,
                Quantity = quantity,
                Date = DateTime.Now.Date,
            };

            Context.Activities.Add(activity);
            Context.SaveChanges();

            //Act
            ICollection activities = controller.GetOtherMinutesActivity(number);
            Context.Users.Remove(user);
            Context.SaveChanges();

            //Assert
            Assert.AreEqual(1, activities.Count);
        }

        [TestMethod]
        public void GetSMSActivity_ValidData()
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

            double quantity = 133;
            ActivityType type = ActivityType.SMS;
            var activity = new ActivityModel
            {
                User = user,
                Type = type,
                Quantity = quantity,
                Date = DateTime.Now.Date,
            };

            Context.Activities.Add(activity);
            Context.SaveChanges();

            //Act
            ICollection activities = controller.GetSMSActivity(number);
            Context.Users.Remove(user);
            Context.SaveChanges();

            //Assert
            Assert.AreEqual(1, activities.Count);
        }
    }
}
