using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{ /// <summary>
  /// Class Name      : ComPmpaLibB
  /// File Name       : frmPmPaVIEWJicwonmisu.cs
  /// Description     : 조합별 년간통계 조회
  /// Author          : 김효성
  /// Create Date     : 2017-09-18
  /// Update History  : 
  /// </summary>
  /// <history>  
  /// 
  /// </history>
  /// <seealso cref= "D:\psmh\IPD\ilrepd\ilrepd.vbp\FrmTaCTList(ILREPD09.FRM)  >> frmPmPaVIEWJicwonmisu.cs 폼이름 재정의" />	
    public partial class frmPmPaViewILREPD09 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu pm = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrJobName = "";

        public frmPmPaViewILREPD09(string strJobName)
        {
            GstrJobName = strJobName;

            InitializeComponent();
        }

        public frmPmPaViewILREPD09()
        {
            InitializeComponent();
        }

        private void frmPmPaViewILREPD09_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            int nYY = 0;
            int nMM = 0;

            ssView_Sheet1.RowCount = 30;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));



            for (i = 0; i <= 11; i++)
            {
                cboFDate.Items.Add(Convert.ToInt32(nYY).ToString("0000") + "년" + Convert.ToInt32(nMM).ToString("00") + "월분");
                cboTDate.Items.Add(Convert.ToInt32(nYY).ToString("0000") + "년" + Convert.ToInt32(nMM).ToString("00") + "월분");
                nMM = nMM - 1;

                if (nMM == 0)
                {
                    nMM = 12;
                    nYY = nYY - 1;
                }
            }
            cboFDate.SelectedIndex = 2;
            cboTDate.SelectedIndex = 0;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int nRow = 0;
            string strDate1 = "";
            string StrDate2 = "";
            string strOldData = "";
            string strNewData = "";
            string strGup = "";
            string strOldDept = "";
            string strDrName = "";
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            //'작업일자 From,TO Set
            strDate1 = VB.Left(cboFDate.Text, 4) + "-" + VB.Mid(cboFDate.Text, 6, 2) + "-01";
            StrDate2 = VB.Left(cboTDate.Text, 4) + "-" + VB.Mid(cboTDate.Text, 6, 2) + "-01";
            StrDate2 = CF.READ_LASTDAY(clsDB.DbCon, StrDate2);

            try
            {


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,TO_CHAR(Bdate,'YY-MM-DD') Bdate,'O' IpdOpd,DeptCode,Bi,    ";
                SQL = SQL + ComNum.VBLF + "       SuCode,GbSelf,SUM(Qty*Nal) MiCnt                                ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "Opd_SLIP                                ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1             ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate >= TO_DATE('" + strDate1 + "','YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "   AND Bdate   >= TO_DATE('" + strDate1 + "','YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "   AND Bdate   <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "   AND Bi  IN ('52','55')                                              ";
                SQL = SQL + ComNum.VBLF + "   AND Bun IN ('72','73')                                              ";
                SQL = SQL + ComNum.VBLF + "   AND GbHost <= '1'                                                   ";
                SQL = SQL + ComNum.VBLF + "GROUP  BY Pano,Bdate,DeptCode,Bi,SuCode,GbSelf                         ";
                SQL = SQL + ComNum.VBLF + "UNION  ALL                                                             ";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,TO_CHAR(Bdate,'YY-MM-DD') Bdate,'I' IpdOpd,DeptCode,Bi,    ";
                SQL = SQL + ComNum.VBLF + "       SuCode,GbSelf,SUM(Qty*Nal) MiCnt                                ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                            ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND ActDate >= TO_DATE('" + strDate1 + "','YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "   AND Bdate   >= TO_DATE('" + strDate1 + "','YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "   AND Bdate   <= TO_DATE('" + StrDate2 + "','YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "   AND Bi  IN ('52','55')                                              ";
                SQL = SQL + ComNum.VBLF + "   AND Bun IN ('72','73')                                              ";
                SQL = SQL + ComNum.VBLF + "   AND GbHost <= '1'                                                   ";
                SQL = SQL + ComNum.VBLF + "GROUP  BY Pano,Bdate,DeptCode,Bi,SuCode,GbSelf                         ";
                SQL = SQL + ComNum.VBLF + "ORDER  BY 1,2,3,4,5,6                                                  ";

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (VB.Val(dt.Rows[i]["MiCnt"].ToString().Trim()) != 0)
                    {
                        nRow = nRow + 1;
                        if (nRow != ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow + 10;
                        }

                        strNewData = dt.Rows[i]["Pano"].ToString().Trim();

                        if (strNewData != strOldData)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = CPF.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim()).Rows[0]["Sname"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["MiCnt"].ToString().Trim()).ToString();
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["GbSelf"].ToString().Trim()).ToString();

                        //'수가명칭을 READ
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT SuNameK ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN                                       ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SuNext = '" + dt.Rows[i]["SuCode"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dtFc.Rows[0]["SuNameK"].ToString().Trim().ToString();

                        dtFc.Dispose();
                        dtFc = null;

                        //'급여여부 Check
                        if (dt.Rows[i]["GbSelf"].ToString().Trim() != "0")
                        {
                            strGup = "";
                        }
                        else
                        {
                            switch (dt.Rows[i]["Bi"].ToString().Trim())
                            {
                                case "52":
                                    strGup = "급여";
                                    break;
                                default:
                                    strGup = "";
                                    break;
                            }
                        }
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = strGup;
                    }
                }

                this.ssView.ActiveSheet.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);
                this.ssView.ActiveSheet.SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);

                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = nRow;
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
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string PrintDate = "";
            string JobMan = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " Select TO_Char(SysDate,'YY-MM-DD HH24:MM') SDate";
                SQL = SQL + ComNum.VBLF + " From Dual ";

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

                PrintDate = dt.Rows[0]["SDate"].ToString().Trim();

                dt.Dispose();
                dt = null;

                JobMan = GstrJobName;

                strTitle = "자보 CT,MRI촬영 명단";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("기간 : " + cboFDate.Text + " ~ " + cboTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + PrintDate + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 180, 10);

                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, true, false);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

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
    }
}
