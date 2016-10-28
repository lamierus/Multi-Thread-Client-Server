using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace MultiThreadServer {
    class Program {
        public static int Counter = 0;
        public static List<Client> ClientList = new List<Client>();

        static void Main(string[] args) {
            //listen to the current ip addresses on the system
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
            serverSocket.Start();

            Console.WriteLine(" >> " + "Server Started");
            
            Thread AcceptClients = new Thread(ConnectClient);
            AcceptClients.Start(serverSocket);

            string input = string.Empty;

            //this is merely so the server admin can shut down the server at any point in time.
            while (input != "/quit") {
                input = Console.ReadLine();
            }
            
            AcceptClients.Interrupt();
            serverSocket.Stop();
            Console.WriteLine(" >> exit");
            Console.ReadLine();
        }

        /// <summary>
        ///     awaits the connection of a client via the specified socket, and 
        ///     creates a new Client object thread
        /// </summary>
        /// <param name="parameter"></param>
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
                    Console.WriteLine(" >> Something Happened!! " + e.Message.ToString());
                    clientSocket.Close();
                    break;
                }
            }
        }

        /// <summary>
        ///     sends out the broadcasted chat or system message to each client that is connected.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="flag"></param>
        public static void Broadcast(Packet packet, bool flag = true) {
            foreach (Client client in ClientList) {
                client.ClientSocket.GetStream().Write(packet.GetDataStream(), 0, packet.Length);
                client.ClientSocket.GetStream().Flush();
            }
        }
    }
}
