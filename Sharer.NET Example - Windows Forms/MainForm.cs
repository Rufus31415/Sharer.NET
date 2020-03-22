/*
                ███████╗██╗  ██╗ █████╗ ██████╗ ███████╗██████╗ 
                ██╔════╝██║  ██║██╔══██╗██╔══██╗██╔════╝██╔══██╗
                ███████╗███████║███████║██████╔╝█████╗  ██████╔╝
                ╚════██║██╔══██║██╔══██║██╔══██╗██╔══╝  ██╔══██╗
                ███████║██║  ██║██║  ██║██║  ██║███████╗██║  ██║
                ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝    

    Sharer is a .NET and Arduino Library to facilitate communication between and Arduino board and a desktop application.
    With Sharer it is easy to remote call a function executed by Arduino, read and write a variable inside Arduino board memory.
    Sharer uses the Serial communication to implement the Sharer protocol and remote execute commands.

    This example allows you to send user messages, read/write variables and remote call functions on your Arduino board.

    License: MIT
    Author: Rufus31415
    Website: https://rufus31415.github.io
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sharer.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Get last COM port used and fill combo box
            if (Properties.Settings.Default.ComPort != null && Properties.Settings.Default.ComPort.StartsWith("COM"))
            {
                cbPort.Text = Properties.Settings.Default.ComPort;
            }

            refreshGUI();
        }

        #region Function list
        /// <summary>
        /// Returns a colored label
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Label _getFunctionLabel(string text, Color color)
        {
            var ctrl = new Label();
            ctrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            ctrl.AutoSize = true;
            ctrl.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            ctrl.Text = text;
            ctrl.ForeColor = color;
            return ctrl;
        }

        /// <summary>
        /// Returns a colored checkbox
        /// </summary>
        private CheckBox _getFunctionCheckbox(string text, Color color)
        {
            var ctrl = new CheckBox();
            ctrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            ctrl.AutoSize = true;
            ctrl.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            ctrl.Text = text;
            ctrl.ForeColor = color;
            return ctrl;
        }

        /// <summary>
        /// Returns a textbox that contains function argument value
        /// </summary>
        private TextBox _getFunctionTextbox()
        {
            var ctrl = new TextBox();
            ctrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            ctrl.AutoSize = true;
            ctrl.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            return ctrl;
        }

        /// <summary>
        /// Refresh GUI according to connection state
        /// </summary>
        private void refreshGUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => refreshGUI()));
                return;
            }

            try
            {
                grpConnect.Enabled = !Connected;
                grpSession.Enabled = Connected;

                if (Connected)
                {
                    try
                    {
                        this.pnlFunctions.SuspendLayout();
                        this.pnlFunctions.Controls.Clear();
                        this.pnlVariables.SuspendLayout();
                        this.pnlVariables.Controls.Clear();

                        foreach (var func in _connection.Functions)
                        {
                            var pnl = new System.Windows.Forms.FlowLayoutPanel();


                            pnl.Controls.Add(_getFunctionLabel(func.ReturnType.ToString(), Color.Purple));
                            pnl.Controls.Add(_getFunctionLabel(func.Name + "(", Color.Black));

                            // call button
                            var btn = new Button();
                            btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                            btn.Margin = new System.Windows.Forms.Padding(0);
                            btn.Text = "Call";
                            btn.AutoSize = true;

                            var args = new List<Control>();
                            for (int i = 0; i < func.Arguments.Count; i++)
                            {
                                var arg = func.Arguments[i];
                                pnl.Controls.Add(_getFunctionLabel(arg.Type.ToString(), Color.Purple));
                                pnl.Controls.Add(_getFunctionLabel(arg.Name + " = ", Color.Black));
                                var box = _getFunctionTextbox();

                                // Send command with enter button
                                box.KeyUp += (object sender, KeyEventArgs e) => { if (e.KeyCode == Keys.Enter) btn.PerformClick(); };

                                pnl.Controls.Add(box);
                                args.Add(box);
                                if (i != func.Arguments.Count - 1)
                                {
                                    pnl.Controls.Add(_getFunctionLabel(" , ", Color.Black));
                                }
                            }

                            pnl.Controls.Add(_getFunctionLabel(")", Color.Black));


                            pnl.Controls.Add(btn);

                            pnl.Controls.Add(_getFunctionLabel(" = ", Color.Black));

                            var lblResult = _getFunctionLabel("?", Color.SlateGray);
                            pnl.Controls.Add(lblResult);

                            btn.Click += (object sender, EventArgs e) =>
                            {
                                if (Connected)
                                {
                                    Cursor = Cursors.WaitCursor;
                                    try
                                    {
                                        var sw = Stopwatch.StartNew();
                                        var ret = _connection.Call(func.Name, TimeSpan.FromSeconds(10), args.Select((x) => x.Text).ToArray());

                                        var t = sw.Elapsed;

                                        if (ret.Status == Command.SharerCallFunctionStatus.OK)
                                        {
                                            lblResult.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            lblResult.ForeColor = Color.Red;
                                        }

                                        lblResult.Text = $"{ret.ToString()} ({(int)t.TotalMilliseconds} ms)";
                                    }
                                    catch (Exception ex)
                                    {
                                        lblResult.ForeColor = Color.Red;
                                        lblResult.Text = ex.Message;
                                        if (ex.InnerException != null)
                                        {
                                            lblResult.Text += " (" + ex.InnerException.Message + ")";
                                        }
                                    }
                                    finally
                                    {
                                        Cursor = Cursors.Default;
                                    }
                                }
                            };

                            pnl.AutoSize = true;
                            this.pnlFunctions.Controls.Add(pnl);
                        }

                        foreach (var var in _connection.Variables)
                        {
                            var pnl = new System.Windows.Forms.FlowLayoutPanel();

                            var chk = _getFunctionCheckbox(var.Type.ToString(), Color.Purple);
                            chk.Name = var.Name;
                            chk.Checked = true;
                            var lbl = _getFunctionLabel(var.Name + " =", Color.Black);
                            pnl.Controls.Add(chk);
                            pnl.Controls.Add(lbl);

                            lbl.Click += (object sender, EventArgs e) => chk.Checked = !chk.Checked;

                            var txt = _getFunctionTextbox();

                            pnl.Controls.Add(txt);

                            txt.KeyUp += (object sender, KeyEventArgs e) =>
                            {
                                chk.Checked = !string.IsNullOrEmpty(txt.Text);
                                txt.ForeColor = Color.Black;
                                tt.SetToolTip(txt, "Ready");
                            };

                            pnl.AutoSize = true;
                            this.pnlVariables.Controls.Add(pnl);
                        }

                    }
                    finally
                    {
                        this.pnlFunctions.ResumeLayout(true);
                        this.pnlVariables.ResumeLayout(true);

                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Refresh and display function list
        /// </summary>
        private void btnGetFunctionList_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_connection != null && _connection.Connected)
                {
                    _connection.RefreshFunctions();
                    _connection.RefreshVariables();
                }
                refreshGUI();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Connection

        /// <summary>
        /// Pointer to the Sharer connection
        /// </summary>
        private SharerConnection _connection;

        /// <summary>
        /// Is the communication opened
        /// </summary>
        private bool Connected => _connection != null && _connection.Connected;

        /// <summary>
        /// Refresh available COM ports on drop down
        /// </summary>
        private void cbPort_DropDown(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                cbPort.Items.Clear();
                cbPort.Items.AddRange(SharerConnection.GetSerialPortNames());
                refreshGUI();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Try to connect
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_connection != null)
                {
                    _connection.Disconnect();
                }

                _connection = new SharerConnection(cbPort.Text, (int)udBaud.Value);

                _connection.UserDataReceived += _connection_UserDataReceived;

                _connection.Connect();

                Properties.Settings.Default.ComPort = cbPort.Text;
                Properties.Settings.Default.Save();

                refreshGUI();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Popup a message box on exception
        /// </summary>
        private void handleException(Exception ex)
        {
            MessageBox.Show($"{ex.Message}\r\n\r\n{ex.StackTrace}", "Someting went wrong...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            refreshGUI();
        }

        /// <summary>
        /// Disconnect interface
        /// </summary>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_connection != null)
                {
                    _connection.Disconnect();
                }
                refreshGUI();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Popup board information
        /// </summary>
        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            var nfo = _connection.GetInfos();

            MessageBox.Show(nfo.ToString(), "Board infos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Variables
        /// <summary>
        /// Write variable
        /// </summary>
        private void btnWrite(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_connection != null && _connection.Connected)
                {
                    var names = new List<string>();
                    var txts = new List<TextBox>();
                    var values = new List<Command.SharerWriteValue>();

                    foreach (var pnl in this.pnlVariables.Controls.OfType<FlowLayoutPanel>())
                    {
                        var chk = pnl.Controls.OfType<CheckBox>().First();
                        if (chk.Checked)
                        {
                            names.Add(chk.Name);
                            var txt = pnl.Controls.OfType<TextBox>().First();
                            txts.Add(txt);
                            values.Add(new Command.SharerWriteValue(chk.Name, txt.Text));
                        }
                    }


                    var allSuccess = _connection.WriteVariables(values);

                    for (int i = 0; i < values.Count; i++)
                    {
                        txts[i].ForeColor = values[i].Status == Command.SharerWriteVariableStatus.OK ? Color.Black : Color.Red;
                        tt.SetToolTip(txts[i], values[i].Status.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Read a variable once
        /// </summary>
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_connection != null && _connection.Connected)
                {
                    var names = new List<string>();
                    var txts = new List<TextBox>();

                    foreach (var pnl in this.pnlVariables.Controls.OfType<FlowLayoutPanel>())
                    {
                        var chk = pnl.Controls.OfType<CheckBox>().First();
                        if (chk.Checked)
                        {
                            names.Add(chk.Name);
                            txts.Add(pnl.Controls.OfType<TextBox>().First());
                        }
                    }

                    var values = _connection.ReadVariables(names.ToArray());

                    for (int i = 0; i < values.Count; i++)
                    {
                        txts[i].Text = values[i].ToString();
                        txts[i].ForeColor = values[i].Status == Command.SharerReadVariableStatus.OK ? Color.Black : Color.Red;
                        tt.SetToolTip(txts[i], values[i].Status.ToString());

                        if (startRecord)
                        {
                            _record.Append(txts[i].Text);
                            _record.Append(";");
                        }
                    }
                }

                if (startRecord) _record.AppendLine();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Start/Stop continuous read when check box state changes
        /// </summary>
        private void chkRead_CheckedChanged(object sender, EventArgs e)
        {
            tmrRead.Enabled = chkRead.Checked;
        }

        /// <summary>
        /// Buffer of recorded values
        /// </summary>
        private StringBuilder _record = new StringBuilder();

        /// <summary>
        /// Record has started
        /// </summary>
        bool startRecord = false;

        /// <summary>
        /// Start a CSV record
        /// </summary>
        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (!startRecord)
            {
                _record.Remove(0, _record.Length); // Clear
                foreach (var pnl in this.pnlVariables.Controls.OfType<FlowLayoutPanel>())
                {
                    var chk = pnl.Controls.OfType<CheckBox>().First();
                    if (chk.Checked)
                    {
                        _record.Append(chk.Name);
                        _record.Append(";");
                    }
                }
                _record.AppendLine();
                startRecord = true;
                btnRecord.Text = "Stop";
            }
            else
            {
                startRecord = false;
                btnRecord.Text = "Record";
            }
        }

       /// <summary>
       /// Copy recorded data to clipboard
       /// </summary>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_record.ToString());
        }

        #endregion

        #region User Messages
        /// <summary>
        /// Enqueue received user message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _connection_UserDataReceived(object sender, UserData.UserDataReceivedEventArgs e)
        {
            try
            {
                var str = e.GetReader().ReadString();
                lock (_receivedData) _receivedData.Enqueue(str);
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        /// <summary>
        /// Send a user message
        /// </summary>
        private void btnSend_Click(object sender, EventArgs e)
        {
            _connection.WriteUserData(txtSend.Text);
        }

        /// <summary>
        /// Enqueued user message. We use a queue to separate serial thread from GUI
        /// </summary>
        private readonly Queue<string> _receivedData = new Queue<string>();

        /// <summary>
        /// Dequeue and display user messages if any
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUnqueue_Tick(object sender, EventArgs e)
        {
            lock (_receivedData)
            {
                while (_receivedData.Any())
                {
                    txtReceivedUserData.AppendText(_receivedData.Dequeue());
                    txtReceivedUserData.ScrollToCaret();
                }
            }
        }

        /// <summary>
        /// Press enter inside txtSend to send message
        /// </summary>
        private void txtSend_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnSend.PerformClick();
        }

        #endregion
    }
}
