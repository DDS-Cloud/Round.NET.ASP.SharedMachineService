using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Round.NET.WebServer.Cs;

namespace Round.NET.WebServer.Pages.DDS
{
    public class RegistedModel : PageModel
    {
        public bool zts { get; set; }
        public void OnGet(string qq, string username, string password)
        {
            if (!System.IO.File.Exists("Config\\UserConfig.json"))
            {
                Directory.CreateDirectory("Config");
                System.IO.File.WriteAllText("Config\\UserConfig.json", "" +
                    "{\n" +
                    "   \"User\":[\n" +
                    "   ]\n" +
                    "}\n");
            }

            UserContainer userContainer = JsonConvert.DeserializeObject<UserContainer>(System.IO.File.ReadAllText("Config\\UserConfig.json"));
            foreach (var item in userContainer.User)
            {
                if (item.qq == qq || item.name == username)
                {
                    zts = true;
                }
                else
                {
                    zts = false;
                }
            }
            Users.AddUser(qq, username, password);
            Users.RegUser(qq);
        }
    }
}
