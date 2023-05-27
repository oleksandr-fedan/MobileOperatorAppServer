using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MobileOperatorAppServer.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public double? InternetQuantity { get; set; }

        public int? MinutesQuantity { get; set; }

        public int? OtherMinutesQuantity { get; set; }

        public int? SMSQuantity { get; set; }
    }
}
