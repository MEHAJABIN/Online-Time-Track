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


        Task<IEnumerable<Worklog>> GetAll(long ProjectID);
        Task<IEnumerable<Worklog>> Get(long UserID);
        Task<IEnumerable<Worklog>> GetAll(string Features);
        Worklog Create(Worklog worklog,int EstimateWorkTime,string Features,DateTime ActualTimeStart,DateTime ActualTimeEnd);
        void Update(Worklog worklog, int EstimateWorkTime,  DateTime ActualTime, string Features = null);
        int? GetprojectIDFromContext(HttpContext context);

    }
}


