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
using Newtonsoft.Json;

namespace Homework6.Controllers
{
    public class productsController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();
        static string searchTrigger;
        // GET: products
        public ActionResult Index(int? i)
        {
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            if (searchTrigger !=null)
            {
                products = db.products.Where(zz => zz.product_name.Contains(searchTrigger)).Include(p => p.brand).Include(p => p.category);
            }
            return View(products.ToList().ToPagedList(i ?? 1,10));
        }
        public ActionResult Search(string searchText)
        {
            //Don't forget to write a function to clear it and set the searchTrigger back to 0
            searchTrigger = searchText;
            var products = db.products.Where(zz => zz.product_name.Contains(searchTrigger)).Include(p => p.brand).Include(p => p.category);
            int? i = 1 ;
            return View("Index", products.ToList().ToPagedList(i ?? 1, 10));
        }
        public ActionResult EditView()
        {
            return PartialView();
        }
        public ActionResult CreateView()
        {
            return PartialView();
        }
        public string EditProd(int? id)
        {
            
            db.Configuration.ProxyCreationEnabled = false;
        
            object product = db.products.Where(x => x.product_id == id).Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, model = p.model_year, price = p.list_price }).FirstOrDefault();
         
            return JsonConvert.SerializeObject(product);
        }
        public string CreateProd(int? id)
        {

            db.Configuration.ProxyCreationEnabled = false;

            object product = db.products.Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, model = p.model_year, price = p.list_price }).FirstOrDefault();

            return JsonConvert.SerializeObject(product);
        }

        public string GetCatergoryNames()
        {
            object catergoryData = db.categories.Select(p => new { id = p.category_id, name = p.category_name }).ToList();
            return JsonConvert.SerializeObject(catergoryData);
        }
        public string GetBrandData()
        {
            db.Configuration.ProxyCreationEnabled = false;

            List<brand> data = db.brands.ToList();

            return JsonConvert.SerializeObject(data);
        }
        //For details
        public string ProductDetails(int id)
        {
            object productDetial = db.stocks.Where(y => y.product_id == id).Include(v => v.product).Select(p => new {
                productname = p.product.product_name,
                year = p.product.model_year,
                price = p.product.list_price,
                brand = p.product.brand.brand_name,
                catergory = p.product.category.category_name,
                stores = db.stocks.Where(s => s.product_id == id).Select(n => new { storename = n.store.store_name, quantity = n.quantity })

            }).FirstOrDefault();
            return JsonConvert.SerializeObject(productDetial);

        }
        public ActionResult Details(int? id)
        {
            return PartialView();
        }
        //For the deletes 
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            int? i = 1;
            return RedirectToAction("Index", products.ToList().ToPagedList(i ?? 1, 10));
        }
        public string GetProducts(int? i)
        {
            db.Configuration.ProxyCreationEnabled = false;
            object productDatas = db.products.Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, model = p.model_year, price = p.list_price }).ToList().ToPagedList(i ?? 1, 10);
            return JsonConvert.SerializeObject(productDatas);
        }
        // GET: products/Create
        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return View();
        }

        // POST: products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return PartialView("Edit_Partial_View", product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return RedirectToAction("Index");
        }

        public ActionResult Verify(int product_id, string product_name, int brand_id, int category_id, short model_year, decimal list_price)
        {
            product product = new product
            {
                product_id = product_id,
                product_name = product_name,
                brand_id = brand_id,
                category_id = category_id,
                model_year = model_year,
                list_price = list_price

            };
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                var products = db.products.Include(p => p.brand).Include(p => p.category);
                int? i = 1;
                return RedirectToAction("Index", products.ToList().ToPagedList(i ?? 1, 10));
            }
           
            return View(product);
        }

        public ActionResult AddFinal(int product_id, string product_name, int brand_id, int category_id, short model_year, decimal list_price)
        {
            product product = new product
            {
                product_id = product_id,
                product_name = product_name,
                brand_id = brand_id,
                category_id = category_id,
                model_year = model_year,
                list_price = list_price

            };
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                var products = db.products.Include(p => p.brand).Include(p => p.category);
                int? i = 1;
                return RedirectToAction("Index", products.ToList().ToPagedList(i ?? 1, 10));
            }
           
            return View(product);
        }

    }
}
