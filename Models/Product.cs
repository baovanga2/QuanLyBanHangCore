using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(100)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng!")]
        [Display(Name = "Số lượng")]
        public ushort SoLuong { get; set; }

        public int ProducerID { get; set; }

        [Display(Name = "Nhà sản xuất")]
        public Producer Producer { get; set; }  
        
        public int CategoryID { get; set; }

        [Display(Name = "Loại")]
        public Category Category { get; set; }

        public List<DetailOrder> DetailOrders { get; set; }

        [Display(Name = "Giá")]
        public List<ProductPrice> ProductPrices { get; set; }
    }
}
