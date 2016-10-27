using System;
using System.Collections.Generic;
using System.Text;

namespace MultiThreadClient {
    // ----------------
    // Packet Structure
    // ----------------

    // Description   -> |dataIdentifier|name length|message length|    name   |    message   |
    // Size in bytes -> |       4      |     4     |       4      |name length|message length|

    public enum DataIdentifier {
        Message,
        LogIn,
        LogOut,
        Null
    }

    public class Packet {
        public DataIdentifier ChatDataIdentifier { get; set; }
        public string ChatName { get; set; }
        public string ChatMessage { get; set; }

        public int Length {
            get { return GetDataStream().Length; }
        }

        // Default Constructor
        public Packet() {
            ChatDataIdentifier = DataIdentifier.Null;
            ChatMessage = null;
            ChatName = null;
        }

        public Packet(byte[] dataStream) {
            // Read the data identifier from the beginning of the stream (4 bytes)
            ChatDataIdentifier = (DataIdentifier)BitConverter.ToInt32(dataStream, 0);

            // Read the length of the name (4 bytes)
            int nameLength = BitConverter.ToInt32(dataStream, 4);

            // Read the length of the message (4 bytes)
            int msgLength = BitConverter.ToInt32(dataStream, 8);

            // Read the name field
            if (nameLength > 0)
                ChatName = Encoding.UTF8.GetString(dataStream, 12, nameLength);
            else
                ChatName = null;

            // Read the message field
            if (msgLength > 0)
                ChatMessage = Encoding.UTF8.GetString(dataStream, 12 + nameLength, msgLength);
            else
                ChatMessage = null;
        }

        // Converts the packet into a byte array for sending/receiving 
        public byte[] GetDataStream() {
            List<byte> dataStream = new List<byte>();

            // Add the dataIdentifier
            dataStream.AddRange(BitConverter.GetBytes((int)ChatDataIdentifier));

            // Add the name length
            if (ChatName != null)
                dataStream.AddRange(BitConverter.GetBytes(ChatName.Length));
            else
                dataStream.AddRange(BitConverter.GetBytes(0));

            // Add the message length
            if (ChatMessage != null)
                dataStream.AddRange(BitConverter.GetBytes(ChatMessage.Length));
            else
                dataStream.AddRange(BitConverter.GetBytes(0));

            // Add the name
            if (ChatName != null)
                dataStream.AddRange(Encoding.UTF8.GetBytes(ChatName));

            // Add the message
            if (ChatMessage != null)
                dataStream.AddRange(Encoding.UTF8.GetBytes(ChatMessage));

            return dataStream.ToArray();
        }
    }
}