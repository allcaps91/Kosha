using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewJaboAgree.cs
    /// Description     : 자동차보험 진료수가 지급청구검토 동의서 - 청구기준
    /// Author          : 박창욱
    /// Create Date     : 2017-10-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUT300.frm(FrmAgree.frm) >> frmPmpaViewJaboAgree.cs 폼이름 재정의" />	
    public partial class frmPmpaViewJaboAgree : Form
    {
        string FstrRowid = "";
        string FstrWrtno = "";
        int FnAccNum = 0;

        public frmPmpaViewJaboAgree()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            FstrRowid = "";

            ssView_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 5;

            Screen_Clear2();

            btnRegist.Enabled = false;
            btnDelete.Enabled = false;
            btnPrint.Enabled = false;
        }

        void Screen_Clear2()
        {

            //기본사항
            txtKiho.Text = "";
            txtKihoName.Text = "";
            txtCDate.Text = "";
            txtJDate.Text = "";
            txtNo.Text = "";
            txtSeqno.Text = "";
            txtActDate.Text = "";

            //검토내역
            txtPano.Text = "";
            txtSName.Text = "";
            txtFDate.Text = "";
            txtTDate.Text = "";
            txtAmt.Text = "";
            txtJAmt.Text = "";
            txtIAmt.Text = "";

            //세부내역
            ssView4_Sheet1.Cells[0, 0, ssView4_Sheet1.RowCount - 1, ssView4_Sheet1.ColumnCount - 1].Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strYYMM = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            try
            {
                //청구에서 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.PANO, A.SNAME, A.JAMT,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.FRDATE,'YYYY-MM-DD') FRDATE, TO_CHAR(A.TODATE,'YYYY-MM-DD') TODATE, A.DEPTCODE1,";
                SQL = SQL + ComNum.VBLF + "       A.WRTNO, B.ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_TAID A, " + ComNum.DB_PMPA + "ETC_TA_JMST B ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND A.YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.ZIPCODE1 ='PRT' ";
                if (rdoIO0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDOPD = 'I' ";
                }
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDOPD = 'O' ";
                }
                SQL = SQL + ComNum.VBLF + "    AND A.WRTNO = B.WRTNO(+) ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewJaboAgree_Load(object sender, EventArgs e)
        {
           // if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
           // {
           //      this.Close(); //폼 권한 조회
           //       return;
           //  }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 24, "", "1");
            Screen_Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComFunc.MsgBoxQ("해당 DATA를 삭제하시겠습니까?", "삭제") == DialogResult.Yes)
                {
                    SQL = "";
                    SQL = ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "ETC_TA_JMST WHERE ACCNUM = '" + FnAccNum + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "ETC_TA_JdtL WHERE ACCNUM = '" + FnAccNum + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("삭제하였습니다.");

                Screen_Clear();
                Search_Data();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            //TODO : 폼 호출
            //일괄발행작업
            //FrmAgreePRT.Show
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nRow = 0;
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;


            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //DATA MOVE
                ssView2_Sheet1.Cells[2, 6].Text = txtKihoName.Text; //사업장명
                ssView2_Sheet1.Cells[3, 6].Text = "(" + txtKiho.Text + ")"; //기호

                ssView2_Sheet1.Cells[5, 0].Text = txtCDate.Text;    //지급청구일
                ssView2_Sheet1.Cells[5, 2].Text = txtJDate.Text;    //검토서접수일
                ssView2_Sheet1.Cells[5, 4].Text = txtNo.Text;       //사고접수번호
                ssView2_Sheet1.Cells[5, 6].Text = txtSeqno.Text;    //청구서일련번호

                ssView2_Sheet1.Cells[8, 0].Text = txtSName.Text;    //환자명
                ssView2_Sheet1.Cells[8, 1].Text = txtFDate.Text + "~" + txtTDate.Text;  //진료기간
                ssView2_Sheet1.Cells[8, 3].Text = txtAmt.Text;      //청구액
                ssView2_Sheet1.Cells[8, 5].Text = txtJAmt.Text;     //조정액
                ssView2_Sheet1.Cells[8, 6].Text = txtIAmt.Text;     //인정가능액

                ssView2_Sheet1.Cells[13, 3].Text = VB.Left(txtActDate.Text, 4) + "년 " + VB.Mid(txtActDate.Text, 6, 2) + "월 " +
                                                   VB.Mid(txtActDate.Text, 9, txtActDate.Text.Length) + "일";

                //Clear
                ssView2_Sheet1.Cells[19, 0, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].Text = "";

                nRow = 19;

                for (i = 1; i < ssView4_Sheet1.RowCount; i++)
                {
                    if (ssView4_Sheet1.Cells[i - 1, 7].Text == "")
                    {
                        break;
                    }
                    nRow += 1;
                    ssView2_Sheet1.Cells[nRow - 1, 0].Text = ssView4_Sheet1.Cells[i - 1, 0].Text;
                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = ssView4_Sheet1.Cells[i - 1, 1].Text;
                    ssView2_Sheet1.Cells[nRow - 1, 2].Text = ssView4_Sheet1.Cells[i - 1, 2].Text;
                    ssView2_Sheet1.Cells[nRow - 1, 3].Text = ssView4_Sheet1.Cells[i - 1, 3].Text;
                    ssView2_Sheet1.Cells[nRow - 1, 4].Text = ssView4_Sheet1.Cells[i - 1, 4].Text.Replace("\n", " ").Replace("\r", " ");
                    ssView2_Sheet1.Cells[nRow - 1, 6].Text = ssView4_Sheet1.Cells[i - 1, 5].Text;
                    ssView2_Sheet1.Cells[nRow - 1, 7].Text = ssView4_Sheet1.Cells[i - 1, 6].Text;
                }

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 100, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "ETC_TA_JMST SET PRTDATE = SYSDATE  WHERE ACCNUM = '" + FnAccNum + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nMaxAccNum = 0;
            double nCamt = 0;
            double nJamt = 0;
            double nIAmt = 0;
            string strROWID = "";
            string strIpdOpd = "";
            string strRemark = "";
            string strBun = "";
            string strJcode = "";
            string strAgree = "";
            string strYYMM = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FnAccNum == 0)
                {
                    //nmaxaccnum 읽기
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT MAX(ACCNUM) MACCNUM FROM " + ComNum.DB_PMPA + "ETC_TA_JMST ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nMaxAccNum = (int)VB.Val(dt.Rows[0]["MACCNUM"].ToString().Trim()) + 1;
                }

                if (rdoIO0.Checked == true)
                {
                    strIpdOpd = "I";
                }
                if (rdoIO1.Checked == true)
                {
                    strIpdOpd = "O";
                }

                //ETC_TA_JMST
                SQL = "";
                if (FnAccNum == 0) //신규자료
                {
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_TA_JMST ( ACCNUM, ACTDATE, WRTNO, YYMM, IPDOPD, PANO, SNAME, ";
                    SQL = SQL + ComNum.VBLF + "                                                 CDATE, JDATE, TNO, CAMT, JAMT, IAMT, ENTDATE, PRTDATE) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ( " + nMaxAccNum + ", TRUNC(SYSDATE), '" + FstrWrtno + "', ";
                    SQL = SQL + ComNum.VBLF + "          '" + strYYMM + "', '" + strIpdOpd + "', '" + txtPano.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "          '" + txtSName.Text + "', TO_DATE('" + txtCDate.Text + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "          TO_DATE('" + txtJDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "          '" + txtNo.Text + "', '" + VB.Val(txtAmt.Text).ToString("#########0") + "', '" + VB.Val(txtJAmt.Text).ToString("#########0") + "', ";
                    SQL = SQL + ComNum.VBLF + "          '" + VB.Val(txtIAmt.Text).ToString("##########0") + "', SYSDATE, '') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_TA_JMST SET ";
                    SQL = SQL + ComNum.VBLF + "  YYMM = '" + strYYMM + "' ,";
                    SQL = SQL + ComNum.VBLF + "  IPDOPD = '" + strIpdOpd + "', ";
                    SQL = SQL + ComNum.VBLF + "  PANO = '" + txtPano.Text + "',";
                    SQL = SQL + ComNum.VBLF + "  SNAME = '" + txtSName.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "  CDATE = TO_DATE('" + txtCDate.Text + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "  JDATE = TO_DATE('" + txtJDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "  TNO = '" + txtNo.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "  CAMT = '" + VB.Val(txtAmt.Text).ToString("#########0") + "',";
                    SQL = SQL + ComNum.VBLF + "  JAMT = '" + VB.Val(txtJAmt.Text).ToString("#########0") + "', ";
                    SQL = SQL + ComNum.VBLF + "  IAMT = '" + VB.Val(txtIAmt.Text).ToString("#########0") + "', ";
                    SQL = SQL + ComNum.VBLF + "  ENTDATE = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ACCNUM = " + FnAccNum + "  ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //ETC_TA_JdtL
                for (i = 1; i < ssView4_Sheet1.RowCount; i++)
                {
                    strBun = ssView4_Sheet1.Cells[i - 1, 0].Text;
                    nCamt = VB.Val(ssView4_Sheet1.Cells[i - 1, 1].Text);
                    nJamt = VB.Val(ssView4_Sheet1.Cells[i - 1, 2].Text);
                    nIAmt = VB.Val(ssView4_Sheet1.Cells[i - 1, 3].Text);
                    strRemark = ssView4_Sheet1.Cells[i - 1, 4].Text;
                    strJcode = ssView4_Sheet1.Cells[i - 1, 5].Text;
                    strAgree = ssView4_Sheet1.Cells[i - 1, 6].Text;
                    strROWID = ssView4_Sheet1.Cells[i - 1, 7].Text;
                    if (FnAccNum != 0)
                    {
                        nMaxAccNum = FnAccNum;
                    }
                    if (strROWID == "") //신규자료
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_TA_JdtL(ACCNUM, BUN, CAMT, JAMT, IAMT, ";
                        SQL = SQL + ComNum.VBLF + "                                               REMARK, JCODE, AGREE, ENTDATE) ";
                        SQL = SQL + ComNum.VBLF + " VALUES ( " + nMaxAccNum + ", '" + strBun + "','" + nCamt + "', '" + nJamt + "', ";
                        SQL = SQL + ComNum.VBLF + "          '" + nIAmt + "','" + strRemark + "','" + strJcode + "', '" + strAgree + "', SYSDATE) ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_TA_JdtL SET ";
                        SQL = SQL + ComNum.VBLF + " BUN = '" + strBun + "', ";
                        SQL = SQL + ComNum.VBLF + " CAMT = '" + nCamt + "', ";
                        SQL = SQL + ComNum.VBLF + " JAMT = '" + nJamt + "', ";
                        SQL = SQL + ComNum.VBLF + " IAMT = '" + nIAmt + "', ";
                        SQL = SQL + ComNum.VBLF + " REMARK = '" + strRemark + "',";
                        SQL = SQL + ComNum.VBLF + " JCODE = '" + strJcode + "', ";
                        SQL = SQL + ComNum.VBLF + " AGREE = '" + strAgree + "',";
                        SQL = SQL + ComNum.VBLF + " ENTDATE = SYSDATE ";
                        SQL = SQL + ComNum.VBLF + " WHERE ACCNUM = " + FnAccNum + "  ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("등록하였습니다.");

                Screen_Clear();
                Search_Data();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;

            FnAccNum = 0;
            FstrWrtno = "";
            FstrRowid = "";

            if (e.Row == -1 || e.Column == -1)
            {
                return;
            }

            FstrWrtno = ssView_Sheet1.Cells[e.Row, 4].Text;

            Screen_Clear2();

            try
            {
                //발행내역
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, PANO, SNAME,";
                SQL = SQL + ComNum.VBLF + "       CAMT, JAMT, IAMT,";
                SQL = SQL + ComNum.VBLF + "       ACCNUM, WRTNO, TO_CHAR(CDATE,'YYYY-MM-DD') CDATE,";
                SQL = SQL + ComNum.VBLF + "       TNO, ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_TA_JMST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + FstrWrtno + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssView3_Sheet1.RowCount = 0;
                ssView3_Sheet1.RowCount = dt.Rows.Count;
                ssView3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["IAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ACCNUM"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CDATE"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                nRead = dt.Rows.Count;
                dt.Dispose();
                dt = null;

                if (nRead == 0)
                {
                    //신규자료 READ
                    //기본사항, 검토내역
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT A.SEQNO, TO_CHAR(A.FRDATE,'YYYY-MM-DD') FRDATE, TO_CHAR(A.TODATE,'YYYY-MM-DD') TODATE,  ";
                    SQL = SQL + ComNum.VBLF + "        A.GELCODE, A.PANO, A.SNAME,";
                    SQL = SQL + ComNum.VBLF + "        A.JAMT, A.WRTNO, B.MIANAME,";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR( C.BDATE ,'YYYY-MM-DD') BDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_TAID A, " + ComNum.DB_PMPA + "BAS_MIA B, " + ComNum.DB_PMPA + "MISU_IDMST C ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND A.WRTNO = '" + FstrWrtno + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.GELCODE = B.MIACODE(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = C.MISUID(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND A.YYMM = C.MIRYYMM(+) ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    //기본사항
                    txtKiho.Text = dt.Rows[0]["GELCODE"].ToString().Trim();
                    txtKihoName.Text = dt.Rows[0]["MIANAME"].ToString().Trim();
                    txtCDate.Text = dt.Rows[0]["BDATE"].ToString().Trim();
                    txtJDate.Text = "";
                    txtNo.Text = "";
                    txtSeqno.Text = dt.Rows[0]["WRTNO"].ToString().Trim();
                    txtActDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

                    //검토내역
                    txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtFDate.Text = dt.Rows[0]["FRDATE"].ToString().Trim();
                    txtTDate.Text = dt.Rows[0]["TODATE"].ToString().Trim();
                    txtAmt.Text = VB.Val(dt.Rows[0]["JAMT"].ToString().Trim()).ToString("###,###,###,0");
                    txtJAmt.Text = "";
                    txtIAmt.Text = "";

                    dt.Dispose();
                    dt = null;

                    ssView4_Sheet1.Cells[0, 0, ssView4_Sheet1.RowCount - 1, ssView4_Sheet1.ColumnCount - 1].Text = "";

                    btnRegist.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView3_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (ssView3_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView3_Sheet1.Cells[0, 0, ssView3_Sheet1.RowCount - 1, ssView3_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView3_Sheet1.Cells[e.Row, 0, e.Row, ssView3_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;


            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strROWID = "";

            FnAccNum = 0;

            txtActDate.Text = ssView3_Sheet1.Cells[e.Row, 0].Text;
            txtPano.Text = ssView3_Sheet1.Cells[e.Row, 1].Text;
            txtAmt.Text = VB.Val(ssView3_Sheet1.Cells[e.Row, 3].Text).ToString("###,###,###,##0");
            txtJAmt.Text = VB.Val(ssView3_Sheet1.Cells[e.Row, 4].Text).ToString("###,###,###,##0");
            txtIAmt.Text = VB.Val(ssView3_Sheet1.Cells[e.Row, 5].Text).ToString("###,###,###,##0");
            FnAccNum = (int)VB.Val(ssView3_Sheet1.Cells[e.Row, 6].Text);
            strROWID = ssView3_Sheet1.Cells[e.Row, 8].Text;

            try
            {
                //기본사항, 검토내역
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.ACCNUM, A.ACTDATE, A.WRTNO,";
                SQL = SQL + ComNum.VBLF + "       A.PANO, A.SNAME, TO_CHAR(A.CDATE,'YYYY-MM-DD') CDATE,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE, A.TNO, A.CAMT,";
                SQL = SQL + ComNum.VBLF + "       A.JAMT, A.IAMT, A.WRTNO,";
                SQL = SQL + ComNum.VBLF + "       B.SEQNO, TO_CHAR(B.FRDATE,'YYYY-MM-DD') FRDATE, TO_CHAR(B.TODATE,'YYYY-MM-DD') TODATE,";
                SQL = SQL + ComNum.VBLF + "       B.GELCODE,  C.MIANAME,TO_CHAR( D.BDATE ,'YYYY-MM-DD') BDATE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_TA_JMST A, " + ComNum.DB_PMPA + "MIR_TAID B, ";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_MIA C, " + ComNum.DB_PMPA + "MISU_IDMST D ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND ACCNUM = " + FnAccNum + " ";
                SQL = SQL + ComNum.VBLF + "   AND A.WRTNO = B.WRTNO ";
                SQL = SQL + ComNum.VBLF + "   AND B.GELCODE = C.MIACODE(+) ";
                SQL = SQL + ComNum.VBLF + "   AND B.PANO = D.MISUID(+) ";
                SQL = SQL + ComNum.VBLF + "   AND B.YYMM = D.MIRYYMM(+) ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                //기본사항
                txtKiho.Text = dt.Rows[0]["GELCODE"].ToString().Trim();
                txtKihoName.Text = dt.Rows[0]["MIANAME"].ToString().Trim();
                txtCDate.Text = dt.Rows[0]["BDATE"].ToString().Trim();
                txtJDate.Text = dt.Rows[0]["JDATE"].ToString().Trim();
                txtNo.Text = dt.Rows[0]["TNO"].ToString().Trim();
                txtSeqno.Text = dt.Rows[0]["WRTNO"].ToString().Trim();

                //검토내역
                txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                txtFDate.Text = dt.Rows[0]["FRDATE"].ToString().Trim();
                txtTDate.Text = dt.Rows[0]["TODATE"].ToString().Trim();
                txtAmt.Text = VB.Val(dt.Rows[0]["CAMT"].ToString().Trim()).ToString("###,###,###,##0");
                txtJAmt.Text = VB.Val(dt.Rows[0]["JAMT"].ToString().Trim()).ToString("###,###,###,##0");
                txtIAmt.Text = VB.Val(dt.Rows[0]["IAMT"].ToString().Trim()).ToString("###,###,###,##0");

                dt.Dispose();
                dt = null;

                //상세내역 READ
                ssView4_Sheet1.Cells[0, 0, ssView4_Sheet1.RowCount - 1, ssView4_Sheet1.ColumnCount - 1].Text = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BUN, CAMT, JAMT,";
                SQL = SQL + ComNum.VBLF + "       IAMT, REMARK, JCODE,";
                SQL = SQL + ComNum.VBLF + "       AGREE, ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_TA_JdtL ";
                SQL = SQL + ComNum.VBLF + " WHERE ACCNUM = " + FnAccNum + " ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView4_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    ssView4_Sheet1.Cells[i, 1].Text = VB.Val(dt.Rows[i]["CAMT"].ToString().Trim()).ToString("###,###,###,###");
                    if (string.Compare(dt.Rows[i]["CAMT"].ToString().Trim(), "0") > 0)
                    {
                        ssView4_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()).ToString("###,###,###,###");
                        ssView4_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["IAMT"].ToString().Trim()).ToString("###,###,###,###");
                    }
                    else
                    {
                        ssView4_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()).ToString("###,###,###,###");
                        ssView4_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["IAMT"].ToString().Trim()).ToString("###,###,###,###");
                    }
                    ssView4_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    ssView4_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JCODE"].ToString().Trim();
                    ssView4_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AGREE"].ToString().Trim();
                    ssView4_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                btnDelete.Enabled = true;
                btnRegist.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void ssView4_Change(object sender, ChangeEventArgs e)
        {
            if (e.Column == 2)
            {
                double nCamt = 0;
                double nJamt = 0;

                nCamt = VB.Val(ssView4_Sheet1.Cells[e.Row, 1].Text);
                nJamt = VB.Val(ssView4_Sheet1.Cells[e.Row, 2].Text);
                ssView4_Sheet1.Cells[e.Row, 3].Text = (nCamt - nJamt).ToString();
            }
        }

        private void txtIAmt_TextChanged(object sender, EventArgs e)
        {
            txtIAmt.Text = VB.Val(txtIAmt.Text).ToString("###,###,###,##0");
        }

        private void txtJAmt_TextChanged(object sender, EventArgs e)
        {
            txtIAmt.Text = (VB.Val(txtAmt.Text.Replace(",", "")) - VB.Val(txtJAmt.Text.Replace(",", ""))).ToString("###,###,###,##0");
            txtJAmt.Text = VB.Val(txtJAmt.Text.Replace(",", "")).ToString("###,###,###,##0");
        }
    }
}
