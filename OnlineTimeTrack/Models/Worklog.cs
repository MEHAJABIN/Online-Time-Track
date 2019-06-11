using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int EstimateWorkTime { get; set; }

        public string Features { get; set; }

        public DateTime ActualWorkTimeStart { get; set; }

        public DateTime ActualWorkTimeEnd { get; set; }

        public double TotalWorkTime
        {
            get
            {                
                return (ActualWorkTimeEnd - ActualWorkTimeStart).TotalHours;
            }
        }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }
    }
}








   
