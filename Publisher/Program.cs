using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Config.RabbitConfig;
using static Config.Service;
using Config;
using static Config.LogExtensions;
using Newtonsoft.Json;

namespace Publisher
{
    class Program
    {

        static void Main()
        {
            Thread thread = new Thread(Publisher_Thread);
            thread.Start();
            Console.ReadKey();
        }

        static void Publisher_Thread()
        {
            StartConnection();
        }

        static int StartConnection()
        {
            Publisher publisher = new Publisher();

            if (!publisher.Connect(RabbitHostname, RabbitPort, RabbitUsername, RabbitPassword))
                return LogLine($"Could not connect to Rabbit Broker ({RabbitHostname}) on Port: {RabbitPort}");


            if (!publisher.CreateExchange(Exchange_Name))
                return LogLine($"Could not create exchange : {Exchange_Name} at Broker: {RabbitHostname}");


            using (WebClient client = new WebClient())
            {
                while (true)
                {

                    string json = client.DownloadString(API_URL);
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    Meme meme = JsonConvert.DeserializeObject<Meme>(json, jsonSerializerSettings);
                    LogLine($"Sent {json}\n\n");

                    if (!publisher.Publish(json, meme.subreddit))
                        return LogLine($"Could not publish to exchange: {Exchange_Name} at Broker: {RabbitHostname}");
                    else
                        LogLine($"Publishing to Topic: {meme.subreddit}");

                    Thread.Sleep(Sleep_Delay);
                }

            }
        }

       




    }
}
