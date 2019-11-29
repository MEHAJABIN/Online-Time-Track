using Microsoft.AspNetCore.Http;
using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineTimeTrack.Contexts;

namespace OnlineTimeTrack.Services
{
  public  interface IWorklogService
  {
      
        Task<Worklog> Worklog(Worklog worklog);
        Task<Worklog> UpdateWorklog(Worklog WorklogID);
        Task<Worklog> DeleteWorklog(Worklog WorklogID);
        Task<Worklog> GetById(int id);

        Task<IEnumerable<Worklog>> GetAllWorklogs(int start, int limit, long? WorklogID, long? UserID, long? ProjectID, string Feature, int? EstimateWorkTime,
        DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd, string ProjectTitle, string FullName, string Address);

        Task<IEnumerable<Worklog>> GetProjectWorklog(int ProjectId,long? WorklogID, long? UserID, long? ProjectID,
           string ProjectTitle, string Feature, string FullName, string Address);

        Task<IEnumerable<Worklog>> GetUserWorklog(int userId, long? WorklogID, long? UserID, long? ProjectID,long? TimelogID, 
           string ProjectTitle, string Feature, string FullName, string Address, int? EstimateWorkTime, DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd);

       
        Task<IEnumerable<Worklog>> GetAll(string Feature);
       
    
    }
}


