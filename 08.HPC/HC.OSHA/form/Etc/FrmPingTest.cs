using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace HC_OSHA
{
    public partial class FrmPingTest : Form
    {
        private int nCnt = 0;
        public FrmPingTest()
        {
            InitializeComponent();
        }


        private void FrmPingTest_Load(object sender, EventArgs e)
        {

        }

        private void PingTest()
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send("115.68.23.223", 1000);
                if (reply != null)
                {
                    TxtPing.Text = "전송상태 :  " + reply.Status + "       전송속도 : " + reply.RoundtripTime.ToString() + " ms" + ComNum.VBLF + TxtPing.Text; 
                    //TxtPing.Text = reply.ToString() + ComNum.VBLF + TxtPing.Text;
                }
                //100회 실행 후 중지함
                nCnt++;
                if (nCnt >= 100) timer1.Enabled = false;
            }
            catch
            {
                Console.WriteLine("ERROR: You have Some TIMEOUT issue");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PingTest();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            nCnt = 0;
            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strNow = DateTime.Now.ToString("yyyyMMddHHmmss");

            System.IO.File.WriteAllText(@"C:\temp\네트웍점검_" + strNow + ".txt", TxtPing.Text);
            ComFunc.MsgBox("Temp 폴더에 저장 완료");
        }
    }
}
