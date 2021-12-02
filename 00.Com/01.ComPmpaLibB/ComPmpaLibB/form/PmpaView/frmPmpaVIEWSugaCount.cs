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
    /// File Name       : frmPmpaVIEWSugaCount
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\IPD\iviewa\iviewa.vbp\FrmSugaCount(IVIEWAE.FRM) >> frmPmpaVIEWSugaCount.cs 폼이름 재정의" />

    public partial class frmPmpaVIEWSugaCount : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        double[,] nTotQty = new double[4, 11];

        public frmPmpaVIEWSugaCount()
        {
            InitializeComponent();
        }


        #region //MainFormMessage
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

        public frmPmpaVIEWSugaCount(MainFormMessage pform)
        {
            mCallForm = pform;
            InitializeComponent();
            setEvent();
        }


        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void setEvent()
        {
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }


        private void frmPmpaVIEWSugaCount_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFdate.Value = Convert.ToDateTime(strDTP).AddDays(-10);
            dtpTdate.Value = Convert.ToDateTime(strDTP);
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
            double nQty = 0;
            string strEOF = "";
            string strCode = "";
            string strOldData = "";
            string strNewData = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //'누적할 배열을 Clear
            for (i = 1; i <= 10; i++)
            {
                nTotQty[1, i] = 0;
                nTotQty[2, i] = 0;
                nTotQty[3, i] = 0;
            }

            //'찾을 코드를 변수에 저장

            strCode = "'" + ssSub_Sheet1.Cells[0, 0].Text + "'";
            for (i = 2; i <= ssSub_Sheet1.RowCount; i++)
            {
                if (ssSub_Sheet1.Cells[0, 0].Text == "")
                {
                    strCode = strCode + ",'" + ssSub_Sheet1.Cells[0, 0].Text + "'";
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'자료를 Select
                if (rdoOI0.Checked == true || rdoOI2.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SuNext,'O' cIO,TO_CHAR(ActDate,'YYYYMM') cYYMM,            ";
                    SQL = SQL + ComNum.VBLF + "        Bi,SUM(Qty*Nal) cQty                                       ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                   ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1   ";
                    SQL = SQL + ComNum.VBLF + "    AND ActDate >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "    AND ActDate <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "    AND SuNext IN (" + strCode + ")                                ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY SuNext,TO_CHAR(ActDate,'YYYYMM'),Bi                   ";

                }
                if (rdoOI2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " UNION ALL                               ";
                }

                if (rdoOI1.Checked == true || rdoOI2.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT SuNext,'I' cIO,TO_CHAR(ActDate,'YYYYMM') cYYMM,            ";
                    SQL = SQL + ComNum.VBLF + "        Bi,SUM(Qty*Nal) cQty                                       ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                             ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "    AND ActDate >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "    AND ActDate <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "    AND SuNext IN (" + strCode + ")                                ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY SuNext,TO_CHAR(ActDate,'YYYYMM'),Bi                     ";
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY 1,2,3,4    ";

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = VB.Left(dt.Rows[i]["SuNext"].ToString().Trim() + VB.Space(8), 8);
                    strNewData = strNewData + VB.Left(dt.Rows[i]["cIO"].ToString().Trim() + VB.Space(1), 1);
                    strNewData = strNewData + VB.Left(dt.Rows[i]["cYYMM"].ToString().Trim() + VB.Space(6), 6);

                    if (strOldData == "")
                    {
                        strOldData = strNewData;
                    }

                    if (VB.Left(strOldData, 8) != VB.Left(strNewData, 8))
                    {
                        YYMM_Total_Display(ref nRow, ref strOldData, ref strNewData);
                        IpdOpd_Total_Display(ref nRow);
                        Suga_Total_Display(ref nRow);
                    }
                    else if (VB.Left(strOldData, 9) != VB.Left(strNewData, 9))
                    {
                        YYMM_Total_Display(ref nRow, ref strOldData, ref strNewData);
                        IpdOpd_Total_Display(ref nRow);
                    }
                    else if (strOldData != strNewData)
                    {
                        YYMM_Total_Display(ref nRow, ref strOldData, ref strNewData);
                    }

                    switch (dt.Rows[i]["Bi"].ToString().Trim())
                    {
                        case "11":
                            j = 1;
                            break;
                        case "12":
                            j = 2;
                            break;
                        case "13":
                            j = 3;
                            break;
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":
                            j = 4;
                            break;
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                            j = 5;
                            break;
                        case "31":
                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                        case "37":
                        case "38":
                        case "39":
                            j = 6;
                            break;
                        case "52":
                        case "55":
                            j = 7;
                            break;

                        case "51":
                            j = 8;
                            break;
                        default:
                            j = 9;
                            break;
                    }
                    nQty = VB.Val(dt.Rows[i]["cQty"].ToString().Trim());
                    nTotQty[1, j] = nTotQty[1, j] + nQty;
                    nTotQty[2, j] = nTotQty[2, j] + nQty;
                    nTotQty[3, j] = nTotQty[3, j] + nQty;
                    nTotQty[1, 10] = nTotQty[1, 10] + nQty;
                    nTotQty[2, 10] = nTotQty[2, 10] + nQty;
                    nTotQty[3, 10] = nTotQty[3, 10] + nQty;

                }

                if (dt.Rows.Count < 100)
                {
                    strEOF = "**";
                }

                YYMM_Total_Display(ref nRow, ref strOldData, ref strNewData);
                IpdOpd_Total_Display(ref nRow);
                Suga_Total_Display(ref nRow);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void YYMM_Total_Display(ref int nRow, ref string strOldData, ref string strNewData)
        {
            int j = 0;
            nRow = nRow + 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 0].Text = VB.Left(strOldData, 8);
            ssView_Sheet1.Cells[nRow - 1, 1].Text = VB.Mid(strOldData, 9, 1);
            ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Mid(strOldData, 10, 4) + "/" + VB.Right(strOldData, 2);
            for (j = 1; j <= 10; j++)
            {
                ssView_Sheet1.Cells[nRow - 1, (j + 3 - 1)].Text = nTotQty[1, j].ToString("###,###,##0");
                nTotQty[1, j] = 0;
            }
            strOldData = strNewData;
        }

        private void IpdOpd_Total_Display(ref int nRow)
        {
            int j = 0;

            nRow = nRow + 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 2].Text = "소 계";
            for (j = 1; j <= 10; j++)
            {
                ssView_Sheet1.Cells[nRow - 1, j + 3 - 1].Text = nTotQty[2, j].ToString("###,###,##0");
                nTotQty[2, j] = 0;
            }
        }

        private void Suga_Total_Display(ref int nRow)
        {
            int j = 0;

            nRow = nRow + 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 2].Text = "합 계";
            for (j = 1; j <= 10; j++)
            {
                ssView_Sheet1.Cells[nRow - 1, j + 3 - 1].Text = nTotQty[3, j].ToString("###,###,##0");
                nTotQty[3, j] = 0;
            }
        }

        private void ssSub_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strData = "";

            if (e.Column != 0)
            {
                return;
            }

            strData = ssSub_Sheet1.Cells[e.Row, e.Column].Text;

            if (strData == "")
            {
                ssSub_Sheet1.Cells[e.Row, 1].Text = "";
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SuNameK";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strData + "' ";

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
                    ComFunc.MsgBox("수가코드가 없슴니다.", "확인");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                ssSub_Sheet1.Cells[e.Row, 1].Text = dt.Rows[0]["SNAMEK"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "특정수가 환자종류별 발생건수";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자 : " + strDTP, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void pansubView_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
