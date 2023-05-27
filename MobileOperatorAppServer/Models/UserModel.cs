using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperatorAppServer.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ConnectionDate { get; set; }

        [Required]
        public TariffModel Tariff { get; set; }

        public ICollection<ActivityModel> Activities { get; set; }

        public ICollection<ServiceModel> Services { get; set; }
    }
}
