using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entities
{
    public class AppUser
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string name { get; set; }
        public byte[] passwordHas { get; set; }
        public byte[] passwordSalt { get; set; }


    }
}
