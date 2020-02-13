using VkNet.Model;

namespace TestWebApp.Services
{
    public interface IVkApiIntegrationService
    {
        /// <summary>
        /// Display message text for Vk user
        /// </summary>
        /// <param name="messageText">response message text</param>
        /// <param name="message">user message info</param>
        void SendMessage(string messageText, Message message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetUserFirstAndLastName(long userId);
    }
}
