using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Data;

namespace ComEmrBase
{
    public class clsScanQuery
    {
        public static double GetScanNo(PsmhDb pDbCon)
        {
            double rtnVal = 0;
            DataTable dt = null;
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            rtnVal = 0;

            SQL= string.Empty;
            SQL = SQL + "SELECT " + ComNum.DB_EMR + "EMRSCAN_SCANSEQ.nextVal FunSeqNo FROM Dual";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //Cursor.Current = Cursors.Default
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                //Cursor.Current = Cursors.Default
                return rtnVal;
            }

            rtnVal = VB.Val(dt.Rows[0]["FunSeqNo"].ToString());
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        public static int GetSEQNO(PsmhDb pDbCon, double dblAcpNo, long lngFormNo)
        {
            int rtnVal = 0;
            DataTable dt = null;
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            rtnVal = 0;

            SQL= string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT MAX(SEQNO) AS SEQNO";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRSCAN";
            SQL = SQL + ComNum.VBLF + "    WHERE EMRNO IN " + "( SELECT E.EMRNO";    //, A.IMGNAME";
            SQL = SQL + ComNum.VBLF + "                            FROM  " + ComNum.DB_EMR + "VIEWEMRXML E ";
            SQL = SQL + ComNum.VBLF + "                            INNER JOIN  " + ComNum.DB_EMR + "EMRSCAN S ";
            SQL = SQL + ComNum.VBLF + "                                ON E.EMRNO = S.EMRNO";
            SQL = SQL + ComNum.VBLF + "                            WHERE E.ACPNO = " + dblAcpNo.ToString();
            SQL = SQL + ComNum.VBLF + "                                AND E.FORMNO = " + lngFormNo.ToString();
            SQL = SQL + ComNum.VBLF + "                        ) ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return 1;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return 1;
            }
            rtnVal = ((int)(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()))) + 1;
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        public static bool SaveEMRSCAN(PsmhDb pDbCon, double dblEmrNo, string strScanSeq, int intSeq, string strPathSub,
                                        string strFilename, string strScanDate, string strScanTime, string strSUSEID,
                                        string strVERIFYDATE, string strVERIFYTIME, string strVUSEID, clsTrans TRS = null)
        {
            bool rtnVal = false;
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL= string.Empty;
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRSCAN";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,SCANNO,SEQNO,IMGPATH,SCANDATE,SCANTIME,";
                SQL = SQL + ComNum.VBLF + "      SUSEID,VERIFYDATE,VERIFYTIME,VUSEID)";
                SQL = SQL + ComNum.VBLF + "      VALUES (";
                SQL = SQL + ComNum.VBLF + "     " + dblEmrNo + ",";
                SQL = SQL + ComNum.VBLF + "     " + VB.Val(strScanSeq) + ",";
                SQL = SQL + ComNum.VBLF + "     " + intSeq + ",";
                SQL = SQL + ComNum.VBLF + "     '" + strPathSub + "\\" + strFilename + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strScanDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strScanTime + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strSUSEID + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strVERIFYDATE + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strVERIFYTIME + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strVUSEID + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        public static bool UpdateEMRSCANVERIFY(PsmhDb pDbCon, string strScanSeq, string strVERIFYDATE, string strVERIFYTIME, clsTrans TRS = null)
        {
            bool rtnVal = false;
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL= string.Empty;
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_EMR + "EMRSCAN SET";
                SQL = SQL + ComNum.VBLF + "      VERIFYDATE = '" + strVERIFYDATE + "',";
                SQL = SQL + ComNum.VBLF + "      VERIFYTIME = '" + strVERIFYTIME + "'";
                SQL = SQL + ComNum.VBLF + "      WHETE SCANNO = " + VB.Val(strScanSeq);
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }










    }
}
