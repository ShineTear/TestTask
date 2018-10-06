using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTask.Models
{
    public class AppartmentCreateModel
    {
        public string Street { get; set; }
        public int HomeNumber { get; set; }
        public string Building { get; set; }
        public int Room { get; set; }
    }
}