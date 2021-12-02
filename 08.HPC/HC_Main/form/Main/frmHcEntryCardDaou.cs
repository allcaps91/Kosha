using ComBase; //기본 클래스
using ComBase.Controls;
using ComDbB; //DB연결
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using HC_Main.Dto;
using HC_Main.Service;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
/// <summary>
/// Description : 건진센터 신용카드 / 현금영수 승인
/// Author : 김민철
/// Create Date : 2020.06.10
/// </summary>
/// <history>
/// </history>
/// <seealso cref="Frm신용카드승인_다우(센터).frm / Frm현금영수승인_다우(센터).frm "/> 

namespace HC_Main
{
    public partial class frmHcEntryCardDaou : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string IpCassName, string IpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_SHOWNOMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;


        System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

        clsHcType cHT = null;
        clsHcSpd cHSpd = null;
        clsSpread cSpd = null;
        Card CARD = null;
        ComFunc CF = null;
        clsPmpaFunc cPF = null;
        clsOumsad cO = null;
        clsApi cAPI = null;

        Bitmap          bmp = null;
        Graphics        gr = null;
        StreamReader    Reader;
        StreamWriter    Writer;
        TcpClient       Client;
        NetworkStream   stream;
        Thread          ReceiveThread;

        CardApprovCenterService cardApprovCenterService = null;

        string m_cardApproveGbn = string.Empty;
        string FstrGubun        = string.Empty;
        string FstrRowid        = string.Empty;
        string FstrRePrt        = string.Empty;
        string FstrJob          = string.Empty;
        string FstrHPhone       = string.Empty;
        string FstrBDate        = string.Empty;     //진료일자

        long FnHPano = 0;
        long FnHWrtno = 0;

        int FnTimer = 0;    //승인버튼 연타 방지

        bool isConnected        = false;
        bool Connected;
        
        private delegate void DisplayMsgDelegate(string strText);
        private delegate void ApprovDataSetDelegate(string strText);

        //public delegate void rEventCSign(string strPath);
        //public static event rEventCSign rCSign;

        public frmHcEntryCardDaou()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            
            Connect_Port();
            
