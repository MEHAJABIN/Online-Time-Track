﻿using OnlineTimeTrack.Contexts;
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

        public Worklog Create(Worklog worklog)
        {
            
            

           
           // Add Features
            if (string.IsNullOrEmpty(worklog.Features))
                throw new AppException("Feature is required");

            if (_onlineTimeTrackContext.Worklogs.Any(x => x.Features == worklog.Features))
                throw new AppException("Features \"" + worklog.Features + "\" is already taken");
            
            worklog.DateAdded = DateTime.UtcNow;
            worklog.DateModified = DateTime.UtcNow;
            
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
           
            // save the worklog
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

        public void Update(Worklog worklog, int EstimateWorkTime, DateTime ActualTime, string Features = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Worklog Create(Worklog worklog, int EstimateWorkTime, string Features, DateTime ActualTimeStart, DateTime ActualTimeEnd)
        {
            throw new NotImplementedException();
        }
    }
}











