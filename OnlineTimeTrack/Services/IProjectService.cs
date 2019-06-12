using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OnlineTimeTrack.Services
{
    public interface IProjectService
    {
        Task<Project> Project(Project project);


        IEnumerable<Project> GetAll();
        Project GetById(long id);
        Project Create(Project projectID, string ProjectTitle);
        void Update(Project project, string ProjectTitle = null);
       
    }
}
