using MobileOperatorAppServer.Controllers.API;
using MobileOperatorAppServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorAppServerTest
{
    [TestClass]
    public class APIServiceControllerTest : BaseTest
    {
        public ServiceController controller;

        public APIServiceControllerTest()
        {
            controller = new ServiceController(Context);
        }

        [TestMethod]
        public void GetServices_RealCount()
        {
            Assert.AreEqual(Context.Services.Count(), controller.GetServices().Count);
        }

        [TestMethod]
        public void GetUserServices_RealCount() 
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

            var service = new ServiceModel
            {
                Name = "Test1",
                Price = 123,
                InternetQuantity = 1,
            };

            var connectedService1 = new UserConnectedServicesModel
            {
                User = user,
                Service = service,
                ConnectionDate = DateTime.Now.Date,
            };

            var connectedService2 = new UserConnectedServicesModel
            {
                User = user,
                Service = service,
                ConnectionDate = DateTime.Now.Date,
            };

            Context.Users.Add(user);
            Context.Services.Add(service);
            Context.UserConnectedServices.Add(connectedService1);
            Context.UserConnectedServices.Add(connectedService2);
            Context.SaveChanges();

            //Act
            int userConnectedServicesCount = controller.GetUserServices(number).Count;
            
            //Assert
            Assert.AreEqual(2, userConnectedServicesCount);

            Context.UserConnectedServices.Remove(connectedService1);
            Context.UserConnectedServices.Remove(connectedService2);
            Context.Services.Remove(service);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }
    }
}
