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
    public class TariffControllerTest : BaseTest
    {
        public TariffController controller;

        public TariffControllerTest()
        {
            controller = new TariffController(Context);
        }

        [TestMethod]
        public void CreateTariff_ValidData()
        {
            //Arrange
            string name = "test_name";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            //Act
            controller.Create(name, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);

            var tariff = Context.Tariffs.FirstOrDefault(t => t.Name == name);

            if (tariff != null)
            {
                Context.Tariffs.Remove(tariff);
                Context.SaveChanges();
            }

            //Assert
            Assert.IsNotNull(tariff);
            Assert.AreEqual(name, tariff.Name);
            Assert.AreEqual(priceStr, tariff.Price.ToString());
            Assert.AreEqual(internetQuantityStr, tariff.InternetQuantity.ToString());
            Assert.AreEqual(minutesQuantityStr, tariff.MinutesQuantity.ToString());
            Assert.AreEqual(otherMinutesQuantityStr, tariff.OtherMinutesQuantity.ToString());
            Assert.AreEqual(smsQuantityStr, tariff.SMSQuantity.ToString());
        }

        [TestMethod]
        public void CreateTariff_InvalidData()
        {
            //Arrange
            string name = "test_name";
            string priceStr = "-123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            //Act + Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Create(name, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr));
        }

        [TestMethod]
        public void DeleteTariff_NoUserReference()
        {
            //Arrange
            string name = "test_name";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";
            controller.Create(name, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);

            //Act
            int tariffId = Context.Tariffs.FirstOrDefault(s => s.Name == name).Id;
            controller.Delete(tariffId);
            var tariff = Context.Tariffs.FirstOrDefault(s => s.Id == tariffId);

            //Assert
            Assert.IsNull(tariff);
        }

        [TestMethod]
        public void DeleteService_UserReference()
        {
            //Arrange
            string name = "test_name";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";
            controller.Create(name, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);
            int tariffId = Context.Tariffs.FirstOrDefault(s => s.Name == name).Id;
            var tariff = Context.Tariffs.FirstOrDefault(s => s.Id == tariffId);

            UserModel user = new UserModel();
            user.Name = "test";
            user.MiddleName = "test";
            user.Surname = "test";
            user.Tariff = tariff;
            user.PhoneNumber = "test";

            Context.Users.Add(user);
            Context.SaveChanges();

            //Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Delete(tariffId));

            Context.Users.Remove(user);
            Context.Tariffs.Remove(tariff);
            Context.SaveChanges();
        }

        [TestMethod]
        public void UpdateTariff_ValidData()
        {
            //Arrange
            string name = "test_name";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            string newName = "test_name1";
            string newPriceStr = "1231";
            string newInternetQuantityStr = "1231";
            string newMinutesQuantityStr = "1231";
            string newOtherMinutesQuantityStr = "1231";
            string newSmsQuantityStr = "1231";

            controller.Create(name, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);
            int tariffId = Context.Tariffs.FirstOrDefault(s => s.Name == name).Id;

            //Act
            controller.Update(tariffId, newName, newPriceStr, newInternetQuantityStr, newMinutesQuantityStr, newOtherMinutesQuantityStr, newSmsQuantityStr);
            var tariff = Context.Tariffs.FirstOrDefault(s => s.Id == tariffId);

            if (tariff != null)
            {
                Context.Tariffs.Remove(tariff);
                Context.SaveChanges();
            }

            //Assert
            Assert.IsNotNull(tariff);
            Assert.AreEqual(newName, tariff.Name);
            Assert.AreEqual(newPriceStr, tariff.Price.ToString());
            Assert.AreEqual(newInternetQuantityStr, tariff.InternetQuantity.ToString());
            Assert.AreEqual(newMinutesQuantityStr, tariff.MinutesQuantity.ToString());
            Assert.AreEqual(newOtherMinutesQuantityStr, tariff.OtherMinutesQuantity.ToString());
            Assert.AreEqual(newSmsQuantityStr, tariff.SMSQuantity.ToString());
        }

        [TestMethod]
        public void UpdateTariff_InvalidData()
        {
            //Arrange
            string name = "test_name";
            string priceStr = "123";
            string internetQuantityStr = "123";
            string minutesQuantityStr = "123";
            string otherMinutesQuantityStr = "123";
            string smsQuantityStr = "123";

            string invalidData = "invalid";

            controller.Create(name, priceStr, internetQuantityStr, minutesQuantityStr, otherMinutesQuantityStr, smsQuantityStr);
            int tariffId = Context.Tariffs.FirstOrDefault(t => t.Name == name).Id;

            //Act + Assert
            Assert.ThrowsException<NullReferenceException>(() => controller.Update(tariffId, invalidData, invalidData, invalidData, invalidData, invalidData, invalidData));

            var tariff = Context.Tariffs.FirstOrDefault(t => t.Id == tariffId);

            if (tariff != null)
            {
                Context.Tariffs.Remove(tariff);
                Context.SaveChanges();
            }
        }
    }
}
