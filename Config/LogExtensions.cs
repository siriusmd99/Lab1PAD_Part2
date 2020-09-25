using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public static class LogExtensions
    {
        public static int Log(string str)
        {
            Console.Write(str);
            return 0;
        }
        public static int LogLine(string str)
        {
            Console.WriteLine(str);
            return 0;
        }
    }
}
