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
    public class GenericRepository<T>:IGenericRepository<T> where T: BaseEntity
    {
        private protected  readonly AppDbContext _context;//NULL


        public GenericRepository(AppDbContext context)  //ASK CLR to create object from DBContexts 
        {
            _context = context;
        }

        public async Task Add(T entity)
        {

           
               await  _context.AddAsync(entity);
                
           
        }
        public void Update(T entity)
        {
            _context.Update(entity);

        }

        public void Delete(T entity)
        {
           
                _context.Remove(entity);
                
            
            
        }

        public async Task<T> Get(int id)
        {
            // var department = _context.Departments.FirstOrDefault(D => D.Id == id);
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
           // var department = _context.Departments.ToList();
           if(typeof(T)==typeof(Employee))
            {
                return  (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }
            else
            {
                return await _context.Set<T>().ToListAsync();
            }
        }

       
    }
}
