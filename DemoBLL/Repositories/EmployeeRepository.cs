using DemoBLL.Interfaces;
using DemoDAL.Data;
using DemoDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context):base(context) 
        {
            
        }

        public async Task<IEnumerable<Employee>> GetByName(string name)
        {
           return await _context.Employees.Where(E=>E.Name.ToLower().Contains(name.ToLower())).Include(E=>E.Department).ToListAsync();
        }
    }
}
