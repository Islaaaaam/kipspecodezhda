using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace SleekClothing.Pages.admin.users
{
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public IdentityUser User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userInDb = await _userManager.FindByIdAsync(User.Id);
            if (userInDb == null)
            {
                return NotFound();
            }

            userInDb.UserName = User.UserName;
            userInDb.Email = User.Email;

            var result = await _userManager.UpdateAsync(userInDb);
            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
