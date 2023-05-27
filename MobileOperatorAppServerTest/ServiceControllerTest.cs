using MobileOperatorAppServer;
using MobileOperatorAppServer.Controllers;
using MobileOperatorAppServer.Models;

namespace MobileOperatorAppServerTest
{
    [TestClass]
    public class ServiceControllerTest : BaseTest
    {
        public ServiceController controller;

        public ServiceControllerTest()
        {
            controller = new ServiceController(Context);
        }

        [TestMethod]
        public void CreateService_ValidData()
        {
            //Arrange
            string name = "test_name";
            string description = "test_desc";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            //Act
            controller.Create(name, description, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);

            var service = Context.Services.FirstOrDefault(s => s.Name == name);

            if (service != null)
            {
                Context.Services.Remove(service);
                Context.SaveChanges();
            }

            //Assert
            Assert.IsNotNull(service);
            Assert.AreEqual(name, service.Name);
            Assert.AreEqual(description, service.Description);
            Assert.AreEqual(priceStr, service.Price.ToString());
            Assert.AreEqual(internetQuantityStr, service.InternetQuantity.ToString());
            Assert.AreEqual(minutesQuantityStr, service.MinutesQuantity.ToString());
            Assert.AreEqual(otherMinutesQuantityStr, service.OtherMinutesQuantity.ToString());
            Assert.AreEqual(smsQuantityStr, service.SMSQuantity.ToString());
        }

        [TestMethod]
        public void CreateService_InvalidData()
        {
            //Arrange
            string name = "test_name";
            string description = "test_desc";
            string priceStr = "-123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            //Act + Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Create(name, description, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr));
        }

        [TestMethod]
        public void DeleteService_NoUserReference()
        {
            //Arrange
            string name = "test_name";
            string description = "test_desc";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";
            controller.Create(name, description, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);

            //Act
            int serviceId = Context.Services.FirstOrDefault(s => s.Name == name).Id;
            controller.Delete(serviceId);
            var service = Context.Services.FirstOrDefault(s => s.Id == serviceId);

            //Assert
            Assert.IsNull(service);
        }

        [TestMethod]
        public void DeleteService_UserReference()
        {
            //Arrange
            string name = "test_name";
            string description = "test_desc";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";
            controller.Create(name, description, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);
            int serviceId = Context.Services.FirstOrDefault(s => s.Name == name).Id;
            var service = Context.Services.FirstOrDefault(s => s.Id == serviceId);

            UserModel user = new UserModel();
            user.Name = "test";
            user.MiddleName = "test";
            user.Surname = "test";
            user.Tariff = Context.Tariffs.FirstOrDefault();
            user.PhoneNumber = "test";
            Context.Users.Add(user);

            var connectedService = new UserConnectedServicesModel
            {
                User = user,
                Service = service,
                ConnectionDate = DateTime.Now.Date,
            };

            Context.UserConnectedServices.Add(connectedService);

            Context.SaveChanges();

            //Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Delete(serviceId));

            Context.UserConnectedServices.Remove(connectedService);
            Context.Services.Remove(service);
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        [TestMethod]
        public void UpdateService_ValidData()
        {
            //Arrange
            string name = "test_name";
            string description = "test_desc";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            string newName = "test_name1";
            string newDescription = "test_desc1";
            string newPriceStr = "1231";
            string newInternetQuantityStr = "1231";
            string newMinutesQuantityStr = "1231";
            string newOtherMinutesQuantityStr = "1231";
            string newSmsQuantityStr = "1231";

            controller.Create(name, description, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);
            int serviceId = Context.Services.FirstOrDefault(s => s.Name == name).Id;

            //Act
            controller.Update(serviceId, newName, newDescription, newPriceStr, newInternetQuantityStr, newMinutesQuantityStr, newOtherMinutesQuantityStr, newSmsQuantityStr);
            var service = Context.Services.FirstOrDefault(s => s.Id == serviceId);

            if (service != null)
            {
                Context.Services.Remove(service);
                Context.SaveChanges();
            }

            //Assert
            Assert.IsNotNull(service);
            Assert.AreEqual(newName, service.Name);
            Assert.AreEqual(newDescription, service.Description);
            Assert.AreEqual(newPriceStr, service.Price.ToString());
            Assert.AreEqual(newInternetQuantityStr, service.InternetQuantity.ToString());
            Assert.AreEqual(newMinutesQuantityStr, service.MinutesQuantity.ToString());
            Assert.AreEqual(newOtherMinutesQuantityStr, service.OtherMinutesQuantity.ToString());
            Assert.AreEqual(newSmsQuantityStr, service.SMSQuantity.ToString());
        }

        [TestMethod]
        public void UpdateService_InvalidData()
        {
            //Arrange
            string name = "test_name";
            string description = "test_desc";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            string invalidData = "invalid";

            controller.Create(name, description, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);
            int serviceId = Context.Services.FirstOrDefault(s => s.Name == name).Id;

            //Act + Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Update(serviceId, invalidData, invalidData, invalidData, invalidData, invalidData, invalidData, invalidData));
            
            var service = Context.Services.FirstOrDefault(s => s.Id == serviceId);

            if (service != null)
            {
                Context.Services.Remove(service);
                Context.SaveChanges();
            }
        }
    }
}