using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class ProductIndexViewModel
    {
        public ProductIndexViewModel()
        {
            Products = new List<ProductWithCurrentPrice>();
        }
        public List<ProductWithCurrentPrice> Products { get; set; }
        public SelectList Categories { get; set; }
        [Display(Name = "Loại sản phẩm")]
        public string Category { get; set; }
        public SelectList Producers { get; set; }
        [Display(Name = "Nhà sản xuất")]
        public string Producer { get; set; }
    }
}
