using ComBase;
using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Description : 수표조회
/// Author : 김민철
/// Create Date : 2018.02.09
/// </summary>
/// <history>
/// </history>
/// <seealso cref=d:\psmh\"FRMCHK01.FRM "/> 
namespace ComPmpaLibB
{
    public partial class frmCheckView : Form
    {
        Card CARD = new Card();

        StreamReader Reader;
        StreamWriter Writer;
        TcpClient Client;
        NetworkStream stream;
        Thread ReceiveThread;

        bool isConnected = false;
        bool Connected;

        private delegate void DisplayMsgDelegate(string strText);

        public frmCheckView()
        {
            InitializeComponent();
            SetControl();
            Connect_Port();
            SetEvents();
        }

        void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);

            this.txtBill.KeyPress   += new KeyPressEventHandler(eKeyPress);
            this.cboAmtGbn.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtAmt.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtDate.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtCredit.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.cboAmtGbn.SelectedIndexChanged += new EventHandler(eCboChange);
        }

        void eCboChange(object sender, EventArgs e)
        {
            if (sender == this.cboAmtGbn)
            {
                if (cboAmtGbn.SelectedIndex == 4)
                {
                    txtAmt.Text = "";
                    txtAmt.Enabled = true;
                }
                else
                {
                    //cboAmtGbn.Items.Add("13. 10 만원권");
                    //cboAmtGbn.Items.Add("14. 30 만원권");
                    //cboAmtGbn.Items.Add("15. 50 만원권");
                    //cboAmtGbn.Items.Add("16. 100만원권");
                    if (cboAmtGbn.SelectedIndex == 0)
                    {
                        txtAmt.Text = "100000";
                    }
                    else if (cboAmtGbn.SelectedIndex == 1)
                    {
                        txtAmt.Text = "300000";
                    }
                    else if (cboAmtGbn.SelectedIndex == 2)
                    {
                        txtAmt.Text = "500000";
                    }
                    else if (cboAmtGbn.SelectedIndex == 3)
                    {
                        txtAmt.Text = "1000000";
                    }

                    txtAmt.Enabled = false;
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (sender == this.txtBill)
                {
                    this.cboAmtGbn.Focus();
                }
                else if (sender == this.cboAmtGbn)
                {
                    if (txtAmt.Enabled == true)
                    {
                        this.txtAmt.Focus();
                    }
                    else
                    {
                        this.txtDate.Focus();
                    }
                }
                else if (sender == this.txtDate)
                {
                    this.btnSearch.Focus();
                    //this.txtCredit.Focus();
                }
                else if (sender == this.txtCredit)
                {
                    this.btnSearch.Focus();
                }
            }
            
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                         
                this.Close();
             //   return;
            }
            else if (sender == btnSearch)
            {
                eSearch();
            }
            else if (sender == btnCancel)
            {
                txtBill.Text = "";
                txtAmt.Text = "";
                txtDate.Text = "";
                txtCredit.Text = "";
            }
        }

        void eSearch()
        {
         

            if (isConnected == true)
            {
                try
                {
                    
                    StringBuilder req = new StringBuilder();
                    
                    byte[] req_array = new byte[3073];
                    byte[] rep_array = new byte[3073];

                    req.Clear();
                    Array.Clear(req_array, 0, req_array.Length);
                    Array.Clear(rep_array, 0, rep_array.Length);
                    
                    // 전문구분
                    req = CARD.insertLeftJustify(req, "0200", 4);
                    // 업무구분
                    req = CARD.insertLeftJustify(req, "15", 2);

                    if (clsCompuInfo.gstrCOMIP == "192.168.2.68")
                    {
                        req = CARD.insertLeftJustify(req, Card.GstrTerminal_SID_Test, 8);    // 단말기 번호
                    }
                    else
                    {
                        req = CARD.insertLeftJustify(req, Card.GstrTerminal_SID, 8);    // 단말기 번호
                    }
                                        
                    // 전표번호
                    req = CARD.insertLeftJustify(req, "0000", 4);
                    // 암호화정보
                    req = CARD.insertLeftJustify(req, "0             ", 14);
                    // 수표정보
                    req = CARD.insertLeftJustify(req, "K" + txtBill.Text, 15);
                    // 수표권종코드
                    req = CARD.insertLeftJustify(req, cboAmtGbn.Text, 2);
                    // 수표금액
                    req = CARD.insertLeftJustify(req, txtAmt.Text, 12);
                    // 수표발행일자
                    req = CARD.insertLeftJustify(req, txtDate.Text, 8);
                    // 수표계좌일련번호
                    req = CARD.insertLeftJustify(req, txtCredit.Text, 12);
                    
                    Writer.Write(req.ToString());
                    Writer.Flush();
                }
                catch (Exception ex)
                {
                    txtMsg.Text = "서버와 연결이 실패되었습니다.";
                }

            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            txtBill.Text = "";
            //txtAmt.Text = "";
            txtAmt.Enabled = false;
            txtDate.Text = "";
            txtCredit.Text = "";
        }

        void SetControl()
        {
            cboAmtGbn.Items.Clear();
            cboAmtGbn.Items.Add("13. 10 만원권");
            cboAmtGbn.Items.Add("14. 30 만원권");
            cboAmtGbn.Items.Add("15. 50 만원권");
            cboAmtGbn.Items.Add("16. 100만원권");
            cboAmtGbn.Items.Add("19. 일반권");
            cboAmtGbn.SelectedIndex = 0;

            txtAmt.Text = "100000";
        }

        void Connect_Port()
        {
            try
            {
                string IP = Card.GstrServIP;
                int port = Card.GnServPort;

                Client = new TcpClient();

                Client.Connect(IP, port);
                stream = Client.GetStream();
               
                Connected = true;
                isConnected = true;

                lblServiece.Text = "On-Line";
                lblServiece.BackColor = Color.LightGreen;

                Reader = new StreamReader(stream, Encoding.Default);
                Writer = new StreamWriter(stream);

                ReceiveThread = new Thread(new ThreadStart(Receive));
                ReceiveThread.Start();
            }
            catch (Exception ConnE)
            {
                txtMsg.Text = ConnE.Message;
                lblServiece.Text = "Off-Line";
                lblServiece.BackColor = Color.Silver;
            }
        }

        void Receive()
        {
            string strMsg = string.Empty;

            DisplayMsgDelegate DisplayMsg = new DisplayMsgDelegate(Display_Message);

            try
            {
                while (Connected)
                {

                    Thread.Sleep(1);

                    if (stream.CanRead)
                    {
                        char[] c = null;
                        c = new char[5000];

                        char[] d = null;
                        d = new char[5000];

                        int adv;
                        adv = Reader.Read(c, 0, c.Length);

                        string s = new string(c);
                        string tempStr = s.Replace("\0", "");

                        string tempStr1 = string.Empty;

                        if (stream.DataAvailable)
                        {
                            adv = Reader.Read(d, 0, d.Length);

                            string s1 = new string(d);
                            tempStr1 = s1.Replace("\0", "");
                        }

                        if (tempStr.Length > 0)
                        {

                            Invoke(DisplayMsg, tempStr + tempStr1);

                            #region Response String
                            if ((tempStr.Substring(0, 4) == "1000") || (tempStr.Substring(0, 4) == "2000"))
                            {
                                strMsg = CARD.Message(tempStr);
                                Invoke(DisplayMsg, strMsg);
                            }
                            else if (tempStr.Substring(0, 1) == "K")
                            {
                                strMsg = CARD.Message(tempStr);
                                Invoke(DisplayMsg, strMsg);
                            }
                            else if (tempStr.Substring(0, 1) == "S")
                            {
                                strMsg = CARD.Message(tempStr);
                                Invoke(DisplayMsg, strMsg);
                            }
                            else if (tempStr.Substring(0, 4) == "0000")
                            {
                                if (tempStr.Substring(4, 1) == "C")
                                {
                                    strMsg = CARD.Message(tempStr);
                                    Invoke(DisplayMsg, strMsg);
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }

        void Display_Message(string strMsg)
        {
            txtMsg.Text = VB.Mid(strMsg, 115, 59);
            //txtMsg.Text = strMsg;
        }
        
    }
}
