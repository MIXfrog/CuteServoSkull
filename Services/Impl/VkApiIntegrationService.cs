using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace TestWebApp.Services.Impl
{
    public class VkApiIntegrationService : IVkApiIntegrationService
    {
        private readonly IVkApi _vkApi;

        public VkApiIntegrationService(IVkApi vkApi)
        {
            _vkApi = vkApi;
        }

        public string GetUserFirstAndLastName(long userId)
        {
            var user = _vkApi.Users.Get(new List<long> { userId }).FirstOrDefault();

            return $"{user.FirstName} {user.LastName}";
        }

        public void SendMessage(string messageText, Message message)
        {
            _vkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = message.PeerId.Value,
                Message = messageText
            });
        }
    }
}
