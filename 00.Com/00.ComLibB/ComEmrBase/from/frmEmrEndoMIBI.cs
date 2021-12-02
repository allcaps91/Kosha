using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrEndoMIBI
    /// Description     : 내시경 챠트 작성 여부 확인
    /// Author          : 전상원
    /// Create Date     : 2018-05-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= \mtsEmr\EMRWARD\MHEMRWARD.vbp(Frm내시경챠트미비)" >> frmEmrEndoMIBI.cs 폼이름 재정의" />
    public partial class frmEmrEndoMIBI : Form
    {
        public frmEmrEndoMIBI()
        {
            InitializeComponent();
        }

        private void frmEmrEndoMIBI_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpDATE.Text = strSysDate;

            ssView_Sheet1.RowCount = 0;
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int j = 0;
            int nRead = 0;
            string strToDate = "";
            string strNextDate = "";

            string strPtNo = "";
            string strBDATE = "";
            string strChartDate = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            using (ComFunc CF = new ComFunc())
            {
                strToDate = CF.DATE_ADD(clsDB.DbCon, dtpDATE.Text, 0);
                strNextDate = CF.DATE_ADD(clsDB.DbCon, dtpDATE.Text, 1);
            }

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, GBDRG, B.DRNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "    AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "    AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7', '*')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'I')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT 0 ROOMCODE, PANO, SNAME, AGE, SEX, '' GBDRG, B.DRNAME, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'O' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7', '*')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'O')";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GUBUN, SNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = nRead;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        strBDATE = VB.Trim(ssView_Sheet1.Cells[i, 2].Text);
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        strPtNo = VB.Trim(ssView_Sheet1.Cells[i, 3].Text);
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        SQL = "";
                        SQL = " SELECT CHARTDATE, CHARTTIME, USEID, FORMNO";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST";
                        SQL = SQL + ComNum.VBLF + "WHERE FORMNO IN (2429, 2430, 2431, 2433, 2467)";
                        SQL = SQL + ComNum.VBLF + "  AND PTNO = '" + strPtNo + "'";
                        SQL = SQL + ComNum.VBLF + "  AND CHARTDATE = '" +strToDate.Replace("-", "") + "'";
                        SQL = SQL + ComNum.VBLF + "UNION ALL";
                        SQL = SQL + ComNum.VBLF + " SELECT CHARTDATE, CHARTTIME, CHARTUSEID, FORMNO";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST";
                        SQL = SQL + ComNum.VBLF + "WHERE FORMNO IN (2429, 2430, 2431, 2433, 2467)";
                        SQL = SQL + ComNum.VBLF + "  AND PTNO = '" + strPtNo + "'";
                        SQL = SQL + ComNum.VBLF + "  AND CHARTDATE = '" + strToDate.Replace("-", "") + "'";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY FORMNO ASC";

                        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                switch ((int)VB.Val(dt1.Rows[j]["FORMNO"].ToString().Trim()))
                                {
                                    case 2429:
                                    case 2467:
                                        ssView_Sheet1.Cells[i, 8].Text = MAKE_DATE(dt1.Rows[j]["CHARTDATE"].ToString().Trim(), dt1.Rows[j]["CHARTTIME"].ToString().Trim(), dt1.Rows[j]["USEID"].ToString().Trim());
                                        strChartDate = dt1.Rows[0]["CHARTDATE"].ToString().Trim();
                                        break;
                                    case 2430:
                                        ssView_Sheet1.Cells[i, 9].Text = MAKE_DATE(dt1.Rows[j]["CHARTDATE"].ToString().Trim(), dt1.Rows[j]["CHARTTIME"].ToString().Trim(), dt1.Rows[j]["USEID"].ToString().Trim());
                                        strChartDate = dt1.Rows[0]["CHARTDATE"].ToString().Trim();
                                        break;
                                    case 2431:
                                        ssView_Sheet1.Cells[i, 10].Text = MAKE_DATE(dt1.Rows[j]["CHARTDATE"].ToString().Trim(), dt1.Rows[j]["CHARTTIME"].ToString().Trim(), dt1.Rows[j]["USEID"].ToString().Trim());
                                        strChartDate = dt1.Rows[0]["CHARTDATE"].ToString().Trim();
                                        break;
                                    case 2433:
                                        ssView_Sheet1.Cells[i, 11].Text = MAKE_DATE(dt1.Rows[j]["CHARTDATE"].ToString().Trim(), dt1.Rows[j]["CHARTTIME"].ToString().Trim(), dt1.Rows[j]["USEID"].ToString().Trim());
                                        strChartDate = dt1.Rows[0]["CHARTDATE"].ToString().Trim();
                                        break;
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strChartDate != "")
                        {
                            ssView_Sheet1.Cells[i, 7].Text = READ_ENDO_NAME(strPtNo, strChartDate);
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_ENDO_NAME(string argPTNO, string ArgBDate)
        {
            string rtnVal = "";

            DataTable dt3 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ORDERNAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "ENDO_JUPMST A, " + ComNum.DB_MED + "OCS_ORDERCODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.ORDERCODE = B.ORDERCODE";
                SQL = SQL + ComNum.VBLF + "   AND A.RDATE >= TO_DATE('" + ArgBDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND A.RDATE <= TO_DATE('" + ArgBDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + argPTNO + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt3, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt3.Rows.Count > 0)
                {
                    rtnVal = dt3.Rows[0]["ORDERNAME"].ToString().Trim();
                }

                dt3.Dispose();
                dt3 = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt3 != null)
                {
                    dt3.Dispose();
                    dt3 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string MAKE_DATE(string Arg1, string Arg2, string argSABUN)
        {
            string rtnVal = "";

            Arg1 = VB.Left(Arg1, 4) + "-" + VB.Mid(Arg1, 5, 2) + "-" + VB.Right(Arg1, 2);
            Arg2 = VB.Left(Arg2, 2) + ":" + VB.Mid(Arg2, 3, 2);

            rtnVal = Arg1 + " " + Arg2 + "(" + clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(argSABUN).ToString("00000")) + ")";

            return rtnVal;
        }
    }
}
