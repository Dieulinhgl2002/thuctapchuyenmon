using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EntityFramework;
using Common;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();

        // GET: Admin/Product
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDAO();
            var model = dao.getAllPaging(keyword, page, pageSize);
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            ViewBag.keyword = keyword;
            return View(model);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // extract only the fielname
                    fileName = Path.GetFileName(ImageFile.FileName);
                    // store the file inside ~/Assets/client/images folder
                    var path = Path.Combine(Server.MapPath("~/Assets/client/images"), fileName);
                    ImageFile.SaveAs(path);
                }
                product.CreatedDate = DateTime.Now;
                var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
                product.CreatedBy = userLogin.UserName;
                product.Image = "/Assets/client/images/" + fileName;
                product.Code = db.Database.SqlQuery<string>("SELECT [dbo].[AUTO_CODE]()").FirstOrDefault();
                product.MetaTitle = StringHelper.ToUnsignString(product.Name);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,MetaTitle,Description,Image,MoreImages,Price,PromotionPrice,IncludedVAT,Quantity,CategoryID,Detail,Warranty,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,TopHot,ViewCount")] Product product, HttpPostedFileBase ImageFile = null)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                var sp = db.Products.Find(product.ID);
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // extract only the fielname
                    fileName = Path.GetFileName(ImageFile.FileName);
                    // store the file inside ~/Assets/client/images folder
                    var path = Path.Combine(Server.MapPath("~/Assets/client/images"), fileName);
                    ImageFile.SaveAs(path);
                    sp.Image = "/Assets/client/images/" + fileName;
                }
                sp.MetaTitle = StringHelper.ToUnsignString(product.Name);
                sp.Price = product.Price;
                sp.PromotionPrice = product.PromotionPrice;
                sp.Description = product.Description;
                sp.Name = product.Name;
                sp.CategoryID = product.CategoryID;
                sp.Quantity = product.Quantity;
                if (ImageFile == null)
                {
                    product.Image = sp.Image;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void SetViewBag(long? selectedId = null)
        {
            var list = db.ProductCategories.ToList();
            ViewBag.CategoryID = new SelectList(list, "ID", "Name", selectedId);
        }

    }
}
