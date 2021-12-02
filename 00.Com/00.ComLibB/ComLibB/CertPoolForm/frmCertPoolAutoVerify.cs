using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmCertPoolAutoVerify : Form
    {
        int FnTimer = 0;
        public frmCertPoolAutoVerify()
        {
            InitializeComponent();
        }

        private void frmCertPoolAutoVerify_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            
            ComFunc.ReadSysDate(clsDB.DbCon);

            cboTime.Items.Clear();
            cboTime.Items.Add("5분");
            cboTime.Items.Add("10분");
            cboTime.Items.Add("30분");
            cboTime.Items.Add("60분");
            cboTime.Items.Add("1분");

            cboTime.SelectedIndex = 0;

            chkAuto.Checked = true;
            timer1.Enabled = true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strJUMIN = string.Empty;
            string strCERTPASS = string.Empty;

            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            FnTimer = 0;

            ssView.ActiveSheet.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.SABUN, B.KORNAME, B.JUMIN3, B.CERTPASS, MAX(A.UDATE) UDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MSTS A ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_ADM.INSA_MST B ";
                SQL = SQL + ComNum.VBLF + "    ON A.SABUN = B.SABUN ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SABUN IN(SELECT SABUN FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + "                   WHERE TOIDAY IS NULL ";
                SQL = SQL + ComNum.VBLF + "                     AND CERTPASS IS NOT NULL) ";
                SQL = SQL + ComNum.VBLF + "   AND A.UDATE >= TRUNC(SYSDATE - " + txtDay.Text + " )";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.SABUN,B.KORNAME,B.JUMIN3,B.CERTPASS ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                progressBar1.Maximum = dt.Rows.Count;

                //1.API 초기화 : API_INIT
                if (clsCertWork.API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
                {
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView.ActiveSheet.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {                        
                        ssView.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["UDATE"].ToString().Trim();

                        strJUMIN = clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString().Trim());
                        strCERTPASS = clsAES.DeAES(dt.Rows[i]["CERTPASS"].ToString().Trim());

                        if (clsCertWork.ROAMING_NOVIEW_FORM(strJUMIN, strCERTPASS) == true)
                        {
                            ssView.ActiveSheet.Cells[i, 3].Text = "성공";
                        }
                        else
                        {
                            ssView.ActiveSheet.Cells[i, 3].Text = "실패";
                            ssView.ActiveSheet.Cells[i, 3].BackColor = Color.FromArgb(254, 41, 73);
                        }

                        progressBar1.Value = i + 1;
                    }
                }

                dt.Dispose();
                dt = null;
                clsCertWork.API_RELEASE();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                clsCertWork.API_RELEASE();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int nSec = 60;

            switch (cboTime.Text.Trim())
            {
                case "5분": 
                    nSec = 300;
                    break;
                case "10분":
                    nSec = 600;
                    break;
                case "30분":
                    nSec = 1800;
                    break;
                case "60분":
                    nSec = 3600;
                    break;
                case "1분":
                    nSec = 60;
                    break;
            }

            FnTimer = FnTimer + 1;
            lblTimer.Text = FnTimer.ToString();

            if (FnTimer >= nSec)
            {
                timer1.Enabled = false;

                btnView.PerformClick();

                timer1.Enabled = true;

                FnTimer = 0;
            }
        }

        private void chkAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAuto.Checked == true)
            {
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
            }
        }
    }
}
