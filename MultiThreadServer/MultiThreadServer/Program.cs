using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace MultiThreadServer {
    class Program {
        private static int Counter = 0;
        public static List<Client> ClientList = new List<Client>();

        static void Main(string[] args) {
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");
            
            Thread AcceptClients = new Thread(ConnectClient);
            AcceptClients.Start(serverSocket);

            string input = string.Empty;

            while (input != "/quit") {
                input = Console.ReadLine();
            }
            
            AcceptClients.Interrupt();
            serverSocket.Stop();
            Console.WriteLine(" >> exit");
            Console.ReadLine();
        }

        private static void ConnectClient(object parameter) {
            var serverSocket = parameter as TcpListener;
            var clientSocket = default(TcpClient);
            while (true) {
                try {
                    clientSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine(" >> " + "Client No:" + (Counter + 1).ToString() + " started!");
                    ClientList.Add(new Client());
                    ClientList[Counter++].startClient(clientSocket, Counter);
                } catch (SocketException e) {
                    clientSocket.Close();
                    break;
                }
            }
        }

        public static void Broadcast(Packet packet, bool flag) {
            foreach (Client client in ClientList) {
                //NetworkStream broadcastStream = client.ClientSocket.GetStream();

                client.ClientSocket.GetStream().Write(packet.GetDataStream(), 0, packet.Length);
                client.ClientSocket.GetStream().Flush();
            }
        }
    }
}
