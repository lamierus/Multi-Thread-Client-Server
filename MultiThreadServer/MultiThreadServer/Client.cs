using System;
using System.Net.Sockets;
using System.Text;
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

        public void startClient(TcpClient inClientSocket, int clineNo) {
            ClientSocket = inClientSocket;
            ClientSocket.NoDelay = true;
            ClientNo = clineNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat() {
            int requestCount = 0;
            byte[] inStream = new byte[10025];
            NetworkStream networkStream;

            while (true) {
                try {
                    requestCount++;
                    networkStream = ClientSocket.GetStream();
                    networkStream.Read(inStream, 0, ClientSocket.ReceiveBufferSize);
                    ReceivePack = new Packet(inStream);
                    Console.WriteLine(" >> " + "From client - " + ReceivePack.ChatName + ": " + ReceivePack.ChatMessage);
                    Program.Broadcast(ReceivePack, true);
                } catch (Exception ex) {
                    Console.WriteLine(" >> " + ex.Message.ToString());
                    Console.WriteLine(" >> Client " + ClientNo + " Disconnected");
                    break;
                }
            }
        }
    }
}
