using System;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.SupXray;
using System.Data;
using ComSupLibB.Com;

namespace ComSupLibB.SupFnEx
{
    /// <summary>기능검사에 대한 처방 접수, 자체 처방 만들기, 원처방에 대한 상태 변경</summary>
    public class clsComSupOrd_Etc
    {
        string SQL = "";
        string SqlErr = ""; //에러문 받는 변수
        public string[] argOrd = null; //쿼리에 사용될 변수 배열값    
        public string[] argOrd2 = null; //쿼리에 사용될 변수 배열값               

        clsComSQL comSql = new clsComSQL();
        clsComSup sup = new clsComSup();
        clsComSupXraySend xraySend = new clsComSupXraySend();

        public clsComSupOrd_Etc()
        {
            
        }
      
        /// <summary>        /// 진료지원 ETC_JUPMST 생성관련 class 변수  </summary>
        public class Ord_EtcJupMst
        {
            public string STS = ""; //작업구분 00:메뉴얼 01:
            public string GbJob = ""; //상태 etc_jupmst 1.미접수, 2예약 3.접수,완료, 9취소
            public string Gubun = ""; //검사구분
            public string Gubun2 = "";
            public string GbIO = "";
            public string BDate = "";
            public string RDate = "";
            public string Pano = "";
            public string SName = "";
            public string Sex = "";
            public int Age = 0;
            public string Room = "";
            public string OrderCode = "";
            public long OrderNo = 0;
            public string Bun = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string GbPort = ""; //M 포터블
            public string Remark = "";
            public string OrderDate = "";
            public string SendDate = "";
            public string ArrDate = "";
            public string StartDate = "";
            public string CrDate = "";
            public string CDate = ""; //확인일자
            public long CSabun = 0; //확인사번
            public string GbFTP = "";
            public string File_Path = "";
            public string Image_Gbn = "";
            public string Stress_Sogen = "";
            public string ResultDrSabun = "";
            public string ASA = "";
            public string CVR = "";
            public string CVRDetail = "";
            public string CP = "";
            public string Check_File = "";

        }

