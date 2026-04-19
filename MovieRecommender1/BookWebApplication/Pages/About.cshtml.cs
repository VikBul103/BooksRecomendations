using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWebApplication.Pages
{
    public class AboutModel : PageModel
    {
        public required string CurrentDate { get; set; }
        public void OnGet()
        {
            CurrentDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }
    }
}
