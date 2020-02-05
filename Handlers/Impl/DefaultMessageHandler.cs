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
        private readonly Message _message;
        private Random _random;

        private readonly List<string> DefaultMessages = new List<string>
        {
            "Какова Ваша воля?",
            "Да, Мастер?",
            "Ожидаю Ваших инструкций",
            "Да, о Великолепный?",
            "Я здесь, чтобы служить",
            "Исполню Вашу волю",
            "Моя жизнь - служение"
        };

        public DefaultMessageHandler(IVkApi vkApi, Message message)
        {
            _vkApi = vkApi;
            _message = message;

            _random = new Random();
        }

        public void Handle()
        {
            _vkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = _message.PeerId.Value,
                Message = DefaultMessages[_random.Next(0, DefaultMessages.Count - 1)]
            });
        }
    }
}
