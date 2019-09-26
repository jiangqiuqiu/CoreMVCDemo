using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVCDemo.DataAccess;
using CoreMVCDemo.Models;
using Microsoft.Extensions.Logging;

namespace CoreMVCDemo.Repository
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;//readonly防止误操作，符合安全规范
        private readonly ILogger<SQLStudentRepository> logger;

        //通过依赖注入将数据库上下文连接注入进来
        public SQLStudentRepository(AppDbContext context,ILogger<SQLStudentRepository> logger)
        {
            this._context = context;
            this.logger = logger;
        }

        public Student Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _context.Students.Find(id);
            if (student!=null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            //现在要模拟在CoreMVCDemo.Controllers.HomeController的Details中要记录所有级别的日志
            //而在此Action中只记录Error级别以上的日志
            //要完成这个，只需要在appsettings.json的Logging的LogLevel中配置
            //"CoreMVCDemo.Controllers.HomeController": "Trace",
            //"CoreMVCDemo.Repository.SQLStudentRepository": "Error",
            //即可
            logger.LogTrace("学生信息 Trace(跟踪) Log");
            logger.LogDebug("学生信息 Debug(调试) Log");
            logger.LogInformation("学生信息 Information(信息)Log");
            logger.LogWarning("学生信息 Warning(警告)Log");
            logger.LogError("学生信息 Error(错误)Log");
            logger.LogCritical("学生信息 Critical(严重)Log");

            return _context.Students;
        }

        public Student GetStudent(int id)
        {
            return  _context.Students.Find(id);            
        }

        public void Save(Student student)
        {
            throw new NotImplementedException();
        }

        public Student Update(Student updateStudent)
        {
            var student = _context.Students.Attach(updateStudent);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateStudent;
        }
    }
}
