using System;
using System.Text;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using System.Diagnostics;
using Oracle.DataAccess.Client;

namespace ComPmpaLibB
{
    public class clsCall
    {
        //창구설정조회
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll", CharSet = CharSet.Ansi)]
        public static extern string DLL_DeskInfo(string DeskIP, string StaffID, string StaffName);

        //창구정보조회
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll", CharSet = CharSet.Ansi)]
        public static extern string DLL_DeskSend(int ZoneID, int GroupID, int DeskID);

        //발권
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll")]
        public static extern string DLL_KioskOCSSend(int ZoneID, int GroupID, int iDiv, string sPatInfo, string sPatName, string sDesc);

        //호출
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll", CharSet = CharSet.Ansi)]
        public static extern string DLL_CallNextTicket(int ZoneID, int GroupID, int DeskID, string StaffID, string StaffName);

        //재호출
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll")]
        public static extern string DLL_ReCall(int ZoneID, int GroupID, int DeskID, string StaffID, string StaffName, string TicketID);

        //지정호출
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll", CharSet = CharSet.Ansi)]
        public static extern string DLL_NumCall(int ZoneID, int GroupID, int DeskID, string StaffID, string StaffName, int TicketNO);

        //부재시작
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll")]
        public static extern string DLL_CallerIdleStart(int ZoneID, int GroupID, int DeskID, string StaffID, string StaffName, int IdleID, string IdleRemark);

        //부재종료
        [DllImport("C:\\PSMHEXE\\exenet\\ProjectDllClient.dll")]
        public static extern string DLL_CallerIdleEnd(int ZoneID, int GroupID, int DeskID, string StaffID, string StaffName);

        /// <summary>
        /// ini 파일 읽는 함수
        /// </summary>
        /// <param name="section">가져올 값의 키가 속해있는 섹션이름</param>
        /// <param name="key">가져올 값의 키이름</param>
        /// <param name="def">키의값이 없을경우 기본값(default)</param>
        /// <param name="retVal">가져올 값</param>
        /// <param name="size">가져올 값의 길이</param>
        /// <param name="filePath">읽어올 ini 파일경로</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);



        static string path = @"C:\03.호출\Config.ini";

        DataTable DtCall = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        //int intRowCnt = 0;


        public static string GstrZoneID;
        public static string GstrGroupID;
        public static string GstrDeskID;
        public static string GstrStaffID;
        public static string GstrStaffName;
        public static string GstrCallIP;
        public static string GstrCallUSE = "OK";
        public static string GstrCurTKID;
        public static string GstrCurTKNo;
        public static string GstrRetTKNo;
        public static int GnCallCnt;
        public static string GstrCallMsg;
        public static string GstrCallWait; //대기자수
        public static string GstrDeskInfo; //창구정보


        public static string GstrPano_Call = "";
        public static string GstrJumin1_Call = "";
        public static string GstrJumin2_Call = "";
        public static string GstrJumin1_WaitCall = "";
        public static string GstrJumin2_WaitCall = "";

        public static long RowIndicator_Call;

        public static string GstrSunapFlag_Call;


