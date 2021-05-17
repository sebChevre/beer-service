using System;
using RabbitMQ.Client;
using System.Text;
using BeerApi.Services.Event;

namespace BeerApi.Infrastructure.Messaging.Impl.RabbitMq
{


    public class RabbitMqHandler
    {


        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;

        string _rabbitMqHost;
        int _rabbitMqPort;

        public RabbitMqHandler(){
            
            _rabbitMqHost =  Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            _rabbitMqPort = Int32.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));
            
            _connectionFactory = new ConnectionFactory() { HostName = _rabbitMqHost, Port = _rabbitMqPort };

            CreateConnection();
           
        }
        public void Send(string beerId)
        {
            if (ConnectionExists())
            {
                using(var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                    var b = new BeerCreatedEvent(){
                        BeerId = beerId,
                        Location = "/api/beer/" + beerId
                        
                    };

                    string message = b.AsJson();

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                        routingKey: "hello",
                                        basicProperties: null,
                                        body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
            

        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }

        private void CreateConnection()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }
    }

}