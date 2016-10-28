namespace MultiThreadClient {
    partial class ChatWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblStatus = new System.Windows.Forms.Label();
            this.tbOutputBox = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.bwBroadcastStream = new System.ComponentModel.BackgroundWorker();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(39, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 4;
            // 
            // tbOutputBox
            // 
            this.tbOutputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputBox.Location = new System.Drawing.Point(15, 25);
            this.tbOutputBox.Multiline = true;
            this.tbOutputBox.Name = "tbOutputBox";
            this.tbOutputBox.ReadOnly = true;
            this.tbOutputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutputBox.Size = new System.Drawing.Size(311, 192);
            this.tbOutputBox.TabIndex = 3;
            this.tbOutputBox.TabStop = false;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSend.Location = new System.Drawing.Point(42, 270);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(260, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // bwBroadcastStream
            // 
            this.bwBroadcastStream.WorkerReportsProgress = true;
            this.bwBroadcastStream.WorkerSupportsCancellation = true;
            this.bwBroadcastStream.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwBroadcastStream_DoWork);
            this.bwBroadcastStream.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwBroadcastStream_ProgressChanged);
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(15, 244);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(311, 20);
            this.tbInput.TabIndex = 0;
            this.tbInput.WordWrap = false;
            // 
            // ChatWindow
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(338, 305);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbOutputBox);
            this.Controls.Add(this.lblStatus);
            this.Name = "ChatWindow";
            this.Text = "Multi Thread Chat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatWindow_FormClosed);
            //this.Load += new System.EventHandler(this.ChatWindow_Load);
            this.Activated += new System.EventHandler(this.ChatWindow_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox tbOutputBox;
        private System.Windows.Forms.Button btnSend;
        private System.ComponentModel.BackgroundWorker bwBroadcastStream;
        private System.Windows.Forms.TextBox tbInput;
    }
}

