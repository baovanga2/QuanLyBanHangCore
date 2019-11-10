using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Ten = createUserViewModel.Ten,
                    NgaySinh = createUserViewModel.NgaySinh,
                    GioiTinh = createUserViewModel.GioiTinh,
                    SDT = createUserViewModel.SDT,
                    Email = createUserViewModel.Email,
                    DiaChi = createUserViewModel.DiaChi,
                    UserName = createUserViewModel.TaiKhoan
                };

                var result = await _userManager.CreateAsync(user, createUserViewModel.MatKhau);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(createUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.TaiKhoan,
                    loginViewModel.MatKhau, loginViewModel.GhiNhoToi, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Products");
                    }
                }
                ModelState.AddModelError(string.Empty, "Tài khoản và mật khẩu không đúng!");
            }
            return View(loginViewModel);
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