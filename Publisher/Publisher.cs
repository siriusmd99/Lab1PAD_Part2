using System;
using System.Text;
using Config;
using RabbitMQ.Client;
using static Config.LogExtensions;

namespace Publisher
{
    class Publisher : BasicClient
    {
      
        public bool Publish(string msg, string routingKey = "", string exchange = null )
        {
            try
            {
                channel.BasicPublish(exchange: (exchange == null) ? this.exchange : exchange, routingKey: routingKey, basicProperties: null, body: Encoding.UTF8.GetBytes(msg));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
    }
}
