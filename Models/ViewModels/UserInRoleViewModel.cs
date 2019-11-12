using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class UserInRoleViewModel
    {
        public int UserID { get; set; }
        public string TaiKhoan { get; set; }

        public string Ten { get; set; }
        public bool IsSelected { get; set; }
    }
}
