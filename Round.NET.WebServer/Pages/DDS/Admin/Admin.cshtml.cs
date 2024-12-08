using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Round.NET.WebServer.Cs;
using System.Diagnostics;

namespace Round.NET.WebServer.Pages.DDS.Admin
{
    public class AdminModel : PageModel
    {
        public List<User> Items { get; set; } = new List<User>();
        public int pagenum1 { get; set; } = 0;
        public int UsersNum { get; set; }
        public int TaskNum { get; set; }
        public string CommandOut { get; set; }
        public void OnGet(string pagenum = null, string qq = null, string type = null, string keys = null, string command = null)
        {
            UsersNum = Users.GetUserList().Count;
            TaskNum = Process.GetProcesses().Count();

            if (pagenum != null)
            {
                pagenum1 = int.Parse(pagenum);
            }
            if (type == "dot")
            {
                Console.WriteLine("dot " + qq);
                Users.UnRegUser(qq);
            }
            else if (type == "yes")
            {
                Console.WriteLine("yes " + qq);
                Users.RegUser(qq);
            }

            Items = new List<User>();
            foreach (var item in Users.GetUserList())
            {
                if (!item.zt)
                {
                    Items.Add(item);
                }
            }

            if (pagenum == "1")
            {
                Console.WriteLine("del " + keys);
                Users.DelRegUser(keys);
            }

            if (pagenum == "3") {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe", // 指定要启动的程序
                    Arguments = $"/c {command}", // 使用/c参数执行命令然后关闭命令提示符
                    UseShellExecute = false, // 不使用系统外壳程序启动
                    RedirectStandardOutput = true, // 重定向标准输出
                    RedirectStandardError = true, // 重定向错误输出
                    CreateNoWindow = true // 不创建新窗口
                };

                try
                {
                    // 创建进程并启动
                    using (Process process = new Process { StartInfo = startInfo })
                    {
                        process.Start();

                        // 读取标准输出
                        string output = process.StandardOutput.ReadToEnd();

                        // 读取错误输出（如果有）
                        string error = process.StandardError.ReadToEnd();

                        // 等待进程结束
                        process.WaitForExit();

                        // 打印输出
                        Console.WriteLine("Output:");
                        Console.WriteLine(output);
                        CommandOut = output;

                        // 打印错误（如果有）
                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine("Error:");
                            Console.WriteLine(error);

                            CommandOut += "\n\n错误信息:\n" + error;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
