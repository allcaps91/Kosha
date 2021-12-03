using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaJengSanNEW6.cs
    /// Description     : 연말정산 내역서 2016년 신규개정약식
    /// Author          : 이정현
    /// Create Date     : 2018-08-16
    /// <history> 
    /// 연말정산 내역서 2016년 신규개정약식
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\FrmJengSan_NEW6.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanNEW6 : Form
    {
        private clsPmpaFunc PF = null;
        private clsIument cIM = null;
        clsBasAcct CBA = new clsBasAcct();

        private int GintTimer = 0;
        private string GstrAutoMatic = "N";

        private string GstrPano = "";
        private string GstrName = "";
        private string GstrYear = "";
        private string GstrSname = "";
        private string GstrJumin1 = "";
        private string GstrJumin2 = "";
        private string GstrStartDate = "";
        private string GstrLastDate = "";
        private string GstrZipCode = "";
        private string GstrJiname = "";
        private string GstrJuso = "";
        private string GstrJuso1 = "";
        private string GstrSex = "";
        private string GstrFal = "";
        private string GstrJumin = "";
        private string GstrFDate = "";
        private string GstrTDate = "";
        private string GstrJeaDate = "";
        private string GstrJeaPart = "";

        public frmPmpaJengSanNEW6()
        {
            InitializeComponent();
        }

        public frmPmpaJengSanNEW6(string strAutoMatic)
        {
            InitializeComponent();

            GstrAutoMatic = strAutoMatic;
        }

        private void frmPmpaJengSanNEW6_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            PF = new ComPmpaLibB.clsPmpaFunc();
            cIM = new ComPmpaLibB.clsIument();
            
            PF.Report_Print_2012_Clear();

            txtJumin1.Text = "";
            txtJumin2.Text = "";
            lblPrtinfo.Text = "";
            lblMsg.Visible = false;
            btnPrint2.Enabled = false;

            cboYear.SelectedIndex = 0;

            if (GstrAutoMatic == "Y")
            {
                rdoGubun1.Checked = true;
                panPrint.Visible = true;
                panPrint.Location = new Point(0, 0);
                panPrint.Width = this.Width;
                panPrint.Height = this.Height;
            }
            else
            {
                rdoGubun0.Checked = true;
                panPrint.Visible = false;
            }

            txtName.Text = "";

            Data_Clear();
            btnPrint.Enabled = false;

            //진료비 수납내역서용
            SetCbo();

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpTDate.Value = dtpFDate.Value;

            panDetail.Visible = true;

            if (clsType.User.Sabun != "4349" || GstrAutoMatic == "Y")
            {
                panDetail.Visible = false;
            }

            clsPmpaPb.GstrMirFlag = "";
            ComFunc.ReadSysDate(clsDB.DbCon);

            CBA.Bas_Opd_Bon();              //외래본인부담율
            CBA.Bas_Ipd_Bon();              //입원본인부담율
            CBA.Bas_Joje();                 //내복약조제료 일수
            CBA.Bas_Gisul();                //병원기술료가산
            CBA.Bas_Night();                //심야가산
            CBA.Bas_Night_22();             //중복가산(신생아,소아,노인등)
            CBA.Bas_Gamek();                //감액율(진찰료, 보험, 일반, 보험100%)
            CBA.IPD_BON_SANG();             //본인부담 상한액
            CBA.Bas_PedAdd();               //만6세미만

            Clear();
        }

        private void Data_Clear()
        {
            lblPano.Text = "";
            lblSName.Text = "";
            lblSex.Text = "";
            lblJumin1.Text = "";
            lblJumin2.Text = "";
            lblStartDate.Text = "";
            lblLastDate.Text = "";
            lblJiname.Text = "";
            lblZipCode.Text = "";
            lblJuso.Text = "";
            lblJuso1.Text = "";
            lblTotBoAmt.Text = "";
        }

        private void SetCbo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboDept.Text = "";
            cboDept.Items.Clear();
            cboDept.Items.Add("");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DEPTCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "     WHERE DEPTCODE NOT IN ('HD','ER','II','R6','TO','HR','PT','MD','PD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Clear()
        {
            GstrName = "";
            GstrYear = "";
            GstrSname = "";
            GstrJumin1 = "";
            GstrJumin2 = "";
            GstrStartDate = "";
            GstrLastDate = "";
            GstrZipCode = "";
            GstrJiname = "";
            GstrJuso = "";
            GstrJuso1 = "";
            GstrSex = "";
            GstrFal = "";
            GstrJumin = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            int k = 0;
            int p = 0;

            string strTempDept = "";
            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            int intCnt1 = 0;
            int intCnt2 = 0;

            double dblSeqNo = 0;
            double dblBoamt2 = 0;
            double dblBigub = 0;
            double dblMisuAmt = 0;
            double dblBoninAmt22 = 0;
            double dblBoninAmti22 = 0;
            double dblBoninAmtiB22 = 0;
            double dblIPDNO = 0;
            double dblTRSNo = 0;
            double dblChaAmt = 0; //차액
            double dblEtcAmt = 0; //기타
            double dblBiAmt = 0;
            double dblBoAmt = 0;
            double dblGamAmt = 0;
            double dblTotBiAmt = 0;
            double dblTotBoAmt = 0;
            double dblCardTot = 0;
            double dblCashTot = 0;

            string strRemark = "";
            string strInDate = "";
            string strOutDate = "";
            string strBun = "";
            string strBi = "";
            string strSEQNO = "";
            string strBDate = "";
            string strBi2 = "";
            string strActDate2 = "";
            string strBDate2 = "";
            string strDept2 = "";
            string strDept3 = "";

            GstrYear = VB.Left(cboYear.Text.Trim(), 4);
            GstrName = txtName.Text.Trim();
            GstrFal = "0";

            ssView_Sheet1.RowCount = 0;

            if (VB.Val(GstrYear) >= 2009)
            {
                GstrFDate = GstrYear + "-01-01";
                GstrTDate = GstrYear + "-12-31";
            }
            else if (VB.Val(GstrYear) >= 2008)
            {
                //2008년도 작년12월부터 올해 12월까지 변경 되었습니다.
                GstrFDate = (VB.Val(GstrYear) - 1).ToString() + "-12-01";
                GstrTDate = GstrYear + "-12-31";
            }
            else if (VB.Val(GstrYear) >= 2007)
            {
                GstrFDate = (VB.Val(GstrYear) - 1).ToString() + "-12-01";
                GstrTDate = GstrYear + "-11-30";
            }
            else if (VB.Val(GstrYear) >= 2006)
            {
                GstrFDate = GstrYear + "-01-01";
                GstrTDate = GstrYear + "-11-30";
            }
            else
            {
                GstrFDate = GstrYear + "-01-01";
                GstrTDate = GstrYear + "-12-31";
            }

            GstrFDate = GstrFDate.Trim();
            GstrTDate = GstrTDate.Trim();

            if (txtName.Text == "")
            {
                ComFunc.MsgBox("수신자명 또는 등록번호가 없습니다.");
                return;
            }

            Data_Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (rdoGubun0.Checked == true)
                {
                    GstrFal = "1";

                    clsPmpaPb.GstrFal = "1";
                    clsPmpaPb.GstrName = txtName.Text;  

                    frmPmpaViewSnameView frm = new frmPmpaViewSnameView();
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog();

                    GstrPano = clsPmpaPb.GstrPANO;
                    GstrName = clsPmpaPb.GstrName;
                    GstrYear = clsPmpaPb.GstrYear;
                    GstrSname = clsPmpaPb.GstrSname;
                    GstrJumin1 = clsPmpaPb.GstrJumin1;
                    GstrJumin2 = clsPmpaPb.GstrJumin2;
                    GstrStartDate = clsPmpaPb.GstrStartDate;
                    GstrLastDate = clsPmpaPb.GstrLastDate;
                    GstrZipCode = clsPmpaPb.GstrZipCode;
                    GstrJiname = clsPmpaPb.GstrJiname;
                    GstrJuso = clsPmpaPb.GstrJuso;
                    GstrJuso1 = clsPmpaPb.GstrJuso;
                    GstrSex = clsPmpaPb.GstrSex;
                    GstrFal = clsPmpaPb.GstrFal;
                    GstrJumin = clsPmpaPb.GstrJumin;

                    lblPano.Text = GstrPano;
                    lblSName.Text = GstrSname;
                    lblSex.Text = GstrSex;
                    lblJumin1.Text = GstrJumin1;
                    lblJumin2.Text = GstrJumin2;
                    lblStartDate.Text = GstrStartDate;
                    lblLastDate.Text = GstrLastDate;
                    lblJiname.Text = GstrJiname;
                    lblZipCode.Text = GstrZipCode;
                    lblJuso1.Text = GstrJuso1;
                    lblJuso.Text = GstrJuso;
                }
                else
                {
                    txtName.Text = ComFunc.LPAD(txtName.Text.Trim(), 8, "0");
                    GstrPano = txtName.Text.Trim();

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PANO";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + GstrPano + "' ";

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
                        ComFunc.MsgBox("해당 등록번호가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                    #region //JengSanView00

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     Pano, Sname, Sex, Jumin1, Jumin2, Jumin3,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(StartDate,'YYYY-MM-DD') AS StartDate,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(LastDate,'YYYY-MM-DD') AS LastDate, JiName, P.ZipCode1, ";
                    SQL = SQL + ComNum.VBLF + "     P.ZipCode2, ZipName1, ZipName2, ZipName3, Juso, Tel";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_AREA A, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_ZIPSNEW Z";
                    SQL = SQL + ComNum.VBLF + "     WHERE P.JiCode = A.JiCode(+)";
                    SQL = SQL + ComNum.VBLF + "         AND P.ZipCode1 = Z.ZipCode1(+)";
                    SQL = SQL + ComNum.VBLF + "         AND P.ZipCode2 = Z.ZipCode2(+)";

                    if (GstrFal == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND Sname LIKE '" + GstrName + "%' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND Pano = '" + GstrPano + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "GROUP BY Pano, Sname, Sex, Jumin1, Jumin2,jumin3, startdate, Lastdate, ";
                    SQL = SQL + ComNum.VBLF + "     JiName, P.ZipCode1, P.ZipCode2, ZipName1, ZipName2, ZipName3, Juso, Tel ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY P.JUMIN1, Sname, Pano  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    #endregion //JengSanView00

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        lblPano.Text = dt.Rows[0]["Pano"].ToString().Trim();
                        lblSName.Text = dt.Rows[0]["Sname"].ToString().Trim();
                        lblSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                        lblJumin1.Text = dt.Rows[0]["Jumin1"].ToString().Trim();
                        lblJumin2.Text = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                        GstrJumin1 = lblJumin1.Text;
                        GstrJumin2 = lblJumin2.Text;
                        lblStartDate.Text = dt.Rows[0]["StartDate"].ToString().Trim();
                        lblLastDate.Text = dt.Rows[0]["LastDate"].ToString().Trim();
                        lblJiname.Text = dt.Rows[0]["JiName"].ToString().Trim();
                        lblZipCode.Text = dt.Rows[0]["ZipCode1"].ToString().Trim() + "-" + dt.Rows[0]["ZipCode2"].ToString().Trim();
                        lblJuso.Text = dt.Rows[0]["Juso"].ToString().Trim();
                        lblJuso1.Text = dt.Rows[0]["ZipName1"].ToString().Trim() + " " + dt.Rows[0]["ZipName2"].ToString().Trim() + " " + dt.Rows[0]["ZipName3"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                strTempDept = "";

                if (chkJenSang.Checked == false)
                {
                    GstrFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
                    GstrTDate = dtpTDate.Value.ToString("yyyy-MM-dd");
                    strTempDept = cboDept.Text.Trim();
                }

                #region //Creat View
                SQL = "";
                SQL = "CREATE OR REPLACE VIEW VIEW_JENGSAN ";
                SQL = SQL + ComNum.VBLF + "     (SDATE, INDATE, OUTDATE, GUBUN, DEPTCODE, BI, BIAMT, BOAMT, BOAMT2, BiGub, REMARK, BUN, IPDNO) AS ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     ACTDATE, '', '', '1', DEPTCODE, BI, SUM(Y98), SUM(Y99), 0, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE,'1', DEPTCODE, BI, ";
                SQL = SQL + ComNum.VBLF + "         CASE WHEN SUNEXT = 'Y98' THEN SUM(AMT1 + AMT2) END AS Y98, ";
                SQL = SQL + ComNum.VBLF + "         CASE WHEN SUNEXT = 'Y99' THEN SUM(AMT1 + AMT2) END AS Y99 ";
                SQL = SQL + ComNum.VBLF + "     From " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + "         WHERE ACTDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND SUNEXT IN ('Y98', 'Y99') ";
                SQL = SQL + ComNum.VBLF + "             AND Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "             AND SEQNO <> -1 ";  //예약자 제외
                SQL = SQL + ComNum.VBLF + "             AND (GBPOSCO <> 'N' OR GBPOSCO IS NULL) ";
                SQL = SQL + ComNum.VBLF + "             AND PART <> '#' ";
                SQL = SQL + ComNum.VBLF + "             AND TRIM(SUNEXT) NOT IN";
                SQL = SQL + ComNum.VBLF + "                         (SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "                             WHERE GUBUN = '원무영수제외코드') ";  //저가약제 제외코드 2010-11-22

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "             AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, DEPTCODE, BI, SUNEXT) X ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ACTDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "HAVING (SUM(Y98) <> 0 OR SUM(Y99) <> 0) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     ACTDATE, '', '','1', DEPTCODE, BI, SUM(Y98), SUM(Y99), 0, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, '1', DEPTCODE, BI, 0 AS Y98, SUM(AMT1 + AMT2) AS Y99 ";
                SQL = SQL + ComNum.VBLF + "     From " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + "         WHERE ACTDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";

                if (clsVbfunc.READ_Gamek_infoSabun(GstrJumin1 + GstrJumin2) != "")
                {
                    SQL = SQL + ComNum.VBLF + "             AND sunext in ('Y92I','Y92J','Y92K','Y92L','Y92M','Y92A','Y92B','Y92C','Y92D') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "             AND 1 = 2 ";
                }

                SQL = SQL + ComNum.VBLF + "             AND Pano = '" + GstrPano + "' ";
                //SQL = SQL + ComNum.VBLF + "             AND SEQNO <> -1 ";  //예약자 제외
                SQL = SQL + ComNum.VBLF + "             AND (GBPOSCO <> 'N' OR GBPOSCO IS NULL) ";
                //SQL = SQL + ComNum.VBLF + "             AND PART  <> '#' ";
                SQL = SQL + ComNum.VBLF + "             AND TRIM(SUNEXT) NOT IN";
                SQL = SQL + ComNum.VBLF + "                         (SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "                             WHERE GUBUN = '원무영수제외코드') ";  //저가약제 제외코드 2010-11-22

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "             AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, DEPTCODE, BI, SUNEXT) X ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ACTDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "HAVING (SUM(Y98) <> 0 OR SUM(Y99) <> 0) ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //예약금
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(DATE1,'YYYY-MM-DD') AS ACTDATE, '', '', '1', ";
                SQL = SQL + ComNum.VBLF + "     'R+', BI, SUM(AMT4) AS AMT4, SUM(AMT7 - amt2) AS AMT7, SUM(amt2) AS amt2, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND Date1 >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DATE1 <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY DATE1, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //예약금
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(DATE1,'YYYY-MM-DD') AS ACTDATE, '', '', '1', ";
                SQL = SQL + ComNum.VBLF + "     'R+', BI, 0 AS AMT4, SUM(AMT5) AS AMT7, 0, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND Date1 >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DATE1 <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                }

                if (clsVbfunc.READ_Gamek_infoSabun(GstrJumin1 + GstrJumin2) != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND gbgamek in ('21','22','23','24','25' ,'11','12','13','14') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND 1 = 2 ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY DATE1, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //예약금 환불
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RETDATE,'YYYY-MM-DD') AS ACTDATE, '', '', '1', ";
                SQL = SQL + ComNum.VBLF + "     'R-', BI, SUM(AMT4) * -1 AS AMT4, SUM(RETAMT + amt2) AS AMT7, SUM(AMT2) * -1 AS AMT2, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE < TO_DATE('" + Convert.ToDateTime(GstrTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NOT NULL ";

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY RETDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //예약금 환불
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RETDATE,'YYYY-MM-DD') AS ACTDATE, '', '', '1', ";
                SQL = SQL + ComNum.VBLF + "     'R-', BI, 0 AS AMT4, SUM(AMT5) * -1 AS AMT7, 0 AS AMT2, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE < TO_DATE('" + Convert.ToDateTime(GstrTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NOT NULL ";

                if (clsVbfunc.READ_Gamek_infoSabun(GstrJumin1 + GstrJumin2) != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND gbgamek in ('21','22','23','24','25' ,'11','12','13','14') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND 1 = 2 ";
                }

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY RETDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //-------------------------------------------------------------------------------------------------------------
                //예약검사 2011시행
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ActDate,'YYYY-MM-DD') AS ACTDATE, '', '', '1', ";
                SQL = SQL + ComNum.VBLF + "     'RE+', BI, 0, 0, SUM(AMT6) AS AMT7, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND ActDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ActDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY ActDate, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //예약검사 환불 2011시행
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RETDATE,'YYYY-MM-DD') AS ACTDATE, '', '', '1', ";
                SQL = SQL + ComNum.VBLF + "     'RE-', BI, 0 , 0, SUM(RETAMT) AS AMT7, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE < TO_DATE('" + Convert.ToDateTime(GstrTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NOT NULL ";

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY RETDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //예약검사 대체 2011시행
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(TransDATE,'YYYY-MM-DD') AS ACTDATE, '', '', '1', ";
                SQL = SQL + ComNum.VBLF + "     'RE-', BI, 0 , 0, SUM(TransAMT) * -1 AS AMT7, 0, '', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND TransDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND TransDATE < TO_DATE('" + Convert.ToDateTime(GstrTDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND TransDate IS NOT NULL ";

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY TransDATE, DEPTCODE, BI ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //------------------------------------------------------------------------------------------------------------

                //당일 접수한것
                if (Convert.ToDateTime(GstrTDate) >= Convert.ToDateTime(strSysDate))
                {
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, '', '', '1', 'JUP', BI, AMT4, AMT7 - amt2, AMT2, 0, '', '', 0 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Master ";
                    SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND ActDate = TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";

                    if (strTempDept != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(DATE1,'YYYY-MM-DD') AS ACTDATE, '', '', '1', 'JUPR', BI, AMT4, AMT7 - amt2, amt2, 0, '', '', 0 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Reserved_NEW ";
                    SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND Date1 = TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NULL ";

                    if (strTempDept != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strTempDept + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                }

                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     CASE WHEN (A.ACTDATE IS NULL OR TO_CHAR(A.ACTDATE,'YYYY-MM-DD') > '" + GstrTDate + "') THEN '" + GstrTDate + "' ";
                SQL = SQL + ComNum.VBLF + "     else TO_CHAR(A.ACTDATE,'YYYY-MM-DD') END AS ACTDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.INDATE,'YYYY-MM-DD') AS INDATE, ";
                SQL = SQL + ComNum.VBLF + "     CASE WHEN TO_CHAR(A.OUTDATE,'YYYY-MM-DD') > '" + GstrTDate + "' THEN '" + GstrTDate + "' ";
                SQL = SQL + ComNum.VBLF + "     else TO_CHAR(A.OUTDATE,'YYYY-MM-DD') END AS OUTDATE, '2', ";
                SQL = SQL + ComNum.VBLF + "     A.DEPTCODE, A.BI, SUM(X.BIAMT), SUM(X.BOAMT), SUM(X.BOAMT2), 0, TO_CHAR(A.ILSU,'000'), '', a.IPDNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "IPD_TRANS B, ";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         IPDNO, TRSNO, ";
                SQL = SQL + ComNum.VBLF + "         SUM(CASE WHEN BUN = '98' THEN AMT END) AS BIAMT, ";

                if (clsVbfunc.READ_Gamek_infoSabun(GstrJumin1 + GstrJumin2) != "")
                {
                    SQL = SQL + ComNum.VBLF + "         SUM(CASE WHEN BUN = '85' OR BUN = '87' OR BUN = '89' or sunext = 'Y92I' OR sunext = 'Y92J' OR sunext = 'Y92K' or sunext = 'Y92L' or sunext = 'Y92M' or sunext = 'Y92A' or sunext = 'Y92B' or sunext = 'Y92C' or sunext = 'Y92D'THEN AMT END) BOAMT, ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         SUM(CASE WHEN BUN = '85' OR BUN = '87' OR BUN = '89' THEN AMT END) BOAMT, ";
                }

                SQL = SQL + ComNum.VBLF + "         SUM(CASE WHEN BUN = '91' THEN AMT END) AS BOAMT2 ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                SQL = SQL + ComNum.VBLF + "         WHERE ACTDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "             AND PANO = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "             AND PART <> '#' ";
                SQL = SQL + ComNum.VBLF + "             AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //저가약제 제외코드 2010-11-22
                SQL = SQL + ComNum.VBLF + "     GROUP BY BUN, IPDNO, TRSNO ";
                SQL = SQL + ComNum.VBLF + "     HAVING SUM(CASE WHEN BUN = '98' THEN AMT END) <> 0 OR ";

                if (clsVbfunc.READ_Gamek_infoSabun(GstrJumin1 + GstrJumin2) != "")
                {
                    SQL = SQL + ComNum.VBLF + "     SUM(CASE WHEN BUN = '85' OR BUN = '87' OR BUN = '89' or sunext = 'Y92I' OR sunext = 'Y92J' OR sunext = 'Y92K' or sunext = 'Y92L' or sunext = 'Y92M'  or sunext = 'Y92A' or sunext = 'Y92B' or sunext = 'Y92C' or sunext = 'Y92D' THEN AMT END) <> 0 OR ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     SUM(CASE WHEN BUN = '85' OR BUN = '87' OR BUN = '89'  THEN AMT END) <> 0 OR ";
                }

                SQL = SQL + ComNum.VBLF + "     SUM(CASE WHEN BUN = '91' THEN AMT END) <> 0) X ";
                SQL = SQL + ComNum.VBLF + "WHERE A.IPDNO = X.IPDNO ";
                SQL = SQL + ComNum.VBLF + "     AND A.PANO = '" + GstrPano + "' ";
                SQL = SQL + ComNum.VBLF + "     AND B.TRSNO = X.TRSNO ";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.ACTDATE, A.INDATE, A.OUTDATE, A.DEPTCODE, A.BI, A.ILSU ,a.IPDNO ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";

                //종합건진
                if (strTempDept == "" || strTempDept == "TO")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.SUDATE,'YYYY-MM-DD') AS SDATE, '', '', '1', 'TO', '51', ";

                    //이 환자만함. 일반건진에서 요청 이창우
                    if (GstrPano == "06709988")
                    {
                        SQL = SQL + ComNum.VBLF + "     0, SUM(A.TOTAMT) AS AMT, 0, 0, '종합건진', '', 0 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     0, SUM(A.BONINAMT-A.HALINAMT) AS AMT, 0, 0, '종합건진', '', 0 ";
                    }

                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HEA_SUNAP A, " + ComNum.DB_PMPA + "HIC_PATIENT B, " + ComNum.DB_PMPA + "HEA_JEPSU C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.SUDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";

                    if (GstrPano != "06709988")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.BONINAMT <> '0' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "         AND A.WRTNO = C.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND A.WRTNO > 0 ";
                    SQL = SQL + ComNum.VBLF + "         AND C.GBSTS NOT IN ('0')  ";
                    SQL = SQL + ComNum.VBLF + "         AND B.Ptno = '" + GstrPano + "' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUDATE ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                }

                //일반건진
                if (strTempDept == "" || strTempDept == "HR")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.SUDATE,'YYYY-MM-DD') AS SDATE, '', '', '1', 'TO', '51', ";
                    SQL = SQL + ComNum.VBLF + "     0, SUM(A.BONINAMT) AS AMT, 0, 0, '일반건진', '', 0 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HIC_SUNAP A, " + ComNum.DB_PMPA + "HIC_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.SUDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.BONINAMT <> 0 ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "         AND B.Ptno = '" + GstrPano + "' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUDATE ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                }

                //기타수납
                if (strTempDept == "")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(ACTDATE,'YYYY-MM-DD') AS SDATE, '', '', '1', 'R7', '51', ";
                    SQL = SQL + ComNum.VBLF + "     0, SUM(AMT) AS AMT, 0, 0, '기타수납', '', 0 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND JUMIN1||JUMIN2 = '" + GstrJumin1 + GstrJumin2 + "' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY ACTDATE ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL ";
                }

                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS SDATE, '', '', DECODE(GUBUN2,'1','1','2') AS GUBUN, '', '51', ";
                SQL = SQL + ComNum.VBLF + "     0, AMT, 0, 0, '미수입금', '', 0 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND BDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND GUBUN1 = '2' ";
                SQL = SQL + ComNum.VBLF + "         AND (POBUN IS NULL  OR POBUN = '') ";  //포스코 위탁검사는 제외함. 업무의뢰서 요청
                SQL = SQL + ComNum.VBLF + "         AND PANO  = '" + GstrPano + "' ";

                if (strTempDept != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND SubStr(MisuDtl,2,2) = '" + strTempDept + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #endregion //Creat View

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 1;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SDATE, INDATE, OUTDATE, GUBUN, BI,SUM(BIAMT) BIAMT, SUM(BOAMT) BOAMT, SUM(BOAMT2) BOAMT2,SUM(BiGub) BiGub,REMARK, BUN ";
                SQL = SQL + ComNum.VBLF + "FROM VIEW_JENGSAN ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SDATE, INDATE, OUTDATE,GUBUN, BI,REMARK, BUN ";
                SQL = SQL + ComNum.VBLF + "ORDER BY GUBUN, SDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        dblEtcAmt = 0;
                        strRemark = "";
                        dblBiAmt = 0;
                        dblGamAmt = 0;
                        dblMisuAmt = 0;
                        dblBoAmt = 0;
                        dblBigub = 0;
                        dblBoninAmt22 = 0;
                        dblBoninAmtiB22 = 0;
                        dblBoninAmti22 = 0;

                        dblBiAmt = VB.Val(dt.Rows[i]["BIAMT"].ToString().Trim());

                        if (dt.Rows[i]["GUBUN"].ToString().Trim() == "1")
                        {
                        //dblBoAmt = VB.Val((VB.Val(dt.Rows[i]["BoAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["BoAmt2"].ToString().Trim())).ToString("###########0")) / 10 * 10;
                            dblBoAmt = Math.Round((VB.Val(dt.Rows[i]["BoAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["BoAmt2"].ToString().Trim())) / 10) * 10;
                        }
                        else
                        {
                            //입원 환불급 -해줌
                            //dblBoAmt = VB.Val((VB.Val(dt.Rows[i]["BoAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["BoAmt2"].ToString().Trim())).ToString("###########0")) / 10 * 10;
                            dblBoAmt = Math.Round((VB.Val(dt.Rows[i]["BoAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["BoAmt2"].ToString().Trim())) / 10) * 10; 
                        }

                        strRemark = dt.Rows[i]["REMARK"].ToString().Trim();
                        strBun = dt.Rows[i]["BUN"].ToString().Trim();
                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        strBDate = dt.Rows[i]["SDATE"].ToString().Trim();
                        strInDate = dt.Rows[i]["INDATE"].ToString().Trim();
                        strOutDate = dt.Rows[i]["OUTDATE"].ToString().Trim();

                        if (dblBoAmt != 0)
                        {
                            dblGamAmt = 0;

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                            #region //외래
                            if (dt.Rows[i]["GUBUN"].ToString().Trim() == "1")
                            {
                                dblBoninAmt22 = 0;
                                dblBigub = 0;

                                //일자별 개별건수
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     GUBUN, DeptCode, BI, SUM(BIAMT) AS BIAMT, ";
                                SQL = SQL + ComNum.VBLF + "     SUM(BOAMT) AS BOAMT, SUM(BOAMT2) AS BOAMT2, SUM(BiGub) BiGub ";
                                SQL = SQL + ComNum.VBLF + "FROM VIEW_JENGSAN ";
                                SQL = SQL + ComNum.VBLF + "     WHERE SDATE = '" + strBDate + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND Bi ='" + strBi + "'";
                                SQL = SQL + ComNum.VBLF + "         AND InDate IS NULL ";
                                SQL = SQL + ComNum.VBLF + "GROUP BY GUBUN, BI, DeptCode ";
                                SQL = SQL + ComNum.VBLF + "ORDER BY GUBUN, Bi, DeptCode ";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    for (k = 0; k < dt1.Rows.Count; k++)
                                    {
                                        strDept2 = dt1.Rows[k]["DEPTCODE"].ToString().Trim();

                                        switch (strDept2)
                                        {
                                            case "R+":
                                            case "R-":
                                            case "RE+":
                                            case "RE-":
                                            case "JUP":
                                                dblBigub = dblBigub + VB.Val(VB.Val(dt1.Rows[k]["BOAMT2"].ToString().Trim()).ToString("###########0")) / 10 * 10;
                                                dblBoninAmt22 = dblBoninAmt22 + VB.Val(VB.Val(dt1.Rows[k]["BoAmt"].ToString().Trim()).ToString("###########0")) / 10 * 10;
                                                break;
                                            default:
                                                SQL = "";
                                                SQL = "SELECT";
                                                SQL = SQL + ComNum.VBLF + "     Pano, DeptCode, Seqno, Bi, Part, TO_CHAR(ACTDATE,'YYYY-MM-DD') AS ACTDATE, TO_CHAR(bDATE,'YYYY-MM-DD') AS bDATE  ";
                                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                                                SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + GstrPano + "' ";
                                                SQL = SQL + ComNum.VBLF + "         AND Bi = '" + strBi + "'";
                                                SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strDept2 + "' ";
                                                SQL = SQL + ComNum.VBLF + "         AND BUN = '99' ";
                                               // SQL = SQL + ComNum.VBLF + "         AND Seqno <> -1 ";
                                               // SQL = SQL + ComNum.VBLF + "         AND Part <> '#' ";
                                                SQL = SQL + ComNum.VBLF + "GROUP BY Pano, DeptCode, Seqno, Bi, Part, TO_CHAR(ACTDATE,'YYYY-MM-DD'), TO_CHAR(bDATE,'YYYY-MM-DD')   ";

                                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    return;
                                                }
                                                if (dt2.Rows.Count > 0)
                                                {
                                                    for (p = 0; p < dt2.Rows.Count; p++)
                                                    {
                                                        dblSeqNo = VB.Val(dt2.Rows[p]["SEQNO"].ToString().Trim());
                                                        strSEQNO = strSEQNO + dblSeqNo + ",";
                                                        strBi2 = dt2.Rows[p]["BI"].ToString().Trim();
                                                        strActDate2 = dt2.Rows[p]["ACTDATE"].ToString().Trim();
                                                        clsPmpaPb.GstrJeaDate = strActDate2;
                                                        strBDate2 = dt2.Rows[p]["BDATE"].ToString().Trim();
                                                        strDept3 = dt2.Rows[p]["DEPTCODE"].ToString().Trim();
                                                        clsPmpaPb.GstrJeaPart = dt2.Rows[p]["PART"].ToString().Trim();

                                                        clsPmpaType.RPG.Amt1[36] = 0;
                                                        clsPmpaType.RPG.Amt1[38] = 0;
                                                        clsPmpaType.RPG.Amt1[40] = 0;

                                                        PF.Report_Print_Sunap_2012_Gesan(clsDB.DbCon, GstrPano, "과", "이름", "", (int)dblSeqNo, "", "", strBi2, strBDate2, "", "", strDept3, "", "", "", "", "", "", "");

                                                        dblBoninAmt22 += clsPmpaType.RPG.Amt1[36];

                                                        clsPmpaType.RPG.Amt1[34] = clsPmpaType.RPG.Amt1[34]; //감액
                                                        clsPmpaType.RPG.Amt1[35] = clsPmpaType.RPG.Amt1[35]; //미수

                                                        clsPmpaType.RPG.Amt1[33] = clsPmpaType.RPG.Amt1[33]; //본인합
                                                        clsPmpaType.RPG.Amt1[36] = clsPmpaType.RPG.Amt1[36]; //급여 본인합
                                                        clsPmpaType.RPG.Amt1[37] = clsPmpaType.RPG.Amt1[37]; //급여 공단합
                                                        clsPmpaType.RPG.Amt1[38] = clsPmpaType.RPG.Amt1[38]; //전액부담
                                                        clsPmpaType.RPG.Amt1[39] = clsPmpaType.RPG.Amt1[39]; //선택진료합
                                                        clsPmpaType.RPG.Amt1[40] = clsPmpaType.RPG.Amt1[40]; //비급여합

                                                        dblGamAmt += clsPmpaType.RPG.Amt1[34];
                                                        dblEtcAmt += dblGamAmt;

                                                        dblBigub += clsPmpaType.RPG.Amt1[40] + clsPmpaType.RPG.Amt1[38] + clsPmpaType.RPG.Amt1[39];     //비급여
                                                    }
                                                }

                                                dt2.Dispose();
                                                dt2 = null;
                                                break;
                                        }
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;

                                if (VB.Val(strBi2) <= 13)
                                {
                                    dblBoninAmt22 = (VB.Fix((int)dblBoninAmt22 / 100) * 100);
                                }
                                else
                                {
                                    dblBoninAmt22 = (VB.Fix((int)dblBoninAmt22 / 10) * 10);
                                }

                                dblBigub = VB.Fix((int)dblBigub / 10) * 10;
                            }
                        #endregion //외래
                            #region //입원
                            else if (dt.Rows[i]["GUBUN"].ToString().Trim() == "2")
                            {
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     IPDNO ";
                                SQL = SQL + ComNum.VBLF + "FROM VIEW_JENGSAN ";
                                SQL = SQL + ComNum.VBLF + "     WHERE SDATE = '" + strBDate + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '2'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    for (k = 0; k < dt1.Rows.Count; k++)
                                    {
                                        dblIPDNO = VB.Val(dt1.Rows[0]["IPDNO"].ToString().Trim());

                                        SQL = "";
                                        SQL = "SELECT";
                                        SQL = SQL + ComNum.VBLF + "     Trsno, Amt51 ";
                                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                                        SQL = SQL + ComNum.VBLF + "     WHERE InDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";

                                        if (strOutDate != "")
                                        {
                                            SQL = SQL + ComNum.VBLF + "         AND OutDate <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                                        }

                                        SQL = SQL + ComNum.VBLF + "         AND IPDNO = " + dblIPDNO;
                                        SQL = SQL + ComNum.VBLF + "         AND GbIPD <> 'D' ";

                                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            Cursor.Current = Cursors.Default;
                                            return;
                                        }
                                        if (dt2.Rows.Count > 0)
                                        {
                                            for (p = 0; p < dt2.Rows.Count; p++)
                                            {
                                                dblTRSNo = VB.Val(dt2.Rows[p]["TRSNO"].ToString().Trim());

                                                cIM.Ipd_Trans_PrtAmt_Read(clsDB.DbCon, (long)dblTRSNo, "");
                                                cIM.Ipd_Tewon_PrtAmt_Gesan(clsDB.DbCon, GstrPano, (long)dblIPDNO, (long)dblTRSNo, "", "");

                                                if (VB.Val(dt2.Rows[p]["TRSNO"].ToString().Trim()) == 1399306)
                                                {
                                                    clsPmpaType.RPG.Amt5[50] = 804203;
                                                    clsPmpaType.RPG.Amt4[50] = 31172;
                                                }
                                                dblBoninAmti22 += clsPmpaType.RPG.Amt5[50];

                                                //dblBoninAmti22 = (Math.Round(dblBoninAmti22 * 0.1) * 10);  //절사금액??
                                              
                                                dblBoninAmtiB22 += clsPmpaType.RPG.Amt2[50] + clsPmpaType.RPG.Amt3[50] + clsPmpaType.RPG.Amt4[50];

                                                dblGamAmt += clsPmpaType.TIT.Amt[54];
                                                dblEtcAmt += dblGamAmt;
                                            }
                                        }

                                        dt2.Dispose();
                                        dt2 = null;
                                    }

                                    SQL = "";
                                    SQL = "SELECT";
                                    SQL = SQL + ComNum.VBLF + "     SUM(AMT) AS AMT ";
                                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                                    SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";

                                    if (strOutDate != "")
                                    {
                                        SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                                    }

                                    SQL = SQL + ComNum.VBLF + "         AND IPDNO = " + dblIPDNO + " ";
                                    SQL = SQL + ComNum.VBLF + "         AND Part = '#' ";

                                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                    if (dt2.Rows.Count > 0)
                                    {
                                    //보증금 급여부분에 -처리
                                        dblBoninAmti22 -= VB.Val(dt2.Rows[0]["AMT"].ToString().Trim());
                                    }

                                    dt2.Dispose();
                                    dt2 = null;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                            #endregion //입원

                            ComFunc CF = new ComFunc();
                            switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                            {
                                case "1":
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "외래진료비";
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = strRemark;
                                    break;
                                case "2":
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "퇴원진료비";
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["INDATE"].ToString().Trim();

                                    if (dblBoninAmti22 + dblBoninAmtiB22 == 0)
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "";
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (CF.DATE_ILSU(clsDB.DbCon, strSysDate, dt.Rows[i]["INDATE"].ToString().Trim()) + 1).ToString();
                                        if (strRemark == "미수입금") { ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = strRemark; }
                                    }
                                    else
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                                        if (dt.Rows[i]["OUTDATE"].ToString().Trim() == "")
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (CF.DATE_ILSU(clsDB.DbCon, strSysDate, dt.Rows[i]["INDATE"].ToString().Trim()) + 1).ToString();
                                        }
                                        else
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (CF.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["OUTDATE"].ToString().Trim(), dt.Rows[i]["INDATE"].ToString().Trim()) + 1).ToString();
                                        }
                                    }
                                    break;
                                case "3":
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "예약검사비";
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = strRemark;
                                    break;
                            }
                            CF = null;

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["BI"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dblBiAmt.ToString("###,###,###,##0");

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = dblGamAmt.ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = dblMisuAmt.ToString("###,###,###,##0");

                            if (dt.Rows[i]["GUBUN"].ToString().Trim() == "1")
                            {
                                dblChaAmt = 0;

                                if (dblEtcAmt == 0)
                                {
                                    dblChaAmt = dblBoAmt - (dblBoninAmt22 + dblBigub);
                                }

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dblBoAmt.ToString("###,###,###,##0");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dblBigub.ToString("###,###,###,##0");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = (dblBoninAmt22 + dblChaAmt).ToString("###,###,###,##0");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = (dblBiAmt + dblBoAmt).ToString("###,###,###,##0");
                            }
                            else
                            {
                                dblChaAmt = 0;

                                if (dblEtcAmt == 0)
                                {
                                    dblChaAmt = (dblBoAmt + dblBoamt2) - (dblBoninAmtiB22 + dblBoninAmti22);
                                }

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (dblBoAmt + dblBoamt2).ToString("###,###,###,##0");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dblBoninAmtiB22.ToString("###,###,###,##0");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = dblBoninAmti22.ToString("###,###,###,##0");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = (dblBoAmt + dblBoamt2 + dblBiAmt).ToString("###,###,###,##0");
                            }

                            dblTotBiAmt = dblTotBiAmt + dblBiAmt;
                            dblTotBoAmt = dblTotBoAmt + dblBoAmt;

                            if (ssView_Sheet1.RowCount - 1 != i)
                            {
                                if (intCnt1 == 1 && dt.Rows[i + 1]["GUBUN"].ToString().Trim() == "2")
                                {
                                    intCnt1 = i;
                                }
                            }

                            if (ssView_Sheet1.RowCount - 1 == i)
                            {
                                intCnt2 = i;
                            }

                            //카드및 현금영수증
                            switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                            {
                                case "1":
                                    SQL = "";
                                    SQL = "SELECT";
                                    SQL = SQL + ComNum.VBLF + "     X.GUBUN, SUM(X.AMT) AS AMT";
                                    SQL = SQL + ComNum.VBLF + "FROM";
                                    SQL = SQL + ComNum.VBLF + "     (SELECT";
                                    SQL = SQL + ComNum.VBLF + "         SUM(CASE WHEN  TRANHEADER = '1' THEN 1 else -1 END * TRADEAMT) AS AMT, GUBUN";
                                    SQL = SQL + ComNum.VBLF + "     From " + ComNum.DB_PMPA + "CARD_APPROV ";
                                    SQL = SQL + ComNum.VBLF + "         WHERE PANO = '" + lblPano.Text + "' ";
                                    SQL = SQL + ComNum.VBLF + "             AND GBIO  = 'O' ";
                                    SQL = SQL + ComNum.VBLF + "             AND (CARDNO IS NULL OR CARDNO <> '0100001234') ";

                                    if (ssView_Sheet1.RowCount == 1)
                                    {
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDATE <= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                    }
                                    else if (intCnt1 == i)
                                    {
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate >= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                                    }
                                    else
                                    {
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate > TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                    }

                                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, GUBUN, GBIO ";
                                    SQL = SQL + ComNum.VBLF + "     UNION ALL ";
                                    SQL = SQL + ComNum.VBLF + "     SELECT";
                                    SQL = SQL + ComNum.VBLF + "         SUM(CASE WHEN  TRANHEADER = '1' THEN 1 else -1 END * TRADEAMT) AS AMT, GUBUN ";
                                    SQL = SQL + ComNum.VBLF + "     From " + ComNum.DB_PMPA + "CARD_APPROV_CENTER ";
                                    SQL = SQL + ComNum.VBLF + "         WHERE PANO  = '" + lblPano.Text.Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "             AND (CARDNO IS NULL OR CARDNO <> '0100001234') ";

                                    if (ssView_Sheet1.RowCount == 1)
                                    {
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDATE <= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                    }
                                    else if (intCnt1 == i)
                                    {
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate >= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                                    }
                                    else
                                    {
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate > TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                        SQL = SQL + ComNum.VBLF + "             AND ACTDate <= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text.Trim() + "','YYYY-MM-DD') ";
                                    }

                                    SQL = SQL + ComNum.VBLF + "     GROUP BY ACTDATE, GUBUN ) X ";
                                    SQL = SQL + ComNum.VBLF + "GROUP BY GUBUN ";

                                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            if (dt1.Rows[k]["GUBUN"].ToString().Trim() == "1" || dt1.Rows[k]["GUBUN"].ToString().Trim() == "3")
                                            {
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = VB.Val(dt1.Rows[k]["AMT"].ToString().Trim()).ToString("###,###,###,###");
                                                dblCardTot += VB.Val(dt1.Rows[k]["AMT"].ToString().Trim());
                                            }
                                            else
                                            {
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Val(dt1.Rows[k]["AMT"].ToString().Trim()).ToString("###,###,###,###");
                                                dblCashTot += VB.Val(dt1.Rows[k]["AMT"].ToString().Trim());
                                            }
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                    break;
                                case "2":
                                    SQL = "";
                                    SQL = "SELECT";
                                    SQL = SQL + ComNum.VBLF + "     SUM(CASE WHEN  TRANHEADER = '1' THEN 1 else -1 END * TRADEAMT) AS AMT, GUBUN";
                                    SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "CARD_APPROV ";
                                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + lblPano.Text.Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "         AND (CARDNO IS NULL OR CARDNO <> '0100001234') ";

                                    if (i == 0)
                                    {
                                        SQL = SQL + ComNum.VBLF + "         AND GBIO IN ('I','O') ";
                                    }
                                    else
                                    {
                                        SQL = SQL + ComNum.VBLF + "         AND GBIO  = 'I' ";
                                    }

                                    if (intCnt2 == i - 1)
                                    {
                                        if (Convert.ToDateTime(ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text.Trim()) <= Convert.ToDateTime(GstrFDate))
                                        {
                                            SQL = SQL + ComNum.VBLF + "         AND ACTDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                                        }
                                        else
                                        {
                                            SQL = SQL + ComNum.VBLF + "         AND ACTDate >= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text.Trim() + "','YYYY-MM-DD') ";
                                        }

                                        if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text.Trim() == "")
                                        {
                                            SQL = SQL + ComNum.VBLF + "         AND ACTDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                                        }
                                        else
                                        {
                                            SQL = SQL + ComNum.VBLF + "         AND ACTDate <= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text.Trim() + "','YYYY-MM-DD') ";
                                        }
                                    }
                                    else
                                    {
                                        if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text.Trim() !="")
                                        {
                                            if (Convert.ToDateTime(GstrFDate) <= Convert.ToDateTime(ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text.Trim()))
                                            {
                                                SQL = SQL + ComNum.VBLF + "         AND ACTDate >= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text.Trim() + "','YYYY-MM-DD') ";
                                            }
                                            else
                                            {
                                                SQL = SQL + ComNum.VBLF + "         AND ACTDate >= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                                            }
                                        }
                                      

                                        SQL = SQL + ComNum.VBLF + "         AND ACTDate <= TO_DATE('" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text.Trim() + "','YYYY-MM-DD') ";
                                    }

                                    SQL = SQL + ComNum.VBLF + "GROUP BY GUBUN ";

                                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 66
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (k = 0; k < dt1.Rows.Count; k++)
                                        {
                                            if (dt1.Rows[k]["GUBUN"].ToString().Trim() == "1" || dt1.Rows[k]["GUBUN"].ToString().Trim() == "3")
                                            {
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = VB.Val(dt1.Rows[k]["AMT"].ToString().Trim()).ToString("###,###,###,###");
                                                dblCardTot += VB.Val(dt1.Rows[k]["AMT"].ToString().Trim());
                                            }
                                            else
                                            {
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Val(dt1.Rows[k]["AMT"].ToString().Trim()).ToString("###,###,###,###");
                                                dblCashTot += VB.Val(dt1.Rows[k]["AMT"].ToString().Trim());
                                            }
                                        }
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                    break;
                            }

                            strRemark = "";

                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "** 합 계 **";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dblTotBiAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dblTotBoAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dblCardTot.ToString("###,###,###,###");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dblCashTot.ToString("###,###,###,###");

                lblTotBoAmt.Text = dblTotBoAmt.ToString("###,###,###,##0원정");

                SQL = "";
                SQL = "DROP VIEW VIEW_JENGSAN";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                if (Convert.ToDateTime(GstrFDate) <= Convert.ToDateTime(strSysDate).AddDays(-1830))
                {
                    ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다.");

                    btnPrint.Enabled = false;

                    if (clsVbfunc.JinAmtPrintChk(clsDB.DbCon, clsType.User.Sabun) == true)
                    {
                        btnPrint.Enabled = true;
                    }
                }
                else
                {
                    btnPrint.Enabled = true;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GetPrint();
        }

        private void GetPrint()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            if (ssView_Sheet1.RowCount == 0) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            int k = 0;

            int intMM = 0;
            int intRow = 0;
            int intIlsu = 0;
            int intPrtCnt = 0;

            string strSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            string strSDate = "";
            string strInDate = "";
            string strOutDate = "";
            string strRemark = "";
            string strGubun = "";
            string strDate = "";
            string strFSdate = "";
            string strOK = "";

            double dblSubBiAmt = 0;
            double dblSubBoAmt = 0;
            double dblSubBonin1 = 0;
            double dblSubBonin2 = 0;
            double dblSubGamAmt = 0;
            double dblBoAmt = 0;
            double dblBiAmt = 0;
            double dblGamAmt = 0;
            double dblMiBoAmt1 = 0;
            double dblMiBiAmt1 = 0;
            double dblMiToAmt1 = 0;
            double dblCardAmt = 0;
            double dblCashAmt = 0;
            double dblSubCardAmt = 0;
            double dblSubCashAmt = 0;
            double dblBoinGubAmt = 0;
            double dblBoinBiGubAmt = 0;

            double[,] dblOAmt = new double[13, 3];
            double[,] dblIAmt = new double[13, 3];

            string[] strTDate = new string[13];
            string[] strFDate = new string[13];
            string[] strTdate_1 = new string[13];
            string[] strFdate_1 = new string[13];
            string[] strMidDate = new string[21];

            Print_Clear();

            Cursor.Current = Cursors.WaitCursor;

            if (VB.Left(cboYear.Text.Trim(), 4) == "2008")
            {
                switch (lblPano.Text.Trim())
                {
                    case "07117678":
                    case "07148932":
                    case "07516411":
                        strOK = "Y";
                        break;
                }
            }

            if (strOK == "Y")
            {
                ComFunc.MsgBox("산전지원금이 포함된 환자일수 있습니다..slip이나 입원금을 확인하시고 환자에게 주십시오");

                if (ComFunc.MsgBoxQ("그래도 인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            //자동인쇄 아닐경우 his
            if (GstrAutoMatic == "N")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PRTCNT";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_PRTCNT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE JOBDATE = TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "         AND SABUN = '" + clsType.User.Sabun + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_PRTCNT";
                        SQL = SQL + ComNum.VBLF + "     (JOBDATE, SABUN, PRTCNT)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
                        SQL = SQL + ComNum.VBLF + "         '" + clsType.User.Sabun + "', ";
                        SQL = SQL + ComNum.VBLF + "         1 ";
                        SQL = SQL + ComNum.VBLF + "     )";
                    }
                    else
                    {
                        intPrtCnt = (int)VB.Val(dt.Rows[0]["PRTCNT"].ToString().Trim()) + 1;

                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_PRTCNT";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         PRTCNT  = " + intPrtCnt + " ";
                        SQL = SQL + ComNum.VBLF + "WHERE JOBDATE = TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + "     AND SABUN = '" + clsType.User.Sabun + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            strDate = Convert.ToDateTime(strSysDate).ToString("yyyy년 MM월 dd일");

            ssPrint_Sheet1.Cells[2, 2].Text = VB.Left(lblSName.Text.Trim(), 1) + " " + VB.Mid(lblSName.Text.Trim(), 2, 1) + " " + VB.Mid(lblSName.Text.Trim(), 3, 2);
            ssPrint_Sheet1.Cells[2, 8].Text = lblJumin1.Text.Trim() + " - " + lblJumin2.Text.Trim();

            for (i = 0; i < 13; i++)
            {
                for (k = 0; k < 3; k++)
                {
                    dblOAmt[i, k] = 0;
                    dblIAmt[i, k] = 0;
                }
            }

            for (i = 0; i < 13; i++)
            {
                strTDate[i] = "";
                strFDate[i] = "";
            }

            intRow = 6;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                strSDate = ssView_Sheet1.Cells[i, 0].Text.Trim();

                intMM = (int)VB.Val(VB.Mid(strSDate, 6, 2));

                dblBiAmt = VB.Val(ssView_Sheet1.Cells[i, 4].Text.Trim().Replace(",",""));
                dblBoAmt = VB.Val(ssView_Sheet1.Cells[i, 5].Text.Trim().Replace(",", ""));
                dblCardAmt = VB.Val(ssView_Sheet1.Cells[i, 9].Text.Trim().Replace(",", ""));
                dblCashAmt = VB.Val(ssView_Sheet1.Cells[i, 10].Text.Trim().Replace(",", ""));

                dblBoinBiGubAmt = VB.Val(ssView_Sheet1.Cells[i, 11].Text.Trim().Replace(",", ""));
                dblBoinGubAmt = VB.Val(ssView_Sheet1.Cells[i, 14].Text.Trim().Replace(",", ""));

                dblGamAmt = VB.Val(ssView_Sheet1.Cells[i, 12].Text.Trim().Replace(",", ""));

                strGubun = ssView_Sheet1.Cells[i, 1].Text.Trim();

                if (strGubun == "외래진료비")
                {
                    dblSubBiAmt = dblSubBiAmt + dblBiAmt;
                    dblSubBoAmt = dblSubBoAmt + dblBoAmt;

                    dblSubBonin1 = dblSubBonin1 + dblBoinGubAmt;
                    dblSubBonin2 = dblSubBonin2 + dblBoinBiGubAmt;

                    dblSubGamAmt = dblSubGamAmt + dblGamAmt;

                    dblSubCardAmt = dblSubCardAmt + dblCardAmt;
                    dblSubCashAmt = dblSubCashAmt + dblCashAmt;

                    ssPrint_Sheet1.Cells[intRow, 1].Text = strSDate;
                    ssPrint_Sheet1.Cells[intRow, 2].Text = "외래";
                    ssPrint_Sheet1.Cells[intRow, 3].Text = (dblBiAmt + dblBoAmt).ToString("#,###,###,##0");
                    ssPrint_Sheet1.Cells[intRow, 4].Text = dblBiAmt.ToString("#,###,###,##0");
                    ssPrint_Sheet1.Cells[intRow, 5].Text = dblBoinGubAmt.ToString("#,###,###,##0");
                    ssPrint_Sheet1.Cells[intRow, 6].Text = dblBoinBiGubAmt.ToString("#,###,###,##0");

                    ssPrint_Sheet1.Cells[intRow, 7].Text = dblBoAmt.ToString("#,###,###,##0");
                    ssPrint_Sheet1.Cells[intRow, 8].Text = dblCardAmt.ToString("#,###,###,###");
                    ssPrint_Sheet1.Cells[intRow, 9].Text = dblCashAmt.ToString("#,###,###,###");
                    ssPrint_Sheet1.Cells[intRow, 10].Text = (dblBoAmt - dblCardAmt - dblCashAmt).ToString("#,###,###,##0");

                    intRow++;

                    if (intRow == 24)
                    {
                        ssPrint_Sheet1.Cells[24, 3].Text = (dblSubBiAmt + dblSubBoAmt).ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 4].Text = dblSubBiAmt.ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 5].Text = dblSubBonin1.ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 6].Text = dblSubBonin2.ToString("#,###,###,##0");

                        ssPrint_Sheet1.Cells[24, 7].Text = dblSubBoAmt.ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 8].Text = dblSubCardAmt.ToString("#,###,###,###");
                        ssPrint_Sheet1.Cells[24, 9].Text = dblSubCashAmt.ToString("#,###,###,###");
                        ssPrint_Sheet1.Cells[24, 10].Text = (dblSubBoAmt - dblSubCardAmt - dblSubCashAmt).ToString("#,###,###,##0");

                        ssPrint_Sheet1.Cells[25, 8].Text = dblSubBoAmt.ToString("#,###,###,##0");

                        if (dblSubGamAmt > 0)
                        {
                            ssPrint_Sheet1.Cells[25, 1].Text = "소득공제 대상액 총계 (감액 : " + (dblSubGamAmt * -1).ToString("#,###,###,##0") + " )";
                        }

                        ssPrint_Sheet1.Cells[29, 1].Text = strDate;

                        dblSubBiAmt = 0;
                        dblSubBoAmt = 0;
                        dblSubCardAmt = 0;
                        dblSubCashAmt = 0;
                        dblSubBonin1 = 0;
                        dblSubBonin2 = 0;
                        dblSubGamAmt = 0;
                        intRow = 6;

                        Print_Sheet();
                        ComFunc.Delay(200);
                        Print_Clear();
                    }
                }
            }

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                strGubun = ssView_Sheet1.Cells[i, 1].Text.Trim();
                strRemark = ssView_Sheet1.Cells[i, 8].Text.Trim();

                if (strGubun == "퇴원진료비" && strRemark != "입원중 수납금액")
                {
                    dblBiAmt = VB.Val(ssView_Sheet1.Cells[i, 4].Text.Trim().Replace(",", ""));
                    dblBoAmt = VB.Val(ssView_Sheet1.Cells[i, 5].Text.Trim().Replace(",", ""));
                    dblCardAmt = VB.Val(ssView_Sheet1.Cells[i, 9].Text.Trim().Replace(",", ""));
                    dblCashAmt = VB.Val(ssView_Sheet1.Cells[i, 10].Text.Trim().Replace(",", ""));

                    dblBoinBiGubAmt = VB.Val(ssView_Sheet1.Cells[i, 11].Text.Trim().Replace(",", ""));
                    dblBoinGubAmt = VB.Val(ssView_Sheet1.Cells[i, 14].Text.Trim().Replace(",", ""));

                    dblGamAmt = VB.Val(ssView_Sheet1.Cells[i, 12].Text.Trim().Replace(",", ""));

                    strFSdate = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strOutDate = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    intIlsu = (int)VB.Val(ssView_Sheet1.Cells[i, 8].Text.Trim().Replace(",", ""));

                    if (strRemark == "미수입금")
                    {
                        if (intRow != 6)
                        {
                            dblMiToAmt1 = VB.Val(ssView_Sheet1.Cells[intRow - 1, 3].Text.Trim().Replace(",", ""));
                            dblMiBiAmt1 = VB.Val(ssView_Sheet1.Cells[intRow - 1, 4].Text.Trim().Replace(",", ""));

                            dblMiBoAmt1 = VB.Val(ssView_Sheet1.Cells[intRow - 1, 7].Text.Trim().Replace(",", ""));

                            ssPrint_Sheet1.Cells[intRow - 1, 3].Text = (dblMiToAmt1 + dblBiAmt + dblBoAmt).ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow - 1, 4].Text = (dblMiBiAmt1 + dblBiAmt).ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow - 1, 5].Text = (dblMiBoAmt1 + dblBoAmt).ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[intRow - 1, 8].Text = dblCardAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow - 1, 9].Text = dblCashAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow - 1, 10].Text = (dblMiBoAmt1 + dblBoAmt - dblCardAmt - dblCashAmt).ToString("#,###,###,##0");
                        }
                        else
                        {
                            dblMiToAmt1 = VB.Val(ssPrint_Sheet1.Cells[intRow, 3].Text.Trim().Replace(",", ""));
                            dblMiBiAmt1 = VB.Val(ssPrint_Sheet1.Cells[intRow, 4].Text.Trim().Replace(",", ""));

                            dblMiBoAmt1 = VB.Val(ssPrint_Sheet1.Cells[intRow, 7].Text.Trim().Replace(",", ""));

                            ssPrint_Sheet1.Cells[intRow, 3].Text = (dblMiToAmt1 + dblBiAmt + dblBoAmt).ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 4].Text = (dblMiBiAmt1 + dblBiAmt).ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 5].Text = (dblMiBoAmt1 + dblBoAmt).ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[intRow, 8].Text = dblCardAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow, 9].Text = dblCashAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow, 10].Text = (dblMiBoAmt1 + dblBoAmt - dblCardAmt - dblCashAmt).ToString("#,###,###,##0");
                        }

                        dblSubBiAmt = dblSubBiAmt + dblBiAmt;
                        dblSubBoAmt = dblSubBoAmt + dblBoAmt;

                        dblSubBonin1 = dblSubBonin1 + dblBoinGubAmt;
                        dblSubBonin2 = dblSubBonin2 + dblBoinBiGubAmt;

                        dblSubGamAmt = dblSubGamAmt + dblGamAmt;

                        dblSubCardAmt = dblSubCardAmt + dblCardAmt;
                        dblSubCashAmt = dblSubCashAmt + dblCashAmt;
                    }
                    else
                    {
                        if (intRow != 6)
                        {
                            ssPrint_Sheet1.Cells[intRow, 1].Text = strFSdate + "~" + VB.Right((strOutDate == "" ? "현재" : strOutDate), 5) + "(" + intIlsu + ")";
                            ssPrint_Sheet1.Cells[intRow, 2].Text = "입원";
                            ssPrint_Sheet1.Cells[intRow, 3].Text = (dblBiAmt + dblBoAmt).ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 4].Text = dblBiAmt.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 5].Text = dblBoinGubAmt.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 6].Text = dblBoinBiGubAmt.ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[intRow, 7].Text = dblBoAmt.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 8].Text = dblCardAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow, 9].Text = dblCashAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow, 10].Text = (dblBoAmt - dblCardAmt - dblCashAmt).ToString("#,###,###,##0");
                        }
                        else
                        {
                            ssPrint_Sheet1.Cells[intRow, 1].Text = strFSdate + "~" + VB.Right((strOutDate == "" ? "현재" : strOutDate), 5) + "(" + intIlsu + ")";
                            ssPrint_Sheet1.Cells[intRow, 2].Text = "입원";

                            dblMiToAmt1 = VB.Val(ssPrint_Sheet1.Cells[intRow, 3].Text.Trim().Replace(",", ""));
                            dblMiBiAmt1 = VB.Val(ssPrint_Sheet1.Cells[intRow, 4].Text.Trim().Replace(",", ""));

                            ssPrint_Sheet1.Cells[intRow, 7].Text = dblBoAmt.ToString("#,###,###,##0");

                            dblMiBoAmt1 = VB.Val(ssView_Sheet1.Cells[intRow, 7].Text.Trim().Replace(",", ""));

                            ssPrint_Sheet1.Cells[intRow, 3].Text = (dblMiToAmt1 + dblBiAmt + dblBoAmt).ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 4].Text = (dblMiBiAmt1 + dblBiAmt).ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[intRow, 5].Text = dblBoinGubAmt.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 6].Text = dblBoinBiGubAmt.ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[intRow, 8].Text = dblCardAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow, 9].Text = dblCashAmt.ToString("#,###,###,###");
                            ssPrint_Sheet1.Cells[intRow, 10].Text = (dblMiBoAmt1 + dblBoAmt - dblCardAmt - dblCashAmt).ToString("#,###,###,##0");
                        }

                        dblSubBiAmt = dblSubBiAmt + dblBiAmt;
                        dblSubBoAmt = dblSubBoAmt + dblBoAmt;

                        dblSubBonin1 = dblSubBonin1 + dblBoinGubAmt;
                        dblSubBonin2 = dblSubBonin2 + dblBoinBiGubAmt;

                        dblSubGamAmt = dblSubGamAmt + dblGamAmt;

                        dblSubCardAmt = dblSubCardAmt + dblCardAmt;
                        dblSubCashAmt = dblSubCashAmt + dblCashAmt;

                        dblMiToAmt1 = 0;
                        dblMiBiAmt1 = 0;
                        dblMiBoAmt1 = 0;

                        intRow++;

                        if (intRow == 24)
                        {
                            ssPrint_Sheet1.Cells[intRow, 3].Text = (dblSubBiAmt + dblSubBoAmt).ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 4].Text = dblSubBiAmt.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 5].Text = dblSubBonin1.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 6].Text = dblSubBonin2.ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[intRow, 7].Text = dblSubBoAmt.ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[intRow, 8].Text = dblSubCardAmt.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 9].Text = dblSubCashAmt.ToString("#,###,###,##0");
                            ssPrint_Sheet1.Cells[intRow, 10].Text = (dblSubBonin2 - dblSubCardAmt - dblSubCashAmt).ToString("#,###,###,##0");

                            ssPrint_Sheet1.Cells[25, 8].Text = dblSubBoAmt.ToString("#,###,###,##0");

                            if (dblSubGamAmt > 0)
                            {
                                ssPrint_Sheet1.Cells[25, 1].Text = "소득공제 대상액 총계 (감액 : " + (dblSubGamAmt * -1).ToString("#,###,###,##0") + " )";
                            }

                            ssPrint_Sheet1.Cells[29, 1].Text = strDate;

                            dblSubBiAmt = 0;
                            dblSubBoAmt = 0;
                            dblSubCashAmt = 0;
                            dblSubCardAmt = 0;
                            dblSubBonin1 = 0;
                            dblSubBonin2 = 0;
                            dblSubGamAmt = 0;

                            intRow = 6;

                            Print_Sheet();

                            ComFunc.Delay(200);

                            Print_Clear();
                        }
                    }
                }
            }

            if (intRow == 24)
            {
                ssPrint_Sheet1.Cells[24, 3].Text = (dblSubBiAmt + dblSubBoAmt).ToString("#,###,###,##0");
                ssPrint_Sheet1.Cells[24, 4].Text = dblSubBiAmt.ToString("#,###,###,##0");

                ssPrint_Sheet1.Cells[24, 5].Text = dblSubBonin1.ToString("#,###,###,##0");
                ssPrint_Sheet1.Cells[24, 6].Text = dblSubBonin2.ToString("#,###,###,##0");

                ssPrint_Sheet1.Cells[24, 7].Text = dblSubBoAmt.ToString("#,###,###,##0");

                ssPrint_Sheet1.Cells[24, 8].Text = dblSubCardAmt.ToString("#,###,###,##0");
                ssPrint_Sheet1.Cells[24, 9].Text = dblSubCashAmt.ToString("#,###,###,##0");
                ssPrint_Sheet1.Cells[24, 10].Text = (dblSubBoAmt - dblSubCardAmt - dblSubCashAmt).ToString("#,###,###,##0");

                ssPrint_Sheet1.Cells[25, 8].Text = dblSubBoAmt.ToString("#,###,###.##0");

                ssPrint_Sheet1.Cells[29, 1].Text = strDate;

                dblSubBiAmt = 0;
                dblSubBoAmt = 0;
                dblSubBonin1 = 0;
                dblSubBonin2 = 0;
                dblSubGamAmt = 0;

                intRow = 6;

                Print_Sheet();

                ComFunc.Delay(200);

                Print_Clear();
            }

            //입원경우 중간납
            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                strGubun = ssView_Sheet1.Cells[i, 1].Text.Trim();
                strRemark = ssView_Sheet1.Cells[i, 8].Text.Trim();

                if (strGubun == "퇴원진료비" && strRemark == "입원중 수납금액")
                {
                    dblBiAmt = VB.Val(ssView_Sheet1.Cells[i, 4].Text.Trim().Replace(",", ""));

                    dblBoAmt = VB.Val(ssView_Sheet1.Cells[i, 7].Text.Trim().Replace(",", ""));
                    dblCardAmt = VB.Val(ssView_Sheet1.Cells[i, 9].Text.Trim().Replace(",", ""));
                    dblCashAmt = VB.Val(ssView_Sheet1.Cells[i, 10].Text.Trim().Replace(",", ""));

                    strFSdate = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strInDate = ssView_Sheet1.Cells[i, 0].Text.Trim();

                    ssPrint_Sheet1.Cells[intRow, 1].Text = strFSdate + "∼" + VB.Right(strInDate, 5) + "(" + VB.DateDiff("d", Convert.ToDateTime(strInDate), Convert.ToDateTime(strFSdate)) + ")";
                    ssPrint_Sheet1.Cells[intRow, 2].Text = "입원";
                    ssPrint_Sheet1.Cells[intRow, 3].Text = (dblBiAmt + dblBoAmt).ToString("#,###,###,##0");
                    ssPrint_Sheet1.Cells[intRow, 4].Text = dblBiAmt.ToString("#,###,###,##0");

                    ssPrint_Sheet1.Cells[intRow, 7].Text = dblBoAmt.ToString("#,###,###,##0");

                    ssPrint_Sheet1.Cells[intRow, 8].Text = dblCardAmt.ToString("#,###,###,##0");
                    ssPrint_Sheet1.Cells[intRow, 9].Text = dblCashAmt.ToString("#,###,###,##0");
                    ssPrint_Sheet1.Cells[intRow, 10].Text = (dblBoAmt - dblCardAmt - dblCashAmt).ToString("#,###,###,##0");

                    dblSubBiAmt = dblSubBiAmt + dblBiAmt;
                    dblSubBoAmt = dblSubBoAmt + dblBoAmt;

                    dblSubBonin1 = dblSubBonin1 + dblBoinGubAmt;
                    dblSubBonin2 = dblSubBonin2 + dblBoinBiGubAmt;

                    dblSubCardAmt = dblSubCardAmt + dblCardAmt;
                    dblSubCashAmt = dblSubCashAmt + dblCashAmt;

                    intRow++;

                    if (intRow == 23)
                    {
                        ssPrint_Sheet1.Cells[24, 3].Text = (dblSubBiAmt + dblSubBoAmt).ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 4].Text = dblSubBiAmt.ToString("#,###,###,##0");

                        ssPrint_Sheet1.Cells[24, 5].Text = dblSubBonin1.ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 6].Text = dblSubBonin2.ToString("#,###,###.##0");

                        ssPrint_Sheet1.Cells[24, 7].Text = dblSubBoAmt.ToString("#,###,###,##0");

                        ssPrint_Sheet1.Cells[24, 8].Text = dblSubCardAmt.ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 9].Text = dblSubCashAmt.ToString("#,###,###,##0");
                        ssPrint_Sheet1.Cells[24, 10].Text = (dblSubBoAmt - dblSubCardAmt - dblSubCashAmt).ToString("#,###,###,##0");

                        ssPrint_Sheet1.Cells[25, 8].Text = dblSubBoAmt.ToString("#,###,###,##0");

                        if (dblSubGamAmt > 0)
                        {
                            ssPrint_Sheet1.Cells[25, 1].Text = "소득공제 대상액 총계 (감액 : " + (dblSubGamAmt * -1).ToString("#,###,###,##0") + " )";
                        }

                        ssPrint_Sheet1.Cells[29, 1].Text = strDate;

                        dblSubBiAmt = 0;
                        dblSubBoAmt = 0;
                        dblSubBonin1 = 0;
                        dblSubBonin2 = 0;
                        dblSubGamAmt = 0;

                        intRow = 6;

                        Print_Sheet();

                        ComFunc.Delay(200);

                        Print_Clear();
                    }
                }
            }

            ssPrint_Sheet1.Cells[24, 3].Text = (dblSubBiAmt + dblSubBoAmt).ToString("#,###,###,##0");
            ssPrint_Sheet1.Cells[24, 4].Text = dblSubBiAmt.ToString("#,###,###,##0");

            ssPrint_Sheet1.Cells[24, 5].Text = dblSubBonin1.ToString("#,###,###,##0");
            ssPrint_Sheet1.Cells[24, 6].Text = dblSubBonin2.ToString("#,###,###,##0");

            ssPrint_Sheet1.Cells[24, 7].Text = dblSubBoAmt.ToString("#,###,###,##0");

            ssPrint_Sheet1.Cells[24, 8].Text = dblSubCardAmt.ToString("#,###,###,###");
            ssPrint_Sheet1.Cells[24, 9].Text = dblSubCashAmt.ToString("#,###,###,###");
            ssPrint_Sheet1.Cells[24, 10].Text = (dblSubBoAmt - dblSubCardAmt - dblSubCashAmt).ToString("#,###,###,##0");

            ssPrint_Sheet1.Cells[25, 8].Text = dblSubBoAmt.ToString("#,###,###,##0");

            ssPrint_Sheet1.Cells[25, 1].Text = "소득공제 대상액 총계";

            if (dblSubGamAmt > 0)
            {
                ssPrint_Sheet1.Cells[25, 1].Text = "소득공제 대상액 총계 (감액 : " + (dblSubGamAmt * -1).ToString("#,###,###,##0") + " )";
            }

            ssPrint_Sheet1.Cells[29, 1].Text = strDate;

            dblSubBiAmt = 0;
            dblSubBoAmt = 0;
            dblSubCardAmt = 0;
            dblSubCashAmt = 0;
            dblSubBonin1 = 0;
            dblSubBonin2 = 0;
            dblSubGamAmt = 0;

            ssPrint_Sheet1.Cells[intRow, 3, 23, 9].Text = "";

            Print_Sheet();
        }

        private void Print_Clear()
        {
            ssPrint_Sheet1.Cells[6, 1, 23, 2].Text = "";
            ssPrint_Sheet1.Cells[6, 3, 23, 10].Text = "";
            ssPrint_Sheet1.Cells[24, 2, 24, 10].Text = "";
            ssPrint_Sheet1.Cells[25, 8].Text = "";
            ssPrint_Sheet1.Cells[26, 1].Text = "소득공제 대상액 총계";
        }

        private void Print_Sheet()
        {
            ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 60;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = true;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = true;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPrint_Sheet1.PrintInfo.Preview = false;
            ssPrint.PrintSheet(0);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (txtJumin2.Text.Length == 7 && txtJumin1.Text.Length == 6 && lblPrtinfo.Text.Trim() != "")
            {
                if (ssView_Sheet1.RowCount > 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "자료 출력중입니다 !!";

                    GintTimer = 0;

                    Application.DoEvents();

                    GetPrint();
                    Save_Auto_Print_His(txtName.Text.Trim(), "자동출력");
                    btnNumber_Click(btnCC, e);
                }
            }
        }

        private void Save_Auto_Print_His(string strPano, string strRemark)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_PRINT_AUTO";
            SQL = SQL + ComNum.VBLF + "     (ACTDATE, PANO, REMARK, ENTDATE)";
            SQL = SQL + ComNum.VBLF + "VALUES";
            SQL = SQL + ComNum.VBLF + "     (";
            SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
            SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
            SQL = SQL + ComNum.VBLF + "         '" + strRemark + "', ";
            SQL = SQL + ComNum.VBLF + "         SYSDATE";
            SQL = SQL + ComNum.VBLF + "     )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoGubun_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                txtName.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GintTimer++;

            if (GintTimer >= 5)
            {
                GintTimer = 0;
                lblMsg.Visible = false;
                lblPrtinfo.Text = "주민번호 13자리를 정확히 입력하세요!!";
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            if (VB.Right(((Button)sender).Name, 2) == "CC")
            {
                lblPrtinfo.Text = "";
                txtJumin1.Text = "";
                txtJumin2.Text = "";
                txtJumin1.Focus();
            }
            else if (VB.Right(((Button)sender).Name, 2) == "BC")
            {
                if (VB.Len(txtJumin1.Text) == 6 && VB.Len(txtJumin2.Text) == 0)
                {
                    txtJumin1.Text = VB.Left(txtJumin1.Text, VB.Len(txtJumin1.Text) - 1);
                    txtJumin1.SelectionLength = VB.Len(txtJumin1.Text);
                    txtJumin1.Focus();
                }
                else if (VB.Len(txtJumin1.Text) == 6 && VB.Len(txtJumin2.Text) > 0)
                {
                    txtJumin2.Text = VB.Left(txtJumin2.Text, VB.Len(txtJumin2.Text) - 1);
                    txtJumin2.SelectionLength = VB.Len(txtJumin2.Text);
                    txtJumin2.Focus();
                }
                else if (VB.Len(txtJumin1.Text) < 6 && VB.Len(txtJumin2.Text) == 0)
                {
                    if (txtJumin1.Text.Trim() != "")
                    {
                        txtJumin1.Text = VB.Left(txtJumin1.Text, VB.Len(txtJumin1.Text) - 1);
                        txtJumin1.SelectionLength = VB.Len(txtJumin1.Text);
                        txtJumin1.Focus();
                    }
                }
                else if (VB.Len(txtJumin1.Text) < 6 && VB.Len(txtJumin2.Text) > 0)
                {
                    txtJumin2.Text = VB.Left(txtJumin2.Text, VB.Len(txtJumin2.Text) - 1);
                    txtJumin2.SelectionLength = VB.Len(txtJumin2.Text);
                    txtJumin2.Focus();
                }
                else if (VB.Len(txtJumin1.Text) == 0 && VB.Len(txtJumin2.Text) == 0)
                {
                    txtJumin1.SelectionLength = VB.Len(txtJumin1.Text);
                    txtJumin1.Focus();
                    return;
                }
            }
            else
            {
                if (VB.Len(txtJumin1.Text) >= 6)
                {
                    txtJumin2.Text += ((Button)sender).Text;
                }
                else
                {
                    txtJumin1.Text += ((Button)sender).Text;
                }
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }
     }
}
