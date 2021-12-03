using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMisuSunap.cs
    /// Description     : 외래/입원 계약처미수 발생내역(수납기준)
    /// Author          : 박창욱
    /// Create Date     : 2017-09-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR209.frm(FrmSunapView.frm) >> frmPmpaViewMisuSunap.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMisuSunap : Form
    {
        string[] GstrGels = new string[401];

        public frmPmpaViewMisuSunap()
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
                return;
            }

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string PrintDate = "";
            string Jname = "";

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            Jname = clsType.User.JobName;

            //Print Head
            strFont1 = "/c/fn\"굴림체\" /fz\"15\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/l/f1" + VB.Space(20) + " 계약처 미수 발생 현황(수납기준) " + "/f1/n" + "  " + "/f1/n";
            strHead2 = "/l/f2" + "출력시간 : " + PrintDate + VB.Space(10) + "출력자 : " + Jname;

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 20;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            string strOldData = "";
            string strNewData = "";

            string strFDate = "";
            string strTDate = "";
            long nAmt = 0;

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            ssView_Sheet1.RowCount = 0;
            nRow = 0;
            nAmt = 0;
            strOldData = "";

            try
            {
                SQL = "";
                if (chkPano.Checked == true)  //등록번호순으로
                {
                    SQL = SQL + ComNum.VBLF + " SELECT b.GelCode,a.Pano,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,'1' IpdOpd,  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SELECT b.GelCode,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,'1' IpdOpd,a.Pano,  ";
                }
                SQL = SQL + ComNum.VBLF + "        a.Bi,a.DeptCode,b.Sname,                 ";
                SQL = SQL + ComNum.VBLF + "        SUM(a.Amt1+a.Amt2) Amt                                     ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP a, " + ComNum.DB_PMPA + "OPD_MASTER b  ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND a.BDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')     ";
                SQL = SQL + ComNum.VBLF + "    AND a.BDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')     ";
                if (chkY96Y.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.SuNext IN ('Y96Y')                                "; //남부경찰서,계약처
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.SuNext IN ('Y96J','Y96M')                                "; //남부경찰서,계약처
                }
                SQL = SQL + ComNum.VBLF + "    AND a.Pano=b.Pano(+)                                           ";
                SQL = SQL + ComNum.VBLF + "    AND a.BDate = b.BDate(+)                                   ";
                SQL = SQL + ComNum.VBLF + "    AND a.DeptCode = b.DeptCode(+)                                   ";

                if (VB.Left(cboWon.Text, 1) == "2")
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.GELCODE IN ( SELECT RTRIM(MIACODE) FROM BAS_MIA WHERE GBWON = '*' ) ";
                }
                else if (VB.Left(cboWon.Text, 1) == "3")
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.GELCODE NOT IN ( SELECT RTRIM(MIACODE) FROM BAS_MIA WHERE GBWON = '*' ) ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY b.GelCode,a.BDate,a.Pano,a.Bi,a.DeptCode,b.Sname          ";
                SQL = SQL + "  UNION ALL                                                        ";
                if (chkPano.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.GelCode,a.Pano,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,'2' IpdOpd,             ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SELECT a.GelCode,TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,'2' IpdOpd,a.Pano,             ";
                }
                SQL = SQL + ComNum.VBLF + "        a.Bi,a.DeptCode,b.Sname,                 ";
                SQL = SQL + ComNum.VBLF + "        SUM(a.Amt) Amt                                             ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH a, " + ComNum.DB_PMPA + "BAS_PATIENT b       ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND a.BDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')     ";
                SQL = SQL + ComNum.VBLF + "    AND a.BDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')     ";
                if (chkY96Y.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.SuNext IN ('Y96Y')                                "; //남부경찰서,계약처
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.SuNext IN ('Y96J','Y96M')                                "; //남부경찰서,계약처
                }
                SQL = SQL + ComNum.VBLF + "    AND a.Pano=b.Pano(+)                                           ";

                if (VB.Left(cboWon.Text, 1) == "2")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.GELCODE IN ( SELECT RTRIM(MIACODE) FROM BAS_MIA WHERE GBWON = '*' ) ";
                }
                else if (VB.Left(cboWon.Text, 1) == "3")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.GELCODE NOT IN ( SELECT RTRIM(MIACODE) FROM BAS_MIA WHERE GBWON = '*' ) ";
                }

                if (chkPano.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  GROUP BY a.GelCode,a.Pano,a.BDate,a.Bi,a.DeptCode,b.Sname        ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  GROUP BY a.GelCode,a.BDate,a.Pano,a.Bi,a.DeptCode,b.Sname        ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY 1,2,3,4,5                                               ";

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
                    if (VB.Val(dt.Rows[i]["Amt"].ToString().Trim()) != 0)
                    {
                        nRow += 1;
                        if (ssView_Sheet1.RowCount < nRow)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        strNewData = dt.Rows[i]["GelCode"].ToString().Trim();
                        if (strOldData != strNewData)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = READ_BAS_MIA(strNewData);
                            strOldData = strNewData;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "1")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "외래";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "입원";
                        }
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0 ");
                        nAmt += (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                nRow += 1;
                if (ssView_Sheet1.RowCount < nRow)
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow - 1, 6].Text = nAmt.ToString("###,###,###,##0");
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string READ_BAS_MIA(string argCode)
        {
            string rtnVal = "";

            if (argCode.Trim() == "")
            {
                rtnVal = "계약코드 미등록";
                return rtnVal;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT MiaName";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL = SQL + ComNum.VBLF + "  WHERE MiaCode = '" + argCode.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = "계약코드 미등록";
                    return rtnVal;
                }
                else
                {
                    rtnVal = dt.Rows[0]["MiaName"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void frmPmpaViewMisuSunap_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //this.Close();
            //return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            cboWon.Items.Clear();
            cboWon.Items.Add("1.전체");
            cboWon.Items.Add("2.원무행정");
            cboWon.Items.Add("3.계약처");
            cboWon.SelectedIndex = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
        }
    }
}
