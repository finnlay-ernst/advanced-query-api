using AdvancedQueryAPI.Interfaces;
using AdvancedQueryAPI.Models;
using ExpandedQueryParams;
using ExpandedQueryParams.ModelBinding;
using ExpandedQueryParams.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;


namespace AdvancedQueryAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> logger;
        private readonly IMovieProvider provider;

        public MovieController(ILogger<MovieController> logger, IMovieProvider provider)
        {
            this.logger = logger;
            this.provider = provider;
        }

        /// <summary>
        /// Get all movies, includes more advanced query parameters.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Movie>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll (
            [ModelBinder(Name = nameof(Movie.Name), BinderType = typeof(AdvancedModelBinder))] StringAdvancedQueryParam nameQueryParam,
            [ModelBinder(Name = nameof(Movie.Director), BinderType = typeof(AdvancedModelBinder))] StringAdvancedQueryParam directorQueryParam,
            [ModelBinder(Name = nameof(Movie.Price), BinderType = typeof(AdvancedModelBinder))] IntAdvancedQueryParam priceQueryParam,
            [ModelBinder(Name = nameof(Movie.Profit), BinderType = typeof(AdvancedModelBinder))] IntAdvancedQueryParam profitQueryParam,
            [ModelBinder(Name = nameof(Movie.Rating), BinderType = typeof(AdvancedModelBinder))] DecimalAdvancedQueryParam ratingQueryParam
            )
        {
            //logger.LogInformation($"Query object received: {nameQueryParam}");
            var result = await provider.GetAllAsync(nameQueryParam, directorQueryParam, priceQueryParam, profitQueryParam, ratingQueryParam);
            return Ok(result);
        }

        /// <summary>
        /// Get movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByID (long id)
        {
            logger.LogInformation($"Received call to get by id");
            await GetByID(id);
            return Ok(new Movie());
        }
    }
}
