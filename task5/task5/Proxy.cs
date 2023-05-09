using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task5
{
    enum RolePriority
    {
        customer = 1,
        manager = 2,
        admin = 3
    }

    internal class PermissionProxy
    {
        public static Dictionary<string, int> ExpectedPriority()
        {
            return new Dictionary<string, int>
            {
                {"Search", 1 },
                {"Sort", 1 },
                {"Show", 1 },
                {"ShowById", 1 },
                {"ShowDrafts", 2 },
                {"ShowModerated", 2 },
                {"Pop", 3 },
                {"Publishing", 3 },
                {"Moderation", 2 },
                {"AddInto", 2 },
                {"Edit", 2 }
            };
        }

        public static bool CheckPermission(User user, string actionName, Event ev)
        {
            try
            {
                Dictionary<string, int> expectedPriority = ExpectedPriority();
                ev.UserName = String.Join(' ', user.Firstname, user.Lastname);
                ev.Time = DateTime.Now;
                ev.ActionName = actionName;
                if (Convert.ToInt32(Enum.Parse(typeof(RolePriority), user.Role)) < expectedPriority[actionName])
                {
                    ev.Description = "Attempt to complete action without appropriate level of priority";
                    throw new Exception($"For \"{actionName}\" you are expected to have higher level of priority!");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
    class Event
    {
        private string userName;
        private string actionName;
        private DateTime time;
        private string description;
        public string UserName { get { return userName; } set { userName = value; } }
        public string ActionName { get { return actionName; } set { actionName = value; } }
        public DateTime Time { get { return time; } set { time = value; } }
        public string Description { get { return description; } set { description = value; } }
        public Event()
        {

        }
        public override string ToString()
        {
            return $"{userName}, {actionName}, {time}, {description}\n";
        }
    }
    class ProxyLogger
    {
        public static void UpdateEvents(Event ev)
        {
            string filename = @"C:\np_4term\task5\task5\History.txt";
            using (TextWriter tw = File.AppendText(filename))
            {
                tw.WriteLine(ev);
            }
        }
    }
}
