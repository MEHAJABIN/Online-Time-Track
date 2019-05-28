using OnlineTimeTrack.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Services;
using OnlineTimeTrack.Models;

namespace OnlineTimeTrack.Services
{
    public class WorklogService : IWorklogService
    {

        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;


        public WorklogService(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
        }


        public IEnumerable<Worklog> GetAll()
        {
            return _onlineTimeTrackContext.Worklogs;
        }

        public Worklog GetById(long id)
        {
            return _onlineTimeTrackContext.Worklogs.Find(id);
        }

        public Worklog Create(Worklog worklog, DateTime EstimateWorkTime, string Features, DateTime ActualTime)
        {
            
            

           
           // Add Features
            if (string.IsNullOrEmpty(Features))
                throw new AppException("Feature is required");

            if (_onlineTimeTrackContext.Worklogs.Any(x => x.Features == worklog.Features))
                throw new AppException("Features \"" + worklog.Features + "\" is already taken");

           



            worklog.EstimateWorkTimeStart =EstimateWorkTime;
            worklog.EstimateWorkTimeEnd = EstimateWorkTime;
            worklog.Features = Features;
            worklog.ActualWorkTime = ActualTime;

            _onlineTimeTrackContext.Worklogs.Add(worklog);
            _onlineTimeTrackContext.SaveChanges();

            return worklog;
        }



        void Update(Worklog worklog, string Features = null)
        {
            var Worklog = _onlineTimeTrackContext.Projects.Find(worklog.WorklogID);
            if (worklog == null)
                throw new AppException("worklog not found");

            if (worklog.Features != worklog.Features)
            {
                //Features has changed so check if the new Feature is already taken
                if (_onlineTimeTrackContext.Worklogs.Any(x => x.Features == worklog.Features))
                    throw new AppException("Worklog " + worklog.Features + " is already taken");
            }

        }

        public async Task<Worklog> Worklog(Worklog worklog)


        {
           
            // save the project
            var addedWorklog = await _onlineTimeTrackContext.Worklogs.AddAsync(worklog);
            await _onlineTimeTrackContext.SaveChangesAsync();
            // addedProject.Entity.ProjectID = long;
            addedWorklog.Entity.Features = worklog.Features;



            // return the project
            return addedWorklog.Entity;


        }

     

        public Project GetProject(long id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(Worklog worklog, DateTime EstimateWorkTime, DateTime ActualTime, string Features = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Worklog Create(Worklog worklog, DateTime EstimateWorkTimeStart, DateTime EstimateWorkTimeEnd, string Features, DateTime ActualTime)
        {
            throw new NotImplementedException();
        }
    }
}












