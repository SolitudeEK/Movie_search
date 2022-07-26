using Microsoft.AspNetCore.Mvc;
using Movie_search.Logic;
using Movie_search.Model;
using System.Collections.Generic;


namespace Movie_search.Controllers
{
    [Route("search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IMDBReader reader = new IMDBReader();
        private const int limit = 10;

        [HttpGet("{title}")]
        public List<Movie> GetMovieByTitle(string title)
        {
            if (title.Length < 5)
                return null;
            else
            {
                List<Movie> movies = reader.FindByTitle(title);
                if (movies.Count > limit)
                    return movies.GetRange(0, limit);
                else
                    return movies;
            }
                
        }

        [HttpGet("extended/{id}")]
        public Movie GetExtendedInfoByID(string id)
        {
            if(id.StartsWith("tt"))
                return reader.ExtendedInfoByID(id);
            else
                return null;
        }
    }
}
