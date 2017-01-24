using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQTest.Messages;

namespace EasyNetQTest.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus("host=127.0.0.1:5672;username=admin;password=P@ssw0rd.123"))
            {
                var input = string.Empty;
                Console.WriteLine("Enter a message. 'Quit' to quit.");
                while ((input = Console.ReadLine()) != "Quit")
                {
                    bus.Publish(new TextMessage { Text = input }, p => p.WithTopic(""));
                }
            }
        }
    }
}
