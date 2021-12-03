using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaVIEWMonthTax.cs
    /// Description     : 진료부가세 상세내역 통계
    /// Author          : 김효성
    /// Create Date     : 2017-09-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\psmh\mir\miretc\FrmMonthTax.frm  >> frmPmpaVIEWMonthTax.cs 폼이름 재정의" />	

    public partial class frmPmpaVIEWMonthTax : Form
    {
        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc cf = new ComFunc();

        public frmPmpaVIEWMonthTax()
        {
            InitializeComponent();
        }

        private void frmPmpaVIEWMonthTax_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nYY = 0;
            int nMM = 0;

            ssView1_Sheet1.RowCount = 0;

            nYY = (int)VB.Val(VB.Left(strdtP, 4));
            nMM = (int)VB.Val(VB.Mid(strdtP, 6, 2));

            for (i = 1; i <= 60; i++)
            {
                cboFdate.Items.Add((nYY).ToString("0000") + "-" + (nMM).ToString("00"));
                cboTdate.Items.Add((nYY).ToString("0000") + "-" + (nMM).ToString("00"));
                nMM = nMM - 1;

                if (nMM == 0)
                {
                    nYY = nYY - 1;
                    nMM = 12;
                }
            }
            cboFdate.SelectedIndex = 1;
            cboTdate.SelectedIndex = 1;

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboFdate.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTdate.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            int nRow = 0;
            string strFDate = "";
            string strTDate = "";
            string strBi = "";
            string strPano = "";
            string strActdate = "";
            string strDept = "";
            string strName = "";
            string strBDate = "";
            string strDrCode = "";
            double nAmt = 0;
            double nTaxAmt = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            double nTot3 = 0;
            double nGTot1 = 0;
            double nGTot2 = 0;
            double nGTot3 = 0;
            double nGTot4 = 0;
            DataTable dt = null;
            DataTable dtSub = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView1_Sheet1.RowCount = 0;

            strFDate = VB.Left(cboFdate.Text, 4) + "-" + VB.Mid(cboFdate.Text, 6, 2) + "-01";
            strTDate = VB.Left(cboTdate.Text, 4) + "-" + VB.Mid(cboTdate.Text, 6, 2) + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strTDate);

            nTot1 = 0;
            nTot2 = 0;
            nTot3 = 0;
            nGTot1 = 0;
            nGTot2 = 0;
            nGTot3 = 0;
            nGTot4 = 0;
            nRow = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,DeptCode,Pano,Bi,DrCode, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(BDate,'YYYY-MM-DD') BDate ";
                SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + " Where 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND Actdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Actdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND AMT4 <> 0 ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN ('DM','FM','dt','EN') ";// '진료부가세 발생
                SQL = SQL + ComNum.VBLF + " GROUP By ActDate,DeptCode,Pano,Bi,DrCode,BDate ";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(AMT4) <> 0 ";
                SQL = SQL + ComNum.VBLF + " ORDER By ActDate, Pano";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //스프레드 출력문
                ssView1_Sheet1.RowCount = dt.Rows.Count;
                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    SQL = "SELECT SNAME From " + ComNum.DB_PMPA + "BAS_PATIENT WHERE PANO='" + strPano + "' ";

                    SqlErr = clsDB.GetDataTable(ref dtSub, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dtSub.Rows.Count == 0)
                    {

                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dtSub.Rows.Count > 0)
                    {
                        strName = dtSub.Rows[0]["SNAME"].ToString().Trim();
                    }
                    dtSub.Dispose();
                    dtSub = null;

                    strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strBDate = dt.Rows[i]["BDate"].ToString().Trim();
                    strActdate = dt.Rows[i]["ActDate"].ToString().Trim();
                    strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                    if (strPano == "08700542")
                    {
                        i = i;
                    }

                    if (string.Compare(strActdate, strFDate) >= 0 && string.Compare(strActdate, strTDate) <= 0)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT a.Sunext,b.SuNameK,Sum(a.amt1+a.amt2) SAmt, Sum(a.amt4) TaxAmt, Sum(DanAmt) DanAmt ";
                        SQL += ComNum.VBLF + "  From OPD_SLIP a, BAS_SUN b ";
                        SQL += ComNum.VBLF + "  Where 1=1";
                        SQL += ComNum.VBLF + "   AND a.Bdate = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "   AND a.Actdate = TO_DATE('" + strActdate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "   AND a.DeptCode = '" + strDept + "' ";
                        SQL += ComNum.VBLF + "   AND a.Pano ='" + strPano + "' ";
                        SQL += ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                        SQL += ComNum.VBLF + "   AND SUBSTR(a.SuNext,1,1) <> '@' ";
                        SQL += ComNum.VBLF + "   AND SUBSTR(a.SuNext,1,2) <> '##' ";
                        SQL += ComNum.VBLF + "   AND SUBSTR(a.SuNext,1,2) <> '$$' ";
                        SQL += ComNum.VBLF + "   AND a.BUN <= '90' ";
                        SQL += ComNum.VBLF + "   AND a.amt4 <> 0 ";
                        SQL += ComNum.VBLF + " Group By a.Sunext,b.SuNameK ";
                        SQL += ComNum.VBLF + " ORDER By a.Sunext ";

                        SqlErr = clsDB.GetDataTable(ref dtSub, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dtSub.Rows.Count == 0)
                        {

                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dtSub.Rows.Count > 0)
                        {
                            for (j = 0; j < dtSub.Rows.Count; j++)
                            {
                                nRow = nRow + 1;
                                if (ssView1_Sheet1.RowCount < nRow)
                                {
                                    ssView1_Sheet1.RowCount = nRow;
                                }

                                nAmt = Convert.ToInt32(dtSub.Rows[j]["SAMT"].ToString().Trim());
                                nTaxAmt = Convert.ToInt32(dtSub.Rows[j]["TAXAMT"].ToString().Trim());

                                nTot1 = nTot1 + nAmt;
                                nTot2 = nTot2 + nTaxAmt;

                                ssView1_Sheet1.Cells[nRow - 1, 0].Text = cf.Read_Bi_Name(clsDB.DbCon, strBi, "1");
                                ssView1_Sheet1.Cells[nRow - 1, 1].Text = strPano;
                                ssView1_Sheet1.Cells[nRow - 1, 2].Text = strName;
                                ssView1_Sheet1.Cells[nRow - 1, 3].Text = strDept;
                                ssView1_Sheet1.Cells[nRow - 1, 4].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrCode);
                                ssView1_Sheet1.Cells[nRow - 1, 5].Text = dtSub.Rows[j]["SUNAMEK"].ToString().Trim();
                                ssView1_Sheet1.Cells[nRow - 1, 6].Text = Convert.ToDouble(nAmt).ToString("###,###,##0");
                                ssView1_Sheet1.Cells[nRow - 1, 7].Text = Convert.ToDouble(nTaxAmt).ToString("###,###,##0");
                                ssView1_Sheet1.Cells[nRow - 1, 8].Text = strActdate;
                            }
                            dtSub.Dispose();
                            dtSub = null;
                            //'현금영수증 카드결제금액 표시
                            SQL = "";
                            SQL += ComNum.VBLF + "  SELECT GUBUN,  SUM(TRADEAMT) TAMT ";
                            SQL += ComNum.VBLF + "       FROM ADMIN.CARD_APPROV A ";
                            SQL += ComNum.VBLF + "     WHERE PANO ='" + strPano + "' ";
                            SQL += ComNum.VBLF + "         AND ( ACTDATE =TO_DATE('" + strActdate + "','YYYY-MM-DD') OR ACTDATE =TO_DATE('" + (dt.Rows[0]["BDate"].ToString().Trim() + "") + "','YYYY-MM-DD') ) ";
                            SQL += ComNum.VBLF + "         AND DEPTCODE ='" + strDept + "'  ";
                            SQL += ComNum.VBLF + "  GROUP BY GUBUN     ";
                            SQL += ComNum.VBLF + "  ORDER By GUBUN ";
                            SqlErr = clsDB.GetDataTable(ref dtSub, SQL, clsDB.DbCon);

                            for (j = 0; j < dtSub.Rows.Count; j++)
                            {
                                if (dtSub.Rows[j]["Gubun"].ToString().Trim() == "1")  //카드
                                {
                                    ssView1_Sheet1.Cells[nRow - 1, 9].Text = Convert.ToDouble(dtSub.Rows[j]["TAMT"].ToString().Trim()).ToString("###,###,##0");
                                    nGTot3 = nGTot3 + VB.Val(dtSub.Rows[j]["TAMT"].ToString().Trim());
                                }
                                else
                                {
                                    ssView1_Sheet1.Cells[nRow - 1, 10].Text = Convert.ToDouble(dtSub.Rows[j]["TAMT"].ToString().Trim()).ToString("###,###,##0");
                                    nGTot4 = nGTot4 + VB.Val(dtSub.Rows[j]["TAMT"].ToString().Trim());
                                }
                            }

                            dtSub.Dispose();
                            dtSub = null;

                            nRow = nRow + 1;
                            if (ssView1_Sheet1.RowCount < nRow)
                            {
                                ssView1_Sheet1.RowCount = nRow;
                            }
                            ssView1_Sheet1.Cells[nRow - 1, 5].Text = "소      계";
                            ssView1_Sheet1.Cells[nRow - 1, 6].Text = Convert.ToDouble(nTot1).ToString("###,###,##0");
                            ssView1_Sheet1.Cells[nRow - 1, 7].Text = Convert.ToDouble(nTot2).ToString("###,###,##0");

                            nGTot1 = nGTot1 + nTot1;
                            nGTot2 = nGTot2 + nTot2;

                            nTot1 = 0;
                            nTot2 = 0;
                        }

                    }
                }

                nRow = nRow + 1;
                if (ssView1_Sheet1.RowCount < nRow)
                {
                    ssView1_Sheet1.RowCount = nRow;
                }
                ssView1_Sheet1.Cells[nRow - 1, 5].Text = "합      계";
                ssView1_Sheet1.Cells[nRow - 1, 6].Text = Convert.ToDouble(nGTot1).ToString("###,###,##0");
                ssView1_Sheet1.Cells[nRow - 1, 7].Text = Convert.ToDouble(nGTot2).ToString("###,###,##0");
                ssView1_Sheet1.Cells[nRow - 1, 9].Text = Convert.ToDouble(nGTot3).ToString("###,###,##0");
                ssView1_Sheet1.Cells[nRow - 1, 10].Text = Convert.ToDouble(nGTot4).ToString("###,###,##0");

                dt.Dispose();
                dt = null;

                //'원단위절사 금액 합계
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sum(DanAmt) DanAmt ";
                SQL += ComNum.VBLF + "  From " + ComNum.DB_PMPA + "OPD_SLIP a, " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL += ComNum.VBLF + " Where a.Actdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.Actdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.SuNext=b.SuNext(+) ";
                SQL += ComNum.VBLF + "   AND SUBSTR(a.SuNext,1,1) <> '@' ";
                SQL += ComNum.VBLF + "   AND SUBSTR(a.SuNext,1,2) <> '##' ";
                SQL += ComNum.VBLF + "   AND SUBSTR(a.SuNext,1,2) <> '$$' ";
                SQL += ComNum.VBLF + "   AND a.BUN = '97' ";

                SqlErr = clsDB.GetDataTable(ref dtSub, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dtSub.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dtSub.Rows.Count > 0)
                {
                    nRow = nRow + 1;
                    if (ssView1_Sheet1.RowCount < nRow)
                    {
                        ssView1_Sheet1.RowCount = nRow;
                    }

                    nTot3 = VB.Val(dtSub.Rows[0]["DanAmt"].ToString().Trim());

                    ssView1_Sheet1.Cells[nRow - 1, 5].Text = "원단위절삭";
                    ssView1_Sheet1.Cells[nRow - 1, 6].Text = "0";
                    ssView1_Sheet1.Cells[nRow - 1, 7].Text = Convert.ToDouble(dtSub.Rows[0]["DanAmt"].ToString().Trim()).ToString("###,###,##0");
                }
                dtSub.Dispose();
                dtSub = null;

                nRow = nRow + 1;
                if (ssView1_Sheet1.RowCount < nRow)
                {
                    ssView1_Sheet1.RowCount = nRow;
                }
                ssView1_Sheet1.Cells[nRow - 1, 5].Text = "총      계";
                ssView1_Sheet1.Cells[nRow - 1, 6].Text = Convert.ToDouble(nGTot1).ToString("###,###,##0");
                ssView1_Sheet1.Cells[nRow - 1, 7].Text = Convert.ToDouble(nGTot2 - nTot3).ToString("###,###,##0");

                ssView1_Sheet1.Cells[nRow - 1, 9].Text = Convert.ToDouble(nGTot3).ToString("###,###,##0");
                ssView1_Sheet1.Cells[nRow - 1, 10].Text = Convert.ToDouble(nGTot4).ToString("###,###,##0");
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnexcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            bool x = false;

            if (ComFunc.MsgBoxQ("파일로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                return;

            x = ssView1.SaveExcel("C:\\진료부가세 상세내역 통계.xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
            {
                if (x == true)
                    ComFunc.MsgBox("엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string strFont3 = "";
            string strHead3 = "";

            strFont1 = "/fn\"굴림체\" /fz\"20\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/l";
            strFont3 = "/fn\"굴림체\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs3";

            strHead1 = "/c/f1" + "진료부가세 상세내역" + "/f1/n";   //제목 센터
            strHead2 = "/l/f2" + VB.Space(15) + "작업월 : " + (cboFdate.Text) + " ~" + (cboTdate.Text) + "/f2";
            strHead3 = "/l/f2" + "인쇄일자 : " + strdtP;

            btnPrint.Enabled = false;

            ssView1_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView1_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont3 + strHead2 + strFont3 + strHead3;
            ssView1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView1_Sheet1.PrintInfo.Margin.Top = 50;
            ssView1_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView1_Sheet1.PrintInfo.ShowBorder = true;
            ssView1_Sheet1.PrintInfo.ShowColor = false;
            ssView1_Sheet1.PrintInfo.ShowGrid = false;
            ssView1_Sheet1.PrintInfo.ShowShadows = true;
            ssView1_Sheet1.PrintInfo.UseMax = true;
            ssView1_Sheet1.PrintInfo.PrintType = 0;
            ssView1.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }
    }
}
