using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestTask.DataBase.Entities;

namespace TestTask.DataBase.Context
{
    public class AppartmentContext: DbContext
    {
        public AppartmentContext()
            :base(DataBaseConfig.DataBaseName)
        { }

        public DbSet<Appartment> Appartments { get; set; }

        public DbSet<Meter> Meters { get; set; }

        public DbSet<MeterData> MeterDatas { get; set; }
    }
}