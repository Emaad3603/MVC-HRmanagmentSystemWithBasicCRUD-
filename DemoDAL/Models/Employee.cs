using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDAL.Models
{
    //Model
    public class Employee : BaseEntity
    {

        [Required]
        public string Name { get; set; }
        
        public int Age { get; set; }

        public decimal Salary { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        
        public DateTime HireDate { get; set; }

        public string ImageName { get; set; }

        public DateTime DateOfCreation { get; set;}

        public int? DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
