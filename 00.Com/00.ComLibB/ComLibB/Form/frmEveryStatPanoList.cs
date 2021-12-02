using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmEveryStatPanoList : Form
    {
        /// <summary>
        /// Class Name : frmEveryStatPanoList
        /// File Name : frmEveryStatPanoList.cs
        /// Title or Description : 전원환자 목록 페이지
        /// Author : 박성완
        /// Create Date : 2017-06-01
        /// <history> 
        /// </history>
        /// </summary>
        public frmEveryStatPanoList()
        {
            InitializeComponent();
        }

        private void frmEveryStatPanoList_Load(object sender, EventArgs e)
        {
            //TODO: Call FormInfo_History(Me.Name, Me.Caption) 폼로딩 사용빈도 
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboGubun.Items.Clear();
            cboGubun.Items.Add("전체");
            cboGubun.Items.Add("입원");
            cboGubun.Items.Add("외래");

            cboGubun.SelectedIndex = 0;

            //서버시간의 -100일 값
            dtpSDate.Text = DateTime.Parse(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-100).ToShortDateString();
            dtpEDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            if (ViewData() == false) return;
        }

        /// <summary>
        /// 조회 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        /// <returns></returns>
        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ss1_Sheet1.Rows.Count = 0;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SQL = "";
                SQL += ComNum.VBLF + " SELECT PANO, DEPTCODE, SEX, INTIME, OUTTIME, HOSPITAL, SAYU1, SAYU2, DISE1, DISE2, DISE3, GUBUN";
                SQL += ComNum.VBLF + " FROM (";
                SQL += ComNum.VBLF + "      SELECT PANO, DEPTCODE, SEX,";
                SQL += ComNum.VBLF + "       INTIME, OUTTIME, HUSONGNAME HOSPITAL, HUSONGSAYU SAYU1, '' SAYU2, DISEASE DISE1, '' DISE2, '' DISE3, 'ER' GUBUN";
                SQL += ComNum.VBLF + "      From " + ComNum.DB_PMPA + "NUR_ER_PATIENT";
                SQL += ComNum.VBLF + " WHERE OUTTIME >= TO_DATE('" + dtpSDate.Value.ToShortDateString() + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND OUTTIME <= TO_DATE('" + dtpEDate.Value.ToShortDateString() + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND OUTGBN = '6'";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + "  SELECT PANO, DEPTCODE, SEX, INDATE INTIME, TDATE OUTTIME, THOSNAME,";
                SQL += ComNum.VBLF + "      CASE";
                SQL += ComNum.VBLF + "         WHEN SAYUP1 = '1' THEN '연고지'";
                SQL += ComNum.VBLF + "         WHEN SAYUP2 = '1' THEN '3차 병원 진료 원해'";
                SQL += ComNum.VBLF + "         WHEN SAYUP3 = '1' THEN '기존 진료받던 병원'";
                SQL += ComNum.VBLF + "         WHEN SAYUP4 = '1' THEN '지정병원'";
                SQL += ComNum.VBLF + "         WHEN SAYUP5 = '1' THEN '지인(병원)'";
                SQL += ComNum.VBLF + "         WHEN SAYUPETC IS NOT NULL THEN '기타'";
                SQL += ComNum.VBLF + "     END SAYU1,";
                SQL += ComNum.VBLF + "     CASE";
                SQL += ComNum.VBLF + "         WHEN SAYUH1 = '1' THEN '진료과 부재'";
                SQL += ComNum.VBLF + "         WHEN SAYUH2A = '1' OR SAYUH2B = '1' OR SAYUH2C = '1' OR SAYUH2D = '1' OR SAYUH2E = '1' OR SAYUH2F = '1' OR SAYUH2G = '1' OR SAYUH2ETC IS NOT NULL THEN '응급검사/처치불가'";
                SQL += ComNum.VBLF + "         WHEN SAYUH3 = '1' THEN '고위험환자로 3차병원 권유'";
                SQL += ComNum.VBLF + "         WHEN SAYUH4 = '1' THEN '응급수술 지연'";
                SQL += ComNum.VBLF + "         WHEN SAYUH5A = '1' OR SAYUH5B = '1' OR SAYUH5C = '1' OR SAYUH5ETC IS NOT NULL THEN '전문응급의료를 요하여'";
                SQL += ComNum.VBLF + "         WHEN SAYUH6 = '1'  THEN '병동, 병실부족'";
                SQL += ComNum.VBLF + "         WHEN SAYUH7 = '1'  THEN '중환자실 병동 부족'";
                SQL += ComNum.VBLF + "         WHEN SAYUH8 = '1' THEN '연고지 관계'";
                SQL += ComNum.VBLF + "         WHEN SAYUHETC IS NOT NULL THEN '기타'";
                SQL += ComNum.VBLF + "     END SAYU2,";
                SQL += ComNum.VBLF + "  DIAG1, DIAG2, DIAG3, '입원' GUBUN";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL += ComNum.VBLF + " WHERE TDATE >= TO_DATE('" + dtpSDate.Value.ToShortDateString() + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      AND TDATE <= TO_DATE('" + dtpEDate.Value.ToShortDateString() + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "      )";
                if (cboGubun.Text == "입원")
                {
                    SQL += ComNum.VBLF + " WHERE GUBUN = '입원'";
                }
                else if (cboGubun.Text == "외래")
                {
                    SQL += ComNum.VBLF + " WHERE GUBUN = 'ER'";
                }

                SQL += ComNum.VBLF + " ORDER BY OUTTIME, PANO";

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

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = VB.Left(dt.Rows[i]["OUTTIME"].ToString().Trim(), 10);
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = VB.Left(dt.Rows[i]["INTIME"].ToString().Trim(), 10);
                    ss1_Sheet1.Cells[i, 4].Text = Read_PatientName(dt.Rows[i]["PANO"].ToString().Trim());
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = "";
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["HOSPITAL"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SAYU1"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["SAYU2"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DISE1"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["DISE2"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["DISE3"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

        }

        //TODO: VB READ_PatientName(vbfunc.bas 모듈)함수 임시로 만들어서 사용
        private string Read_PatientName(string argPano)
        {
            string SQL = "";
            string rtnVal = "";
            string SqlErr = "";
            DataTable dt = null;
            try
            {
                if (VB.Val(argPano) == 0)
                {
                    rtnVal = "";
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano='" + argPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        /// <summary>
        /// 닫기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
