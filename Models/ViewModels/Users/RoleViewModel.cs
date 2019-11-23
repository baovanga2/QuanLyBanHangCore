using System.Collections.Generic;

namespace QuanLyBanHangCore.Models.ViewModels
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