using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class CardInfo
    {
        [Required(ErrorMessage ="Hãy nhập tên người nhận")]
        public string name { get; set; }
        [Required(ErrorMessage = "Hãy nhập số điện thoại")]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Hãy nhập địa chỉ nhận hàng")]
        public string address { get; set; }
    }
}