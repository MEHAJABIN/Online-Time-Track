﻿using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace OnlineTimeTrack.Services
{
    public class projectService : IProjectService
    {
        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;
        private readonly WorklogService _worklogService;


        public projectService(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
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

        void IProjectService.Update(Project project, string ProjectTitle)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

    }


}



































