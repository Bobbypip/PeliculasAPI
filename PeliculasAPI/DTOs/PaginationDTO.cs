using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        private int recordsQuantityPerPage = 10;
        private readonly int maxRecordsQuantityPerPage = 50;
        
        public int RecordsQuantityPerPage
        {
            get => recordsQuantityPerPage;
            set 
            {
                recordsQuantityPerPage = (value > maxRecordsQuantityPerPage) ? maxRecordsQuantityPerPage : value;
            }
        }
    }
}
