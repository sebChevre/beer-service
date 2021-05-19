using System;
using RabbitMQ.Client;
using System.Text;
using BeerApi.Services.Event;
using Microsoft.Extensions.Logging;


namespace BeerApi.Infrastructure.Messaging.Impl.RabbitMq
{


    public class RabbitMqHandler 
    {

        
        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;

        private ILogger<RabbitMqHandler> _logger;
        string _rabbitMqHost;
        int _rabbitMqPort;

        public RabbitMqHandler(ILogger<RabbitMqHandler> logger){
            _logger = logger;
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
                    channel.QueueDeclare(queue: MessagingConfig.BeerCreatedQueueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                    var b = new BeerCreatedEvent(){
                        BeerId = beerId,
                        Location = MessagingConfig.BeerCreatedApiPath + beerId
                        
                    };

                    string message = b.AsJson();

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: MessagingConfig.DefaultExchangeName,
                                        routingKey: MessagingConfig.BeerCreatedQueueName,
                                        basicProperties: null,
                                        body: body);

                   _logger.LogInformation("Message Sent {0}", message);
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
                _logger.LogError($"Could not create connection: {ex.Message}");
            }
        }
    }

}