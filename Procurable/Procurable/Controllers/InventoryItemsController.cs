﻿using Procurable.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Procurable.Models
{
    public class InventoryItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InventoryItems
        [Authorize]
        public ActionResult Index()
        {
            if (Request.AcceptTypes.Contains("application/json"))
            {
                return Json(db.InventoryItems.ToList(), JsonRequestBehavior.AllowGet);
            }
            var inventoryCount = db.InventoryItems.ToList().GroupBy(x => new { x.Name }).Select(group => new InventoryItemIndex() { Item= group.ToList<InventoryItem>(), Count = group.Count() }).ToList();
            return View(inventoryCount);
        }

        // GET: InventoryItems/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                if (Request.AcceptTypes.Contains("application/json"))
                {
                    return Json(new { Error = "BadRequest" }, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null)
            {
                if (Request.AcceptTypes.Contains("application/json"))
                {
                    return Json(new { Error = "NotFound" }, JsonRequestBehavior.AllowGet);
                }
                return HttpNotFound();
            }
            if (Request.AcceptTypes.Contains("application/json"))
            {
                return Json(inventoryItem, JsonRequestBehavior.AllowGet);
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,Name,Price,Comments,PartNumber,Location,Status, VendorID")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                db.InventoryItems.Add(inventoryItem);
                db.SaveChanges();
                if (Request.AcceptTypes.Contains("application/json"))
                {
                    return Json(new { Succeeded = true });
                }
                return RedirectToAction("Index");
            }

            return View(inventoryItem);
        }

        // GET: InventoryItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // POST: InventoryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,Name,Price,Comments,PartNumber,Location,Status,VendorID")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventoryItem).State = EntityState.Modified;
                db.SaveChanges();
                if (Request.AcceptTypes.Contains("application/json"))
                {
                    return Json(new { Succeeded = true });
                }
                return RedirectToAction("Index");
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            InventoryItem inventoryItem = db.InventoryItems.Find(id);
            db.InventoryItems.Remove(inventoryItem);
            db.SaveChanges();
            if (Request.AcceptTypes.Contains("application/json"))
            {
                return Json(new { Succeeded = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
