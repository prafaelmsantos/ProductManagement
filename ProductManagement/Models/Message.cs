using Newtonsoft.Json;
using ProductManagement.Models.Enums;

namespace ProductManagement.Models
{
    public class Message
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public Message(string text, MessageType type = MessageType.Info)
        {
            Type = type;
            Text = text;
        }

        public static string Serializar(string text, MessageType type = MessageType.Info)
        {
            var message = new Message(text, type);
            return JsonConvert.SerializeObject(message);
        }

        public static Message Desserializar(string message)
        {
            return JsonConvert.DeserializeObject<Message>(message);
        }
    }
}
