using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComDbB;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPoscoResList
    /// Description     : 
    /// Author          : 전상원
    /// Create Date     : 2018-04-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\OPD\oiguide\oiguide.vbp(FrmPoscoRes리스트) >> frmPmpaViewPoscoResList.cs 폼이름 재정의" />
    public partial class frmPmpaViewPoscoResList : Form
    {
        public frmPmpaViewPoscoResList()
        {
            InitializeComponent();
        }

        private void frmPmpaViewPoscoResList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ComFunc CF = new ComFunc();

            ssView_Sheet1.Columns[7].Visible = false;
            ssView_Sheet1.Columns[8].Visible = false;

            dtpFDate.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, strSysDate, -100));
            dtpTDate.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, strSysDate, 30));

            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int j = 0;
            int nRead = 0;
            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            DataTable dt1 = null;
            string SqlErr = "";

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd").Trim();
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd").Trim();

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT JDATE, PTNO, SNAME, DRNAME,";
                SQL = SQL + ComNum.VBLF + "RESULT_DATE, DRCODE, MCNO, PRT_GB, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI28";
                SQL = SQL + ComNum.VBLF + "  WHERE JDATE BETWEEN TO_DATE('" + strFDate + "', 'YYYY-MM-DD') AND TO_DATE('" + strTDate + "', 'YYYY-MM-DD')";
                if (rdoGUBUN_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND RESULT_DATE IS NOT NULL";
                }
                else if (rdoGUBUN_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND RESULT_DATE IS NULL";
                }
                SQL = SQL + ComNum.VBLF + "   GROUP BY JDATE, PTNO, SNAME, DRNAME, RESULT_DATE, DRCODE, MCNO, PRT_GB, ROWID";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRead = dt1.Rows.Count;

                if (dt1.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = 0;
                    ssView_Sheet1.RowCount = nRead;

                    for (j = 0; j < dt1.Rows.Count; j++)
                    {
                        ssView_Sheet1.Cells[j, 0].Text = Convert.ToDateTime(dt1.Rows[j]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssView_Sheet1.Cells[j, 1].Text = Posco_Exam_Date(Convert.ToDateTime(dt1.Rows[j]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd"), dt1.Rows[j]["PTNO"].ToString().Trim());
                        ssView_Sheet1.Cells[j, 2].Text = dt1.Rows[j]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[j, 3].Text = dt1.Rows[j]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[j, 4].Text = dt1.Rows[j]["DRNAME"].ToString().Trim();

                        if (VB.IsDate(dt1.Rows[j]["RESULT_DATE"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[j, 5].Text = Convert.ToDateTime(dt1.Rows[j]["RESULT_DATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            ssView_Sheet1.Cells[j, 5].Text = "";
                        }
                        
                        ssView_Sheet1.Cells[j, 6].Text = dt1.Rows[j]["PRT_GB"].ToString().Trim();
                        ssView_Sheet1.Cells[j, 7].Text = dt1.Rows[j]["DRCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[j, 8].Text = dt1.Rows[j]["ROWID"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;

            }
            catch (Exception ex)
            {
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "위탁검사 결과통보 리스트";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일 : " + strSysDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
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
                    rtnVal = Convert.ToDateTime(dt2.Rows[0]["MIN_EXAMRES"].ToString().Trim()).ToString("yyyy-MM-dd");
                    ;
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int Row = 0;
            string strROWID = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            strROWID = ssView_Sheet1.Cells[e.Row, 8].Text;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT PTNO, MCNO, JDATE, SEX, SABUN, SNAME, JUMIN1, JUMIN3, BUSE, JUSO, GUBUN, SABUN,";
                SQL = SQL + ComNum.VBLF + "     EXAM01_01, EXAM01_02, EXAM01_03, EXAM01_04, EXAM01_05,";
                SQL = SQL + ComNum.VBLF + "     EXAM01_06, EXAM01_07, EXAM01_08, EXAM01_09, EXAM01_10, EXAM01_REMARK,";
                SQL = SQL + ComNum.VBLF + "     EXAM02_01, EXAM02_02, EXAM02_03, EXAM02_04, EXAM02_05,";
                SQL = SQL + ComNum.VBLF + "     EXAM02_06, EXAM02_07, EXAM02_08,EXAM02_09,EXAM02_10,EXAM02_11,EXAM02_REMARK,";
                SQL = SQL + ComNum.VBLF + "     EXAM03_01, EXAM03_02, EXAM03_03, EXAM03_04, EXAM03_05, EXAM03_REMARK,";
                SQL = SQL + ComNum.VBLF + "     CHK_EXAM02_01, CHK_EXAM02_02, CHK_EXAM02_03, CHK_EXAM02_04,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(EXAM_DATE,'YYYY-MM-DD') EXAM_DATE, TO_CHAR(RESULT_DATE,'YYYY-MM-DD') RESULT_DATE, ";
                SQL = SQL + ComNum.VBLF + "     DRNAME, HOSPITAL, DRCODE, LICENSE, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI28";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssTab1.SelectedIndex = (int)VB.Val(dt.Rows[0]["GUBUN"].ToString().Trim() != "" ? dt.Rows[0]["GUBUN"].ToString().Trim() : "0");

                    //위|대장|복부초음파|폐
                    ssView1_Sheet1.Cells[1, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView1_Sheet1.Cells[1, 3].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim(), "0"), 1) + "******";
                    ssView1_Sheet1.Cells[1, 5].Text = dt.Rows[0]["PTNO"].ToString().Trim();

                    ssView1_Sheet1.Cells[2, 1].Text = dt.Rows[0]["SABUN"].ToString().Trim();
                    ssView1_Sheet1.Cells[2, 3].Text = dt.Rows[0]["BUSE"].ToString().Trim();

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

                    //CT | 초음파
                    ssView2_Sheet1.Cells[1, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[1, 3].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim(), "0"), 1) + "******";
                    ssView2_Sheet1.Cells[1, 5].Text = dt.Rows[0]["PTNO"].ToString().Trim();

                    ssView2_Sheet1.Cells[2, 1].Text = dt.Rows[0]["SABUN"].ToString().Trim();
                    ssView2_Sheet1.Cells[2, 3].Text = dt.Rows[0]["BUSE"].ToString().Trim();

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
                        ssView2_Sheet1.Cells[9, 3].Text = dt.Rows[0]["EXAM02_03"].ToString().Trim();
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[10, 3].Text = dt.Rows[0]["EXAM02_03"].ToString().Trim();
                    }

                    if (dt.Rows[0]["CHK_EXAM02_04"].ToString().Trim() == "1")
                    {
                        ssView2_Sheet1.Cells[11, 3].Text = dt.Rows[0]["EXAM02_04"].ToString().Trim();
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[12, 3].Text = dt.Rows[0]["EXAM02_04"].ToString().Trim();
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
                    ssView3_Sheet1.Cells[1, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView3_Sheet1.Cells[1, 3].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim(), "0"), 1) + "******";
                    ssView3_Sheet1.Cells[1, 5].Text = dt.Rows[0]["PTNO"].ToString().Trim();

                    ssView3_Sheet1.Cells[2, 1].Text = dt.Rows[0]["SABUN"].ToString().Trim();
                    ssView3_Sheet1.Cells[2, 3].Text = dt.Rows[0]["BUSE"].ToString().Trim();

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


                txtDelRowid.Text = dt.Rows[0]["ROWID"].ToString().Trim();
                txtDelResultDate.Text = dt.Rows[0]["RESULT_DATE"].ToString().Trim();

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

        private void btnPrt1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(ssView1, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
        }

        private void btnPrt2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
        }

        private void btnPrt3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(ssView3, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            if (VB.Trim(txtDelRowid.Text) == "")
            {
                ComFunc.MsgBox("통보서가 선택되지 않았습니다." + ComNum.VBLF + ComNum.VBLF + "삭제할 통보서를 선택하여 주십시요.");
                return;
            }
            else if (VB.Trim(txtDelResultDate.Text) != "")
            {
                ComFunc.MsgBox("해당통보서는 이미 완료되었습니다." + ComNum.VBLF + ComNum.VBLF + "미완료된 통보서를 선택하여 주십시요.");
                return;
            }

            if (ComFunc.MsgBoxQ("해당 통보서를 삭제하시겠습니까?.", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
                        
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO KOSMOS_OCS.OCS_MCCERTIFI28_HISTORY ";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_OCS.OCS_MCCERTIFI28";
                SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + VB.Trim(txtDelRowid.Text) + "' ";
                //'SQL = SQL & "   WHERE MCNO = '" & Format(Trim(pnlMcNo.Caption), "00000000") & "' "
                
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                SQL = " DELETE KOSMOS_OCS.OCS_MCCERTIFI28 ";
                SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + VB.Trim(txtDelRowid.Text) + "' ";
                //'SQL = SQL & "   WHERE MCNO = '" & Format(Trim(pnlMcNo.Caption), "00000000") & "' "

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;

                txtDelRowid.Text = "";
                txtDelResultDate.Text = "";        
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
