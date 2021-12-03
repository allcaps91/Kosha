using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// 작성자 : 이상훈
    /// 처방 Lock Check Class
    /// </summary>
    public class clsLocklchk
    {
        public static string GstrLockPtno;
        public static string GstrLockRemark;
        public static string GstrPart;

        public static string SQL;
        public static string strIpAddress;
        public static string SqlErr = ""; //에러문 받는 변수
        public static int intRowAffected = 0; //변경된 Row 받는 변수
        public static int rowcounter;
        public static string strValue;

        public static DataTable dt = null;
        public static DataTable dt1 = null;
        public static DataTable dt2 = null;

        public static void IpdOcs_Lock_Delete()
        {
            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " DELETE FROM ADMIN.IPD_LOCK                   \r";
                SQL += "  WHERE Pano = '" + VB.UCase(GstrLockPtno) + "'     \r";
                SQL += "    AND Seq  = 0                                    \r";
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

        public static void IpdOcs_Lock_Delete_NEW()
        {
            if (GstrLockPtno == "" || GstrLockPtno == "        ") return;

            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " DELETE ADMIN.OCS_OLOCK                        \r";
                SQL += "  WHERE Ptno   = '" + GstrLockPtno + "'             \r";
                if (GstrPart != "" && GstrPart != null)
                {
                    SQL += "    AND PART = '" + GstrPart + "'               \r";
                }
                //SQL += "    AND EXENAME  = '" + VB.UCase(App.EXEName) & "'  \r";
                if (clsPublic.GstrIpAddress != "" && clsPublic.GstrIpAddress != null)
                {
                    SQL += "   AND IP  = '" + clsPublic.GstrIpAddress + "'  \r";
                }
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

        public static string IpdOcs_Lock_Insert()
        {
            string strTcpIp;
            string strPassname;
            string strComment;
            string strValue;

            strTcpIp = "";

            if (VB.Left(strTcpIp, 1) == ".")
            {
                strTcpIp = VB.Mid(strTcpIp.Trim(), 2, 2);
            }
            else if (VB.Mid(strTcpIp, 2, 1) == ".")
            {
                strTcpIp = VB.Mid(strTcpIp.Trim(), 3, 1);
            }

            strPassname = strTcpIp + clsPublic.GstrJobName.Trim();
            strComment = "#OCS 처방입력중 입니다.";

            try
            {
                SQL = "";
                SQL += " SELECT * FROM ADMIN.IPD_LOCK     \r";
                SQL += " WHERE Pano   = '" + GstrLockPtno + "'  \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    dt.Dispose();
                    dt = null;
                    strValue = "";
                    return strValue;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    strValue = "";
                    return strValue;
                }

                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    LOCK_CHECK();
                    dt.Dispose();
                    dt = null;
                    return "NO";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " INSERT INTO ADMIN.IPD_LOCK                           \r";
                SQL += "        (Pano,Seq,UserName,JobComment,WrtTime)              \r";
                SQL += " VALUES                                                     \r";
                SQL += "        ('" + GstrLockPtno + "',0, '" + strPassname + "',   \r";
                SQL += "         '" + strComment + "', SYSDATE )                    \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    LOCK_CHECK();
                    return "NO";
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return "OK";
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return "";
            }
        }

        public static void LOCK_CHECK()
        {
            try
            {
                SQL = "";
                SQL += "SELECT UserName, JobComment, TO_CHAR(WrtTime, 'yy-mm-dd hh24:mi') Jtime \r";
                SQL += "  FROM ADMIN.IPD_LOCK                                             \r";
                SQL += " WHERE Pano   = '" + VB.UCase(GstrLockPtno.Trim()) + "'                 \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                clsPublic.GstrMsgTitle = "주의 !";
                clsPublic.GstrMsgList = " ";
                clsPublic.GstrMsgList += "작업자명 : " + dt.Rows[0]["UserName"].ToString().Trim() + "\r";
                clsPublic.GstrMsgList += "작업내용 : " + dt.Rows[0]["JobComment"].ToString().Trim() + "\r";
                clsPublic.GstrMsgList += "시작시간 : " + dt.Rows[0]["Jtime"].ToString().Trim() + "\r";
                clsPublic.GstrMsgList += "\r" + "잠시후에 다시 작업을 하시거나 ";
                clsPublic.GstrMsgList += "\r" + " 해당 담당자(원무과, 심사과 등등) 전화하여 작업완료 요청하세요";

                MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        public static string IpdOcs_Lock_Insert_NEW()
        {
            //string strIpAddress;

            try
            {
                SQL = "";
                SQL += " SELECT Remark, TO_CHAR(EntDate,'YY-MM-DD HH24:MI') EntTime, PART, SABUN, IP, EXENAME   \r";
                SQL += "   FROM ADMIN.OCS_OLOCK                                                            \r";
                SQL += "  WHERE Ptno   = '" + GstrLockPtno + "'                                                 \r";
                SQL += "    AND ROWNUM = 1                                                                      \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "OK";
                }
                
                rowcounter = dt.Rows.Count;
                
                if (rowcounter > 0)
                {
                    strValue = "NO";  //--  이두줄의 위치 변경하면 안됨
                    LOCK_CHECK_1(dt, dt.Rows[0]["IP"].ToString().Trim(), Convert.ToInt64(dt.Rows[0]["SABUN"].ToString().Trim()));
                }
                else
                {
                    strValue = "OK";
                    LOCK_INSERT();
                }
                
                dt.Dispose();
                dt = null;

                return strValue;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "OK";
            }
        }

        public static string LOCK_CHECK_1(DataTable PreDt, string strIP, long nSabun)
        {
            if (strIP == clsPublic.GstrIpAddress && clsPublic.GnJobSabun == nSabun)
            {
                strValue = "OK";
                return strValue;
            }

            string sIP;
            string sPart;
            string strPart;
            
            sIP = string.Format("{0:000}", VB.Pstr(PreDt.Rows[0]["IP"].ToString().Trim(), ".", 1));
            sIP += "." + string.Format("{0:000}", VB.Pstr(PreDt.Rows[0]["IP"].ToString().Trim(), ".", 2));
            sIP += "." + string.Format("{0:000}", VB.Pstr(PreDt.Rows[0]["IP"].ToString().Trim(), ".", 3));
            sIP += "." + string.Format("{0:000}", VB.Pstr(PreDt.Rows[0]["IP"].ToString().Trim(), ".", 4));
            
            try
            {
                SQL = "";
                SQL += " SELECT A.IPADDR,  A.USE,  A.USENAME, A.BUCODE, B.NAME          \r";
                SQL += "      , SUBSTR(JACODE, 1,4) || '-' || SUBSTR(JACODE,5) JACODE   \r";
                SQL += "   FROM ADMIN.JAS_MASTER A                                 \r";
                SQL += "      , ADMIN.BAS_BUSE  B                                 \r";
                SQL += "  WHERE IPADDR ='" + strIP + "'                                 \r";
                SQL += "    AND A.BUCODE = B.BUCODE                                     \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                rowcounter = dt.Rows.Count;
                strPart = "I      P : " + PreDt.Rows[0]["IP"] + "\r\r";

                if (rowcounter > 0)
                {
                    strPart += "장    소 :" + dt.Rows[0]["Name"].ToString().Trim() + " " + "\r\r";
                    strPart += "P  C  명 :" + dt.Rows[0]["usename"].ToString().Trim() + " " + "\r\r";
                    strPart += "자산번호 :" + dt.Rows[0]["JACODE"].ToString().Trim() + " " + "\r\r";
                }
                else
                {   
                    SQL = " SELECT A.REMARK FROM ADMIN.JAS_ETCIPADDR A \r";
                    SQL += " WHERE IPADDR = '" + strIP + "'                 \r";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return "OK";
                    }

                    rowcounter = dt2.Rows.Count;
                    if (rowcounter > 0)
                    {
                        strPart += "(" + dt2.Rows[0]["Remark"].ToString().Trim() + ")  ";
                    }
                    dt2.Dispose();
                    dt2 = null;
                    
                }
                clsPublic.GstrMsgTitle = "주의 !";
                clsPublic.GstrMsgList = " ";
                clsPublic.GstrMsgList += "\r" + "작업내용 : " + PreDt.Rows[0]["Remark"].ToString().Trim() + "\r";
                clsPublic.GstrMsgList += "\r" + "시작시간 : " + PreDt.Rows[0]["EntTime"].ToString().Trim() + "\r";
                clsPublic.GstrMsgList += "\r" + "잠시후에 다시 작업을 하시거나, : ";
                clsPublic.GstrMsgList += "다른환자에 대한 작업을 하십시오 ! " + "\r\r";
                clsPublic.GstrMsgList += "컴 퓨 터 정 보 ======================================" + "\r\r" + strPart + "\r";
                clsPublic.GstrMsgList += "=====================================================" + "\r";

                MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

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

        public static void LOCK_INSERT()
        {
            Cursor.Current = Cursors.WaitCursor;

            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO ADMIN.OCS_OLOCK                            \r";
                SQL += "      (Ptno,Remark,EntDate, SABUN,  part, EXENAME, ip )     \r";
                SQL += "VALUES                                                      \r";
                SQL += "       ('" + GstrLockPtno + "', '" + GstrLockRemark + "'    \r";
                SQL += "       , SYSDATE,  '" + clsPublic.GnJobSabun + "'           \r";
                SQL += "       , '" + GstrPart + "', ''                             \r";
                SQL += "       , '" + clsPublic.GstrIpAddress + "')                 \r";
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
    }
}
