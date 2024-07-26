using Microsoft.AspNetCore.Mvc;
using DemoBLL.Repositories;
using DemoBLL.Interfaces;
using DemoDAL.Models;
using System.Threading;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace DemoPL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
       // private IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)//ASK CLR to create object 
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid) //Server Side Validation
            {

                
                    
                    await _unitOfWork.DepartmentRepository.Add(model);
                    int count = await _unitOfWork.Complete();
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
            }
            return View();
        }
        public async Task<IActionResult> Details(int? Id, string ViewName ="Details")
        {
            if(Id is null)
            {
                return BadRequest();//400
            }
            var department =await _unitOfWork.DepartmentRepository.Get(Id.Value);

            if (department is null)
            {
                return NotFound();//404
            }
           

            return View(ViewName,department);
        }
        [HttpGet]
        public Task<IActionResult> Edit(int? Id)
        {
            return Details(Id, "Edit");
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit ([FromRoute] int? id ,Department model)
        {
            if(id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)//server side validation
            {
                _unitOfWork.DepartmentRepository.Update(model);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
         
            return View(model);
        }
        [HttpGet]
        public Task<IActionResult> Delete (int? Id)
        {
            return Details(Id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Delete([FromRoute] int? id, Department model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)//server side validation
            {
                 _unitOfWork.DepartmentRepository.Delete(model);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
