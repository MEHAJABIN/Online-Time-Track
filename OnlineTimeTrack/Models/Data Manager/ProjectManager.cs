using OnlineTimeTrack.Contexts;
using OnlineTimeTrack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Models.Data_Manager
{
    public class ProjectManager : IProjectService<Project>
    {
        readonly OnlineTimeTrackContext _onlineTimeTrackContext;

        public ProjectManager(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
        }

        public IEnumerable<Project> GetAll()
        {
            return _onlineTimeTrackContext.Projects.ToList();
        }

        public Project Get(long id)
        {
            return _onlineTimeTrackContext.Projects.FirstOrDefault(e => e.ProjectID == id);
        }

        public void Add(Project entity)
        {
            _onlineTimeTrackContext.Projects.Add(entity);
            _onlineTimeTrackContext.SaveChanges();
        }

        public void Update(Project project, Project entity)
        {
           project.ProjectTitle = entity.ProjectTitle;
            _onlineTimeTrackContext.SaveChanges();
        }

        public void Delete(Project project)
        {
            _onlineTimeTrackContext.Projects.Remove(project);
            _onlineTimeTrackContext.SaveChanges();
        }

    }
}
