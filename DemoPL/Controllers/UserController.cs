using DemoDAL.Models;
using DemoPL.Helper;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(string searchinput)
        {
            var users = Enumerable.Empty<UserViewModel>();

            //  _unitOfWork.Complete();  

            if (string.IsNullOrEmpty(searchinput))
            {
                users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync() ;

            }
            else
            {
                users = await _userManager.Users.Where(U => U.Email
                                                            .ToLower()
                                                            .Contains(searchinput.ToLower()))
                                                            .Select(U => new UserViewModel()
                                                            {
                                                                Id = U.Id,
                                                                FirstName = U.FirstName,
                                                                LastName = U.LastName,
                                                                Email = U.Email,
                                                                Roles = _userManager.GetRolesAsync(U).Result
                                                            }).ToListAsync();
            }

   

            return View(users);
        }

        public async Task<IActionResult> Details(string? Id, string ViewName = "Details")
        {
            if (Id is null)
            {
                return BadRequest();//400
            }
            var userFromDb = await _userManager.FindByIdAsync(Id);

           

            if (userFromDb is null)
            {
                return NotFound();//404
            }

            var user = new UserViewModel()
            {
                Id = userFromDb.Id,
                FirstName = userFromDb.FirstName,
                LastName = userFromDb.LastName,
                Email = userFromDb.Email,
                Roles = _userManager.GetRolesAsync(userFromDb).Result
            };

            return View(ViewName, user);
        }
        [HttpGet]
        public Task<IActionResult> Edit(string? Id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            return Details(Id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
          
            
            if (ModelState.IsValid)//server side validation
            {
                var userFromDb = await _userManager.FindByIdAsync(id);


                if (userFromDb is null)
                {
                    return NotFound();//404
                }

                userFromDb.FirstName = model.FirstName;
                userFromDb.LastName = model.LastName;
                userFromDb.Email = model.Email;
                userFromDb.Id = model.Id;
                
                var result = await  _userManager.UpdateAsync(userFromDb);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        [HttpGet]
        public Task<IActionResult> Delete(string? Id)
        {
            return Details(Id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }


            if (ModelState.IsValid)//server side validation
            {
                var userFromDb = await _userManager.FindByIdAsync(id);


                if (userFromDb is null)
                {
                    return NotFound();//404
                }

             
                var result = await _userManager.DeleteAsync(userFromDb);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}

