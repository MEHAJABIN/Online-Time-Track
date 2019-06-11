using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineTimeTrack.Contexts;


namespace OnlineTimeTrack.Models.Data_Manager
{
    public class WorklogManager : IWorklogService<Worklog>
    {
        readonly OnlineTimeTrackContext _onlineTimeTrackContext;

        public WorklogManager(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
        }

       
        public Worklog Get(long id)
        {
            return _onlineTimeTrackContext.Worklogs.FirstOrDefault(e => e.WorklogID == id);
        }

        public void Add(Worklog entity)
        {
            _onlineTimeTrackContext.Worklogs.Add(entity);
            _onlineTimeTrackContext.SaveChanges();
        }

       

      

        public void Delete(Worklog worklog)
        {
            _onlineTimeTrackContext.Worklogs.Remove(worklog);
            _onlineTimeTrackContext.SaveChanges();
        }

    }
}


