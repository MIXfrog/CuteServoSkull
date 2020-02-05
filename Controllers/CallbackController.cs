using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestWebApp.Factory;
using TestWebApp.Model;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace TestWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly IVkApi _vkApi;

        private readonly IIncomeMessageHandlerFactory _handlerFactory;

        public CallbackController(IConfiguration configuration, IVkApi vkApi, IIncomeMessageHandlerFactory handlerFactory)
        {
            _configuration = configuration;
            _vkApi = vkApi;
            _handlerFactory = handlerFactory;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            // Проверяем, что находится в поле "type" 
            switch (updates.Type)
            {
                case "confirmation":
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                    {
                        var message = Message.FromJson(new VkResponse(updates.Object));

                        _handlerFactory.CreateHandler(message).Handle();
                        break;
                    }
            }
            // Возвращаем "ok" серверу Callback API
            return Ok("ok");
        }

    }
}
