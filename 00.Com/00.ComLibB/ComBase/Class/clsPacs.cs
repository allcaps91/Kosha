using ComDbB; //DB연결
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// PACS 관련 함수 모음
    /// </summary>
    public class clsPacs
    {

        #region //maroview : 변수
        public const string PACS_EXE_PATH = @"c:\Marotech\m-view\maroview.exe";
        public static string PACS_PASSWORD = "";
        public static long PACS_ID_Number = 0;

        #endregion //maroview : 변수


        #region //Agpa
        /// <summary>
        /// Web1000Viewer.exe 실행하고 있는지 Check OK:실행중 NO:실행않함
        /// </summary>
        /// <returns></returns>
        public static string Web1000_Execute_Check()
        {
            string rtnVal = "NO";

            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("Web1000Viewer.EXE");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("Web1000Viewer.EXE");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        rtnVal = "OK";
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// Web1000Viewer.exe 실행
        /// </summary>
        public static string Web1000_Login()
        {
            string rtnVal = "NO";
            VB.Shell(@"C:\Agfa\Web1000Viewer.EXE" + " ", "NormalFocus");
            //System.Diagnostics.Process program = System.Diagnostics.Process.Start(@"C:\Agfa\Web1000Viewer.EXE", "");
            return rtnVal;
        }
        #endregion //Agpa


        #region //maroview : 함수
        /// <summary>
        /// PACS VIEW 실행
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgPacsNo"></param>
        /// <param name="ArgUSER"></param>
        /// <param name="ArgCombine"></param>
        public static void PACS_Image_View(PsmhDb pDbCon, string ArgPano, string ArgPacsNo = "", string ArgUSER = "", bool ArgCombine = false)
        {
            if (File.Exists(PACS_EXE_PATH) == false)
            {
                ComFunc.MsgBox("인피니트 PACS Viewer가 설치 안됨!!");
                return;
            }

            if (ArgPano.Trim() == "")
            {
                ComFunc.MsgBox("등록번호가 공란입니다.");
                return;
            }

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strLogin = "";
            string strCombine = "";
            PACS_PASSWORD = "";

            if (ArgUSER != PACS_ID_Number.ToString() && ArgUSER != "23515")
            {
                SQL = "SELECT UserID,PassWD FROM " + ComNum.DB_PACS + "MUSER ";
                SQL = SQL + ComNum.VBLF + "WHERE UserID = '" + ArgUSER + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    PACS_PASSWORD = dt.Rows[0]["PassWD"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }

            if (ArgUSER != "0" && ArgUSER != "23515" && PACS_PASSWORD != "")
            {
                strLogin = ArgUSER + "@" + PACS_PASSWORD.Trim();
            }
            else
            {
                strLogin = "RR@0";
            }

            if (ArgCombine == true) strCombine = " /combine ";

            //인피니트 m-View를 실행함
            if (ArgPacsNo == "")
            {
                //VB.Shell(PACS_EXE_PATH + " /hp" + ArgPano + strCombine + " /u " + strLogin, "NormalFocus");
                //System.Diagnostics.Process program = System.Diagnostics.Process.Start(@"C:\Agfa\Web1000Viewer.EXE", "");
                //c:\\Marotech\\m-view\\maroview.exe /u 
                System.Diagnostics.Process program = System.Diagnostics.Process.Start(PACS_EXE_PATH, " /hp " + ArgPano + strCombine + " /u " + strLogin);
            }
            else
            {
                //VB.Shell(PACS_EXE_PATH + " /h" + ArgPacsNo + " /hp " + ArgPano + strCombine + " /u " + strLogin, "NormalFocus");
                System.Diagnostics.Process program = System.Diagnostics.Process.Start(PACS_EXE_PATH, " /h " + ArgPacsNo + " /hp " + ArgPano + strCombine + " /u " + strLogin);
            }

        }

        /// <summary>
        /// PACS VIEW 실행
        /// </summary>
        /// <param name="ArgUSER"></param>
        /// <param name="ArgPass"></param>
        public static void PACS_Image_View_AES(string ArgUSER, string ArgPass)
        {
            if (File.Exists(PACS_EXE_PATH) == false)
            {
                ComFunc.MsgBox("인피니트 PACS Viewer가 설치 안됨!!");
                return;
            }

            //VB.Shell(PACS_EXE_PATH + " /u" + ArgUSER + "@" + ArgPass, "NormalFocus");
            System.Diagnostics.Process program = System.Diagnostics.Process.Start(PACS_EXE_PATH, " /u " + ArgUSER + "@" + ArgPass);
        }

        /// <summary>
        /// 판독완료세팅
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgPacsNo"></param>
        /// <returns></returns>
        public static bool SET_XRAY_READ_UPDATE_INFINITT(PsmhDb pDbCon, string ArgPano, string ArgPacsNo)
        {
            bool rtnVal = false;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            
            clsDB.setBeginTran(pDbCon);
            try
            {
                SQL = " SELECT ROWID FROM ADMIN.DEXAM ";
                SQL = SQL + ComNum.VBLF + " WHERE patid ='" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "  AND serialno ='" + ArgPacsNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND ESTATUS ='C' ";  //초기판독
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = " UPDATE ADMIN.DEXAM SET ESTATUS ='R' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// CVR관련 대상자 영상 클릭시 시간갱신
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgPacsNo"></param>
        /// <returns></returns>
        public static bool SET_XRAY_CVR_READ_UPDATE(PsmhDb pDbCon, string ArgPano, string ArgPacsNo)
        {
            bool rtnVal = false;
            string strPacsNo = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ArgPacsNo == "") return rtnVal;

            DataTable dt = null;
            string SQL = "";
            int icvr = 0;
            string[] arryPacNo = VB.Split(ArgPacsNo, " ");

            Cursor.Current = Cursors.WaitCursor;
            
            clsDB.setBeginTran(pDbCon);
            try
            {
                for (icvr = 0; icvr < arryPacNo.Length; icvr++)
                {
                    strPacsNo = arryPacNo[icvr];
                    if (strPacsNo != "")
                    {
                        //CVR용 영상본 최초시각 update
                        SQL = " SELECT ROWID FROM ADMIN.XRAY_DETAIL ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + ArgPano + "'";
                        SQL = SQL + ComNum.VBLF + "  AND PACSNO ='" + strPacsNo + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND CVR IN ('Y','S' ) ";
                        SQL = SQL + ComNum.VBLF + "  AND (CVR_CDate IS NULL OR CVR_CDate ='') ";
                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            SQL = " UPDATE ADMIN.XRAY_DETAIL SET CVR_CDATE =SYSDATE ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("CVR저장세팅 완료.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion //maroview : 함수


        /// <summary>
        /// 영상 판독/ 영상프로그램 로그인 권환을 확인한다.
        /// READ_PACS_LOGIN_CHK
        /// READ_PACS_READ_CHK
        /// </summary>
        /// <param name="ArgSabun">사번</param>
        /// <param name="strPROGRAMID">권한 구분 ex) MVIEW, XRESULT</param>
        /// <returns></returns>
        public static bool ChkPacsLogin(PsmhDb pDbCon, string ArgSabun, string strPROGRAMID)
        {
            bool bolVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSabun = "";

            if (strPROGRAMID == "MVIEW")
            {
                switch (ArgSabun.ToUpper())
                {
                    case "KJH3343":
                    case "CD":
                    case "CS":
                    case "OS":
                    case "RR":
                    case "RS":
                    case "TP":
                    case "ADM":
                    case "RAD":
                        return true;
                }
            }

            strSabun = ComFunc.LPAD(ArgSabun, 5, "0");

            try
            {

                SQL = "SELECT NAME FROM ADMIN.BAS_PASS";
                SQL = SQL + ComNum.VBLF + " WHERE IDNUMBER = " + strSabun + " ";
                SQL = SQL + ComNum.VBLF + " AND PROGRAMID = '" + strPROGRAMID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return bolVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolVal = true;
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return bolVal;
        }
    }
}
