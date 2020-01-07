using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly QuanLyBanHangCoreContext _context;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager, QuanLyBanHangCoreContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Roles = "Quản trị")]
        public IActionResult Index()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [Authorize(Roles = "Quản trị")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> Create([Bind("Ten,NgaySinh,GioiTinh,SDT,Email,DiaChi,TaiKhoan,MatKhau,XacNhanMatKhau")] UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Ten = model.Ten,
                    NgaySinh = model.NgaySinh,
                    GioiTinh = model.GioiTinh,
                    SDT = model.SDT,
                    Email = model.Email,
                    DiaChi = model.DiaChi,
                    UserName = model.TaiKhoan
                };

                var result = await _userManager.CreateAsync(user, model.MatKhau);

                if (result.Succeeded)
                {
                    var nguoiCapNhat = await _userManager.GetUserAsync(User);
                    var userChange = new UserChange
                    {
                        ThoiGian = DateTime.Now,
                        NguoiCapNhat = nguoiCapNhat.UserName,
                        HanhDong = "Thêm mới",
                        NguoiBiCapNhat = user.UserName
                    };
                    _context.UserChanges.Add(userChange);
                    await _context.SaveChangesAsync();
                    TempData["messageSuccess"] = $"Người dùng \"{user.Ten}\" đã được thêm!";
                    return RedirectToAction("Details", "Users", new { taiKhoan = user.UserName });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> Details(string taiKhoan)
        {
            if (taiKhoan == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(taiKhoan);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserDetailsViewModel
            {
                TaiKhoan = user.UserName,
                Ten = user.Ten,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                SDT = user.SDT,
                Email = user.Email,
                DiaChi = user.DiaChi,
                Roles = userRoles
            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> Edit(string taiKhoan)
        {
            if (taiKhoan == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(taiKhoan);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserEditViewModel
            {
                TaiKhoan = user.UserName,
                Ten = user.Ten,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                SDT = user.SDT,
                Email = user.Email,
                DiaChi = user.DiaChi
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> Edit(string taiKhoan, [Bind("TaiKhoan,Ten,NgaySinh,GioiTinh,SDT,Email,DiaChi")] UserEditViewModel model)
        {
            if (taiKhoan != model.TaiKhoan)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.TaiKhoan);
                if (user == null)
                {
                    return NotFound();
                }
                user.Ten = model.Ten;
                user.NgaySinh = model.NgaySinh;
                user.GioiTinh = model.GioiTinh;
                user.SDT = model.SDT;
                user.Email = model.Email;
                user.DiaChi = model.DiaChi;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["messageSuccess"] = $"Người dùng \"{model.TaiKhoan}\" đã cập nhật!";
                    return RedirectToAction("Details", new { taiKhoan = user.UserName });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> Delete(string taiKhoan)
        {
            if (taiKhoan == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(taiKhoan);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserDetailsViewModel
            {
                TaiKhoan = taiKhoan,
                Ten = user.Ten,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                SDT = user.SDT,
                Email = user.Email,
                DiaChi = user.DiaChi,
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> DeleteConfirmed(string taiKhoan)
        {
            var user = await _userManager.FindByNameAsync(taiKhoan);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserDetailsViewModel
            {
                TaiKhoan = taiKhoan,
                Ten = user.Ten,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                SDT = user.SDT,
                Email = user.Email,
                DiaChi = user.DiaChi,
                Roles = userRoles
            };
            if (!_context.Orders.Any(o => o.UserID == user.Id))
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["messageSuccess"] = $"Người dùng \"{taiKhoan}\" đã xóa";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            ModelState.AddModelError("", "Vì có đơn hàng do người dùng này lập nên không thể xóa người dùng này, chỉ có thể xóa khi không có đơn hàng nào do người dùng này lập!");
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> UserPasswordChange(string taiKhoan)
        {
            if (taiKhoan == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(taiKhoan);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserPasswordChangeViewModel
            {
                TaiKhoan = user.UserName,
                Ten = user.Ten,
                MatKhauMoi = string.Empty,
                XacNhanMatKhauMoi = string.Empty
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Quản trị")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserPasswordChange(string taiKhoan, [Bind("TaiKhoan,Ten,MatKhauMoi,XacNhanMatKhauMoi")] UserPasswordChangeViewModel model)
        {
            if (taiKhoan != model.TaiKhoan)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.TaiKhoan);
                if (user == null)
                {
                    return NotFound();
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.MatKhauMoi);
                if (result.Succeeded)
                {
                    var nguoiCapNhat = await _userManager.GetUserAsync(User);
                    var userChange = new UserChange
                    {
                        ThoiGian = DateTime.Now,
                        NguoiCapNhat = nguoiCapNhat.UserName,
                        HanhDong = "Đổi mật khẩu",
                        NguoiBiCapNhat = user.UserName
                    };
                    _context.UserChanges.Add(userChange);
                    await _context.SaveChangesAsync();
                    TempData["messageSuccess"] = $"Mật khẩu người dùng \"{model.TaiKhoan}\" đã cập nhật!";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> PersonalDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new PersonalDetailsViewModel
            {
                TaiKhoan = user.UserName,
                Ten = user.Ten,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                SDT = user.SDT,
                Email = user.Email,
                DiaChi = user.DiaChi,
                Roles = userRoles
            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> PersonalEdit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var model = new PersonalEditViewModel
            {
                Ten = user.Ten,
                NgaySinh = user.NgaySinh,
                GioiTinh = user.GioiTinh,
                SDT = user.SDT,
                Email = user.Email,
                DiaChi = user.DiaChi
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Quản trị,Bán hàng,Thủ kho,Kế toán")]
        public async Task<IActionResult> PersonalEdit([Bind("Ten,NgaySinh,GioiTinh,SDT,Email,DiaChi")] PersonalEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                user.Ten = model.Ten;
                user.NgaySinh = model.NgaySinh;
                user.GioiTinh = model.GioiTinh;
                user.SDT = model.SDT;
                user.Email = model.Email;
                user.DiaChi = model.DiaChi;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["messageSuccess"] = "Thông tin của bạn đã cập nhật!";
                    return RedirectToAction("PersonalDetails", new { taiKhoan = user.UserName });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> ChangePassword([Bind("MatKhauHienTai,MatKhauMoi,XacNhanMatKhauMoi")] ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ChangePasswordAsync(user, model.MatKhauHienTai, model.MatKhauMoi);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["messageSuccess"] = "Mật khẩu của bạn đã cập nhật!";
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/Login")]
        public IActionResult Login(string ReturnUrl = "")
        {
            var model = new LoginVM { ReturnUrl = ReturnUrl };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.TaiKhoan,
                    model.MatKhau, model.GhiNhoToi, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Tài khoản và mật khẩu không đúng!");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> ListRoles()
        {
            var model = new List<RoleViewModel>();
            var roles = _roleManager.Roles;
            foreach (var r in roles)
            {
                var roleVM = new RoleViewModel
                {
                    Ten = r.Name
                };
                foreach (var u in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(u, r.Name))
                    {
                        roleVM.Users.Add(u.UserName);
                    }
                }
                model.Add(roleVM);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> EditUsersInRole(string roleTen)
        {
            if (roleTen == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByNameAsync(roleTen);
            if (role == null)
            {
                return NotFound();
            }
            var model = new List<UserInRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                var userInRole = new UserInRoleViewModel
                {
                    TaiKhoan = user.UserName,
                    Ten = user.Ten
                };
                if (await _userManager.IsInRoleAsync(user, roleTen))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                model.Add(userInRole);
            }
            ViewBag.roleTen = roleTen;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Quản trị")]
        public async Task<IActionResult> EditUsersInRole(string roleTen, List<UserInRoleViewModel> model)
        {
            var role = await _roleManager.FindByNameAsync(roleTen);
            if (role == null)
            {
                return NotFound();
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByNameAsync(model[i].TaiKhoan);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, roleTen)))
                {
                    result = await _userManager.AddToRoleAsync(user, roleTen);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, roleTen))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, roleTen);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("ListRoles", "Users");
                    }
                }
            }
            return RedirectToAction("ListRoles", "Users");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> KiemTraTaiKhoan(string taiKhoan)
        {
            var user = await _userManager.FindByNameAsync(taiKhoan);
            if (user == null)
            {
                return Json(true);
            }
            return Json($"Tên tài khoản \"{taiKhoan}\" đã được sử dụng!");
        }
    }
}