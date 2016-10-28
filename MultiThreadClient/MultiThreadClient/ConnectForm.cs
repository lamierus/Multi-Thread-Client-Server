using System;
using System.Net;
using System.Windows.Forms;

namespace MultiThreadClient {
    public partial class ConnectForm : Form {
        public IPAddress ReturnIP { get; set; }
        public string ReturnUserName { get; set; }

        public ConnectForm() {
            InitializeComponent();
        }

        /// <summary>
        ///     this is it. a button with logic to send the entered IP and username then return that information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e) {
            IPAddress ipAddress;
            if (IPAddress.TryParse(tbIPAddress.Text, out ipAddress)) {
                ReturnIP = ipAddress;
                if (ReturnUserName != "" || ReturnUserName != null) {
                    ReturnUserName = tbUserName.Text;
                } else {
                    ReturnUserName = "Client";
                }
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        ///     exit out of everything, so it doesn't open the other form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (DialogResult != DialogResult.OK) {
                Environment.Exit(0);
            }
        }
    }
}
