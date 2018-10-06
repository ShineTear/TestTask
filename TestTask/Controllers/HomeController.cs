using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestTask.DataBase.Context;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            CultureInfo ci = new CultureInfo("ru");
            Thread.CurrentThread.CurrentCulture = ci;
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AppartmentList(FilterModel filter)
        {
            var model = new List<AppartmentViewModel>();
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
                            model.Add(new AppartmentViewModel()
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


                        model.Add(new AppartmentViewModel()
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
            return PartialView(model);
        }

        public ActionResult CreateAppartment()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateNewAppartment(AppartmentCreateModel model)
        {
            using (AppartmentContext db = new AppartmentContext())
            {
                var address = model.Street + " " + model.HomeNumber + (!string.IsNullOrEmpty(model.Building) ? "/" + model.Building : "") + " " + model.Room;
                db.Appartments.Add(new DataBase.Entities.Appartment() { Address = address });
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddNewMeter(int Id)
        {
            return PartialView(Id);
        }

        [HttpPost]
        public ActionResult AddNewMeter(CreateMeterModel model)
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
                address.Meters.Add(new DataBase.Entities.Meter()
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
            return RedirectToAction("Index");
        }

        public ActionResult AddNewValue(int Id)
        {
            var model = new AddMeterValueModel();
            using (AppartmentContext db = new AppartmentContext())
            {
                var meter = db.Appartments.First(x => x.Id == Id).Meters.OrderBy(x => x.InstallationDate).Last();
                model.MeterId = meter.Id;
                model.LastValue = meter.LastValue;
                model.NextCheck = DateTime.Today.AddMonths(1);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AddNewValue(AddMeterValueModel model)
        {
            var modelValue = double.Parse(model.Value.Replace(".", ","));
            modelValue = Math.Round(modelValue, 2);
            using (AppartmentContext db = new AppartmentContext())
            {
                var meter = db.Meters.First(x => x.Id == model.MeterId);
                if (modelValue < meter.LastValue) { return PartialView("Error", "Переданные данные меньше преждних значений"); }
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
            return RedirectToAction("Index");
        }

        public ActionResult ShowAllChange(int Id)
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
            return PartialView(model);
        }

        public ActionResult Error(string text)
        {
            return PartialView(text);
        }

        [ChildActionOnly]
        public ActionResult Filters()
        {
            var model = new FilterViewModel();

            using (AppartmentContext db = new AppartmentContext())
            {
                model.Streets = db.Appartments.Select(x => x.Address).ToList().Select(x => x.Split(' ').First()).ToList();
                model.Streets.Add("");
                model.Streets = model.Streets.Distinct().OrderBy(x => x).ToList();
            }
            return PartialView(model);
        }
        
        [HttpPost]
        public JsonResult LoadHouseNumber(string street)
        {
            var result = new List<string>();

            using (AppartmentContext db = new AppartmentContext())
            {
                result.AddRange(db.Appartments.Where(x => x.Address.Contains(street)).Select(x => x.Address).ToList().Select(x => (x.Split(' '))[1]).ToList());
            }
            return Json(result);
        }
    }
}