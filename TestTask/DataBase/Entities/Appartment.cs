using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTask.DataBase.Entities
{
    public class Appartment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Address {get; set;}

        public virtual ICollection<Meter> Meters { get; set; }

        public Appartment()
        {
            Meters = new List<Meter>();
        }
    }
}