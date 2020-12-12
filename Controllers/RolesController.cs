using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using test5.Data;

namespace test5.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }


        [HttpGet]
        public IActionResult Upsert(string id)
        { if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                var objFromDb = _db.Roles.FirstOrDefault(u => u.Id == id);
                return View(objFromDb);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roleObj)
        { if (await _roleManager.RoleExistsAsync(roleObj.Name))
            {
                return View();
            }
            if (string.IsNullOrEmpty(roleObj.Id))
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = roleObj.Name });
            }
            else
            {
                var objRoleFromDb = _db.Roles.FirstOrDefault(u => u.Id == roleObj.Id);
                objRoleFromDb.Name = roleObj.Name;
                objRoleFromDb.NormalizedName = roleObj.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(objRoleFromDb);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}