using Model.DAO;
using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                var model = db.Slides.Where(x => x.Status == true).OrderBy(x=>x.DisplayOrder).ToList();
                return View(model);
            }
            
        }

        public JsonResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetContacts(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [ChildActionOnly]
        public ActionResult TopBrand()
        {
            ViewBag.TuKhoa = "Top Hot";
            var model = new ProductDAO().getListHot();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopRecommend()
        {
            var model = new ProductDAO().getRecommend();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuDAO().getList(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _Footer()
        {
            var model = new FooterDAO().getFooter();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Logo()
        {
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                var model = db.SystemConfigs.Where(x => x.ID == "LOGO").FirstOrDefault();
                return PartialView(model);
            } 
        }
    }
}