using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Order
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Thời gian")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy HH:mm:ss", ApplyFormatInEditMode = true)]
        public DateTime ThoiGianTao { get; set; }

        public int UserID { get; set; }

        [Display(Name = "Nhân viên")]
        public User User { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khách hàng!")]
        [Display(Name = "Khách hàng")]
        public int CustomerID { get; set; }
 
        public Customer Customer { get; set; }

        public List<DetailOrder> DetailOrders { get; set; }
    }
}
