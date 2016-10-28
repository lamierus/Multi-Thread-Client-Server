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
        private Packet SendPacket = new Packet();
        private bool WindowLoaded = false;

        public ChatWindow() {
            InitializeComponent();
            
        }
        
        /// <summary>
        ///     I prefer to use the _Activated, instead of the _Load, due to having the window drawn
        ///     instead of just having a floating dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatWindow_Activated(object sender, EventArgs e) {
            if (!WindowLoaded) {
                WindowLoaded = true;
                using (var ipForm = new ConnectForm()) {
                    var result = ipForm.ShowDialog();
                    if (result == DialogResult.OK) {
                        
                        //this sets the client's packet username as well as displays it to the user
                        msg("Client Started for user: " + (SendPacket.ChatName = ipForm.ReturnUserName));
                        
                        //this loop allows for multiple attempts to connect to the server before timing out
                        for (int i = 0; i < 5; i++) {
                            msg(">> Attempting to connect...");
                            try {
                                ClientSocket.Connect(ipForm.ReturnIP, 8888);
                            } catch (Exception ex) {
                                msg(ex.Message);
                            }

                            //once connected, the client sends a packet to the server to agknowledge logging in.
                            if (ClientSocket.Connected) {
                                NetworkStream loginStream = ClientSocket.GetStream();
                                SendPacket.ChatDataIdentifier = DataIdentifier.LogIn;
                                loginStream.Write(SendPacket.CreateDataStream(), 0, SendPacket.Length);
                                loginStream.Flush();
                                lblStatus.Text = "Server Connected ... " + ipForm.ReturnIP.ToString();
                                bwBroadcastStream.RunWorkerAsync();
                                break;
                            }
                            msg(">> Waiting 3 Seconds before trying again.");
                            DateTime start = DateTime.Now;
                            while (DateTime.Now.Subtract(start).Seconds < 3) { }
                        }
                    }
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e) {
            try {
                ServerStream = ClientSocket.GetStream();
                SendPacket.ChatDataIdentifier = DataIdentifier.Message;
                SendPacket.ChatMessage = tbInput.Text;
                tbInput.Clear();
                ServerStream.Write(SendPacket.CreateDataStream(), 0, SendPacket.Length);
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
            NetworkStream inputStream;
            while (true) {
                try {
                    byte[] inStream = new byte[10025];
                    inputStream = ClientSocket.GetStream();
                    inputStream.Read(inStream, 0, ClientSocket.ReceiveBufferSize);
                    Packet receivePacket = new Packet(inStream);
                    bwBroadcastStream.ReportProgress(0, receivePacket.ChatName + ": " + receivePacket.ChatMessage);
                } catch (Exception ex) {
                    bwBroadcastStream.ReportProgress(0, ex.Message.ToString());
                    break;
                }
            }
        }

        private void bwBroadcastStream_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            msg((string)e.UserState);
        }

        private void ChatWindow_FormClosed(object sender, FormClosedEventArgs e) {
            if (ClientSocket.Connected) {
                ServerStream = ClientSocket.GetStream();
                SendPacket.ChatDataIdentifier = DataIdentifier.LogOut;
                ServerStream.Write(SendPacket.CreateDataStream(), 0, SendPacket.Length);
                ServerStream.Flush();
            }
        }
    }
}