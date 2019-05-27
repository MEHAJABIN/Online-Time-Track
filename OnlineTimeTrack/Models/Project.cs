using System.ComponentModel.DataAnnotations;
 
namespace OnlineTimeTrack.Models
{
    public class Project
    {
         [Key]
         public long ProjectID { get; set; }
         public string ProjectTitle { get; set; }
    }
}



