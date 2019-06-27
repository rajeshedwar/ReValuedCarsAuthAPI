using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReValuedCarsAuthAPI.Models
{
    public class JClient
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
    }
}
