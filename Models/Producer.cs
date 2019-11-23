using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models
{
    public class Producer
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(20)]
        [Remote(action: "KiemTraTen", controller: "Producers", AdditionalFields = nameof(ID))]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        public List<Product> Products { get; set; }
    }
}