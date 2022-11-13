using Akka.IO;
using System.Text;

namespace DungeonOfTheWickedEventSourcing.Common.Tools
{
    public static class TcpTools
    {
        public static Tcp.Write CreateWsClientMessage(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var encodedMessageBytes = WebSocketMessageTools.Encode(messageBytes);
            var write = Tcp.Write.Create(ByteString.FromBytes(encodedMessageBytes));
            return write;
        }
    }
}
