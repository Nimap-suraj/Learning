using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication.Areas.Identity.Pages
{
    [Authorize(Roles ="admin")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
