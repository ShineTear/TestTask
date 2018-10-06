using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestTask.Models
{
    public class ShowAllChangeViewModel
    {
        [DisplayName("Адрес квартиры")]
        public string Address { get; set; }
        public List<MeterViewModel> Meters { get; set; }
    }

    public class MeterViewModel
    {
        [DisplayName("Идентификатор счетчика")]
        public string MeterNumber { get; set; }
        [DisplayName("Начальное значение")]
        public double StartValue { get; set; }
        [DisplayName("Дата установки")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime InstallationDate { get; set; }
        
        public List<MeterDataView> MeterDatas { get; set; }
    }

    public class MeterDataView
    {
        [DisplayName("Значение")]
        public double Value { get; set; }
        [DisplayName("Дата изменения")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ChangeData { get; set; }
    }
}