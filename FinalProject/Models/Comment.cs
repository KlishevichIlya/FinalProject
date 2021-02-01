using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public string Nick { get; set; }
        public Item Item { get; set; }
        public string ItemId { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
       
    }
}
