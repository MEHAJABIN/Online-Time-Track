using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Services
{
  public  interface IWorklogService
    {
        Task<Worklog> Worklog(Worklog worklog);


        IEnumerable<Worklog> GetAll();
        Worklog GetById(long id);
        Project GetProject(long id);
        User GetUser(long id);
        Worklog Create(Worklog worklog,int EstimateWorkTime,string Features,DateTime ActualTimeStart,DateTime ActualTimeEnd);
        void Update(Worklog worklog, int EstimateWorkTime,  DateTime ActualTime, string Features = null);
        void Delete(long id);

    }
}
