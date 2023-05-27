using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace MobileOperatorAppServer.Controllers.API
{
    
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly Context context;

        public UserController(Context context)
        {
            this.context = context;
        }

        [HttpGet("{phoneNumber}")]
        public UserModel GetUser(string phoneNumber)
        {
            UserModel user = context
                .Users
                .Include(u => u.Tariff)
                .FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user != null)
                user.Tariff.Users = null;

            return user;
        }

        [HttpPost("update_tariff")]
        public IActionResult UpdateTariff([FromBody] JsonDocument requestBody)
        {
            string phoneNumber = requestBody.RootElement.GetProperty("phoneNumber").GetString();

            UserModel user = context
                .Users
                .Include(u => u.Tariff)
                .FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            user.Balance -= user.Tariff.Price;
            user.ConnectionDate = DateTime.Now.Date;

            List<UserConnectedServicesModel> connectedServices = context.UserConnectedServices
                .Include(s => s.User)
                .Where(s => s.User.PhoneNumber == phoneNumber)
                .ToList();

            context.UserConnectedServices.RemoveRange(connectedServices);

            var activities = context.Activities
               .Include(a => a.User)
               .Where(a => a.User.PhoneNumber == phoneNumber)
               .ToList();

            context.Activities.RemoveRange(activities);

            context.SaveChanges();

            return Ok();
        }

        [HttpPost("connect_tariff")]
        public IActionResult UserConnectTariff([FromBody] JsonDocument requestBody)
        {
            string phoneNumber = requestBody.RootElement.GetProperty("userPhoneNumber").GetString();
            int tariffId = requestBody.RootElement.GetProperty("tariffId").GetInt32();

            UserModel user = context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            TariffModel tariff = context.Tariffs.FirstOrDefault(t => t.Id == tariffId);

            user.Balance -= tariff.Price;
            user.Tariff = tariff;
            user.ConnectionDate = DateTime.Now.Date;

            List<UserConnectedServicesModel> connectedServices = context.UserConnectedServices
                .Include(s => s.User)
                .Where(s => s.User.PhoneNumber == phoneNumber)
                .ToList();

            context.UserConnectedServices.RemoveRange(connectedServices);

            var activities = context.Activities
               .Include(a => a.User)
               .Where(a => a.User.PhoneNumber == phoneNumber)
               .ToList();

            context.Activities.RemoveRange(activities);

            context.SaveChanges();
            return Ok();
        }

        [HttpPost("connect_service")]
        public IActionResult UserConnectService([FromBody] JsonDocument requestBody)
        {
            string userPhoneNumber = requestBody.RootElement.GetProperty("userPhoneNumber").GetString();
            int serviceId = requestBody.RootElement.GetProperty("serviceId").GetInt32();

            UserModel user = context.Users.FirstOrDefault(u => u.PhoneNumber == userPhoneNumber);
            ServiceModel service = context.Services.FirstOrDefault(s => s.Id == serviceId);
            user.Balance -= service.Price;
            var connectedServices = context.UserConnectedServices;
            var data = new UserConnectedServicesModel
            {
                User = user,
                Service = service,
                ConnectionDate = DateTime.Now.Date
            };
            connectedServices.Add(data);

            context.SaveChanges();
            return Ok();
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] JsonDocument requestBody)
        {
            string phoneNumber = requestBody.RootElement.GetProperty("phoneNumber").GetString();
            decimal deposit = requestBody.RootElement.GetProperty("deposit").GetDecimal();

            UserModel user = context
                .Users
                .FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            user.Balance += deposit;

            context.SaveChanges();

            return Ok();
        }

        [HttpGet("available_services/{phoneNumber}")]
        public IActionResult GetAwailableServices(string phoneNumber)
        {
            double availableInternet = 0;
            double availableMinutes = 0;
            double availableOtherMinutes = 0;
            double availableSMS = 0;

            var connectedServices = context.UserConnectedServices
                .Include(ucs => ucs.User)
                .Include(ucs => ucs.Service)
                .Where(ucs => ucs.User.PhoneNumber == phoneNumber)
                .Select(ucs => ucs.Service)
                .ToList();

            foreach (var service in connectedServices)
            {
                availableInternet += (double)service.InternetQuantity;
                availableMinutes += (int)service.MinutesQuantity;
                availableOtherMinutes += (int)service.OtherMinutesQuantity;
                availableSMS += (int)service.SMSQuantity;
            }

            TariffModel tariff = context.Users.Include(u => u.Tariff).FirstOrDefault(u => u.PhoneNumber == phoneNumber).Tariff;

            if (tariff.InternetQuantity == -1)
                availableInternet = -1;
            else
                availableInternet += tariff.InternetQuantity;

            if (tariff.MinutesQuantity == -1)
                availableMinutes = -1;
            else
                availableMinutes += tariff.MinutesQuantity;

            if (tariff.OtherMinutesQuantity == -1)
                availableOtherMinutes = -1;
            else
                availableOtherMinutes += tariff.OtherMinutesQuantity;

            if (tariff.SMSQuantity == -1)
                availableSMS = -1;
            else
                availableSMS += tariff.SMSQuantity;

            var availableServices = new
            {
                AvailableInternet = availableInternet,
                AvailableMinutes = availableMinutes,
                AvailableOtherMinutes = availableOtherMinutes,
                AvailableSMS = availableSMS,
            };

            return Ok(availableServices);
        }
    }
}