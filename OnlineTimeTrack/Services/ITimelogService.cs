using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack.Services
{
   public interface ITimelogService
    {

        Task<Timelog>Timelog(Timelog timelog);
        Task<Timelog> UpdateTimelog(Timelog TimelogID);
        Task<Timelog> DeleteTimelog(Timelog TimelogID);
        Task<Timelog> GetById(long? id);

        Task<IEnumerable<Timelog>> GetAllTimelogs(int start, int limit);
    }
}
