using System.Collections.Generic;

namespace MobileOperatorAppServer.Models.ViewModels
{
    public class UserServicesViewModel
    {
        public UserModel User { get; set; }
        public List<ServiceModel> Services { get; set; }
    }
}
