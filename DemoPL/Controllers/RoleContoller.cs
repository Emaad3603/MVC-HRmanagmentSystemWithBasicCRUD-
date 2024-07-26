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
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;



        // Get, GetAll, Add, Update , Delete
        // Index, Details, Edit, Delete

        public RoleController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Index
        public async Task<IActionResult> Index(string searchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();


            if (string.IsNullOrEmpty(searchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name,
                }).ToListAsync();
            }
            else
            {

                roles = await _roleManager.Roles.Where(R => R.Name
                                  .ToLower()
                                  .Contains(searchInput.ToLower()))
                                  .Select(R => new RoleViewModel()
                                  {
                                      Id = R.Id,
                                      RoleName = R.Name

                                  }).ToListAsync();

            }

            return View(roles);
        }
        #endregion
        
        #region Create
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                var role = new IdentityRole()
                {

                    Name = model.RoleName
                };

                var result = await _roleManager.CreateAsync(role);
                var count = result.Succeeded;
                if (count)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View();
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest(); // 400



            var rolesFromDb = await _roleManager.FindByIdAsync(id);
            if (rolesFromDb is null)
                return NotFound(); // 404

            var role = new RoleViewModel()
            {
                Id = rolesFromDb.Id,
                RoleName = rolesFromDb.Name,
            };

            return View(ViewName, role);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {


                var rolesFromDb = await _roleManager.FindByIdAsync(id);
                if (rolesFromDb is null)
                    return NotFound(); // 404

                rolesFromDb.Name = model.RoleName;
                rolesFromDb.Id = model.Id;

                await _roleManager.UpdateAsync(rolesFromDb);

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }

        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var userFromDb = await _roleManager.FindByIdAsync(id);
                if (userFromDb is null)
                    return NotFound(); // 404


                await _roleManager.DeleteAsync(userFromDb);

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }

        #endregion

        #region AddOrRemoveUser
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();

            }
            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {

                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new { Id = roleId });
            }
            return View(users);
        }
    } 
    #endregion
}
