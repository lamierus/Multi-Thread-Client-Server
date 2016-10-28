using System;
using System.Net.Sockets;
using System.Threading;

namespace MultiThreadServer {
    //Class to handle each client request seperately
    public class Client {
        public TcpClient ClientSocket;
        public int ClientNo { get; set; }

        public Packet ReceivePack;
        public Packet SendPack = new Packet(){
            ChatName = "Server",
            ChatDataIdentifier = DataIdentifier.Message
        };

        /// <summary>
        ///     this creates and starts the tread for the client object, on the server.
        /// </summary>
        /// <param name="inClientSocket"></param>
        /// <param name="clineNo"></param>
        public void startClient(TcpClient inClientSocket, int clineNo) {
            ClientSocket = inClientSocket;
            ClientSocket.NoDelay = true;
            ClientNo = clineNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }


        /// <summary>
        ///     this is the function that takes care of receiving the packets from the different users,
        ///     interpreting them and sending out the correct broadcast messages to the other users.
        /// </summary>
        private void doChat() {
            byte[] inStream = new byte[10025];
            NetworkStream networkStream;

            while (true) {
                try {
                    networkStream = ClientSocket.GetStream();
                    networkStream.Read(inStream, 0, ClientSocket.ReceiveBufferSize);
                    ReceivePack = new Packet(inStream);

                    //different logic, depending on the type of DataIdentifier received in the data Packet.
                    if (ReceivePack.ChatDataIdentifier == DataIdentifier.Message) {
                        Console.WriteLine(" >> " + "From client - " + ReceivePack.ChatName + ": " + ReceivePack.ChatMessage);
                        Program.Broadcast(ReceivePack);
                    } 
                    else if (ReceivePack.ChatDataIdentifier == DataIdentifier.LogIn) {
                        ReceivePack.ChatMessage = "Joined the chat!";
                        Program.Broadcast(ReceivePack);
                    } 
                    else if (ReceivePack.ChatDataIdentifier == DataIdentifier.LogOut) {
                        ReceivePack.ChatMessage = "Left the chat...";
                        RemoveClient();
                        Program.Broadcast(ReceivePack);
                        break;
                    }
                } catch (Exception ex) {
                    Console.WriteLine(" >> " + ex.Message.ToString());
                    RemoveClient();
                    break;
                }
            }
        }

        /// <summary>
        ///     code to remove the client from the server's list and update the console with that information.
        /// </summary>
        private void RemoveClient() {
            Console.WriteLine(" >> Client " + ClientNo + " Disconnected");
            Program.ClientList.Remove(this);
            Program.Counter--;
        }
    }
}
