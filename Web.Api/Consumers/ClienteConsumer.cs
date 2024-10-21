using Application.Dto;
using MassTransit;

namespace Web.Api.Consumers
{
    public class ClienteConsumer : IConsumer<ClienteDto>
    {
        public Task Consume(ConsumeContext<ClienteDto> context)
        {
            var aa = context.Message;

            return Task.FromResult("");
        }
    }
}
