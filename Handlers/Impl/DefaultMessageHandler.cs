using System;
using System.Collections.Generic;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace TestWebApp.Handlers.Impl
{
    public class DefaultMessageHandler : IHandler
    {
        private readonly IVkApi _vkApi;
        private Random _random;

        private readonly List<string> DefaultMessage = new List<string>
        {
            "Какова Ваша воля?",
            "Да, Мастер?",
            "Ожидаю Ваших инструкций",
            "Да, о Великолепный?",
            "Я здесь, чтобы служить",
            "Исполню Вашу волю",
            "Моя жизнь - служение"
        };

        public DefaultMessageHandler(IVkApi vkApi)
        {
            _vkApi = vkApi;
            _random = new Random();
        }

        public void Handle(Message message)
        {
            if (message.Text.Contains("покажи ID"))
            {
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = new DateTime().Millisecond,
                    PeerId = message.PeerId.Value,
                    Message = message.OwnerId.Value +"_"+ message.PeerId.Value
                });
            }
            else
            {
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = new DateTime().Millisecond,
                    PeerId = message.PeerId.Value,
                    Message = DefaultMessage[_random.Next(0, DefaultMessage.Count - 1)]
                });
            }
        }
    }
}
