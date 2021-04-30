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

        public async Task<IEnumerable<Models.Movie>> GetAllAsync(
            StringAdvancedQueryParam nameQuery,
            StringAdvancedQueryParam directorQuery,
            IntAdvancedQueryParam priceQuery,
            IntAdvancedQueryParam profitQuery,
            DecimalAdvancedQueryParam ratingQuery
        )
        {
            var result = await dbContext.Movies.Where(m => 
                nameQuery.Passes(m.Name) &&
                directorQuery.Passes(m.Director) &&
                priceQuery.Passes(m.Price) &&
                profitQuery.Passes(m.Profit) &&
                ratingQuery.Passes(m.Rating)
            ).ToListAsync();
            return mapper.Map<IEnumerable<Models.Movie>>(result);
        }

        public async Task<Models.Movie> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
