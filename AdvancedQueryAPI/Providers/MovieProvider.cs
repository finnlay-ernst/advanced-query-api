using AdvancedQueryAPI.Db;
using AdvancedQueryAPI.Interfaces;
using AutoMapper;
using Bogus;
using ExpandedQueryParams;
using ExpandedQueryParams.QueryParams;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedQueryAPI.Providers
{
    public class MovieProvider : IMovieProvider
    {
        private readonly ILogger<MovieProvider> logger;
        private readonly MovieDbContext dbContext;
        private readonly IMapper mapper;
        public MovieProvider(ILogger<MovieProvider> logger, MovieDbContext dbContext, IMapper mapper)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;

            CreateSeedData();
        }

        private void CreateSeedData()
        {
            if (dbContext.Movies.Any()) { return; }
            var f = new Faker("en");
            string directorName = f.Name.FullName();
            for (int i = 1; i < 201; i++) 
            {
                if (i % 5 == 0) 
                {
                    directorName = f.Name.FullName();
                }
                dbContext.Movies.Add(new Db.Movie
                {
                    Id = i,
                    Name = f.Hacker.Noun() + " " + f.Hacker.Adjective(),
                    Director = directorName,
                    Price = f.Random.Int(0, 100),
                    Profit = f.Random.Int(10000, Int32.MaxValue),
                    Rating = f.Random.Decimal(0, 10)
                }); 
            }
            dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Models.Movie>> GetAllAsync(IEnumerable<object> queryParams)
        {
            //var result = await dbContext.Movies.ToListAsync();
            IntAdvancedQueryParam aqpId = (IntAdvancedQueryParam)queryParams.Where(o => ((AdvancedQueryParam) o).Name == "Id").First();
            List<AdvancedQueryParam> advancedQueryParams = queryParams.Select<object, AdvancedQueryParam>((o, i) => {
                var prop = typeof(Movie).GetProperty(((AdvancedQueryParam)o).Name);
                
                if (prop.PropertyType == typeof(int))
                {
                    return new IntAdvancedQueryParam(prop.Name);
                }
                else if (prop.PropertyType == typeof(decimal))
                {
                    return new DecimalAdvancedQueryParam(prop.Name);
                }
                else
                {
                    return new StringAdvancedQueryParam(prop.Name);
                }
            }).ToList();
            //var aqp = advancedQueryParams.Find(qp => qp.Name == newField.Name);
            //return aqp == null || agg && aqp.Passes(typeof(Movie).GetProperty(newField.Name).GetValue(m));
            
            // C# CAN"T CONVERT THIS TO SQL, NEED A DIFFERENT APPROACH
            var result = await dbContext.Movies.Where(m => typeof(Movie).GetProperties()
            .Aggregate(true, (agg, newField) => 
                agg && (advancedQueryParams.Find(qp => qp.Name == newField.Name) == null || advancedQueryParams.Find(qp => qp.Name == newField.Name).Passes(typeof(Movie).GetProperty(newField.Name).GetValue(m))))).ToListAsync();
            return mapper.Map<IEnumerable<Models.Movie>>(result);
        }

        public async Task<Models.Movie> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
