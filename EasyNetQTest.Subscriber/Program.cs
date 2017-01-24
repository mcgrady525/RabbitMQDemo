using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQTest.Messages;

namespace EasyNetQTest.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bus= RabbitHutch.CreateBus("host=127.0.0.1:5672;username=admin;password=P@ssw0rd.123"))
            {
                bus.Subscribe<TextMessage>("test", HandleTextMessage);

                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="textMessage"></param>
        static void HandleTextMessage(TextMessage textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Get message: {0}", textMessage.Text);
            Console.ResetColor();
        }
    }
}
