using AdvancedQueryAPI.Models;
using ExpandedQueryParams.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedQueryAPI.Interfaces
{
    public interface IMovieProvider
    {

        public Task<IEnumerable<Movie>> GetAllAsync(IntAdvancedQueryParam priceQuery);
        public Task<Movie> GetByIdAsync(int id);

    }
}
