using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMonthMisu_NEW.cs
    /// Description     : (경리과용)월별개인/기타총괄표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs20.frm(FrmMonthMisu_NEW.frm) >> frmPmpaViewMonthMisu_NEW.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMonthMisu_NEW : Form
    {
        double[,] FnAmt = new double[25, 9];
        clsPmpaMisu cpm = new clsPmpaMisu();

        public frmPmpaViewMonthMisu_NEW()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string READ_CLASS(string argClass)
        {
            string rtnVal = "";

            switch (argClass)
            {
                case "01":
                    rtnVal = "공단";
                    break;
                case "02":
                    rtnVal = "직장";
                    break;
                case "03":
                    rtnVal = "지역";
                    break;
                case "04":
                    rtnVal = "보호";
                    break;
                case "05":
                    rtnVal = "산재";
                    break;
                case "07":
                    rtnVal = "자보";
                    break;
                case "08":
                    rtnVal = "계약처";
                    break;
                case "09":
                    rtnVal = "헌혈미수";
                    break;
                case "11":
                    rtnVal = "보훈청";
                    break;
                case "12":
                    rtnVal = "시각장애";
                    break;
                case "13":
                    rtnVal = "심신장애";
                    break;
                case "14":
                    rtnVal = "장애보장구";
                    break;
                case "15":
                    rtnVal = "직원대납";
                    break;
                case "16":
                    rtnVal = "장기요양소견";
                    break;
                case "17":
                    rtnVal = "가정방문소견";
                    break;
                default:
                    rtnVal = "기타미수";
                    break;
            }

            return rtnVal;
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

            strTitle = "(재무회계팀용)월별 개인/기타 미수금 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업월: " + cboFYYMM.Text + "~" + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

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
            int nRow = 0;
            int nRow2 = 0;
            int nRead = 0;
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";

            //누적할 배열을 Clear
            for (i = 0; i < 25; i++)
            {
                for (k = 0; k < 9; k++)
                {
                    FnAmt[i, k] = 0;
                }
            }

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 100;

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);
            strTdate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strTYYMM, 4)), Convert.ToInt32(VB.Right(strTYYMM, 2)));

            nRow = 0;
            ssView_Sheet1.Cells[0, 0].Text = "개" + ComNum.VBLF + ComNum.VBLF + "인" + ComNum.VBLF + ComNum.VBLF + "미" + ComNum.VBLF + ComNum.VBLF + "수";
            try
            {
                //월별 개인미수 통계 ADD
                #region GainMisu_Monthly_ADD

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT GUBUN, SUM(DECODE(YYMM, '" + strFYYMM + "',IwolAmt,0)) IwolAmt, SUM(MonMAmt) MisuAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(MonIAmt) IpgumAmt, SUM(MONSAMT) SakAmt, 0 SAKAMT2, ";
                SQL = SQL + ComNum.VBLF + "       SUM(MONBAMT) BanAmt, SUM(MONGAMT) EtcAmt, SUM(DECODE(YYMM,'" + strTYYMM + "',JanAmt,0)) JanAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_GAINTONG ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND YYMM >= '" + strFYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strTYYMM + "' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY GUBUN ";

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
                    nRow += 1;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = cpm.READ_PerMisuGye(dt.Rows[i]["Gubun"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()).ToString("###,###,###,##0 ");   //전월이월
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()).ToString("###,###,###,##0 ");   //당월미수
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim()).ToString("###,###,###,##0 ");  //입금액
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()).ToString("###,###,###,##0 ");    //삭감액
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim()).ToString("###,###,###,##0 ");   //절사삭감액
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");    //반송액
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()).ToString("###,###,###,##0 ");    //기타입금
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");    //월말잔액


                    //소계에 ADD
                    FnAmt[1, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());      //전월이월
                    FnAmt[1, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());      //당월미수
                    FnAmt[1, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());     //입금액
                    FnAmt[1, 4] += VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim());       //삭감액
                    FnAmt[1, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());      //절사삭감액
                    FnAmt[1, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());       //반송액
                    FnAmt[1, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());       //기타입금
                    FnAmt[1, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());       //월말잔액


                    //합계에 ADD
                    FnAmt[3, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());      //전월이월
                    FnAmt[3, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());      //당월미수
                    FnAmt[3, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());     //입금액
                    FnAmt[3, 4] += VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim());       //삭감액
                    FnAmt[3, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());      //절사삭감액
                    FnAmt[3, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());       //반송액
                    FnAmt[3, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());       //기타입금
                    FnAmt[3, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());       //월말잔액
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView_Sheet1.Cells[nRow - 1, 1].Text = "소  계";
                for (k = 1; k < 9; k++)
                {
                    ssView_Sheet1.Cells[nRow - 1, k + 1].Text = FnAmt[1, k].ToString("###,###,###,##0 ");
                }
                ssView_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(224, 224, 224);

                #endregion

                ssView_Sheet1.AddSpanCell(0, 0, nRow, 1);
                FarPoint.Win.Spread.CellType.TextCellType tct = new FarPoint.Win.Spread.CellType.TextCellType();
                tct.Multiline = true;
                ssView_Sheet1.Cells[0, 0].CellType = tct;
                nRow2 = nRow;
                ssView_Sheet1.Cells[nRow, 0].Text = "기" + ComNum.VBLF + ComNum.VBLF + "타" + ComNum.VBLF + ComNum.VBLF + "미" + ComNum.VBLF + ComNum.VBLF + "수";

                //월별 미수번호별 통계 ADD
                #region Misu_Monthly_ADD

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.Class, SUM(DECODE(A.YYMM, '" + strFYYMM + "',A.IwolAmt,0)) IwolAmt, SUM(A.MisuAmt) MisuAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(A.IpgumAmt) IpgumAmt, SUM(A.SakAmt) SakAmt, sum(a.SakAmt2) SakAmt2,";
                SQL = SQL + ComNum.VBLF + "       SUM(A.BanAmt) BanAmt, SUM(A.EtcAmt) EtcAmt, SUM(DECODE(A.YYMM,'" + strTYYMM + "',A.JanAmt,0)) JanAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_MONTHLY A";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND A.YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.YYMM <= '" + strTYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.Class NOT IN ('01','02','03','04','05','07','10','15','20') "; //계약처,직원대납,자보예상액 통계는 제외
                SQL = SQL + ComNum.VBLF + " GROUP BY A.Class, A.YYMM ";
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
                    nRow += 1;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = READ_CLASS(dt.Rows[i]["Class"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()).ToString("###,###,###,##0 ");   //전월이월
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()).ToString("###,###,###,##0 ");   //당월미수
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim()).ToString("###,###,###,##0 ");  //입금액
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()).ToString("###,###,###,##0 ");    //삭감액
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim()).ToString("###,###,###,##0 ");   //절사삭감액
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");    //반송액
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()).ToString("###,###,###,##0 ");    //기타입금
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");    //월말잔액


                    //소계에 ADD
                    FnAmt[2, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());      //전월이월
                    FnAmt[2, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());      //당월미수
                    FnAmt[2, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());     //입금액
                    FnAmt[2, 4] += VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim());       //삭감액
                    FnAmt[2, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());      //절사삭감액
                    FnAmt[2, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());       //반송액
                    FnAmt[2, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());       //기타입금
                    FnAmt[2, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());       //월말잔액


                    //합계에 ADD
                    FnAmt[3, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());      //전월이월
                    FnAmt[3, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());      //당월미수
                    FnAmt[3, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());     //입금액
                    FnAmt[3, 4] += VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim());       //삭감액
                    FnAmt[3, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());      //절사삭감액
                    FnAmt[3, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());       //반송액
                    FnAmt[3, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());       //기타입금
                    FnAmt[3, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());       //월말잔액
                }
                dt.Dispose();
                dt = null;

                nRow += 1;
                ssView_Sheet1.Cells[nRow - 1, 1].Text = "소  계";
                for (k = 1; k < 9; k++)
                {
                    ssView_Sheet1.Cells[nRow - 1, k + 1].Text = FnAmt[2, k].ToString("###,###,###,##0 ");
                }
                ssView_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(224, 224, 224);

                #endregion
                ssView_Sheet1.AddSpanCell(nRow2, 0, nRow - nRow2, 1);
                ssView_Sheet1.Cells[nRow2, 0].CellType = tct;

                nRow += 1;
                ssView_Sheet1.AddSpanCell(nRow - 1, 0, 1, 2);
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "전 체 합 계";
                for (i = 1; i < 9; i++)
                {
                    ssView_Sheet1.Cells[nRow - 1, i + 1].Text = FnAmt[3, i].ToString("###,###,###,##0 ");
                }
                ssView_Sheet1.Rows[nRow - 1].BackColor = Color.Silver;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMonthMisu_NEW_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 36, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 36, "", "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboFYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    cboTYYMM.DropDownStyle = ComboBoxStyle.DropDown;
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
