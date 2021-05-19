
using BeerApi.Infrastructure.Messaging.Impl.RabbitMq;
using Microsoft.Extensions.Logging;

namespace BeerApi.Infrastructure.Messaging.Impl
{

    

    public class RabbitMqMessagingService : MessagingService
    {

        private readonly RabbitMqHandler _rabbitMqHandler;

        public RabbitMqMessagingService(ILogger<RabbitMqHandler> logger){
             _rabbitMqHandler = new RabbitMqHandler(logger);
            
        }
        public void Send(string beerId){
           _rabbitMqHandler.Send(beerId);
        }
    }
}