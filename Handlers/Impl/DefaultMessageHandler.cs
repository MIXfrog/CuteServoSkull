using System;
using System.Collections.Generic;
using TestWebApp.Services;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace TestWebApp.Handlers.Impl
{
    public class DefaultMessageHandler : IHandler
    {
        private readonly IVkApiIntegrationService _vkApiIntegration;
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

        public DefaultMessageHandler(IVkApiIntegrationService vkApiIntegration, Message message)
        {
            _vkApiIntegration = vkApiIntegration;
            _message = message;

            _random = new Random();
        }

        public void Handle()
        {
            _vkApiIntegration.SendMessage(
                DefaultMessages[_random.Next(0, DefaultMessages.Count - 1)], 
                _message);
        }
    }
}
