using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Round.NET.WebServer.Cs;

namespace Round.NET.WebServer.Pages.DDS.Help
{
    public class ShowUserInqModel : PageModel
    {
        public string zts { get; set; }
        public void OnGet(string qq)
        {
            zts=Users.GetUserZT(qq);
        }
    }
}
