using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmPaViewmisubs55 : Form
    {

        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmPaViewmisubs55.cs
        /// Description     : 퇴원자심사조정내역
        /// Author          : 김효성
        /// Create Date     : 2017-08-23
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "psmh\misu\misubs\Frmmisubs55.FRM(Frmmisubs55) >> frmPmpaViewDischargePatientList.cs 폼이름 재정의" />	
        /// 
        string GstrRetValue = "";
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaViewmisubs55(string strRetValue)
        {
            GstrRetValue = strRetValue;

            InitializeComponent();
        }

        public frmPmPaViewmisubs55()
        {
            InitializeComponent();
        }

        private void frmPmPaViewmisubs55_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lblmsg.Text = "";
            Screen_Display2();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Screen_Display2();
        }

        private void Screen_Display2()    //기준 IPD_NEW_MASTER 변경
        {
            int i = 0;
            int j = 0;
            int nIPDNO = 0;
            int nTRSNO = 0;
            double nTotAmt = 0;
            string strPano = "";
            string strSname = "";
            string strBi = "";
            string strYYMM = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            nIPDNO = Convert.ToInt32(VB.Val(VB.Pstr(GstrRetValue, ",", 1)));
            nTRSNO = Convert.ToInt32(VB.Val(VB.Pstr(GstrRetValue, ",", 2)));
            strPano = Convert.ToString(VB.Val(VB.Pstr(GstrRetValue, ",", 3)));
            strSname = Convert.ToString(VB.Val(VB.Pstr(GstrRetValue, ",", 4)));
            strBi = Convert.ToString(VB.Val(VB.Pstr(GstrRetValue, ",", 5)));
            strYYMM = Convert.ToString(VB.Val(VB.Pstr(GstrRetValue, ",", 6)));

            lblmsg.Text = "등록번호 :" + strPano + "성명 :" + strSname;
            lblmsg.Text = lblmsg.Text + " IPDNO :" + nIPDNO + " TRSNO :" + nTRSNO;
            try
            {

                if (int.Parse(strYYMM) < int.Parse("201207"))
                {
                    SQL = "SELECT  A.BDATE, a.SuNext,b.SuNameK,'2' Gubun, A.BUN, '0' GBGISUL,'0' GBCHILD,  BASEAMT, GBSELF, A.PART,";
                    SQL = SQL + ComNum.VBLF + "       a.Qty, a.Nal , a.Amt1 + A.AMT2 Amt, C.BONRATE RateBon, C.BOHUN , '' GBSLIP, C.VCode, D.KORNAME, E.NAME ";
                    SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP a,BAS_SUN b , IPD_TRANS C,";
                    SQL = SQL + ComNum.VBLF + "       (SELECT TO_NUMBER(RTRIM(SABUN),'999999') SABUN, KORNAME, BUSE";
                    SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_ADM.INSA_MST) D , ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_BUSE E     ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO  = " + nIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + "   AND A.TRSNO  = " + nTRSNO + "  ";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = C.IPDNO ";
                    SQL = SQL + ComNum.VBLF + "   AND A.TRSNO = C.TRSNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext =b.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND A.PART  = D.SABUN(+)";
                    SQL = SQL + ComNum.VBLF + "   AND D.BUSE = E.BUCODE(+)";
                    SQL = SQL + ComNum.VBLF + "   AND A.PART NOT IN ( '!','#','$','+','-','?','\','^','*') ";
                    SQL = SQL + ComNum.VBLF + "   AND D.BUSE NOT IN ('100570','033102','033103') ";// '혈관조영실, 수술실, 마취과
                }

                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT    A.BDATE, a.SuNext,b.SuNameK";
                    SQL = SQL + ComNum.VBLF + "	    , '2' Gubun, A.BUN, '0' GBGISUL";
                    SQL = SQL + ComNum.VBLF + "	    , '0' GBCHILD,  BASEAMT, GBSELF, A.PART";
                    SQL = SQL + ComNum.VBLF + "	    , a.Qty, a.Nal , a.Amt1 + A.AMT2 Amt";
                    SQL = SQL + ComNum.VBLF + "	    , C.BONRATE RateBon, C.BOHUN , '' GBSLIP";
                    SQL = SQL + ComNum.VBLF + "	    , C.VCode, D.KORNAME, E.NAME ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b , " + ComNum.DB_PMPA + "IPD_TRANS C,";
                    SQL = SQL + ComNum.VBLF + "       (SELECT TO_NUMBER(RTRIM(SABUN),'999999') SABUN, KORNAME, BUSE";
                    SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_ADM.INSA_MST) D , ";
                    SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_BUSE E     ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO  = " + nIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + "   AND A.TRSNO  = " + nTRSNO + "  ";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = C.IPDNO ";
                    SQL = SQL + ComNum.VBLF + "   AND A.TRSNO = C.TRSNO ";
                    SQL = SQL + ComNum.VBLF + "   AND a.SuNext =b.SuNext(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND A.PART  = D.SABUN(+)";
                    SQL = SQL + ComNum.VBLF + "   AND D.BUSE = E.BUCODE(+)";
                    SQL = SQL + ComNum.VBLF + "   AND A.PART IN  (  SELECT SABUN                ";
                    SQL = SQL + ComNum.VBLF + "                       FROM KOSMOS_ADM.INSA_MST A, KOSMOS_PMPA.BAS_BUSE B                  ";
                    SQL = SQL + ComNum.VBLF + "                      WHERE A.BUSE  = B.BUCODE                ";
                    SQL = SQL + ComNum.VBLF + "                        AND B.NAME LIKE '%심사%'   )  ";
                }

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                nTotAmt = 0;
                ssView_Sheet1.RowCount = 0;
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count + 1;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i + 1, 0].Text = DateTime.Parse(dt.Rows[i]["BDate"].ToString().Trim()).ToString("yyyy-MM-dd");
                    ssView_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 2].Text = dt.Rows[i]["QTY"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 3].Text = dt.Rows[i]["NAL"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 4].Text = dt.Rows[i]["BASEAMT"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 5].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 6].Text = dt.Rows[i]["AMT"].ToString().Trim();
                    nTotAmt = nTotAmt + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                    ssView_Sheet1.Cells[i + 1, 7].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 8].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssView_Sheet1.Cells[i + 1, 9].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                }

                ssView_Sheet1.Cells[1 - 1, 6].Text = nTotAmt.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[1 - 1, 9].Text = " ** 합 계 ** ";
                ssView_Sheet1.RowHeader.Cells[0, 0].BackColor = Color.FromArgb(255, 0, 0);

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
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            string sHead1 = "";
            string sHead2 = "";
            string sFont1 = "";
            string sFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            sFont1 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs1";
            sFont2 = "/fn\"굴림체\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";

            btnPrint.Enabled = false;
            sHead1 = "/f1" + lblmsg.Text;
            sHead2 = "/f2" + "인쇄 일자: " + strDTP;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Header = sFont1 + sHead1 + "/n" + sFont2 + sHead2 + "/n";
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
