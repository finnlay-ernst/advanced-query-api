using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedQueryAPI.Profiles
{
    public class MovieProfile : AutoMapper.Profile
    {
        public MovieProfile()
        {
            CreateMap<Db.Movie, Models.Movie>();
            CreateMap<Models.Movie, Db.Movie>();
        }
    }
}
