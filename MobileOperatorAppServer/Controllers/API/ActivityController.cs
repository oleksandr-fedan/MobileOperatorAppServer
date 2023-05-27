using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileOperatorAppServer.Models;
using System.Linq;
using System.Collections;

namespace MobileOperatorAppServer.Controllers.API
{
    [Route("api/[controller]")]
    public class ActivityController
    {
        private readonly Context context;

        public ActivityController(Context context)
        {
            this.context = context;
        }

        //https://localhost:44385/ get_internet_activity/963831721
        [HttpGet("get_internet_activity/{phoneNumber}")]
        public ICollection GetInternetActivity(string phoneNumber)
        {
            var activities = context.Activities
                .Include(a => a.User)
                .Where(a => a.User.PhoneNumber == phoneNumber && a.Type == ActivityType.INTERNET)
                .GroupBy(a => new { a.User.Id, a.Type, a.Date.Date })
                .Select(g => new { g.Key.Id, g.Key.Type, Quantity = g.Sum(a => a.Quantity), g.Key.Date })
                .ToList();

            return activities;
        }

        //https://localhost:44385/api/activity/get_minutes_activity/963831721
        [HttpGet("get_minutes_activity/{phoneNumber}")]
        public ICollection GetMinutesActivity(string phoneNumber)
        {
            var activities = context.Activities
                .Include(a => a.User)
                .Where(a => a.User.PhoneNumber == phoneNumber && a.Type == ActivityType.MINUTES)
                .GroupBy(a => new { a.User.Id, a.Type, a.Date.Date })
                .Select(g => new { g.Key.Id, g.Key.Type, Quantity = g.Sum(a => a.Quantity), g.Key.Date })
                .ToList();

            return activities;
        }

        //https://localhost:44385/api/activity/get_other_minutes_activity/963831721
        [HttpGet("get_other_minutes_activity/{phoneNumber}")]
        public ICollection GetOtherMinutesActivity(string phoneNumber)
        {
            var activities = context.Activities
                .Include(a => a.User)
                .Where(a => a.User.PhoneNumber == phoneNumber && a.Type == ActivityType.OTHER_MINUTES)
                .GroupBy(a => new { a.User.Id, a.Type, a.Date.Date })
                .Select(g => new { g.Key.Id, g.Key.Type, Quantity = g.Sum(a => a.Quantity), g.Key.Date })
                .ToList();

            return activities;
        }

        //https://localhost:44385/api/activity/get_sms_activity/963831721
        [HttpGet("get_sms_activity/{phoneNumber}")]
        public ICollection GetSMSActivity(string phoneNumber)
        {
            var activities = context.Activities
                .Include(a => a.User)
                .Where(a => a.User.PhoneNumber == phoneNumber && a.Type == ActivityType.SMS)
                .GroupBy(a => new { a.User.Id, a.Type, a.Date.Date })
                .Select(g => new { g.Key.Id, g.Key.Type, Quantity = g.Sum(a => a.Quantity), g.Key.Date })
                .ToList();

            return activities;
        }
    }
}