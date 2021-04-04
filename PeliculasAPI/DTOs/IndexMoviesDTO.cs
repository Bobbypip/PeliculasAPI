using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class IndexMoviesDTO
    {
        public List<MovieDTO>  FutureReleases { get; set; }
        public List<MovieDTO> OnCinemas { get; set; }
    }
}
