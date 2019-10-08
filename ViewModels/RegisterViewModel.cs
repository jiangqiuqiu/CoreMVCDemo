using CoreMVCDemo.CustomerUtil;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVCDemo.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="邮箱地址")]
        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller:"Account")]//通过指定的路由去后台验证该属性的值知否有效，属于服务器验证
        [ValidEmailDomain(allowedDomain:"52abp.com",ErrorMessage ="电子邮箱后缀必须是52abp.com")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="确认密码")]
        [Compare("Password",
            ErrorMessage ="密码与确认密码不一致，请重新输入.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }
    }
}
