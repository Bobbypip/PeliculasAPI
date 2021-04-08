using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class ActorMovieDetailDTO
    {
        public int ActorId { get; set; }
        public string Character { get; set; }
        public string NamePerson { get; set; }
    }
}
