using OnlineTimeTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTimeTrack
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
         //   CreateMap<User, UserDto>();
            CreateMap<User, User>();
        }

        private void CreateMap<T1, T2>()
        {
            throw new NotImplementedException("Its Successfully entered");
        }
    }
}
