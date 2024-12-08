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
                    FileName = "cmd.exe", // ָ��Ҫ�����ĳ���
                    Arguments = $"/c {command}", // ʹ��/c����ִ������Ȼ��ر�������ʾ��
                    UseShellExecute = false, // ��ʹ��ϵͳ��ǳ�������
                    RedirectStandardOutput = true, // �ض����׼���
                    RedirectStandardError = true, // �ض���������
                    CreateNoWindow = true // �������´���
                };

                try
                {
                    // �������̲�����
                    using (Process process = new Process { StartInfo = startInfo })
                    {
                        process.Start();

                        // ��ȡ��׼���
                        string output = process.StandardOutput.ReadToEnd();

                        // ��ȡ�������������У�
                        string error = process.StandardError.ReadToEnd();

                        // �ȴ����̽���
                        process.WaitForExit();

                        // ��ӡ���
                        Console.WriteLine("Output:");
                        Console.WriteLine(output);
                        CommandOut = output;

                        // ��ӡ��������У�
                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine("Error:");
                            Console.WriteLine(error);

                            CommandOut += "\n\n������Ϣ:\n" + error;
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
