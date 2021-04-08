using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class MovieDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool OnCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<GenreDTO> Genres { get; set; }
        public List<ActorMovieDetailDTO> Actors { get; set; }
    }
}
