using Microsoft.EntityFrameworkCore.Migrations;
using MobileOperatorAppServer.Models;
using System;

#nullable disable

namespace MobileOperatorAppServer.Migrations
{
    /// <inheritdoc />
    public partial class insert_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"ALTER TABLE {nameof(Context.Admins)} AUTO_INCREMENT = 1;");
            migrationBuilder.Sql($"ALTER TABLE {nameof(Context.Users)} AUTO_INCREMENT = 1;");
            migrationBuilder.Sql($"ALTER TABLE {nameof(Context.Tariffs)} AUTO_INCREMENT = 1;");

            migrationBuilder.InsertData(
                table: nameof(Context.Admins),
                columns: new[] { nameof(AdminModel.Name), nameof(AdminModel.Surname),
                        nameof(AdminModel.MiddleName), nameof(AdminModel.Username), nameof(AdminModel.Password)},
                values: new object[] { "Олександр", "Федан", "Андрійович", "admin", "admin" });

            migrationBuilder.InsertData(
                table: nameof(Context.Tariffs),
                columns: new[] { nameof(TariffModel.Name), nameof(TariffModel.Price), nameof(TariffModel.InternetQuantity),
                    nameof(TariffModel.MinutesQuantity), nameof(TariffModel.OtherMinutesQuantity), nameof(TariffModel.SMSQuantity),},
                values: new object[,]
                {
                    { "Тестовий тариф 1", 200, 10000, -1, 300, 0 },
                    { "Тестовий тариф 2", 300, 15000, -1, 500, 100 },
                    { "Тестовий тариф 3", 400, 20000, -1, 800, 200 },
                    { "Тестовий тариф 4", 500, 25000, -1, 1200, 300 },
                    { "Тестовий тариф 5", 600, 30000, -1, 1700, 400 },
                    { "Тестовий тариф 6", 700, 35000, -1, 2300, 500 },
                    { "Тестовий тариф 7", 60, 2000, 1000, 0, 10 }
                });

            migrationBuilder.InsertData(
                table: nameof(Context.Users),
                columns: new[] { nameof(UserModel.PhoneNumber), nameof(UserModel.Name), nameof(UserModel.Surname),
                    nameof(UserModel.MiddleName), nameof(UserModel.Balance), nameof(UserModel.ConnectionDate), "TariffId"},
                values: new object[] { "963831721", "Олександр", "Федан", "Андрійович", 1234, DateTime.Now.Date, 1 });

            migrationBuilder.InsertData(
                table: nameof(Context.Activities),
                columns: new[] { "UserId", "Type", nameof(ActivityModel.Quantity), nameof(ActivityModel.Date) },
                values: new object[,]
                {
                    { 1, (int)ActivityType.INTERNET, 1000.0, DateTime.Now.Date },
                    { 1, (int)ActivityType.INTERNET, 500.0, DateTime.Now.AddDays(-1).Date },
                    { 1, (int)ActivityType.INTERNET, 2000.0, DateTime.Now.AddDays(-2).Date },
                    { 1, (int)ActivityType.INTERNET, 1500.0, DateTime.Now.AddDays(-3).Date },
                    { 1, (int)ActivityType.INTERNET, 3000.0, DateTime.Now.AddDays(-4).Date },
                    { 1, (int)ActivityType.INTERNET, 200.0, DateTime.Now.AddDays(-5).Date },
                    { 1, (int)ActivityType.INTERNET, 400.0, DateTime.Now.AddDays(-6).Date },

                    { 1, (int)ActivityType.MINUTES, 50.0, DateTime.Now.Date },
                    { 1, (int)ActivityType.MINUTES, 100.0, DateTime.Now.AddDays(-1).Date },
                    { 1, (int)ActivityType.MINUTES, 200.0, DateTime.Now.AddDays(-2).Date },
                    { 1, (int)ActivityType.MINUTES, 150.0, DateTime.Now.AddDays(-3).Date },
                    { 1, (int)ActivityType.MINUTES, 0.0, DateTime.Now.AddDays(-4).Date },
                    { 1, (int)ActivityType.MINUTES, 250.0, DateTime.Now.AddDays(-5).Date },
                    { 1, (int)ActivityType.MINUTES, 300.0, DateTime.Now.AddDays(-6).Date },

                    { 1, (int)ActivityType.OTHER_MINUTES, 20.0, DateTime.Now.Date },
                    { 1, (int)ActivityType.OTHER_MINUTES, 50.0, DateTime.Now.AddDays(-1).Date },
                    { 1, (int)ActivityType.OTHER_MINUTES, 100.0, DateTime.Now.AddDays(-2).Date },
                    { 1, (int)ActivityType.OTHER_MINUTES, 0.0, DateTime.Now.AddDays(-3).Date },
                    { 1, (int)ActivityType.OTHER_MINUTES, 150.0, DateTime.Now.AddDays(-4).Date },
                    { 1, (int)ActivityType.OTHER_MINUTES, 200.0, DateTime.Now.AddDays(-5).Date },
                    { 1, (int)ActivityType.OTHER_MINUTES, 250.0, DateTime.Now.AddDays(-6).Date },

                    { 1, (int)ActivityType.SMS, 5.0, DateTime.Now.Date },
                    { 1, (int)ActivityType.SMS, 10.0, DateTime.Now.AddDays(-1).Date },
                    { 1, (int)ActivityType.SMS, 20.0, DateTime.Now.AddDays(-2).Date },
                    { 1, (int)ActivityType.SMS, 0.0, DateTime.Now.AddDays(-3).Date },
                    { 1, (int)ActivityType.SMS, 30.0, DateTime.Now.AddDays(-4).Date },
                    { 1, (int)ActivityType.SMS, 40.0, DateTime.Now.AddDays(-5).Date },
                    { 1, (int)ActivityType.SMS, 50.0, DateTime.Now.AddDays(-6).Date },
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: nameof(Context.Users),
            keyColumn: nameof(UserModel.PhoneNumber),
            keyValue: "963831721");

            migrationBuilder.DeleteData(
                table: nameof(Context.Tariffs),
                keyColumn: nameof(TariffModel.Name),
                keyValues: new object[]
                {
                    "Тестовий тариф 1",
                    "Тестовий тариф 2",
                    "Тестовий тариф 3",
                    "Тестовий тариф 4",
                    "Тестовий тариф 5",
                    "Тестовий тариф 6",
                    "Тестовий тариф 7"
                });

            migrationBuilder.DeleteData(
                table: nameof(Context.Admins),
                keyColumn: nameof(AdminModel.Username),
                keyValue: "admin");

            migrationBuilder.DeleteData(
                table: nameof(Context.Activities),
                keyColumn: nameof(ActivityModel.User.Id),
                keyValue: 1);
        }
    }
}
