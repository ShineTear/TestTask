using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestTask.DataBase.Context;
using TestTask.Models;

namespace TestTask.DataBase.Code
{
    public static class FunctionWithDB
    {
        public static List<AppartmentViewModel> GetAllAppartmentsDataWithFilter(FilterModel filter)
        {
            var result = new List<AppartmentViewModel>();
            using (AppartmentContext db = new AppartmentContext())
            {
                var appartments = db.Appartments.ToList();
                if (!string.IsNullOrEmpty(filter.Street) && !filter.SelectAllAddress) appartments = appartments.Where(x => x.Address.Contains(filter.Street)).ToList();
                if (!string.IsNullOrEmpty(filter.HouseNumber) && !filter.SelectAllAddress) appartments = appartments.Where(x => x.Address.Contains(filter.HouseNumber)).ToList();
                if (filter.SelectOnlyBadData)
                {
                    foreach (var appart in appartments.Where(x => x.Meters.Count > 0))
                    {
                        var meter = appart.Meters.OrderBy(x => x.Id).Last();
                        if (meter.NextCheckData <= DateTime.Today)
                            result.Add(new AppartmentViewModel()
                            {
                                Address = appart.Address,
                                AddressId = appart.Id,
                                MeterNumber = meter.MeterNumber,
                                LastValue = meter.LastValue,
                                LastCheckData = meter.LastCheckData,
                                NextCheckData = meter.NextCheckData
                            });
                    }
                }
                else
                {
                    foreach (var appart in appartments)
                    {
                        var meter = appart.Meters.Count > 0 ? appart.Meters.OrderBy(x => x.Id).Last() : null;


                        result.Add(new AppartmentViewModel()
                        {
                            Address = appart.Address,
                            AddressId = appart.Id,
                            MeterNumber = meter == null ? null : meter.MeterNumber,
                            LastValue = meter == null ? 0 : meter.LastValue,
                            LastCheckData = meter == null ? null : meter.LastCheckData,
                            NextCheckData = meter == null ? null : meter.NextCheckData
                        });
                    }

                }
            }
            return result;
        }

        public static void CreateNewAppartment(AppartmentCreateModel model)
        {
            using (AppartmentContext db = new AppartmentContext())
            {
                var address = model.Street + " " + model.HomeNumber + (!string.IsNullOrEmpty(model.Building) ? "/" + model.Building : "") + " " + model.Room;
                db.Appartments.Add(new Entities.Appartment() { Address = address });
                db.SaveChanges();
            }
            return ;
        }

        public static void AddNewMeter(CreateMeterModel model)
        {
            using (AppartmentContext db = new AppartmentContext())
            {
                var address = db.Appartments.First(x => x.Id == model.AppartmentId);
                double startMeterData = 0;
                int? lastMeterId = null;
                if (address.Meters.Count != 0)
                {
                    var lastMeter = address.Meters.Last();
                    lastMeterId = lastMeter.Id;
                    startMeterData = lastMeter.LastValue;
                }
                address.Meters.Add(new Entities.Meter()
                {
                    AppartmentId = address.Id,
                    MeterNumber = model.MeterNumber,
                    LastCheckData = null,
                    NextCheckData = DateTime.Today.AddMonths(1),
                    InstallationDate = DateTime.Now,
                    StartValue = startMeterData,
                    LastMeterId = lastMeterId,
                    LastValue = 0
                });
                db.SaveChanges();
            }
            return ;
        }

        public static AddMeterValueModel GetDataForAddNewValue(int Id)
        {
            var model = new AddMeterValueModel();
            using (AppartmentContext db = new AppartmentContext())
            {
                var meter = db.Appartments.First(x => x.Id == Id).Meters.OrderBy(x => x.InstallationDate).Last();
                model.MeterId = meter.Id;
                model.LastValue = meter.LastValue;
                model.NextCheck = DateTime.Today.AddMonths(1);
            }
            return model;
        }

        public static string AddNewValue(AddMeterValueModel model)
        {
            var modelValue = double.Parse(model.Value.Replace(".", ","));
            modelValue = Math.Round(modelValue, 2);
            using (AppartmentContext db = new AppartmentContext())
            {
                var meter = db.Meters.First(x => x.Id == model.MeterId);
                if (modelValue < meter.LastValue) { return "Переданные данные меньше преждних значений"; }
                meter.MeterDatas.Add(new DataBase.Entities.MeterData()
                {
                    CheckData = DateTime.Now,
                    MeterId = meter.Id,
                    Value = modelValue
                });
                meter.LastCheckData = DateTime.Now;
                meter.LastValue = modelValue;
                meter.NextCheckData = model.NextCheck >= DateTime.Today ? model.NextCheck : DateTime.Today.AddMonths(1);
                db.SaveChanges();
            }
            return "";
        }

        public static ShowAllChangeViewModel ShowAllChangeInOneAppartment(int Id)
        {
            var model = new ShowAllChangeViewModel();
            using (AppartmentContext db = new AppartmentContext())
            {
                var appartment = db.Appartments.First(x => x.Id == Id);
                model.Address = appartment.Address;
                model.Meters = new List<MeterViewModel>();
                foreach (var meter in appartment.Meters)
                {
                    var meterData = meter.MeterDatas.Select(x => new MeterDataView() { ChangeData = x.CheckData, Value = x.Value }).ToList();
                    model.Meters.Add(new MeterViewModel()
                    {
                        InstallationDate = meter.InstallationDate,
                        MeterDatas = meterData,
                        MeterNumber = meter.MeterNumber,
                        StartValue = meter.StartValue
                    });
                }
            }
            return model;
        }

        public static FilterViewModel DataForFilters()
        {
            var model = new FilterViewModel();

            using (AppartmentContext db = new AppartmentContext())
            {
                model.Streets = db.Appartments.Select(x => x.Address).ToList().Select(x => x.Split(' ').First()).ToList();
                model.Streets.Add("");
                model.Streets = model.Streets.Distinct().OrderBy(x => x).ToList();
            }
            return model;
        }

        public static List<string> LoadHouseNumber(string street)
        {
            var result = new List<string>();

            using (AppartmentContext db = new AppartmentContext())
            {
                result.AddRange(db.Appartments.Where(x => x.Address.Contains(street)).Select(x => x.Address).ToList().Select(x => (x.Split(' '))[1]).Distinct().ToList());
            }
            return result;
        }
    }
}
