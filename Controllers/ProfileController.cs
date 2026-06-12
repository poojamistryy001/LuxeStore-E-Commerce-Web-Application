using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LuxeStore.Models;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        return View(user);
    }

    // GET
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);

        var model = new EditProfileVM
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Edit(EditProfileVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);

        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;

        await _userManager.UpdateAsync(user);

        TempData["Success"] = "Profile updated successfully";

        return RedirectToAction("Index");
    }
}