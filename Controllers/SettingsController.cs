using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(ApplicationDbContext context,
                                  IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Display Company Settings
        public async Task<IActionResult> Index()
        {
            var settings = await _context.CompanySettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        // Edit Company Settings
        public async Task<IActionResult> Edit()
        {
            var settings = await _context.CompanySettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanySettings model, IFormFile? LogoFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            var settings = await _context.CompanySettings
                                         .FirstOrDefaultAsync(x => x.SettingId == model.SettingId);

            if (settings == null)
                return NotFound();

            settings.CompanyName = model.CompanyName;
            settings.Address = model.Address;
            settings.City = model.City;
            settings.State = model.State;
            settings.Country = model.Country;
            settings.PinCode = model.PinCode;
            settings.Phone = model.Phone;
            settings.Email = model.Email;
            settings.Website = model.Website;
            settings.FinancialYear = model.FinancialYear;
            settings.OfficeStart = model.OfficeStart;
            settings.OfficeEnd = model.OfficeEnd;
            settings.GraceMinutes = model.GraceMinutes;

            // Upload Logo
            if (LogoFile != null && LogoFile.Length > 0)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "CompanyLogo");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(LogoFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await LogoFile.CopyToAsync(stream);
                }

                settings.CompanyLogo = fileName;
            }

            _context.Update(settings);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Company settings updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}