using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UnderControl.Models
{
    public class MyData 
    {
        public DateTime Date { get; set; }
        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$")]
        public double Time { get; set; }
        public int ID { get; set; }
        //[Column(TypeName = "decimal(18, 2)")]

        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$")]
        public double Temperature { get; set; }
        public string Feeling { get; set; }
        public string Color { get; set; }
        public string Consistency { get; set; }
        public string Quantity { get; set; }
        public string Cervix { get; set; }
        public string Bleeding { get; set; }
        public string Sex { get; set; }
        public string Others { get; set; }

        public MyData()
        {

        }

    }

}
