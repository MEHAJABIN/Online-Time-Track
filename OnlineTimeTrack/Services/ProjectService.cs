using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTimeTrack.Services
{
    public class ProjectService : IProjectService
    {

        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;
       

        public ProjectService(OnlineTimeTrackContext onlineTimeTrackContext)
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


        public IEnumerable<Project> GetAll()
        {
            return _onlineTimeTrackContext.Projects;
        }


        public Project GetById(long id)
        {
            return _onlineTimeTrackContext.Projects.Find(id);
        }



        public Project Create(Project project, string ProjectTitle)
        {
            // Add ProjectTitle
            if (string.IsNullOrEmpty(ProjectTitle))
                throw new AppException("ProjectTitle is required");

            if (_onlineTimeTrackContext.Projects.Any(x => x.ProjectTitle == project.ProjectTitle))
                throw new AppException("ProjectTitle \"" + project.ProjectTitle + "\" is already taken");

            project.ProjectTitle = ProjectTitle;
            _onlineTimeTrackContext.Projects.Add(project);
            _onlineTimeTrackContext.SaveChanges();
            return project;
        }



        void Update(Project project, string ProjectID = null)
        {
            var Project = _onlineTimeTrackContext.Projects.Find(project.ProjectID);

            if (project == null)
                throw new AppException("ProjectTitle not found");

            if (project.ProjectTitle != project.ProjectTitle)
            {
                // ProjectTitle has changed so check if the new Project is already taken
                if (_onlineTimeTrackContext.Projects.Any(x => x.ProjectTitle == project.ProjectTitle))
                    throw new AppException("ProjectTitle " + project.ProjectTitle + " is already taken");
            }

        }


        public async Task<Project> Project(Project project)
        {
            // save the project
            var addedProject = await _onlineTimeTrackContext.Projects.AddAsync(project);
            await _onlineTimeTrackContext.SaveChangesAsync();
            addedProject.Entity.ProjectTitle = project.ProjectTitle;

            // return the project
            return addedProject.Entity;
        }

    }


}












































































