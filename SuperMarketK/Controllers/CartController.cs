using Common;
using Model.DAO;
using Model.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperMarketK.Common;
using SuperMarketK.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        private static int tienShip;
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadProvince()
        {
            string path = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province";
            var list = getProvince(path);
            return Json(new
            {
                data = list,
                status = true
            });
        }

        public List<DistrictModel> getDistrict(string path, int provinceID)
        {
            List<DistrictModel> list = new List<DistrictModel>();
            var data = new { province_id = provinceID };
            using (WebClient webClient = new WebClient())
            {
                var dataString = JsonConvert.SerializeObject(data);
                webClient.Headers.Add("Token", "60ab843a-e19a-11eb-9388-d6e0030cbbb7");
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.Encoding = ASCIIEncoding.UTF8;
                string sql = webClient.UploadString(path, dataString);
                dynamic jsonData = JsonConvert.DeserializeObject(sql);
                foreach (var item in jsonData.data)
                {
                    int DistrictID = item.DistrictID;
                    int ProvinceID = item.ProvinceID;
                    string DistrictName = item.DistrictName;
                    list.Add(new DistrictModel(DistrictID, DistrictName, ProvinceID));
                }
            }
            return list;
        }

        public List<WardModel> getPhuongXa(string path, int districtID)
        {
            List<WardModel> list = new List<WardModel>();
            var data = new { district_id = districtID };
            using (WebClient webClient = new WebClient())
            {
                var dataString = JsonConvert.SerializeObject(data);
                webClient.Headers.Add("Token", "60ab843a-e19a-11eb-9388-d6e0030cbbb7");
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.Encoding = ASCIIEncoding.UTF8;
                string sql = webClient.UploadString(path, dataString);
                dynamic jsonData = JsonConvert.DeserializeObject(sql);
                foreach (var item in jsonData.data)
                {
                    int DistrictID = item.WardCode;
                    int ProvinceID = item.DistrictID;
                    string DistrictName = item.WardName;
                    list.Add(new WardModel(DistrictID, DistrictName, ProvinceID));
                }
            }
            return list;
        }

        public JsonResult LoadPhuongXa(int districtID)
        {
            string path = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id";
            var list = getPhuongXa(path, districtID);
            return Json(new
            {
                data = list,
                status = true
            });
        }

        public JsonResult LoadDistrict(int ProvinceID)
        {
            string path = "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district";
            var list = getDistrict(path, ProvinceID);
            return Json(new
            {
                data = list,
                status = true
            });
        }

        [HttpPost]
        public JsonResult getTienShip(int wardCode, int districtID)
        {
            string path = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";
            var dataJson = new { from_district_id = 1451, service_type_id = 2, to_district_id = districtID, to_ward_code = wardCode, weight = 1000 };
            using (WebClient webClient = new WebClient())
            {
                var dataString = JsonConvert.SerializeObject(dataJson);
                webClient.Headers.Add("Token", "60ab843a-e19a-11eb-9388-d6e0030cbbb7");
                webClient.Headers.Add("ShopId", "81328");
                webClient.Encoding = ASCIIEncoding.UTF8;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                //string sql = webClient.UploadString(path, dataString);
                //dynamic jsonData = JsonConvert.DeserializeObject(sql);
                //tienShip = jsonData.data.total;
                return Json(new
                {
                    data = 0,
                    status = true
                });
            }
    }

        public List<ProvinceModel> getProvince(string path)
        {
            List<ProvinceModel> list = new List<ProvinceModel>();
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("Token", "60ab843a-e19a-11eb-9388-d6e0030cbbb7");
                webClient.Encoding = ASCIIEncoding.UTF8;
                dynamic jsonData = JsonConvert.DeserializeObject<object>(webClient.DownloadString(path));
                foreach (var item in jsonData.data)
                {
                    int provinceID = item.ProvinceID;
                    string ProvinceName = item.ProvinceName;
                    list.Add(new ProvinceModel(provinceID, ProvinceName));
                }
            }
            return list;
        }

        [HttpPost]
        public JsonResult AddItem(long productID, int quantity)
        {
            var product = new ProductDAO().getProductByID(productID);
            var session = Session[CartSession];
            if (session != null)
            {
                var list = (List<CartItem>)session;
                if (list.Exists(x=>x.product.ID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.product.ID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                } else
                {
                    var item = new CartItem();
                    item.product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[CartSession] = list;
            } else
            {
                var item = new CartItem();
                item.product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list;
            }
            return Json(new
            {
                status = true,
            });
        }
        [HttpPost]
        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new {
                status = true
            });
        }

        [HttpPost]
        public JsonResult UpdateUp(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.Find(x=>x.product.ID == id).Quantity++;
            Session[CartSession] = sessionCart;
            return Json(new
            {
                priceItem = sessionCart.Find(x => x.product.ID == id).product.Price,
                status = true
            });
        }

        [HttpPost]
        public JsonResult UpdateDown(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.Find(x => x.product.ID == id).Quantity--;
            Session[CartSession] = sessionCart;
            return Json(new
            {
                priceItem = sessionCart.Find(x => x.product.ID == id).product.Price,
                status = true
            });
        }
        public static long id;

        [HttpPost]
        public ActionResult Payment(CardInfo cardInfo, bool pay, string ProvinceName, string huyen, string xa)
        {
            if (ModelState.IsValid)
            {
                if (Session[Common.CommonContants.USER_SESSION] == null)
                {
                    return Redirect("/dang-nhap");
                }
                UserLogin userLogin = (UserLogin)Session[Common.CommonContants.USER_SESSION];
                var order = new Order
                {
                    CreatedDate = DateTime.Now,
                    ShipAddress = cardInfo.address + ", " + xa.Trim() + ", " + huyen.Trim() + ", " + ProvinceName.Trim(),
                    ShipMobile = cardInfo.mobile,
                    ShipName = cardInfo.name,
                    ShipEmail = userLogin.UserName,
                    CustomerID = userLogin.UserID,
                    Status = 0,
                    TienShip = tienShip,
                    StatusDelivery = 0
                };
                TempData["Email"] = new UserDAO().getEmailByUserName(userLogin.UserName);
                TempData["Phone"] = order.ShipMobile;
                TempData["Name"] = order.ShipName;
                TempData["Address"] = order.ShipAddress;

                var dao = new OrderDAO();
                id = dao.Insert(order);
                TempData["OrderID"] = id;
                var list = (List<CartItem>)Session[CartSession];
                var orderDetailDAO = new OrderDetailDAO();
                decimal? tongTien = 0;

                SuperMarketKDbContext db = new SuperMarketKDbContext();

                foreach (var item in list)
                {
                    try
                    {
                        var getItemQuantity = db.Products.Find(item.product.ID);
                        getItemQuantity.Quantity -= item.Quantity;
                        db.SaveChanges();
                        var orderDetail = new OrderDetail();
                        orderDetail.OrderID = id;
                        orderDetail.Price = item.product.Price;
                        orderDetail.ProductID = item.product.ID;
                        orderDetail.Quantity = item.Quantity;
                        orderDetailDAO.Insert(orderDetail);
                        tongTien += item.product.Price * item.Quantity;
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                    }
                   
                }
                // Payment MoMo
                if (pay)
                {
                    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                    string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();
                    string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
                    string serectkey = ConfigurationManager.AppSettings["secretKey"].ToString();
                    string orderInfo = "Đơn hàng mới";
                    string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
                    string notifyurl = ConfigurationManager.AppSettings["notifyUrl"].ToString();
                    string amount = tongTien.ToString();
                    string orderid = Guid.NewGuid().ToString();
                    string requestId = Guid.NewGuid().ToString();
                    string extraData = "";

                    //Before sign HMAC SHA256 signature
                    string rawHash = "partnerCode=" +
                        partnerCode + "&accessKey=" +
                        accessKey + "&requestId=" +
                        requestId + "&amount=" +
                        amount + "&orderId=" +
                        orderid + "&orderInfo=" +
                        orderInfo + "&returnUrl=" +
                        returnUrl + "&notifyUrl=" +
                        notifyurl + "&extraData=" +
                        extraData;

                    MomoSecurity crypto = new MomoSecurity();
                    //sign signature SHA256
                    string signature = crypto.signSHA256(rawHash, serectkey);

                    //build body json request
                    JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
                    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                    JObject jmessage = JObject.Parse(responseFromMomo);
                    return Redirect(jmessage.GetValue("payUrl").ToString());
                }
                else
                {
                    sendMail();
                    Session[CartSession] = null;
                    return Redirect("/hoan-thanh");
                }
            }
            else return View("Index", cardInfo);
        }

        public void sendMail()
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/template/guimail.html"));
            content = content.Replace("{{CustomerName}}", TempData["Name"].ToString());
            content = content.Replace("{{Phone}}", TempData["Phone"].ToString());
            content = content.Replace("{{DiaChiVanChuyen}}", TempData["Address"].ToString());
            content = content.Replace("{{OrderID}}", TempData["OrderID"].ToString());
            new MailHelper().SendMail(TempData["Email"].ToString(), "ĐẶT HÀNG THÀNH CÔNG", content);
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult ReturnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MomoSecurity crypto = new MomoSecurity();
            string serectkey = ConfigurationManager.AppSettings["secretKey"].ToString();
            string signature = crypto.signSHA256(param, serectkey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.Message = "Thông tin request không hợp lệ";
            } else if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.Message = "Thanh toán thất bại";
                Session[CartSession] = null;
                sendMail();
            } else
            {
                ViewBag.Message = "Thanh toán thành công";
                Session[CartSession] = null;
                sendMail();
            }
            return View();
        }

        [HttpPost]
        public JsonResult NotifyUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MomoSecurity crypto = new MomoSecurity();
            string serectkey = ConfigurationManager.AppSettings["secretKey"].ToString();
            string signature = crypto.signSHA256(param, serectkey);
            var dao = new OrderDAO();
            if (signature != Request["signature"].ToString())
            {
                dao.UpdateStatus(0, id);
            }
            else if (!Request.QueryString["errorCode"].Equals("0"))
            {
                dao.UpdateStatus(0, id);
            }
            else
            {
                dao.UpdateStatus(1, id);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}