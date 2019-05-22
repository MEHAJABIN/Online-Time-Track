using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Contexts;

namespace OnlineTimeTrack.Services
{
    public class ProjectService : IProjectService
    {
        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;


        /*public ProjectService(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
        }*/
        public async Task<Project> Project(Project project)
        {

        }

    }



}
