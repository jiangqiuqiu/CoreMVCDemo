using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVCDemo.Models;
using CoreMVCDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVCDemo.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //将数据从RegisterViewModel复制到IdentityUser
                var user = new ApplicationUser()
                {
                    UserName=model.Email,
                    Email=model.Email,
                    City=model.City
                };

                //将用户数据存储在AspNetUsers数据库表中
                //*************************************************
                //注意:添加成功后会在AspNetUsers表中添加入用户信息
                //*************************************************
                var result = await userManager.CreateAsync(user,model.Password);

                //如果成功创建用户，则使用登录服务器登录用户信息
                //并重定向到Home Index
                if (result.Succeeded)
                {
                    //isPersistent:其实就是记住我的功能
                    await signInManager.SignInAsync(user,isPersistent:false);
                    
                    return RedirectToAction("Index","Home");
                }

                //如果有任何错误，将它们添加到ModelState对象中
                //将由验证摘要标记助手显示到视图中
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }                
            }
            return View(model);
        }

        /// <summary>
        /// 注销用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        //[AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]        
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //防止开放式重定向攻击
                        if (Url.IsLocalUrl(returnUrl))//如果是本地的URL则返回
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            //ModelState.AddModelError(string.Empty, "防止开放式重定向攻击");
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }                    
                }

                ModelState.AddModelError(string.Empty,"登录失败，请重试");
            }

            return View(model);
        }

        [AcceptVerbs("Get","Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user==null)
            {
                return Json(true);
            }
            else
            {
                return Json($"邮箱:{email}已经被注册使用了");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FangYu()
        {
            return View();
        }
    }
}