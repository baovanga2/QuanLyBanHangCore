using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class ProductWithPriceList
    {
        public Product Product { get; set; }
        [DisplayFormat(DataFormatString = "{0:### ### ### ### VND}", ApplyFormatInEditMode = false)]
        [Display(Name = "Giá")]
        public ulong Gia { get; set; }
        [Display(Name = "Danh sách giá")]
        public List<ProductPrice> ProductPrices { get; set; }
    }
}
