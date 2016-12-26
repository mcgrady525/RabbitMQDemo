using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace RabbitMQDemo.PubSub.Consumer
{
    /// <summary>
    /// 发布订阅Publish/Subscribe
    /// </summary>
    public class Consumer
    {
        public static void Main(string[] args)
        {
            //consumer端需要创建交换机，创建队列，然后将交换机与队列绑定，最后接收消息
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
                    channel.ExchangeDeclare(exchange: "logsNew", type: ExchangeType.Fanout, durable: true);

                    //创建queue，服务器生成的随机队列名，后面会自动清除
                    var queueName = channel.QueueDeclare().QueueName;

                    //创建bindings
                    channel.QueueBind(queue: queueName, exchange: "logsNew", routingKey: "");

                    //接收(消费)消息
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var msg = Encoding.UTF8.GetString(body);
                        Console.WriteLine("[x] Received {0}", msg);

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
