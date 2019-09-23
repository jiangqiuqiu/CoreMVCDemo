using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVCDemo.DataAccess;
using CoreMVCDemo.Models;

namespace CoreMVCDemo.Repository
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;//readonly防止误操作，符合安全规范

        //通过依赖注入将数据库上下文连接注入进来
        public SQLStudentRepository(AppDbContext context)
        {
            this._context = context;
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
