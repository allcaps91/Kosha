using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstOutDrugIdentification.cs
    /// Description     : 외래약처방 식별 요청
    /// Author          : 이정현
    /// Create Date     : 2017-12-08
    /// <history> 
    /// 외래약처방 식별 요청
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\Frm식별외래약처방선택.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstOutDrugIdentification : Form
    {
        private string GstrPano = "";
        private string GstrWRTNO = "";
        private string GstrBDate = "";

        public frmSupDrstOutDrugIdentification()
        {
            InitializeComponent();
        }

        public frmSupDrstOutDrugIdentification(string strPano, string strWRTNO, string strBDate)
        {
            InitializeComponent();

            GstrPano = strPano;
            GstrWRTNO = strWRTNO;
            GstrBDate = strBDate;
        }

        private void frmSupDrstOutDrugIdentification_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-100);
            dtpEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            ssList_Sheet1.RowCount = 0;
            ssSlip_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            READ_OPD_DRUG(GstrPano, dtpSDate.Value.ToString("yyyy-MM-dd"), dtpEDate.Value.ToString("yyyy-MM-dd"));
            READ_SEND_DRUG(GstrWRTNO);
        }

        private void READ_OPD_DRUG(string strPTNO, string strSDATE, string strEDATE)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, DEPTCODE, DRCODE, 'OUT' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND FLAG = 'P' ";
                SQL = SQL + ComNum.VBLF + "         AND SLIPDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND SLIPDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "GROUP BY PANO, ACTDATE, DEPTCODE, DRCODE, 'OUT'";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, TO_CHAR(BDATE, 'YYYY-MM-DD') AS ACTDATE, DEPTCODE, DRCODE, 'ATC' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGATC ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SENDKEY = 'Y'";
                SQL = SQL + ComNum.VBLF + "         AND GBIO = '1'";
                SQL = SQL + ComNum.VBLF + "         AND BDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "GROUP BY PANO, TO_CHAR(BDATE, 'YYYY-MM-DD'), DEPTCODE, DRCODE, 'ATC'";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(B.BDATE,'YYYY-MM-DD') AS ACTDATE, A.DEPTCODE, A.DRCODE, 'TOI' AS GUBUN ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_MED + "OCS_PHARMACY B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PTNO";
                SQL = SQL + ComNum.VBLF + "         AND A.OUTDATE = B.BDATE ";
                SQL = SQL + ComNum.VBLF + "         AND B.GBTFLAG = 'T' ";
                SQL = SQL + ComNum.VBLF + "         AND B.BDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND B.BDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, TO_CHAR(B.BDATE,'YYYY-MM-DD'), A.DEPTCODE, A.DRCODE, 'TOI' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ACTDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                    }
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
            }
        }

        private void READ_SEND_DRUG(string strWRTNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            if (strWRTNO == "") { return; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.RP, A.WRTNO, A.SUCODE, B.SUNAMEK, A.QTY1, A.QTY, A.DOSCODE, A.NAL, A.EDICODE, A.GUBUN, A.PAGENO, A.ROWID, A.TUDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOISLIP_SEND A, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "         AND A.WRTNO = " + strWRTNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY RP, DOSCODE, QTY1";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RP"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["QTY1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["EDICODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["PAGENO"].ToString().Trim();

                        if (dt.Rows[i]["TUDATE"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 11].Text = Convert.ToDateTime(dt.Rows[i]["TUDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
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
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            READ_OPD_DRUG(GstrPano, dtpSDate.Value.ToString("yyyy-MM-dd"), dtpEDate.Value.ToString("yyyy-MM-dd"));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                this.Close();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strSucode = "";
            string strEDICODE = "";
            string strBDATE = "";
            string strPAGENO = "";
            string strDosCode = "";
            string strQTY = "";
            //string strOK = "";
            string strRemark = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssSlip_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssSlip_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strQTY = ssSlip_Sheet1.Cells[i, 5].Text.Trim();
                        strSucode = ssSlip_Sheet1.Cells[i, 8].Text.Trim();
                        strEDICODE = ssSlip_Sheet1.Cells[i, 9].Text.Trim();
                        strBDATE = ssSlip_Sheet1.Cells[i, 10].Text.Trim();
                        strPAGENO = ssSlip_Sheet1.Cells[i, 11].Text.Trim();
                        strDosCode = ssSlip_Sheet1.Cells[i, 12].Text.Trim();
                        strRemark = ssSlip_Sheet1.Cells[i, 13].Text.Trim();

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOISLIP_SEND";
                        SQL = SQL + ComNum.VBLF + "     (SUCODE, TUDATE, PAGENO, GUBUN, EDICODE, BDATE, WSABUN, PANO, QTY, DOSCODE)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + strSucode + "', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strBDATE + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPAGENO + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strRemark + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strEDICODE + "', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + GstrBDate + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrPano + "', ";
                        SQL = SQL + ComNum.VBLF + "         " + strQTY + ", ";
                        SQL = SQL + ComNum.VBLF + "         '" + strDosCode + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strDate = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            string strGubun = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();

            ssSlip_Sheet1.RowCount = 0;

            try
            {
                if (strGubun == "ATC")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     'O' AS GBN, B.SUNAMEK, C.DOSNAME, SUM(A.QTY * A.NAL) AS QTYCNT, A.SUCODE AS SUNEXT, ";
                    SQL = SQL + ComNum.VBLF + "     '외래원내조제약' AS REMARK, C.DOSCODE, '11' AS BUN, '' AS DIVQTY, C.GBDIV AS DIV, A.QTY, B.BCODE AS EDICODE, A.BDATE, A.TUYAKNO AS PAGENO";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGATC A, " + ComNum.DB_PMPA + "BAS_SUN B, " + ComNum.DB_MED + "OCS_ODOSAGE C";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + GstrPano + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.BDATE  = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.SENDKEY = 'Y'";
                    SQL = SQL + ComNum.VBLF + "GROUP BY 'O', B.SUNAMEK, C.DOSNAME, A.SUCODE, '외래원내조제약', C.DOSCODE, '11', '', C.GBDIV, A.QTY, B.BCODE, A.BDATE, A.TUYAKNO";
                    SQL = SQL + ComNum.VBLF + "ORDER BY B.SUNAMEK ";
                }
                else if (strGubun == "OUT")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     'O' AS GBN, B.SUNAMEK, C.DOSNAME, SUM(A.QTY * A.NAL) AS QTYCNT, A.SUCODE AS SUNEXT, ";
                    SQL = SQL + ComNum.VBLF + "     '외래진료약' AS REMARK, C.DOSCODE, A.BUN, A.DIVQTY, A.DIV, A.QTY, A.EDICODE, A.SLIPDATE AS BDATE, A.SLIPNO AS PAGENO";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OUTDRUG A, " + ComNum.DB_PMPA + "BAS_SUN B, " + ComNum.DB_MED + "OCS_ODOSAGE C";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + GstrPano + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.SLIPDATE  = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('11', '12', '20')";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.FLAG = 'Y'";
                    SQL = SQL + ComNum.VBLF + "Group By 'O', B.SUNAMEK, C.DOSNAME, A.SUCODE, '외래진료약', C.DOSCODE, A.BUN, A.DIVQTY, A.DIV, A.QTY, A.EDICODE, A.SLIPDATE, A.SLIPNO";
                    SQL = SQL + ComNum.VBLF + "ORDER BY B.SUNAMEK";
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     'T' AS GBN, B.SUNAMEK, C.DOSNAME, SUM(A.QTY * A.NAL) AS QTYCNT, A.SUCODE AS SUNEXT, ";
                    SQL = SQL + ComNum.VBLF + "     '퇴원약' AS REMARK, A.DOSCODE, A.BUN, (A.QTY/A.GBDIV) AS DIVQTY, A.GBDIV AS DIV, A.QTY, B.BCODE AS EDICODE, A.BDATE, A.GBPRINT AS PAGENO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_PHARMACY A, " + ComNum.DB_PMPA + "BAS_SUN B, " + ComNum.DB_MED + "OCS_ODOSAGE C";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + GstrPano + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBTFLAG = 'T'";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE";
                    SQL = SQL + ComNum.VBLF + "GROUP BY 'T', B.SUNAMEK, C.DOSNAME, A.SUCODE, '퇴원약', A.DOSCODE, A.BUN, A.QTY/A.GBDIV, A.GBDIV, A.QTY, B.BCODE, A.BDATE, A.GBPRINT  ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY B.SUNAMEK ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssSlip_Sheet1.RowCount = dt.Rows.Count;
                    ssSlip_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssSlip_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GBN"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DIV"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 5].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DIVQTY"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 7].Text = dt.Rows[i]["QTYCNT"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 9].Text = dt.Rows[i]["EDICODE"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 10].Text = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssSlip_Sheet1.Cells[i, 11].Text = dt.Rows[i]["PAGENO"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 12].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssSlip_Sheet1.Cells[i, 13].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    }
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
            }
        }

        private void ssSlip_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssSlip_Sheet1.RowCount == 0) { return; }
            
            if (e.ColumnHeader == true || e.Column == 0)
            {
                if (Convert.ToBoolean(ssSlip_Sheet1.ColumnHeader.Cells[0, 0].Value) == true)
                {
                    ssSlip_Sheet1.ColumnHeader.Cells[0, 0].Value = false;
                    ssSlip_Sheet1.Cells[0, 0, ssSlip_Sheet1.RowCount - 1, 0].Value = false;
                }
                else
                {
                    ssSlip_Sheet1.ColumnHeader.Cells[0, 0].Value = true;
                    ssSlip_Sheet1.Cells[0, 0, ssSlip_Sheet1.RowCount - 1, 0].Value = true;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (DelData() == true)
            {
                READ_SEND_DRUG(GstrWRTNO);
            }
        }

        private bool DelData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strROWID = ssView_Sheet1.Cells[i, 12].Text.Trim();

                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_ERP + "DRUG_HOISLIP_SEND ";
                        SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
    }
}
