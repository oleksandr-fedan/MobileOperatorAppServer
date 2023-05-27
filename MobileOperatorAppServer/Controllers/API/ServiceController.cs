using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;

namespace MobileOperatorAppServer.Controllers.API
{
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly Context context;

        public ServiceController(Context context)
        {
            this.context = context;
        }

        //https://localhost:44385/api/service/963831721
        [HttpGet("{phoneNumber}")]
        public ICollection GetUserServices(string phoneNumber)
        {
            var connectedServices = context.UserConnectedServices
                .Include(ucs => ucs.User)
                .Include(ucs => ucs.Service)
                .Where(ucs => ucs.User.PhoneNumber == phoneNumber)
                .Select(ucs => ucs.Service)
                .ToList();

            return connectedServices;
        }

        //https://localhost:44385/api/service
        [HttpGet]
        public ICollection GetServices()
        {
            return context.Services.ToList();
        }
    }
}
