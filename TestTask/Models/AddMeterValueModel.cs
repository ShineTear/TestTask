using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestTask.Models
{
    public class AddMeterValueModel
    {
        public int MeterId { get; set; }
        [Required]
        public string Value { get; set; }
        public double LastValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime NextCheck { get; set; }

    }
}