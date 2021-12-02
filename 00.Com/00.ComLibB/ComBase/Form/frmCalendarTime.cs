using System;
using System.Windows.Forms;
using ComDbB; //DB연결

namespace ComBase
{
    /// <summary>
    /// Class Name : frmCalendarTime
    /// File Name : frmCalendarTime.cs
    /// Title or Description : 예약설정 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-03
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmCalendarTime : Form
    {
        //전달 받은 GstrDate 날짜형으로
        DateTime FstrDate;

        public frmCalendarTime()
        {
            InitializeComponent();
        }

        private void frmCalendarTime_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            FstrDate = Convert.ToDateTime(clsPublic.GstrDate);
            monthDate.SetDate(FstrDate);

            txtHH.Text = FstrDate.ToString("HH");
            txtMM.Text = FstrDate.ToString("mm");
            lblYYMMDD.Text = FstrDate.ToShortDateString();

            //dtpDate.Text = VB.Format(clsPublic.GstrDate, "YYYY-MM-DD");

            //txtHH.Text = VB.Left(VB.Format(clsPublic.GstrDate, "HH:MM"), 2);
            //txtMM.Text = VB.Right(VB.Format(clsPublic.GstrDate, "HH:MM"), 2);
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
                btnSave.Focus();
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
        /// 확인 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
            this.Close();
        }

        /// <summary>
        /// 등록 함수
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            int intRowAffected = 0; //변경된 Row 받는 변수
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            //string sSysDate = "";

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                if (MessageBox.Show("예약시간을 수정하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                if (txtHH.Text.Trim() == "")
                {
                    txtHH.Focus();
                    return false;
                }
                if (txtMM.Text.Trim() == "")
                {
                    txtMM.Focus();
                    return false;
                }



                DateTime selectTime = new DateTime(monthDate.SelectionStart.Year, monthDate.SelectionStart.Month, monthDate.SelectionStart.Day, Convert.ToInt32(txtHH.Text), Convert.ToInt32(txtMM.Text), 0);

                DateTime nowTime = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                if (selectTime <= nowTime)
                {
                    MessageBox.Show("현재시간:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\r\n" + "현재일시분 보다 이전으로 변경 할 수 없습니다.", "변경불가");
                    monthDate.Focus();
                    return false;
                }
                clsPublic.GstrDate = selectTime.ToString("yyyy-MM-dd HH:mm");

                //TODO:테스트 필요
                //구분자
                string[] separator = { "@@" };
                //구분자 적용한 문자열 배열
                string[] separatedValue = clsPublic.GstrRetValue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (separatedValue[1] != "")
                {
                    if (separatedValue[0] == "")
                    {
                        return false;
                    }

                    clsDB.setBeginTran(clsDB.DbCon);


                    switch (separatedValue[0])
                    {
                        case "1":
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL SET ";
                                SQL += ComNum.VBLF + " SeekDate =TO_DATE('" + clsPublic.GstrDate + "','YYYY-MM-DD HH24:MI') ";
                                SQL += ComNum.VBLF + " WHERE ROWID ='" + separatedValue[1] + "' ";
                                break;
                            }
                        case "2":
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST SET ";
                                SQL += ComNum.VBLF + " RDate =TO_DATE('" + clsPublic.GstrDate + "','YYYY-MM-DD HH24:MI') ";
                                SQL += ComNum.VBLF + " WHERE ROWID ='" + separatedValue[1] + "' ";
                                break;
                            }
                        case "3":
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "ETC_JUPMST SET ";
                                SQL += ComNum.VBLF + " RDate =TO_DATE('" + clsPublic.GstrDate + "','YYYY-MM-DD HH24:MI') ";
                                SQL += ComNum.VBLF + " WHERE ROWID ='" + separatedValue[1] + "' ";
                                break;
                            }
                    }
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsPublic.GstrDate = "취소";
            this.Close();
        }

        private void monthDate_DateChanged(object sender, DateRangeEventArgs e)
        {
            FstrDate = e.Start;
            clsPublic.GstrDate = FstrDate.ToShortDateString();

            lblYYMMDD.Text = FstrDate.ToShortDateString();
            txtHH.Focus();
        }
    }
}
