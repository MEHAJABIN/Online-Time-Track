using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTimeTrack.Models
{
    public class Project
    {
        
        [Key]
        public long ProjectID { get; set; }

        public string ProjectTitle { get; set; }

    }
}






