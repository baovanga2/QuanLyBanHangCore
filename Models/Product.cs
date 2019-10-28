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

        [Required]
        [MaxLength(100)]
        [Display(Name = "Tên")]

        public string Ten { get; set; }

        [Required]
        [Display(Name = "Giá")]
        public ulong Gia { get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }
        public int ProducerID { get; set; }

        [Display(Name = "Nhà sản xuất")]
        public Producer Producer { get; set; }
        public int CategoryID { get; set; }

        [Display(Name = "Loại")]
        public Category Category { get; set; }

        public List<DetailOrder> DetailOrders { get; set; }
    }
}
