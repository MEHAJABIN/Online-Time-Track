﻿using System;
using OnlineTimeTrack.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Services;
using OnlineTimeTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;
using System.Data.SqlClient;


namespace OnlineTimeTrack.Services
{
    public class WorklogService : IWorklogService
    {

        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;
      



        public WorklogService(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
           
        }


        public Worklog Create(Worklog worklog)
        {
            // Add Features
            if (string.IsNullOrEmpty(worklog.Feature))
                throw new AppException("Feature is required");

            if (_onlineTimeTrackContext.Worklogs.Any(x => x.Feature == worklog.Feature))
                throw new AppException("Feature \"" + worklog.Feature + "\" is already taken");



            _onlineTimeTrackContext.Worklogs.Add(worklog);
            _onlineTimeTrackContext.SaveChanges();

            return worklog;
        }


        public void Update(Worklog worklog, string Feature = null)
        {
            var Worklog = _onlineTimeTrackContext.Projects.Find(worklog.WorklogID);
            if (worklog == null)
                throw new AppException("worklog not found");

            if (worklog.Feature != worklog.Feature)
            {
                //Features has changed so check if the new Feature is already taken
                if (_onlineTimeTrackContext.Worklogs.Any(x => x.Feature == worklog.Feature))
                    throw new AppException("Worklog " + worklog.Feature + " is already taken");
            }

        }


        public async Task<Worklog> Worklog(Worklog worklog)
        {
            // save the worklog
            var Worklog = await _onlineTimeTrackContext.Worklogs.AddAsync(worklog);

            worklog.Feature = worklog.Feature;
            worklog.DateAdded = DateTime.UtcNow;
            worklog.DateModified = DateTime.UtcNow;
            await _onlineTimeTrackContext.SaveChangesAsync();

            // return the worklog
            return worklog;
        }


        public async Task<Worklog> GetById(int id)
        {

            var result = await _onlineTimeTrackContext.Worklogs.Where(w => w.WorklogID == id).ToListAsync();

               return _onlineTimeTrackContext.Worklogs.FirstOrDefault(w => w.WorklogID == id);

           
        }

    




        public async Task<IEnumerable<Worklog>> GetProjectWorklog(int ProjectId, long? WorklogID, long? UserID, long? ProjectID,
           string ProjectTitle, string Feature, string FullName, string Address)
        {
            var worklog = _onlineTimeTrackContext.Worklogs

                    .Join(_onlineTimeTrackContext.Projects, w => w.ProjectID, p => p.ProjectID,
                    (w, p) =>
                    new Worklog
                    {
                        WorklogID = w.WorklogID,
                        UserID = w.UserID,
                        Feature = w.Feature,
                        ProjectID = p.ProjectID,
                        ProjectTitle = p.ProjectTitle


                    });
            worklog = worklog
                 .Join(_onlineTimeTrackContext.Users, comb => comb.UserID, u => u.UserID,
                (comb, u) =>
                new Worklog
                {
                    WorklogID = comb.WorklogID,
                    Feature = comb.Feature,
                    UserID = u.UserID,
                    FullName = u.FullName,
                    Address = u.Address,
                    ProjectID = comb.ProjectID,
                    ProjectTitle = comb.ProjectTitle
                });

            if (ProjectID != null)
            {
                worklog = worklog.Where(p => p.ProjectID == ProjectID);
            }

            if (ProjectTitle != null)
            {
                worklog = worklog.Where(p => p.ProjectTitle == ProjectTitle);
            }

            if (Feature != null)
            {
                worklog = worklog.Where(w => w.Feature == Feature);
            }


            if (UserID != null)
            {
                worklog = worklog.Where(u => u.UserID == UserID);
            }

            if (FullName != null)
            {
                worklog = worklog.Where(u => u.FullName == FullName);
            }

            if (Address != null)
            {
                worklog = worklog.Where(u => u.Address == Address);

            }
            var result = await worklog
                  .GroupBy(x => x.WorklogID)
                  .Select(g =>
                  new Worklog 
                  {
                      WorklogID = g.FirstOrDefault().WorklogID,
                      ProjectID = g.FirstOrDefault().ProjectID,
                      UserID = g.FirstOrDefault().UserID,
                      ProjectTitle = g.FirstOrDefault().ProjectTitle,
                      Feature = g.FirstOrDefault().Feature,
                      FullName = g.FirstOrDefault().FullName,
                      Address = g.FirstOrDefault().Address,



                      Users = g.Select(u => u.User).ToList()
                  }).ToListAsync();

            if (ProjectId != 0)
            {
                result = result.Take(ProjectId).ToList();
            }

            return result;
        }



