using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer.Models;
using System.Linq;
using System;

namespace MobileOperatorAppServer.Controllers.API
{
    [Route("api/[controller]")]
    public class LoginController
    {
        private readonly Context context;

        public LoginController(Context context)
        {
            this.context = context;
        }

        [HttpGet("{phoneNumber}")]
        public string CheckEnteredPhoneNumber(string phoneNumber)
        {
            var user = context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user != null)
            {
                Random random = new Random();
                int code = random.Next(1000, 10000);
                context.UserCodes.Add(new UserCodeModel { Code = code.ToString(), User = user });
                context.SaveChanges();
                return code.ToString();
            }
            else
                return "" + -1;
        }

        [HttpGet("{phoneNumber} {code}")]
        public bool CheckEnteredCode(string phoneNumber, string code)
        {
            var user = context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            bool result = false;

            int userId;
            if (user != null)
                userId = user.Id;
            else
                return result;

            var userCodes = context.UserCodes
                .Include(u => u.User)
                .OrderBy(u => u.Id);

            string userCode = null;
            if (userCodes != null && userCodes.Any())
                userCode = userCodes.LastOrDefault(u => u.User.Id == userId).Code;

            result = userCode != null && userCode == code;

            var codesToDelete = context.UserCodes.Where(u => u.User.Id == userId);
            context.UserCodes.RemoveRange(codesToDelete);
            context.SaveChanges();
            return result;
        }
    }
}
