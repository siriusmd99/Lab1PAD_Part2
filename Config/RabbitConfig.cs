using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public static class RabbitConfig
    {
        public static string RabbitHostname = "localhost";
        public static int RabbitPort = 5672;
        public static string RabbitUsername = "guest";
        public static string RabbitPassword = "guest";
        public static int Sub_Conn_Delay = 1000;
        public static string Exchange_Name = "t_meme_exchange";
    }
}
