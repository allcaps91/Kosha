using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSmsSend : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();

        string SQL = "";
        string SqlErr = "";
        int intRowAffected = 0;

        string GstrPassProgramID = "";
        string FstrKTSMS_사용여부 = "";
        string GstrSMS114_Result = "";

        public frmSmsSend()
        {
            InitializeComponent();
        }

        private void frmSmsSend_Load(object sender, EventArgs e)
        {
            read_sysdate();
            CheckIP_Set();

            Screen_Clear();

            FstrKTSMS_사용여부 = "Y";

            if (FstrKTSMS_사용여부 == "Y")
            {
                lbl_P_STS.Text = "KT SMS";
            }
            else
            {
                lbl_P_STS.Text = "문자 114";
            }

            //전송목록 조회
            SMS_Many_Message_Send_View();

            //메세지 전송 시작하기
            TmrFlow.Enabled = true;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = VB.Left(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T"),5);
        }

        void CheckIP_Set()
        {
            int nREAD = 0;
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string strIP = "";
            string strHostName = "";
            string strDateTime = "";
            string strDateHour = "";
            string strGubun = "";
            int nREAD_2 = 0;
            int k = 0;
            string strKorname = "";
            string strHTEL = "";
            string strRettel = "";
            string strSENDMSG = "";
            int nREAD_3 = 0;

            strDateTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strDateHour = VB.Mid(strDateTime, 1, 13);
            strGubun = "A";
            strRettel = "0542608338";

            SQL = "";
            SQL = SQL + " SELECT IPaddress, hostname ";
            SQL = SQL + " from ADMIN.etc_pingtest ";
            SQL = SQL + " where deldate is null ";
            SQL = SQL + " order by ipaddress ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("IP프로그램 체크 확인 요망", "확인");
                return;
            }

            nREAD = dt.Rows.Count;

            SS2_Sheet1.Rows.Count = 0;
            SS2_Sheet1.Rows.Count = nREAD;

            for (i = 0; i < nREAD; i++)
            {
                strIP = dt.Rows[i]["IPaddress"].ToString().Trim();
                SS2_Sheet1.Cells[i, 0].Text = strIP.Trim();
                strHostName = dt.Rows[i]["Hostname"].ToString().Trim();
                SS2_Sheet1.Cells[i, 1].Text = strHostName.Trim();

                if (Ping(strIP))
                {
                    SS2_Sheet1.Cells[i, 2].Text = "O";
                }
                else
                {
                    SS2_Sheet1.Cells[i, 2].Text = "X";
                }

                strSENDMSG = "★네트웍오류★" + strIP + "★" + strHostName + "★";

                if (!(Ping(strIP)))
                {
                    //전산실직원 조회
                    SQL = "";
                    SQL = SQL + " select korname, htel ";
                    SQL = SQL + " from ADMIN.insa_mst ";
                    SQL = SQL + " where buse = '077501' ";
                    SQL = SQL + " and toiday is null ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    nREAD_2 = dt1.Rows.Count;

                    clsDB.setBeginTran(clsDB.DbCon);

                    for (k = 0; k < nREAD_2; k++)
                    {
                        strKorname = dt1.Rows[k]["Korname"].ToString().Trim();
                        strHTEL = dt1.Rows[k]["HTEL"].ToString().Trim();

                        //이미전송했는지 확인
                        SQL = "";
                        SQL = SQL + " select to_char(jobdate, 'yyyy-mm-dd hh24') jobdate, gubun, sname, hphone ";
                        SQL = SQL + " from ADMIN.etc_sms ";
                        SQL = SQL + " where gubun = '" + strGubun + "' ";
                        SQL = SQL + " and sname = '" + strKorname + "' ";
                        SQL = SQL + " and hphone = '" + strHTEL + "' ";
                        SQL = SQL + " and to_char(jobdate, 'yyyy-mm-dd hh24') = '" + strDateHour + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        nREAD_3 = dt2.Rows.Count;

                        //전송 안했으면 etc_sms 테이블에 입력하기
                        if (nREAD_3 == 0)
                        {
                            SQL = " insert into ADMIN.etc_sms(jobdate, gubun, sname, hphone, rettel, sendmsg,PSMHSEND) values(";
                            SQL = SQL + "to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'), '" + strGubun + "', '" + strKorname + "',";
                            SQL = SQL + "'" + strHTEL + "', '" + strRettel + "', '" + strSENDMSG + "','Y')";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("ETC_SMS 문자 INSERT 요망", "확인");
                                return;
                            }
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    dt1.Dispose();
                    dt1 = null;
                }
            }

            dt.Dispose();
            dt = null;

        }

        public static bool Ping(string ip)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);

                Ping pingSender = new Ping();

                PingOptions options = new PingOptions();

                options.DontFragment = true;

                string data = "abcdefghijklmnopqrstuvwxyz012345";

                byte[] buffer = Encoding.ASCII.GetBytes(data);

                int timeout = 1000;

                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        void Screen_Clear()
        {
            int k = 0;

            TxtEDateSend.Text = "";
            txtTimeCnt.Text = "0";

            //전송주기 
            CboTimeCycle.Items.Clear();
            CboTimeCycle.Items.Add(5);
            for (k = 1; k <= 10; k++)
            {
                CboTimeCycle.Items.Add(k * 30);
            }

            CboTimeCycle.SelectedIndex = 1;
        }

        void SMS_Many_Message_Send_View()
        {
            string strDate = "";
            DataTable rsSMS = null;
            int i = 0;
            int nREAD = 0;

            strDate = cpublic.strSysDate;

            SQL = "";
            SQL = SQL + " select to_char(jobdate, 'yyyy-mm-dd hh24:mi') jobdate, to_char(sendtime, 'yyyy-mm-dd hh24:mi') sendtime, ";
            SQL = SQL + " hphone, sname, rettel, gubun, sendmsg, state, rowid ";
            SQL = SQL + " from ADMIN.etc_sms ";
            SQL = SQL + " where jobdate >= to_date('" + strDate + " 00:01', 'yyyy-mm-dd hh24:mi') ";
            SQL = SQL + " and gubun <> '8' ";
            SQL = SQL + " and ROWNUM< 500 ";
            SQL = SQL + " order by sendtime desc ";

            SqlErr = clsDB.GetDataTableEx(ref rsSMS, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("이전날 보낸 데이터 확인 요망", "확인");
                return;
            }

            nREAD = rsSMS.Rows.Count;

            SS1_Sheet1.Rows.Count = 0;
            SS1_Sheet1.Rows.Count = nREAD;

            for (i = 0; i < nREAD; i++)
            {
                SS1_Sheet1.Cells[i, 0].Text = rsSMS.Rows[i]["JOBDATE"].ToString().Trim();
                SS1_Sheet1.Cells[i, 1].Text = rsSMS.Rows[i]["SendTime"].ToString().Trim();
                SS1_Sheet1.Cells[i, 2].Text = rsSMS.Rows[i]["Hphone"].ToString().Trim();
                SS1_Sheet1.Cells[i, 3].Text = rsSMS.Rows[i]["Sname"].ToString().Trim();
                SS1_Sheet1.Cells[i, 4].Text = rsSMS.Rows[i]["Rettel"].ToString().Trim();
                SS1_Sheet1.Cells[i, 6].Text = rsSMS.Rows[i]["GUBUN"].ToString().Trim();
                SS1_Sheet1.Cells[i, 7].Text = rsSMS.Rows[i]["State"].ToString().Trim();
                SS1_Sheet1.Cells[i, 8].Text = rsSMS.Rows[i]["Sendmsg"].ToString().Trim();
                SS1_Sheet1.Cells[i, 9].Text = rsSMS.Rows[i]["ROWID"].ToString().Trim();
            }

            rsSMS.Dispose();
            rsSMS = null;
        }

        void SMS_Many_Message_Send_KT_SMS()
        {
            int i = 0;
            int nREAD = 0;
            string strTel = "";
            string strMsg = "";
            string strDeptCode = "";
            string strRettel = "";
            string strResult = "";
            string strDeptName = "";

            DataTable rsAllSend = null;

            string strDateTime = "";
            string strDateTime_1 = "";
            string strDateTime_2 = "";
            string strDateTime_3 = "";
            string strDateTime_4 = "";
            string strDateTime_5 = "";

            string strPano = "";
            string strSname = "";
            string strROWID = "";
            string strOK = "";

            string strGubun = "";  //0.즉시, 1.예약
            string strSTime = "";  //시간
            string strRTime = "";  //예약시간
            string strRDate = "";  //날짜
            string strGBPUSH = ""; //푸시메세지

            strDateTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strDateTime = VB.Left(strDateTime, 16);
            strSTime = VB.Replace(strDateTime, " ", "");
            strSTime = VB.Replace(strSTime, "-", "");
            strSTime = VB.Replace(strSTime, ":", "");

            strDateTime_1 = VB.DateAdd("D", -1, cpublic.strSysDate) + "";
            strDateTime_1 = VB.Left(strDateTime_1, 10) + " 00:01";
            strDateTime_2 = VB.DateAdd("D", -1, cpublic.strSysDate) + "";
            strDateTime_2 = VB.Left(strDateTime_2, 10) + " 08:40";
            strDateTime_3 = cpublic.strSysDate + " 00:01";
            strDateTime_4 = cpublic.strSysDate + " 08:40";
            //strDateTime_5 = cpublic.strSysDate + " 23:40";
            strDateTime_5 = cpublic.strSysDate + " 23:59";

            SQL = " select jobdate, send_cnt, pano, sname, hphone, gubun, deptcode, " + ComNum.VBLF;
            SQL += " drcode, rettel, sendmsg, " + ComNum.VBLF;
            SQL += " to_char(RTime, 'yyyy-mm-dd') RTime,TO_CHAR(RTime, 'yyyymmddhh24mi') RTime2, sendmsgback, rowid, GBPUSH " + ComNum.VBLF;
            SQL += " from ADMIN.etc_sms " + ComNum.VBLF;

            if (cpublic.strSysTime.CompareTo("00:00") >= 0 && cpublic.strSysTime.CompareTo("06:59") <= 0)
            {
                //00:00 ~ 06:59  문자 구분 보내기
                SQL += " where jobdate between to_date('" + strDateTime_3 + "', 'yyyy-mm-dd hh24:mi') and to_date('" + strDateTime_5 + "', 'yyyy-mm-dd hh24:mi') " + ComNum.VBLF;
                SQL += " and gubun in ('A', '16', '17', '18', '19', '20', '21', '22', '68','73','T1','T','74','82') " + ComNum.VBLF;
            }
            else if (cpublic.strSysTime.CompareTo("07:00") >= 0 && cpublic.strSysTime.CompareTo("07:59") <= 0)
            {
                //07:00 ~ 07:59  문자 구분 보내기
                SQL += " where jobdate between to_date('" + strDateTime_3 + "', 'yyyy-mm-dd hh24:mi') and to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi') " + ComNum.VBLF;
                SQL += " and gubun in ('A','10', '16', '17', '18', '19', '20', '21', '22', '68','73','T1','T','74','82') " + ComNum.VBLF;
            }
            else if (cpublic.strSysTime.CompareTo("08:00") >= 0 && cpublic.strSysTime.CompareTo("08:39") <= 0)
            {
                //08:00 ~ 08:39  문자 구분보내기
                SQL += " where jobdate between to_date('" + strDateTime_1 + "', 'yyyy-mm-dd hh24:mi') and to_date('" + strDateTime_4 + "', 'yyyy-mm-dd hh24:mi') " + ComNum.VBLF;
                SQL += " and gubun != '84' " + ComNum.VBLF; //코로나 입원자 사전 내용 문자 구분 제외
            }
            else if (cpublic.strSysTime.CompareTo("08:40") >= 0 && cpublic.strSysTime.CompareTo("18:59") <= 0)
            {
                //08:40 ~ 18:59  문자 구분보내기
                SQL += " where jobdate between to_date('" + strDateTime_3 + "', 'yyyy-mm-dd hh24:mi') and to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi') " + ComNum.VBLF;
                SQL += " and gubun != '84' " + ComNum.VBLF; //코로나 입원자 사전 내용 문자 구분 제외
            }
            else if (cpublic.strSysTime.CompareTo("19:00") >= 0 && cpublic.strSysTime.CompareTo("23:59") <= 0)
            {
                //19:00 ~ 23:59  문자 구분보내기
                SQL += " where jobdate between to_date('" + strDateTime_3 + "', 'yyyy-mm-dd hh24:mi') and to_date('" + strDateTime_5 + "', 'yyyy-mm-dd hh24:mi') " + ComNum.VBLF;
                SQL += " and gubun in ('A', 'N','10', '16', '17', '18', '19', '20', '21', '22', '68','73','T1','T','74','82')" + ComNum.VBLF;
            }

            SQL += " and sendtime is null " + ComNum.VBLF;
            SQL += " and gubun <> '8' " + ComNum.VBLF;
            SQL += " order by jobdate asc " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rsAllSend, SQL, clsDB.DbCon);

            nREAD = rsAllSend.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strTel = VB.Replace(TelNo_Edit_Process(rsAllSend.Rows[i]["HPHONE"].ToString().Trim()), "-", "");
                strRettel = VB.Replace(TelNo_Edit_Process(rsAllSend.Rows[i]["RETTEL"].ToString().Trim()), "-", "");

                strMsg = rsAllSend.Rows[i]["SENDMSG"].ToString().Trim();
                strDeptCode = rsAllSend.Rows[i]["DEPTCODE"].ToString().Trim();
                strPano = rsAllSend.Rows[i]["PANO"].ToString().Trim();
                strSname = rsAllSend.Rows[i]["SNAME"].ToString().Trim();
                strROWID = rsAllSend.Rows[i]["ROWID"].ToString().Trim();

                strRTime = rsAllSend.Rows[i]["RTime2"].ToString().Trim();
                strRDate = rsAllSend.Rows[i]["RTime"].ToString().Trim();
                strGBPUSH = rsAllSend.Rows[i]["GBPUSH"].ToString().Trim();

                //2016-10-18 푸시메세지관련 수정(사용안함)
                //if(strGBPUSH != "N") { strOK = APP_PUSH_TEST(strTel, strPano, strMsg); }

                //에약, 즉시 발송 구분 
                if (cpublic.strSysDate.CompareTo(strRTime) < 0)
                {
                    strGubun = "1";
                }
                else
                {
                    strGubun = "0";
                }

                if (strRTime.Trim() == "") { strRTime = DateTime.Now.ToString("yyyyMMddHHmm"); } //KT모듈에는 RTIME은 NOT NULL임   NULL일경우 오류 발생함

                int bytecount = System.Text.Encoding.Default.GetByteCount(strMsg);
                if (bytecount >= 90)
                {
                    strOK = MYSQL_KT_MMS_INSERT(strTel, strPano, strMsg, strSTime, DateTime.Now.ToString("yyyyMMddHHmm"), strRettel, strGubun);
                }
                else
                {
                    strOK = MYSQL_KT_SMS_INSERT(strTel, strPano, strMsg, strSTime, DateTime.Now.ToString("yyyyMMddHHmm"), strRettel, strGubun);
                }

                clsDB.setBeginTran(clsDB.DbCon);

                SQL = " update ADMIN.etc_sms set ";
                SQL += " sendtime = to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'), ";
                SQL += " state = '" + GstrSMS114_Result + "', ";
                SQL += " GbPush = '" + strOK + "'";
                SQL += " where rowid = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    MessageBox.Show("ETC_SMS 업데이트 에러", "확인");
                    return;
                }
                else
                {
                    TxtEDateSend.Text = cpublic.strSysDate + " " + cpublic.strSysTime;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }

            rsAllSend.Dispose();
            rsAllSend = null;

        }

        void MMS_IMG_Message_Send_KT_MMS()
        {
            int i = 0;
            int nREAD = 0;
            string strTel = "";
            string strMsg = "";
            string strDeptCode = "";
            string strRettel = "";

            DataTable rsAllSend = null;

            string strDateTime = "";
            string strDateTime_3 = "";

            string strPano = "";
            string strSname = "";
            string strROWID = "";
            string strOK = "";

            string strGubun = "";  //0.즉시, 1.예약
            string strSTime = "";  //시간
            string strRTime = "";  //예약시간
            string strRDate = "";  //날짜
            string strGBPUSH = ""; //푸시메세지

            strDateTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strDateTime = VB.Left(strDateTime, 16);
            strSTime = VB.Replace(strDateTime, " ", "");
            strSTime = VB.Replace(strSTime, "-", "");
            strSTime = VB.Replace(strSTime, ":", "");

            strDateTime_3 = cpublic.strSysDate + " 00:01";

            SQL = " select jobdate, send_cnt, pano, sname, hphone, gubun, deptcode, " + ComNum.VBLF;
            SQL += " drcode, rettel, sendmsg, " + ComNum.VBLF;
            SQL += " to_char(RTime, 'yyyy-mm-dd') RTime,TO_CHAR(RTime, 'yyyymmddhh24mi') RTime2, sendmsgback, rowid, GBPUSH " + ComNum.VBLF;
            SQL += " from ADMIN.etc_sms " + ComNum.VBLF;
            SQL += " where jobdate between to_date('" + strDateTime_3 + "', 'yyyy-mm-dd hh24:mi') and to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi:') " + ComNum.VBLF;
            SQL += " and GUBUN = '84' " + ComNum.VBLF; //코로나 입원자 사전 내용 문자 구분 제외
            SQL += " and sendtime is null " + ComNum.VBLF;
            SQL += " order by jobdate asc " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rsAllSend, SQL, clsDB.DbCon);

            nREAD = rsAllSend.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strTel = VB.Replace(TelNo_Edit_Process(rsAllSend.Rows[i]["HPHONE"].ToString().Trim()), "-", "");
                strRettel = VB.Replace(TelNo_Edit_Process(rsAllSend.Rows[i]["RETTEL"].ToString().Trim()), "-", "");

                strMsg = rsAllSend.Rows[i]["SENDMSG"].ToString().Trim();
                strDeptCode = rsAllSend.Rows[i]["DEPTCODE"].ToString().Trim();
                strPano = rsAllSend.Rows[i]["PANO"].ToString().Trim();
                strSname = rsAllSend.Rows[i]["SNAME"].ToString().Trim();
                strROWID = rsAllSend.Rows[i]["ROWID"].ToString().Trim();

                strRTime = rsAllSend.Rows[i]["RTime2"].ToString().Trim();
                strRDate = rsAllSend.Rows[i]["RTime"].ToString().Trim();
                strGBPUSH = rsAllSend.Rows[i]["GBPUSH"].ToString().Trim();

                //에약, 즉시 발송 구분 
                if (cpublic.strSysDate.CompareTo(strRTime) < 0)
                {
                    strGubun = "1";
                }
                else
                {
                    strGubun = "0";
                }

                if (strRTime.Trim() == "") { strRTime = DateTime.Now.ToString("yyyyMMddHHmm"); } //KT모듈에는 RTIME은 NOT NULL임   NULL일경우 오류 발생함

                MYSQL_KT_MMS_IMG_INSERT(strTel, strPano, strMsg, strSTime, DateTime.Now.ToString("yyyyMMddHHmm"), strRettel, strGubun);
          
                clsDB.setBeginTran(clsDB.DbCon);

                SQL = " update ADMIN.etc_sms set ";
                SQL += " sendtime = to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'), ";
                SQL += " state = '" + GstrSMS114_Result + "', ";
                SQL += " GbPush = '" + strOK + "', ";
                SQL += " PSMHSEND = 'Y'";
                SQL += " where rowid = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    MessageBox.Show("ETC_SMS 업데이트 에러", "확인");
                    return;
                }
                else
                {
                    TxtEDateSend.Text = cpublic.strSysDate + " " + cpublic.strSysTime;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }

            rsAllSend.Dispose();
            rsAllSend = null;
        }

        private void TmrFlow_Tick(object sender, EventArgs e)
        {
            int i = 0;
            int nTime = 0;
            int nTimeMax = 0;

            nTime = Convert.ToInt16(VB.Val(txtTimeCnt.Text));
            nTimeMax = Convert.ToInt16(VB.Val(CboTimeCycle.Text));

            if (nTime < nTimeMax)
            {
                txtTimeCnt.Text = (nTime + 1).ToString();
                TmrFlow.Enabled = true;
                TmrAction.Enabled = false;
            }
            else
            {
                TmrAction.Enabled = true;
            }
        }

        private void TmrAction_Tick(object sender, EventArgs e)
        {
            int nTime = 0;
            int nTimeMax = 0;

            read_sysdate();

            //▼--------------------------------------------------------------------▼
            nTime = Convert.ToInt16(VB.Val(txtTimeCnt.Text));
            nTimeMax = Convert.ToInt16(VB.Val(CboTimeCycle.Text));

            if (nTime >= nTimeMax)
            {
                txtTimeCnt.Text = "1";
                TmrFlow.Enabled = false;
                lblShow.Text = "메세지 전송중...";
                Application.DoEvents();
            }
            else
            {
                return;
            }
            //▲--------------------------------------------------------------------▲

            Consult_To_Doctor_Send_ReadTime(); //마취과 피부과는 제외
            Consult_To_Doctor_Req_RealTime();  //마취과는 제외

            Exam_Specmst_Sun_Send(); //코로나 선별 진료 검체 결과 문자 발송            

            switch (CF.READ_YOIL(clsDB.DbCon, cpublic.strSysDate))
            {
                case "월요일":
                case "화요일":
                case "수요일":
                case "목요일":
                case "금요일":
                    if (cpublic.strSysTime.CompareTo("07:00") >= 0 && cpublic.strSysTime.CompareTo("18:20") <= 0)
                    {
                        //컨설트 전송 마취과(타과에서 마취과로 의뢰시 마취가에서 수신함)
                        //컨설트생성시 마취과로 바로 전송함(타과->마취과 의뢰시)
                        Consult_PC_Only_Realtime_Send(); //마취과
                    }
                    break;
            }

            //ICU 전실 문자 전송
            Transfor_To_Doctor_Send_RealTime();

            //흡입제 발생시 문자 보내기
            if (cpublic.strSysTime.CompareTo("09:00") >= 0 && cpublic.strSysTime.CompareTo("16:30") <= 0)
            {
                InserGubun39(cpublic.strSysDate);
            }

            //IPD_입원자_의사에게문자 
            IPD_입원자_의사에게문자();

            //제한 항생제 관련 문자 메세지 실시간으로 전송
            switch (CF.READ_YOIL(clsDB.DbCon, cpublic.strSysDate))
            {
                case "토요일":
                case "일요일":
                    if (cpublic.strSysTime.CompareTo("11:30") >= 0 && cpublic.strSysTime.CompareTo("12:00") <= 0)
                    {
                        SMS_OCS_ANTI_MST();
                    }
                    break;
                default:
                    if (cpublic.strSysTime.CompareTo("17:30") >= 0 && cpublic.strSysTime.CompareTo("18:30") <= 0)
                    {
                        SMS_OCS_ANTI_MST();
                    }
                    break;
            }

            //인공신장실 안내 문자 
            switch (CF.READ_YOIL(clsDB.DbCon, cpublic.strSysDate))
            {
                case "월요일":
                case "화요일":
                    if (Convert.ToInt16(VB.Mid(cpublic.strSysDate, 9, 2)) >= 21)
                    {
                        if (cpublic.strSysTime.CompareTo("14:00") >= 0 && cpublic.strSysTime.CompareTo("15:00") <= 0)
                        {
                            HD_SMS_SEND();
                        }
                    }
                    break;
            }

            //의료급여 내시경 예약환자 원무과 직원에게 통보함
            if (cpublic.strSysTime.CompareTo("08:00") >= 0 && cpublic.strSysTime.CompareTo("08:30") <= 0)
            {
                의료급여_내시경();
            }

            //직원 생일, 축일자 조회 
            if (cpublic.strSysTime.CompareTo("08:50") >= 0 && cpublic.strSysTime.CompareTo("09:20") <= 0)
            {
                INSA_Birthday();
            }

            if (cpublic.strSysTime.CompareTo("09:00") >= 0 && cpublic.strSysTime.CompareTo("09:30") <= 0)
            {
                Mibi_Chart_Send();
            }

            //방사선과 익일 예약자 SMS DATA 형성 
            if (cpublic.strSysTime.CompareTo("13:00") >= 0 && cpublic.strSysTime.CompareTo("13:30") <= 0)
            {
                OPD_DRUG_SMS_Send();
            }

            //종검 예약자중 대장내시경 검사자 3~4일전 안내문자
            //일반건진 암예약 전송(검진 3일전 예약문자 발송
            if (cpublic.strSysTime.CompareTo("14:00") >= 0 && cpublic.strSysTime.CompareTo("15:00") <= 0)
            {
                HEA_TO_TX32_Reserved_SMS_Send();
                HIC_CANCER_3Day_SMS_Send();
            }

            //심평원 연령,병용 금기 고지사항
            if (cpublic.strSysTime.CompareTo("09:00") >= 0 && cpublic.strSysTime.CompareTo("09:05") <= 0)
            {
                Date_Change_심평원_연령금기_고지사항();
                Date_Change_심평원_병용금기_고지사항();
            }

            if (cpublic.strSysTime.CompareTo("18:00") >= 0 && cpublic.strSysTime.CompareTo("18:30") <= 0)
            {
                ECHO_SMS_Send();
            }

            //NBST검사 결과 전종 SMS Data 형성: (18:00 ~18:20)
            if (cpublic.strSysTime.CompareTo("18:00") >= 0 && cpublic.strSysTime.CompareTo("18:20") <= 0)
            {
                NBST_SMS_Send();
            }

            if (cpublic.strSysTime.CompareTo("18:00") >= 0 && cpublic.strSysTime.CompareTo("18:20") <= 0)
            {
                POSCO_3Day_Receive_SMS_Send();                  //포스코 검사예약 문자전송(3일전 문자전송)
                POSCO_7Day_Colonoscopy_Receive_SMS_Send();      //포스코 검사예약(대장내시경) 문자전송(7일전 문자전송)
                OPD_CANCER_SERVICE_Receive_SMS_Send();          //외래 암환자 안부문자
            }

            if (cpublic.strSysTime.CompareTo("07:00") >= 0 && cpublic.strSysTime.CompareTo("18:20") <= 0)
            {
                POSCO_Reserve_SMS_Send();  //포스코 검사 예약 등록시 문자전송(접수당일 전송)
            }

            switch (CF.READ_YOIL(clsDB.DbCon, cpublic.strSysDate))
            {
                case "월요일":
                case "화요일":
                case "수요일":
                case "목요일":
                case "금요일":
                    if (cpublic.strSysTime.CompareTo("08:00") >= 0 && cpublic.strSysTime.CompareTo("18:00") <= 0)
                    {
                        Web_To_InsaRecruit_SMS_Send_NEW();
                        Web_To_InsaRecruit_2_SMS_Send_NEW();
                    }
                    break;
                case "토요일":
                    if (cpublic.strSysTime.CompareTo("08:00") >= 0 && cpublic.strSysTime.CompareTo("13:00") <= 0)
                    {
                        Web_To_InsaRecruit_SMS_Send_NEW();
                        Web_To_InsaRecruit_2_SMS_Send_NEW();
                    }
                    break;
            }

            //내시경 예약자 명단 전송 - 5일전에 통보 - 내시경 SMS Data 형성(17:00)
            if (cpublic.strSysTime.CompareTo("17:00") >= 0 && cpublic.strSysTime.CompareTo("17:30") <= 0)
            {
                Endo_Resv_SMS_Send_NEW();
            }

            //류마티스내과 타이치운동 교육참석 안내 통보
            if (cpublic.strSysTime.CompareTo("09:00") >= 0 && cpublic.strSysTime.CompareTo("10:00") <= 0)
            {
                TAICHI_SMS_Send();
            }

            //'고가약품 사용예정 통보(외래진료실 -> 약제과 로 알림)
            High_Price_Drug_SMS_Send();

            //기호쌤 원무인원수 문자
            if (cpublic.strSysTime.CompareTo("08:00") >= 0 && cpublic.strSysTime.CompareTo("08:30") <= 0)
            {
                HANKIHO_SMS();
            }

            //행정 당직 문자 2020-11-27 8시 발송에서 9시 발송으로 변경
            if (cpublic.strSysTime.CompareTo("09:00") >= 0 && cpublic.strSysTime.CompareTo("09:30") <= 0)
            {
                DangJik_CALL_SEND();
            }

            //Ping Test할 IP_Address를 SET
            CheckIP_Set();

            //90일 이상 경과된 SMS는 삭제
            SMS_Old_DATA_Delete();

            //장례식장 SMS 전송
            Funeral_SMS_Send();

            //입원환자 생일SMS 전송
            if (cpublic.strSysTime.CompareTo("17:00") >= 0 && cpublic.strSysTime.CompareTo("17:30") <= 0)
            {
                Patient_Birth_Sms_Send();
            }

            //전자인증 관련 갱신 인증서 전송
            if (cpublic.strSysTime.CompareTo("09:00") >= 0 && cpublic.strSysTime.CompareTo("09:30") <= 0)
            {
                CERT_SMS_SEND();
            }

            //모바일 웹 연동 SMS 전송 추가 2015-01-15
            APP_SMS();

            //영상의학과 CVR SMS 전송
            CVR_Send();

            NST_Send();

            //진료회송SMS 전송
            RETURN_SMS_Send();

            //문자 샌드 시간 업데이트작업
            up_Bas_BCode_RunChk("문자샌드");


            //2021-08-13 KT SMS 구분 
            if (FstrKTSMS_사용여부 == "Y")
            {
                SMS_Many_Message_Send_KT_SMS();

                //코로나 사전예약 이미지 문자 발송  구분 84 전용
                MMS_IMG_Message_Send_KT_MMS();
            }

            //전송목록 조회
            SMS_Many_Message_Send_View();


            //전송시켰으면 처음부터 다시 카운트시작
            TmrAction.Enabled = false;
            TmrFlow.Enabled = true;
            lblShow.Text = "메세지 대기중...";
            Application.DoEvents();
        }

        void SMS_BROKER_SEND_SDATA2(string ArgCode, string ArgGubun, string ArgRHphone, string ArgMsg, bool argSendChk)
        {
            DataTable rsSend = null;
            DataTable rsSend2 = null;

            string strS = "";
            int i = 0;
            int nREAD = 0;
            string strDept = "";

            strDept = "XX";

            SQL = " SELECT Round((SYSDATE - JOBDATE) * 24 * 60) " + ComNum.VBLF;
            SQL += " From ADMIN.ETC_SMS " + ComNum.VBLF;
            SQL += " Where TRUNC(JOBDATE) = TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND GUBUN ='T1' " + ComNum.VBLF;
            SQL += " AND Bigo ='" + ArgCode + "' " + ComNum.VBLF;
            SQL += " AND  (ROUND((SYSDATE - JOBDATE)  * 24 * 60) ) > 180 " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rsSend, SQL, clsDB.DbCon);  

            if (rsSend.Rows.Count > 0)
            {
                strDept = "X1";
            }

            rsSend.Dispose();
            rsSend = null;

            if (argSendChk == true)
            {
                SQL = " SELECT ROWID FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE TRUNC(JOBDATE) =TRUNC(SYSDATE) " + ComNum.VBLF;
                SQL += " AND GUBUN ='T1' " + ComNum.VBLF;
                SQL += " AND Bigo ='" + ArgCode + "' " + ComNum.VBLF;
                SQL += " AND DeptCode ='" + strDept + "' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rsSend2, SQL, clsDB.DbCon);

                if (rsSend2.Rows.Count > 0)
                {
                    rsSend2.Dispose();
                    rsSend2 = null;
                    return;
                }

                rsSend2.Dispose();
                rsSend2 = null;
            }

            SQL = " SELECT NAME FROM ADMIN.BAS_BCODE " + ComNum.VBLF;
            SQL += " WHERE  1=1 " + ComNum.VBLF;
            SQL += " AND GUBUN ='ETC_샌드프로그램체크_SMS' " + ComNum.VBLF;
            SQL += " AND GUBUN2 ='" + ArgCode + "' " + ComNum.VBLF;
            SQL += " AND NAME IS NOT NULL " + ComNum.VBLF;
            SQL += " AND (DELDATE IS NULL OR DELDATE ='') " + ComNum.VBLF;
            SQL += " ORDER BY CODE " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rsSend, SQL, clsDB.DbCon);

            nREAD = rsSend.Rows.Count;

            if (nREAD > 0)
            {
                for (i = 0; i < nREAD; i++)
                {

                    SQL = "INSERT INTO ADMIN.ETC_SMS ( JOBDATE,PANO,SNAME,HPHONE,GUBUN,DEPTCODE,DRCODE,RTIME,RETTEL,";
                    SQL += "SENDMSG , ENTSABUN, ENTDATE,  BIGO, GBPUSH,PSMHSEND) VALUES (";
                    SQL += "SYSDATE,'','','" + rsSend.Rows[i]["NAME"].ToString().Trim() + "','" + ArgGubun + "','" + strDept + "','0000',";
                    SQL += "SYSDATE,'" + ArgRHphone + "','" + ArgMsg + "'," + clsType.User.Sabun + ",SYSDATE,'" + ArgCode + "' , 'N','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            rsSend.Dispose();
            rsSend = null;
        }

        void up_Bas_BCode_RunChk(string ArgCode)
        {
            DataTable RsTemp = null;

            SQL = "  SELECT ROUND((SYSDATE - JDATE)  * 24 * 60) min_diff , " + ComNum.VBLF;
            SQL += " CASE WHEN ROUND((SYSDATE - JDATE)  * 24 * 60) > CNT THEN 'CHK' ELSE 'OK' END min_diff2 " + ComNum.VBLF;
            SQL += " FROM ADMIN.BAS_BCODE " + ComNum.VBLF;
            SQL += " WHERE GUBUN ='ETC_샌드프로그램체크' " + ComNum.VBLF;
            SQL += " AND Code ='" + ArgCode + "'" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref RsTemp, SQL, clsDB.DbCon);

            if (RsTemp.Rows.Count > 0)
            {
                if (RsTemp.Rows[0]["min_diff2"].ToString().Trim() != "OK")
                {
                    SMS_BROKER_SEND_SDATA2(ArgCode, "T1", "0542608335", ArgCode + " 실행체크 : " + RsTemp.Rows[0]["min_diff"].ToString().Trim() + "분 지남", true);
                }
            }
        }

        void RETURN_SMS_Send()
        {
            DataTable AdoRes = null;
            DataTable rs1 = null;

            int i = 0;
            int nREAD = 0;
            string strSname = "";
            string strPano = "";
            string strTime = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strMinRTime = "";
            string strMAGAMSABUN = "";

            read_sysdate();

            //전송 희망시각 SET
            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT ACTDATE, PANO, SNAME, MAGAM, MAGAMDATE, MAGAMSABUN " + ComNum.VBLF;
            SQL += " FROM ADMIN.ETC_RETURN A, ADMIN.ETC_RETURN_CODE B " + ComNum.VBLF;
            SQL += " WHERE A.H_CODE = B.Code " + ComNum.VBLF;
            SQL += " AND A.MAGAMDATE >= TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND A.OPDIPD1 = 'OPD' " + ComNum.VBLF;
            SQL += " AND A.MAGAM = 'Y' " + ComNum.VBLF;
            SQL += " AND B.GUBUN = '01' " + ComNum.VBLF;
            SQL += " AND B.DELDATE IS NULL " + ComNum.VBLF;
            SQL += " AND B.CHARGE IS NOT NULL " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strTel = "01096859036";
                strPano = VB.Val(AdoRes.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                strSname = AdoRes.Rows[i]["SNAME"].ToString().Trim();
                strMAGAMSABUN = AdoRes.Rows[i]["MAGAMSABUN"].ToString().Trim();

                //이미자료를 넘겼는지 확인함
                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND Pano='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND Gubun='72'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";

                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                if (strMinRTime != "" && strMinRTime.CompareTo(strTime) <= 0)
                {
                    strTel = "";
                }

                //재원중인 환자는 제외
                SQL = " SELECT Pano FROM ADMIN.IPD_NEW_MASTER " + ComNum.VBLF;
                SQL += " WHERE Pano='" + strPano + "' " + ComNum.VBLF;
                SQL += "  AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    strTel = "";
                }

                rs1.Dispose();
                rs1 = null;

                SQL = " SELECT Ptno,SUCODE FROM ADMIN.OCS_OORDER " + ComNum.VBLF;
                SQL += " WHERE Ptno='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND BDATE >= TRUNC(SYSDATE) " + ComNum.VBLF;
                SQL += " AND SUCODE IN ('IA231', 'IA213') " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    strTel = "";
                }

                rs1.Dispose();
                rs1 = null;

                //SMS 자료에 INSERT
                if (strSname != "" && strTel != "")
                {
                    strRettel = "0542720151";

                    strMsg = "등록번호 : " + AdoRes.Rows[i]["PANO"].ToString().Trim();
                    strMsg = strMsg + " " + AdoRes.Rows[i]["SName"].ToString().Trim() + "님";
                    strMsg = strMsg + " 회송확인바랍니다.";

                    //자료를 DB에 INSERT
                    SQL = "INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                    SQL += "RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'";
                    SQL += strPano + "','" + strSname + "','" + strTel + "','72','','',";
                    SQL += "TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon); 

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("진료의뢰회신문자 저장오류", "확인");
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }


        void NST_Send()
        {
            DataTable AdoRes = null;
            DataTable rs1 = null;

            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            string strPano = "";
            string strSname = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strRoomCode = "";
            string strDeptCode = "";

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT PANO, SNAME, DEPTCODE, DRCODE, WARDCODE,ROOMCODE, COMPLITE " + ComNum.VBLF;
            SQL += " FROM ADMIN.DIET_NST_PROGRESS " + ComNum.VBLF;
            SQL += " WHERE COMPLITE IS NOT NULL " + ComNum.VBLF;
            SQL += " AND GBSMS  IS NULL " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            if (nREAD > 0)
            {
                for (i = 0; i < nREAD; i++)
                {
                    strPano = AdoRes.Rows[i]["PANO"].ToString().Trim();
                    strSname = AdoRes.Rows[i]["SName"].ToString().Trim();
                    strDeptCode = AdoRes.Rows[i]["DEPTCODE"].ToString().Trim();
                    strRoomCode = AdoRes.Rows[i]["WardCode"].ToString().Trim() + "(" + AdoRes.Rows[i]["RoomCode"].ToString().Trim() + ")";

                    //핸드폰체크
                    //핸드폰체크
                    SQL = " SELECT A.HTEL " + ComNum.VBLF;
                    SQL += " FROM ADMIN.INSA_MST A, ADMIN.OCS_DOCTOR B  " + ComNum.VBLF;
                    SQL += " WHERE a.SABUN = b.SABUN " + ComNum.VBLF;
                    SQL += " AND b.DrCode ='" + AdoRes.Rows[i]["DRCODE"].ToString().Trim() + "' " + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                    nREAD2 = rs1.Rows.Count;

                    if (nREAD2 > 0)
                    {
                        strTel = rs1.Rows[0]["HTEL"].ToString().Trim();

                        strMsg = "★의뢰한 NST 컨설트 회신★" + strDeptCode + "/" + strPano + "/" + strSname + "/" + strRoomCode;

                        strRettel = "0542608084";

                        if (strTel != "")
                        {
                            SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                            SQL += "RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'";
                            SQL += strPano + "','" + strSname + "', '" + strTel + " ','69','','',";
                            SQL += "SYSDATE ,'" + strRettel + "','','" + strMsg + "','Y')";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("ETC_SMS NST 문자저장오류", "확인");
                                return;
                            }

                            SQL = " UPDATE ADMIN.DIET_NST_PROGRESS SET";
                            SQL += " GBSMS ='Y'";
                            SQL += " WHERE PANO ='" + strPano + "'";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("DIET_NST_PROGRESS NST 문자갱신오류", "확인");
                                return;
                            }
                        }
                    }

                    rs1.Dispose();
                    rs1 = null;
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void CVR_Send()
        {
            DataTable AdoRes = null;
            DataTable rs1 = null;

            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;

            string strPano = "";
            string strName = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strIO = "";
            string strROWID = "";
            string strRoomCode = "";
            string strxjong = "";

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT Ipdopd, Cvr, Cvr_Gubun,DeptCode,DrCode,Pano,SName,ROWID, WardCode,RoomCode,xjong " + ComNum.VBLF;
            SQL += " FROM ADMIN.XRAY_DETAIL " + ComNum.VBLF;
            SQL += " WHERE Cvr = 'Y' " + ComNum.VBLF;
            SQL += " AND Cvr_Gubun = '1' " + ComNum.VBLF;
            SQL += " AND DEPTCODE IN ('ER', 'NE', 'NS') " + ComNum.VBLF;
            SQL += " AND CVR_Date >= TRUNC(SYSDATE-5) " + ComNum.VBLF;
            SQL += " AND SeekDate >= TRUNC(SYSDATE-5)" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            if (nREAD > 0)
            {
                strPano = AdoRes.Rows[i]["Pano"].ToString().Trim();
                strName = AdoRes.Rows[i]["SName"].ToString().Trim();
                strIO = AdoRes.Rows[i]["IPDOPD"].ToString().Trim();
                strROWID = AdoRes.Rows[i]["ROWID"].ToString().Trim();

                strRoomCode = AdoRes.Rows[i]["WARDCODE"].ToString().Trim() + "(" + AdoRes.Rows[i]["ROOMCODE"].ToString().Trim() + ")";
                strxjong = AdoRes.Rows[i]["xjong"].ToString().Trim();

                //핸드폰체크
                SQL = " SELECT A.HTEL " + ComNum.VBLF;
                SQL += " FROM ADMIN.INSA_MST A, ADMIN.OCS_DOCTOR B  " + ComNum.VBLF;
                SQL += " WHERE a.SABUN = b.SABUN " + ComNum.VBLF;
                SQL += " AND b.DrCode ='" + AdoRes.Rows[i]["DRCODE"].ToString().Trim() + "' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                nREAD2 = rs1.Rows.Count;

                strTel = "";

                if (nREAD2 > 0)
                {
                    strTel = rs1.Rows[0]["HTEL"].ToString().Trim();

                    switch (AdoRes.Rows[i]["IPDOPD"].ToString().Trim())
                    {
                        case "O": strMsg = strPano + "(" + strName + ") 환자분 CVR대상입니다."; break;
                        case "I": strMsg = strRoomCode + " (" + strName + ") 환자분 CVR대상입니다."; break;
                        default: strMsg = "CVR대상입니다."; break;
                    }

                    switch (AdoRes.Rows[i]["xjong"].ToString().Trim())
                    {
                        case "4": strRettel = "0542608173"; break;
                        case "5": strRettel = "0542608172"; break;
                        default: strRettel = "1004"; break;
                    }

                    if (strTel != "")
                    {
                        SQL = "INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                        SQL += "RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'";
                        SQL += strPano + "','" + strName + "', '" + strTel + " ','65','','',";
                        SQL += "SYSDATE ,'" + strRettel + "','','" + strMsg + "','Y')";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("ETC_SMS CVR 문자저장오류", "확인");
                            return;
                        }

                        SQL = " UPDATE ADMIN.XRAY_DETAIL SET";
                        SQL += " CVR ='S', CVR_SEND =SYSDATE";
                        SQL += "WHERE ROWID ='" + strROWID + "'";
                        SQL += " AND PANO ='" + strPano + "' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("XRAY_DETAIL CVR 문자갱신오류", "확인");
                            return;
                        }
                    }
                }

                rs1.Dispose();
                rs1 = null;
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void APP_SMS()
        {
            DataTable RsBas = null;
            DataTable Rs = null;
            int i = 0;
            int nREAD = 0;

            string strPano = "";
            string strSENDMSG = "";
            string strSname = "";
            string strHTEL = "";
            string strOK = "";

            clsDbMySql.DBConnect("221.157.239.2", "3306", "psmh", "psmh", "phsmh");

            SQL = " SELECT SMSDATE, PID, SENDMSG  FROM tb_sms " + ComNum.VBLF;
            SQL += " WHERE SENDTIME IS NULL " + ComNum.VBLF;

            RsBas = clsDbMySql.GetDataTable(SQL);

            nREAD = RsBas.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strOK = "OK";
                strHTEL = "";
                strPano = RsBas.Rows[i]["PID"].ToString().Trim();
                strSENDMSG = RsBas.Rows[i]["SENDMSG"].ToString().Trim();

                SQL = " SELECT HPHONE, SNAME  FROM BAS_PATIENT " + ComNum.VBLF;
                SQL += " WHERE PANO ='" + strPano + "'  " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                if (Rs.Rows.Count > 0)
                {
                    strHTEL = VB.Replace(Rs.Rows[0]["HPHONE"].ToString().Trim(), "-", "");
                    strSname = Rs.Rows[0]["SNAME"].ToString().Trim();
                }

                Rs.Dispose();
                Rs = null;

                if (strHTEL == "") { strOK = "OK"; }
                if (strOK == "OK")
                {
                    strHTEL = "01093284620";

                    SQL = "INSERT INTO ADMIN.ETC_SMS (JOBDATE, PANO, SNAME , HPHONE , GUBUN, SENDMSG,PSMHSEND) ";
                    SQL += "VALUES ( SYSDATE,'" + strPano + "','" + strSname + "','" + strHTEL + "','61','" + strSENDMSG + "','Y' )";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    SQL = " UPDATE tb_sms SET ";
                    SQL += "  sendtime = '" + VB.Replace(cpublic.strSysDate, "-", "") + cpublic.strSysTime + "'  ";
                    SQL += "  WHERE PID = '" + strPano + "'   ";

                    clsDbMySql.ExecuteNonQuery(SQL);
                }
            }

            clsDbMySql.DisDBConnect();
        }

        void CERT_SMS_SEND()
        {
            DataTable AdoRes = null;
            DataTable Rs = null;
            DataTable rs1 = null;

            int i = 0;
            int nREAD = 0;
            string strJobTime = "";
            string strSabun = "";

            string strTel = "";
            string strSname = "";
            string strPano = "";
            string strMinRTime = "";
            string strRettel = "";
            string strMsg = "";

            read_sysdate();

            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            //SMS전송 대상자 자료조회
            SQL = " SELECT SABUN, CDATE FROM( " + ComNum.VBLF;
            SQL += " SELECT A.SABUN, TRUNC(MAX(CERDATE) + 365) CDATE " + ComNum.VBLF;
            SQL += " FROM ADMIN.INSA_MSTS A, ADMIN.INSA_MST B " + ComNum.VBLF;
            SQL += " WHERE A.SABUN = B.SABUN " + ComNum.VBLF;
            SQL += " AND B.TOIDAY IS NULL " + ComNum.VBLF;
            SQL += " GROUP BY A.SABUN) " + ComNum.VBLF;
            SQL += " WHERE CDATE = TRUNC(SYSDATE)" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strSabun = AdoRes.Rows[i]["SABUN"].ToString().Trim();

                //INSA_MST 에서 전화번호, 등록번호, 이름 조회
                SQL = " SELECT KORNAME, HTEL, PANO " + ComNum.VBLF;
                SQL += " FROM ADMIN.INSA_MST " + ComNum.VBLF;
                SQL += " WHERE SABUN = '" + strSabun + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                strTel = Rs.Rows[0]["HTEL"].ToString().Trim();
                strSname = Rs.Rows[0]["KORNAME"].ToString().Trim();
                strPano = Rs.Rows[0]["PANO"].ToString().Trim();

                Rs.Dispose();
                Rs = null;

                //이미 자료를 넘겼는지 확인함 
                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND HPHONE='" + strTel + "' " + ComNum.VBLF;
                SQL += " AND Gubun='75'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";

                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                //SMS 자료에 INSERT
                if (strMinRTime == "" && strTel != "" && strSabun != "")
                {
                    strRettel = "0542608041";

                    strMsg = "포항성모병원 전자서명 인증서 갱신 예정입니다. 참고하시기 바랍니다. 의료정보팀 8041";

                    //자료를 DB에 INSERT 
                    SQL = "INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                    SQL += "RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'";
                    SQL += strPano + "','" + strSname + "','" + strTel + "','75','','',";
                    SQL += "TO_DATE('" + strJobTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("전자인증 관련 인증서갱신 문자 저장오류", "확인");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Patient_Birth_Sms_Send()
        {
            DataTable Rs = null;
            DataTable rs1 = null;

            int i = 0;
            int nREAD = 0;
            int nREAD_2 = 0;
            string strWardCode = "";
            string strName = "";
            string strDateTime = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strGubun = "";
            string strPano = "";
            string strBirth = "";

            strDateTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strTel = "010-6524-3120";
            strRettel = "054-272-0151";
            strGubun = "54";

            SQL = " SELECT b.indate, " + ComNum.VBLF;
            SQL += " a.pano apano, b.wardcode bwardcode, a.sname asname, a.birth abirth" + ComNum.VBLF;
            SQL += " FROM ADMIN.bas_patient a, " + ComNum.VBLF;
            SQL += " ADMIN.ipd_new_master b " + ComNum.VBLF;
            SQL += " WHERE a.pano = b.pano(+) " + ComNum.VBLF;
            SQL += " AND b.gbsts = '0' " + ComNum.VBLF;
            SQL += " and b.jdate =to_date('1900-01-01','yyyy-mm-dd') " + ComNum.VBLF;
            SQL += " AND TO_CHAR(a.birth,'mm-dd') = TO_CHAR(SYSDATE + 1,'mm-dd') " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < nREAD; i++)
            {
                strPano = Rs.Rows[i]["APANO"].ToString().Trim();
                strWardCode = Rs.Rows[i]["BWARDCODE"].ToString().Trim();
                strName = Rs.Rows[i]["asname"].ToString().Trim();
                strBirth = Rs.Rows[i]["ABIRTH"].ToString().Trim();
                strMsg = "(" + strWardCode + ")병동 (" + strName + ") 님 " + VB.Mid(strBirth, 6, 2) + "월" + VB.Mid(strBirth, 9, 2) + "일 생일입니다.";

                SQL = " select rowid " + ComNum.VBLF;
                SQL += " from ADMIN.etc_sms " + ComNum.VBLF;
                SQL += " where gubun = '" + strGubun + "' " + ComNum.VBLF;
                SQL += " and sname = '" + strName + "' " + ComNum.VBLF;
                SQL += " and bigo = '" + strWardCode + "' " + ComNum.VBLF;
                SQL += " and pano = '" + strPano + "'" + ComNum.VBLF;
                SQL += " and trunc(jobdate) = trunc(sysdate)" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                nREAD_2 = rs1.Rows.Count;

                if (nREAD_2 == 0)
                {
                    SQL = "insert into ADMIN.etc_sms(";
                    SQL += "jobdate, pano, bigo, sname, hphone, rettel, gubun, sendmsg,PSMHSEND) values(";
                    SQL += "to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'),";
                    SQL += "'" + strPano + "', '" + strWardCode + "', '" + strName + "',";
                    SQL += "'" + strTel + "', '" + strRettel + "', '" + strGubun + "','" + strMsg + "','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                rs1.Dispose();
                rs1 = null;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            Rs.Dispose();
            Rs = null;

        }

        void Funeral_SMS_Send()
        {
            DataTable rsSub = null;
            int i = 0;
            string strName = "";
            string strHTEL = "";
            string strMsg = "";
            string strCUSTNO = "";

            //'===========================
            //'장례식장 고인 문자 발송(아직 사용 안함)
            //'===========================
            SQL = " SELECT CUSTNO, DEAD_NAME, TEL HPHONE " + ComNum.VBLF;
            SQL += " FROM ADMIN.FMS_SMS " + ComNum.VBLF;
            SQL += " WHERE GUBUN IS NULL" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rsSub, SQL, clsDB.DbCon);

            if (rsSub.Rows.Count > 0)
            {
                for (i = 0; i < rsSub.Rows.Count; i++)
                {
                    strName = rsSub.Rows[i]["DEAD_NAME"].ToString().Trim();
                    strHTEL = rsSub.Rows[i]["HPHONE"].ToString().Trim();
                    strCUSTNO = rsSub.Rows[i]["CUSTNO"].ToString().Trim();
                    strMsg = "큰 슬픔을 위로하오며 삼가 고인의 명복을 빕니다. -포항성모병원 병원장-";

                    SQL = "INSERT INTO ETC_SMS (JobDate,SName,HPhone,Gubun, RetTel,SendMsg,PSMHSEND) VALUES (";
                    SQL += "TO_DATE('" + cpublic.strSysDate + " " + cpublic.strSysDate + "','YYYY-MM-DD HH24:MI'),";
                    SQL += "'" + strName + "','" + strHTEL + "','34', '0542608048',";
                    SQL += "'" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    SQL = " UPDATE ADMIN.FMS_SMS SET GUBUN = '1' ";
                    SQL += " WHERE CUSTNO = '" + strCUSTNO + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }
        }

        void SMS_Old_DATA_Delete()
        {
            //90일 이상된 SMS 통보내역은 삭제함
            clsDB.setBeginTran(clsDB.DbCon);
            SQL = " DELETE ADMIN.ETC_SMS WHERE JOBDATE < TRUNC(SYSDATE - 90) ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            clsDB.setCommitTran(clsDB.DbCon);

        }

        void DangJik_CALL_SEND()
        {
            DataTable RsEdps1 = null;
            DataTable RsEdps2 = null;
            int i = 0;
            string strHTEL = "";
            string strDateTime = "";
            string strROWID = "";
            string strDName1 = "";
            string strDName2 = "";

            strDateTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strROWID = "";
            strDName1 = "";
            strDName2 = "";

            SQL = " SELECT DNAME1, DNAME2, ROWID " + ComNum.VBLF;
            SQL += " FROM ADMIN.ETC_DANGJIK " + ComNum.VBLF;
            SQL += " WHERE GUBUN ='97' " + ComNum.VBLF;
            SQL += " AND TDATE = TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND SMS IS NULL " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref RsEdps1, SQL, clsDB.DbCon);

            for (i = 0; i < RsEdps1.Rows.Count; i++)
            {
                strDName1 = RsEdps1.Rows[i]["DNAME1"].ToString().Trim();
                strDName2 = RsEdps1.Rows[i]["DNAME2"].ToString().Trim();
                strROWID = RsEdps1.Rows[i]["ROWID"].ToString().Trim();


                SQL = " SELECT  HTEL FROM ADMIN.INSA_MST " + ComNum.VBLF;
                if (strDName2 != "")
                {
                    SQL += " WHERE KORNAME ='" + strDName2 + "' " + ComNum.VBLF;
                }
                else
                {
                    SQL += " WHERE KORNAME ='" + strDName1 + "' " + ComNum.VBLF;
                }

                SQL += " AND TOIDAY IS NULL " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref RsEdps2, SQL, clsDB.DbCon);

                if (RsEdps2.Rows.Count > 0)
                {
                    strHTEL = VB.Replace(RsEdps2.Rows[0]["HTEL"].ToString().Trim(), "-", "");

                    if (strHTEL != "")
                    {
                        SQL = "insert into ADMIN.etc_sms(";
                        SQL += "jobdate, sname, hphone, rettel, gubun, sendmsg, PSMHSEND) values(";
                        SQL += "to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'),";
                        SQL += "'행정당직', '" + strHTEL + "', '054-260-8016', 'A', '" + "오늘 행정당직입니다." + "','Y')";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        SQL = " UPDATE ADMIN.ETC_DANGJIK SET SMS = '*' ";
                        SQL += " WHERE ROWID = '" + strROWID + "' ";
                        SQL += " AND GUBUN = '97' ";
                        SQL += " AND SMS IS NULL";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                }
                RsEdps2.Dispose();
                RsEdps2 = null;
            }
            RsEdps1.Dispose();
            RsEdps1 = null;
        }

        void HANKIHO_SMS()
        {
            DataTable RsEdps1 = null;
            DataTable rs1 = null;
            int i = 0;
            string strDateTime = "";
            string strSENDMSG1 = "";

            strDateTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            strSENDMSG1 = "";

            SQL = " select YYMMDD,sum(ocnt1+ocnt2) HAP from ADMIN.TONG_DAILY " + ComNum.VBLF;
            SQL += " where YYMMDD >= TO_CHAR(trunc(sysdate-3),'YYYYMMDD') " + ComNum.VBLF;
            SQL += " group by YYMMDD " + ComNum.VBLF;
            SQL += " order by YYMMDD " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref RsEdps1, SQL, clsDB.DbCon);

            for (i = 0; i < RsEdps1.Rows.Count; i++)
            {
                strSENDMSG1 = strSENDMSG1 + RsEdps1.Rows[i]["YYMMDD"].ToString().Trim() + " " + RsEdps1.Rows[i]["HAP"].ToString().Trim() + ComNum.VBLF;
            }

            RsEdps1.Dispose();
            RsEdps1 = null;

            SQL = " SELECT JOBDATE, RETTEL,SENDTIME FROM ETC_SMS " + ComNum.VBLF;
            SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
            SQL += " AND SNAME = '원무팀인원문자' " + ComNum.VBLF;
            SQL += " AND Gubun='80'" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

            if (rs1.Rows.Count == 0 && strSENDMSG1 != "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //SMS 자료에 INSERT 
                SQL = "insert into ADMIN.etc_sms(";
                SQL += "jobdate, sname, hphone, rettel, gubun, sendmsg,PSMHSEND) values(";
                SQL += "to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'),";
                SQL += "'원무팀인원문자', '01085669765', '054-260-8338', '80', '" + strSENDMSG1 + "','Y')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                SQL = "insert into ADMIN.etc_sms(";
                SQL += "jobdate, sname, hphone, rettel, gubun, sendmsg,PSMHSEND) values(";
                SQL += "to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'),";
                SQL += "'원무팀인원문자', '01071594679', '054-260-8338', '80', '" + strSENDMSG1 + "','Y')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                clsDB.setCommitTran(clsDB.DbCon);

            }

            rs1.Dispose();
            rs1 = null;
        }

        void High_Price_Drug_SMS_Send()
        {
            DataTable Rs = null;
            int i = 0;
            int nREAD = 0;
            string strPano = "";
            string strSucode = "";
            string strRDate = "";
            string strEntdate = "";
            string strSabun = "";
            string strQty = "";
            string strJobTime = "";
            string strGubun = "";
            string strDeptName = "";
            string strKorname = "";
            string strTel = "";
            string strRettel = "";
            string strSENDMSG = "";

            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strGubun = "49";
            strKorname = "이상희(요한릿다)";

            strTel = "010-4400-4756";
            strRettel = "0542608051";

            SQL = " select pano, sucode, rdate, entdate, entsabun, qty " + ComNum.VBLF;
            SQL += " from ADMIN.etc_mr_drug_sms " + ComNum.VBLF;
            SQL += " where deldate is null " + ComNum.VBLF;
            SQL += " and sms = 'N' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < nREAD; i++)
            {
                strPano = Rs.Rows[i]["PANO"].ToString().Trim();
                strSucode = Rs.Rows[i]["SUCODE"].ToString().Trim();
                strRDate = Rs.Rows[i]["RDATE"].ToString().Trim();
                strEntdate = Rs.Rows[i]["ENTDATE"].ToString().Trim();
                strSabun = Rs.Rows[i]["ENTSABUN"].ToString().Trim();
                strQty = Rs.Rows[i]["QTY"].ToString().Trim();
                strDeptName = Read_Insa_Sabun_by_DeptName(strSabun);
                strSENDMSG = VB.Mid(Rs.Rows[i]["RDate"].ToString().Trim(), 6, 2) + "월" + VB.Mid(Rs.Rows[i]["RDate"].ToString().Trim(), 9, 2) + "일 "
                             + strSucode + " " + strQty + "개 사용예정입니다. 준비부탁드립니다. -" + strDeptName;

                //약제과 수녀님(요한릿다)에게 전송
                SQL="insert into ADMIN.etc_sms(jobdate,pano,sname,hphone,gubun,rettel,sendmsg,PSMHSEND)values(";
                SQL+="to_date('"+strJobTime+"','yyyy-mm-dd hh24:mi'),'"+strPano+"','"+strKorname+"',";
                SQL+="'"+strTel+"','"+strGubun+"','"+strRettel+"','"+strSENDMSG+"','Y')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                SQL = " update ADMIN.etc_mr_drug_sms set ";
                SQL += " sms = 'Y'";
                SQL += " where pano = '" + strPano + "'";
                SQL += " and sucode = '" + strSucode + "'";
                SQL += " and entsabun = '" + strSabun + "'";
                SQL += " and rdate = to_date('" + strRDate + "', 'yyyy-mm-dd')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);

            Rs.Dispose();
            Rs = null;
        }

        void TAICHI_SMS_Send()
        {
            DataTable AdoRes = null;
            DataTable rs1 = null;

            int i = 0;
            int nREAD = 0;

            string strPano = "";
            string strName = "";
            string strTime = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";

            read_sysdate();

            switch (CF.READ_YOIL(clsDB.DbCon, cpublic.strSysDate))//월목에만 통보
            {
                case "월요일":
                case "목요일":
                    break;
                default:
                    return;
            }

            //오늘이 휴일인지 점검 
            SQL = " SELECT HOLYDAY " + ComNum.VBLF;
            SQL += " FROM ADMIN.BAS_JOB " + ComNum.VBLF;
            SQL += " WHERE JOBDATE=TRUNC(SYSDATE) " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            if (AdoRes.Rows[0]["HolyDay"].ToString().Trim() == "*")
            {
                AdoRes.Dispose();
                AdoRes = null;
                return;
            }

            AdoRes.Dispose();
            AdoRes = null;

            //전송희망시각 SET 
            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            //타이치 운동 대상자를 읽음
            SQL = " SELECT a.Pano,b.SName,b.HPhone " + ComNum.VBLF;
            SQL += " FROM ADMIN.ETC_CSINFO_DATA a, " + ComNum.VBLF;
            SQL += " ADMIN.BAS_PATIENT b " + ComNum.VBLF;
            SQL += " WHERE a.Gubun='121' " + ComNum.VBLF;
            SQL += " AND a.BDate<=TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND (a.EndDate IS NULL OR a.EndDate>=TRUNC(SYSDATE)) " + ComNum.VBLF;
            SQL += " AND a.Pano=b.Pano(+) " + ComNum.VBLF;
            SQL += " AND (B.GBSMS <> 'X' OR B.GBSMS IS NULL)" + ComNum.VBLF;  //2021-07-22 의료정보팀 요청으로 문자 수신비동의 빼고 다보냄
            SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " ORDER BY a.Pano,b.SName,b.HPhone " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strPano = VB.Val(AdoRes.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                strName = AdoRes.Rows[i]["Sname"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HPHONE"].ToString().Trim();

                //이미자료를 넘겼는지확인함

                SQL = " SELECT MIN(TO_CHAR(JobDate,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS  " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND Pano='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND Gubun='9' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    if (rs1.Rows[0]["RTime"].ToString().Trim() != "")
                    {
                        strTel = "";
                    }
                }

                rs1.Dispose();
                rs1 = null;

                //SMS 자료에 INSERT 등록
                if (strName != "" && strTel != "")
                {
                    strRettel = "0542608090";

                    strMsg = "★포항성모병원★";
                    strMsg = strMsg + strName + "님 " + VB.Mid(cpublic.strSysDate, 6, 2) + "월";
                    strMsg = strMsg + VB.Right(cpublic.strSysDate, 2) + "일 ";
                    strMsg = strMsg + "오후3시 타이치운동 참석하세요";

                    //자료를 DB에 INSERT
                    SQL = "INSERT INTO ETC_SMS(JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                    SQL +="RetTel,SendTime,SendMsg,PSMHSEND)VALUES(SYSDATE,'";
                    SQL +=strPano+"','"+strName+"','"+strTel+"','9','','',";
                    SQL +="TO_DATE('"+strTime+"','YYYY-MM-DD HH24:MI'),'";
                    SQL +=strRettel+"','','"+strMsg+"','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Endo_Resv_SMS_Send_NEW()
        {
            //        '<위대장내시경>
            //        ' 10 일전: OOO님내시경검사O월O일오후O시O분변경시연락바랍니다.
            //        '  4 일전: OOO님내시경검사O월O일오후O시O분오늘부터식사조절하십시오
            //        '  1 일전: OOO님안내문확인후저녁10시부터금식하십시오.
            //        '<위내시경>
            //        ' 10 일전: OOO님내시경검사O월O일오전O시O분변경시연락바랍니다.
            //        '  1 일전: OOO님안내문확인후저녁10시부터금식하십시오.

            DataTable AdoRes = null;
            DataTable rs1 = null;

            int i = 0;
            int nREAD = 0;

            string strPano = "";
            string strName = "";
            string strTime = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strMinRTime = "";

            read_sysdate();

            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT a.Ptno,b.SName,b.HPhone,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDate , b.gbsms ,  '10' day " + ComNum.VBLF;
            SQL += " FROM ADMIN.ENDO_JUPMST a, ADMIN.BAS_PATIENT b " + ComNum.VBLF;
            SQL += " WHERE a.RDate >= trunc(sysdate + 10) " + ComNum.VBLF;
            SQL += " AND a.RDate < trunc(sysdate + 11) " + ComNum.VBLF;
            SQL += " AND a.Ptno=b.Pano(+) " + ComNum.VBLF;
            SQL += " AND (b.GbSMS <> 'X' OR B.GBSMS IS NULL ) " + ComNum.VBLF;  //2021-07-20 의료정보팀 협의로 문자전송번경
            SQL += " AND a.Gbsunap NOT IN ('*') " + ComNum.VBLF;
            SQL += " AND a.GBIO NOT IN ('I') " + ComNum.VBLF; //입원은 제외
            SQL += " AND a.GbJob in ('3') " + ComNum.VBLF;
            SQL += " AND A.ORDERCODE NOT IN ('00440180','00440181','GI6','00440110','00440120','E7611SA') " + ComNum.VBLF; //2020-07-17 김수연 의뢰서
            SQL += " AND A.RESULTDATE IS NULL" + ComNum.VBLF;
            SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " GROUP BY a.Ptno,b.SName,b.HPhone,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') , b.gbsms " + ComNum.VBLF;
            SQL += " union all " + ComNum.VBLF;
            SQL += " SELECT a.Ptno,b.SName,b.HPhone,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDate , b.gbsms ,  '4' day " + ComNum.VBLF;
            SQL += " FROM ADMIN.ENDO_JUPMST a, ADMIN.BAS_PATIENT b " + ComNum.VBLF;
            SQL += " WHERE a.RDate >= trunc(sysdate + 4) " + ComNum.VBLF;
            SQL += " AND a.RDate < trunc(sysdate + 5) " + ComNum.VBLF;
            SQL += " AND a.Ptno=b.Pano(+) " + ComNum.VBLF;
            SQL += " AND (b.GbSMS <> 'X' OR B.GBSMS IS NULL ) " + ComNum.VBLF;
            SQL += " AND a.Gbsunap NOT IN ('*') " + ComNum.VBLF;
            SQL += " AND a.GBIO NOT IN ('I')" + ComNum.VBLF;
            SQL += " AND a.GbJob in ( '3')" + ComNum.VBLF;
            SQL += " AND A.ORDERCODE NOT IN ('00440180','00440181','GI6','00440110','00440120','E7611SA') " + ComNum.VBLF;
            SQL += " AND A.RESULTDATE IS NULL " + ComNum.VBLF;
            SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " GROUP BY a.Ptno,b.SName,b.HPhone,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') , b.gbsms " + ComNum.VBLF;
            SQL += " union all " + ComNum.VBLF;
            SQL += " SELECT a.Ptno,b.SName,b.HPhone,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDate , b.gbsms ,  '1' day " + ComNum.VBLF;
            SQL += " FROM ADMIN.ENDO_JUPMST a, ADMIN.BAS_PATIENT b " + ComNum.VBLF;
            SQL += " WHERE a.RDate >= trunc(sysdate + 1) " + ComNum.VBLF;
            SQL += " AND a.RDate < trunc(sysdate + 2) " + ComNum.VBLF;
            SQL += " AND a.Ptno=b.Pano(+) " + ComNum.VBLF;
            SQL += " AND (b.GbSMS <> 'X' OR B.GBSMS IS NULL ) " + ComNum.VBLF;
            SQL += " AND a.Gbsunap NOT IN ('*') " + ComNum.VBLF;
            SQL += " AND a.GBIO NOT IN ('I') " + ComNum.VBLF;
            SQL += " AND a.GbJob in ( '3','2') " + ComNum.VBLF;
            SQL += " AND A.ORDERCODE NOT IN ('00440180','00440181','GI6','00440110','00440120','E7611SA')  " + ComNum.VBLF;
            SQL += " AND A.RESULTDATE IS NULL " + ComNum.VBLF;
            SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " GROUP BY a.Ptno,b.SName,b.HPhone,TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') , b.gbsms " + ComNum.VBLF;
            SQL += " ORDER BY 1,2,3 " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strPano = VB.Val(AdoRes.Rows[i]["PTNO"].ToString().Trim()).ToString("00000000");
                strTime = AdoRes.Rows[i]["RDate"].ToString().Trim();

                strName = AdoRes.Rows[i]["Sname"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HPHONE"].ToString().Trim();

                //이미 자료를 넘겼는지 확인함
                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND Pano='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND Gubun='K' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";
                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                if (strMinRTime != "" && strMinRTime.CompareTo(strTime) <= 0)
                {
                    strTel = "";
                }

                //SMS 자료에 INSERT
                if (strName != "" && strTel != "")
                {
                    strRettel = "0542608241"; //내시경실 접수

                    switch (AdoRes.Rows[i]["Day"].ToString().Trim())
                    {
                        case "1":
                            strMsg = "♡포항성모병원♡";
                            strMsg = strMsg + AdoRes.Rows[i]["SName"].ToString().Trim() + "님 내시경검사 ";
                            strMsg = strMsg + "안내문 확인 후 저녁10시부터 금식하십시오";
                            break;
                        case "4":
                            strMsg = "♡포항성모병원♡";
                            strMsg = strMsg + AdoRes.Rows[i]["SName"].ToString().Trim() + "님 내시경검사 ";
                            strMsg = strMsg + VB.Format(VB.Val(VB.Mid(strTime, 6, 2)), "#0") + "월";
                            strMsg = strMsg + VB.Format(VB.Val(VB.Mid(strTime, 9, 2)), "#0") + "일 ";
                            strMsg = strMsg + "안내문 확인 후 식사조절 하십시오";
                            break;
                        case "10":
                            strMsg = "♡포항성모병원♡";
                            strMsg = strMsg + AdoRes.Rows[i]["SName"].ToString().Trim() + "님 내시경검사 ";
                            strMsg = strMsg + VB.Format(VB.Val(VB.Mid(strTime, 6, 2)), "#0") + "월";
                            strMsg = strMsg + VB.Format(VB.Val(VB.Mid(strTime, 9, 2)), "#0") + "일 입니다";
                            strMsg = strMsg + "변경시 연락바랍니다.";
                            break;
                    }

                    //자료를 DB에 INSERT 
                    SQL = "INSERT INTO ETC_SMS(JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime," + ComNum.VBLF;
                    SQL += "RetTel,SendTime,SendMsg,PSMHSEND)VALUES(SYSDATE,'" + strPano + "'," + ComNum.VBLF;
                    SQL += "'" + strName + "','" + strTel + "','K','',''," + ComNum.VBLF;
                    SQL += "TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'" + ComNum.VBLF;
                    SQL += strRettel + "','','" + strMsg + "','Y')" + ComNum.VBLF;

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Web_To_InsaRecruit_2_SMS_Send_NEW()
        {
            DataTable AdoRes = null;
            DataTable rs = null;

            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            string strIdx = "";
            string strUpdate = "";
            string strState = "";
            string strType = "";
            string strPhoto = "";
            string strName = "";
            string strpwd = "";
            string strEmail = "";
            string strPhone = "";
            string strData = "";

            string strMobile2 = "";
            string strSENDMSG = "";
            string strGubun = "";
            string strJepSu2 = "";

            strJepSu2 = "2";

            clsDbMySql.DBConnect("221.157.239.2", "3306", "psmh", "psmh", "home_pohangsmh_201708");

            //문자 전송할 목록 조회
            SQL = " SELECT ra_idx, ra_wdate, ra_mdate, ra_state, ra_gubun, ra_photo, ra_name_kor, ra_pwd, ra_email, ra_mobile " + ComNum.VBLF;
            SQL += " FROM recruit_apply " + ComNum.VBLF;
            SQL += " WHERE ra_state = '" + strJepSu2 + "' " + ComNum.VBLF;
            SQL += " AND ra_wdate >= '2017-11-28' " + ComNum.VBLF;

            AdoRes = clsDbMySql.GetDataTable(SQL);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strIdx = AdoRes.Rows[i]["ra_idx"].ToString().Trim();
                strData = AdoRes.Rows[i]["ra_wdate"].ToString().Trim();
                strUpdate = AdoRes.Rows[i]["ra_mdate"].ToString().Trim();
                strState = AdoRes.Rows[i]["ra_state"].ToString().Trim();
                strType = AdoRes.Rows[i]["ra_gubun"].ToString().Trim();
                strPhoto = AdoRes.Rows[i]["ra_photo"].ToString().Trim();
                strName = AdoRes.Rows[i]["ra_name_kor"].ToString().Trim();
                strpwd = AdoRes.Rows[i]["ra_pwd"].ToString().Trim();
                strEmail = AdoRes.Rows[i]["ra_email"].ToString().Trim();
                strPhone = AdoRes.Rows[i]["ra_mobile"].ToString().Trim();

                strMobile2 = "054-260-8012";
                strSENDMSG = strName + "님 입사지원서가 접수되었습니다. 당 병원에 지원해 주셔서 감사드리며 서류전형결과는 추후 개별통보 드립니다. 포항성모병원 인사담당자 드림";
                strGubun = "25";

                //이미 문자를 전송했는지 확인 작업
                SQL = " SELECT PANO, SENDTIME " + ComNum.VBLF;
                SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE GUBUN = '" + strGubun + "' " + ComNum.VBLF;
                SQL += " AND PANO = '" + strIdx + "' " + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strPhone + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs, SQL, clsDB.DbCon);

                nREAD2 = rs.Rows.Count;

                rs.Dispose();
                rs = null;

                clsDB.setBeginTran(clsDB.DbCon);

                if (nREAD2 < 1)
                {
                    SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG, ENTDATE, PSMHSEND) ";
                    SQL += " VALUES(SYSDATE, '" + strIdx + "', '" + strName + "', '" + strPhone + "', ";
                    SQL += " '" + strGubun + "', '" + strMobile2 + "', '" + strSENDMSG + "', TO_DATE('" + cpublic.strSysDate + "', 'YYYY-MM-DD'),'Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

            }

            clsDbMySql.DisDBConnect();
        }

        void Web_To_InsaRecruit_SMS_Send_NEW()
        {
            DataTable AdoRes = null;
            DataTable rs = null;

            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;

            string strIdx = "";
            string strDate = "";
            string strUpdate = "";
            string strState = "";
            string strType = "";
            string strPhoto = "";
            string strName = "";
            string strpwd = "";
            string strEmail = "";
            string strPhone = "";

            string strMobile = "";
            string strMobile_1 = "";
            string strMobile2 = "";
            string strSENDMSG = "";
            string strGubun = "";
            string strJepSu1 = "";

            strJepSu1 = "1";  //기획홍보팀에서 확인안한 상태(지원자가 수정가능한상태)

            clsDbMySql.DBConnect("221.157.239.2", "3306", "psmh", "psmh", "home_pohangsmh_201708");

            SQL = " SELECT ra_idx, ra_wdate, ra_mdate, ra_state, ra_gubun, ra_photo, ra_name_kor, ra_pwd, ra_email, ra_mobile " + ComNum.VBLF;
            SQL += " FROM recruit_apply " + ComNum.VBLF;
            SQL += " WHERE ra_state = '" + strJepSu1 + "'" + ComNum.VBLF;

            AdoRes = clsDbMySql.GetDataTable(SQL);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strIdx = AdoRes.Rows[i]["ra_idx"].ToString().Trim();
                strDate = VB.Left(AdoRes.Rows[i]["ra_wdate"].ToString().Trim(),16);
                strUpdate = AdoRes.Rows[i]["ra_mdate"].ToString().Trim();
                strState = AdoRes.Rows[i]["ra_state"].ToString().Trim();
                strType = AdoRes.Rows[i]["ra_gubun"].ToString().Trim();
                strPhoto = AdoRes.Rows[i]["ra_photo"].ToString().Trim();
                strName = AdoRes.Rows[i]["ra_name_kor"].ToString().Trim();
                strpwd = AdoRes.Rows[i]["ra_pwd"].ToString().Trim();
                strEmail = AdoRes.Rows[i]["ra_email"].ToString().Trim();
                strPhone = AdoRes.Rows[i]["ra_mobile"].ToString().Trim();

                strMobile = "010-3397-5673";   //문자받을번호(총무팀 이은희주임)
                strMobile_1 = "010-8552-0380"; //문자받을번호(총무팀 하상진쌤)

                strMobile2 = "054-272-0151";
                strSENDMSG = strName + "님 입사지원 등록";
                strGubun = "25";

                //이미 문자를 전송했는지 확인작업
                SQL = " SELECT PANO, SENDTIME " + ComNum.VBLF;
                SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE GUBUN = '" + strGubun + "'" + ComNum.VBLF;
                SQL += " AND PANO = '" + strIdx + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs, SQL, clsDB.DbCon);

                nREAD2 = rs.Rows.Count;

                rs.Dispose();
                rs = null;

                clsDB.setBeginTran(clsDB.DbCon);

                if (nREAD2 < 1)
                {
                    SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG, ENTDATE, PSMHSEND) ";
                    SQL += " VALUES(TO_DATE('" + strDate + "', 'YYYY-MM-DD HH24:MI'), '" + strIdx + "', '" + strName + "', '" + strMobile + "', ";
                    SQL += " '" + strGubun + "', '" + strMobile2 + "', '" + strSENDMSG + "', TO_DATE('" + cpublic.strSysDate + "', 'YYYY-MM-DD'),'Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //총무팀 하상진 선생님 추가
                    SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG, ENTDATE, PSMHSEND) ";
                    SQL += " VALUES(TO_DATE('" + strDate + "', 'YYYY-MM-DD HH24:MI'), '" + strIdx + "', '" + strName + "', '" + strMobile_1 + "', ";
                    SQL += " '" + strGubun + "', '" + strMobile2 + "', '" + strSENDMSG + "', TO_DATE('" + cpublic.strSysDate + "', 'YYYY-MM-DD'),'Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }

            clsDbMySql.DisDBConnect();
        }

        void POSCO_Reserve_SMS_Send()
        {
            DataTable Rs = null;
            DataTable rs2 = null;

            string strGubun = "";
            int i = 0;
            int k = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            string strJDate = "";
            string strPano = "";
            string[] strEXAMRES_DATE = new string[15];
            string strMin = "";
            string strMax = "";
            string strMM = "";
            string strDD = "";
            string strHH24 = "";
            string strName = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strSendTime = "";

            strMax = VB.DateAdd("yyyy", 2, cpublic.strSysDate) + "";
            strMax = VB.Left(strMax, 10) + " 23:00";

            strSendTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            strGubun = "5";
            strRettel = "0542608004";

            SQL = " SELECT JDATE, PANO, SNAME, HPHONE, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES1, 'YYYY-MM-DD HH24:MI') EXAMRES1, TO_CHAR(EXAMRES2, 'YYYY-MM-DD HH24:MI') EXAMRES2, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES3, 'YYYY-MM-DD HH24:MI') EXAMRES3, TO_CHAR(EXAMRES4, 'YYYY-MM-DD HH24:MI') EXAMRES4, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES6, 'YYYY-MM-DD HH24:MI') EXAMRES6, TO_CHAR(EXAMRES7, 'YYYY-MM-DD HH24:MI') EXAMRES7, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES8, 'YYYY-MM-DD HH24:MI') EXAMRES8, TO_CHAR(EXAMRES9, 'YYYY-MM-DD HH24:MI') EXAMRES9, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES10, 'YYYY-MM-DD HH24:MI') EXAMRES10, TO_CHAR(EXAMRES11, 'YYYY-MM-DD HH24:MI') EXAMRES11, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES12, 'YYYY-MM-DD HH24:MI') EXAMRES12, TO_CHAR(EXAMRES13, 'YYYY-MM-DD HH24:MI') EXAMRES13, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES14, 'YYYY-MM-DD HH24:MI') EXAMRES14, TO_CHAR(EXAMRES15, 'YYYY-MM-DD HH24:MI') EXAMRES15, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES16, 'YYYY-MM-DD HH24:MI') EXAMRES16 " + ComNum.VBLF;
            SQL += " FROM ADMIN.BAS_PATIENT_POSCO " + ComNum.VBLF;
            SQL += " WHERE JDATE = TO_DATE('" + cpublic.strSysDate + "', 'YYYY-MM-DD') " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strJDate = Rs.Rows[i]["JDATE"].ToString().Trim();

                if (Rs.Rows[i]["PANO"].ToString().Trim() == "")
                {
                    strPano = "00000000";
                }
                else
                {
                    strPano = Rs.Rows[i]["PANO"].ToString().Trim();
                }

                strName = Rs.Rows[i]["SNAME"].ToString().Trim();
                strTel = Rs.Rows[i]["HPHONE"].ToString().Trim();

                strEXAMRES_DATE[0] = Rs.Rows[i]["EXAMRES1"].ToString().Trim();   //복부초음파
                strEXAMRES_DATE[1] = Rs.Rows[i]["EXAMRES2"].ToString().Trim();   //위내시경
                strEXAMRES_DATE[2] = Rs.Rows[i]["EXAMRES3"].ToString().Trim();   //위내시경(수면)
                strEXAMRES_DATE[3] = Rs.Rows[i]["EXAMRES4"].ToString().Trim();   //대장내시경
                strEXAMRES_DATE[4] = Rs.Rows[i]["EXAMRES6"].ToString().Trim();   //대장내시경(수면)
                strEXAMRES_DATE[5] = Rs.Rows[i]["EXAMRES7"].ToString().Trim();   //흉부 CT
                strEXAMRES_DATE[6] = Rs.Rows[i]["EXAMRES8"].ToString().Trim();   //여성자궁검사
                strEXAMRES_DATE[7] = Rs.Rows[i]["EXAMRES9"].ToString().Trim();   //뇌 CT
                strEXAMRES_DATE[8] = Rs.Rows[i]["EXAMRES10"].ToString().Trim();   //경추 CT
                strEXAMRES_DATE[9] = Rs.Rows[i]["EXAMRES11"].ToString().Trim();   //요추 CT
                strEXAMRES_DATE[10] = Rs.Rows[i]["EXAMRES12"].ToString().Trim();   //심장 CT
                strEXAMRES_DATE[11] = Rs.Rows[i]["EXAMRES13"].ToString().Trim();   //심장초음파
                strEXAMRES_DATE[12] = Rs.Rows[i]["EXAMRES14"].ToString().Trim();   //경동맥초음파
                strEXAMRES_DATE[13] = Rs.Rows[i]["EXAMRES15"].ToString().Trim();   //뇌혈류초음파
                strEXAMRES_DATE[14] = Rs.Rows[i]["EXAMRES16"].ToString().Trim();   //여성유방검진

                strMin = strEXAMRES_DATE[0];
                if (strMin == "") { strMin = strMax; }

                for (k = 0; k < 15; k++)
                {
                    if (strEXAMRES_DATE[k] != "")
                    {
                        if (strMin.CompareTo(strEXAMRES_DATE[k]) > 0)
                        {
                            strMin = strEXAMRES_DATE[k];
                        }
                    }
                }

                strMM = VB.Mid(strMin, 6, 2) + "월";
                strDD = VB.Mid(strMin, 9, 2) + "일";
                strHH24 = VB.Mid(strMin, 12, 2);

                //이미자료를 전송했는지 점검 
                SQL = " SELECT JOBDATE, PANO, SNAME, HPHONE, GUBUN " + ComNum.VBLF;
                SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JOBDATE BETWEEN TO_DATE('" + cpublic.strSysDate + " 00:00', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND TO_DATE('" + cpublic.strSysDate + " 23:59', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND PANO = '" + strPano + "' " + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strTel + "' " + ComNum.VBLF;
                SQL += " AND GUBUN = '" + strGubun + "' " + ComNum.VBLF;
                SQL += " AND SNAME = '" + strName + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                nREAD2 = rs2.Rows.Count;

                if (nREAD2 == 0 && strName != "" && strTel != "")
                {
                    strMsg = strName + "님 포항성모병원";
                    strMsg = strMsg + strMM + strDD;
                    strMsg = strMsg + " 검사 예약이 완료되었습니다.";

                    SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG, PSMHSEND) VALUES( ";
                    SQL += " TO_DATE('" + strSendTime + "', 'YYYY-MM-DD HH24:MI'), ";
                    SQL += " '" + strPano + "', ";
                    SQL += " '" + strName + "', ";
                    SQL += " '" + strTel + "', ";
                    SQL += " '" + strGubun + "', ";
                    SQL += " '" + strRettel + "', ";
                    SQL += " '" + strMsg + "','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                rs2.Dispose();
                rs2 = null;
            }
            Rs.Dispose();
            Rs = null;
        }

        void OPD_CANCER_SERVICE_Receive_SMS_Send()
        {
            DataTable Rs = null;
            DataTable rs2 = null;
            DataTable rs3 = null;

            string strGubun = "";
            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            int nREAD4 = 0;
            string strPano = "";
            string strName = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strSendTime = "";
            string strRegdate = "";
            string strDeptCode = "";


            strGubun = "36";
            strSendTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            SQL = " SELECT A.REGDATE AREGDATE, A.PANO APANO,B.SNAME BSNAME, B.HPHONE BHPHONE, A.DEPT_CODE ADEPT_CODE " + ComNum.VBLF;
            SQL += " FROM ADMIN.ETC_CANCER_SERVICE A, ADMIN.BAS_PATIENT B " + ComNum.VBLF;
            SQL += " WHERE A.PANO = B.PANO(+) " + ComNum.VBLF;
            SQL += " AND A.DELDATE IS NULL " + ComNum.VBLF;
            SQL += " AND B.GBSMS = 'Y' " + ComNum.VBLF;
            SQL += " AND A.REGDATE = TO_DATE('" + cpublic.strSysDate + "', 'YYYY-MM-DD') " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strRegdate = Rs.Rows[i]["AREGDATE"].ToString().Trim();
                strDeptCode = Rs.Rows[i]["ADEPT_CODE"].ToString().Trim();

                if (Rs.Rows[i]["APANO"].ToString().Trim() == "")
                {
                    strPano = "00000000";
                }
                else
                {
                    strPano = Rs.Rows[i]["APANO"].ToString().Trim();
                }

                strName = Rs.Rows[i]["BSNAME"].ToString().Trim();
                strTel = Rs.Rows[i]["BHPHONE"].ToString().Trim();

                //진료과별 회신번호 SET
                SQL = " SELECT DRCODE,DRDEPT1,DRNAME,TELNO,ROWID " + ComNum.VBLF;
                SQL += " From ADMIN.BAS_DOCTOR " + ComNum.VBLF;
                SQL += " WHERE TOUR = 'N' " + ComNum.VBLF;
                SQL += " AND TELNO IS NOT NULL " + ComNum.VBLF;
                SQL += " AND DRDEPT1 = '" + strDeptCode + "' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs3, SQL, clsDB.DbCon);

                nREAD4 = rs3.Rows.Count;

                if (nREAD > 0)
                {
                    strRettel = rs3.Rows[0]["TELNO"].ToString().Trim();
                    if (VB.Left(strRettel, 3) != "054")
                    {
                        strRettel = "054" + strRettel;
                    }
                    if (strRettel == "")
                    {
                        strRettel = "0542720151";
                    }
                }

                rs3.Dispose();
                rs3 = null;

                //이미 자료를 전송했는지 점검 
                SQL = " SELECT JOBDATE, PANO, SNAME, HPHONE, GUBUN, DEPTCODE " + ComNum.VBLF;
                SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JOBDATE BETWEEN TO_DATE('" + cpublic.strSysDate + " 00:00', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND TO_DATE('" + cpublic.strSysDate + " 23:59', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND PANO = '" + strPano + "' " + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strTel + "' " + ComNum.VBLF;
                SQL += " AND GUBUN = '" + strGubun + "' " + ComNum.VBLF;
                SQL += " AND SNAME = '" + strName + "' " + ComNum.VBLF;
                SQL += " AND DEPTCODE = '" + strDeptCode + "' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                nREAD2 = rs2.Rows.Count;

                if (nREAD2 == 0 && strName != "" && strTel != "")
                {
                    strMsg = "정기검진 달입니다. 내원하셔서 정기검진 받으시기 바랍니다. 포항성모병원";

                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, DEPTCODE, RETTEL, SENDMSG, PSMHSEND) VALUES( ";
                    SQL += " TO_DATE('" + strSendTime + "', 'YYYY-MM-DD HH24:MI'),";
                    SQL += " '" + strPano + "', ";
                    SQL += " '" + strName + "', ";
                    SQL += " '" + strTel + "', ";
                    SQL += " '" + strGubun + "', ";
                    SQL += " '" + strDeptCode + "', ";
                    SQL += " '" + strRettel + "', ";
                    SQL += " '" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }

                rs2.Dispose();
                rs2 = null;
            }

            Rs.Dispose();
            Rs = null;
        }

        void POSCO_7Day_Colonoscopy_Receive_SMS_Send()
        {
            DataTable Rs = null;
            DataTable rs2 = null;

            string strGubun = "";
            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;

            string strJDate = "";
            string strPano = "";
            string strMin = "";
            string strYDATE = "";
            string strMM = "";
            string strDD = "";
            string strHH24 = "";
            string strHH24MI = "";

            string strName = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strSendTime = "";

            strGubun = "5";
            strYDATE = CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, 7);
            strSendTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            SQL = " SELECT JDATE, PANO, SNAME, HPHONE, TO_CHAR(EXAMRES6, 'YYYY-MM-DD HH24:MI') EXAMRES6 " + ComNum.VBLF;
            SQL += " FROM ADMIN.BAS_PATIENT_POSCO " + ComNum.VBLF;
            SQL += " WHERE TRUNC(EXAMRES6) = TO_DATE('" + strYDATE + "', 'YYYY-MM-DD') " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strJDate = Rs.Rows[i]["JDATE"].ToString().Trim();

                if (Rs.Rows[i]["PANO"].ToString().Trim() == "")
                {
                    strPano = "00000000";
                }
                else
                {
                    strPano = Rs.Rows[i]["PANO"].ToString().Trim();
                }

                strName = Rs.Rows[i]["SNAME"].ToString().Trim();
                strTel = Rs.Rows[i]["HPHONE"].ToString().Trim();

                strMM = VB.Mid(strMin, 6, 2) + "월";
                strDD = VB.Mid(strMin, 9, 2) + "일";
                strHH24 = VB.Mid(strMin, 12, 2);
                strHH24MI = VB.Mid(strMin, 12, 5);

                //이미 자료를 전송했는지 점검
                SQL = " SELECT JOBDATE, PANO, SNAME, HPHONE, GUBUN " + ComNum.VBLF;
                SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JOBDATE BETWEEN TO_DATE('" + cpublic.strSysDate + " 00:00', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND TO_DATE('" + cpublic.strSysDate + " 23:59', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND PANO = '" + strPano + "'" + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strTel + "'" + ComNum.VBLF;
                SQL += " AND GUBUN = '" + strGubun + "'" + ComNum.VBLF;
                SQL += " AND SNAME = '" + strName + "'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                nREAD2 = rs2.Rows.Count;

                if (nREAD2 == 0 && strName != "" && strTel != "")
                {
                    strRettel = "0542608004";

                    strMsg = "대장약수령을 못하신분은 본관2층 내시경실 방문하여 주십시오(수령받으신분은 제외)";

                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG, PSMHSEND) VALUES( ";
                    SQL += " TO_DATE('" + strSendTime + "', 'YYYY-MM-DD HH24:MI'), ";
                    SQL += " '" + strPano + "', ";
                    SQL += " '" + strName + "', ";
                    SQL += " '" + strTel + "',";
                    SQL += " '" + strGubun + "',";
                    SQL += " '" + strRettel + "',";
                    SQL += " '" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }

                rs2.Dispose();
                rs2 = null;
            }

            Rs.Dispose();
            Rs = null;
        }

        void POSCO_3Day_Receive_SMS_Send()
        {
            DataTable Rs = null;
            DataTable rs2 = null;

            string strGubun = "";
            int i = 0;
            int k = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            string strJDate = "";                          //접수일자 
            string strPano = "";                           //등록번호
            string strSname = "";                          //성명
            string strHPHONE = "";                         //휴대전화
            string[] strEXAMRES_DATE = new string[15];     //포스코 검사 예약일's
            string[] strEXAMRES_NAME = new string[15];     //포스코 검사 이름's
            string strMinName = "";                        //가장 빠른 검사 이름
            string strMin = "";                            //가장 빠른 검사예약일
            string strYDate = "";                          //가장빠른 검사예약일 3일전-전송날짜형식(YYYY-MM-DD)
            string strMM = "";                             //전송날짜형식(월)
            string strDD = "";                             //전송날짜형식(일)
            string strHH24 = "";                           //전송날짜형식(시)
            string strHH24MI = "";                         //전송날짜형식(HH24:MI)
            int nExam = 0;                                 //검사갯수

            string strName = "";                           //받는사람 이름
            string strTel = "";                            //받는사람 번호 
            string strRettel = "";                         //보내는사람 번호 
            string strMsg = "";                            //메세지 내용
            string strSendTime = "";                       //전송시간

            strGubun = "5";
            strYDate = CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, 3);
            strSendTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            SQL = " SELECT JDATE, PANO, SNAME, HPHONE, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES1, 'YYYY-MM-DD HH24:MI') EXAMRES1, TO_CHAR(EXAMRES2, 'YYYY-MM-DD HH24:MI') EXAMRES2, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES3, 'YYYY-MM-DD HH24:MI') EXAMRES3, TO_CHAR(EXAMRES4, 'YYYY-MM-DD HH24:MI') EXAMRES4, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES6, 'YYYY-MM-DD HH24:MI') EXAMRES6, TO_CHAR(EXAMRES7, 'YYYY-MM-DD HH24:MI') EXAMRES7, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES8, 'YYYY-MM-DD HH24:MI') EXAMRES8, TO_CHAR(EXAMRES9, 'YYYY-MM-DD HH24:MI') EXAMRES9, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES10, 'YYYY-MM-DD HH24:MI') EXAMRES10, TO_CHAR(EXAMRES11, 'YYYY-MM-DD HH24:MI') EXAMRES11, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES12, 'YYYY-MM-DD HH24:MI') EXAMRES12, TO_CHAR(EXAMRES13, 'YYYY-MM-DD HH24:MI') EXAMRES13, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES14, 'YYYY-MM-DD HH24:MI') EXAMRES14, TO_CHAR(EXAMRES15, 'YYYY-MM-DD HH24:MI') EXAMRES15, " + ComNum.VBLF;
            SQL += " TO_CHAR(EXAMRES16, 'YYYY-MM-DD HH24:MI') EXAMRES16 " + ComNum.VBLF;
            SQL += " FROM ADMIN.BAS_PATIENT_POSCO " + ComNum.VBLF;
            SQL += " WHERE TRUNC(EXAMRES1) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES2) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES3) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES4) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES6) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES7) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES8) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES9) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES10) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES11) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES12) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES13) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES14) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES15) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;
            SQL += " OR TRUNC(EXAMRES16) = TO_DATE('" + strYDate + "', 'YYYY-MM-DD')" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strJDate = Rs.Rows[i]["JDate"].ToString().Trim();

                if (Rs.Rows[i]["PANO"].ToString().Trim() == "")
                {
                    strPano = "00000000";
                }
                else
                {
                    strPano = VB.Val(Rs.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");

                    strName = Rs.Rows[i]["SNAME"].ToString().Trim();
                    strSname = Rs.Rows[i]["SNAME"].ToString().Trim();

                    strHPHONE = Rs.Rows[i]["HPHONE"].ToString().Trim();
                    strTel = Rs.Rows[i]["HPHONE"].ToString().Trim();

                    strEXAMRES_DATE[0] = Rs.Rows[i]["EXAMRES1"].ToString().Trim();   //복부초음파
                    strEXAMRES_DATE[1] = Rs.Rows[i]["EXAMRES2"].ToString().Trim();   //위내시경
                    strEXAMRES_DATE[2] = Rs.Rows[i]["EXAMRES3"].ToString().Trim();   //위내시경(수면)
                    strEXAMRES_DATE[3] = Rs.Rows[i]["EXAMRES4"].ToString().Trim();   //대장내시경
                    strEXAMRES_DATE[4] = Rs.Rows[i]["EXAMRES6"].ToString().Trim();   //대장내시경(수면)
                    strEXAMRES_DATE[5] = Rs.Rows[i]["EXAMRES7"].ToString().Trim();   //흉부 CT
                    strEXAMRES_DATE[6] = Rs.Rows[i]["EXAMRES8"].ToString().Trim();   //여성자궁검사 2012-08-27 기존검사명))위장조영촬영
                    strEXAMRES_DATE[7] = Rs.Rows[i]["EXAMRES9"].ToString().Trim();   //뇌 CT
                    strEXAMRES_DATE[8] = Rs.Rows[i]["EXAMRES10"].ToString().Trim();  //경추 CT
                    strEXAMRES_DATE[9] = Rs.Rows[i]["EXAMRES11"].ToString().Trim();  //요추 CT
                    strEXAMRES_DATE[10] = Rs.Rows[i]["EXAMRES12"].ToString().Trim(); //심장 CT
                    strEXAMRES_DATE[11] = Rs.Rows[i]["EXAMRES13"].ToString().Trim(); //심장초음파
                    strEXAMRES_DATE[12] = Rs.Rows[i]["EXAMRES14"].ToString().Trim(); //경동맥초음파
                    strEXAMRES_DATE[13] = Rs.Rows[i]["EXAMRES15"].ToString().Trim(); //뇌혈류초음파
                    strEXAMRES_DATE[14] = Rs.Rows[i]["EXAMRES16"].ToString().Trim(); //여성유방검진

                    strEXAMRES_NAME[0] = "복부초음파";
                    strEXAMRES_NAME[1] = "위내시경";
                    strEXAMRES_NAME[2] = "위내시경";
                    strEXAMRES_NAME[3] = "대장내시경";
                    strEXAMRES_NAME[4] = "대장내시경";
                    strEXAMRES_NAME[5] = "흉부CT";
                    strEXAMRES_NAME[6] = "여성자궁검사";
                    strEXAMRES_NAME[7] = "뇌CT";
                    strEXAMRES_NAME[8] = "경추CT";
                    strEXAMRES_NAME[9] = "요추CT";
                    strEXAMRES_NAME[10] = "심장CT";
                    strEXAMRES_NAME[11] = "심장초음파";
                    strEXAMRES_NAME[12] = "경동맥초음파";
                    strEXAMRES_NAME[13] = "뇌혈류초음파";
                    strEXAMRES_NAME[14] = "여성유방검진";

                    //건수초기화
                    nExam = 0;
                    for (k = 0; k < 14; k++)
                    {
                        if (strEXAMRES_DATE[k] != "")
                        {
                            nExam = nExam + 1;
                        }
                    }

                    //실제 외 몇건으로 쓰이므르 1빼기 ex)초음파외 2건
                    nExam = nExam - 1;

                    //가장 빠른 검사 예약일&검사명
                    strMin = strEXAMRES_DATE[0];
                    strMinName = strEXAMRES_NAME[0];

                    if (strMin == "") { strMin = strYDate + " 23:00"; }

                    for (k = 1; k < 14; k++)
                    {
                        if (strEXAMRES_DATE[k] != "")
                        {
                            if (strMin.CompareTo(strEXAMRES_DATE[k]) > 0)
                            {
                                strMin = strEXAMRES_DATE[k];
                                strMinName = strEXAMRES_NAME[k];
                            }
                        }
                    }

                    strMM = VB.Mid(strMin, 6, 2) + "월";
                    strDD = VB.Mid(strMin, 9, 2) + "일";
                    strHH24 = VB.Mid(strMin, 12, 2);
                    strHH24MI = VB.Mid(strMin, 12, 5);

                    //이미 자료를 전송했는지 점검
                    SQL = " SELECT JOBDATE, PANO, SNAME, HPHONE, GUBUN " + ComNum.VBLF;
                    SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                    SQL += " WHERE JOBDATE BETWEEN TO_DATE('" + cpublic.strSysDate + " 00:00', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL += " AND TO_DATE('" + cpublic.strSysDate + " 23:59', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL += " AND PANO = '" + strPano + "' " + ComNum.VBLF;
                    SQL += " AND HPHONE = '" + strTel + "' " + ComNum.VBLF;
                    SQL += " AND GUBUN = '" + strGubun + "' " + ComNum.VBLF;
                    SQL += " AND SNAME = '" + strName + "' " + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                    nREAD2 = rs2.Rows.Count;

                    if (nREAD2 == 0 && strName != "" && strTel != "")
                    {
                        strRettel = "0542608004";

                        strMsg = strName + "님 ";
                        strMsg = strMsg + strMM + strDD + strHH24MI + "분에";

                        if (nExam > 0)
                        {
                            strMsg = strMsg + strMinName + "외" + nExam + "건 ";
                        }
                        else
                        {
                            strMsg = strMsg + strMinName + " ";
                        }

                        strMsg = strMsg + " 검사예약되었습니다. 포항성모병원";

                        SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG, PSMHSEND) VALUES( " + ComNum.VBLF;
                        SQL += " TO_DATE('" + strSendTime + "', 'YYYY-MM-DD HH24:MI'), " + ComNum.VBLF;
                        SQL += " '" + strPano + "', " + ComNum.VBLF;
                        SQL += " '" + strName + "', " + ComNum.VBLF;
                        SQL += " '" + strTel + "', " + ComNum.VBLF;
                        SQL += " '" + strGubun + "', " + ComNum.VBLF;
                        SQL += " '" + strRettel + "', " + ComNum.VBLF;
                        SQL += " '" + strMsg + "','Y')" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    rs2.Dispose();
                    rs2 = null;
                }
            }

            Rs.Dispose();
            Rs = null;


        }

        void NBST_SMS_Send()
        {
            int i = 0;
            int nREAD = 0;
            string strPano = "";
            string strTime = "";
            string strName = "";
            string strTel = "";
            string strROWID = "";
            string strRettel = "";
            string strMsg = "";

            DataTable AdoRes = null;
            DataTable rs1 = null;


            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT PANO , SNAME,  EXAMRESULT, ROWID " + ComNum.VBLF;
            SQL += " FROM ADMIN.ETC_ARS " + ComNum.VBLF;
            SQL += " WHERE ENTDATE  >= TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, 01) + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND CLASSCODE ='2' " + ComNum.VBLF; //NBST 검사
            SQL += " AND VIEWCHK NOT IN 'Y' " + ComNum.VBLF;
            SQL += " AND EXAMRESULT ='N' " + ComNum.VBLF;
            SQL += " ORDER BY PANO " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strPano = VB.Val(AdoRes.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                strROWID = AdoRes.Rows[i]["ROWID"].ToString().Trim();

                strTime = "";
                strName = "";
                strTel = "";

                strName = AdoRes.Rows[i]["SNAME"].ToString().Trim();

                SQL = " SELECT GBSMS, HPHONE " + ComNum.VBLF;
                SQL += " FROM ADMIN.BAS_PATIENT " + ComNum.VBLF;
                SQL += " WHERE PANO ='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND GBSMS ='Y' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    strTel = rs1.Rows[0]["HPhone"].ToString().Trim();
                }
                else
                {
                    strTel = "";
                }

                rs1.Dispose();
                rs1 = null;

                //SMS 자료에 INSERT
                if (strName != "" && strTel != "")
                {
                    strRettel = "0542608244";

                    strMsg = "★포항성모병원★";
                    strMsg = strMsg + AdoRes.Rows[i]["SNAME"].ToString().Trim() + "님";
                    strMsg = strMsg + " 정신박약검사결과 ";

                    if (AdoRes.Rows[i]["EXAMRESULT"].ToString().Trim() == "N")
                    {
                        strMsg = strMsg + " 아무 이상이 없습니다";
                    }
                    else
                    {
                        strMsg = strMsg + "를 진료과에 상담하시기 바랍니다.";
                    }

                    //자료를 DB에 INSERT 
                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg,PSMHSEND) VALUES (SYSDATE,'";
                    SQL += strPano + "','" + strName + "','" + strTel + "','4','','',";
                    SQL += "TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    SQL = " UPDATE ETC_ARS SET VIEWCHK ='Y' WHERE ROWID ='" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void ECHO_SMS_Send()
        {
            int i = 0;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPano = "";
            string strName = "";
            string strTime = "";
            string strOrderCode = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strMinRTime = "";

            int n = 0;

            DataTable AdoRes = null;
            DataTable rs1 = null;

            read_sysdate();

            strFDate = CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, 1); //내일
            strTDate = CF.DATE_ADD(clsDB.DbCon, strFDate, 1); //모레

            //전송 희망시각 SET
            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT a.Ptno, a.SName, a.DeptCode, a.GuBun,a.ordercode   , TO_CHAR(C.RDate,'YYYY-MM-DD HH24:Mi') RDate, b.HPhone " + ComNum.VBLF;
            SQL += " FROM ADMIN.ETC_JUPMST a, ADMIN.BAS_PATIENT B, ADMIN.ETC_ECHO_RESV C " + ComNum.VBLF;
            SQL += " WHERE C.RDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND C.RDate <  TO_DATE('" + strTDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND A.GUBUN IN ('2','3','9','10','11','16','22') " + ComNum.VBLF;
            SQL += " AND A.PTNO = B.PANO " + ComNum.VBLF;
            SQL += " AND A.PTNO = C.PANO " + ComNum.VBLF;
            SQL += " AND A.BDATE = C.BDATE " + ComNum.VBLF;
            SQL += " AND A.ORDERNO = C.ORDERNO " + ComNum.VBLF;
            SQL += " AND A.DEPTCODE NOT IN ('TO','HR') " + ComNum.VBLF;
            SQL += " AND a.GbJob IN ( '1','2' ) " + ComNum.VBLF;
            SQL += " AND a.OrderCode NOT IN ( 'FZ736' ) " + ComNum.VBLF;
            SQL += " AND (B.GBSMS <> 'X' OR B.GBSMS IS NULL) " + ComNum.VBLF;
            SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " ORDER BY a.ptno, a.ordercode" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strPano = VB.Val(AdoRes.Rows[i]["PTNO"].ToString().Trim()).ToString("00000000");
                strTime = AdoRes.Rows[i]["RDate"].ToString().Trim();
                strOrderCode = AdoRes.Rows[i]["OrderCode"].ToString().Trim();
                strName = "";
                strTel = "";

                strName = AdoRes.Rows[i]["Sname"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HPhone"].ToString().Trim();

                //이미 자료를 넘겼는지 확인함
                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND Pano='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND Gubun='70'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";
                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                //이미 전송한 예약시간이 적으면 다시 전송안함
                if (strMinRTime != "" && strMinRTime.CompareTo(strTime) <= 0)
                {
                    strTel = "";
                }

                //재원중인 환자는 제외 
                SQL = " SELECT Pano FROM ADMIN.IPD_NEW_MASTER " + ComNum.VBLF;
                SQL += " WHERE Pano='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    strTel = "";
                }

                rs1.Dispose();
                rs1 = null;

                //SMS 자료에 INSERT
                if (strName != "" && strTel != "")
                {
                    switch (AdoRes.Rows[i]["GUBUN"].ToString().Trim())
                    {
                        case "2": strMsg = "[포항성모병원 2층심전도실]";
                            strRettel = "0542608157";
                            break;
                        case "16": strMsg = "[포항성모병원 2층심전도실]";
                            strRettel = "0542608220";
                            break;
                        default: strMsg = "[포항성모병원 심장초음파실]";
                            strRettel = "0542608231";
                            break;
                    }

                    strMsg = strMsg + AdoRes.Rows[i]["SNAME"].ToString().Trim() + "님 ";
                    strMsg = strMsg + VB.Format(VB.Val(VB.Mid(strTime, 6, 2)), "#0") + "월 ";
                    strMsg = strMsg + VB.Format(VB.Val(VB.Mid(strTime, 9, 2)), "#0") + "일 ";

                    n = Convert.ToInt16(VB.Val(VB.Mid(strTime, 12, 2)));

                    if (n > 12)
                    {
                        n = n - 12;
                        strMsg = strMsg + "오후" + VB.Format(n, "#0") + "시 ";
                    }
                    else
                    {
                        strMsg = strMsg + "오전" + VB.Format(n, "#0") + "시 ";
                    }

                    if (Convert.ToInt16(VB.Val(VB.Right(strTime, 2))) != 0)
                    {
                        strMsg = strMsg + VB.Format(VB.Val(VB.Right(strTime, 2)), "#0") + "분";
                    }
                    else
                    {
                        strMsg = strMsg + " ";
                    }

                    switch (AdoRes.Rows[i]["GUBUN"].ToString().Trim())
                    {
                        case "3":
                            if (strOrderCode == "US-TEE" || strOrderCode == "US-TEE4D" || strOrderCode == "EB611" || strOrderCode == "EB610")
                            {
                                strMsg = strMsg + "경싱도 심장초음파 검사 되어있습니다. *금식여부: 8시간 금식 *가능하면 보호자 동반";
                            }
                            else if (strOrderCode == "US-CADU1" || strOrderCode == "EB482A-1")
                            {
                                strMsg = strMsg + "경동맥 초음파 검사 예약되어 있습니다.";
                            }
                            else
                            {
                                strMsg = strMsg + "삼장초음파 검사 예약되어 있습니다.";
                            }
                            break;
                        case "11": strMsg = strMsg + "운동부하 심전도 검사예약 되어 있습니다. *간편한 복장/양말 착용 ";
                            break;
                        case "10": strMsg = strMsg + "24시간 활동 심전도 검사예약 되어 있습니다. *간편한 복장 ";
                            break;
                        case "9": strMsg = strMsg + "24시간 활동 혈압 검사예약 되어 있습니다. *간편한 복장 ";
                            break;
                        case "22": strMsg = strMsg + "기립 경 검사예약 되어 있습니다. *금식여부: 8시간 금식 ";
                            break;
                        case "2": strMsg = strMsg + "심전도실 뇌파 검사예약 되어있습니다. ";
                            break;
                        case "16": strMsg = strMsg + "기관지유발천식 검사 예약 되어있습니다. ";
                            break;
                        default: strMsg = strMsg + "심초음파실 검사예약 되어있습니다. ";
                            break;
                    }

                    //자료를 DB에 INSERT
                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg,PSMHSEND) VALUES (SYSDATE,'";
                    SQL += strPano + "','" + strName + "','" + strTel + "','70','','',";
                    SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Date_Change_심평원_연령금기_고지사항()
        {
            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strGubun = "";
            string strADate = "";
            string strBDate = "";
            string strKorname = "";
            string strTel = "";
            string strHiraCode = "";
            string strHiraGubun = "";

            DataTable Rs = null;
            DataTable rs2 = null;
            DataTable rs3 = null;
            DataTable AdoRes = null;

            //HIRA 테이블 일자가 BCODE 일자보다 크면 업데이트(BCODE테이블) 시켜주고 SMS 전송시킨
            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strGubun = "42";
            strRettel = "0542608051";
            strHiraCode = "연령금기";
            strHiraGubun = "DUR_고시일자";
            strMsg = "심평원 연령금기 고시 사항이 변동되었습니다.";

            SQL = " SELECT MAX(ANNCE_DT) ADATE FROM ADMIN.HIRA_TBJBD44 " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            strADate = Rs.Rows[0]["ADATE"].ToString().Trim();
            strADate = VB.Left(strADate, 4) + "-" + VB.Mid(strADate, 5, 2) + "-" + VB.Right(strADate, 2);

            Rs.Dispose();
            Rs = null;

            SQL = " SELECT CODE, NAME " + ComNum.VBLF;
            SQL += " FROM ADMIN.BAS_BCODE " + ComNum.VBLF;
            SQL += " WHERE GUBUN = 'DUR_고시일자' AND CODE = '" + strHiraCode + "' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

            strBDate = rs2.Rows[0]["NAME"].ToString().Trim();

            rs2.Dispose();
            rs2 = null;

            if (strADate.CompareTo(strBDate) > 0)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                SQL = " SELECT SABUN, KORNAME, HTEL, MSTEL " + ComNum.VBLF;
                SQL += " FROM ADMIN.INSA_MST " + ComNum.VBLF;
                SQL += " WHERE SABUN IN('02186', '25180', '29439') " + ComNum.VBLF; //약제과수녀님

                SqlErr = clsDB.GetDataTableEx(ref rs3, SQL, clsDB.DbCon);

                nREAD = rs3.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    strKorname = rs3.Rows[i]["KORNAME"].ToString().Trim();
                    strTel = rs3.Rows[i]["HTEL"].ToString().Trim();

                    //이미전송했는지 확인
                    SQL = " SELECT JOBDATE, GUBUN, HPHONE " + ComNum.VBLF;
                    SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                    SQL += " WHERE JOBDATE BETWEEN TO_DATE('" + cpublic.strSysDate + " 00:00', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL += " AND TO_DATE('" + cpublic.strSysDate + " 23:59', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL += " AND GUBUN = '" + strGubun + "' " + ComNum.VBLF;
                    SQL += " AND HPHONE = '" + strTel + "'" + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

                    nREAD2 = AdoRes.Rows.Count;

                    if (nREAD2 == 0 && strTel != "")
                    {
                        SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG,PSMHSEND) VALUES( ";
                        SQL += " TO_DATE('" + strJobTime + "', 'YYYY-MM-DD HH24:MI'), '" + strKorname + "', ";
                        SQL += " '" + strTel + "', '" + strGubun + "', '" + strRettel + "', '" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    AdoRes.Dispose();
                    AdoRes = null;

                }

                //BCODE 테이블에 새로고시된(HIRA 테이블 일자 업데이트 해줌)
                SQL = " UPDATE ADMIN.BAS_BCODE SET ";
                SQL += " NAME = '" + strADate + "' ";
                SQL += " WHERE GUBUN = '" + strHiraGubun + "' ";
                SQL += " AND CODE = '" + strHiraCode + "' ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                rs3.Dispose();
                rs3 = null;

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void Date_Change_심평원_병용금기_고지사항()
        {
            int i = 0;
            int nREAD = 0;
            int nREAD2 = 0;
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strGubun = "";
            string strADate = "";
            string strBDate = "";
            string strKorname = "";
            string strTel = "";
            string strHiraCode = "";
            string strHiraGubun = "";

            DataTable Rs = null;
            DataTable rs2 = null;
            DataTable rs3 = null;
            DataTable AdoRes = null;

            //HIRA 테이블 일자가 BCODE 일자보다 크면 업데이트(BCODE테이블) 시켜주고 SMS전송 시킴
            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;
            strGubun = "43";
            strRettel = "0542608051";
            strHiraCode = "병용금기";
            strHiraGubun = "DUR_고지일자";
            strMsg = "심평원 병용금기 고시 사항이 변동되었습니다.";

            SQL = " SELECT MAX(ANNCE_DT) ADATE FROM ADMIN.HIRA_TBJBD43 " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            strADate = Rs.Rows[0]["ADATE"].ToString().Trim();
            strADate = VB.Left(strADate, 4) + "-" + VB.Mid(strADate, 5, 2) + "-" + VB.Right(strADate, 2);

            Rs.Dispose();
            Rs = null;

            SQL = " SELECT CODE, NAME " + ComNum.VBLF;
            SQL += " FROM ADMIN.BAS_BCODE " + ComNum.VBLF;
            SQL += " WHERE GUBUN = 'DUR_고시일자' AND CODE = '" + strHiraCode + "' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

            strBDate = rs2.Rows[0]["NAME"].ToString().Trim();

            rs2.Dispose();
            rs2 = null;

            if (strADate.CompareTo(strBDate) > 0)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                SQL = " SELECT SABUN, KORNAME, HTEL, MSTEL " + ComNum.VBLF;
                SQL += " FROM ADMIN.INSA_MST " + ComNum.VBLF;
                SQL += " WHERE SABUN IN('02186', '25180', '29439') " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs3, SQL, clsDB.DbCon);

                nREAD = rs3.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    strKorname = rs3.Rows[i]["KORNAME"].ToString().Trim();
                    strTel = rs3.Rows[i]["HTEL"].ToString().Trim();

                    //이미 전송했는지 확인 
                    SQL = " SELECT JOBDATE, GUBUN, HPHONE " + ComNum.VBLF;
                    SQL += " FROM ADMIN.ETC_SMS " + ComNum.VBLF;
                    SQL += " WHERE JOBDATE BETWEEN TO_DATE('" + cpublic.strSysDate + " 00:00', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL += " AND TO_DATE('" + cpublic.strSysDate + " 23:59', 'YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL += " AND GUBUN = '" + strGubun + "' " + ComNum.VBLF;
                    SQL += " AND HPHONE = '" + strTel + "' " + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

                    nREAD2 = AdoRes.Rows.Count;

                    AdoRes.Dispose();
                    AdoRes = null;

                    if (nREAD2 == 0 && strTel != "")
                    {
                        SQL = " INSERT INTO ADMIN.ETC_SMS(JOBDATE, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG,PSMHSEND) VALUES( ";
                        SQL += " TO_DATE('" + strJobTime + "', 'YYYY-MM-DD HH24:MI'), '" + strKorname + "', ";
                        SQL += " '" + strTel + "', '" + strGubun + "', '" + strRettel + "', '" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                }

                //BCODE 테이블에 새로고시된 (HIRA 테이블 일자 업데이트 해줌)
                SQL = " UPDATE ADMIN.BAS_BCODE SET ";
                SQL += " NAME = '" + strADate + "' ";
                SQL += " WHERE GUBUN = '" + strHiraGubun + "' ";
                SQL += " AND CODE = '" + strHiraCode + "' ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                rs3.Dispose();
                rs3 = null;

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void HIC_CANCER_3Day_SMS_Send()
        {
            int i = 0;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPano = "";
            string strName = "";
            string strTime = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strMinRTime = "";

            string strTime2 = "";
            string strRTIME2 = "";

            DataTable AdoRes = null;
            DataTable rs1 = null;

            read_sysdate();
            strFDate = CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, 3);
            strTDate = CF.DATE_ADD(clsDB.DbCon, strFDate, 1);

            //전송 희망시각 SET
            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT Pano,SName,HPhone, TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') RTIME, " + ComNum.VBLF;
            SQL += " DECODE(TO_CHAR(RTIME, 'AM'),'AM', '오전', '오전', '오전', '오후') || ' ' || TO_CHAR(RTIME, 'HH:MI') RTIME2, TO_CHAR(RTIME,'YYYY-MM-DD HH24:MI') YDate " + ComNum.VBLF;
            SQL += " FROM HIC_CANCER_RESV2 " + ComNum.VBLF;
            SQL += " WHERE RTime BETWEEN TO_DATE('" + strFDate + " 00:01','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
            SQL += " AND TO_DATE('" + strFDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
            SQL += " AND SMSOK = 'Y' " + ComNum.VBLF;
            SQL += " AND SUBSTR(HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " AND RTIME > TO_DATE('2000-01-01','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " ORDER BY RTime,SName,HPhone " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                if (AdoRes.Rows[i]["PANO"].ToString().Trim() == "")
                {
                    strPano = "00000000";
                }
                else
                {
                    strPano = VB.Val(AdoRes.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                }

                strTime = AdoRes.Rows[i]["YDate"].ToString().Trim();
                strTime2 = AdoRes.Rows[i]["RTIME"].ToString().Trim();
                strName = AdoRes.Rows[i]["SNAME"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HPHONE"].ToString().Trim();

                strRTIME2 = AdoRes.Rows[i]["RTIME2"].ToString().Trim();

                //이미 자료를 넘겼는지 확인함

                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND SName='" + strName + "' " + ComNum.VBLF;
                SQL += " AND HPhone ='" + strTel + "' " + ComNum.VBLF;
                SQL += " AND Gubun='H' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";

                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                //이미 전송한 예약시간이 적으면 다시 전송안함
                if (strMinRTime != "" && strMinRTime.CompareTo(strTime) <= 0)
                {
                    strTel = "";
                }

                //SMS 자료에 INSERT
                if (strName != "" && strTel != "")
                {
                    strRettel = "0542608188";

                    strMsg = AdoRes.Rows[i]["SName"].ToString().Trim() + "님";
                    strMsg = strMsg + " 건강검진 ";
                    strMsg = strMsg + VB.Mid(strTime2, 6, 2) + "월";
                    strMsg = strMsg + VB.Mid(strTime2, 9, 2) + "일";
                    strMsg = strMsg + " " + strRTIME2 + " ";
                    strMsg = strMsg + " 로 예약되었습니다.포항성모병원 건강증진센터 1층";

                    //자료를 DB에 INSERT 

                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg, PSMHSEND) VALUES (TO_DATE('" + cpublic.strSysDate + " 10:00" + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strPano + "','" + strName + "','" + strTel + "','H','','',";
                    SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);

        }

        void HEA_TO_TX32_Reserved_SMS_Send()
        {
            int i = 0;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";

            string strPano = "";
            string strName = "";
            string strTime = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strMinRTime = "";

            DataTable AdoRes = null;
            DataTable rs1 = null;

            read_sysdate();
            strFDate = CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, 4);
            strTDate = CF.DATE_ADD(clsDB.DbCon, strFDate, 4);

            //전송 희망시각 SET
            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            //   '익일의 종검예약자를 SMS 자료로 Update
            SQL = " SELECT a.Ptno,a.SName,b.HPhone,b.Tel,TO_CHAR(a.SDate,'YYYY-MM-DD HH24:MI') YDate " + ComNum.VBLF;
            SQL += " FROM ADMIN.HEA_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HEA_RESULT c " + ComNum.VBLF;
            SQL += " WHERE a.Pano=b.Pano(+) " + ComNum.VBLF;
            SQL += " AND a.WRTNO=c.WRTNO(+) " + ComNum.VBLF;
            SQL += " AND c.EXCODE ='TX32' " + ComNum.VBLF;
            SQL += " AND a.SDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND a.SDate <=  TO_DATE('" + strFDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND a.GbSTS = '0' " + ComNum.VBLF;
            SQL += " AND b.GbSMS = 'Y' " + ComNum.VBLF;
            SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " AND TO_CHAR(a.sdate,'MM-DD')  <> '12-25' " + ComNum.VBLF;
            SQL += " ORDER BY SDate,SName,HPhone " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                if (AdoRes.Rows[i]["PTNO"].ToString().Trim() == "")
                {
                    strPano = "00000000";
                }
                else
                {
                    strPano = VB.Val(AdoRes.Rows[i]["PTNO"].ToString().Trim()).ToString("00000000");
                }

                strTime = AdoRes.Rows[i]["YDate"].ToString().Trim();
                strName = "";
                strTel = "";

                strName = AdoRes.Rows[i]["Sname"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HPhone"].ToString().Trim();

                //이미 자료를 넘겼는지 확인함
                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND SName='" + strName + "' " + ComNum.VBLF;
                SQL += " AND HPhone ='" + strTel + "' " + ComNum.VBLF;
                SQL += " AND Gubun='2' " + ComNum.VBLF; //종검예약

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";
                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                //이미 전송한 예약시간이 적으면 다시 전송 안함
                if (strMinRTime != "" && strMinRTime.CompareTo(strTime) <= 0)
                {
                    strTel = "";
                }

                //SMS 자료에 INSERT
                if (strName != "" && strTel != "")
                {
                    strRettel = "0542608188";
                    strMsg = "대장내시경검사 2~3일전부터씨있는과일,검정잡곡류,해조류등은 금식 포항성모병원종검";

                    //자료를 DB에 INSERT
                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg,PSMHSEND) VALUES (TO_DATE('" + cpublic.strSysDate + " 10:00" + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strPano + "','" + strName + "','" + strTel + "','2','','',";
                    SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void OPD_DRUG_SMS_Send()
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;
            int nREAD4 = 0;
            string strPano = "";
            string strName = "";
            string strTime = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strMinRTime = "";
            string strDeptCode = "";
            string strDRCODE = "";

            DataTable rs1 = null;
            DataTable AdoRes = null;
            DataTable Rs = null;

            read_sysdate();

            strJobTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT JOBDATE FROM ADMIN.ETC_SMS_OPDDRUG " + ComNum.VBLF;
            SQL += " WHERE JOBDATE = TRUNC(SYSDATE) " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

            if (rs1.Rows.Count == 0)
            {
                SQL = " INSERT INTO ADMIN.ETC_SMS_OPDDRUG( JOBDATE, BDATE, PANO, DEPTCODE,DRCODE, SNAME, HPHONE , NAL  ) ";
                SQL += " SELECT  TRUNC(SYSDATE), A.BDATE,  A.PANO, A.DEPTCODE, A.DRCODE, B.SNAME, B.HPHONE, MAX(A.NAL) ";
                SQL += " FROM ADMIN.OPD_SLIP A,  ADMIN.BAS_PATIENT B ";
                SQL += " WHERE A.BDATE =TRUNC(SYSDATE -15) ";
                SQL += " AND A.BUN IN ('11') ";
                SQL += " AND A.NAL >= 30 ";
                SQL += " AND A.NAL <  60 ";
                SQL += " AND a.Pano=b.Pano(+) ";
                SQL += " AND b.GbSMS = 'Y' ";
                SQL += " AND b.GbSMS_DRUG = '*' ";
                SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') ";
                SQL += " GROUP BY  A.BDATE, A.PANO, A.DEPTCODE, A.DRCODE,  B.SNAME, B.HPHONE ";
                SQL += " Having Sum(A.QTY * A.NAL) > 0 ";
                SQL += " Union All ";
                SQL += " SELECT  TRUNC(SYSDATE), A.BDATE,  A.PANO, A.DEPTCODE, A.DRCODE, B.SNAME, B.HPHONE, MAX(A.NAL) ";
                SQL += " FROM ADMIN.OPD_SLIP A,  ADMIN.BAS_PATIENT B ";
                SQL += " WHERE A.BDATE =TRUNC(SYSDATE -30) ";
                SQL += " AND A.BUN IN ('11') ";
                SQL += " AND A.NAL >= 60 ";
                SQL += " AND A.NAL <  90 ";
                SQL += " AND a.Pano=b.Pano(+) ";
                SQL += " AND b.GbSMS = 'Y' ";
                SQL += " AND b.GbSMS_DRUG = '*' ";
                SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') ";
                SQL += " GROUP BY A.BDATE, A.PANO, A.DEPTCODE, A.DRCODE, B.SNAME, B.HPHONE ";
                SQL += " Having Sum(A.QTY * A.NAL) > 0 ";
                SQL += " Union All ";
                SQL += " SELECT  TRUNC(SYSDATE), A.BDATE,  A.PANO, A.DEPTCODE, A.DRCODE, B.SNAME, B.HPHONE, MAX(A.NAL) ";
                SQL += " FROM ADMIN.OPD_SLIP A,  ADMIN.BAS_PATIENT B ";
                SQL += " WHERE A.BDATE =TRUNC(SYSDATE -45) ";
                SQL += " AND A.BUN IN ('11') ";
                SQL += " AND A.NAL >= 90 ";
                SQL += " AND a.Pano=b.Pano(+) ";
                SQL += " AND b.GbSMS = 'Y' ";
                SQL += " AND b.GbSMS_DRUG = '*' ";
                SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') ";
                SQL += " GROUP BY A.BDATE, A.PANO, A.DEPTCODE, A.DRCODE, B.SNAME, B.HPHONE ";
                SQL += " Having Sum(A.QTY * A.NAL) > 0 ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            rs1.Dispose();
            rs1 = null;

            SQL = " SELECT  BDATE,  PANO, DEPTCODE, DRCODE, SNAME, HPHONE, ROWID " + ComNum.VBLF;
            SQL += " FROM ADMIN.ETC_SMS_OPDDRUG " + ComNum.VBLF;
            SQL += " WHERE JOBDATE =TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND SMSSEND IS NULL " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strPano = VB.Val(AdoRes.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");

                strName = AdoRes.Rows[i]["Sname"].ToString().Trim();
                strDeptCode = AdoRes.Rows[i]["DeptCode"].ToString().Trim();
                strDRCODE = AdoRes.Rows[i]["DRCODE"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HPHONE"].ToString().Trim();

                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND SName='" + strName + "' " + ComNum.VBLF;
                SQL += " AND HPhone ='" + strTel + "' " + ComNum.VBLF;
                SQL += " AND Gubun='14' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";

                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                if (strMinRTime != "" && strMinRTime.CompareTo(strTime) <= 0)
                {
                    strTel = "";
                }

                if (strName != "" && strTel != "")
                {
                    strRettel = "";

                    //진료과별 회신번호 SET
                    SQL = " SELECT DRCODE,DRDEPT1,DRNAME,TELNO,ROWID " + ComNum.VBLF;
                    SQL += " From ADMIN.BAS_DOCTOR " + ComNum.VBLF;
                    SQL += " WHERE TOUR = 'N' " + ComNum.VBLF;
                    SQL += " AND TELNO IS NOT NULL " + ComNum.VBLF;
                    SQL += " AND DRDEPT1 = '" + strDeptCode + "' " + ComNum.VBLF;
                    SQL += " AND DRCODE = '" + strDRCODE + "' " + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                    nREAD4 = Rs.Rows.Count;
                    if (nREAD4 > 0)
                    {
                        strRettel = VB.Replace(Rs.Rows[0]["TELNO"].ToString().Trim(), "-", "");
                        if (VB.Left(strRettel, 3) != "054") { strRettel = "054" + strRettel; }
                        if (strRettel == "") { strRettel = "0542720151"; }
                    }

                    Rs.Dispose();
                    Rs = null;

                    strMsg = "♡포항성모병원♡";
                    strMsg = strMsg + "약은 잘 복용하시고";
                    strMsg = strMsg + "건강한 모습으로 다음내원일에 뵙겠습니다.";

                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg,PSMHSEND) VALUES (TO_DATE('" + cpublic.strSysDate + " 10:00" + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strPano + "','" + strName + "','" + strTel + "','14','','',";
                    SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),' ";
                    SQL += strRettel + "','','" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    SQL = " UPDATE ADMIN.ETC_SMS_OPDDRUG  SET SMSSEND ='Y' ";
                    SQL += " WHERE ROWID = '" + AdoRes.Rows[i]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Mibi_Chart_Send()
        {
            //매주 수요일 09:00에 발송함
            //2013-10-01 미비현황 발생시킨 의사에게 전송
            int i = 0;
            int nREAD = 0;
            string strLDate = "";
            string strDRCODE = "";
            string strSabun = "";
            string strHTEL = "";
            string strRettel = "";
            string strName = "";
            string strMsg = "";
            string strTime = "";
            string strOldDoct = "";
            int nCnt1 = 0;
            int nCnt2 = 0;

            DataTable AdoRes = null;
            DataTable rs2 = null;

            //요일을 점검함(수요일만 통보)
            switch (CF.READ_YOIL(clsDB.DbCon, cpublic.strSysDate))
            {
                case "수요일":
                    break;
                default:
                    return;
            }

            strLDate = VB.Left(cpublic.strSysDate, 4) + VB.Mid(cpublic.strSysDate, 6, 2) + VB.Right(cpublic.strSysDate, 2);
            strTime = cpublic.strSysDate + " " + "09:00";

            SQL = " SELECT A.DrCode, A.GUBUN, SUM(A.CNT1) CNT1, SUM(A.CNT2) CNT2 " + ComNum.VBLF;
            SQL += " FROM ( " + ComNum.VBLF;
            SQL += " SELECT DOCCODE DrCode, PATID,OUTDATE, DECODE(CHECKED, '1',1,0) CNT1, 1 CNT2 , 'G' GUBUN " + ComNum.VBLF;
            SQL += " FROM ADMIN.EMR_MIBIT " + ComNum.VBLF;
            SQL += " WHERE OUTDATE >='20050101' " + ComNum.VBLF;
            SQL += " AND OUTDATE <='" + strLDate + "' " + ComNum.VBLF;
            SQL += " AND CHECKED='1' " + ComNum.VBLF;
            SQL += " GROUP BY PATID, DOCCODE, OUTDATE, CHECKED, 'G' " + ComNum.VBLF;
            SQL += " UNION ALL " + ComNum.VBLF;
            SQL += " SELECT MEDDRCD DrCode, PTNO PATID, MEDENDDATE OUTDATE, DECODE(MIBICLS, '1', 1, 0) CNT1, 1 CNT2, 'T' GUBUN " + ComNum.VBLF;
            SQL += " FROM ADMIN.EMRMIBI " + ComNum.VBLF;
            SQL += " WHERE MIBIFNDATE Is Null " + ComNum.VBLF;
            SQL += " AND MIBICLS = 1 " + ComNum.VBLF;
            SQL += " AND MEDENDDATE <= '" + strLDate + "' " + ComNum.VBLF;
            SQL += " AND MEDENDDATE >= '20090801' " + ComNum.VBLF;
            SQL += " GROUP BY PTNO, MEDDRCD, MEDENDDATE, MIBICLS, 'T' " + ComNum.VBLF;
            SQL += " UNION ALL " + ComNum.VBLF;
            SQL += " SELECT MEDDRCD1 DrCode, PTNO PATID, MEDENDDATE OUTDATE, DECODE(MIBICLS, '1', 1, 0) CNT1, 1 CNT2, 'T' GUBUN " + ComNum.VBLF;
            SQL += " FROM ADMIN.EMRMIBI " + ComNum.VBLF;
            SQL += " WHERE MIBIFNDATE Is Null " + ComNum.VBLF;
            SQL += " AND MIBICLS = 1 " + ComNum.VBLF;
            SQL += " AND MEDENDDATE <= '" + strLDate + "' " + ComNum.VBLF;
            SQL += " AND MEDENDDATE >= '20090801' " + ComNum.VBLF;
            SQL += " AND MEDDRCD1>= ' ' " + ComNum.VBLF;
            SQL += " AND MEDDRCD1<>MEDDRCD " + ComNum.VBLF;
            SQL += " GROUP BY PTNO, MEDDRCD1, MEDENDDATE, MIBICLS, 'T') A " + ComNum.VBLF;
            SQL += " GROUP BY A.DrCode , A.GUBUN " + ComNum.VBLF;
            SQL += " ORDER BY A.DrCode , A.GUBUN " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;
            strOldDoct = "";
            nCnt1 = 0;
            nCnt2 = 0;

            for (i = 0; i < nREAD; i++)
            {
                strDRCODE = AdoRes.Rows[i]["DrCode"].ToString().Trim();
                if (strDRCODE != strOldDoct)
                {
                    if (strOldDoct != "" && (nCnt1 > 0 || nCnt2 > 0))
                    {
                        #region Mibi_SMS_Send();
                        strSabun = strOldDoct.Trim();
                        if (strSabun == "") { return; }

                        //인사에서 휴대폰 번호를 읽음
                        SQL = " SELECT HTel, MSTEL,KorName FROM ADMIN.INSA_MST " + ComNum.VBLF;
                        SQL += " WHERE Sabun='" + strSabun + "' " + ComNum.VBLF;
                        SQL += " AND (ToiDay IS NULL OR ToiDay > TRUNC(SYSDATE)) " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                        strHTEL = "";
                        strName = "";

                        if (rs2.Rows.Count > 0)
                        {
                            strHTEL = rs2.Rows[0]["MSTEL"].ToString().Trim();
                            strName = rs2.Rows[0]["KorName"].ToString().Trim();
                            if (strHTEL == "")
                            {
                                strHTEL = rs2.Rows[0]["HTEL"].ToString().Trim();
                            }
                        }

                        rs2.Dispose();
                        rs2 = null;

                        if (strHTEL != "")
                        {
                            strHTEL = VB.Replace(strHTEL, "-", "");

                            //이미자료를 넘겼는지 확인함.
                            SQL = " SELECT MIN(TO_CHAR(JobDate,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS  " + ComNum.VBLF;
                            SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                            SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                            SQL += " AND TRIM(HPhone) ='" + strHTEL + "' " + ComNum.VBLF;   
                            SQL += " AND Gubun='I'" + ComNum.VBLF; //미비차트전송

                            SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                            if (rs2.Rows.Count > 0)
                            {
                                if (rs2.Rows[0]["RTime"].ToString().Trim() != "")
                                {
                                    strHTEL = "";
                                }
                            }

                            rs2.Dispose();
                            rs2 = null;
                        }

                        if (strHTEL != "")
                        {
                            strHTEL = VB.Replace(strHTEL, "-", "");
                            strHTEL = VB.Replace(strHTEL, " ", "");

                            strRettel = "0542608041";

                            strMsg = "현재 " + strName + "님미비건수";
                            strMsg = strMsg + ComNum.VBLF + "종이챠트:" + nCnt1 + "건 ";
                            strMsg = strMsg + ComNum.VBLF + "EMR:" + nCnt2 + "건입니다 ";
                            strMsg = strMsg + ComNum.VBLF + " -의료정보팀- ";

                            clsDB.setBeginTran(clsDB.DbCon);

                            SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                            SQL += " RetTel,SendTime,SendMsg,PSMHSEND) VALUES (SYSDATE,'','',' ";
                            SQL += strHTEL + "','I','','',";
                            SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                            SQL += strRettel + "','','" + strMsg + "','Y')";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                        #endregion
                    }
                    strOldDoct = strDRCODE;
                    nCnt1 = 0;
                    nCnt2 = 0;
                }
                if (AdoRes.Rows[i]["GUBUN"].ToString().Trim() == "G")
                {
                    nCnt1 = nCnt1 + Convert.ToInt32(VB.Val(AdoRes.Rows[i]["CNT1"].ToString().Trim()));
                }
                else if (AdoRes.Rows[i]["GUBUN"].ToString().Trim() == "T")
                {
                    nCnt2 = nCnt2 + Convert.ToInt32(VB.Val(AdoRes.Rows[i]["CNT1"].ToString().Trim()));
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            if (strOldDoct != "" && (nCnt1 > 0 || nCnt2 > 0))
            {
                #region Mibi_SMS_Send();
                strSabun = strOldDoct.Trim();
                if (strSabun == "") { return; }

                //인사에서 휴대폰 번호를 읽음
                SQL = " SELECT HTel, MSTEL,KorName FROM ADMIN.INSA_MST " + ComNum.VBLF;
                SQL += " WHERE Sabun='" + strSabun + "' " + ComNum.VBLF;
                SQL += " AND (ToiDay IS NULL OR ToiDay > TRUNC(SYSDATE)) " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                strHTEL = "";
                strName = "";

                if (rs2.Rows.Count > 0)
                {
                    strHTEL = rs2.Rows[0]["MSTEL"].ToString().Trim();
                    strName = rs2.Rows[0]["KorName"].ToString().Trim();
                    if (strHTEL == "")
                    {
                        strHTEL = rs2.Rows[0]["HTEL"].ToString().Trim();
                    }
                }

                rs2.Dispose();
                rs2 = null;

                if (strHTEL != "")
                {
                    strHTEL = VB.Replace(strHTEL, "-", "");

                    //이미자료를 넘겼는지 확인함.
                    SQL = " SELECT MIN(TO_CHAR(JobDate,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS  " + ComNum.VBLF;
                    SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                    SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                    SQL += " AND HPhone='" + strHTEL + "' " + ComNum.VBLF;
                    SQL += " AND Gubun='I'" + ComNum.VBLF; //미비차트전송

                    SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                    if (rs2.Rows.Count > 0)
                    {
                        if (rs2.Rows[0]["RTime"].ToString().Trim() != "")
                        {
                            strHTEL = "";
                        }
                    }

                    rs2.Dispose();
                    rs2 = null;
                }

                if (strHTEL != "")
                {
                    strHTEL = VB.Replace(strHTEL, "-", "");
                    strHTEL = VB.Replace(strHTEL, " ", "");

                    strRettel = "0542608041";

                    strMsg = "현재 " + strName + "님미비건수";
                    strMsg = strMsg + ComNum.VBLF + "종이챠트:" + nCnt1 + "건 ";
                    strMsg = strMsg + ComNum.VBLF + "EMR:" + nCnt2 + "건입니다 ";
                    strMsg = strMsg + ComNum.VBLF + " -의료정보팀- ";

                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'','',' ";
                    SQL += strHTEL + "','I','','',";
                    SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }
                #endregion
            }
        }

        void INSA_Birthday()
        {
            DataTable AdoRes = null;
            DataTable Rs = null;

            int i = 0;
            string strHTEL = "";
            string strName = "";
            string strMsg = "";
            string strBirt = "";

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT SERENAME, HTEL, CHUK_MM, CHUK_DD FROM ADMIN.INSA_MST " + ComNum.VBLF;
            SQL += " WHERE CHUK_MM = '" + VB.Mid(cpublic.strSysDate, 6, 2) + "' " + ComNum.VBLF;
            SQL += " AND CHUK_DD = '" + VB.Right(cpublic.strSysDate, 2) + "' " + ComNum.VBLF;
            SQL += " AND TOIDAY IS NULL " + ComNum.VBLF;
            SQL += " AND (HTEL IS NOT NULL OR HTEL <> '') " + ComNum.VBLF;
            SQL += " ORDER BY SERENAME" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            for (i = 0; i < AdoRes.Rows.Count; i++)
            {
                strName = AdoRes.Rows[i]["SERENAME"].ToString().Trim();
                strHTEL = AdoRes.Rows[i]["HTEL"].ToString().Trim();
                if (strHTEL.Length == 13)
                {
                    strHTEL = VB.Left(strHTEL, 3) + VB.Mid(strHTEL, 5, 4) + VB.Right(strHTEL, 4);
                }
                else
                {
                    strHTEL = VB.Left(strHTEL, 3) + VB.Mid(strHTEL, 5, 3) + VB.Right(strHTEL, 4);
                }

                strMsg = strName + "님 진심으로 축일을 축하드립니다.행복한 하루가 되세요!! 전산정보팀일동";

                SQL = " SELECT SNAME FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JOBDATE = TO_DATE('" + cpublic.strSysDate + " 09:00" + "','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strHTEL + "' " + ComNum.VBLF;
                SQL += " AND GUBUN  = 'C' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                if (Rs.Rows.Count == 0)
                {
                    SQL = " INSERT INTO ETC_SMS (JobDate,SName,HPhone,Gubun, RetTel,SendMsg, PSMHSEND) VALUES ( ";
                    SQL += " TO_DATE('" + cpublic.strSysDate + " 09:00" + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += " '" + strName + "','" + strHTEL + "','C', '054-260-8331', ";
                    SQL += " '" + strMsg + "','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                Rs.Dispose();
                Rs = null;
            }

            AdoRes.Dispose();
            AdoRes = null;

            //생일(양력)자 조회
            SQL = " SELECT KORNAME, HTEL, CHUK_MM, CHUK_DD FROM ADMIN.INSA_MST " + ComNum.VBLF;
            SQL += " WHERE (CHUK_MM IS NULL OR CHUK_MM = '') " + ComNum.VBLF;
            SQL += " AND BIRTHGU = '0' " + ComNum.VBLF;
            SQL += " AND TO_CHAR(BIRTHDAY,'MM-DD') = '" + VB.Mid(cpublic.strSysDate, 6, 5) + "' " + ComNum.VBLF;
            SQL += " AND TOIDAY IS NULL " + ComNum.VBLF;
            SQL += " AND (HTEL IS NOT NULL OR HTEL <> '') " + ComNum.VBLF;
            SQL += " ORDER BY KORNAME" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            for (i = 0; i < AdoRes.Rows.Count; i++)
            {
                strName = AdoRes.Rows[i]["KORNAME"].ToString().Trim();
                strHTEL = AdoRes.Rows[i]["HTEL"].ToString().Trim();

                if (strHTEL.Length == 13)
                {
                    strHTEL = VB.Left(strHTEL, 3) + VB.Mid(strHTEL, 5, 4) + VB.Right(strHTEL, 4);
                }
                else
                {
                    strHTEL = VB.Left(strHTEL, 3) + VB.Mid(strHTEL, 5, 3) + VB.Right(strHTEL, 4);
                }

                strMsg = strName + "님 진심으로 생일을 축하드립니다.행복한 하루가 되세요!! 전산정보팀일동";

                SQL = " SELECT SNAME FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JOBDATE = TO_DATE('" + cpublic.strSysDate + " 09:00" + "','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strHTEL + "' " + ComNum.VBLF;
                SQL += " AND GUBUN  = 'C' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                if (Rs.Rows.Count == 0)
                {
                    SQL = " INSERT INTO ETC_SMS (JobDate,SName,HPhone,Gubun, RetTel,SendMsg) VALUES ( ";
                    SQL += " TO_DATE('" + cpublic.strSysDate + " 09:00" + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += " '" + strName + "','" + strHTEL + "','C', '054-260-8331', ";
                    SQL += " '" + strMsg + "')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                Rs.Dispose();
                Rs = null;
            }

            AdoRes.Dispose();
            AdoRes = null;

            //생일(음력)자 조회

            //양력->음력 변경
            strBirt = ToLunarDate(DateTime.Now);

            SQL = " SELECT KORNAME, HTEL, CHUK_MM, CHUK_DD FROM ADMIN.INSA_MST " + ComNum.VBLF;
            SQL += " WHERE (CHUK_MM IS NULL OR CHUK_MM = '') " + ComNum.VBLF;
            SQL += " AND BIRTHGU = '1' " + ComNum.VBLF;
            SQL += " AND TO_CHAR(BIRTHDAY,'MM-DD') = '" + VB.Mid(strBirt, 6, 5) + "' " + ComNum.VBLF;
            SQL += " AND TOIDAY IS NULL " + ComNum.VBLF;
            SQL += " AND (HTEL IS NOT NULL OR HTEL <> '') " + ComNum.VBLF;
            SQL += " ORDER BY KORNAME " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            for (i = 0; i < AdoRes.Rows.Count; i++)
            {
                strName = AdoRes.Rows[i]["KORNAME"].ToString().Trim();
                strHTEL = AdoRes.Rows[i]["HTEL"].ToString().Trim();
                if(strHTEL.Length == 13)
                {
                    strHTEL = VB.Left(strHTEL, 3) + VB.Mid(strHTEL, 5, 4) + VB.Right(strHTEL, 4);
                }
                else
                {
                    strHTEL = VB.Left(strHTEL, 3) + VB.Mid(strHTEL, 5, 3) + VB.Right(strHTEL, 4);
                }

                strMsg = strName + "님 진심으로 생일을 축하드립니다. 행복한 하루가 되세요!! 전산정보팀일동";

                SQL = " SELECT SNAME FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JOBDATE = TO_DATE('" + cpublic.strSysDate + " 09:00" + "','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strHTEL + "'" + ComNum.VBLF;
                SQL += " AND GUBUN  = 'C'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref Rs , SQL, clsDB.DbCon);

                if(Rs.Rows.Count == 0)
                {
                    SQL = " INSERT INTO ETC_SMS (JobDate,SName,HPhone,Gubun, RetTel,SendMsg, PSMHSEND) VALUES ( ";
                    SQL += " TO_DATE('" + cpublic.strSysDate + " 09:00" + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += " '" + strName + "','" + strHTEL + "','C', '054-260-8331',";
                    SQL += " '" + strMsg + "','Y')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                Rs.Dispose();
                Rs = null;
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void 의료급여_내시경()
        {
            DataTable rsSub = null;
            DataTable Rs = null;
            int i = 0;
            string strPano = "";
            string strName = "";
            string strHTEL = "";
            string strMsg = "";
            int nREAD = 0;

            SQL = " SELECT A.PTNO,  TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE, " + ComNum.VBLF;
            SQL += " A.DEPTCODE, A.DRCODE, B.SNAME " + ComNum.VBLF;
            SQL += " FROM ADMIN.ENDO_JUPMST A, ADMIN.BAS_PATIENT B " + ComNum.VBLF;
            SQL += " WHERE A.RDATE = TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND A.ORDERCODE IN ('GI1','GI1A','GI2','GI2A','GI3','GI3A') " + ComNum.VBLF;
            SQL += " AND A.PTNO = B.PANO(+) " + ComNum.VBLF;
            SQL += " AND A.RESULTDATE IS NULL " + ComNum.VBLF;
            SQL += " ORDER BY PTNO " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref rsSub, SQL, clsDB.DbCon);

            nREAD = rsSub.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strName = rsSub.Rows[i]["SNAME"].ToString().Trim();
                strPano = rsSub.Rows[i]["PTNO"].ToString().Trim();
                strHTEL = "0168784371";

                strMsg = "★의료급여 내시경 검사★ 등록번호: " + strPano + " 성명: " + strName;

                SQL = " SELECT SNAME FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JOBDATE = TO_DATE('" + cpublic.strSysDate + " 08:00" + "','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND HPHONE = '" + strHTEL + "' " + ComNum.VBLF;
                SQL += " AND GUBUN  = 'C' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                if (Rs.Rows.Count == 0)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = " INSERT INTO ETC_SMS (JobDate,SName,HPhone,Gubun, RetTel,SendMsg,PSMHSEND) VALUES ( ";
                    SQL += " TO_DATE('" + cpublic.strSysDate + " 08:00" + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += " '김은진','" + strHTEL + "','C', '1004', ";
                    SQL += " '" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }

                Rs.Dispose();
                Rs = null;
            }

            rsSub.Dispose();
            rsSub = null;
        }

        void HD_SMS_SEND()
        {
            //20일후 월,화 인공신장실 문자 
            int i = 0;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";

            string strPano = "";
            string strName = "";
            string strTime = "";
            string strTel = "";
            string strRettel = "";
            string strData = "";
            string strMsg = "";
            string strMinRTime = "";

            DataTable AdoRes = null;
            DataTable rs1 = null;

            //익일 근무일자를 구함 
            read_sysdate();

            strFDate = CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, 1);
            strTDate = CF.DATE_ADD(clsDB.DbCon, strFDate, 1);

            //전송희망시각 SET 
            strTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT A.PANO, A.SNAME,B.HPHONE " + ComNum.VBLF;
            SQL += " FROM ADMIN.OPD_MASTER A, ADMIN.BAS_PATIENT B " + ComNum.VBLF;
            SQL += " WHERE A.PANO = B.PANO " + ComNum.VBLF;
            SQL += " AND ACTDATE = TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND A.DEPTCODE = 'HD' " + ComNum.VBLF;
            SQL += " AND (B.GBSMS <> 'X' OR B.GBSMS IS NULL) " + ComNum.VBLF;
            SQL += " AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') " + ComNum.VBLF;
            SQL += " GROUP BY a.Pano,A.SName,b.HPhone " + ComNum.VBLF;
            SQL += " ORDER BY a.Pano,A.SName,b.HPhone " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strName = "";
                strTel = "";
                strPano = VB.Val(AdoRes.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                strName = AdoRes.Rows[i]["SNAME"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HPHONE"].ToString().Trim();
                strData = AdoRes.Rows[i]["Sname"].ToString().Trim();

                //이미 자료를 넘겼는지 확인함
                SQL = " SELECT MIN(TO_CHAR(RTime,'YYYY-MM-DD HH24:MI')) RTime FROM ETC_SMS " + ComNum.VBLF;
                SQL += " WHERE JobDate>=TO_DATE('" + cpublic.strSysDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += " AND JobDate<=TO_DATE('" + cpublic.strSysDate + " 23:59','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += " AND Pano='" + strPano + "'" + ComNum.VBLF;
                SQL += " AND Gubun='81'" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                strMinRTime = "";

                if (rs1.Rows.Count > 0)
                {
                    strMinRTime = rs1.Rows[0]["RTime"].ToString().Trim();
                }

                rs1.Dispose();
                rs1 = null;

                //이미 전송한 예약시간이 적으면 다시 전송안함 
                if (strMinRTime != "" && strMinRTime.CompareTo(strTime) <= 0)
                {
                    strTel = "";
                }

                //재원중인 환자는 제외
                SQL = " SELECT Pano FROM ADMIN.IPD_NEW_MASTER " + ComNum.VBLF;
                SQL += " WHERE Pano='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    strTel = "";
                }

                rs1.Dispose();
                rs1 = null;

                //SMS 자료를 INSERT
                if (strName != "" && strTel != "")
                {
                    strRettel = "0542608271"; //인공신장실 접수

                    strMsg = "[포항성모병원 인공신장실 혈액검사 안내]";
                    strMsg = strMsg + "  ";
                    strMsg = strMsg + " 매월 초 혈액검사 예정입니다.";
                    strMsg = strMsg + " 처방되어진 약을 규칙적으로 복용하여 주시고";
                    strMsg = strMsg + " 칼륨 및 인 함량이 높은 음식은 제한하여 주십시요. ";

                    //자료를 DB에 INSERT 
                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'";
                    SQL += strPano + "','" + strName + "','" + strTel + "','81','HD','',";
                    SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                    SQL += strRettel + "','','" + strMsg + "','Y') "; 

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void SMS_OCS_ANTI_MST()
        {
            DataTable Rs = null;
            DataTable rs2 = null;

            int i = 0;
            string strPano = "";
            string strName = "";
            string strHTEL = "";
            string strMsg = "";
            int nREAD = 0;
            int nREAD3 = 0;
            string strKorname = "";

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT A.PANO, B.SNAME, A.SUCODE, A.STATE, A.EXDATE, A.OKDATE, A.SMSDATE, " + ComNum.VBLF;
            SQL += " A.SMSDATE2, A.SABUN, A.ROWID, C.KORNAME, C.HTEL " + ComNum.VBLF;
            SQL += " FROM ADMIN.OCS_ANTI_MST A, ADMIN.BAS_PATIENT B, ADMIN.INSA_MST C " + ComNum.VBLF;
            SQL += " WHERE (A.SMSDATE IS NULL OR A.SMSDATE2 IS NULL) " + ComNum.VBLF;
            SQL += " AND A.OKDATE IS NOT NULL " + ComNum.VBLF;
            SQL += " AND A.PANO = B.PANO(+) " + ComNum.VBLF;
            SQL += " AND A.SABUN = C.SABUN " + ComNum.VBLF;
            SQL += " AND C.TOIDAY IS NULL " + ComNum.VBLF;
            SQL += " ORDER BY A.PANO " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strName = Rs.Rows[i]["SNAME"].ToString().Trim();
                strPano = Rs.Rows[i]["PANO"].ToString().Trim();
                strHTEL = VB.Replace(Rs.Rows[i]["HTEL"].ToString().Trim(), "-", "");
                strKorname = Rs.Rows[i]["KORNAME"].ToString().Trim();

                //입원환자 필터링(입원중인 환자만 SMS전송)
                SQL = " SELECT PANO, INDATE, OUTDATE " + ComNum.VBLF;
                SQL += " FROM ADMIN.IPD_NEW_MASTER " + ComNum.VBLF;
                SQL += " WHERE PANO = '" + strPano + "'" + ComNum.VBLF;
                SQL += " AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                nREAD3 = rs2.Rows.Count;

                rs2.Dispose();
                rs2 = null;

                strMsg = "";

                if (strName != "" && strHTEL != "" && nREAD > 0)
                {
                    if (Rs.Rows[i]["SMSDATE"].ToString().Trim() == "")
                    {
                        strMsg = "";
                        strMsg = strMsg + strName + " 환자의 " + Rs.Rows[i]["SuCode"].ToString().Trim() + " 사용 요청이 ";

                        //상태(1.승인, 2.보류, 3.불가)
                        switch (Rs.Rows[i]["STATE"].ToString().Trim())
                        {
                            case "1": strMsg = strMsg + "[승인] 되었습니다."; break;
                            case "2": strMsg = strMsg + "[보류] 되었습니다."; break;
                            case "3": strMsg = strMsg + "[불가] 되었습니다."; break;
                        }

                        strMsg = strMsg + "★유효기간: " + Rs.Rows[i]["EXDATE"].ToString().Trim() + " 까지입니다.";

                        SQL = " UPDATE ADMIN.OCS_ANTI_MST SET SMSDATE = '" + VB.Replace(cpublic.strSysDate, "-", "") + "' ";
                        SQL += " WHERE ROWID = '" + Rs.Rows[i]["ROWID"].ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                    else if (Rs.Rows[i]["SMSDATE2"].ToString().Trim() == "")
                    {
                        if (Rs.Rows[i]["EXDATE"].ToString().Trim() == cpublic.strSysDate)
                        {
                            strMsg = "";
                            strMsg = strMsg + strName + "환자의 " + Rs.Rows[i]["SuCode"].ToString().Trim() + " ";
                            strMsg = strMsg + "★유효기간이 " + Rs.Rows[i]["EXDATE"].ToString().Trim() + " 까지 입니다. 신청서 작성 요망!!";

                            SQL = " UPDATE ADMIN.OCS_ANTI_MST SET SMSDATE2 = '" + VB.Replace(cpublic.strSysDate, "-", "") + "' ";
                            SQL += " WHERE ROWID = '" + Rs.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        }
                    }
                }

                if (strMsg != "")
                {
                    SQL = " INSERT INTO ETC_SMS (JobDate, PANO , SName,HPhone,Gubun, RetTel,SendMsg, PSMHSEND) VALUES ( ";
                    SQL += " SYSDATE, ";
                    SQL += " '" + strPano + "' ,'" + strName + "' ,'" + strHTEL + "','J', '054-272-0151', ";
                    SQL += " '" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            Rs.Dispose();
            Rs = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void IPD_입원자_의사에게문자()
        {
            DataTable AdoRes = null;
            DataTable rs1 = null;

            int i = 0;
            int j = 0;
            int nREAD = 0;
            string strPano = "";
            string strTime = "";
            string strName = "";
            string strTel = "";
            string strData = "";
            string strROWID = "";
            string strRettel = "";
            string strMsg = "";
            string strJobTime = "";
            string strDeptCode = "";
            string strDRCode = "";

            string strOK = "";

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT d.HTEL,c.DrName,a.SName,a.Pano,a.Sex,a.Age, a.DrCode,a.DeptCode,a.WardCode,a.RoomCode, c.SABUN " + ComNum.VBLF;
            SQL += " FROM  ADMIN.IPD_NEW_MASTER a, ADMIN.OCS_DOCTOR c,  ADMIN.INSA_MST D " + ComNum.VBLF;
            SQL += " Where TRUNC(A.ipwontime) = TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND A.DRCODE = C.DRCODE " + ComNum.VBLF;
            SQL += " AND C.SABUN = D.SABUN " + ComNum.VBLF;
            SQL += " AND A.Pano <> '81000004' " + ComNum.VBLF;
            SQL += " AND D.TOIDAY IS NULL " + ComNum.VBLF;
            SQL += " AND a.DrCode NOT IN ('5208')" + ComNum.VBLF;
            SQL += " group by d.HTEL,c.drname,a.sname,a.pano,a.sex,a.age, a.drcode,a.deptcode,a.wardcode,a.roomcode,c.SABUN " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strOK = "OK";

                strPano = AdoRes.Rows[i]["Pano"].ToString().Trim();
                strDRCode = AdoRes.Rows[i]["DrCode"].ToString().Trim();
                strDeptCode = AdoRes.Rows[i]["DeptCode"].ToString().Trim();


                SQL = " SELECT Pano FROM ADMIN.IPD_NEW_MASTER " + ComNum.VBLF;  //오늘이전 재원건 있으면 제외
                SQL += " WHERE Pano ='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND TRUNC(ipwontime) < TRUNC(SYSDATE) " + ComNum.VBLF;
                SQL += " AND (ACTDATE IS NULL OR ACTDATE ='') " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    strOK = "";
                }

                rs1.Dispose();
                rs1 = null;

                SQL = " SELECT Pano FROM ADMIN.ETC_SMS " + ComNum.VBLF;  //당일 문자전송건 제외
                SQL += " WHERE Pano ='" + strPano + "' " + ComNum.VBLF;
                SQL += " AND GUBUN ='28' " + ComNum.VBLF;
                SQL += " AND TRUNC(JOBDATE) =TRUNC(SYSDATE) " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs1, SQL, clsDB.DbCon);

                if (rs1.Rows.Count > 0)
                {
                    strOK = "";
                }

                rs1.Dispose();
                rs1 = null;

                if (strOK == "OK")
                {
                    strTime = "";
                    strName = "";
                    strTel = "";

                    strName = AdoRes.Rows[i]["SName"].ToString().Trim();
                    strTel = AdoRes.Rows[i]["HTEL"].ToString().Trim();

                    if (strName != "" && strTel != "")
                    {
                        strRettel = "054-272-0151";

                        strMsg = "입원 알림 ";
                        strMsg = strMsg + "" + "담당의:" + AdoRes.Rows[i]["DrName"].ToString().Trim() + " ";
                        strMsg = strMsg + "" + AdoRes.Rows[i]["PANO"].ToString().Trim() + " " + AdoRes.Rows[i]["SNAME"].ToString().Trim() + " ";
                        strMsg = strMsg + "" + AdoRes.Rows[i]["WARDCODE"].ToString().Trim() + "/" + AdoRes.Rows[i]["RoomCode"].ToString().Trim() + "호실";
                        strMsg = strMsg + " 입원됨.";

                        //자료를 DB에 INSERT 
                        SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                        SQL += " RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'";
                        SQL += strPano + "','" + strName + "','" + strTel + "','28','" + strDeptCode + "','" + strDRCode + "', ";
                        SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                        SQL += strRettel + "','','" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        //2018-12-28(산부인과 김도균과장님 환자 입원시 코딩네이터 휴대폰으로도 문자 발송)
                        if (strDRCode == "3111")
                        {
                            SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime,";
                            SQL += "RetTel,SendTime,SendMsg,PSMHSEND) VALUES (SYSDATE,'";
                            SQL += strPano + "','" + strName + "','010-4880-7715','28','" + strDeptCode + "','" + strDRCode + "',";
                            SQL += "TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                            SQL += strRettel + "','','" + strMsg + "','Y')";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }

                        //전담에게도 입원자 문자전송 
                        OCS_DOCTOR_SCH(strPano, strName, strTime, strRettel, strMsg, AdoRes.Rows[i]["SABUN"].ToString().Trim(),"28");
                    }

                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void InserGubun39(string ArgDATE)
        {
            int i = 0;
            string strMsg = "";
            string strPano = "";
            int nREAD = 0;

            DataTable RS4 = null;

            SQL = " SELECT BDATE, PANO, ROOMCODE " + ComNum.VBLF;
            SQL += " FROM ( " + ComNum.VBLF;
            SQL += " SELECT B.BDATE, A.PANO, A.ROOMCODE " + ComNum.VBLF;
            SQL += " FROM IPD_NEW_MASTER A, IPD_NEW_SLIP B, BAS_DOCTOR C " + ComNum.VBLF;
            SQL += " WHERE B.BDATE = TO_DATE('" + ArgDATE + "','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND B.SUNEXT IN (       SELECT CODE FROM ADMIN.DRUG_YAKMUL " + ComNum.VBLF;
            SQL += " WHERE BUN IN ('02')) " + ComNum.VBLF;
            SQL += " AND A.IPDNO = B.IPDNO " + ComNum.VBLF;
            SQL += " AND A.PANO = B.PANO   AND B.DRCODE = C.DRCODE " + ComNum.VBLF;
            SQL += " GROUP BY  B.BDATE, A.PANO, A.ROOMCODE) MST " + ComNum.VBLF;
            SQL += " WHERE NOT EXISTS " + ComNum.VBLF;
            SQL += " (SELECT  * FROM ADMIN.ETC_SMS SUB  " + ComNum.VBLF;
            SQL += " WHERE MST.PANO = SUB.PANO " + ComNum.VBLF;
            SQL += " AND SUB.GUBUN = '39' " + ComNum.VBLF;
            SQL += " AND TRUNC(SUB.JOBDATE ) = MST.BDATE) " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref RS4, SQL, clsDB.DbCon);

            nREAD = RS4.Rows.Count;

            if (nREAD > 0)
            {
                for (i = 0; i < nREAD; i++)
                {
                    strPano = VB.Val(RS4.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                    strMsg = "★흡입기처방발생★" + ComNum.VBLF + ComNum.VBLF + RS4.Rows[i]["ROOMCODE"].ToString().Trim() + "호 " + READ_PatientName(strPano) + "(등록번호:" + strPano + ")";

                    //2016-05-19 약제과 장정민 약사 요청 수신자 번호 변경
                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg,PSMHSEND) VALUES (SYSDATE,'";
                    SQL += strPano + "','" + READ_PatientName(strPano) + "','010-7756-4746','39','','',  ";
                    SQL += " SYSDATE, ";
                    SQL += " '054-260-9054','','" + strMsg + "','Y')  ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            RS4.Dispose();
            RS4 = null;
        }

        string READ_PatientName(string ArgPano)
        {
            string rtnVal = "";

            DataTable RsFunc = null;

            if (ArgPano == "0") { return rtnVal; }

            SQL = " SELECT SName FROM ADMIN.BAS_PATIENT " + ComNum.VBLF;
            SQL += " WHERE Pano='" + ArgPano + "' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref RsFunc, SQL, clsDB.DbCon);

            if (RsFunc.Rows.Count > 0)
            {
                rtnVal = RsFunc.Rows[0]["SName"].ToString().Trim();
            }
            else
            {
                rtnVal = "";
            }

            RsFunc.Dispose();
            RsFunc = null;

            return rtnVal;
        }

        void Transfor_To_Doctor_Send_RealTime()
        {
            //의사들에게 전실 심장내과(병동 이동(ICU-> 일반병실 or 일반병실 -> ICU) 문자 보내는 부분 
            int i = 0;
            int j = 0;
            int nREAD = 0;
            string strPano = "";
            string strTime = "";
            string strName = "";
            string strHTEL = "";
            string strMsg = "";
            bool strSMSSend = true;

            string strSabun = "";
            string strYYMM = "";

            DataTable AdoRes = null;
            DataTable Rs = null;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT A.PANO, A.SNAME, A.FRWARD, A.TOWARD, A.TOROOM, A.TODOCTOR , A.ROWID ,  B.SABUN " + ComNum.VBLF;
            SQL += " FROM IPD_TRANSFOR    A, ADMIN.OCS_DOCTOR B " + ComNum.VBLF;
            SQL += " WHERE TRSDATE >= TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND ( A.FRWARD = 'IU'  OR A.TOWARD ='IU') " + ComNum.VBLF;
            SQL += " AND ( A.FRDEPT = 'MC' OR A.TODEPT= 'MC') " + ComNum.VBLF;
            SQL += " AND A. FRWARD <> A.TOWARD " + ComNum.VBLF;
            SQL += " AND A.TODOCTOR = B.DRCODE " + ComNum.VBLF;
            SQL += " AND (A.SMS IS NULL OR A.SMS <>'*') " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                //MC 과로 한정 
                strPano = VB.Val(AdoRes.Rows[i]["PANO"].ToString().Trim()).ToString("00000000");
                strSabun = AdoRes.Rows[i]["SABUN"].ToString().Trim();
                strTime = "";
                strName = AdoRes.Rows[i]["SNAME"].ToString().Trim();
                strMsg = "[전실알림]" + AdoRes.Rows[i]["FRWARD"].ToString().Trim() + "=>" + AdoRes.Rows[i]["TOROOM"].ToString().Trim() + "호실/" + strName;

                //전공의 전화번호 READ
                SQL = " SELECT MAX(YYMM) YYMM FROM ADMIN.OCS_DOCTOR_SCH " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                if (Rs.Rows.Count > 0)
                {
                    strYYMM = Rs.Rows[0]["YYMM"].ToString().Trim();
                }

                Rs.Dispose();
                Rs = null;

                SQL = " SELECT A.SABUN, a.setsabun,  B.HTEL  FROM ADMIN.OCS_DOCTOR_SCH  A, ADMIN.INSA_MST B " + ComNum.VBLF;
                SQL += " WHERE A.SABUN  = '" + strSabun + "' " + ComNum.VBLF;
                SQL += " AND A.SETSABUN = B.SABUN " + ComNum.VBLF;
                if (strYYMM != "")
                {
                    SQL += " AND A.YYMM = '" + strYYMM + "' " + ComNum.VBLF;
                }

                SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

                strSMSSend = false;

                for (j = 0; j < Rs.Rows.Count; j++)
                {
                    strHTEL = Rs.Rows[i]["HTEL"].ToString().Trim();

                    SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                    SQL += " RetTel,SendTime,SendMsg,PSMHSEND) VALUES (SYSDATE,'";
                    SQL += strPano + "','" + strName + "','" + strHTEL + "','37','','', ";
                    SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'0542720151','','" + strMsg + "','Y') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    strSMSSend = true;
                }

                Rs.Dispose();
                Rs = null;

                if (strSMSSend == true)
                {
                    SQL = " UPDATE ADMIN.IPD_TRANSFOR SET SMS = '*' ";
                    SQL += " WHERE ROWID = '" + AdoRes.Rows[i]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);


                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Consult_PC_Only_Realtime_Send()
        {
            DataTable Rs = null;
            DataTable rs3 = null;
            DataTable rs2 = null;

            string strSendDateRange = "";
            int nREAD = 0;
            int i = 0;
            int nREAD3 = 0;
            int k = 0;
            string strSENDMSG = "";
            string strSname = "";
            string strGbconfirm = "";
            string strGbsend = "";
            string strSms_send = "";
            int nREAD2 = 0;
            string strFrDeptCode = "";
            string strHTEL = "";
            string strRettel = "";
            string strDateTime = "";

            strSendDateRange = VB.DateAdd("D", -5, cpublic.strSysDate).ToString();
            strSendDateRange = VB.Left(strSendDateRange, 10);

            strDateTime = cpublic.strSysDate + " " + cpublic.strSysTime;

            //마취과 컨설트 현황
            SQL = " select bdate, sname, gbconfirm, todrcode, gbsend, sms_send, frdeptcode, binpid " + ComNum.VBLF;
            SQL += " From ADMIN.ocs_itransfer " + ComNum.VBLF;
            SQL += " where todeptcode = 'PC' " + ComNum.VBLF;
            SQL += " and bdate >= to_date('" + cpublic.strSysDate + "', 'yyyy-mm-dd') " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref Rs, SQL, clsDB.DbCon);

            nREAD = Rs.Rows.Count;

            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < nREAD; i++)
            {
                strRettel = Read_Insa_Sabun_by_Mobile(Rs.Rows[i]["BINPID"].ToString().Trim());
                strSname = Rs.Rows[i]["SNAME"].ToString().Trim();
                strGbconfirm = Rs.Rows[i]["GBCONFIRM"].ToString().Trim();
                strGbsend = Rs.Rows[i]["GBSEND"].ToString().Trim();
                strSms_send = Rs.Rows[i]["SMS_Send"].ToString().Trim();
                strFrDeptCode = Rs.Rows[i]["FRDEPTCODe"].ToString().Trim();

                //마취과(진료과장) 부서만 조회
                SQL = " select htel, mstel " + ComNum.VBLF;
                SQL += " from ADMIN.insa_mst " + ComNum.VBLF;
                SQL += " where buse = '011118' " + ComNum.VBLF;
                SQL += " and sabun not in ('06882','17723') " + ComNum.VBLF; //20131004-노선주과장님 임시제외(수술실 수간호사 요청)
                SQL += " and toiday is null" + ComNum.VBLF;

                SqlErr = clsDB.GetDataTableEx(ref rs3, SQL, clsDB.DbCon);

                nREAD3 = rs3.Rows.Count;

                for (k = 0; k < nREAD3; k++)
                {
                    strHTEL = rs3.Rows[k]["HTEL"].ToString().Trim();
                    strSENDMSG = "[컨설트수신알림]" + strFrDeptCode + "-" + strSname;

                    SQL = " select bigo " + ComNum.VBLF;
                    SQL += " from ADMIN.etc_sms " + ComNum.VBLF;
                    SQL += " where gubun = 'L1' " + ComNum.VBLF;
                    SQL += " and sname = '" + strSname + "' " + ComNum.VBLF;
                    SQL += " and hphone = '" + strHTEL + "' " + ComNum.VBLF;
                    SQL += " and jobdate between to_date('" + cpublic.strSysDate + " 00:01', 'yyyy-mm-dd hh24:mi') and to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi') " + ComNum.VBLF;

                    SqlErr = clsDB.GetDataTableEx(ref rs2, SQL, clsDB.DbCon);

                    nREAD2 = rs2.Rows.Count;

                    if (nREAD > 0 && nREAD2 == 0)
                    {
                        SQL = " insert into ADMIN.etc_sms( ";
                        SQL += " jobdate, sname, hphone, rettel, gubun, sendmsg, bigo, PSMHSEND) values( ";
                        SQL += " to_date('" + strDateTime + "', 'yyyy-mm-dd hh24:mi'), ";
                        SQL += " '" + strSname + "', '" + strHTEL + "', '" + strRettel + "', 'L1', '" + strSENDMSG + "', '전송완료','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    rs2.Dispose();
                    rs2 = null;
                }

                rs3.Dispose();
                rs3 = null;

            }

            Rs.Dispose();
            Rs = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Exam_Specmst_Sun_Send()
        {
            int i = 0;
            int nREAD = 0;
            string strJumin = "";
            string strName = "";
            string strHTEL = "";
            string strROWID = "";
            string strMsg = "";
            string strJobDate = "";
            string strReceiveDate = "";
            string strResultDate = "";
            string strPano = "";

            DataTable AdoRes = null;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT A.SPECNO, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.PANO, A.SNAME, E.JUMIN1, E.JUMIN2, B.SEX, B.AGE, C.IPDOPD, C.DEPTCODE, C.DRCODE, F.DRNAME, TO_CHAR(C.RECEIVEDATE,'YYYY-MM-DD HH24:MI') AS RECEIVEDATE, TO_CHAR(C.RESULTDATE,'YYYY-MM-DD HH24:MI') AS RESULTDATE, TO_CHAR(C.BDATE,'YYYY-MM-DD') AS GDATE " + ComNum.VBLF;
            SQL += ", B.MASTERCODE, C.SPECCODE, A.HPHONE, D.RESULT, A.EDOCUGU, A.SUGI1, A.SUGI2, B.ORDERNO, D.SUBCODE,A.ROWID " + ComNum.VBLF;
            SQL += " FROM ADMIN.EXAM_SPECMST_SUN A, ADMIN.EXAM_ORDER B, ADMIN.EXAM_SPECMST C " + ComNum.VBLF;
            SQL += ", ADMIN.EXAM_RESULTC D, ADMIN.BAS_PATIENT E, ADMIN.BAS_DOCTOR F " + ComNum.VBLF;
            SQL += " WHERE D.RESULTDATE >= TRUNC(SYSDATE - 1) " + ComNum.VBLF;
            SQL += " AND  D.RESULTDATE < TRUNC(SYSDATE + 1) " + ComNum.VBLF;
            SQL += " AND A.PANO = E.PANO " + ComNum.VBLF;
            SQL += " AND A.SPECNO = B.SPECNO " + ComNum.VBLF;
            SQL += " AND A.SPECNO = C.SPECNO " + ComNum.VBLF;
            SQL += " AND A.SPECNO = D.SPECNO " + ComNum.VBLF;
            SQL += " AND C.DRCODE = F.DRCODE " + ComNum.VBLF;
            SQL += " AND RESULT IN ('Negative','Indeterminate') " + ComNum.VBLF;
            //SQL += " AND (E.GBSMS <> 'X' OR E.GBSMS IS NULL) " + ComNum.VBLF; //2021-07-22 의료정보팀 요청으로 문자 비동의하신분 제외 처리
            SQL += " AND SUGI2 IS NULL " + ComNum.VBLF;
            SQL += " AND D.SUBCODE NOT IN ('GP26','GP251','GP254') " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strJumin = VB.Left(AdoRes.Rows[i]["JUMIN1"].ToString().Trim() + "-" + AdoRes.Rows[i]["JUMIN2"].ToString().Trim(), 8);
                strPano = AdoRes.Rows[i]["PANO"].ToString().Trim();
                strName = AdoRes.Rows[i]["SNAME"].ToString().Trim();
                strHTEL = AdoRes.Rows[i]["HPHONE"].ToString().Trim();
                strJobDate = AdoRes.Rows[i]["GDATE"].ToString().Trim();
                strReceiveDate = AdoRes.Rows[i]["RECEIVEDATE"].ToString().Trim();
                strResultDate = AdoRes.Rows[i]["RESULTDATE"].ToString().Trim();
                strROWID = AdoRes.Rows[i]["ROWID"].ToString().Trim();

                if (AdoRes.Rows[i]["RESULT"].ToString().Trim() == "Indeterminate")
                {
                    strMsg = "【포항성모병원:코로나19 확진검사(PCR)결과 안내】";
                    strMsg = strMsg + "1." + strReceiveDate + " 시행한 " + strName + "님(주민번호:" + strJumin + ")의 코로나19 확진검사의 결과가 미결정(Indeterminate)으로";
                    strMsg = strMsg + "확인되었음을 알려드립니다. (결과일: " + strResultDate + ") 2.2단계 재검을 통해 확정된 결과를 다시 문자로 안내해드릴 예정이오니 착오 없으시길 바랍니다.";
                    strMsg = strMsg + "(2단계 검사 도출 예상 소요시간 : 최대 6시간)";
                }
                else
                {
                    //if (AdoRes.Rows[i]["SUBCODe"].ToString().Trim() == "GP26A")
                    //{
                    //    strMsg = "【포항성모병원:코로나19 확진검사(PCR)결과 안내】";
                    //    strMsg = strMsg + "1." + strJobDate + " 시행한 " + strName + "님(주민번호:" + strJumin + ")의 코로나19-인플루엔자 동시검사의 결과가 음성임을 알려드립니다.";
                    //    strMsg = strMsg + "2.추가 진료가 필요한 경우 검사일로부터 3일 내에 꼭 진료를 받으십시오.(이후의 검사 결과는 유효하지 않음)";
                    //    strMsg = strMsg + "3.결과지 사본 등 서류발급은 본관 1층 원무팀 접수 후 제증명발급창구에서 발급 가능합니다.";
                    //    strMsg = strMsg + "서류발급 문의 : ☎054-260-8108(원무팀 제증명 창구)";
                    //    strMsg = strMsg + "※소아인 경우, 진료관련 문의사항은 소아청소년과 외래:☎054-260-8244,8245 로 연락바랍니다.";
                    //}
                    //else
                    //{
                        strMsg = "【포항성모병원:코로나19 확진검사(PCR)결과 안내】";
                        strMsg = strMsg + "1." + strReceiveDate + " 시행한 " + strName + "님(주민번호:" + strJumin + ")의 코로나19 확진검사의 결과가 음성임을 알려드립니다. (결과일: " + strResultDate + ")";
                        strMsg = strMsg + "2.추가 진료가 필요한 경우 검사일로부터 3일 내에 꼭 진료를 받으십시오.(이후의 검사 결과는 유효하지 않음)";
                        strMsg = strMsg + "3.결과지 사본 등 서류발급은 본관 1층 원무팀 접수 후 제증명발급창구에서 발급 가능합니다.";
                        strMsg = strMsg + "서류발급 문의 : ☎054-260-8108(원무팀 제증명 창구)";
                    //}
                }
                SQL = " INSERT INTO ETC_SMS (JobDate,Pano,Sname,HPhone,Gubun, ";
                SQL += " RetTel,SendMsg,EntSabun,EntDate, PSMHSEND) VALUES (SYSDATE, ";
                SQL += " '" + strPano + "','" + strName + "','" + strHTEL + "' ,'82','054-272-0151','" + strMsg + "','49834',SYSDATE,'Y') ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                SQL = " UPDATE ADMIN.EXAM_SPECMST_SUN SET SUGI2 = TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI') ";
                SQL += " WHERE ROWID = '" + strROWID + "'  ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
            
        }

        void Consult_To_Doctor_Req_RealTime()
        {
            int i = 0;
            int nREAD = 0;
            string strPano = "";
            string strTime = "";
            string strName = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strTODEPTCODE = "";
            bool strSMS = true;
            bool strSMSSend = true;

            DataTable AdoRes = null;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = " SELECT  B.DEPTCODE, B.PANO, B.SNAME, B.WARDCODE, A.FRDRCODE, " + ComNum.VBLF;
            SQL += " ( " + ComNum.VBLF;
            SQL += " SELECT HTEL FROM ADMIN.INSA_MST " + ComNum.VBLF;
            SQL += " Where SABUN = c.SABUN " + ComNum.VBLF;
            SQL += " AND TOIDAY IS NULL " + ComNum.VBLF;
            SQL += " AND SABUN NOT IN ('18210','20110','35104') " + ComNum.VBLF;
            SQL += " AND BUSE NOT IN ('011118', '011114') " + ComNum.VBLF;
            SQL += " ) HTEL, " + ComNum.VBLF;
            SQL += " C.SABUN, A.FRDEPTCODE, A.ROWID, a.binpid abinpid " + ComNum.VBLF;
            SQL += " FROM ADMIN.OCS_ITRANSFER A, ADMIN.IPD_NEW_MASTER B, ADMIN.OCS_DOCTOR C " + ComNum.VBLF;
            SQL += " WHERE A.GBCONFIRM = '*' " + ComNum.VBLF;
            SQL += " AND A.BDATE between TRUNC(SYSDATE-5) and TRUNC(SYSDATE) " + ComNum.VBLF;
            SQL += " AND A.GBDEL <> '*' " + ComNum.VBLF;
            SQL += " AND A.PTNO = B.PANO " + ComNum.VBLF;
            SQL += " AND B.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') " + ComNum.VBLF;
            SQL += " AND A.FRDRCODE IS NOT NULL " + ComNum.VBLF;
            SQL += " AND A.FRDRCODE = C.DRCODE " + ComNum.VBLF;
            SQL += " AND A.Ptno <> '81000004' " + ComNum.VBLF;
            SQL += " AND C.SABUN NOT IN ('18210', '20110', '35104') " + ComNum.VBLF;
            SQL += " AND C.DEPTCODE NOT IN('DM', 'PC') " + ComNum.VBLF;
            SQL += " AND (A.GBSEND IS NULL OR A.GBSEND = ' ') " + ComNum.VBLF;
            SQL += " AND (A.SMS_REQ IS NULL OR A.SMS_REQ = '') " + ComNum.VBLF;
            SQL += " ORDER BY A.TODRCODE " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref AdoRes, SQL, clsDB.DbCon);

            nREAD = AdoRes.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                strTODEPTCODE = AdoRes.Rows[i]["FRDEPTCODE"].ToString().Trim();
                strPano = AdoRes.Rows[i]["PANO"].ToString().Trim();

                strTime = "";
                strName = "";
                strTel = "";

                strName = AdoRes.Rows[i]["Sname"].ToString().Trim();
                strTel = AdoRes.Rows[i]["HTEL"].ToString().Trim();

                if (strName != "" && strTel != "")
                {
                    strSMSSend = false;

                    if (Read_Insa_Sabun_by_Rettel(AdoRes.Rows[i]["ABINPID"].ToString().Trim()) != "")
                    {
                        strRettel = Read_Insa_Sabun_by_Rettel(AdoRes.Rows[i]["ABINPID"].ToString().Trim());
                    }
                    else
                    {
                        strRettel = Read_Insa_Sabun_by_Mobile(AdoRes.Rows[i]["ABINPID"].ToString().Trim());
                    }

                    strMsg = "★의뢰한 컨설트 회신★";
                    strMsg = strMsg + "[" + AdoRes.Rows[i]["DEPTCODE"].ToString().Trim() + "]";
                    strMsg = strMsg + "[" + strPano + "]";
                    strMsg = strMsg + "[" + AdoRes.Rows[i]["SNAME"].ToString().Trim() + "]";
                    strMsg = strMsg + "[" + AdoRes.Rows[i]["WARDCODE"].ToString().Trim() + "]";

                    //자료를 DB에 인설트 
                    switch (AdoRes.Rows[i]["SABUN"].ToString().Trim())
                    {
                        case "07790": //이찬우 과장
                        case "04387": //성영호 과장
                        case "18210":
                        case "20110": //신대열 과장
                        case "35104": //김미정 과장
                        case "55288": //이찬우 과장
                        case "17723": //박미경 과장
                        case "37403": //이용식 과장
                            strSMS = false;
                            break;
                        default:
                            strSMS = true;
                            break;
                    }

                    //전공의 컨설트 문자전송 
                    strSMSSend = OCS_DOCTOR_SCH(strPano, strName, strTime, strRettel, strMsg, AdoRes.Rows[i]["SABUN"].ToString().Trim());

                    if (strSMS == true)
                    {
                        SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                        SQL += " RetTel,SendTime,SendMsg, PSMHSEND) VALUES (SYSDATE,'";
                        SQL += strPano + "','" + strName + "','" + strTel + "','L','','',";
                        SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),'";
                        SQL += strRettel + "','','" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("컨설트 문자 인설트 에러", "확인");
                            return;
                        }

                        strSMSSend = true;
                    }

                    if (strSMSSend == true)
                    {
                        SQL = " UPDATE ADMIN.OCS_ITRANSFER SET ";
                        SQL += " SMS_REQ = 'Y'  ";
                        SQL += " WHERE ROWID = '" + AdoRes.Rows[i]["ROWID"].ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("OCS_ITRANSFER 업데이트 에러", "확인");
                            return;
                        }
                    }

                }

            }

            AdoRes.Dispose();
            AdoRes = null;

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void Consult_To_Doctor_Send_ReadTime()
        {
            int i = 0;
            int nREAD = 0;
            string strPano = "";
            string strTime = "";
            string strName = "";
            string strTel = "";
            string strRettel = "";
            string strMsg = "";
            string strTODEPTCODE = "";
            string strTODRCODE = "";
            bool strSMS = true;
            bool strSMSSend = true;
            string strWardCode = "";

            DataTable dt = null;


            SQL = " SELECT  B.DEPTCODE, B.PANO, B.SNAME, B.WARDCODE, A.TODRCODE, ";
            SQL = SQL + " ( ";
            SQL = SQL + "  SELECT HTEL FROM ADMIN.INSA_MST ";
            SQL = SQL + "  Where SABUN = c.SABUN ";
            SQL = SQL + "  AND TOIDAY IS NULL ";
            SQL = SQL + "  AND SABUN NOT IN ('18210','20110','35104') ";
            SQL = SQL + "  AND BUSE NOT IN ('011118', '011114') ";
            SQL = SQL + "  ) HTEL, ";
            SQL = SQL + " C.SABUN, A.TODEPTCODE, A.GBEMSMS, A.ROWID, a.binpid abinpid ";
            SQL = SQL + " FROM ADMIN.OCS_ITRANSFER A, ADMIN.IPD_NEW_MASTER B, ADMIN.OCS_DOCTOR C  ";
            SQL = SQL + " WHERE A.GBCONFIRM <> '*' ";
            SQL = SQL + " AND A.BDATE between TRUNC(SYSDATE-5) and TRUNC(SYSDATE) ";
            SQL = SQL + " AND A.GBDEL <> '*' ";
            SQL = SQL + " AND A.PTNO = B.PANO ";
            SQL = SQL + " AND B.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
            SQL = SQL + " AND A.TODRCODE IS NOT NULL ";
            SQL = SQL + " AND A.TODRCODE = C.DRCODE ";
            SQL = SQL + " AND A.Ptno <> '81000004' ";
            SQL = SQL + " AND C.SABUN NOT IN ('18210','20110','35104') ";
            SQL = SQL + " AND C.DEPTCODE NOT IN ('DM','EN') ";
            SQL = SQL + " AND (A.GBSEND IS NULL OR A.GBSEND = ' ') ";
            SQL = SQL + " AND (A.SMS_SEND IS NULL OR A.SMS_SEND = '') ";
            SQL = SQL + " ORDER BY A.TODRCODE ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            nREAD = dt.Rows.Count;

            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < nREAD; i++)
            {
                strTODEPTCODE = dt.Rows[i]["TODEPTCODE"].ToString().Trim();
                strTODRCODE = dt.Rows[i]["TODRCODE"].ToString().Trim();
                strPano = dt.Rows[i]["PANO"].ToString().Trim();
                strWardCode = dt.Rows[i]["WARDCODE"].ToString();

                strTime = "";
                strName = "";
                strTel = "";

                strName = dt.Rows[i]["Sname"].ToString().Trim();
                strTel = dt.Rows[i]["HTEL"].ToString().Trim();

                if (strName != "" && strTel != "")
                {
                    strRettel = Read_Insa_Sabun_by_Mobile(dt.Rows[i]["abinpid"].ToString().Trim());

                    if (dt.Rows[i]["GBEMSMS"].ToString().Trim() == "Y") { strMsg = "★응급★"; } else { strMsg = ""; }

                    strMsg = strMsg + "컨설트 알림 ";
                    strMsg = strMsg + "[" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "]";
                    strMsg = strMsg + "[" + strPano + "]";
                    strMsg = strMsg + "[" + dt.Rows[i]["SNAME"].ToString().Trim() + "]";
                    strMsg = strMsg + "[" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "]";
                    strMsg = strMsg + " 의뢰되었습니다.";

                    switch (dt.Rows[i]["SABUN"].ToString().Trim())
                    {
                        case "07790": //이찬우 과장
                        case "04387": //성영호 과장
                        case "18210":
                        case "20110": //신대열 과장
                        case "35104": //김미정 과장
                        case "55288": //이찬우 과장
                        case "17723": //박미경 과장
                        case "37403": //이용식 과장
                            strSMS = false;
                            break;
                        default:
                            strSMS = true;
                            break;
                    }

                    strSMSSend = OCS_DOCTOR_SCH(strPano, strName, strTime, strRettel, strMsg, dt.Rows[i]["SABUN"].ToString().Trim());

                    if (strSMS == true)
                    {
                        SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                        SQL += " RetTel,SendTime,SendMsg, PSMHSEND ) VALUES (SYSDATE,'";
                        SQL += strPano + "','" + strName + "','" + strTel + "','L','','',";
                        SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'),' ";
                        SQL += strRettel + "','','" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("컨설트 문자 인설트 에러", "확인");
                            return;
                        }

                        strSMSSend = true;
                    }

                    if (strSMSSend == true)
                    {
                        SQL = " UPDATE ADMIN.OCS_ITRANSFER SET ";
                        SQL += " SMS_SEND = 'Y'  ";
                        SQL += " WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("OCS_ITRANSFER 업데이트 에러", "확인");
                            return;
                        }
                    }

                    //영상의학과 컨설트 일 경우 은지쌤에게도 문자 날아가게 해놓음 
                    if (strTODEPTCODE == "RD" && strTODRCODE != "5309")
                    {
                        SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                        SQL += " RetTel,SendTime,SendMsg, PSMHSEND ) VALUES (SYSDATE, ";
                        SQL += " '" + strPano + "','" + strName + "','01093831203','L','','', ";
                        SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL += " '" + strRettel + "','','" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);  

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    //영상의학과 컨설트 일 경우 정태란쌤에게도 문자 날아가게 해놓음 
                    if (strTODEPTCODE == "RD" && strTODRCODE != "5309" && strTODRCODE != "5311")
                    {
                        SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                        SQL += " RetTel,SendTime,SendMsg,PSMHSEND ) VALUES (SYSDATE, ";
                        SQL += " '" + strPano + "','" + strName + "','01099123858','L','','', ";
                        SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL += " '" + strRettel + "','','" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    //영상의학과 컨설트 일 경우 박효근쌤에게도 문자 날아가게 해놓음 
                    if (strTODEPTCODE == "RD" && strTODRCODE != "5309")
                    {
                        SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                        SQL += " RetTel,SendTime,SendMsg, PSMHSEND ) VALUES (SYSDATE, ";
                        SQL += " '" + strPano + "','" + strName + "','01072367297','L','','', ";
                        SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL += " '" + strRettel + "','','" + strMsg + "','Y') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    //중환자실(내과) 컨설트일 경우 이종훈 전공의 에게도 문자 날아가게 해놓음
                    switch (strTODEPTCODE)
                    {
                        case "MG": case "MC": case "MP": case "MI": case "MN": case "MO": case "MR": case "HD": case "NP": case "NE": case "RD": case "FM":
                            if (strWardCode == "33" || strWardCode == "35")//중환자실 인경우 문자 전송
                            {
                                SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, ";
                                SQL += " RetTel,SendTime,SendMsg, PSMHSEND ) VALUES (SYSDATE, ";
                                SQL += " '" + strPano + "','" + strName + "','01096840595','L','','', ";
                                SQL += " TO_DATE('" + strTime + "','YYYY-MM-DD HH24:MI'), ";
                                SQL += " '" + strRettel + "','','" + strMsg + "','Y') ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    return;
                                }
                            }
                            break;
                    }
                }

            }

            clsDB.setCommitTran(clsDB.DbCon);

            dt.Dispose();
            dt = null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MYSQL_KT_SMS_INSERT("01066667472", "81000004", "SMS TEST SEND", "202108211200", DateTime.Now.ToString("yyyyMMddHHmmss"), "0542608338", "0");
        }

        string MYSQL_KT_SMS_INSERT(string ArgHTel, string ArgPano, string ArgMsg, string ArgTime, string ArgRTime, string ArgRTel, string Arg_SMS_Gubun)
        {
            clsDbMySql.DBConnect("192.168.2.35", "3306", "root", "rose3560@", "mcs");

            string rtnVal = "";

            rtnVal = "Y";

            SQL = "  ";
            SQL = SQL + " INSERT INTO SDK_SMS_SEND (MSG_ID, USER_ID, SCHEDULE_TYPE, SUBJECT, NOW_DATE, SEND_DATE, ";
            SQL = SQL + " CALLBACK, DEST_COUNT, DEST_INFO, SMS_MSG) ";
            SQL = SQL + " VALUES (0, 'smssend', '" + Arg_SMS_Gubun + "', '', ";
            SQL = SQL + " '" + ArgTime + "','" + ArgRTime + "','" + ArgRTel + "',0 ,'^" + ArgHTel + "','" + ArgMsg + "') ";

            if (!(clsDbMySql.ExecuteNonQuery(SQL)))
            {
                MessageBox.Show("INSERT SDK_SMS_SEND 테스트 오류", "확인");
                return rtnVal = "N";
            }

            clsDbMySql.DisDBConnect();

            return rtnVal;
        }

        string MYSQL_KT_MMS_IMG_INSERT(string ArgHTel, string ArgPano, string ArgMsg, string ArgTime, string ArgRTime, string ArgRTel, string Arg_SMS_Gubun)
        {
            clsDbMySql.DBConnect("192.168.2.35", "3306", "root", "rose3560@", "mcs");

            string rtnVal = "";

            rtnVal = "Y";

            SQL = "  ";
            SQL = SQL + " INSERT INTO SDK_MMS_SEND (USER_ID,SUBJECT, SCHEDULE_TYPE , NOW_DATE, SEND_DATE, DEST_COUNT, DEST_INFO, MSG_TYPE, MMS_MSG, ";
            SQL = SQL + " CONTENT_COUNT,CONTENT_DATA,CALLBACK) ";
            SQL = SQL + " VALUES ('xrobiztest5', '포항성모병원','" + Arg_SMS_Gubun + "', '" + ArgTime + "', '" + ArgRTime + "',1,'^" + ArgHTel + "',0,'격리병동 입원 안내서 입니다. 반드시 안내문을 읽고 준비하세요.', ";
            SQL = SQL + " 1,'corona.jpg^1^0','" + ArgRTel + "') ";
            //SQL = SQL + " 2,'test1.jpg^1^0|test2.sis^1^0','01066667472') ";

            if (!(clsDbMySql.ExecuteNonQuery(SQL)))
            {
                MessageBox.Show("INSERT SDK_MMS_SEND 테스트 오류", "확인");  
                return rtnVal = "N";
            }

            clsDbMySql.DisDBConnect();

            return rtnVal;
        }

        string MYSQL_KT_MMS_INSERT(string ArgHTel, string ArgPano, string ArgMsg, string ArgTime, string ArgRTime, string ArgRTel, string Arg_SMS_Gubun)
        {
            clsDbMySql.DBConnect("192.168.2.35", "3306", "root", "rose3560@", "mcs");

            string rtnVal = "";

            rtnVal = "Y";

            SQL = " INSERT INTO SDK_MMS_SEND ";
            SQL = SQL + " (USER_ID,  SCHEDULE_TYPE,  SUBJECT,  NOW_DATE,  SEND_DATE  ,  CALLBACK,  DEST_COUNT,  DEST_INFO, ";
            SQL = SQL + " MSG_TYPE, MMS_MSG , CONTENT_COUNT, CONTENT_DATA) ";
            SQL = SQL + " Values ";
            SQL = SQL + "  (  'mmssend',  '" + Arg_SMS_Gubun + "',  '포항성모병원', '" + ArgTime + "','" + ArgRTime + "', ";
            SQL = SQL + " '" + ArgRTel + "', 1, '^" + ArgHTel + "', 0, '" + ArgMsg + "' , 0, '');";

            if (!(clsDbMySql.ExecuteNonQuery(SQL)))
            {
                MessageBox.Show("INSERT SDK_MMS_SEND 테스트 오류", "확인");
                return rtnVal = "N";
            }

            clsDbMySql.DisDBConnect();

            return rtnVal;
        }

        string Read_Insa_Sabun_by_Mobile(string argSABUN)
        {
            DataTable RsFunc = null;

            string rtnVal = "";

            SQL = " SELECT SABUN, KORNAME, BUSE, HTEL ";
            SQL = SQL + " FROM ADMIN.INSA_MST ";
            SQL = SQL + " WHERE SABUN = '" + argSABUN + "' ";

            SqlErr = clsDB.GetDataTableEx(ref RsFunc, SQL, clsDB.DbCon);

            if (RsFunc.Rows.Count > 0)
            {
                if (RsFunc.Rows[0]["HTEL"].ToString().Trim() != "")
                {
                    rtnVal = RsFunc.Rows[0]["HTEL"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }

            RsFunc.Dispose();
            RsFunc = null;

            return rtnVal;
        }

        string Read_Insa_Sabun_by_Rettel(string argSABUN)
        {
            string rtnVal = "";

            DataTable RsFunc = null;

            if (argSABUN == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            //회수번호 설정(사내정보게시판-전공의스케줄 하단)
            SQL = " select code, name " + ComNum.VBLF;
            SQL += " from ADMIN.bas_bcode " + ComNum.VBLF;
            SQL += " where gubun = '개인회신번호설정' " + ComNum.VBLF;
            SQL += " and deldate is null " + ComNum.VBLF;
            SQL += " and code = '" + argSABUN + "' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref RsFunc, SQL, clsDB.DbCon);

            if (RsFunc.Rows.Count > 0)
            {
                if (RsFunc.Rows[0]["Name"].ToString().Trim() != "")
                {
                    rtnVal = RsFunc.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }

            RsFunc.Dispose();
            RsFunc = null;

            return rtnVal;
        }

        Boolean OCS_DOCTOR_SCH(string ArgPano, string ArgName, string ArgTime, string ArgRetTel, string ArgMsg, string ArgSABUN, string ArgGuBun = "")
        {
            DataTable RsSch = null;
            int j = 0;
            string strYYMM = "";
            string strHTEL = "";
            string strSchRettel = "";

            SQL = " SELECT MAX(YYMM) MYYMM ";
            SQL = SQL + " FROM ADMIN.OCS_DOCTOR_SCH ";
            SQL = SQL + " WHERE SABUN = '" + ArgSABUN.Trim() + "' ";

            SqlErr = clsDB.GetDataTableEx(ref RsSch, SQL, clsDB.DbCon);

            if (RsSch.Rows.Count > 0) { strYYMM = RsSch.Rows[0]["MYYMM"].ToString().Trim(); }

            RsSch.Dispose();
            RsSch = null;

            if (strYYMM != "")
            {
                //컨설트의 경우 전공의에게도 문자전송
                //전공의문자 세팅은 사내게시판-근무표-전공의스케줄(의국장이 셋팅함)
                SQL = " select b.sabun bsabun, b.htel bhtel, b.mstel bmstel ";
                SQL = SQL + " from ADMIN.ocs_doctor_sch a, ADMIN.insa_mst b ";
                SQL = SQL + " where a.sabun = '" + ArgSABUN.Trim() + "' ";
                SQL = SQL + " and a.yymm = '" + strYYMM + "' ";
                SQL = SQL + " and a.setsabun = b.sabun ";
                SQL = SQL + " and b.toiday is null ";
                SQL = SQL + " and a.setsabun not in ('31615','48757') ";

                SqlErr = clsDB.GetDataTableEx(ref RsSch, SQL, clsDB.DbCon);

                if (RsSch.Rows.Count > 0)
                {
                    for (j = 0; j < RsSch.Rows.Count; j++)
                    {
                        if (Read_Insa_Sabun_by_Rettel(RsSch.Rows[j]["BSABUN"].ToString().Trim()) != "")
                        {
                            strSchRettel = RsSch.Rows[j]["BSABUN"].ToString().Trim();
                        }
                        else
                        {
                            strSchRettel = ArgRetTel;
                        }

                        if (RsSch.Rows[j]["BMSTEL"].ToString().Trim() != "")
                        {
                            strHTEL = RsSch.Rows[j]["BMSTEL"].ToString().Trim();
                        }
                        else
                        {
                            strHTEL = RsSch.Rows[j]["BHTEL"].ToString().Trim();
                        }
                        if(ArgGuBun == "")
                        {
                            SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, RetTel,SendTime,SendMsg) VALUES ";
                            SQL += " (SYSDATE,'" + ArgPano + "','" + ArgName + "','" + strHTEL + "' ,'L','','', ";
                            SQL += " TO_DATE('" + ArgTime + "','YYYY-MM-DD HH24:MI'),'" + strSchRettel + "','','" + ArgMsg + "') ";
                        }
                        else if(ArgGuBun == "28") //입원자 알림 문자 전담 전송
                        {
                            SQL = " INSERT INTO ETC_SMS (JobDate,Pano,SName,HPhone,Gubun,DeptCode,DrCode,RTime, RetTel,SendTime,SendMsg) VALUES ";
                            SQL += " (SYSDATE,'" + ArgPano + "','" + ArgName + "','" + strHTEL + "' ,'28','','', ";
                            SQL += " TO_DATE('" + ArgTime + "','YYYY-MM-DD HH24:MI'),'" + strSchRettel + "','','" + ArgMsg + "') ";
                        }
                        

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("전공의 입원자 문자 알림 에러", "확인");
                            return false;
                        }
                    }
                }

                RsSch.Dispose();
                RsSch = null;
            }

            return true;
        }

        string TelNo_Edit_Process(string ArgTel)
        {
            string rtnVal = "";
            int i = 0;

            if (ArgTel == "") { rtnVal = ""; return rtnVal; }

            for (i = 1; i <= ArgTel.Length; i++)
            {
                if (Convert.ToInt32(VB.Val(VB.Mid(ArgTel, i, 1))) >= 0 && Convert.ToInt32(VB.Val(VB.Mid(ArgTel, i, 1))) <= 9)
                {
                    rtnVal = rtnVal + VB.Mid(ArgTel, i, 1);
                }
            }

            return rtnVal;
        }

        string APP_PUSH_TEST(string ArgHTel, string ArgPano, string ArgMsg)
        {
            DataTable RsBas = null;
            int nREAD = 0;

            string strPano = "";
            string strSENDMSG = "";
            string strHTEL = "";
            string strOK = "";

            string rtnVal = "";

            clsDbMySql.DBConnect("221.157.239.2", "3306", "psmh", "pmsh", "phsmh");

            rtnVal = "Y";

            strSENDMSG = ArgMsg;

            if (ArgPano == "")
            {
                strHTEL = clsSHA.SHA256(VB.Replace(ArgHTel, "-", ""));
            }

            SQL = " SELECT m_ptno   FROM tb_patbav " + ComNum.VBLF;
            if (ArgPano != "")
            {
                SQL += " WHERE M_PTNO = '" + ArgPano + "' " + ComNum.VBLF;
            }
            else
            {
                SQL += " WHERE M_TELNO = '" + strHTEL + "' " + ComNum.VBLF;
            }

            SQL += " AND M_GCMKEY IS NOT NULL " + ComNum.VBLF;

            RsBas = clsDbMySql.GetDataTable(SQL);

            nREAD = RsBas.Rows.Count;

            if (nREAD == 0) { rtnVal = "N"; return rtnVal; }

            strPano = RsBas.Rows[0]["PANO"].ToString().Trim();

            strOK = "OK"; 

            SQL = " INSERT INTO tb_mrappmmo(pid,resdate,deptcode,appnote,sendyn,senddate,rightnow,appemp,appdate ";
            SQL += " ,updemp,upddate,mrappmmoid,readyn,category,rtry_cnt,apptitle,callback, msgtype, webflag) ";
            SQL += " VALUES ('" + strPano + "','','00000','" + ArgMsg + "','N','','Y','WEB',NOW(),'WEB',NOW() ";
            SQL += " , (SELECT MAX(mrappmmoid)+1 FROM tb_mrappmmo t1) ,'N','NOTI','1','포항성모병원[알림]','','t', '') ";

            if (!(clsDbMySql.ExecuteNonQuery(SQL)))
            {
                strOK = "NO";
            }

            clsDbMySql.DisDBConnect();

            return rtnVal;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        string Read_Insa_Sabun_by_DeptName(string ArgSABUN)
        {
            string rtnVal = "";

            DataTable RsFunc = null;

            if (ArgSABUN == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            SQL = " select a.deptcode adeptcode, b.deptnamek bdeptnamek " + ComNum.VBLF;
            SQL += " from ADMIN.ocs_doctor a, ADMIN.bas_clinicdept b " + ComNum.VBLF;
            SQL += " where a.deptcode = b.deptcode " + ComNum.VBLF;
            SQL += " and a.sabun = '" + ArgSABUN + "' " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableEx(ref RsFunc, SQL, clsDB.DbCon);

            if (RsFunc.Rows.Count > 0)
            {
                if (RsFunc.Rows[0]["BDEPTNAMEK"].ToString().Trim() != "")
                {
                    rtnVal = RsFunc.Rows[0]["BDEPTNAMEK"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }

            RsFunc.Dispose();
            RsFunc = null;

            return rtnVal;
        }

        private string ToLunarDate(DateTime dt)
        {
            int nLunaMM;
            int nYY, nMM, nDD;
            bool bLunaMM = false;
            System.Globalization.KoreanLunisolarCalendar klc =
            new System.Globalization.KoreanLunisolarCalendar();

            nYY = klc.GetYear(dt);
            nMM = klc.GetMonth(dt);
            nDD = klc.GetDayOfMonth(dt);
            if (klc.GetMonthsInYear(nYY) > 12)             //1년이 12이상이면 윤달이 있음..
            {
                bLunaMM = klc.IsLeapMonth(nYY, nMM);     //윤월인지
                nLunaMM = klc.GetLeapMonth(nYY);             //년도의 윤달이 몇월인지?
                if (nMM >= nLunaMM)                           //달이 윤월보다 같거나 크면 -1을 함 즉 윤8은->9 이기때문
                    nMM--;
            }
            return nYY.ToString() + "-" + (bLunaMM ? "*" : "") + VB.Format(nMM,"00") + "-" + VB.Format(nDD,"00");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MYSQL_KT_MMS_IMG_INSERT("01066667472", "81000004", "SMS TEST SEND", "202108211200", DateTime.Now.ToString("yyyyMMddHHmmss"), "0542608338", "0");

            //MYSQL_KT_MMS_IMG_INSERT("01028134394", "81000004", "SMS TEST SEND", "202108211200", DateTime.Now.ToString("yyyyMMddHHmmss"), "0542608338", "0");
        }
    }
}
