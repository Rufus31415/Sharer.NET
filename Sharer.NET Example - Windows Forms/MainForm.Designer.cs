namespace Sharer.Demo
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.udBaud = new System.Windows.Forms.NumericUpDown();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grpConnect = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtReceivedUserData = new System.Windows.Forms.RichTextBox();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.chkRead = new System.Windows.Forms.CheckBox();
            this.pnlVariables = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnRecord = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnGetInfo = new System.Windows.Forms.Button();
            this.pnlFunctions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGetFunctionList = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.tmrRead = new System.Windows.Forms.Timer(this.components);
            this.tmrUnqueue = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.grpSession = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udBaud)).BeginInit();
            this.grpConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpSession.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbPort
            // 
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(119, 141);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(83, 21);
            this.cbPort.TabIndex = 0;
            this.cbPort.Text = "COM10";
            this.cbPort.DropDown += new System.EventHandler(this.cbPort_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud rate :";
            // 
            // udBaud
            // 
            this.udBaud.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.udBaud.Location = new System.Drawing.Point(287, 141);
            this.udBaud.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.udBaud.Name = "udBaud";
            this.udBaud.Size = new System.Drawing.Size(83, 20);
            this.udBaud.TabIndex = 2;
            this.udBaud.Value = new decimal(new int[] {
            115200,
            0,
            0,
            0});
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(403, 139);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // grpConnect
            // 
            this.grpConnect.BackColor = System.Drawing.Color.Gainsboro;
            this.grpConnect.Controls.Add(this.richTextBox1);
            this.grpConnect.Controls.Add(this.pictureBox1);
            this.grpConnect.Controls.Add(this.cbPort);
            this.grpConnect.Controls.Add(this.btnConnect);
            this.grpConnect.Controls.Add(this.label1);
            this.grpConnect.Controls.Add(this.udBaud);
            this.grpConnect.Controls.Add(this.label2);
            this.grpConnect.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpConnect.Location = new System.Drawing.Point(0, 0);
            this.grpConnect.Name = "grpConnect";
            this.grpConnect.Size = new System.Drawing.Size(1192, 176);
            this.grpConnect.TabIndex = 4;
            this.grpConnect.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBox1.Location = new System.Drawing.Point(901, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(288, 157);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "Sharer .NET is a library that facilitates the communication between an Arduino Bo" +
    "ard and a desktop application.\n\nPlease visit : https://github.com/Rufus31415/Sha" +
    "rer";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sharer.Demo.Properties.Resources.Sharer;
            this.pictureBox1.Location = new System.Drawing.Point(180, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(208, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // txtReceivedUserData
            // 
            this.txtReceivedUserData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceivedUserData.Location = new System.Drawing.Point(3, 217);
            this.txtReceivedUserData.Name = "txtReceivedUserData";
            this.txtReceivedUserData.Size = new System.Drawing.Size(290, 243);
            this.txtReceivedUserData.TabIndex = 9;
            this.txtReceivedUserData.Text = "";
            // 
            // txtSend
            // 
            this.txtSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSend.Location = new System.Drawing.Point(3, 190);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(195, 20);
            this.txtSend.TabIndex = 7;
            this.txtSend.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSend_KeyUp);
            // 
            // chkRead
            // 
            this.chkRead.AutoSize = true;
            this.chkRead.Location = new System.Drawing.Point(6, 47);
            this.chkRead.Name = "chkRead";
            this.chkRead.Size = new System.Drawing.Size(103, 17);
            this.chkRead.TabIndex = 6;
            this.chkRead.Text = "Continuous read";
            this.chkRead.UseVisualStyleBackColor = true;
            this.chkRead.CheckedChanged += new System.EventHandler(this.chkRead_CheckedChanged);
            // 
            // pnlVariables
            // 
            this.pnlVariables.AutoScroll = true;
            this.pnlVariables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVariables.Controls.Add(this.flowLayoutPanel4);
            this.pnlVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVariables.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlVariables.Location = new System.Drawing.Point(0, 127);
            this.pnlVariables.Name = "pnlVariables";
            this.pnlVariables.Size = new System.Drawing.Size(429, 342);
            this.pnlVariables.TabIndex = 5;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel4.TabIndex = 5;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(300, 39);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(149, 23);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy record to clipboard";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(300, 8);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(149, 23);
            this.btnRecord.TabIndex = 3;
            this.btnRecord.Text = "Record";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(129, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Write";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnWrite);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(6, 18);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(117, 23);
            this.btnRead.TabIndex = 3;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(204, 188);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(89, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send user data";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGetInfo.Location = new System.Drawing.Point(85, 70);
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(117, 23);
            this.btnGetInfo.TabIndex = 3;
            this.btnGetInfo.Text = "Get Infos";
            this.btnGetInfo.UseVisualStyleBackColor = true;
            this.btnGetInfo.Click += new System.EventHandler(this.btnGetInfo_Click);
            // 
            // pnlFunctions
            // 
            this.pnlFunctions.AutoScroll = true;
            this.pnlFunctions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFunctions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFunctions.Location = new System.Drawing.Point(0, 52);
            this.pnlFunctions.Name = "pnlFunctions";
            this.pnlFunctions.Size = new System.Drawing.Size(447, 417);
            this.pnlFunctions.TabIndex = 5;
            // 
            // btnGetFunctionList
            // 
            this.btnGetFunctionList.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGetFunctionList.Location = new System.Drawing.Point(85, 41);
            this.btnGetFunctionList.Name = "btnGetFunctionList";
            this.btnGetFunctionList.Size = new System.Drawing.Size(117, 23);
            this.btnGetFunctionList.TabIndex = 3;
            this.btnGetFunctionList.Text = "Refresh lists";
            this.btnGetFunctionList.UseVisualStyleBackColor = true;
            this.btnGetFunctionList.Click += new System.EventHandler(this.btnGetFunctionList_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDisconnect.Location = new System.Drawing.Point(84, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(118, 23);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // tmrRead
            // 
            this.tmrRead.Tick += new System.EventHandler(this.btnRead_Click);
            // 
            // tmrUnqueue
            // 
            this.tmrUnqueue.Enabled = true;
            this.tmrUnqueue.Tick += new System.EventHandler(this.tmrUnqueue_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.txtReceivedUserData);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetFunctionList);
            this.splitContainer1.Panel1.Controls.Add(this.btnDisconnect);
            this.splitContainer1.Panel1.Controls.Add(this.txtSend);
            this.splitContainer1.Panel1.Controls.Add(this.btnSend);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1186, 471);
            this.splitContainer1.SplitterDistance = 298;
            this.splitContainer1.TabIndex = 10;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pnlFunctions);
            this.splitContainer2.Panel1.Controls.Add(this.label11);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pnlVariables);
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Panel2.Controls.Add(this.label12);
            this.splitContainer2.Size = new System.Drawing.Size(884, 471);
            this.splitContainer2.SplitterDistance = 449;
            this.splitContainer2.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(447, 52);
            this.label11.TabIndex = 6;
            this.label11.Text = "Shared Functions";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkRead);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnRead);
            this.panel1.Controls.Add(this.btnRecord);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 75);
            this.panel1.TabIndex = 8;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(429, 52);
            this.label12.TabIndex = 7;
            this.label12.Text = "Shared Variables";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpSession
            // 
            this.grpSession.Controls.Add(this.splitContainer1);
            this.grpSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSession.Location = new System.Drawing.Point(0, 176);
            this.grpSession.Name = "grpSession";
            this.grpSession.Size = new System.Drawing.Size(1192, 490);
            this.grpSession.TabIndex = 4;
            this.grpSession.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(290, 52);
            this.label3.TabIndex = 10;
            this.label3.Text = "User messages";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1192, 666);
            this.Controls.Add(this.grpSession);
            this.Controls.Add(this.grpConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Sharer";
            ((System.ComponentModel.ISupportInitialize)(this.udBaud)).EndInit();
            this.grpConnect.ResumeLayout(false);
            this.grpConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlVariables.ResumeLayout(false);
            this.pnlVariables.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpSession.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown udBaud;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox grpConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnGetFunctionList;
        private System.Windows.Forms.FlowLayoutPanel pnlFunctions;
        private System.Windows.Forms.FlowLayoutPanel pnlVariables;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.ToolTip tt;
        private System.Windows.Forms.CheckBox chkRead;
        private System.Windows.Forms.Timer tmrRead;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnGetInfo;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Timer tmrUnqueue;
        private System.Windows.Forms.RichTextBox txtReceivedUserData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox grpSession;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
    }
}

