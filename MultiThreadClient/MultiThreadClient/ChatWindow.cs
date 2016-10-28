using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.ComponentModel;

namespace MultiThreadClient {
    public partial class ChatWindow : Form {
        private TcpClient ClientSocket = new TcpClient() {
            NoDelay = true
        };
        private NetworkStream ServerStream;
        private Packet ReceivePacket;
        private Packet SendPacket = new Packet() {
            ChatName = "Client",
            ChatDataIdentifier = DataIdentifier.Message
        };
        private string UserName;

        public ChatWindow() {
            InitializeComponent();
        }

        private void ChatWindow_Load(object sender, EventArgs e) {
            using (var ipForm = new ConnectForm()) {
                var result = ipForm.ShowDialog();
                if (result == DialogResult.OK) {
                    msg("Client Started");
                    ClientSocket.Connect(ipForm.ReturnIP, 8888);
                    SendPacket.ChatName = ipForm.ReturnUserName;
                    lblStatus.Text = "Server Connected ... " + ipForm.ReturnIP.ToString();
                    bwBroadcastStream.RunWorkerAsync();
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e) {
            try {
                ServerStream = ClientSocket.GetStream();
                SendPacket.ChatMessage = tbInput.Text;
                tbInput.Clear();
                ServerStream.Write(SendPacket.GetDataStream(), 0, SendPacket.Length);
                ServerStream.Flush();
            } catch (Exception ex) {
                msg(ex.Message.ToString());
                msg("Disconnected from server!");
            }
        }

        public void msg(string mesg) {
            tbOutputBox.AppendText(" >> " + mesg);
            tbOutputBox.AppendText(Environment.NewLine);
            tbOutputBox.ScrollToCaret();
        }

        private void bwBroadcastStream_DoWork(object sender, DoWorkEventArgs e) {
            while (true) {
                try {
                    byte[] inStream = new byte[10025];
                    NetworkStream inputStream = ClientSocket.GetStream();
                    inputStream.Read(inStream, 0, ClientSocket.ReceiveBufferSize);
                    ReceivePacket = new Packet(inStream);
                    bwBroadcastStream.ReportProgress(0, ReceivePacket.ChatName + ": " + ReceivePacket.ChatMessage);
                } catch (Exception ex) {
                    bwBroadcastStream.ReportProgress(0, ex.Message.ToString());
                    break;
                }
            }
        }

        
        private void bwBroadcastStream_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            msg((string)e.UserState);
        }
    }
}