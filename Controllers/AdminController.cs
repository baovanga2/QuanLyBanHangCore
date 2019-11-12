using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuanLyBanHangCore.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuanLyBanHangCore.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AdminController(RoleManager<IdentityRole<int>> roleManager)
        {
            this._roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateViewModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole<int> identityRole = new IdentityRole<int>
                {
                    Name = role.Ten
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Products");
                }
                foreach (IdentityError e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
            }
            return View();
        }
    }
}
