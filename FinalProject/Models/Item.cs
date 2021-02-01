using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Tag> Tags { get;set; }
       

        /// <summary>
        ///  Optional Fields
        /// </summary>
        public int FInt1 { get; set; }
        public string FInt1_Name { get; set; }

        public int FInt2 { get; set; }
        public string FInt2_Name { get; set; }

        public int FInt3 { get; set; }
        public string FInt3_Name { get; set; }

        [Column(TypeName = "varchar(75)")]
        public string FStr1 { get; set; }
        public string FStr1_Name { get; set; }

        [Column(TypeName = "varchar(75)")]
        public string FStr2 { get; set; }
        public string FStr2_Name { get; set; }

        [Column(TypeName = "varchar(75)")]
        public string FStr3 { get; set; }
        public string FStr3_Name { get; set; }


        public string FText1 { get; set; }
        public string FText1_Name { get; set; }

        public string FText2 { get; set; }
        public string FText2_Name { get; set; }

        public string FText3 { get; set; }
        public string FText3_Name { get; set; }


        public DateTime FDate1 { get; set; }
      
        public string FDate1_Name { get; set; }

        public DateTime FDate2 { get; set; }
        public string FDate2_Name { get; set; }

        public DateTime FDate3 { get; set; }
        public string FDate3_Name { get; set; }


        public bool FChBox1 { get; set; }
        public string FChBox1_Name { get; set; }

        public bool FChBox2 { get; set; }
        public string FChBox2_Name { get; set; }

        public bool FChBox3 { get; set; }
        public string FChBox3_Name { get; set; }

        
        public uint  BitMask { get; set; }

        public string CollectionId { get; set; }
        public Collection Collection { get; set; }

       
        public List<Like> Like { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
