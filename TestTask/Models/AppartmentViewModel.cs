using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestTask.Models
{
    public class AppartmentViewModel
    {
        [DisplayName("Адрес квартиры")]
        public string Address { get; set; }

        [DisplayName("Номер счетчика")]
        public string MeterNumber { get; set; }

        [DisplayName("Дата последней проверки")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? LastCheckData { get; set; }

        [DisplayName("Дата следующей проверки")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? NextCheckData { get; set; }

        [DisplayName("Последние показания")]
        public double? LastValue { get; set; }
        public int AddressId { get; set; }
    }
}