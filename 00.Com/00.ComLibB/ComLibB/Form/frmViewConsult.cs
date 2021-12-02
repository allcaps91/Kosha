using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Drawing;
using FarPoint.Win.Spread;

namespace ComLibB
{
    public partial class frmViewConsult : Form
    {
        public frmViewConsult()
        {
            InitializeComponent();
        }

        private void frmViewConsult_Load(object sender, EventArgs e)
        {
            //Call FormInfo_History(Me.Name, Me.Caption)

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            DataTable dt = null;

            SetYear(cboYear, 10);

            cboDept.Items.Clear();
            cboDept.Items.Add("전체");

            SQL = "SELECT DeptCode FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
            SQL = SQL + "WHERE DeptCode NOT IN ('TO','HR','HD','CS','PC','OC','II','R6',";
            SQL = SQL + "      'PT','AN','HC','OM','LM') ";
            SQL = SQL + "ORDER BY PrintRanking ";

            //Call AdoOpenSet(AdoRes, SQL)
            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboDept.Items.Add(dt.Rows[i]["DeptCode"]).ToString().Trim();
            }
            cboDept.SelectedIndex = 0;
        }
        private void SetYear(ComboBox cbo1, int intMonthCNT) //2004년 01월분
        {
            int i = 0;
            string strDate = "";
            string argGBDis = "";
            strDate = VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), 4);

            cboYear.Items.Clear();

            for (i = 0; i < intMonthCNT; i++)
            {
                if(argGBDis == "1")
                { 
                    cboYear.Items.Add(strDate);
                }
                else
                {
                    cboYear.Items.Add(strDate + "년도");
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            Set_Print();
        }

        void Set_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
   
            strTitle = "협의진료 - 임상 질 지표";
  

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 25, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, true, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
        }

        private void btnSuch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;
            int intRow = 0;
            string strDRCODE = "";
            string strFDate = "";
            string strTDate = "";
            string SqlErr = ""; //에러문 받는 변수
            int intSubTot1 = 0;
            string SQL = "";
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            strFDate = VB.Left(cboYear.Text.Trim(), 4) + "-01-01";
            strFDate = VB.Left(cboYear.Text.Trim(), 4) + "-12-31";

            btnSuch.Enabled = false;
            try
            {
                SQL = " SELECT TO_CHAR(BDATE, 'MM') MONTH, c.PrintRanking,  A.TODEPTCODE DEPTCODE, A.TODRCODE DRCODE, B.DRNAME, COUNT(*) QTY  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_ITRANSFER A, BAS_DOCTOR b,BAS_CLINICDEPT c  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.GBCONFIRM ='*' ";
                SQL = SQL + ComNum.VBLF + "   AND A.GBDEL <> '*'  ";
                SQL = SQL + ComNum.VBLF + "   AND A.GBFLAG ='1' ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + strFDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strTDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.Ptno <> '81000004' "; //"전산실연습은 제외";
                SQL = SQL + ComNum.VBLF + "   AND to_char(bdate ,'DY') not in ( 'SAT','SUN') ";  //'휴일 토요일, 공휴일  제외;
                SQL = SQL + ComNum.VBLF + "   AND BDATE NOT IN ( SELECT JOBDATE FROM BAS_JOB WHERE HOLYDAY ='*' ) ";

                //If Trim(ComboDept.Text) <> "전체" Then SQL = SQL & "  AND a.TODEPTCODE ='" & Trim(ComboDept.Text) & "' " & vbLf;
                if (cboDept.Text.Trim () != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "  AND a.TODEPTCODE ='" + (cboDept.Text.Trim ()) + "' ";
                }

                //If OptER(1).Value = True Then SQL = SQL & " AND A.GBEMSMS ='Y' " '응급
                if (optER1.Checked == true)
                {
                    SQL = SQL + " AND A.GBEMSMS ='Y' "; //'응급;
                }
                //If OptER(2).Value = True Then SQL = SQL & " AND (A.GBEMSMS <>'Y' OR A.GBEMSMS IS NULL)  " '비응급
                if (optER2.Checked == true)
                {
                    SQL = SQL + " AND (A.GBEMSMS <>'Y' OR A.GBEMSMS IS NULL)  "; //'비응급
                }

                //If Opt24(1).Value = True Then SQL = SQL & " AND  TRUNC(EDATE -SDATE) = 0  "
                //If Opt24(2).Value = True Then SQL = SQL & " AND  TRUNC(EDATE -SDATE) > 0  "
                if (opt24go.Checked == true)
                {
                    SQL = SQL + " AND  TRUNC(EDATE -SDATE) = 0  ";
                }
                if (opt24back.Checked == true)
                {
                    SQL = SQL + " AND  TRUNC(EDATE -SDATE) > 0  ";
                }

                SQL = SQL + "   AND a.ToDeptcode=c.DeptCode(+) ";
                SQL = SQL + "   AND a.Todrcode=b.DrCode(+) ";
                SQL = SQL + " GROUP BY TO_CHAR(BDATE, 'mm') , c.PrintRanking,a.ToDeptcode,a.Todrcode,b.DrName ";
                SQL = SQL + " ORDER BY c.PrintRanking,a.ToDeptcode,a.Todrcode,b.DrName,TO_CHAR(BDATE, 'mm') ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                intSubTot1 = 0;
                strDRCODE = "";
                intRow = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strDRCODE != dt.Rows [i] ["DRCODE"].ToString ().Trim ())
                    {

                        if (i != 0)
                        {
                            ssView_Sheet1.Cells [intRow , 13].Text = Convert.ToString (intSubTot1);
                        }

                        intRow = intRow + 1;

                        if (intRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = intRow;
                        }
                        strDRCODE = dt.Rows [i] ["drname"].ToString ().Trim ();
                        intSubTot1 = 0;

                    }
                    ssView_Sheet1.Cells [i , Convert.ToInt32 (VB.Val (dt.Rows [i] ["Month"].ToString ().Trim ()))].Text = dt.Rows [i] ["qty"].ToString ().Trim ();
                    intSubTot1 = intSubTot1 + Convert.ToInt32 (VB.Val (dt.Rows [i] ["qty"].ToString ().Trim ()));
                }

                if (intRow > dt.Rows.Count)
                {
                    ssView_Sheet1.RowCount = intRow;
                }

                if (i != 0)
                {
                    ssView_Sheet1.Cells [intRow , 13].Text = Convert.ToString (intSubTot1);
                }

                strDRCODE = "";
                dt.Dispose ();
                dt = null;
                Cursor.Current = Cursors.Default;

                intRow = intRow + 1;

                if (intRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = intRow + 1;
                }

                ssView_Sheet1.Cells [ssView_Sheet1.RowCount - 1 , 1].Text = "합 계";

                for (i = 1; i < ssView_Sheet1.ColumnCount; i++)
                {
                    intSubTot1 = 0;

                    for (j = 0; j < ssView_Sheet1.RowCount; j++)
                    {
                        intSubTot1 = intSubTot1 + Convert.ToInt32 (VB.Val (ssView_Sheet1.Cells [j , i].Text));
                    }

                    ssView_Sheet1.Cells [j , i].Text = VB.Format (intSubTot1 , "##,###,###,##0");
                    intSubTot1 = 0;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

    }
}
