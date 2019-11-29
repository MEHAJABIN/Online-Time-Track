 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Models
{
    public class User
    {
        [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserID { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public DateTime Dob { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string ContactNumber { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }
        
        public string PasswordKey { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string Token { get; set; }

        [ForeignKey("WorklogID")]
        public Worklog Worklog { get; set; }

        [NotMapped]
        public long TimelogID { get; set; }

    }
}
