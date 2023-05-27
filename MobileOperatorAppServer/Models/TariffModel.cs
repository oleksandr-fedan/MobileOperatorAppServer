using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileOperatorAppServer.Models
{
    public class TariffModel
    {        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public double InternetQuantity { get; set; }

        [Required]
        public int MinutesQuantity { get; set; }

        [Required]
        public int OtherMinutesQuantity { get; set; }

        [Required]
        public int SMSQuantity { get; set; }

        public ICollection<UserModel> Users { get; set; }
    }
}
