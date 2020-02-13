using TestWebApp.Handlers;
using TestWebApp.Handlers.Impl;
using TestWebApp.Services;
using VkNet.Abstractions;
using VkNet.Model;

namespace TestWebApp.Factory.Impl
{
    public class IncomeMessageHandlerFactory : IIncomeMessageHandlerFactory
    {
        private readonly IVkApiIntegrationService _vkApiIntegration;

        private const string ChastushkiStringIndicator = "частуш";
        private const string RecordingStringIndicator = "запись";

        public IncomeMessageHandlerFactory(IVkApiIntegrationService vkApiIntegration)
        {
            _vkApiIntegration = vkApiIntegration;
        }

        public IHandler CreateHandler(Message message)
        {
            var incomeMessage = message.Text.ToLower().Trim();

            if (incomeMessage.Contains(ChastushkiStringIndicator))
            {
                return new ChastushkiMessageHandler(_vkApiIntegration, message);
            }
            if (incomeMessage.Contains(RecordingStringIndicator))
            {
                return new RecordingMessageHandler(_vkApiIntegration, message);
            }
            else
            {
                return new DefaultMessageHandler(_vkApiIntegration, message);
            }
        }
    }
}
