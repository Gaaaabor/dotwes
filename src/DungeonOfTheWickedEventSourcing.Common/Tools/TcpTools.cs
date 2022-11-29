using Akka.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DungeonOfTheWickedEventSourcing.Common.Tools
{
    public static class TcpTools
    {
        /// <summary>
        /// Serializes the given message using the first non-interface type of the class and creates a Tcp.Write.
        /// </summary>
        /// <typeparam name="TClass">The typy of the message.</typeparam>
        /// <param name="message">The message to send.</param>        
        /// <returns>A Tcp.Write object which contains the serialized message.</returns>
        public static async Task<Tcp.Write> CreateTcpWriteMessageAsync<TClass>(TClass message)
        {
            var firstNonInterfaceType = GetFirstNonInterfaceType(message);
            var serializedMessage = JsonSerializer.Serialize(message, firstNonInterfaceType, JsonSerializerOptions.Default);

            using var memoryStream = new MemoryStream();
            using var webSocket = WebSocket.CreateFromStream(memoryStream, new WebSocketCreationOptions
            {
                IsServer = true
            });

            var messageBytes = Encoding.UTF8.GetBytes(serializedMessage);
            await webSocket.SendAsync(messageBytes, WebSocketMessageType.Text, true, CancellationToken.None);

            var write = Tcp.Write.Create(ByteString.FromBytes(memoryStream.ToArray()));
            return write;
        }

        /// <summary>
        /// Reads Tcp message from a byte[] representation into a string.
        /// </summary>
        /// <param name="receivedBytes">The message in form of a byte[]</param>
        /// <returns>A string message</returns>
        public static async Task<string> ReadMessageBytesAsync(byte[] receivedBytes)
        {
            using var memoryStream = new MemoryStream();
            memoryStream.Write(receivedBytes.ToArray());
            memoryStream.Position = 0;

            using var webSocket = WebSocket.CreateFromStream(memoryStream, new WebSocketCreationOptions
            {
                IsServer = true
            });

            var buffer = WebSocket.CreateServerBuffer(receivedBytes.Length);
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer[..result.Count]);
            return message;
        }

        private static Type GetFirstNonInterfaceType<TEntity>(TEntity entity)
        {
            var entityType = entity.GetType();
            if (entityType?.IsInterface ?? false)
            {
                var result = GetFirstNonInterfaceType(entityType.UnderlyingSystemType);
                return result ?? entityType;
            }

            return entityType;
        }
    }
}
