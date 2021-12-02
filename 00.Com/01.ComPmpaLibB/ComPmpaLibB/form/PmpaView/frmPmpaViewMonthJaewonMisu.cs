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
    /// File Name       : frmPmpaViewMonthJaewonMisu.cs
    /// Description     : (경리과용)월말현재재원미수금총괄표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs28.frm(FrmMonthMisu.frm) >> frmPmpaViewMonthJaewonMisu.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMonthJaewonMisu : Form
    {
        double[,] FnAmt = new double[25, 9];

        public frmPmpaViewMonthJaewonMisu()
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

            strTitle = "(재무회계팀)월별 미수금 총괄표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업월: " + cboFYYMM.Text + "~" + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 10);
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
            int n = 0;
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

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";

            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 7, 2);
            strTdate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strTYYMM, 4)), Convert.ToInt32(VB.Right(strTYYMM, 2)));

            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            try
            {
                //월별 미수번호별 통계를 ADD
                #region Misu_Monthly_ADD

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.Class, A.IpdOpd, SUM(DECODE(A.YYMM, '" + strFYYMM + "',A.IwolAmt,0)) IwolAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(A.MisuAmt) MisuAmt, SUM(A.IpgumAmt) IpgumAmt,SUM(A.SakAmt) SakAmt, sum(a.SakAmt2) SakAmt2,  ";
                SQL = SQL + ComNum.VBLF + "       SUM(A.BanAmt) BanAmt, SUM(A.EtcAmt) EtcAmt, SUM(DECODE(A.YYMM,'" + strTYYMM + "',A.JanAmt,0)) JanAmt, ";
                SQL = SQL + ComNum.VBLF + "       B.TONGGBN ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_MONTHLY A, " + ComNum.DB_PMPA + "MISU_IDMST B ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND A.YYMM >= '" + strFYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.YYMM <= '" + strTYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.Class NOT IN ('10','15','20') "; //계약처,직원대납,자보예상액 통계는 제외
                SQL = SQL + ComNum.VBLF + "   AND A.WRTNO = B.WRTNO ";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.Class,A.IpdOpd, B.TONGGBN, A.YYMM ";

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
                            k = 1;
                            break;  //의료보험
                        case "04":
                            k = 2;
                            break;  //의료보험
                        case "05":
                            k = 3;
                            break;  //산재
                        case "07":
                            k = 4;
                            break;  //자보
                        case "11":
                            k = 18;
                            break;  //보훈청
                        case "08":
                            k = 19;
                            break;  //계약처
                        case "13":
                            k = 20;
                            break;  //심신장애
                        case "09":
                            k = 21;
                            break;  //헌혈미수
                        default:
                            k = 22;
                            break;  //기타미수
                    }

                    if (k < 5)
                    {
                        if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                        {
                            k += 5;
                            if (dt.Rows[i]["TONGGBN"].ToString().Trim() == "2")
                            {
                                k += 5;
                            }
                        }
                    }

                    FnAmt[k, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());     //전월이월
                    FnAmt[k, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());     //당월미수
                    FnAmt[k, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());    //입금액
                    FnAmt[k, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());      //삭감액
                    FnAmt[k, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());     //절사삭감액
                    FnAmt[k, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());      //반송액
                    FnAmt[k, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());      //기타입금
                    FnAmt[k, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());      //월말잔액

                    if (k <= 4)
                    {
                        n = 5;
                    }
                    else if (k <= 9)
                    {
                        n = 10;
                    }
                    else if (k <= 14)
                    {
                        n = 15;
                    }
                    else
                    {
                        n = 23;
                    }


                    //소계에 ADD
                    FnAmt[n, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());     //전월이월
                    FnAmt[n, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());     //당월미수
                    FnAmt[n, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());    //입금액
                    FnAmt[n, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());      //삭감액
                    FnAmt[n, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());     //절사삭감액
                    FnAmt[n, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());      //반송액
                    FnAmt[n, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());      //기타입금
                    FnAmt[n, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());      //월말잔액

                    //기관미수계
                    if (k <= 15)
                    {
                        n = 16;
                        FnAmt[n, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());     //전월이월
                        FnAmt[n, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());     //당월미수
                        FnAmt[n, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());    //입금액
                        FnAmt[n, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());      //삭감액
                        FnAmt[n, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());     //절사삭감액
                        FnAmt[n, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());      //반송액
                        FnAmt[n, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());      //기타입금
                        FnAmt[n, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());      //월말잔액
                    }

                    //미수합계에 ADD
                    n = 24;
                    FnAmt[n, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());     //전월이월
                    FnAmt[n, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());     //당월미수
                    FnAmt[n, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());    //입금액
                    FnAmt[n, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());      //삭감액
                    FnAmt[n, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());     //절사삭감액
                    FnAmt[n, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());      //반송액
                    FnAmt[n, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());      //기타입금
                    FnAmt[n, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());      //월말잔액
                }
                dt.Dispose();
                dt = null;

                #endregion


                //월별 개인미수 통계를 ADD
                #region GainMisu_Monthly_ADD

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SUM(DECODE(YYMM, '" + strFYYMM + "',IwolAmt,0)) IwolAmt, SUM(MonMAmt) MisuAmt, SUM(MonIAmt) IpgumAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(MONSAMT) SakAmt, 0 SAKAMT2, SUM(MONBAMT) BanAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(MONGAMT) EtcAmt, SUM(DECODE(YYMM,'" + strTYYMM + "',JanAmt,0)) JanAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_GAINTONG";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strTYYMM + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    for (i = 0; i < nRead; i++)
                    {
                        k = 17;
                        FnAmt[k, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());     //전월이월
                        FnAmt[k, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());     //당월미수
                        FnAmt[k, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());    //입금액
                        FnAmt[k, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());      //삭감액
                        FnAmt[k, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());     //절사삭감액
                        FnAmt[k, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());      //반송액
                        FnAmt[k, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());      //기타입금
                        FnAmt[k, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());      //월말잔액

                        //소계에 ADD
                        n = 23;
                        FnAmt[n, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());     //전월이월
                        FnAmt[n, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());     //당월미수
                        FnAmt[n, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());    //입금액
                        FnAmt[n, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());      //삭감액
                        FnAmt[n, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());     //절사삭감액
                        FnAmt[n, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());      //반송액
                        FnAmt[n, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());      //기타입금
                        FnAmt[n, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());      //월말잔액

                        //미수합계에 ADD
                        n = 24;
                        FnAmt[n, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());     //전월이월
                        FnAmt[n, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());     //당월미수
                        FnAmt[n, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());    //입금액
                        FnAmt[n, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());      //삭감액
                        FnAmt[n, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim());     //절사삭감액
                        FnAmt[n, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());      //반송액
                        FnAmt[n, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim());      //기타입금
                        FnAmt[n, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());      //월말잔액
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                //내용을 Sheet에 Display
                for (i = 1; i < 25; i++)
                {
                    for (k = 1; k < 9; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k + 1].Text = FnAmt[i, k].ToString("###,###,###,##0 ");
                    }
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMonthJaewonMisu_Load(object sender, EventArgs e)
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
