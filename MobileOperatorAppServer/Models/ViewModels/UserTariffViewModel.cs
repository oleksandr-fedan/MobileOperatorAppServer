using System.Collections.Generic;

namespace MobileOperatorAppServer.Models.ViewModels
{
    public class UserTariffViewModel
    {
        public List<UserModel> Users { get; set; }
        public List<TariffModel> Tariffs { get; set; }
    }
}
