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
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-11
    /// Update History  : 
    /// </summary>+
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misubs\misubs.vbp\misubs60_2.frm(FrmReMirView2_1.frm) >> frmPmpaViewmisubs602.cs 폼이름 재정의" />
    /// <seealso cref= "D:\psmh\misu\misubs\misubs.vbp\misubs60_3.frm(FrmReMirView2_2.frm) >> frmPmpaViewmisubs602.cs 폼이름 재정의" />

    public partial class frmPmpaViewmisubs602 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string GstrYYMM = "";
        string GstrMenu = "";
        string GstrSMenu = "";
        string GstrSakYYMM = "";
        string GstrSakIO = "";
        string GstrSakJohap = "";
        string GstrSakGBN = "";

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewmisubs602()
        {
            InitializeComponent();
        }

        public frmPmpaViewmisubs602(string strstrYYMM, string strstrMenu, string strstrSMenu, string strstrSakYYMM, string strstrSakIO, string strstrSakJohap, string strstrSakGBN)
        {
            GstrSakYYMM = strstrSakYYMM;
            GstrYYMM = strstrYYMM;
            GstrMenu = strstrMenu;
            GstrSMenu = strstrSMenu;
            GstrSakIO = strstrSakIO;
            GstrSakJohap = strstrSakJohap;
            GstrSakGBN = strstrSakGBN;
            InitializeComponent();
        }

        private void frmPmpaViewmisubs602_Load(object sender, System.EventArgs e)
        {
            //    if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //    {
            //        this.Close();
            //        return;
            //    } //폼 권한 조회
            //    ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboEDIyyyy0, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboEDIyyyy1, 24, "", "1");

            if (rdosel0.Checked == true)
            {
                pansel0.Visible = true;
                pansel1.Visible = false;
            }
            if (rdosel1.Checked == true)
            {
                pansel0.Visible = false;
                pansel1.Visible = true;
            }

            ssFive.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int K = 0;
            int nRead = 0;
            int nRead2 = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strBi = "";
            int nBiNo = 0;
            double nAmt = 0;
            int nRow = 0;
            int nRow2 = 0;
            string strTeYYMM = "";
            double nSakAmt = 0;
            double nTotCAmt = 0;
            double nTotCCnt = 0;
            double nTotSakAmt = 0;
            double nTotReMir = 0;
            double nTotResultAmt = 0;
            double nTotSakAmtOut = 0;
            double nTotRemirOut = 0;
            double nTotResultAmtOut = 0;
            double nTotRemirOutT = 0;
            double nTotResultOutT = 0;

            DataTable dt = null;
            DataTable dtFc = null;
            DataTable dtFc1 = null;
            DataTable dtFc2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int sel = 1;        //보험 조건1 = "1" , 보험 조건2 = "2"

            strYYMM = VB.Left(cboEDIyyyy0.Text, 4) + VB.Mid(cboEDIyyyy0.Text, 7, 2);

            strTeYYMM = (VB.Val(VB.Left(cboEDIyyyy0.Text, 4)) - 1).ToString(); //'2016-04-01

            strFYYMM = VB.Left(cboEDIyyyy0.Text, 4) + VB.Mid(cboEDIyyyy0.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";

            strTYYMM = VB.Left(cboEDIyyyy1.Text, 4) + VB.Mid(cboEDIyyyy1.Text, 7, 2);
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01");

            //'jjy(2003-01-13) '통계 remark 등록 공용변수
            GstrYYMM = strFYYMM;
            GstrMenu = "4";
            GstrSMenu = "1";

            nTotCAmt = 0;
            nTotCCnt = 0;
            nTotSakAmt = 0;
            nTotReMir = 0;
            nTotResultAmt = 0;
            nTotSakAmtOut = 0;
            nTotRemirOut = 0;
            nTotRemirOutT = 0;
            nTotResultOutT = 0;
            nTotResultAmtOut = 0;

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            try
            {

                #region 통합 보험조건1
                if (rdosel0.Checked == true)
                {
                    sel = 1;

                    if (rdoG0.Checked == true)
                    {

                        SQL = "";
                        SQL = "SELECT";
                        SQL += ComNum.VBLF + " DEPTCODE1 , SUM (EDITAMT) CAMT, COUNT (WRTNO) CCNT ";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                        SQL += ComNum.VBLF + "  WHERE 1=1";
                        SQL += ComNum.VBLF + "    AND EDIMIRNO IN (";

                        SQL += ComNum.VBLF + "   SELECT MIRNO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_JEPSU A ";
                        SQL += ComNum.VBLF + "       WHERE 1=1";
                        SQL += ComNum.VBLF + "          AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "          AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";


                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + "                    ) ";
                        SQL += ComNum.VBLF + " GROUP BY DEPTCODE1 ";
                        SQL += ComNum.VBLF + " union all ";
                        SQL += ComNum.VBLF + "  select '',0,0 from dual  ";
                    }
                    else
                    {

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT DEPTCODE1 ,SUM(EDITAMT) CAMT, COUNT(WRTNO) CCNT ";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                        SQL += ComNum.VBLF + "  WHERE 1=1";
                        SQL += ComNum.VBLF + "    AND EDIMIRNO IN (";
                        SQL += ComNum.VBLF + "        SELECT MIRNO ";
                        SQL += ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "EDI_JEPSU A ";
                        SQL += ComNum.VBLF + "          WHERE YYMM >='" + strFYYMM + "'";
                        SQL += ComNum.VBLF + "           AND YYMM <='" + strTYYMM + "'";

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + "                    ) ";

                        SQL += ComNum.VBLF + " GROUP BY DEPTCODE1 ";

                        SQL += ComNum.VBLF + " union all ";
                        SQL += ComNum.VBLF + "  select '',0,0 ";
                        SQL += ComNum.VBLF + "  from dual  ";
                    }
                }

                #endregion


                #region 통합 보험조건2
                if (rdosel1.Checked == true)
                {
                    sel = 2;

                    if (rdoG0.Checked == true)
                    {
                        //'산재
                        if (rdoselGB0.Checked == true)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT DEPTCODE DeptCode1 , SUM(EDITAMT) CAMT, COUNT(WRTNO) CCNT ";
                            SQL += ComNum.VBLF + " FROM MIR_sanID ";
                            SQL += ComNum.VBLF + "  WHERE 1=1";
                            SQL += ComNum.VBLF + "    AND EDIMIRNO IN (";
                            SQL += ComNum.VBLF + "                      SELECT MIRNO ";
                            SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_sanJEPSU A ";
                            SQL += ComNum.VBLF + "                        WHERE 1=1";
                            SQL += ComNum.VBLF + "                          AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

                            if (rdoIO0.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                            }
                            if (rdoIO1.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                            }
                            SQL += ComNum.VBLF + "                    ) ";

                            SQL += ComNum.VBLF + " GROUP BY DEPTCODE ";
                            SQL += ComNum.VBLF + " union all ";
                            SQL += ComNum.VBLF + "  select '',0,0 from dual  ";
                        }
                        else if (rdoselGB1.Checked == true)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT DEPTCODE1 , SUM(EDITAMT) CAMT, COUNT(WRTNO) CCNT ";
                            SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_taID ";
                            SQL += ComNum.VBLF + "  WHERE 1=1";
                            SQL += ComNum.VBLF + "    AND EDIMIRNO IN (";
                            SQL += ComNum.VBLF + "                      SELECT MIRNO ";
                            SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_taJEPSU A ";
                            SQL += ComNum.VBLF + "                        WHERE 1=1";
                            SQL += ComNum.VBLF + "                         AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                         AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

                            if (rdoIO0.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                            }
                            if (rdoIO1.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                            }

                            SQL += ComNum.VBLF + "                    ) ";
                            SQL += ComNum.VBLF + " GROUP BY DEPTCODE1 ";
                            SQL += ComNum.VBLF + " union all ";
                            SQL += ComNum.VBLF + "  select '',0,0 from dual  ";
                        }
                    }
                    else
                    {
                        //'산재
                        if (rdoselGB0.Checked == true)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT DEPTCODE DeptCode1  ,SUM(EDITAMT) CAMT, COUNT(WRTNO) CCNT ";
                            SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_sanID ";
                            SQL += ComNum.VBLF + "  WHERE 1=1";
                            SQL += ComNum.VBLF + "    AND EDIMIRNO IN (";
                            SQL += ComNum.VBLF + "                      SELECT MIRNO ";
                            SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_sanJEPSU A ";
                            SQL += ComNum.VBLF + "                        WHERE 1=1";
                            SQL += ComNum.VBLF + "                         AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                         AND YYMM <='" + strTYYMM + "'";

                            if (rdoIO0.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                            }
                            if (rdoIO1.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                            }

                            SQL += ComNum.VBLF + "                    ) ";
                            SQL += ComNum.VBLF + " GROUP BY DEPTCODE ";
                            SQL += ComNum.VBLF + " union all ";
                            SQL += ComNum.VBLF + "  select '',0,0 from dual  ";
                        }

                        else if (rdoselGB1.Checked == true)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT DEPTCODE1   ,SUM(EDITAMT) CAMT, COUNT(WRTNO) CCNT ";
                            SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_taID ";
                            SQL += ComNum.VBLF + "  WHERE 1=1";
                            SQL += ComNum.VBLF + "    AND EDIMIRNO IN (";


                            SQL += ComNum.VBLF + "                      SELECT MIRNO ";
                            SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_taJEPSU A ";
                            SQL += ComNum.VBLF + "                        WHERE 1=1";
                            SQL += ComNum.VBLF + "                         AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                         AND YYMM <='" + strTYYMM + "'";
                            if (rdoIO0.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                            }
                            if (rdoIO1.Checked == true)
                            {
                                SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                            }

                            SQL += ComNum.VBLF + "                    ) ";
                            SQL += ComNum.VBLF + " GROUP BY DEPTCODE1 ";
                            SQL += ComNum.VBLF + " union all ";
                            SQL += ComNum.VBLF + "  select '',0,0 from dual  ";
                        }
                    }
                }
                #endregion


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nRow = 0;
                progressBar1.Maximum = dt.Rows.Count;
                progressBar1.Value = 0;

                for (K = 0; K < dt.Rows.Count; K++)
                {

                    progressBar1.Value = (K + 1);
                    Application.DoEvents();
                    nRow = nRow + 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[K]["DEPTCODE1"].ToString().Trim();

                    ssView_Sheet1.Cells[nRow - 1, 1].Text = VB.Val(dt.Rows[K]["CCNT"].ToString().Trim()).ToString("##,###,###,##0 ");
                    nTotCCnt = nTotCCnt + VB.Val(dt.Rows[K]["CCNT"].ToString().Trim());

                    ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Val(dt.Rows[K]["CAMT"].ToString().Trim()).ToString("##,###,###,##0 ");
                    nTotCAmt = nTotCAmt + VB.Val(dt.Rows[K]["CAMT"].ToString().Trim());

                    #region 보험조건1

                    if (sel == 1)
                    {

                        nSakAmt = 0;

                        //'삭감 보험
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT jepno,a.actdate,'' KIHO, '' BOHUN, A.CHASU , '01' BUN, ";
                        SQL += ComNum.VBLF + " TRUNC( SUM(";
                        SQL += ComNum.VBLF + "     DECODE(RTRIM(A.JCODE),";
                        SQL += ComNum.VBLF + "     'E',  ((A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  ) /100 + a.DJAMT),";
                        SQL += ComNum.VBLF + "     'F',  ((A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  ) /100 + a.DJAMT),";
                        SQL += ComNum.VBLF + "     'L',  ((A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  ) /100 + a.DJAMT),";
                        SQL += ComNum.VBLF + "     'TE',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "     'TG',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "           ((A.JAMT   * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) ) *  DECODE(C.BOHUN,'3 ',( 100 - A.RATEBON ) /100,1)  + a.DJAMT)))) SAMT ";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_F0203 A,  " + ComNum.DB_PMPA + "MIR_INSID C, " + ComNum.DB_PMPA + "BAS_MIA D  ";

                        SQL += ComNum.VBLF + "  WHERE 1=1";
                        SQL += ComNum.VBLF + "  AND (A.JEPNO ) in (";
                        SQL += ComNum.VBLF + "                   SELECT jepno ";
                        SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + "                                 )";
                        SQL += ComNum.VBLF + " AND c.JOHAP ='1' ";

                        SQL += ComNum.VBLF + "    AND C.DeptCode1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND c.yymm >='" + strTeYYMM + "' ";

                        SQL += ComNum.VBLF + "    AND A.MIRNO = C.EDIMIRNO";
                        SQL += ComNum.VBLF + "    AND A.MIRSEQ =  C.SEQNO";
                        SQL += ComNum.VBLF + "    AND A.MIRSEQ =  LTRIM(TO_CHAR(C.SEQNO,'99999'))";

                        SQL += ComNum.VBLF + "    AND A.JCODE >= 'A'";
                        SQL += ComNum.VBLF + "    AND A.JCODE <= 'ZZ'";
                        SQL += ComNum.VBLF + "    AND C.KIHO = D.MIACODE(+)";
                        SQL += ComNum.VBLF + " Group By jepno,a.actdate,A.CHASU ";
                        SQL += ComNum.VBLF + "  UNION ALL ";

                        SQL += ComNum.VBLF + " SELECT  jepno,a.actdate,C.KIHO KIHO, '' BOHUN, A.CHASU, '08' BUN , ";
                        SQL += ComNum.VBLF + "    TRUNC(SUM(DECODE(RTRIM(A.JCODE),";
                        SQL += ComNum.VBLF + "               'E',  (A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (A.RATEBON)  /100 ),";
                        SQL += ComNum.VBLF + "               'F',  (A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (A.RATEBON)  /100 ),";
                        SQL += ComNum.VBLF + "               'TE',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "               'TG',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "                     (A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (A.RATEBON /100))))) SAMT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0203 A,  MIR_INSID C, BAS_MIA D ";

                        SQL += ComNum.VBLF + "  WHERE 1=1";
                        SQL += ComNum.VBLF + "    AND (A.JEPNO ) in (";
                        SQL += ComNum.VBLF + "                   SELECT jepno";
                        SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        if (rdoIO1.Checked == true)
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        if (rdoGB0.Checked == true)
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        if (rdoGB1.Checked == true)
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";

                        SQL += ComNum.VBLF + "                                 )";

                        SQL += ComNum.VBLF + " AND c.JOHAP ='1' ";

                        SQL += ComNum.VBLF + "    AND C.DeptCode1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND c.yymm >='" + strTeYYMM + "' ";
                        SQL += ComNum.VBLF + "    AND A.MIRNO = C.EDIMIRNO";
                        SQL += ComNum.VBLF + "    AND A.MIRSEQ =  LTRIM(TO_CHAR(C.SEQNO,'99999'))";
                        SQL += ComNum.VBLF + "    AND A.JCODE >='A'";
                        SQL += ComNum.VBLF + "    AND A.JCODE <='ZZ'";
                        SQL += ComNum.VBLF + "    AND C.BOHUN = '3 '";// '보호장애 대불 있을 경우만
                        SQL += ComNum.VBLF + "    AND C.KIHO = D.MIACODE(+)";
                        SQL += ComNum.VBLF + " GROUP BY jepno,a.actdate,C.KIHO, A.CHASU ";

                        SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {

                            dt.Dispose();
                            dt = null;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;

                        }

                        for (j = 0; j < dtFc.Rows.Count; j++)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT A.ROWID, A.WRTNO   ";
                            SQL += ComNum.VBLF + "  FROM ADMIN.MISU_SLIP A , MISU_IDMST B ";
                            SQL += ComNum.VBLF + "  WHERE 1=1";
                            SQL += ComNum.VBLF + "    AND A.MISUID = '" + Convert.ToInt32(dtFc.Rows[j]["jepno"].ToString().Trim()).ToString("00000000") + "' ";
                            SQL += ComNum.VBLF + "    AND A.GUBUN ='11'";

                            if (dtFc.Rows[j]["KIHO"].ToString().Trim() != "")
                            {
                                switch (dtFc.Rows[j]["KIHO"].ToString().Trim())
                                {
                                    case "00000000000":
                                    case "*":
                                        SQL += ComNum.VBLF + "     AND A.GELCODE IN (  '" + dtFc.Rows[j]["KIHO"].ToString().Trim() + "' ,'0011')  ";
                                        break;
                                    default:
                                        SQL += ComNum.VBLF + "     AND A.GELCODE IN (  '" + dtFc.Rows[j]["KIHO"].ToString().Trim() + "')  ";
                                        break;
                                }
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "   AND A.QTY <> '0' ";// '보험상한대불 제외
                            }

                            SQL += ComNum.VBLF + "   AND A.WRTNO = B.WRTNO ";

                            if (dtFc.Rows[j]["BUN"].ToString().Trim() == "08")
                            {
                                SQL += ComNum.VBLF + "   AND B.BUN = '08'";
                            }
                            else if (dtFc.Rows[j]["BUN"].ToString().Trim() == "22")
                            {
                                SQL += ComNum.VBLF + "   AND B.BUN = '22'";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "   AND B.BUN NOT IN ( '22','08' ) ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dtFc1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                btnSearch.Enabled = true;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            if (dtFc1.Rows.Count > 0)
                            {
                                nSakAmt = nSakAmt + VB.Val(dtFc.Rows[j]["samt"].ToString().Trim());
                            }

                            dtFc1.Dispose();
                            dtFc1 = null;
                        }

                        dtFc.Dispose();
                        dtFc = null;

                        //'삭감 의료급여

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT jepno,a.actdate,'B' || D.MIADETAIL KIHO , '01' BUN ,";
                        SQL += ComNum.VBLF + " TRUNC( SUM(";
                        SQL += ComNum.VBLF + "     DECODE(RTRIM(A.JCODE),";
                        SQL += ComNum.VBLF + "     'E',  ((A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  ) /100 + a.DJAMT) ,";
                        SQL += ComNum.VBLF + "     'L',  ((A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  ) /100 + a.DJAMT) ,";
                        SQL += ComNum.VBLF + "     'F',  ((A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  ) /100 + a.DJAMT),";
                        SQL += ComNum.VBLF + "     'TE',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "     'TG',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "           ((A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) ) *  DECODE(C.BOHUN,'3 ',( 100 - A.RATEBON ) /100,1) + a.DJAMT) ))) SAMT ";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_F0603 A,  " + ComNum.DB_PMPA + "MIR_INSID C, " + ComNum.DB_PMPA + "BAS_MIA D  ";

                        SQL += ComNum.VBLF + "  WHERE 1=1";
                        SQL += ComNum.VBLF + "    AND (A.JEPNO ) in (";
                        SQL += ComNum.VBLF + "                   SELECT jepno";
                        SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                               AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                               AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        if (rdoIO1.Checked == true)
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        if (rdoGB0.Checked == true)
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        if (rdoGB1.Checked == true)
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";

                        SQL += ComNum.VBLF + "                                 )";
                        SQL += ComNum.VBLF + " AND c.JOHAP ='5' ";

                        SQL += ComNum.VBLF + "    AND C.DeptCode1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND c.yymm >='" + strTeYYMM + "' ";

                        SQL += ComNum.VBLF + "    AND A.MIRNO = C.EDIMIRNO";
                        SQL += ComNum.VBLF + "    AND A.MIRSEQ =  C.SEQNO";
                        SQL += ComNum.VBLF + "    AND A.MIRSEQ =  LTRIM(TO_CHAR(C.SEQNO,'99999'))";

                        SQL += ComNum.VBLF + "    AND A.JCODE >= 'A'";
                        SQL += ComNum.VBLF + "    AND A.JCODE <= 'ZZ'";
                        SQL += ComNum.VBLF + "    AND C.KIHO = D.MIACODE(+)";
                        SQL += ComNum.VBLF + " Group By jepno,a.actdate,'B' || D.MIADETAIL";

                        SQL += ComNum.VBLF + "  UNION ALL ";

                        SQL += ComNum.VBLF + " SELECT  jepno,a.actdate,C.KIHO KIHO, '08' BUN, ";
                        SQL += ComNum.VBLF + "    TRUNC(SUM(DECODE(RTRIM(A.JCODE),";
                        SQL += ComNum.VBLF + "               'E',  (A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (A.RATEBON)  /100 ),";
                        SQL += ComNum.VBLF + "               'F',  (A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (A.RATEBON)  /100 ),";
                        SQL += ComNum.VBLF + "               'L',  (A.JAMT  * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (A.RATEBON)  /100 ),";
                        SQL += ComNum.VBLF + "               'TE',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "               'TG',  C.EDIJAMT,";
                        SQL += ComNum.VBLF + "                     (A.JAMT * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (A.RATEBON /100)  ) ))) SAMT ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0603 A,  " + ComNum.DB_PMPA + "MIR_INSID C, " + ComNum.DB_PMPA + "BAS_MIA D ";
                        SQL += ComNum.VBLF + "  WHERE 1=1";
                        SQL += ComNum.VBLF + "    AND (A.JEPNO  ) in (";
                        SQL += ComNum.VBLF + "                   SELECT jepno";
                        SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                               AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + "                                 )";
                        SQL += ComNum.VBLF + " AND c.JOHAP ='5' ";

                        SQL += ComNum.VBLF + "    AND C.DeptCode1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND c.yymm >='" + strTeYYMM + "' ";

                        SQL += ComNum.VBLF + "    AND A.MIRNO = C.EDIMIRNO";

                        SQL += ComNum.VBLF + "    AND A.MIRSEQ =  LTRIM(TO_CHAR(C.SEQNO,'99999'))";
                        SQL += ComNum.VBLF + "    AND A.JCODE >= 'A'";
                        SQL += ComNum.VBLF + "    AND A.JCODE <= 'ZZ'";
                        SQL += ComNum.VBLF + "    AND C.BOHUN = '3 '";// '보호장애 대불 있을 경우만

                        SQL += ComNum.VBLF + "    AND C.KIHO = D.MIACODE(+)";
                        SQL += ComNum.VBLF + " GROUP BY jepno,a.actdate,C.KIHO ";

                        SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        for (j = 0; j < dtFc.Rows.Count; j++)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT A.ROWID, A.WRTNO   ";
                            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_SLIP A , " + ComNum.DB_PMPA + "MISU_IDMST B";
                            SQL += ComNum.VBLF + "  WHERE 1=1";
                            SQL += ComNum.VBLF + "    AND A.MISUID = '" + VB.Val(dtFc.Rows[j]["jepno"].ToString().Trim()).ToString("00000000") + "' ";
                            SQL += ComNum.VBLF + "    AND A.GUBUN ='11'";
                            SQL += ComNum.VBLF + "     AND A.GELCODE = '" + dtFc.Rows[j]["KIHO"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "   AND A.WRTNO = B.WRTNO ";

                            if (dtFc.Rows[j]["BUN"].ToString().Trim() == "08")
                            {
                                SQL += ComNum.VBLF + "   AND B.BUN = '08'";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "   AND B.BUN <>'08' ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dtFc1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                btnSearch.Enabled = true;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            if (dtFc1.Rows.Count > 0)
                            {
                                nSakAmt = nSakAmt + Convert.ToDouble(dtFc.Rows[j]["samt"].ToString().Trim());
                            }

                            dtFc1.Dispose();
                            dtFc1 = null;
                        }

                        dtFc.Dispose();
                        dtFc = null;

                        ssView_Sheet1.Cells[nRow - 1, 3].Text = nSakAmt.ToString("##,###,###,###,##0 ");
                        nTotSakAmt = nTotSakAmt + nSakAmt;

                        //(%)
                        if (nSakAmt != 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = (nSakAmt / VB.Val(dt.Rows[K]["CAMT"].ToString().Trim()) * 100).ToString("##0.00 ");
                        }

                        //'이의신청
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SUM(a.REAMT1 + a.REAMT2) AS REMIR, SUM(a.RESULTAMT) AS RESULTAMT ,SUM(a.REOUTAMT1 + a.REOUTAMT2) AS REMIROUT ";
                        SQL += ComNum.VBLF + "  From " + ComNum.DB_PMPA + "MIR_REMIRDTL a, " + ComNum.DB_PMPA + "mir_insid c";
                        SQL += ComNum.VBLF + "  WHERE  1=1";
                        SQL += ComNum.VBLF + "   AND  a.wrtno=c.wrtno";
                        SQL += ComNum.VBLF + "   AND A.WRTNO IN ( ";
                        SQL += ComNum.VBLF + "                    SELECT WRTNO FROM MIR_INSID ";
                        SQL += ComNum.VBLF + "                     WHERE  EDIMIRNO IN (";
                        SQL += ComNum.VBLF + "                                        SELECT mirno";
                        SQL += ComNum.VBLF + "                                         FROM EDI_JEPSU ";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                               AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + "                                 )";
                        SQL += ComNum.VBLF + "           )";

                        SQL += ComNum.VBLF + "  and c.DeptCode1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND c.IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND c.IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND c.JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND c.JOHAP ='5' ";
                        }

                        SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dtFc.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dtFc.Rows[0]["REMIR"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                            nTotReMir = nTotReMir + VB.Val(dtFc.Rows[0]["REMIR"].ToString().Trim());

                            //(%)
                            if (nSakAmt != 0)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = (VB.Val(dtFc.Rows[0]["REMIR"].ToString().Trim()) / (nSakAmt) * 100).ToString("##0.00 ");
                                Application.DoEvents();
                            }

                            ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dtFc.Rows[0]["RESULTAMT"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                            nTotResultAmt = nTotResultAmt + VB.Val(dtFc.Rows[0]["RESULTAMT"].ToString().Trim());

                            //(%)
                            if (VB.Val(dtFc.Rows[0]["RESULTAMT"].ToString().Trim()) != 0)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = (VB.Val(dtFc.Rows[0]["RESULTAMT"].ToString().Trim()) / VB.Val(dtFc.Rows[0]["REMIR"].ToString().Trim()) * 100).ToString("##0.00 ");
                            }
                        }
                        dtFc.Dispose();
                        dtFc = null;

                        //'삭감원외

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SUM(C.AMT1) SAKAMTOUT ";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_F0901 A, " + ComNum.DB_PMPA + "EDI_F0903 C, " + ComNum.DB_PMPA + "MIR_INSID D";
                        SQL += ComNum.VBLF + " WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND A.JEPNO = C.JEPNO";
                        SQL += ComNum.VBLF + "      AND A.MIRNO =D.EDIMIRNO";
                        SQL += ComNum.VBLF + "      AND C.MIRSEQ =D.SEQNO";
                        SQL += ComNum.VBLF + "      AND (A.JEPNO,A.MUKNO) IN ( ";
                        SQL += ComNum.VBLF + "                             SELECT jepno,mukno FROM EDI_JEPSU";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                               AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + " )";
                        SQL += ComNum.VBLF + "  and d.DeptCode1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dtFc, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dtFc.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dtFc.Rows[0]["SAKAMTOUT"].ToString().Trim()).ToString("##,###,###,###,##0 ");
                            nTotSakAmtOut = nTotSakAmtOut + VB.Val(dtFc.Rows[0]["SAKAMTOUT"].ToString().Trim());
                        }

                        dtFc.Dispose();
                        dtFc = null;

                        //'이의신청 원외
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT  REMIRNO,PANO,MIRSEQ,SUM(REOUTAMT1+REOUTAMT2) REOUTAMT ";
                        SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_REMIRDTL ";
                        SQL += ComNum.VBLF + "  WHERE REMIRNO IN ( ";
                        SQL += ComNum.VBLF + " SELECT REMIRNO  FROM MIR_REMIRMST ";
                        SQL += ComNum.VBLF + "   WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND (JEPNO,YYMM) IN (";
                        SQL += ComNum.VBLF + "         SELECT JEPNO,YYMM ";
                        SQL += ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "EDI_JEPSU";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                               AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + " )";

                        SQL += ComNum.VBLF + " GROUP BY REMIRNO";
                        SQL += ComNum.VBLF + " HAVING SUM(REOUTAMT1 + REOUTAMT2) <> 0";
                        SQL += ComNum.VBLF + " )";
                        SQL += ComNum.VBLF + " GROUP BY REMIRNO,PANO,MIRSEQ";

                        SqlErr = clsDB.GetDataTable(ref dtFc1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        nTotRemirOutT = 0;

                        if (dtFc1.Rows.Count > 0)
                        {
                            for (j = 0; j < dtFc1.Rows.Count; j++)
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT ROWID ";
                                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                                SQL += ComNum.VBLF + " WHERE 1=1";
                                SQL += ComNum.VBLF + "  AND PANO ='" + VB.Val(dtFc1.Rows[j]["PANO"].ToString().Trim()) + "' ";
                                SQL += ComNum.VBLF + "  AND SEQNO ='" + VB.Val(dtFc1.Rows[j]["MIRSEQ"].ToString().Trim()).ToString("00000") + "' ";
                                SQL += ComNum.VBLF + "  AND DEPTCODE1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";
                                SQL += ComNum.VBLF + "   AND (PANO,EDIMIRNO) IN (";
                                SQL += ComNum.VBLF + "         SELECT PANO,MIRNO ";
                                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "FROM EDI_JEPSU";

                                if (rdoG0.Checked == true)
                                {
                                    SQL += ComNum.VBLF + "                               WHERE 1=1";
                                    SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                                    SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                                }
                                else
                                {
                                    SQL += ComNum.VBLF + "                               WHERE 1=1";
                                    SQL += ComNum.VBLF + "                               WHERE YYMM >='" + strFYYMM + "'";
                                    SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                                }

                                if (rdoIO0.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                                }
                                if (rdoIO1.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                                }

                                if (rdoGB0.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                                }
                                if (rdoGB1.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                                }

                                SQL += ComNum.VBLF + " )";

                                SqlErr = clsDB.GetDataTable(ref dtFc2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (dtFc2.Rows.Count > 0)
                                {
                                    nTotRemirOutT = nTotRemirOutT + VB.Val(dtFc1.Rows[j]["REOUTAMT"].ToString().Trim());

                                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotRemirOutT.ToString("##,###,###,###,##0 ");

                                    nRow2 = nRow2 + 1;
                                    ssFive_Sheet1.Cells[nRow2 - 1, 0].Text = dtFc1.Rows[j]["PANO"].ToString().Trim();
                                    ssFive_Sheet1.Cells[nRow2 - 1, 1].Text = VB.Val(dtFc1.Rows[j]["MIRSEQ"].ToString().Trim()).ToString("00000");
                                    ssFive_Sheet1.Cells[nRow2 - 1, 2].Text = dt.Rows[K]["DEPTCODE1"].ToString().Trim();
                                    ssFive_Sheet1.Cells[nRow2 - 1, 3].Text = dtFc1.Rows[j]["REOUTAMT"].ToString().Trim();
                                }

                                dtFc2.Dispose();
                                dtFc2 = null;
                            }
                        }
                        dtFc1.Dispose();
                        dtFc1 = null;

                        nTotRemirOut = nTotRemirOut + nTotRemirOutT;

                        //'이의신청인정 원외
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT A.MIRNO,MIRSEQ,SUM(B.BAMT2) RESULTOUTAMT ";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_F0701 A, " + ComNum.DB_PMPA + "EDI_F0703 B";
                        SQL += ComNum.VBLF + " WHERE 1=1";
                        SQL += ComNum.VBLF + "    AND A.JEPNO = B.JEPNO";
                        SQL += ComNum.VBLF + "    AND A.JCHASU=B.JCHASU";
                        SQL += ComNum.VBLF + "    AND A.JEPNO IN ( ";
                        SQL += ComNum.VBLF + "         SELECT JEPNO ";
                        SQL += ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "EDI_JEPSU";

                        if (rdoG0.Checked == true)
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                                AND JEPDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "                                AND JEPDATE <=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "                               WHERE 1=1";
                            SQL += ComNum.VBLF + "                               AND YYMM >='" + strFYYMM + "'";
                            SQL += ComNum.VBLF + "                                AND YYMM <='" + strTYYMM + "'";
                        }

                        if (rdoIO0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                        }
                        if (rdoIO1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                        }

                        if (rdoGB0.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                        }
                        if (rdoGB1.Checked == true)
                        {
                            SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                        }

                        SQL += ComNum.VBLF + " )";
                        SQL += ComNum.VBLF + " GROUP BY  A.MIRNO,MIRSEQ";

                        SqlErr = clsDB.GetDataTable(ref dtFc1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        nTotResultOutT = 0;

                        if (dtFc1.Rows.Count > 0)
                        {
                            for (j = 0; j < dtFc1.Rows.Count; j++)
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " SELECT ROWID ";
                                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                                SQL += ComNum.VBLF + " WHERE 1=1";
                                SQL += ComNum.VBLF + "  AND SEQNO ='" + VB.Val(dtFc1.Rows[j]["MIRSEQ"].ToString().Trim()).ToString("00000") + "' ";
                                SQL += ComNum.VBLF + "  AND EDIMIRNO ='" + dtFc1.Rows[j]["MIRNO"].ToString().Trim() + "' ";
                                SQL += ComNum.VBLF + "  AND DEPTCODE1 ='" + dt.Rows[K]["DEPTCODE1"].ToString().Trim() + "' ";

                                if (rdoIO0.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND IPDOPD ='I' ";
                                }
                                if (rdoIO1.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND IPDOPD ='O' ";
                                }

                                if (rdoGB0.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND JOHAP ='1' ";
                                }
                                if (rdoGB1.Checked == true)
                                {
                                    SQL += ComNum.VBLF + " AND JOHAP ='5' ";
                                }

                                SqlErr = clsDB.GetDataTable(ref dtFc2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (dtFc2.Rows.Count > 0)
                                {
                                    nTotResultOutT = nTotResultOutT + VB.Val(dtFc1.Rows[j]["resultoutamt"].ToString().Trim());
                                    ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotResultOutT.ToString("##,###,###,###,##0 ");
                                }
                                dtFc2.Dispose();
                                dtFc2 = null;
                            }
                        }

                        dtFc1.Dispose();
                        dtFc1 = null;

                        nTotResultAmtOut = nTotResultAmtOut + nTotResultOutT;
                    }

                }
                ComFunc.MsgBox("조회 완료", "확인");
                dt.Dispose();
                dt = null;
                #endregion

                //합계
                nRow = nRow + 1;
                if (ssView_Sheet1.RowCount < nRow)
                {
                    ssView_Sheet1.RowCount = nRow;
                }

                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 합계 **";
                ssView_Sheet1.Cells[nRow - 1, 1].Text = nTotCCnt.ToString("###,###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 2].Text = nTotCAmt.ToString("###,###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 3].Text = nTotSakAmt.ToString("###,###,###,###,##0 ");

                if (nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = (nTotSakAmt / nTotCAmt * 100).ToString("##0.00 ");
                }
                ssView_Sheet1.Cells[nRow - 1, 5].Text = nTotReMir.ToString("###,###,###,###,##0 ");

                //'(%)
                if (nTotSakAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = (nTotReMir / nTotSakAmt * 100).ToString("##0.00 ");
                }
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nTotResultAmt.ToString("###,###,###,###,##0 ");

                //'(%)
                if (nTotResultAmt != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = (nTotResultAmt / nTotReMir * 100).ToString("##0.00 ");
                }

                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTotSakAmtOut.ToString("###,###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTotRemirOut.ToString("###,###,###,###,##0 ");

                if (nTotSakAmtOut != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = (nTotRemirOut / nTotSakAmtOut * 100).ToString("##0.00 ");
                }
                ssView_Sheet1.Cells[nRow - 1, 12].Text = nTotResultAmtOut.ToString("###,###,###,###,##0 ");

                //'(%)
                if (nTotRemirOut != 0)
                {
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = (nTotResultAmtOut / nTotRemirOut * 100).ToString("##0.00 ");
                }

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnTing_Click(object sender, EventArgs e)
        {
            frmPmpaReMirBuildSTS frm = new frmPmpaReMirBuildSTS();
            frm.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strJob = "";
            string strIO = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            if (rdoGB2.Checked == true)
            {
                strJob = "조회구분: 전체";
            }
            else if (rdoGB0.Checked == true)
            {
                strJob = "조회구분: 건강보험";
            }
            else
            {
                strJob = "조회구분: 의료급여";
            }

            if (rdoIO0.Checked == true)
            {
                strIO = "(입원)";
            }
            else
            {
                strIO = "(외래)";
            }

            if (rdoG0.Checked == true)
            {
                strTitle = "청구삭감액 이의신청 현황";
            }
            else
            {
                strTitle = "진료월별 이의신청 처리현황";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업 월 : " + cboEDIyyyy0.Text + " ~ " + cboEDIyyyy1.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(strJob + strIO, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdosel_CheckedChanged(object sender, EventArgs e)
        {
            if (rdosel0.Checked == true)
            {
                pansel0.Visible = true;
                pansel1.Visible = false;
            }
            if (rdosel1.Checked == true)
            {
                pansel0.Visible = false;
                pansel1.Visible = true;
            }
        }

        private void rdoG_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoG0.Checked == true)
            {
                lblEDIyyyy.Text = "EDI청구년월";
            }
            else
            {
                lblEDIyyyy.Text = "진료년월";
            }

        }
    }
}
