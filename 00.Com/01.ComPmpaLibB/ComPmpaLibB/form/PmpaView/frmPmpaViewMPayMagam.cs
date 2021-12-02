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
    /// File Name       : frmPmpaViewMPayMagam.cs
    /// Description     : 당일 미수금 내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\ILREPA07.FRM(FrmMPayMagam.frm) >> frmPmpaViewMPayMagam.cs 폼이름 재정의" />
    public partial class frmPmpaViewMPayMagam : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        string strPano = "";
        string strBi = "";
        string strBiGubun = "";
        string strName = "";
        string strGwa = "";
        string strGwaName = "";
        string strRoom = "";
        string strDate = "";
        string strTDate = "";
        string strAmset6 = "";
        string strBigo = "";
        double nAmt = 0;
        double nSubTot = 0;
        double nTot = 0;

        int nSel = 0;
        int nCount = 0;

        int nSubCnt = 0;
        int nSubCut = 0;
        long nCutAmt = 0;
        int nChoice = 0;

        public frmPmpaViewMPayMagam()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            clsSpread cSPD = new clsSpread();

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int nRow = 0;
            int nCut = 0;

            nCount = 0;
            nSubTot = 0;
            nTot = 0;

            ssView_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT C.Pano, C.Bi, M.Sname,";
                SQL = SQL + ComNum.VBLF + "        C.DeptCode, M.RoomCode, C.Bigo,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') InDate, TO_CHAR(A.OutDate,'YYYY-MM-DD') OutDate, C.Amt,";
                SQL = SQL + ComNum.VBLF + "        M.Amset6";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  C, " + ComNum.DB_PMPA + "IPD_NEW_MASTER M,";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS A";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND C.ActDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                if (nChoice == 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Bun = '96'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Bun = '98'";
                }
                SQL = SQL + ComNum.VBLF + "    AND C.IPDNO = M.IPDNO(+)";
                SQL = SQL + ComNum.VBLF + "     AND C.TRSNO = A.TRSNO";
                SQL = SQL + ComNum.VBLF + "  ORDER BY C.Pano";

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

                nRow = dt.Rows.Count;

                for (i = 0; i < nRow; i++)
                {
                    nCount += 1;
                    ssView_Sheet1.RowCount = nCount;
                    if (i == 0)
                    {
                        nSel = 1;
                    }
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strName = dt.Rows[i]["SName"].ToString().Trim();
                    strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strDate = dt.Rows[i]["InDate"].ToString().Trim();
                    strTDate = dt.Rows[i]["OutDate"].ToString().Trim();
                    strBigo = dt.Rows[i]["Bigo"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    strAmset6 = dt.Rows[i]["AmSet6"].ToString().Trim();

                    strBiGubun = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strBi);
                    strGwaName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strGwa);

                    nCut = (int)(nAmt % 10);   //절사
                    if (nAmt < 0)
                    {
                        nAmt = (int)(nAmt * -1 / 10) * 10 * -1;
                    }
                    else
                    {
                        nAmt = (int)(nAmt / 10) * 10;
                    }

                    nSubTot += nAmt;
                    nSubCut += nCut;

                    nTot += nAmt;
                    nCutAmt += nCut;

                    ssView_Sheet1.Cells[nCount - 1, 0].Text = strPano;
                    ssView_Sheet1.Cells[nCount - 1, 1].Text = strBiGubun;
                    ssView_Sheet1.Cells[nCount - 1, 2].Text = strName;
                    ssView_Sheet1.Cells[nCount - 1, 3].Text = strGwaName;
                    ssView_Sheet1.Cells[nCount - 1, 4].Text = strRoom;
                    ssView_Sheet1.Cells[nCount - 1, 5].Text = strDate;
                    ssView_Sheet1.Cells[nCount - 1, 6].Text = strTDate;
                    ssView_Sheet1.Cells[nCount - 1, 7].Text = ((int)(nAmt / 10) * 10).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nCount - 1, 8].Text = nCut.ToString("##0");
                    if (strAmset6 == "*")
                    {
                        ssView_Sheet1.Cells[nCount - 1, 9].Text = "구분변경자";
                    }
                    ssView_Sheet1.Cells[nCount - 1, 10].Text = strBigo;
                }
                dt.Dispose();
                dt = null;

                nSubCnt = 0;
                nCount += 1;
                ssView_Sheet1.RowCount = nCount;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.RowHeader.Cells[nCount - 1, 0].Text = " ";
                ssView_Sheet1.Cells[nCount - 1, 1].Text = "소      계";
                ssView_Sheet1.Cells[nCount - 1, 7].Text = nSubTot.ToString("###,###,##0");
                ssView_Sheet1.Cells[nCount - 1, 8].Text = nSubCut.ToString("##,##0");
                cSPD.setSpdCellColor(ssView, nCount - 1, 0, nCount - 1, ssView.ActiveSheet.ColumnCount - 1, Color.FromArgb(192, 255, 192));
                clsSpread.gSpreadLineBoder(ssView, nCount - 1, 0, nRow - 1, ssView.ActiveSheet.ColumnCount - 1, Color.Black, 3, false, true, false, true);
                nSubTot = 0;
                nSubCut = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            Cursor.Current = Cursors.Default;
            btnPrint.Enabled = true;
            btnPrint.Focus();
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
            int i = 0;

            ssPrint_Sheet1.Cells[2, 0].Text = "작성자 : " + clsType.User.JobMan + "  " + "작업일자 : " + dtpDate.Value.ToString("yyyy-MM-dd");
            ssPrint_Sheet1.Cells[3, 0].Text = "출력시간 : " + VB.Now().ToString();
            ssPrint_Sheet1.RowCount = 6;
            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].ColumnSpan = 3;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 7].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].Text = ssView_Sheet1.Cells[i, 8].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 9].Text = ssView_Sheet1.Cells[i, 9].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Text = ssView_Sheet1.Cells[i, 10].Text;
            }
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strTitle = "미  수  금  내  역  현  황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, false, false, false, false,(float)0.9);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void rdoSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSelect0.Checked == true)
            {
                nChoice = 0;
            }
            else if (rdoSelect1.Checked == true)
            {
                nChoice = 1;
            }
        }

        private void frmPmpaViewMPayMagam_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            ssView.Dock = DockStyle.Fill;
            ssPrint.Visible = false;
            nChoice = 0;
        }
    }
}
