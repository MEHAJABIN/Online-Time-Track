using OnlineTimeTrack.Contexts;
using System;
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

namespace OnlineTimeTrack.Services
{
    public class ProjectService : IProjectService
    {

        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;
       

        public ProjectService(OnlineTimeTrackContext onlineTimeTrackContext,IUserService userService,ITimelogService timelogService)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
            
        }





        public async Task<Project> UpdateProject(Project ProjectID)
        {
            _onlineTimeTrackContext.Projects.Update(ProjectID);
            await _onlineTimeTrackContext.SaveChangesAsync();
            var ExistingProject = _onlineTimeTrackContext.Projects.FirstOrDefault(x => x.ProjectID == ProjectID.ProjectID);

            return ExistingProject;
        }


        public async Task<Project> DeleteProject(Project ProjectID)
        {
            _onlineTimeTrackContext.Projects.Remove(ProjectID);
            await _onlineTimeTrackContext.SaveChangesAsync();
            var ExistingProject = _onlineTimeTrackContext.Projects.FirstOrDefault(x => x.ProjectID == ProjectID.ProjectID);

            return ExistingProject;
        }

        public async Task<Project> GetById(long? id)
        {
            var result = await _onlineTimeTrackContext.Projects.Where(p => p.ProjectID == id).ToListAsync();

            return _onlineTimeTrackContext.Projects.FirstOrDefault(p => p.ProjectID == id);
        }



      





        public Project Create(Project project, string ProjectTitle)
        {
            // Add ProjectTitle
            if (string.IsNullOrEmpty(ProjectTitle))
                throw new Exception("ProjectTitle is required");

            if (_onlineTimeTrackContext.Projects.Any(x => x.ProjectTitle == project.ProjectTitle))
                throw new Exception("ProjectTitle \"" + project.ProjectTitle + "\" is already taken");

            project.ProjectTitle = ProjectTitle;
            project.DateAdded = DateTime.UtcNow;
            project.DateModified = DateTime.UtcNow;
            _onlineTimeTrackContext.Projects.Add(project);
            _onlineTimeTrackContext.SaveChanges();
            return project;
        }



        void Update(Project project, string ProjectID = null)
        {
            var Project = _onlineTimeTrackContext.Projects.Find(project.ProjectID);

            if (project == null)
                throw new Exception("ProjectTitle not found");

            if (project.ProjectTitle != project.ProjectTitle)
            {
                // ProjectTitle has changed so check if the new Project is already taken
                if (_onlineTimeTrackContext.Projects.Any(x => x.ProjectTitle == project.ProjectTitle))
                    throw new Exception("ProjectTitle " + project.ProjectTitle + " is already taken");
            }

        }


        public async Task<Project> Project(Project project)
        {
            // save the project
            var Project = await _onlineTimeTrackContext.Projects.AddAsync(project);
            project.DateAdded = DateTime.UtcNow;
            project.DateModified = DateTime.UtcNow;
            project.ProjectTitle = project.ProjectTitle;
            await _onlineTimeTrackContext.SaveChangesAsync();
           

            // return the project
            return project;
        }




       

        public async Task<IEnumerable<Project>> GetAllProjects(int start, int limit)
        {

            if (start == 0 & limit == 0)
            {
                var Project = await _onlineTimeTrackContext.Projects.ToListAsync();
                return Project;

            }

            else
            {
                var result = await _onlineTimeTrackContext.Projects.Skip(start).Take(limit).ToListAsync();

                return result;
            }

        }
    }


}












































































