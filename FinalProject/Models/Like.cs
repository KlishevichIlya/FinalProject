using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Like
    {
        public string Id { get; set; }
        public bool isLike { get; set; }

        public Item Item { get; set; }
        public string ItemId { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
