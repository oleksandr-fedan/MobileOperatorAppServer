using System.Collections.Generic;

namespace MobileOperatorAppServer.Models.ViewModels
{
    public class UserActivitiesViewModel
    {
        public List<ActivityModel> Activities { get; set; }
        public UserModel User { get; set; }
    }
}
