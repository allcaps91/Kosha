using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmEveryStatByReason : Form
    {
        /// <summary>
        /// Class Name : frmEveryStatByReason
        /// File Name : frmEveryStatByReason.cs
        /// Title or Description : 전원사유별 통계(입원환자) 페이지
        /// Author : 박성완
        /// Create Date : 2017-06-01
        /// <history> 
        /// </history>
        /// </summary>
        public frmEveryStatByReason()
        {
            InitializeComponent();
        }

        private void frmEveryStatByReason_Load(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            //TODO: Call FormInfo_History(Me.Name, Me.Caption) 폼로딩 사용빈도 

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboYYMM.Items.Clear();
            SQL = "";
            SQL += ComNum.VBLF + "  SELECT TO_CHAR(ADD_MONTHS(SYSDATE, (-LEVEL + 1) * 12),'YYYY') YEAR";
            SQL += ComNum.VBLF + "    FROM DUAL";
            SQL += ComNum.VBLF + "  CONNECT BY LEVEL < 13";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboYYMM.Items.Add(dt.Rows[i]["YEAR"].ToString() + "년도");
                }
                dt.Dispose();
                dt = null;
                cboYYMM.SelectedIndex = 0;
                Cursor.Current = Cursors.Default;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
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

            string strSDate = "";
            string strEDate = "";
            double nTot = 0;
            int i = 0;
            int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                for (i = 2; i < ss1_Sheet1.Columns.Count; i++)
                {
                    for (j = 0; j < ss1_Sheet1.Rows.Count; j++)
                    {
                        ss1_Sheet1.Cells[j, i].Text = "";
                    }
                }

                strSDate = VB.Left(cboYYMM.Text, 4) + "-01-01";
                strEDate = VB.Left(cboYYMM.Text, 4) + "-12-31";

                SQL = "";
                SQL += ComNum.VBLF + "  SELECT  TO_CHAR(TDATE, 'MM') TDATE, SAYU1, SUM(1) CNT, '환자사유' GUBUN";
                SQL += ComNum.VBLF + "   FROM (";
                SQL += ComNum.VBLF + "   SELECT PANO, TDATE, SEX,";
                SQL += ComNum.VBLF + "       CASE";
                SQL += ComNum.VBLF + "          WHEN SAYUP1 = '1' THEN '연고지'";
                SQL += ComNum.VBLF + "          WHEN SAYUP2 = '1' THEN '3차 병원 진료 원해'";
                SQL += ComNum.VBLF + "          WHEN SAYUP3 = '1' THEN '기존 진료받던 병원'";
                SQL += ComNum.VBLF + "          WHEN SAYUP4 = '1' THEN '지정병원'";
                SQL += ComNum.VBLF + "          WHEN SAYUP5 = '1' THEN '지인(병원)'";
                SQL += ComNum.VBLF + "          WHEN SAYUPETC IS NOT NULL THEN '기타(환자사유)'";
                SQL += ComNum.VBLF + "      END SAYU1, '입원' GUBUN";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL += ComNum.VBLF + "  WHERE TDATE >= TO_DATE('" + strSDate + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "       AND TDATE <= TO_DATE('" + strEDate + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "       AND DELDATE IS NULL";
                SQL += ComNum.VBLF + "       )";
                SQL += ComNum.VBLF + "       Where SAYU1 Is Not Null";
                SQL += ComNum.VBLF + "       GROUP BY TO_CHAR(TDATE, 'MM'), SAYU1";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + " SELECT  TO_CHAR(TDATE, 'MM') TDATE, SAYU2 SAYU1, SUM(1) CNT, '병원사유' GUBUN";
                SQL += ComNum.VBLF + "   FROM (";
                SQL += ComNum.VBLF + "   SELECT PANO, TDATE, SEX,";
                SQL += ComNum.VBLF + "      CASE";
                SQL += ComNum.VBLF + "          WHEN SAYUH1 = '1' THEN '진료과 부재'";
                SQL += ComNum.VBLF + "          WHEN SAYUH2A = '1' OR SAYUH2B = '1' OR SAYUH2C = '1' OR SAYUH2D = '1' OR SAYUH2E = '1' OR SAYUH2F = '1' OR SAYUH2G = '1' OR SAYUH2ETC IS NOT NULL THEN '응급검사/처치불가'";
                SQL += ComNum.VBLF + "          WHEN SAYUH3 = '1' THEN '고위험환자로 3차병원 권유'";
                SQL += ComNum.VBLF + "          WHEN SAYUH4 = '1' THEN '응급수술 지연'";
                SQL += ComNum.VBLF + "          WHEN SAYUH5A = '1' OR SAYUH5B = '1' OR SAYUH5C = '1' OR SAYUH5ETC IS NOT NULL THEN '전문응급의료를 요하여'";
                SQL += ComNum.VBLF + "          WHEN SAYUH6 = '1'  THEN '병동, 병실부족'";
                SQL += ComNum.VBLF + "          WHEN SAYUH7 = '1'  THEN '중환자실 병동 부족'";
                SQL += ComNum.VBLF + "          WHEN SAYUH8 = '1' THEN '연고지 관계'";
                SQL += ComNum.VBLF + "          WHEN SAYUHETC IS NOT NULL THEN '기타(병원사유)'";
                SQL += ComNum.VBLF + "      END SAYU2, '입원' GUBUN";
                SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "NUR_QI_TRANSFOR";
                SQL += ComNum.VBLF + "  WHERE TDATE >= TO_DATE('" + strSDate + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "       AND TDATE <= TO_DATE('" + strEDate + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "       )";
                SQL += ComNum.VBLF + "       Where SAYU2 Is Not Null";
                SQL += ComNum.VBLF + "       GROUP BY TO_CHAR(TDATE, 'MM'), SAYU2";
                SQL += ComNum.VBLF + "  ORDER BY GUBUN DESC, TDATE";

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
                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (ss1_Sheet1.Cells[i, 1].Text.Trim() == dt.Rows[j]["SAYU1"].ToString().Trim())
                            {
                                break;
                            }
                        }

                        ss1_Sheet1.Cells[i, Convert.ToInt32(dt.Rows[j]["TDATE"]) + 1].Text = dt.Rows[j]["CNT"].ToString();
                    }
                }

                //행 갯수 만큼 합계
                for (int a = 0; a < ss1_Sheet1.Rows.Count; a++)
                {
                    nTot = 0;
                    //열들의 합
                    for (int b = 2; b < 14; b++)
                    {
                        nTot += VB.Val(ss1_Sheet1.Cells[a, b].Text);
                    }
                    ss1_Sheet1.Cells[a, 14].Text = nTot.ToString();
                    ss1_Sheet1.Cells[a, 15].Text = VB.Format(nTot / 12, "0.0");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
    }
}