        public async Task<IEnumerable<Worklog>> GetUserWorklog( long? WorklogID, int UserID, long? ProjectID, long? TimelogID,
              string ProjectTitle, string Feature, string FullName, string Address, int? EstimateWorkTime, DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd)
        {




            var worklog = _onlineTimeTrackContext.Worklogs

                .Join(_onlineTimeTrackContext.Projects, w => w.ProjectID, p => p.ProjectID,
                (w, p) =>
                new Worklog
                {
                    WorklogID = w.WorklogID,
                    UserID = w.UserID,
                    Feature = w.Feature,
                    EstimateWorkTime = w.EstimateWorkTime,
                    ProjectID = p.ProjectID,
                    ProjectTitle = p.ProjectTitle


                });
            worklog = worklog
                 .Join(_onlineTimeTrackContext.Users, comb => comb.UserID, u => u.UserID,
                (comb, u) =>
                new Worklog
                {
                    WorklogID = comb.WorklogID,
                    Feature = comb.Feature,
                    EstimateWorkTime = comb.EstimateWorkTime,
                    UserID = u.UserID,
                    FullName = u.FullName,
                    Address = u.Address,
                    ProjectID = comb.ProjectID,
                    ProjectTitle = comb.ProjectTitle
                });

            worklog = worklog
                .Join(_onlineTimeTrackContext.Timelogs, comb => comb.WorklogID, t => t.WorklogID,
                (comb, t) =>
                new Worklog
                {
                    WorklogID = comb.WorklogID,
                    Feature = comb.Feature,
                    EstimateWorkTime = comb.EstimateWorkTime,
                    UserID = comb.UserID,
                    FullName = comb.FullName,
                    Address = comb.Address,
                    ProjectID = comb.ProjectID,
                    ProjectTitle = comb.ProjectTitle,
                    TimeLog = t
                });



            if (ProjectID != null)
            {
                worklog = worklog.Where(p => p.ProjectID == ProjectID);
            }

            if (ProjectTitle != null)
            {
                worklog = worklog.Where(p => p.ProjectTitle == ProjectTitle);
            }

            if (Feature != null)
            {
                worklog = worklog.Where(w => w.Feature == Feature);
            }

            if (EstimateWorkTime != null)
            {
                worklog = worklog.Where(w => w.EstimateWorkTime == EstimateWorkTime);
            }


            if (UserID != null)
            {
                worklog = worklog.Where(u => u.UserID == UserID);
            }

            if (FullName != null)
            {
                worklog = worklog.Where(u => u.FullName == FullName);
            }

            if (Address != null)
            {
                worklog = worklog.Where(u => u.Address == Address);

            }
            var result = await worklog
                  .GroupBy(x => x.WorklogID)
                  .Select(g =>
                  new Worklog
                  {
                      WorklogID = g.FirstOrDefault().WorklogID,
                      ProjectID = g.FirstOrDefault().ProjectID,
                      UserID = g.FirstOrDefault().UserID,
                      ProjectTitle = g.FirstOrDefault().ProjectTitle,
                      Feature = g.FirstOrDefault().Feature,
                      EstimateWorkTime = g.FirstOrDefault().EstimateWorkTime,
                      FullName = g.FirstOrDefault().FullName,
                      Address = g.FirstOrDefault().Address,

                      Timelogs = g.Select(t => t.TimeLog).ToList()
                  }).ToListAsync();

           

            return result.ToList();

         }




        public async Task<IEnumerable<Worklog>> GetAll(string Feature)
        {
          var result = await _onlineTimeTrackContext.Worklogs.Where(f => f.Feature == Feature).ToListAsync();
          return result;
        }



       public async Task<Worklog> UpdateWorklog(Worklog WorklogID)
       {

        string query = @"UPDATE Worklogs SET ProjectID = @ProjectID, Date = @Date, EstimateWorkTime = @EstimateWorkTime,
                       Feature = @Feature WHERE UserID = @UserID AND WorklogID = @WorklogID";


         var result = _onlineTimeTrackContext.Database.ExecuteSqlCommand(query,

              new SqlParameter("@ProjectID", WorklogID.ProjectID),
              new SqlParameter("@Date", WorklogID.Date),
              new SqlParameter("@EstimateWorkTime", WorklogID.EstimateWorkTime),
              new SqlParameter("@Feature", WorklogID.Feature),
              new SqlParameter("@UserID", WorklogID.UserID),
              new SqlParameter("@WorklogID", WorklogID.WorklogID));

              if (result == 0)
              {
                  Console.Write("Invalid User");

              }
              else

                if (result == 1)
                {
                    Console.Write("Succesfully Updated");
                }


              var ExistingWorklog = _onlineTimeTrackContext.Worklogs.FirstOrDefault(x => x.WorklogID == WorklogID.WorklogID);
              await _onlineTimeTrackContext.SaveChangesAsync();

              return ExistingWorklog;
            }






