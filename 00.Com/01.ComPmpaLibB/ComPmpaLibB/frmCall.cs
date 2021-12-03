using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public partial class frmCall : Form
    {

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //수신부 선언
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, ref COPYDATASTRUCT lParam);

        private const int WM_COPYDATA = 0x004A;

        string mstrData = "";

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        public frmCall()
        {
            InitializeComponent();

            //this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
            //this.Visible = false;
            //this.notifyIcon1.Visible = true;
            //notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        }

        public frmCall(string strData)
        {
            InitializeComponent();

            //this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
            //this.Visible = false;
            //this.notifyIcon1.Visible = true;
            //notifyIcon1.ContextMenuStrip = contextMenuStrip1;

            mstrData = strData;
        }

        void button1_Click(object sender, EventArgs e)
        {
            int intTag = Convert.ToInt32((sender as Button).Tag);
            string strSendData = "";

            //창구설정
            if (intTag == 1001)
            {
                //strSendData = "1001" + "^" + edIP.Text + "^" + edWorkID.Text + "^" + edWorkNm.Text;
                strSendData = "1001" + "^" + "192.168.33.32" + "^" + "18631" + "^" + "이채현";
            }
            //창구정보
            else if (intTag == 1002)
            {
                strSendData = "1002" + "^" + edZone.Text + "^" + edGrp.Text + "^" + edDesk.Text;
            }
            //호출
            else if (intTag == 1011)
            {
                strSendData = "1011" + "^" + edZone.Text + "^" + edGrp.Text + "^" + edDesk.Text + "^" + edWorkID.Text + "^" + edWorkNm.Text;
            }
            //재호출
            else if (intTag == 1012)
            {
                strSendData = "1012" + "^" + edZone.Text + "^" + edGrp.Text + "^" + edDesk.Text + "^" + edWorkID.Text + "^" + edWorkNm.Text + "^" + edTicketID.Text;
            }
            //지정호출
            else if (intTag == 1013)
            {
                strSendData = "1013" + "^" + edZone.Text + "^" + edGrp.Text + "^" + edDesk.Text + "^" + edWorkID.Text + "^" + edWorkNm.Text + "^" + edTicketNo.Text;
            }
            //부재시작
            else if (intTag == 1021)
            {
                strSendData = "1021" + "^" + edZone.Text + "^" + edGrp.Text + "^" + edDesk.Text + "^" + edWorkID.Text + "^" + edWorkNm.Text + "^" + edWaitGbn.Text + "^" + edWaitInfo.Text;
            }
            //부재종료
            else if (intTag == 1022)
            {
                strSendData = "1022" + "^" + edZone.Text + "^" + edGrp.Text + "^" + edDesk.Text + "^" + edWorkID.Text + "^" + edWorkNm.Text;
            }

            txtPrg.Text = strSendData;

            procSend(strSendData);
        }

        public void procSend(string strData)
        {

            Process proc = new Process();

            proc.StartInfo.FileName = @"C:\cmc\call\" + "PrjDemon.exe";
            proc.StartInfo.Arguments = strData;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            bool result = proc.Start();

        }

        void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //수신(Send 받는부분)
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void WndProc(ref Message m)
        {

            try
            {
                switch (m.Msg)
                {
                    case WM_COPYDATA:
                        COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));

                        if (Convert.ToString(cds.dwData) == "1001")
                        {
                            string strRtn = cds.lpData;

                            listBox1.Items.Add(string.Format("[{0}]  {1}", DateTime.Now.ToString("HH:mm:ss"), strRtn));



                            string[] strData = strRtn.Split('^');

                            if (strData[0] == "0")
                            {
                                //창구설정
                                if (strData[1] == "1001")
                                {
                                }
                                //창구정보
                                else if (strData[1] == "1002")
                                {
                                    clsCall.GstrCallMsg = string.Format("[{0}]  {1}", DateTime.Now.ToString("HH:mm:ss"), strRtn);
                                    clsCall.GstrCallWait = strData[2].Substring(0, 3); //대기자수
                                    clsCall.GstrDeskInfo = strData[2];
                                }
                                //호출
                                else if (strData[1] == "1011")
                                {
                                    clsCall.GstrCallMsg = string.Format("[{0}]  {1}", DateTime.Now.ToString("HH:mm:ss"), strRtn);

                                    edTicketID.Text = strData[2].Substring(7, 10);
                                    edTicketNo.Text = strData[2].Substring(17, 4);

                                    clsCall.GstrCurTKID = edTicketID.Text; 
                                    clsCall.GstrCurTKNo = edTicketNo.Text; //호출번호
                                }
                                //재호출
                                else if (strData[1] == "1012")
                                {
                                    clsCall.GstrCallMsg = string.Format("[{0}]  {1}", DateTime.Now.ToString("HH:mm:ss"), strRtn);
                                }
                                //지정호출
                                else if (strData[1] == "1013")
                                {
                                    clsCall.GstrCallMsg = string.Format("[{0}]  {1}", DateTime.Now.ToString("HH:mm:ss"), strRtn);

                                    edTicketID.Text = strData[2].Substring(7, 10);
                                    edTicketNo.Text = strData[2].Substring(17, 4);

                                    clsCall.GstrCurTKID = edTicketID.Text;
                                    clsCall.GstrCurTKNo = edTicketNo.Text; //호출번호
                                }
                                //부재시작
                                else if (strData[1] == "1021")
                                {
                                }
                                //부재종료
                                else if (strData[1] == "1022")
                                {
                                }
                            }
                        }

                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //this.Close();
        }

        void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmCall_Load(object sender, EventArgs e)
        {
            if (mstrData != "")
            {
                procSend(mstrData);
            }
        }
    }
}
