using CoreMVCDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVCDemo.DataAccess
{
    //public class AppDbContext:DbContext
    public class AppDbContext : IdentityDbContext<ApplicationUser>//要使用.NET CORE的Identity系统，第一步先在这里继承IdentityDbContext
    {
        //DbContextOptions 实例承载配置信息，如连接字符串，数据库提供商使用等
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }        
        public DbSet<Student> Students { get; set; }

        //为我们的数据库添加初始数据，就称之为种子数据
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//继承基类里的继续执行

            modelBuilder.Seed();//通过扩展方法封装种子数据
        }
    }
}
