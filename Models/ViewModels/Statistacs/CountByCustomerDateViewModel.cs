using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class CountByCustomerDateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập thời gian bắt đầu!")]
        [DataType(DataType.Date)]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime Dau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời gian kết thúc!")]
        [DataType(DataType.Date)]
        [Display(Name = "Thời gian kết thúc")]
        [Remote(action: "KiemTraNgayCuoi", controller: "Statistics", AdditionalFields = "Dau")]
        public DateTime Cuoi { get; set; }

        public List<CustomerCount> Customers { get; set; }
    }

    public class CustomerCount
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public double Tien { get; set; }
    }
}