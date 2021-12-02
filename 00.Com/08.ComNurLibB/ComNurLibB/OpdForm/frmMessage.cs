using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmMessage.cs
    /// Description     : 원무과 메세지 등록
    /// Author          : 박창욱
    /// Create Date     : 2018-01-19
    /// Update History  : 
    /// </summary>
    /// <history>      
    /// </history>
    /// <seealso cref= "\emr\emrprt\frmMess01.frm(FrmMessage.frm) >> frmMessage.cs 폼이름 재정의" />
    public partial class frmMessage : Form
    {
        string FstrPassId = "";
        string strDRCode = "";
        string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        string strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

        public frmMessage()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Screen_Clear()
        {
            txtSDate.Text = "00:00";
            txtEDate.Text = "00:00";
            txtMessage.Text = "";
            txtMessage1.Text = "";
            lblImFor.Text = "";
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;
            btnRegist.Enabled = false;
            panMain.Enabled = false;

            rdoGbn0.Checked = false;
            rdoGbn1.Checked = false;
            rdoGbn2.Checked = false;
            rdoGbn3.Checked = false;
            rdoGbn4.Checked = false;
            rdoGbn5.Checked = false;
            rdoGbn6.Checked = false;
            rdoGbn7.Checked = false;

            cboDept.Focus();
        }

        void Screen_Clear2()
        {
            txtSDate.Text = "00:00";
            txtEDate.Text = "00:00";
            txtMessage.Text = "";
            txtMessage1.Text = "";
            lblImFor.Text = "";
            btnRegist.Enabled = false;
            panMain.Enabled = false;

            cboMess.SelectedIndex = -1;

            rdoGbn0.Checked = false;
            rdoGbn1.Checked = false;
            rdoGbn2.Checked = false;
            rdoGbn3.Checked = false;
            rdoGbn4.Checked = false;
            rdoGbn5.Checked = false;
            rdoGbn6.Checked = false;
            rdoGbn7.Checked = false;

            cboDept.Focus();
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.DRCODE, A.DRNAME,B.GBJIN, B.GBJIN2";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR A, " + ComNum.DB_PMPA + "BAS_SCHEDULE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.DRDEPT1 = '" + VB.Left(cboDept.Text, 2) + "' ";
                if (VB.Left(cboDept.Text, 2) != "MD")
                {
                    //JJY(2005-06-09) 내과는 진료과도 적용함.
                    SQL = SQL + ComNum.VBLF + "   AND A.PRINTRANKING NOT IN ('99')  ";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.TOUR ='N'";
                SQL = SQL + ComNum.VBLF + "   AND A.SCHEDULE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + "   AND B.SCHDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.PRINTRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                    switch (dt.Rows[i]["GBJIN"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:진료";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:수술";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:특검";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:휴진";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:학회";
                            break;
                        case "6":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:휴가";
                            break;
                        case "7":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:출장";
                            break;
                        case "8":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:기타";
                            break;
                        case "9":
                            ssView_Sheet1.Cells[i, 1].Text = "AM:OFF";
                            break;
                    }

                    switch (dt.Rows[i]["GBJIN2"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:진료";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:수술";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:특검";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:휴진";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:학회";
                            break;
                        case "6":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:휴가";
                            break;
                        case "7":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:출장";
                            break;
                        case "8":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:기타";
                            break;
                        case "9":
                            ssView_Sheet1.Cells[i, 1].Text = "PM:OFF";
                            break;
                    }

                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmMessage_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Screen_Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DEPTCODE, DEPTNAMEK";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboDept.Items.Clear();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim() + "    " + dt.Rows[i]["DeptNameK"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT CODE, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_MESSAGE_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN  = '01' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboMess.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                }

                cboMess.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

                if (clsType.User.Sabun == "13386" || clsType.User.Sabun == "4349")
                {
                    btnDelete.Enabled = true;
                }

                ssView_Sheet1.Columns[2].Visible = false;
                ssView_Sheet1.Columns[3].Visible = false;
                ssView_Sheet1.Columns[4].Visible = false;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {            
            frmCode frmCodeX = new frmCode();
            frmCodeX.StartPosition = FormStartPosition.CenterParent;
            frmCodeX.ShowDialog();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strDept = "";
            string strRowId = "";
            string strDrname = "";
            string strBdate = "";

            if (txtSDate.Text.Trim().Length < 5 || txtEDate.Text.Trim().Length < 5)
            {
                ComFunc.MsgBox("휴진시간을 정확한 형식으로 입력해 주세요.");
                txtSDate.Focus();
                return;
            }
            if (txtSDate.Text.Trim() == "00:00" || txtEDate.Text.Trim() == "00:00")
            {
                ComFunc.MsgBox("휴진시간을 정확한 형식으로 입력해 주세요.");
                txtSDate.Focus();
                return;
            }

            strBdate = dtpDate.Value.ToString("yyyy-MM-dd");
            strDRCode = VB.Left(lblImFor.Text, 4);
            strDept = VB.Mid(lblImFor.Text, 6, 2);
            strDrname = VB.Mid(lblImFor.Text, 10, 3);
            strRowId = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_MESSAGE";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + strDRCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strRowId = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (ComFunc.MsgBoxQ(strDrname + "과장님의 메세지를 등록 하겠습니까?") == DialogResult.Yes)
                {
                    if (strRowId == "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_MESSAGE ";
                        SQL = SQL + ComNum.VBLF + " (ACTDATE, ENTDATE, DEPT, DRCODE, SDATE, EDATE, MESSAGE, IDNUMBER, MESSAGE1, MESSAGE2) ";
                        SQL = SQL + ComNum.VBLF + "  VALUES( TO_DATE('" + strBdate + "','YYYY-MM-DD'), sysdate,";
                        SQL = SQL + ComNum.VBLF + "  '" + strDept + "', '" + strDRCode + "',";
                        SQL = SQL + ComNum.VBLF + "  TO_DATE('" + strSysDate + " " + txtSDate.Text + "','YYYY-MM-DD HH24:MI') ,";
                        SQL = SQL + ComNum.VBLF + "  TO_DATE('" + strSysDate + " " + txtEDate.Text + "','YYYY-MM-DD HH24:MI') ,";
                        SQL = SQL + ComNum.VBLF + "  '" + txtMessage.Text + "', '" + FstrPassId + "' ,'" + VB.Left(cboMess.Text, 2) + "',";
                        SQL = SQL + ComNum.VBLF + "   '" + txtMessage1.Text + "' ) ";

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
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "NUR_MESSAGE_HIS ";
                        SQL = SQL + ComNum.VBLF + " (ACTDATE, ENTDATE, DEPT, DRCODE, SDATE, EDATE, MESSAGE, IDNUMBER, MESSAGE1, MESSAGE2) ";
                        SQL = SQL + ComNum.VBLF + "  VALUES( TO_DATE('" + strBdate + "','YYYY-MM-DD'), sysdate,";
                        SQL = SQL + ComNum.VBLF + "  '" + strDept + "', '" + strDRCode + "',";
                        SQL = SQL + ComNum.VBLF + "  TO_DATE('" + strSysDate + " " + txtSDate.Text + "','YYYY-MM-DD HH24:MI') ,";
                        SQL = SQL + ComNum.VBLF + "  TO_DATE('" + strSysDate + " " + txtEDate.Text + "','YYYY-MM-DD HH24:MI') ,";
                        SQL = SQL + ComNum.VBLF + "  '" + txtMessage.Text + "', '" + FstrPassId + "', '" + VB.Left(cboMess.Text, 2) + "',";
                        SQL = SQL + ComNum.VBLF + "  '" + txtMessage1.Text + "') ";

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
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_MESSAGE SET ";
                        SQL = SQL + ComNum.VBLF + " SDATE =TO_DATE('" + strSysDate + " " + txtSDate.Text + "','YYYY-MM-DD HH24:MI') ,";
                        SQL = SQL + ComNum.VBLF + " EDATE =TO_DATE('" + strSysDate + " " + txtEDate.Text + "','YYYY-MM-DD HH24:MI') ,";
                        SQL = SQL + ComNum.VBLF + " MESSAGE = '" + txtMessage.Text + "', ";
                        SQL = SQL + ComNum.VBLF + " MESSAGE1 = '" + VB.Left(cboMess.Text, 2) + "', ";
                        SQL = SQL + ComNum.VBLF + " MESSAGE2 = '" + txtMessage1.Text + "'";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strRowId + "' ";

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
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "NUR_MESSAGE_HIS ";
                        SQL = SQL + ComNum.VBLF + "(ACTDATE, ENTDATE, DEPT, DRCODE, SDATE, EDATE, MESSAGE, IDNUMBER, MESSAGE1, MESSAGE2) ";
                        SQL = SQL + ComNum.VBLF + " SELECT ACTDATE, ENTDATE, DEPT, DRCODE, SDATE, EDATE, MESSAGE, IDNUMBER, MESSAGE1, MESSAGE2 ";
                        SQL = SQL + ComNum.VBLF + " FROM NUR_MESSAGE ";
                        SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + strRowId + "' ";

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
                }

                Screen_Clear2();

                Select_Play(strDRCode);

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strDRCode = VB.Left(lblImFor.Text, 4);

            if (ComFunc.MsgBoxQ("정말로 삭제하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_MESSAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE =TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + strDRCode + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
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

        private void rdoGbn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoGbn0.Checked == true)
            {
                txtSDate.Text = "09:00";
                txtEDate.Text = "12:30";
            }
            else if (rdoGbn1.Checked == true)
            {
                txtSDate.Text = "01:00";
                txtEDate.Text = "17:30";
            }
            else if (rdoGbn2.Checked == true)
            {
                txtSDate.Text = "09:00";
                txtEDate.Text = "17:30";
            }
            else if (rdoGbn3.Checked == true)
            {
                txtSDate.Text = strSysTime;
                txtEDate.Text = Convert.ToDateTime(strSysTime).AddHours(1).ToString("hh:mm");
            }
            else if (rdoGbn4.Checked == true)
            {
                txtSDate.Text = strSysTime;
                txtEDate.Text = Convert.ToDateTime(strSysTime).AddHours(2).ToString("hh:mm");
            }
            else if (rdoGbn5.Checked == true)
            {
                txtSDate.Text = strSysTime;
                txtEDate.Text = Convert.ToDateTime(strSysTime).AddHours(3).ToString("hh:mm");
            }
            else if (rdoGbn6.Checked == true)
            {
                txtSDate.Text = strSysTime;
                txtEDate.Text = Convert.ToDateTime(strSysTime).AddHours(4).ToString("hh:mm");
            }
            else if (rdoGbn7.Checked == true)
            {
                txtSDate.Text = strSysTime;
                txtEDate.Text = Convert.ToDateTime(strSysTime).AddHours(5).ToString("hh:mm");
            }
        }

        void Select_Play(string ArgData)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(A.SDATE,'HH24:MI') SDATE, TO_CHAR(A.EDATE,'HH24:MI') EDATE,";
                SQL = SQL + ComNum.VBLF + "        A.MESSAGE, A.MESSAGE1, A.MESSAGE2, B.NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_MESSAGE A , " + ComNum.DB_PMPA + "NUR_MESSAGE_CODE B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE =TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE ='" + ArgData + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.MESSAGE1 = B.CODE(+)";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                txtSDate.Text = dt.Rows[0]["SDATE"].ToString().Trim();
                txtEDate.Text = dt.Rows[0]["EDATE"].ToString().Trim();
                txtMessage.Text = dt.Rows[0]["MESSAGE"].ToString().Trim();

                for (i = 0; i < cboMess.SelectedIndex; i++)
                {
                    cboMess.SelectedIndex = i;
                    if (VB.Left(cboMess.Text, 2) == dt.Rows[0]["MESSAGE1"].ToString().Trim())
                    {
                        cboMess.SelectedIndex = i;
                        return;
                    }
                    cboMess.SelectedIndex = -1;
                }

                txtMessage1.Text = dt.Rows[0]["MESSAGE2"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            Screen_Clear2();

            strDRCode = ssView_Sheet1.Cells[e.Row, 2].Text;
            lblImFor.Text = strDRCode + "[" + VB.Left(cboDept.Text, 2) + "]";
            lblImFor.Text += " " + ssView_Sheet1.Cells[e.Row, 0].Text;
            lblImFor.Text += " 진료형태[ " + ssView_Sheet1.Cells[e.Row, 1].Text + " ]";

            Select_Play(strDRCode);

            btnRegist.Enabled = true;
            panMain.Enabled = true;
        }

        private void txtSDate_Leave(object sender, EventArgs e)
        {
            int i = 0;
            string strTime = "";
            string strFlag = "";

            strTime = txtSDate.Text;
            if (strTime.Trim().Length < 5)
            {
                ComFunc.MsgBox("시간:분(00:00) 각각 2자리형식으로 입력 바랍니다.");
                txtSDate.Text = "00:00";
                txtSDate.Focus();
                return;
            }

            strFlag = "N";
            for (i = 1; i <= strTime.Length; i++)
            {
                if (VB.Mid(strTime, i, 1) == ":")
                {
                    strFlag = "Y";
                }
            }

            if (strFlag == "N")
            {
                ComFunc.MsgBox("시간:분(00:00) 각각 2자리형식으로 입력 바랍니다.");
                txtSDate.Text = "00:00";
                txtSDate.Focus();
                return;
            }
        }

        private void txtSDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtEDate.Focus();
            }
        }

        private void txtEDate_Leave(object sender, EventArgs e)
        {
            int i = 0;
            string strTime = "";
            string strFlag = "";

            strTime = txtEDate.Text;
            if (strTime.Trim().Length < 5)
            {
                ComFunc.MsgBox("시간:분(00:00) 각각 2자리형식으로 입력 바랍니다.");
                txtEDate.Text = "00:00";
                txtEDate.Focus();
                return;
            }

            strFlag = "N";
            for (i = 1; i <= strTime.Length; i++)
            {
                if (VB.Mid(strTime, i, 1) == ":")
                {
                    strFlag = "Y";
                }
            }

            if (strFlag == "N")
            {
                ComFunc.MsgBox("시간:분(00:00) 각각 2자리형식으로 입력 바랍니다.");
                txtEDate.Text = "00:00";
                txtEDate.Focus();
                return;
            }
        }
    }
}
