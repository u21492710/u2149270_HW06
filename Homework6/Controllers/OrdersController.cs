using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Homework6.Models;
using PagedList.Mvc;
using PagedList;

namespace Homework6.Controllers
{
    public class OrdersController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();
        static DateTime searchTrigger;
        // GET: Orders
       
        public ActionResult Index(int? i)
        {
            var ordersList = db.orders;
            
            if (searchTrigger.ToShortDateString() != "2000/01/01")
            {

                var ordersListNew = db.orders.Where(zz => zz.order_date >= searchTrigger);
                return View(ordersListNew.ToList().ToPagedList(i ?? 1, 10));
            }
            return View(ordersList.ToList().ToPagedList(i ?? 1, 10));
        }
        public ActionResult Search(DateTime searchText)
        {
            //Don't forget to write a function to clear it and set the searchTrigger back to 0
            searchTrigger = searchText;
            var ordersList = db.orders.Where(zz => zz.order_date>=searchText);
            int? i = 1;
            return View("Index", ordersList.ToList().ToPagedList(i ?? 1, 10));
           
        }
        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
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

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Orders/Edit/5
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

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Orders/Delete/5
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
