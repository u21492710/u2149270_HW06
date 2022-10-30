using Homework6.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Collections;
using System.Data.Entity;

namespace Homework6.Controllers
{
    public class ReportController : Controller
    {

        private BikeStoresEntities db = new BikeStoresEntities();
        // GET: Report
        public ActionResult Index()
        {
            
            return View();
        }
        public string ReportBack()
        {
            db.Configuration.ProxyCreationEnabled = false;
            object mountainBikes = db.orders.Select(o => new
            {
                month = o.order_date.Month,
                bike = db.order_items.Where(x => x.product.category.category_id == 6 && x.order_id == o.order_id).ToList(),
            }).ToList();

            return JsonConvert.SerializeObject(mountainBikes);
        }
        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
