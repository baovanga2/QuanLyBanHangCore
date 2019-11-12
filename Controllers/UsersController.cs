using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    [Authorize(Roles = "Quản trị")]
    public class UsersController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole<int>> _roleManager;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users;
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ten,NgaySinh,GioiTinh,SDT,Email,DiaChi,TaiKhoan,MatKhau,XacNhanMatKhau")] UserCreateViewModel UserCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Ten = UserCreateViewModel.Ten,
                    NgaySinh = UserCreateViewModel.NgaySinh,
                    GioiTinh = UserCreateViewModel.GioiTinh,
                    SDT = UserCreateViewModel.SDT,
                    Email = UserCreateViewModel.Email,
                    DiaChi = UserCreateViewModel.DiaChi,
                    UserName = UserCreateViewModel.TaiKhoan
                };

                var result = await _userManager.CreateAsync(user, UserCreateViewModel.MatKhau);

                if (result.Succeeded)
                {
                    TempData["messageSuccess"] = $"Người dùng \"{user.Ten}\" đã được thêm!";
                    return RedirectToAction("Details", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(UserCreateViewModel);
        }

        [HttpGet]
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
        public async Task<IActionResult> Edit([Bind("TaiKhoan,Ten,NgaySinh,GioiTinh,SDT,Email,DiaChi")] UserEditViewModel model)
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
                ViewData["messageSuccess"] = $"Người dùng \"{model.TaiKhoan}\" đã cập nhật!";
                return RedirectToAction("Details");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
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
            var login = new LoginViewModel { ReturnUrl = ReturnUrl };
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.TaiKhoan,
                    loginViewModel.MatKhau, loginViewModel.GhiNhoToi, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
                    {
                        return LocalRedirect(loginViewModel.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Tài khoản và mật khẩu không đúng!");
            }
            return View(loginViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            var roleViewModels = new List<RoleViewModel>();
            var roles = _roleManager.Roles;
            foreach (var r in roles)
            {
                var roleViewModel = new RoleViewModel
                {
                    ID = r.Id,
                    Ten = r.Name
                };
                foreach (var u in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(u, r.Name))
                    {
                        roleViewModel.Users.Add(u.UserName);
                    }
                }
                roleViewModels.Add(roleViewModel);
            }
            return View(roleViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleTen)
        {
            ViewBag.roleTen = roleTen;
            var role = await _roleManager.FindByNameAsync(roleTen);
            if (role == null)
            {
                return NotFound();
            }
            var usersInRole = new List<UserInRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserID = user.Id,
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
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsersInRole(List<UserInRoleViewModel> usersInRole, string roleTen)
        {
            var role = await _roleManager.FindByNameAsync(roleTen);
            if (role == null)
            {
                return NotFound();
            }
            for (int i = 0; i < usersInRole.Count; i++)
            {
                var user = await _userManager.FindByNameAsync(usersInRole[i].TaiKhoan);
                IdentityResult result = null;
                if (usersInRole[i].IsSelected && !(await _userManager.IsInRoleAsync(user, roleTen)))
                {
                    result = await _userManager.AddToRoleAsync(user, roleTen);
                }
                else if (!usersInRole[i].IsSelected && await _userManager.IsInRoleAsync(user, roleTen))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, roleTen);
                }
                else
                {
                    continue;
                }
                
                if (result.Succeeded)
                {
                    if (i < (usersInRole.Count -1))
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
        public async Task<IActionResult> IsTaiKhoanExists(string taiKhoan)
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