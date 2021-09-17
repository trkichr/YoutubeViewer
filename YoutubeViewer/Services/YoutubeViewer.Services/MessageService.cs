using YoutubeViewer.Services.Interfaces;

namespace YoutubeViewer.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
