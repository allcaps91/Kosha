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
    /// File Name       : frmPmpaViewMonthSuip.cs
    /// Description     : (경리과용)진료과별,환자종류별외래(입원)수익명세서
    /// Author          : 박창욱
    /// Create Date     : 2017-10-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-10-27 박창욱 : FrmMonthSuipNew4, FrmMonthSuipNew, FrmMonthSuip 폼을 통합
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs45.frm(FrmMonthSuipNew4.frm) >> frmPmpaViewMonthSuip.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\misubs\misubs30.frm(FrmMonthSuipNew.frm) >> frmPmpaViewMonthSuip.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\misubs\misubs27.frm(FrmMonthSuip.frm) >> frmPmpaViewMonthSuip.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMonthSuip : Form
    {
        public frmPmpaViewMonthSuip()
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

            if (rdoOpt0.Checked == true)
            {
                if (rdoIO0.Checked == true)
                {
                    strTitle = "진료과별,환자종류별 입원수익명세서";
                }
                if (rdoIO1.Checked == true)
                {
                    strTitle = "진료과별,환자종류별 외래수익명세서";
                }
            }
            if (rdoOpt1.Checked == true)
            {
                if (rdoIO0.Checked == true)
                {
                    strTitle = "진료과별,환자종류별 입원수익명세서(감액제외)";
                }
                if (rdoIO1.Checked == true)
                {
                    strTitle = "진료과별,환자종류별 외래수익명세서(감액제외)";
                }
            }
            if (rdoOpt2.Checked == true)
            {
                if (rdoIO0.Checked == true)
                {
                    strTitle = "진료과별,환자종류별 입원수익명세서(감액제외,미시행적용)";
                }
                if (rdoIO1.Checked == true)
                {
                    strTitle = "진료과별,환자종류별 외래수익명세서(감액제외,미시행적용)";
                }
            }

            if (rdoOpt2.Checked == true) 
            {
                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 17, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            }
            else
            {
                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            }

            

            if (rdoOpt2.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("작업년도 : " + cboYY.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else
            {
                strHeader += CS.setSpdPrint_String("작업월 : " + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("(단위:원) " , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);


            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

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
            int intRowAffected = 0; //변경된 Row 받는 변수

            int k = 0;
            int nRow = 0;
            int nCol = 0;
            double nTDept = 0;
            double[] nTot = new double[8];
            string strFYYMM = "";
            string strTYYMM = "";
            string strDeptCdoe = "";
            string strFDate = "";
            string strTdate = "";
            string strFYY = "";
            string strTYY = "";
            string strYY1 = "";
            string strYY2 = "";

            ComFunc cf = new ComFunc();

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);
            strFDate = VB.Left(cboFYYMM.Text, 4) + "-" + VB.Mid(cboFYYMM.Text, 7, 2) + "-01";
            strTdate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);

            strFYY = VB.Left(cboYY.Text, 4) + "01";
            strTYY = VB.Left(cboYY.Text, 4) + "12";
            strYY1 = (VB.Val(VB.Left(cboYY.Text, 4)) - 1) + "12";
            strYY2 = VB.Left(cboYY.Text, 4) + "12";

            for (i = 0; i < 8; i++)
            {
                nTot[i] = 0;
            }

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";

                if (rdoOpt0.Checked == true)
                {
                    //자료를 SELECT
                    SQL = SQL + ComNum.VBLF + "SELECT B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE,";
                    SQL = SQL + ComNum.VBLF + "       A.Bi, SUM(A.Amt1+A.Amt2+A.Amt3) SA1, SUM(A.Amt4) SA4";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "TONG_INCOM A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND A.Class = 'M' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.YYMMDD >= '" + strFYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.YYMMDD <= '" + strTYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE NOT IN ('R6')";
                    if (rdoIO0.Checked == true) //입원55
                    {
                        SQL = SQL + ComNum.VBLF + "   AND (( A.BUN >='01' AND A.BUN <='74' OR A.BUN = '77') OR (A.YYMMDD >='200612' AND  A.BUN ='82'))";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND A.BUN NOT IN ('75','79','83','80') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BUN <='90'   ";
                    }

                    if (rdoIO1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'O' ";
                    }
                    if (rdoIO0.Checked == true) //입원
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.Gubun = '1'";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = B.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.BI ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.BI ";
                }
                if (rdoOpt1.Checked == true)
                {
                    if (rdoIO0.Checked == true) //입원
                    {
                        SQL = SQL + ComNum.VBLF + " CREATE OR REPLACE VIEW VIEW_TONG_INCOM ( DEPTCODE, BI,  SA, SA2 ) AS ";
                        SQL = SQL + ComNum.VBLF + " SELECT A.DEPTCODE, A.Bi,SUM(A.Amt1+A.Amt2+A.Amt3) SA1, SUM(A.Amt4) SA4  ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "TONG_INCOM A ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.YYMMDD >= '" + strFYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.YYMMDD <= '" + strTYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE NOT IN ('R6')";
                        SQL = SQL + ComNum.VBLF + "    AND (( A.BUN >='01' AND A.BUN <='74' OR A.BUN = '77') OR (A.YYMMDD >='200612' AND  A.BUN ='82'))";
                        SQL = SQL + ComNum.VBLF + "    AND A.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.Gubun = '1'";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";
                        SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "SELECT A.DEPTCODE, A.Bi,SUM(Amt) SA1, 0  SA2  ";
                        SQL = SQL + ComNum.VBLF + " FROM  (";
                        SQL = SQL + ComNum.VBLF + "   SELECT 'M' CLASS, TO_CHAR(ACTDATE,'YYYYMM') YYMMDD, DEPTCODE,";
                        SQL = SQL + ComNum.VBLF + "          BI, SUM (AMT) * -1 AMT, '1' GUBUN , 'I' IPDOPD,";
                        SQL = SQL + ComNum.VBLF + "          BUN";
                        SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                        SQL = SQL + ComNum.VBLF + "    WHERE TO_CHAR(ACTDATE,'YYYYMM') >= '" + strFYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "      AND TO_CHAR(ACTDATE,'YYYYMM') <= '" + strTYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "    GROUP BY  TO_CHAR(ACTDATE,'YYYYMM'),  DEPTCODE, BI, BUN ) A  ";
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD >= '" + strFYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD <= '" + strTYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.BUN ='92' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.Gubun = '1'";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " CREATE OR REPLACE VIEW VIEW_TONG_INCOM ( DEPTCODE, BI,  SA, SA2 ) AS ";
                        SQL = SQL + ComNum.VBLF + " SELECT A.DEPTCODE, A.Bi,SUM(A.Amt1+A.Amt2+A.Amt3) SA1,SUM(A.Amt4) SA4  ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "TONG_INCOM A ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "   AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.YYMMDD >= '" + strFYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.YYMMDD <= '" + strTYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE NOT IN ('R6')";
                        SQL = SQL + ComNum.VBLF + "   AND A.BUN NOT IN ('75','79','83','80') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BUN <='90'   ";
                        SQL = SQL + ComNum.VBLF + "   AND A.IpdOpd = 'O' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";
                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "SELECT A.DEPTCODE, A.Bi,SUM(A.Amt1+A.Amt2+A.Amt3) * -1 SA1,SUM(A.Amt3) * -1 SA2  ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "TONG_INCOM A ";
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD >= '" + strFYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD <= '" + strTYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.BUN ='92' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'O' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }


                    //자료를 SELECT
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE,";
                    SQL = SQL + ComNum.VBLF + "       A.Bi,SUM(A.SA) SA1,SUM(A.SA2) SA4";
                    SQL = SQL + ComNum.VBLF + " FROM VIEW_TONG_INCOM A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.DEPTCODE = B.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.BI ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.BI ";
                }
                if (rdoOpt2.Checked == true)
                {
                    if (rdoIO0.Checked == true) //입원
                    {
                        SQL = SQL + ComNum.VBLF + " CREATE OR REPLACE VIEW VIEW_TONG_INCOM ( DEPTCODE, BI,  SA, SA2 ) AS ";
                        SQL = SQL + ComNum.VBLF + " SELECT A.DEPTCODE, A.Bi,SUM(A.Amt1+A.Amt2+A.Amt3) SA1, SUM(A.Amt4) SA4  ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "TONG_INCOM A ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.YYMMDD >= '" + strFYY + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.YYMMDD <= '" + strTYY + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE NOT IN ('R6')";
                        SQL = SQL + ComNum.VBLF + "    AND (( A.BUN >='01' AND A.BUN <='74' OR A.BUN = '77') OR (A.YYMMDD >='200612' AND  A.BUN ='82'))";
                        SQL = SQL + ComNum.VBLF + "    AND A.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.Gubun = '1'";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";
                        SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "SELECT A.DEPTCODE, A.Bi,SUM(Amt) SA1, 0  SA2  ";
                        SQL = SQL + ComNum.VBLF + " FROM  (";
                        SQL = SQL + ComNum.VBLF + "   SELECT 'M' CLASS, TO_CHAR(ACTDATE,'YYYYMM') YYMMDD, DEPTCODE,";
                        SQL = SQL + ComNum.VBLF + "          BI, SUM (AMT) * -1 AMT, '1' GUBUN , 'I' IPDOPD,";
                        SQL = SQL + ComNum.VBLF + "          BUN";
                        SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                        SQL = SQL + ComNum.VBLF + "    WHERE TO_CHAR(ACTDATE,'YYYYMM') >= '" + strFYY + "' ";
                        SQL = SQL + ComNum.VBLF + "      AND TO_CHAR(ACTDATE,'YYYYMM') <= '" + strTYY + "' ";
                        SQL = SQL + ComNum.VBLF + "    GROUP BY  TO_CHAR(ACTDATE,'YYYYMM'),  DEPTCODE, BI, BUN ) A  ";
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD >= '" + strFYY + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD <= '" + strTYY + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.BUN ='92' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.Gubun = '1'";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";

                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        SQL = SQL + ComNum.VBLF + " SELECT  A.DEPTCODE,  A.BI, SUM(A.AMT) SA , 0  SA2 ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_EXAM_RESERVED_SLIP A ";
                        SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + strYY1 + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE <> 'R6' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE,   A.BI";
                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        SQL = SQL + ComNum.VBLF + " SELECT  A.DEPTCODE,  A.BI, SUM(A.AMT) * -1 SA , 0  SA2 ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_EXAM_RESERVED_SLIP A ";
                        SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + strYY2 + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE <> 'R6' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE,   A.BI";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " CREATE OR REPLACE VIEW VIEW_TONG_INCOM ( DEPTCODE, BI,  SA, SA2 ) AS ";
                        SQL = SQL + ComNum.VBLF + " SELECT A.DEPTCODE, A.Bi,SUM(A.Amt1+A.Amt2+A.Amt3) SA1,SUM(A.Amt4) SA4  ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "TONG_INCOM A ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "   AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.YYMMDD >= '" + strFYY + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.YYMMDD <= '" + strTYY + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE NOT IN ('R6')";
                        SQL = SQL + ComNum.VBLF + "   AND A.BUN NOT IN ('75','79','83','80') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BUN <='90'   ";
                        SQL = SQL + ComNum.VBLF + "   AND A.IpdOpd = 'O' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";
                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "SELECT A.DEPTCODE, A.Bi,SUM(A.Amt1+A.Amt2+A.Amt3) * -1 SA1,SUM(A.Amt3) * -1 SA2  ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "TONG_INCOM A ";
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND A.Class = 'M' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD >= '" + strFYY + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.YYMMDD <= '" + strTYY + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.BUN ='92' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'O' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE, A.BI";

                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        SQL = SQL + ComNum.VBLF + " SELECT  A.DEPTCODE,  A.BI, SUM(A.AMT) SA , 0  SA2 ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_EXAM_RESERVED_SLIP A ";
                        SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + strYY1 + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IpdOpd = 'O' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE <> 'R6' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE,   A.BI";
                        SQL = SQL + ComNum.VBLF + " UNION ALL ";
                        SQL = SQL + ComNum.VBLF + " SELECT  A.DEPTCODE,  A.BI, SUM(A.AMT) * -1 SA , 0  SA2 ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_EXAM_RESERVED_SLIP A ";
                        SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + strYY2 + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.IpdOpd = 'O' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE <> 'R6' ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY A.DEPTCODE,   A.BI";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }


                    //자료를 SELECT
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE,";
                    SQL = SQL + ComNum.VBLF + "       A.Bi,SUM(A.SA) SA1,SUM(A.SA2) SA4";
                    SQL = SQL + ComNum.VBLF + " FROM VIEW_TONG_INCOM A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.DEPTCODE = B.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.BI ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY B.PRINTRANKING, B.DEPTNAMEK, A.DEPTCODE, A.BI ";
                }
                //OPD_ETCSLIP 은 TONG_IMCOM에서 R6으로 처리됨. MISU_BALDILAY에는 일반 누적으로 금액 집계됨
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                nRow = 0;
                strDeptCdoe = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strDeptCdoe != dt.Rows[i]["DeptCode"].ToString().Trim())
                    {
                        if (i != 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = nTDept.ToString();
                        }
                        nRow += 1;
                        ssView_Sheet1.RowCount = nRow;
                        strDeptCdoe = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();
                        nTDept = 0;
                    }

                    switch (dt.Rows[i]["Bi"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "32":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                            nCol = 1;   //보험
                            break;
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                            nCol = 2;   //보호
                            break;
                        case "52":
                            nCol = 3;   //자보
                            break;
                        case "31":
                        case "33":
                            nCol = 4;   //산재
                            break;
                        default:
                            nCol = 5;   //일반
                            break;
                    }

                    ssView_Sheet1.Cells[nRow - 1, nCol].Text = (VB.Val(ssView_Sheet1.Cells[nRow - 1, nCol].Text) + VB.Val(dt.Rows[i]["SA1"].ToString().Trim()) -
                                                                VB.Val(dt.Rows[i]["SA4"].ToString().Trim())).ToString();
                    nTDept += VB.Val(dt.Rows[i]["SA1"].ToString().Trim()) - VB.Val(dt.Rows[i]["SA4"].ToString().Trim());
                    nTot[nCol] += VB.Val(dt.Rows[i]["SA1"].ToString().Trim()) - VB.Val(dt.Rows[i]["SA4"].ToString().Trim());
                    nTot[6] += VB.Val(dt.Rows[i]["SA1"].ToString().Trim()) - VB.Val(dt.Rows[i]["SA4"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                if (i != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nTDept.ToString();
                }


                //합계
                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "합계 ";
                for (i = 1; i < 7; i++)
                {
                    ssView_Sheet1.Cells[nRow - 1, i].Text = nTot[i].ToString("###,###,###,###,##0 ");
                }

                for (i = 1; i < ssView_Sheet1.RowCount; i++)
                {
                    for (k = 1; k < ssView_Sheet1.ColumnCount; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k].Text = VB.Val(ssView_Sheet1.Cells[i - 1, k].Text).ToString("###,###,###,###,##0 ");
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMonthSuip_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 24, "", "1");
            clsVbfunc.SetCboDateYY(clsDB.DbCon, cboYY, 24, "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboFYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    cboYY.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void rdoOpt_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOpt0.Checked == true)
            {
                grbMonth.Visible = true;
                grbYear.Visible = false;
            }
            if (rdoOpt1.Checked == true)
            {
                grbMonth.Visible = true;
                grbYear.Visible = false;
            }
            if (rdoOpt2.Checked == true)
            {
                grbMonth.Visible = false;
                grbYear.Visible = true;
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
