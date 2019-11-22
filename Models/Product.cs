using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models
{
    public class Product
    {
        public int ID { get; set; }
        [MaxLength(100)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        [Display(Name = "Số lượng")]
        public uint SoLuong { get; set; }

        [Display(Name = "Nhà sản xuất")]
        public int ProducerID { get; set; }
        public Producer Producer { get; set; }
        [Display(Name = "Loại")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public List<DetailOrder> DetailOrders { get; set; }
        [Display(Name = "Giá")]
        public List<ProductPrice> ProductPrices { get; set; }
    }
}