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
            var addedTimelog = await _onlineTimeTrackContext.Timelogs.AddAsync(timelog);

            addedTimelog.Entity.WorklogID = timelog.WorklogID;

            addedTimelog.Entity.ActualWorkTimeStart = timelog.ActualWorkTimeStart;

            addedTimelog.Entity.ActualWorkTimeEnd = timelog.ActualWorkTimeEnd;

            await _onlineTimeTrackContext.SaveChangesAsync();

            // return thetimelog
            return timelog;
        }


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





        public async Task<Timelog> DeleteTimelog(Timelog TimelogID)
        {
            _onlineTimeTrackContext.Timelogs.Remove(TimelogID);
            await _onlineTimeTrackContext.SaveChangesAsync();
            var ExistingTimelog = _onlineTimeTrackContext.Timelogs.FirstOrDefault(x => x.TimelogID == TimelogID.TimelogID);

            return ExistingTimelog;
        }




        public async Task<IEnumerable<Timelog>> GetAllTimelogs(int start, int limit)
        {

            if (start == 0 & limit == 0)
            {
                var timelog = await _onlineTimeTrackContext.Timelogs.ToListAsync();
                return timelog;
            }
            else
            {
                var result = await _onlineTimeTrackContext.Timelogs.Skip(start).Take(limit).ToListAsync();

                return result;
            }

        }
    }
}