            FstrJob = "CARD";       //기본값
        }

        public frmHcEntryCardDaou(string strJob, string strIO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            
            Connect_Port();
            
            FstrJob = strJob;

            if (strJob == "CASH")
            {
                tabCtrl.SelectTab(1);       //현금영수증
            }
            else if (strJob == "CARD")
            {
                tabCtrl.SelectTab(0);       //신용카드
            }
            else
            {
                tabCtrl.SelectTab(0);       //신용카드
            }

            if (strIO != "")
            {
                if (cboIO.FindString(strIO, -1) > 0)
                {
                    cboIO.SelectedIndex = cboIO.FindString(strIO, -1);
                }
                else
                {
                    cboIO.SelectedIndex = 0;
                }
            }
            else
            {
                cboIO.SelectedIndex = 0;
            }
        }

        public frmHcEntryCardDaou(string ArgPano, string ArgSname, string ArgDept, string IO, long nAmt, string strJob, string strBDate, long gnHPano, long gnHWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            
            Connect_Port();

            txtPano.Text = ArgPano;
            lblSName.Text = ArgSname;
            cboDept.Text = ArgDept;
            FnHWrtno = gnHWRTNO;
            FnHPano = gnHPano;

            if (IO != "")
            {
                if (cboIO.FindString(IO, -1) > 0)
                {
                    cboIO.SelectedIndex = cboIO.FindString(IO, -1);
                }
                else
                {
                    cboIO.SelectedIndex = 0;
                }
            }
            else
            {
                cboIO.SelectedIndex = 0;
            }
           
            FstrJob = strJob;       //작업구분
            FstrBDate = strBDate;   //진료일자
            
            //원단위절삭
            if (strJob == "CASH")
            {
                tabCtrl.SelectTab(1);       //현금영수증
                nAmt = nAmt / 10 * 10;
                txtCashAmt.Text = nAmt.ToString("###,###,##0");
            }
            else
            {
                tabCtrl.SelectTab(0);       //신용카드
                nAmt = nAmt / 10 * 10;
                txtCardAmt.Text = nAmt.ToString("###,###,##0");
            }

            SetActControl(FstrJob, nAmt);
        }
        
        void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.FormClosing += new FormClosingEventHandler(eFormClosing);
            this.dtpBDate.ValueChanged          += new EventHandler(CF.eDtpFormatSet);
            this.dtpBDate2.ValueChanged         += new EventHandler(CF.eDtpFormatSet);

            this.btnExit.Click                  += new EventHandler(eBtnClick);
            this.btnSave1.Click                 += new EventHandler(eBtnClick);
            this.btnSave2.Click                 += new EventHandler(eBtnClick);
            this.btnSearch.Click                += new EventHandler(eBtnClick);
            this.btnRSign2.Click                += new EventHandler(eBtnClick);
            this.btnPead2.Click                 += new EventHandler(eBtnClick);
            this.btnCancel1.Click               += new EventHandler(eBtnClick);
            this.btnCancel2.Click               += new EventHandler(eBtnClick);
            this.btnPrint.Click                 += new EventHandler(eBtnClick);
            this.btnPrint2.Click                += new EventHandler(eBtnClick);

            this.txtPano.KeyPress               += new KeyPressEventHandler(eKeyPress);
            this.txtHphone.KeyPress             += new KeyPressEventHandler(eKeyPress);
            this.cboDept.KeyPress               += new KeyPressEventHandler(eKeyPress);
            this.cboIO.KeyPress                 += new KeyPressEventHandler(eKeyPress);
            this.txtCardAmt.KeyPress            += new KeyPressEventHandler(eKeyPress);
            this.txtCashAmt.KeyPress            += new KeyPressEventHandler(eKeyPress);
            this.txtDivideTerm.KeyPress         += new KeyPressEventHandler(eKeyPress);

            this.cboDept.MouseWheel             += new MouseEventHandler(eCboWheel);
            this.cboIO.MouseWheel               += new MouseEventHandler(eCboWheel);

            this.txtPano.GotFocus               += new EventHandler(eSelStart);
            this.txtHphone.GotFocus             += new EventHandler(eSelStart);
            this.txtCardAmt.GotFocus            += new EventHandler(eSelStart);
            this.txtCashAmt.GotFocus            += new EventHandler(eSelStart);
            this.txtDivideTerm.GotFocus         += new EventHandler(eSelStart);

            this.timer2.Tick                    += new EventHandler(eCountTime);
            this.SS1.CellClick                  += new CellClickEventHandler(eSpdDblClick);
            this.SS1.CellDoubleClick            += new CellClickEventHandler(eSpdDblClick);
            this.tabCtrl.SelectedIndexChanged   += new EventHandler(eTabSelCtrl);
            this.rdoGubun1.CheckedChanged       += new EventHandler(eRdoChecked);
            this.rdoGubun2.CheckedChanged       += new EventHandler(eRdoChecked);
            this.rdoGubun3.CheckedChanged       += new EventHandler(eRdoChecked);
            this.rdoGubun4.CheckedChanged       += new EventHandler(eRdoChecked);       //현금승인 취소
            this.rdoKey1.CheckedChanged         += new EventHandler(eRdoChecked);       //현금승인 취소
            this.rdoGbn3.CheckedChanged         += new EventHandler(eRdoChecked);
            this.chkPrt.CheckedChanged          += new EventHandler(eChkPrt);           //신용카드 영수증
            this.chkPrt2.CheckedChanged         += new EventHandler(eChkPrt);           //신용카드 영수증

            
        }

        private void eFormClosing(object sender, FormClosingEventArgs e)
        {
            //이미 폼이 떠있는지 확인한다.
            foreach (Form frmFindform in Application.OpenForms)
            {
                if (frmFindform.GetType() == typeof(frmTablet_b))
                {
                    frmFindform.Close();
                    frmFindform.Dispose();
                    break;
                }
            }
        }

        void eCountTime(object sender, EventArgs e)
        {
            FnTimer += 1;

            if (FnTimer > 2)
            {
                FnTimer = 0;
                timer2.Stop();
                btnSave1.Enabled = true;
                btnSave2.Enabled = true;
            }
        }

        void eChkPrt(object sender, EventArgs e)
        {
            Card.bCardPrt = chkPrt.Checked;
            Card.bCardPrt2 = chkPrt2.Checked;
        }

        void SetControl()
        {
            cHT =  new clsHcType();
            cHSpd = new clsHcSpd();
            cSpd = new clsSpread();
            CARD = new Card();
            CF = new ComFunc();
            cPF = new clsPmpaFunc();
            cO = new clsOumsad();
            cAPI = new clsApi();

            cardApprovCenterService = new CardApprovCenterService();

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 2);
            
            cboIO.Items.Clear();
            cboIO.Items.Add("H.일반");
            cboIO.Items.Add("T.종검");

            cboCan.Items.Clear();
            cboCan.Items.Add(" ");
            cboCan.Items.Add("1.거래취소");
            cboCan.Items.Add("2.오류발급");
            cboCan.Items.Add("3.기타");
            cboCan.SelectedIndex = 0;
            
        }
       
        void SetActControl(string ArgJob, long ArgAmt)
        {
            if (ArgJob == "CARD")
            {
                btnSave1.Enabled = true;
                btnCancel1.Enabled = true;
                btnSave2.Enabled = false;
                btnCancel2.Enabled = false;
                txtCardAmt.Text = ArgAmt.ToString();
                txtCashAmt.Text = "";
            }
            else if (ArgJob == "CASH")
            {
                btnSave1.Enabled = false;
                btnCancel1.Enabled = false;
                btnSave2.Enabled = true;
                btnCancel2.Enabled = true;
                txtCashAmt.Text = ArgAmt.ToString();
                txtCardAmt.Text = "";
            }
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
                lblMsg.Text = ConnE.Message;
                lblServiece.Text = "Off-Line";
                lblServiece.BackColor = Color.Silver;
            }
        }

        void Receive()
        {
            string strMsg = string.Empty;

            DisplayMsgDelegate DisplayMsg    = new DisplayMsgDelegate(Display_Message);
            ApprovDataSetDelegate AppDataSet = new ApprovDataSetDelegate(Card_Approv_Data_Set);

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
                            if ((VB.Left(tempStr, 4) == "1000") || (VB.Left(tempStr, 4) == "2000"))
                            {
                                
                                //삼성페이 구분
                                if ((VB.Mid(tempStr, 5, 4) == "0210") || (VB.Mid(tempStr, 5, 4) == "0430"))
                                {
                                    clsPmpaType.RD.HPay = "1";
                                }
                                else
                                {
                                    clsPmpaType.RD.HPay = "0";
                                }

                                strMsg = VB.Mid(tempStr, 171, 67);
                                
                                Invoke(AppDataSet, tempStr);
                                Invoke(DisplayMsg, strMsg);

                            }
                            else if (VB.Left(tempStr, 1) == "K")
                            {
                                strMsg = CARD.Message(tempStr);
                                Invoke(DisplayMsg, strMsg);
                            }
                            else if (VB.Left(tempStr, 1) == "S")
                            {                                
                                strMsg = CARD.Message(tempStr);
                                Invoke(DisplayMsg, strMsg);
                            }
                            else if (VB.Left(tempStr, 4) == "0000")
                            {
                                if (VB.Mid(tempStr, 5, 1) == "C")
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

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            ComFunc.Form_Center(this);

            cHSpd.sSpd_enmCardApprov(SS1, cHT.sSpdCardApprov, cHT.nSpdCardApprov, 10, 0);
            cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.GUBUN1, clsSpread.enmSpdType.Hide);

            Screen_Clear1();

            if (txtPano.Text.Trim() != "") { SetHphone(clsDB.DbCon); }
            
            string strGb = tabCtrl.SelectedTab == tabPage1 ? "1" : "2";
            DisPlay_Card_Approv(clsDB.DbCon, SS1, txtPano.Text, VB.Left(cboDept.Text, 2), VB.Left(cboIO.Text, 1), strGb, chkMenual.Checked);

            if (clsPmpaPb.GstrCreditIF != "P")
            {
                rdoKey2.Enabled = false;
                Request_Sign();

                if (rdoGubun2.Checked)
                    cO.Customer_Display("환불님!", txtCardAmt.Text.Trim());
                else
                    cO.Customer_Display(lblSName.Text.Trim(), txtCardAmt.Text.Trim());
                
            }

            chkPrt.Checked = Card.bCardPrt;
            chkPrt2.Checked = Card.bCardPrt2;

            ComFunc.Delay(500);

            IntPtr hWnd = FindWindow(null, this.Text);

            if (!hWnd.Equals(IntPtr.Zero))
            {
                ShowWindowAsync(hWnd, SW_SHOWNOMAL);
                SetForegroundWindow(hWnd);
            }

            txtPano.Select();
        }

        void SetHphone(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                FstrHPhone = "";

                SQL = "";
                SQL += " SELECT CARDNO                                              \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "CARD_APPROV_CENTER            \r\n";
                SQL += "  WHERE PANO = '" + txtPano.Text + "'                       \r\n";
                SQL += "    AND ACTDATE = (                                         \r\n";
                SQL += "        SELECT MAX(ACTDATE)                                 \r\n";
                SQL += "          FROM " + ComNum.DB_PMPA + "CARD_APPROV_CENTER     \r\n";
                SQL += "         WHERE PANO ='" + txtPano.Text + "'                 \r\n";
                SQL += "           AND GUBUN = '2'            )                     \r\n";
                SQL += "    AND GUBUN = '2'                                         \r\n";
                SQL += "  ORDER BY ENTERDATE DESC                                   \r\n";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    FstrHPhone = clsAES.DeAES(Dt.Rows[0]["CARDNO"].ToString().Trim());
                    txtHphone.Text = clsAES.DeAES(Dt.Rows[0]["CARDNO"].ToString().Trim());
                }

                Dt.Dispose();
                Dt = null;

                if (FstrHPhone == "" || FstrHPhone == "0100001234")
                {
                    rdoGbn3.Checked = true;
                }

                #region 현금영수증 거부인지 확인
                SQL = "";
                SQL += " SELECT CASHYN                              \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT   \r\n";
                SQL += "  WHERE PANO = '" + txtPano.Text + "'       \r\n";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["CASHYN"].ToString().Trim() == "1")
                    {
                        chkCash.Checked = true;
                        rdoGbn3.Checked = true;     //자진발급
                        txtHphone.Text = "0100001234";
                    }
                    else
                    {
                        chkCash.Checked = false;
                    }
                }

                Dt.Dispose();
                Dt = null;
                #endregion

                //if (clsPmpaPb.GstrCreditIF != "T")
                //{
                //    btnRSign.Enabled = false;
                //    btnPead.Enabled = false;
                //}
                //else
                //{
                //    timer1.Start();
                //}
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }    
                
        }
        
        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)             //닫기
            {
                #region btnExit
                
                CARD.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);
                cAPI.Tablet_Shell_Check("tablet_d", "현금영수증");

                this.Close();
                #endregion
            }
            else if (sender == this.btnSearch)
            {
                #region btnSearch
                string strGb = tabCtrl.SelectedTab == tabPage1 ? "1" : "2";
                DisPlay_Card_Approv(clsDB.DbCon, SS1, txtPano.Text, VB.Left(cboDept.Text, 2), VB.Left(cboIO.Text, 1), strGb, chkMenual.Checked);
                #endregion
            }
            else if (sender == this.btnSave1 || sender == this.btnSave2)
            {
                Approval_Data(clsDB.DbCon);        //신용승인, 
                btnSave1.Enabled = false;
                btnSave2.Enabled = false;
                timer2.Start();
            }
            //else if (sender == this.btnRSign || sender == this.btnRSign2)
            else if (sender == this.btnRSign2)
            {
                Request_Sign();         //서명요청
            }
            //else if (sender == this.btnPead )
            //{
            //    Confirm_Sign_Card(Card.GstrpFile);         //서명확인
            //}
            else if (sender == this.btnPead2)
            {
                Confirm_Sign_Cash();         //번호확인
            }
            else if (sender == this.btnCancel1 || sender == this.btnCancel2)
            {
                Screen_Clear1();        //신용카드 화면 취소
            }
            else if (sender == this.btnPrint)
            {
                Card_Print();    //신용카드 재발행구분
            }
            else if (sender == this.btnPrint2)
            {
                Card_Print();    //현금영수증 재발행구분
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano)
            {
                #region txtPano
                if (e.KeyChar == (char)13)
                {
                    string strGb = tabCtrl.SelectedTab == tabPage1 ? "1" : "2";

                    if (CF.READ_BARCODE(txtPano.Text.Trim()) == true)
                    {
                        txtPano.Text = clsPublic.GstrBarPano;
                        if (cboDept.FindString(clsPublic.GstrBarDept, -1) > 0)
                        {
                            cboDept.SelectedIndex = cboDept.FindString(clsPublic.GstrBarDept, -1);
                        }
                        else
                        {
                            cboDept.SelectedIndex = 0;
                        }

                        txtPano.Text = VB.Format(VB.Val(txtPano.Text), "0#######");
                        lblSName.Text = "";

                        CARD.SELECT_JUMIN(clsDB.DbCon, txtPano.Text);

                        lblSName.Text = Card.CVar.gstrCdSName;
                        txtCardAmt.Focus();
                    }
                    else
                    {
                        txtPano.Text = VB.Format(VB.Val(txtPano.Text), "0#######");
                        lblSName.Text = "";

                        CARD.SELECT_JUMIN(clsDB.DbCon, txtPano.Text);

                        lblSName.Text = Card.CVar.gstrCdSName;
                        cboDept.Focus();
                    }

                    DisPlay_Card_Approv(clsDB.DbCon, SS1, txtPano.Text, VB.Left(cboDept.Text, 2), VB.Left(cboIO.Text, 1), strGb, chkMenual.Checked);
                }                
                #endregion
            }
            else if (sender == this.cboDept)
            {
                #region 진료과 대문자 입력
                string str = e.KeyChar.ToString().ToUpper();
                char[] ch = str.ToCharArray();
                e.KeyChar = ch[0];

                if (e.KeyChar == (char)13)
                {
                    if (cboIO.Enabled == true)
                    {
                        cboIO.Focus();
                    }
                    else
                    {
                        if (tabCtrl.SelectedTab == tabPage1)
                        {
                            txtCardAmt.Focus();         //신용승인
                        }
                        else
                        {
                            txtHphone.Focus();          //현금영수
                        }
                    }
                } 
                #endregion
            }
            else if (sender == this.cboIO)
            {
                #region cboIO
                //진료과 입력칸 대문자 고정
                string str = e.KeyChar.ToString().ToUpper();
                char[] ch = str.ToCharArray();
                e.KeyChar = ch[0];

                if (e.KeyChar == (char)13)
                {
                    if (tabCtrl.SelectedTab == tabPage1)
                    {
                        txtCardAmt.Focus();         //신용승인
                    }
                    else
                    {
                        txtHphone.Focus();          //현금영수
                    }
                }
                
                #endregion
            }
            else if (sender == this.txtCardAmt && e.KeyChar == (char)13)
            {
                #region txtCardAmt
                if (txtCardAmt.Text.Trim() == "")
                {
                    txtCardAmt.Text = "0";
                }

                if (clsPmpaPb.GstrCreditIF.Trim() != "P")      //테블릿 환경
                {
                    clsOumsad cO = new clsOumsad();

                    if (rdoGubun2.Checked)
                        cO.Customer_Display("환불님!", txtCardAmt.Text.Trim());
                    else
                        cO.Customer_Display(lblSName.Text.Trim(), txtCardAmt.Text.Trim());
                }
                
                txtDivideTerm.Focus();
                #endregion
            }
            else if (sender == this.txtDivideTerm && e.KeyChar == (char)13)
            {
                #region txtDivideTerm
                //if (clsPmpaPb.GstrCreditIF == "T")
                //{
                //    btnRSign.Focus();
                //}
                //else
                //{
                //    btnSave1.Focus();
                //}

                btnSave1.Focus();

                #endregion
            }
            else if (sender == this.txtHphone && e.KeyChar == (char)13)
            {
                txtCashAmt.Focus();
            }
            else if (sender == this.txtCashAmt && e.KeyChar == (char)13)
            {
                #region 금액입력란
                if (txtCashAmt.Text.Trim() == "")
                {
                    txtCashAmt.Text = "0";
                }

                if (clsPmpaPb.GstrCreditIF == "T")
                {
                    btnSave2.Focus();
                }
                else
                {
                    btnPead2.Focus();
                } 
                #endregion
            }

        }

        void eTabSelCtrl(object sender, EventArgs e)
        {
            clsSpread cSpd = new clsSpread();

            long nAMT = 0;
            string strGb = tabCtrl.SelectedTab == tabPage1 ? "1" : "2";
            
            if (tabCtrl.SelectedTab == tabPage1)
            {
                chkMenual.Visible = true;
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.GUBUN1,      clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.CARDNO,      clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.ORIGINNO2,   clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.FINAME,      clsSpread.enmSpdType.View);

                FstrJob = "CARD";

                nAMT = Convert.ToInt64(VB.Replace(txtCashAmt.Text, ",", ""));
            }
            else if (tabCtrl.SelectedTab == tabPage2)
            {
                chkMenual.Checked = false;
                chkMenual.Visible = false;
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.GUBUN1,      clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.CARDNO,      clsSpread.enmSpdType.View);
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.ORIGINNO2,   clsSpread.enmSpdType.View);
                cSpd.setColStyle(SS1, -1, (int)clsHcType.enmCardApprov.FINAME,      clsSpread.enmSpdType.Hide);

                SetHphone(clsDB.DbCon);

                FstrJob = "CASH";

                nAMT = Convert.ToInt64(VB.Replace(txtCardAmt.Text, ",", ""));
            }

            SetActControl(FstrJob, nAMT);

            DisPlay_Card_Approv(clsDB.DbCon, SS1, txtPano.Text, VB.Left(cboDept.Text, 2), VB.Left(cboIO.Text, 1), strGb, chkMenual.Checked);
        }

        void eCboWheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Focused == false)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        void eSelStart(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            
            Screen_Clear1();
           
            if (cboDept.FindString(SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.DEPTCODE].Text, -1) > 0)
            {
                cboDept.SelectedIndex = cboDept.FindString(SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.DEPTCODE].Text, -1);
            }
            else
            {
                cboDept.SelectedIndex = 0;
            }

            if (cboIO.FindString(SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.GBIO].Text, -1) > 0)
            {
                cboIO.SelectedIndex = cboIO.FindString(SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.GBIO].Text, -1);
            }
            else
            {
                cboIO.SelectedIndex = 0;
            }

            if (FstrJob == "CASH")
            {
                txtHphone.Text      = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.CARDNO].Text;
                txtOrNo2.Text       = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ORIGINNO].Text;
                txtOrDate2.Text     = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.TRANDATE].Text;
                txtCashAmt.Text     = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.TRADEAMT].Text;
            }
            else
            {
                txtOrNo.Text        = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ORIGINNO].Text;
                txtOrDate.Text      = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.TRANDATE].Text;
                txtCardAmt.Text     = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.TRADEAMT].Text;
                txtDivideTerm.Text  = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.DIV].Text;
            }
            
            FstrRowid = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ROWID].Text;
            if (FstrJob == "CASH")
            {
                Read_Card_Approv_Data(clsDB.DbCon, txtPano.Text, txtOrNo2.Text);
            }
            else
            {
                Read_Card_Approv_Data(clsDB.DbCon, txtPano.Text, txtOrNo.Text);
            }

            //Read_Card_Approv_Data(clsDB.DbCon, txtPano.Text, txtOrNo.Text);

            //이미 취소된건은 취소불가
            if (SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ORIGINNO2].Text == "")
            {
                if (FstrJob == "CARD")
                {
                    rdoGubun2.Checked = true;
                    btnSave1.Enabled = true;
                }
                else if (FstrJob == "CASH")
                {
                    rdoGubun4.Checked = true;
                    btnSave2.Enabled = true;
                    cboCan.Text = "1.거래취소";
                    txtOrDate2.Text = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.TRANDATE].Text;
                    txtOrNo2.Text = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ORIGINNO].Text;
                    FstrRowid = SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ROWID].Text;

                    if (Card.CVar.gstrGubun == "1")
                    {
                        rdoGbn1.Checked = true;
                    }
                    else if(Card.CVar.gstrGubun =="2")
                    {
                        rdoGbn2.Checked = true;
                    }
                    else if (Card.CVar.gstrGubun == "3")
                    {
                        rdoGbn3.Checked = true;
                    }
                }
            }
            else
            {
                if (SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ORIGINNO].Text != SS1.ActiveSheet.Cells[e.Row, (int)clsHcType.enmCardApprov.ORIGINNO2].Text)
                {
                    ComFunc.MsgBox("이미 취소된 결제이거나 취소건 입니다.", "취소불가");
                    if (FstrJob == "CARD")
                    {
                        btnSave1.Enabled = false;
                    }
                    else if (FstrJob == "CASH")
                    {
                        btnSave2.Enabled = false;
                    }
                }
                else
                {
                    if (FstrJob == "CARD")
                    {
                        rdoGubun2.Checked = true;
                        btnSave1.Enabled = true;
                    }
                    else if (FstrJob == "CASH")
                    {
                        rdoGubun4.Checked = true;
                        btnSave2.Enabled = true;
                        cboCan.Text = "1.거래취소";
                    }
                }
            }
        }

        void eRdoChecked(object sender, EventArgs e)
        {
            if (sender == rdoGubun1 || sender == rdoGubun3)
            {
                Screen_Clear1();
            }
            else if (sender == rdoGubun4)
            {
                cboCan.Text = "1.거래취소";
            }
            else if (sender == rdoKey1)
            {
                txtHphone.Text = "1544****";
            }
            
            else if (sender == rdoGbn3)     //자진발급
            {
                if (rdoGbn3.Checked)
                {
                    txtHphone.Text = "0100001234";
                }
            }
            else if (sender == rdoGbn1 || sender == rdoGbn2)    //영수발급, 지출증빙
            {
                txtHphone.Text = FstrHPhone;
            }
            
        }

        void eChkChecked(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strYN = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            strYN = chkCash.Checked == true ? "1" : "2";

            try
            {
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT SET CashYN ='" + strYN + "' ";
                SQL += "  Where Pano ='" + txtPano.Text + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void Screen_Clear1()
        {
            rdoGbn1.Checked = true;
            rdoKey3.Checked = true;
            txtDivideTerm.Text = "00";
            txtOrDate.Text = "";
            txtOrNo.Text = "";
            CF.dtpClear(dtpBDate);
            txtMemsData.Text = "";
            txtOrDate2.Text = "";
            txtOrNo2.Text = "";
            CF.dtpClear(dtpBDate2);
            txtMemsData2.Text = "";
            lblMsg.Text = "";
            
            cboCan.Text = "";
            btnSave1.Enabled = true;
            btnSave2.Enabled = true;
            FstrRowid = "";
            FstrGubun = "";
            FstrRePrt = "";

            if (FstrBDate != "" && FstrBDate != null)
            {
                dtpBDate.Text = FstrBDate;
                dtpBDate2.Text = FstrBDate;
            }
            else
            {
                dtpBDate.Text = clsPublic.GstrSysDate;
                dtpBDate2.Text = clsPublic.GstrSysDate;
            }
        }

        void DisPlay_Card_Approv(PsmhDb pDbCon, FpSpread Spd, string ArgPano, string ArgDept, string ArgIO, string ArgGb, bool chkMenual)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            string strFind = string.Empty;

            SS1.ActiveSheet.RowCount = 0;
            cSpd.Spread_All_Clear(SS1);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(TRANDATE,'YYYYMMDD') TRANDATE, TRANHEADER, CARDNO,        ";
                SQL += ComNum.VBLF + "        DECODE(TRANHEADER, '2', TRADEAMT * -1, '1', TRADEAMT) TRADEAMT,   ";
                SQL += ComNum.VBLF + "        DEPTCODE, GBIO, FINAME, ORIGINNO, ORIGINNO2,                      ";
                SQL += ComNum.VBLF + "        DECODE(GUBUN1,'0','Y','','Y','') GUBUN1, IC, ROWID,               ";
                SQL += ComNum.VBLF + "        INSTPERIOD                                                        "; 
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV_CENTER                          ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                                          ";
                if (ArgDept != "**" && ArgDept != "")
                {
                   SQL += ComNum.VBLF + "    AND (DEPTCODE = '" + ArgDept + "' OR ACTDATE=TRUNC(SYSDATE) )      ";
                   SQL += ComNum.VBLF + "    AND ACTDATE >= TRUNC(SYSDATE-365)                                  ";
             
                }
                if (ArgIO != "*" && ArgIO != "")
                {
                    SQL += ComNum.VBLF + "    AND GBIO = '" + ArgIO + "'                                        ";
                }
                SQL += ComNum.VBLF + "    AND TRANHEADER IN ('1', '2')                                          "; //승인 + 취소
                SQL += ComNum.VBLF + "    AND GUBUN  = '" + ArgGb + "'                                          ";  //카드, 현금영수증 구분
                if (chkMenual == false)
                {
                    SQL += ComNum.VBLF + "   AND INPUTMETHOD <> 'T'                                             ";      //수동 입력자료
                }
                SQL += ComNum.VBLF + " ORDER BY ACTDATE DESC ,ORIGINNO DESC ,TRANDATE DESC                      ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;

                        if (Spd.ActiveSheet.Rows.Count < nRow)
                        {
                            Spd.ActiveSheet.Rows.Count = nRow;
                        }

                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.TRANDATE].Text   = Dt.Rows[i]["TRANDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.TRADEAMT].Text   = VB.Val(Dt.Rows[i]["TRADEAMT"].ToString()).ToString("###,###,###");
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.DEPTCODE].Text   = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.GBIO].Text       = Dt.Rows[i]["GbIO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.FINAME].Text     = Dt.Rows[i]["FINAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.CARDNO].Text     = clsAES.DeAES(Dt.Rows[i]["CARDNO"].ToString().Trim());
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.ORIGINNO].Text   = Dt.Rows[i]["ORIGINNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.ORIGINNO2].Text  = Dt.Rows[i]["ORIGINNO2"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.GUBUN1].Text     = Dt.Rows[i]["GUBUN1"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.TRANHEADER].Text = Dt.Rows[i]["TRANHEADER"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.IC].Text         = Dt.Rows[i]["IC"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.ROWID].Text      = Dt.Rows[i]["ROWID"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.DIV].Text        = Dt.Rows[i]["INSTPERIOD"].ToString().Trim();

                        if (Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.ORIGINNO2].Text != "")
                        {
                            if (Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.ORIGINNO].Text != Spd.ActiveSheet.Cells[nRow - 1, (int)clsHcType.enmCardApprov.ORIGINNO2].Text)
                            {
                                cSpd.setSpdForeColor(Spd, nRow - 1, 0, nRow - 1, Spd.ActiveSheet.ColumnCount - 1, Color.Red);
                            }
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

        }
        
        void Display_Sub_Form(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length < 2)
            {
                timer1.Stop();
                return;
            }

            //서브 모니터 번호 찾기
            int nMo = Screen.AllScreens[0].Primary == true ? 1 : 0;     //주   모니터
            int nSo = Screen.AllScreens[1].Primary == true ? 1 : 0;     //서브 모니터

            int screenWidth = Screen.AllScreens[nSo].Bounds.Width;      //서브 모니터 화면 넓이
            int screenHeight = Screen.AllScreens[nSo].Bounds.Height;    //서브 모니터 화면 높이
            
            int screenLeft = Screen.AllScreens[nMo].Bounds.Width + Screen.AllScreens[nSo].Bounds.Left;   //주 모니터 넓이 + 서브 모니터 시작점
            int screenTop = Screen.AllScreens[nSo].Bounds.Top;

            try
            {
                bmp = new Bitmap(screenWidth, screenHeight);
                gr = Graphics.FromImage(bmp);
                gr.CopyFromScreen(screenLeft, screenTop, 0, 0, new Size(bmp.Width, bmp.Height));
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                clsApi.FlushMemory();
            }
            catch (Exception ex)
            {
                timer1.Stop();
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void Request_Sign()
        {
            Process Proc = new Process(); //외부프로그램 실행

            if (clsPmpaPb.GstrCreditIF.Trim() == "P")      //테블릿 환경이 아니면 빠져나감
            {
                return;
            }

            if (FstrJob == "CASH")
            {
                #region 현금영수증 화면
                if (!File.Exists(clsPublic.GstrTabletD_FilePath))
                {
                    ComFunc.MsgBox("CMC 폴더에 tablet_d.exe 파일이 없습니다.", "파일누락");
                    return;
                }

                cAPI.Tablet_Shell_Check("tablet_d", "현금영수증");

                Thread.Sleep(200);

                if (rdoGubun4.Checked)
                {
                    Proc.StartInfo.FileName = clsPublic.GstrTabletD_FilePath;
                    Proc.StartInfo.Arguments = "환불님!" + txtCashAmt.Text.Trim() + "!";
                    Proc.Start();
                }
                else
                {
                    Proc.StartInfo.FileName = clsPublic.GstrTabletD_FilePath;
                    Proc.StartInfo.Arguments = lblSName.Text.Trim() + "!" + txtCashAmt.Text.Trim() + "!";
                    Proc.Start();
                }
                #endregion
            }
            else
            {
                #region 신용카드승인 싸인 화면

                //picture1.Image = null;

                //clsOumsad cO = new clsOumsad();

                //if (rdoGubun2.Checked)
                //    cO.Customer_Display("환불님!", txtCardAmt.Text.Trim());
                //else
                //    cO.Customer_Display(lblSName.Text.Trim(), txtCardAmt.Text.Trim());

                //btnPead.Focus();

                #endregion
            }

        }
        
        /// <summary>
        /// 서명확인 버튼
        /// </summary>
        //void Confirm_Sign_Card(string strPath)
        //{
        //    if (clsPmpaPb.GstrCreditIF != "T")
        //    {
        //        return;
        //    }

        //    picture1.Image = null;

        //    rCSign(strPath);

        //    //이미 폼이 떠있는지 확인한다.
        //    foreach (Form frmFindform in Application.OpenForms)
        //    {
        //        if (frmFindform.GetType() == typeof(frmTablet_b))
        //        {
        //            frmFindform.Close();
        //            frmFindform.Dispose();
        //            break;
        //        }
        //    }
            
        //    if (!File.Exists(Card.GstrpFile))
        //    {
        //        ComFunc.MsgBox("SignData 폴더에 svk_sign.bmp 파일이 없습니다.", "파일누락");
        //        return;
        //    }
        //    else
        //    {

        //        if (Image.FromFile(Card.GstrpFile) != null)
        //        {
        //            picture1.Image = Image.FromFile(Card.GstrpFile);                    
        //        }

        //        K100_Request();

        //        btnSave1.Focus();

        //    }
            
        //}

        /// <summary>
        /// 입력번호 확인 버튼
        /// </summary>
        /// 
        void Confirm_Sign_Cash()
        {
            if (!File.Exists(Card.GstrcFile))
            {
                ComFunc.MsgBox("CashData 폴더에 CashData.dat 파일이 없습니다.", "파일누락");
                return;
            }
            else
            {
                string[] txtNum = File.ReadAllLines(Card.GstrcFile);

                if (txtNum.Length > 0)
                {
                    txtHphone.Text = VB.Left(txtNum[0].Trim(), 12).Trim();
                }

                btnSave2.Focus();
            }
           
        }

        //void K100_Request()
        //{
        //    string strAmt = string.Empty;

        //    Card CARD = new Card();

        //    if (clsPmpaPb.GstrCreditIF != "T")
        //    {
        //        return;
        //    }

        //    if (txtCardAmt.Text.Trim() != "")
        //    {
        //        strAmt = VB.Replace(txtCardAmt.Text, ",", "").Trim();
        //    }
            
        //    if (strAmt == "")
        //    {
        //        strAmt = "0";
        //    }

        //    //테블릿 사용창구
        //    if (isConnected == true)
        //    {
        //        try
        //        {
        //            StringBuilder req = new StringBuilder();
        //            req.Clear();

        //            req = CARD.insertLeftJustify(req, "K100", 4);                   // 전문구분
        //            if (clsCompuInfo.gstrCOMIP == "192.168.2.68")
        //            {
        //                req = CARD.insertLeftJustify(req, Card.GstrTerminal_SID_Test, 8);    // 단말기 번호
        //            }
        //            else
        //            {
        //                req = CARD.insertLeftJustify(req, Card.GstrTerminal_SID2, 8);    // 단말기 번호
        //            }
        //            req = CARD.insertLeftJustify(req, strAmt, 12);                  // 거래금액
        //            req = CARD.insertLeftJustify(req, "1", 1);                      // 입력요청구분

        //            Writer.Write(req.ToString());
        //            Writer.Flush();

        //        }
        //        catch (Exception ex)
        //        {
        //            lblMsg.Text = "서버와 연결이 실패되었습니다.";
        //        }

        //    }
        //}

        void Approval_Data(PsmhDb pDbCon)
        {
            DataTable Dt        = null;
            string strAmt       = string.Empty;
            string strTrade     = string.Empty;
            string strKey       = string.Empty;
            string strJobGbn    = string.Empty; //승인, 취소구분
            string strMemData   = string.Empty; //가맹점 Data

            #region CardVariable Clear
            Card.CVar.gstrCdPtno = "";
            Card.CVar.gstrCdSName = "";
            Card.CVar.gstrCdDeptCode = "";
            Card.CVar.gstrCdPart = "";
            Card.CVar.gstrRemark = "";
            Card.CVar.gstrCdGbIo = "";
            Card.CVar.gnHPano = 0;
            Card.CVar.gnHWrtno = 0;
            Card.CVar.gstrTradeKey = "";
            //Card.CVar.gstrCdPCode = "";
            #endregion

            #region Data_Check
            if (FstrJob == "CARD")
            {
                if (txtCardAmt.Text.Trim() == "")
                {
                    txtCardAmt.Text = "0";
                }

                strAmt = VB.Replace(txtCardAmt.Text, ",", "").Trim();
                strJobGbn = rdoGubun1.Checked == true ? "CARD+" : "CARD-";
            }
            else
            {
                if (txtCashAmt.Text.Trim() == "")
                {
                    txtCashAmt.Text = "0";
                }

                strAmt = VB.Replace(txtCashAmt.Text, ",", "").Trim();
                strJobGbn = rdoGubun3.Checked == true ? "CASH+" : "CASH-";
            }
            
            if (strJobGbn == "CASH-")
            {
                if (cboCan.Text.Trim() == "")
                {
                    ComFunc.MsgBox("취소 사유가 공란입니다.", "작업불가");
                    return;
                }
            }

            if (strAmt == "")
            {
                strAmt = "0";
            }

            if (Convert.ToInt64(strAmt) == 0)
            {
                ComFunc.MsgBox("승인금액이 없습니다.","금액확인");
                return;
            }

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호가 없습니다.", "확인");
                return;
            }

            if (VB.Left(cboIO.Text, 1) != "T" && VB.Left(cboIO.Text, 1) != "H" && VB.Left(cboIO.Text, 1) != "*")
            {
                ComFunc.MsgBox("종검/일반 구분오류 .", "확인");
                return;
            }


            if (cboDept.Text.Trim() == "" || cboDept.Text.Trim() == "**" || VB.Left(cboDept.Text, 2).Trim() == "")
            {
                ComFunc.MsgBox("진료과목이 없습니다.", "확인");
                return;
            }

            if (cboIO.Text.Trim() == "" || VB.Left(cboIO.Text, 1).Trim() == "*" || VB.Left(cboIO.Text, 1).Trim() == "")
            {
                ComFunc.MsgBox("종검/일반 구분이 없습니다.", "확인");
                return;

            }

            if (FstrJob == "CASH")
            {

                //1,2,3 (소득공제, 지출증빙, 자진발금)strTrade
                //거래구분
                if (rdoGbn1.Checked)
                {   //영수발급
                    strTrade = rdoGubun3.Checked == true ? "1" : "A";
                }
                else if (rdoGbn2.Checked)
                {   //지출증빙
                    strTrade = rdoGubun3.Checked == true ? "2" : "B";
                }
                else if (rdoGbn3.Checked)
                {   //자진발급
                    strTrade = rdoGubun3.Checked == true ? "3" : "E";
                }

                //입력방식 (KeyIn)
                if (rdoKey1.Checked)
                {
                    strKey = "0";
                }
                else if (rdoKey2.Checked)
                {
                    strKey = "1";
                }
                else if (rdoKey3.Checked)
                {
                    strKey = "2";
                }
            }

            if (FstrJob == "CASH")
            {
                if (txtHphone.Text.Trim() == "")
                {
                    ComFunc.MsgBox("현금승인 번호가 없습니다.", "승인불가");
                    return;
                }
            }

            //2018-09-07 자진발급 번호 매칭이 안되면 0100001234 다우에서 일일근로 영수증으로 등록됨 
            if (FstrJob == "CASH")
            {
                if (txtHphone.Text.Trim() != "0100001234" && strTrade =="3" )
                {
                    ComFunc.MsgBox("자진발급 번호가 잘못입력되었습니다.0100001234 ", "승인불가");
                    return;
                }
            }

            #endregion

            if (isConnected == true)
            {
                #region Data_Set 
                Card.CVar.gstrCdPtno = txtPano.Text.Trim();
                Card.CVar.gstrCdSName = lblSName.Text.Trim();
                Card.CVar.gstrCdDeptCode = VB.Left(cboDept.Text, 2).ToUpper().Trim();
                Card.CVar.gstrCdPart = clsType.User.IdNumber;
                Card.CVar.gnHPano = FnHPano;
                Card.CVar.gnHWrtno = FnHWrtno;

                //Card.CVar.gstrTradeKey = "2";
                Card.CVar.gstrTradeKey = strTrade;

                switch (strTrade)
                {
                    case "A": Card.CVar.gstrTradeKey = "1"; break;
                    case "B": Card.CVar.gstrTradeKey = "2"; break;
                    case "E": Card.CVar.gstrTradeKey = "3"; break;
                    default: break;
                }

                //가맹점 Data Set
                strMemData = "";
                strMemData += txtPano.Text + "@";
                strMemData += FstrBDate + "@";
                strMemData += VB.Left(cboDept.Text, 2).ToUpper().Trim() + "@";
                strMemData += clsType.User.IdNumber;

                Dt = cPF.Get_BasPatient(pDbCon, Card.CVar.gstrCdPtno);

                if (Dt == null || Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("등록된 환자가 없습니다.", "등록번호 확인요망!");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                Dt.Dispose();
                Dt = null;

                CARD.SELECT_JUMIN(pDbCon, Card.CVar.gstrCdPtno);

                if (VB.Left(cboDept.Text, 2) == "II")
                {
                    Card.CVar.gstrCdGbIo = "O";
                }
                else
                {
                    Card.CVar.gstrCdGbIo = VB.Left(cboIO.Text, 1);
                }
                
                if (strJobGbn == "CARD+" || strJobGbn == "CASH+")
                {
                    m_cardApproveGbn = "1";
                    FstrGubun = "카드승인";
                    lblMsg.Text = "승인요청중입니다. 결과를 기다리십시오.";
                    if (Card.CVar.gstrCdPCode == "" || Card.CVar.gstrCdPCode == null)
                    {
                        Card.CVar.gstrCdPCode = "SUP+";
                    }
                    else
                    {
                        Card.CVar.gstrCdPCode = VB.Left(Card.CVar.gstrCdPCode, 3) + "+";
                    }
                }
                else if (strJobGbn == "CARD-" || strJobGbn == "CASH-")
                {
                    m_cardApproveGbn = "2";
                    FstrGubun = "카드취소";
                    lblMsg.Text = "승인취소요청중입니다. 결과를 기다리십시오.";
                    if (Card.CVar.gstrCdPCode == "" || Card.CVar.gstrCdPCode == null)
                    {
                        Card.CVar.gstrCdPCode = "SUP-";
                    }
                    else
                    {
                        Card.CVar.gstrCdPCode = VB.Left(Card.CVar.gstrCdPCode, 3) + "-";
                    }
                }

                if (txtDivideTerm.Text.Trim() == "")
                {
                    txtDivideTerm.Text = "00";
                }
                else
                {
                    txtDivideTerm.Text = VB.Val(txtDivideTerm.Text).ToString("00");
                }

                txtMemsData.Text = "GO" + strMemData;
                txtMemsData2.Text = "GO" + strMemData;

                //btnSave1.Enabled = false;
                //btnSave2.Enabled = false;

                if (FstrJob == "CARD")
                {
                    clsPmpaType.RSD.Gubun = "1";                                                        //신용승인
                    clsPmpaType.RSD.CardData = "";                                                      //카드 번호
                    clsPmpaType.RSD.TotAmt = Convert.ToInt64(VB.Replace(txtCardAmt.Text, ",", ""));     //승인금액
                    clsPmpaType.RSD.EntryMode = "S";                                                    //카드 처리구분
                    clsPmpaType.RSD.CardDivide = Convert.ToInt32(txtDivideTerm.Text);                   //카드 할부기간
                    if (m_cardApproveGbn == "2")
                    {
                        clsPmpaType.RSD.AApproveNo = txtOrNo.Text.Trim();                               //승인번호(취소할때)
                        clsPmpaType.RSD.ASaleDate = txtOrDate.Text.Trim();                              //승인일자(취소할때)
                    }
                    else
                    {
                        clsPmpaType.RSD.AApproveNo = "";                                                //승인번호(취소할때)
                        clsPmpaType.RSD.ASaleDate = "";                                                 //승인일자(취소할때)
                    }
                }
                else if (FstrJob == "CASH")
                {
                    clsPmpaType.RSD.Gubun = "2";                                                        //현금영수증
                    clsPmpaType.RSD.CardData = clsAES.AES(txtHphone.Text);                                          //현금영수증 번호
                    clsPmpaType.RSD.TotAmt = Convert.ToInt64(VB.Replace(txtCashAmt.Text, ",", ""));     //승인금액
                    //clsPmpaType.RSD.EntryMode = strTrade;                                               //카드 처리구분
                    clsPmpaType.RSD.EntryMode = "K";                                                    //카드 처리구분
                    clsPmpaType.RSD.KeyIn = strKey;                                                     //입력방식
                    clsPmpaType.RSD.CanSayu = VB.Left(cboCan.Text, 1);                                  //취소사유코드
                    if (m_cardApproveGbn == "2")
                    {
                        clsPmpaType.RSD.AApproveNo = txtOrNo2.Text.Trim();                              //승인번호(취소할때)
                        clsPmpaType.RSD.ASaleDate = txtOrDate2.Text.Trim();                             //승인일자(취소할때)
                    }
                    else
                    {
                        clsPmpaType.RSD.AApproveNo = "";                                                //승인번호(취소할때)
                        clsPmpaType.RSD.ASaleDate = "";                                                 //승인일자(취소할때)
                    }
                }

                clsPmpaType.RSD.OrderGb     = m_cardApproveGbn;                                         //승인요청
                clsPmpaType.RSD.CardData2   = "";                                                       //카드 Full 번호
                clsPmpaType.RSD.SEQNO       = cPF.GET_NEXT_CDSEQNO(pDbCon);                             //거래일련번호 가져오기
                clsPmpaType.RSD.CardSeqNo   = cPF.GET_NEXT_CARDSEQNO(pDbCon);                           //카드승인 일련번호 
                clsPmpaType.RSD.Cardthru    = "";                                                       //카드 유효기간
                
                #endregion

                try
                {
                    #region Data_Build
                    StringBuilder req = new StringBuilder();
                    req.Clear();

                    if (clsPmpaType.RSD.Gubun == "1")
                    {
                        if (m_cardApproveGbn == "1")        //신용승인
                        {
                            req = CARD.insertLeftJustify(req, "0100", 4);               // 전문구분
                            req = CARD.insertLeftJustify(req, "10", 2);                 // 업무구분
                        }
                        else if (m_cardApproveGbn == "2")   //승인취소
                        {
                            req = CARD.insertLeftJustify(req, "0400", 4);               // 전문구분
                            req = CARD.insertLeftJustify(req, "10", 2);                 // 업무구분
                        }
                    }
                    else if (clsPmpaType.RSD.Gubun == "2")
                    {
                        if (m_cardApproveGbn == "1")        //현금승인
                        {
                            req = CARD.insertLeftJustify(req, "0200", 4);           // 전문구분
                            req = CARD.insertLeftJustify(req, "40", 2);             // 업무구분
                        }
                        else if (m_cardApproveGbn == "2")   //현금취소
                        {
                            req = CARD.insertLeftJustify(req, "0420", 4);           // 전문구분
                            req = CARD.insertLeftJustify(req, "40", 2);             // 업무구분
                        }
                    }
                    if (clsCompuInfo.gstrCOMIP == "192.168.41.65")
                    {
                        req = CARD.insertLeftJustify(req, Card.GstrTerminal_SID_Test, 8);    // 단말기 번호
                    }
                    else
                    {
                        req = CARD.insertLeftJustify(req, Card.GstrTerminal_SID2, 8);    // 단말기 번호
                    }
                    req = CARD.insertLeftJustify(req, "0000", 4);                   //전표번호
                    req = CARD.insertLeftJustify(req, "0", 14);                     //단말기정보 (단말기 구분, 둥글, RF카드 등등)

                    if (clsPmpaType.RSD.Gubun == "1")
                    {
                        req = CARD.insertLeftJustify(req, txtDivideTerm.Text, 2);   //할부개월
                    }
                    else if (clsPmpaType.RSD.Gubun == "2")
                    {
                        req = CARD.insertLeftJustify(req, strTrade, 2);             //거래구분
                    }
                    
                    req = CARD.insertLeftJustify(req, strAmt, 12);                  //거래금액
                    req = CARD.insertLeftJustify(req, "0", 12);                     //봉사료
                    req = CARD.insertLeftJustify(req, "0", 12);                     //부가세
                    req = CARD.insertLeftJustify(req, "0", 12);                     //비과세

                    if (clsPmpaType.RSD.Gubun == "1")
                    {
                        req = CARD.insertLeftJustify(req, txtOrDate.Text, 8);           //원승인일자
                        req = CARD.insertLeftJustify(req, txtOrNo.Text, 12);            //원승인번호
                    }
                    else if (clsPmpaType.RSD.Gubun == "2")
                    {
                        req = CARD.insertLeftJustify(req, txtOrDate2.Text, 8);           //원승인일자
                        req = CARD.insertLeftJustify(req, txtOrNo2.Text, 12);            //원승인번호
                    }
                   
                    if (clsPmpaType.RSD.Gubun == "2")
                    {
                        req = CARD.insertLeftJustify(req, VB.Left(cboCan.Text, 1), 1);  //취소사유
                        req = CARD.insertLeftJustify(req, strKey, 1);                   //KeyIn
                        req = CARD.insertLeftJustify(req, txtHphone.Text, 30);          //휴대폰번호
                    }

                    if (clsPmpaType.RSD.Gubun == "1")
                    { 
                        req = CARD.insertLeftJustify(req, txtMemsData.Text, 42);        //가맹점데이터
                    }
                    else if (clsPmpaType.RSD.Gubun == "2")
                    { 
                        req = CARD.insertLeftJustify(req, txtMemsData2.Text, 42);        //가맹점데이터
                    }

                    #endregion

                    lblMsg.Text = "";

                    Writer.Write(req.ToString());
                    Writer.Flush();

                    req.Clear();
                    req = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.ToString());
                    lblMsg.Text = "서버와 연결이 실패되었습니다.";
                }
            }
        }

        void Card_Approv_Data_Set(string strRD)
        {
            clsPmpaType.RD.Trade = VB.Left(strRD, 4);                   //정상승인여부 (1000, 2000)
            clsPmpaType.RD.ReqMsg = VB.Mid(strRD, 23, 4);                 //응답코드 정상승인여부 (0000)
            clsPmpaType.RD.OrderGb = m_cardApproveGbn;                      //1.승인 2.승인취소

            if (clsPmpaType.RSD.Gubun == "1")
            {    
                clsPmpaType.RD.MDate        = CARD.gWordByByte(strRD, 42, 14);  //승인일시
                clsPmpaType.RD.ApprovalNo   = CARD.gWordByByte(strRD, 68, 12);  //승인번호
                clsPmpaType.RD.PublishBank  = CARD.gWordByByte(strRD, 83, 16);  //발급사명
                clsPmpaType.RD.BankName     = CARD.gWordByByte(strRD, 99, 20);  //매입사명
                clsPmpaType.RD.BankId       = CARD.gWordByByte(strRD, 80, 3);   //매입코드
                clsPmpaType.RD.MemberNo     = CARD.gWordByByte(strRD, 119, 15); //가맹점번호
                clsPmpaType.RD.Massage      = CARD.gWordByByte(strRD, 170, 67); //화면메세지
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                clsPmpaType.RD.MDate        = CARD.gWordByByte(strRD, 42, 14);  //승인일시
                clsPmpaType.RD.ApprovalNo   = CARD.gWordByByte(strRD, 68, 12);  //승인번호
                clsPmpaType.RD.PublishBank  = "";                               //발급사명
                clsPmpaType.RD.BankName     = "";                               //매입사명
                clsPmpaType.RD.BankId       = "";                               //매입코드
                clsPmpaType.RD.MemberNo     = CARD.gWordByByte(strRD, 119, 15); //가맹점번호
                clsPmpaType.RD.Massage      = CARD.gWordByByte(strRD, 187, 67); //화면메세지
            }

            if (clsPmpaType.RD.ApprovalNo == "")
            {
                btnSave1.Enabled = true;
                btnCancel1.Enabled = true;
                btnExit.Enabled = true;
                return;
            }

            //승인번호가 없으면 Error
            if (clsPmpaType.RD.ApprovalNo == "")
            {
                ComFunc.MsgBox("승인번호가 없습니다. 다시 결제해주세요.", "승인에러");
                return;
            }

            if ((clsPmpaType.RD.Trade.Trim() != "1000" && clsPmpaType.RD.Trade.Trim() != "2000") || clsPmpaType.RD.ReqMsg != "0000")
            {
                //ComFunc.MsgBox("거래가 완료되지 않았습니다.다시 결제해주세요.", "승인에러");
                lblMsg.Text = VB.Mid(strRD, 171, 67);
                return;
            }
            // 2018-08-02
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (CARD.CardApprov_Insert_HIC(clsDB.DbCon, FstrJob) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (m_cardApproveGbn == "2")
                {
                    if (CARD.UpDate_Card_Refund_HIC(clsDB.DbCon, FstrRowid) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                return;
            }
                       

            //활성화된 탭에서 선택된 컨트롤을 확인해야함 (중복코딩 아님)
            if (FstrJob == "CARD" && chkPrt.Checked)
            {
                Card_Print();       //POS 프린트 출력
            }
            else if (FstrJob == "CASH" && chkPrt2.Checked)
            {
                Card_Print();       //POS 프린트 출력
            }

            cAPI.Tablet_Shell_Check("tablet_d", "현금영수증");

            //정상승인인 경우만 창 닫음
            Application.DoEvents();
            this.Close();
            return;
        }

        void Read_Card_Approv_Data(PsmhDb pDbCon, string argPano, string argOrigin)
        {

            CARD_APPROV_CENTER item = cardApprovCenterService.GetDataByPanoOrigin(argPano, argOrigin);

            if (!item.IsNullOrEmpty())
            {
                FstrRePrt = "Y";    //전표재발행

                Card.CVar.gstrCdSName = item.SNAME;
                Card.CVar.gstrCdPtno = item.PANO;
                Card.CVar.gstrGubun = item.GUBUN1;
                Card.CVar.gnHPano  = item.HPANO;
                Card.CVar.gnHWrtno = item.HWRTNO;
                clsPmpaType.RSD.TotAmt = item.TRADEAMT;      //거래금액
                clsPmpaType.RD.BankName = item.ACCEPTERNAME;     //매입사명
                clsPmpaType.RD.PublishBank = item.FINAME;        //발급사명
                clsPmpaType.RSD.CardDivide = item.INSTPERIOD.To<int>(); //할부개월수
                clsPmpaType.RD.ApprovalNo = item.ORIGINNO;       //승인번호
                clsPmpaType.RD.MDate = item.APPROVDATE;          //거래일시

                if (item.TRANHEADER.Trim() == "2")
                {
                    FstrGubun = "카드취소";
                }
                else
                {
                    FstrGubun = "카드승인";
                }

                if (FstrJob == "CASH")
                {
                    clsPmpaType.RSD.CardData = clsAES.DeAES(item.CARDNO.Trim());  //휴대폰번호
                }
            }
        }

        void Display_Message(string strMsg)
        {
            lblMsg.Text = strMsg;
        }

        void DisConnect_Port()
        {
            Connected = false;

            if (Reader != null)
            {
                Reader.Close();
            }

            if (Writer != null)
            {
                Writer.Close();
            }

            if (Client != null)
            {
                Client.Close();
            }

            if (ReceiveThread != null)
            {
                ReceiveThread.Abort();
            }
        }

        void frmHcEntryCardDaou_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisConnect_Port();
        }
        
        void Card_Print()
        {

            clsPrint CP = new clsPrint();

            PrintDocument pd;

            if (CP.isPmpaBarCodePrinter("신용카드") == false)
            {
                ComFunc.MsgBox("지정된 프린터를 찾을수 없습니다.", "신용카드");
                return;
            }

            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = CP.getPmpaBarCodePrinter("신용카드");
            pd.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize("POSSIZE", 50, 30);

            if (FstrJob == "CARD")
            {
                pd.PrintPage += new PrintPageEventHandler(ePosPrint);
            }
            else if (FstrJob == "CASH")
            {
                pd.PrintPage += new PrintPageEventHandler(ePosPrint2);
            }
            
            pd.Print();    //프린트
        }

        //신용카드 전표발행
        void ePosPrint(object sender, PrintPageEventArgs ev)
        {
            string strOPD = string.Empty;
            string strAmt = string.Empty;

            int nX = 0;
            int nY = 0;
            int nCY = 18;

            //Image Img = picture1.Image;

            strOPD = Card.CVar.gstrCdSName + " OPD:" + Card.CVar.gstrCdPtno;

            if (FstrGubun == "카드취소")
            {
                //strAmt = (clsPmpaType.RSD.TotAmt * -1).ToString("###,###,##0");
                strAmt = (VB.Val(VB.Replace(txtCardAmt.Text, ",", "")) * -1).ToString("###,###,##0");
            }
            else
            {
                //strAmt = clsPmpaType.RSD.TotAmt.ToString("###,###,##0");
                strAmt = VB.Val(VB.Replace(txtCardAmt.Text, ",", "")).ToString("###,###,##0");
            }
            
            ev.Graphics.DrawString("       ◈ 고 객 용 ◈",                                   new Font("굴림체", 14f), Brushes.Black, nX + 5, nY + (nCY * 0), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",               new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 1), new StringFormat());
            ev.Graphics.DrawString("가  맹  점 : (재)포항성모병원",                         new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 2), new StringFormat());
            ev.Graphics.DrawString("사업자번호 : 506 - 82 - 00896",                         new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 3), new StringFormat());
            ev.Graphics.DrawString("주      소 : 경북 포항시 남구 대잠동길 17",             new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 4), new StringFormat());
            ev.Graphics.DrawString("가맹점번호 : " + Card.GstrMerchantNo,                   new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 5), new StringFormat());
            ev.Graphics.DrawString("대  표  자 : " + Card.GstrCardCEO,                      new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 6), new StringFormat());
            ev.Graphics.DrawString("대  표 TEL : " + Card.GstrCardTel,                      new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 7), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",               new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 8), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",               new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 9), new StringFormat());
            ev.Graphics.DrawString(" 매  입  사 : " + clsPmpaType.RD.BankName,              new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 10), new StringFormat());
            ev.Graphics.DrawString("카 드 종 류 : " + clsPmpaType.RD.PublishBank,           new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 11), new StringFormat());
            ev.Graphics.DrawString("거 래 유 형 : " + FstrGubun,                            new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 12), new StringFormat());
            ev.Graphics.DrawString("할 부 개 월 : " + clsPmpaType.RSD.CardDivide + "개월",  new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 13), new StringFormat());
            ev.Graphics.DrawString("승 인 번 호 : " + clsPmpaType.RD.ApprovalNo,            new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 14), new StringFormat());
            ev.Graphics.DrawString("거 래 일 시 : " + clsPmpaType.RD.MDate,                 new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 15), new StringFormat());
            ev.Graphics.DrawString("성 명 (OPD) : " + strOPD,                               new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 16), new StringFormat());
            ev.Graphics.DrawString("카드금액 : " + strAmt,                                  new Font("굴림체", 13f), Brushes.Black, nX + 0, nY + (nCY * 17), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",               new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 18), new StringFormat());
            if (FstrRePrt == "Y")
            {
                ev.Graphics.DrawString("전표재발행", new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 19), new StringFormat());
            }
            else
            {
                ev.Graphics.DrawString(" ",         new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 19), new StringFormat());
            }

            //if (Img != null)
            //{
            //    ev.Graphics.DrawImage(Img, nX + 20, nY + (nCY * 19));
            //}
            //ev.Graphics.DrawString(" 포  항  성  모  병  원",                           new Font("굴림체", 16f), Brushes.Black, nX + 0, nY + (nCY * 22), new StringFormat());
            ev.Graphics.DrawString(" 포  항  성  모  병  원", new Font("굴림체", 16f), Brushes.Black, nX + 0, nY + (nCY * 20), new StringFormat());
        }

        //현금영수증 전표발행
        void ePosPrint2(object sender, PrintPageEventArgs ev)
        {
            string strOPD = string.Empty;
            string strAmt = string.Empty;

            int nX = 0;
            int nY = 0;
            int nCY = 18;
            
            strOPD = Card.CVar.gstrCdSName + " OPD:" + Card.CVar.gstrCdPtno;

            if (FstrGubun == "카드취소")
            {
                //strAmt = (clsPmpaType.RSD.TotAmt * -1).ToString("###,###,##0");
                strAmt = (VB.Val(VB.Replace(txtCashAmt.Text, ",", "")) * -1).ToString("###,###,##0");
            }
            else
            {
                //strAmt = clsPmpaType.RSD.TotAmt.ToString("###,###,##0");
                strAmt = VB.Val(VB.Replace(txtCashAmt.Text, ",", "")).ToString("###,###,##0");
            }

            if (rdoGbn1.Checked)
            {
                ev.Graphics.DrawString("   ◈ 현 금 (소득공제) ◈", new Font("굴림체", 14f), Brushes.Black, nX + 5, nY + (nCY * 0), new StringFormat());
            }
            else if (rdoGbn2.Checked)
            {
                ev.Graphics.DrawString("   ◈ 현 금 (지출증빙) ◈", new Font("굴림체", 14f), Brushes.Black, nX + 5, nY + (nCY * 0), new StringFormat());
            }
            
            ev.Graphics.DrawString("---------------------------------------",       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 1), new StringFormat());
            ev.Graphics.DrawString("가  맹  점 : (재)포항성모병원",                 new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 2), new StringFormat());
            ev.Graphics.DrawString("사업자번호 : 506 - 82 - 00896",                 new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 3), new StringFormat());
            ev.Graphics.DrawString("주      소 : 경북 포항시 남구 대잠동길 17",     new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 4), new StringFormat());
            ev.Graphics.DrawString("가맹점번호 : " + Card.GstrMerchantNo,           new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 5), new StringFormat());
            ev.Graphics.DrawString("대  표  자 : " + Card.GstrCardCEO,              new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 6), new StringFormat());
            ev.Graphics.DrawString("대  표 TEL : " + Card.GstrCardTel,              new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 7), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 8), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 9), new StringFormat());
            //ev.Graphics.DrawString("식 별 정 보 : " + clsPmpaType.RSD.CardData,     new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 10), new StringFormat());
            ev.Graphics.DrawString("식 별 정 보 : " + VB.Left(clsPmpaType.RSD.CardData,8) + "******", new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 10), new StringFormat());
            ev.Graphics.DrawString("승 인 번 호 : " + clsPmpaType.RD.ApprovalNo,    new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 11), new StringFormat());
            ev.Graphics.DrawString("승 인 일 시 : " + clsPmpaType.RD.MDate,         new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 12), new StringFormat());
            ev.Graphics.DrawString("성 명 (OPD) : " + strOPD,                       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 13), new StringFormat());
            ev.Graphics.DrawString("현       금 : " + strAmt,                       new Font("굴림체", 13f), Brushes.Black, nX + 0, nY + (nCY * 14), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 15), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 16), new StringFormat());
            ev.Graphics.DrawString("   현금영수증 상담센터 국번없이 126",           new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 17), new StringFormat());
            ev.Graphics.DrawString("   http://현금영수증.kr",                       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 18), new StringFormat());
            ev.Graphics.DrawString("---------------------------------------",       new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 19), new StringFormat());
            ev.Graphics.DrawString(" ",                                             new Font("굴림체", 10f), Brushes.Black, nX + 0, nY + (nCY * 20), new StringFormat());
            ev.Graphics.DrawString(" 포  항  성  모  병  원",                       new Font("굴림체", 16f), Brushes.Black, nX + 0, nY + (nCY * 21), new StringFormat());
        }

    }
}
