using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQDemo.HelloWorld.Producer
{
    /// <summary>
    /// 基本使用Hello world
    /// </summary>
    public class Producer
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",//或127.0.0.1
                Port = 5672,//默认值
                UserName = "admin",
                Password = "P@ssw0rd.123"
            };

            //创建connection和channel
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //创建queue
                    channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    //生成消息
                    var msg = "Hello world!";
                    var body = Encoding.UTF8.GetBytes(msg);

                    //发送消息
                    channel.BasicPublish(exchange: "",
                        routingKey: "hello",
                        basicProperties: null,
                        body: body);

                    Console.WriteLine("[x] Sent {0}", msg);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
