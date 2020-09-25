using System;
using System.Text;
using Config;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Subscriber
{
    class Subscriber : BasicClient
    {
        private string queue_name;
        private Action<Meme> callback;

        public bool NewQueue(string subreddit)
        {
            try 
            {
                queue_name = channel.QueueDeclare();
                channel.QueueBind(queue: queue_name, exchange: this.exchange, routingKey: subreddit);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
           
        }

        public bool BeginConsume(Action<Meme> callback)
        {
            try
            {
                this.callback = callback;
                
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += OnConsumerReceive;
                channel.BasicConsume(queue: queue_name, autoAck: true, consumer: consumer);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private void OnConsumerReceive(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            Meme meme = JsonConvert.DeserializeObject<Meme>(message, jsonSerializerSettings);

            callback(meme);
        }

        public void DropQueue()
        {
            channel.QueueDelete(queue_name);
        }

        public string GetQueue()
        {
            return queue_name;
        }
            
    }
}
