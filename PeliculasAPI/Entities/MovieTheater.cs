﻿using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Entities
{
    public class MovieTheater : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public Point Location { get; set; }
        public List<MoviesMovieTheaters> MoviesMovieTheaters { get; set; }
    }
}
