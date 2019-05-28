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

        public DateTime EstimateWorkTimeStart { get; set; }
        public DateTime EstimateWorkTimeEnd { get; set; }
        public string Features { get; set; }
        public DateTime ActualWorkTime { get; set; }

    }
}
