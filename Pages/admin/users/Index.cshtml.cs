using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace SleekClothing.Pages.admin.users
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<IdentityUser> Users { get; set; }

        public void OnGet()
        {
            Users = _userManager.Users.ToList();
        }
    }
}
