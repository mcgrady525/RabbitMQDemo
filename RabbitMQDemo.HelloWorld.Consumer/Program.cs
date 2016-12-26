using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQDemo.HelloWorld.Consumer
{
    /// <summary>
    /// 基本使用Hello world
    /// </summary>
    public class Consumer
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

                    //接收(消费)消息
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var msg = Encoding.UTF8.GetString(body);

                        Console.WriteLine("[x] Received {0}", msg);
                    };

                    channel.BasicConsume(queue: "hello",
                        noAck: true,
                        consumer: consumer);

                    //这段代码不能放在外面，否则接收不到消息
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
