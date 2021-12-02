using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMisuManager.cs
    /// Description     : 미수금관리표
    /// Author          : 박창욱
    /// Create Date     : 2017-09-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs81.frm(FrmMisuManager.frm) >> frmPmpaViewMisuManager.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMisuManager : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaMisu cpm = new clsPmpaMisu();

        string FstrYYMM = "";
        string FstrFDate = "";
        string FstrTDate = "";
        string FstrIO = "";
        double GnWRTNO = 0;

        public frmPmpaViewMisuManager()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;
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
            string strIO = "";
            bool PrePrint = true;

            if (rdoIO0.Checked == true)
            {
                strIO = "(외래)";
            }
            else if (rdoIO1.Checked == true)
            {
                strIO = "(입원)";
            }
            else if (rdoIO2.Checked == true)
            {
                strIO = "(전체)";
            }

            strTitle = "월별 미수금 관리 총괄표 " + strIO;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strIO = "";
            bool PrePrint = true;

            if (rdoIO0.Checked == true)
            {
                strIO = "(외래)";
            }
            else if (rdoIO1.Checked == true)
            {
                strIO = "(입원)";
            }
            else if (rdoIO2.Checked == true)
            {
                strIO = "(전체)";
            }

            strTitle = "월별 미수금 관리 상세내역표 " + strIO;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);
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
            int nClass = 0;
            double[,] nTotAmt = new double[7, 6];

            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;

            FstrYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            FstrFDate = VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01";
            FstrTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(FstrYYMM, 4)), (int)VB.Val(VB.Right(FstrYYMM, 2)));

            if (rdoIO0.Checked == true)
            {
                FstrIO = "O";
            }
            if (rdoIO1.Checked == true)
            {
                FstrIO = "I";
            }
            if (rdoIO2.Checked == true)
            {
                FstrIO = "*";
            }

            //누적할 배열을 Clear
            for (i = 0; i < 7; i++)
            {
                for (k = 1; k < 6; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }

            try
            {
                //미수 총액을 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Class, SUM(Amt2) Amt2, SUM(Amt3) Amt3,";
                SQL = SQL + ComNum.VBLF + "       SUM(Amt4) amt4, sum(amt8) Amt8, SUM(Amt5) Amt5, ";
                SQL = SQL + ComNum.VBLF + "       SUM(Amt6) Amt6, SUM(Amt7) Amt7";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND BDate >= TO_DATE('" + FstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate <= TO_DATE('" + FstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Class <= '07' ";
                if (FstrIO == "O")
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd = 'O' "; //외래
                }
                else if (FstrIO == "I")
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd = 'I' "; //입원
                }
                SQL = SQL + ComNum.VBLF + "GROUP BY Class ";

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
                    switch (dt.Rows[i]["Class"].ToString().Trim())
                    {
                        case "01":
                        case "02":
                        case "03":
                            nClass = 1;
                            break;
                        case "04":
                            nClass = 2;
                            break;
                        case "05":
                            nClass = 3;
                            break;
                        case "07":
                            nClass = 4;
                            break;
                        default:
                            nClass = 1;
                            break;
                    }

                    nTotAmt[1, nClass] += VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()); //청구
                    nTotAmt[2, nClass] += VB.Val(dt.Rows[i]["Amt3"].ToString().Trim()); //입금
                    nTotAmt[2, nClass] += VB.Val(dt.Rows[i]["Amt6"].ToString().Trim()); //과지급금
                    nTotAmt[2, nClass] += VB.Val(dt.Rows[i]["Amt7"].ToString().Trim()); //계산착오
                    nTotAmt[3, nClass] += VB.Val(dt.Rows[i]["Amt5"].ToString().Trim()); //반송
                    nTotAmt[4, nClass] += VB.Val(dt.Rows[i]["Amt4"].ToString().Trim()) + VB.Val(dt.Rows[i]["Amt8"].ToString().Trim()); //삭감
                }
                dt.Dispose();
                dt = null;


                //입금소계, 미수잔액을 계산
                for (i = 1; i < 5; i++)
                {
                    nTotAmt[5, i] = nTotAmt[2, i] + nTotAmt[3, i] + nTotAmt[4, i];
                    nTotAmt[6, i] = nTotAmt[1, i] - nTotAmt[5, i];
                }

                //합계를 구함
                for (i = 1; i < 7; i++)
                {
                    for (k = 1; k < 5; k++)
                    {
                        nTotAmt[i, 5] += nTotAmt[i, k];
                    }
                }


                //집계된 금액을 Display
                for (i = 1; i < 7; i++)
                {
                    if (i == 1)
                    {
                        for (k = 1; k < 6; k++)
                        {
                            ssView_Sheet1.Cells[0, k].Text = nTotAmt[i, k].ToString("###,###,###,##0 ");
                        }
                    }
                    else if (i < 6)
                    {
                        for (k = 1; k < 6; k++)
                        {
                            ssView_Sheet1.Cells[i, k].Text = nTotAmt[i, k].ToString("###,###,###,##0 ");
                        }
                    }
                    else
                    {
                        for (k = 1; k < 6; k++)
                        {
                            ssView_Sheet1.Cells[i + 1, k].Text = nTotAmt[i, k].ToString("###,###,###,##0 ");
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMisuManager_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView2_Sheet1.Columns[13].Visible = false;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 36, "", "1");
            cboYYMM.SelectedIndex = 0;
            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }


        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 1, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            long nJanAmt = 0;
            double[] nTotAmt = new double[7];
            string strOK = "";

            if (e.Column < 1 || e.Column > 5)
            {
                return;
            }

            if (e.Row ==1 || e.Row==5 || e.Row == 6)
            {
                return;
            }
            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;

            for (i = 1; i < 7; i++)
            {
                nTotAmt[i] = 0;
            }

            try
            {
                //미수 상세내역을 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, b.MiaName, a.Class,";
                SQL = SQL + ComNum.VBLF + "       a.MisuID, a.IpdOpd, a.Bun,";
                SQL = SQL + ComNum.VBLF + "       a.Amt2, a.Amt3, a.Amt4 amt4,";
                SQL = SQL + ComNum.VBLF + "       a.amt8 amt8, a.Amt5, a.Amt6 + a.Amt7 Amt6,";
                SQL = SQL + ComNum.VBLF + "       a.WRTNO";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST a, " + ComNum.DB_PMPA + "BAS_MIA b ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND a.BDate>=TO_DATE('" + FstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.BDate<=TO_DATE('" + FstrTDate + "','YYYY-MM-DD') ";
                if (FstrIO != "*")
                {
                    SQL = SQL + ComNum.VBLF + "   AND a.IpdOpd='" + FstrIO + "' ";
                }
                //미수종류
                switch (e.Column)
                {
                    case 1:
                        SQL = SQL + ComNum.VBLF + "   AND a.Class IN ('01','02','03') ";
                        break;
                    case 2:
                        SQL = SQL + ComNum.VBLF + "   AND a.Class = '04' ";
                        break;
                    case 3:
                        SQL = SQL + ComNum.VBLF + "   AND a.Class = '05' ";
                        break;
                    case 4:
                        SQL = SQL + ComNum.VBLF + "   AND a.Class = '07' ";
                        break;
                    case 5:
                        SQL = SQL + ComNum.VBLF + "   AND a.Class IN ('01','02','03','04','05','07') ";
                        break;
                }
                //구분별
                switch (e.Row)
                {
                    case 0:
                        SQL = SQL + ComNum.VBLF + "   AND a.Amt2 <> 0 ";
                        break;
                    case 2:
                        SQL = SQL + ComNum.VBLF + "   AND (a.Amt3 <> 0 Or a.Amt6 <> 0 Or a.Amt7 <> 0) ";
                        break;
                    case 3:
                        SQL = SQL + ComNum.VBLF + "   AND a.Amt5 <> 0 ";
                        break;
                    case 4:
                        SQL = SQL + ComNum.VBLF + "   AND a.Amt4 <> 0 ";
                        break;
                    case 6:
                        SQL = SQL + ComNum.VBLF + "   AND a.GbEnd = '0' ";
                        break;
                }
                SQL = SQL + ComNum.VBLF + "   AND RTRIM(a.GelCode)=b.MiaCode(+) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.BDate,b.MiaName,a.Class,a.MisuID ";
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
                nRow = 0;

                for (i = 0; i < nRead; i++)
                {
                    //미수 잔액을 계산
                    nJanAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()));
                    nJanAmt -= Convert.ToInt32(VB.Val(dt.Rows[i]["Amt3"].ToString().Trim()));
                    nJanAmt -= Convert.ToInt32(VB.Val(dt.Rows[i]["Amt4"].ToString().Trim()));
                    nJanAmt -= Convert.ToInt32(VB.Val(dt.Rows[i]["Amt8"].ToString().Trim()));
                    nJanAmt -= Convert.ToInt32(VB.Val(dt.Rows[i]["Amt5"].ToString().Trim()));
                    nJanAmt -= Convert.ToInt32(VB.Val(dt.Rows[i]["Amt6"].ToString().Trim()));

                    strOK = "OK";
                    if (e.Row == 3 && VB.Val(dt.Rows[i]["Amt5"].ToString().Trim()) == 0)
                    {
                        strOK = ""; //반송
                    }
                    if (e.Row == 4 && (VB.Val(dt.Rows[i]["Amt4"].ToString().Trim()) + VB.Val(dt.Rows[i]["Amt8"].ToString().Trim())) == 0)
                    {
                        strOK = ""; //삭감
                    }
                    if (e.Row == 7 && nJanAmt == 0)
                    {
                        strOK = "";
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (nRow > ssView2_Sheet1.RowCount)
                        {
                            ssView2_Sheet1.RowCount = nRow;
                        }
                        ssView2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssView2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["MiaName"].ToString().Trim();
                        switch (dt.Rows[i]["Class"].ToString().Trim())
                        {
                            case "01":
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = "공단";
                                break;
                            case "02":
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = "직장";
                                break;
                            case "03":
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = "지역";
                                break;
                            case "04":
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = "보호";
                                break;
                            case "05":
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = "산재";
                                break;
                            case "07":
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = "자보";
                                break;
                            default:
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = "";
                                break;
                        }
                        ssView2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                        if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "O")
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 4].Text = "외래";
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 4].Text = "입원";
                        }

                        if (string.Compare(dt.Rows[i]["Class"].ToString().Trim(), "04") <= 0)
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 5].Text = cpm.READ_MisuBunName(dt.Rows[i]["Bun"].ToString().Trim());
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 5].Text = "";
                        }

                        ssView2_Sheet1.Cells[nRow - 1, 6].Text = VB.DateDiff("D", Convert.ToDateTime(dt.Rows[i]["BDate"].ToString().Trim()),
                                                                 Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"))).ToString();
                        ssView2_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()).ToString("###,###,###,##0 ");
                        ssView2_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["Amt3"].ToString().Trim()).ToString("###,###,###,##0 ");
                        ssView2_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt.Rows[i]["Amt5"].ToString().Trim()).ToString("###,###,###,##0 ");
                        ssView2_Sheet1.Cells[nRow - 1, 10].Text = (VB.Val(dt.Rows[i]["Amt4"].ToString().Trim()) + VB.Val(dt.Rows[i]["Amt8"].ToString().Trim())).ToString("###,###,###,##0 ");
                        ssView2_Sheet1.Cells[nRow - 1, 11].Text = VB.Val(dt.Rows[i]["Amt6"].ToString().Trim()).ToString("###,###,###,##0 ");
                        ssView2_Sheet1.Cells[nRow - 1, 12].Text = nJanAmt.ToString("###,###,###,##0 ");
                        ssView2_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                        nTotAmt[1] += VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()); //청구액
                        nTotAmt[2] += VB.Val(dt.Rows[i]["Amt3"].ToString().Trim()); //입금액
                        nTotAmt[3] += VB.Val(dt.Rows[i]["Amt5"].ToString().Trim()); //반송액
                        nTotAmt[4] += VB.Val(dt.Rows[i]["Amt4"].ToString().Trim()) + VB.Val(dt.Rows[i]["Amt8"].ToString().Trim()); //삭감액
                        nTotAmt[5] += VB.Val(dt.Rows[i]["Amt6"].ToString().Trim()); //기타입금
                        nTotAmt[6] += nJanAmt;                         //잔액
                    }
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView2_Sheet1.RowCount = nRow;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView2_Sheet1.Cells[nRow - 1, 1].Text = "** 합계 **";
                for (i = 1; i < 7; i++)
                {
                    ssView2_Sheet1.Cells[nRow - 1, i + 6].Text = nTotAmt[i].ToString("###,###,###,##0 ");
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView2_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView2_Sheet1.Cells[0, 0, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView2_Sheet1.Cells[e.Row, 0, e.Row, ssView2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GnWRTNO = VB.Val(ssView2_Sheet1.Cells[e.Row, 13].Text);

            
            frmPmpaViewPanoMisuDtl frm = new frmPmpaViewPanoMisuDtl(GnWRTNO);


            frm.ShowDialog();
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
