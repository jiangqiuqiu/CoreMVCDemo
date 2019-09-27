﻿using System;
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
    }
}
