using AdvancedQueryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedQueryAPI.Interfaces
{
    public interface IMovieProvider
    {

        public Task<IEnumerable<Movie>> GetAllAsync(IEnumerable<object> queryParams);
        public Task<Movie> GetByIdAsync(int id);

    }
}
