using DemoDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBLL.Interfaces
{
    public interface IGenericRepository <T> where T : BaseEntity 
    {
        Task< IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
