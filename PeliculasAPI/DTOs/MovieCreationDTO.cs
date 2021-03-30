using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Helpers;
using PeliculasAPI.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class MovieCreationDTO
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool OnCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        [FileWeightValidation(maxWeightInMegaBytes: 4)]
        [FileTypeValidation(fileTypeGroup: FileTypeGroup.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorMoviesCreationDTO>>))]
        public List<ActorMoviesCreationDTO> Actors { get; set; }
    }
}
