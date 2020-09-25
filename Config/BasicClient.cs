using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public abstract class BasicClient
    {
        protected ConnectionFactory factory;
        protected IConnection connection;
        protected IModel channel;
        protected string exchange;

        public bool Connect(string RabbitHostname, int RabbitPort, string RabbitUsername, string RabbitPassword)
        {
            try
            {
                factory = new ConnectionFactory
                {
                    HostName = RabbitHostname,
                    Port = RabbitPort,
                    UserName = RabbitUsername,
                    Password = RabbitPassword
                };

                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateExchange(string exchange, bool setexchange = true)
        {
            try
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, true, false, null);
                if (setexchange)
                    SetExchange(exchange);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SetExchange(string exchange)
        {
            this.exchange = exchange;
        }
    }
}
