using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstEndoList.cs
    /// Description     : 내시경실 약 조회
    /// Author          : 이정현
    /// Create Date     : 2017-10-31
    /// <history> 
    /// 내시경실 약 조회
    /// </history>
    /// <seealso>
    /// PSMH\drug\drEndoTong\FrmEndoLIST.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drEndoTong\drEndoTong.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstEndoList : Form
    {
        public frmSupDrstEndoList()
        {
            InitializeComponent();
        }

        private void frmSupDrstEndoList_Load(object sender, EventArgs e)
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

            SetSpreadFilter();

            cboSort.Items.Clear();
            cboSort.Items.Add("등록번호");
            cboSort.Items.Add("일자");
            cboSort.Items.Add("약코드");
            cboSort.SelectedIndex = 0;

            ssView_Sheet1.RowCount = 1;
        }
        
        private void SetSpreadFilter()
        {
            clsSpread methodSpd = new clsSpread();

            for (int i = 0; i < ssView.ActiveSheet.ColumnCount; i++)
            {
                methodSpd.setSpdFilter(ssView, i, AutoFilterMode.EnhancedContextMenu, true);
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SQL2 = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strDosCode = "";
            string strPtNo = "";
            
            ssView_Sheet1.RowCount = 0;

            try
            {
                //내시경과 관련된 용법 코드 읽어오기
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DOSCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + "     WHERE DOSFULLCODE LIKE '%내시%' OR DOSNAME LIKE '%내시%' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDosCode = strDosCode + "'" + dt.Rows[i]["DOSCODE"].ToString().Trim() + "',";
                    }

                    strDosCode = VB.Mid(strDosCode, 1, strDosCode.Length - 1);
                }

                dt.Dispose();
                dt = null;

                SQL = "";

                if (rdoGubun0.Checked == true)
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PTNO, BDATE, SUCODE, SUM(QTY * NAL) AS QTY, 'I' AS GBIO, DRCODE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE IN (" + strDosCode + ") ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BUN IN ('11','12','20') ";
                    
                    if (txtSuCode.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + txtSuCode.Text.Trim() + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY PTNO, BDATE, SUCODE, 'I', DRCODE  ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(Qty * NAL) > 0 ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PTNO, BDATE, SUCODE, SUM(QTY * NAL) AS QTY, 'O' AS GBIO, DRCODE  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE IN (" + strDosCode + ") ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BUN IN ('11','12','20') ";

                    if (txtSuCode.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + txtSuCode.Text.Trim() + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY PTNO, BDATE, SUCODE, 'O' , DRCODE  ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL) > 0 ";
                }
                else if (rdoGubun1.Checked == true)
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PTNO, BDATE, SUCODE, SUM(QTY * NAL) AS QTY, 'O' AS GBIO, DRCODE  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE IN (" + strDosCode + ") ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BUN IN ('11','12','20') ";

                    if (txtSuCode.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + txtSuCode.Text.Trim() + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY PTNO, BDATE, SUCODE, 'O' , DRCODE  ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(QTY * NAL) > 0 ";
                }
                else if (rdoGubun2.Checked == true)
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PTNO, BDATE, SUCODE, SUM(QTY * NAL) AS QTY, 'I' AS GBIO, DRCODE  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE IN (" + strDosCode + ") ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND BUN IN ('11','12','20') ";

                    if (txtSuCode.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + txtSuCode.Text.Trim() + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY PTNO, BDATE, SUCODE, 'I' , DRCODE ";
                    SQL = SQL + ComNum.VBLF + "HAVING SUM(Qty * NAL) > 0 ";
                }

                switch (cboSort.Text.Trim())
                {
                    case "일자":
                        SQL = SQL + ComNum.VBLF + "ORDER BY BDATE ";
                        break;
                    case "약코드":
                        SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "ORDER BY PTNO ";
                        break;
                }

                if (rdoGroup1.Checked == true)
                {
                    SQL2 = "";
                    SQL2 = SQL2 + ComNum.VBLF + "SELECT * ";
                    SQL2 = SQL2 + ComNum.VBLF + "FROM ";
                    SQL2 = SQL2 + ComNum.VBLF + "( ";
                    SQL2 = SQL2 + ComNum.VBLF + "   SELECT";
                    SQL2 = SQL2 + ComNum.VBLF + "        '' PTNO, BDATE, SUCODE, SUM(QTY) QTY, '' GBIO, '' DRCODE ";
                    SQL2 = SQL2 + ComNum.VBLF + "   FROM";
                    SQL2 = SQL2 + ComNum.VBLF + "      (";
                    SQL2 = SQL2 + ComNum.VBLF + "          " + SQL;
                    SQL2 = SQL2 + ComNum.VBLF + "      )";
                    SQL2 = SQL2 + ComNum.VBLF + "   GROUP BY '', BDATE, SUCODE, '', ''";
                    SQL2 = SQL2 + ComNum.VBLF + ") ";
                    
                    if (chkOrder.Checked == true)
                    {
                        SQL2 = SQL2 + ComNum.VBLF + "WHERE SUCODE IN ( ";
                        SQL2 = SQL2 + ComNum.VBLF + "                    SELECT JEPCODE FROM KOSMOS_ADM.DRUG_SETCODE ";
                        SQL2 = SQL2 + ComNum.VBLF + "                    WHERE GUBUN = '02' ";
                        SQL2 = SQL2 + ComNum.VBLF + "                      AND DELDATE IS NULL ";
                        SQL2 = SQL2 + ComNum.VBLF + "                      AND VFLAG2 = '1' ";
                        SQL2 = SQL2 + ComNum.VBLF + "                ) ";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL2, clsDB.DbCon);
                    
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL2, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                }
                else
                {
                    SQL2 = "";
                    SQL2 = SQL2 + ComNum.VBLF + "SELECT * ";
                    SQL2 = SQL2 + ComNum.VBLF + "FROM ";
                    SQL2 = SQL2 + ComNum.VBLF + "( ";
                    SQL2 = SQL2 + ComNum.VBLF + "          " + SQL;
                    SQL2 = SQL2 + ComNum.VBLF + ") ";

                    if (chkOrder.Checked == true)
                    {
                        SQL2 = SQL2 + ComNum.VBLF + "WHERE SUCODE IN ( ";
                        SQL2 = SQL2 + ComNum.VBLF + "                    SELECT JEPCODE FROM KOSMOS_ADM.DRUG_SETCODE ";
                        SQL2 = SQL2 + ComNum.VBLF + "                    WHERE GUBUN = '02' ";
                        SQL2 = SQL2 + ComNum.VBLF + "                      AND DELDATE IS NULL ";
                        SQL2 = SQL2 + ComNum.VBLF + "                      AND VFLAG2 = '1' ";
                        SQL2 = SQL2 + ComNum.VBLF + "                ) ";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL2, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL2, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                }
                
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strPtNo != dt.Rows[i]["PTNO"].ToString().Trim())
                        {
                            strPtNo = dt.Rows[i]["PTNO"].ToString().Trim();

                            ssView_Sheet1.Cells[i, 0].Text = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                            ssView_Sheet1.Cells[i, 1].Text = strPtNo;
                            ssView_Sheet1.Cells[i, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PTNO"].ToString().Trim());

                            if (dt.Rows[i]["GBIO"].ToString().Trim() == "O")
                            {
                                ssView_Sheet1.Cells[i, 3].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                            }
                            else if (dt.Rows[i]["GBIO"].ToString().Trim() == "I")
                            {
                                ssView_Sheet1.Cells[i, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                            }
                        }

                        //2020-01-06
                        ssView_Sheet1.Cells[i, 4].Text = READ_JEHYENGBUN(dt.Rows[i]["SUCODE"].ToString().Trim());

                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim());

                        switch (dt.Rows[i]["GBIO"].ToString().Trim())
                        {
                            case "O":
                                ssView_Sheet1.Cells[i, 8].Text = "외래";
                                break;
                            case "I":
                                ssView_Sheet1.Cells[i, 8].Text = "병실";
                                break;
                            default:
                                ssView_Sheet1.Cells[i, 8].Text = "";
                                break;
                        }
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "내시경실 약 내역" + "/f1/n";
            strHead2 = "/l/f2" + "출력일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "/f2/n";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private string READ_JEHYENGBUN(string strSuCode)
        {
            string rtnVal = "";
            string SQL = "";            
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT JEPCODE, SUGABUN, JEHYENGBUN ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.DRUG_MASTER2 ";
                SQL = SQL + ComNum.VBLF + " WHERE JEPCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["JEHYENGBUN"].ToString().Trim())
                    {
                        case "01":
                            rtnVal = "01.주사약";
                            break;
                        case "02":
                            rtnVal = "02.수액제";
                            break;
                        case "03":
                            rtnVal = "03.경구약";
                            break;
                        case "04":
                            rtnVal = "04.외용제";
                            break;
                        case "05":
                            rtnVal = "05.생제약";
                            break;
                        case "06":
                            rtnVal = "06.인공신장실약";
                            break;
                        case "07":
                            rtnVal = "07.방사선과약";
                            break;
                        case "08":
                            rtnVal = "08.향정신성약품";
                            break;
                        case "09":
                            rtnVal = "09.마약";
                            break;
                        case "10":
                            rtnVal = "10.마취약";
                            break;
                        case "11":
                            rtnVal = "11.제제약";
                            break;
                    }
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
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
                return rtnVal;
            }
        }
    }
}
