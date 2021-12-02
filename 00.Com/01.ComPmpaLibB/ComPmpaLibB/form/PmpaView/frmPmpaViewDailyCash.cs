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
    /// File Name       : frmPmpaViewDailyCash.cs
    /// Description     : 당일 현금내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\frm당일현금내역조회.frm(frm당일현금내역조회.frm) >> frmPmpaViewDailyCash.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDailyCash : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        string strGubun = "";
        string StrGuName = "";
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
        string strPart = "";

        double nAmt = 0;
        double nSubTot = 0;
        double nTot = 0;

        int nSel = 0;
        int nSel1 = 0;
        int nCount = 0;
        int nCount1 = 0;
        int nSELECT = 0;

        int nSubCnt = 0;
        int nSubCut = 0;
        long nCutAmt = 0;

        public frmPmpaViewDailyCash()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;

            nCount = 0;
            nSubTot = 0;
            nTot = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            Cursor.Current = Cursors.WaitCursor;

            for (i = 85; i < 92; i += 2)
            {
                strGubun = i.ToString("00");
                SSBuildProcess(strGubun);
            }

            btnExit.Enabled = true;
            Cursor.Current = Cursors.Default;

            if (nCount > 0)
            {
                nCount += 1;
                ssView_Sheet1.RowCount = nCount;
                ssView_Sheet1.Cells[nCount - 1, 1].Text = "현금  합계";
                ssView_Sheet1.Cells[nCount - 1, 9].Text = nTot.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nCount - 1, 10].Text = nCutAmt.ToString("###,##0");
                nCutAmt = 0;
                btnPrint.Enabled = true;
            }

            if (chkBun.Checked == true)
            {
                //외래입원 보증금
                strGubun = "#";
                SSBuildProcess(strGubun);
            }
            btnPrint.Focus();
        }

        void SSBuildProcess(string strGubun)
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

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT C.Pano, C.Bi, M.Sname,";
                SQL = SQL + ComNum.VBLF + "        C.DeptCode, C.DrCode, I.WardCode,";
                SQL = SQL + ComNum.VBLF + "        I.RoomCode, C.Part, C.Amt,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(D.InDate,'YYYY-MM-DD') InDate, TO_CHAR(D.OutDate,'YYYY-MM-DD') OutDate, I.AmSet6";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH C, " + ComNum.DB_PMPA + "BAS_PATIENT M,";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER I, " + ComNum.DB_PMPA + "IPD_TRANS D";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND C.ActDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                if (strGubun == "#")
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Bun = '87'";
                    SQL = SQL + ComNum.VBLF + "    AND C.Part = '#'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Bun = '" + strGubun.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "    AND C.Part <> '#'";
                }
                if (chkAppPay.Checked == true)  //2021-11-09
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Part NOT IN ('5050')";
                }

                SQL = SQL + ComNum.VBLF + "    AND C.Pano = M.Pano(+)";
                SQL = SQL + ComNum.VBLF + "    AND C.IpdNo= I.IPDNO(+)";
                SQL = SQL + ComNum.VBLF + "    AND C.PANO = D.PANO(+)";
                SQL = SQL + ComNum.VBLF + "    AND C.IPDNO = D.IPDNO";
                SQL = SQL + ComNum.VBLF + "    AND C.TRSNO = D.TRSNO";
                if (txtPart.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Part = '" + txtPart.Text + "'";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY C.Pano";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY C.DeptCode,C.DrCode,I.RoomCode,C.Pano";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY I.WardCode,I.RoomCode,C.Pano";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY c.bi,C.Pano";
                }

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
                    return;
                }

                nRow = dt.Rows.Count;
                nCount += 1;

                for (i = 0; i < nRow; i++)
                {
                    ssView_Sheet1.RowCount = nCount;
                    if (i == 0)
                    {
                        nSel = 1;
                    }

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strName = dt.Rows[i]["SName"].ToString().Trim();
                    strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (dt.Rows[i]["Part"].ToString().Trim() == "5050")
                    {
                        strPart = "모바일앱";
                    }
                    else
                    {
                        strPart = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["Part"].ToString().Trim());
                    }
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strDate = dt.Rows[i]["InDate"].ToString().Trim();
                    strTDate = dt.Rows[i]["OutDate"].ToString().Trim();
                    strAmset6 = dt.Rows[i]["AmSet6"].ToString().Trim();

                    if (strPano == "08706653" && dtpDate.Value.ToString("yyyy-MM-dd") == "2014-06-25")
                    {
                        SSBuildADD("08706653");
                    }
                    else
                    {
                        SSBuildADD("");
                    }
                    nCount += 1;
                }
                dt.Dispose();
                dt = null;

                if (nRow > 0)
                {
                    nSubCnt = 0;
                    ssView_Sheet1.RowCount = nCount;
                    ssView_Sheet1.Cells[nCount - 1, 1].Text = "소   계";
                    ssView_Sheet1.Cells[nCount - 1, 9].Text = nSubTot.ToString("###,###,##0");
                    ssView_Sheet1.Cells[nCount - 1, 10].Text = nSubCut.ToString("##,##0");
                    cSPD.setSpdCellColor(ssView, nCount - 1, 0, nCount - 1, ssView.ActiveSheet.ColumnCount - 1, Color.FromArgb(192, 255, 192));
                    //clsSpread.gSpreadLineBoder(ssView, nCount - 1, 0, nRow - 1, ssView.ActiveSheet.ColumnCount - 1, Color.Black, 3, false, true, false, true);
                    clsSpread.gSpreadLineBoder(ssView, nCount - 1, 0, nCount - 1, ssView.ActiveSheet.ColumnCount - 1, Color.Black, 3, false, true, false, true);
                    nSubTot = 0;
                    nSubCut = 0;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SSBuildADD(string argGBN)
        {
            int nCut = 0;

            switch (strGubun)
            {
                case "85":
                    StrGuName = "가퇴원금";
                    break;
                case "87":
                    StrGuName = "중간납";
                    break;
                case "89":
                    StrGuName = "퇴원금";
                    break;
                case "91":
                    StrGuName = "환  불";
                    break;
                case "#":
                    StrGuName = "외래보증금";
                    break;
            }

            strBiGubun = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strBi);

            strGwaName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strGwa);

            if (argGBN == "")
            {
                nCut = (int)(nAmt % 10);   //절사
                nAmt = VB.Fix((int)(nAmt / 10)) * 10;   //금액을 원 미만 절사
            }

            nSubTot += nAmt;
            nSubCut += nCut;

            if (strGubun == "91")
            {
                nTot -= nAmt;
                nCutAmt -= nCut;
            }
            else if (strGubun == "85" || strGubun == "87" || strGubun == "89")
            {
                nTot += nAmt;
                nCutAmt += nCut;
            }

            nSubCnt += 1;
            ssView_Sheet1.Cells[nCount - 1, 0].Text = nSubCnt.ToString();
            if (nSubCnt == 1)
            {
                ssView_Sheet1.Cells[nCount - 1, 1].Text = StrGuName;
            }

            ssView_Sheet1.Cells[nCount - 1, 2].Text = strPano;
            ssView_Sheet1.Cells[nCount - 1, 3].Text = strBiGubun;
            ssView_Sheet1.Cells[nCount - 1, 4].Text = strName;
            ssView_Sheet1.Cells[nCount - 1, 5].Text = strGwaName;
            ssView_Sheet1.Cells[nCount - 1, 6].Text = strRoom;
            ssView_Sheet1.Cells[nCount - 1, 7].Text = strDate;
            ssView_Sheet1.Cells[nCount - 1, 8].Text = strTDate;
            if (argGBN != "")
            {
                ssView_Sheet1.Cells[nCount - 1, 9].Text = nAmt.ToString("###,###,##0");
            }
            else
            {
                ssView_Sheet1.Cells[nCount - 1, 9].Text = ((int)(nAmt / 10) * 10).ToString("###,###,##0");
            }
            ssView_Sheet1.Cells[nCount - 1, 10].Text = nCut.ToString("##0");
            ssView_Sheet1.Cells[nCount - 1, 11].Text = strPart;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            clsSpread Spd = new clsSpread();

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            int i = 0;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 9].ColumnSpan = 3;
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
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 12].Text = ssView_Sheet1.Cells[i, 10].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 13].Text = ssView_Sheet1.Cells[i, 11].Text;

                for (int j = 0; j < 11; j++)
                {
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, j].Border = ssView_Sheet1.Cells[i, j].Border;
                }
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 12].Border = ssView_Sheet1.Cells[i, 11].Border;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 13].Border = ssView_Sheet1.Cells[i, 11].Border;
                Spd.CellAlignMent(ssPrint, ssPrint_Sheet1.RowCount - 1, 9, ssPrint_Sheet1.RowCount - 1, 9, clsSpread.HAlign_R, clsSpread.VAlign_C);   //셀정렬  
            }

            strTitle = "현금 내역 명세서";

            ssPrint_Sheet1.Cells[2, 0].Text = "작성자  : " + clsType.User.JobMan + "  " + "작업일자: " + dtpDate.Value.ToString("yyyy-MM-dd");
            ssPrint_Sheet1.Cells[3, 0].Text = "출력시간 : " + VB.Now().ToString();

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strHeader += CS.setSpdPrint_String("작성자  : " + clsType.User.JobMan + "  " + "작업일자: " + dtpDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            //strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String(VB.Space(120) + "책임수녀", new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmPmpaViewDailyCash_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            txtPart.Text = "";
            lblName.Text = "";
            ssView.Dock = DockStyle.Fill;
            ssPrint.Visible = false;
        }

        private void txtPart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void txtPart_Leave(object sender, EventArgs e)
        {
            txtPart.Text = txtPart.Text.ToUpper().Trim();
            if (txtPart.Text != "")
            {
                txtPart.Text = VB.Val(txtPart.Text).ToString("00000");
                lblName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, txtPart.Text);
            }
            else
            {
                lblName.Text = "전체";
            }
        }
    }
}