        /// <summary> 진료지원 ETC_JUPMST 생성 </summary>        
        public bool Ord_EtcJupMst_INSERT(PsmhDb pDbCon, Ord_EtcJupMst OrdEtc)
        {
            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //이미 전송된것 체크
                SQL = "";
                SQL +=" SELECT ROWID                                                    \r\n";
                SQL +="  FROM " + ComNum.DB_MED + "ETC_JUPMST                           \r\n";
                SQL += "  WHERE 1=1                                                     \r\n";
                SQL +="   AND PTNO = '" + OrdEtc.Pano + "'                              \r\n";
                SQL +="   AND BDate = TO_DATE('" + OrdEtc.BDate + "','YYYY-MM-DD')      \r\n";
                SQL +="   AND OrderNo = " + OrdEtc.OrderNo + "                          \r\n";
                SQL +="   AND OrderCode = '" + OrdEtc.OrderCode + "'                    \r\n";
                SQL +="   AND Gubun = '" + OrdEtc.Gubun + "'                            \r\n";

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
                    SQL +=" UPDATE  " + ComNum.DB_MED + "ETC_JUPMST  SET                \r\n";
                    SQL +="   OrderCode = '" + OrdEtc.OrderCode + "'                    \r\n";
                    SQL +="   ,Gubun = '" + OrdEtc.Gubun + "'                           \r\n";
                    SQL +=" WHERE ROWID ='" + dt.Rows[0]["ROWID"].ToString() + "'       \r\n";
                    SQL +="  AND  PTNO = '" + OrdEtc.Pano + "'                          \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        //Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else
                {

                    SQL = "";
                    SQL +="INSERT INTO  " + ComNum.DB_MED + "ETC_JUPMST                             \r\n";
                    SQL += " (                                                                      \r\n";
                    SQL += " BDate, Ptno,     SName,  Sex,    Age,   OrderCode, Orderno, GbIO       \r\n";
                    SQL += " ,Bun,   DeptCode, DrCode, Remark, RDate, Amt, EntDate, GbJob           \r\n";
                    SQL += " ,GbEr,  RoomCode, Gubun                                                \r\n";
                    SQL += " )                                                                      \r\n";
                    SQL += " VALUES                                                                 \r\n";
                    SQL += " (                                                                      \r\n";
                    SQL +="  TO_DATE('" + OrdEtc.BDate + "','YYYY-MM-DD')                           \r\n"; 
                    SQL +=" ,'" + OrdEtc.Pano + "'                                                  \r\n"; 
                    SQL +=" ,'" + OrdEtc.SName + "'                                                 \r\n"; 
                    SQL +=" ,'" + OrdEtc.Sex + "'                                                   \r\n"; 
                    SQL +=" ," + OrdEtc.Age + "                                                     \r\n"; 
                    SQL +=" ,'" + OrdEtc.OrderCode + "'                                             \r\n"; 
                    SQL +=" ," + OrdEtc.OrderNo + "                                                 \r\n"; 
                    SQL +=" ,'" + OrdEtc.GbIO + "'                                                  \r\n"; 
                    SQL +=" ,'" + OrdEtc.Bun + "'                                                   \r\n"; 
                    SQL +=" ,'" + OrdEtc.DeptCode + "'                                              \r\n"; 
                    SQL +=" ,'" + OrdEtc.DrCode + "'                                                \r\n"; 
                    SQL +=" ,'" + OrdEtc.Remark + "'                                                \r\n"; 
                    SQL +=" , TO_DATE('" + OrdEtc.RDate + "','YYYY-MM-DD HH24:MI')                  \r\n"; 
                    SQL +=" , 0                                                                     \r\n"; 
                    SQL +=" , SYSDATE                                                               \r\n"; 
                    SQL +=" ,'1'                                                                    \r\n"; 
                    SQL +=" ,''                                                                     \r\n"; 
                    SQL +=" ,'" + OrdEtc.Room + "'                                                  \r\n"; 
                    SQL +=" ,'" + OrdEtc.Gubun + "'                                                 \r\n"; 
                    SQL += " )                                                                      \r\n";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
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

        /// <summary>  기능검사 ETC_JUPMST 접수구분 변경(미접수,접수 등) </summary>        
        public bool Ord_EtcJupMst_UPDATE(PsmhDb pDbCon, string[] arg)
        {                        
            DataTable dt = null;                        
            int intRowAffected = 0; //변경된 Row 받는 변수
            
            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //이미 발생된것 체크
                dt = sel_Etc_JupMst(pDbCon, arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.ROWID]);

                if (dt.Rows.Count > 0)
                {
                    //접수구분 갱신
                    SqlErr = up_Etc_JupMst(pDbCon, arg, ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                        
                        return false;
                    }
                    if (SqlErr =="")
                    {
                        #region //2017.10.24.윤조연 : 처방 상태 업데이트
                        clsComSQL.enmSENDEPT_STAT_020 SENDEPT_STAT_020 = clsComSQL.enmSENDEPT_STAT_020.CMPL;

                        //2017.10.24.윤조연: 처방 상태 업데이트
                        #region // 구분설정
                        if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.GbJob] == "1")
                        {
                            SENDEPT_STAT_020 = clsComSQL.enmSENDEPT_STAT_020.UN_RCPN;
                        }
                        else if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.GbJob] == "2")
                        {
                            SENDEPT_STAT_020 = clsComSQL.enmSENDEPT_STAT_020.APNT;
                        }
                        else if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.GbJob] == "3")
                        {
                            SENDEPT_STAT_020 = clsComSQL.enmSENDEPT_STAT_020.CMPL;
                        }
                        else if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.GbJob] == "9")
                        {
                            SENDEPT_STAT_020 = clsComSQL.enmSENDEPT_STAT_020.CNCL;
                        }
                        #endregion
                        SqlErr = sup.up_OCS_ORDER_SendDept_20(pDbCon,  arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.Gubun], SENDEPT_STAT_020, arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.Pano], arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.BDate], arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.OrderNo], ref intRowAffected);
                        
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                        
                            return false;
                        }
                        #endregion
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
        /// 기능검사 xray_detail,xray_pacssend 연동
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool Ord_EtcJupMst_Xray_UpDate(PsmhDb pDbCon, string[] arg)
        {            
            int intRowAffected = 0; //변경된 Row 받는 변수

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {               
                //접수구분 갱신
                SqlErr = xraySend.up_XRAY_DETAIL_FnEx(pDbCon, arg, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                        
                    return false;
                }
                
                SqlErr = xraySend.ins_XRAY_PACSSEND_FnEx(pDbCon, arg, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                        
                    return false;
                }
                

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


        #region ETC_JUPMST 관련쿼리

        public DataTable sel_Etc_JupMst(PsmhDb pDbCon, string argROWID,string argPano="", string argBDate="")
        {
            DataTable dt = null;


            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "       ROWID                                                                \r\n";
            SQL += "  FROM " + ComNum.DB_MED + "ETC_JUPMST                                      \r\n";            
            SQL += "   WHERE 1 = 1                                                              \r\n";
            if (argROWID != "") SQL += " AND ROWID ='" + argROWID + "'                          \r\n";
            if (argPano !="") SQL += "   AND Pano ='" + argPano + "'                            \r\n";
            if (argBDate != "") SQL += "  AND BDate =TO_DATE('" + argPano + "','YYYY-MM-DD')    \r\n";
            
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


        #endregion


        #region 트랜잭션 쿼리 + enum INSERT, UPDATE,DELETE .... 

        /// <summary>
        /// ETC_JUPMST 접수 상태 관련 
        /// </summary>
        public enum enm_Etc_Jupmst { STS, Pano, BDate, RDate,RTime,RDateTime, Gubun, OrderNo, GbJob, sysdate, jobSabun, ROWID }


        /// <summary>
        /// 기능검사 ETC_JUPMST 접수구분 갱신 GBJOB
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_Etc_JupMst(PsmhDb pDbCon, string[] arg, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUPMST  SET                                                                    \r\n";
            
            SQL += "    GbJob = '" + arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.GbJob] + "'                                           \r\n";

            //미접수>>접수로
            if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.STS]=="0" || arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.STS] == "2")
            {
                SQL += "    ,RDate = TO_DATE('" + arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.RDateTime] + "','YYYY-MM-DD HH24:MI')    \r\n";
                SQL += "    ,CDate = SYSDATE                                                                                        \r\n";
                SQL += "    ,CSabun = '" + arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.jobSabun] + "'                                  \r\n";
                SQL += "    ,startDate = SYSDATE                                                                                    \r\n";
            }
            else if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.STS] == "1")
            {
                SQL += "    ,startDate = ''                                                                                         \r\n";
            }
            
            SQL += "  WHERE 1=1                                                                                                     \r\n";
            SQL += "    AND ROWID = '" + arg[(int)enm_Etc_Jupmst.ROWID] + "'                                                        \r\n";

            //미접수>>접수로
            if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.STS] == "0" || arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.STS] == "2")
            {
                SQL += "    AND GbJob <> '3'                                                                                        \r\n";
            }
            else if (arg[(int)clsComSupOrd_Etc.enm_Etc_Jupmst.STS] == "1")
            {
                SQL += "    AND GbJob = '3'                                                                                         \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        #endregion

    }
}