        public void READ_CALL(PsmhDb pDbCon)
        {
            string rtnVal = "";

        //public static string GstrZoneID;
        //public static string GstrGroupID;
        //public static string GstrDeskID;
        //public static string GstrStaffID;
        //public static string GstrStaffName;
        //public static string GstrCallIP;
        //public static string GstrCallUSE = "OK";
        //public static string GstrCurTKID;
        //public static string GstrCurTKNo;
        //public static string GstrRetTKNo;
        //public static int GnCallCnt;
        //public static string GstrCallMsg;
        //public static string GstrCallWait; //대기자수
        //public static string GstrDeskInfo; //창구정보

            OracleCommand cmd = new OracleCommand();
            OracleDataReader reader = null;
            DataTable dt = new DataTable();

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_PMPA.PR_DESKCALL_PROC";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("P_JOBTYPE", OracleDbType.Varchar2, 9, "1", ParameterDirection.Input);
            cmd.Parameters.Add("P_ZONEID", OracleDbType.Varchar2, 9, "9", ParameterDirection.Input);
            cmd.Parameters.Add("p_GRPNO", OracleDbType.Varchar2, 9, "1", ParameterDirection.Input);
            cmd.Parameters.Add("P_DESKNO", OracleDbType.Varchar2, 9, "1", ParameterDirection.Input);
            cmd.Parameters.Add("p_WORKID", OracleDbType.Varchar2, 9, "20380", ParameterDirection.Input);
            cmd.Parameters.Add("p_WORKNM", OracleDbType.Varchar2, 9, "", ParameterDirection.Input);
            cmd.Parameters.Add("p_TKNOSEQ", OracleDbType.Varchar2, 9, "", ParameterDirection.Input);

            cmd.Parameters.Add("R_ERRYN", OracleDbType.Varchar2, ParameterDirection.Output);
            cmd.Parameters.Add("R_ERRMSG", OracleDbType.Varchar2, ParameterDirection.Output);
            cmd.Parameters.Add("R_TKNO", OracleDbType.Varchar2, ParameterDirection.Output);
            cmd.Parameters.Add("R_TKNOSEQ", OracleDbType.Varchar2, ParameterDirection.Output);
            cmd.Parameters.Add("R_CUSTID", OracleDbType.Varchar2, ParameterDirection.Output);
            cmd.Parameters.Add("R_CUSTNM", OracleDbType.Varchar2, ParameterDirection.Output);

            reader = cmd.ExecuteReader(); 

            dt.Load(reader);
            reader.Dispose();
            reader = null;

            cmd.Dispose();
            cmd = null;

            if (dt == null)
            {
                Cursor.Current = Cursors.Default;
             
            }
            if (dt.Rows.Count > 0)
            {
                GstrCurTKNo = dt.Rows[0]["R_TKNO"].ToString().Trim();
                GstrCurTKID = dt.Rows[0]["R_TKNOSEQ"].ToString().Trim();

                rtnVal = dt.Rows[0]["R_ERRYN"].ToString().Trim();
            }

           

        }

