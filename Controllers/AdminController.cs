using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVCDemo.Models;
using CoreMVCDemo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVCDemo.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        //GET POST

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole() 
                { 
                    Name=model.RoleName
                };
                //如果您尝试创建具有已存在的同名的角色，则会收到验证错误
                IdentityResult result=await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles","Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }

            return View(model);
        }

        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role==null)
            {
                ViewBag.ErrorMessage = $"角色id为{id}的信息不存在，请重试！";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id=role.Id,
                RoleName=role.Name
            };

            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                //如果用户拥有此角色，请将用户名添加到
                //EditRoleViewModel模型中的Users属性中
                //然后将对象传递给视图显示到客户端
                if (await userManager.IsInRoleAsync(user,role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色id为{model.Id}的信息不存在，请重试！";
                return View("NotFound");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    role.Name = model.RoleName;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }                
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role =await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色id为{roleId}的信息不存在，请重试！";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId=user.Id,
                    UserName=user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model,string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色id为{roleId}的信息不存在，请重试！";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                //判断当前用户是否已经属于该角色且已经被选中
                //不属于的话添加到角色中
                //没有选中要移除出来
                var isInRole = await userManager.IsInRoleAsync(user,role.Name);
                IdentityResult result = null;

                //被选中，不属于该角色，这个时候，添加到角色中
                if (model[i].IsSelected && !isInRole)
                {
                    result =await userManager.AddToRoleAsync(user,role.Name);
                }
                else if(!model[i].IsSelected && isInRole)
                { //没有被选中，但是用户已经在角色中，移除出来
                    result = await userManager.RemoveFromRoleAsync(user,role.Name);
                }
                else//被选中，且已经存在角色中，不发生任何改变的数据
                {
                    continue;//跳出当次循环
                }

                if (result.Succeeded)
                {
                    //7个总用户数，0开始进行索引。
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { id = roleId });

                    }

                }
            }


            return RedirectToAction("EditRole", new { id = roleId });
        }
    }
}