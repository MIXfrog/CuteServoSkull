﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Model.RequestParams.Polls;

namespace TestWebApp.Handlers.Impl
{
    public class PollMessageHandler : IHandler
    {
        private readonly IVkApi _vkApi;
        private readonly Message _message;

        private readonly int OurGroupId = 142512108;
        private readonly List<string> Games = new List<string>
        {
            "WarHammer 40k",
            "Kill Team",
            "Underworls-Shadespire",
            "Age of Sigmar",
            "Я новичёк, хочу посмотреть",
            "Не иду"
        };
        private readonly string PollText = "Мы будем рады всем желающим. Клуб выдает разовые армии и проводит индивидуальное обучение новичков.";

        public PollMessageHandler(IVkApi vkApi, Message message)
        {
            _vkApi = vkApi;
            _message = message;
        }

        public void Handle()
        {
            // Создаем опрос Похоже что все это время он не мог его создать. Почему? Дотянуть try до сюда           
            try
            {
                var poll = _vkApi.PollsCategory.Create(new PollsCreateParams
                {
                    AddAnswers = Games,
                    Question = "test",
                    IsMultiple = false,
                    OwnerId = -OurGroupId
                });

                _vkApi.Messages.Send(new MessagesSendParams
                {
                    Attachments = new List<MediaAttachment> { poll },
                    RandomId = new DateTime().Millisecond,
                    PeerId = 2000000001
                });
            }
            catch (Exception ex)
            {
                _vkApi.Messages.Send(new MessagesSendParams
                {
                    RandomId = new DateTime().Millisecond,
                    PeerId = _message.PeerId.Value,
                    Message = ex.Message
                });
            }
        }
    }
}
