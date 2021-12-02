using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComPmpaLibB
{
    /// <summary> 
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaPrintPosco
    /// Description     : 
    /// Author          : 전상원
    /// Create Date     : 2018-04-09
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO: 전역변수(clsPrint)
    /// </history>
    /// <seealso cref= "\OPD\oiguide\oiguide.vbp(FrmPoscoRes출력) >> frmPmpaPrintPosco.cs 폼이름 재정의" />
    public partial class frmPmpaPrintPosco : Form
    {
        public frmPmpaPrintPosco()
        {
            InitializeComponent();
        }

        private void frmPmpaPrintPosco_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int j = 0;
            int k = 0;
            int q = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            int nREAD3 = 0;
            int nREAD4 = 0;

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SqlErr = "";

            ssView11_Sheet1.Columns[2].Visible = false;
            ssView11_Sheet1.Columns[2].Visible = false;
            ssView22_Sheet1.Columns[2].Visible = false;
            ssView22_Sheet1.Columns[2].Visible = false;
            ssView33_Sheet1.Columns[2].Visible = false;
            ssView33_Sheet1.Columns[2].Visible = false;
            ssView99_Sheet1.Columns[2].Visible = false;

            //TODO: 전역변수(clsPrint)
            ssView1_Sheet1.Cells[1, 1].Text = clsPrint.GstrPoscoPrtName;
            ssView1_Sheet1.Cells[1, 3].Text = VB.Left(clsPrint.GstrPoscoPrtJumin, 8) + "******";
            ssView1_Sheet1.Cells[1, 5].Text = clsPrint.GstrPoscoPrtPtno;
            ssView1_Sheet1.Cells[2, 1].Text = clsPrint.GstrPoscoPrtJikbun;
            ssView1_Sheet1.Cells[2, 3].Text = clsPrint.GstrPoscoPrtBuse;

            ssView2_Sheet1.Cells[1, 1].Text = clsPrint.GstrPoscoPrtName;
            ssView2_Sheet1.Cells[1, 3].Text = VB.Left(clsPrint.GstrPoscoPrtJumin, 8) + "******";
            ssView2_Sheet1.Cells[1, 5].Text = clsPrint.GstrPoscoPrtPtno;
            ssView2_Sheet1.Cells[2, 1].Text = clsPrint.GstrPoscoPrtJikbun;
            ssView2_Sheet1.Cells[2, 3].Text = clsPrint.GstrPoscoPrtBuse;

            ssView3_Sheet1.Cells[1, 1].Text = clsPrint.GstrPoscoPrtName;
            ssView3_Sheet1.Cells[1, 3].Text = VB.Left(clsPrint.GstrPoscoPrtJumin, 8) + "******";
            ssView3_Sheet1.Cells[1, 5].Text = clsPrint.GstrPoscoPrtPtno;
            ssView3_Sheet1.Cells[2, 1].Text = clsPrint.GstrPoscoPrtJikbun;
            ssView3_Sheet1.Cells[2, 3].Text = clsPrint.GstrPoscoPrtBuse;

            //접수일
            dtpJDate.Text = clsPrint.GstrPoscoPrtJDate;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //의사설정(위|대장|복부초음파|폐)
                SQL = "";
                SQL = "SELECT B.SABUN BSABUN, B.DRNAME BDRNAME, B.DRCODE BDRCODE, A.MYEN_BUNHO AMYEN_BUNHO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_MED + "OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + "  AND A.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "  AND a.SABUN IN (select sabun from " + ComNum.DB_PMPA + "bas_patient_posco_dr_list where gubun = '포스코통보서_검사_의사설정_0')";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nREAD = dt1.Rows.Count;

                ssView11_Sheet1.RowCount = 0;
                ssView11_Sheet1.RowCount = nREAD;
                if (dt1.Rows.Count > 0)
                {                    
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        ssView11_Sheet1.Cells[i, 0].Text = dt1.Rows[i]["BSABUN"].ToString().Trim();
                        ssView11_Sheet1.Cells[i, 1].Text = dt1.Rows[i]["BDRNAME"].ToString().Trim();
                        ssView11_Sheet1.Cells[i, 2].Text = dt1.Rows[i]["BDRCODE"].ToString().Trim();
                        ssView11_Sheet1.Cells[i, 3].Text = dt1.Rows[i]["AMYEN_BUNHO"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;

                //의사설정(CT|초음파)
                SQL = "SELECT B.SABUN BSABUN, B.DRNAME BDRNAME, B.DRCODE BDRCODE, A.MYEN_BUNHO AMYEN_BUNHO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_MED + "OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + "  AND A.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "  AND a.SABUN IN (select sabun from " + ComNum.DB_PMPA + "bas_patient_posco_dr_list where gubun = '포스코통보서_검사_의사설정_1')";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nREAD2 = dt2.Rows.Count;

                ssView22_Sheet1.RowCount = 0;
                ssView22_Sheet1.RowCount = nREAD2;
                if (dt2.Rows.Count > 0)
                {                    
                    for (j = 0; j < dt2.Rows.Count; j++)
                    {
                        ssView22_Sheet1.Cells[j, 0].Text = dt2.Rows[j]["BSABUN"].ToString().Trim();
                        ssView22_Sheet1.Cells[j, 1].Text = dt2.Rows[j]["BDRNAME"].ToString().Trim();
                        ssView22_Sheet1.Cells[j, 2].Text = dt2.Rows[j]["BDRCODE"].ToString().Trim();
                        ssView22_Sheet1.Cells[j, 3].Text = dt2.Rows[j]["AMYEN_BUNHO"].ToString().Trim();
                    }
                }

                dt2.Dispose();
                dt2 = null;

                //의사설정(여성검진)
                SQL = "SELECT B.SABUN BSABUN, B.DRNAME BDRNAME, B.DRCODE BDRCODE, A.MYEN_BUNHO AMYEN_BUNHO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_MED + "OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + "  AND A.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "  AND a.SABUN IN (select sabun from " + ComNum.DB_PMPA + "bas_patient_posco_dr_list where gubun = '포스코통보서_검사_의사설정_2')";

                SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nREAD3 = dt3.Rows.Count;

                ssView33_Sheet1.RowCount = 0;
                ssView33_Sheet1.RowCount = nREAD3;
                if (dt3.Rows.Count > 0)
                {                    
                    for (k = 0; k < dt3.Rows.Count; k++)
                    {
                        ssView33_Sheet1.Cells[k, 0].Text = dt3.Rows[k]["BSABUN"].ToString().Trim();
                        ssView33_Sheet1.Cells[k, 1].Text = dt3.Rows[k]["BDRNAME"].ToString().Trim();
                        ssView33_Sheet1.Cells[k, 2].Text = dt3.Rows[k]["BDRCODE"].ToString().Trim();
                        ssView33_Sheet1.Cells[k, 3].Text = dt3.Rows[k]["AMYEN_BUNHO"].ToString().Trim();
                    }
                }

                dt3.Dispose();
                dt3 = null;

                //완료목록
                SQL = "SELECT TO_CHAR(RESULT_DATE,'YYYY-MM-DD') AS RESULT_DATE, DRNAME, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI28";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + clsPrint.GstrPoscoPrtPtno + "'";
                SQL = SQL + ComNum.VBLF + "  AND JDATE = TO_DATE('" + clsPrint.GstrPoscoPrtJDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND RESULT_DATE IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "  AND PRT_GB IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nREAD4 = dt.Rows.Count;

                ssView99_Sheet1.RowCount = 0;
                ssView99_Sheet1.RowCount = nREAD4;
                if (dt.Rows.Count > 0)
                {                    
                    for (q = 0; q < dt.Rows.Count; q++)
                    {
                        ssView99_Sheet1.Cells[q, 0].Text = dt.Rows[q]["RESULT_DATE"].ToString().Trim();
                        ssView99_Sheet1.Cells[q, 1].Text = dt.Rows[q]["DRNAME"].ToString().Trim();
                        ssView99_Sheet1.Cells[q, 2].Text = dt.Rows[q]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                dtpFTDate.Text = Posco_Exam_Date(clsPrint.GstrPoscoPrtJDate, clsPrint.GstrPoscoPrtPtno);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDrSetting_Click(object sender, EventArgs e)
        {            
            frmPmpaSetPoscoDr frmPmpaSetPoscoDrX = new frmPmpaSetPoscoDr();
            frmPmpaSetPoscoDrX.StartPosition = FormStartPosition.CenterParent;
            frmPmpaSetPoscoDrX.ShowDialog();
        }

        private void btnList_Click(object sender, EventArgs e)
        {            
            frmPmpaViewPoscoResList frm = new frmPmpaViewPoscoResList();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string Posco_Exam_Date(string ArgJDate, string ArgPano)
        {
            string nREAD_Posco_Row = "";
            string rtnVal = "";

            string SQL = "";
            DataTable dt2 = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //검사일 구하기(가장 빠른 검사일)
                SQL = "";
                SQL = "SELECT LEAST(EXAMRES1, EXAMRES2, EXAMRES3, EXAMRES4, EXAMRES6, EXAMRES7, EXAMRES8, EXAMRES9,";
                SQL = SQL + ComNum.VBLF + "EXAMRES10,EXAMRES11, EXAMRES12, EXAMRES13, EXAMRES14, EXAMRES15, EXAMRES16,EXAMRES17,EXAMRES18,EXAMRES19,EXAMRES20) MIN_EXAMRES";
                SQL = SQL + ComNum.VBLF + " FROM (SELECT (CASE WHEN EXAMRES1 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES1 END) EXAMRES1,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES2 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES2 END) EXAMRES2,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES3 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES3 END) EXAMRES3,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES4 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES4 END) EXAMRES4,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES6 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES6 END) EXAMRES6,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES7 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES7 END) EXAMRES7,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES8 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES8 END) EXAMRES8,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES9 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES9 END) EXAMRES9,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES10 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES10 END) EXAMRES10,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES11 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES11 END) EXAMRES11,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES12 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES12 END) EXAMRES12,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES13 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES13 END) EXAMRES13,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES14 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES14 END) EXAMRES14,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES15 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES15 END) EXAMRES15,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES16 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES16 END) EXAMRES16,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES17 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES17 END) EXAMRES17,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES18 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES18 END) EXAMRES18,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES19 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES19 END) EXAMRES19,";
                SQL = SQL + ComNum.VBLF + "       (CASE WHEN EXAMRES20 IS NULL THEN TO_DATE('9999-11-11 11:11', 'YYYY-MM-DD HH24:MI')ELSE EXAMRES20 END) EXAMRES20";
                SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO";
                SQL = SQL + ComNum.VBLF + "          WHERE PANO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "          AND JDATE = TO_DATE('" + ArgJDate + "', 'YYYY-MM-DD'))";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                nREAD_Posco_Row = dt2.Rows.Count.ToString();

                if (dt2.Rows.Count > 0)
                {
                    if (VB.IsDate(dt2.Rows[0]["MIN_EXAMRES"].ToString().Trim()) == true)
                    {
                        rtnVal = Convert.ToDateTime(dt2.Rows[0]["MIN_EXAMRES"].ToString().Trim()).ToString("yyyy-MM-dd");
                    }                    
                }

                dt2.Dispose();
                dt2 = null;
                return rtnVal;

            }
            catch (Exception ex)
            {
                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void ssView11_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {            
            string strJDate = "";
            string strDrName = "";
            string strDrCode = "";
            string strLicense = "";
            string strSabun = "";
            string strSname = "";
            string strSex = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strBuse = "";
            string strJuso = "";
            string strMCCLASS = "";
            string strMCNO = "";
            string strHospital = "";
            string strExamDate = "";
            string strGubun = "";
            string strZipJuso = "";
            string strZipCode1 = "";
            string strZipCode2 = "";
            string[] strExam01 = new string[11];

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;

            ComQuery CQ = new ComQuery();

            //선택된 탭
            strGubun = ssTab1.SelectedIndex.ToString();

            strExam01[1] = chkExam01_0.Checked == true ? "작성바랍니다." : "";
            strExam01[2] = chkExam01_1.Checked == true ? "작성바랍니다." : "";
            strExam01[3] = chkExam01_2.Checked == true ? "작성바랍니다." : "";
            strExam01[4] = chkExam01_3.Checked == true ? "작성바랍니다." : "";
            strExam01[5] = chkExam01_4.Checked == true ? "작성바랍니다." : "";
            strExam01[6] = chkExam01_5.Checked == true ? "작성바랍니다." : "";
            strExam01[7] = chkExam01_6.Checked == true ? "작성바랍니다." : "";
            strExam01[8] = chkExam01_7.Checked == true ? "작성바랍니다." : "";
            strExam01[9] = chkExam01_8.Checked == true ? "작성바랍니다." : "";
            strExam01[10] = chkExam01_9.Checked == true ? "작성바랍니다." : "";

            strJDate = dtpJDate.Text.Trim();
            strExamDate = dtpFTDate.Text.Trim();

            if (clsPrint.GstrPoscoPrtPtno == "")
            {
                ComFunc.MsgBox("선택된 대상자가 없습니다.");
                return;
            }
            else if (strExamDate == "")
            {
                ComFunc.MsgBox("검사일을 입력해주세요.");
                dtpFTDate.Focus();
                return;
            }

            strSabun = ssView11_Sheet1.Cells[e.Row, 0].Text; //사번
            strDrName = ssView11_Sheet1.Cells[e.Row, 1].Text; //의사명
            strDrCode = ssView11_Sheet1.Cells[e.Row, 2].Text; //의사코드
            strLicense = ssView11_Sheet1.Cells[e.Row, 3].Text; //면허번호

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT PANO, JDATE, SEX, SABUN, SNAME, JUMIN1, JUMIN3, BUSE, JUSO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + clsPrint.GstrPoscoPrtPtno + "'";
                SQL = SQL + ComNum.VBLF + "  AND JDATE = TO_DATE('" + strJDate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT ZIPCODE1, ZIPCODE2 From " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " Where Pano ='" + clsPrint.GstrPoscoPrtPtno + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    strZipCode1 = dt1.Rows[0]["ZIPCODE1"].ToString().Trim();
                    strZipCode2 = dt1.Rows[0]["ZIPCODE2"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                strZipJuso = CQ.Read_Juso(clsDB.DbCon, strZipCode1);
                strZipJuso = CQ.Read_Juso(clsDB.DbCon, strZipCode2);

                strSex = dt.Rows[0]["SEX"].ToString().Trim();
                strSabun = dt.Rows[0]["SABUN"].ToString().Trim();
                strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                strBuse = dt.Rows[0]["BUSE"].ToString().Trim();
                strJuso = strZipJuso + " " + dt.Rows[0]["JUSO"].ToString().Trim();
                strMCCLASS = "28";
                strMCNO = "";
                strHospital = "포항성모병원";

                dt.Dispose();
                dt = null;

                CQ = null;

                clsDB.setBeginTran(clsDB.DbCon);

                if (strMCNO == "")
                {
                    strMCNO = GetMcNo();

                    if (strMCNO == "-1")
                    {
                        return;
                    }

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_MCCERTIFI28(";
                    SQL = SQL + ComNum.VBLF + "MCCLASS, PTNO, MCNO,";
                    SQL = SQL + ComNum.VBLF + "JUMIN1, JUMIN3, SEX, SNAME, JUSO,";
                    SQL = SQL + ComNum.VBLF + "JDATE, SABUN, BUSE,";
                    SQL = SQL + ComNum.VBLF + "HOSPITAL, EXAM_DATE,";
                    SQL = SQL + ComNum.VBLF + "DRNAME, GUBUN,";
                    SQL = SQL + ComNum.VBLF + "EXAM01_01, EXAM01_02, EXAM01_03, EXAM01_04, EXAM01_05,";
                    SQL = SQL + ComNum.VBLF + "EXAM01_06 , EXAM01_07, EXAM01_08, EXAM01_09, EXAM01_10,";
                    SQL = SQL + ComNum.VBLF + "DRCODE, LICENSE) VALUES(";
                    SQL = SQL + ComNum.VBLF + "'" + strMCCLASS + "', '" + clsPrint.GstrPoscoPrtPtno + "', '" + strMCNO + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strJumin1 + "', '" + clsAES.AES(strJumin2) + "', '" + strSex + "', '" + strSname + "', '" + strJuso + "',";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + strJDate + "', 'YYYY-MM-DD'), '" + strSabun + "', '" + strBuse + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strHospital + "', TO_DATE('" + strExamDate + "', 'YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "'" + strDrName + "', '" + strGubun + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strExam01[1] + "', '" + strExam01[2] + "', '" + strExam01[3] + "', '" + strExam01[4] + "', '" + strExam01[5] + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strExam01[6] + "', '" + strExam01[7] + "', '" + strExam01[8] + "', '" + strExam01[9] + "', '" + strExam01[10] + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strDrCode + "', '" + strLicense + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("저장 중 에러 7발생");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("전송 완료");
                    Cursor.Current = Cursors.Default;
                }
            }

            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView22_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int Row = 0;
            string strJDate = "";
            string strDrName = "";
            string strDrCode = "";
            string strLicense = "";
            string strSabun = "";
            string strSname = "";
            string strSex = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strBuse = "";
            string strJuso = "";
            string strMCCLASS = "";
            string strMCNO = "";
            string strHospital = "";
            string strExamDate = "";
            string strGubun = "";
            string strZipJuso = "";
            string strZipCode1 = "";
            string strZipCode2 = "";
            string[] strExam02 = new string[15];
            string[] strExam02_1 = new string[5];

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;

            ComQuery CQ = new ComQuery();

            //선택된 탭
            strGubun = ssTab1.SelectedIndex.ToString();

            //검사항목 전송시 멘트전송
            strExam02[1] = chkExam02_0.Checked == true ? "작성바랍니다." : "";
            strExam02[2] = chkExam02_1.Checked == true ? "작성바랍니다." : "";
            strExam02[3] = chkExam02_2.Checked == true ? "작성바랍니다." : "";
            strExam02[4] = chkExam02_3.Checked == true ? "작성바랍니다." : "";
            strExam02[5] = chkExam02_4.Checked == true ? "작성바랍니다." : "";
            strExam02[6] = chkExam02_5.Checked == true ? "작성바랍니다." : "";
            strExam02[7] = chkExam02_6.Checked == true ? "작성바랍니다." : "";
            strExam02[8] = chkExam02_7.Checked == true ? "작성바랍니다." : "";
            strExam02[9] = chkExam02_8.Checked == true ? "작성바랍니다." : "";
            strExam02[10] = chkExam02_9.Checked == true ? "작성바랍니다." : "";
            strExam02[11] = chkExam02_10.Checked == true ? "작성바랍니다." : "";

            strExam02_1[1] = chkExam02_0_0.Checked == true ? "1" : "0";
            strExam02_1[2] = chkExam02_1_1.Checked == true ? "1" : "0";
            strExam02_1[3] = chkExam02_2_1.Checked == true ? "1" : "0";
            strExam02_1[4] = chkExam02_3_1.Checked == true ? "1" : "0";

            strJDate = dtpJDate.Text.Trim();
            strExamDate = dtpFTDate.Text.Trim();

            if (clsPrint.GstrPoscoPrtPtno == "")
            {
                ComFunc.MsgBox("선택된 대상자가 없습니다.");
                return;
            }
            else if (strExamDate == "")
            {
                ComFunc.MsgBox("검사일을 입력해주세요.");
                dtpFTDate.Focus();
                return;
            }

            strSabun = ssView22_Sheet1.Cells[e.Row, 0].Text; //사번
            strDrName = ssView22_Sheet1.Cells[e.Row, 1].Text; //의사명
            strDrCode = ssView22_Sheet1.Cells[e.Row, 2].Text; //의사코드
            strLicense = ssView22_Sheet1.Cells[e.Row, 3].Text; // 면허번호

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT PANO, JDATE, SEX, SABUN, SNAME, JUMIN1, JUMIN3, BUSE, JUSO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + clsPrint.GstrPoscoPrtPtno + "'";
                SQL = SQL + ComNum.VBLF + "  AND JDATE = TO_DATE('" + strJDate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT ZIPCODE1, ZIPCODE2 From " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " Where Pano ='" + clsPrint.GstrPoscoPrtPtno + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    strZipCode1 = dt1.Rows[0]["ZipCode1"].ToString().Trim();
                    strZipCode2 = dt1.Rows[0]["ZipCode2"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                if (dt.Rows.Count > 0)
                {
                    strZipJuso = CQ.Read_Juso(clsDB.DbCon, strZipCode1);
                    strZipJuso = CQ.Read_Juso(clsDB.DbCon, strZipCode2);

                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    strSabun = dt.Rows[0]["SABUN"].ToString().Trim();
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    strBuse = dt.Rows[0]["BUSE"].ToString().Trim();
                    strJuso = strZipJuso + " " + dt.Rows[0]["JUSO"].ToString().Trim();
                    strMCCLASS = "28";
                    strMCNO = "";
                    strHospital = "포항성모병원";
                }

                dt.Dispose();
                dt = null;

                CQ = null;

                clsDB.setBeginTran(clsDB.DbCon);

                if (strMCNO == "")
                {
                    strMCNO = GetMcNo();
                    if (strMCNO == "-1")
                    {
                        return;
                    }

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_MCCERTIFI28(";
                    SQL = SQL + ComNum.VBLF + "MCCLASS, PTNO, MCNO,";
                    SQL = SQL + ComNum.VBLF + "JUMIN1, JUMIN3, SEX, SNAME, JUSO,";
                    SQL = SQL + ComNum.VBLF + "JDATE, SABUN, BUSE,";
                    SQL = SQL + ComNum.VBLF + "HOSPITAL, EXAM_DATE,";
                    SQL = SQL + ComNum.VBLF + "DRNAME, GUBUN,";
                    SQL = SQL + ComNum.VBLF + "EXAM02_01, EXAM02_02, EXAM02_03, EXAM02_04,";
                    SQL = SQL + ComNum.VBLF + "EXAM02_05, EXAM02_06, EXAM02_07,EXAM02_08,EXAM02_09,EXAM02_10,EXAM02_11,";
                    SQL = SQL + ComNum.VBLF + "CHK_EXAM02_01, CHK_EXAM02_02, CHK_EXAM02_03, CHK_EXAM02_04,";
                    SQL = SQL + ComNum.VBLF + "DRCODE, LICENSE) VALUES(";
                    SQL = SQL + ComNum.VBLF + "'" + strMCCLASS + "', '" + clsPrint.GstrPoscoPrtPtno + "', '" + strMCNO + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strJumin1 + "', '" + clsAES.AES(strJumin2) + "', '" + strSex + "', '" + strSname + "', '" + strJuso + "',";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + strJDate + "', 'YYYY-MM-DD'), '" + strSabun + "', '" + strBuse + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strHospital + "', TO_DATE('" + strExamDate + "', 'YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "'" + strDrName + "', '" + strGubun + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strExam02[1] + "', '" + strExam02[2] + "', '" + strExam02[3] + "', '" + strExam02[4] + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strExam02[5] + "', '" + strExam02[6] + "', '" + strExam02[7] + "', '" + strExam02[8] + "', '" + strExam02[9] + "', '" + strExam02[10] + "', '" + strExam02[11] + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strExam02_1[1] + "', '" + strExam02_1[2] + "', '" + strExam02_1[3] + "', '" + strExam02_1[4] + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strDrCode + "', '" + strLicense + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("저장 중 에러 7 발생");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("전송 완료");
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView33_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int Row = 0;
            string strJDate = "";
            string strDrName = "";
            string strDrCode = "";
            string strLicense = "";
            string strSabun = "";
            string strSname = "";
            string strSex = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strBuse = "";
            string strJuso = "";
            string strMCCLASS = "";
            string strMCNO = "";
            string strHospital = "";
            string strExamDate = "";
            string strGubun;
            string strZipJuso = "";
            string strZipCode1 = "";
            string strZipCode2 = "";
            string[] strExam03 = new string[11];

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;

            ComQuery CQ = new ComQuery();

            //선택된 탭
            strGubun = ssTab1.SelectedIndex.ToString();

            strExam03[1] = chkExam03_0.Checked == true ? "작성바랍니다." : "";
            strExam03[2] = chkExam03_1.Checked == true ? "작성바랍니다." : "";
            strExam03[3] = chkExam03_2.Checked == true ? "작성바랍니다." : "";
            strExam03[4] = chkExam03_3.Checked == true ? "작성바랍니다." : "";
            strExam03[5] = chkExam03_4.Checked == true ? "작성바랍니다." : "";

            strJDate = dtpJDate.Text.Trim();
            strExamDate = dtpFTDate.Text.Trim();

            if (clsPrint.GstrPoscoPrtPtno == "")
            {
                ComFunc.MsgBox("선택된 대상자가 없습니다.");
                return;
            }
            else if (strExamDate == "")
            {
                ComFunc.MsgBox("검사일을 입력해주세요.");
                return;
            }

            strSabun = ssView33_Sheet1.Cells[e.Row, 0].Text; //사번
            strDrName = ssView33_Sheet1.Cells[e.Row, 1].Text; //의사명
            strDrCode = ssView33_Sheet1.Cells[e.Row, 2].Text; //의사코드
            strLicense = ssView33_Sheet1.Cells[e.Row, 3].Text; //면허번호

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT PANO, JDATE, SEX, SABUN, SNAME, JUMIN1, JUMIN3, BUSE, JUSO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + clsPrint.GstrPoscoPrtPtno + "'";
                SQL = SQL + ComNum.VBLF + "  AND JDATE = TO_DATE('" + strJDate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT ZIPCODE1, ZIPCODE2 From " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " Where Pano ='" + clsPrint.GstrPoscoPrtPtno + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    strZipCode1 = dt1.Rows[0]["ZIPCODE1"].ToString().Trim();
                    strZipCode2 = dt1.Rows[0]["ZIPCODE2"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                if (dt.Rows.Count > 0)
                {
                    strZipJuso = CQ.Read_Juso(clsDB.DbCon, strZipCode1);
                    strZipJuso = CQ.Read_Juso(clsDB.DbCon, strZipCode2);

                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    strSabun = dt.Rows[0]["SABUN"].ToString().Trim();
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    strBuse = dt.Rows[0]["BUSE"].ToString().Trim();
                    strJuso = strZipJuso + " " + dt.Rows[0]["JUSO"].ToString().Trim();
                    strMCCLASS = "28";
                    strMCNO = "";
                    strHospital = "포항성모병원";
                }

                dt.Dispose();
                dt = null;

                CQ = null;

                clsDB.setBeginTran(clsDB.DbCon);

                if (strMCNO == "")
                {
                    strMCNO = GetMcNo();

                    if (strMCNO == "-1")
                    {
                        return;
                    }

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_MCCERTIFI28(";
                    SQL = SQL + ComNum.VBLF + "MCCLASS, PTNO, MCNO,";
                    SQL = SQL + ComNum.VBLF + "JUMIN1, JUMIN3, SEX, SNAME, JUSO,";
                    SQL = SQL + ComNum.VBLF + "JDATE, SABUN, BUSE,";
                    SQL = SQL + ComNum.VBLF + "HOSPITAL, EXAM_DATE,";
                    SQL = SQL + ComNum.VBLF + "DRNAME, GUBUN,";
                    SQL = SQL + ComNum.VBLF + "EXAM03_01, EXAM03_02, EXAM03_03,";
                    SQL = SQL + ComNum.VBLF + "EXAM03_04, EXAM03_05,";
                    SQL = SQL + ComNum.VBLF + "DRCODE, LICENSE) VALUES(";
                    SQL = SQL + ComNum.VBLF + "'" + strMCCLASS + "', '" + clsPrint.GstrPoscoPrtPtno + "', '" + strMCNO + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strJumin1 + "', '" + clsAES.AES(strJumin2) + "', '" + strSex + "', '" + strSname + "', '" + strJuso + "',";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + strJDate + "', 'YYYY-MM-DD'), '" + strSabun + "', '" + strBuse + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strHospital + "', TO_DATE('" + strExamDate + "', 'YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "'" + strDrName + "', '" + strGubun + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strExam03[1] + "', '" + strExam03[2] + "', '" + strExam03[3] + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strExam03[4] + "', '" + strExam03[5] + "',";
                    SQL = SQL + ComNum.VBLF + "'" + strDrCode + "', '" + strLicense + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("저장 중 에러 7발생");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("전송 완료");
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }


        }

        private void ssView99_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int Row = 0;
            int nRead = 0;
            string strROWID = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            strROWID = ssView99_Sheet1.Cells[e.Row, 2].Text;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT PTNO, MCNO, JDATE, SEX, SABUN, SNAME, JUMIN1, JUMIN3, BUSE, JUSO, GUBUN,";
                SQL = SQL + ComNum.VBLF + "     EXAM01_01, EXAM01_02, EXAM01_03, EXAM01_04, EXAM01_05,";
                SQL = SQL + ComNum.VBLF + "     EXAM01_06, EXAM01_07, EXAM01_08, EXAM01_09, EXAM01_10, EXAM01_REMARK,";
                SQL = SQL + ComNum.VBLF + "     EXAM02_01, EXAM02_02, EXAM02_03, EXAM02_04, EXAM02_05,";
                SQL = SQL + ComNum.VBLF + "     EXAM02_06, EXAM02_07, EXAM02_08,EXAM02_09,EXAM02_10,EXAM02_11,EXAM02_REMARK,";
                SQL = SQL + ComNum.VBLF + "     EXAM03_01, EXAM03_02, EXAM03_03, EXAM03_04, EXAM03_05, EXAM03_REMARK,";
                SQL = SQL + ComNum.VBLF + "     CHK_EXAM02_01, CHK_EXAM02_02, CHK_EXAM02_03, CHK_EXAM02_04,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(EXAM_DATE,'YYYY-MM-DD') EXAM_DATE, TO_CHAR(RESULT_DATE,'YYYY-MM-DD') RESULT_DATE, ";
                SQL = SQL + ComNum.VBLF + "     DRNAME, HOSPITAL, DRCODE, LICENSE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI28";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "'";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRead = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    ssTab1.SelectedIndex = (int)VB.Val(dt.Rows[0]["GUBUN"].ToString().Trim() != "" ? dt.Rows[0]["GUBUN"].ToString().Trim() : "0");

                    txtPANO.Text = dt.Rows[0]["PTNO"].ToString().Trim();
                    txtMCNO.Text = dt.Rows[0]["MCNO"].ToString().Trim();

                    //위|대장|복부초음파|폐
                    ssView1_Sheet1.Cells[5, 3].Text = dt.Rows[0]["EXAM01_01"].ToString().Trim();
                    ssView1_Sheet1.Cells[6, 3].Text = dt.Rows[0]["EXAM01_02"].ToString().Trim();
                    ssView1_Sheet1.Cells[7, 3].Text = dt.Rows[0]["EXAM01_03"].ToString().Trim();
                    ssView1_Sheet1.Cells[8, 3].Text = dt.Rows[0]["EXAM01_04"].ToString().Trim();
                    ssView1_Sheet1.Cells[9, 3].Text = dt.Rows[0]["EXAM01_05"].ToString().Trim();
                    ssView1_Sheet1.Cells[10, 3].Text = dt.Rows[0]["EXAM01_06"].ToString().Trim();
                    ssView1_Sheet1.Cells[11, 3].Text = dt.Rows[0]["EXAM01_07"].ToString().Trim();
                    ssView1_Sheet1.Cells[12, 3].Text = dt.Rows[0]["EXAM01_08"].ToString().Trim();
                    ssView1_Sheet1.Cells[13, 3].Text = dt.Rows[0]["EXAM01_09"].ToString().Trim();
                    ssView1_Sheet1.Cells[14, 3].Text = dt.Rows[0]["EXAM01_10"].ToString().Trim();

                    ssView1_Sheet1.Cells[16, 1].Text = dt.Rows[0]["EXAM01_REMARK"].ToString().Trim();

                    ssView1_Sheet1.Cells[17, 1].Text = dt.Rows[0]["EXAM_DATE"].ToString().Trim();
                    ssView1_Sheet1.Cells[18, 1].Text = dt.Rows[0]["DRNAME"].ToString().Trim();

                    ssView1_Sheet1.Cells[17, 4].Text = dt.Rows[0]["RESULT_DATE"].ToString().Trim();
                    ssView1_Sheet1.Cells[18, 4].Text = dt.Rows[0]["HOSPITAL"].ToString().Trim();

                    //CT|초음파
                    if (dt.Rows[0]["CHK_EXAM02_01"].ToString().Trim() == "1")
                    {
                        ssView2_Sheet1.Cells[5, 3].Text = dt.Rows[0]["EXAM02_01"].ToString().Trim();
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[6, 3].Text = dt.Rows[0]["EXAM02_01"].ToString().Trim();
                    }

                    if (dt.Rows[0]["CHK_EXAM02_02"].ToString().Trim() == "1")
                    {
                        ssView2_Sheet1.Cells[7, 3].Text = dt.Rows[0]["EXAM02_02"].ToString().Trim();
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[8, 3].Text = dt.Rows[0]["EXAM02_02"].ToString().Trim();
                    }

                    if (dt.Rows[0]["CHK_EXAM02_03"].ToString().Trim() == "1")
                    {
                        ssView2_Sheet1.Cells[7, 3].Text = dt.Rows[0]["EXAM02_03"].ToString().Trim();
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[8, 3].Text = dt.Rows[0]["EXAM02_03"].ToString().Trim();
                    }

                    if (dt.Rows[0]["CHK_EXAM02_04"].ToString().Trim() == "1")
                    {
                        ssView2_Sheet1.Cells[7, 3].Text = dt.Rows[0]["EXAM02_04"].ToString().Trim();
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[8, 3].Text = dt.Rows[0]["EXAM02_04"].ToString().Trim();
                    }

                    ssView2_Sheet1.Cells[13, 2].Text = dt.Rows[0]["EXAM02_08"].ToString().Trim();
                    ssView2_Sheet1.Cells[14, 2].Text = dt.Rows[0]["EXAM02_09"].ToString().Trim();
                    ssView2_Sheet1.Cells[15, 2].Text = dt.Rows[0]["EXAM02_10"].ToString().Trim();
                    ssView2_Sheet1.Cells[16, 2].Text = dt.Rows[0]["EXAM02_11"].ToString().Trim();


                    ssView2_Sheet1.Cells[17, 2].Text = dt.Rows[0]["EXAM02_05"].ToString().Trim();
                    ssView2_Sheet1.Cells[18, 2].Text = dt.Rows[0]["EXAM02_06"].ToString().Trim();
                    ssView2_Sheet1.Cells[19, 2].Text = dt.Rows[0]["EXAM02_07"].ToString().Trim();

                    ssView2_Sheet1.Cells[21, 1].Text = dt.Rows[0]["EXAM02_REMARK"].ToString().Trim();

                    ssView2_Sheet1.Cells[22, 1].Text = dt.Rows[0]["EXAM_DATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[23, 1].Text = dt.Rows[0]["DRNAME"].ToString().Trim();

                    ssView2_Sheet1.Cells[22, 4].Text = dt.Rows[0]["RESULT_DATE"].ToString().Trim();
                    ssView2_Sheet1.Cells[23, 4].Text = dt.Rows[0]["HOSPITAL"].ToString().Trim();

                    //여성검진(자궁검진|유방검진)
                    ssView3_Sheet1.Cells[5, 3].Text = dt.Rows[0]["EXAM03_01"].ToString().Trim();
                    ssView3_Sheet1.Cells[6, 3].Text = dt.Rows[0]["EXAM03_02"].ToString().Trim();
                    ssView3_Sheet1.Cells[7, 3].Text = dt.Rows[0]["EXAM03_03"].ToString().Trim();
                    ssView3_Sheet1.Cells[8, 3].Text = dt.Rows[0]["EXAM03_04"].ToString().Trim();
                    ssView3_Sheet1.Cells[9, 3].Text = dt.Rows[0]["EXAM03_05"].ToString().Trim();

                    ssView3_Sheet1.Cells[11, 1].Text = dt.Rows[0]["EXAM03_REMARK"].ToString().Trim();

                    ssView3_Sheet1.Cells[12, 1].Text = dt.Rows[0]["EXAM_DATE"].ToString().Trim();
                    ssView3_Sheet1.Cells[13, 1].Text = dt.Rows[0]["DRNAME"].ToString().Trim();

                    ssView3_Sheet1.Cells[12, 4].Text = dt.Rows[0]["RESULT_DATE"].ToString().Trim();
                    ssView3_Sheet1.Cells[13, 4].Text = dt.Rows[0]["HOSPITAL"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string GetMcNo()
        {
            string SQL = "";
            DataTable dt2 = null;
            string SqlErr = "";
            string rtnVal = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  " + ComNum.DB_MED + "SEQ_MCNO.NEXTVAL SEQ ";
                SQL = SQL + ComNum.VBLF + " FROM    DUAL ";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (SqlErr == "-1")
                {
                    rtnVal = "-1";
                    return rtnVal;
                }
                if (dt2.Rows.Count > 0)
                {
                    rtnVal = VB.Val(dt2.Rows[0]["SEQ"].ToString().Trim()).ToString("00000000");
                }

                dt2.Dispose();
                dt2 = null;
                return rtnVal;

            }
            catch (Exception ex)
            {
                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnPrt1_Click(object sender, EventArgs e)
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(ssView1, PrePrint, setMargin, setOption, strHeader, strFooter);
            ComFunc.Delay(200);


            CS = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_MCCERTIFI28 SET";
                SQL = SQL + ComNum.VBLF + " PRT_GB = 'Y'";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + txtPANO.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "  AND MCNO = '" + txtMCNO.Text.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("출력 중 에러 7발생");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("출력 완료");
                Cursor.Current = Cursors.Default;
                SCREEN_CLEAR();
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

        private void btnPrt2_Click(object sender, EventArgs e)
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);
            ComFunc.Delay(200);

            CS = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_MCCERTIFI28 SET";
                SQL = SQL + ComNum.VBLF + " PRT_GB = 'Y'";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + txtPANO.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "  AND MCNO = '" + txtMCNO.Text.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("출력 중 에러 7발생");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("출력 완료");
                Cursor.Current = Cursors.Default;
                SCREEN_CLEAR();
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

        private void btnPrt3_Click(object sender, EventArgs e)
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(ssView3, PrePrint, setMargin, setOption, strHeader, strFooter);
            ComFunc.Delay(200);

            CS = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_MCCERTIFI28 SET";
                SQL = SQL + ComNum.VBLF + " PRT_GB = 'Y'";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + txtPANO.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "  AND MCNO = '" + txtMCNO.Text.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("출력 중 에러 7발생");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("출력 완료");
                Cursor.Current = Cursors.Default;
                SCREEN_CLEAR();
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

        private void SCREEN_CLEAR()
        {
            clsPrint.GstrPoscoPrtName = "";
            clsPrint.GstrPoscoPrtJumin = "";
            clsPrint.GstrPoscoPrtPtno = "";
            clsPrint.GstrPoscoPrtJikbun = "";
            clsPrint.GstrPoscoPrtBuse = "";
            clsPrint.GstrPoscoPrtJDate = "";
        }

        private void ssView11_CellClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
