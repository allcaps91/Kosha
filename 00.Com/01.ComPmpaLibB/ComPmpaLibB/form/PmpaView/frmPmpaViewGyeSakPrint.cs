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
    /// File Name       : frmPmpaViewGyeSakPrint.cs
    /// Description     : 계약처 진료비 삭감분석
    /// Author          : 박창욱
    /// Create Date     : 2017-10-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR207.FRM(FrmSakPrint.frm) >> frmPmpaViewGyeSakPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeSakPrint : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsPmpaFunc cpf = new clsPmpaFunc();

        string[] GstrGels = new string[31];

        public frmPmpaViewGyeSakPrint()
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

            strTitle = cboYYMM.Text + " 계약처 진료비 삭감 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자:" + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.9f);

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
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nREAD = 0;
            int nRow = 0;
            double nWRTNO = 0;
            double nSakRATE = 0;
            string strYYMM = "";
            string strOldData = "";
            string strNewData = "";
            string strIDate = "";

            ssView_Sheet1.RowCount = 0;
            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            try
            {
                //해당월 마감여부 Checking
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Count(*) Cnt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_MONTHLY";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND Class = '08'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월의 통계가 형성되지 않았습니다.");
                    return;
                }

                dt.Dispose();
                dt = null;

                //해당월중에 입금완료자 Select
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,a.GelCode,b.Bdate";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '08'";
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt < 1";
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.GelCode,b.Bdate";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strOldData = "";
                nRow = 0;
                nREAD = dt.Rows.Count;
                for (i = 0; i < nREAD; i++)
                {
                    nWRTNO = VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());
                    cpm.READ_MISU_IDMST((long)nWRTNO);
                    nSakRATE = 0;
                    if (clsPmpaType.TMM.Amt[2] != 0 && clsPmpaType.TMM.Amt[4] != 0)
                    {
                        nSakRATE = clsPmpaType.TMM.Amt[4] / clsPmpaType.TMM.Amt[2] * 100;
                    }
                    if (nSakRATE >= VB.Val(txtYul.Text))
                    {
                        #region SAK_Display_Rtn

                        //1건의 삭감내역을 Display

                        strNewData = clsPmpaType.TMM.GelCode.Trim();
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        if (strOldData != strNewData)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = cpf.GET_BAS_MIA(clsDB.DbCon, strNewData);
                            strOldData = strNewData;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = clsPmpaType.TMM.MisuID;
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = cpf.Get_BasPatient(clsDB.DbCon, clsPmpaType.TMM.MisuID.Trim()).Rows[0]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = clsPmpaType.TMM.DeptCode;
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = clsPmpaType.TMM.FromDate + "-" + clsPmpaType.TMM.ToDate;
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = clsPmpaType.TMM.BDate;

                        //최종 입금일자 구하기
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(Bdate,'YYYY-MM-DD') IDate";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE WRTNO = " + nWRTNO + "";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate DESC";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        strIDate = dt1.Rows[0]["IDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = strIDate;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.DateDiff("D", Convert.ToDateTime(clsPmpaType.TMM.BDate), Convert.ToDateTime(strIDate)).ToString("###0 ");
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = clsPmpaType.TMM.Amt[2].ToString("###,###,###,##0 ");    //청구액
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = clsPmpaType.TMM.Amt[3].ToString("###,###,###,##0 ");    //입금액
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = clsPmpaType.TMM.Amt[4].ToString("###,###,###,##0 ");   //삭감액
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = nSakRATE.ToString("###0.00") + "(%) ";                 //삭감율

                        dt1.Dispose();
                        dt1 = null;

                        #endregion
                    }
                }

                dt.Dispose();
                dt = null;

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewGyeSakPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
            cboYYMM.SelectedIndex = 1;
            btnPrint.Enabled = false;
        }
    }
}
