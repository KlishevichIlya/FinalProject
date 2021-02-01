using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Tag
    {
        public string Id { get; set; }
        public string TagName { get; set; }

        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}
