using Microsoft.EntityFrameworkCore;
using OnlineTimeTrack.Contexts;
using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Services
{
    public class TimelogService : ITimelogService
    {
        private readonly OnlineTimeTrackContext _onlineTimeTrackContext;
       

        public TimelogService(OnlineTimeTrackContext onlineTimeTrackContext)
        {
            _onlineTimeTrackContext = onlineTimeTrackContext;
          


        }


        public async Task<Timelog> Timelog(Timelog timelog)
        {
            // save the Timelog
            var Timelog = await _onlineTimeTrackContext.Timelogs.AddAsync(timelog);

            timelog.WorklogID = timelog.WorklogID;

            timelog.ActualWorkTimeStart = timelog.ActualWorkTimeStart;

            timelog.ActualWorkTimeEnd = timelog.ActualWorkTimeEnd;
            timelog.DateAdded = DateTime.UtcNow;
            timelog.DateModified = DateTime.UtcNow;

            await _onlineTimeTrackContext.SaveChangesAsync();

            // return thetimelog
            return timelog;
        }


        //Get particular timelog
        public async Task<Timelog> GetById(long? id)
        {
            var result = await _onlineTimeTrackContext.Timelogs.Where(t => t.TimelogID == id).ToListAsync();

            return _onlineTimeTrackContext.Timelogs.FirstOrDefault(w => w.WorklogID == id);
        }





        //Update Timelog
        public async Task<Timelog> UpdateTimelog(Timelog TimelogID)
        {
            _onlineTimeTrackContext.Timelogs.Update(TimelogID);
            await _onlineTimeTrackContext.SaveChangesAsync();
            var ExistingTimelog = _onlineTimeTrackContext.Timelogs.FirstOrDefault(x => x.TimelogID == TimelogID.TimelogID);

            return ExistingTimelog;
        }




        //Delete Timelog
        public async Task<Timelog> DeleteTimelog(Timelog TimelogID)
        {
            _onlineTimeTrackContext.Timelogs.Remove(TimelogID);
            await _onlineTimeTrackContext.SaveChangesAsync();
            var ExistingTimelog = _onlineTimeTrackContext.Timelogs.FirstOrDefault(x => x.TimelogID == TimelogID.TimelogID);

            return ExistingTimelog;
        }
    }
}
