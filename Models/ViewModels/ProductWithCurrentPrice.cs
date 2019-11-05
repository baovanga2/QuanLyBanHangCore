using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QuanLyBanHangCore.Models;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class ProductWithCurrentPrice
    {
        public Product Product { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Display(Name = "Giá")]     
        public ulong Gia { get; set; }
    }
}
