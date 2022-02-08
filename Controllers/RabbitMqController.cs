using HttpIntegrationRabbitMq.BusMessages;
using HttpIntegrationRabbitMq.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HttpIntegrationRabbitMq.Controllers
{
    [Route("api/rabbitmq")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IBusClient _busClient;

        public RabbitMqController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok(nameof(RabbitMqController));
        }

        [Route("publish")]
        [HttpPost]
        public ActionResult PublishMessage([FromBody] PublishGeneric message)
        {
            var result = _busClient.Publish(message);
            return Ok(message);
        }

        [Route("publish/deuda-pagada")]
        [HttpPost]
        public ActionResult PublisDeudaCancelada([FromBody] PublishMessage<DeudaPagada> message)
        {
            var result = _busClient.Publish(message);
            return Ok(message);
        }
        [Route("publish/deuda-cancelada")]
        [HttpPost]
        public ActionResult PublisDeudaCancelada([FromBody] PublishMessage<DeudaCancelada> message)
        {
            var result = _busClient.Publish(message);
            return Ok(message);
        }
    }
}
