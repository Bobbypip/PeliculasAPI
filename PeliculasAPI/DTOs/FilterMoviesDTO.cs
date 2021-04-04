using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class FilterMoviesDTO
    {
        public int Page { get; set; } = 1;
        public int recordsQuantityPerPage { get; set; } = 10;
        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, RecordsQuantityPerPage = recordsQuantityPerPage }; }
        }

        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool OnCinemas { get; set; }
        public bool FutureReleases { get; set; }
    }
}
