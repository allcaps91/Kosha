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
    /// File Name       : frmPmpaViewJaSakPrint
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misuta.vbp\MISUT207.FRM (FrmSakPrint.frm)>> frmPmpaViewJaSakPrint.cs 폼이름 재정의" />
    /// 
    public partial class frmPmpaViewJaSakPrint : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        long GnJobSabun = 0;

        public frmPmpaViewJaSakPrint(long nJobSabun)
        {
            GnJobSabun = nJobSabun;

            InitializeComponent();
        }

        public frmPmpaViewJaSakPrint()
        {
            InitializeComponent();
        }

        private void frmPmpaViewJaSakPrint_Load(object sender, System.EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFDate, 12, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTDate, 12, "", "1");
            txtYul.Text = "0";

            cboFDate.SelectedIndex = 1;
            cboTDate.SelectedIndex = 1;
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            ssView_Sheet1.RowCount = 0;

            int i = 0;
            int nRow = 0;
            int nQty1 = 0;
            int nQty2 = 0;
            int nREAD = 0;
            int nWRTNO = 0;
            double nSakRATE = 0;
            double nToTal1 = 0;
            double nToTal2 = 0;
            double nToTal3 = 0;
            double nMisuIlsu = 0;
            string strYYMM = "";
            string strYYMM2 = "";
            string strOldData = "";
            string strNewData = "";
            string strIDate = "";
            string strIO2 = "";
            DataTable dt = null;
            DataTable dtFn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            nMisuIlsu = 0;
            nToTal1 = 0;
            nToTal2 = 0;
            nToTal3 = 0;

            strYYMM = VB.Left(cboFDate.Text, 4) + VB.Mid(cboFDate.Text, 7, 2);
            strYYMM2 = VB.Left(cboTDate.Text, 4) + VB.Mid(cboTDate.Text, 7, 2);

            progressBar1.Value = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //' 해당월 마감여부 Checking

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Count(*) Cnt                ";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_MONTHLY               ";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM >= '" + strYYMM + "'  ";
                SQL = SQL + ComNum.VBLF + "    AND YYMM <= '" + strYYMM2 + "' ";
                SQL = SQL + ComNum.VBLF + "    AND Class = '07'               ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("통계 형성중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("통계 형성중 문제가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("통계 형성중 문제가 발생했습니다.");
                    return;
                }

                dt.Dispose();
                dt = null;

                //' 해당월중에 입금완료자 Select


                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,a.GelCode,b.Bdate,a.JanAmt,a.misuamt,a.sakamt,b.deptcode,b.drcode,b.amt2,b.qty1,b.amt4,b.qty3      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a," + ComNum.DB_PMPA + "MISU_IDMST b    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM >= '" + strYYMM + "'    ";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM <= '" + strYYMM2 + "'   ";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '07'                 ";

                if (chk0.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.JanAmt < 1                   ";
                }
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO              ";

                if (rdo1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.IPDOPD = 'I' ";
                }
                else if (rdo2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.IPDOPD = 'O' ";
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY a.GelCode,b.Bdate           ";

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

                strOldData = "";
                nRow = 0;

                nREAD = dt.Rows.Count;

                progressBar1.Maximum = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    progressBar1.Value = i + 1;
                    nWRTNO = (int)VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());
                    CPM.READ_MISU_IDMST(nWRTNO);
                    nSakRATE = 0;
                    if (clsPmpaType.TMM.Amt[2] != 0 && clsPmpaType.TMM.Amt[4] != 0)
                    {
                        nSakRATE = clsPmpaType.TMM.Amt[4] / clsPmpaType.TMM.Amt[2] * 100;
                    }

                    if (nSakRATE >= VB.Val(txtYul.Text))
                    {
                        #region    GoSub SAK_Display_Rtn;
                        strNewData = clsPmpaType.TMM.GelCode.Trim();
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                        if (strOldData != strNewData)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = CPM.READ_BAS_MIA(strNewData);
                            strOldData = strNewData;
                        }
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = clsPmpaType.TMM.MisuID;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = CPF.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim()).Rows[0]["sname"].ToString().Trim();

                        if (clsPmpaType.TMM.IpdOpd == "I")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "입원";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "외래";
                        }
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = clsPmpaType.TMM.DeptCode;

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = CPF.READ_DOCTOR_NAME(clsDB.DbCon, clsPmpaType.TMM.DrCode);
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = clsPmpaType.TMM.FromDate + " - " + clsPmpaType.TMM.ToDate;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = clsPmpaType.TMM.BDate;

                        //최종 입금일자 구하기
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate,'YYYY-MM-DD') IDate  ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP    ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                ";
                        SQL = SQL + ComNum.VBLF + "    AND WRTNO = " + nWRTNO + "             ";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate DESC                      ";

                        SqlErr = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dtFn.Rows.Count == 0)
                        {

                            dtFn.Dispose();
                            dtFn = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;

                        }

                        strIDate = dtFn.Rows[0]["IDate"].ToString().Trim();

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = strIDate;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = VB.DateDiff("d", Convert.ToDateTime(clsPmpaType.TMM.BDate), Convert.ToDateTime(strIDate)).ToString();
                        nMisuIlsu += VB.DateDiff("d", Convert.ToDateTime(clsPmpaType.TMM.BDate), Convert.ToDateTime(strIDate));

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = (clsPmpaType.TMM.Amt[2]).ToString("###,###,###,##0 "); //'청구액
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = (clsPmpaType.TMM.Amt[3]).ToString("###,###,###,##0 "); //'입금액
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = (clsPmpaType.TMM.Amt[4]).ToString("###,###,###,##0 "); //'삭감액
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = (nSakRATE).ToString("###0.00") + "(%) "; //'삭감율

                        nToTal1 = nToTal1 + clsPmpaType.TMM.Amt[2];
                        nToTal2 = nToTal2 + clsPmpaType.TMM.Amt[3];
                        nToTal3 = nToTal3 + clsPmpaType.TMM.Amt[4];

                        dtFn.Dispose();
                        dtFn = null;
                        Cursor.Current = Cursors.Default;

                        #endregion
                    }

                    if (rdo1.Checked == true)
                    {
                        strIO2 = "I";
                    }
                    else if (rdo2.Checked == true)
                    {
                        strIO2 = "O";
                    }
                    else
                    {
                        if (chk0.Checked == true)
                        {
                            ComFunc.MsgBox("통계형성시 외래입원구분");
                            return;
                        }
                    }

                    if (chk0.Checked == true)
                    {

                        nQty1 = (int)VB.Val(dt.Rows[i]["qty1"].ToString().Trim());
                        nQty2 = (int)VB.Val(dt.Rows[i]["qty3"].ToString().Trim());
                        if (VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()) < 1)
                        {
                            nQty1 = 0;
                            nQty2 = 0;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "합   계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "Avg:" + (nMisuIlsu / nREAD).ToString("###,###,###,##0 "); //평균미수일수
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nToTal1.ToString("###,###,###,##0 "); //'청구액
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nToTal2.ToString("###,###,###,##0 "); //'입금액
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = nToTal3.ToString("###,###,###,##0 "); //'삭감액

                if (nToTal1 > 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = (nToTal3 / nToTal1 * 100).ToString("###0.00") + "(%) ";
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "자보 진료비 삭감 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboFDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "출력자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, true);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
