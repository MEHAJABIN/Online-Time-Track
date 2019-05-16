using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Models
{
    public class User
    {
        [Key]
        public long UserID { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public DateTime Dob { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string Username { get; set; }

        public string PasswordKey { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
       
        //  public object Id { get; internal set; }
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
    }
}
