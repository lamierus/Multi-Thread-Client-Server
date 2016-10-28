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
    }
}
