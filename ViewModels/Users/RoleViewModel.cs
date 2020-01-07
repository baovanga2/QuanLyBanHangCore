using System.Collections.Generic;

namespace QuanLyBanHangCore.ViewModels
{
    public class RoleViewModel
    {
        public string Ten { get; set; }
        public List<string> Users { get; set; }

        public RoleViewModel()
        {
            Users = new List<string>();
        }
    }
}