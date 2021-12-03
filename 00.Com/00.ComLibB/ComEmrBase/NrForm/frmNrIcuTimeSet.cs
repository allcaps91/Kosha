using ComBase;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmNrIcuTimeSet : Form
    {
        public delegate void SetTime(string strTime);
        public event SetTime rSetTime;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmNrIcuTimeSet()
        {
            InitializeComponent();
        }

        private void frmNrIcuTimeSet_Load(object sender, EventArgs e)
        {
            CboTimeSet();
            txtSTime.SelectedIndex = 0;
            txtSTime1.SelectedIndex = 0;
            txtETime.SelectedIndex = 0;
            txtETime1.SelectedIndex = 0;
            txtTime.SelectedIndex = 0;
        }

        private void CboTimeSet()
        {
            int i = 0;

            for (i = 0 ; i <=23 ; i++)
            {
                txtSTime.Items.Add(i.ToString("00"));
            }

            for (i = 1; i <= 24; i++)
            {
                txtETime.Items.Add(i.ToString("00"));
            }

            for (i = 0; i <= 59; i++)
            {
                txtSTime1.Items.Add(i.ToString("00"));
                txtETime1.Items.Add(i.ToString("00"));
            }
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            //시간을 03:00/03:30/

            //int i = 0;
            //00:00 ~ 2400;
            //10
            string strTime = "";
            //int int60 = 0;

            double intTime = VB.Val(txtTime.Text.Trim());

            int intStime = (int)VB.Val(txtSTime.Text.Trim() + txtSTime1.Text.Trim());
            int intEtime = (int)VB.Val(txtETime.Text.Trim() + txtETime1.Text.Trim());

            if (intEtime < intStime)
            {
                ComFunc.MsgBoxEx(this, "시작시간, 종료시간이 잘못되었습니다.");
                return;
            }

            if (intTime == 1 && ComFunc.MsgBoxQEx(this, "선택하신 구간은 1분입니다.\r\n잘못 선택하셨다면 다시 선택해주세요.") == DialogResult.No)
            {
                txtTime.Focus();
                return;
            }

            int intHour = (int)VB.Val(txtSTime.Text.Trim()+"00");
            int intBun = (int)VB.Val(txtSTime1.Text.Trim());

            DateTime sDate = Convert.ToDateTime(txtSTime.Text.Trim() + ":" + txtSTime1.Text.Trim());
            DateTime eDate;
            if (intEtime >= 2400)
            {
                eDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10)).AddDays(+1);
            }
            else
            {
                eDate = Convert.ToDateTime(txtETime.Text.Trim() + ":" + txtETime1.Text.Trim());
            }

            strTime += sDate.ToString("HH:mm") + "/";

            while (sDate < eDate)
            {
                sDate = sDate.AddMinutes(intTime);
                if (sDate <= eDate)
                {
                    strTime += sDate.ToString("HH:mm") + "/";
                }
            }

            //for (i = 0; i < 100000; i++)
            //{
            //    if (i == 0)
            //    {
            //        int60 = intBun;
            //    }
            //    else
            //    {
            //        int60 = int60 + intTime;
            //    }

            //    //int60 = int60 + intTime;
            //    if (intStime >= intEtime)
            //    {
            //        break;
            //    }
            //    if (int60 >= 60)
            //    {
            //        intStime = intStime + 100;

            //        string strXX = intStime.ToString("0000");
            //        intHour = (int)VB.Val((VB.Left(strXX, 2) + "00"));

            //        intStime = intHour + (int60 - 60);
            //        int60 = (int60 - 60);
            //        strTime = strTime + (intStime).ToString("00:00") + "/";
            //    }
            //    else
            //    {
            //        string strXX = intStime.ToString("0000");
            //        intHour = (int)VB.Val((VB.Left(strXX, 2) + "00"));
            //        intStime = intHour + int60;

            //        strTime = strTime + (intStime).ToString("00:00") + "/";
            //    }
            //}

            if (strTime.Length > 2) strTime = VB.Left(strTime, strTime.Length - 1);

            rSetTime(strTime);
        }

        private void txtSTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSTime1.Focus();
            }
        }

        private void txtETime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtETime1.Focus();
            }
        }
    }
}
