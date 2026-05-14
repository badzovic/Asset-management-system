using AMS_data;
using AMS_data.Entities;
using Asset_management_Web_Core.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMS_services.Audit;

namespace Asset_management_Web_Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly AuditLogService _auditLogService;
        public UsersController(
         UserManager<ApplicationUser> userManager,
         RoleManager<IdentityRole> roleManager,
         ApplicationDbContext db,
         AuditLogService auditLogService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _auditLogService = auditLogService;
        }

        public async Task<IActionResult> Create()
        {
            var model = await BuildModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await BuildModel(model);
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = $"{model.UserName}@local.local",

                Ime = model.Ime,
                Prezime = model.Prezime,

                OrganizacionaJedinicaId = model.OrganizacionaJedinicaId,
                SkladisteId = model.SkladisteId,

                Aktivan = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                model = await BuildModel(model);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.RoleName);

            await _auditLogService.LogAsync(
            action: "CREATE_USER",
            entityName: "ApplicationUser",
            entityId: user.Id,
            newValues: new
            {
                user.UserName,
                user.Ime,
                user.Prezime,
                Role = model.RoleName,
                user.OrganizacionaJedinicaId,
                user.SkladisteId
            });

            TempData["SuccessMessage"] = "Korisnik uspješno kreiran.";

            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName!,
                Ime = user.Ime ?? "",
                Prezime = user.Prezime ?? "",
                RoleName = roles.FirstOrDefault() ?? "",
                OrganizacionaJedinicaId = user.OrganizacionaJedinicaId,
                SkladisteId = user.SkladisteId,
                Aktivan = user.Aktivan
            };

            await PopulateEditModel(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateEditModel(model);
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            user.UserName = model.UserName;
            user.Ime = model.Ime;
            user.Prezime = model.Prezime;
            user.OrganizacionaJedinicaId = model.OrganizacionaJedinicaId;
            user.SkladisteId = model.SkladisteId;
            user.Aktivan = model.Aktivan;

            await _userManager.UpdateAsync(user);

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, model.RoleName);

            TempData["SuccessMessage"] = "Korisnik uspješno izmijenjen.";

            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var model = new ResetPasswordViewModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? ""
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            TempData["SuccessMessage"] =
                $"Password uspješno resetovan za korisnika {user.UserName}.";

            return RedirectToAction(nameof(Create));
        }
        private async Task PopulateEditModel(EditUserViewModel model)
        {
            model.Roles = await _roleManager.Roles
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Name!,
                    Text = x.Name!
                })
                .ToListAsync();

            model.OrganizacioneJedinice = await _db.OrganizacioneJedinice
                .OrderBy(x => x.Naziv)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                })
                .ToListAsync();

            model.Skladista = await _db.Skladista
                .OrderBy(x => x.Naziv)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                })
                .ToListAsync();
        }

        private async Task<CreateUserViewModel> BuildModel(CreateUserViewModel? model = null)
        {
            model ??= new CreateUserViewModel();

            model.Roles = await _roleManager.Roles
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Name!,
                    Text = x.Name!
                })
                .ToListAsync();

            model.OrganizacioneJedinice = await _db.OrganizacioneJedinice
                .OrderBy(x => x.Naziv)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                })
                .ToListAsync();

            model.Skladista = await _db.Skladista
                .OrderBy(x => x.Naziv)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                })
                .ToListAsync();
            var users = await _userManager.Users
    .OrderBy(x => x.UserName)
    .ToListAsync();

            model.Users = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Users.Add(new UserListItemViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Ime = user.Ime,
                    Prezime = user.Prezime,
                    RoleName = roles.FirstOrDefault(),
                    OrganizacionaJedinica = user.OrganizacionaJedinicaId?.ToString(),
                    Skladiste = user.SkladisteId?.ToString(),
                    Aktivan = user.Aktivan
                });
            }


            return model;
        }
    }
}