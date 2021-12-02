using System;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// Class Name : frmCalendar1Time
    /// File Name : frmCalendar1Time.cs
    /// Title or Description : 예약일시 선택 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-03
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmCalendar1Time : Form
    {
        //전달 받은 GstrDate 날짜형으로
        DateTime FstrDate;
        //string FstrDate;
        string FstrTime; 

        public frmCalendar1Time()
        {
            InitializeComponent();
        }

        private void frmCalendar1Time_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            
            FstrDate = Convert.ToDateTime(clsPublic.GstrDate);
            FstrTime = clsPublic.GstrTime;

            monthDate.SetDate(FstrDate);

            txtHH.Text = VB.Left(FstrTime, 2);
            txtMM.Text = VB.Right(FstrTime, 2);
            lblYYMMDD.Text = FstrDate.ToShortDateString();

            //monthDate.SetDate(Convert.ToDateTime(VB.Format(clsPublic.GstrDate, "YYYY-MM-DD")));

            //txtHH.Text = VB.Left(VB.Format(clsPublic.GstrDate, "HH:MM"), 2);
            //txtMM.Text = VB.Right(VB.Format(clsPublic.GstrDate, "HH:MM"), 2);

            clsPublic.GstrDate = "취소";
            clsPublic.GstrTime = "";
        }

        private void txtHH_Enter(object sender, EventArgs e)
        {
            txtHH.SelectionStart = 0;
            txtHH.SelectionLength = 2;
        }

        private void txtHH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtMM.Focus();
            }
        }

        private void txtHH_Leave(object sender, EventArgs e)
        {
            int numChk = 0;
            if (int.TryParse(txtHH.Text, out numChk) == false || txtHH.Text.Trim() == "")
            {
                txtHH.Focus();
            }

            txtHH.Text = string.Format(txtHH.Text.Trim(), "00");
            if (numChk > 23)
            {
                txtHH.Focus();
                return;
            }
        }

        private void txtMM_Enter(object sender, EventArgs e)
        {
            txtMM.SelectionStart = 0;
            txtMM.SelectionLength = 2;
        }

        private void txtMM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnOK.Focus();
            }
        }

        private void txtMM_Leave(object sender, EventArgs e)
        {
            int numChk;
            if (int.TryParse(txtMM.Text.Trim(), out numChk) == false || txtMM.Text.Trim() == "")
            {
                txtMM.Focus();
            }

            txtMM.Text = string.Format(txtMM.Text.Trim(), "00");
            if (numChk > 59)
            {
                txtMM.Focus();
                return;
            }
        }

        /// <summary>
        /// 확인 버튼 클릭 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtHH.Text.Trim() == "")
            {
                txtHH.Focus();
                return;
            }
            if (txtMM.Text.Trim() == "")
            {
                txtMM.Focus();
                return;
            }

            //DateTime selectTime = new DateTime(monthDate.SelectionStart.Year, monthDate.SelectionStart.Month, monthDate.SelectionStart.Day, Convert.ToInt32(txtHH.Text), Convert.ToInt32(txtMM.Text), 0);

            //DateTime nowTime = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));


            //if (selectTime <= nowTime)
            //{
            //    MessageBox.Show("현재시간:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\r\n" + "현재일시분 보다 이전으로 변경 할 수 없습니다.", "변경불가");
            //    monthDate.Focus();
            //    return;
            //}

            clsPublic.GstrDate = lblYYMMDD.Text;
            clsPublic.GstrTime = txtHH.Text.Trim() + ":" + txtMM.Text.Trim();

            this.Close();
        }

        /// <summary>
        /// 취소 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsPublic.GstrDate = "취소";
            this.Close();
        }

        private void monthDate_DateChanged(object sender, DateRangeEventArgs e)
        {
            FstrDate = e.Start;
            //clsPublic.GstrDate = FstrDate.ToShortDateString();

            lblYYMMDD.Text = FstrDate.ToShortDateString();
            txtHH.Focus();
        }
    }
}
