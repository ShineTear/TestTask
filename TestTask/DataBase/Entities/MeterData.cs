using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTask.DataBase.Entities
{
    public class MeterData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime CheckData { get; set; }


        public int MeterId { get; set; }
        public virtual Meter Meter { get; set; }
    }
}