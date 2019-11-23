using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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