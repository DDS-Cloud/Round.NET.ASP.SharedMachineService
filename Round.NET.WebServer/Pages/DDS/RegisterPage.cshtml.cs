using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Round.NET.WebServer.Cs;
using System.Diagnostics;

namespace Round.NET.WebServer.Pages.DDS
{
    public class RegisterPageModel : PageModel
    {
        public bool zts {  get; set; }
        public void OnGet(string red)
        {
            if (red == "yes")
            {
                zts = false;
            }
            else
            {
                zts = false;
            }
        }
    }
}