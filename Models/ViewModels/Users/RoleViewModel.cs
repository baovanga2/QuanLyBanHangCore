using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class RoleViewModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public List<string> Users { get; set; }

        public RoleViewModel()
        {
            Users = new List<string>();
        }
    }
}
