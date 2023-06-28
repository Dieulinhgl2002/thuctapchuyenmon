using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string keyword)
        {
            ViewBag.ListProductByKeyword = new ProductDAO().getProductByKeyword(keyword);
            ViewBag.Keyword = keyword;
            return View();
        }

        public ActionResult Category(string metatitle)
        {
            var dao = new ProductDAO().getCategory(metatitle);
            ViewBag.ListProductByID = new ProductDAO().getProductByCategoryID(dao.ID);
            return View(dao);
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDAO().getList();
            return PartialView(model);
        }

        [OutputCache(Duration = int.MaxValue, VaryByParam ="id")]
        public ActionResult Detail(long id)
        {
            ViewBag.ListProductLQ = new ProductDAO().getProductByCateID(id);
            return View(new ProductDAO().getProductByID(id));
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDAO().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet); 
        }
    }
}