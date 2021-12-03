using ComDbB; //DB연결
using System;
using System.Data;

namespace ComBase
{
    /// <summary>
    /// Description : OCS 추가오더 알림창 모듈
    /// Author : 이상훈
    /// Create Date : 2017.07.24
    /// </summary>
    /// <history>
    /// </history>
    public class clsOcsTray
    {   
        public static string fn_Read_CV(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread SpdNm)
        {
            string SQL;
            DataTable dt = null;
            string SqlErr = "";
            string strReturn = "";

            clsSpread SP = new clsSpread();

            SP.Spread_All_Clear(SpdNm);
            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT '' CHK, B.PANO,B.SNAME, B.ROOMCODE, C.EXAMFNAME         \r";
                SQL += "      , TO_CHAR(A.RESULTDATE, 'MM/DD HH24:MI') RESULTDATE       \r";
                SQL += "      , A.RESULT, A.UNIT, A.ROWID                               \r";
                SQL += "   FROM ADMIN.IPD_NEW_MASTER B                            \r";
                SQL += "      , ADMIN.EXAM_RESULTC_CV A                            \r";
                SQL += "      , ADMIN.EXAM_MASTER     C                            \r";
                SQL += "   WHERE B.JDate = TO_DATE('1900-01-01', 'YYYY-MM-DD')          \r";
                SQL += "     AND B.GBSTS IN ('0', '2', '3', '4', '5', '6')              \r";
                SQL += "     and a.pano = b.pano                                        \r";
                SQL += "     AND A.SUBCODE = C.MASTERCODE                               \r";
                SQL += "     AND A.GBN = '3'                                            \r";
                SQL += "     AND A.CHKDATE IS NULL                                      \r";

                switch (clsPublic.GstrWardCode)
                {
                    case "전체":
                        SQL += "    AND B.WardCode > ' '                                \r";
                        break;
                    case "MICU":
                        SQL += "    AND B.RoomCode = '234'                              \r";
                        break;
                    case "SICU":
                        SQL += "    AND B.RoomCode = '233'                              \r";
                        break;
                    case "ND":
                        SQL += "    AND B.WardCode IN ('ND','IQ','NR')                  \r";
                        break;
                    case "NR":
                        SQL += "    AND B.WardCode IN ('ND','IQ','NR')                  \r";
                        break;
                    default:
                        SQL += "    AND B.WardCode = '" + clsPublic.GstrWardCode.Trim() + "' \r";
                        break;
                }
                //SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                //2020-07-29 로그 제외처리
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, SpdNm, 0, true);
                    strReturn = "OK";
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
                    return strReturn;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return strReturn;
        }

        public static string fn_Read_Ocs_Msg(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread SpdNm)
        {
            string SQL;
            DataTable dt = null;
            string SqlErr = "";
            string strReturn = "";

            clsSpread SP = new clsSpread();

            SP.Spread_All_Clear(SpdNm);
            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT '' CHK, PTNO, SNAME, ROOMCODE                           \r";
                SQL += "      , TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, ROWID    \r";
                SQL += "   FROM ADMIN.OCS_MSG                                      \r";
                SQL += "  WHERE ACTDATE =TRUNC(SYSDATE)                                 \r";

                switch (clsPublic.GstrWardCode)
                {
                    case "전체":
                        SQL += "    AND WardCode > ' '                                  \r";
                        break;
                    case "MICU":
                        SQL += "    AND RoomCode = '234'                                \r";
                        break;
                    case "SICU":
                        SQL += "    AND RoomCode = '233'                                \r";
                        break;
                    case "ND":
                        SQL += "    AND WardCode IN ('ND','IQ','NR')                    \r";
                        break;
                    case "NR":
                        SQL += "    AND WardCode IN ('ND','IQ','NR')                    \r";
                        break;
                    default:
                        SQL += "    AND WardCode = '" + clsPublic.GstrWardCode.Trim() + "' \r";
                        break;
                }
                SQL += "    AND (STATE <> 'Y' OR STATE IS NULL)                         \r";
                SQL += "  ORDER BY ROOMCODE, PTNO                                       \r";
                //SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                //2020-07-29 로그 제외처리
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, SpdNm, 0, true);
                    strReturn = "OK";
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
                    return strReturn;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return strReturn;
        }

        public static string fn_Read_ITransfer(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread SpdNm)
        {
            string SQL;
            DataTable dt = null;
            string SqlErr = "";
            string strReturn = "";

            clsSpread SP = new clsSpread();

            SP.Spread_All_Clear(SpdNm);
            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT '' CHK                                                  \r";
                SQL += "      , A.PTNO                                                 \r";
                SQL += "      , B.SNAME                                                 \r";
                SQL += "      , A.ROOMCODE                                              \r";
                SQL += "      , TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE                     \r";
                SQL += "      , A.FRDEPTCODE                                            \r";
                SQL += "      , C.DRNAME CDRNAME                                        \r";
                SQL += "      , A.TODEPTCODE                                            \r";
                SQL += "      , D.DRNAME DDRNAME                                        \r";
                SQL += "      , A.FRDRCODE                                              \r";
                SQL += "      , A.TODRCODE                                              \r";
                SQL += "      , A.FRREMARK                                              \r";
                SQL += "      , TO_CHAR(A.SDATE,'YYYY-MM-DD HH24:MI')  SDATE            \r";
                SQL += "      , TO_CHAR(A.EDATE,'YYYY-MM-DD HH24:MI')  EDATE            \r";
                SQL += "      , A.TOREMARK                                              \r";
                SQL += "      , a.ROWID                                                 \r";
                SQL += "  FROM ADMIN.OCS_ITRANSFER   A                             \r";
                SQL += "     , ADMIN.BAS_PATIENT    B                             \r";
                SQL += "     , ADMIN.BAS_DOCTOR     C                             \r";
                SQL += "     , ADMIN.BAS_DOCTOR     D                             \r";
                SQL += " WHERE A.EDATE IS NOT NULL                                      \r";
                SQL += "   AND A.BDATE >=TRUNC(SYSDATE -100)                            \r";

                switch (clsPublic.GstrWardCode)
                {
                    case "전체":
                        SQL += "    AND A.WardCode > ' '                                \r";
                        break;
                    case "MICU":
                        SQL += "    AND A.RoomCode = '234'                              \r";
                        break;
                    case "SICU":
                        SQL += "    AND A.RoomCode = '233'                              \r";
                        break;
                    case "ND":
                        SQL += "    AND A.WardCode IN ('ND','IQ','NR')                  \r";
                        break;
                    case "NR":
                        SQL += "    AND A.WardCode IN ('ND','IQ','NR')                  \r";
                        break;
                    default:
                        SQL += "    AND A.WardCode = '" + clsPublic.GstrWardCode.Trim() + "' \r";
                        break;
                }
                SQL += "    AND A.GBDEL <> '*'                                          \r"; //삭제 제외
                SQL += "    AND A.GBCONFIRM = '*'                                       \r"; //완료된 내역만
                SQL += "    AND A.NURSEOK IS NULL                                       \r"; //간호사 확인 않한것
                SQL += "    AND A.PTNO  = B.PANO(+)                                     \r";
                SQL += "    AND A.FRDRCODE = C.DRCODE(+)                                \r";
                SQL += "    AND A.TODRCODE = D.DRCODE(+)                                \r";
                SQL += "  ORDER BY BDATE DESC                                           \r";
                //SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                //2020-07-29 로그 제외처리
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, SpdNm, 0, true);
                    strReturn = "OK";
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
                    return strReturn;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return strReturn;
        }
    }
}
