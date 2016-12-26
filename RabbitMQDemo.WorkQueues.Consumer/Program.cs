using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace RabbitMQDemo.WorkQueues.Consumer
{
    /// <summary>
    /// 工作队列Work queues
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
                    channel.QueueDeclare(queue: "task_queue",
                        durable: true,//消息持久化
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    //公平调度，同一时刻，不要发送超过1条消息给一个消费者
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

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

                        //消息确认
                        channel.BasicAck(deliveryTag:ea.DeliveryTag, multiple:false);

                    };

                    channel.BasicConsume(queue: "task_queue",
                        noAck:false,
                        consumer: consumer);

                    //这段代码不能放在外面，否则接收不到消息
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
