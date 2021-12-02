using System;
using System.Data;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComSupLibB.SupEnds
{
    /// <summary>내시경에 대한 처방 접수, 자체 처방 만들기, 원처방에 대한 상태 변경</summary>
    public class clsComSupOrd_Endo
    {
        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수

        public clsComSupOrd_Endo()
        {

        }

        /// <summary>
        /// 내시경 메인 테이블 class
        /// </summary>
        public class Ord_Endo_JupMst
        {
            public string STS = ""; //작업구분 00:메뉴얼 01:.. 
            public string Job = "";           
            public string Pano = "";
            public string JDate = "";
            public string OrderCode = "";
            public long OrderNo = 0;
            public string GbSunap = ""; //상태 etc_jupmst 1.접수, 2.미접수 7.완료, *취소
            public string Gubun = ""; //검사구분종류 1.기관지 2.위 3,대장 4.ERCP
            public string Gubun2 = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string RDrName = "";
            public string Ward = "";
            public string Room = "";
            public string GbIO = "";
            public int Amt = 0;
            public long Seqno = 0;
            public string PacsSend = "";
            public string JepsuName = "";
            public string EntDate = "";
            public string VDate = "";
            public string BDate = "";
            public string RDate = "";
            public string RDateTime = "";
            public string SName = "";
            public string Sex = "";
            public string Birth = "";
            public string SeqNum = "";
            public string Buse = "";
            public string ASA = "";
            public string Remark = "";
            public string ROWID = "";
  

        }

        public class Ord_Endo_JupHis
        {
            public long Seqno = 0;
            public string EntDate = "";
            public string ToDate = "";
            public string EntTime = "";
            public string JepsuName = "";
        }
        
        public bool Ord_EndoJupMst_INSERT(PsmhDb pDbCon, Ord_Endo_JupMst argCls)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                //이미 전송된것 체크
                SQL = "";
                SQL += " SELECT ROWID                                                           \r\n";
                SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST                                 \r\n";
                SQL += "  WHERE 1=1                                                             \r\n";
                SQL += "   AND PTNO = '" + argCls.Pano + "'                                     \r\n";
                SQL += "   AND BDate = TO_DATE('" + argCls.BDate + "','YYYY-MM-DD')             \r\n";
                SQL += "   AND OrderNo = " + argCls.OrderNo + "                                 \r\n";
                SQL += "   AND OrderCode = '" + argCls.OrderCode + "'                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL += " UPDATE  " + ComNum.DB_MED + "ENDO_JUPMST  SET                      \r\n";
                    SQL += "   OrderCode = '" + argCls.OrderCode + "'                           \r\n";
                    SQL += "   ,Gubun = '" + argCls.Gubun + "'                                  \r\n";
                    SQL += " WHERE ROWID ='" + dt.Rows[0]["ROWID"].ToString() + "'              \r\n";
                    SQL += "  AND  PTNO = '" + argCls.Pano + "'                                 \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        //Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else
                {

                    SQL = "";
                    SQL += "INSERT INTO  " + ComNum.DB_MED + "ENDO_JUPMST                           \r\n";
                    SQL += " (                                                                      \r\n";
                    SQL += " Ptno,     JDate,  OrderCode, Orderno,  GbJob, RDate                    \r\n";
                    SQL += " ,DeptCode, DrCode, WardCode,  RoomCode, GbIO,  GbSunap                 \r\n";
                    SQL += " ,Amt,      Seqno,  JupsuName, EntDate,  VDate, Sname                   \r\n";
                    SQL += " ,Sex,      BirthDate, PacsSend, SeqNum, BUSE,BDATE                     \r\n"; 
                    SQL += " )                                                                      \r\n";
                    SQL += " VALUES                                                                 \r\n";
                    SQL += " (                                                                      \r\n";
                    SQL += " '" + argCls.Pano + "'                                                  \r\n"; 
                    SQL += " , SYSDATE                                                              \r\n";
                    SQL += " ,'" + argCls.OrderCode + "'                                            \r\n"; 
                    SQL += " ," + argCls.OrderNo + "                                                \r\n"; 
                    SQL += " ,'" + argCls.Gubun + "'                                                \r\n"; 
                    SQL += " , TO_DATE('" + argCls.RDateTime + "','YYYY-MM-DD HH24:MI')             \r\n";
                    SQL += " ,'" + argCls.DeptCode + "'                                             \r\n"; 
                    SQL += " ,'" + argCls.DrCode + "'                                               \r\n"; 
                    SQL += " ,'" + argCls.Ward + "'                                                 \r\n"; 
                    SQL += " ,'" + argCls.Room + "'                                                 \r\n"; 
                    SQL += " ,'" + argCls.GbIO + "'                                                 \r\n"; 
                    SQL += " ,'" + argCls.GbSunap + "'                                              \r\n"; 
                    SQL += " ," + argCls.Amt + "                                                    \r\n"; 
                    SQL += " ," + argCls.Seqno + "                                                  \r\n"; 
                    SQL += " ,'" + argCls.JepsuName + "'                                            \r\n"; 
                    SQL += " ,SYSDATE                                                               \r\n";
                    SQL += " , TO_DATE('" + argCls.VDate + "','YYYY-MM-DD')                         \r\n";
                    SQL += " ,'" + argCls.SName + "'                                                \r\n"; 
                    SQL += " ,'" + argCls.Sex + "'                                                  \r\n";                                          
                    SQL += " , TO_DATE('" + argCls.Birth + "','YYYY-MM-DD HH24:MI')                 \r\n";
                    SQL += " ,'*'                                                                   \r\n"; 
                    SQL += " ,'" + argCls.SeqNum + "'                                               \r\n"; 
                    SQL += " ,'" + argCls.Buse + "'                                                 \r\n"; 
                    SQL += " ,SYSDATE                                                               \r\n";                    
                    SQL += " )                                                                      \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        //Cursor.Current = Cursors.Default;
                        return false;
                    }

                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(pDbCon);

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                

                return false;
            }
        }

        public bool Ord_EndoJupMst_UPDATE(PsmhDb pDbCon, string argSunap, string argROWID)
        {

            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수


            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                //이미 발생된것 체크
                SQL = "";
                SQL += " SELECT ROWID                                   \r\n";
                SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST         \r\n";
                SQL += "  WHERE 1=1                                     \r\n";
                SQL += "   AND ROWID = '" + argROWID + "'               \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL += " UPDATE  " + ComNum.DB_MED + "ENDO_JUPMST  SET  \r\n";
                    SQL += "  GbSunap = '" + argSunap + "'                  \r\n";
                    SQL += " WHERE ROWID ='" + argROWID + "'                \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        //Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                

                return false;
            }

        }

        /// <summary>
        /// 내시경 메인 테이블 생성
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_Endo_JupMst(PsmhDb pDbCon, Ord_Endo_JupMst argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "INSERT INTO  " + ComNum.DB_MED + "ENDO_JUPMST                           \r\n";
            SQL += " (                                                                      \r\n";
            SQL += " Ptno,     JDate,  OrderCode, Orderno,  GbJob, RDate                    \r\n";
            SQL += " ,DeptCode, DrCode, WardCode,  RoomCode, GbIO,  GbSunap                 \r\n";
            SQL += " ,Amt,      Seqno,  JupsuName, EntDate,  VDate, Sname                   \r\n";
            SQL += " ,Sex,      BirthDate, PacsSend, SeqNum, BUSE,BDATE                     \r\n";
            SQL += " )                                                                      \r\n";
            SQL += " VALUES                                                                 \r\n";
            SQL += " (                                                                      \r\n";
            SQL += " '" + argCls.Pano + "'                                                  \r\n";
            SQL += " , SYSDATE                                                              \r\n";
            SQL += " ,'" + argCls.OrderCode + "'                                            \r\n";
            SQL += " ," + argCls.OrderNo + "                                                \r\n";
            SQL += " ,'" + argCls.Gubun + "'                                                \r\n";
            SQL += " , TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')                         \r\n";
            SQL += " ,'" + argCls.DeptCode + "'                                             \r\n";
            SQL += " ,'" + argCls.DrCode + "'                                               \r\n";
            SQL += " ,'" + argCls.Ward + "'                                                 \r\n";
            SQL += " ,'" + argCls.Room + "'                                                 \r\n";
            SQL += " ,'" + argCls.GbIO + "'                                                 \r\n";
            SQL += " ,'" + argCls.GbSunap + "'                                              \r\n";
            SQL += " ," + argCls.Amt + "                                                    \r\n";
            SQL += " ," + argCls.Seqno + "                                                  \r\n";
            SQL += " ,'" + argCls.JepsuName + "'                                            \r\n";
            SQL += " ,SYSDATE                                                               \r\n";
            SQL += " , TO_DATE('" + argCls.VDate + "','YYYY-MM-DD')                         \r\n";
            SQL += " ,'" + argCls.SName + "'                                                \r\n";
            SQL += " ,'" + argCls.Sex + "'                                                  \r\n";
            SQL += " , TO_DATE('" + argCls.Birth + "','YYYY-MM-DD HH24:MI')                 \r\n";
            SQL += " ,'*'                                                                   \r\n";
            SQL += " ,'" + argCls.SeqNum + "'                                               \r\n";
            SQL += " ,'" + argCls.Buse + "'                                                 \r\n";
            SQL += " ,SYSDATE                                                               \r\n";
            SQL += " )                                                                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 내시경 메인 테이블 갱신
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_Endo_JupMst(PsmhDb pDbCon, Ord_Endo_JupMst argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE  " + ComNum.DB_MED + "ENDO_JUPMST  SET                                      \r\n";
            SQL += "  EntDate = SYSDATE                                                                 \r\n";

            if (argCls.STS =="값있는것만")
            {
                if (argCls.OrderCode != "")
                {
                    SQL += " , OrderCode = '" + argCls.OrderCode + "'                                   \r\n";
                }
                if (argCls.RDateTime != "")
                {
                    SQL += " , RDate= TO_DATE('" + argCls.RDateTime + "','YYYY-MM-DD HH24:MI')          \r\n";
                }
                if (argCls.VDate != "")
                {
                    SQL += " , VDate= TO_DATE('" + argCls.VDate + "','YYYY-MM-DD')                      \r\n";
                }
                if (argCls.Birth != "")
                {
                    SQL += " , BirthDate= TO_DATE('" + argCls.Birth + "','YYYY-MM-DD')                  \r\n";
                }
                if (argCls.GbSunap != "")
                {
                    SQL += " , GbSunap ='" + argCls.GbSunap + "'                                        \r\n";
                }
                if (argCls.PacsSend != "")
                {
                    SQL += " , PacsSend ='" + argCls.PacsSend + "'                                      \r\n";
                }
                if (argCls.Sex != "")
                {
                    SQL += " , Sex ='" + argCls.Sex + "'                                                \r\n";
                }
                if (argCls.RDrName != "")
                {
                    SQL += " , RDrName ='" + argCls.RDrName + "'                                        \r\n";
                }
                if (argCls.Remark != "")
                {
                    SQL += " , Remark ='" + argCls.Remark + "'                                          \r\n";
                }
                if (argCls.JepsuName != "")
                {
                    SQL += " , JupsuName ='" + argCls.JepsuName + "'                                    \r\n";
                }
            }
            else
            {

            }
            

            SQL += " WHERE ROWID ='" +argCls.ROWID + "'                                                 \r\n";
            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

            return SqlErr;
        }

        public string up_Endo_JupMst(PsmhDb pDbCon, string argROWID, string argUpCols, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET           \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

            return SqlErr;
        }

        public string up_Endo_JupMst(PsmhDb pDbCon, string argROWID, string argUpCols, string argWheres, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST  SET           \r\n";

            SQL += "    " + argUpCols + "                                   \r\n";

            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                      \r\n";
            if (argWheres !="")
            {
                SQL += "    " + argWheres + "                               \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 200);

            return SqlErr;
        }

        /// <summary>
        /// 내시경 미접수에서 접수로 전환시 테이블 INSERT 문
        /// </summary>
        /// <param name="argROWID"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_ENDO_JUSAMST(PsmhDb pDbCon, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "INSERT INTO  " + ComNum.DB_MED + "ENDO_JUSAMST                          \r\n";
            SQL += " (                                                                      \r\n";
            SQL += "  BDATE, ENTERDATE, PTNO, SEQNO, ORDERCODE, ORDERNO, DEPT               \r\n";
            SQL += " ,DRCODE, GBDIV, DOSCODE, QTY, NAL, REMARK, GBBOTH                      \r\n";
            SQL += " )                                                                      \r\n";
            SQL += " SELECT                                                                 \r\n";
            SQL += "  A.BDate, SYSDATE, B.Ptno, B.SeqNo, A.OrderCode, A.OrderNo             \r\n";
            SQL += " ,RTrim(B.DeptCode), A.DrCode, A.GbDIV, RTrim(A.DOSCODE)                \r\n";
            SQL += " , A.Qty, A.Nal,A.Remark, '2'                                           \r\n";                        
            SQL += "  FROM " + ComNum.DB_MED + "OCS_OORDER a                                \r\n";
            SQL += "     , " + ComNum.DB_MED + "ENDO_JUPMST b                               \r\n";
            SQL += "     , " + ComNum.DB_MED + "OCS_ODOSAGE d                               \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND b.RowID = '" + argROWID + "'                                    \r\n";
            SQL += "    AND A.Bdate    = B.JDate                                            \r\n";
            SQL += "    AND A.Ptno     = B.Ptno                                             \r\n";
            SQL += "    AND A.DeptCode = B.DeptCode                                         \r\n";
            SQL += "    AND A.DOSCODE = D.DOSCODE                                           \r\n";
            SQL += "    AND D.WARDCODE = 'EN'                                               \r\n";
            SQL += "    AND A.GbSuNap  = '1'                                                \r\n";
            SQL += "    AND A.GbBoth   <> '3'                                               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_ENDO_JUSAMST(PsmhDb pDbCon, long argSEQNO, string argCols, string argWhere, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "UPDATE  " + ComNum.DB_MED + "ENDO_JUSAMST  SET                          \r\n";
            SQL += "   " + argCols + "                                                      \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND SEQNO = " + argSEQNO + "                                        \r\n";
            if (argWhere !="")
            {
                SQL += "    " + argWhere + "                                                \r\n";
            }                                    

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 내시경 미접수에서 접수 전환시 외래처방 테이블 갱신 
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_ENDO_JUSAMST_ORDER(PsmhDb pDbCon, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "UPDATE  " + ComNum.DB_MED + "OCS_OORDER  SET                            \r\n";
            SQL += "   GbBoth ='3'                                                          \r\n";            
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND ROWID IN (                                                      \r\n";
            SQL += "                    SELECT                                              \r\n";
            SQL += "                        NVL(A.RowID,'0')                                \r\n";
            SQL += "                        FROM " + ComNum.DB_MED + "OCS_OORDER a          \r\n";
            SQL += "                           , " + ComNum.DB_MED + "ENDO_JUPMST b         \r\n";
            SQL += "                        WHERE 1=1                                       \r\n";
            SQL += "                          AND b.ROWID = '" + argROWID + "'              \r\n";
            SQL += "                          AND a.BDate = b.JDate                         \r\n";
            SQL += "                          AND a.Ptno = b.Ptno                           \r\n";
            SQL += "                          AND a.DeptCode = b.DeptCode                   \r\n";
            SQL += "                          AND RTRIM(a.DosCode) LIKE '9%3'               \r\n";
            SQL += "                          AND a.GbSuNap  = '1'                          \r\n";
            SQL += "                 )                                                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }                

        public string ins_ENDO_RDATE_HISTORY(PsmhDb pDbCon, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "INSERT INTO  " + ComNum.DB_MED + "ENDO_RDATE_HISTORY                    \r\n";
            SQL += " (                                                                      \r\n";
            SQL += "  PTNO, JDATE, ORDERCODE, ORDERNO                                       \r\n";
            SQL += " ,RDATE, SEQNO, WRITEDATE, WRITESABUN                                   \r\n";            
            SQL += " )                                                                      \r\n";
            SQL += " SELECT                                                                 \r\n";
            SQL += "  PTNO, JDATE, ORDERCODE, ORDERNO                                       \r\n";
            SQL += " ,RDATE, SEQNO, SYSDATE                                                 \r\n";
            SQL += " , " + Convert.ToInt32(clsType.User.IdNumber) + "                       \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ENDO_JUPMST                                 \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string ins_Endo_JupHis(PsmhDb pDbCon, Ord_Endo_JupHis argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "INSERT INTO  " + ComNum.DB_MED + "ENDO_JUPHIS                           \r\n";            
            SQL += "  (SEQNO, ENTDATE, TODATE, ENTTIME, JUPSUNAME )  VALUES                 \r\n";                        
            SQL += " (                                                                      \r\n";
            SQL += "  " + argCls.Seqno + "                                                  \r\n";
            SQL += "  ,TRUNC(SYSDATE)                                                       \r\n";
            SQL += "  ,TO_DATE('" + argCls.ToDate + "','YYYY-MM-DD HH24:MI')                \r\n";
            SQL += "  ,'" + argCls.EntTime + "'                                             \r\n";
            SQL += "  ,'" + argCls.JepsuName + "'                                           \r\n";
            SQL += " )                                                                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string up_Endo_JupHis(PsmhDb pDbCon, Ord_Endo_JupHis argCls, ref int intRowAffected, bool bSys = true)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "UPDATE  " + ComNum.DB_MED + "ENDO_JUPHIS  SET                           \r\n";            
            SQL += "  EntDate =TRUNC(SYSDATE)                                               \r\n";
            SQL += "  ,ToDate =TO_DATE('" + argCls.ToDate + "','YYYY-MM-DD HH24:MI')        \r\n";
            SQL += "  ,EntTime ='" + argCls.EntTime + "'                                    \r\n";
            SQL += "  ,JupsuName ='" + argCls.JepsuName + "'                                \r\n";
            SQL += "  WHERE 1=1                                                             \r\n";
            SQL += "    AND SEQNO = " + argCls.Seqno + "                                    \r\n";
            if (bSys==true)
            {
                SQL += "    AND EntDate = TRUNC(SYSDATE)                                    \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public DataTable sel_Endo_JupHis(PsmhDb pDbCon, Ord_Endo_JupHis argCls, bool bSys = true)
        {
            DataTable dt = null;

            SQL = "";


            SQL = "";
            SQL += " SELECT                                                                 \r\n";
            SQL += "    SEQNO, ENTDATE, TODATE, ENTTIME, JUPSUNAME                          \r\n";            
            SQL += "   FROM " + ComNum.DB_MED + "ENDO_JUPHIS                                \r\n";
            SQL += "    WHERE 1=1                                                           \r\n";
            SQL += "     AND SEQNO = " + argCls.Seqno + "                                   \r\n";
            if (bSys == true)
            {
                SQL += "    AND EntDate = TRUNC(SYSDATE)                                    \r\n";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        
       

    }
}
