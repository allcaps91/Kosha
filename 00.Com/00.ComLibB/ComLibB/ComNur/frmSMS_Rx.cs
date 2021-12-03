using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\FrmSMS전송" >> frmSMS_Rx.cs 폼이름 재정의" />

    public partial class frmSMS_Rx : Form
    {

        string FstrIpdOpd = "";
        string FstrRoom = "";
        string FstrPANO = "";
        string FstrSname = "";
        string FstrAge = "";
        string FstrDept = "";
        string FstrDrCode = "";
        string FstrDrName = "";
        string FstrWardCode = "";

        public frmSMS_Rx(string strIpdOpd, string strRoom, string strWardCode, string strPANO, string strSname, string strAge, string strDept, string strDrName, string strDrCode)
        {
            InitializeComponent();

            FstrIpdOpd = strIpdOpd;
            FstrRoom = strRoom;
            FstrWardCode = strWardCode;
            FstrPANO = strPANO;
            FstrSname = strSname;
            FstrAge = strAge;
            FstrDept = strDept;
            FstrDrName = strDrName;
            FstrDrCode = strDrCode;
        }

        public frmSMS_Rx()
        {
            InitializeComponent();
        }

        private void frmSMS_Rx_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            lblPatient.Text = FstrWardCode + "/" + FstrRoom + " " + FstrPANO + " " + FstrSname + " " + FstrAge + " ";
            lblPatient.Text = lblPatient.Text + FstrDept + " " + FstrDrName;

            ComboJong.Items.Clear();
            ComboJong.Items.Add("1.타 병원 이송");
            ComboJong.Items.Add("2.ICU로 환자 이실");
            ComboJong.Items.Add("3.사망");
            ComboJong.Items.Add("4.환자의 상태 변화");
            ComboJong.Items.Add("5.ER 중증환자 발생");
            ComboJong.Items.Add("6.전공의 문자전송");
            ComboJong.Items.Add("7.전담간호사 문자전송");

            ComboGbn.Items.Clear();

            TxtMsg.Text = "";
            TxtDoct.Text = "";
            LabelCnt.Text = "";


            p_info.Text = "";
            TxtMsg2.Text = "";
            LabelCnt2.Text = "";

            p_info.Text = "담당의:" + FstrDrName + "으로 문자전송됨!! - 문자내용이 많을경우 여러번 나눠서 전송하십오";
            TxtMsg2.Text = "입원관련서류신청 " + FstrPANO + " " + FstrSname + " " + FstrWardCode + "/" + FstrRoom + "호 ";

            try
            {
                //SMS 전송로그를 표시함
                SQL = "";
                SQL = "SELECT TO_CHAR(JobDate,'YYYY-MM-DD HH24:MI') JobDate,Gubun,SendMsg ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "WHERE JobDate>=TRUNC(SYSDATE-5) ";
                SQL = SQL + ComNum.VBLF + "  AND Pano='" + FstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Gubun IN ('16','17','18','19','20') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY JobDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.Rows.Count = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = VB.Right(dt.Rows[i]["JobDate"].ToString().Trim(), 11);

                        switch (dt.Rows[i]["Gubun"].ToString().Trim())
                        {
                            case "16":
                                SS1_Sheet1.Cells[i, 1].Text = "전원";
                                break;
                            case "17":
                                SS1_Sheet1.Cells[i, 1].Text = "ICU";
                                break;
                            case "18":
                                SS1_Sheet1.Cells[i, 1].Text = "사망";
                                break;
                            case "19":
                                SS1_Sheet1.Cells[i, 1].Text = "상태";
                                break;
                            case "20":
                                SS1_Sheet1.Cells[i, 1].Text = "중증";
                                break;
                            default:
                                SS1_Sheet1.Cells[i, 1].Text = "기타";
                                break;
                        }
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SendMsg"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT C.HTEL,A.SABUN,A.DRCODE,A.DRNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_DOCTOR A , " + ComNum.DB_PMPA + "BAS_CLINICDEPT B , " + ComNum.DB_ERP + "INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.DEPTCODE = B.DEPTCODE";
                SQL = SQL + ComNum.VBLF + " AND A.SABUN =C.SABUN(+)";
                SQL = SQL + ComNum.VBLF + " AND A.GBOUT ='N'";
                SQL = SQL + ComNum.VBLF + " AND C.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + " AND A.DEPTCODE NOT IN ('ER','OM','LM','FM')";
                SQL = SQL + ComNum.VBLF + " AND C.HTEL IS NOT NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.GRADE,B.PRINTRANKING";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ComboDoct_2.Items.Add(dt.Rows[i]["Sabun"].ToString().Trim() + "." + dt.Rows[i]["DrName"].ToString().Trim());
                    }

                }

                ComboDoct_2.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

                //김경동
                if (clsType.User.Sabun == "34878" || clsType.User.Sabun == "44551")
                {
                    TxtMsg.Text = "";
                }
                if (clsType.User.Sabun == "34878" || clsType.User.Sabun == "44551")
                {
                    TxtMsg.Text = "";
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



        private void ChkSMS_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSMS0.Checked == true)
            {
                TxtMsg2.Text = TxtMsg2.Text + " ※진단서";
            }
            if (ChkSMS1.Checked == true)
            {
                TxtMsg2.Text = TxtMsg2.Text + " ※입사증";
            }
            if (ChkSMS2.Checked == true)
            {
                TxtMsg2.Text = TxtMsg2.Text + " ※소견서";
            }
            if (ChkSMS3.Checked == true)
            {
                TxtMsg2.Text = TxtMsg2.Text + " ※진료의뢰서";
            }
            if (ChkSMS4.Checked == true)
            {
                TxtMsg2.Text = TxtMsg2.Text + " ※전원사유서";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            //int j = 0;
            int Inx = 0;
            string strTxtTel = "";
            //string strTxtRetTel = "";
            string strTxtMsg = "";
            string strRTime = "";
            //int nRead = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //bool rtnVal = false;
            DataTable dt = null;
            string strJong = "";
            string strGbn = "";
            string strTelList = "";
            string strRetTel = "";
            int nTelCnt = 0;
            //string StrDrCode = "";
            //string strWardCode = "";
            string strSabun = "";
            double nSabun = 0;
            //double nCnt1 = 0;
            //double nCnt2 = 0;
            string strYYMM = "";


            Cursor.Current = Cursors.WaitCursor;

            strYYMM = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", "");

            strJong = VB.Left(ComboJong.Text, 1);
            strGbn = VB.Left(ComboGbn.Text, 1);
            strTxtMsg = TxtMsg.Text;

            if (Encoding.Default.GetByteCount(strTxtMsg) > 80)
            {
                MessageBox.Show("메세지는 80자까지만 가능합니다.", "확인");
                return;
            }
            if (clsType.User.Sabun == "34878" || clsType.User.Sabun == "44551")
            {
                FstrIpdOpd = "I";// 'NST담당자;
            }

            if (clsType.User.JobGroup == "JOB009003")
            {
                //보험심사팀 해당조건 안타도록 강제 설정('2021-03-23')
            }
            else
            {
                if (strJong != "5" && FstrIpdOpd != "I")
                {
                    MessageBox.Show("ER은 응급환자 발생 통보만 가능합니다.");
                    return;
                }
            }
            if (strJong == "6" && (ComboGbn.Text) == "")
            {
                MessageBox.Show("전공의를 선택 후 전송 버튼을 클릭하십시요.", "확인");
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                //담당과장의 휴대폰번호를 인사마스타에서 가져옴
                if (strJong == "5")
                {
                    for (i = 0; i < SSLstDoct1_Sheet1.RowCount; i++)
                    {

                    }
                    nTelCnt = 0;
                    strTelList = "";

                    for (i = 0; i < SSLstDoct1_Sheet1.RowCount; i++)
                    {

                        if (Convert.ToBoolean(SSLstDoct1_Sheet1.Cells[i, 0].Value) == true)
                        {

                            strSabun = VB.Right(SSLstDoct1_Sheet1.Cells[i, 3].Text, 5);
                            SQL = "";
                            SQL = "SELECT HTel FROM " + ComNum.DB_ERP + "INSA_MST ";
                            SQL = SQL + ComNum.VBLF + "WHERE Sabun='" + strSabun + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt.Rows.Count > 0 && dt.Rows[0]["HTel"].ToString().Trim() != "")
                            {
                                nTelCnt = nTelCnt + 1;
                                strTelList = strTelList + dt.Rows[0]["HTel"].ToString().Trim() + "{}";
                            }
                            dt.Dispose();
                            dt = null;
                        }
                    }
                }
                else if (strJong == "6" || strJong == "7")
                {
                    strTelList = VB.Right(ComboGbn.Text, 20);
                    nTelCnt = 1;
                }
                else
                {
                    nSabun = 0;
                    //strWardCode = "";
                    //'사번찾기

                    SQL = "";
                    SQL = "SELECT Sabun FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + FstrDrCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        dt.Dispose();
                        dt = null;
                        MessageBox.Show("OCS_Doctor에서 사번찾기 실패", "오류");
                        return;
                    }
                    nSabun = VB.Val(dt.Rows[0]["Sabun"].ToString().Trim());

                    dt.Dispose();
                    dt = null;

                    //인사마스타에서 휴대폰번호를 읽음

                    SQL = "";
                    SQL = "SELECT HTel FROM " + ComNum.DB_ERP + "INSA_MST ";
                    SQL = SQL + ComNum.VBLF + "WHERE Sabun='" + (nSabun).ToString("00000") + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {

                        dt.Dispose();
                        dt = null;
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("인사마스터가 등록 안 됨", "확인");
                        return;
                    }
                    strTelList = dt.Rows[0]["HTel"].ToString().Trim();

                    if (strTelList != "")
                    {
                        nTelCnt = 1;
                    }

                    dt.Dispose();
                    dt = null;
                }

                strRTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

                //병동별 회신번호
                if (strJong == "5")// '응급실
                {
                    strRetTel = "054-260-8117";
                }
                else if (FstrRoom == "234")
                {
                    strRetTel = "054-289-4739";
                }
                else if (FstrRoom == "233")
                {
                    strRetTel = "054-289-4740";
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT Tel FROM ADMIN.BAS_WARD ";
                    SQL = SQL + ComNum.VBLF + "WHERE WardCode='" + FstrWardCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strRetTel = dt.Rows[0]["Tel"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (strRetTel == "")
                    {
                        strRetTel = "054-272-0151";// '병동별 회신번호 설정 필요;
                    }
                }

                if (clsType.User.Sabun == "34878" || clsType.User.Sabun == "44551")
                {
                    strRetTel = "010-9684-0579";// 'NST담당자
                }

                for (Inx = 1; Inx <= nTelCnt; Inx++)
                {
                    strTxtTel = VB.Pstr(strTelList.Trim(), "{}", Inx);

                    //핸드폰 오류 점검
                    switch (VB.Left(strTxtTel, 3))
                    {
                        case "010":
                        case "011":
                        case "016":
                        case "017":
                        case "018":
                        case "019":
                            break;
                        default:
                            strTxtTel = "";
                            break;
                    }

                    if (strTxtTel == "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("수신번호가 존재하지 않습니다.", "오류");
                        return;
                    }

                    if (strJong == "7")
                    {
                        strJong = "6";
                    }

                    SQL = "";
                    SQL = " INSERT INTO ETC_SMS(JobDate,Pano,Hphone,Gubun,";
                    SQL = SQL + ComNum.VBLF + " Rettel,SendMsg,EntSabun,EntDate, SNAME,GBPUSH)";
                    SQL = SQL + ComNum.VBLF + " VALUES ( SYSDATE,'" + FstrPANO + "','" + (strTxtTel).Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "'" + (VB.Val(strJong) + 15).ToString("00") + "','" + (strRetTel.Replace("-", "")).Trim() + "','" + strTxtMsg.Replace("'", "`") + "',";
                    SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ",SYSDATE, '" + READ_PatientName(FstrPANO) + "','N') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("전송 완료.");
                Cursor.Current = Cursors.Default;

                this.Close();

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

        private string READ_PatientName(string ArgPano)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano='" + ArgPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["Sname"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strVal;
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
                return strVal;
            }

        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            //int i = 0;
            //int j = 0;
            //int Inx = 0;
            string strTxtTel = "";
            //string strTxtRetTel = "";
            string strTxtMsg = "";
            //int nRead = 0;

            //string strJong = "";
            //string strGbn = "";
            string strTelList = "";
            string strRetTel = "";
            //int nTelCnt = 0;
            //string StrDrCode = "";
            //string strWardCode = "";
            //string strSabun = "";
            double nSabun = 0;

            //double nCnt1 = 0;
            //double nCnt2 = 0;
            string strYYMM = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            strYYMM = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", "");

            strTxtMsg = (TxtMsg2.Text).Trim();

            if (Encoding.Default.GetByteCount(strTxtMsg) > 80)
            {
                MessageBox.Show("메세지는 80자까지만 가능합니다.", "확인");
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //담당과장의 휴대폰번호를 인사마스타에서 가져옴
                nSabun = 0;
                //strWardCode = "";

                //사번찾기
                SQL = "";
                SQL = "SELECT Sabun FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + "WHERE DrCode='" + FstrDrCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("OCS_Doctor에서 사번찾기 실패", "오류");
                    return;
                }
                nSabun = VB.Val(dt.Rows[0]["SABUN"].ToString().Trim());

                dt.Dispose();
                dt = null;

                //인사마스타에서 휴대폰번호를 읽음
                SQL = "";
                SQL = "SELECT HTel FROM ADMIN.INSA_MST ";
                SQL = SQL + ComNum.VBLF + "WHERE Sabun='" + nSabun.ToString("00000") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    MessageBox.Show("인사 마스터가 등록되지 안음", "확인");
                    return;
                }
                strTelList = dt.Rows[0]["HTel"].ToString().Trim();

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT Tel FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WardCode='" + FstrWardCode + "' ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strRetTel = dt.Rows[0]["Tel"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strRetTel == "")
                {
                    strRetTel = "054-272-0151";// '병동별 회신번호 설정 필요
                }

                strTxtTel = strTelList;

                //핸드폰 오류 점검
                switch (VB.Left(strTxtTel, 3))
                {
                    case "010":
                    case "011":
                    case "016":
                    case "017":
                    case "018":
                    case "019":
                        break;
                    default:
                        strTxtTel = "";
                        break;
                }

                if (strTxtTel == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("인사등록시 담담의사 연락처 미설정상태입니다..문자전송 실패!!");
                    return;
                }

                if (strTxtTel != "")
                {
                    SQL = "";
                    SQL = " INSERT INTO ETC_SMS(JobDate,Pano,SName,Hphone,Gubun,";
                    SQL = SQL + ComNum.VBLF + " Rettel,SendMsg,EntSabun,EntDate,GBPUSH)";
                    SQL = SQL + ComNum.VBLF + " VALUES ( SYSDATE,'" + FstrPANO + "','" + FstrSname + "','" + (strTxtTel).Trim() + "',";
                    SQL = SQL + ComNum.VBLF + " '29','" + (strRetTel).Trim() + "','" + strTxtMsg.Replace("'", "`") + "'," + clsType.User.Sabun + ",SYSDATE, 'N') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("전송 완료.");
                Cursor.Current = Cursors.Default;

                return;
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

        private void ComboGbn_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            string strJong = "";
            string strGbn = "";
            string strMsg = "";
            string strSmsErSend_Buse = "";
            string strNurse_Buse = "";
            //int nRead = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            TxtDoct.Text = "";
            strJong = (VB.Left(ComboJong.Text, 1)).Trim();
            if (strJong == "")
            {
                return;
            }
            strGbn = (VB.Left(ComboGbn.Text, 1)).Trim();
            if (strGbn == "")
            {
                return;
            }
            if (FstrRoom == "233")
            {
                strMsg = "★포항성모★SICU " + FstrSname + " " + FstrAge + " ";
            }
            else if (FstrRoom == "234")
            {
                strMsg = "★포항성모★MICU " + FstrSname + " " + FstrAge + " ";
            }
            else
            {
                strMsg = "★포항성모★" + FstrRoom + "호 " + FstrSname + " " + FstrAge + " ";
            }

            if (strJong == "1") // '타병원이송
            {
                strMsg = strMsg + "[전원]";
                switch (strGbn)
                {
                    case "1":
                        strMsg = strMsg + "응급검사/처치불가로 전원함";
                        break;
                    case "2":
                        strMsg = strMsg + "고위험환자로3차병원권유";
                        break;
                    case "3":
                        strMsg = strMsg + "3차병원진료위해 전원";
                        break;
                    case "4":
                        strMsg = strMsg + "기존진료받던병원으로전원";
                        break;
                    case "5":
                        strMsg = strMsg + "연고지관계로전원";
                        break;
                    default:
                        strMsg = strMsg + "기타사유로전원";
                        break;
                }


            }
            else if (strJong == "2")// 'ICU로 이실
            {
                switch (strGbn)
                {
                    case "1":
                        strMsg = strMsg + "CPR후 SICU로 이실";
                        break;
                    case "2":
                        strMsg = strMsg + "CPR후 MICU로 이실";
                        break;
                    case "3":
                        strMsg = strMsg + "호흡곤란으로 SICU 이실";
                        break;
                    case "4":
                        strMsg = strMsg + "호흡곤란으로 MICU 이실";
                        break;
                    case "5":
                        strMsg = strMsg + "출혈로 SICU 이실";
                        break;
                    case "6":
                        strMsg = strMsg + "출혈로 MICU 이실";
                        break;
                    case "7":
                        strMsg = strMsg + "심혈관계 이상으로 SICU 이실";
                        break;
                    case "8":
                        strMsg = strMsg + "심혈관계 이상으로 MICU 이실";
                        break;
                    default:
                        strMsg = strMsg + "기타사유로 ICU 이실";
                        break;
                }
            }
            else if (strJong == "4") // '환자의 상태 변화
            {
                strMsg = strMsg + "[환자상태]";
                switch (strGbn)
                {
                    case "1":
                        strMsg = strMsg + "호흡곤란 악화";
                        break;
                    case "2":
                        strMsg = strMsg + "심혈관계 이상";
                        break;
                    case "3":
                        strMsg = strMsg + "수술부위 출혈";
                        break;
                    case "4":
                        strMsg = strMsg + "V/S의 변화";
                        break;
                    default:
                        strMsg = strMsg + "기타 환자상태 변화";
                        break;
                }
            }
            else if (strJong == "5")// '중증외상 등
            {
                switch (strGbn)
                {
                    case "1":
                        strMsg = "★포항성모★ER[응급뇌혈관 환자 발생] " + FstrSname + " " + FstrAge;
                        break;
                    case "2":
                        strMsg = "★포항성모★ER[중증외상 환자 발생] " + FstrSname + " " + FstrAge;
                        break;
                    case "3":
                        strMsg = "★포항성모★ER[심혈관 환자 발생] " + FstrSname + " " + FstrAge;
                        break;
                }
                /*'=========================================================================*/
                /*'SMS전송 과장님 명단 표시*/
                switch (strGbn)
                {
                    case "1":
                        strSmsErSend_Buse = "'011106', '011110', '011118', '011119', '011123'";
                        strNurse_Buse = "'NS', 'NE'";
                        break;
                    case "2":
                        strSmsErSend_Buse = "'011106', '011102', '011105', '011107', '011119', '011118'";
                        strNurse_Buse = "'NS', 'GS', 'OS'";
                        break;
                    case "3":
                        strSmsErSend_Buse = "'100130', '011119', '011123'";
                        strNurse_Buse = "'MC', 'ER', 'RD'";
                        break;
                    default:
                        strSmsErSend_Buse = "";
                        strNurse_Buse = "";
                        break;
                }
            }

            try
            {
                if (strSmsErSend_Buse != "" && strNurse_Buse != "")
                {
                    SQL = "";
                    SQL = "SELECT A.SABUN, A.KORNAME SNAME, A.HTEL, B.SNAME DEPTCODE";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.INSA_MST A, ADMIN.BAS_BUSE B";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.BUSE = B.BUCODE(+)";
                    SQL = SQL + ComNum.VBLF + "  AND A.TOIDAY IS NULL";
                    SQL = SQL + ComNum.VBLF + "  AND A.BUSE IN (" + strSmsErSend_Buse + ")";
                    SQL = SQL + ComNum.VBLF + "UNION ALL";
                    SQL = SQL + ComNum.VBLF + "SELECT SABUN, SNAME, HTEL, DEPTCODE";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_CHARGE_NURSE";
                    SQL = SQL + ComNum.VBLF + "  WHERE DELDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE IN (" + strNurse_Buse + ")";
                    SQL = SQL + ComNum.VBLF + "  AND HTEL IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "   ORDER BY DEPTCODE DESC";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        TxtDoct.Text = "";
                        SSLstDoct1_Sheet1.Rows.Count = 0;

                        SSLstDoct1_Sheet1.Rows.Count = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            TxtDoct.Text = TxtDoct.Text + dt.Rows[i]["deptcode"].ToString().Trim() + "-" + dt.Rows[i]["sname"].ToString().Trim() + ComNum.VBLF;
                            SSLstDoct1_Sheet1.Cells[i, 0].Value = false;
                            SSLstDoct1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            SSLstDoct1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            SSLstDoct1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    TxtDoct.Text = "";
                }
                TxtMsg.Text = strMsg;

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

        private void ComboJong_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strJong = "";
            string strMsg = "";

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SSLstDoct1_Sheet1.RowCount = 0;

            TxtDoct.Text = "";
            strJong = VB.Left(ComboJong.Text, 1);


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (strJong == "1")//'전원
                {
                    ComboGbn.Items.Clear();
                    ComboGbn.Items.Add("1.응급검사/처치불가");
                    ComboGbn.Items.Add("2.고위험환자로 3차병원 권유");
                    ComboGbn.Items.Add("3.3차병원 진료 위해");
                    ComboGbn.Items.Add("4.기존 진료 받던 병원");
                    ComboGbn.Items.Add("5.연고지 관계");
                }
                else if (strJong == "2")// 'ICU이실
                {
                    ComboGbn.Items.Clear();
                    ComboGbn.Items.Add("1.CPR후 SICU");
                    ComboGbn.Items.Add("2.CPR후 MICU");
                    ComboGbn.Items.Add("3.호흡곤란으로 SICU");
                    ComboGbn.Items.Add("4.호흡곤란으로 MICU");
                    ComboGbn.Items.Add("5.출혈로 인한 SICU");
                    ComboGbn.Items.Add("6.출혈로 인한 MICU");
                    ComboGbn.Items.Add("7.심혈관계 이상으로 SICU");
                    ComboGbn.Items.Add("8.심혈관계 이상으로 MICU");
                }
                else if (strJong == "3")// '사망
                {
                    strMsg = "★포항성모★" + FstrRoom + "호 " + FstrSname + " (" + FstrAge;
                    strMsg = strMsg + ") 환자가 사망하셨습니다";
                    TxtMsg.Text = strMsg;
                    ComboGbn.Items.Clear();
                }
                else if (strJong == "4")//'환자의 상태변화
                {
                    ComboGbn.Items.Clear();
                    ComboGbn.Items.Add("1.호흡곤란 악화");
                    ComboGbn.Items.Add("2.심혈관계 이상");
                    ComboGbn.Items.Add("3.수술부위 출혈");
                    ComboGbn.Items.Add("4.Pupil&Motor&V/S의 변화");
                }
                else if (strJong == "5")//'ER 환자발생 통보
                {
                    ComboGbn.Items.Clear();
                    ComboGbn.Items.Add("1.응급뇌질환");
                    ComboGbn.Items.Add("2.중증외상");
                    ComboGbn.Items.Add("3.심혈관");
                }
                else if (strJong == "6")//   '전공의 문자 전송
                {
                    ComboGbn.Items.Clear();
                    SQL = " SELECT KORNAME, COALESCE(MSTEL, HTEL) HTEL, BUSE";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE BUSE LIKE '02%'";
                    SQL = SQL + ComNum.VBLF + "   AND TOIDAY IS NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND SABUN <'600000' ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY BUSE, KORNAME ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ComboGbn.Items.Add("(" + ReadBasBuse(dt.Rows[i]["BUSE"].ToString().Trim()) + ")" + dt.Rows[i]["KORNAME"].ToString().Trim() + "                                     " + dt.Rows[i]["HTEL"].ToString().Trim());
                        }
                    }

                    dt.Dispose();
                    dt = null;

                }
                else if (strJong == "7")//  '전담간호사 목록
                {
                    ComboGbn.Items.Clear();
                    SQL = "";
                    SQL = " SELECT SNAME, DEPTCODE, HTEL";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_CHARGE_NURSE";
                    SQL = SQL + ComNum.VBLF + " WHERE DELDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DEPTCODE ASC, SNAME ASC";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ComboGbn.Items.Add("(" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + ")" + dt.Rows[i]["SNAME"].ToString().Trim() + "                                     " + dt.Rows[i]["HTEL"].ToString().Trim());
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

        string ReadBasBuse(string strBucode)
        {
            DataTable dt = null;
            string strVal = "";
            string SQL = "";
            string SqlErr = "";

            if (strBucode.Trim() == "")
            {
                strVal = "";
                return strVal;
            }


            SQL = "";
            SQL = SQL + "SELECT Name,SName FROM ADMIN.BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + " WHERE BuCode='" + strBucode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return strVal;
            }

            if (dt == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return strVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return strVal;
            }

            if (dt.Rows[0]["SName"].ToString().Trim() != "")
            {
                strVal = dt.Rows[0]["SName"].ToString().Trim();
            }
            else
            {
                strVal = dt.Rows[0]["Name"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return strVal;
        }

        private void SSLstDoct1_CellClick(object sender, CellClickEventArgs e)
        {
            //int i = 0;

            if (Convert.ToBoolean(SSLstDoct1_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                SSLstDoct1_Sheet1.Cells[e.Row, 0].Value = false;
                SSLstDoct1_Sheet1.Rows[e.Row].BackColor = System.Drawing.Color.White;


            }
            else
            {
                SSLstDoct1_Sheet1.Cells[e.Row, 0].Value = true;
                SSLstDoct1_Sheet1.Rows[e.Row].BackColor = System.Drawing.Color.SkyBlue;
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSmsSend_Click(object sender, EventArgs e)
        {
            frmSMSHistory frmSMSHistoryX = new frmSMSHistory();
            frmSMSHistoryX.StartPosition = FormStartPosition.CenterParent;
            //frmSMSHistoryX.WindowState = FormWindowState.Normal;
            frmSMSHistoryX.ShowDialog();
            frmSMSHistoryX = null;
        }

        private void TxtMsg_TextChanged(object sender, EventArgs e)
        {
            LabelCnt.Text = Convert.ToString((Encoding.Default.GetByteCount(TxtMsg.Text))) + " / 80";

            if(Encoding.Default.GetByteCount(TxtMsg.Text) <= 80)
            {
                TxtMsg.ForeColor = Color.Black;
                LabelCnt.ForeColor = Color.Black;
            }
            else
            {
                TxtMsg.ForeColor = Color.Red;
                LabelCnt.ForeColor = Color.Red;
            }
        }

        private void TxtMsg2_TextChanged(object sender, EventArgs e)
        {
            LabelCnt2.Text = Convert.ToString((Encoding.Default.GetByteCount(TxtMsg2.Text))) + " / 80";

            if (Encoding.Default.GetByteCount(TxtMsg2.Text) <= 80)
            {
                TxtMsg2.ForeColor = Color.Black;
                LabelCnt2.ForeColor = Color.Black;
            }
            else
            {
                TxtMsg2.ForeColor = Color.Red;
                LabelCnt2.ForeColor = Color.Red;
            }
        }
    }

}

