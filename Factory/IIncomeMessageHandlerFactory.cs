using TestWebApp.Handlers;
using VkNet.Model;

namespace TestWebApp.Factory
{
    /// <summary>
    /// Фабрика создания обработчиков единичных входящих сообщений
    /// </summary>
    public interface IIncomeMessageHandlerFactory
    {
        IHandler CreateHandler(Message message);
    }
}
