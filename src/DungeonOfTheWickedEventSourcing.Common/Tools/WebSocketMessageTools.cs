using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DungeonOfTheWickedEventSourcing.Common.Tools
{
    public static class WebSocketMessageTools
    {
        private static readonly byte EXT = 0x03;
        private static readonly Regex _keyMatcher = new("Sec-WebSocket-Key: (.*)");
        public static byte[] CloseMessage = new byte[4] { 136, 2, 3, 232 };

        /// <summary>
        /// Extracts the Sec-WebSocket-Key from the message, if not found empty string will be returned.
        /// </summary>
        /// <param name="message">Message to search for</param>
        /// <returns>The Sec-WebSocket-Key or empty string</returns>
        public static string GetSecWebSocketKey(string message)
        {
            var match = _keyMatcher.Match(message);
            if (match.Success && match.Groups.Count >= 1)
            {
                return match.Groups[1].Value.Trim();
            }

            return string.Empty;
        }

        public static byte[] Decode(byte[] messageBytes)
        {
            var packet = new List<byte>();

            using var messageStream = new MemoryStream(messageBytes);

            messageStream.Position = 0;

            packet.Add((byte)messageStream.ReadByte());
            var fin = (packet[0] & 1 << 7) != 0;
            var rsv1 = (packet[0] & 1 << 6) != 0;
            var rsv2 = (packet[0] & 1 << 5) != 0;
            var rsv3 = (packet[0] & 1 << 4) != 0;

            packet.Add((byte)messageStream.ReadByte());
            var masked = (packet[1] & 1 << 7) != 0;
            var pseudoLength = packet[1] - (masked ? 128 : 0);

            ulong actualLength = 0;
            if (pseudoLength > 0 && pseudoLength < 125)
            {
                actualLength = (ulong)pseudoLength;
            }
            else if (pseudoLength == 126)
            {
                var length = new byte[2];
                messageStream.Read(length, 0, length.Length);
                packet.AddRange(length);
                Array.Reverse(length);
                actualLength = BitConverter.ToUInt16(length, 0);
            }
            else if (pseudoLength == 127)
            {
                var length = new byte[8];
                messageStream.Read(length, 0, length.Length);
                packet.AddRange(length);
                Array.Reverse(length);
                actualLength = BitConverter.ToUInt64(length, 0);
            }

            var mask = new byte[4];
            if (masked)
            {
                messageStream.Read(mask, 0, mask.Length);
                packet.AddRange(mask);
            }

            using var messageBuffer = new MemoryStream();

            if (actualLength > 0)
            {
                var data = new byte[actualLength];
                messageStream.Read(data, 0, data.Length);
                packet.AddRange(data);

                if (masked)
                {
                    data = ApplyMask(data, mask);
                }

                messageBuffer.Write(data, 0, data.Length);
            }

            if (!fin)
            {
                // TODO: Long message processing
            }

            var decodedMessage = messageBuffer.ToArray();
            messageBuffer.SetLength(0);
            return decodedMessage;
        }

        public static byte[] Encode(byte[] messageBytes, bool masking = false, bool isBinary = false)
        {
            var mask = new byte[4];
            if (masking)
            {
                new Random().NextBytes(mask);
            }

            if (masking && mask is null)
            {
                throw new ArgumentException(nameof(mask));
            }

            using (var packet = new MemoryStream())
            {
                byte firstbyte = 0b0_0_0_0_0000; // fin | rsv1 | rsv2 | rsv3 | [ OPCODE | OPCODE | OPCODE | OPCODE ]

                firstbyte |= 0b1_0_0_0_0000; // fin
                //firstbyte |= 0b0_1_0_0_0000; // rsv1
                //firstbyte |= 0b0_0_1_0_0000; // rsv2
                //firstbyte |= 0b0_0_0_1_0000; // rsv3

                var opcode = isBinary
                    ? 0x2
                    : 0x1;
                firstbyte += (byte)opcode; // Text
                packet.WriteByte(firstbyte);

                // Set bit: bytes[byteIndex] |= mask;

                byte secondbyte = 0b0_0000000; // mask | [SIZE | SIZE  | SIZE  | SIZE  | SIZE  | SIZE | SIZE]

                if (masking)
                {
                    secondbyte |= 0b1_0000000; // mask
                }

                if (messageBytes.LongLength <= 0b0_1111101) // 125
                {
                    secondbyte |= (byte)messageBytes.Length;
                    packet.WriteByte(secondbyte);
                }
                else if (messageBytes.LongLength <= ushort.MaxValue) // If length takes 2 bytes
                {
                    secondbyte |= 0b0_1111110; // 126
                    packet.WriteByte(secondbyte);

                    var len = BitConverter.GetBytes(messageBytes.LongLength);
                    Array.Reverse(len, 0, 2);
                    packet.Write(len, 0, 2);
                }
                else // if (payload.LongLength <= Int64.MaxValue) // If length takes 8 bytes
                {
                    secondbyte |= 0b0_1111111; // 127
                    packet.WriteByte(secondbyte);

                    var len = BitConverter.GetBytes(messageBytes.LongLength);
                    Array.Reverse(len, 0, 8);
                    packet.Write(len, 0, 8);
                }

                if (masking)
                {
                    packet.Write(mask, 0, 4);
                    messageBytes = ApplyMask(messageBytes, mask);
                }

                // Write all data to the packet
                packet.Write(messageBytes, 0, messageBytes.Length);

                var encodedMessage = packet.ToArray();
                return encodedMessage;
            }
        }

        /// <summary>
        ///     Additionally, the server can decide on extension/subprotocol requests here; see Miscellaneous for details.<br />
        ///     The Sec-WebSocket-Accept header is important in that the server must derive it from the Sec-WebSocket-Key that the
        ///     client sent to it.<br />
        ///     To get it, concatenate the client's Sec-WebSocket-Key and the string "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
        ///     together (it's a "magic string")<br />
        ///     then take the SHA-1 hash of the result and return the base64 encoding of that hash.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string CreateAck(string key)
        {
            const string magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker

            var keyWithMagic = string.Concat(key, magic);
            var stuff = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(keyWithMagic));
            var accept = Convert.ToBase64String(stuff);

            var ackBuilder = new StringBuilder("HTTP/1.1 101 Switching Protocols");
            ackBuilder.Append(eol);
            ackBuilder.Append("Upgrade: websocket");
            ackBuilder.Append(eol);
            ackBuilder.Append("Connection: Upgrade");
            ackBuilder.Append(eol);
            ackBuilder.Append($"Sec-WebSocket-Accept: {accept}");
            ackBuilder.Append(eol);
            ackBuilder.Append(eol);

            return ackBuilder.ToString();
        }

        public static bool IsCloseMessage(byte[] message)
        {
            if (message.Length == 2)
            {
                return message.SequenceEqual(new byte[] { EXT, 232 });
            }

            if (message.Length == 4)
            {
                return message.SequenceEqual(CloseMessage);
            }

            return false;
        }

        private static byte[] ApplyMask(IReadOnlyList<byte> message, IReadOnlyList<byte> mask)
        {
            var decoded = new byte[message.Count];
            for (var i = 0; i < message.Count; i++)
            {
                decoded[i] = (byte)(message[i] ^ mask[i % 4]);
            }

            return decoded;
        }
    }
}