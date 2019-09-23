using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVCDemo.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "请输入名字"), MaxLength(50, ErrorMessage = "名字的长度不能超过50个字符")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        //值类型（例如 int，float，decimal，DateTime）本身就是必需的，不需要添加 Required 属性。
        [Required]
        [Display(Name = "班级信息")]
        public ClassNameEnum? ClassName { get; set; }//如果通过包含问号而使 ClassName 属性成为可为空的属性，就需要添加[Required]属性才能使该字段成为必填字段。

        [Display(Name = "电子邮件")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "邮箱的格式不正确")]
        [Required]
        public string Email { get; set; }
        public string PhotoPath { get; set; }
       
    }
}
