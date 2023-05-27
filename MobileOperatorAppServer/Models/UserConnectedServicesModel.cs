using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileOperatorAppServer.Models
{
    public class UserConnectedServicesModel
    {
        public int Id { get; set; }

        [Required]
        public UserModel User { get; set; }

        [Required]
        public ServiceModel Service { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ConnectionDate { get; set; }
    }
}
