namespace Model.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public long ID { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime? CreatedDate { get; set; }

        public long? CustomerID { get; set; }
        [Display(Name = "Họ và tên")]
        [StringLength(50)]
        public string ShipName { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(50)]
        public string ShipMobile { get; set; }
        [Display(Name = "Địa chỉ")]
        public string ShipAddress { get; set; }
        [Display(Name = "Email")]
        [StringLength(50)]
        public string ShipEmail { get; set; }
        [Display(Name = "Tình trạng")]
        public int? Status { get; set; }
        public int? StatusDelivery { get; set; }

        public int TienShip { get; set; }
        public List<OrderDetail> ListOrderDetail { get; set; }
    }


}
