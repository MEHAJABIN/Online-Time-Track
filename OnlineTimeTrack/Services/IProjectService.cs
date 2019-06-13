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
        Task<Project> UpdateProject(Project ProjectID);
        Task<Project> DeleteProject(Project ProjectID);


        IEnumerable<Project> GetAll();
        Project GetById(long id);
    }
}



