using AutoMapper;
using DemoBLL.Interfaces;
using DemoBLL.Repositories;
using DemoDAL.Models;
using DemoPL.Helper;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
       // private IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper/*,IDepartmentRepository departmentRepository*/)//ASK CLR to create object 
        {
            _unitOfWork = unitOfWork;
            
            _mapper = mapper;
            /*_departmentRepository = departmentRepository*/
            ;
        }
        public async Task<IActionResult> Index(string searchinput)
        {
            var employees = Enumerable.Empty<Employee>();

          //  _unitOfWork.Complete();  

            if (string.IsNullOrEmpty(searchinput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAll();

            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByName(searchinput.ToLower());
            }

             var result =  _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);





            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.Add(employee);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View();
        }
        public async Task<IActionResult> Details(int? Id, string ViewName = "Details")
        {
            if (Id is null)
            {
                return BadRequest();//400
            }
            var employee = await _unitOfWork.EmployeeRepository.Get(Id.Value);

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            if (employee is null)
            {
                return NotFound();//404
            }


            return View(ViewName, employeeViewModel);
        }
        [HttpGet]
        public Task<IActionResult> Edit(int? Id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            return Details(Id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if(model.ImageName is not null)
            {
                DocumentSettings.DeleteFile(model.ImageName,"images");
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

            }
            else
            {
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
            }
            var employee = _mapper.Map<Employee>(model);
            if (ModelState.IsValid)//server side validation
            {
                
                _unitOfWork.EmployeeRepository.Update(employee);

                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        [HttpGet]
        public Task<IActionResult> Delete(int? Id)
        {
            return Details(Id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var employee = _mapper.Map<Employee>(model);

            if (ModelState.IsValid)//server side validation
            {
                _unitOfWork.EmployeeRepository.Delete(employee);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
