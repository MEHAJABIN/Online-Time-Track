using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Models
{
    public class Project
    {
         [Key]
         public long ProjectID { get; set; }
         public string ProjectTitle { get; set; }
    }
}



