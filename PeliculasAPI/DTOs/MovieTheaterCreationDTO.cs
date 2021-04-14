﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class MovieTheaterCreationDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
    }
}
