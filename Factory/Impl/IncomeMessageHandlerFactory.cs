using Microsoft.Extensions.Configuration;
using TestWebApp.Handlers;
using TestWebApp.Handlers.Impl;
using VkNet.Abstractions;
using VkNet.Model;

namespace TestWebApp.Factory.Impl
{
    public class IncomeMessageHandlerFactory : IIncomeMessageHandlerFactory
    {
        private readonly IVkApi _vkApi;
        private readonly IConfiguration _configuration;

        public IncomeMessageHandlerFactory(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
        }

        public IHandler CreateHandler(Message message)
        {
            if (message.Text.ToLower().Trim().Contains("частуш"))
            {
                return new ChastushkiMessageHandler(_vkApi);
            }
            if (message.Text.ToLower().Trim().Contains("создай опрос"))
            {
                return new PollMessageHandler(_vkApi, _configuration);
            }
            else
            {
                return new DefaultMessageHandler(_vkApi);
            }
        }
    }
}
