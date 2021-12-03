using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaVIEWJPayMagam.cs
    /// Description     : 직원 미수조회
    /// Author          : 김효성
    /// Create Date     : 2017-09-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\psmh\IPD\iument\iument.vbp(ILREPA08.FRM)  >> frmPmPaVIEWJPayMagam.cs 폼이름 재정의" />

    public partial class frmPmPaVIEWJPayMagam : Form
    {
        string GstrJobName = "";
        string strPano = "";
        string strBi = "";
        string strBiGubun = "";
        string strName = "";
        string strGwa = "";
        string strSucode = "";
        string Gcode = "";
        string strGwaName = "";
        string strRoom = "";
        string strDate = "";
        string strTDate = "";
        string strAmset6 = "";
        string strBigo = "";
        double nAmt = 0;
        double nSubTot = 0;
        double nGrdTot = 0;
        double nTot = 0;
        int nSel = 0;
        int nCount = 0;
        int nSELECT = 0;
        int nSubCnt = 0;
        int nSubCut = 0;
        int nChoice = 1;
        long nCutAmt = 0;
        long nGrdCut = 0;

        public frmPmPaVIEWJPayMagam(string strJobName)
        {
            GstrJobName = strJobName;

            InitializeComponent();
        }

        public frmPmPaVIEWJPayMagam()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWJPayMagam_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.RowCount = 0;

            ssView_Sheet1.Columns[4].Visible = false;
            ssView_Sheet1.Columns[5].Visible = false;
            ssView_Sheet1.Columns[6].Visible = false;
            ssView_Sheet1.Columns[7].Visible = false;

            btnPrint.Enabled = true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int nRow = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            nCount = 0;
            nSubTot = 0;
            nTot = 0;
            nGrdTot = 0;
            nGrdCut = 0;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT C.Pano, C.Bi, M.Sname, C.DeptCode, M.RoomCode,C.Bigo, C.Sunext,                    ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(M.InDate,'YYYY-MM-DD') InDate, TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,    ";
                SQL = SQL + ComNum.VBLF + "        C.Amt, M.Amset6                                                                    ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  C, " + ComNum.DB_PMPA + "IPD_NEW_MASTER M  ";
                SQL = SQL + ComNum.VBLF + "  WHERE C.ActDate = TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')                          ";
                SQL = SQL + ComNum.VBLF + "    AND C.IPDNO   = M.IPDNO(+)                                                             ";

                if (nChoice == 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Sunext = 'Y96B'                                                              ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY C.Pano                                                                      ";
                }
                else if (nChoice == 1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND C.Sunext = 'Y96D'                                                              ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY C.Pano                                                                      ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND (C.Sunext = 'Y96B' Or C.Sunext = 'Y96D')                                       ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY C.Sunext, C.Pano                                                            ";
                }

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

                nRow = dt.Rows.Count;

                for (i = 0; i < nRow; i++)
                {
                    nCount = nCount + 1;
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
                    strSucode = dt.Rows[i]["Sunext"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    strAmset6 = dt.Rows[i]["AmSet6"].ToString().Trim();

                    SS1Col1Build();
                    SS1BuildADD();
                }

                dt.Dispose();
                dt = null;

                if (nRow > 0)
                {
                    SS1_Sub_Total();
                }

                if (nRow > 0 && nChoice == 2)
                {
                    SS1_Tot_Total();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
        void SS1Col1Build()
        {
            if (nChoice == 0 && nCount == 1)
            {
                ssView_Sheet1.Cells[nCount - 1, 0].Text = "직원본인";
            }
            else if (nChoice == 1 && nCount == 1)
            {
                ssView_Sheet1.Cells[nCount - 1, 0].Text = "직원대불";
            }
            else if (nChoice == 2)
            {
                SS1_Col_Title();
            }
        }

        void SS1_Col_Title()
        {
            if (nCount == 1)
            {
                ssView_Sheet1.Cells[nCount - 1, 0].Text = "직원본인";
                Gcode = strSucode;
            }
            else if (Gcode != strSucode)
            {
                SS1_Sub_Total();
                Gcode = strSucode;
                nCount = nCount + 1;
                ssView_Sheet1.RowCount = nCount;
                ssView_Sheet1.Cells[nCount - 1, 0].Text = "직원대납";
            }
        }

        void SS1_Sub_Total()
        {
            nSubCnt = 0;
            nCount = nCount + 1;
            ssView_Sheet1.RowCount = nCount;
            ssView_Sheet1.RowHeader.Cells[nCount - 1, 0].Text = " ";
            ssView_Sheet1.Cells[nCount - 1, 2].Text = "소      계";
            ssView_Sheet1.Cells[nCount - 1, 8].Text = nSubTot.ToString("###,###,##0");
            ssView_Sheet1.Cells[nCount - 1, 9].Text = nCutAmt.ToString("###,###,##0");

            nGrdTot = nGrdTot + nSubTot;
            nGrdCut = nGrdCut + nSubCut;
            nSubTot = 0;
            nSubCut = 0;
            nCutAmt = 0;
        }

        void SS1_Tot_Total()
        {
            nSubCnt = 0;
            nCount = nCount + 1;
            ssView_Sheet1.RowCount = nCount;
            ssView_Sheet1.RowHeader.Cells[nCount - 1, 0].Text = " ";
            ssView_Sheet1.Cells[nCount - 1, 2].Text = "합      계";
            ssView_Sheet1.Cells[nCount - 1, 8].Text = nGrdTot.ToString("###,###,##0");
            ssView_Sheet1.Cells[nCount - 1, 9].Text = nGrdCut.ToString("###,###,##0");

            nSubTot = 0;
            nSubCut = 0;
            nGrdTot = 0;
            nGrdCut = 0;
        }

        void SS1BuildADD()
        {
            int i = 0;
            int j = 0;
            int nCut = 0;

            ComFunc cf = new ComFunc();

            strBiGubun = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strBi);
            strGwaName = cf.READ_DEPTNAMEK(clsDB.DbCon, strGwa);

            nCut = (int)Convert.ToDouble(nAmt % 10);                     //'절사
            nAmt = (int)Convert.ToDouble((nAmt / 10) * 10);

            nSubTot = nSubTot + nAmt;
            nSubCut = nSubCut + nCut;

            nTot = nTot + nAmt;
            nCutAmt = nCutAmt + nCut;

            ssView_Sheet1.Cells[nCount - 1, 1].Text = strPano;
            ssView_Sheet1.Cells[nCount - 1, 2].Text = strBiGubun;
            ssView_Sheet1.Cells[nCount - 1, 3].Text = strName;
            ssView_Sheet1.Cells[nCount - 1, 4].Text = strGwaName;
            ssView_Sheet1.Cells[nCount - 1, 5].Text = strRoom;
            ssView_Sheet1.Cells[nCount - 1, 6].Text = strDate;
            ssView_Sheet1.Cells[nCount - 1, 7].Text = strTDate;
            ssView_Sheet1.Cells[nCount - 1, 8].Text = Convert.ToInt32((nAmt / 10) * 10).ToString("###,###,##0");
            ssView_Sheet1.Cells[nCount - 1, 9].Text = nCut.ToString("##0");

            if (strAmset6 == "*")
            {
                ssView_Sheet1.Cells[nCount - 1, 10].Text = "구분변경자";
            }
            ssView_Sheet1.Cells[nCount - 1, 11].Text = strBigo;

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
            string PrintDate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string JobDate = "";
            string JobMan = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_Char(SysDate,'YY-MM-DD  HH24:MM') SDate ";
                SQL = SQL + ComNum.VBLF + " FROM Dual ";

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
                PrintDate = dt.Rows[0]["SDATE"].ToString().Trim();
                JobDate = dtpFdate.Text;
                JobMan = GstrJobName;

                dt.Dispose();
                dt = null;

                strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
                strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

                strHead1 = "/c/f1" + "미수금 내역 현황" + "/f1/n";   //제목 센터
                strHead2 = "/n/l/f2" + "작성자 : " + JobMan + "/f2/n";
                strHead2 = strHead2 + "/n/l/f2" + "작업일자 : " + JobDate + "/f2/n";
                strHead2 = strHead2 + "/l/f2" + "출력시간 : " + PrintDate;


                ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
                ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
                ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
                ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssView_Sheet1.PrintInfo.Margin.Top = 50;
                ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
                ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssView_Sheet1.PrintInfo.ShowBorder = true;
                ssView_Sheet1.PrintInfo.ShowColor = false;
                ssView_Sheet1.PrintInfo.ShowGrid = false;
                ssView_Sheet1.PrintInfo.ShowShadows = true;
                ssView_Sheet1.PrintInfo.UseMax = true;
                ssView_Sheet1.PrintInfo.PrintType = 0;
                ssView.PrintSheet(0);

                btnPrint.Enabled = true;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo0.Checked == true)
            {
                nChoice = 0;
            }
            else if (rdo1.Checked == true)
            {
                nChoice = 1;
            }
            else if (rdo2.Checked == true)
            {
                nChoice = 2;
            }
        }
    }
}
