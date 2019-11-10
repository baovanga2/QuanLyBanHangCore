using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Producer
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(20)]
        [Remote(action: "IsProducerNameExists", controller: "Producers", AdditionalFields = nameof(ID))]
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        public List<Product> Products { get; set; }
    }
}