            public async Task<IEnumerable<Worklog>> GetAllWorklogs(int start, int limit, long? WorklogID, long? UserID, long? ProjectID,
            string Feature, int? EstimateWorkTime, DateTime ActualWorkTimeStart, DateTime ActualWorkTimeEnd, string ProjectTitle, string FullName, string Address)
            {



                var worklog = _onlineTimeTrackContext.Worklogs

                    .Join(_onlineTimeTrackContext.Projects, w => w.ProjectID, p => p.ProjectID,
                    (w, p) =>
                    new Worklog
                    {
                        WorklogID = w.WorklogID,
                        UserID = w.UserID,
                        Feature = w.Feature,
                        EstimateWorkTime =w.EstimateWorkTime,
                        ProjectID = p.ProjectID,
                        ProjectTitle = p.ProjectTitle


                    });
                worklog = worklog
                     .Join(_onlineTimeTrackContext.Users, comb => comb.UserID, u => u.UserID,
                    (comb, u) =>
                    new Worklog
                    {
                        WorklogID = comb.WorklogID,
                        Feature = comb.Feature,
                        EstimateWorkTime = comb.EstimateWorkTime,
                        UserID = u.UserID,
                        FullName = u.FullName,
                        Address = u.Address,
                        ProjectID = comb.ProjectID,
                        ProjectTitle = comb.ProjectTitle
                    });

                worklog = worklog
                    .Join(_onlineTimeTrackContext.Timelogs, comb => comb.WorklogID, t => t.WorklogID,
                    (comb, t) =>
                    new Worklog
                    {
                        WorklogID = comb.WorklogID,
                        Feature = comb.Feature,
                        EstimateWorkTime = comb.EstimateWorkTime,
                        UserID = comb.UserID,
                        FullName = comb.FullName,
                        Address = comb.Address,
                        ProjectID = comb.ProjectID,
                        ProjectTitle = comb.ProjectTitle,
                        TimeLog = t
                    });



                if (ProjectID != null)
                {
                    worklog = worklog.Where(p => p.ProjectID == ProjectID);
                }

                if (ProjectTitle != null)
                {
                    worklog = worklog.Where(p => p.ProjectTitle == ProjectTitle);
                }

                if (Feature != null)
                {
                    worklog = worklog.Where(w => w.Feature == Feature);
                }

               if (EstimateWorkTime!= null)
               {
                worklog = worklog.Where(w => w.EstimateWorkTime == EstimateWorkTime);
               }


                if (UserID != 0)
                {
                    worklog = worklog.Where(u => u.UserID == UserID);
                }

                if (FullName != null)
                {
                    worklog = worklog.Where(u => u.FullName == FullName);
                }

                if (Address != null)
                {
                    worklog = worklog.Where(u => u.Address == Address);

                }
                var result = await worklog
                      .GroupBy(x => x.WorklogID)
                      .Select(g =>
                      new Worklog
                      {
                          WorklogID = g.FirstOrDefault().WorklogID,
                          ProjectID = g.FirstOrDefault().ProjectID,
                          UserID = g.FirstOrDefault().UserID,
                          ProjectTitle = g.FirstOrDefault().ProjectTitle,
                          Feature = g.FirstOrDefault().Feature,
                          EstimateWorkTime = g.FirstOrDefault().EstimateWorkTime,
                          FullName = g.FirstOrDefault().FullName,
                          Address = g.FirstOrDefault().Address,

                          Timelogs = g.Select(t => t.TimeLog).ToList()
                      }).ToListAsync();

                if (limit != 0)
                {
                    result = result.Skip(start).Take(limit).ToList();
                }

                return result;

            }

            public async Task<Worklog> DeleteWorklog(Worklog WorklogID)
            {
                _onlineTimeTrackContext.Worklogs.Remove(WorklogID);
                await _onlineTimeTrackContext.SaveChangesAsync();
                var ExistingWorklog = _onlineTimeTrackContext.Worklogs.FirstOrDefault(x => x.WorklogID == WorklogID.WorklogID);

                return ExistingWorklog;
            }

       




    }
}































































































