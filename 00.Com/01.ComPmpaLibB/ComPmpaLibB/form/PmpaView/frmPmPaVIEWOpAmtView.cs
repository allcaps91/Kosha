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
    /// File Name       : frmPmPaVIEWOpAmtView.cs
    /// Description     : 접수번호별 삭감 조회
    /// Author          : 김효성
    /// Create Date     : 2017-09-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\psmh\misu\misubsmisubs.vbp\FrmReMirView4(misubs64.FRM)  >> frmPmPaVIEWOpAmtView.cs 폼이름 재정의" />	

    public partial class frmPmPaVIEWOpAmtView : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();


        string[] GstrSETBis = new string[100];
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        double[] nAmAMT = new double[60];
        string strIllCode = "";

        public frmPmPaVIEWOpAmtView()
        {
            InitializeComponent();
        }

        void ComboDept_SET(ComboBox ArgCombo, string ArgAll = "", string ArgTYPE = "1")
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            if (ArgAll == "")
            {
                ArgAll = "1";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DEPTCODE, DEPTNAMEK";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ArgCombo.Items.Clear();

            if (ArgAll == "1")
            {
                switch (ArgTYPE)
                {
                    case "1":
                        ArgCombo.Items.Add("**.전체");
                        break;
                    case "2":
                        ArgCombo.Items.Add("**");
                        break;
                    case "3":
                        ArgCombo.Items.Add("전체");
                        break;
                }
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                switch (ArgTYPE)
                {
                    case "1":
                        ArgCombo.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim() + "." + dt.Rows[i]["DeptNameK"].ToString().Trim());
                        break;
                    case "2":
                        ArgCombo.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        break;
                    case "3":
                        ArgCombo.Items.Add(dt.Rows[i]["DeptNamek"].ToString().Trim());
                        break;
                }
            }

            ArgCombo.SelectedIndex = 0;

            dt.Dispose();
            dt = null;
        }

        private void frmPmPaVIEWOpAmtView_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView.Dock = DockStyle.Fill;
            panHidden.Visible = false;
            ssView_Sheet1.Columns[13].Visible = false;
            btnReView.Enabled = false;
            btnPrint.Enabled = false;
            txtOpCode.Text = "";
            txtOpName.Text = "";

            dtpFdate.Value = Convert.ToDateTime(strDTP);
            dtpTdate.Value = Convert.ToDateTime(strDTP);

            ComboDept_SET(cboDept, "", "1");

        }

        private void Clear()
        {

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnReView.Enabled = true;
            txtOpCode.Enabled = true;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            panHidden.Visible = false;

            ssView_Sheet1.RowCount = 30;

            ssView_Sheet1.Cells[0, ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.ColumnCount - 1].Text = "";
            txtOpCode.Text = "";
            txtOpName.Text = "";
            txtOpCode.Select();
        }
        private void btnReView_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            panHidden.Visible = false;
            btnView.Enabled = false;
            btnPrint.Enabled = false;
            btnReView.Enabled = false;
            txtOpCode.Enabled = false;

            if (txtOpCode.Text == "")
            {
                if (rdoOptGB0.Checked == true)
                {
                    ComFunc.MsgBox("상병코드가 없습니다.", "확인");
                }
                else
                {
                    ComFunc.MsgBox("수가코드가 없습니다.", "확인");
                }

                btnView.Enabled = true;
                btnPrint.Enabled = true;
                btnReView.Enabled = true;
                txtOpCode.Enabled = true;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 1;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (rdoOptGB0.Checked == true)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.Indate, 'YY-MM-DD') Indate1,A.Pano, B.Sname, A.Bi, A.DeptCode,   ";
                    SQL = SQL + ComNum.VBLF + "        B.Ilsu , B.Sex, B.Age,                                                     ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt01+A.Amt02+A.Amt03+A.Amt04+A.Amt05+A.Amt06+A.Amt07+A.Amt08+           ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt09+A.Amt10+A.Amt11+A.Amt12+A.Amt13+A.Amt14+A.Amt15+A.Amt16+A.Amt17+   ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt18+A.Amt19+A.Amt20 PayTot,                                            ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt21+A.Amt22+A.Amt23+A.Amt24+A.Amt25+A.Amt26+A.Amt27+A.Amt28+           ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt29+A.Amt30+A.Amt31+A.Amt32+A.Amt33+A.Amt34+A.Amt35+A.Amt36+A.Amt37+   ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt38+A.Amt39+A.Amt40+A.Amt41+A.Amt42+A.Amt43+A.Amt44+A.Amt45+A.Amt46+   ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt47+A.Amt48+A.Amt49 INPayTot,A.Amt50,                                  ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt50-A.Amt53 BonInTot,A.Amt53,A.ROWID                                   ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A , " + ComNum.DB_PMPA + "IPD_NEW_MASTER B ";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.ACTDATE >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                 ";
                    SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                 ";
                    SQL = SQL + ComNum.VBLF + "    AND A.Amt50 > 0                                                                ";

                    SQL = SQL + ComNum.VBLF + "    AND (A.IllCode1 = '" + VB.UCase(txtOpCode.Text) + "'  or (a.illcode1 IS NULL and a.illcode2 = '" + VB.UCase(txtOpCode.Text) + "'))  ";

                    if (VB.Left(cboDept.Text, 2) != "**")
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "'";

                    SQL = SQL + ComNum.VBLF + "    AND A.AmSet5 <> '5'                                                            ";
                    SQL = SQL + ComNum.VBLF + "    AND (A.Bi <> '31' OR A.Bi <> '52')                                             ";
                    SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = B.IPDNO                                                          ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.Indate DESC,A.Pano                                                    ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.Indate, 'YY-MM-DD') Indate1,A.Pano, B.Sname, A.Bi, A.DeptCode,   ";
                    SQL = SQL + ComNum.VBLF + "        B.Ilsu , B.Sex, B.Age,                                                     ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt01+A.Amt02+A.Amt03+A.Amt04+A.Amt05+A.Amt06+A.Amt07+A.Amt08+           ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt09+A.Amt10+A.Amt11+A.Amt12+A.Amt13+A.Amt14+A.Amt15+A.Amt16+A.Amt17+   ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt18+A.Amt19+A.Amt20 PayTot,                                            ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt21+A.Amt22+A.Amt23+A.Amt24+A.Amt25+A.Amt26+A.Amt27+A.Amt28+           ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt29+A.Amt30+A.Amt31+A.Amt32+A.Amt33+A.Amt34+A.Amt35+A.Amt36+A.Amt37+   ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt38+A.Amt39+A.Amt40+A.Amt41+A.Amt42+A.Amt43+A.Amt44+A.Amt45+A.Amt46+   ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt47+A.Amt48+A.Amt49 INPayTot,A.Amt50,                                  ";
                    SQL = SQL + ComNum.VBLF + "        A.Amt50-A.Amt53 BonInTot,A.Amt53,A.ROWID                                   ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A , " + ComNum.DB_PMPA + "IPD_NEW_MASTER B, " + ComNum.DB_PMPA + "IPD_NEW_SLIP C                                             ";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.ACTDATE >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                 ";
                    SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                 ";
                    SQL = SQL + ComNum.VBLF + "    AND A.Amt50 > 0                                                                ";

                    if (VB.Left(cboDept.Text, 2) != "**")
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "'";

                    SQL = SQL + ComNum.VBLF + "    AND A.AmSet5 <> '5'                                                            ";
                    SQL = SQL + ComNum.VBLF + "    AND (A.Bi <> '31' OR A.Bi <> '52')                                             ";
                    SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = B.IPDNO                                                          ";
                    SQL = SQL + ComNum.VBLF + "    AND A.TRSNO = C.TRSNO ";

                    SQL = SQL + ComNum.VBLF + "    AND C.SUCODE ='" + (txtOpCode.Text) + "' ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.Indate DESC,A.Pano                                                    ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    txtOpCode.Enabled = true;
                    txtOpCode.Text = "";
                    txtOpCode.Select();
                    txtOpName.Text = "";
                    btnView.Enabled = true;
                    btnExit.Enabled = false;
                    btnPrint.Enabled = false;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("자료가 1건도 없습니다.");
                    return;
                }


                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Indate1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2 - 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3 - 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4 - 1].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5 - 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6 - 1].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7 - 1].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8 - 1].Text = dt.Rows[i]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9 - 1].Text = Convert.ToDouble(dt.Rows[i]["PayTot"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[i, 10 - 1].Text = Convert.ToDouble(dt.Rows[i]["InPayTot"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[i, 11 - 1].Text = Convert.ToDouble(dt.Rows[i]["Amt50"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[i, 12 - 1].Text = Convert.ToDouble(dt.Rows[i]["BonInTot"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[i, 13 - 1].Text = Convert.ToDouble(dt.Rows[i]["Amt53"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[i, 14 - 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                Cursor.Current = Cursors.Default;
                btnView.Enabled = true;
                btnPrint.Enabled = true;
                btnReView.Enabled = true;
                txtOpCode.Enabled = true;

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    btnView.Enabled = true;
                    btnPrint.Enabled = true;
                    btnReView.Enabled = true;
                    txtOpCode.Enabled = true;

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)

                if (rdoOptGB0.Checked == true)
                {
                    strTitle = "수술별로 예상진료비 조회";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("수술코드 : " + VB.UCase(txtOpCode.Text) + "[" + txtOpName.Text + "]", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }
                else
                {
                    strTitle = "수술별로 예상진료비 조회";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                    strHeader += CS.setSpdPrint_String("수가코드 : " + VB.UCase(txtOpCode.Text) + "[" + txtOpName.Text + "]", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoOptGB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOptGB0.Checked == true)
            {
                ComFunc.MsgBox("수가코드로 조회 시 소요시간이 길어집니다.", "확인");
                txtFrameCode.Text = "수가코드";
            }
            else
            {
                txtFrameCode.Text = "주 상병코드";
            }
        }

        private DataTable strIllCode1()
        {
            //조회
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + VB.UCase(strIllCode) + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return dt;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return dt;
                }
                return dt;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return dt;
            }
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int j = 0;
            int K = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            string strAmt = "";
            string strTOT1 = "";
            string strTOT2 = "";
            string strROWID = "";
            string strLabel = "";


            panHidden.Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strROWID = ssView_Sheet1.Cells[e.Row, 13].Text;

                if (strROWID == "")
                {
                    return;

                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.Indate, 'YYYY-MM-DD') Indate1,A.Pano,B.Sname,A.Bi,A.DeptCode,A.Ilsu,B.Sex, ";
                SQL = SQL + ComNum.VBLF + "        A.IllCode1,A.IllCode2,A.IllCode3,A.IllCode4,A.IllCode5,A.IllCode6,                     ";
                SQL = SQL + ComNum.VBLF + "        A.Amt01,A.Amt02,A.Amt03,A.Amt04,A.Amt05,A.Amt06,A.Amt07,A.Amt08,A.Amt09,A.Amt10,               ";
                SQL = SQL + ComNum.VBLF + "        A.Amt11,A.Amt12,A.Amt13,A.Amt14,A.Amt15,A.Amt16,A.Amt17,A.Amt18,A.Amt19,A.Amt20,               ";
                SQL = SQL + ComNum.VBLF + "        A.Amt21,A.Amt22,A.Amt23,A.Amt24,A.Amt25,A.Amt26,A.Amt27,A.Amt28,A.Amt29,A.Amt30,               ";
                SQL = SQL + ComNum.VBLF + "        A.Amt31,A.Amt32,A.Amt33,A.Amt34,A.Amt35,A.Amt36,A.Amt37,A.Amt38,A.Amt39,A.Amt40,               ";
                SQL = SQL + ComNum.VBLF + "        A.Amt41,A.Amt42,A.Amt43,A.Amt44,A.Amt45,A.Amt46,A.Amt47,A.Amt48,A.Amt49,A.Amt50,               ";
                SQL = SQL + ComNum.VBLF + "        A.Amt51,A.Amt52,A.Amt53,A.Amt54,A.Amt55,A.Amt56,A.Amt57,A.Amt58,A.Amt59,A.Amt60, A.ROWID       ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS a, " + ComNum.DB_PMPA + "BAS_PATIENT B                         ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ROWID = '" + strROWID + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO(+)  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                ssView.Visible = false;
                panHidden.Visible = true;

                label3.Text = "";
                label5.Text = "";
                label4.Text = "";

                // GoSub AMT_Move
                for (i = 1; i <= 60; i++)
                {
                    strAmt = "AMT" + i.ToString("00");
                }

                //AMT_TOT_Gesan:              '급여, 비급여 소계
                nTot1 = 0;
                nTot2 = 0;

                for (j = 1; j <= 49; j++)
                {
                    if (j <= 20)
                    {
                        nTot1 = nTot1 + nAmAMT[j];
                    }
                    else
                    {
                        nTot2 = nTot2 + nAmAMT[j];
                    }
                }

                //Label_Show:
                label3.Text = "등록번호 : " + dt.Rows[0]["pano"].ToString().Trim() + " " + dt.Rows[0]["sname"].ToString().Trim();
                label5.Text = "입원일자 : " + dt.Rows[0]["Indate1"].ToString().Trim() + " " + "(" + dt.Rows[0]["Ilsu"].ToString().Trim() + "일)";
                strLabel = "성별나이 : " + Convert.ToInt32(dt.Rows[0]["Age"].ToString().Trim()) + " " + "(" + dt.Rows[0]["Ilsu"].ToString().Trim() + "일)";
                strLabel = strLabel + " (" + GstrSETBis[(int)VB.Val(dt.Rows[0]["bi"].ToString().Trim())] + ")";
                label4.Text = strLabel;

                //Sheet_Show:
                for (i = 1; i <= 6; i++)
                {
                    switch (i)
                    {
                        case 1:
                            strIllCode = dt.Rows[0]["IllCode1"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                return;
                            }

                            SS3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SS3_Sheet1.Cells[i - 1, 1].Text = strIllCode1().Rows[0]["IllNameK"].ToString().Trim();
                            break;
                        case 2:
                            strIllCode = dt.Rows[0]["IllCode2"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                return;
                            }

                            SS3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SS3_Sheet1.Cells[i - 1, 1].Text = strIllCode1().Rows[0]["IllNameK"].ToString().Trim();
                            break;
                        case 3:
                            strIllCode = dt.Rows[0]["IllCode3"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                return;
                            }

                            SS3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SS3_Sheet1.Cells[i - 1, 1].Text = strIllCode1().Rows[0]["IllNameK"].ToString().Trim();
                            break;
                        case 4:
                            strIllCode = dt.Rows[0]["IllCode4"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                return;
                            }

                            SS3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SS3_Sheet1.Cells[i - 1, 1].Text = strIllCode1().Rows[0]["IllNameK"].ToString().Trim();
                            break;
                        case 5:
                            strIllCode = dt.Rows[0]["IllCode5"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                return;
                            }

                            SS3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SS3_Sheet1.Cells[i - 1, 1].Text = strIllCode1().Rows[0]["IllNameK"].ToString().Trim();
                            break;
                        case 6:
                            strIllCode = dt.Rows[0]["IllCode6"].ToString().Trim();
                            if (strIllCode == "")
                            {
                                return;
                            }

                            SS3_Sheet1.Cells[i - 1, 0].Text = strIllCode;
                            SS3_Sheet1.Cells[i - 1, 1].Text = strIllCode1().Rows[0]["IllNameK"].ToString().Trim();
                            break;
                    }
                }

                //AMT_Show:               'Spread Move Show
                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (K = 1; K <= 60; K++)
                {
                    if (K >= 11 && K <= 20)
                    {
                        SS2_Sheet1.Cells[(K - 10) - 1, 3].Text = nAmAMT[K].ToString("###,###,##0") + " ";
                    }
                    else if (K >= 21 && K <= 30)
                    {
                        SS2_Sheet1.Cells[(K - 10) - 1, 5].Text = nAmAMT[K].ToString("###,###,##0") + " ";
                    }

                    else if (K >= 31 && K <= 40)
                    {
                        SS2_Sheet1.Cells[(K - 10) - 1, 7].Text = nAmAMT[K].ToString("###,###,##0") + " ";
                    }
                    else if (K == 50)
                    {
                        SS2_Sheet1.Cells[11 - 1, 9].Text = nAmAMT[K].ToString("###,###,##0") + " ";
                    }
                    else if (K >= 51 && K <= 55)
                    {
                        SS2_Sheet1.Cells[13 - 1, ((K - 50) * 2) - 1].Text = nAmAMT[K].ToString("###,###,##0") + " ";
                    }
                    else if (K >= 56 && K <= 60)
                    {
                        SS2_Sheet1.Cells[14 - 1, ((K - 55) * 2) - 1].Text = nAmAMT[K].ToString("###,###,##0") + " ";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[K - 1, 1].Text = nAmAMT[K].ToString("###,###,##0") + " ";
                    }
                }
                SS2_Sheet1.Cells[10, 3].Text = nTot1.ToString("###,###,##0");
                SS2_Sheet1.Cells[10, 7].Text = nTot1.ToString("###,###,##0");

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtOpCode_KeyDown(object sender, KeyEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtOpCode.Text = VB.UCase(txtOpCode.Text);

                    if (rdoOptGB0.Checked == true)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK                  ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS     ";
                        SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + txtOpCode.Text + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            txtOpCode.Text = "";
                            txtOpCode.Select();
                            txtOpCode.Enabled = true;
                            txtOpName.Text = "";
                            btnView.Enabled = true;
                            btnExit.Enabled = false;
                            btnPrint.Enabled = false;
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("상병코드가 없습니다.");
                            dt.Dispose();
                            dt = null;
                            return;
                        }
                        else
                        {
                            txtOpName.Text = dt.Rows[0]["IllNamek"].ToString().Trim();
                            SendKeys.Send("{Tab}");
                        }
                    }
                    else if (rdoOptGB1.Checked == true)
                    {

                        txtOpCode.Text = VB.UCase(txtOpCode.Text);

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT SUNAMEK                  ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN     ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SUNEXT = '" + txtOpCode.Text + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            txtOpCode.Text = "";
                            txtOpCode.Select();
                            txtOpCode.Enabled = true;
                            txtOpName.Text = "";
                            btnView.Enabled = true;
                            btnExit.Enabled = false;
                            btnPrint.Enabled = false;
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("수가가 없습니다.");
                            dt.Dispose();
                            dt = null;
                            return;
                        }
                        else
                        {
                            txtOpName.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                            SendKeys.Send("{Tab}");
                        }

                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtOpCode_Leave(object sender, EventArgs e)
        {
            //string SQL = "";
            //DataTable dt = null;
            //string SqlErr = "";
            //int i = 0;

            //Cursor.Current = Cursors.WaitCursor;
            //try
            //{


            //    txtOpCode.Text = VB.UCase(txtOpCode.Text);

            //    if (rdoOptGB0.Checked == true)
            //    {
            //        SQL = "";
            //        SQL = SQL + ComNum.VBLF + " SELECT IllCode, IllNameK                  ";
            //        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS     ";
            //        SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + txtOpCode.Text + "' ";

            //        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //        if (SqlErr != "")
            //        {
            //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //            return;
            //        }
            //        if (dt.Rows.Count == 0)
            //        {
            //            txtOpCode.Text = "";
            //            txtOpCode.Select();
            //            txtOpCode.Enabled = true;
            //            txtOpName.Text = "";
            //            btnView.Enabled = true;
            //            btncansel.Enabled = false;
            //            btnPrint.Enabled = false;
            //            Cursor.Current = Cursors.Default;
            //            ComFunc.MsgBox("상병코드가 없습니다.");
            //            dt.Dispose();
            //            dt = null;
            //            return;
            //        }
            //        else
            //        {
            //            txtOpName.Text = dt.Rows[0]["IllNamek"].ToString().Trim();
            //            //SendKeys.Send("{Tab}");
            //        }
            //    }
            //    else if (rdoOptGB1.Checked == true)
            //    {
            //        txtOpCode.Text = VB.UCase(txtOpCode.Text);

            //        SQL = "";
            //        SQL = SQL + ComNum.VBLF + " SELECT SUNAMEK                  ";
            //        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN     ";
            //        SQL = SQL + ComNum.VBLF + "  WHERE SUNEXT = '" + txtOpCode.Text + "' ";

            //        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //        if (SqlErr != "")
            //        {
            //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //            return;
            //        }
            //        if (dt.Rows.Count == 0)
            //        {
            //            txtOpCode.Text = "";
            //            txtOpCode.Select();
            //            txtOpCode.Enabled = true;
            //            txtOpName.Text = "";
            //            btnView.Enabled = true;
            //            btncansel.Enabled = false;
            //            btnPrint.Enabled = false;
            //            Cursor.Current = Cursors.Default;
            //            ComFunc.MsgBox("수가가 없습니다.");
            //            dt.Dispose();
            //            dt = null;
            //            return;
            //        }
            //        else
            //        {
            //            txtOpName.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
            //            //SendKeys.Send("{Tab}");
            //        }
            //    }
            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    if (dt != null)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        Cursor.Current = Cursors.Default;
            //    }

            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //}
        }



    }
}