        public void READ_RECALL(PsmhDb pDbCon)
        {
            string rtnVal = "";

            //public static string GstrZoneID;
            //public static string GstrGroupID;
            //public static string GstrDeskID;
            //public static string GstrStaffID;
            //public static string GstrStaffName;
            //public static string GstrCallIP;
            //public static string GstrCallUSE = "OK";
            //public static string GstrCurTKID;
            //public static string GstrCurTKNo;
            //public static string GstrRetTKNo;
            //public static int GnCallCnt;
            //public static string GstrCallMsg;
            //public static string GstrCallWait; //대기자수
            //public static string GstrDeskInfo; //창구정보

            OracleCommand cmd = new OracleCommand();
            OracleDataReader reader = null;
            DataTable dt = new DataTable();

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_PMPA.PR_DESKCALL_PROC";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("P_JOBTYPE", OracleDbType.Varchar2, 9, "2", ParameterDirection.Input);
            cmd.Parameters.Add("P_ZONEID", OracleDbType.Varchar2, 9, "9", ParameterDirection.Input);
            cmd.Parameters.Add("p_GRPNO", OracleDbType.Varchar2, 9, "1", ParameterDirection.Input);
            cmd.Parameters.Add("P_DESKNO", OracleDbType.Varchar2, 9, "1", ParameterDirection.Input);
            cmd.Parameters.Add("p_WORKID", OracleDbType.Varchar2, 9, "20380", ParameterDirection.Input);
            cmd.Parameters.Add("p_WORKNM", OracleDbType.Varchar2, 9, "", ParameterDirection.Input);
            cmd.Parameters.Add("p_TKNOSEQ", OracleDbType.Varchar2, 9, GstrCurTKNo, ParameterDirection.Input);

            cmd.Parameters.Add("R_ERRYN", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add("R_ERRMSG", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add("R_TKNO", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add("R_TKNOSEQ", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add("R_CUSTID", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add("R_CUSTNM", OracleDbType.RefCursor, ParameterDirection.Output);

            reader = cmd.ExecuteReader();

            dt.Load(reader);
            reader.Dispose();
            reader = null;

            cmd.Dispose();
            cmd = null;

            if (dt == null)
            {
                Cursor.Current = Cursors.Default;
             
            }
            if (dt.Rows.Count > 0)
            {
                GstrCurTKNo = dt.Rows[0]["R_TKNO"].ToString().Trim();
                GstrCurTKID = dt.Rows[0]["R_TKNOSEQ"].ToString().Trim();

                rtnVal = dt.Rows[0]["R_ERRYN"].ToString().Trim();
            }

            

        }




        public int Read_Last_TkNo(PsmhDb pDbCon, string ArgZone, string ArgDesk, string ArgID)
        {
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MAX(TKNO) MTKNO FROM SMC.TBTICKETHST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND TKTM      >= TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND ZONEID    = '" + ArgZone + "' ";
            SQL += ComNum.VBLF + "    AND DESKNO    = '" + ArgDesk + "' ";
            SQL += ComNum.VBLF + "    AND WORKID    = '" + ArgID + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtCall, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtCall.Dispose();
                DtCall = null;
                return rtnVal;
            }

            if (DtCall.Rows.Count > 0)
                rtnVal = Convert.ToInt32(VB.Val(DtCall.Rows[0]["MTKNO"].ToString().Trim()));

            DtCall.Dispose();
            DtCall = null;

            return rtnVal;
        }

        public void Display_Call_List(FpSpread o, string ArgList)
        {
            o.Sheets[0].Cells[1, 0].Text = VB.Val(VB.Mid(ArgList, 8, 2)).ToString();
            o.Sheets[0].Cells[1, 1].Text = VB.Val(VB.Mid(ArgList, 10, 4)).ToString();
            o.Sheets[0].Cells[1, 2].Text = VB.Val(VB.Mid(ArgList, 14, 2)).ToString();
            o.Sheets[0].Cells[1, 3].Text = VB.Val(VB.Mid(ArgList, 16, 4)).ToString();
            o.Sheets[0].Cells[1, 4].Text = VB.Val(VB.Mid(ArgList, 20, 2)).ToString();
            o.Sheets[0].Cells[1, 5].Text = VB.Val(VB.Mid(ArgList, 22, 4)).ToString();
            o.Sheets[0].Cells[1, 6].Text = VB.Val(VB.Mid(ArgList, 26, 2)).ToString();
            o.Sheets[0].Cells[1, 7].Text = VB.Val(VB.Mid(ArgList, 28, 4)).ToString();
            o.Sheets[0].Cells[1, 8].Text = VB.Val(VB.Mid(ArgList, 32, 2)).ToString();
            o.Sheets[0].Cells[1, 9].Text = VB.Val(VB.Mid(ArgList, 34, 4)).ToString();
        }

        public void Display_Wait_List(PsmhDb pDbCon, FpSpread o)
        {
            int nCol = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT  B.GRPNM AS GBN,";
            SQL += ComNum.VBLF + "         TO_CHAR(DEFTM, 'HH24:MI:SS') AS  DAYNUM ,";
            SQL += ComNum.VBLF + "         TOTWAITCNT AS TOTCNT,";
            SQL += ComNum.VBLF + "         DESKMNGCNT AS DESKMNGCNT,";
            SQL += ComNum.VBLF + "         WORKPPCNT  AS NOPPCNT,";
            SQL += ComNum.VBLF + "         WAITPPCNT  AS WAITCNT,";
            SQL += ComNum.VBLF + "         SUBSTR(numtodsinterval(WAITTMAVG,'SECOND')  ,12,8) AS WAITTM, ";
            SQL += ComNum.VBLF + "         SUBSTR(numtodsinterval(PROCTMAVG,'SECOND')  ,12,8) AS PROCTM, ";
            SQL += ComNum.VBLF + "         NVL(DESKMNGCNT,0) + NVL(DESKNOCNT,0) AS DESKCNT ";
            SQL += ComNum.VBLF + "    FROM SMC.TBREALINFO A , SMC.TBGRPMST B";
            SQL += ComNum.VBLF + "   Where 1        = 1 ";
            SQL += ComNum.VBLF + "     AND B.ZoneID = A.ZoneID";
            SQL += ComNum.VBLF + "     AND B.GRPNO  = A.GRPNO";
            SQL += ComNum.VBLF + "     AND TO_CHAR(DEFTM, 'YYYYMMDD') = TO_CHAR(SYSDATE,'YYYYMMDD') ";
            SQL += ComNum.VBLF + "     AND (SEQNO) IN (SELECT MAX(SEQNO) ";
            SQL += ComNum.VBLF + "                       FROM SMC.TBREALINFO ";
            SQL += ComNum.VBLF + "                      WHERE ZONEID    = A.ZONEID ";
            SQL += ComNum.VBLF + "                        AND GRPNO     = A.GRPNO ) ";

            if (string.Compare(clsCall.GstrZoneID, "1") >= 0 && string.Compare(clsCall.GstrZoneID, "4") <= 0)
                SQL += ComNum.VBLF + "    AND A.ZONEID IN ('1','2','3','4') ";
            else if (string.Compare(clsCall.GstrZoneID, "6") >= 0 && string.Compare(clsCall.GstrZoneID, "9") <= 0)
                SQL += ComNum.VBLF + "    AND A.ZONEID IN ('6','7','8','9') ";
            else if (string.Compare(clsCall.GstrZoneID, "11") >= 0 && string.Compare(clsCall.GstrZoneID, "14") <= 0)
                SQL += ComNum.VBLF + "    AND A.ZONEID IN ('11','12','13','14') ";

            SQL += ComNum.VBLF + "   ORDER BY B.ZONEID,B.GRPNO ";
            SqlErr = clsDB.GetDataTableEx(ref DtCall, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtCall.Dispose();
                DtCall = null;
                return;
            }

            if (DtCall.Rows.Count > 0)
            {
                for (int i = 0; i < DtCall.Rows.Count; i++)
                {
                    switch (DtCall.Rows[i]["GBN"].ToString().Trim())
                    {
                        case "1층제증명":
                            nCol = 1;
                            break;
                        case "1층퇴원수납":
                            nCol = 3;
                            break;
                        case "1층입원수속":
                            nCol = 5;
                            break;
                        case "1층접수·수납":
                            nCol = 7;
                            break;
                        case "2층제증명":
                            nCol = 1;
                            break;
                        case "2층퇴원수납":
                            nCol = 3;
                            break;
                        case "2층입원수속":
                            nCol = 5;
                            break;
                        case "2층접수·수납":
                            nCol = 7;
                            break;
                        case "3층제증명":
                            nCol = 1;
                            break;
                        case "3층퇴원수납":
                            nCol = 3;
                            break;
                        case "3층입원수속":
                            nCol = 5;
                            break;
                        case "3층접수·수납":
                            nCol = 7;
                            break;
                    }

                    o.Sheets[0].Cells[1, nCol].Text = DtCall.Rows[i]["TOTCNT"].ToString().Trim();
                }
            }

            DtCall.Dispose();
            DtCall = null;

        }

        public void Read_TkNo_PatInfo(PsmhDb pDbCon, string ArgTKNO, string ArgTKID)
        {
          //  string SQL1 = string.Empty;
            GstrJumin1_Call = "";
            GstrJumin2_Call = "";
            GstrPano_Call = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CUSTID,CUSTNM,JUMIN_NO, ";
            SQL += ComNum.VBLF + "        SUBSTR(JUMIN_NO, 1, 6) JUMIN1, SUBSTR(JUMIN_NO, 7) JUMIN2 ";
            SQL += ComNum.VBLF + "   From SMC.TBTICKETHST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND TKTM      >= TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND TKNO      = '" + ArgTKNO + "' "; 
            SQL += ComNum.VBLF + "    AND TKNOSEQ   = '" + ArgTKID + "' ";
           // SQL += ComNum.VBLF + "    AND TKNOSEQ  = TO_number('" + ArgTKID + "')  ";
            SqlErr = clsDB.GetDataTableEx(ref DtCall, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtCall.Dispose();
                DtCall = null;
                return;
            }

            if (DtCall.Rows.Count > 0)
            {
                if (DtCall.Rows[0]["JUMIN_NO"].ToString().Trim() != "")
                {
                    GstrJumin1_Call = DtCall.Rows[0]["JUMIN1"].ToString().Trim();
                    GstrJumin2_Call = clsAES.DeAES(DtCall.Rows[0]["JUMIN2"].ToString().Trim());
                    GstrPano_Call = DtCall.Rows[0]["CUSTID"].ToString().Trim();
                }
            }
            //SQL = VB.Replace(SQL, "'", "@");
            //GstrPano_Call = VB.Replace(GstrPano_Call, "'", "");
            //ArgTKNO = VB.Replace(ArgTKNO, "'", "");
            //ArgTKID = VB.Replace(ArgTKID, "'", "");
            
           // int intRowAffected = 0;
           // SQL1 = "";
           // SQL1 = SQL1 + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.KYOTEST_SQLLOG(SQL,SQL1,SQL2,SQL3,ENTDATE) VALUES ( ";
           // SQL1 = SQL1 + ComNum.VBLF + " '" + SQL + "','"  + GstrPano_Call + "','" + ArgTKNO + "','" + ArgTKID + "', SysDate) ";
           // SqlErr = clsDB.ExecuteNonQueryEx(SQL1, ref intRowAffected, pDbCon);

            DtCall.Dispose();
            DtCall = null;
        }

        public bool Read_Opd_Master_Call(PsmhDb pDbCon, string ArgPtno)
        {
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  Where PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE = TRUNC(SYSDATE) ";
            SqlErr = clsDB.GetDataTableEx(ref DtCall, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                DtCall.Dispose();
                DtCall = null;
                return rtnVal;
            }

            if (DtCall.Rows.Count > 0)
                rtnVal = true;

            DtCall.Dispose();
            DtCall = null;

            return rtnVal;
        }

        public void Load_Call_Info()
        {
            GstrZoneID = "";
            GstrGroupID = "";
            GstrDeskID = "";
            GstrStaffID = clsType.User.IdNumber;
            GstrStaffName = clsType.User.JobName;
            GstrCallIP = "";
            GstrCallUSE = "OK";

            StringBuilder IP = new StringBuilder();
            GetPrivateProfileString("ITEM", "IPAddress1", "", IP, 32, path);
            GstrCallIP = IP.ToString();

            if (GstrCallIP == "")
            {
                GstrCallUSE = "";
                return;
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ZONEID, GRPNO, DESKNO,        --ZONEID,GROUPID,창구번호";
                SQL += ComNum.VBLF + "        WORKID, DEVIP, DESKSTAT,      --근무자ID,창구IP,창구상태(1.부재,2.호출,3.상담)";
                SQL += ComNum.VBLF + "        USEYN, EDITIP, EDITID,        --사용여부,수정자IP,수정자ID";
                SQL += ComNum.VBLF + "        EDITTM                        --수정시간";
                SQL += ComNum.VBLF + "   FROM SMC.TBDESKMST                 --창구마스터 테이블";
                SQL += ComNum.VBLF + "  WHERE DEVIP = '" + GstrCallIP + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtCall, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("TBDESKMST TABLE 조회중 오류발생");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtCall.Rows.Count > 0)
                {
                    GstrZoneID = DtCall.Rows[0]["ZONEID"].ToString().Trim();
                    GstrGroupID = DtCall.Rows[0]["GRPNO"].ToString().Trim();
                    GstrDeskID = DtCall.Rows[0]["DESKNO"].ToString().Trim();

                    GstrCallUSE = "OK";
                }
                else
                {
                    GstrCallUSE = "";
                }

                DtCall.Dispose();
                DtCall = null;
            }
        }

        /// <summary>
        /// 최근 호출한 번호와 창구 Display (최대 5개)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Spd"></param>
        public void Display_Called_No(PsmhDb pDbCon, FpSpread Spd)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            int nREAD = 0;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DESKNO, TKNO                                                     ";
                SQL = SQL + ComNum.VBLF + "   FROM SMC.TBTICKETHST                                                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                            ";
                SQL = SQL + ComNum.VBLF + "    AND TKTM >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "    AND STANBYTM IS NOT NULL                                             ";
                SQL = SQL + ComNum.VBLF + "    AND ZONEID = '" + clsCall.GstrZoneID + "'                            ";
                SQL = SQL + ComNum.VBLF + "  ORDER By STANBYTM DESC                                                 ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD > 0)
                {
                    if (nREAD <= 1)
                    {
                        Spd.ActiveSheet.Cells[1, 0].Text = Dt.Rows[0]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 1].Text = Dt.Rows[0]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 2].Text = "";
                        Spd.ActiveSheet.Cells[1, 3].Text = "";
                        Spd.ActiveSheet.Cells[1, 4].Text = "";
                        Spd.ActiveSheet.Cells[1, 5].Text = "";
                        Spd.ActiveSheet.Cells[1, 6].Text = "";
                        Spd.ActiveSheet.Cells[1, 7].Text = "";
                        Spd.ActiveSheet.Cells[1, 8].Text = "";
                        Spd.ActiveSheet.Cells[1, 9].Text = "";
                    }
                    else if (nREAD <= 2)
                    {
                        Spd.ActiveSheet.Cells[1, 0].Text = Dt.Rows[0]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 1].Text = Dt.Rows[0]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 2].Text = Dt.Rows[1]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 3].Text = Dt.Rows[1]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 4].Text = "";
                        Spd.ActiveSheet.Cells[1, 5].Text = "";
                        Spd.ActiveSheet.Cells[1, 6].Text = "";
                        Spd.ActiveSheet.Cells[1, 7].Text = "";
                        Spd.ActiveSheet.Cells[1, 8].Text = "";
                        Spd.ActiveSheet.Cells[1, 9].Text = "";
                    }
                    else if (nREAD <= 3)
                    {
                        Spd.ActiveSheet.Cells[1, 0].Text = Dt.Rows[0]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 1].Text = Dt.Rows[0]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 2].Text = Dt.Rows[1]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 3].Text = Dt.Rows[1]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 4].Text = Dt.Rows[2]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 5].Text = Dt.Rows[2]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 6].Text = "";
                        Spd.ActiveSheet.Cells[1, 7].Text = "";
                        Spd.ActiveSheet.Cells[1, 8].Text = "";
                        Spd.ActiveSheet.Cells[1, 9].Text = "";
                    }
                    else if (nREAD <= 4)
                    {
                        Spd.ActiveSheet.Cells[1, 0].Text = Dt.Rows[0]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 1].Text = Dt.Rows[0]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 2].Text = Dt.Rows[1]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 3].Text = Dt.Rows[1]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 4].Text = Dt.Rows[2]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 5].Text = Dt.Rows[2]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 6].Text = Dt.Rows[3]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 7].Text = Dt.Rows[3]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 8].Text = "";
                        Spd.ActiveSheet.Cells[1, 9].Text = "";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[1, 0].Text = Dt.Rows[0]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 1].Text = Dt.Rows[0]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 2].Text = Dt.Rows[1]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 3].Text = Dt.Rows[1]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 4].Text = Dt.Rows[2]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 5].Text = Dt.Rows[2]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 6].Text = Dt.Rows[3]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 7].Text = Dt.Rows[3]["TKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 8].Text = Dt.Rows[4]["DESKNO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[1, 9].Text = Dt.Rows[4]["TKNO"].ToString().Trim();
                    }
                    
                }
                else
                {
                    Spd.ActiveSheet.Cells[1, 0].Text = "";
                    Spd.ActiveSheet.Cells[1, 1].Text = "";
                    Spd.ActiveSheet.Cells[1, 2].Text = "";
                    Spd.ActiveSheet.Cells[1, 3].Text = "";
                    Spd.ActiveSheet.Cells[1, 4].Text = "";
                    Spd.ActiveSheet.Cells[1, 5].Text = "";
                    Spd.ActiveSheet.Cells[1, 6].Text = "";
                    Spd.ActiveSheet.Cells[1, 7].Text = "";
                    Spd.ActiveSheet.Cells[1, 8].Text = "";
                    Spd.ActiveSheet.Cells[1, 9].Text = "";
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }
        
        /// <summary>
        /// 대기순번 대기인원 Count
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public string Display_Wait_Count(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            string rtnVal = "0";

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT COUNT(*) CNT                                                     ";
                SQL = SQL + ComNum.VBLF + "   FROM SMC.TBTICKETHST                                                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                            ";
                SQL = SQL + ComNum.VBLF + "    AND TKTM >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "    AND STANBYTM IS NULL                                             ";
                SQL = SQL + ComNum.VBLF + "    AND ZONEID = '" + clsCall.GstrZoneID + "'                            ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Dt.Rows[0]["CNT"].ToString().Trim();
                }
                
                Dt.Dispose();
                Dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return rtnVal;
            }
        }


        /// <summary>
        /// 재호출
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Spd"></param>
        public string Display_ReCalled_No(PsmhDb pDbCon, string ArgNum)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            int nREAD = 0;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TKNO                                                     ";
                SQL = SQL + ComNum.VBLF + "   FROM SMC.TBTICKETHST                                                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                            ";
                SQL = SQL + ComNum.VBLF + "    AND TKTM >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "    AND STANBYTM IS NOT NULL                                             ";
                SQL = SQL + ComNum.VBLF + "    AND ZONEID = '" + clsCall.GstrZoneID + "'                            ";
                SQL = SQL + ComNum.VBLF + "    AND GRPNO = '" + clsCall.GstrGroupID + "'                            ";
                SQL = SQL + ComNum.VBLF + "    AND DESKNO = '" + clsCall.GstrDeskID + "'                            ";
                SQL = SQL + ComNum.VBLF + "    AND TKNO = '" + ArgNum + "'  ";

                SQL = SQL + ComNum.VBLF + "  ORDER By STANBYTM DESC                                                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD > 0)
                {
                    rtnVal = Dt.Rows[0]["TKNO"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return rtnVal;
            }
        }

    }
}
