using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : FrmOPDReserveSMS.cs
    /// Description     : 외래 예약문자 등록작업
    /// Author          : 이정현
    /// Create Date     : 2018-04-05
    /// <history> 
    /// 외래 예약문자 등록작업
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\OpdOcs\Oorder\mtsoorder\FrmOPDReserveSMS.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\OpdOcs\Oorder\mtsoorder.vbp
    /// </vbp>
    /// </summary>
    public partial class FrmOPDReserveSMS : Form
    {
        private string GstrPano = "";
        private string GstrDeptCode = "";
        private string GstrOCSSMSGubun = "";        //GstrOCS문자구분
        private string GstrROWID = "";

        public FrmOPDReserveSMS()
        {
            InitializeComponent();
        }

        public FrmOPDReserveSMS(string strPano, string strDeptCode, string strOCSSMSGubun = "")
        {
            InitializeComponent();

            GstrPano = strPano;
            GstrDeptCode = strDeptCode;
            GstrOCSSMSGubun = strOCSSMSGubun;
        }

        private void FrmOPDReserveSMS_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            MessageBox.Show("현재 사용 불가!!! (문의 : 전산정보팀(8331)", "확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnSave2.Enabled = false;
            return;

            txtPano.Text = GstrPano;

            Read_Bas_Patient(txtPano.Text);

            lblP_Info.Text = "";
            lblP_Info2.Text = "";

            //문자구분세팅
            ComboSET();

            string strJong = "";

            if (GstrOCSSMSGubun != "")
            {
                strJong = GstrOCSSMSGubun;

                Combo_SEL_SET(strJong);
            }

            strJong = VB.Pstr(cboJong.Text.Trim(), ".", 1).Trim();

            Data_View(strJong);
        }

        private void Read_Bas_Patient(string strPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            //정보동의 여부 체크
            txtPhone.Text = "";
            lblP_SMS.Text = "";
            txtSName.Text = "";
            txtJumin.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.HPhone, a.GbSMS, a.SName, a.Jumin1, a.Jumin2, a.Jumin3 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPano.Trim() + "' ";

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
                    txtPhone.Text = dt.Rows[0]["HPHONE"].ToString().Trim();
                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        txtJumin.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()), 1) + "******";
                    }
                    else
                    {
                        txtJumin.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";
                    }

                    switch (dt.Rows[0]["GBSMS"].ToString().Trim())
                    {
                        case "Y":
                            lblP_SMS.Text = "문자동의 Y";
                            lblP_SMS.BackColor = Color.FromArgb(192, 255, 255);
                            break;
                        case "N":
                            lblP_SMS.Text = "문자동의 N";
                            lblP_SMS.BackColor = Color.FromArgb(255, 255, 192);
                            break;
                        case "X":
                            lblP_SMS.Text = "문자동의 X";
                            lblP_SMS.BackColor = Color.FromArgb(255, 192, 255);
                            break;
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

        private void ComboSET()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            int intCnt = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                cboJong.Items.Clear();

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'ETC_OCS_문자구분' ";
                SQL = SQL + ComNum.VBLF + "         AND (DELDATE IS NULL OR DELDATE = '')";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

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
                        cboJong.Items.Add(dt.Rows[i]["CODE"].ToString().Trim() + "." + VB.Pstr(dt.Rows[i]["NAME"].ToString().Trim(), "^^", 1));
                    }

                    cboJong.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                lblP_Info.Text = VB.Pstr(cboJong.Text, ".", 2);

                cboDept_Tel.Items.Clear();

                //과 번호
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "     WHERE substr(drcode, 3, 2) = '99'";
                SQL = SQL + ComNum.VBLF + "         AND DrDept1 NOT IN ('RD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DRCODE ";

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
                        cboDept_Tel.Items.Add(dt.Rows[i]["DRDEPT1"].ToString().Trim() + "." + dt.Rows[i]["TELNO"].ToString().Trim());

                        if (dt.Rows[i]["DRDEPT1"].ToString().Trim() == clsOrdFunction.Pat.DeptCode.Trim())
                        {
                            intCnt = i;
                        }
                    }

                    cboDept_Tel.SelectedIndex = intCnt;
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

        private void Combo_SEL_SET(string strJong)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'ETC_OCS_문자구분' ";
                SQL = SQL + ComNum.VBLF + "         AND Code = '" + strJong + "' ";
                SQL = SQL + ComNum.VBLF + "         AND (DELDATE IS NULL OR DELDATE = '')";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

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
                    cboJong.Text = dt.Rows[0]["CODE"].ToString().Trim() + "." + VB.Pstr(dt.Rows[0]["NAME"].ToString().Trim(), "^^", 1);
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

        private void Data_View(string strJong)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(JOBDATE,'YYYY-MM-DD HH24:MI') AS RDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SENDTIME,'YYYY-MM-DD HH24:MI') AS SENDTIME, ";
                SQL = SQL + ComNum.VBLF + "     PANO, SNAME, HPHONE, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO ='" + GstrPano + "' ";

                switch (strJong)
                {
                    case "001":
                        SQL = SQL + ComNum.VBLF + "         AND GUBUN = '47' ";
                        break;
                    case "002":
                        SQL = SQL + ComNum.VBLF + "         AND GUBUN = '51' ";
                        break;
                    case "003":
                        SQL = SQL + ComNum.VBLF + "         AND GUBUN = '52' ";
                        break;
                    case "004":
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '59' "; //골다공증
                        break;
                    case "005":
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '60' "; //치매
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "         AND GUBUN = '47' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "         AND ENTDATE >= TO_DATE('2014-01-16','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY JOBDATE DESC ";

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
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RDATE"].ToString().Trim();

                        if (dt.Rows[i]["SENDTIME"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 3].Text = "전송";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 3].Text = "미전송";
                        }

                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                this.Close();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strPano = "";
            string strTime = "";
            string strTime2 = "";
            string strName = "";
            string strTel = "";
            string strMsg = "";
            string strData = "";
            string strMinRTime = "";
            string strRettel = "";
            string strJong = "";
            string strChk1 = "";

            if (cboDept_Tel.Text.Trim() != "")
            {
                lblP_Info2.Text = "054" + VB.Pstr(cboDept_Tel.Text, ".", 2).Replace("-", "").Trim();
            }

            if (lblP_Info2.Text.Trim() == "")
            {
                ComFunc.MsgBox("회신번호 설정이 없음 전산실 연락요망!! ☎8332");
                return rtnVal;
            }

            if (cboJong.Text.Trim() == "")
            {
                ComFunc.MsgBox("문자전송 구분을 확인 후 등록하십시오.");
                return rtnVal;
            }

            strJong = VB.Pstr(cboJong.Text.Trim(), ".", 1).Trim();

            switch(strJong)
            {
                case "001":
                    if (lblP_SMS.Text == "문자동의 Y") { }
                    else
                    {
                        ComFunc.MsgBox("문자동의 신청이 안된 고객입니다. 동의된 대상만 문자전송됩니다.");
                        return rtnVal;
                    }
                    break;
            }

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호 공란");
                txtPano.Focus();
                return rtnVal;
            }

            if (txtPhone.Text.Trim() == "")
            {
                ComFunc.MsgBox("휴대폰번호 공란");
                txtPhone.Focus();
                return rtnVal;
            }

            strPano = GstrPano;
            strTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //당뇨 5개월후 문자관련 체크
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(JOBDATE,'YYYY-MM-DD HH24:MI') AS RDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SENDTIME,'YYYY-MM-DD HH24:MI') AS SENDTIME,";
                SQL = SQL + ComNum.VBLF + "     PANO, SNAME, HPHONE, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '55' ";
                SQL = SQL + ComNum.VBLF + "         AND (SendTime IS NULL OR SendTime = '') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strChk1 = "Y";
                }

                dt.Dispose();
                dt = null;

                if (rdoSMS0.Checked == true) { strTime = Convert.ToDateTime(strTime).AddDays(30).ToString("yyyy-MM-dd"); }
                else if (rdoSMS1.Checked == true) { strTime = Convert.ToDateTime(strTime).AddDays(90).ToString("yyyy-MM-dd"); }
                else if (rdoSMS2.Checked == true) { strTime = Convert.ToDateTime(strTime).AddDays(180).ToString("yyyy-MM-dd"); }
                else if (rdoSMS3.Checked == true) { strTime = Convert.ToDateTime(strTime).AddDays(365).ToString("yyyy-MM-dd"); }
                else if (rdoSMS4.Checked == true) { strTime = Convert.ToDateTime(strTime).AddDays(60).ToString("yyyy-MM-dd"); }

                strTime2 = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(150).ToString("yyyy-MM-dd");

                strData = clsOrdFunction.Pat.sName;

                for (i = 1; i <= strData.Length; i++)
                {
                    if (VB.Mid(strData, i, 1) != " ")
                    {
                        strName += VB.Mid(strData, i, 1);
                    }
                }

                strData = txtPhone.Text.Trim();

                for (i = 1; i <= strData.Length; i++)
                {
                    if (VB.Val(VB.Mid(strData, i, 1)) >= 0 && VB.Val(VB.Mid(strData, i, 1)) <= 9)
                    {
                        strTel += VB.Mid(strData, i, 1);
                    }
                }

                strRettel = "0542608219";       ///갑상선센터
                strRettel = lblP_Info2.Text;

                //이미 자료를 넘겼는지 확인함
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) AS RTime";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "     WHERE JobDate >= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND JobDate <= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "         AND Pano = '" + txtPano.Text.Trim() + "' ";

                switch (strJong)
                {
                    case "001":
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '47' "; //갑상선
                        break;
                    case "002":
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '51' "; //당뇨
                        break;
                    case "003":
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '52' "; //안과
                        break;
                    case "004":
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '59' "; //골다공증
                        break;
                    case "005":
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '60' "; //치매
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "         AND Gubun = '47' "; //갑상선
                        break;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strMinRTime = dt.Rows[0]["RTIME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //이미 전송한 예약시간이 적으면 다시 전송 않함
                if (strMinRTime != "" && Convert.ToDateTime(strMinRTime) <= Convert.ToDateTime(strTime))
                {
                    strTel = "";
                }

                //SMS 자료에 INSERT
                if (strName != "" && strTel != "")
                {
                    switch (strJong)
                    {
                        case "001":
                            strMsg = "귀하의 갑상선검사 주기가 되었습니다 예약260-8001~2 <포항성모병원 갑상선센터>";
                            break;
                        case "002":
                            strMsg = "귀하의 혈액검사,소변검사 정기검진 할 시기입니다.<포항성모병원>";
                            break;
                        case "003":
                            strMsg = "귀하의 안과검사 정기검진 할 시기입니다.";
                            break;
                        case "004":
                            strMsg = "귀하의 골다공증검사 정기검진 할 시기입니다.";
                            break;
                        case "005":
                            strMsg = "귀하의 치매검사 정기검진 할 시기입니다.";
                            break;
                        default:
                            strMsg = "귀하의 갑상선검사 주기가 되었습니다 예약260-8001~2 <포항성모병원 갑상선센터>";
                            break;
                    }

                    //------------( 자료를 DB에 INSERT )---------------------
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                    SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, SendTime, DeptCode, DrCode, RTime, RetTel, SendMsg, EntDate)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strTime + " 10:00" + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strName + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strTel + "', ";

                    switch (strJong)
                    {
                        case "001":
                            SQL = SQL + ComNum.VBLF + "         '47', ";
                            break;
                        case "002":
                            SQL = SQL + ComNum.VBLF + "         '51', ";
                            break;
                        case "003":
                            SQL = SQL + ComNum.VBLF + "         '52', ";
                            break;
                        case "004":
                            SQL = SQL + ComNum.VBLF + "         '59', ";
                            break;
                        case "005":
                            SQL = SQL + ComNum.VBLF + "         '60', ";
                            break;
                        default:
                            SQL = SQL + ComNum.VBLF + "         '47', ";
                            break;
                    }

                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrDeptCode + "', ";
                    SQL = SQL + ComNum.VBLF + "         '', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strTime + " 10:00','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + "         '" + strRettel + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMsg + "', ";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    //당뇨혈액검사일경우 기본 저장시 5개월후 문자 한건더 생성
                    if (strJong == "002")
                    {
                        if (strChk1 == "")
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                            SQL = SQL + ComNum.VBLF + "     (JobDate, Pano, SName, HPhone, Gubun, SendTime, DeptCode, DrCode, RTime, RetTel, SendMsg, EntDate)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strTime2 + " 10:00" + "','YYYY-MM-DD HH24:MI'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strName + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTel + "', ";
                            SQL = SQL + ComNum.VBLF + "         '55', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";
                            SQL = SQL + ComNum.VBLF + "         '" + GstrDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '', ";    //문자전송안함-전송
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strTime2 + " 10:00','YYYY-MM-DD HH24:MI'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + strRettel + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strMsg + "', ";
                            SQL = SQL + ComNum.VBLF + "         SYSDATE";
                            SQL = SQL + ComNum.VBLF + "     )";

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
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("정상등록되었습니다..!!");
                Cursor.Current = Cursors.Default;

                Data_View(strJong);

                rtnVal = true;
                return rtnVal;
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
                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DelData() == true)
            {

            }
        }

        private bool DelData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strJong = "";

            if (cboJong.Text.Trim() == "")
            {
                ComFunc.MsgBox("문자전송 구분을 확인 후 삭제하십시오.");
                return rtnVal;
            }

            strJong = VB.Pstr(cboJong.Text.Trim(), ".", 1).Trim();

            if (GstrROWID == "")
            {
                ComFunc.MsgBox("삭제할 대상을 선택 후 작업하십시오!!");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_SMS";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         GUBUN = '48', ";
                SQL = SQL + ComNum.VBLF + "         SENDTIME = SYSDATE ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "' ";

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

                Data_View(strJong);

                btnDelete.Enabled = false;

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboJong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboJong.Text.Trim() == "") { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strJong = "";

            strJong = VB.Pstr(cboJong.Text.Trim(), ".", 1).Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'ETC_OCS_문자구분' ";
                SQL = SQL + ComNum.VBLF + "         AND Code = '" + strJong + "' ";
                SQL = SQL + ComNum.VBLF + "         AND (DELDATE IS NULL OR DELDATE = '')";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

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
                    lblP_Info.Text = VB.Pstr(dt.Rows[0]["NAME"].ToString().Trim(), "^^", 1).Trim();
                }

                dt.Dispose();
                dt = null;

                if (cboDept_Tel.Text.Trim() != "")
                {
                    lblP_Info2.Text = "054" + VB.Pstr(cboDept_Tel.Text, ".", 2).Replace("-", "").Trim();
                }

                switch (strJong)
                {
                    case "001":
                        ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "문자전송일자";
                        ssView_Sheet1.ColumnHeader.Cells[0, 3].Text = "전송여부";

                        lblP_Info2.Text = "0542608219";
                        rdoSMS1.Checked = true;
                        break;
                    case "002":
                        ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "정기검사일자";
                        ssView_Sheet1.ColumnHeader.Cells[0, 3].Text = "전송여부";

                        rdoSMS2.Checked = true;
                        break;
                    case "003":
                    case "004":
                    case "005":
                        ssView_Sheet1.ColumnHeader.Cells[0, 2].Text = "정기검사일자";
                        ssView_Sheet1.ColumnHeader.Cells[0, 3].Text = "등록여부";

                        rdoSMS3.Checked = true;
                        break;
                }

                Cursor.Current = Cursors.Default;

                Data_View(strJong);
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

        private void cboDept_Tel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strJong = "";

            strJong = VB.Pstr(cboJong.Text.Trim(), ".", 1).Trim();

            if (cboDept_Tel.Text.Trim() != "")
            {
                if (strJong == "001")
                {
                    lblP_Info2.Text = "0542608219";
                }
                else
                {
                    lblP_Info2.Text = "054" + VB.Pstr(cboDept_Tel.Text, ".", 2).Replace("-", "").Trim();
                }
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPano.Text.Trim() == "") { return; }

                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");

                Read_Bas_Patient(txtPano.Text.Trim());
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            if (txtPano.Text.Trim() == "") { return; }

            txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");

            Read_Bas_Patient(txtPano.Text.Trim());
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호 공란");
                txtPano.Focus();
                return;
            }

            if (txtPhone.Text.Trim() == "")
            {
                ComFunc.MsgBox("휴대폰번호 공란");
                txtPhone.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         HPHONE ='" + txtPhone.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO ='" + txtPano.Text.Trim() + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) { return; }

            string strJong = "";

            strJong = VB.Pstr(cboJong.Text.Trim(), ".", 1).Trim();
            
            if (ssView_Sheet1.Cells[e.Row, 3].Text.Trim() == "미전송")
            {
                GstrROWID = ssView_Sheet1.Cells[e.Row, 4].Text.Trim();

                btnDelete.Enabled = true;
            }
            else
            {
                switch (strJong)
                {
                    case "001":
                        ComFunc.MsgBox("전송된것은 삭제 불가!!");
                        break;
                    case "002":
                    case "003":
                        GstrROWID = ssView_Sheet1.Cells[e.Row, 4].Text.Trim();
                        btnDelete.Enabled = true;
                        break;
                    default:
                        ComFunc.MsgBox("전송된것은 삭제 불가!!");
                        break;
                }
            }
        }
    }
}
