using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQDemo.WorkQueues.Producer
{
    /// <summary>
    /// 工作队列Work queues
    /// 1，循环调度
    /// 2，消息确认
    /// 3，消息持久化
    /// 4，公平调度
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
                    channel.QueueDeclare(queue: "task_queue",
                        durable: true,//消息持久化
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    //消息持久化
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    //生成消息(批量)
                    for (int i = 0; i < 10; i++)
                    {
                        var msg = string.Format("message.{0}", i);
                        var body = Encoding.UTF8.GetBytes(msg);

                        //发送消息
                        channel.BasicPublish(exchange: "",
                            routingKey: "task_queue",//???
                            basicProperties: properties,
                            body: body);

                        Console.WriteLine("[x] Sent {0}", msg);
                    }
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
