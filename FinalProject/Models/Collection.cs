using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Collection
    {
        public string id { get; set; }
        public string name { get; set; } 
        public string description { get; set; }
        public string url { get; set; }
        [NotMapped]
        public IFormFile formFile { get; set; }
        public string ImageStorageName { get; set; }
        public User User { get; set; }
        

    }
}
