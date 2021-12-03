using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-03-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\nurse\nrer\FrmSimsaDirect.frm" >> frmSimsaDirect.cs 폼이름 재정의" />

    public partial class frmSimsaDirect : Form
    {

        string FstrROWID = "";
        string FstrSabun = "";
        string FstrSendate = "";
        string FstrBUSE = "";
        string FstrWKBUSE = "";

        public frmSimsaDirect()
        {
            InitializeComponent();
        }

        private void frmSimsaDirect_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboGUBUN.Items.Clear();
            cboGUBUN.Items.Add("전체");
            cboGUBUN.Items.Add("미확인");
            cboGUBUN.Items.Add("확인");
            cboGUBUN.SelectedIndex = 0;


            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT BUSE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST WHERE SABUN = '" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrBUSE = dt.Rows[0]["Buse"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                dtpSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-7);
                dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));


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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Search();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            btnCancle();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (Del() == false)
            {
                return;
            }
            btnCancle();
            Search();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ss2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btn_mainPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ss3_Sheet1.Cells[0, 0].Text = txtSIMSA.Text;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ss3, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btn_PartSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (PartSave() == false)
            {
                return;
            }
            btnCancle();
            Search();
        }

        private void btnCancle()
        {
            txtSIMSA.Text = "";
            txtWARD.Text = "";
            btnSave.Enabled = false;
            btn_PartSave.Enabled = true;
            btnDelete.Enabled = false;
            FstrROWID = "";
            FstrSendate = "";
            FstrSabun = "";
            FstrWKBUSE = "";

            ss1_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (Save() == false)
            {
                return;
            }

            RePlash();
        }

        private bool PartSave()
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            if (FstrROWID != "")
            {
                ComFunc.MsgBox("작성된 내용이 공란입니다", "확인");
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_SIMSA_ERDIRECT(";
                SQL = SQL + ComNum.VBLF + " SENDDATE, BUCODE, PART, LAV,";
                SQL = SQL + ComNum.VBLF + " SABUN, DC_YN  ) VALUES (";
                SQL = SQL + ComNum.VBLF + " SYSDATE ,'" + FstrBUSE + "','" + txtSIMSA.Text.Replace("'", "`") + "','1', ";
                SQL = SQL + ComNum.VBLF + "'" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "','C' )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private void Search()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            btnCancle();

            ss1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(A.SENDDATE,'YYYY-MM-DD HH24:MI:SS') SENDDATE, B.NAME , A.PART,A.LAV ,A.SABUN,C.KORNAME ,A.BUCODE, TO_CHAR(A.CKDATE,'YYYY-MM-DD') CKDATE,A.ROWID,A.SABUN";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SIMSA_ERDIRECT A ,KOSMOS_PMPA.BAS_BUSE B  ,KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE SENDDATE >= TO_DATE('" + (dtpSDATE.Text) + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + " AND SENDDATE <= TO_DATE('" + (dtpEDATE.Text) + " 23:59','YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + " AND DC_YN='C'  AND A.BUCODE=B.BUCODE AND A.SABUN=C.SABUN AND A.LAV='1' ";

                switch (cboGUBUN.Text)
                {
                    case "전체":
                        break;
                    case "확인":
                        SQL = SQL + ComNum.VBLF + "    AND CKDATE IS NOT NULL ";
                        break;
                    case "미확인":
                        SQL = SQL + ComNum.VBLF + "    AND CKDATE  IS NULL ";
                        break;
                }
                SQL = SQL + ComNum.VBLF + "   ORDER BY A.SENDDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.Rows.Count = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SENDDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CKDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PART"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BUCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                    }
                }
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

        private bool Del()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            if (FstrROWID == "")
            {
                ComFunc.MsgBox("삭제할 내용을 선택하세요.");
                return rtnVal;
            }

            if (FstrSabun != ComFunc.LPAD(clsType.User.Sabun, 5, "0"))
            {
                ComFunc.MsgBox("작성자만 삭제 할수 있습니다.");
                return rtnVal;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_SIMSA_ERDIRECT SET DC_YN='D' ";
                SQL = SQL + ComNum.VBLF + " WHERE senddate = TO_DATE('" + FstrSendate + "','YYYY-MM-DD HH24:MI:SS') ";
                SQL = SQL + ComNum.VBLF + " AND LAV = '1' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private bool Save()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            if (FstrROWID == "")
            {
                return rtnVal;
            }

            if (txtWARD.Text.Trim() == "")
            {
                MessageBox.Show("작성할 내용이 공란입니다", "확인");
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_SIMSA_ERDIRECT(";
                SQL = SQL + ComNum.VBLF + " SENDDATE, BUCODE, POPUP, LAV,";
                SQL = SQL + ComNum.VBLF + " SABUN, DC_YN ,CKDATE ) VALUES (";
                SQL = SQL + ComNum.VBLF + " TO_DATE('" + FstrSendate + "','YYYY-MM-DD HH24:MI:SS')  ,'" + FstrBUSE + "','" + txtWARD.Text.Replace("'", "`") + "','2', ";
                SQL = SQL + ComNum.VBLF + "'" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "','C',SYSDATE )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (FstrWKBUSE != FstrBUSE)
                {
                    SQL = "";
                    SQL = " UPDATE   KOSMOS_PMPA.NUR_SIMSA_ERDIRECT SET CKDATE=SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE senddate = TO_DATE('" + FstrSendate + "','YYYY-MM-DD HH24:MI:SS') ";
                    SQL = SQL + ComNum.VBLF + " AND LAV = '1' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private void RePlash()
        {
            int i = 0;
            int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            ss2_Sheet1.RowCount = 0;

            txtSIMSA.Text = "";
            txtWARD.Text = "";

            if (FstrROWID == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT  SENDDATE ,PART,KORNAME,A.SABUN,POPUP,BUCODE,TO_CHAR(CKDATE,'MM-DD HH24:MI') CKDATE,A.ROWID,'1' lav  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SIMSA_ERDIRECT A,KOSMOS_ADM.INSA_MST B ";
                SQL = SQL + ComNum.VBLF + " WHERE senddate =  TO_DATE('" + FstrSendate + "','YYYY-MM-DD HH24:MI:SS') and dc_yn='C' ";
                SQL = SQL + ComNum.VBLF + " AND A.SABUN=B.SABUN ";
                SQL = SQL + ComNum.VBLF + " AND  LAV ='1'  union all ";
                SQL = SQL + ComNum.VBLF + " SELECT  SENDDATE ,PART,KORNAME,A.SABUN,POPUP,BUCODE,TO_CHAR(CKDATE,'MM-DD HH24:MI') CKDATE ,A.ROWID, '2' lav  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SIMSA_ERDIRECT A,KOSMOS_ADM.INSA_MST B ";
                SQL = SQL + ComNum.VBLF + " WHERE senddate = TO_DATE('" + FstrSendate + "','YYYY-MM-DD HH24:MI:SS') and dc_yn='C'  ";
                SQL = SQL + ComNum.VBLF + " AND A.SABUN=B.SABUN ";
                SQL = SQL + ComNum.VBLF + " AND LAV ='2'   order by lav,ckdate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtSIMSA.Text = dt.Rows[0]["PART"].ToString().Trim();
                    btnSave.Enabled = true;
                    btn_PartSave.Enabled = false;

                    ss2_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BUCODE"].ToString().Trim() == "078201")
                        {
                            ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CKDATE"].ToString().Trim() + " " + dt.Rows[i]["KORNAME"].ToString().Trim();
                            ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                            ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["POPUP"].ToString().Trim();
                            ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        }
                        else
                        {
                            ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["POPUP"].ToString().Trim();
                            ss2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                            ss2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["POPUP"].ToString().Trim();
                            ss2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CKDATE"].ToString().Trim() + " " + dt.Rows[i]["KORNAME"].ToString().Trim();
                            ss2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        }
                    }
                }

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int i = 0;
            int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int nRead = 0;

            Cursor.Current = Cursors.WaitCursor;

            if (ss1_Sheet1.RowCount == 0)
            {
                return;
            }

            ss2_Sheet1.RowCount = 0;

            FstrSendate = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();
            FstrWKBUSE = ss1_Sheet1.Cells[e.Row, 5].Text.Trim();
            FstrROWID = ss1_Sheet1.Cells[e.Row, 6].Text.Trim();
            FstrSabun = ss1_Sheet1.Cells[e.Row, 7].Text.Trim();

            txtSIMSA.Text = "";
            txtWARD.Text = "";
            btn_PartSave.Enabled = false;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            if (FstrROWID == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT  SENDDATE ,PART,KORNAME,A.SABUN,POPUP,BUCODE,TO_CHAR(CKDATE,'MM/DD HH24:MI') CKDATE,A.ROWID,'1' lav  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_SIMSA_ERDIRECT A," + ComNum.DB_ERP + "INSA_MST B ";
                SQL = SQL + ComNum.VBLF + " WHERE senddate =  TO_DATE('" + FstrSendate + "','YYYY-MM-DD HH24:MI:SS') and dc_yn='C'  ";
                SQL = SQL + ComNum.VBLF + " AND A.SABUN=B.SABUN ";
                SQL = SQL + ComNum.VBLF + " AND  LAV ='1'  union all ";
                SQL = SQL + ComNum.VBLF + " SELECT  SENDDATE ,PART,KORNAME,A.SABUN,POPUP,BUCODE,TO_CHAR(CKDATE,'MM/DD HH24:MI') CKDATE ,A.ROWID, '2' lav  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_SIMSA_ERDIRECT A," + ComNum.DB_ERP + "INSA_MST B ";
                SQL = SQL + ComNum.VBLF + " WHERE senddate = TO_DATE('" + FstrSendate + "','YYYY-MM-DD HH24:MI:SS') and dc_yn='C'  ";
                SQL = SQL + ComNum.VBLF + " AND A.SABUN=B.SABUN ";
                SQL = SQL + ComNum.VBLF + " AND LAV ='2'   order by lav,ckdate ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                txtSIMSA.Text = dt.Rows[0]["PART"].ToString().Trim();
                btnSave.Enabled = true;
                btn_PartSave.Enabled = false;
                btnDelete.Enabled = true;

                nRead = dt.Rows.Count;

                ss2_Sheet1.RowCount = nRead - 1;

                for (i = 1; i < nRead; i++)
                {
                    if (dt.Rows[i]["BUCODE"].ToString().Trim() == "078201")
                    {
                        ss2_Sheet1.Cells[i - 1, 0].Text = dt.Rows[i]["CKDATE"].ToString().Trim() + " " + dt.Rows[i]["KORNAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i - 1, 2].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ss2_Sheet1.Cells[i - 1, 3].Text = dt.Rows[i]["POPUP"].ToString().Trim();
                        ss2_Sheet1.Cells[i - 1, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                    else
                    {
                        ss2_Sheet1.Cells[i - 1, 5].Text = dt.Rows[i]["POPUP"].ToString().Trim();
                        ss2_Sheet1.Cells[i - 1, 7].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ss2_Sheet1.Cells[i - 1, 8].Text = dt.Rows[i]["POPUP"].ToString().Trim();
                        ss2_Sheet1.Cells[i - 1, 8].Text = dt.Rows[i]["CKDATE"].ToString().Trim() + " " + dt.Rows[i]["KORNAME"].ToString().Trim();
                        ss2_Sheet1.Cells[i - 1, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

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

        private void ss2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            if (ss2_Sheet1.RowCount > 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("선택한 내용을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                switch (e.Column)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        if (ss2_Sheet1.Cells[e.Row, 2].Text != ComFunc.LPAD(clsType.User.Sabun, 5, "0"))
                        {
                            MessageBox.Show("작성자만 삭제 할 수 있습니다.");
                            return;
                        }

                        SQL = "";
                        SQL = " UPDATE   KOSMOS_PMPA.NUR_SIMSA_ERDIRECT SET dc_yn='D' ";
                        SQL = SQL + ComNum.VBLF + " WHERE rowid ='" + ss2_Sheet1.Cells[e.Row, 4].Text.Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        break;


                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        if (ss2_Sheet1.Cells[e.Row, 7].Text != ComFunc.LPAD(clsType.User.Sabun, 5, "0"))
                        {
                            MessageBox.Show("작성자만 삭제 할 수 있습니다.");
                            return;
                        }

                        SQL = "";
                        SQL = " UPDATE   KOSMOS_PMPA.NUR_SIMSA_ERDIRECT SET dc_yn='D' ";
                        SQL = SQL + ComNum.VBLF + " WHERE rowid ='" + ss2_Sheet1.Cells[e.Row, 9].Text.Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        break;
                }
                RePlash();
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
    }
}
