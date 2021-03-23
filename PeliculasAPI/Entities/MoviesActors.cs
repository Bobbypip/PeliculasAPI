using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Entities
{
    public class MoviesActors
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public string Character { get; set; }
        public int Sort { get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
    }
}
