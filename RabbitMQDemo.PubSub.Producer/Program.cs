using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQDemo.PubSub.Producer
{
    /// <summary>
    /// 发布订阅Publish/Subscribe
    /// 1，交换机Exchange，交换机类型：direct,topic,headers和fanout
    /// 2，绑定Bindings，将交换机与队列绑定
    /// </summary>
    public class Producer
    {
        public static void Main(string[] args)
        {
            //producer端只需要创建交换机，然后将消息发送给交换机就可以

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

                    //生成消息(批量)
                    for (int i = 0; i < 10; i++)
                    {
                        var msg = string.Format("message.{0}", i);
                        var body = Encoding.UTF8.GetBytes(msg);

                        //发送消息
                        channel.BasicPublish(exchange: "logsNew",
                            routingKey: "",//如果使用了exchange这里就可以为空
                            basicProperties: null,
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
