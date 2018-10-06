using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTask.Models
{
    public class FilterModel
    {
        public string Street { get; set; }
        public bool SelectAllAddress { get; set; }
        public bool SelectOnlyBadData { get; set; }
        public string HouseNumber { get; set; }
    }
}