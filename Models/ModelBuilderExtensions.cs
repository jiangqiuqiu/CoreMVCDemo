using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVCDemo.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 1,
                    Name = "张三",
                    ClassName = ClassNameEnum.FirstGrade,
                    Email = "zhangsan@163.com"
                },
                new Student()
                {
                    Id = 2,
                    Name = "李四",
                    ClassName = ClassNameEnum.SecondGrade,
                    Email = "Lisi@163.com"
                }
                );
        }
    }
}
