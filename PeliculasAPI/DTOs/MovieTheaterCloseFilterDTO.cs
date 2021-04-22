using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class MovieTheaterCloseFilterDTO
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
        private int distanceInKM = 10;
        private int distanceMaxKM = 50;
        public int DistanceInKM
        {
            get
            {
                return distanceInKM;
            }
            set
            {
                distanceInKM = (value > distanceMaxKM) ? distanceMaxKM : value;
            }
        }
    }
}
