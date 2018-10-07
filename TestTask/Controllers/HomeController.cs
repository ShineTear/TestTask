using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestTask.DataBase.Code;
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
            var model = FunctionWithDB.GetAllAppartmentsDataWithFilter(filter);
            return PartialView(model);
        }

        public ActionResult CreateAppartment()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateNewAppartment(AppartmentCreateModel model)
        {
            FunctionWithDB.CreateNewAppartment(model);
            return RedirectToAction("Index");
        }

        public ActionResult AddNewMeter(int Id)
        {
            return PartialView(Id);
        }

        [HttpPost]
        public ActionResult AddNewMeter(CreateMeterModel model)
        {
            FunctionWithDB.AddNewMeter(model);
            return RedirectToAction("Index");
        }

        public ActionResult AddNewValue(int Id)
        {
            var model = FunctionWithDB.GetDataForAddNewValue(Id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AddNewValue(AddMeterValueModel model)
        {
            var res = FunctionWithDB.AddNewValue(model);
            if (!string.IsNullOrEmpty(res))
            {
                return PartialView("Error", "Переданные данные меньше необходимых значений");
            }
            return RedirectToAction("Index");
        }

        public ActionResult ShowAllChange(int Id)
        {
            var model = FunctionWithDB.ShowAllChangeInOneAppartment(Id);
            return PartialView(model);
        }

        public ActionResult Error(string text)
        {
            return PartialView(text);
        }

        [ChildActionOnly]
        public ActionResult Filters()
        {
            var model = FunctionWithDB.DataForFilters();
            return PartialView(model);
        }
        
        [HttpPost]
        public JsonResult LoadHouseNumber(string street)
        {
            var result = FunctionWithDB.LoadHouseNumber(street);
            return Json(result);
        }
    }
}