using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSmsPay : Form
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
        int FnExamSunCnt = 0;

        public frmSmsPay()
        {
            InitializeComponent();
        }

        private void frmSmsPay_Load(object sender, EventArgs e)
        {
            read_sysdate();

            Screen_Clear();

            FstrKTSMS_사용여부 = "Y";

            lbl_P_STS.Text = "급여승인문자";

            //메세지 전송 시작하기
            TmrFlow.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = VB.Left(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T"), 5);
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

            CboTimeCycle.SelectedIndex = 0;
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

            nTime = Convert.ToInt16(VB.Val(txtTimeCnt.Text));
            nTimeMax = Convert.ToInt16(VB.Val(CboTimeCycle.Text));

            if (nTime >= nTimeMax)
            {
                txtTimeCnt.Text = "1";
                TmrFlow.Enabled = false;
                lblShow.Text = "메세지 전송중...";
            }
            else
            {
                return;
            }

            SMS_Many_Message_Send_KT_SMS();

            //전송시켰으면 처음부터 다시 카운트시작
            TmrAction.Enabled = false;
            TmrFlow.Enabled = true;
            lblShow.Text = "메세지 대기중...";
        }

        void SMS_Many_Message_Send_KT_SMS()
        {
            int i = 0;
            int nREAD = 0;
            string strTel = "";
            string strMsg = "";
            string strDeptCode = "";
            string strRettel = "";

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
            strSTime = VB.Replace(strDateTime, " ", "");
            strSTime = VB.Replace(strSTime, "-", "");
            strSTime = VB.Replace(strSTime, ":", "");

            strDateTime_1 = VB.DateAdd("D", -1, cpublic.strSysDate) + "";
            strDateTime_1 = VB.Left(strDateTime, 10) + " 00:01";
            strDateTime_2 = VB.DateAdd("D", -1, cpublic.strSysDate) + " 08:40";
            strDateTime_2 = VB.Left(strDateTime, 10) + " 08:40";
            strDateTime_3 = cpublic.strSysDate + " 00:01";
            strDateTime_4 = cpublic.strSysDate + " 08:40";
            strDateTime_5 = cpublic.strSysDate + " 23:40";

            SQL = " select jobdate, send_cnt, pano, sname, hphone, gubun, deptcode, " + ComNum.VBLF;
            SQL += " drcode, rettel, sendmsg, " + ComNum.VBLF;
            SQL += " to_char(RTime, 'yyyy-mm-dd') RTime,TO_CHAR(RTime, 'yyyymmddhh24mi') RTime2, sendmsgback, rowid, GBPUSH " + ComNum.VBLF;
            SQL += " from ADMIN.etc_sms " + ComNum.VBLF;
            SQL += " Where sendtime is null " + ComNum.VBLF;
            SQL += " And gubun ='71' " + ComNum.VBLF;
            SQL += " Order by jobdate asc " + ComNum.VBLF;

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


                if (strMsg.Length >= 90)
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
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    TxtEDateSend.Text = cpublic.strSysDate + " " + cpublic.strSysTime;
                }
            }

            rsAllSend.Dispose();
            rsAllSend = null;

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
    }
}
