using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MobileOperatorAppServer.Models
{
    public class ActivityModel
    {        
        public int Id { get; set; }

        [Required]
        public UserModel User { get; set; }

        [Required]
        public ActivityType Type { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        [Column(TypeName = "datetime(0)")]
        public DateTime Date { get; set; }
    }
}
