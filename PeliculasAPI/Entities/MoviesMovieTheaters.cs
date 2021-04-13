using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Entities
{
    public class MoviesMovieTheaters
    {
        public int MovieId { get; set; }
        public int MovieTheaterId { get; set; }
        public Movie Movie { get; set; }
        public MovieTheater MovieTheater { get; set; }
    }
}
