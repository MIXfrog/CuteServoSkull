using TestWebApp.Handlers;
using TestWebApp.Handlers.Impl;
using VkNet.Abstractions;
using VkNet.Model;

namespace TestWebApp.Factory.Impl
{
    public class IncomeMessageHandlerFactory : IIncomeMessageHandlerFactory
    {
        private readonly IVkApi _vkApi;

        private const string ChastushkiStringIndicator = "частуш";
        private const string PollStringIndicator = "создай опрос";
        private const string RecordingStringIndicator = "запись";

        public IncomeMessageHandlerFactory(IVkApi vkApi)
        {
            _vkApi = vkApi;
        }

        public IHandler CreateHandler(Message message)
        {
            var incomeMessage = message.Text.ToLower().Trim();

            if (incomeMessage.Contains(ChastushkiStringIndicator))
            {
                return new ChastushkiMessageHandler(_vkApi, message);
            }
            if (incomeMessage.Contains(PollStringIndicator))
            {
                return new PollMessageHandler(_vkApi, message);
            }
            if (incomeMessage.Contains(RecordingStringIndicator))
            {
                return new RecordingMessageHandler(_vkApi, message);
            }
            else
            {
                return new DefaultMessageHandler(_vkApi, message);
            }
        }
    }
}
