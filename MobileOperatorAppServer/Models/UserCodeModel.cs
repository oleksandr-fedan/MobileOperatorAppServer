using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MobileOperatorAppServer.Models
{
    public class UserCodeModel
    {        
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public UserModel User { get; set; }
    }
}
