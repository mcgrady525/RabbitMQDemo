using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace RabbitMQDemo.Routing.Consumer
{
    /// <summary>
    /// 路由Routing
    /// </summary>
    public class Consumer
    {
        public static void Main(string[] args)
        {
            //需要为每个感兴趣的routingKey创建一个绑定

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
                    //创建交换机
                    channel.ExchangeDeclare(exchange: "logs_direct", type: ExchangeType.Direct, durable: true);

                    //创建queue，服务器生成的随机队列名，后面会自动清除
                    var queueName = channel.QueueDeclare().QueueName;

                    //创建bindings
                    channel.QueueBind(queue: queueName, exchange: "logs_direct", routingKey: "error");

                    //接收(消费)消息
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var msg = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine("[x] Received RoutingKey:{0},msg:{1}",routingKey, msg);

                        //模拟耗时操作
                        Thread.Sleep(1000);
                        Console.WriteLine("[x] Done");
                    };

                    channel.BasicConsume(queue: queueName,
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
