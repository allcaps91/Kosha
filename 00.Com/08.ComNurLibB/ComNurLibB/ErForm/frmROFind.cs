using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmROFind.cs
    /// Description     : 상병찾기
    /// Author          : 박창욱
    /// Create Date     : 2018-03-22
    /// Update History  : 
    /// </summary>
    /// <history>      
    /// </history>
    /// <seealso cref= "\nurse\nrer\nrer22.frm(FrmROFind.frm) >> frmROFind.cs 폼이름 재정의" />
    public partial class frmROFind : Form
    {
        //이벤트를 전달할 경우
        public delegate void SendDisease(string strValue);
        public event SendDisease rSendDisease;

        //폼이 Close 될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string GstrPANO = "";
        string GstrJDate = "";
        string GstrInDate = "";
        string GstrRetName = "";

        //string FstriLLName = "";

        public frmROFind()
        {
            InitializeComponent();
        }

        public frmROFind(string GstrPANO, string GstrJDate, string GstrInDate, string GstrRetName)
        {
            InitializeComponent();
            this.GstrPANO = GstrPANO;
            this.GstrJDate = GstrJDate;
            this.GstrInDate = GstrInDate;
            this.GstrRetName = GstrRetName;
        }

        void Select_View(string argGubun)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int k = 0;
            string strRemark1 = "";
            string strRemark2 = "";
            string strRemark3 = "";
            string strRemark4 = "";
            string strRemark5 = "";
            string strJBun = "";

            ComFunc cf = new ComFunc();

            ssView23_Sheet1.RowCount = 0;
            ssView24_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT GUBUN, ILLCODE, JBUN, REMARK1, ";
                SQL = SQL + ComNum.VBLF + " REMARK2, REMARK3, REMARK4, REMARK5, JBUN ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_JUNGDO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('" + GstrJDate + "','YYYY-MM-DD') ";
                if (argGubun != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND GUBUN  = '" + argGubun + "' ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strRemark1 = dt.Rows[0]["REMARK1"].ToString().Trim();
                    strRemark2 = dt.Rows[0]["REMARK2"].ToString().Trim();
                    strRemark3 = dt.Rows[0]["REMARK3"].ToString().Trim();
                    strRemark4 = dt.Rows[0]["REMARK4"].ToString().Trim();
                    strRemark5 = dt.Rows[0]["REMARK5"].ToString().Trim();

                    switch (dt.Rows[0]["Gubun"].ToString().Trim())
                    {
                        case "1":
                            txtiLLCode1.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                            lbliLLName11.Text = cf.Read_IllsName(clsDB.DbCon, dt.Rows[0]["ILLCODE"].ToString().Trim(), "2");
                            txtDate11.Text = VB.Left(strRemark1, 10);
                            txtTime11.Text = VB.Right(strRemark1, 5);
                            txtDate12.Text = VB.Left(strRemark2, 10);
                            txtTime12.Text = VB.Right(strRemark2, 5);
                            txtJep11.Text = strRemark3;
                            txtJep12.Text = strRemark4;
                            panTabPage1.Visible = true;
                            panTabPage2.Visible = false;
                            panTabPage3.Visible = false;
                            tabControl1.SelectedTab = tabPage1;
                            strJBun = dt.Rows[0]["JBUN"].ToString().Trim();

                            if (strJBun != "")
                            {
                                for (j = 1; j <= strJBun.Length; j += 2)
                                {
                                    for (k = 0; k <= ssView11_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); k++)
                                    {
                                        if (VB.Mid(strJBun, j, 2) == VB.Left(ssView11_Sheet1.Cells[k, 1].Text, 2))
                                        {
                                            ssView11_Sheet1.Cells[k, 0].Value = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case "2":
                            txtiLLCode2.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                            lbliLLName21.Text = cf.Read_IllsName(clsDB.DbCon, dt.Rows[0]["ILLCODE"].ToString().Trim(), "2");
                            lblCTDate.Text = VB.Left(strRemark1, 10);
                            lblCTTime.Text = VB.Right(strRemark1, 5);
                            lblMRIDate.Text = VB.Left(strRemark2, 10);
                            lblMRITime.Text = VB.Right(strRemark2, 5);

                            if (strRemark3 == "")
                            {
                                lblDDate31.Text = VB.Left(GstrInDate, 10);
                                lblDTime32.Text = VB.Right(GstrInDate, 5);
                            }
                            else
                            {
                                lblDDate21.Text = VB.Left(strRemark3, 10);
                                lblDTime22.Text = VB.Right(strRemark3, 5);
                            }

                            panTabPage1.Visible = false;
                            panTabPage2.Visible = true;
                            panTabPage3.Visible = false;
                            tabControl1.SelectedTab = tabPage2;
                            strJBun = dt.Rows[0]["JBUN"].ToString().Trim();

                            if (strJBun != "")
                            {
                                for (j = 1; j <= strJBun.Length; j += 2)
                                {
                                    for (k = 0; k <= ssView21_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); k++)
                                    {
                                        if (VB.Mid(strJBun, j, 2) == VB.Left(ssView21_Sheet1.Cells[k, 1].Text, 2))
                                        {
                                            ssView21_Sheet1.Cells[k, 0].Value = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case "3":
                            txtiLLCode3.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                            lbliLLName31.Text = cf.Read_IllsName(clsDB.DbCon, dt.Rows[0]["ILLCODE"].ToString().Trim(), "2");
                            txtDate31.Text = VB.Left(strRemark1, 10);
                            txttime32.Text = VB.Right(strRemark1, 5);

                            if (strRemark2 == "")
                            {
                                lblDDate31.Text = VB.Left(GstrInDate, 10);
                                lblDTime32.Text = VB.Right(GstrInDate, 5);
                            }
                            else
                            {
                                lblDDate31.Text = VB.Left(strRemark2, 10);
                                lblDTime32.Text = VB.Right(strRemark2, 5);
                            }

                            panTabPage1.Visible = false;
                            panTabPage2.Visible = false;
                            panTabPage3.Visible = true;
                            tabControl1.SelectedTab = tabPage3;
                            strJBun = dt.Rows[0]["JBUN"].ToString().Trim();

                            if (strJBun != "")
                            {
                                for (j = 1; j <= strJBun.Length; j += 2)
                                {
                                    for (k = 0; k <= ssView31_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); k++)
                                    {
                                        if (VB.Mid(strJBun, j, 2) == VB.Left(ssView31_Sheet1.Cells[k, 1].Text, 2))
                                        {
                                            ssView31_Sheet1.Cells[k, 0].Value = true;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (argGubun)
                    {
                        case "3":
                            lblDDate31.Text = VB.Left(GstrInDate, 10);
                            lblDTime32.Text = VB.Right(GstrInDate, 5);
                            break;
                    }
                }

                dt.Dispose();
                dt = null;


                switch (argGubun)
                {
                    case "2":
                        SQL = "";
                        SQL = " SELECT TO_CHAR(XSENDDATE, 'YYYY-MM-DD') SENDDATE, ORDERNAME, ";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(XSENDDATE, 'HH24:MI') SENDTIME, XJONG ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                        SQL = SQL + ComNum.VBLF + " WHERE XJONG IN('4','5') "; //CT,MRI
                        SQL = SQL + ComNum.VBLF + "   AND WARDCODE  = 'ER' ";
                        SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + GstrPANO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ENTERDATE >= TO_DATE('" + GstrJDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ENTERDATE < TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, GstrJDate, 1) + "23:59" + "','YYYY-MM-DDHH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "   AND GBRESERVED IN ('6','7') ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["XJONG"].ToString().Trim() == "4")
                                {
                                    ssView23_Sheet1.RowCount++;
                                    ssView23_Sheet1.Cells[ssView23_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SENDDATE"].ToString().Trim() + " " + dt.Rows[i]["SENDTIME"].ToString().Trim();
                                    ssView23_Sheet1.Cells[ssView23_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }
                                else
                                {
                                    ssView24_Sheet1.RowCount++;
                                    ssView24_Sheet1.Cells[ssView24_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["SENDDATE"].ToString().Trim() + " " + dt.Rows[i]["SENDTIME"].ToString().Trim();
                                    ssView24_Sheet1.Cells[ssView24_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                }
                            }

                            if (ssView23_Sheet1.NonEmptyRowCount == 1)
                            {
                                lblCTDate.Text = VB.Left(ssView23_Sheet1.Cells[0, 0].Text.Trim(), 10);
                                lblCTTime.Text = VB.Right(ssView23_Sheet1.Cells[0, 0].Text.Trim(), 5);
                            }

                            if (ssView24_Sheet1.NonEmptyRowCount == 1)
                            {
                                lblMRIDate.Text = VB.Left(ssView24_Sheet1.Cells[0, 0].Text.Trim(), 10);
                                lblMRITime.Text = VB.Right(ssView24_Sheet1.Cells[0, 0].Text.Trim(), 5);
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        lblDDate21.Text = VB.Left(GstrInDate, 10);
                        lblDTime22.Text = VB.Right(GstrInDate, 5);

                        break;
                }

                ssView23_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView24_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrPANO = "";
            GstrJDate = "";

            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            paniLL.Visible = true;
            txtName.Focus();
        }

        private void btnHelpExit_Click(object sender, EventArgs e)
        {
            paniLL.Visible = false;
        }

        private void btnHelpView_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strName = "";

            strName = txtName.Text.Trim();

            ssView4_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ILLCODE, ILLNAMEK, ILLNAMEE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_ILLS ";
                if (strName != "")
                {
                    if (rdoBun0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE ILLCODE LIKE '%" + strName + "%' ";
                    }
                    if (rdoBun1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE ILLNAMEK LIKE '%" + strName + "%' ";
                    }
                    if (rdoBun2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE ILLNAMEE LIKE '%" + strName + "%' ";
                    }
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY ILLCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
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

                ssView4_Sheet1.RowCount = dt.Rows.Count;
                ssView4_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView4_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                    ssView4_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    ssView4_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtiLLName.Text.Trim() != "")
            {
                rSendDisease(txtiLLName.Text);
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strChk = "";
            string strNameK = "";
            string strNameE = "";
            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //코드저장
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strChk = "";
                    strNameK = "";
                    strNameE = "";
                    strROWID = "";

                    strChk = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true ? "1" : "";
                    strNameE = ssView_Sheet1.Cells[i, 1].Text;
                    strNameK = ssView_Sheet1.Cells[i, 2].Text;
                    strROWID = ssView_Sheet1.Cells[i, 3].Text.Trim();

                    if (strChk == "")
                    {
                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_ER_ROHELP (Namek,Namee) ";
                            SQL = SQL + ComNum.VBLF + " VALUES('" + strNameK + "','" + strNameE + "') ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_ER_ROHELP SET ";
                            SQL = SQL + ComNum.VBLF + " Namek = '" + strNameK + "', ";
                            SQL = SQL + ComNum.VBLF + " Namee = '" + strNameE + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                        }
                    }
                    else if (strChk == "1" && strROWID != "")
                    {
                        SQL = "";
                        SQL = "DELETE FROM " + ComNum.DB_PMPA + "NUR_ER_RoHelp WHERE ROWID='" + strROWID + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("상병저장시 DB에 에러발생");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 완료");

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

            Search_Data();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strIllCode = "";
            string strJBun = "";
            string strGubun = "";
            string strRemark1 = "";
            string strRemark2 = "";
            string strRemark3 = "";
            string strRemark4 = "";

            strGubun = VB.Left(cboGubun.Text, 1);
            strIllCode = txtiLLCode1.Text.Trim();
            strRemark1 = txtDate11.Text.Trim() + " " + txtTime11.Text.Trim();
            strRemark2 = txtDate12.Text.Trim() + " " + txtTime12.Text.Trim();
            strRemark3 = txtJep11.Text.Trim();
            strRemark4 = txtJep12.Text.Trim();

            strJBun = "";

            for (i = 0; i < ssView11_Sheet1.NonEmptyRowCount; i++)
            {
                if (Convert.ToBoolean(ssView11_Sheet1.Cells[i, 0].Value) == true)
                {
                    strJBun += VB.Left(ssView11_Sheet1.Cells[i, 1].Text, 2);
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_JUNGDO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('" + GstrJDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN  = '" + strGubun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_ER_JUNGDO( GUBUN, PANO, JDATE, ILLCODE, ";
                    SQL = SQL + ComNum.VBLF + " JBUN, REMARK1, REMARK2, REMARK3, REMARK4) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '" + strGubun + "', '" + GstrPANO + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + GstrJDate + "', 'YYYY-MM-DD'), '" + strIllCode + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strJBun + "', '" + strRemark1 + "', '" + strRemark2 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strRemark3 + "', '" + strRemark4 + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_ER_JUNGDO SET ";
                    SQL = SQL + ComNum.VBLF + " ILLCODE = '" + strIllCode + "', ";
                    SQL = SQL + ComNum.VBLF + " JBUN  = '" + strJBun + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK1 = '" + strRemark1 + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK2 = '" + strRemark2 + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK3 = '" + strRemark3 + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK4 = '" + strRemark4 + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                dt.Dispose();
                dt = null;

                //clsDB.setRollbackTran(clsDB.DbCon);     //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("저장하였습니다.");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return;
            }

            Search_Data1();
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strIllCode = "";
            string strJBun = "";
            string strGubun = "";
            string strRemark1 = "";
            string strRemark2 = "";
            string strRemark3 = "";

            strGubun = VB.Left(cboGubun.Text, 1);
            strIllCode = txtiLLCode2.Text.Trim();
            strRemark1 = lblCTDate.Text.Trim() + " " + lblCTTime.Text.Trim();
            strRemark2 = lblMRIDate.Text.Trim() + " " + lblMRITime.Text.Trim();
            strRemark3 = lblDDate21.Text.Trim() + " " + lblDTime22.Text.Trim();

            strJBun = "";

            for (i = 0; i < ssView21_Sheet1.NonEmptyRowCount; i++)
            {
                if (Convert.ToBoolean(ssView21_Sheet1.Cells[i, 0].Value) == true)
                {
                    strJBun += VB.Left(ssView21_Sheet1.Cells[i, 1].Text, 2);
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_JUNGDO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('" + GstrJDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN  = '" + strGubun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_ER_JUNGDO( GUBUN, PANO, JDATE, ILLCODE, ";
                    SQL = SQL + ComNum.VBLF + " JBUN, REMARK1, REMARK2, REMARK3) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '" + strGubun + "', '" + GstrPANO + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + GstrJDate + "', 'YYYY-MM-DD'), '" + strIllCode + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strJBun + "', '" + strRemark1 + "', '" + strRemark2 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strRemark3 + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_ER_JUNGDO SET ";
                    SQL = SQL + ComNum.VBLF + " ILLCODE = '" + strIllCode + "', ";
                    SQL = SQL + ComNum.VBLF + " JBUN  = '" + strJBun + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK1 = '" + strRemark1 + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK2 = '" + strRemark2 + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK3 = '" + strRemark3 + "', ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                dt.Dispose();
                dt = null;

                //clsDB.setRollbackTran(clsDB.DbCon);     //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("저장하였습니다.");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return;
            }

            Search_Data2();
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strIllCode = "";
            string strJBun = "";
            string strGubun = "";
            string strRemark1 = "";
            string strRemark2 = "";
            string strRemark3 = "";

            strGubun = VB.Left(cboGubun.Text, 1);
            strIllCode = txtiLLCode3.Text.Trim();
            strRemark1 = txtDate31.Text.Trim() + " " + txttime32.Text.Trim();
            strRemark3 = lblDDate31.Text.Trim() + " " + lblDTime32.Text.Trim();

            strJBun = "";

            for (i = 0; i < ssView31_Sheet1.NonEmptyRowCount; i++)
            {
                if (Convert.ToBoolean(ssView31_Sheet1.Cells[i, 0].Value) == true)
                {
                    strJBun += VB.Left(ssView31_Sheet1.Cells[i, 1].Text, 2);
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_JUNGDO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('" + GstrJDate + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN  = '" + strGubun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_ER_JUNGDO( GUBUN, PANO, JDATE, ILLCODE, ";
                    SQL = SQL + ComNum.VBLF + " JBUN, REMARK1, REMARK2, REMARK3) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '" + strGubun + "', '" + GstrPANO + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + GstrJDate + "', 'YYYY-MM-DD'), '" + strIllCode + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strJBun + "', '" + strRemark1 + "', '" + strRemark2 + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_ER_JUNGDO SET ";
                    SQL = SQL + ComNum.VBLF + " ILLCODE = '" + strIllCode + "', ";
                    SQL = SQL + ComNum.VBLF + " JBUN  = '" + strJBun + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK1 = '" + strRemark1 + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK2 = '" + strRemark2 + "', ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                dt.Dispose();
                dt = null;

                //clsDB.setRollbackTran(clsDB.DbCon);     //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("저장하였습니다.");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return;
            }

            Search_Data3();
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

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT NAMEK,NAMEE,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_ROHELP ";
                SQL = SQL + ComNum.VBLF + " ORDER BY NAMEE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
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

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Namee"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Namek"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount += 20;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            Search_Data1();
        }

        void Search_Data1()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strGubun = "";
            string strRemark = "";

            strGubun = VB.Left(cboGubun.Text, 1);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT BUN2, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_GUBUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN1 = '2'";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_NUMBER(BUN2) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView11_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssView11_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView11_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUN2"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim();
                        ssView11_Sheet1.SetRowHeight(i, (int)ssView11_Sheet1.GetPreferredRowHeight(i) + 5);
                    }
                }

                dt.Dispose();
                dt = null;


                strRemark = "";

                SQL = "";
                SQL = "SELECT BUN2, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_GUBUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN1 = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_NUMBER(BUN2) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRemark += dt.Rows[i]["BUN2"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim() + ComNum.VBLF;
                    }
                }

                dt.Dispose();
                dt = null;

                ssView12_Sheet1.Cells[0, 0].Text = strRemark;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            Select_View(strGubun);
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            Search_Data2();
        }

        void Search_Data2()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strGubun = "";
            string strRemark = "";

            strGubun = VB.Left(cboGubun.Text, 1);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT BUN2, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_GUBUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN1 = '2'";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_NUMBER(BUN2) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView21_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssView21_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView21_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUN2"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim();
                        ssView21_Sheet1.SetRowHeight(i, (int)ssView21_Sheet1.GetPreferredRowHeight(i) + 5);
                    }
                }

                dt.Dispose();
                dt = null;


                strRemark = "";

                SQL = "";
                SQL = "SELECT BUN2, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_GUBUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN1 = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_NUMBER(BUN2) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRemark += dt.Rows[i]["BUN2"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim() + ComNum.VBLF;
                    }
                }

                dt.Dispose();
                dt = null;

                ssView22_Sheet1.Cells[0, 0].Text = strRemark;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            Select_View(strGubun);
        }

        private void btnSearch3_Click(object sender, EventArgs e)
        {
            Search_Data3();
        }

        void Search_Data3()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strGubun = "";
            string strRemark = "";

            strGubun = VB.Left(cboGubun.Text, 1);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT BUN2, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_GUBUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN1 = '2'";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_NUMBER(BUN2) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView31_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    ssView31_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView31_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUN2"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim();
                        ssView31_Sheet1.SetRowHeight(i, (int)ssView31_Sheet1.GetPreferredRowHeight(i) + 5);
                    }
                }

                dt.Dispose();
                dt = null;


                strRemark = "";

                SQL = "";
                SQL = "SELECT BUN2, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_GUBUN ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN1 = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY TO_NUMBER(BUN2) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRemark += dt.Rows[i]["BUN2"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim() + ComNum.VBLF;
                    }
                }

                dt.Dispose();
                dt = null;

                ssView32_Sheet1.Cells[0, 0].Text = strRemark;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            Select_View(strGubun);
        }

        private void cboGubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (VB.Left(cboGubun.Text, 1))
            {
                case "1":
                    panTabPage1.Visible = true;
                    panTabPage2.Visible = false;
                    panTabPage3.Visible = false;
                    tabControl1.SelectedTab = tabPage1;
                    Search_Data1();
                    break;
                case "2":
                    panTabPage1.Visible = false;
                    panTabPage2.Visible = true;
                    panTabPage3.Visible = false;
                    tabControl1.SelectedTab = tabPage2;
                    Search_Data2();
                    break;
                case "3":
                    panTabPage1.Visible = false;
                    panTabPage2.Visible = false;
                    panTabPage3.Visible = true;
                    tabControl1.SelectedTab = tabPage3;
                    Search_Data3();
                    break;
            }
        }

        void lbl_Clear()
        {
            lbliLLName11.Text = "";
            lbliLLName21.Text = "";
            lblCTDate.Text = "";
            lblCTTime.Text = "";
            lblMRIDate.Text = "";
            lblMRITime.Text = "";
            lblDDate21.Text = "";
            lblDTime22.Text = "";
            lbliLLName31.Text = "";
            lblDDate31.Text = "";
            lblDTime32.Text = "";
        }

        private void frmROFind_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strGubun = "";

            ssView_Sheet1.Columns[3].Visible = false;

            Search_Data();

            lbl_Clear();

            cboGubun.Items.Clear();
            cboGubun.Items.Add(" ");
            cboGubun.Items.Add("1.급성심근경색증 환자");
            cboGubun.Items.Add("2.급성(뇌출혈,뇌경색) 환자");
            cboGubun.Items.Add("3.급성뇌경색 환자");
            cboGubun.SelectedIndex = 0;

            clsVbfunc.TextBoxClear(this);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_JUNGDO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JDATE = TO_DATE('" + GstrJDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strGubun = dt.Rows[0]["GUBUN"].ToString().Trim();
                    cboGubun.SelectedIndex = (int)VB.Val(strGubun);
                }

                dt.Dispose();
                dt = null;

                if (clsPublic.GstrHelpCode != "")
                {
                    txtiLLName.Text = clsPublic.GstrHelpCode;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            txtiLLName.Select();
            txtiLLName.Focus();
        }

        
        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            rSendDisease(txtiLLName.Text);




            //int nSelStart = 0;
            //string strTxtMsg = "";

            //switch (GstrRetName)
            //{
            //    case "PATIENTENTRY20160202":
            //        //nSelStart = FrmPatientEntry20160216.TxtDisease.SelStart
            //        break;
            //    case "PATIENTENTRY_NEW":
            //        //nSelStart = FrmPatientEntry_New.TxtDisease.SelStart
            //        break;
            //    case "KTAS":
            //        //nSelStart = FrmKTAS.TxtDisease.SelStart
            //        break;
            //    default:
            //        //nSelStart = FrmPatientEntry.TxtDisease.SelStart
            //        break;
            //}

            //if (nSelStart == 0)
            //{
            //    txtiLLName.Text = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            //}
            //else
            //{
            //    switch (GstrRetName)
            //    {
            //        case "PATIENTENTRY20160202":
            //            //strTxtMsg = Left(FrmPatientEntry20160216.TxtDisease.Text, nSelStart) & SS1.Text
            //            //strTxtMsg = strTxtMsg & Right(FrmPatientEntry20160216.TxtDisease.Text, Len(FrmPatientEntry20160216.TxtDisease.Text) - nSelStart)
            //            //FrmPatientEntry20160216.TxtDisease.Text = strTxtMsg
            //            //FrmPatientEntry20160216.TxtDisease.SelStart = nSelStart + 1
            //            break;
            //        case "PATIENTENTRY_NEW":
            //            //strTxtMsg = Left(FrmPatientEntry_New.TxtDisease.Text, nSelStart) & SS1.Text
            //            //strTxtMsg = strTxtMsg & Right(FrmPatientEntry_New.TxtDisease.Text, Len(FrmPatientEntry_New.TxtDisease.Text) - nSelStart)
            //            //FrmPatientEntry_New.TxtDisease.Text = strTxtMsg
            //            //FrmPatientEntry_New.TxtDisease.SelStart = nSelStart + 1
            //            break;
            //        case "KTAS":
            //            //strTxtMsg = Left(FrmKTAS.TxtDisease.Text, nSelStart) & SS1.Text
            //            //strTxtMsg = strTxtMsg & Right(FrmKTAS.TxtDisease.Text, Len(FrmKTAS.TxtDisease.Text) - nSelStart)
            //            //FrmKTAS.TxtDisease.Text = strTxtMsg
            //            //FrmKTAS.TxtDisease.SelStart = nSelStart + 1
            //            break;
            //        default:
            //            //strTxtMsg = Left(FrmPatientEntry.TxtDisease.Text, nSelStart) & SS1.Text
            //            //strTxtMsg = strTxtMsg & Right(FrmPatientEntry.TxtDisease.Text, Len(FrmPatientEntry.TxtDisease.Text) - nSelStart)
            //            //FrmPatientEntry.TxtDisease.Text = strTxtMsg
            //            //FrmPatientEntry.TxtDisease.SelStart = nSelStart + 1
            //            break;
            //    }
            //}

        }

        private void ssView11_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (Convert.ToBoolean(ssView11_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                ssView11_Sheet1.Rows[e.Row].ForeColor = Color.Blue;
            }
            else
            {
                ssView11_Sheet1.Rows[e.Row].ForeColor = Color.Black;
            }
        }

        private void ssView21_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (Convert.ToBoolean(ssView21_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                ssView21_Sheet1.Rows[e.Row].ForeColor = Color.Blue;
            }
            else
            {
                ssView21_Sheet1.Rows[e.Row].ForeColor = Color.Black;
            }
        }

        private void ssView31_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (Convert.ToBoolean(ssView31_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                ssView31_Sheet1.Rows[e.Row].ForeColor = Color.Blue;
            }
            else
            {
                ssView31_Sheet1.Rows[e.Row].ForeColor = Color.Black;
            }
        }

        private void ssView23_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView23_Sheet1.RowCount == 0)
            {
                return;
            }

            lblCTDate.Text = VB.Left(ssView23_Sheet1.Cells[e.Row, 0].Text.Trim(), 10);
            lblCTTime.Text = VB.Right(ssView23_Sheet1.Cells[e.Row, 0].Text.Trim(), 5);
        }

        private void ssView24_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView24_Sheet1.RowCount == 0)
            {
                return;
            }

            lblMRIDate.Text = VB.Left(ssView24_Sheet1.Cells[e.Row, 0].Text.Trim(), 10);
            lblMRITime.Text = VB.Right(ssView24_Sheet1.Cells[e.Row, 0].Text.Trim(), 5);
        }

        private void ssView4_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView4_Sheet1.RowCount == 0)
            {
                return;
            }

            string strCODE = "";
            string strName = "";

            strCODE = ssView4_Sheet1.Cells[e.Row, 0].Text.Trim();
            strName = ssView4_Sheet1.Cells[e.Row, 2].Text.Trim();

            switch (VB.Left(cboGubun.Text, 1))
            {
                case "1":
                    txtiLLCode1.Text = strCODE;
                    lbliLLName11.Text = strName;
                    break;
                case "2":
                    txtiLLCode2.Text = strCODE;
                    lbliLLName21.Text = strName;
                    break;
                case "3":
                    txtiLLCode3.Text = strCODE;
                    lbliLLName31.Text = strName;
                    break;
            }

            paniLL.Visible = false;
        }

        private void txtiLLName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.Focus();
            }
        }

        private void txtJep11_Leave(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (txtJep11.Text.Trim() != "")
                {
                    SQL = "";
                    SQL = "SELECT JEPCODE";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_JEP ";
                    SQL = SQL + ComNum.VBLF + " WHERE JEPCODE = '" + txtJep11.Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        txtJep11.Text = "";
                        ComFunc.MsgBox("약코드를 확인 해주세요!!");
                        return;
                    }

                    dt.Dispose();
                    dt = null;
                }
                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtJep12_Leave(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (txtJep11.Text.Trim() != "")
                {
                    SQL = "";
                    SQL = "SELECT JEPCODE";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "DRUG_JEP ";
                    SQL = SQL + ComNum.VBLF + " WHERE JEPCODE = '" + txtJep12.Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        txtJep12.Text = "";
                        ComFunc.MsgBox("약코드를 확인 해주세요!!");
                        return;
                    }

                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
