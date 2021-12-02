using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstAntiCancerAndTPN.cs
    /// Description     : 항암 및 TPN
    /// Author          : 이정현
    /// Create Date     : 2017-09-26
    /// <history> 
    /// 항암 및 TPN
    /// 2021-04-02 ComLibB로 이동(심사팀 동시사용)
    /// </history>
    /// <seealso>
    /// PSMH\drug\drservice\Frm항암및TPN.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drservice\drservice.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstAntiCancerAndTPN : Form
    {
        ComFunc CF = new ComFunc();
        public frmSupDrstAntiCancerAndTPN()
        {
            InitializeComponent();
        }

        private void frmSupDrstAntiCancerAndTPN_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            ssPrint_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;
        }

        private void rdoIO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoIO0.Checked == true)
            {
                rdoBun1.Enabled = true;
            }
            else if (rdoIO1.Checked == true)
            {
                rdoBun1.Enabled = false;
            }
            else
            {

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            ssView_Sheet1.RowCount = 0;

            ssView.ActiveSheet.Columns[0].Visible = true;
            ssView.ActiveSheet.Columns[2].Visible = true;
            ssView.ActiveSheet.Columns[4].Visible = true;
            ssView.ActiveSheet.Columns[6].Visible = true;
            ssView.ActiveSheet.Columns[7].Visible = true;
            ssView.ActiveSheet.Columns[8].Visible = true;
            ssView.ActiveSheet.Columns[11].Visible = true;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     IPDOPD, PANO, SNAME, SEX, AGE, WARDCODE, ROOMCODE, DEPTCODE, DRSABUN,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ENTDATE,'YYYY-MM-DD') AS ENTDATE, SEQNO, TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JUSAMIX ";
                SQL = SQL + ComNum.VBLF + "     WHERE ENTDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ENTDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (rdoIO0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'I' ";
                }
                else if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                }
                else if (rdoIO2.Checked == true)
                {

                }

                if (rdoBun0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN IN ('1','2') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '3' ";
                }

                if (txtPtno.Text.Trim() != "" && txtPtno.Text.Length > 7)
                {
                    SQL = SQL + ComNum.VBLF + "         AND PANO = '" + txtPtno.Text.Trim() + "'";
                }

                SQL = SQL + ComNum.VBLF + "           AND DOSNAME IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ENTDATE, BDATE, IPDOPD, PANO, SNAME, SEX, AGE, WARDCODE, ROOMCODE, DEPTCODE, DRSABUN, SEQNO ";
                //19-10-17(서류 제출 위해서 필요하다고 해서 선처리 함).
                SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE, WARDCODE, ROOMCODE, SNAME";

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
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SEX"].ToString().Trim();

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     JEPCODE, REALQTY";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JUSAMIX ";
                        SQL = SQL + ComNum.VBLF + "     WHERE ENTDATE = TO_DATE('" + dt.Rows[i]["ENTDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND SEQNO = " + dt.Rows[i]["SEQNO"].ToString().Trim();
                        SQL = SQL + ComNum.VBLF + "         AND DOSNAME IS NOT NULL ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                if (k != 0)
                                {
                                    ssView_Sheet1.Cells[i, 9].Text = ssView_Sheet1.Cells[i, 9].Text + ComNum.VBLF;
                                }

                                ssView_Sheet1.Cells[i, 9].Text = ssView_Sheet1.Cells[i, 9].Text + " " + dt1.Rows[k]["JEPCODE"].ToString().Trim() + " : " + dt1.Rows[k]["REALQTY"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "I")
                        {
                            ssView_Sheet1.Cells[i, 10].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DRSABUN"].ToString().Trim());
                        }
                        else
                        {                            
                            ssView_Sheet1.Cells[i, 10].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRSABUN"].ToString().Trim());
                        }

                        ssView_Sheet1.Cells[i, 11].Text = READ_ILLS(dt.Rows[i]["IPDOPD"].ToString().Trim()
                                                                  , dt.Rows[i]["PANO"].ToString().Trim()
                                                                  , dt.Rows[i]["DEPTCODE"].ToString().Trim()
                                                                  , dt.Rows[i]["BDATE"].ToString().Trim());

                        ssView_Sheet1.SetRowHeight(i, Convert.ToInt32(ssView_Sheet1.GetPreferredRowHeight(i)) + 5);
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

        public string READ_ILLS(string argGBIO, string argPTNO, string argDEPTCODE, string argBATE)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;

            if(argGBIO == "I")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT INDATE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND OUTDATE IS NULL";
                SQL += ComNum.VBLF + "  AND GBSTS IN ('0', '1', '2', '3', '4')";
                SQL += ComNum.VBLF + "  AND PANO = '" + argPTNO + "'";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt2.Rows.Count > 0)
                {
                    argBATE = VB.Left(dt2.Rows[0]["INDATE"].ToString().Trim(), 10);
                }

                dt2.Dispose();
                dt2 = null;

            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT B.ILLCODED, B.ILLNAMEK                                                     ";

            if (argGBIO == "O")
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B       ";
            }
            else
            {
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B       ";
            }

            SQL += ComNum.VBLF + "WHERE 1=1                                                                         ";
            SQL += ComNum.VBLF + "  AND A.ILLCODE = B.ILLCODE                                                       ";
            SQL += ComNum.VBLF + "  AND B.ILLCLASS = '1'                                                            ";
            SQL += ComNum.VBLF + "  AND A.PTNO = '" + argPTNO + "'                                                  ";
            SQL += ComNum.VBLF + "  AND A.DEPTCODE = '" + argDEPTCODE + "'                                          ";
            if (argGBIO == "O")
            {
                SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + argBATE + "', 'YYYY-MM-DD')                      ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND A.ENTDATE = TO_DATE('" + argBATE + "', 'YYYY-MM-DD')                    ";
            }
            SQL += ComNum.VBLF + "ORDER BY A.SEQNO                                                                  ";

            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt2.Rows.Count > 0)
            {
                rtnVal = dt2.Rows[0]["ILLCODED"].ToString().Trim() + ", " + dt2.Rows[0]["ILLNAMEK"].ToString().Trim();
            }

            dt2.Dispose();
            dt2 = null;

            return rtnVal;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssPrint_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            txtPtno.Text = "";
            txtPano.Text = "";
            txtSName.Text = "";
            txtSexAge.Text = "";
            txtDiagno.Text = "";

            ssView.ActiveSheet.Columns[0].Visible = true;
            ssView.ActiveSheet.Columns[2].Visible = true;
            ssView.ActiveSheet.Columns[4].Visible = true;
            ssView.ActiveSheet.Columns[6].Visible = true;
            ssView.ActiveSheet.Columns[7].Visible = true;
            ssView.ActiveSheet.Columns[8].Visible = true;            
            ssView.ActiveSheet.Columns[11].Visible = true;
        }

        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ssView_Sheet1.RowCount == 0) { return; }

            ssPrint_Sheet1.RowCount = 0;

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                ssPrint_Sheet1.SetRowHeight(i, (int)ssView_Sheet1.Rows[i].Height);

                ssPrint_Sheet1.Cells[i, 0].Text = ssView_Sheet1.Cells[i, 1].Text.Trim();
                ssPrint_Sheet1.Cells[i, 1].Text = ssView_Sheet1.Cells[i, 2].Text.Trim();
                ssPrint_Sheet1.Cells[i, 2].Text = ssView_Sheet1.Cells[i, 3].Text.Trim();
                ssPrint_Sheet1.Cells[i, 3].Text = ssView_Sheet1.Cells[i, 4].Text.Trim();
                ssPrint_Sheet1.Cells[i, 4].Text = ssView_Sheet1.Cells[i, 5].Text.Trim();
                ssPrint_Sheet1.Cells[i, 5].Text = ssView_Sheet1.Cells[i, 6].Text.Trim();
                ssPrint_Sheet1.Cells[i, 6].Text = ssView_Sheet1.Cells[i, 7].Text.Trim();
                ssPrint_Sheet1.Cells[i, 7].Text = ssView_Sheet1.Cells[i, 8].Text.Trim();
                ssPrint_Sheet1.Cells[i, 8].Text = ssView_Sheet1.Cells[i, 9].Text;
                ssPrint_Sheet1.Cells[i, 9].Text = ssView_Sheet1.Cells[i, 10].Text;
                ssPrint_Sheet1.Cells[i, 10].Text = ssView_Sheet1.Cells[i, 11].Text.Trim();
            }

            PreViewAndPrint();
        }

        private void btnPrintSel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ssView_Sheet1.RowCount == 0) { return; }

            ssPrint_Sheet1.RowCount = 0;

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                {
                    ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                    ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, (int)ssView_Sheet1.Rows[i].Height);

                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 8].Text.Trim();
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].Text = ssView_Sheet1.Cells[i, 9].Text;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 9].Text = ssView_Sheet1.Cells[i, 10].Text;
                }
            }

            PreViewAndPrint();

            ssPrint.ActiveSheet.Columns[9].Visible = true;
        }

        private void PreViewAndPrint()
        {
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strFont3 = "";
            string strHead3 = "";

            string strName = "";
            string strIO = "";

            if (ssPrint_Sheet1.RowCount == 0) { return; } 

            if(txtPtno.Text.Trim() != "")
            {
                strName = "환자별 ";
            }

            if (rdoBun0.Checked == true)
            {
                strName += "항암주사제 무균";
            }
            else
            {
                strName += "TPN 무균";
            }

            if (rdoIO0.Checked == true)
            {
                strIO = "입원";
            }
            else if (rdoIO1.Checked)
            {
                strIO = "외래";
            }
            else
            {
                strIO = "전체";
            }

            ssPrint.ActiveSheet.Columns[9].Visible = false;

            strFont1 = "/fn\"맑은 고딕\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strFont3 = "/fn\"맑은 고딕\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + strName + "조제 현황" + "(" + strIO + ")" + "/f1/n";
            strHead2 = "/l/f2" + "☞ 조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd") + "/f2/n";
            strHead3 = "/r/f2" + "조제자 : 　　　　　　";

            ssPrint_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2 + strFont3 + strHead3;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 100;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPrint_Sheet1.PrintInfo.Margin.Header = 20;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowBorder = true;
            ssPrint_Sheet1.PrintInfo.ShowGrid = true;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = true;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPrint_Sheet1.PrintInfo.Preview = false;
            ssPrint_Sheet1.PrintInfo.ZoomFactor = (float)0.90;
            ssPrint.PrintSheet(0);
            
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strOK = "";  //동일환자 체크용

            if (ssView.ActiveSheet.Rows.Count == 0) return;

            if (e.Column != 0)
            {
                strOK = "OK";

                txtPtno.Text = ssView.ActiveSheet.Cells[e.Row, 6].Text;

                btnSearch.PerformClick();

                ssView.ActiveSheet.Columns[0].Visible = false;
                ssView.ActiveSheet.Columns[2].Visible = false;
                ssView.ActiveSheet.Columns[4].Visible = false;
                ssView.ActiveSheet.Columns[6].Visible = false;
                ssView.ActiveSheet.Columns[7].Visible = false;
                ssView.ActiveSheet.Columns[8].Visible = false;                
                ssView.ActiveSheet.Columns[11].Visible = false;

                for(int i = 0; i < ssView.ActiveSheet.Rows.Count; i++)
                {
                    if(txtPtno.Text != ssView.ActiveSheet.Cells[i, 6].Text)
                    {
                        strOK = "NO";
                    }
                }

                if (strOK == "OK")
                {
                    txtSName.Text = ssView.ActiveSheet.Cells[0, 5].Text;
                    txtPano.Text = ssView.ActiveSheet.Cells[0, 6].Text;
                    txtSexAge.Text = ssView.ActiveSheet.Cells[0, 8].Text + "/" + ssView.ActiveSheet.Cells[0, 7].Text;
                    txtDiagno.Text = ssView.ActiveSheet.Cells[0, 11].Text;
                }
            }

        }

        private void btnSearch2_Click(object sender, EventArgs e)
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
                SQL = SQL + ComNum.VBLF + "     IPDOPD, PANO, SNAME, (SEX || '/' || AGE) AS SEXAGE, WARDCODE, MAX(ROOMCODE) ROOMCODE , DEPTCODE, DRSABUN,";
                SQL = SQL + ComNum.VBLF + "     CASE WHEN IPDOPD = 'I' THEN (SELECT EMP_NM FROM KOSMOS_ERP.HR_EMP_BASIS WHERE EMP_ID = LPAD(DRSABUN, 5, '0')) ";
                SQL = SQL + ComNum.VBLF + "                            ELSE (SELECT DRNAME FROM KOSMOS_PMPA.BAS_DOCTOR WHERE DRCODE = DRSABUN)";
                SQL = SQL + ComNum.VBLF + "       END DRNAME  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_JUSAMIX ";
                SQL = SQL + ComNum.VBLF + " WHERE ENTDATE >= TO_DATE('" + dtpSDetail.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ENTDATE <= TO_DATE('" + dtpEDetail.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (rdoIO2_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND IPDOPD = 'I' ";
                }
                else if (rdoIO2_2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND IPDOPD = 'O' ";
                }

                if (rdoBun2_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN IN ('1','2') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN = '3' ";
                }

                if (txtPano2.Text.Trim() != "" && txtPano2.Text.Length > 7)
                {
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + txtPano2.Text.Trim() + "'";
                }

                SQL = SQL + ComNum.VBLF + "   AND DOSNAME IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " GROUP BY IPDOPD, PANO, SNAME, SEX, AGE, WARDCODE, DEPTCODE, DRSABUN";
                SQL = SQL + ComNum.VBLF + " ORDER BY SNAME, WARDCODE, ROOMCODE";

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
                        ssList_Sheet1.Cells[i, 0].Text  = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 0].Tag   = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text  = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Tag   = dt.Rows[i]["SEXAGE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text  = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text  = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text  = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text  = dt.Rows[i]["DRNAME"].ToString().Trim();

                        ssList_Sheet1.SetRowHeight(i, Convert.ToInt32(ssList_Sheet1.GetPreferredRowHeight(i)) + 5);
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

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtSname2.Text     = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtPanoDetail.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtAgeDetail.Text  = ssList_Sheet1.Cells[e.Row, 1].Tag.ToString().Trim();
            string IPDOPD = ssList_Sheet1.Cells[e.Row, 0].Tag.ToString().Trim();

            //txtDiagnosis.Text  = READ_ILLS(IPDOPD
            //                                                      , txtPanoDetail.Text
            //                                                      , dt.Rows[i]["DEPTCODE"].ToString().Trim()
            //                                                      , dt.Rows[i]["BDATE"].ToString().Trim());


            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView2_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT	TO_CHAR(BDATE, 'YYYY-MM-DD') BDATE                                                                                            ";
                SQL = SQL + ComNum.VBLF + "	,	WARDCODE                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "	,	ROOMCODE                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "	,	(SELECT LISTAGG(TRIM(JEPCODE) || ' : ' || REALQTY, '\r\n') WITHIN GROUP(ORDER BY SEQNO)                                           ";
                SQL = SQL + ComNum.VBLF + "	       FROM KOSMOS_ADM.DRUG_JUSAMIX                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "	      WHERE SEQNO = A.SEQNO                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "	        AND JEPCODE IS NOT NULL                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "	    ) AS ORDERS                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "	,	CASE WHEN IPDOPD = 'I' THEN (SELECT EMP_NM FROM KOSMOS_ERP.HR_EMP_BASIS WHERE EMP_ID = LPAD(DRSABUN, 5, '0'))                     ";
                SQL = SQL + ComNum.VBLF + "							   ELSE (SELECT DRNAME FROM KOSMOS_PMPA.BAS_DOCTOR WHERE DRCODE = DRSABUN)                                    ";
                SQL = SQL + ComNum.VBLF + "		END DRNAME				                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_JUSAMIX A                                                                                                       ";
                SQL = SQL + ComNum.VBLF + " WHERE ENTDATE >= TO_DATE('" + dtpSDetail.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                                              ";
                SQL = SQL + ComNum.VBLF + "   AND ENTDATE <= TO_DATE('" + dtpEDetail.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                                              ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + txtPanoDetail.Text + "'                                                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND IPDOPD = '" + IPDOPD +  "' ";

                if (rdoBun2_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN IN ('1','2') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN = '3' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND DOSNAME IS NOT NULL                                                                                                             ";
                SQL = SQL + ComNum.VBLF + " GROUP BY ENTDATE, BDATE, IPDOPD, PANO, SNAME, SEX, AGE, WARDCODE, ROOMCODE, DEPTCODE, SEQNO, DRSABUN                                  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ENTDATE, SEQNO                                                                                                               ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ORDERS"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        ssView2_Sheet1.SetRowHeight(i, Convert.ToInt32(ssView2_Sheet1.GetPreferredRowHeight(i)) + 5);
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
    }
}
