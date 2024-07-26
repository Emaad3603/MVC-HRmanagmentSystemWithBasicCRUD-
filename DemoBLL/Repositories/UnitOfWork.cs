using DemoBLL.Interfaces;
using DemoDAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBLL.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly AppDbContext _context;
        private Lazy< IDepartmentRepository> departmentRepository;
        private Lazy<IEmployeeRepository> employeeRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            departmentRepository =new Lazy<IDepartmentRepository>(new DepartmentRepository(_context));
            employeeRepository = new Lazy<IEmployeeRepository>(new EmployeeRepository(_context));
            
        }
        public IDepartmentRepository DepartmentRepository => departmentRepository.Value;
        public IEmployeeRepository EmployeeRepository => employeeRepository.Value;

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 
