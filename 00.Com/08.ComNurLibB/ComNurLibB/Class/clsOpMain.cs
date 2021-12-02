using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using ComDbB;
using ComBase;
using Oracle.DataAccess.Client;

namespace ComNurLibB
{
    public class clsOpMain
    {
        //int rowcounter;

        //DataTable dt = null;
        //string SQL = "";    //Query문
        //string SqlErr = ""; //에러문 받는 변수
        //string strValue;

        //Declare Function GetImeMode Lib "han.dll" (ByVal hwnd As Integer) As Integer
        //Declare Sub SetImeMode Lib "han.dll" (ByVal hwnd As Integer, ByVal bHangul As Integer)

        [DllImport("han.dll")]
        public static extern int GetImeMode(int hwnd);

        [DllImport("han.dll")]
        public static extern void SetImeMode(long hWnd, int bHangul);

        public const string BUSE_SUSUL = "033102";      //'수술실
        public const string BUSE_MARCH = "033103";      //'마취통증의학과
        public const string BUSE_ANGIO = "100570";      //'ANGIO
        public const string BUSE_NSBLOCK = "011106";    //'NS

        public static string GstrBuCode = "";
        public static string GstrSlipSend = "";//'원무Slip에 전송여부
        public static string GstrTime = "";
        public static string GstrOpDate = "";
        public static string GstrSlipName = "";
        public static string GstrSlipOK = "";

        public string[] GstrHosp = new string[4];// 의료급여환자의 선택의료기관

        public static string[] GstrPosition = new string[6];    //'수술 Positions
        public static string[] GstrAnes = new string[7];        //'마취방법
        public static string[] GstrOpDept = new string[10];     //'수술하는 진료과

        public static string GstrGbNight = "";                       //심야가산 여부 check

        public static string GstrTestPano = "";                      //'test 용 입니다.
        public static string GstrBuseGbn = "";
        public static string GstrRetJep = "";
        public static string gsWard = "";

        public static string GstrFM_전용여부;                   //'가정의학과 전용

        //수술환자 Master
        public struct Table_ORAN_Master
        {
            public int WRTNO;
            public string OpDate;
            public string Pano;
            public string sName;
            public string Jumin1;
            public string Jumin2;
            public string Bi;
            public string DeptCode;
            public string DrCode; 
            public string AssistSabun; //FstrAssist_Sabun 대신 사용
            public string OpStaff;  //'수술의사 사번
            public string IpdOpd;
            public string WardCode;
            public string RoomCode;
            public int Age;
            public string Sex;
            public string OpTimeFrom;
            public string OpTimeTo;
            public string FLAG;
            public string GbNight;
            public string Oproom;
            public string DIAGNOSIS;
            public string Optitle;
            public string OpRemark;
            public string SpadeWork;
            public string PtRemark;
            public string OpDoct1;
            public string OpDoct2;
            public string OpNurse;
            public int OpCode;
            public string OpPosition;
            public string AnGbn;
            public string AnDoct1;
            public int ANTIME;
            public string AnNurse;
            public string OpBun;
            public string OpRe;
            public string OpCancel;
            public string OpStime;
            public string CNurse;
            public string KIDNEY;
            public string JDATE;
            public string CystoResult;
            public string OpErr;    // '적신호사건
            public string GbAngio;  // 'Angio검사 여부
            public string GbDay;    // '일일수술센타 환자 여부(Y/N)
            public string PCA;
            public string ROWID;
            public string ASA;      //2019-09-01    ASA가산
        }
        public static Table_ORAN_Master TORM;

        public static void ClearTORM()
        {
            TORM.OpDate = "";
            TORM.Pano = "";
            TORM.sName = "";
            TORM.Bi = "";
            TORM.DeptCode = "";
            TORM.DrCode = "";
            TORM.AssistSabun = "";
            TORM.IpdOpd = "";
            TORM.WardCode = "";
            TORM.RoomCode = "";
            TORM.Age = 0;
            TORM.Sex = "";
            TORM.OpTimeFrom = "";
            TORM.OpTimeTo = "";
            TORM.FLAG = "";
            TORM.GbNight = "";
            TORM.Oproom = "";
            TORM.DIAGNOSIS = "";
            TORM.Optitle = "";
            TORM.OpDoct1 = "";
            TORM.OpDoct2 = "";
            TORM.OpNurse = "";
            TORM.OpCode = 0;
            TORM.OpPosition = "";
            TORM.AnGbn = "";
            TORM.AnDoct1 = "";
            TORM.ANTIME = 0;
            TORM.AnNurse = "";
            TORM.ROWID = "";
            TORM.WRTNO = 0;
            TORM.GbAngio = "N";
            TORM.GbDay = "N";
        }
        
