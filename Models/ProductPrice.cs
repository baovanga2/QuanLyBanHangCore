using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models
{
    public class ProductPrice
    {
        public int ID { get; set; }
        public ulong Gia { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime TGBD { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime TGKT { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}