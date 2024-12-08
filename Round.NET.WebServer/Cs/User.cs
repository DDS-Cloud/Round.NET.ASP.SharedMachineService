using Newtonsoft.Json;
using System.Diagnostics;

namespace Round.NET.WebServer.Cs
{
    public class User
    {
        public string qq { get; set; }
        public string pwd { get; set; }
        public string name { get; set; }
        public bool zt { get; set; }
    }
    public class UserContainer
    {
        public List<User> User { get; set; } = new List<User>();
    }

    public static class Users
    {
        public static void AddUser(string qq, string name, string pwd)
        {
            UserContainer userContainer = JsonConvert.DeserializeObject<UserContainer>(File.ReadAllText("Config\\UserConfig.json"));

            // 创建一个用户实例并添加到UserContainer的User列表中
            userContainer.User.Add(new User { name = name, pwd = pwd, zt = false, qq = qq });

            // 将UserContainer对象序列化为JSON字符串
            string json = JsonConvert.SerializeObject(userContainer, Formatting.Indented);

            // 输出JSON字符串
            File.WriteAllText("Config\\UserConfig.json", json);
        }
        public static string GetUserZT(string qq)
        {
            UserContainer userContainer = JsonConvert.DeserializeObject<UserContainer>(File.ReadAllText("Config\\UserConfig.json"));

            foreach (var item in userContainer.User)
            {
                if (item.qq == qq)
                {
                    if (item.zt)
                    {
                        return "已审核，可以连接";
                    }
                    else
                    {
                        return "未审核";
                    }
                }
            }
            return "未注册或已被拒绝!";
        }
        public static List<User> GetUserList()
        {
            UserContainer userContainer = JsonConvert.DeserializeObject<UserContainer>(File.ReadAllText("Config\\UserConfig.json"));

            return userContainer.User;
        }
        public static void RegUser(string qq)
        {
            UserContainer userContainer = JsonConvert.DeserializeObject<UserContainer>(File.ReadAllText("Config\\UserConfig.json"));

            foreach (var item in userContainer.User)
            {
                if (item.qq == qq)
                {
                    item.zt = true;
                    AddUser(item.name, item.pwd);
                    AddToRemoteDesktopGroup(item.name);
                }
            }

            // 将UserContainer对象序列化为JSON字符串
            string json = JsonConvert.SerializeObject(userContainer, Formatting.Indented);

            // 输出JSON字符串
            File.WriteAllText("Config\\UserConfig.json", json);
        }
        public static void UnRegUser(string qq)
        {
            UserContainer userContainer = JsonConvert.DeserializeObject<UserContainer>(File.ReadAllText("Config\\UserConfig.json"));

            foreach (var item in userContainer.User)
            {
                if (item.qq == qq)
                {
                    userContainer.User.Remove(item);
                    break;
                }
            }

            // 将UserContainer对象序列化为JSON字符串
            string json = JsonConvert.SerializeObject(userContainer, Formatting.Indented);

            // 输出JSON字符串
            File.WriteAllText("Config\\UserConfig.json", json);
        }

        public static void DelRegUser(string qq)
        {
            UserContainer userContainer = JsonConvert.DeserializeObject<UserContainer>(File.ReadAllText("Config\\UserConfig.json"));

            foreach (var item in userContainer.User)
            {
                if (item.qq == qq||item.name == qq)
                {
                    DelUser(item.name);
                    userContainer.User.Remove(item);
                    break;
                }
            }

            // 将UserContainer对象序列化为JSON字符串
            string json = JsonConvert.SerializeObject(userContainer, Formatting.Indented);

            // 输出JSON字符串
            File.WriteAllText("Config\\UserConfig.json", json);
        }

        static void AddUser(string username, string password)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c net user {username} {password} /add",
                Verb = "runas", // 以管理员身份运行
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(processStartInfo).WaitForExit();
        }
        static void DelUser(string username)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c net user {username} /del",
                Verb = "runas", // 以管理员身份运行
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(processStartInfo).WaitForExit();
        }

        static void AddToRemoteDesktopGroup(string username)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c net localgroup \"Remote Desktop Users\" {username} /add",
                Verb = "runas", // 以管理员身份运行
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(processStartInfo).WaitForExit();
        }
    }
}
