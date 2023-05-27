using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections;

namespace MobileOperatorAppServer.Controllers.API
{
    
    [Route("api/[controller]")]
    public class TariffController
    {
        private readonly Context context;

        public TariffController(Context context)
        {
            this.context = context;
        }

        [HttpGet("get_tariffs")]
        public ICollection GetTariffs()
        {
            return context.Tariffs.ToList();
        }
    }
}