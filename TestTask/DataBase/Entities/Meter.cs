using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestTask.DataBase.Entities
{
    public class Meter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double StartValue { get; set; }
        public double LastValue { get; set; }
        public DateTime InstallationDate { get; set; }
        public string MeterNumber { get; set; }
        public DateTime? LastCheckData { get; set; }
        public DateTime? NextCheckData { get; set; }

        public int? LastMeterId { get; set; }

        [ForeignKey("Appartment")]
        public int AppartmentId { get; set; }
        public Appartment Appartment { get; set; }
        public virtual ICollection<MeterData> MeterDatas { get; set; }

        public Meter()
        {
            MeterDatas = new List<MeterData>();
        }
    }
}