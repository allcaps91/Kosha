using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Text;

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSMS_Misu
    /// File Name : frmSMS_Misu.cs
    /// Title or Description : 개별문자전송
    /// Author : 박창욱
    /// Create Date : 2017-06-14
    /// Update Histroy : 2018-12-06 이현종 SMS_NEW로 수정.
    /// </summary>
    /// <history>  
    /// VB\FrmSMS_Misu.frm(FrmSMS_Misu) -> frmSMS_Misu.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\misu\FrmSMS_Misu.frm(FrmSMS_Misu)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\misu\misuper.vbp
    /// </vbp>
    public partial class frmSMS_Misu : Form
    {
        private string gstrRetValue = "";
        private string gnJobSabun = "";

        string FstrSabun = "";
        string FstrYYMM = "";
        int FintMMS = 0;
        int strWonMu = 0;
        string FstrHphone = "";

        public frmSMS_Misu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 의료정보팀 용
        /// </summary>
        /// <param name="strSabun">보낼 사번</param>
        public frmSMS_Misu(string strSabun)
        {
            InitializeComponent();
            FstrSabun = strSabun;
        }

        public frmSMS_Misu(string strRetValue, string strJobSabun)
        {
            InitializeComponent();
            gstrRetValue = strRetValue;
            gnJobSabun = strJobSabun;
        }

        public frmSMS_Misu(int strFintMMS, string strHphone)
        {
            InitializeComponent();
            strWonMu = strFintMMS;
            FstrHphone = strHphone;
        }

        private void frmSMS_Misu_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회 
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //2020-09-18 원무과 전용 80자이상사용가능
            if(strWonMu == 1)
            {
                //원무팀은 80이상 사용가능
                FintMMS = 1;
            }
            else
            {
                GetMMS();
            }
            

            string strYYMM_1 = "";

            screen_Clear();
            ComFunc.ReadSysDate(clsDB.DbCon);
            FstrYYMM = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2); //당월
            strYYMM_1 = clsVbfunc.DateYYMMAdd(FstrYYMM, -1);    //전월

            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            txtTime.Text = clsPublic.GstrSysTime;

            txtSabun.Text = string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber.ToString() : FstrSabun;
            txtName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, (string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber.ToString().Trim() : FstrSabun));

            ssTel_Sheet1.Columns[4].Visible = false;

            if (gstrRetValue != "")
            {
                txtTel.Text = VB.Pstr(gstrRetValue, "^^", 1).Trim();
            }

            //2020-10-07 원무팀 김윤희 선생님 요청
            if(FstrHphone != "")
            {
                txtTel.Text = FstrHphone;
            }

            //txtRetTel.Text = string.IsNullOrEmpty(FstrSabun) ? "054-260-8104" : "";

            if (txtSabun.Text.Trim().Equals("36540") == false && txtSabun.Text.Trim().Equals("29116") == false && txtSabun.Text.Trim().Equals("36282") == false)
            {
                panApp.Visible = false;
                chkApp.Visible = false;
                btnSearchApp.Visible = false;
                btnSaveApp.Visible = false;
            }
            //박시철팀장 부재로 함종현계장 추가함(2021-06-26) 의뢰 올린다고 하였음~
            if (txtSabun.Text.Trim().Equals("36540") == false && txtSabun.Text.Trim().Equals("19684") == false && txtSabun.Text.Trim().Equals("27111") == false && txtSabun.Text.Trim().Equals("12306") == false && txtSabun.Text.Trim().Equals("20175") == false)
            {
                btnSearchDoctor.Visible = false;
            }

            if (clsType.User.BuseCode != "070101")
            {
                PanPR.Visible = false;
            }
            else
            {
                //'전화번호부 종류를 READ
                SetPR();
            }

            ssView_Sheet1.RowCount = 300; // 2020-12-01 조성근 선생님 요청으로 50 -> 300 변경
            ssListFind_Sheet1.RowCount = 0;            

            //직원검색 2020-09-09
            //switch (clsType.User.IdNumber)
            //{
            //    case "45316":
            //        Width = Width - panRight.Width - 200;
            //        break;
            //    default:
            //        Width = Width - panRight.Width - 200;
            //        break;
            //}

            Width = Width - panRight.Width - 218;
            panRight.Visible = false;

            //2020-10-07
            if(strWonMu == 1)
            {
                //수신번호 받아올시 원무팀 미수 번호 입력
                txtRetTel.Text = "054-260-8103";
            }
            {
                txtRetTel.Focus();
            }

        }

        private void SetPR()
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            SQL.AppendLine("SELECT CODE, NAME FROM " + ComNum.DB_PMPA + "BAS_BCODE");
            SQL.AppendLine("WHERE GUBUN = '기획홍보팀_전화번호부'");
            SQL.AppendLine("ORDER BY CODE");

            SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CboPR.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            CboPR.DropDownStyle = ComboBoxStyle.DropDownList;
            CboPR.SelectedIndex = 0;
        }

        void GetMMS()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL.AppendLine("SELECT SABUN, MMS");
                SQL.AppendLine("FROM " + ComNum.DB_PMPA + "REPORT_SMS");
                SQL.AppendLine("WHERE SABUN = '" + (string.IsNullOrEmpty(FstrSabun) ? clsType.User.Sabun : FstrSabun) + "'");
                SQL.AppendLine("  AND DELDATE IS NULL");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                FintMMS = (int)VB.Val(dt.Rows[0]["MMS"].ToString().Trim());

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        private void screen_Clear()
        {
            txtTel.Text = "";
            txtRetTel.Text = "";
            txtMsg.Text = "";
            txtSabun.Text = "";
            txtName.Text = "";
            txtCName.Text = "";
            txtCTel.Text = "";
            txtROWID.Text = "";
            txtGroup.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                for (i = 0; i < ssTel_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssTel_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL = " DELETE ADMIN.KMS_TELNO ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ssTel_Sheet1.Cells[i, 4].Text + "' ";
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
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                search();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (panRight.Visible == false) return;

            //직원검색 2020-09-09
            switch (clsType.User.IdNumber)
            {
                case "45316":
                case "48603":
                case "4636":
                case "16092"://오재국팀장
                case "36550"://김성준
                case "36765"://문선미
                case "55241": //기획홍보팀신규직원
                    Width = Width - panRight.Width - 217;
                    break;
                default:
                    Width = Width - panRight.Width;
                    break;
            }
            
            panRight.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (txtCName.Text == "")
            {
                return;
            }
            if (txtCTel.Text == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                if (txtROWID.Text == "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "KMS_TELNO ";
                    SQL = SQL + ComNum.VBLF + "        (NAME, HPHONE, BUSEGBN, BUCODE, GROUP_NAME) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" + txtCName.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + txtCTel.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "         '2', ";
                    //18-12-05 의료정보팀 이동춘팀장님 요청으로 수정
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Val((string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber : FstrSabun)).ToString("000000") + "' ,";
                    SQL = SQL + ComNum.VBLF + "         '" + txtGroup.Text.Trim() + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "KMS_TELNO SET ";
                    SQL = SQL + ComNum.VBLF + "   NAME          = '" + txtCName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "   HPHONE        = '" + txtCTel.Text + "',";
                    SQL = SQL + ComNum.VBLF + "   GROUP_NAME    = '" + txtGroup.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID    = '" + txtROWID.Text.Trim() + "'";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            search();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            search();
        }

        private void search()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssTel_Sheet1.RowCount = 0;

            string strGroup = txtGroup.Text.Trim();
            string strCname = txtCName.Text.Trim();

            txtCName.Text = "";
            txtCTel.Text = "";
            txtGroup.Text = "";
            txtROWID.Text = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT NAME, HPHONE, GROUP_NAME, ROWID ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.KMS_TELNO ";
                SQL = SQL + ComNum.VBLF + "  WHERE BUCODE   = '" + VB.Val((string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber : FstrSabun)).ToString("000000") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BUSEGBN  = '2' ";

                if (string.IsNullOrWhiteSpace(strCname) == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND NAME  LIKE '%" + strCname + "%' ";
                }

                if (string.IsNullOrWhiteSpace(strGroup) == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND GROUP_NAME LIKE '%" + strGroup + "%' ";
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY GROUP_NAME, NAME ";
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

                ssTel_Sheet1.RowCount = dt.Rows.Count;
                ssTel_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssTel_Sheet1.Cells[i, 0].Text = "";
                    ssTel_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssTel_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HPHONE"].ToString().Trim();
                    ssTel_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GROUP_NAME"].ToString().Trim();
                    ssTel_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void TextSearch()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssList1_Sheet1.RowCount = 2;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT SABUN, RESULT, ROWID ";
            SQL = SQL + ComNum.VBLF + "   FROM ADMIN.ETC_SMS_RESULT ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList1_Sheet1.RowCount = dt.Rows.Count + 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RESULT"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    //Row 높이 설정 2020-08-26 
                    FarPoint.Win.Spread.Row row;
                    row = ssList1.ActiveSheet.Rows[i];
                    float rowSize = row.GetPreferredHeight();
                    row.Height = rowSize;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtCName.Text = "";
            txtGroup.Text = "";
            txtROWID.Text = "";
            txtCTel.Text = "";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            panRight.Visible = true;

            //2020-12-29  상용구 비활성화
            panel12.Visible = false;

            //직원검색 2020-09-09
            switch (clsType.User.IdNumber)
            {
                case "45316":
                case "48603":
                case "4636":
                case "16092"://오재국팀장
                case "36550"://김성준
                case "36765"://문선미
                case "55241"://기획홍보팀 신규직원
                    if(clsType.User.IdNumber =="48603" || clsType.User.IdNumber == "45316" || clsType.User.IdNumber == "55241")
                    {
                        panel12.Visible = true;
                    }
                    Width = 1020;
                    break;
                default:
                    Width = 803;
                    break;

            }  
            
            search();
            TextSearch();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string strTxtMsg = "";
            string strRTime = "";
            int nRead = 0;
            long nCnt1 = 0;
            long nCNT2 = 0;
            string strYYMM = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            ComFunc.ReadSysDate(clsDB.DbCon);
            strYYMM = Convert.ToDateTime(clsPublic.GstrSysDate).ToString("YYYYMM");

            if (txtTel.Text.Trim() == "")
            {
                ComFunc.MsgBox("휴대전화 번호를 입력해주세요.", "오류");
                txtTel.Focus();
                return;
            }
            if (txtMsg.Text.Trim() == "")
            {
                ComFunc.MsgBox("메시지를 입력해주세요.", "오류");
                txtMsg.Focus();
                return;
            }
            if (txtRetTel.Text.Trim() == "")
            {
                ComFunc.MsgBox("회신번호를 입력해주세요.", "오류");
                txtRetTel.Focus();
                return;
            }

            //휴대전화 오류 점검
            switch (VB.Left(txtTel.Text, 3))
            {
                case "010":
                case "011":
                case "016":
                case "017":
                case "018":
                case "019":
                    break;
                default:
                    ComFunc.MsgBox("휴대전화 번호만 가능합니다.");
                    return;
            }

            strTxtMsg = txtMsg.Text;
            if (FintMMS == 0)
            {
                //   if(Encoding.UTF8.GetByteCount(strTxtMsg) > 90)
                //   {
                //        ComFunc.MsgBox("메시지는 90자까지만 가능합니다.");
                //        txtMsg.Focus();
                //        return;
                //    }
            }

            if (chkYeyak.Checked == true)
            {
                strRTime = dtpDate.Value.ToString("yyyy-MM-dd ") + txtTime.Text.Trim();

                if (Convert.ToDateTime(strRTime) <= Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")))
                {
                    ComFunc.MsgBox("현재시간 이후로만 예약전송이 가능합니다.");
                    return;
                }
            }
            else
            {
                strRTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "        (JobDate,Hphone,Gubun, ";
                SQL = SQL + ComNum.VBLF + "         Rettel,SendMsg,EntSabun, ";
                SQL = SQL + ComNum.VBLF + "         EntDate) ";
                SQL = SQL + ComNum.VBLF + " VALUES (TO_DATE('" + strRTime + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + "         '" + txtTel.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '10', ";
                SQL = SQL + ComNum.VBLF + "         '" + txtRetTel.Text.Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strTxtMsg + "', ";
                //18-12-05 의료정보팀 이동춘팀장님 요청으로 수정
                SQL = SQL + ComNum.VBLF + "         '" + (string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber : FstrSabun) + "', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    txtTel.Focus();
                    return;
                }

                //전송건수를 누적
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sabun FROM KMS_SMS WHERE Sabun= '" + (string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber : FstrSabun) + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                nRead = dt.Rows.Count;
                if (nRead == 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO KMS_SMS (Sabun, YYMM, SmsCNT1, SmsCNT2) ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" + (string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber : FstrSabun) + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strYYMM + "', ";
                    SQL = SQL + ComNum.VBLF + "          " + nCnt1 + ", ";
                    SQL = SQL + ComNum.VBLF + "          " + nCNT2 + ") ";
                }
                else if (nRead == 1)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE KMS_SMS SET SmsCNT2 = " + nCNT2 + " ";
                    SQL = SQL + ComNum.VBLF + "  WHERE Sabun = '" + (string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber : FstrSabun) + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    txtTel.Focus();
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("개별전송이 완료되었습니다.");
                Cursor.Current = Cursors.Default;
                txtTel.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSend2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string strTxtTel = "";
            string strTxtMsg = "";
            string strSname = "";
            string strRTime = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strYYMM = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strYYMM = Convert.ToDateTime(clsPublic.GstrSysDate).ToString("YYYYMM");
            if (txtMsg.Text.Trim() == "")
            {
                ComFunc.MsgBox("메시지를 입력해주세요.", "오류");
                txtMsg.Focus();
                return;
            }
            if (txtRetTel.Text.Trim() == "")
            {
                ComFunc.MsgBox("회신번호를 입력해주세요.", "오류");
                txtRetTel.Focus();
                return;
            }
            strTxtMsg = txtMsg.Text;

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (chkYeyak.Checked == true)
            {
                strRTime = dtpDate.Value.ToString("yyyy-MM-dd").Trim() + " " + txtTime.Text.Trim();

                if (Convert.ToDateTime(strRTime) <= Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime))
                {
                    ComFunc.MsgBox("현재시간 이후로만 예약전송이 가능합니다.");
                    return;
                }
            }
            else
            {
                strRTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strTxtTel = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strSname = ssView_Sheet1.Cells[i, 1].Text.Trim();

                    if (strTxtTel != "")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO ETC_SMS ";
                        SQL = SQL + ComNum.VBLF + "        (JobDate,Hphone,Gubun,";
                        SQL = SQL + ComNum.VBLF + "         Rettel,SendMsg,EntSabun, ";
                        SQL = SQL + ComNum.VBLF + "         EntDate,Sname)"; 
                        SQL = SQL + ComNum.VBLF + " VALUES (TO_DATE('" + strRTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL = SQL + ComNum.VBLF + "         '" + strTxtTel + "', ";
                        SQL = SQL + ComNum.VBLF + "         '10', ";
                        SQL = SQL + ComNum.VBLF + "         '" + txtRetTel.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strTxtMsg + "', ";
                        //18-12-05 의료정보팀 이동춘팀장님 요청으로 수정
                        SQL = SQL + ComNum.VBLF + "         '" + (string.IsNullOrEmpty(FstrSabun) ? clsType.User.IdNumber : FstrSabun) + "', "; 
                        SQL = SQL + ComNum.VBLF + "         SYSDATE, '" + strSname + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            txtTel.Focus();
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("일괄 전송이 완료되었습니다.");
                Cursor.Current = Cursors.Default;
                txtTel.Focus();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }



        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            //ssView_Sheet1.Rows[e.Row].Remove();
        }

        private void ssTel_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssTel_Sheet1.RowCount == 0) { return; }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssTel, e.Column);
                return;
            }

            ssTel_Sheet1.Cells[0, 0, ssTel_Sheet1.RowCount - 1, ssTel_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssTel_Sheet1.Cells[e.Row, 0, e.Row, ssTel_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            int i = 0;
            string strTel = "";
            string strCName = "";

            if (e.Row < 0)
            {
                return;
            }

            if (e.Column == 2)
            {
                if (ssTel_Sheet1.Cells[e.Row, 1].Text == "")
                {
                    return;
                }
                strTel = ssTel_Sheet1.Cells[e.Row, 2].Text.Trim();
                strCName = ssTel_Sheet1.Cells[e.Row, 1].Text.Trim();

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Cells[i, 0].Text == "")
                    {
                        ssView_Sheet1.RowCount += 1;
                        ssView_Sheet1.Cells[i, 0].Text = strTel;
                        ssView_Sheet1.Cells[i, 1].Text = strCName;
                        break;
                    }
                }
            }
            else if (e.Column == 3)
            {
                if (ssTel_Sheet1.Cells[e.Row, 1].Text == "")
                {
                    return;
                }
                txtCName.Text = ssTel_Sheet1.Cells[e.Row, 1].Text.Trim();
                txtCTel.Text = ssTel_Sheet1.Cells[e.Row, 2].Text.Trim();
                txtGroup.Text = ssTel_Sheet1.Cells[e.Row, 3].Text.Trim();
                txtROWID.Text = ssTel_Sheet1.Cells[e.Row, 4].Text.Trim();
            }
        }

        private void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtTime.Focus();
            }
        }

        private void txtMsg_TextChanged(object sender, EventArgs e)
        {
            //lblCount.Text = Encoding.UTF8.GetByteCount(txtMsg.Text).ToString() + "/80";
            lblCount.Text = (System.Text.Encoding.Default.GetByteCount(txtMsg.Text)).ToString() + "/80";

            int strbyte = System.Text.Encoding.Default.GetByteCount(txtMsg.Text);

            if (FintMMS == 0 && strbyte > 80)
            {
                ComFunc.MsgBox("80자 이상 문자는 전송이 안됩니다.");
                txtMsg.Text = txtMsg.Text.Remove(txtMsg.Text.Length - 1); 
            }
            return;
        }

        private void btnSearchApp_Click(object sender, EventArgs e)
        {
            GetSearchApp();
        }

        void GetSearchApp()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                #region 전문의
                if (chk1.Checked)
                {
                    SQL.Clear();
                    SQL.AppendLine("SELECT HTEL, KORNAME");
                    SQL.AppendLine("FROM " + ComNum.DB_ERP + "INSA_MST ");
                    SQL.AppendLine("WHERE BUSE LIKE '01%' ");
                    SQL.AppendLine("AND TOIDAY IS NULL ");

                    SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HTEML"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region 전체
                if (chk2.Checked)
                {
                    SQL.Clear();
                    SQL.AppendLine("SELECT HTEL, KORNAME");
                    SQL.AppendLine("FROM " + ComNum.DB_ERP + "INSA_MST ");
                    SQL.AppendLine("WHERE BUSE >= 030000 ");
                    SQL.AppendLine("AND TOIDAY IS NULL ");

                    SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HTEML"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region 전공의
                if (chk3.Checked)
                {
                    SQL.Clear();
                    SQL.AppendLine("SELECT HTEL, KORNAME");
                    SQL.AppendLine("FROM " + ComNum.DB_ERP + "INSA_MST ");
                    SQL.AppendLine("WHERE BUSE LIKE '%02%' ");
                    SQL.AppendLine("AND TOIDAY IS NULL ");

                    SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HTEML"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSaveApp_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (string.IsNullOrEmpty(txtMsg.Text.Trim()))
            {
                ComFunc.MsgBox("문자메시지가 공란입니다.");
                txtMsg.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtRetTel.Text.Trim()))
            {
                ComFunc.MsgBox("회신번호가 공란입니다.");
                txtRetTel.Focus();
                return;
            }

            if (Encoding.Default.GetByteCount(txtMsg.Text.Trim()) > 80)
            {
                if (FintMMS == 0)
                {
                    ComFunc.MsgBox("메시지는 80자까지만 가능합니다.", "확인");
                    return;
                }
            }

            string strRTime = dtpDate.Value.ToString("yyyy-MM-dd").Trim() + " " + txtTime.Text.Trim();
            if (chkYeyak.Checked)
            {
                if (Convert.ToDateTime(strRTime) <= Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime))
                {
                    ComFunc.MsgBox("현재시간 이후로만 예약전송이 가능합니다.");
                    return;
                }
            }
            else
            {
                strRTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            }

            if (chkApp.Checked)
            {
                //if(APP_PUSH_TEST() == false)
                //{
                //    return;
                //}
            }
            else
            {
                if (Save_Data(strRTime) == false)
                {
                    return;
                }
            }

            ComFunc.MsgBox("일괄전송이 완료되었습니다");
            return;
        }

        bool Save_Data(string strRTime)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            string strTxtTel = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strTxtTel = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    if (string.IsNullOrEmpty(strTxtTel) == false)
                    {
                        SQL.Clear();
                        SQL.AppendLine(" INSERT INTO ETC_SMS(JobDate,Hphone,Gubun,");
                        SQL.AppendLine("  Rettel,SendMsg,EntSabun,EntDate)");
                        SQL.AppendLine(" VALUES ( TO_DATE('" + strRTime + "','YYYY-MM-DD HH24:MI'),'" + strTxtTel + "',");
                        SQL.AppendLine("'10', '" + txtRetTel.Text.Trim() + "','" + txtMsg.Text.Trim() + "'," + clsType.User.Sabun + ",SYSDATE) ");

                        SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                            txtTel.Focus();
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool APP_PUSH_TEST()
        {
            DataTable dt = null;
            StringBuilder SQL = new StringBuilder();
            string strTxtTel = string.Empty;
            string strPano = string.Empty;
            bool rtnVal = false;

            clsDbMySql.DBConnect("", "", "psmh", "psmh", "psmh2");
            clsDbMySql.setBeginTran();

            try
            {
                for (int i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strTxtTel = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    if (string.IsNullOrEmpty(strTxtTel) == false)
                    {
                        SQL.Clear();
                        SQL.AppendLine("SELECT m_ptno   FROM tb_patbav");
                        SQL.AppendLine(" WHERE M_TELNO = '" + clsSHA.SHA256(strTxtTel) + "'");
                        SQL.AppendLine("   AND M_GCMKEY IS NOT NULL");
                        dt = clsDbMySql.GetDataTable(SQL.ToString().Trim());
                        if (dt.Rows.Count > 0)
                        {
                            strPano = dt.Rows[0]["M_PTNO"].ToString().Trim();


                            SQL.Clear();
                            SQL.AppendLine(" INSERT INTO tb_mrappmmo(pid,resdate,deptcode,appnote,sendyn,senddate,rightnow,appemp,appdate");
                            SQL.AppendLine(" ,updemp,upddate,mrappmmoid,readyn,category,rtry_cnt,apptitle,callback, msgtype, webflag)");
                            SQL.AppendLine(" VALUES ('" + strPano + "','','00000','" + txtMsg.Text.Trim() + "','N','','Y','WEB',NOW(),'WEB',NOW()");
                            SQL.AppendLine(" , (SELECT MAX(mrappmmoid)+1 FROM tb_mrappmmo t1) ,'N','NOTI','1','포항성모병원[알림]','','t', '')");
                            clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim());
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }
                clsDbMySql.setCommitTran();
                rtnVal = true;
            }
            catch
            {

            }
            finally
            {
                clsDbMySql.DisDBConnect();
            }
            return rtnVal;
        }

        private void btnSearchDoctor_Click(object sender, EventArgs e)
        {
            GetSearchDoctor();
        }

        /// <summary>
        /// 문자보낼 의사 - 인사정보
        /// </summary>
        void GetSearchDoctor()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL.AppendLine("SELECT D.HTEL, C.DRNAME SName ");
                SQL.AppendLine(" FROM  " + ComNum.DB_MED + "OCS_DOCTOR c,  " + ComNum.DB_ERP + "INSA_MST D ");
                SQL.AppendLine(" Where C.SABUN = D.SABUN ");
                SQL.AppendLine("  AND D.TOIDAY IS NULL ");
                SQL.AppendLine("  AND c.Gbout  ='N' ");
                SQL.AppendLine("  AND c.Grade  ='1' ");
                SQL.AppendLine("  AND substr(c.drcode,3,2) <>'99'"); // 전체과 제외
                SQL.AppendLine("  AND c.drcode <>'0104' "); //김중구소장 제외
                SQL.AppendLine("  AND c.deptcode not in ( 'DT','ER','FM','LM','OM','PC','RD')"); // '치과,응급의학,가정의학,진단검사,건진,통증치료,영상의학
                SQL.AppendLine("  AND d.HTel IS NOT NULL "); //폰있는것만

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 30;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HTEL"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearchBuse_Click(object sender, EventArgs e)
        {
            GetSearchBuse();
        }

        /// <summary>
        /// 문자보낼 부서장 - 인사정보( ETC_PROGSIGN 부서결제자 참조)
        /// </summary>
        void GetSearchBuse()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL.AppendLine("SELECT A.KORNAME, A.HTEL ");
                SQL.AppendLine(" FROM  " + ComNum.DB_ERP + "INSA_MST A,  " + ComNum.DB_PMPA + "ETC_PROGSIGN B "); 
                SQL.AppendLine(" Where a.SABUN = b.SABUN2 ");
                SQL.AppendLine("  AND B.SABUN2 > '0' ");
                //SQL.AppendLine("  AND B.BUCODE <> '088201' "); //장례식장 제외
                SQL.AppendLine("  AND A.TOIDAY IS NULL ");
                SQL.AppendLine("  AND A.HTel IS NOT NULL");
                SQL.AppendLine("  GROUP BY A.KORNAME, A.HTEL");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 30;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HTEL"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssTel_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == false)
            {
                return;
            }

            clsSpread.gSpdSortRow(ssTel, e.Column);
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == false)
            {
                return;
            }

            clsSpread.gSpdSortRow(ssView, e.Column);
        }

        private void txtTime_Leave(object sender, EventArgs e)
        {
            string strTime = txtTime.Text.Trim();
            if (string.IsNullOrEmpty(strTime) == false && strTime.Length != 5)
            {
                ComFunc.MsgBox("입력형식이 올바르지 않습니다." + ComNum.VBLF + "예시1 : 08:38" + ComNum.VBLF + "예시2 : 21:09");
            }
            else if (VB.IsNumeric(VB.Right(strTime, 2)) == false)
            {
                ComFunc.MsgBox("입력형식이 올바르지 않습니다." + ComNum.VBLF + "예시1 : 08:38" + ComNum.VBLF + "예시2 : 21:09");
            }
            else if (VB.IsNumeric(VB.Left(strTime, 2)) == false)
            {
                ComFunc.MsgBox("입력형식이 올바르지 않습니다." + ComNum.VBLF + "예시1 : 08:38" + ComNum.VBLF + "예시2 : 21:09");
            }
            else if (VB.Mid(strTime, 3, 1).Equals(":") == false)
            {
                ComFunc.MsgBox("입력형식이 올바르지 않습니다." + ComNum.VBLF + "예시1 : 08:38" + ComNum.VBLF + "예시2 : 21:09");
            }
            else if (VB.Val(VB.Left(strTime, 2)) > 23 || VB.Val(VB.Right(strTime, 2)) > 59)
            {
                ComFunc.MsgBox("입력형식이 올바르지 않습니다." + ComNum.VBLF + "예시1 : 08:38" + ComNum.VBLF + "예시2 : 21:09");
            }
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CboPR_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnPR_Click(object sender, EventArgs e)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            string strGubun = VB.Left(CboPR.Text, 2);

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL.AppendLine(" SELECT NAME, TEL1  ");
                SQL.AppendLine(" FROM " + ComNum.DB_PMPA + "KMS_TELNO ");
                SQL.AppendLine(" WHERE BUSEGBN = '1'");
                SQL.AppendLine(" AND BUCODE = '070101'");
                SQL.AppendLine(" AND GUBUN = '" + strGubun + "'");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["TEL1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL.Clear();
                SQL.AppendLine(" SELECT A.EMP_ID, A.EMP_NM, B.HP_NO  ");
                SQL.AppendLine(" FROM ADMIN.HR_EMP_BASIS A, ADMIN.HR_EMP_PERSONAL B ");
                SQL.AppendLine(" WHERE A.EMP_ID = B.EMP_ID");
                SQL.AppendLine(" AND A.EMP_NM LIKE '%" + txtFindName.Text.Trim() + "%'");
                SQL.AppendLine(" AND A.RETIRE_YMD IS NULL");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssListFind_Sheet1.RowCount = dt.Rows.Count;
                    ssListFind_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssListFind_Sheet1.Cells[i, 0].Text = dt.Rows[i]["EMP_ID"].ToString().Trim();
                        ssListFind_Sheet1.Cells[i, 1].Text = dt.Rows[i]["EMP_NM"].ToString().Trim();
                        ssListFind_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HP_NO"].ToString().Trim();

                    }

                    dt.Dispose();
                    dt = null;
                }                

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssListFind_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            for(int i = 0; i < ssView_Sheet1.Rows.Count; i++)
            {
                if(ssView_Sheet1.Cells[i,0].Text == "")
                {
                    ssView_Sheet1.Cells[i, 0].Text = ssListFind_Sheet1.Cells[e.Row, 2].Text;
                    return;
                }
            }
        }

        private void btnsslist1save_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for(i = 0; i < ssList1_Sheet1.Rows.Count; i++)
                {

                    if (ssList1_Sheet1.Cells[i,2].Text == "")
                    {
                        if(ssList1_Sheet1.Cells[i,0].Text != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS_RESULT ";
                            SQL = SQL + ComNum.VBLF + "        (SABUN, RESULT,BIGO) ";
                            SQL = SQL + ComNum.VBLF + " VALUES ('" + clsType.User.IdNumber + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + ssList1_Sheet1.Cells[i, 0].Text + "', ";
                            SQL = SQL + ComNum.VBLF + "         '') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        }
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_SMS_RESULT SET ";
                        SQL = SQL + ComNum.VBLF + "   SABUN          = '" + clsType.User.IdNumber + "',";
                        SQL = SQL + ComNum.VBLF + "   RESULT        = '" + ssList1_Sheet1.Cells[i, 0].Text + "',";
                        SQL = SQL + ComNum.VBLF + "   BIGO    = ''";
                        SQL = SQL + ComNum.VBLF + "  WHERE ROWID    = '" + ssList1_Sheet1.Cells[i, 2].Text.Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            TextSearch();
        }

        private void btnsslist1Del_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssList1_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssList1_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL = " DELETE ADMIN.ETC_SMS_RESULT ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ssList1_Sheet1.Cells[i, 2].Text + "' ";
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
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                search();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssList1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Row < 0)
            {
                return;
            }

            if (e.Column == 3)
            {
                if (ssList1_Sheet1.Cells[e.Row, 0].Text == "")
                {
                    return;
                }

                txtMsg.Text = txtMsg.Text + ssList1_Sheet1.Cells[e.Row, 0].Text;
            }
        }
    }
}
