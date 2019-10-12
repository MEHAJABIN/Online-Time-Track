using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Models
{
    public class Worklog

    {
        [Key]
        public long WorklogID { get; set; }

        public long ProjectID { get; set; }

        public long UserID { get; set; }

        public DateTime Date { get; set; }

        public int EstimateWorkTime { get; set; }

        public string Feature { get; set; }

        [NotMapped]
        public string ProjectTitle { get; set; }


        [NotMapped]
        public string FullName { get; set; }


        [NotMapped]
        public string Address { get; set; }

        [NotMapped]
        public DateTime ActualWorkTimeStart { get; set; }

        [NotMapped]
        public DateTime ActualWorkTimeEnd { get; set; }


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

        [NotMapped]
        public Timelog TimeLog { get; set; }

        public List<Timelog> Timelogs { get; set; }


        [NotMapped]
        public User User { get; set; }

        public List<User> Users { get; set; }
    }
}

       













   
