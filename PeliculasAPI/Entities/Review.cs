using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Entities
{
    public class Review : IId
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Score { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