        public static void ORAN_Master_READ(ref Table_ORAN_Master argTORM, string ArgPano, string argDATE, string ArgDeptCode = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID,Sname,Bi,DeptCode,DrCode,IpdOpd,             ";
                SQL = SQL + ComNum.VBLF + "        WardCode,RoomCode,Age,Sex,OptimeFrom,              ";
                SQL = SQL + ComNum.VBLF + "        OpTimeTo,FLAG,GbNight,OpRoom,DiagNosis,            ";
                SQL = SQL + ComNum.VBLF + "        OpTitle,OpDoct1,OpDoct2,OpNurse,OpCode,            ";
                SQL = SQL + ComNum.VBLF + "        OpPosition,AnGbn,AnDoct1,AnTime,AnNurse,           ";
                SQL = SQL + ComNum.VBLF + "        Opbun, OpRe, OpCancel, OpSTime, CNurse,            ";
                SQL = SQL + ComNum.VBLF + "        Pano,TO_CHAR(OpDate,'YYYY-MM-DD') OpDate,          ";
                SQL = SQL + ComNum.VBLF + "        OpRemark,SpadeWork,PtRemark,WRTNO,GbAngio,GbDay    ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_Master                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + ArgPano + "'                           ";
                SQL = SQL + ComNum.VBLF + "    AND OpDate = TO_DATE('" + argDATE + "','YYYY-MM-DD')   ";
                if (ArgDeptCode != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND DeptCode='" + ArgDeptCode + "' ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    argTORM.OpDate = "";
                    argTORM.Pano = "";
                    argTORM.sName = "";
                    argTORM.Bi = "";
                    argTORM.DeptCode = "";
                    argTORM.DrCode = "";
                    argTORM.IpdOpd = "";
                    argTORM.WardCode = "";
                    argTORM.RoomCode = "";
                    argTORM.Age = 0;
                    argTORM.Sex = "";
                    argTORM.OpTimeFrom = "";
                    argTORM.OpTimeTo = "";
                    argTORM.FLAG = "";
                    argTORM.GbNight = "";
                    argTORM.Oproom = "";
                    argTORM.DIAGNOSIS = "";
                    argTORM.Optitle = "";
                    argTORM.OpDoct1 = "";
                    argTORM.OpDoct2 = "";
                    argTORM.OpNurse = "";
                    argTORM.OpCode = 0;
                    argTORM.OpPosition = "";
                    argTORM.AnGbn = "";
                    argTORM.AnDoct1 = "";
                    argTORM.ANTIME = 0;
                    argTORM.AnNurse = "";
                    argTORM.ROWID = "";
                    argTORM.WRTNO = 0;
                    argTORM.GbAngio = "N";
                    argTORM.GbDay = "N";

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                else
                {
                    argTORM.OpDate = argDATE;
                    argTORM.Pano = ArgPano;
                    argTORM.WRTNO = Convert.ToInt32(VB.Val(dt.Rows[0]["WRTNO"].ToString().Trim()));
                    argTORM.sName = dt.Rows[0]["Sname"].ToString().Trim();
                    argTORM.Bi = dt.Rows[0]["Bi"].ToString().Trim();
                    argTORM.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                    argTORM.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                    argTORM.IpdOpd = dt.Rows[0]["IpdOpd"].ToString().Trim();
                    argTORM.WardCode = dt.Rows[0]["WardCode"].ToString().Trim();
                    argTORM.RoomCode = dt.Rows[0]["RoomCode"].ToString().Trim();
                    argTORM.Age = Convert.ToInt32(dt.Rows[0]["Age"].ToString().Trim());
                    argTORM.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                    argTORM.OpTimeFrom = dt.Rows[0]["OpTimeFrom"].ToString().Trim();
                    argTORM.OpTimeTo = dt.Rows[0]["OpTimeTo"].ToString().Trim();
                    argTORM.FLAG = dt.Rows[0]["FLAG"].ToString().Trim();
                    argTORM.GbNight = dt.Rows[0]["GbNight"].ToString().Trim();
                    argTORM.Oproom = dt.Rows[0]["OpRoom"].ToString().Trim();
                    argTORM.DIAGNOSIS = dt.Rows[0]["DiagNosis"].ToString().Trim();
                    argTORM.Optitle = dt.Rows[0]["OpTitle"].ToString().Trim();
                    argTORM.OpDoct1 = dt.Rows[0]["OpDoct1"].ToString().Trim();
                    argTORM.OpDoct2 = dt.Rows[0]["OpDoct2"].ToString().Trim();
                    argTORM.OpNurse = dt.Rows[0]["OpNurse"].ToString().Trim();
                    argTORM.OpCode = Convert.ToInt32(dt.Rows[0]["OpCode"].ToString().Trim());
                    argTORM.OpPosition = dt.Rows[0]["OpPosition"].ToString().Trim();
                    argTORM.AnGbn = dt.Rows[0]["AnGbn"].ToString().Trim();
                    argTORM.AnDoct1 = dt.Rows[0]["AnDoct1"].ToString().Trim();
                    argTORM.ANTIME = Convert.ToInt32(dt.Rows[0]["AnTime"].ToString().Trim());
                    argTORM.AnNurse = dt.Rows[0]["AnNurse"].ToString().Trim();
                    argTORM.GbAngio = dt.Rows[0]["GbAngio"].ToString().Trim();
                    argTORM.GbDay = dt.Rows[0]["GbDay"].ToString().Trim();
                    argTORM.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

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

        public static void ORAN_Master_READ2(PsmhDb pDbCon, ref Table_ORAN_Master argTORM, double argWRTNO)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID,WRTNO,Sname,Bi,DeptCode,DrCode,IpdOpd,       ";
                SQL = SQL + ComNum.VBLF + "        WardCode,RoomCode,Age,Sex,OptimeFrom,              ";
                SQL = SQL + ComNum.VBLF + "        OpTimeTo,FLAG,GbNight,OpRoom,DiagNosis,            ";
                SQL = SQL + ComNum.VBLF + "        OpTitle,OpDoct1,OpDoct2,OpNurse,OpCode,            ";
                SQL = SQL + ComNum.VBLF + "        OpPosition,AnGbn,AnDoct1,AnTime,AnNurse,           ";
                SQL = SQL + ComNum.VBLF + "        Opbun, OpRe, OpCancel, OpSTime, CNurse,            ";
                SQL = SQL + ComNum.VBLF + "        Pano,TO_CHAR(OpDate,'YYYY-MM-DD') OpDate,          ";
                SQL = SQL + ComNum.VBLF + "        OpRemark,SpadeWork,PtRemark,GbAngio,GbDay          ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_Master                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE WRTNO = " + argWRTNO + "                           ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    argTORM.OpDate = "";
                    argTORM.Pano = "";
                    argTORM.sName = "";
                    argTORM.Bi = "";
                    argTORM.DeptCode = "";
                    argTORM.DrCode = "";
                    argTORM.IpdOpd = "";
                    argTORM.WardCode = "";
                    argTORM.RoomCode = "";
                    argTORM.Age = 0;
                    argTORM.Sex = "";
                    argTORM.OpTimeFrom = "";
                    argTORM.OpTimeTo = "";
                    argTORM.FLAG = "";
                    argTORM.GbNight = "";
                    argTORM.Oproom = "";
                    argTORM.DIAGNOSIS = "";
                    argTORM.Optitle = "";
                    argTORM.OpDoct1 = "";
                    argTORM.OpDoct2 = "";
                    argTORM.OpNurse = "";
                    argTORM.OpCode = 0;
                    argTORM.OpPosition = "";
                    argTORM.AnGbn = "";
                    argTORM.AnDoct1 = "";
                    argTORM.ANTIME = 0;
                    argTORM.AnNurse = "";
                    argTORM.ROWID = "";
                    argTORM.OpBun = "";
                    argTORM.OpRe = "";
                    argTORM.OpCancel = "";
                    argTORM.OpStime = "";
                    argTORM.CNurse = "";
                    argTORM.PtRemark = "";
                    argTORM.OpRemark = "";
                    argTORM.SpadeWork = "";
                    argTORM.WRTNO = 0;
                    argTORM.GbAngio = "";
                    argTORM.GbDay = "";
                }
                else
                {
                    argTORM.OpDate = dt.Rows[0]["OpDate"].ToString().Trim();
                    argTORM.Pano = dt.Rows[0]["Pano"].ToString().Trim();
                    argTORM.sName = dt.Rows[0]["Sname"].ToString().Trim();
                    argTORM.Bi = dt.Rows[0]["Bi"].ToString().Trim();
                    argTORM.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                    argTORM.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                    argTORM.IpdOpd = dt.Rows[0]["IpdOpd"].ToString().Trim();
                    argTORM.WardCode = dt.Rows[0]["WardCode"].ToString().Trim();
                    argTORM.RoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
                    argTORM.Age = Convert.ToInt32(dt.Rows[i]["Age"].ToString().Trim());
                    argTORM.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                    argTORM.OpTimeFrom = dt.Rows[0]["OpTimeFrom"].ToString().Trim();
                    argTORM.OpTimeTo = dt.Rows[0]["OpTimeTo"].ToString().Trim();
                    argTORM.FLAG = dt.Rows[0]["FLAG"].ToString().Trim();
                    argTORM.GbNight = dt.Rows[0]["GbNight"].ToString().Trim();
                    argTORM.Oproom = dt.Rows[0]["OpRoom"].ToString().Trim();
                    argTORM.DIAGNOSIS = dt.Rows[0]["DiagNosis"].ToString().Trim();
                    argTORM.Optitle = dt.Rows[0]["OpTitle"].ToString().Trim();
                    argTORM.OpRemark = dt.Rows[0]["OpRemark"].ToString().Trim();
                    argTORM.SpadeWork = dt.Rows[0]["SpadeWork"].ToString().Trim();
                    argTORM.PtRemark = dt.Rows[0]["PtRemark"].ToString().Trim();
                    argTORM.OpDoct1 = dt.Rows[0]["OpDoct1"].ToString().Trim();
                    argTORM.OpDoct2 = dt.Rows[0]["OpDoct2"].ToString().Trim();
                    argTORM.OpNurse = dt.Rows[0]["OpNurse"].ToString().Trim();
                    argTORM.OpCode = Convert.ToInt32(dt.Rows[i]["OpCode"].ToString().Trim());
                    argTORM.OpPosition = dt.Rows[0]["OpPosition"].ToString().Trim();
                    argTORM.AnGbn = dt.Rows[0]["AnGbn"].ToString().Trim();
                    argTORM.AnDoct1 = dt.Rows[0]["AnDoct1"].ToString().Trim();
                    argTORM.ANTIME = Convert.ToInt32(dt.Rows[i]["AnTime"].ToString().Trim());
                    argTORM.AnNurse = dt.Rows[0]["AnNurse"].ToString().Trim();
                    argTORM.OpBun = dt.Rows[0]["OpBun"].ToString().Trim();
                    argTORM.OpRe = dt.Rows[0]["OpRe"].ToString().Trim();
                    argTORM.OpCancel = dt.Rows[0]["OpCancel"].ToString().Trim();
                    argTORM.OpStime = dt.Rows[0]["OPSTime"].ToString().Trim();
                    argTORM.CNurse = dt.Rows[0]["CNurse"].ToString().Trim();
                    argTORM.GbAngio = dt.Rows[0]["GbAngio"].ToString().Trim();
                    argTORM.GbDay = dt.Rows[0]["GbDay"].ToString().Trim();
                    argTORM.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

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



        /// <summary>
        /// 수술환자 명단 ORAN_MASTER NEW WRTNO Read
        /// OPMAIN1.bas 에서...
        /// </summary>
        /// <returns></returns>
        public static long READ_New_JepsuNo(PsmhDb pDbCon)
        {
            int nValue;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nValue = 0;

            try
            {
                SQL = "";
                SQL += " SELECT  " + ComNum.DB_PMPA + "SEQ_OPRMASTER.NEXTVAL cWRTNO     \r";
                SQL += "   FROM DUAL                                                    \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return nValue;
                }
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("신규번호 부여시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dt.Dispose();
                    dt = null;
                    return nValue;
                }
                else if (dt.Rows.Count > 0)
                {
                    nValue = Convert.ToInt32(dt.Rows[0]["cWRTNO"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                return nValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return nValue;
            }
        }

        /// <summary>
        /// 처방 수술환자 선택 화면에서 사용(외래 수술인데 재원중인지 확인)
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="argOpDate"></param>
        /// <returns></returns>
        public static bool OPD_SUSUL_Check(PsmhDb pDbCon, string ArgPano, string argOpDate)
        {
            bool blnValue;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            blnValue = true;

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(InDate,'YYYY-MM-DD') InDate,TO_CHAR(OutDate,'YYYY-MM-DD') OutDate   \r";
                SQL += "   FROM KOSMOS_PMPA.IPD_NEW_MASTER                                                  \r";
                SQL += "  WHERE PANO = '" + ArgPano + "'                                                    \r";
                SQL += "    AND TO_CHAR(InDate,'YYYY-MM-DD') <= '" + argOpDate + "'                         \r";
                SQL += "    AND (OutDate IS NULL OR OutDate  >= TO_DATE('" + argOpDate + "','YYYY-MM-DD'))  \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return true;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }
                else if (dt.Rows.Count > 0)
                {
                    blnValue = false;
                }

                dt.Dispose();
                dt = null;

                return blnValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return blnValue;
            }
        }

        public static string TIME_CHECK(string Arg1)
        {
            string strValue = "";

            double nTT = 0;
            double nMM = 0;

            strValue = "OK";

            if (Arg1.Length != 5)
            {
                strValue = "NO";
                return strValue;
            }
            if (Arg1.Substring(2, 1) != ":")
            {
                strValue = "NO";
                return strValue;
            }

            nTT = VB.Val(Arg1.Substring(0, 2));
            nMM = VB.Val(Arg1.Substring(3, 2));

            if (nTT < 0 || nTT > 23)
            {
                strValue = "NO";
            }
            if (nMM < 0 || nMM > 59)
            {
                strValue = "NO";
            }
            return strValue;
        }

        /// <summary>
        /// 사원번호로 현재 근무중인 부서코드를 읽기
        /// </summary>
        /// <param name="argSabun"></param>
        /// <returns></returns>
        public static string Sabun_To_BuseCode(PsmhDb pDbCon, int argSabun)
        {
            string strValue = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strValue = "";

            try
            {
                SQL = "";
                SQL += " SELECT BUSE FROM KOSMOS_ADM.INSA_MST                                   \r";
                if (argSabun > 99999)
                {
                    SQL += "  WHERE SABUN = '" + string.Format("{0:000000}", argSabun) + "'     \r";
                }
                else
                {
                    SQL += "  WHERE SABUN = '" + string.Format("{0:00000}", argSabun) + "'      \r";
                }
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }
                else if (dt.Rows.Count > 0)
                {
                    strValue = dt.Rows[0]["BUSE"].ToString().Trim() == "011105" ? "011106" : dt.Rows[0]["BUSE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strValue;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strValue;
            }
        }

        /// <summary>
        /// Create : 2018-01-12
        /// Author : 안정수
        /// <seealso cref="OpMain.bas : READ_JEPNAME"/>        
        /// OpMain.bas 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        public static string READ_JEPNAME(PsmhDb pDbCon, string ArgCode)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgCode.Trim() == "")
            {
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  JepName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ORD_JEP";
            SQL += ComNum.VBLF + "WHERE JepCode = '" + ArgCode.Trim() + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count == 0)
            {
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["JepName"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return rtnVal;

        }

        public static void TORM_AgeSex_SET(PsmhDb pDbCon, ref Table_ORAN_Master argTORM, string argPano, string argOpDate)
        {
            string strJumin = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = "SELECT SNAME,JUMIN1,JUMIN2,SEX FROM BAS_PATIENT ";
            SQL = SQL + " WHERE PANO='" + argPano.Trim() + "' ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim();
                argTORM.sName = dt.Rows[0]["SNAME"].ToString().Trim();
                argTORM.Age = ComFunc.AgeCalc(pDbCon, strJumin.Trim());
                argTORM.Sex = dt.Rows[0]["Sex"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }



        public static void SetGstrBuCode(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT BUSE      ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_ADM.INSA_MST     ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN = '" + clsType.User.Sabun + "'     ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    GstrBuCode = dt.Rows[0]["BUSE"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        public static string Read_Pano_SELECT_MST_OP(PsmhDb pDbCon, string ArgPano, string ArgIO, string ArgDrCode, string ArgBDate, double ArgIpdNo)
        {
            DataTable dt = null;
            string SQL = "";
            string strBDate;
            double nIpdNo;
            string strValue = "";
            string SqlErr = "";

            strBDate = ArgBDate;
            nIpdNo = ArgIpdNo;
            strValue = "";

            if (ArgIO == "I")
            {
                if (READ_IPD_NEW_MASTER_INDATE_CHK(clsDB.DbCon, nIpdNo) != "OK")
                    return strValue;
            }

            try
            {
                SQL = "";
                SQL += " SELECT Pano, SETC6                                         \r";
                SQL += "      , TO_CHAR(SDATE,'YYYY-MM-DD') SDATE                   \r";
                SQL += "      , TO_CHAR(EDATE,'YYYY-MM-DD') EDATE                   \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_SELECT_MST                          \r";
                SQL += "  WHERE PANO = '" + ArgPano + "'                            \r";
                SQL += "    AND DRCODE = '" + ArgDrCode + "'                        \r";
                SQL += "    AND GUBUN = '" + ArgIO + "'                             \r";
                SQL += "    AND SDate <= TO_DATE('" + strBDate + "','YYYY-MM-DD')   \r";
                SQL += "    AND (DelDate IS NULL OR DelDate ='')                    \r";
                SQL += "    AND SET6 ='Y'                                           \r";
                SQL += "  ORDER BY SDate DESC                                       \r";
                clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["EDATE"].ToString().Trim() == "")
                    {
                        strValue = "OK" + dt.Rows[0]["SETC6"].ToString().Trim();
                    }
                    else if (DateTime.Parse(strBDate) <= DateTime.Parse(dt.Rows[0]["EDATE"].ToString().Trim()))
                    {
                        strValue = "OK" + dt.Rows[0]["SETC6"].ToString().Trim();
                    }
                    else
                    {
                        strValue = "";
                    }
                }
                dt.Dispose();
                dt = null;
                return strValue;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        public static string READ_IPD_NEW_MASTER_INDATE_CHK(PsmhDb pDbCon, double ArgIpdNo)
        {
            DataTable dt = null;
            string SqlErr = "";
            string SQL = "";
            string strValue = "";

            try
            {
                SQL = "";
                SQL += "SELECT Pano,IPDNO                                               \r";
                SQL += "  FROM KOSMOS_PMPA.IPD_NEW_MASTER                               \r";
                SQL += " WHERE IPDNO = " + ArgIpdNo + "                                 \r";
                SQL += "   AND INDATE >= TO_DATE('2011-06-01 00:01','YYYY-MM-DD HH24:MI')\r";
                SQL += "   AND GBSTS NOT IN ('9')                                       \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    strValue = "";
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {
                    strValue = "";
                    dt.Dispose();
                    dt = null;
                    return strValue;
                }

                if (dt.Rows.Count > 0)
                {
                    strValue = "OK";

                }
                dt.Dispose();
                dt = null;
                return strValue;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }

        public static string Chan_String(string ArgString, string argChStr1, string argChStr2, string argChReturn)
        {
            //'대상문자열(argString$)에서
            //'첫번째 값(argChStr1$)과 두번째 값(argChStr2$)을
            //'바뀔문자(argChReturn$)로 바꾸어서 문자열을 형성한다.
            //'두번째 문자열이 없으면 ""(공백)으로 넣으면 두번째 값은 체크하지 않는다.
            //'공백도 사용 가능하며 바꿀문자와 바뀔 문자가 틀려야 한다.
            //'2000-04-19 손동현

            int X = 0;
            int Y = 0;
            //int z = 0;

            for (X = 1; X <= 10000; X++)
            {
                Y = VB.InStr(ArgString, argChStr1);

                if (Y > 0)
                {
                    ArgString = VB.Left(ArgString, Y - 1) + argChReturn + VB.Mid((ArgString), Y + 1, VB.Len(ArgString) - Y + 1);
                }
                else if (Y == 0)
                {
                    if (argChStr2 != "")
                    {
                        Y = VB.InStr(ArgString, argChStr2);

                        if (Y > 0)
                        {
                            ArgString = VB.Left(ArgString, Y - 1) + argChReturn + VB.Mid(ArgString, Y + 1, VB.Len(ArgString) - Y + 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (argChStr2 == "")
                    {
                        break;
                    }
                }
            }

            return ArgString;
        }

        /// <summary>
        /// 수술실 욕창
        /// 김효성
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argDATE"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgAge"></param>
        /// <param name="argWARD"></param>
        /// <param name="ArgDate2"></param>
        /// <returns></returns>
        public static string READ_WARNING_BRADEN(PsmhDb pDbCon, string argPTNO, string argDATE, string ArgIpdNo, string ArgAge, string argWARD, string ArgDate2 = "")
        {
            DataTable dt = null;
            string SqlErr = "";
            string SQL = "";
            string strValue = "";
            string strBraden = "";
            string strGubun = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ArgIpdNo == "")
                {
                    SQL = "";
                    SQL = "SELECT IPDNO, WARDCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return strGubun;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ArgIpdNo = dt.Rows[0]["IPDNO"].ToString().Trim();
                        argWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }



                if (VB.IsNumeric(ArgAge))
                {
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return strGubun;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ArgAge = dt.Rows[0]["AGE"].ToString().Trim();
                    }
                    else
                    {
                        ArgAge = "";
                    }

                    dt.Dispose();
                    dt = null;

                    if (ArgAge == "")
                    {
                        return ArgAge;
                    }

                    if (argWARD == "NR" || argWARD == "ND" || argWARD == "IQ")
                    {
                        strGubun = "신생아";
                    }
                    else if (string.Compare(ArgAge, "5") < 0)
                        strGubun = "소아";
                    else
                    {
                        strGubun = "";
                    }

                    if (strGubun == "")
                    {
                        SQL = "";
                        SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE A";
                        SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";
                        if (ArgDate2 != "")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        }

                        SQL = SQL + ComNum.VBLF + "     AND A.TOTAL <= 18";
                        SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
                        SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                        SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_BRADEN_SCALE";
                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                        SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(dt.Rows[0]["AGR"].ToString().Trim()) >= 60 && Convert.ToInt32(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18 ||
                                    Convert.ToInt32(dt.Rows[0]["AGR"].ToString().Trim()) < 60 && Convert.ToInt32(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16)
                                {
                                    strBraden = "OK";
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                    else if (strGubun == "소아")
                    {
                        SQL = "";
                        SQL = "SELECT TOTAL ";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_CHILD ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "    AND PANO = '" + argPTNO + "' ";
                        if (ArgDate2 != "")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        }

                        SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 16)
                            {
                                //strOK = "OK";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    else if (strGubun == "신생아")
                    {
                        SQL = "";
                        SQL = "SELECT TOTAL ";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_BABY ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                        if (ArgDate2 != "")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 20)
                            {

                                strBraden = "OK";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }

                    if (strBraden == "")
                    {
                        SQL = "";
                        SQL = " SELECT *";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_WARNING ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ( ";
                        SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
                        SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                        SQL = SQL + ComNum.VBLF + "      )";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strBraden = "OK";
                        }

                        dt.Dispose();
                        dt = null;

                    }
                }

                if (strBraden == "OK")
                {
                    strValue = "욕창위험";
                }

                return strValue;
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
                return strValue;
            }


        }

        public static string READ_ALLERGY_OP(PsmhDb pDbCon, string ArgPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT REMARK ,B.NAME INF, A.ENTDATE ,C.KORNAME SANAME  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.ETC_ALLERGY_MST A,KOSMOS_PMPA.BAS_BCODE B, KOSMOS_ADM.INSA_MST C  ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN='환자정보_알러지종류'";
                SQL = SQL + ComNum.VBLF + "   AND A.CODE=B.CODE    ";
                SQL = SQL + ComNum.VBLF + "  AND A.SABUN=C.SABUN";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
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
                return rtnVal;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <returns></returns>
        public static void READ_ALLERGY_POPUP(PsmhDb pDbCon, string ArgPano)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT REMARK ,B.NAME INF, A.ENTDATE ,C.KORNAME SANAME  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.ETC_ALLERGY_MST A,KOSMOS_PMPA.BAS_BCODE B, KOSMOS_ADM.INSA_MST C  ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN='환자정보_알러지종류'";
                SQL = SQL + ComNum.VBLF + "   AND A.CODE=B.CODE    ";
                SQL = SQL + ComNum.VBLF + "  AND A.SABUN=C.SABUN(+)";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                        rtnVal = " " + dt.Rows[i]["REMARK"].ToString().Trim() + dt.Rows[i]["INF"].ToString().Trim() + ComNum.VBLF;
                    }
                }
                else
                {

                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                ComFunc.MsgBox("================================================================== " + ComNum.VBLF + ComNum.VBLF +
                                     " ▷환자의 알러지 정보가 있습니다." + ComNum.VBLF + ComNum.VBLF +
                                    "================================================================== " + ComNum.VBLF + ComNum.VBLF +
                                     rtnVal + ComNum.VBLF +
                                    "================================================================== " + ComNum.VBLF
                                    , "환자의 알러지 정보");

                return;
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
                return;
            }
        }

        public static void SetcboOpCode(ComboBox cboOpCode, string strDeptCode, int intOpCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            int i = 0;
            int intCBO = -1;

            //'수술분류 ComboBox Set
            SQL = "SELECT BUN,BUNNAME FROM KOSMOS_PMPA.ORAN_OPBUN ";
            SQL = SQL + ComNum.VBLF + "WHERE DEPT='" + strDeptCode + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY BUN ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cboOpCode.Items.Clear();

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboOpCode.Items.Add(VB.Val(dt.Rows[i]["BUN"].ToString().Trim()).ToString("000") + "." + dt.Rows[i]["BUNNAME"].ToString().Trim());

                    if (Convert.ToInt32(VB.Val(dt.Rows[i]["BUN"].ToString().Trim())) == intOpCode)
                    {
                        intCBO = i;
                    }
                }
            }

            dt.Dispose();
            dt = null;

            cboOpCode.SelectedIndex = intCBO;
        }
        
        /// <summary>
        /// 김효성
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCombo"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgMD">MD: 내과 (MD) 일경우 세부내과로 모든의사표시</param>
        /// <param name="ArgAll">1: **.전체 , 2: 내과(MD)일경우 세부내과 모두 표시</param>
        /// <param name="ArgTYPE">1: 코드 + "." + 명칭 ,  2: 코드, 3: 명칭</param>
        public static void ComboDrCode_SET(PsmhDb pDbCon, ComboBox ArgCombo, string ArgDept, string ArgMD, string ArgAll = "", string ArgTYPE = "1")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            if (ArgAll == "")
            {
                ArgAll = "1";
            }

            try
            {
                SQL = "";
                SQL = "    SELECT DRCODE, DRNAME ";
                SQL = SQL + ComNum.VBLF + "      FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "    WHERE TOUR <> 'Y'";
                if (ArgMD == "MD")
                {
                    if (ArgDept == "MD")
                        SQL = SQL + ComNum.VBLF + "         AND ( DRDEPT1 = '" + ArgDept + "' OR DRDEPT1 LIKE 'M%' ) ";
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND DRDEPT1 = '" + ArgDept + "' ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND DRDEPT1 = '" + ArgDept + "' ";
                }
                SQL = SQL + ComNum.VBLF + "    ORDER BY PRINTRANKING";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ArgCombo.Items.Clear();

                if (ArgAll == "1")
                {
                    ArgCombo.Items.Add("****.전체");
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (ArgTYPE)
                    {
                        case "1":
                            ArgCombo.Items.Add(dt.Rows[i]["DRcode"].ToString().Trim() + "." + dt.Rows[i]["Drname"].ToString().Trim());
                            break;
                        case "2":
                            ArgCombo.Items.Add(dt.Rows[i]["DRcode"].ToString().Trim());
                            break;
                        case "3":
                            ArgCombo.Items.Add(dt.Rows[i]["Drname"].ToString().Trim());
                            break;
                    }
                }

                ArgCombo.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

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

        public static string READ_OPR_CODE(PsmhDb pDbCon, string ArgGbn, string ArgCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            //int i = 0;
            string strVal = "";

            if (ArgCode == "")
            {
                strVal = "";
                return strVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Name FROM " + ComNum.DB_PMPA + "OPR_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='" + (ArgGbn) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Code ='" + (ArgCode) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["name"].ToString().Trim();
                }
                else
                {
                    strVal = "";
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
            }
            return strVal;
        }

        /// <summary>
        /// 콤보박스 세팅(OPR)
        /// Create : 2018-01-09
        /// Author : 안정수
        /// <seealso cref="vbfunc.bas : Combo_OPRCODE_SET"/>
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCombobox"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgAll"></param>
        public static void Combo_OPRCODE_SET(PsmhDb pDbCon, ComboBox ArgCombobox, string ArgGubun, int ArgTYPE, string ArgAll = "")
        {
            //---------------------------------------------------------------------------------
            //자료사전을 이용하여 ComboBox SET
            //ArgClear: True=Combobox를 Clear후 다른 자료를 Additem, False=Clear 안함
            //ArgType:  1=(코드) + "." + (명칭)형식 2: (코드) 3.(명칭)
            //ArgNULL:  N = " " 제외
            //---------------------------------------------------------------------------------
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            ArgCombobox.Items.Clear();

            if (ArgAll == "ALL")
            {
                ArgCombobox.Items.Add("*.전체");
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  Sort,Code,Name";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPR_CODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Gubun='" + ArgGubun + "'";
            if (ArgTYPE == 4)
            {
                SQL += ComNum.VBLF + "  AND CODE NOT IN '07'";
            }
            SQL += ComNum.VBLF + "ORDER BY Sort,Code";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (ArgTYPE == 1)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }

                    else if (ArgTYPE == 2)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                    else if (ArgTYPE == 3)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

    }
}
