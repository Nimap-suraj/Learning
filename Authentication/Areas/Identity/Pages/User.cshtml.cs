using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication.Areas.Identity.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUser? appUser { get; set; }

        public UserModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            appUser = await userManager.GetUserAsync(User);
            
        }
    }
}
