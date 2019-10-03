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
        Task<Worklog> GetById(long? id);

        Task<IEnumerable<Worklog>> GetAllWorklogs(int start, int limit, long? WorklogID, long? UserID, long? ProjectID, string Feature, int EstimateWorkTime,
        DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd, string ProjectTitle, string FullName, string Address);

        Task<IEnumerable<Worklog>> GetAll(long ProjectID);
        Task<IEnumerable<Worklog>> Get(long UserID);
        Task<IEnumerable<Worklog>> GetAll(string Feature);
        Worklog Create(Worklog worklog,int EstimateWorkTime,string Feature,DateTime ActualTimeStart,DateTime ActualTimeEnd);
    
    }
}


