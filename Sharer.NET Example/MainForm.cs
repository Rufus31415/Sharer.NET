using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharer;
using System.Diagnostics;

namespace Sharer.NETTest
{
    public partial class MainForm : Form
    {
        #region Members
        private SharerConnection _connection;

        #endregion
        #region General
        public MainForm()
        {
            InitializeComponent();
            refreshGUI();

        }

        private void _connection_InternalError(object o, ErrorEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(()=>_connection_InternalError(o, e)));
                return;
            }
            MessageBox.Show("Internal error", e.Exception.ToString());
        }

        private void _connection_Ready(object sender, EventArgs e)
        {
            refreshGUI();
        }

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

        private TextBox _getFunctionTextbox()
        {
            var ctrl = new TextBox();
            ctrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            ctrl.AutoSize = true;
            ctrl.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            return ctrl;
        }

        private Boolean Connected
        {
            get
            {
                return _connection != null && _connection.Connected;
            }
        }

        private void refreshGUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(()=>refreshGUI()));
                return;
            }

            try
            {
                grpConnect.Enabled = !Connected;
                grpSession.Enabled = Connected;

                if (Connected)
                {
                    txtNbFunctions.Text = _connection.Functions.Count.ToString();


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

                                        lblResult.Text = ret.ToString() + " (" + t.ToString(@"ss\:fff") + ")";
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


        private void handleException(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Oups...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            refreshGUI();
        }
        #endregion

        #region Connection
        private void cbPort_DropDown(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                cbPort.Items.Clear();
                cbPort.Items.AddRange(SharerConnection.GetSerialPortNames());
                refreshGUI();
            }
            catch(Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

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
                _connection.Ready += _connection_Ready;
                _connection.InternalError += _connection_InternalError;

                _connection.Connect();

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

        #region Session
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


                    var allSuccess= _connection.WriteVariables(values);

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



        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (_connection != null && _connection.Connected)
                {
                    var names = new List<string>();
                    var txts = new List<TextBox>();

                    foreach(var pnl in this.pnlVariables.Controls.OfType<FlowLayoutPanel>())
                    {
                       var chk= pnl.Controls.OfType<CheckBox>().First();
                        if (chk.Checked)
                        {
                            names.Add(chk.Name);
                            txts.Add(pnl.Controls.OfType<TextBox>().First());
                        }
                    }

                   var values= _connection.ReadVariables(names.ToArray());

                    for(int i = 0; i < values.Count; i++)
                    {
                        txts[i].Text = values[i].ToString();
                        txts[i].ForeColor = values[i].Status == Command.SharerReadVariableStatus.OK ? Color.Black : Color.Red;
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
    }
}
