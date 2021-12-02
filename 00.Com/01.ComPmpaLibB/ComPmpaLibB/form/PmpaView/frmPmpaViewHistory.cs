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
    /// File Name       : frmPmpaViewHistory.cs
    /// Description     : 미수관리 자료변경 내역
    /// Author          : 박창욱
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MUMAIN09.FRM(FrmHistoryView.frm) >> frmPmpaViewHistory.cs 폼이름 재정의" />	
    public partial class frmPmpaViewHistory : Form, MainFormMessage
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        double GnWRTNO = 0;
        //1.건당  2.전체합계
        double[,] nTotAmt = new double[3, 7];

        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion

        public frmPmpaViewHistory(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;            
        }

        public frmPmpaViewHistory()
        {
            InitializeComponent();            
        }

        string Data_Class(string argDate)
        {
            string strClass = "";

            switch (argDate)
            {
                case "01":
                    strClass = "공단";
                    break;
                case "02":
                    strClass = "직장";
                    break;
                case "03":
                    strClass = "지역";
                    break;
                case "04":
                    strClass = "보험";
                    break;
                case "05":
                    strClass = "산재";
                    break;
                case "07":
                    strClass = "자보";
                    break;
            }

            return strClass;
        }

        string Sabun_Name(string argDate)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT NAME FROM " + ComNum.DB_PMPA + "BAS_PASS";
                SQL = SQL + ComNum.VBLF + "  WHERE IDNUMBER = '" + argDate + "'";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
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

            int nRead = 0;
            int nRow = 0;
            string strNewData = "";
            string strOldData = "";
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            //Sheet Clear
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;



            try
            {
                //발생일별 미수 상세내역 Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, TO_CHAR(a.UpdateTime,'YYYY-MM-DD HH24:MI') UpdateTime, a.UpDateTable,";
                SQL = SQL + ComNum.VBLF + "        a.UpDateJob, a.UpDateSabun, a.MisuID,";
                SQL = SQL + ComNum.VBLF + "        a.Class, a.GelCode, TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,";
                SQL = SQL + ComNum.VBLF + "        a.IpdOpd, a.Class, a.Qty,";
                SQL = SQL + ComNum.VBLF + "        a.Tamt, a.Amt, a.Remark";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_HISTORY a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.UpdateTime>=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND a.UpdateTime<= TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO=b.WRTNO(+)";
                if (rdoClass0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '01'"; //공단
                }
                else if (rdoClass1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '02'"; //직장
                }
                else if (rdoClass2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '03'"; //지역
                }
                else if (rdoClass3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '04'"; //보험
                }
                else if (rdoClass4.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class < '05'"; //전체
                }
                else if (rdoClass5.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '05'"; //산재
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Class = '07'"; //자보
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.WRTNO, a.UpDateTable, a.UpDateTime";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strOldData = VB.Space(18);
                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;

                for (i = 0; i < nRead; i++)
                {
                    strNewData = dt.Rows[i]["WRTNO"].ToString().Trim();
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNewData;
                        strOldData = strNewData;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["UpdateTime"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = Sabun_Name(dt.Rows[i]["UpdateSabun"].ToString().Trim()).Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["UpdateTable"].ToString().Trim();
                    if (dt.Rows[i]["UpdateJob"].ToString().Trim() == "M")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = "수정";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = "삭제";
                    }
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["MisuId"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["GelCode"].ToString().Trim();
                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "O")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = "외래";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = "입원";
                    }
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = Data_Class(dt.Rows[i]["Class"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = VB.Val(dt.Rows[i]["Tamt"].ToString().Trim()).ToString("#,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("#,###,###,##0");
                }

                dt.Dispose();
                dt = null;

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                lblMsg.Text = "해당 줄을 더블클릭하면 상세내역 조회";
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
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
            string strClass = "";
            bool PrePrint = true;

            if (rdoClass0.Checked == true)
            {
                strClass = "(공단)";
            }
            else if (rdoClass1.Checked == true)
            {
                strClass = "(직장)";
            }
            else if (rdoClass2.Checked == true)
            {
                strClass = "(지역)";
            }
            else if (rdoClass3.Checked == true)
            {
                strClass = "(보호)";
            }
            else
            {
                strClass = "(전체)";
            }

            strTitle = cboYYMM.Text + " 현재 미수자 명부 " + strClass;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobName + " 인 " + VB.Space(8), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaViewHistory_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 60, "", "1");
            cboYYMM.SelectedIndex = 0;
            lblMsg.Text = "";
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

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            if (ssView_Sheet1.Cells[e.Row, 0].Text == "")
            {
                GnWRTNO = VB.Val(ssView_Sheet1.Cells[e.Row - 1, 0].Text);
            }
            else
            {
                GnWRTNO = VB.Val(ssView_Sheet1.Cells[e.Row, 0].Text);
            }

            frmPmpaViewMisuIdcs f = new frmPmpaViewMisuIdcs(GnWRTNO);

            if(f != null)
            {
                f.Dispose();
                f = null;
            }

            f.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
