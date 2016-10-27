using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace MultiThreadClient {
    public partial class Form1 : Form {
        private TcpClient ClientSocket = new TcpClient() {
            NoDelay = true
        };
        private NetworkStream ServerStream;
        private Packet ReceivePacket;
        private Packet SendPacket = new Packet() {
            ChatName = "Client",
            ChatDataIdentifier = DataIdentifier.Message
        };

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            msg("Client Started");
            ClientSocket.Connect(IPAddress.Loopback.ToString(), 8888);
            label1.Text = "Client Socket Program - Server Connected ...";
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                ServerStream = ClientSocket.GetStream();
                msg(SendPacket.ChatMessage = "Message from Client!");
                ServerStream.Write(SendPacket.GetDataStream(), 0, SendPacket.Length);
                ServerStream.Flush();

                byte[] inStream = new byte[10025];
                ServerStream.Read(inStream, 0, ClientSocket.ReceiveBufferSize);
                ReceivePacket = new Packet(inStream);
                msg("Data from Server: " + ReceivePacket.ChatMessage);
            } catch (Exception ex) {
                msg(ex.Message.ToString());
                msg("Disconnected from server!");
            }
        }

        public void msg(string mesg) {
            textBox1.AppendText(" >> " + mesg);
            textBox1.AppendText(Environment.NewLine);
            textBox1.ScrollToCaret();
        }
    }
}