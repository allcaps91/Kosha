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
    /// File Name       : frmEduAttendReg.cs
    /// Description     : 교육참석여부등록하기
    /// Author          : 박창욱
    /// Create Date     : 2018-01-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육참석여부.frm(Frm교육참석여부.frm) >> frmEduAttendReg.cs 폼이름 재정의" />	
    public partial class frmEduAttendReg : Form
    {
        string FstrBuse = "";
        string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmEduAttendReg()
        {
            InitializeComponent();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (Update_Data() == true)
            {
                Screen_Clear();
                Search_Data();
            }
        }

        bool Update_Data()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strWRTNO = "";

            if (ComFunc.MsgBoxQ("교육 참석을 취소하시겟습니까?") == DialogResult.No)
            {
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_EDU_MST ";
                SQL = SQL + ComNum.VBLF + " SET SIGN = NULL ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO='" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SABUN = '" + clsType.User.Sabun.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnEduSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            EduSearch_Data();
        }

        void EduSearch_Data()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Screen_Clear();

            ssEdu_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT  A.GUBUN , A.JONG, A.Code, A.Name, B.SIGN, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.FrDate,'YYYY-MM-DD') FrDate, TO_CHAR(A.ToDate,'YYYY-MM-DD') ToDate,";
                SQL = SQL + ComNum.VBLF + "  A.OptTime , A.STIME, A.MAN, A.OptPlace, A.PLACE, A.JUMSU, A.EntDate, A.ROWID, b.WRTNO WRTNO_B";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE A, " + ComNum.DB_PMPA + "NUR_EDU_MST B, " + ComNum.DB_ERP + "INSA_MST C";
                SQL = SQL + ComNum.VBLF + "  WHERE A.Gubun ='2'";
                SQL = SQL + ComNum.VBLF + "    AND A.CODE = B.WRTNO(+)";
                SQL = SQL + ComNum.VBLF + "    AND A.ENTSABUN = C.SABUN";
                if (rdoGubun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND (B.WRTNO IS NULL OR B.SIGN IS NULL) ";
                }
                else if (rdoGubun2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND B.WRTNO IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "    AND B.SIGN = '1'";
                }
                SQL = SQL + ComNum.VBLF + "    AND B.CERT = '1'";
                SQL = SQL + ComNum.VBLF + "    AND ((C.BUSE = '" + FstrBuse + "' AND EDUGUBUN1 = '1') OR EDUGUBUN1 = '0')";
                SQL = SQL + ComNum.VBLF + "    AND TRUNC(B.SABUN) = '" + clsType.User.Sabun + "'";
                SQL = SQL + ComNum.VBLF + "  Order By EntDate";

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

                ssEdu_Sheet1.RowCount = dt.Rows.Count;
                ssEdu_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssEdu_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();
                    switch (dt.Rows[i]["Jong"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 1].Text = "감염";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 1].Text = "QI";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 1].Text = "CS";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 1].Text = "연수";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[i, 1].Text = "자격";
                            break;
                        case "6":
                            ssView_Sheet1.Cells[i, 1].Text = "Report";
                            break;
                        case "7":
                            ssView_Sheet1.Cells[i, 1].Text = "봉사";
                            break;
                        case "8":
                            ssView_Sheet1.Cells[i, 1].Text = "직무";
                            break;
                        case "9":
                            ssView_Sheet1.Cells[i, 1].Text = "병동";
                            break;
                        case "10":
                            ssView_Sheet1.Cells[i, 1].Text = "학술";
                            break;
                        case "11":
                            ssView_Sheet1.Cells[i, 1].Text = "건강";
                            break;
                        case "12":
                            ssView_Sheet1.Cells[i, 1].Text = "기타";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 1].Text = "기타";
                            break;
                    }
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FrDate"].ToString().Trim();
                    if (dt.Rows[i]["ToDate"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 3].Text += "~" + dt.Rows[i]["ToDate"].ToString().Trim();
                    }
                    switch (dt.Rows[i]["OptTime"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[i, 4].Text = "10분";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[i, 4].Text = "30분내";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 4].Text = "1시간내";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 4].Text = "90분";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 4].Text = "2시간";
                            break;
                    }
                    if (dt.Rows[i]["STime"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["STime"].ToString().Trim();
                    }
                    switch (dt.Rows[i]["OptPlace"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[i, 5].Text = "마리아홀";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[i, 5].Text = "466호실";
                            break;
                    }
                    if (dt.Rows[i]["Place"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Place"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Man"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["WRTNO_B"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SIGN"].ToString().Trim();

                    if (ssView_Sheet1.Cells[i, 9].Text == "")
                    {
                        ssEdu_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 200, 200);
                    }
                    else
                    {
                        ssEdu_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 255);
                    }
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

        void Screen_Clear()
        {
            txtSeqNo.Text = "";
            txtTopic.Text = "";
            txtTime.Text = "";
            txtPlace.Text = "";
            txtMan.Text = "";
            txtRemark.Text = "";
            txtJumsu.Text = "";
            txtROWID.Text = "";
            cboJong.SelectedIndex = -1;

            rdoTime0.Checked = false;
            rdoTime1.Checked = false;
            rdoTime2.Checked = false;
            rdoTime3.Checked = false;
            rdoTime4.Checked = false;

            rdoPlace0.Checked = false;
            rdoPlace1.Checked = false;

            btnSave.Enabled = false;
            btnDel.Enabled = false;
            btnSave.Visible = true;
            lblPMsg.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (Save_Data() == true)
            {
                Screen_Clear();
                Search_Data();
            }
        }

        bool Save_Data()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strROWID = "";
            string strBuCode = "";

            string strWRTNO = "";
            string strSabun = "";
            string strIpSaDay = "";
            string strTopic = "";
            string strJik = "";
            string strOptTime = "";
            string strOptPlace = "";
            string strEduDate = "";
            string strEduDate2 = "";
            string strEduTime = "";
            string strMan = "";
            string strPlace = "";
            string strJumsu = "";
            string strJong = "";
            string strSName = "";
            string strRemark = "";
            string strManJong = "";

            strManJong = VB.Left(cboManJong.Text.Trim(), 2);

            if (strManJong == "")
            {
                ComFunc.MsgBox("강사종류를 선택하세요.");
                return false;
            }

            strWRTNO = txtSeqNo.Text.Trim();

            if (strWRTNO == "")
            {
                ComFunc.MsgBox("다시 작업하세요.");
                return false;
            }

            strJong = VB.Left(cboJong.Text, 2).Trim();
            strTopic = txtTopic.Text.Trim();
            strEduDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strEduDate2 = dtpDate2.Value.ToString("yyyy-MM-dd");

            if (rdoTime0.Checked == true)
            {
                strOptTime = "0";
            }
            else if (rdoTime1.Checked == true)
            {
                strOptTime = "1";
            }
            else if (rdoTime2.Checked == true)
            {
                strOptTime = "2";
            }
            else if (rdoTime3.Checked == true)
            {
                strOptTime = "3";
            }
            else if (rdoTime4.Checked == true)
            {
                strOptTime = "4";
            }

            strEduTime = txtTime.Text.Trim();
            strMan = txtMan.Text.Trim();

            if (rdoPlace0.Checked == true)
            {
                strOptPlace = "0";
            }
            else if (rdoPlace1.Checked == true)
            {
                strOptPlace = "1";
            }

            strPlace = txtPlace.Text.Trim();

            if (!(VB.IsNumeric(txtJumsu.Text)))
            {
                ComFunc.MsgBox("점수는 숫자만 가능합니다.");
                return false;
            }

            strJumsu = txtJumsu.Text.Trim();
            strRemark = txtRemark.Text.Trim();
            strSabun = clsType.User.Sabun;
            strSName = clsType.User.JobName;
            strBuCode = VB.Pstr(lblSts.Text, "^^", 1).Trim();
            strJik = VB.Pstr(lblSts.Text, "^^", 2).Trim();
            strIpSaDay = VB.Pstr(lblSts.Text, "^^", 3).Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO ='" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(SABUN) = '" + clsType.User.Sabun.Trim() + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strROWID == "")
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_EDU_MST (WRTNO,Sabun,SName,BuCode,IpsaDate,SDate,JikJong,";
                    SQL = SQL + ComNum.VBLF + " EDUJONG , EDUNAME, FrDate,ToDate, OptTime, EDUTIME, MAN,ManJong,OptPlace,PLACE,Remark, JUMSU ,Gubun,EntDate, SIGN ) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" + strWRTNO + "','" + VB.Val(strSabun).ToString("00000") + "','" + strSName + "','" + strBuCode + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strIpSaDay + "', TO_DATE('" + strSysDate + "','YYYY-MM-DD') ,  ";
                    SQL = SQL + ComNum.VBLF + " '" + strJik + "','" + strJong + "','" + strTopic + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strEduDate + "','YYYY-MM-DD'),TO_DATE('" + strEduDate2 + "','YYYY-MM-DD'),'" + strOptTime + "', '" + strEduTime + "',  ";
                    SQL = SQL + ComNum.VBLF + " '" + strMan + "','" + strManJong + "','" + strOptPlace + "','" + strPlace + "','" + strRemark + "', '" + strJumsu + "', '1', SYSDATE, '1'  ) ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_EDU_MST SET ";
                    SQL = SQL + ComNum.VBLF + " JikJong ='" + strJik + "', ";
                    SQL = SQL + ComNum.VBLF + " BuCode ='" + strBuCode + "',";
                    SQL = SQL + ComNum.VBLF + " EDUJONG ='" + strJong + "', ";
                    SQL = SQL + ComNum.VBLF + " EDUNAME ='" + strTopic + "', ";
                    SQL = SQL + ComNum.VBLF + " FrDate =TO_DATE('" + strEduDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " ToDate =TO_DATE('" + strEduDate2 + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " OptTime ='" + strOptTime + "', ";
                    SQL = SQL + ComNum.VBLF + " EDUTIME ='" + strEduTime + "', ";
                    SQL = SQL + ComNum.VBLF + " MAN ='" + strMan + "', ";
                    SQL = SQL + ComNum.VBLF + " ManJong ='" + strManJong + "', ";
                    SQL = SQL + ComNum.VBLF + " Remark ='" + strRemark + "',";
                    SQL = SQL + ComNum.VBLF + " OptPlace ='" + strOptPlace + "', ";
                    SQL = SQL + ComNum.VBLF + " PLACE ='" + strPlace + "', ";
                    SQL = SQL + ComNum.VBLF + " JUMSU ='" + strJumsu + "', ";
                    SQL = SQL + ComNum.VBLF + " SIGN = '1'";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data();
        }

        void Search_Data()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Screen_Clear();

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WRTNO,SABUN,SNAME,BUCODE,IPSADATE,SDATE,JIKJONG,EDUJONG,EDUNAME,FrDate,ToDate,";
                SQL = SQL + ComNum.VBLF + "    OptTime,EDUTIME , MAN, OptPlace,PLACE, JUMSU, GUBUN, ENTDATE,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_EDU_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE (Gubun ='2' AND SIGN = '1') ";
                SQL = SQL + ComNum.VBLF + " AND CERT = '1' ";
                SQL = SQL + ComNum.VBLF + "  AND TRUNC(Sabun) = '" + clsType.User.Sabun + "'";
                SQL = SQL + ComNum.VBLF + "  AND EntDate >= TRUNC(SYSDATE - 600 ) ";
                SQL = SQL + ComNum.VBLF + " Order By EntDate DESC";

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
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    switch (dt.Rows[i]["EduJong"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 1].Text = "병동";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 1].Text = "감염";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 1].Text = "QI";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 1].Text = "CS";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[i, 1].Text = "CPR";
                            break;
                        case "6":
                            ssView_Sheet1.Cells[i, 1].Text = "학술";
                            break;
                        case "7":
                            ssView_Sheet1.Cells[i, 1].Text = "직무";
                            break;
                        case "8":
                            ssView_Sheet1.Cells[i, 1].Text = "전직원";
                            break;
                        case "9":
                            ssView_Sheet1.Cells[i, 1].Text = "특강";
                            break;
                        case "10":
                            ssView_Sheet1.Cells[i, 1].Text = "연수";
                            break;
                        case "11":
                            ssView_Sheet1.Cells[i, 1].Text = "10대";
                            break;
                        case "12":
                            ssView_Sheet1.Cells[i, 1].Text = "보수";
                            break;
                        case "13":
                            ssView_Sheet1.Cells[i, 1].Text = "Report";
                            break;
                        case "14":
                            ssView_Sheet1.Cells[i, 1].Text = "강사";
                            break;
                        case "15":
                            ssView_Sheet1.Cells[i, 1].Text = "프리셉터";
                            break;
                        case "16":
                            ssView_Sheet1.Cells[i, 1].Text = "Cyber";
                            break;
                        case "17":
                            ssView_Sheet1.Cells[i, 1].Text = "승진자";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 1].Text = "기타";
                            break;
                    }
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["EduName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FrDate"].ToString().Trim();
                    if (dt.Rows[i]["ToDate"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 3].Text += "~" + dt.Rows[i]["ToDate"].ToString().Trim();
                    }
                    switch (dt.Rows[i]["OptTime"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[i, 4].Text = "10분";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[i, 4].Text = "30분내";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 4].Text = "1시간내";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 4].Text = "90분";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 4].Text = "2시간";
                            break;
                    }
                    if (dt.Rows[i]["EDUTime"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["EDUTime"].ToString().Trim();
                    }
                    switch (dt.Rows[i]["OptPlace"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[i, 5].Text = "마리아홀";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[i, 5].Text = "466호실";
                            break;
                    }
                    if (dt.Rows[i]["Place"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Place"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Man"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void cboManJong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtRemark.Focus();
            }
        }

        private void frmEduAttendReg_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSabun = "";

            ssView_Sheet1.Columns[7].Visible = false;

            Screen_Clear();

            cboJong.Items.Clear();
            cboJong.Items.Add(" ");
            cboJong.Items.Add("01.병동교육");
            cboJong.Items.Add("02.감염교육");
            cboJong.Items.Add("03.QI교육");
            cboJong.Items.Add("04.CS교육");
            cboJong.Items.Add("05.CPR교육");
            cboJong.Items.Add("06.학술강좌");
            cboJong.Items.Add("07.간호부주최 직무교육");
            cboJong.Items.Add("08.전직원교육");
            cboJong.Items.Add("09.특강(간협)");
            cboJong.Items.Add("10.연수교육");
            cboJong.Items.Add("11.10대질환");
            cboJong.Items.Add("12.보수교육");
            cboJong.Items.Add("13.기타Report");
            cboJong.Items.Add("14.강사활동(교육)");
            cboJong.Items.Add("15.프리셉터교육");
            cboJong.Items.Add("16.Cyber 교육");
            cboJong.Items.Add("17.승진자교육");
            cboJong.Items.Add("18.기타");
            cboJong.SelectedIndex = 0;

            cboManJong.Items.Clear();
            cboManJong.Items.Add(" ");
            cboManJong.Items.Add("01.의사");
            cboManJong.Items.Add("02.전담간호사");
            cboManJong.Items.Add("03.병동간호사");
            cboManJong.Items.Add("04.타부서의뢰");
            cboManJong.Items.Add("05.기타");
            cboManJong.Items.Add("06.간호사");

            dtpSDate.Value = Convert.ToDateTime(strSysDate).AddDays(-30);
            dtpEDate.Value = Convert.ToDateTime(strSysDate).AddDays(30);

            if (VB.Val(clsType.User.Sabun) <= 99999)
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("00000");
            }
            else
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("000000");
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //개인정보 읽기
                SQL = "";
                SQL = "SELECT c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST a,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_BUSE b,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_ERP + "INSA_CODE c ";
                SQL = SQL + ComNum.VBLF + "WHERE  a.IpsaDay<=TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + strSysDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Sabun  ='" + strSabun + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.Buse=b.BuCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Jik=c.Code(+) ";
                SQL = SQL + ComNum.VBLF + "  AND c.Gubun='2' "; //직책
                SQL = SQL + ComNum.VBLF + "ORDER BY c.Code, a.Sabun, a.KorName ";

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
                    lblInfo.Text += "[ 부서 : " + dt.Rows[0]["BuseName"].ToString().Trim() + " ] ";
                    lblInfo.Text += "[ 직책 : " + dt.Rows[0]["JikName"].ToString().Trim() + " ] ";
                    lblInfo.Text += "[ 입사일자 : " + dt.Rows[0]["IpsaDay"].ToString().Trim() + " ] ";

                    lblSts.Text += dt.Rows[0]["Buse"].ToString().Trim() + "^^";
                    lblSts.Text += dt.Rows[0]["Jik"].ToString().Trim() + "^^";
                    lblSts.Text += dt.Rows[0]["IpsaDay"].ToString().Trim() + "^^";

                    FstrBuse = dt.Rows[0]["Buse"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                EduSearch_Data();
                Search_Data();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            EduSearch_Data();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            Screen_Clear();

            string strWRTNO = "";

            strWRTNO = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (strWRTNO == "")
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WRTNO,SABUN,SNAME,BUCODE,IPSADATE,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(FrDate,'YYYY-MM-DD') FrDate,TO_CHAR(ToDate,'YYYY-MM-DD') ToDate,";
                SQL = SQL + ComNum.VBLF + "     JIKJONG,EDUJONG,EDUNAME,Remark, ";
                SQL = SQL + ComNum.VBLF + "     OptTime,EDUTIME , MAN,ManJong, OptPlace,PLACE, JUMSU ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_EDU_MST ";
                SQL = SQL + ComNum.VBLF + "  WHERE WRTNO ='" + strWRTNO + "'";

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
                    lblPMsg.Text = "참석";
                    btnDel.Visible = true;
                    btnDel.Enabled = true;
                    btnSave.Visible = false;

                    txtSeqNo.Text = dt.Rows[0]["WRTNO"].ToString().Trim();
                    cboJong.SelectedIndex = (int)VB.Val(dt.Rows[0]["EDUJONG"].ToString().Trim());
                    cboManJong.SelectedIndex = (int)VB.Val(dt.Rows[0]["ManJong"].ToString().Trim());
                    txtTopic.Text = dt.Rows[0]["EDUNAME"].ToString().Trim();
                    dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["FrDate"].ToString().Trim());
                    dtpDate2.Value = Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString().Trim());
                    if (dt.Rows[0]["OptTime"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["OptTime"].ToString().Trim())
                        {
                            case "0":
                                rdoTime0.Checked = true;
                                break;
                            case "1":
                                rdoTime1.Checked = true;
                                break;
                            case "2":
                                rdoTime2.Checked = true;
                                break;
                            case "3":
                                rdoTime3.Checked = true;
                                break;
                            case "4":
                                rdoTime4.Checked = true;
                                break;
                        }
                    }
                    txtTime.Text = dt.Rows[0]["EDUTIME"].ToString().Trim();
                    if (dt.Rows[0]["OptPlace"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["OptPlace"].ToString().Trim())
                        {
                            case "0":
                                rdoPlace0.Checked = true;
                                break;
                            case "1":
                                rdoPlace1.Checked = true;
                                break;
                        }
                    }
                    txtPlace.Text = dt.Rows[0]["Place"].ToString().Trim();
                    txtMan.Text = dt.Rows[0]["Man"].ToString().Trim();
                    txtJumsu.Text = dt.Rows[0]["Jumsu"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
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

        private void ssEdu_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssEdu_Sheet1.RowCount == 0)
            {
                return;
            }

            Screen_Clear();

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strWRTNO = "";
            string strWRTNO_B = "";
            string strSIGN = "";

            strWRTNO = ssEdu_Sheet1.Cells[e.Row, 0].Text.Trim();
            strWRTNO_B = ssEdu_Sheet1.Cells[e.Row, 8].Text.Trim();
            strSIGN = ssEdu_Sheet1.Cells[e.Row, 9].Text.Trim();

            if (strWRTNO == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT GUBUN,JONG,CODE,NAME,TO_CHAR(FrDate,'YYYY-MM-DD') FrDate,TO_CHAR(ToDate,'YYYY-MM-DD') ToDate,";
                SQL = SQL + ComNum.VBLF + " OptTime,STIME,MAN,ManJong,OptPlace,PLACE,JUMSU,ENTDATE,REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE CODE = '" + strWRTNO + "' ";

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
                    if (strWRTNO_B != "" && strSIGN != "")
                    {
                        lblPMsg.Text = "참석";
                        btnDel.Visible = true;
                        btnDel.Enabled = true;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        lblPMsg.Text = "";
                        btnDel.Visible = false;
                        btnSave.Visible = true;
                    }

                    txtSeqNo.Text = dt.Rows[0]["Code"].ToString().Trim();
                    cboJong.SelectedIndex = (int)VB.Val(dt.Rows[0]["Jong"].ToString().Trim());
                    cboManJong.SelectedIndex = (int)VB.Val(dt.Rows[0]["ManJong"].ToString().Trim());
                    txtTopic.Text = dt.Rows[0]["Name"].ToString().Trim();
                    dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["FrDate"].ToString().Trim());
                    dtpDate2.Value = Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString().Trim());
                    if (dt.Rows[0]["OptTime"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["OptTime"].ToString().Trim())
                        {
                            case "0":
                                rdoTime0.Checked = true;
                                break;
                            case "1":
                                rdoTime1.Checked = true;
                                break;
                            case "2":
                                rdoTime2.Checked = true;
                                break;
                            case "3":
                                rdoTime3.Checked = true;
                                break;
                            case "4":
                                rdoTime4.Checked = true;
                                break;
                        }
                    }
                    txtTime.Text = dt.Rows[0]["STime"].ToString().Trim();
                    if (dt.Rows[0]["OptPlace"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["OptPlace"].ToString().Trim())
                        {
                            case "0":
                                rdoPlace0.Checked = true;
                                break;
                            case "1":
                                rdoPlace1.Checked = true;
                                break;
                        }
                    }
                    txtPlace.Text = dt.Rows[0]["Place"].ToString().Trim();
                    txtMan.Text = dt.Rows[0]["Man"].ToString().Trim();
                    txtJumsu.Text = dt.Rows[0]["Jumsu"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                btnSave.Enabled = true;
                btnDel.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
