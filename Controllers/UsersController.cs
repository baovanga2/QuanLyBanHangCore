using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuanLyBanHangCore.Controllers
{
    public class UsersController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;
        public UsersController(QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID, Ten, GioiTinh, SDT, Email, DiaChi, TaiKhoan, MatKhau, XacNhanMatKhau, RoleID")] CreateUserViewModel createdUser)
        {
            if (ModelState.IsValid)
            {
                
                return RedirectToAction(nameof(Index));
            }            
            return View(createdUser);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        //private bool UserExists(int id, string ten, string sdt)
        //{
        //    return _context.Users.Any(c => c.Ten == ten && c.SDT == sdt && c.ID != id);
        //}

        //private bool TaiKhoanTonTai(string taiKhoan)
        //{
        //    return _context.Users.Any(u => u.TaiKhoan == taiKhoan);
        //}
    }
}
