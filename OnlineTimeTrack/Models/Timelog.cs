using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Models
{
    public class Timelog
    {
        [Key]
        public long TimelogID { get; set; }


        public long WorklogID { get; set; } 

        [NotMapped]
        public long UserID { get; set; }
 
        
        public DateTime ActualWorkTimeStart { get; set; }

        
        public DateTime ActualWorkTimeEnd { get; set; }

        [NotMapped]
        public double TotalWorkTime
        {
            get
            {
                return (ActualWorkTimeEnd - ActualWorkTimeStart).TotalHours;
            }
        }


        [DefaultValue("getutcdate()")]
        public DateTime DateAdded { get; set; }


        [DefaultValue("getutcdate()")]
        public DateTime DateModified { get; set; }




        [ForeignKey("WorklogID")]
        public Worklog Worklog { get; set; }



        [NotMapped]
        public User User { get; set; }

        public List<User> Users { get; set; }


    }
}
