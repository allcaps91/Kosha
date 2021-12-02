using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmEduManageReg2New.cs
    /// Description     : 개인별교육등록
    /// Author          : 박창욱
    /// Create Date     : 2018-01-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리등록2New.frm(Frm교육관리등록2New.frm) >> frmEduManageReg2New.cs 폼이름 재정의" />	
    public partial class frmEduManageReg2New : Form
    {
        string strSysDate = "";

        public frmEduManageReg2New()
        {
            InitializeComponent();
        }

        private void frmEduManageReg2New_Load(object sender, EventArgs e)
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

            string strSabun = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            panInfo.Visible = false;
            ssView_Sheet1.Columns[7].Visible = false;

            Screen_Clear();

            Set_EDU_JONG();

            cboTopic.Items.Clear();
            cboTopic.Items.Add(" ");
            cboTopic.Items.Add("환자권리와 의무");
            cboTopic.Items.Add("의료윤리");
            cboTopic.Items.Add("소방안전");
            cboTopic.Items.Add("감염");
            cboTopic.Items.Add("질 향상");
            cboTopic.Items.Add("개인 정보 관리");
            cboTopic.Items.Add("CPR");
            cboTopic.Items.Add("진정교육");
            cboTopic.SelectedIndex = 0;

            cboEduPlace.Items.Clear();
            cboEduPlace.Items.Add(" ");
            cboEduPlace.Items.Add("마리아홀");
            cboEduPlace.Items.Add("마리아홀 회의실");
            cboEduPlace.Items.Add("BLS 센터");
            cboEduPlace.Items.Add("연구동 2층 회의실");
            cboEduPlace.Items.Add("병동 회의실");
            cboEduPlace.SelectedIndex = 0;

            cboManJong.Items.Clear();
            cboManJong.Items.Add(" ");
            cboManJong.Items.Add("01.의사");
            cboManJong.Items.Add("02.전담간호사");
            cboManJong.Items.Add("03.병동간호사");
            cboManJong.Items.Add("04.타부서의뢰");
            cboManJong.Items.Add("05.기타");
            cboManJong.Items.Add("06.간호사");
            cboManJong.SelectedIndex = 0;

            lblName.Text = "[등록자 : " + clsType.User.JobName + " ] 사번 : " + clsType.User.Sabun;

            if (string.Compare(clsType.User.Sabun, "99999") <= 0)
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("00000");
            }
            else
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("000000");
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //개인정보읽기
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
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                lblInfo.Text += "[ 부서 : " + dt.Rows[0]["BuseName"].ToString().Trim() + " ] ";
                lblInfo.Text += "[ 직책 : " + dt.Rows[0]["JikName"].ToString().Trim() + " ] ";
                lblInfo.Text += "[ 입사일자 : " + dt.Rows[0]["IpsaDay"].ToString().Trim() + " ] ";

                lblSts.Text += dt.Rows[0]["Buse"].ToString().Trim() + "^^";
                lblSts.Text += dt.Rows[0]["Jik"].ToString().Trim() + "^^";
                lblSts.Text += dt.Rows[0]["IpsaDay"].ToString().Trim() + "^^";

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                Search_Data();
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

        private void Screen_Clear()
        {
            txtSeqNo.Text = "";
            txtTime.Text = "";
            txtMan.Text = "";
            txtRemark.Text = "";
            txtJumsu.Text = "";
            txtROWID.Text = "";

            cboJong.SelectedIndex = -1;
            cboManJong.SelectedIndex = -1;
            cboEduPlace.Text = "";
            cboTopic.Text = "";

            rdoEduBun1.Checked = false;
            rdoEduBun2.Checked = false;

            rdoTime0.Checked = false;
            rdoTime1.Checked = false;
            rdoTime2.Checked = false;
            rdoTime3.Checked = false;
            rdoTime4.Checked = false;
            rdoTime5.Checked = false;
            rdoTime6.Checked = false;

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (Delete_Data() == true)
            {
                Screen_Clear();

                Search_Data();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string strSEQNO = "";

            Screen_Clear();

            strSEQNO = Read_NUR_EDU_SEQ();

            btnSave.Enabled = true;

            txtSeqNo.Text = strSEQNO;

            cboTopic.Focus();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            panInfo.Visible = true;
        }

        string Read_NUR_EDU_SEQ()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT " + ComNum.DB_PMPA + "SEQ_NUR_EDU_SEQ.NEXTVAL WRTNO FROM DUAL ";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                rtnVar = strSysDate.Replace("-", "") + VB.Val(dt.Rows[0]["WRTNO"].ToString().Trim()).ToString("000");

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        bool Delete_Data()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strWRTNO = "";

            strWRTNO = txtSeqNo.Text.Trim();

            if (ComFunc.MsgBoxQ("자료를 정말로 삭제하시겠습니까?") == DialogResult.No)
            {
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = "DELETE " + ComNum.DB_PMPA + "NUR_EDU_MST WHERE WRTNO='" + strWRTNO + "' ";

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
                ComFunc.MsgBox("삭제 완료");
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

        bool Save_Data()
        {
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
            string strMan = "";
            string strPlace = "";
            string strJumsu = "";
            string strJong = "";
            string strSName = "";
            string strRemark = "";
            string strManJong = "";
            string StrDate1 = "";
            string StrDate2 = "";
            string strTIME = "";
            string strTIME_REMARK = "";
            string strREQUIRE = "";

            strManJong = VB.Left(cboJong.Text.Trim(), 2);

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

            strSabun = clsType.User.Sabun;
            strSName = clsType.User.JobName;
            strBuCode = VB.Pstr(lblSts.Text, "^^", 1).Trim();
            strJik = VB.Pstr(lblSts.Text, "^^", 2).Trim();
            strIpSaDay = VB.Pstr(lblSts.Text, "^^", 3).Trim();

            strTopic = cboTopic.Text.Trim();
            StrDate1 = dtpDate.Value.ToString("yyyy-MM-dd");
            StrDate2 = dtpDate2.Value.ToString("yyyy-MM-dd");

            if (rdoTime0.Checked == true)
            {
                strTIME = "10";
            }
            else if (rdoTime1.Checked == true)
            {
                strTIME = "20";
            }
            else if (rdoTime2.Checked == true)
            {
                strTIME = "30";
            }
            else if (rdoTime3.Checked == true)
            {
                strTIME = "60";
            }
            else if (rdoTime4.Checked == true)
            {
                strTIME = "90";
            }
            else if (rdoTime5.Checked == true)
            {
                strTIME = "120";
            }
            else if (rdoTime6.Checked == true)
            {
                strTIME = "480";
            }

            if (rdoEduBun1.Checked == true)
            {
                strREQUIRE = "0";
            }
            else if (rdoEduBun2.Checked == true)
            {
                strREQUIRE = "1";
            }

            strTIME_REMARK = txtTime.Text.Trim();

            strPlace = cboEduPlace.Text.Trim();

            strMan = txtMan.Text.Trim();
            strRemark = txtRemark.Text.Trim();
            strJumsu = txtJumsu.Text.Trim();
            strJong = VB.Left(cboJong.Text.Trim(), 2);
            strManJong = VB.Left(cboManJong.Text.Trim(), 2);

            strROWID = txtROWID.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID == "")
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_EDU_MST (";
                    SQL = SQL + ComNum.VBLF + " WRTNO, SABUN, SNAME, BUCODE, ";
                    SQL = SQL + ComNum.VBLF + " IPSADATE, SDATE, JIKJONG, EDUJONG, ";
                    SQL = SQL + ComNum.VBLF + " EDUNAME, FRDATE, TODATE, EDUTIME, ";
                    SQL = SQL + ComNum.VBLF + " EDUTIME_REMARK, MAN, MANJONG, PLACE, ";
                    SQL = SQL + ComNum.VBLF + "  REMARK,JUMSU, GUBUN, ENTDATE, ";
                    SQL = SQL + ComNum.VBLF + "  SIGN, REQUIRE) VALUES (";
                    SQL = SQL + ComNum.VBLF + "'" + strWRTNO + "','" + strSabun.PadLeft(5, '0') + "','" + strSName + "','" + strBuCode + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strIpSaDay + "', TO_DATE('" + strSysDate + "','YYYY-MM-DD') , '" + strJik + "','" + strJong + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strTopic + "', TO_DATE('" + StrDate1 + "','YYYY-MM-DD'),TO_DATE('" + StrDate2 + "','YYYY-MM-DD'),'" + strTIME + "',  ";
                    SQL = SQL + ComNum.VBLF + " '" + strTIME_REMARK + "','" + strMan + "','" + strManJong + "','" + strPlace + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strRemark + "', '" + strJumsu + "', '1', SYSDATE,  ";
                    SQL = SQL + ComNum.VBLF + "'1','" + strREQUIRE + "' ) ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_EDU_MST SET ";
                    SQL = SQL + ComNum.VBLF + " JikJong ='" + strJik + "', ";
                    SQL = SQL + ComNum.VBLF + " BuCode ='" + strBuCode + "',";
                    SQL = SQL + ComNum.VBLF + " EDUJONG ='" + strJong + "', ";
                    SQL = SQL + ComNum.VBLF + " EDUNAME ='" + strTopic + "', ";
                    SQL = SQL + ComNum.VBLF + " FrDate =TO_DATE('" + StrDate1 + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " ToDate =TO_DATE('" + StrDate2 + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " EDUTIME ='" + strTIME + "', ";
                    SQL = SQL + ComNum.VBLF + " EDUTIME_REMARK ='" + strTIME_REMARK + "', ";
                    SQL = SQL + ComNum.VBLF + " MAN ='" + strMan + "', ";
                    SQL = SQL + ComNum.VBLF + " ManJong ='" + strManJong + "', ";
                    SQL = SQL + ComNum.VBLF + " Remark ='" + strRemark + "',";
                    SQL = SQL + ComNum.VBLF + " PLACE ='" + strPlace + "', ";
                    SQL = SQL + ComNum.VBLF + " REQUIRE ='" + strREQUIRE + "', ";
                    SQL = SQL + ComNum.VBLF + " JUMSU ='" + strJumsu + "' ";
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
                ComFunc.MsgBox("저장 완료");
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

        private void Search_Data()
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
                SQL = "SELECT FRDATE, TODATE, DECODE(REQUIRE, '0', '필수', '비필수') REQUIRE, EDUJONG, EDUNAME, MAN, PLACE, EDUTIME, EDUTIME_REMARK, ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_MST";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='1'";
                SQL = SQL + ComNum.VBLF + "   AND SABUN = " + clsType.User.Sabun;
                SQL = SQL + ComNum.VBLF + " ORDER BY EntDate DESC";

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
                    ssView_Sheet1.Cells[i, 0].Text = VB.Left(dt.Rows[i]["FRDATE"].ToString().Trim(), 10);
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["REQUIRE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = Read_EDU_JONG(dt.Rows[i]["EDUJONG"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EDUNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MAN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PLACE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = Read_EDU_TIME(dt.Rows[i]["EDUTIME"].ToString().Trim(), dt.Rows[i]["EDUTIME_REMARK"].ToString().Trim());
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

        private string Read_EDU_TIME(string arg1, string arg2)
        {
            string strTIME = "";
            string strSECOND = "";
            string rtnVar = "";

            if (arg1 != "" && VB.Split(arg1, "시간").Length > 0)
            {
                rtnVar = arg1;
                return rtnVar;
            }

            strTIME = "";
            strSECOND = "";

            if (VB.Fix((int)VB.Val(arg1) / 60) > 0)
            {
                strTIME = VB.Fix((int)VB.Val(arg1) / 60) + "시간" ;
            }
            if (VB.Val(arg1) % 60 > 0)
            {
                strSECOND = VB.Val(arg1) % 60 + "분";
            }
            rtnVar = strTIME + strSECOND;

            if (rtnVar == "")
            {
                rtnVar = arg2;
            }

            return rtnVar;
        }

        string Read_EDU_JONG(string argJong)
        {
            switch (argJong)
            {
                case "01":
                    return "병동교육";
                case "02":
                    return "감염교육";
                case "03":
                    return "QI교육";
                case "04":
                    return "CS교육";
                case "05":
                    return "CPR교육";
                case "06":
                    return "학술강좌";
                case "07":
                    return "간호부주최 직무교육";
                case "08":
                    return "전직원교육";
                case "09":
                    return "특강(간협)";
                case "10":
                    return "연수교육";
                case "11":
                    return "10대질환";
                case "12":
                    return "보수교육";
                case "13":
                    return "기타Report";
                case "14":
                    return "강사활동(교육)";
                case "15":
                    return "프리셉터교육";
                case "16":
                    return "Cyber 교육";
                case "17":
                    return "승진자교육";
                case "18":
                    return "기타";
            }

            return "";
        }

        private void cboManJong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboEduPlace.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboTopic_TextChanged(object sender, EventArgs e)
        {
            if (cboTopic.Text.Trim() != "")
            {
                rdoEduBun2.Checked = true;
            }
        }

        private void cboTopic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTopic.SelectedIndex > 0)
            {
                rdoEduBun1.Checked = true;
            }
        }

        void Set_EDU_JONG()
        {
            cboJong.Items.Clear();
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
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            txtROWID.Text = ssView_Sheet1.Cells[e.Row, 7].Text.Trim();

            try
            {
                SQL = "";
                SQL = " SELECT GUBUN, EDUJONG, WRTNO, EDUNAME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE, TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, MAN, MANJONG,";
                SQL = SQL + ComNum.VBLF + " PLACE, JUMSU, ENTDATE, REMARK, ";
                SQL = SQL + ComNum.VBLF + "  EDUTIME, EDUTIME_REMARK, REQUIRE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_EDU_MST ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID ='" + txtROWID.Text + "'";

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

                txtSeqNo.Text = dt.Rows[0]["WRTNO"].ToString().Trim();
                cboJong.SelectedIndex = (int)VB.Val(dt.Rows[0]["EDUJONG"].ToString().Trim()) - 1; //시작이 0부터라서 -1 추가함 2019-09-20
                cboManJong.SelectedIndex = (int)VB.Val(dt.Rows[0]["ManJong"].ToString().Trim());
                cboTopic.Text = dt.Rows[0]["EDUNAME"].ToString().Trim();
                dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["FrDate"].ToString().Trim());
                dtpDate2.Value = Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString().Trim());

                if (dt.Rows[0]["Require"].ToString().Trim() == "0")
                {
                    rdoEduBun1.Checked = true;
                }
                else if (dt.Rows[0]["Require"].ToString().Trim() == "1")
                {
                    rdoEduBun2.Checked = true;
                }

                switch (dt.Rows[0]["EDUTIME"].ToString().Trim())
                {
                    case "10":
                        rdoTime0.Checked = true;
                        break;
                    case "20":
                        rdoTime1.Checked = true;
                        break;
                    case "30":
                        rdoTime2.Checked = true;
                        break;
                    case "60":
                        rdoTime3.Checked = true;
                        break;
                    case "90":
                        rdoTime4.Checked = true;
                        break;
                    case "120":
                        rdoTime5.Checked = true;
                        break;
                    case "480":
                        rdoTime6.Checked = true;
                        break;
                }

                txtTime.Text = dt.Rows[0]["EDUTIME_REMARK"].ToString().Trim();

                cboEduPlace.Text = dt.Rows[0]["Place"].ToString().Trim();
                txtMan.Text = dt.Rows[0]["Man"].ToString().Trim();
                txtJumsu.Text = dt.Rows[0]["Jumsu"].ToString().Trim();
                txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnInfoExit_Click(object sender, EventArgs e)
        {
            panInfo.Visible = false;
        }

        private void txtJumsu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtMan.Focus();
            }
        }

        private void txtMan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboManJong.Focus();
            }
        }
    }
}
