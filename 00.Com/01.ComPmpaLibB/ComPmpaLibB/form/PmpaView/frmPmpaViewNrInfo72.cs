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
    /// Create Date     : 2017-10-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\nrinfo.vbp\(NrInfo72.frm)" >> frmPmpaViewNrInfo72.cs 폼이름 재정의" />
    /// <seealso cref= D:\psmh\IPD\iviewa\iviewa.vbp\(IVIEWAC.frm)" >> frmPmpaViewNrInfo72.cs 폼이름 재정의" />

    public partial class frmPmpaViewNrInfo72 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string strSuNext = "";
        string strDate = "";
        string strSwPano = "";
        string strSwSname = "";
        string strSwDate = "";
        /// <summary>
        /// Flag : "1" = iviewa.vbp , "2" = nrinfo.vbp
        /// </summary>
        string Flag = "1";

        public frmPmpaViewNrInfo72()
        {
            InitializeComponent();
        }

        public frmPmpaViewNrInfo72(string strFlag)
        {
            Flag = strFlag;

            InitializeComponent();
        }

        private void frmPmpaViewNrInfo72_Load(object sender, System.EventArgs e)
        {

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lblSname.Text = "";
            btnNext.Enabled = false;
            btnPrint.Enabled = false;
            btnSearch.Enabled = false;

            Flag = "2";

            dtpFdate.Value = (Convert.ToDateTime(strDTP)).AddDays(-2);
            dtpTdate.Value = Convert.ToDateTime(strDTP);

            if (Flag == "1")
            {
                ssView_Sheet1.Columns[1].Visible = true;
                ssView_Sheet1.Columns[2].Visible = true;
                ssView_Sheet1.Columns[7].Visible = true;
                ssView_Sheet1.Columns[9].Visible = true;
            }
            else
            {
                ssView_Sheet1.Columns[1].Visible = false;
                ssView_Sheet1.Columns[2].Visible = false;
                ssView_Sheet1.Columns[7].Visible = false;
                ssView_Sheet1.Columns[9].Visible = false;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, ssView_Sheet1.ColumnCount - 1, 0, ssView_Sheet1.ColumnCount - 1].Text = "";

            txtpano.Text = "";
            dtpFdate.Value = (Convert.ToDateTime(strDTP)).AddDays(-2);
            dtpTdate.Value = Convert.ToDateTime(strDTP);
            lblSname.Text = "";

            btnNext.Enabled = false;
            btnPrint.Enabled = false;
            btnSearch.Enabled = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComFunc.MsgBoxQ("인쇄를 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) != System.Windows.Forms.DialogResult.No)
            {
                strTitle = "입원환자 II과 수납내역";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            progressBar1.Value = 0;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,TO_CHAR(ActDate,'yyyy-mm-dd') ActDate,           ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(Bdate,'yyyy-mm-dd') Bdate,DeptCode,Bi,SuNext,   ";
                SQL = SQL + ComNum.VBLF + "        Qty,Nal,BaseAmt,Amt1,Amt2,Part,SeqNo,WardCode, GbIpd, rowid   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                                 ";

                //If OptionJob(0).Value = True Then
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + txtpano.Text + "'                                                           ";
                if (dtpFdate.Value.ToString("yyyy-MM-dd") != "")
                    SQL = SQL + ComNum.VBLF + "    AND ActDate >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')    ";
                if (dtpTdate.Value.ToString("yyyy-MM-dd") != "")
                    SQL = SQL + ComNum.VBLF + "    AND ActDate <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','yyyy-mm-dd')    ";
                SQL = SQL + ComNum.VBLF + "    AND Nu < 51                                                                                 ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Pano,Bdate,Sunext                                                                    ";
                //End If

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

                strSwPano = "";
                strSwDate = "";

                if (Flag == "1")
                {
                    ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "등록번호";
                    ssView_Sheet1.ColumnHeader.Cells[0, 1].Text = "성  명";
                    ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "처방일자";

                    strSwPano = Convert.ToInt32(dt.Rows[0]["Pano"].ToString().Trim()).ToString("00000000");
                    strSwSname = lblSname.Text;
                    strSwDate = "";
                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                progressBar1.Maximum = ssView_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    OneRow_Display1(dt, i);
                    progressBar1.Value = (i + 1);
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = false;
                btnNext.Enabled = true;
                btnPrint.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 1건의 내용을 Sheet에 Display함 (개인별,일자별)
        /// </summary>
        private void OneRow_Display1(DataTable dt, int i)
        {
            strSuNext = dt.Rows[i]["SuNext"].ToString().Trim();
            strDate = dt.Rows[i]["Bdate"].ToString().Trim();

            if (Flag == "1")
            {
                ssView_Sheet1.Cells[i, 0].Text = strSwPano;
                ssView_Sheet1.Cells[i, 1].Text = strSwSname;
            }
            if (strSwDate != strDate)
            {
                if (Flag == "1")
                {
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                }
                else
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                }
                ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();

                strSwDate = strDate;
            }
            else
            {

                ssView_Sheet1.Cells[i, 2].Text = "";
                ssView_Sheet1.Cells[i, 3].Text = "";
                ssView_Sheet1.Cells[i, 4].Text = "";

            }

            ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WardCode"].ToString().Trim();
            ssView_Sheet1.Cells[i, 6].Text = strSuNext;
            ssView_Sheet1.Cells[i, 7].Text = Convert.ToDouble(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("#,###,##0");
            ssView_Sheet1.Cells[i, 8].Text = VB.Format(Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim()) * Convert.ToDouble(dt.Rows[i]["Nal"].ToString().Trim()), "##0.0");
            ssView_Sheet1.Cells[i, 9].Text = VB.Format(Convert.ToDouble(dt.Rows[i]["Amt1"].ToString().Trim()) + Convert.ToDouble(dt.Rows[i]["Amt2"].ToString().Trim()), "###,###,##0");
            ssView_Sheet1.Cells[i, 10].Text = SugaRead(strSuNext);

            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["Part"].ToString().Trim();
            if (ssView_Sheet1.Cells[i, 11].Text == "#")
            {
                ssView_Sheet1.Cells[i, 11].Text = "";
            }
            ssView_Sheet1.Cells[i, 12].Text = Convert.ToDouble(dt.Rows[i]["SeqNo"].ToString().Trim()).ToString("###");

            strSwPano = "";
            strSwSname = "";

        }

        #region 함수 모음

        private string SugaRead(string Code)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strRtn = "";
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SuNameK        ";
                ;
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "    AND SuNext = '" + Code + "'   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strRtn;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return strRtn;
                }
                if (dt.Rows.Count == 1)
                {
                    strRtn = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                }
                else
                {
                    strRtn = "-<ERROR>-";
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return strRtn;
            }
            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strRtn;
            }
        }

        #endregion

        private void txtpano_Leave(object sender, EventArgs e)
        {

        }

        private void dtpTdate_Enter(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
        }

        private void txtpano_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (txtpano.Text == "")
            {
                return;
            }
            txtpano.Text = Convert.ToInt32(txtpano.Text).ToString("00000000");

            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //'입원 Master에서 환자 찾음
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Sname,TO_CHAR(InDate,'YYYY-MM-DD') IDate   ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                 ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                    SQL = SQL + ComNum.VBLF + "    AND Pano = '" + txtpano.Text + "'              ";

                    if (Flag == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND GBSTS IN ('0','2')";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE IS NULL ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND GBSTS IN ('0' ,'2', '3', '4') ";
                        SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND OUTDATE IS NULL ";
                    }
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    if (dt.Rows.Count == 1)
                    {
                        lblSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        dtpFdate.Value = Convert.ToDateTime(dt.Rows[0]["IDate"].ToString().Trim());
                        dtpTdate.Value = Convert.ToDateTime(strDTP);
                        btnSearch.Enabled = true;
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                    //' 퇴원 Master에서 환자 찾음
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Sname,TO_CHAR(InDate,'YYYY-MM-DD') IDate,    ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(OutDate,'YYYY-MM-DD') ODate          ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                               ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1  ";
                    SQL = SQL + ComNum.VBLF + "    AND Pano = '" + txtpano.Text + "'                ";
                    SQL = SQL + ComNum.VBLF + "    AND OUTDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY InDate DESC                               ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("자료가 없습니다. 확인 하세요.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                    if (dt.Rows.Count >= 1)
                    {
                        lblSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        dtpFdate.Value = Convert.ToDateTime(dt.Rows[0]["IDate"].ToString().Trim());
                        dtpTdate.Value = Convert.ToDateTime(dt.Rows[0]["ODate"].ToString().Trim());
                        btnSearch.Enabled = true;
                    }

                    else
                    {
                        ComFunc.MsgBox("입원, 퇴원 환자가 아닙니다. ", "확인");
                        txtpano.Select();
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    dt.Dispose();
                    dt = null;
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
    }
}
