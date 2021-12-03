using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmEveryCompareStat : Form
    {
        /// <summary>
        /// Class Name : frmEveryCompareStat
        /// File Name : frmEveryCompareStat.cs
        /// Title or Description : 환자별 방사선 수납내역 조회 페이지
        /// Author : 박성완
        /// Create Date : 2017-06-01
        /// <history> 
        /// </history>
        /// </summary>
        public frmEveryCompareStat()
        {
            InitializeComponent();
        }

        private void frmEveryCompareStat_Load(object sender, EventArgs e)
        {
            //TODO: FormInfo_History() 함수 폼로딩 사용빈도 사용
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpSDate.Text = DateTime.Parse(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-30).ToShortDateString();
            dtpEDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            cboGubun.Items.Clear();
            cboGubun.Items.Add("1.전원사유서는 있지만 퇴원분석에서 전원이 아닌 경우");
            cboGubun.Items.Add("2.퇴원분석에서 전원이지만 전원사유서가 없는 경우");
            cboGubun.Items.Add("3.전원사유서 목록");
            cboGubun.Items.Add("4.퇴원분석 목록");
            cboGubun.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ss1_Sheet1.Rows.Count = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                switch (cboGubun.Text.Substring(0, 1))
                {
                    case "1": SQL = ReadData(1); break;
                    case "2": SQL = ReadData(2); break;
                    case "3": SQL = ReadData(3); break;
                    case "4": SQL = ReadData(4); break;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.Rows.Count = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["JUMIN1"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 분기에 따른 SQL문 설정 함수
        /// </summary>
        /// <param name="Gubun"></param>
        /// <returns></returns>
        private string ReadData(int Gubun)
        {
            string rtnVal = "";

            switch (Gubun)
            {
                case 1:
                    {
                        rtnVal = "";
                        rtnVal = rtnVal + ComNum.VBLF + " SELECT A.PANO, A.OUTDATE, B.SNAME, B.SEX, B.JUMIN1 FROM (";
                        rtnVal = rtnVal + ComNum.VBLF + " SELECT PANO, TO_DATE(TO_CHAR(OUTTIME,'YYYY-MM-DD'),'YYYY-MM-DD') OUTDATE";
                        rtnVal = rtnVal + ComNum.VBLF + " FROM (";
                        rtnVal = rtnVal + ComNum.VBLF + "  SELECT PANO, DEPTCODE, SEX, INDATE INTIME, TDATE OUTTIME, THOSNAME";
                        rtnVal = rtnVal + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE TDATE >= TO_DATE('" + dtpSDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                        rtnVal = rtnVal + ComNum.VBLF + "       AND TDATE <= TO_DATE('" + dtpEDate.Text + " 23:59','YYYY-MM-DD HH24:MI'))";
                        rtnVal = rtnVal + ComNum.VBLF + "  MINUS";
                        rtnVal = rtnVal + ComNum.VBLF + "  SELECT PANO, OUTDATE FROM " + ComNum.DB_PMPA + "MID_SUMMARY";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE TMODEL = '3'";
                        rtnVal = rtnVal + ComNum.VBLF + "  AND OUTDATE >= TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD')";
                        rtnVal = rtnVal + ComNum.VBLF + "  AND OUTDATE <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD')) A, ADMIN.BAS_PATIENT B";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE A.PANO = B.PANO ";
                        rtnVal = rtnVal + ComNum.VBLF + " ORDER BY OUTDATE ";
                        rtnVal = rtnVal + ComNum.VBLF + "";
                        break;
                    }
                case 2:
                    {
                        rtnVal = "";
                        rtnVal = rtnVal + ComNum.VBLF + " SELECT A.PANO, A.OUTDATE, B.SNAME, B.SEX, B.JUMIN1 FROM (";
                        rtnVal = rtnVal + ComNum.VBLF + "  SELECT PANO, OUTDATE FROM " + ComNum.DB_PMPA + "MID_SUMMARY";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE TMODEL = '3'";
                        rtnVal = rtnVal + ComNum.VBLF + "  AND OUTDATE >= TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD')";
                        rtnVal = rtnVal + ComNum.VBLF + "  AND OUTDATE <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD')";
                        rtnVal = rtnVal + ComNum.VBLF + "  MINUS";
                        rtnVal = rtnVal + ComNum.VBLF + " SELECT PANO, TO_DATE(TO_CHAR(OUTTIME,'YYYY-MM-DD'),'YYYY-MM-DD') OUTDATE";
                        rtnVal = rtnVal + ComNum.VBLF + " FROM (";
                        rtnVal = rtnVal + ComNum.VBLF + "  SELECT PANO, DEPTCODE, SEX, INDATE INTIME, TDATE OUTTIME, THOSNAME";
                        rtnVal = rtnVal + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE TDATE >= TO_DATE('" + dtpSDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                        rtnVal = rtnVal + ComNum.VBLF + "       AND TDATE <= TO_DATE('" + dtpEDate.Text + " 23:59','YYYY-MM-DD HH24:MI'))";
                        rtnVal = rtnVal + ComNum.VBLF + " ) A, ADMIN.BAS_PATIENT B";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE A.PANO = B.PANO ";
                        rtnVal = rtnVal + ComNum.VBLF + " ORDER BY OUTDATE ";
                        rtnVal = rtnVal + ComNum.VBLF + "";
                        break;
                    }
                case 3:
                    {
                        rtnVal = "";
                        rtnVal = rtnVal + ComNum.VBLF + " SELECT A.PANO, A.OUTDATE, B.SNAME, B.SEX, B.JUMIN1 FROM (";
                        rtnVal = rtnVal + ComNum.VBLF + " SELECT PANO, TO_DATE(TO_CHAR(OUTTIME,'YYYY-MM-DD'),'YYYY-MM-DD') OUTDATE";
                        rtnVal = rtnVal + ComNum.VBLF + " FROM (";
                        rtnVal = rtnVal + ComNum.VBLF + "  SELECT PANO, DEPTCODE, SEX, INDATE INTIME, TDATE OUTTIME, THOSNAME";
                        rtnVal = rtnVal + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE TDATE >= TO_DATE('" + dtpSDate.Text + " 00:00','YYYY-MM-DD HH24:MI')";
                        rtnVal = rtnVal + ComNum.VBLF + "       AND TDATE <= TO_DATE('" + dtpEDate.Text + " 23:59','YYYY-MM-DD HH24:MI'))";
                        rtnVal = rtnVal + ComNum.VBLF + " ) A, ADMIN.BAS_PATIENT B";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE A.PANO = B.PANO ";
                        rtnVal = rtnVal + ComNum.VBLF + " ORDER BY OUTDATE ";
                        rtnVal = rtnVal + ComNum.VBLF + "";
                        break;
                    }
                case 4:
                    {
                        rtnVal = "";
                        rtnVal = rtnVal + ComNum.VBLF + " SELECT A.PANO, A.OUTDATE, B.SNAME, B.SEX, B.JUMIN1 FROM (";
                        rtnVal = rtnVal + ComNum.VBLF + "  SELECT PANO, OUTDATE FROM ADMIN.MID_SUMMARY";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE TMODEL = '3'";
                        rtnVal = rtnVal + ComNum.VBLF + "  AND OUTDATE >= TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD')";
                        rtnVal = rtnVal + ComNum.VBLF + "  AND OUTDATE <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD')";
                        rtnVal = rtnVal + ComNum.VBLF + " ) A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                        rtnVal = rtnVal + ComNum.VBLF + "  WHERE A.PANO = B.PANO ";
                        rtnVal = rtnVal + ComNum.VBLF + " ORDER BY OUTDATE ";
                        break;
                    }
            }
            return rtnVal;
        }
    }
}
