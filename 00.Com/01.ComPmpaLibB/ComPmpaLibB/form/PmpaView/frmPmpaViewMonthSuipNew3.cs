using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMonthSuipNew3.cs
    /// Description     : (경리과용)월별환자수및의료수익
    /// Author          : 박창욱
    /// Create Date     : 2017-10-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs40.frm(FrmMonthSuipNew3.frm) >> frmPmpaViewMonthSuipNew3.cs 폼이름 재정의" />
    public partial class frmPmpaViewMonthSuipNew3 : Form
    {
        public frmPmpaViewMonthSuipNew3()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "월별 환자수 및 의료수익";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년도: " + cboYear.Text + "년", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("  (단위:원)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.9f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRead = 0;
            int nMM = 0;
            string strYear = "";
            double[,] nTot = new double[7, 13];
            double nTotal = 0;

            strYear = cboYear.Text.Trim();

            for (i = 1; i < 7; i++)
            {
                for (k = 1; k < 13; k++)
                {
                    nTot[i, k] = 0;
                }
                for (k = 1; k < 13; k++)
                {
                    ssView_Sheet1.Cells[i - 1, k + 1].Text = "";
                }
            }

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            try
            {
                //외래수익
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT YYMM, SUM(OAmt1 + OAmt2 - OAMT4) cAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(OCNT1+OCNT2) cCNT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "TONG_MONTHLY ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND YYMM >= '" + strYear + "01' ";
                SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strYear + "12' ";
                if (chkAll.Checked == false)
                {
                    //수탁,종검,일반건진 제외
                    SQL = SQL + ComNum.VBLF + "   AND DeptCode NOT IN ('R6','TO','HR','EM','II','OM')";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMM ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    nMM = (int)VB.Val(VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2));
                    nTot[1, nMM] += VB.Val(dt.Rows[i]["cCNT"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT YYMMDD YYMM, SUM(Amt1+Amt2+Amt3-AMT4) cAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "TONG_INCOM";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Class   = 'M'";
                SQL = SQL + ComNum.VBLF + "    AND YYMMDD >= '" + strYear + "01'";
                SQL = SQL + ComNum.VBLF + "    AND YYMMDD <= '" + strYear + "12'";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE NOT IN ('R6')";
                SQL = SQL + ComNum.VBLF + "    AND IpdOpd  = 'O'";
                SQL = SQL + ComNum.VBLF + "    AND Gubun   = '1'";
                SQL = SQL + ComNum.VBLF + "    AND BUN NOT IN ('75','79','83','80')";
                SQL = SQL + ComNum.VBLF + "    AND BUN <='90'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY YYMMDD";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nMM = (int)VB.Val(VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2));
                    nTot[2, nMM] += VB.Val(dt.Rows[i]["cAmt"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;


                //입원수익
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT YYMM,SUM(IAmt1 + IAmt2 - IAMT4) cAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(ICNT2) cCNT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "TONG_MONTHLY ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND YYMM >= '" + strYear + "01' ";
                SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strYear + "12' ";
                SQL = SQL + ComNum.VBLF + "   AND DRCODE<>'0000' ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY YYMM ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nMM = (int)VB.Val(VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2));
                    nTot[3, nMM] += VB.Val(dt.Rows[i]["cCNT"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;



                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT YYMMDD YYMM ,SUM(Amt1+Amt2+Amt3 -AMT4) cAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "TONG_INCOM";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND Class   = 'M'";
                SQL = SQL + ComNum.VBLF + "   AND YYMMDD >= '" + strYear + "01'";
                SQL = SQL + ComNum.VBLF + "   AND YYMMDD <= '" + strYear + "12'";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE NOT IN ('R6')";
                SQL = SQL + ComNum.VBLF + "   AND IpdOpd  = 'I'";
                SQL = SQL + ComNum.VBLF + "   AND Gubun   = '1'";
                SQL = SQL + ComNum.VBLF + "   AND (( (BUN >='01' AND BUN <='74') OR BUN = '77') OR (YYMMDD >='200612' AND  BUN ='82'))";
                SQL = SQL + ComNum.VBLF + " GROUP BY YYMMDD";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nMM = (int)VB.Val(VB.Right(dt.Rows[i]["YYMM"].ToString().Trim(), 2));
                    nTot[4, nMM] += VB.Val(dt.Rows[i]["cAmt"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;


                //DRG
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ActDate,'MM') cMM,SUM(ILSU) cCNT,SUM(AMT50) cAmt";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS";
                SQL = SQL + ComNum.VBLF + "WHERE ActDate>=TO_DATE('" + strYear + "-01-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strYear + "-12-31','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND DrgCode IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "  GROUP BY TO_CHAR(ActDate,'MM')";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nMM = (int)VB.Val(VB.Right(dt.Rows[i]["cMM"].ToString().Trim(), 2));
                    nTot[5, nMM] += VB.Val(dt.Rows[i]["cCNT"].ToString().Trim());
                    nTot[6, nMM] += VB.Val(dt.Rows[i]["cAmt"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;


                //화면에 집계된 통계를 표시함
                for (i = 1; i < 7; i++)
                {
                    nTotal = 0;
                    for (k = 1; k < 13; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k + 1].Text = nTot[i, k].ToString("#,##0");
                        nTotal += nTot[i, k];
                    }
                    ssView_Sheet1.Cells[i - 1, 14].Text = nTotal.ToString("#,##0");
                }

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMonthSuipNew3_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int k = 0;

            for (i = 3; i < 15; i++)
            {
                for (k = 1; k < 7; k++)
                {
                    ssView_Sheet1.Cells[k - 1, i - 1].Text = "";
                }
            }
            clsVbfunc.SetCboDateYY(clsDB.DbCon, cboYear, 10, "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYear.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog mDlg = new SaveFileDialog())
            {
                mDlg.InitialDirectory = Application.StartupPath;
                mDlg.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                mDlg.FilterIndex = 1;
                if (mDlg.ShowDialog() == DialogResult.OK)
                {
                    ssView.SaveExcel(mDlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                }
            }
        }
    }
}
