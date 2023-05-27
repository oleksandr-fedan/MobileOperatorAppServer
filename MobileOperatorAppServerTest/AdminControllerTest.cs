using MobileOperatorAppServer;
using MobileOperatorAppServer.Controllers;

namespace MobileOperatorAppServerTest
{
    [TestClass]
    public class AdminControllerTest : BaseTest
    {
        public AdminController controller;

        public AdminControllerTest()
        {
            controller = new AdminController(Context);
        }

        [TestMethod]
        public void CreateAdmin_ValidData()
        {
            //Arrange
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middle_name";
            string username = "test_username";
            string password = "test_password";

            //Act
            controller.Create(name, surname, middleName, username, password);

            var admin = Context.Admins.FirstOrDefault(a => a.Username == username);

            if (admin != null)
            {
                Context.Admins.Remove(admin);
                Context.SaveChanges();
            }

            //Assert
            Assert.IsNotNull(admin);
            Assert.AreEqual(name, admin.Name);
            Assert.AreEqual(surname, admin.Surname);
            Assert.AreEqual(middleName, admin.MiddleName);
            Assert.AreEqual(username, admin.Username);
            Assert.AreEqual(password, admin.Password);
        }

        [TestMethod]
        public void DeleteAdmin_ValidData()
        {
            //Arrange
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middle_name";
            string username = "test_username";
            string password = "test_password";
            controller.Create(name, surname, middleName, username, password);

            //Act
            int adminId = Context.Admins.FirstOrDefault(a => a.Name == name).Id;
            controller.Delete(adminId);
            var admin = Context.Admins.FirstOrDefault(a => a.Id == adminId);

            //Assert
            Assert.IsNull(admin);
        }

        [TestMethod]
        public void UpdateAdmin_ValidData()
        {
            //Arrange
            string name = "test_name";
            string surname = "test_surname";
            string middleName = "test_middle_name";
            string username = "test_username";
            string password = "test_password";
            controller.Create(name, surname, middleName, username, password);
            string newName = "new_test_name";
            string newSurname = "new_test_surname";
            string newMiddleName = "new_test_middle_name";
            string newUsername = "new_test_username";
            string newPassword = "new_test_password";

            //Act
            int adminId = Context.Admins.FirstOrDefault(a => a.Name == name).Id;
            controller.Update(adminId, newName, newSurname, newMiddleName, newUsername, newPassword);
            var admin = Context.Admins.FirstOrDefault(a => a.Id == adminId);
            controller.Delete(adminId);

            //Assert
            Assert.IsNotNull(admin);
            Assert.AreEqual(newName, admin.Name);
            Assert.AreEqual(newSurname, admin.Surname);
            Assert.AreEqual(newMiddleName, admin.MiddleName);
            Assert.AreEqual(newUsername, admin.Username);
            Assert.AreEqual(newPassword, admin.Password);
        }
    }
}