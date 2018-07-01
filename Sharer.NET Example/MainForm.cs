﻿using System;
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
                                box.KeyDown += (object sender, KeyEventArgs e) => { if (e.KeyCode == Keys.Enter) btn.PerformClick(); };

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
                                        var ret = _connection.Call(func.Name,TimeSpan.FromSeconds(10), args.Select((x) => x.Text).ToArray());

                                        var t = sw.Elapsed;

                                        if (ret.Status == Command.SharerCallFunctionStatus.OK)
                                        {
                                            lblResult.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            lblResult.ForeColor = Color.Red;
                                        }

                                        lblResult.Text = ret.ToString() + " (" + t.ToString(@"ss\:fff") + ")" ;
                                    }
                                    catch(Exception ex)
                                    {
                                        lblResult.ForeColor = Color.Red;
                                        lblResult.Text = ex.Message;
                                        if(ex.InnerException != null)
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

                    }
                    finally
                    {
                        this.pnlFunctions.ResumeLayout(true);

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

    }
}
