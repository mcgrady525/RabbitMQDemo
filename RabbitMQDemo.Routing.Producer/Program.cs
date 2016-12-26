using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQDemo.Routing.Producer
{
    /// <summary>
    /// 路由Routing
    /// 1，绑定Bindings
    /// 2，直连交换机direct和绑定key(routingKey)
    /// </summary>
    public class Producer
    {
        public static void Main(string[] args)
        {
            //创建直连交换机，并指定routingKey

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

                    //生成消息(批量)
                    for (int i = 0; i < 10; i++)
                    {
                        var msg = string.Format("message.{0}", i);
                        var body = Encoding.UTF8.GetBytes(msg);

                        //发送消息
                        channel.BasicPublish(exchange: "logs_direct",
                            routingKey: "error",//对直连交换机有用，对扇形交换机无效
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
