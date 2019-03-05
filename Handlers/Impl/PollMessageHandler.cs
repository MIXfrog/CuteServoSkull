using Microsoft.Extensions.Configuration;
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
        private readonly int OurGroupId = 142512108;
        private readonly IVkApi _vkApi;
        public IConfiguration _configuration { get; }
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

        public PollMessageHandler(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
        }

        public void Handle(Message message)
        {
            // Создаем опрос
            var poll = _vkApi.PollsCategory.Create(new PollsCreateParams
            {
                AddAnswers = Games,
                Question = "test",
                IsMultiple = false,
                OwnerId = -OurGroupId
            });

            // Публикуем на стену в группе
            /*_vkApi.Wall.Post(new WallPostParams
            {
                Attachments = new List<MediaAttachment> { poll },
                OwnerId = -OurGroupId,
                FromGroup = true
            });*/
            _vkApi.Authorize(new ApiAuthParams { AccessToken = _configuration["Config:AccessToken"] });
            _vkApi.Wall.Post(new WallPostParams
            {
                Message = "test",
                OwnerId = -OurGroupId,
                FromGroup = true,
            });

            // Делаем репост в канал


            /*_vkApi.Wall.Repost

            _vkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = message.PeerId.Value,
                Attachments
            });*/
        }
    }
}
