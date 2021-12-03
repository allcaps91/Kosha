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
    public partial class frmPmpaViewPoscoReq : Form
    {
        public frmPmpaViewPoscoReq()
        {
            InitializeComponent();
        }

        private void frmPmpaViewPoscoReq_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ComFunc CF = new ComFunc();

            ssView_Sheet1.Columns[21].Visible = false;

            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, strSysDate, -60);
            dtpTDate.Text = strSysDate;
            btnPrint.Enabled = false;

            panMain.Visible = false;

            Screen_Clear2();

            CF = null;
        }

        private void GetData()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            int nRow = 0;
            int nRead = 0;
            string strOK = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComFunc CF = new ComFunc();

            ssView_Sheet1.RowCount = 0;

            panMain.Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(JDATE,'YYYY-MM-DD') JDATE , SINGU, SNAME, SABUN,Buse, PANO, ";
                SQL = SQL + ComNum.VBLF + " JUMIN1, JUMIN3, TEL, HPHONE, TO_CHAR(EXAMRES1,'MM-DD') EXAM1 ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES2,'MM-DD HH24:MI') EXAM2, TO_CHAR(EXAMRES3,'MM-DD') EXAM3, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES4,'MM-DD HH24:MI') EXAM4, TO_CHAR(EXAMRES6,'MM-DD') EXAM6, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES7,'MM-DD HH24:MI') EXAM7,TO_CHAR(EXAMRES8,'MM-DD') EXAM8,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES9,'MM-DD HH24:MI') EXAM9,TO_CHAR(EXAMRES10,'MM-DD') EXAM10,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES11,'MM-DD HH24:MI') EXAM11, TO_CHAR(EXAMRES12,'MM-DD') EXAM12,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES13,'MM-DD HH24:MI') EXAM13, TO_CHAR(EXAMRES14,'MM-DD') EXAM14,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES15,'MM-DD HH24:MI') EXAM15, TO_CHAR(EXAMRES16,'MM-DD HH24:MI') EXAM16,";
                SQL = SQL + ComNum.VBLF + " Gubun,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT_POSCO ";
                if (rdoDate_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE JDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE (EXAMRES1 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES1 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES2 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES2 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES3 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES3 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES4 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES4 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES6 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES6 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES7 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES7 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES8 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES8 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";


                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES9 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES9 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES10 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES10 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES11 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES11 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES12 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES12 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES13 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES13 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES14 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES14 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES15 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES15 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES16 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES16 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                }
                if (rdoGB_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND STOPFLAG ='Y' ";      //완료
                }
                if (rdoGB_2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND STOPFLAG IS NULL ";   //미완료(진행중)
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY JDATE, SNAME ";
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

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = 0;
                ssView_Sheet1.Rows.Count = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "";
                    if (rdoLtd_0.Checked == true)
                    {
                        if (dt.Rows[i]["Gubun"].ToString().Trim() == "01")
                        {
                            strOK = "OK";
                        }
                        else if (dt.Rows[i]["Gubun"].ToString().Trim() == "02")
                        {
                            strOK = "OK";
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow = nRow + 1;
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["JDATE"].ToString().Trim(); //접수일자
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim(); //등록번호
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim(); //이름
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[i]["JUMIN3"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Buse"].ToString().Trim(); //부서
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["SABUN"].ToString().Trim(); //직번
                        switch (dt.Rows[i]["SINGU"].ToString().Trim()) //신/구
                        {
                            case "1":
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = "신환";
                                break;
                            default:
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = "구환";
                                break;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["EXAM1"].ToString().Trim(); //초음파 - USG
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["EXAM8"].ToString().Trim(); //위장조영 - UGI
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["EXAM2"].ToString().Trim(); //위내시경 - GFS
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["EXAM3"].ToString().Trim(); //수면내시경수면 - GFS
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["EXAM6"].ToString().Trim(); //대장경 - 사용안함
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["EXAM7"].ToString().Trim(); //대장경수면 - 수면 CFS
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["EXAM9"].ToString().Trim(); //C/T - Chest CT
                        ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["EXAM10"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["EXAM11"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["EXAM12"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = dt.Rows[i]["EXAM13"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 18].Text = dt.Rows[i]["EXAM14"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 19].Text = dt.Rows[i]["EXAM15"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["EXAM16"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["ROWID"].ToString().Trim(); //ROWID

                        ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    }
                }

                ssView_Sheet1.RowCount = nRow;

                btnPrint.Enabled = true;

                dt.Dispose();
                dt = null;

                CF = null;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "POSCO 검사 의뢰자 LIST";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업기간 : " + dtpFDate.Text + "일부터 " + dtpTDate.Text + "일까지", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자 : " + strSysDate + " " + strSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear2()
        {
            //일자
            txtExamDate1.Text = "";
            txtExamDate2.Text = "";
            txtExamDate3.Text = "";
            txtExamDate4.Text = "";
            txtExamDate6.Text = "";
            txtExamDate7.Text = "";
            txtExamDate8.Text = "";
            txtExamDate9.Text = "";
            txtExamDate10.Text = "";
            txtExamDate11.Text = "";
            txtExamDate12.Text = "";
            txtExamDate13.Text = "";
            txtExamDate14.Text = "";
            txtExamDate15.Text = "";
            txtExamDate15.Text = "";

            //시간
            txtExamTime1.Text = "";
            txtExamTime2.Text = "";
            txtExamTime3.Text = "";
            txtExamTime4.Text = "";
            txtExamTime6.Text = "";
            txtExamTime7.Text = "";
            txtExamTime8.Text = "";
            txtExamTime9.Text = "";
            txtExamTime10.Text = "";
            txtExamTime11.Text = "";
            txtExamTime12.Text = "";
            txtExamTime13.Text = "";
            txtExamTime14.Text = "";
            txtExamTime15.Text = "";
            txtExamTime16.Text = "";

            txtResult1.Text = "";
            txtResult2.Text = "";
            txtResult3.Text = "";
            txtResult4.Text = "";
            txtResult5.Text = "";
            txtResult6.Text = "";
            txtResult7.Text = "";

            //참고사항
            txtRemark_0.Text = "";
            txtRemark_1.Text = "";
            txtRemark_2.Text = "";
            txtRemark_3.Text = "";
            txtRemark_4.Text = "";
            txtRemark_5.Text = "";
            txtRemark_6.Text = "";
            txtRemark_7.Text = "";
            txtRemark_8.Text = "";
            txtRemark_9.Text = "";
            txtRemark_10.Text = "";
            txtRemark_11.Text = "";
            txtRemark_12.Text = "";
            txtRemark_13.Text = "";
            txtRemark_14.Text = "";

            txtJuso.Text = "";

            //주소부분
            txtJuso2.Text = "";
            lblJuSo.Text = "";
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int nRead = 0;
            string strROWID = "";
            string strPANO = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComQuery CQ = new ComQuery();

            Screen_Clear2();

            if (e.RowHeader == true)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            panMain.Visible = true;

            strPANO = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
            strROWID = ssView_Sheet1.Cells[e.Row, 21].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(JDATE,'YYYY-MM-DD') JDATE , SINGU, SNAME, SABUN, PANO, STOPFLAG, SEX,";
                SQL = SQL + ComNum.VBLF + " JUMIN1, JUMIN3, TEL, HPHONE, TO_CHAR(EXAMRES1,'YYYY-MM-DD HH24:MI') EXAM1 ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES2,'YYYY-MM-DD HH24:MI') EXAM2, TO_CHAR(EXAMRES3,'YYYY-MM-DD HH24:MI') EXAM3, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES4,'YYYY-MM-DD HH24:MI') EXAM4, TO_CHAR(EXAMRES6,'YYYY-MM-DD HH24:MI') EXAM6, EXAM5, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES7,'YYYY-MM-DD HH24:MI') EXAM7, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES8,'YYYY-MM-DD HH24:MI') EXAM8, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES9,'YYYY-MM-DD HH24:MI') EXAM9, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES10,'YYYY-MM-DD HH24:MI') EXAM10, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES11,'YYYY-MM-DD HH24:MI') EXAM11, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES12,'YYYY-MM-DD HH24:MI') EXAM12, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES13,'YYYY-MM-DD HH24:MI') EXAM13, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES14,'YYYY-MM-DD HH24:MI') EXAM14, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES15,'YYYY-MM-DD HH24:MI') EXAM15, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EXAMRES16,'YYYY-MM-DD HH24:MI') EXAM16, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULT1,'YYYY-MM-DD') RESULT1 ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULT2,'YYYY-MM-DD') RESULT2 ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULT3,'YYYY-MM-DD') RESULT3 ,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULT4,'YYYY-MM-DD') RESULT4 , ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULT5,'YYYY-MM-DD') RESULT5 , ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULT6,'YYYY-MM-DD') RESULT6 , ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(RESULT7,'YYYY-MM-DD') RESULT7, ";
                SQL = SQL + ComNum.VBLF + " BUSE,Remark, JOBNAME ,Juso ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT_POSCO ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY JDATE, SNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRead = dt.Rows.Count;

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다. 다시 작업하세요");
                    return;
                }

                txtExamDate1.Text = VB.Left(dt.Rows[0]["EXAM1"].ToString().Trim(), 10);
                txtExamTime1.Text = VB.Right(dt.Rows[0]["EXAM1"].ToString().Trim(), 5);
                txtExamDate2.Text = VB.Left(dt.Rows[0]["EXAM2"].ToString().Trim(), 10);
                txtExamTime2.Text = VB.Right(dt.Rows[0]["EXAM2"].ToString().Trim(), 5);
                txtExamDate3.Text = VB.Left(dt.Rows[0]["EXAM3"].ToString().Trim(), 10);
                txtExamTime3.Text = VB.Right(dt.Rows[0]["EXAM3"].ToString().Trim(), 5);
                txtExamDate4.Text = VB.Left(dt.Rows[0]["EXAM4"].ToString().Trim(), 10);
                txtExamTime4.Text = VB.Right(dt.Rows[0]["EXAM4"].ToString().Trim(), 5);
                txtExamDate6.Text = VB.Left(dt.Rows[0]["EXAM6"].ToString().Trim(), 10);
                txtExamTime6.Text = VB.Right(dt.Rows[0]["EXAM6"].ToString().Trim(), 5);
                txtExamDate7.Text = VB.Left(dt.Rows[0]["EXAM7"].ToString().Trim(), 10);
                txtExamTime7.Text = VB.Right(dt.Rows[0]["EXAM7"].ToString().Trim(), 5);
                txtExamDate8.Text = VB.Left(dt.Rows[0]["EXAM8"].ToString().Trim(), 10);
                txtExamTime8.Text = VB.Right(dt.Rows[0]["EXAM8"].ToString().Trim(), 5);
                txtExamDate9.Text = VB.Left(dt.Rows[0]["EXAM9"].ToString().Trim(), 10);
                txtExamTime9.Text = VB.Right(dt.Rows[0]["EXAM9"].ToString().Trim(), 5);
                txtExamDate10.Text = VB.Left(dt.Rows[0]["EXAM10"].ToString().Trim(), 10);
                txtExamTime10.Text = VB.Right(dt.Rows[0]["EXAM10"].ToString().Trim(), 5);
                txtExamDate11.Text = VB.Left(dt.Rows[0]["EXAM11"].ToString().Trim(), 10);
                txtExamTime11.Text = VB.Right(dt.Rows[0]["EXAM11"].ToString().Trim(), 5);
                txtExamDate12.Text = VB.Left(dt.Rows[0]["EXAM12"].ToString().Trim(), 10);
                txtExamTime12.Text = VB.Right(dt.Rows[0]["EXAM12"].ToString().Trim(), 5);
                txtExamDate13.Text = VB.Left(dt.Rows[0]["EXAM13"].ToString().Trim(), 10);
                txtExamTime13.Text = VB.Right(dt.Rows[0]["EXAM13"].ToString().Trim(), 5);
                txtExamDate14.Text = VB.Left(dt.Rows[0]["EXAM14"].ToString().Trim(), 10);
                txtExamTime14.Text = VB.Right(dt.Rows[0]["EXAM14"].ToString().Trim(), 5);
                txtExamDate15.Text = VB.Left(dt.Rows[0]["EXAM15"].ToString().Trim(), 10);
                txtExamTime15.Text = VB.Right(dt.Rows[0]["EXAM15"].ToString().Trim(), 5);
                txtExamDate16.Text = VB.Left(dt.Rows[0]["EXAM16"].ToString().Trim(), 10);
                txtExamTime16.Text = VB.Right(dt.Rows[0]["EXAM16"].ToString().Trim(), 5);

                txtResult1.Text = dt.Rows[0]["RESULT1"].ToString().Trim();
                txtResult2.Text = dt.Rows[0]["RESULT2"].ToString().Trim();
                txtResult3.Text = dt.Rows[0]["RESULT3"].ToString().Trim();
                txtResult4.Text = dt.Rows[0]["RESULT4"].ToString().Trim();
                txtResult5.Text = dt.Rows[0]["RESULT5"].ToString().Trim();
                txtResult6.Text = dt.Rows[0]["RESULT6"].ToString().Trim();
                txtResult7.Text = dt.Rows[0]["RESULT7"].ToString().Trim();

                //참고사항
                txtRemark_0.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 1);
                txtRemark_1.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 2);
                txtRemark_2.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 3);
                txtRemark_3.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 4);
                txtRemark_4.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 5);
                txtRemark_5.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 6);
                txtRemark_6.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 7);
                txtRemark_7.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 8);
                txtRemark_8.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 9);
                txtRemark_9.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 10);
                txtRemark_10.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 11);
                txtRemark_11.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 12);
                txtRemark_12.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 13);
                txtRemark_13.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 14);
                txtRemark_14.Text = VB.Pstr(dt.Rows[0]["Remark"].ToString().Trim(), "^^", 15);

                chkBiopsy.Checked = false;
                if (dt.Rows[0]["EXAM5"].ToString().Trim() == "Y")
                {
                    chkBiopsy.Checked = true;
                }

                txtJuso2.Text = dt.Rows[0]["Juso"].ToString().Trim();

                dt.Dispose();
                dt = null;

                SQL = " SELECT  ZIPCODE1,ZIPCODE2,JUSO,JICODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPANO + "' ";

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
                    lblJuSo.Text = CQ.Read_Juso(clsDB.DbCon, dt.Rows[0]["ZIPCODE1"].ToString().Trim());
                    lblJuSo.Text = CQ.Read_Juso(clsDB.DbCon, dt.Rows[0]["ZIPCODE2"].ToString().Trim());
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

        private void panMain_Click(object sender, EventArgs e)
        {
            panMain.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panMain.Visible = false;
        }
    }
}
