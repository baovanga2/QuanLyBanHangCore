using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class ProductPrice
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá!")]
        [Display(Name = "Giá")]
        public ulong Gia { get; set; }

        [Display(Name = "Thời gian bắt đầu")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime TGBD { get; set; }

        [Display(Name = "Thời gian kết thúc")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime TGKT { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
