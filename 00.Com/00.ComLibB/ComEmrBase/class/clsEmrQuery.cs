using ComBase; //기본 클래스
using ComBase.Controls;
using ComBase.Mvc;
using ComDbB; //DB연결
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComEmrBase
{
    public class clsEmrQuery
    {
        /// <summary>
        /// MO 기록지 오더이름 삭제
        /// </summary>
        /// <param name="pAcp">EMR 환자정보</param>
        public static bool MO_ORDER_NAME_DEL(EmrPatient pAcp)
        {
            #region 변수
            MTSResult result = new MTSResult(true);
            #endregion

            try
            {
                result.SetSuccessCountPlus(FormPatInfoQuery.Query_MO_ORDER_NAME_DEL(pAcp));
                result.SetSuccessMessage("저장하였습니다.");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, "MO_ORDER_NAME_DEL 오류", clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return result.Result == ResultType.Success;
        }

        /// <summary>
        /// MO 기록지 오더이름 저장
        /// </summary>
        /// <param name="pAcp">EMR 환자정보</param>
        public static bool MO_ORDER_NAME_SAVE(EmrPatient pAcp, FpSpread ssWrite)
        {
            #region 변수
            MTSResult result = new MTSResult(true);
            #endregion

            try
            {
                result.SetSuccessCountPlus(FormPatInfoQuery.Query_MO_ORDER_NAME_DEL(pAcp));

                string ORDNAME = string.Empty;

                for (int i = 1; i < 11; i++)
                {
                    ORDNAME = ssWrite.ActiveSheet.RowHeader.Cells[i, 1].Text.Trim();
                    if (ORDNAME.Left(4).Equals("Drug"))
                        continue;

                    result.SetSuccessCountPlus(FormPatInfoQuery.Query_MO_ORDER_NAME_SAVE(pAcp, ORDNAME, i));
                }

                ORDNAME = ssWrite.ActiveSheet.RowHeader.Cells[48, 1].Text.Trim();

                result.SetSuccessCountPlus(FormPatInfoQuery.Query_MO_ORDER_NAME_SAVE(pAcp, ORDNAME, 48));


                result.SetSuccessMessage("저장하였습니다.");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, "MO_ORDER_NAME_SAVE 오류", clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return result.Result == ResultType.Success;
        }

        /// <summary>
        /// 동의서 삭제 
        /// </summary>
        /// <param name="formDataId">EMRNO와 동일</param>
        public static bool AeasFormDelete(string formDataId)
        {
            #region 변수
            bool rtnVal = false;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            #endregion

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string HisId = string.Empty;

                SQL.Clear();
                SQL.AppendLine("SELECT KOSMOS_EMR.EASFORMDATAHIS_SEQ.NEXTVAL        ");
                SQL.AppendLine("  FROM DUAL                                         ");

                SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (SqlErr.NotEmpty())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, "전자동의서 시퀀스 값 생성시 오류", clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    HisId = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();


                int RowAffected = 0;
                #region ISDELETED = 'Y'
                SQL.Clear();
                SQL.AppendLine(" UPDATE KOSMOS_EMR.AEASFORMDATA                                         ");
                SQL.AppendLine("    SET ISDELETED               = 'Y'                                   ");
                SQL.AppendLine("    ,   MODIFIED                = SYSDATE                               ");
                SQL.AppendLine("    ,   MODIFIEDUSER            = '" + clsType.User.IdNumber + "'       ");
                SQL.AppendLine("    ,   EASFORMDATAHISTORYID    = " + HisId                             );
                SQL.AppendLine(" WHERE ID = " + formDataId);

                SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
                if (SqlErr.NotEmpty())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, "전자동의서 UPDATE KOSMOS_EMR.AEASFORMDATA 오류", clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }
                #endregion

                #region 히스토리 저장
                SQL.Clear();
                SQL.AppendLine("INSERT INTO KOSMOS_EMR.AEASFORMDATAHISTORY                      ");
                SQL.AppendLine("(                                                               ");
                SQL.AppendLine("ID ,CREATED   ,MODIFIED,                                        ");
                SQL.AppendLine("BLANKCONTENT,   CANVAS,                                         ");
                SQL.AppendLine("DATABASE64,CREATEDUSER,EASFORMCONTENT,EASFORMDATA,JSON          ");
                SQL.AppendLine(")                                                               ");

                SQL.AppendLine("SELECT                                                          ");
                SQL.AppendLine("        EASFORMDATAHISTORYID                                    ");
                SQL.AppendLine("    ,   SYSDATE                                                 ");
                SQL.AppendLine("    ,   SYSDATE                                                 ");
                SQL.AppendLine("    ,   BLANKCONTENT                                            ");
                SQL.AppendLine("    ,   CANVAS                                                  ");
                SQL.AppendLine("    ,   DATABASE64                                              ");
                SQL.AppendLine("    ,   MODIFIEDUSER                                            ");
                SQL.AppendLine("    ,   EASFORMCONTENT                                          ");
                SQL.AppendLine("    ,   ID                                                      ");
                SQL.AppendLine("    ,   JSON                                                    ");
                SQL.AppendLine("  FROM KOSMOS_EMR.AEASFORMDATA                                  ");
                SQL.AppendLine(" WHERE ID = " + formDataId                                       );

                SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
                if (SqlErr.NotEmpty())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, "전자동의서 INSERT INTO KOSMOS_EMR.AEASFORMDATAHISTORY 오류", clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }
                #endregion

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, "전자동의서 Delete 오류", clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 스캔뷰어 테스트 IP 점검
        /// </summary>
        /// <param name="Dbcon"></param>
        /// <returns></returns>
        public static bool NewScanViewTestIP(PsmhDb Dbcon, string IP)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT BASNAME                 ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRBASCD    ";
                SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS  = '스캔관리'    ";
                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '테스트'       ";
                SQL = SQL + ComNum.VBLF + "    AND BASCD   = '" + IP + "'  ";
                SQL = SQL + ComNum.VBLF + "    AND BASNAME = 'Y'           ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, Dbcon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, Dbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }


                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                rtnVal = true;
                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, Dbcon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// 스캔뷰어 테스트
        /// </summary>
        /// <param name="Dbcon"></param>
        /// <returns></returns>
        public static string NewScanViewTest(PsmhDb Dbcon)
        {
            string rtnVal = "N";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT BASNAME                ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRBASCD   ";
                SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = '스캔관리'    ";
                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '테스트'      ";
                SQL = SQL + ComNum.VBLF + "    AND BASCD   = '사용여부'    ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, Dbcon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, Dbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }


                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Read();
                rtnVal = reader.GetValue(0).ToString().Trim();

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, Dbcon); //에러로그 저장
            }

            return rtnVal;
        }


        public static string PT_Time_Duplicate(PsmhDb Dbcon, EmrPatient AcpEmr, string ChartDate, string ChartTimeText, string EmrNo)
        {
            string rtnVal = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            ChartTimeText = ChartTimeText.Replace(":", string.Empty);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT REPLACE(R.ITEMVALUE, ':', '') ITEMVALUE, F.FORMNAME";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_EMR.AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "        ON A.EMRNO = R.EMRNO";
                SQL = SQL + ComNum.VBLF + "       AND A.EMRNOHIS = R.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "       AND R.ITEMCD = 'I0000032877'";
                //SQL = SQL + ComNum.VBLF + "       AND (R.ITEMVALUE LIKE '%" + ChartTimeText.Split('~')[0] + "%' OR R.ITEMVALUE LIKE '%" + ChartTimeText.Split('~')[1] + "%')";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_EMR.AEMRFORM F";
                SQL = SQL + ComNum.VBLF + "        ON A.FORMNO   = F.FORMNO";
                SQL = SQL + ComNum.VBLF + "       AND A.UPDATENO = F.UPDATENO";
                SQL = SQL + ComNum.VBLF + "       AND F.FORMNAME LIKE '%경과기록%'";
                SQL = SQL + ComNum.VBLF + "       AND F.GRPFORMNO = 1031";
                SQL = SQL + ComNum.VBLF + " WHERE A.MEDFRDATE = '" + AcpEmr.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO      = '" + AcpEmr.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE = '" + ChartDate + "'";
                if (EmrNo.NotEmpty())
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.EMRNO <> " + EmrNo;
                }

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, Dbcon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, Dbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }


                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }


                if (ChartTimeText.IndexOf("~") != -1)
                {
                    double WriteMin = VB.Val(ChartTimeText.Split('~')[0]);
                    double WriteMax = VB.Val(ChartTimeText.Split('~')[1]);

                    while (reader.Read())
                    {
                        double MinTime = VB.Val(reader.GetValue(0).ToString().Split('~')[0]);
                        double MaxTime = VB.Val(reader.GetValue(0).ToString().Split('~')[1]);

                        if (WriteMin >= MinTime && WriteMin < MaxTime ||
                            WriteMax >= MinTime && WriteMax < MaxTime)
                        {
                            rtnVal = reader.GetValue(1).ToString().Trim();
                            break;
                        }
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, Dbcon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// 정형화 동의서 변경여부 체크
        /// </summary>
        /// <param name="Dbcon"></param>
        /// <returns></returns>
        public static string NewArgreeRecord(PsmhDb Dbcon)
        {
            string rtnVal = "N";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT BASCD";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '동의서'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, Dbcon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, Dbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }


                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Read();
                rtnVal = reader.GetValue(0).ToString().Trim();

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, Dbcon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// 수혈 종료시간에 날짜가 없으면 넣는다.
        /// </summary>
        /// <param name="ChartDate"></param>
        /// <param name="ItemCd"></param>
        public static void CheckBloodTimeExists(EmrPatient pAcp, string FormNo, string ChartDate)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            int intRowAffected = 0;

            string Time = string.Empty;
            string AcpNo = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT REPLACE(REPLACE(B.ITEMVALUE, '-', ''), '/', '') CHARTDATE, REPLACE(REPLACE(B2.ITEMVALUE, ':', ''), ';', '') TIME, T.ACPNO ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON A.EMRNO = B.EMRNO";
            SQL = SQL + ComNum.VBLF + "   AND A.EMRNOHIS = B.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRCHARTROW B2";
            SQL = SQL + ComNum.VBLF + "    ON A.EMRNO = B2.EMRNO";
            SQL = SQL + ComNum.VBLF + "   AND A.EMRNOHIS = B2.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBVITALTIME T";
            SQL = SQL + ComNum.VBLF + "    ON A.ACPNO     = T.ACPNO";
            SQL = SQL + ComNum.VBLF + "   AND T.CHARTDATE = '" + ChartDate + "'";
            SQL = SQL + ComNum.VBLF + "   AND T.JOBGB     = 'IVT'";
            SQL = SQL + ComNum.VBLF + "   AND T.TIMEVALUE = REPLACE(REPLACE(B2.ITEMVALUE, ':', ''), ';', '')";
            SQL = SQL + ComNum.VBLF + "   AND T.SUBGB     = '0'";
            SQL = SQL + ComNum.VBLF + "   AND T.FORMNO    = " + FormNo;
            SQL = SQL + ComNum.VBLF + " WHERE A.PTNO      = '" + pAcp.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.FORMNO   IN (1965, 3535)";
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + ChartDate + "'";
            SQL = SQL + ComNum.VBLF + "   AND B.ITEMCD = 'I0000037490' -- 종료일자";
            SQL = SQL + ComNum.VBLF + "   AND REPLACE(REPLACE(B.ITEMVALUE, '-', ''), '/', '') = '" + ChartDate + "'";
            SQL = SQL + ComNum.VBLF + "   AND B2.ITEMCD = 'I0000037491'  -- 종료시간";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Time = dt.Rows[0]["TIME"].ToString();
                AcpNo = dt.Rows[0]["ACPNO"].ToString();
                if (AcpNo.NotEmpty())
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            dt.Dispose();
            dt = null;

            if (Time.IsNullOrEmpty())
                return;

            #region //기본값 설정
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strWDate = VB.Left(strCurDataTime, 8);
                string strWTime = VB.Right(strCurDataTime, 6);

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_EMR.AEMRBVITALTIME ";
                SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, CHARTDATE, JOBGB, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                SQL = SQL + ComNum.VBLF + " SELECT DISTINCT " + FormNo + " AS FORMNO";
                SQL = SQL + ComNum.VBLF + " , A.ACPNO";
                SQL = SQL + ComNum.VBLF + " , REPLACE(REPLACE(B.ITEMVALUE, '-', ''), '/', '') CHARTDATE";
                SQL = SQL + ComNum.VBLF + " , 'IVT' AS JOBGB";
                SQL = SQL + ComNum.VBLF + " , REPLACE(REPLACE(B2.ITEMVALUE, ':', ''), ';', '') TIMEVALUE";
                SQL = SQL + ComNum.VBLF + " , '0' AS SUBGB";
                SQL = SQL + ComNum.VBLF + " , '" + strWDate + "' AS WRITEDATE";
                SQL = SQL + ComNum.VBLF + " , '" + strWTime + "' AS WRITETIME";
                SQL = SQL + ComNum.VBLF + " , '" + clsType.User.IdNumber + "' AS WRITEUSEID";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
                SQL = SQL + ComNum.VBLF + "    ON A.EMRNO = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND A.EMRNOHIS = B.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRCHARTROW B2";
                SQL = SQL + ComNum.VBLF + "    ON A.EMRNO = B2.EMRNO";
                SQL = SQL + ComNum.VBLF + "   AND A.EMRNOHIS = B2.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRBVITALTIME T";
                SQL = SQL + ComNum.VBLF + "    ON A.ACPNO    = T.ACPNO";
                SQL = SQL + ComNum.VBLF + "   AND T.CHARTDATE = REPLACE(REPLACE(B.ITEMVALUE, '-', ''), '/', '')";
                SQL = SQL + ComNum.VBLF + "   AND T.JOBGB     = 'IVT'";
                SQL = SQL + ComNum.VBLF + "   AND T.TIMEVALUE = REPLACE(REPLACE(B2.ITEMVALUE, ':', ''), ';', '')";
                SQL = SQL + ComNum.VBLF + "   AND T.SUBGB     = '0'";
                SQL = SQL + ComNum.VBLF + "   AND T.FORMNO    = " + FormNo;
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO     = '" + pAcp.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.FORMNO   IN (1965, 3535)";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + ChartDate + "'";
                SQL = SQL + ComNum.VBLF + "   AND B.ITEMCD = 'I0000037490' -- 종료일자";
                SQL = SQL + ComNum.VBLF + "   AND REPLACE(REPLACE(B.ITEMVALUE, '-', ''), '/', '') = '" + ChartDate + "'";
                SQL = SQL + ComNum.VBLF + "   AND B2.ITEMCD = 'I0000037491'  -- 종료시간";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "   (";
                SQL = SQL + ComNum.VBLF + "     SELECT 1";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRBVITALTIME";
                SQL = SQL + ComNum.VBLF + "      WHERE ACPNO    = A.ACPNO";
                SQL = SQL + ComNum.VBLF + "        AND CHARTDATE = REPLACE(REPLACE(B.ITEMVALUE, '-', ''), '/', '')";
                SQL = SQL + ComNum.VBLF + "        AND JOBGB     = 'IVT'";
                SQL = SQL + ComNum.VBLF + "        AND TIMEVALUE = REPLACE(REPLACE(B2.ITEMVALUE, ':', ''), ';', '')";
                SQL = SQL + ComNum.VBLF + "        AND SUBGB     = '0'";
                SQL = SQL + ComNum.VBLF + "        AND FORMNO    = " + FormNo;
                SQL = SQL + ComNum.VBLF + "   )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
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
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
            }
            #endregion //기본값 설정
        }

        /// <summary>
        /// 수액 기록지 없으면 시작 생성 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="strROWID"></param>
        /// <returns></returns>
        public static bool SaveActOrder(PsmhDb pDbCon, EmrPatient pAcp, string strROWID, string ActDate, string ActTime, bool dateChk)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                #region 수액 아이템 없으면 추가.
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                SQL = SQL + ComNum.VBLF + "SELECT DISTINCT";
                if (clsType.User.BuseCode.Equals("033109"))
                { 
                    SQL = SQL + ComNum.VBLF + "    1969, --응급실";
                }
                else 
                {
                    SQL = SQL + ComNum.VBLF + "    3150, --그외 병동";
                }
                SQL = SQL + ComNum.VBLF + "     " + pAcp.acpNo + ", ";
                SQL = SQL + ComNum.VBLF + "     '" + pAcp.ptNo + "', ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE, 'YYYYMMDD'),";
                SQL = SQL + ComNum.VBLF + "     'IIO',";
                SQL = SQL + ComNum.VBLF + "     'I0000030580', --정맥주입 ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'YYYYMMDD'), ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'HH24MISS'), ";
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "	 FROM KOSMOS_OCS.OCS_IORDER A";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";
                SQL = SQL + ComNum.VBLF + "   AND NOT EXISTS";
                SQL = SQL + ComNum.VBLF + "   (";
                SQL = SQL + ComNum.VBLF + "     SELECT 1";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_EMR.AEMRBVITALSET";
                SQL = SQL + ComNum.VBLF + "      WHERE ACPNO     = " + pAcp.acpNo;
                SQL = SQL + ComNum.VBLF + "        AND CHARTDATE = TO_CHAR(A.BDATE, 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "        AND JOBGB  = 'IIO'";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I0000030580'";
                if (clsType.User.BuseCode.Equals("033109"))
                {
                    SQL = SQL + ComNum.VBLF + "        AND FORMNO = 1969 --응급실";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "        AND FORMNO = 3150 --그외병동";
                }
                SQL = SQL + ComNum.VBLF + "   )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                #endregion

                #region 수액 시작 확인
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRBIOFLUID ";
                SQL = SQL + ComNum.VBLF + "		( ACTSEQ, ACPNO, PTNO,  ";
                SQL = SQL + ComNum.VBLF + "		ORDDATE, SITEGB, ORDERCODE, ORDNO, ";
                SQL = SQL + ComNum.VBLF + "		SEQNO, ORDROWID, ACTGB, ACTQTY, ACTRMK, ";
                SQL = SQL + ComNum.VBLF + "		ACTDATE, ACTTIME, ACTUSEID,  ";
                SQL = SQL + ComNum.VBLF + "		ACTVAL, ACTTERM, ";
                SQL = SQL + ComNum.VBLF + "		WRITEDATE, WRITETIME, WRITEUSEID ) ";
                SQL = SQL + ComNum.VBLF + "     SELECT ";  //ACTSEQ, 
                SQL = SQL + ComNum.VBLF + "     (KOSMOS_EMR.GETSEQ_AEMRBIOFLUID_ACTSEQ), ";  //ACTSEQ, 
                SQL = SQL + ComNum.VBLF + "     " + pAcp.acpNo + ",";  //ACPNO, 
                SQL = SQL + ComNum.VBLF + "     '" + pAcp.ptNo + "',";  //PTNO, 
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE, 'YYYYMMDD'),";  //ORDDATE, 
                SQL = SQL + ComNum.VBLF + "     CASE WHEN TRIM(DEPTCODE) = 'ER' THEN 'ERD' ELSE 'IPD' END,";  //SITEGB, 
                SQL = SQL + ComNum.VBLF + "     TRIM(ORDERCODE),";  //ORDERCODE, 
                SQL = SQL + ComNum.VBLF + "     ORDERNO,";  //ORDNO, 
                SQL = SQL + ComNum.VBLF + "     " + "1" + ",";  //SEQNO, 
                SQL = SQL + ComNum.VBLF + "     '" + strROWID + "',";  //ORDROWID, 
                SQL = SQL + ComNum.VBLF + "     '00',";  //ACTGB, 
                SQL = SQL + ComNum.VBLF + "     0,";  //ACTQTY, 
                SQL = SQL + ComNum.VBLF + "     '',";  //ACTRMK, 
                if (dateChk)
                {
                    SQL = SQL + ComNum.VBLF + "     '" + ActDate + "',";  //ACTDATE, 
                    SQL = SQL + ComNum.VBLF + "     '" + (ActTime.Length == 4 ? string.Concat(ActTime, "00") : ActTime) + "',";  //ACTTIME, 
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'YYYYMMDD'),";  //ACTDATE, 
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'HH24MISS'),";  //ACTTIME, 
                }
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "',";  //ACTUSEID, 
                SQL = SQL + ComNum.VBLF + "     '',";  //ACTVAL, 
                SQL = SQL + ComNum.VBLF + "     '',";  //ACTTERM, 
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'YYYYMMDD'),";  //WRITEDATE, 
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'HH24MISS'),";  //WRITETIME, 
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'";  //WRITEUSEID
                SQL = SQL + ComNum.VBLF + "		FROM KOSMOS_OCS.OCS_IORDER A";
                SQL = SQL + ComNum.VBLF + "	   WHERE ROWID = '" + strROWID + "'";            
                SQL = SQL + ComNum.VBLF + "      AND EXISTS";
                SQL = SQL + ComNum.VBLF + "      (";
                SQL = SQL + ComNum.VBLF + "         SELECT 1";
                SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_OCS.OCS_IORDER O";
                SQL = SQL + ComNum.VBLF + "             INNER JOIN KOSMOS_ADM.DRUG_MASTER2 F ";
                SQL = SQL + ComNum.VBLF + "                ON O.SUCODE = F.JEPCODE";
                SQL = SQL + ComNum.VBLF + "               AND F.SUGABUN = '20'  ";
                SQL = SQL + ComNum.VBLF + "             INNER JOIN KOSMOS_ADM.DRUG_MASTER1 F2 ";
                SQL = SQL + ComNum.VBLF + "                ON F.JEPCODE = F2.JEPCODE";
                SQL = SQL + ComNum.VBLF + "               AND (F2.GBIO = 'Y' AND F2.POJANG2 = 'ml' OR (F2.GBIO = 'Y' AND F2.HAMYANG2 = 'ml'))";
                SQL = SQL + ComNum.VBLF + "         WHERE O.ROWID = A.ROWID";
                SQL = SQL + ComNum.VBLF + "      )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                #endregion

                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return rtnVal;
        }


        /// <summary>
        /// 처방이 수액인지 True
        /// </summary>
        /// <param name="Dbcon"></param>
        /// <param name="strROWID"></param>
        /// <param name="strOrderNo"></param>
        /// <returns></returns>
        public static bool SapExists(PsmhDb Dbcon, string strROWID)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT 1";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER O";
                SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN KOSMOS_OCS.OCS_ORDERCODE C";
                SQL = SQL + ComNum.VBLF + "       ON O.SLIPNO     =  C.SLIPNO";
                SQL = SQL + ComNum.VBLF + "      AND O.ORDERCODE  =  C.ORDERCODE";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_ADM.DRUG_MASTER2 F ";
                SQL = SQL + ComNum.VBLF + "        ON O.SUCODE = F.JEPCODE";
                SQL = SQL + ComNum.VBLF + "       AND F.SUGABUN = '20'  ";
                //SQL = SQL + ComNum.VBLF + "       AND F.JEHYENGBUN = '02'  ";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_ADM.DRUG_MASTER1 F2 ";
                SQL = SQL + ComNum.VBLF + "        ON F.JEPCODE = F2.JEPCODE";
                SQL = SQL + ComNum.VBLF + "       AND (F2.GBIO = 'Y' AND F2.POJANG2 = 'ml' OR (F2.GBIO = 'Y' AND F2.HAMYANG2 = 'ml'))";
                SQL = SQL + ComNum.VBLF + "  WHERE O.ROWID = '" + strROWID + "'";
                SQL = SQL + ComNum.VBLF + "    AND O.GBPRN <>'S' "; //'JJY 추가(2000/05/22 'S는 선수납(선불);
                SQL = SQL + ComNum.VBLF + "    AND (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
                SQL = SQL + ComNum.VBLF + "    AND (O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND  O.ACTDIV > 0 ) )  ";
                SQL = SQL + ComNum.VBLF + "    AND O.GBPICKUP = '*' ";
                SQL = SQL + ComNum.VBLF + "    AND (O.VERBC IS NULL OR O.VERBC <>'Y' )";
                SQL = SQL + ComNum.VBLF + "    AND O.QTY  <>  0    ";
                SQL = SQL + ComNum.VBLF + "    AND (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, Dbcon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, Dbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Dispose();
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, Dbcon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// 처방이 수액인지 + 수액 기록이 있는지 시작 했으면 True
        /// </summary>
        /// <param name="Dbcon"></param>
        /// <param name="strROWID"></param>
        /// <param name="strOrderNo"></param>
        /// <returns></returns>
        public static bool SapOrderExists(PsmhDb Dbcon, string strROWID, int ActDiv = 0)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT 1";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER O";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_ADM.DRUG_MASTER2 F ";
                SQL = SQL + ComNum.VBLF + "     ON O.SUCODE = F.JEPCODE";
                SQL = SQL + ComNum.VBLF + "    AND F.SUGABUN = '20'  ";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_ADM.DRUG_MASTER1 F2 ";
                SQL = SQL + ComNum.VBLF + "     ON F.JEPCODE = F2.JEPCODE";
                SQL = SQL + ComNum.VBLF + "    AND (F2.GBIO = 'Y' AND F2.POJANG2 = 'ml' OR (F2.GBIO = 'Y' AND F2.HAMYANG2 = 'ml'))";
                if (ActDiv > 0)
                {
                    SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_OCS.OCS_IORDER_ACT AT";
                    SQL = SQL + ComNum.VBLF + "     ON AT.PTNO  = O.PTNO";
                    SQL = SQL + ComNum.VBLF + "    AND AT.ORDERNO = O.ORDERNO";
                    SQL = SQL + ComNum.VBLF + "    AND AT.GBSTATUS = ' ' ";
                    SQL = SQL + ComNum.VBLF + "    AND AT.ACTDIV = " + ActDiv;
                }

                SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRBIOFLUID F3";
                SQL = SQL + ComNum.VBLF + "     ON F3.PTNO  = O.PTNO";
                SQL = SQL + ComNum.VBLF + "    AND F3.ORDDATE  = TO_CHAR(O.BDATE, 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "    AND F3.SITEGB   = DECODE(O.DEPTCODE, 'ER', 'ERD', 'IPD')";
                SQL = SQL + ComNum.VBLF + "    AND F3.ORDNO    = O.ORDERNO";
                SQL = SQL + ComNum.VBLF + "    AND F3.ACTGB    = '00'";
                SQL = SQL + ComNum.VBLF + "    AND F3.DCCLS    = '0' -- DC안한항목";
                if (ActDiv > 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND F3.ACTDATE = TO_CHAR(AT.ACTTIME, 'YYYYMMDD')";
                    SQL = SQL + ComNum.VBLF + "    AND SUBSTR(F3.ACTTIME, 0, 4) = TO_CHAR(AT.ACTTIME, 'HH24MI')";
                }
                SQL = SQL + ComNum.VBLF + "  WHERE O.ROWID = '" + strROWID + "'";

                if (ActDiv == 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND O.GBDIV < ";
                    SQL = SQL + ComNum.VBLF + "    (";
                    SQL = SQL + ComNum.VBLF + "    SELECT COUNT(ORDERNO) CNT";
                    SQL = SQL + ComNum.VBLF + "      FROM KOSMOS_OCS.OCS_IORDER_ACT";
                    SQL = SQL + ComNum.VBLF + "     WHERE PTNO = O.PTNO";
                    SQL = SQL + ComNum.VBLF + "       AND ORDERNO = O.ORDERNO";
                    SQL = SQL + ComNum.VBLF + "       AND GBSTATUS = ' '";
                    SQL = SQL + ComNum.VBLF + "    )";
                }

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, Dbcon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, Dbcon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    reader = null;
                    return rtnVal;
                }

                reader.Dispose();
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, Dbcon); //에러로그 저장
            }

            return rtnVal;
        }


        /// <summary>
        /// 차트일자에 아이템이 존재하지 않는다면 넣는다.
        /// </summary>
        /// <param name="ChartDate"></param>
        /// <param name="ItemCd"></param>
        public static void CheckItemExists(EmrPatient pAcp, string FormNo, string ChartDate, string ItemCd)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT 1 ";
            SQL = SQL + ComNum.VBLF + "FROM DUAL";
            SQL = SQL + ComNum.VBLF + "WHERE EXISTS";
            SQL = SQL + ComNum.VBLF + "(";
            SQL = SQL + ComNum.VBLF + "SELECT 1";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO      = " + FormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO     = " + pAcp.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + ChartDate + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD    = '" + ItemCd + "'";
            SQL = SQL + ComNum.VBLF + ")";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            dt.Dispose();
            dt = null;

            #region //기본값 설정
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDataTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strWDate = VB.Left(strCurDataTime, 8);
                string strWTime = VB.Right(strCurDataTime, 6);

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     " + FormNo + ", ";
                SQL = SQL + ComNum.VBLF + "     " + pAcp.acpNo + ", ";
                SQL = SQL + ComNum.VBLF + "     '" + pAcp.ptNo + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + ChartDate + "',";
                SQL = SQL + ComNum.VBLF + "     CASE WHEN UNITCLS = '임상관찰' THEN 'IVT'";
                SQL = SQL + ComNum.VBLF + "          WHEN UNITCLS = '섭취배설' THEN 'IIO'";
                SQL = SQL + ComNum.VBLF + "          WHEN UNITCLS = '특수치료' THEN 'IST'";
                SQL = SQL + ComNum.VBLF + "          WHEN UNITCLS = '기본간호' THEN 'IBN'";
                SQL = SQL + ComNum.VBLF + "     END JOBGB, ";
                SQL = SQL + ComNum.VBLF + "     '" + ItemCd + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + strWDate + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + strWTime + "', ";
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "  AND UNITCLS IN ('섭취배설', '임상관찰', '특수치료', '기본간호')";
                SQL = SQL + ComNum.VBLF + "  AND BASCD = '" + ItemCd + "'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
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
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
            }
            #endregion //기본값 설정
        }

        /// <summary>
        /// 오더 있는지
        /// </summary>
        /// <returns></returns>
        public static bool ChartOrder_Exists(Control msgForm, EmrPatient pAcp, string Bdate, string ItemCd, string ItemValue)
        {
            if (pAcp == null)
                return false;

            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            #region 쿼리
            SQL = "SELECT 1 ";
            SQL = SQL + ComNum.VBLF + "FROM DUAL";
            SQL = SQL + ComNum.VBLF + "WHERE EXISTS";
            SQL = SQL + ComNum.VBLF + "(";
            SQL = SQL + ComNum.VBLF + " SELECT 1";
            SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER A";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_OCS.OCS_IORDER B";
            SQL = SQL + ComNum.VBLF + "        ON A.ORDERNO = B.ORDERNO";
            SQL = SQL + ComNum.VBLF + "       AND B.PTNO = '" + pAcp.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "       AND B.BDATE = TO_DATE('" + Bdate + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "  WHERE A.EMRNO       = " + Bdate.Replace("-", "");
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD      = '" + ItemCd + "'";
            if (ItemValue.IndexOf("'") != -1)
            {
                SQL = SQL + ComNum.VBLF + "    AND A.ITEMVALUE IN(" + ItemValue + ")";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND A.ITEMVALUE   = '" + ItemValue + "'";
            }
            SQL = SQL + ComNum.VBLF + "    AND A.GBSTATUS    = ' '";
            SQL = SQL + ComNum.VBLF + ")";
            #endregion

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(msgForm as Form, SqlErr);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();
            return rtnVal;
        }

        /// <summary>
        /// 팀장, 수간호사, 책임간호사 인지 확인.
        /// </summary>
        /// <returns></returns>
        public static bool GetEmrSettingAuth(PsmhDb pDbCon)
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            bool rtnVal = false;

            if (clsType.User.IdNumber.Equals("35893") || clsType.User.IdNumber.Equals("47301"))
            {
                rtnVal = true;
                return rtnVal;
            }

            #region 쿼리
            SQL += ComNum.VBLF + "SELECT 1     ";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "  	SELECT 1";
            SQL += ComNum.VBLF + " 	      FROM KOSMOS_ADM.INSA_MST A";
            SQL += ComNum.VBLF + "  	 WHERE SABUN3 = " + clsType.User.IdNumber;
            SQL += ComNum.VBLF + "         AND A.JIK IN ('04', '13', '32', '33') -- 부장, 팀장, 수간호사, 책임";
            SQL += ComNum.VBLF + ")";
      

            #endregion

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr.NotEmpty())
            {
                clsDB.SaveSqlErrLog(SqlErr, SqlErr, pDbCon);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();
            return rtnVal;
        }


        /// <summary>
        /// OCR 출력 히스토리
        /// </summary>
        public static void SetEMROCRPRTHIS(EmrPatient AcpEmr, string FormNo)
        {
            string SQL = string.Empty;
            int RowAffected = 0;

            if (AcpEmr.inOutCls.Equals("I"))
            {
                SQL = " SELECT A.WARDCODE, B.SNAME";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER A,";
                SQL += ComNum.VBLF + "  KOSMOS_PMPA.BAS_PATIENT B";
                SQL += ComNum.VBLF + "  WHERE A.PANO = B.PANO";
                SQL += ComNum.VBLF + "  AND TO_CHAR(A.INDATE,'YYYYMMDD') = '" + AcpEmr.medFrDate + "'";
                SQL += ComNum.VBLF + "  AND A.DEPTCODE = '" + AcpEmr.medDeptCd + "'";
                SQL += ComNum.VBLF + "  AND A.PANO = '" + AcpEmr.ptNo + "'";
            }
            else
            {
                SQL = " SELECT '' AS WARDCODE, B.SNAME";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER A,";
                SQL += ComNum.VBLF + "  KOSMOS_PMPA.BAS_PATIENT B";
                SQL += ComNum.VBLF + "  WHERE A.PANO = B.PANO";
                SQL += ComNum.VBLF + "  AND TO_CHAR(A.BDATE,'YYYYMMDD') = '" + AcpEmr.medFrDate + "'";
                SQL += ComNum.VBLF + "  AND A.DEPTCODE = '" + AcpEmr.medDeptCd + "'";
                SQL += ComNum.VBLF + "  AND A.PANO = '" + AcpEmr.ptNo + "'";
            }

            OracleDataReader reader = null;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }

                string strWARDCODE = string.Empty;
                string strName = string.Empty;

                if (reader.HasRows && reader.Read())
                {
                    strWARDCODE = reader.GetValue(0).ToString().Trim();
                    strName = reader.GetValue(1).ToString().Trim();
                }

                reader.Dispose();


                SQL = " INSERT INTO KOSMOS_EMR.EMROCRPRTHIS";
                SQL += ComNum.VBLF + " (OCRDATE,OCRTIME,PTNO,PTNAME,INOUTCLS,";
                SQL += ComNum.VBLF + "  MEDFRDATE,MEDDEPTCD,WARDCODE,";
                SQL += ComNum.VBLF + "  FORMNO,USEID,DEPTCD,DEPTCD1)";
                SQL += ComNum.VBLF + "  VALUES (";
                SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE, 'YYYYMMDD'),";
                SQL += ComNum.VBLF + "  TO_CHAR(SYSDATE, 'HH24MISS'),";
                SQL += ComNum.VBLF + "  '" + AcpEmr.ptNo + "',";
                SQL += ComNum.VBLF + "  '" + strName + "',";
                SQL += ComNum.VBLF + "  '" + AcpEmr.inOutCls + "',";
                SQL += ComNum.VBLF + "  '" + AcpEmr.medFrDate + "',";
                SQL += ComNum.VBLF + "  '" + AcpEmr.medDeptCd + "',";
                SQL += ComNum.VBLF + "  '" + strWARDCODE + "',";
                SQL += ComNum.VBLF + "  '" + FormNo + "',";
                SQL += ComNum.VBLF + "  '" + clsType.User.IdNumber + "',";
                SQL += ComNum.VBLF + "  '',";
                SQL += ComNum.VBLF + "  '" + clsType.User.BuseCode + "')";

                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// EMRNO로 수가연동된 정보를 가져온다.
        /// </summary>
        /// <param name="EmrNo"></param>
        /// <returns></returns>
        public static string GetOrderInfo(PsmhDb pDbCon, string EmrNo, string ItemCd, string ItemValue, string Pano = "")
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            #region 쿼리
            SQL += ComNum.VBLF + "SELECT S.SUNAMEK, QTY, TRIM(M.KORNAME) KORNAME     ";
            SQL += ComNum.VBLF + "FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER A     ";
            SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_OCS.OCS_IORDER B        ";
            SQL += ComNum.VBLF + "     ON A.ORDERNO = B.ORDERNO               ";
            SQL += ComNum.VBLF + "    AND A.GBSTATUS  = B.GBSTATUS            ";
            if (Pano.NotEmpty())
            {
                SQL += ComNum.VBLF + "    AND B.PTNO      = '" + Pano + "'";
            }
            SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_SUN S          ";
            SQL += ComNum.VBLF + "     ON S.SUNEXT = B.SUCODE                 ";
            SQL += ComNum.VBLF + "   LEFT OUTER JOIN KOSMOS_ADM.INSA_MST M   ";
            SQL += ComNum.VBLF + "     ON M.SABUN = B.NURSEID 				  ";
            SQL += ComNum.VBLF + "WHERE A.EMRNO     = "  + EmrNo;
            SQL += ComNum.VBLF + "  AND A.ITEMCD    = '" + ItemCd + "'";
            if (ItemValue.NotEmpty())
            {
                SQL += ComNum.VBLF + "  AND A.ITEMVALUE = '" + ItemValue + "'";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND A.GBSTATUS = ' '";
            }
            SQL += ComNum.VBLF + "  AND EXISTS";
            SQL += ComNum.VBLF + "   (";
            SQL += ComNum.VBLF + "  	SELECT 1";
            SQL += ComNum.VBLF + " 	      FROM KOSMOS_OCS.OCS_IORDER";
            SQL += ComNum.VBLF + "  	 WHERE ORDERNO = B.ORDERNO";
            SQL += ComNum.VBLF + "  	 GROUP BY ORDERNO ";
            SQL += ComNum.VBLF + "      HAVING SUM(NAL) > 0";
            SQL += ComNum.VBLF + "   ) ";
            #endregion

            StringBuilder Data = new StringBuilder();

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr.NotEmpty())
            {
                clsDB.SaveSqlErrLog(SqlErr, SqlErr, pDbCon);
                return Data.ToString();
            }

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    Data.AppendLine(reader.GetValue(0).ToString() + " " +
                                reader.GetValue(1).ToString() + " (" +
                                reader.GetValue(2).ToString() + ")");
                }
            }

            reader.Dispose();

            return Data.ToString();
        }

        /// <summary>
        /// 처방 삭제
        /// </summary>
        /// <param name="msgForm"></param>
        /// <param name="Trs"></param>
        /// <param name="EmrNo"></param>
        /// <param name="ItemCd"></param>
        public static void DeleteOrderData(Form msgForm, double EmrNo, string ItemCd, string ItemValue, string OrderNo)
        {
            if (EmrNo == 0)
                return;

            Cursor.Current = Cursors.WaitCursor;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                #region 오더 삭제                    
                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE1          \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IORDER  A                    \r";
                SQL += "  WHERE EXISTS                                      \r";
                SQL += "    (                                               \r";
                SQL += "         SELECT 1                                   \r";
                SQL += "           FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER    \r";
                SQL += "          WHERE EMRNO    = " + EmrNo   +            "\r";
                SQL += "            AND ITEMCD   = '" + ItemCd + "'         \r";
                SQL += "            AND ORDERNO  = A.ORDERNO                \r";
                SQL += "            AND GBSTATUS = ' '                      \r";
                SQL += "    )                                               \r";
                SQL += "    AND DEPTCODE  != 'ER'                           \r";
                SQL += "    AND (ACCSEND IS NULL or ACCSEND = 'Z')          \r";
                SQL += "    AND ORDERSITE not in ('NDC', 'ERO')             \r";
                SQL += "    AND GBPRN != 'P'                                \r";
                SQL += "    AND GBSEND = ' '                                \r";
                SQL += "    AND GBSTATUS != 'D'                             \r";
                SQL += "    AND SUCODE IS NOT NULL                          \r";
                SQL += "    AND SLIPNO != '0106'                            \r";    //자가약

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(msgForm, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    strMsg = dt.Rows[0]["BDATE1"].ToString().Trim();
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBoxEx(msgForm, strMsg + "일자에 미 전송된 오더가 있습니다.잠시후에 작업 해주세요..", "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                dt.Dispose();
                dt = null;



                SQL = " UPDATE KOSMOS_OCS.OCS_IORDER A";
                SQL = SQL + ComNum.VBLF + " SET GBSTATUS = 'D', ";
                SQL = SQL + ComNum.VBLF + "     NURSEID = '" + clsType.User.Sabun + "' , ";
                SQL = SQL + ComNum.VBLF + "     GBPICKUP = '*' , ";
                SQL = SQL + ComNum.VBLF + "     PICKUPSABUN = '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "     PICKUPDATE = SYSDATE  ";
                SQL = SQL + ComNum.VBLF + "  WHERE GBSTATUS = ' '";
                SQL = SQL + ComNum.VBLF + "    AND ORDERNO = " + OrderNo;

                //SQL = SQL + ComNum.VBLF + "    AND EXISTS";
                //SQL = SQL + ComNum.VBLF + "    AND EXISTS";
                //SQL = SQL + ComNum.VBLF + "    (";
                //SQL = SQL + ComNum.VBLF + "         SELECT 1";
                //SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER";
                //SQL = SQL + ComNum.VBLF + "          WHERE EMRNO       = " + EmrNo;
                //SQL = SQL + ComNum.VBLF + "            AND ITEMCD      = '" + ItemCd + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ITEMVALUE   = '" + ItemValue + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ORDERNO     = A.ORDERNO";
                //SQL = SQL + ComNum.VBLF + "            AND GBSTATUS    = ' '";
                //SQL = SQL + ComNum.VBLF + "    )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(msgForm, "처방자료를 저장하는데 오류가 발생되었습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }


                SQL = " INSERT INTO KOSMOS_OCS.OCS_IORDER(PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, ";
                SQL = SQL + ComNum.VBLF + "BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, ";
                SQL = SQL + ComNum.VBLF + "GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, ";
                SQL = SQL + ComNum.VBLF + "WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, GBPICKUP, PICKUPSABUN, ";
                SQL = SQL + ComNum.VBLF + "PICKUPDATE, SUBUL_WARD, CORDERCODE, CSUCODE, CBUN )    ";
                SQL = SQL + ComNum.VBLF + "SELECT PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, ";
                SQL = SQL + ComNum.VBLF + "CONTENTS, BCONTENTS, REALQTY, QTY, -1 * REALNAL,  -1 * NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, ";
                SQL = SQL + ComNum.VBLF + "GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, '*', GBPOSITION, 'D-', '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "SYSDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE , '*', ";
                SQL = SQL + ComNum.VBLF + "'" + clsType.User.Sabun + "', SYSDATE, SUBUL_WARD, ";
                SQL = SQL + ComNum.VBLF + "   ORDERCODE, SUCODE, BUN ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER A";
                SQL = SQL + ComNum.VBLF + "  WHERE GBSTATUS = 'D'";
                SQL = SQL + ComNum.VBLF + "    AND ORDERNO = " + OrderNo;
                //SQL = SQL + ComNum.VBLF + "    AND EXISTS";
                //SQL = SQL + ComNum.VBLF + "    (";
                //SQL = SQL + ComNum.VBLF + "         SELECT 1";
                //SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER";
                //SQL = SQL + ComNum.VBLF + "          WHERE EMRNO       = " + EmrNo;
                //SQL = SQL + ComNum.VBLF + "            AND ITEMCD      = '" + ItemCd + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ITEMVALUE   = '" + ItemValue + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ORDERNO     = A.ORDERNO";
                //SQL = SQL + ComNum.VBLF + "            AND GBSTATUS    = ' '";
                //SQL = SQL + ComNum.VBLF + "    )";
                //SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(msgForm, "처방자료를 저장하는데 오류가 발생되었습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #region 매핑 테이블 UPDATE, INSERT

                SQL = " UPDATE KOSMOS_EMR.AEMRSUGAMAPPING_ORDER A";
                SQL = SQL + ComNum.VBLF + " SET GBSTATUS     = 'D'";
                SQL = SQL + ComNum.VBLF + "WHERE ORDERNO     = " + OrderNo;
                SQL = SQL + ComNum.VBLF + "  AND GBSTATUS    = ' '";
                SQL = SQL + ComNum.VBLF + "  AND EMRNO       = " + EmrNo;
                SQL = SQL + ComNum.VBLF + "  AND ITEMCD      = '" + ItemCd + "' ";
                SQL = SQL + ComNum.VBLF + "  AND ITEMVALUE   = '" + ItemValue + "' ";

                //SQL = SQL + ComNum.VBLF + "  WHERE EXISTS";
                //SQL = SQL + ComNum.VBLF + "    (";
                //SQL = SQL + ComNum.VBLF + "         SELECT 1";
                //SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER";
                //SQL = SQL + ComNum.VBLF + "          WHERE EMRNO       = " + EmrNo;
                //SQL = SQL + ComNum.VBLF + "            AND ITEMCD      = '" + ItemCd + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ITEMVALUE   = '" + ItemValue + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ORDERNO     = A.ORDERNO";
                //SQL = SQL + ComNum.VBLF + "            AND GBSTATUS    = ' '";
                //SQL = SQL + ComNum.VBLF + "    )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(msgForm, "처방자료를 저장하는데 오류가 발생되었습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "INSERT INTO KOSMOS_EMR.AEMRSUGAMAPPING_ORDER (EMRNO, ORDERNO, GBSTATUS, ITEMCD, ITEMVALUE, WRITEDATE, WRITETIME, WRITEUSEID) ";
                SQL = SQL + ComNum.VBLF + "  SELECT  ";
                SQL = SQL + ComNum.VBLF + "         " + EmrNo;
                SQL = SQL + ComNum.VBLF + "       , ORDERNO";
                SQL = SQL + ComNum.VBLF + "       , 'D-'";
                SQL = SQL + ComNum.VBLF + "       , '" + ItemCd + "'";
                SQL = SQL + ComNum.VBLF + "       , '" + ItemValue + "'";
                SQL = SQL + ComNum.VBLF + "       , TO_CHAR(SYSDATE, 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "       , TO_CHAR(SYSDATE, 'HH24MISS')";
                SQL = SQL + ComNum.VBLF + "       , '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER A";
                SQL = SQL + ComNum.VBLF + "  WHERE GBSTATUS = 'D'";
                SQL = SQL + ComNum.VBLF + "    AND ORDERNO = " + OrderNo;

                //SQL = SQL + ComNum.VBLF + "    AND EXISTS";
                //SQL = SQL + ComNum.VBLF + "    (";
                //SQL = SQL + ComNum.VBLF + "         SELECT 1";
                //SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER";
                //SQL = SQL + ComNum.VBLF + "          WHERE EMRNO      = " + EmrNo;
                //SQL = SQL + ComNum.VBLF + "            AND ITEMCD     = '" + ItemCd + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ITEMVALUE  = '" + ItemValue + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ORDERNO    = A.ORDERNO";
                //SQL = SQL + ComNum.VBLF + "            AND GBSTATUS   = 'D'";
                //SQL = SQL + ComNum.VBLF + "    )";
                //SQL = SQL + ComNum.VBLF + "    AND NOT EXISTS";
                //SQL = SQL + ComNum.VBLF + "    (";
                //SQL = SQL + ComNum.VBLF + "         SELECT 1";
                //SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRSUGAMAPPING_ORDER";
                //SQL = SQL + ComNum.VBLF + "          WHERE EMRNO      = " + EmrNo;
                //SQL = SQL + ComNum.VBLF + "            AND ITEMCD     = '" + ItemCd + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ITEMVALUE  = '" + ItemValue + "' ";
                //SQL = SQL + ComNum.VBLF + "            AND ORDERNO    = A.ORDERNO";
                //SQL = SQL + ComNum.VBLF + "            AND GBSTATUS   = 'D-'";
                //SQL = SQL + ComNum.VBLF + "    )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(msgForm, "처방자료를 저장하는데 오류가 발생되었습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                #endregion
                #endregion

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(msgForm, ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }


        /// <summary>
        /// 처방 저장
        /// </summary>
        /// <param name="msgForm"></param>
        /// <param name="ss2"></param>
        /// <param name="pAcp"></param>
        /// <param name="strBdate"></param>
        /// <param name="EmrNo"></param>
        public static void SaveOrderData(Form msgForm, FpSpread ss2, EmrPatient pAcp, string strBdate, double EmrNo)
        {
            if (EmrNo == 0)
                return;

            int j = 0;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            int nOrderNo = 0;

            string strPano = string.Empty;
            string strDeptCode = string.Empty;
            string StrDrCode = string.Empty;
            string strSabun = string.Empty;
            string strSlipno = string.Empty;
            string strBun = string.Empty;
            string strORDERCODE = string.Empty;
            string strOrderName = string.Empty;
            string strSucode = string.Empty;
            string strDosCode = string.Empty;
            int nQty = 0;
            string strWardCode = string.Empty;
            string strROOMCODE = string.Empty;
            string strGBInfo = string.Empty;
            string strOrderSite = string.Empty;
            string strBi = string.Empty;

            string strSubulWard = string.Empty;
            string strHighRisk = string.Empty;
            string strGbNgt = string.Empty;

            string strSex = string.Empty;

            string strGbSTS = string.Empty;
            string strSName = string.Empty;
            string strOK = "OK";

            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            //    return; //권한 확인

     

            //if (optGB_0.Checked == true)
            //    strOrderSite = "D";
            //if (optGB_1.Checked == true)
            //    strOrderSite = "E";
            //if (optGB_2.Checked == true)
            //    strOrderSite = "N";


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                #region 오더 등록

                strROOMCODE = pAcp.room;
                strPano = pAcp.ptNo;
                //strROWID    = "";
                strDeptCode = pAcp.medDeptCd;
                StrDrCode = pAcp.medDrCd;
                strWardCode = pAcp.ward;
                strBi = pAcp.bi;

                for (j = 0; j < ss2.ActiveSheet.Rows.Count; j++)
                {
                    if (ss2.ActiveSheet.Cells[j, 0].Text.Equals("True"))
                    {
                        if (ss2.ActiveSheet.Cells[j, 4].Tag != null)
                        {
                            continue;
                        }

                        SQL = "   SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
                        SQL = SQL + ComNum.VBLF + "  WHERE DRCODE = '" + StrDrCode + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND GBOUT = 'N' ";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(msgForm, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strSabun = ComFunc.SetAutoZero(dt.Rows[0]["SABUN"].ToString().Trim(), 5);
                        }
                        dt.Dispose();
                        dt = null;


                        nQty = (int)VB.Val(ss2.ActiveSheet.Cells[j, 3].Text);
                        strORDERCODE = ss2.ActiveSheet.Cells[j, 4].Text.Trim();
                        strSucode = ss2.ActiveSheet.Cells[j, 5].Text.Trim();
                        strSlipno = ss2.ActiveSheet.Cells[j, 7].Text.Trim();
                        strBun = ss2.ActiveSheet.Cells[j, 8].Text.Trim();
                        strGBInfo = ss2.ActiveSheet.Cells[j, 9].Text.Trim();
                        strSubulWard = VB.Right(ss2.ActiveSheet.Cells[j, 12].Text.Trim(), 2);
                        //'2016-06-24 계장 김현욱) 의료급여 환자의 분유수가 제한 요청
                        if (strBi == "21" || strBi == "22")
                        {
                            if (strORDERCODE == "Y5000" || strORDERCODE == "Y5001")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(msgForm, "의료급여환자의 분유수가는 'Z9000'입니다. 확인하시기 바랍니다.");
                                return;
                            }
                        }

                        strGbNgt = "";
                        strHighRisk = "";
                        if (READ_CHILDBIRTH_SUGA(msgForm, strORDERCODE) == true)
                        {
                            clsEmrPublic.GstrHighRisk = "";
                            clsEmrPublic.GstrOP = "";
                            //GstrHelpCode = strORDERCODE;

                            //FrmHighRiskWomen
                            using (frmHighRiskWomen_EMR frmHighRiskWomenX = new frmHighRiskWomen_EMR())
                            {
                                frmHighRiskWomenX.rSetGstr += FrmHighRiskWomenX_rSetGstr;
                                frmHighRiskWomenX.ShowDialog(msgForm);
                            }

                            strHighRisk = clsEmrPublic.GstrHighRisk;
                            strGbNgt = clsEmrPublic.GstrOP;

                            if (strHighRisk.IsNullOrEmpty() || strGbNgt.IsNullOrEmpty())
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(msgForm, "분만의 경우 구분을 선택하여야 합니다. 고위험 구분이 선택되지 않았습니다.");
                                strOK = "NO";
                                return;
                            }

                            clsEmrPublic.GstrHighRisk = "";
                            clsEmrPublic.GstrOP = "";
                        }

                        if (strSlipno != "A7")
                        {
                            strOrderSite = "TEL";
                        }
                        else
                        {
                            strOrderSite = " ";
                        }


                        strDosCode = "";
                        strDosCode = READ_DOSCODE(msgForm, strORDERCODE);


                        //'<오더등록>---------------------------------------------------------------------------------------
                        SQL = "SELECT KOSMOS_OCS.SEQ_ORDERNO.NextVal nNEXTVAL FROM DUAL";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(msgForm, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            nOrderNo = (int)VB.Val(dt.Rows[0]["nNEXTVAL"].ToString().Trim());
                        }
                        dt.Dispose();
                        dt = null;


                        SQL = "INSERT INTO KOSMOS_OCS.OCS_IORDER ( Ptno, BDate,     Seqno,     DeptCode,  ";
                        SQL = SQL + ComNum.VBLF + "  DrCode,  StaffID,    Slipno,    OrderCode, SuCode,  Bun,      ";
                        SQL = SQL + ComNum.VBLF + "  GbOrder, Contents,   BContents, RealQty,   Qty,     RealNal,  ";
                        SQL = SQL + ComNum.VBLF + "  Nal,     DosCode,    GbInfo,    GbSelf,    GbSpc,   GbNgt,    ";
                        SQL = SQL + ComNum.VBLF + "  GbER,    GbPRN,      GbDiv,     GbBoth,    GbAct,   GbTFlag,  ";
                        SQL = SQL + ComNum.VBLF + "  GbSend,  GbPosition, GbStatus,  NurseID,   EntDate, WardCode, ";
                        SQL = SQL + ComNum.VBLF + "  RoomCode, Bi,        OrderNo,   Remark, ";
                        SQL = SQL + ComNum.VBLF + "  ActDate, GbGroup,    GbPort,    OrderSite, GBPICKUP, PICKUPSABUN, ";
                        SQL = SQL + ComNum.VBLF + "  PICKUPDATE, SUBUL_WARD, HIGHRISK, ";
                        SQL = SQL + ComNum.VBLF + "  CORDERCODE, CSUCODE, CBUN ) VALUES     ";
                        SQL = SQL + ComNum.VBLF + "( '" + strPano + "',     TO_DATE('" + strBdate + "','YYYY-MM-DD'),  ";
                        SQL = SQL + ComNum.VBLF + "    999 ,     '" + strDeptCode + "',    ";
                        SQL = SQL + ComNum.VBLF + "  '" + strSabun + "',   '" + StrDrCode + "',    '" + strSlipno + "',   ";
                        SQL = SQL + ComNum.VBLF + "  '" + strORDERCODE + "','" + strSucode + "',     '" + strBun + "',      ";
                        SQL = SQL + ComNum.VBLF + "  '',  0,0,";
                        SQL = SQL + ComNum.VBLF + "  " + nQty + ",   " + nQty + ",1,   ";
                        SQL = SQL + ComNum.VBLF + "   1, '" + strDosCode + "',    '" + strGBInfo + "',   ";
                        SQL = SQL + ComNum.VBLF + "  '',   '',      '" + strGbNgt + "',    ";
                        SQL = SQL + ComNum.VBLF + "  '',     ' ',   1 ,    ";
                        SQL = SQL + ComNum.VBLF + "  '0',   '',      '',  ";
                        SQL = SQL + ComNum.VBLF + "  '*',   '', ' ', ";
                        SQL = SQL + ComNum.VBLF + "  '" + clsType.User.Sabun.PadLeft(5, '0') + "',  SysDate,   '" + strWardCode + "', ";
                        SQL = SQL + ComNum.VBLF + "  '" + strROOMCODE + "', '" + strBi + "', " + nOrderNo + ", '', ";
                        SQL = SQL + ComNum.VBLF + "  TRUNC(SYSDATE),   '',  ";
                        SQL = SQL + ComNum.VBLF + "  '',   '" + strOrderSite + "' , '*',  '" + clsType.User.Sabun.PadLeft(5, '0') + "', SYSDATE,";
                        SQL = SQL + ComNum.VBLF + "  '" + strSubulWard + "', '" + strHighRisk + "', ";
                        SQL = SQL + ComNum.VBLF + "  '" + strORDERCODE + "','" + strSucode + "','" + strBun + "' ) ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(msgForm, "처방자료를 저장하는데 오류가 발생되었습니다");
                            Cursor.Current = Cursors.Default;
                            strOK = "NO";
                            return;
                        }


                        #region 매핑 테이블 INSERT
                        string ItemCd    = ss2.ActiveSheet.Cells[j, 1].Tag.ToString();
                        string ItemValue = ss2.ActiveSheet.Cells[j, 2].Tag.ToString();

                        SQL = "INSERT INTO KOSMOS_EMR.AEMRSUGAMAPPING_ORDER (EMRNO, ORDERNO, GBSTATUS, ITEMCD, ITEMVALUE, WRITEDATE, WRITETIME, WRITEUSEID) ";
                        SQL = SQL + ComNum.VBLF + "  SELECT  ";
                        SQL = SQL + ComNum.VBLF + "         " + EmrNo;
                        SQL = SQL + ComNum.VBLF + "       , ORDERNO";
                        SQL = SQL + ComNum.VBLF + "       , GBSTATUS";
                        SQL = SQL + ComNum.VBLF + "       , '" + ItemCd + "'";
                        SQL = SQL + ComNum.VBLF + "       , '" + ItemValue + "'";
                        SQL = SQL + ComNum.VBLF + "       , TO_CHAR(SYSDATE, 'YYYYMMDD')";
                        SQL = SQL + ComNum.VBLF + "       , TO_CHAR(SYSDATE, 'HH24MISS')";
                        SQL = SQL + ComNum.VBLF + "       , '" + clsType.User.IdNumber + "'";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER";
                        SQL = SQL + ComNum.VBLF + "  WHERE PTNO     = '" + pAcp.ptNo + "'";
                        SQL = SQL + ComNum.VBLF + "    AND ORDERNO  = " + nOrderNo;
                        SQL = SQL + ComNum.VBLF + "    AND GBSTATUS = ' ' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(msgForm, "처방자료를 저장하는데 오류가 발생되었습니다");
                            Cursor.Current = Cursors.Default;
                            strOK = "NO";
                            return;
                        }
                        #endregion
                    }
                }
                #endregion

                if (strOK == "OK")
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(msgForm, "등록하였습니다.");
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(msgForm, "등록 중 오류가 발생했습니다.");
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(msgForm, ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 분만수가 여부 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>        
        /// <seealso cref="nrinfo02.bas : READ_분만수가"/>
        /// <returns></returns>
        public static bool READ_CHILDBIRTH_SUGA(Form msgForm, string ArgSuCode)
        {
            bool rtnVal = false;
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            string strCODE = string.Empty;

            if (string.Compare(clsPublic.GstrSysDate, "2016-11-01") < 0)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += " SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += "  WHERE GUBUN = 'OCS_분만수가코드' ";
                SqlErr = clsDB.GetDataTableREx(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(msgForm, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        strCODE = strCODE + "'" + Dt.Rows[i]["CODE"].ToString().Trim() + "',";
                    }

                    if (strCODE.Trim() != "" && VB.Right(strCODE, 1) == ",")
                    {
                        strCODE = VB.Mid(strCODE, 1, VB.Len(strCODE) - 1);
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (strCODE.Trim() != "")
                {
                    SQL = "";
                    SQL += " SELECT ORDERCODE FROM " + ComNum.DB_MED + "OCS_ORDERCODE   ";
                    SQL += "  WHERE SUCODE IN (" + strCODE + ")                         ";
                    SQL += "    AND ORDERCODE = '" + ArgSuCode + "'                     ";
                    SqlErr = clsDB.GetDataTableREx(ref Dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(msgForm, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                        rtnVal = true;

                    Dt.Dispose();
                    Dt = null;

                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(msgForm, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public static void FrmHighRiskWomenX_rSetGstr(string strGbn, string strTime)
        {
            clsEmrPublic.GstrHighRisk = strGbn;
            clsEmrPublic.GstrOP = strTime;
        }

        public static string READ_DOSCODE(Form msgForm, string ArgOrderCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT SPECCODE  FROM KOSMOS_OCS.OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE ORDERCODE ='" + ArgOrderCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(msgForm, "조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["specCODE"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(msgForm, ex.Message);
            }
            return rtnVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrForm"></param>
        /// <param name="emrPatient"></param>
        /// <param name="pageCount"></param>
        /// <param name="UpdateOcrNo">비었을경우 시퀀스 생성</param>
        /// <returns></returns>
        public static string GetOcrNo(EmrForm emrForm, EmrPatient emrPatient, int pageCount, long UpdateOcrNo = 0)
        {

            long ocrNo = 0;
            DataTable dt = null;
            string SQL      = string.Empty;
            string SqlErr   = string.Empty;
            int intRowAffected = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                if (UpdateOcrNo == 0)
                {
                    SQL = "";
                    SQL = SQL + "SELECT " + ComNum.DB_EMR + "SEQ_AEMROCRPRTHIS_OCRNO.nextVal as ocrNo FROM Dual";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    ocrNo = long.Parse(dt.Rows[0]["ocrNo"].ToString());
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    ocrNo = UpdateOcrNo;
                }

                StringBuilder strSql = new StringBuilder();

                if (UpdateOcrNo == 0)
                {
                    strSql.AppendLine("INSERT INTO KOSMOS_EMR.AEMROCRPRTHIS ");
                    strSql.AppendLine(" (OCRNO, FORMCODE, ACPNO, OCRDATE, OCRTIME, PTNO, PTNAME, INOUTCLS, MEDFRDATE, MEDDEPTCD, WARDCODE, FORMNO, UPDATENO, USEID, PAGECOUNT ) ");
                    strSql.AppendLine("VALUES (             ");
                    strSql.AppendLine("      " + ocrNo + ",");
                    strSql.AppendLine("      '012',");
                    strSql.AppendLine("      " + emrPatient.acpNo + ",");
                    strSql.AppendLine("      TO_CHAR(SYSDATE, 'YYYYMMDD'),");
                    strSql.AppendLine("      TO_CHAR(SYSDATE, 'hh24miss'),");
                    strSql.AppendLine("      '" + emrPatient.ptNo + "',");
                    strSql.AppendLine("      '" + emrPatient.ptName + "',");
                    strSql.AppendLine("      '" + emrPatient.inOutCls + "',");
                    strSql.AppendLine("      '" + emrPatient.medFrDate + "',");
                    strSql.AppendLine("      '" + emrPatient.medDeptCd + "',");
                    strSql.AppendLine("      '" + emrPatient.ward + "',");
                    strSql.AppendLine("      '" + emrForm.FmFORMNO + "',");
                    strSql.AppendLine("      '" + emrForm.FmUPDATENO + "',");
                    strSql.AppendLine("      '" + clsType.User.IdNumber + "',");
                    strSql.AppendLine("      " + pageCount);
                    strSql.AppendLine(")");
                }
                else
                {
                    strSql.AppendLine("UPDATE KOSMOS_EMR.AEMROCRPRTHIS ");
                    strSql.AppendLine(" SET PAGECOUNT = " + pageCount);
                    strSql.AppendLine("WHERE OCRNO = " + ocrNo);
                }

                SqlErr = clsDB.ExecuteNonQueryEx(strSql.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return string.Empty;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return ocrNo.ToString().PadLeft(13, '0');

            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                return string.Empty;
            }
        }

        /// <summary>
        /// 인증저장 했는지 여부.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static bool EmrCertSave(PsmhDb pDbCon, string strEmrNo)
        {
            bool rtnVal = false;
            if (VB.Val(strEmrNo) == 0)
                return rtnVal;

            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT SAVECERT";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
            SQL += ComNum.VBLF + "WHERE EMRNO = " + VB.Val(strEmrNo);

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Equals("1");
            }

            reader.Dispose();

            return rtnVal;
        }

        public static void ErPatientFrTime(PsmhDb pDbCon, string strEmrNo, ref EmrPatient AcpEmr)
        {
            if (VB.Val(strEmrNo) == 0)
                return;

            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT MEDFRTIME";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
            SQL += ComNum.VBLF + "WHERE EMRNO = " + VB.Val(strEmrNo);

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return;
            }

            if (reader.HasRows &&  reader.Read())
            {
                DateTime dtpFrTime = new DateTime();
                if (DateTime.TryParseExact(reader.GetValue(0).ToString().Trim(), "HHmmss", null, System.Globalization.DateTimeStyles.None, out dtpFrTime))
                {
                    AcpEmr.medFrTime = dtpFrTime.ToString("HHmmss");
                }
                else
                {
                    return;
                }
            }

            reader.Dispose();
        }

        /// <summary>
        /// 신규 간호 기록 관련 EMR 저장(정형화)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="AcpEmr">환자정보</param>
        /// <param name="pForm">기록지 정보</param>
        /// <param name="strContent">내용</param>
        /// <returns></returns>
        public static double SaveNurChartROW(PsmhDb pDbCon, Form ptForm, double dblEmrNo, EmrPatient AcpEmr, EmrForm pForm, string strChartDate, string strChartTime, Dictionary<string, string> strContent, bool Trans = false)
        {

            #region 변수
            double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            string mFLOWGB = string.Empty;
            #endregion


            if (Trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            try
            {
                FormXml[] mFormXml = FormDesignQuery.GetDataFormXml(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString());
                if (mFormXml == null)
                {
                    return 0;
                }

                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                
                if (dblEmrNo > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo,
                        dtpSys.ToString("yyyyMMdd"),
                        dtpSys.ToString("HHmmss"), "C", "", clsType.User.IdNumber);
                    if (SqlErr != "OK")
                    {
                        if (Trans)
                        {
                            clsDB.setBeginTran(pDbCon);
                        }
                        ComFunc.MsgBoxEx(ptForm, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                    #endregion
                    dblEmrNoNew = dblEmrNo;
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    if (Trans)
                    {
                        clsDB.setBeginTran(pDbCon);
                    }
                    ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return dblEmrNoNew;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                foreach(KeyValuePair<string, string> keyValue in strContent)
                {
                    string ItemVal = keyValue.Value;
                    string ItemType = mFormXml.Where(d => d.strCONTROLNAME.Equals(keyValue.Key)).FirstOrDefault()?.strCONTROTYPE;

                    if (!string.IsNullOrEmpty(ItemType))
                    {
                        switch (ItemType)
                        {
                            case "System.Windows.Forms.CheckBox":
                                ItemType = "CHECK";
                                break;
                            case "System.Windows.Forms.RadioButton":
                                ItemType = "RADIO";
                                break;
                            case "System.Windows.Forms.TextBox":
                                ItemType = "TEXT";
                                break;
                        }

                        if (ItemType.Equals("RADIO") || ItemType.Equals("CHECK"))
                        {
                            ItemVal = ItemVal.ToUpper().Equals("TRUE") ? "1" : "0";
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + keyValue.Key + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + (keyValue.Key.IndexOf("_") == -1 ? keyValue.Key : keyValue.Key.Split('_')[0]) + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + ItemVal.Replace("'", "`") + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + "0,";   //DSPSEQ
                        SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            if (Trans)
                            {
                                clsDB.setBeginTran(pDbCon);
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return dblEmrNoNew;
                        }
                    }
                }

                #endregion //CHARTROW

                if (Trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }

                #region //전자인증 하기
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(pDbCon, dblEmrNoNew, Trans);
                        }
                    }
                }
                #endregion

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (Trans)
                {
                    clsDB.setBeginTran(pDbCon);
                }
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(ptForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return dblEmrNoNew;
        }


        /// <summary>
        /// EMR 생성 : 신규 간호정보 조사지
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInDate"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strCHARTTIME"></param>
        /// <param name="strData"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool SAVE_2678(PsmhDb pDbCon, Form pForm, double dblEmrNo, string strPtno, 
                                     string strInDate, string strInTime, string strOutDate, string strOutTime, string strDrCd,
                                     string strChartDate, string strChartTime, string strUseId, ref double rtndblEmrNo)
        {

            bool rtVal = false;
            EmrPatient AcpEmr = null; //clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, "O", strInDate.Replace("-", ""), "ER");
            EmrForm ppForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "2678");
            FormXml[] mFormXml = FormDesignQuery.GetDataFormXml(ppForm.FmFORMNO.ToString(), ppForm.FmUPDATENO.ToString());
            if (mFormXml == null )
            {
                return rtVal;
            }

            if (AcpEmr == null)
            {
                AcpEmr = clsEmrChart.ClearPatient();
                AcpEmr.ptNo = strPtno;
                AcpEmr.inOutCls = "O";
                AcpEmr.medDeptCd = "ER";
                AcpEmr.acpNo = "0";
            }

            AcpEmr.medDrCd = strDrCd;

            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            double dblEmrNoNew = 0;
            strChartDate = strChartDate.Replace("-", "");
            strChartTime = strChartTime.Replace(":", "");

            Dictionary<string, string> keysControl = new Dictionary<string, string>();

            //입실일자
            if (strInDate != "" && strInDate.IndexOf("-") != -1)
            {
                AcpEmr.medFrDate = Convert.ToDateTime(strInDate).ToString("yyyyMMdd");
            }
            if (strInTime != "" && strInTime.IndexOf(":") != -1)
            {
                AcpEmr.medFrTime = Convert.ToDateTime(strInTime + ":00").ToString("HHmmss");
            }
            //퇴실일자
            if (strOutDate != "" && strOutDate.IndexOf("-") != -1)
            {
                AcpEmr.medEndDate = Convert.ToDateTime(strOutDate).ToString("yyyyMMdd");
            }
            if (strOutTime != "" && strOutTime.IndexOf(":") != -1)
            {
                AcpEmr.medEndTime = Convert.ToDateTime(strOutTime).ToString("HHmmss");
            }

            try
            {
                #region 컨트롤 => ITEMCD 매핑
                //txtHusongName
                keysControl.Add("txtInCarno", "I0000034152");
                keysControl.Add("txtChartDate", "I0000015537");
                keysControl.Add("txtChartTime", "I0000029535");
                keysControl.Add("txtInDate", "I0000034052_1");
                keysControl.Add("txtInTime", "I0000034052_2");
                keysControl.Add("cboInRt", "I0000029265");
                keysControl.Add("cboInMn", "I0000029266");
                keysControl.Add("txtAkDt", "I0000001226");
                keysControl.Add("txtAkTm", "I0000035463");
                //keysControl.Add("optir1", "I0000034274");
                //keysControl.Add("optir2", "I0000034276");
                keysControl.Add("tex_Firstime_1", "I0000034150");
                keysControl.Add("txtINFOPAT", "I0000034149");
                keysControl.Add("txtHPhone", "I0000032868_1");
                keysControl.Add("cboEmSy", "I0000035462");
                keysControl.Add("cboArCs", "I0000035128");
                keysControl.Add("cboTaip", "I0000035129");
                keysControl.Add("ChkTa_1", "I0000035131");
                keysControl.Add("ChkTa_2", "I0000035132");
                keysControl.Add("ChkTa_3", "I0000035133");
                keysControl.Add("ChkTa_4", "I0000035134");
                keysControl.Add("ChkTa_5", "I0000035135");
                keysControl.Add("ChkTa_6", "I0000035136");
                keysControl.Add("ChkTa_7", "I0000038117");
                keysControl.Add("chkTsNo", "I0000035137");
                keysControl.Add("chkTsUr", "I0000035138");
                keysControl.Add("chkTsUk", "I0000035139");
                keysControl.Add("chkik_1", "I0000011727");
                keysControl.Add("chkik_2", "I0000010573");
                keysControl.Add("chkik_3", "I0000014736");
                keysControl.Add("chkik_4", "I0000034157");
                keysControl.Add("txtHiBp", "I0000002018");
                keysControl.Add("txtLoBp", "I0000001765");
                keysControl.Add("txtPuLs", "I0000014815");
                keysControl.Add("txtBrTh", "I0000002009");
                keysControl.Add("txtBdHt", "I0000001811");
                keysControl.Add("cboAORT", "I0000035464");
                keysControl.Add("optir27", "I0000001195");
                keysControl.Add("optir28", "I0000002159");
                keysControl.Add("txtit144", "I0000034153");
                keysControl.Add("txtInSts", "I0000034158");
                keysControl.Add("optir7", "I0000034385");
                keysControl.Add("optir6", "I0000034386");
                keysControl.Add("ik64", "I0000034384");
                keysControl.Add("txtOpInfo", "I0000034159");
                keysControl.Add("chkik_32", "I0000029075");
                keysControl.Add("chkik_33", "I0000029755");
                keysControl.Add("chkik_34", "I0000034160");
                keysControl.Add("chkik_52", "I0000034161");
                keysControl.Add("chkik_53", "I0000035158");
                keysControl.Add("chkik_54", "I0000034162");
                keysControl.Add("chkik_61", "I0000035160");
                keysControl.Add("chkik_35", "I0000035161_1");
                keysControl.Add("txtik44BiGo", "I0000035161_2");
                keysControl.Add("txtBW_0", "I0000000418");

                keysControl.Add("cboArCf", "I0000035127");
                keysControl.Add("cboDgKd", "I0000035126");

                keysControl.Add("rbWound1", "I0000034245");
                keysControl.Add("rbWound2", "I0000034246");
                keysControl.Add("txtWound", "I0000022364");

                keysControl.Add("txtNation", "I0000018981");
                #endregion

                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                if (dblEmrNo > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo,
                        dtpSys.ToString("yyyyMMdd"),
                        dtpSys.ToString("HHmmss"), "C", "", strUseId);
                    if (SqlErr != "OK")
                    {
                        //clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBoxEx(pForm, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                    #endregion
                    dblEmrNoNew = dblEmrNo;
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                rtndblEmrNo = dblEmrNoNew;


                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, "2678", ppForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    strUseId, strUseId, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                string strItemCd = string.Empty;
                foreach (Control control in ComFunc.GetAllControls(pForm))
                {
                    if (keysControl.ContainsKey(control.Name) == false)
                        continue;

                    if (keysControl.TryGetValue(control.Name, out strItemCd))
                    {
                        string ItemValue = string.Empty;
                        string ItemType = string.Empty;

                        //if (strItemCd.Equals("I0000034152"))
                        //{
                        //    strItemCd = strItemCd;
                        //}

                        if (control.Name.Equals("cboDgKd") || control.Name.Equals("cboArCf") || control.Name.Equals("cboInRt"))
                        {
                            ItemValue = VB.Mid(control.Text, 3, control.Text.Length);
                            if (string.IsNullOrWhiteSpace(ItemValue))
                            {
                                continue;
                            }

                            FormXml formXml = mFormXml.Where(d => d.strCONTROTYPE.Equals("System.Windows.Forms.RadioButton") && d.strCONTROLPARENT.Equals(control.Name) && d.strTEXT.Equals(ItemValue))?.FirstOrDefault();
                            ItemType = formXml?.strCONTROTYPE;
                            strItemCd = formXml?.strCONTROLNAME;
                        }
                        else
                        {
                            ItemType = mFormXml.Where(d => d.strCONTROLNAME.Equals(strItemCd)).FirstOrDefault()?.strCONTROTYPE;

                        }

                        //if (ItemType == null)
                        //    continue;

                        switch (ItemType)
                        {
                            case "System.Windows.Forms.CheckBox":
                                ItemType = "CHECK";
                                break;
                            case "System.Windows.Forms.RadioButton":
                                ItemType = "RADIO";
                                break;
                            case "System.Windows.Forms.TextBox":
                                ItemType = "TEXT";
                                break;
                        }

                        if (control.Name.Equals("cboDgKd") == false && control.Name.Equals("cboArCf") == false &&
                            control.Name.Equals("cboInRt") == false)
                        {
                            if (control is CheckBox)
                            {
                                ItemValue = (control as CheckBox).Checked ? "1" : "0";
                            }
                            else if (control is RadioButton)
                            {
                                ItemValue = (control as RadioButton).Checked ? "1" : "0";
                            }
                            else if (control is TextBox || control is ComboBox)
                            {
                                ItemValue = control.Text.Trim();
                            }
                        }
                        else
                        {
                            ItemValue = (ItemType.Equals("RADIO") || ItemType.Equals("CHECK") ? "1" : "0");
                        }
                           

                        switch (control.Name)
                        {
                            case "cboInMn":
                                ItemValue = VB.Mid(control.Text, 3, control.Text.Length);
                                break;
                            case "cboArCs":
                                ItemValue = VB.Mid(control.Text, 4, control.Text.Length);
                                break;
                            case "cboEmSy":
                                if (VB.Left(control.Text, 1) == "Y")
                                {
                                    ItemValue = "응급";
                                }
                                else if (VB.Left(control.Text, 1) == "N")
                                {
                                    ItemValue = "비응급";
                                }
                                else
                                {
                                    ItemValue = "";
                                }
                                break;
                        }

                        string strITEMVALUE = string.Empty;
                        string strITEMVALUE1 = string.Empty;
                        string strITEMVALUETOT = ItemValue.Trim().Replace("'", "`");

                        int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                        int intLen = 3999;
                        if (intLenTot > 3999)
                        {
                            string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                            if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                            {
                                intLen = 3998;
                            }
                            strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                            strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                        }
                        else
                        {
                            strITEMVALUE = strITEMVALUETOT;
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + strITEMVALUE + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + "0,";   //DSPSEQ
                        SQL += ComNum.VBLF + "'" + strITEMVALUE1 + "',";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + strUseId + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return false;
                        }
                    }
                }

                #region 알러지
                Dictionary<string, string> keyAllergy = FormPatInfoFunc.Set_FormPatInfo_AUTO_ALLERGY(pDbCon, AcpEmr);
                if (keyAllergy.Count > 0)
                {
                    foreach(KeyValuePair<string, string> keyValue in keyAllergy)
                    {
                        string ItemType = mFormXml.Where(d => d.strCONTROLNAME.Equals(keyValue.Key)).FirstOrDefault()?.strCONTROTYPE;
                        switch (ItemType)
                        {
                            case "System.Windows.Forms.CheckBox":
                                ItemType = "CHECK";
                                break;
                            case "System.Windows.Forms.RadioButton":
                                ItemType = "RADIO";
                                break;
                            case "System.Windows.Forms.TextBox":
                                ItemType = "TEXT";
                                break;
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + keyValue.Key + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + (keyValue.Key.IndexOf("_") != -1 ? keyValue.Key.Split('_')[0] : keyValue.Key) + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + keyValue.Value  + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + "0,";   //DSPSEQ
                        SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + strUseId + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return false;
                        }
                    }
                }
                else
                {
                    SQL = string.Empty;
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                    SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                    SQL += ComNum.VBLF + "VALUES (";
                    SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                    SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                    SQL += ComNum.VBLF + "'I0000034274',";   //ITEMCD
                    SQL += ComNum.VBLF + "'I0000034274',"; //ITEMNO
                    SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                    SQL += ComNum.VBLF + "'RADIO',";   //ITEMTYPE
                    SQL += ComNum.VBLF + "'1',";   //ITEMVALUE
                    SQL += ComNum.VBLF + "0,";   //DSPSEQ
                    SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                    SQL += ComNum.VBLF + "'" + strUseId + "',";   //INPUSEID
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                    SQL += ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        return false;
                    }
                }


                #endregion

                #region KTAS
                SQL = string.Empty;
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                SQL += ComNum.VBLF + "'I0000034151',";   //ITEMCD
                SQL += ComNum.VBLF + "'I0000034151',"; //ITEMNO
                SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                SQL += ComNum.VBLF + "'TEXT',";   //ITEMTYPE
                SQL += ComNum.VBLF + "'" + "KTAS " + READ_MAX_KTAS(strPtno, strInDate, strInTime) + "',";   //ITEMVALUE
                SQL += ComNum.VBLF + "0,";   //DSPSEQ
                SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                SQL += ComNum.VBLF + "'" + strUseId + "',";   //INPUSEID
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    return false;
                }
                #endregion


                //#region 상처 무 기본값
                //SQL = string.Empty;
                //SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                //SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                //SQL += ComNum.VBLF + "VALUES (";
                //SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                //SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                //SQL += ComNum.VBLF + "'I0000034245',";   //ITEMCD
                //SQL += ComNum.VBLF + "'I0000034245',"; //ITEMNO
                //SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                //SQL += ComNum.VBLF + "'RADIO',";   //ITEMTYPE
                //SQL += ComNum.VBLF + "'1',";   //ITEMVALUE
                //SQL += ComNum.VBLF + "0,";   //DSPSEQ
                //SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                //SQL += ComNum.VBLF + "'" + strUseId + "',";   //INPUSEID
                //SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                //SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                //SQL += ComNum.VBLF + ")";

                //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                //if (SqlErr != "")
                //{
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                //    return false;
                //}
                //#endregion

                #endregion //CHARTROW

                #region //전자인증 하기
                //if (System.Diagnostics.Debugger.IsAttached == false)
                //{
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, strUseId) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(pDbCon, dblEmrNoNew, false);
                        }
                    }
                //}
                #endregion


                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        // <summary>
        /// EMR 생성 : 신규 응급간호 퇴실 결과지
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInDate"></param>
        /// <param name="strDrCode"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strCHARTTIME"></param>
        /// <param name="strData"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool SAVE_3129(PsmhDb pDbCon, Form pForm, double dblEmrNo, string strPtno, string strInDate, string strInTime, string strChartDate, string strChartTime, string strUseId, ref double rtndblEmrNo)
        {
            bool rtVal = false;
            EmrPatient AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, "O", strInDate.Replace("-", ""), "ER");
            EmrForm ppForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "3129");
            FormXml[] mFormXml = FormDesignQuery.GetDataFormXml(ppForm.FmFORMNO.ToString(), ppForm.FmUPDATENO.ToString());
            if (mFormXml == null)
            {
                return rtVal;
            }
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            double dblEmrNoNew = 0;
            strChartDate = strChartDate.Replace("-", "");
            strChartTime = strChartTime.Replace(":", "");

            if (AcpEmr == null)
            {
                AcpEmr = clsEmrChart.ClearPatient();
                AcpEmr.ptNo = strPtno;
                AcpEmr.inOutCls = "O";
                AcpEmr.medDeptCd = "ER";
                AcpEmr.acpNo = "0";
            }

            //입실일자
            if (strInDate != "" && strInDate.IndexOf("-") != -1)
            {
                AcpEmr.medFrDate = Convert.ToDateTime(strInDate).ToString("yyyyMMdd");
            }
            if (strInTime != "" && strInTime.IndexOf(":") != -1)
            {
                AcpEmr.medFrTime = Convert.ToDateTime(strInTime + ":00").ToString("HHmmss");
            }

            try
            {
                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                if (dblEmrNo > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo,
                        dtpSys.ToString("yyyyMMdd"),
                        dtpSys.ToString("HHmmss"), "C", "", strUseId);
                    if (SqlErr != "OK")
                    {
                        ComFunc.MsgBoxEx(pForm, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                    #endregion
                    dblEmrNoNew = dblEmrNo;
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                rtndblEmrNo = dblEmrNoNew;

                Control control = pForm.Controls.Find("cboDgKd", true).FirstOrDefault();
                #region 귀가
                if (control != null && control.Text.Length > 3 && control.Text.Substring(2).Equals("진료 외 방문"))
                {
                    Cursor.Current = Cursors.Default;
                    return true;
                }
                #endregion

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, ppForm.FmFORMNO.ToString(), ppForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    strUseId, strUseId, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion //저장 CHRATMAST

                #region  //저장 CHARTROW
                string strItemCd = string.Empty;
                string ItemValue = string.Empty;

                control = pForm.Controls.Find("cboArea", true).FirstOrDefault();
                #region 최종진료구역
                if (control != null)
                {
                    strItemCd = "I0000031631";
                    ItemValue = control.Text.Trim();

                    SQL = string.Empty;
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                    SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                    SQL += ComNum.VBLF + "VALUES (";
                    SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                    SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                    SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                    SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                    SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                    SQL += ComNum.VBLF + "'TEXT',";   //ITEMTYPE
                    SQL += ComNum.VBLF + "'" + ItemValue + "',";   //ITEMVALUE
                    SQL += ComNum.VBLF + "0,";   //DSPSEQ
                    SQL += ComNum.VBLF + "'',";   //ITEMVALUE1
                    SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                    SQL += ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        return false;
                    }
                }
                #endregion

                #region 입원
                //입원경로
                string OutGbn = string.Empty;
                control = pForm.Controls.Find("cboOut", true)?.FirstOrDefault();
                if (control != null)
                {
                    OutGbn = control.Text;
                }

                //cboEmRt
                control = pForm.Controls.Find("cboHsrt", true)?.FirstOrDefault();
                if (OutGbn.Equals("입원") && control != null)
                {
                    strItemCd = "I0000035163";

                    SQL = string.Empty;
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                    SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                    SQL += ComNum.VBLF + "VALUES (";
                    SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                    SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                    SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                    SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                    SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                    SQL += ComNum.VBLF + "'CHECK',";   //ITEMTYPE
                    SQL += ComNum.VBLF + "'1',";   //ITEMVALUE
                    SQL += ComNum.VBLF + "0,";   //DSPSEQ
                    SQL += ComNum.VBLF + "'',";   //ITEMVALUE1
                    SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                    SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                    SQL += ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        return false;
                    }


                    //cboEmRt
                    control = pForm.Controls.Find("cboEmRt", true)?.FirstOrDefault();
                    if (control != null)
                    {
                        strItemCd = mFormXml.Where(d => d.strCONTROTYPE.Equals("System.Windows.Forms.CheckBox") && d.strTEXT.Equals(control.Text.Substring(control.Text.IndexOf(".") + 1)))?.FirstOrDefault()?.strCONTROLNAME;

                        if (string.IsNullOrWhiteSpace(strItemCd) == false)
                        {
                            SQL = string.Empty;
                            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                            SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                            SQL += ComNum.VBLF + "VALUES (";
                            SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                            SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                            SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                            SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                            SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                            SQL += ComNum.VBLF + "'CHECK',";   //ITEMTYPE
                            SQL += ComNum.VBLF + "'1',";   //ITEMVALUE
                            SQL += ComNum.VBLF + "0,";   //DSPSEQ
                            SQL += ComNum.VBLF + "'',";   //ITEMVALUE1
                            SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                            SQL += ComNum.VBLF + ")";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                                return false;
                            }


                            //입원경로
                            strItemCd = "I0000013184";
                            ItemValue = control.Text;

                            SQL = string.Empty;
                            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                            SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                            SQL += ComNum.VBLF + "VALUES (";
                            SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                            SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                            SQL += ComNum.VBLF + "'" + strItemCd + "',";   //ITEMCD
                            SQL += ComNum.VBLF + "'" + (strItemCd.IndexOf("_") != -1 ? strItemCd.Split('_')[0] : strItemCd) + "',"; //ITEMNO
                            SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                            SQL += ComNum.VBLF + "'TEXT',";   //ITEMTYPE
                            SQL += ComNum.VBLF + "'" + ItemValue + "',";   //ITEMVALUE
                            SQL += ComNum.VBLF + "0,";   //DSPSEQ
                            SQL += ComNum.VBLF + "'',";   //ITEMVALUE1
                            SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                            SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                            SQL += ComNum.VBLF + ")";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                                return false;
                            }
                        }
                    }


                }
                #endregion

                #endregion //CHARTROW

                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        public static string READ_MAX_KTAS(string strIDNO, string strINDT, string strINTM)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strINDT = VB.Replace(strINDT, "-", "");
            strINTM = VB.Replace(strINTM, ":", "");

            if (strINDT == "" || strINTM == "") return rtnVal;

            try
            {
                SQL = " SELECT MIN(PTMIKTS) KTASLEVL";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "      SELECT PTMIKTS";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + "       WHERE PTMIIDNO = '" + strIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINDT = '" + strINDT + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINTM = '" + strINTM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO = 1";
                SQL = SQL + ComNum.VBLF + "  UNION ALL";
                SQL = SQL + ComNum.VBLF + "      SELECT PTMIKTS";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + "       WHERE PTMIIDNO = '" + strIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINDT = '" + strINDT + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINTM = '" + strINTM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO > 1)";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KTASLEVL"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 신규 간호 기록 관련 EMR 저장(FLOW)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="AcpEmr">환자정보</param>
        /// <param name="pForm">기록지 정보</param>
        /// <param name="strContent">내용</param>
        /// <param name="Trans">트랜잭션 걸건지 기본 False</param>
        /// <returns></returns>
        public static double SaveNurChartFlow(PsmhDb pDbCon, Form ptForm, EmrPatient AcpEmr, EmrForm pForm, string strChartDate, string strChartTime, Dictionary<string, string> strContent, bool Trans = false)
        {

            #region 변수
            double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            string mFLOWGB = string.Empty;
            int mFLOWITEMCNT = 0;
            int mFLOWHEADCNT = 0;
            int mFLOWINPUTSIZE = 0;

            FormFlowSheet[] mFormFlowSheet = null;
            FormFlowSheetHead[,] mFormFlowSheetHead = null;
            #endregion

            if (Trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            try
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);

                if (mFormFlowSheet == null || strContent == null || strContent != null && strContent.Count == 0 )
                    return 0;

                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return dblEmrNoNew;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                for(int i = 0; i < mFormFlowSheet.Length; i++)
                {
                    string ItemVal = string.Empty;
                    if (strContent.TryGetValue(mFormFlowSheet[i].ItemCode, out ItemVal))
                    {
                        string ItemType = string.Empty;
                        switch(mFormFlowSheet[i].CellType)
                        {
                            case "TextCellType":
                            case "ComboBoxCellType":
                                ItemType = "TEXT";
                                break;
                            case "CheckBoxCellType":
                                ItemType = "CHECK";
                                break;
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + ItemVal.Replace("'", "`") + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + i + ",";   //DSPSEQ
                        SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            if (Trans)
                            {
                                clsDB.setRollbackTran(pDbCon);
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return dblEmrNoNew;
                        }
                    }
                }

                #endregion //CHARTROW

                if (Trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }

                #region //전자인증 하기
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(pDbCon, dblEmrNoNew, Trans);
                        }
                    }
                }
                #endregion
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (Trans)
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(ptForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return dblEmrNoNew;
        }

        /// <summary>
        /// 신규 간호 기록 관련 EMR 저장(FLOW)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="AcpEmr">환자정보</param>
        /// <param name="pForm">기록지 정보</param>
        /// <param name="strContent">내용</param>
        /// <param name="Trans">트랜잭션 걸건지 기본 False</param>
        /// <returns></returns>
        public static double SaveNurChartFlowE2(PsmhDb pDbCon, Form ptForm, EmrPatient AcpEmr, EmrForm pForm, string strEmrNo, string strChartDate, string strChartTime, Dictionary<string, string> strContent, bool Trans = false)
        {

            #region 변수
            double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            string mFLOWGB = string.Empty;
            int mFLOWITEMCNT = 0;
            int mFLOWHEADCNT = 0;
            int mFLOWINPUTSIZE = 0;

            FormFlowSheet[] mFormFlowSheet = null;
            FormFlowSheetHead[,] mFormFlowSheetHead = null;
            #endregion

            if (Trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            try
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);

                if (mFormFlowSheet == null || strContent == null || strContent != null && strContent.Count == 0)
                    return 0;

                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                double dblEmrNo = VB.Val(strEmrNo);

                if (dblEmrNo > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo,
                        dtpSys.ToString("yyyyMMdd"),
                        dtpSys.ToString("HHmmss"), "C", "", clsType.User.IdNumber);
                    if (SqlErr != "OK")
                    {
                        if (Trans)
                        {
                            clsDB.setRollbackTran(pDbCon);
                        }
                        ComFunc.MsgBoxEx(ptForm, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                    #endregion
                    dblEmrNoNew = dblEmrNo;
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return dblEmrNoNew;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                for (int i = 0; i < mFormFlowSheet.Length; i++)
                {
                    string ItemVal = string.Empty;
                    if (strContent.TryGetValue(mFormFlowSheet[i].ItemCode, out ItemVal))
                    {
                        string ItemType = string.Empty;
                        switch (mFormFlowSheet[i].CellType)
                        {
                            case "TextCellType":
                            case "ComboBoxCellType":
                                ItemType = "TEXT";
                                break;
                            case "CheckBoxCellType":
                                ItemType = "CHECK";
                                break;
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + ItemVal.Replace("'", "`") + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + i + ",";   //DSPSEQ
                        SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            if (Trans)
                            {
                                clsDB.setRollbackTran(pDbCon);
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return dblEmrNoNew;
                        }
                    }
                }

                #endregion //CHARTROW

                if (Trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }

                #region //전자인증 하기
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(pDbCon, dblEmrNoNew, Trans);
                        }
                    }
                }
                #endregion

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (Trans)
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(ptForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return dblEmrNoNew;
        }

        /// <summary>
        /// 신규 간호 기록 관련 EMR 저장(FLOW)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="pForm"></param>
        /// <param name="AcpEmr">환자정보</param>
        /// <param name="pForm">기록지 정보</param>
        /// <param name="strContent">내용</param>
        /// <param name="Trans">트랜잭션 걸건지 기본 False</param>
        /// <returns></returns>
        public static double SaveNurChartFlowEx(PsmhDb pDbCon, string strUseId, Form ptForm, EmrPatient AcpEmr, EmrForm pForm, string strMedFrDate, string strPtno, string strDrCd, string strChartDate, string strChartTime, Dictionary<string, string> strContent, bool Trans = false)
        {

            #region 변수
            double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            string mFLOWGB = string.Empty;
            int mFLOWITEMCNT = 0;
            int mFLOWHEADCNT = 0;
            int mFLOWINPUTSIZE = 0;

            FormFlowSheet[] mFormFlowSheet = null;
            FormFlowSheetHead[,] mFormFlowSheetHead = null;
            #endregion

            if (Trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            try
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);

                if (mFormFlowSheet == null || strContent == null || strContent != null && strContent.Count == 0)
                    return 0;


                #region 당일 내원내역이 비어있으면 하루전 ER 내원 내역 가져온다 밤~다음날 새벽 6시간 재내원 경우 - 2020-12-14
                if (AcpEmr == null)
                {
                    AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, "O", DateTime.ParseExact(strMedFrDate.Replace("-", ""), "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd"), "ER");
                }
                #endregion

                if (AcpEmr == null)
                {
                    AcpEmr = clsEmrChart.ClearPatient();
                    AcpEmr.medFrDate = strMedFrDate.Replace("-", "");
                    AcpEmr.medFrTime = "090000";
                    AcpEmr.ptNo = strPtno;
                    AcpEmr.inOutCls = "O";
                    AcpEmr.medDeptCd = "ER";
                    AcpEmr.acpNo = "0";
                }

                AcpEmr.medDrCd = strDrCd;

                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    strUseId, strUseId, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return dblEmrNoNew;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                for (int i = 0; i < mFormFlowSheet.Length; i++)
                {
                    string ItemVal = string.Empty;
                    if (strContent.TryGetValue(mFormFlowSheet[i].ItemCode, out ItemVal))
                    {
                        string ItemType = string.Empty;
                        switch (mFormFlowSheet[i].CellType)
                        {
                            case "TextCellType":
                            case "ComboBoxCellType":
                                ItemType = "TEXT";
                                break;
                            case "CheckBoxCellType":
                                ItemType = "CHECK";
                                break;
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + ItemVal.Replace("'", "`") + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + i + ",";   //DSPSEQ
                        SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                        SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            if (Trans)
                            {
                                clsDB.setRollbackTran(pDbCon);
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return dblEmrNoNew;
                        }
                    }
                }

                #endregion //CHARTROW

                if (Trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }

                #region //전자인증 하기
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(pDbCon, dblEmrNoNew, Trans);
                        }
                    }
                }
                #endregion

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (Trans)
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(ptForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return dblEmrNoNew;
        }

        /// <summary>
        /// 신규 간호 기록 관련 EMR 저장(SPECIAL WATCH RECORD)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="dblEmrNo"></param>
        /// <param name="strContent">내용</param>
        /// <param name="Trans">트랜잭션 걸건지 기본 False</param>
        /// <returns></returns>
        public static double SaveNurChart1969(PsmhDb pDbCon, string strUseId, double dblEmrNo,
            Form ptForm, string strPano, string strChartDate, string strChartTime, string strDrcd,
            Dictionary<string, string> strContent, bool Trans = false)
        {

            #region 변수
            double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            string mFLOWGB = string.Empty;
            int mFLOWITEMCNT = 0;
            int mFLOWHEADCNT = 0;
            int mFLOWINPUTSIZE = 0;

            FormFlowSheet[] mFormFlowSheet = null;
            FormFlowSheetHead[,] mFormFlowSheetHead = null;

            OracleDataReader reader = null;
            #endregion

            if (Trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            strUseId = VB.Val(strUseId).ToString();

            strChartDate = strChartDate.Replace("-", "");
            strChartTime = strChartTime.Replace(":", "") + "00";

            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1969");
            EmrPatient AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPano, "O", strChartDate, "ER");

            if (AcpEmr == null)
            {
                AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPano, "O", DateTime.ParseExact(strChartDate, "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd"), "ER");
            }


            if (AcpEmr == null)
            {
                AcpEmr = clsEmrChart.ClearPatient();
                AcpEmr.ptNo = strPano;
                AcpEmr.inOutCls = "O";
                AcpEmr.medDeptCd = "ER";
                AcpEmr.acpNo = "0";
                AcpEmr.medDrCd = strDrcd;
                AcpEmr.medFrDate = strChartDate;
                AcpEmr.medFrTime = strChartTime;
            }

            try
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML("3510", "1", ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);

                if (mFormFlowSheet == null || strContent == null || strContent != null && strContent.Count == 0)
                    return 0;

                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                if (dblEmrNo > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo,
                        dtpSys.ToString("yyyyMMdd"),
                        dtpSys.ToString("HHmmss"), "C", "", strUseId);
                    if (SqlErr != "OK")
                    {
                        if (Trans)
                        {
                            clsDB.setRollbackTran(pDbCon);
                        }
                        ComFunc.MsgBoxEx(ptForm, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                    #endregion
                    dblEmrNoNew = dblEmrNo;
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(),
                                    strChartDate, strChartTime,
                                    strUseId, strUseId, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return dblEmrNoNew;
                }
                #endregion //저장 CHRATMAST

                #region 시간 생성
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT 1";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRBVITALTIME";
                SQL = SQL + ComNum.VBLF + "  WHERE FORMNO = " + pForm.FmFORMNO;
                SQL = SQL + ComNum.VBLF + "   AND ACPNO = " + AcpEmr.acpNo;
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + strChartDate + "'";
                SQL = SQL + ComNum.VBLF + "   AND TIMEVALUE = '" + VB.Left(strChartTime, 4) + "'";
                SQL = SQL + ComNum.VBLF + "   AND JOBGB = 'IVT'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    if (pDbCon.Trs == null)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(ptForm, "조회중 문제가 발생했습니다");
                    return dblEmrNoNew;
                }

                if (reader.HasRows == false)
                {
                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "" + pForm.FmFORMNO + ",";  //FORMNO
                    SQL = SQL + ComNum.VBLF + "" + AcpEmr.acpNo + ",";  //ACPNO
                    SQL = SQL + ComNum.VBLF + "'" + "IVT" + "',";    //JOBGB
                    SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";  //CHARTDATE
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(strChartTime, 4) + "',"; //TIMEVALUE
                    SQL = SQL + ComNum.VBLF + "'0',";   //SUBGB
                    SQL = SQL + ComNum.VBLF + "'" + dtpSys.ToString("yyyyMMdd") + "',";  //WRITEDATE
                    SQL = SQL + ComNum.VBLF + "'" + dtpSys.ToString("HHmmss") + "',";  //WRITETIME
                    SQL = SQL + ComNum.VBLF + "'" + strUseId + "'";    //WRITEUSEID
                    SQL = SQL + ComNum.VBLF + ")";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        if (Trans)
                        {
                            clsDB.setRollbackTran(pDbCon);
                        }
                        reader.Dispose();
                        ComFunc.MsgBoxEx(ptForm, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return dblEmrNoNew;
                    }
                }

                reader.Dispose();
                #endregion

                #region 해당일자 아이템 생성
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    A.ITEMCD";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + pForm.FmFORMNO;
                SQL = SQL + ComNum.VBLF + "  AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + strChartDate + "'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(ptForm, "조회중 문제가 발생했습니다");
                    return dblEmrNoNew;
                }

                if (reader.HasRows == false)
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    " + pForm.FmFORMNO + ","; //FORMNO
                    SQL = SQL + ComNum.VBLF + "    " + AcpEmr.acpNo + ","; //ACPNO
                    SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "',"; //PTNO
                    SQL = SQL + ComNum.VBLF + "    '" + strChartDate + "',"; //CHARTDATE
                    SQL = SQL + ComNum.VBLF + "    CASE WHEN B.UNITCLS = '임상관찰' THEN 'IVT'"; //JOBGB
                    SQL = SQL + ComNum.VBLF + "         WHEN B.UNITCLS = '섭취배설' THEN 'IIO'"; //JOBGB
                    SQL = SQL + ComNum.VBLF + "         WHEN B.UNITCLS = '특수치료' THEN 'IST'"; //JOBGB
                    SQL = SQL + ComNum.VBLF + "         WHEN B.UNITCLS = '기본간호' THEN 'IBN'"; //JOBGB
                    SQL = SQL + ComNum.VBLF + "    END JOBGB, "; //JOBGB
                    SQL = SQL + ComNum.VBLF + "    B.BASCD,"; //ITEMCD
                    SQL = SQL + ComNum.VBLF + "    '" + dtpSys.ToString("yyyyMMdd") + "',";  //WRITEDATE
                    SQL = SQL + ComNum.VBLF + "    '" + dtpSys.ToString("HHmmss") + "',";  //WRITETIME
                    SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "'"; //WRITEUSEID
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD BB ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B ";
                    SQL = SQL + ComNum.VBLF + "   ON BB.BASCD = B.BASCD ";
                    SQL = SQL + ComNum.VBLF + "  AND B.BSNSCLS = '기록지관리' ";
                    SQL = SQL + ComNum.VBLF + "  AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호' ) ";
                    SQL = SQL + ComNum.VBLF + "  AND B.USECLS = '0' ";
                    SQL = SQL + ComNum.VBLF + "WHERE BB.BSNSCLS = '기록지관리' ";
                    SQL = SQL + ComNum.VBLF + "  AND BB.UNITCLS = '임상관찰병동_" + "ER" + "'";
                    SQL = SQL + ComNum.VBLF + "  AND BB.USECLS = '0' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        if (pDbCon.Trs == null)
                        {
                            clsDB.setRollbackTran(pDbCon);
                        }
                        reader.Dispose();
                        ComFunc.MsgBoxEx(ptForm, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return dblEmrNoNew;
                    }
                }
                reader.Dispose();
                #endregion

                #region //CHARTROW
                for (int i = 0; i < mFormFlowSheet.Length; i++)
                {
                    string ItemVal = string.Empty;
                    if (strContent.TryGetValue(mFormFlowSheet[i].ItemCode, out ItemVal))
                    {
                        string ItemType = string.Empty;
                        switch (mFormFlowSheet[i].CellType)
                        {
                            case "TextCellType":
                            case "ComboBoxCellType":
                                ItemType = "TEXT";
                                break;
                            case "CheckBoxCellType":
                                ItemType = "CHECK";
                                break;
                        }

                        SQL = string.Empty;
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                        SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',";   //ITEMCD
                        SQL += ComNum.VBLF + "'" + mFormFlowSheet[i].ItemCode + "',"; //ITEMNO
                        SQL += ComNum.VBLF + "-1,"; //ITEMINDEX
                        SQL += ComNum.VBLF + "'" + ItemType + "',";   //ITEMTYPE
                        SQL += ComNum.VBLF + "'" + ItemVal.Replace("'", "`") + "',";   //ITEMVALUE
                        SQL += ComNum.VBLF + i + ",";   //DSPSEQ
                        SQL += ComNum.VBLF + "NULL, ";   //ITEMVALUE1
                        SQL += ComNum.VBLF + "'" + VB.Val(strUseId) + "',";   //INPUSEID
                        SQL += ComNum.VBLF + "'" + dtpSys.ToString("yyyyMMdd") + "',";  //INPDATE
                        SQL += ComNum.VBLF + "'" + dtpSys.ToString("HHmmss") + "'";  //INPTIME
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            if (Trans)
                            {
                                clsDB.setRollbackTran(pDbCon);
                            }
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(ptForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                            return dblEmrNoNew;
                        }
                    }
                }

                #endregion //CHARTROW

                if (Trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }


                #region //전자인증 하기
                //if (System.Diagnostics.Debugger.IsAttached == false)
                //{
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, strUseId) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(pDbCon, dblEmrNoNew, Trans);
                        }
                    }
                ////}
                #endregion

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (Trans)
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(ptForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return dblEmrNoNew;
        }

        /// <summary>
        /// Data Mapping 삭제
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="emrForm">환자정보</param>
        /// <param name="dblEmrNo">EMRNO</param>
        /// <param name="strMapping1">문자열</param>
        /// <param name="dblMapping2">숫자</param>
        /// <returns></returns>
        public static bool DeleteDataMapping(PsmhDb pDbCon, Form pForm, EmrForm emrForm, double dblEmrNo, string strMapping1, double dblMapping2)
        {

            #region 변수
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            #endregion

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL ="DELETE " + ComNum.DB_EMR + "EMR_DATA_MAPPING ";
                SQL += ComNum.VBLF + "WHERE FORMNO = " + emrForm.FmFORMNO;
                SQL += ComNum.VBLF + "  AND EMRNO  = " + dblEmrNo;
                
                if (!string.IsNullOrWhiteSpace(strMapping1))
                {
                    SQL += ComNum.VBLF + "  AND MAPPING1  = '" + strMapping1.Replace("'", "`") + "'";
                }

                if (dblMapping2 > 0)
                {
                    SQL += ComNum.VBLF + "  AND MAPPING2  = " + dblMapping2;
                }

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(pForm, "SaveDataMapping저장 중 에러가 발생했습니다.");
                    return false;
                }


                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(pForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// Data Mapping 삭제
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="emrForm">환자정보</param>
        /// <param name="strMapping1">문자열</param>
        /// <param name="dblMapping2">숫자</param>
        /// <returns></returns>
        public static bool DeleteDataMappingEx(PsmhDb pDbCon, Form pForm, EmrForm emrForm, string strMapping1, double dblMapping2)
        {

            #region 변수
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            #endregion

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "DELETE " + ComNum.DB_EMR + "EMR_DATA_MAPPING ";
                SQL += ComNum.VBLF + "WHERE FORMNO = " + emrForm.FmFORMNO;

                if (!string.IsNullOrWhiteSpace(strMapping1))
                {
                    SQL += ComNum.VBLF + "  AND MAPPING1 = '" + strMapping1.Replace("'", "`") + "'";
                }

                if (dblMapping2 > 0)
                {
                    SQL += ComNum.VBLF + "  AND MAPPING2 = " + dblMapping2;
                }

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(pForm, "SaveDataMapping저장 중 에러가 발생했습니다.");
                    return false;
                }


                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(pForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// Data Mapping 저장
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="emrForm">환자정보</param>
        /// <param name="dblEmrNo">EMRNO</param>
        /// <param name="strMapping1">문자열</param>
        /// <param name="dblMapping2">숫자</param>
        /// <returns></returns>
        public static bool SaveDataMapping(PsmhDb pDbCon, Form pForm, EmrForm emrForm, double dblEmrNo, string strMapping1, double dblMapping2)
        {

            #region 변수
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            //OracleDataReader reader = null; 
            #endregion

            clsDB.setBeginTran(pDbCon);

            try
            {
                #region 삭제
                SQL = "DELETE " + ComNum.DB_EMR + "EMR_DATA_MAPPING ";
                SQL += ComNum.VBLF + "WHERE FORMNO = " + emrForm.FmFORMNO;

                if (!string.IsNullOrWhiteSpace(strMapping1))
                {
                    SQL += ComNum.VBLF + "  AND MAPPING1 = '" + strMapping1.Replace("'", "`") + "'";
                }

                if (dblMapping2 > 0)
                {
                    SQL += ComNum.VBLF + "  AND MAPPING2 = " + dblMapping2;
                }

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(pForm, "SaveDataMapping저장 중 에러가 발생했습니다.");
                    return false;
                }

                #endregion

                SQL = string.Empty;
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "EMR_DATA_MAPPING ";
                SQL += ComNum.VBLF + "    (FORMNO, EMRNO, MAPPING1, MAPPING2)";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + emrForm.FmFORMNO + ",";    //FORMNO
                SQL += ComNum.VBLF + dblEmrNo.ToString() + ",";    //EMRNO
                SQL += ComNum.VBLF + "'" + strMapping1.Replace("'", "`") + "',";   //MAPPING1
                SQL += ComNum.VBLF + dblMapping2; //MAPPING2
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(pForm, "SaveDataMapping저장 중 에러가 발생했습니다.");
                    return false;
                }


                clsDB.setCommitTran(pDbCon);

                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(pForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }



        /// <summary>
        /// 신규 EMR 경과 기록지 저장
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        /// <param name="AcpEmr">환자정보</param>
        /// <param name="dblEmrNo">저장 전 EMRNO 없으면 0</param>
        /// <param name="strContent">내용</param>
        /// <param name="NewEmrNo">저장 후 EMRNO</param>
        /// <param name="MUNIN">통합예진표 True 혹은 False</param>
        /// <returns></returns>
        public static bool SaveNewProgress(PsmhDb pDbCon, Form pForm, EmrPatient AcpEmr, double dblEmrNo, string strContent, ref double NewEmrNo, bool MUNIN = false)
        {

            #region 변수
            double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            #endregion

            clsDB.setBeginTran(pDbCon);

            try
            {
                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));

                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                if (dblEmrNo > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo, 
                        dtpSys.ToString("yyyyMMdd"), 
                        dtpSys.ToString("HHmmss"), "C", "", clsType.User.IdNumber);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(pForm, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion
                    dblEmrNoNew = dblEmrNo;
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, "963", "2",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmm00"),
                                    clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW

                string strITEMVALUETOT = strContent.Trim().Replace("'", "`");
                string strITEMVALUE = string.Empty;
                string strITEMVALUE1 = string.Empty;

                int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                int intLen = 3999;
                if (intLenTot > 3999)
                {
                    string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                    if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                    {
                        intLen = 3998;
                    }
                    strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                    strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                }
                else
                {
                    strITEMVALUE = strITEMVALUETOT;
                }

                SQL = string.Empty;
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL += ComNum.VBLF + "'I0000000981', ";   //ITEMCD
                SQL += ComNum.VBLF + "'I0000000981', "; //ITEMNO
                SQL += ComNum.VBLF + "-1, "; //ITEMINDEX
                SQL += ComNum.VBLF + "'TEXT', ";   //ITEMTYPE
                SQL += ComNum.VBLF + "'" + strITEMVALUE + "', ";   //ITEMVALUE
                SQL += ComNum.VBLF + "0, ";   //DSPSEQ
                SQL += ComNum.VBLF + "'" + strITEMVALUE1 + "', ";   //ITEMVALUE1
                SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    return false;
                }
                #endregion //CHARTROW


                #region 통합예진표 용으로 저장시
                if (MUNIN)
                {
                    SQL = string.Empty;
                    SQL = " UPDATE KOSMOS_PMPA.OPD_DEPT_MUNJIN SET     ";
                    SQL += ComNum.VBLF + " EMRNO = " + dblEmrNoNew;
                    SQL += ComNum.VBLF + " , CHK = 'Y'";
                    SQL += ComNum.VBLF + " WHERE PANO ='" + AcpEmr.ptNo + "' ";
                    SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + DateTime.ParseExact(AcpEmr.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND DEPTCODE ='" + AcpEmr.medDeptCd + "' ";
                    SQL += ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                #endregion


                clsDB.setCommitTran(pDbCon);

                #region //전자인증 하기
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            blnCert = clsEmrQuery.SaveEmrCert(pDbCon, dblEmrNoNew);
                        }
                    }
                }
                #endregion

                NewEmrNo = dblEmrNoNew;
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(pForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        public static bool SaveNewProgressEx(PsmhDb pDbCon, Form pForm, EmrPatient AcpEmr, double dblEmrNo, string strContent, ref double NewEmrNo, string strChartDate, string strChartTime, bool MUNIN = false)
        {

            #region 변수
            double dblEmrNoNew = 0;
            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            #endregion

            clsDB.setBeginTran(pDbCon);

            try
            {
                DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));

                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                if (dblEmrNo > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo,
                        dtpSys.ToString("yyyyMMdd"),
                        dtpSys.ToString("HHmmss"), "C", "", clsType.User.IdNumber);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(pForm, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion
                    dblEmrNoNew = dblEmrNo;
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = string.Empty;

                if (SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, "963", "2",
                                    strChartDate, strChartTime,
                                    clsType.User.IdNumber, clsType.User.IdNumber, "1", "1", "0",
                                    dtpSys.ToString("yyyyMMdd"), dtpSys.ToString("HHmmss"), strSaveFlag) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW

                string strITEMVALUETOT = strContent.Trim().Replace("'", "`");
                string strITEMVALUE = string.Empty;
                string strITEMVALUE1 = string.Empty;
                string strITEMVALUE2 = string.Empty;

                //int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                //int intLen = 3999;
                //if (intLenTot > 3999)
                //{
                //    string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                //    if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                //    {
                //        intLen = 3998;
                //    }
                //    strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                //    strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                //}
                //else
                //{
                //    strITEMVALUE = strITEMVALUETOT;
                //}


                int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                int intLen = 3999;
                if (intLenTot > 3999)
                {
                    string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                    if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                    {
                        intLen = 3998;
                    }
                    strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                    if (intLenTot > 8000)
                    {
                        strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, 3999);
                        strITEMVALUE2 = ComFunc.GetMidStr(strITEMVALUETOT, intLen + 3999, intLenTot - (intLen + 3999));
                        if (ComFunc.GetWordByByte(strITEMVALUE2) > 4000)
                        {
                            strITEMVALUE2 = ComFunc.GetMidStr(strITEMVALUETOT, intLen + 3999, 3999);
                        }
                    }
                    else
                    {
                        strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                    }
                }
                else
                {
                    strITEMVALUE = strITEMVALUETOT;
                }


                SQL = string.Empty;
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL += ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, ITEMVALUE2, INPUSEID, INPDATE, INPTIME )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL += ComNum.VBLF + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL += ComNum.VBLF + "'I0000000981', ";   //ITEMCD
                SQL += ComNum.VBLF + "'I0000000981', "; //ITEMNO
                SQL += ComNum.VBLF + "-1, "; //ITEMINDEX
                SQL += ComNum.VBLF + "'TEXT', ";   //ITEMTYPE
                SQL += ComNum.VBLF + "'" + strITEMVALUE + "', ";   //ITEMVALUE
                SQL += ComNum.VBLF + "0, ";   //DSPSEQ
                SQL += ComNum.VBLF + "'" + strITEMVALUE1 + "', ";   //ITEMVALUE1
                SQL += ComNum.VBLF + "'" + strITEMVALUE2 + "', ";   //ITEMVALUE2 --추가
                SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                SQL += ComNum.VBLF + ")";
                
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(pForm, "AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    return false;
                }
                #endregion //CHARTROW

                #region 통합예진표 용으로 저장시
                if (MUNIN)
                {
                    SQL = string.Empty;
                    SQL = " UPDATE KOSMOS_PMPA.OPD_DEPT_MUNJIN SET     ";
                    SQL += ComNum.VBLF + " EMRNO = " + dblEmrNoNew;
                    SQL += ComNum.VBLF + " , CHK = 'Y'";
                    SQL += ComNum.VBLF + " WHERE PANO ='" + AcpEmr.ptNo + "' ";
                    SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + DateTime.ParseExact(AcpEmr.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND DEPTCODE ='" + AcpEmr.medDeptCd + "' ";
                    SQL += ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                #endregion

                clsDB.setCommitTran(pDbCon);

                #region //전자인증 하기
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                        {
                            blnCert = SaveEmrCert(pDbCon, dblEmrNoNew);
                        }
                    }
                }
                #endregion

                NewEmrNo = dblEmrNoNew;
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(pForm, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// ORACLE IPADDRESS
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static string READ_IPADDRESS_ORACLE(PsmhDb pDbCon)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {
                SQL = " SELECT SYS_CONTEXT ('USERENV', 'IP_ADDRESS')  IP  FROM DUAL";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 차트 조회 로그 저장
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgGubun2"></param>
        public static void CREATE_CHARTVIEW_LOG(PsmhDb pDbCon, string strPtNo, string ArgGubun, string ArgGubun2 = "")
        {
            if (clsType.User.IdNumber.Equals("4349") || string.IsNullOrWhiteSpace(strPtNo))
            {
                return;
            }

            strPtNo = Regex.Replace(strPtNo, @"[^0-9]", "");
            if (VB.IsNumeric(strPtNo) == false)
            {
                return;
            }

            #region 변수
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;

            string strIP = READ_IPADDRESS_ORACLE(pDbCon);

            string[] str = strIP.Split('.');
            string strJACODE = string.Empty;
            strIP = VB.Val(str[0]).ToString("000") + "." + VB.Val(str[1]).ToString("000") + "." + VB.Val(str[2]).ToString("000") + "." + VB.Val(str[3]).ToString("000");
            #endregion

            clsDB.setBeginTran(pDbCon);
            try
            {
                #region JACODE
                SQL = " SELECT JACODE FROM KOSMOS_ADM.JAS_MASTER";
                SQL += ComNum.VBLF + " WHERE IPADDR = '" + strIP + "'";

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    //ComFunc.MsgBox(sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strJACODE = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                #endregion

                SQL = " INSERT INTO KOSMOS_PMPA.MID_CHARTVIEW_LOG";
                SQL += ComNum.VBLF + " (PTNO, CDATE, GUBUN, IP, JACODE, SABUN, GUBUN2)";
                SQL += ComNum.VBLF + "  Values(";
                SQL += ComNum.VBLF + "  '" + strPtNo + "',";
                SQL += ComNum.VBLF + "  SYSDATE, ";
                SQL += ComNum.VBLF + "  '" + ArgGubun + "',";
                SQL += ComNum.VBLF + "  '" + strIP + "',";
                SQL += ComNum.VBLF + "  '" + strJACODE + "',";
                SQL += ComNum.VBLF + "  " + clsPublic.GnJobSabun + ",";
                SQL += ComNum.VBLF + "  '" + ArgGubun2 + "')";

                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
                //ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 차트 뷰 접근 로그 저장
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <param name="strUseId"></param>
        /// <param name="strUsName"></param>
        /// <param name="strREQTYPE"></param>
        /// <param name="strREQMEMO"></param>
        /// <param name="ArgExeName"></param>
        /// <returns></returns>
        public static bool SetViewLog(PsmhDb pDbCon, string strPtNo, string strUseId, string strUsName, string strREQTYPE, string strREQMEMO, string ArgExeName = "")
        {
            if (strUseId.Equals("4349"))
            {
                return true;
            }

            bool rtnVal = false;

            string SQL = string.Empty;
            string sqlErr = string.Empty;
            
            int RowAffected = 0;

            clsDB.setBeginTran(pDbCon);
            try
            {
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMR_VIEWLOGT ";
                SQL += ComNum.VBLF + "  (JOBDATE, CUSERID, PATID, SNAME, VIEWCODE, CDATE, AUSERID, VDATE, EDATE, VIEWREMARK,Remark )";
                SQL += ComNum.VBLF + "  Values(";
                SQL += ComNum.VBLF + "  TRUNC(SYSDATE),";
                SQL += ComNum.VBLF + "  '" + strUseId + "',";
                SQL += ComNum.VBLF + "  '" + strPtNo + "',";
                SQL += ComNum.VBLF + "  '" + strUsName + "',";
                SQL += ComNum.VBLF + "  '" + strREQTYPE.Trim() + "',";
                SQL += ComNum.VBLF + "  TRUNC(SYSDATE),";
                SQL += ComNum.VBLF + "  '16109',";
                SQL += ComNum.VBLF + "  TRUNC(SYSDATE),";
                SQL += ComNum.VBLF + "  TRUNC(SYSDATE),";
                SQL += ComNum.VBLF + "  '" + strREQMEMO + "','" + ArgExeName + "' )";

                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }

                rtnVal = true;
                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 2019-06-19 응급실에서 병동 보낸 시간 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static string READ_ER_TRANS_TIME(PsmhDb pDbCon, EmrPatient pAcp)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;
            string SQL = string.Empty;

            string strERIpwon = string.Empty;

            try
            {
                #region 응급실 경유 입원 체크
                SQL = " SELECT PANO ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "' ";
                SQL += ComNum.VBLF + "   AND INDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + " " + "00:00','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "   AND INDATE <= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + " " + "23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "   AND AMSET7 IN ('3','4','5') ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    strERIpwon = "OK";
                }

                reader.Dispose();
                #endregion

                #region ER환자일경우
                if (strERIpwon.Equals("OK"))
                {
                    SQL = " SELECT TO_CHAR(FRDATE, 'YYYY-MM-DD HH24:MI') FRDATE ";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_CHECKLIST_TRANS ";
                    SQL += ComNum.VBLF + "  WHERE FRDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + " " + "00:00','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + pAcp.ptNo + "' ";
                    SQL += ComNum.VBLF + "  ORDER BY FRDATE DESC ";

                    sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(sqlErr);
                        return rtnVal;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        rtnVal = reader.GetValue(0).ToString().Trim();
                    }


                    reader.Dispose();
                }

                #endregion


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 차트 수정권한 부여 읽어오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static string READ_PERMISSIONS(PsmhDb pDbCon, string strUseId)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {
                SQL = "SELECT OLDSABUN";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_PERMISSIONS";
                SQL += ComNum.VBLF + "WHERE SABUN = '" + strUseId + "'";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 퇴사한 사번이 사번 갱신 후 그 사번의 퇴사여부를 확인하는 부분
        /// 퇴사하였을 경우 공백을 반환
        /// 퇴사하지 않았을 경우 "OK" 반환
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static string READ_PERMISSIONS_TOIDAY(PsmhDb pDbCon, string ArgOLDSabun)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {
                SQL = "SELECT B.TOIDAY";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_PERMISSIONS A, KOSMOS_ADM.INSA_MST B";
                SQL += ComNum.VBLF + "WHERE A.SABUN = B.SABUN";
                SQL += ComNum.VBLF + "  AND A.OLDSABUN = '" + ArgOLDSabun + "'";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Length > 0 )
                {
                    rtnVal = "OK";
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 수정 할수 있는지 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static string CanModify(PsmhDb pDbCon, string strEmrNo)
        {
            //2)번 사유
            if(clsEmrPublic.gUserGrade.Equals("WRITE") && clsEmrPublic.GstrModify.Equals("YES"))
            {
                return "YES";
            }

            string rtnVal = "NO";
            OracleDataReader reader = null;
            string SQL = string.Empty;

            string strUseId = string.Empty;

            try
            {
                #region 작성자 정보 
                SQL = "SELECT A.USEID";
                SQL += ComNum.VBLF + "FROM(";
                SQL += ComNum.VBLF + "SELECT TRIM(LTRIM(USEID, '0')) USEID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML";
                SQL += ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;

                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT CHARTUSEID AS USEID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL += ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SQL += ComNum.VBLF + ") A";
                SQL += ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMR_USERT B";
                SQL += ComNum.VBLF + "     ON B.USERID = A.USEID";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    strUseId = ComFunc.SetAutoZero(reader.GetValue(0).ToString().Trim(), 5);
                }

                reader.Dispose();
                #endregion

                #region 작성자가 퇴사하였을 경우 수정 못함(전자인증 관련하여 문제 발생함) 2010-12-07 아래 3번 사항일 경우 예외처리
                SQL = "SELECT TOIDAY";
                SQL += ComNum.VBLF + "FROM KOSMOS_ADM.INSA_MST ";
                SQL += ComNum.VBLF + "WHERE SABUN = '" + ComFunc.SetAutoZero(strUseId, 5) + "' ";

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Length > 0)
                {
                    //3)번 사유
                    if(strUseId.Equals(READ_PERMISSIONS(clsDB.DbCon, clsType.User.IdNumber)))
                    {
                        rtnVal = "YES";
                    }

                    //4)번 사유
                    if(rtnVal.Equals("NO") && clsEmrPublic.gUserGrade.Equals("WRITE") && clsEmrPublic.GstrModify.Equals("YES") && READ_PERMISSIONS_TOIDAY(pDbCon, strUseId).Equals("OK"))
                    {
                        rtnVal = "YES";
                    }

                    //5)번 사유
                    if (clsEmrPublic.ModifyCert && strUseId.Equals(clsType.User.IdNumber))
                    {
                        rtnVal = "YES";
                    }



                    //'=====================================================
                    //'2010-02-19 김현욱 추가
                    //'의무기록실일 경우 수정 권한 전체 풀어놓음
                    //'이유 : 입퇴원 기록지에서 진단명에 대한 진단코드가 누락되는 경우가 있는데
                    //'의무기록 위원회에서 의무기록실의 기록사들은 수정 권한을 풀어놓고 누락코드를 입력하게 위함


                    //'=====================================================
                    //'2010-11-30 김현욱 추가 & 정리
                    //'삭제권한관련 정리
                    //'1)작성자와 수정하려는 사람이 동일 사번을 가지고 있을 경우
                    //'2)의무기록실에서 수정권한을 활성화 할 경우
                    //'       =>단! 퇴사자의 사번의 경우는 수정권한 활성화 하여도 수정이 안됨
                    //'3)의무기록실에서 아래와 같은 이유로 인하여 특정사번에 대한 수정권한을 세팅한 경우
                    //'       => 퇴사후 재입사, 사번의 변경(단!! 동일인(주민번호 동일)만 가능함)
                    //'4)갱신 전 사번이 갱신 후 아직까지 퇴사하지 않았을 경우, 의무기록실 권한으로 로그인했을 경우 수정 권한 부여(2011-03-07)
                    //'2011-10-19
                    //'5)의무기록실에서 퇴사자 사번도 수정 권한 풀어줄 경우
                    //'2016-05-02
                    //'6)주치의가 본인이였을 경우 레지던트가 작성한 정형화된 서식지
                    //'=========================================================================
                    //'7)안길영 부장님 퇴사후 재입사 하드코딩 요청(의료정보팀장님) - 2020-04-01
                    //'=========================================================================
                }

                reader.Dispose();
                
                //1)번 사유
                if (strUseId.Equals(clsType.User.Sabun) || strUseId.Equals(clsType.User.IdNumber))
                {
                    rtnVal = "YES";
                }

                //2)번 사유
                if (clsEmrPublic.gUserGrade.Equals("WRITE") && clsEmrPublic.ModifyCert)
                {
                    rtnVal = "YES";
                }

                //7)번 사유 안길영 부장님 퇴사후 재입사 하드코딩 요청(의료정보팀장님)
                if (strUseId.Equals("21225") && clsType.User.IdNumber.Equals("52517"))
                {
                    rtnVal = "YES";
                }

                #endregion

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 차트 신청한(출력 안한) 내역이 있는지 확인
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static bool READ_CHART_APPLY(Form pForm, string strEmrNo)
        {
            if (string.IsNullOrWhiteSpace(strEmrNo))
                return false;

            OracleDataReader dataReader = null;
            bool rtnVal = false;

            string SQL = " SELECT 1 AS CNT";
            SQL = SQL + ComNum.VBLF + " FROM DUAL";
            SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
            SQL = SQL + ComNum.VBLF + " (";
            SQL = SQL + ComNum.VBLF + " SELECT 1";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRPRTREQ";
            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
            SQL = SQL + ComNum.VBLF + "   AND PRINTYN IS NULL ";
            SQL = SQL + ComNum.VBLF + " )";

            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBox(sqlErr);
                return rtnVal;
            }

            if (dataReader.HasRows && ComFunc.MsgBoxQEx(pForm, "해당 챠트는 복사 신청되어 있는 챠트입니다\r\n정말로 수정하시겠습니까?\r\n다시 한번 잘 생각해보시고 그 후에도 정말 수정을 하시겠다면 수정 후 필히 '복사 신청'을 다시 하시고\r\n원무팀 제 증명 창구(8108)로 확인 전화를 해주시기 바랍니다.") == DialogResult.No)
            {
                rtnVal = true;
            }

            dataReader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// 해당 챠트가 인쇄된 적이 있는지 확인하는 모듈
        /// 반환값이 TRUE면 수정하지 않는다.
        /// </summary>
        /// <param name="strEmrNo">EmrNo</param>
        /// <returns></returns>
        public static bool READ_PRTLOG(Form pForm, string strEmrNo)
        {
            if (string.IsNullOrWhiteSpace(strEmrNo))
                return false; 

            OracleDataReader dataReader = null;
            bool rtnVal = false;

            string SQL = " SELECT 1 AS CNT";
            SQL = SQL + ComNum.VBLF + " FROM DUAL";
            SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
            SQL = SQL + ComNum.VBLF + " (";
            SQL = SQL + ComNum.VBLF + " SELECT 1";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRPRTREQ";
            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
            SQL = SQL + ComNum.VBLF + "   AND PRINTYN = 'Y'";
            //SQL = SQL + ComNum.VBLF + " UNION ALL";
            //SQL = SQL + ComNum.VBLF + " SELECT 1";
            //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            //SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
            //SQL = SQL + ComNum.VBLF + "   AND PRNTYN = 'Y'";
            SQL = SQL + ComNum.VBLF + " )";

            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBox(sqlErr);
                return rtnVal;
            }

            if (dataReader.HasRows && ComFunc.MsgBoxQEx(pForm, "해당 챠트는 사본 발급 내역이 존재합니다. 계속 진행하시겠습니까?") == DialogResult.No)
            {
                rtnVal = true;
            }

            dataReader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// 복사신청 된 챠트인지 확인
        /// </summary>
        /// <param name="strEmrNo">EmrNo</param>
        /// <returns></returns>
        public static bool READ_PRTLOG2(string strEmrNo)
        {
            if (strEmrNo.Length == 0)
                return false;

            OracleDataReader dataReader = null;
            bool rtnVal = false;

            string SQL = " SELECT 1 AS CNT";
            SQL = SQL + ComNum.VBLF + " FROM DUAL";
            SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
            SQL = SQL + ComNum.VBLF + " (";
            SQL = SQL + ComNum.VBLF + " SELECT 1";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRPRTREQ";
            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
            SQL = SQL + ComNum.VBLF + "   AND PRINTYN = 'Y'";
            SQL = SQL + ComNum.VBLF + " )";


            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBox(sqlErr);
                return rtnVal;
            }

            if (dataReader.HasRows)
            {
                rtnVal = true;
            }

            dataReader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// EMR 서버, 클라이언트 경로 설정
        /// </summary>
        public static void SetEmrEnv()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strAplDate = "";

            clsEmrType.InitEmrSvrInfo();

            strAplDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT BASCD, BASNAME, BASEXNAME, BASVAL, VFLAG1, VFLAG2, NFLAG1, NFLAG2, UNITCLS, REMARK1, REMARK2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = '프로그램'";
            SQL = SQL + ComNum.VBLF + "       AND UNITCLS = 'PATH'";
            SQL = SQL + ComNum.VBLF + "       AND  '" + strAplDate + "' BETWEEN APLFRDATE AND APLENDDATE ";
            SQL = SQL + ComNum.VBLF + "       AND USECLS = '0' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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
                Cursor.Current = Cursors.Default;
                return;
            }
            
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["BASCD"].ToString().Trim() == "EmrClient")
                {
                    clsEmrType.EmrSvrInfo.EmrClient = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                }
                if (dt.Rows[i]["BASCD"].ToString().Trim() == "EmrFtpSvrIp")
                {
                    clsEmrType.EmrSvrInfo.EmrFtpSvrIp = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                }
                if (dt.Rows[i]["BASCD"].ToString().Trim() == "EmrClient")
                {
                    clsEmrType.EmrSvrInfo.EmrFtpPort = (VB.Val(dt.Rows[i]["BASEXNAME"].ToString().Trim())).ToString();
                }
                if (dt.Rows[i]["BASCD"].ToString().Trim() == "EmrFtpUser")
                {
                    clsEmrType.EmrSvrInfo.EmrFtpUser = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                }
                if (dt.Rows[i]["BASCD"].ToString().Trim() == "EmrFtpPasswd")
                {
                    clsEmrType.EmrSvrInfo.EmrFtpPasswd = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                }
                if (dt.Rows[i]["BASCD"].ToString().Trim() == "EmrFtpPath")
                {
                    clsEmrType.EmrSvrInfo.EmrFtpPath = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;

        }

        /// <summary>
        /// 기초코드를 가지고 온다
        /// </summary>
        /// <param name="strBsnsCls">대분류</param>
        /// <param name="strUnitCls">소분류</param>
        /// <returns></returns>
        public static DataTable GetBBasCd(PsmhDb pDbCon, string strBsnsCls, string strUnitCls)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strAplDate = "";

            strAplDate = ComQuery.CurrentDateTime(pDbCon, "D");
            
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT BASCD, BASNAME, BASEXNAME, BASVAL, VFLAG1, VFLAG2, NFLAG1, NFLAG2, UNITCLS, REMARK1, REMARK2 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = '" + strBsnsCls + "'";
            SQL = SQL + ComNum.VBLF + "       AND UNITCLS = '" + strUnitCls + "'";
            SQL = SQL + ComNum.VBLF + "       AND  '" + strAplDate + "' BETWEEN APLFRDATE AND APLENDDATE ";
            SQL = SQL + ComNum.VBLF + "       AND USECLS = '0' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 기초코드를 가지고 온다
        /// </summary>
        /// <param name="strBsnsCls">대분류</param>
        /// <param name="strUnitCls">소분류</param>
        /// <param name="strAplDate">적용일자</param>
        /// <param name="strSubSql">서브쿼리</param>
        /// <param name="strOrderBy">정렬</param>
        /// <returns></returns>
        public static DataTable GetBBasCd(PsmhDb pDbCon, string strBsnsCls, string strUnitCls, string strAplDate, string strSubSql, string strOrderBy)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";

            if (strAplDate == "")
            {
                strAplDate = ComQuery.CurrentDateTime(pDbCon, "D");
            }

            if (strUnitCls == "")
            {
                SQL = SQL + ComNum.VBLF + " SELECT BASCD, BASNAME, BASEXNAME, BASVAL, VFLAG1, VFLAG2, NFLAG1, NFLAG2, UNITCLS, REMARK1, REMARK2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = '" + strBsnsCls + "'";
                SQL = SQL + ComNum.VBLF + "       AND UNITCLS = '" + strUnitCls + "'";
                SQL = SQL + ComNum.VBLF + "       AND  '" + strAplDate + "' BETWEEN APLFRDATE AND APLENDDATE ";
                SQL = SQL + ComNum.VBLF + "       AND USECLS = '0' ";
                if (strSubSql != "")
                {
                    SQL = SQL + ComNum.VBLF + strSubSql;
                }
                SQL = SQL + strSubSql;
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " SELECT BASCD, BASNAME, BASEXNAME, BASVAL, VFLAG1, VFLAG2, NFLAG1, NFLAG2, UNITCLS, REMARK1, REMARK2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = '" + strBsnsCls + "'";
                SQL = SQL + ComNum.VBLF + "       AND UNITCLS = '" + strUnitCls + "'";
                SQL = SQL + ComNum.VBLF + "       AND  '" + strAplDate + "' BETWEEN APLFRDATE AND APLENDDATE ";
                SQL = SQL + ComNum.VBLF + "       AND USECLS = '0' ";
                if (strSubSql != "")
                {
                    SQL = SQL + ComNum.VBLF + strSubSql;
                }
            }

            if (strOrderBy == "")
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY BASCD ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY " + strOrderBy;
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 기록지 그룹조회
        /// </summary>
        /// <param name="strDEPTH"></param>
        /// <param name="strGRPFORMNO"></param>
        /// <returns></returns>
        public static DataTable GetAEMRGRPFORM(PsmhDb pDbCon, string strDEPTH = "", string strGRPFORMNO = "", string strGROUPPARENT = "", string strSubQuery = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strAplDate = "";

            strAplDate = ComQuery.CurrentDateTime(pDbCon, "D");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT GRPFORMNO, GRPFORMNAME, DEPTH, GROUPPARENT, DISPSEQ, DELYN ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRGRPFORM ";
            if (strDEPTH != "")
            {
                SQL = SQL + ComNum.VBLF + "WHERE DEPTH = " + (VB.Val(strDEPTH)).ToString();
                if (strGROUPPARENT != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GROUPPARENT = " + (VB.Val(strGROUPPARENT)).ToString();
                }
                if (strGRPFORMNO != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GRPFORMNO = " + (VB.Val(strGRPFORMNO)).ToString();
                }
            }
            else
            {
                if (strGROUPPARENT != "")
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE GROUPPARENT = " + (VB.Val(strGROUPPARENT)).ToString();
                    if (strGRPFORMNO != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND GRPFORMNO = " + (VB.Val(strGRPFORMNO)).ToString();
                    }
                }
                else
                {
                    if (strGRPFORMNO != "")
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE GRPFORMNO = " + (VB.Val(strGRPFORMNO)).ToString();
                    }
                }
            }
            SQL = SQL + ComNum.VBLF + strSubQuery;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 간호진단 조회
        /// </summary>
        /// <param name="strDEPTH"></param>
        /// <param name="strGRPFORMNO"></param>
        /// <param name="strGROUPPARENT"></param>
        /// <param name="strSubQuery"></param>
        /// <returns></returns>
        public static DataTable GetAEMRNRJINDANGRP(PsmhDb pDbCon, string strDEPTH = "", string strGRPFORMNO = "", string strGROUPPARENT = "", string strSubQuery = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strAplDate = "";

            strAplDate = ComQuery.CurrentDateTime(pDbCon, "D");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT GRPFORMNO, GRPFORMNAME, DEPTH, GROUPPARENT, DISPSEQ, DELYN ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRNRDIAGNOSISGROUP ";
            if (strDEPTH != "")
            {
                SQL = SQL + ComNum.VBLF + "WHERE DEPTH = " + (VB.Val(strDEPTH)).ToString();
                if (strGROUPPARENT != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GROUPPARENT = " + (VB.Val(strGROUPPARENT)).ToString();
                }
                if (strGRPFORMNO != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GRPFORMNO = " + (VB.Val(strGRPFORMNO)).ToString();
                }
            }
            else
            {
                if (strGROUPPARENT != "")
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE GROUPPARENT = " + (VB.Val(strGROUPPARENT)).ToString();
                    if (strGRPFORMNO != "")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND GRPFORMNO = " + (VB.Val(strGRPFORMNO)).ToString();
                    }
                }
                else
                {
                    if (strGRPFORMNO != "")
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE GRPFORMNO = " + (VB.Val(strGRPFORMNO)).ToString();
                    }
                }
            }
            SQL = SQL + ComNum.VBLF + strSubQuery;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 차트 대출 여부
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public static int GetEmrRequestCheck(PsmhDb pDbCon, string strPtNo, string strUseId)
        {
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            bool blnException = false;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PTNO,REQUSEID,REQSTDDATE,REQENDDATE,REQTYPE,CONDATE,CONUSEID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTREQ ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + " AND REQUSEID ='" + strUseId + "'";
                SQL = SQL + ComNum.VBLF + " AND REQENDDATE >= to_char(sysdate,'yyyymmdd') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    //등록된 사람은 대출 제외 한다.
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT BASCD, BASNAME, BASEXNAME, BASVAL, VFLAG1, VFLAG2, NFLAG1, NFLAG2, UNITCLS, REMARK1, REMARK2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                    SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = '의무기록실'";
                    SQL = SQL + ComNum.VBLF + "       AND UNITCLS = '대출제외'";
                    SQL = SQL + ComNum.VBLF + "       AND BASCD = '" + strUseId + "'";
                    SQL = SQL + ComNum.VBLF + "       AND USECLS = '0' ";
                    DataTable dt1 = null;

                    SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        blnException = true;
                    }
                    dt1.Dispose();
                    dt1 = null;

                    //if (clsType.User.strUseRght == "TOP" || clsType.User.strUseRght == "원무심사")
                    if (blnException == true)
                    {
                        rtnVal = 1;
                    }
                    else
                    {
                        //대출 옵션이 설정이 되면 코딩을 한다
                        rtnVal = 0;
                        //SQL = "";
                        //SQL = SQL + ComNum.VBLF + " SELECT ACPNO FROM";
                        //SQL = SQL + ComNum.VBLF + "    (SELECT ACPNO, INOUTCLS, MEDFRDATE,";
                        //SQL = SQL + ComNum.VBLF + "            CASE WHEN (INOUTCLS = 'I' AND MEDENDDATE = '99991231') THEN TO_CHAR(SYSDATE, 'YYYYMMDD')";
                        //SQL = SQL + ComNum.VBLF + "            ELSE MEDENDDATE END AS MEDENDDATE";
                        //SQL = SQL + ComNum.VBLF + "        FROM " + ComNum.DB_EMR + "AVIEWEMRACP";
                        //SQL = SQL + ComNum.VBLF + "        WHERE PTNO = '" + strPtNo + "')";
                        //SQL = SQL + ComNum.VBLF + " WHERE (INOUTCLS = 'O' AND TO_CHAR(SYSDATE, 'YYYYMMDD') <= TO_CHAR(TO_DATE(MEDFRDATE)+30, 'YYYYMMDD'))";
                        //SQL = SQL + ComNum.VBLF + " OR";
                        //SQL = SQL + ComNum.VBLF + " (INOUTCLS = 'I' AND TO_CHAR(SYSDATE, 'YYYYMMDD') <= TO_CHAR(TO_DATE(MEDENDDATE)+7, 'YYYYMMDD'))";

                        //SQL = "";
                        //SQL = SQL + ComNum.VBLF + " SELECT PATID FROM";
                        //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT  ";
                        //SQL = SQL + ComNum.VBLF + "    WHERE PATID = '" + strPtNo + "'";
                        //SQL = SQL + ComNum.VBLF + "    AND (CLASS = 'O' AND INDATE = TO_CHAR(SYSDATE, 'YYYYMMDD'))";
                        //SQL = SQL + ComNum.VBLF + "    OR";
                        //SQL = SQL + ComNum.VBLF + "    AND (CLASS = 'I' AND INDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') AND OUTDATE = '')";
                        //dt = clsDB.GetDataTableREx(SQL);

                        //if (dt == null)
                        //{
                        //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //    return rtnVal;
                        //}
                        //if (dt.Rows.Count == 0)
                        //{
                        //    rtnVal = 0;
                        //}
                        //else
                        //{
                        //    rtnVal = 1;
                        //}
                        //dt.Dispose();
                        //dt = null;
                    }
                }
                else
                {
                    if ((dt.Rows[0]["CONDATE"].ToString()).Trim() == "")
                    {
                        rtnVal = -1;
                        ComFunc.MsgBox("대출 신청이 승인전입니다.");
                    }
                    else
                    {
                        rtnVal = 1;
                    }
                    dt.Dispose();
                    dt = null;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }

        /// <summary>
        /// 기록지 출력 정보 저장
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <param name="strPRINTOPTION"></param>
        /// <param name="strPRINTOPTIONSUB"></param>
        /// <param name="strPrintDate"></param>
        /// <param name="strPrintTime"></param>
        /// <param name="strUseId"></param>
        /// <param name="strPRTREQNO"></param>
        /// <param name="strSCANNO"></param>
        /// <returns></returns>
        public static bool SaveEmrPrintHis(PsmhDb pDbCon, string strEmrNo, string strPRINTOPTION, string strPRINTOPTIONSUB,
                        string strPrintDate, string strPrintTime, string strUseId, string strPRTREQNO, string strSCANNO)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            //TODO
            try
            {
                if (strPRINTOPTION == "0")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRCHARTMST SET";
                    SQL = SQL + ComNum.VBLF + "      PRNTYN = '1'";
                    SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }


                #region 신규 테이블
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRPRINT";
                SQL = SQL + ComNum.VBLF + "    (PRINTNO, EMRNO, PRINTOPTION, PRINTOPTIONSUB, PRINTDATE, PRINTTIME, USEID, PRTREQNO, SCANNO )";
                SQL = SQL + ComNum.VBLF + "    VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_EMR + "AEMRPRINT_PRINTNO_SEQ.NextVal,";
                SQL = SQL + ComNum.VBLF + "      " + strEmrNo + ",";
                SQL = SQL + ComNum.VBLF + "      '" + strPRINTOPTION + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strPRINTOPTIONSUB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strPrintDate + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strPrintTime + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strUseId + "',";
                SQL = SQL + ComNum.VBLF + "      " + strPRTREQNO + ",";
                SQL = SQL + ComNum.VBLF + "      " + strSCANNO + ")";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                #endregion

                rtnVal = true;
                return rtnVal;
            }
            catch// (Exception ex)
            {
                //ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 스캔 이미지 갯수
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static int ScanImageYn(PsmhDb pDbCon, string strEmrNo)
        {
            int rtnVal = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT COUNT(SCANNO) AS SCANCNT FROM " + ComNum.DB_EMR + "AEMRSCAN";
            SQL = SQL + ComNum.VBLF + "     WHERE EMRNO = " + VB.Val(strEmrNo);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            rtnVal = Convert.ToInt32(VB.Val((dt.Rows[0]["SCANCNT"].ToString() + "").Trim()));
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 외래,입원 24시간 경과여부        
        /// </summary>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool isAuth24Hour(PsmhDb pDbCon, EmrPatient po, string strFormNo, string strUSEID)
        {

            //2016-11-30 락해제 수정전까지 임시로 바이패스
            return true;

            //--------------------------------------------



            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            bool rtnVal = false;

            bool isAuthCheck = false;
            string strINDATE = "";
            string strINTIME = "";

            //2016-11-29 PPM실 결핵정보 입력위해 예외처리
            if (strFormNo == "582")
            {
                return true;
            }

            //2016-07-27 외래,입원 24시간 이후 수정차단 여부 
            SQL = "";
            SQL = SQL + ComNum.VBLF + "            SELECT BASVAL FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "			              WHERE BSNSCLS = '의무기록실'	 ";
            SQL = SQL + ComNum.VBLF + "			              AND UNITCLS = '차트수정관리'	 ";
            SQL = SQL + ComNum.VBLF + "			              AND BASCD = 'BLOCK24H' 	 ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }

            if (VB.Left(dt.Rows[0]["BASVAL"].ToString().Trim(), 1) == "1")
            {
                isAuthCheck = true;
            }
            else
            {
                isAuthCheck = false;
            }


            string strCurDate = ComQuery.CurrentDateTime(pDbCon, "D");

            // 외래
            if ((po.inOutCls == "O") && (isAuthCheck == true))
            {
                // 외래당일 체크
                if (po.medFrDate == strCurDate)
                {
                    return true;
                }
                else
                {
                    if (po.medDeptCd == "EM")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT GB_SPC FROM MED_PMPA.OPD_MAST ";
                        SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + po.ptNo + "' ";
                        SQL = SQL + ComNum.VBLF + " AND ACT_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["GB_SPC"].ToString().Trim() != "*")
                            {
                                return true;
                            }
                        }
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(MAX(SYS_DATE),'YYYYMMDD') AS S_DATE, MAX(SYS_TIME) AS S_TIME ";
                    SQL = SQL + ComNum.VBLF + " FROM  MED_PMPA.OPD_SLIP ";
                    SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + po.ptNo + "' ";
                    SQL = SQL + ComNum.VBLF + " AND DEPT_CODE = '" + po.medDeptCd + "'";
                    SQL = SQL + ComNum.VBLF + " AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(po.medFrDate, "D", "-"), "D");
                    SQL = SQL + ComNum.VBLF + " AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입') ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {

                        strINDATE = dt.Rows[0]["S_DATE"].ToString().Trim();

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(MAX(SYS_DATE),'YYYYMMDD') AS S_DATE, MAX(SYS_TIME) AS S_TIME ";
                        SQL = SQL + ComNum.VBLF + " FROM  MED_PMPA.OPD_SLIP ";
                        SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + po.ptNo + "' ";
                        SQL = SQL + ComNum.VBLF + " AND DEPT_CODE = '" + po.medDeptCd + "'";
                        SQL = SQL + ComNum.VBLF + " AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(po.medFrDate, "D", "-"), "D");
                        SQL = SQL + ComNum.VBLF + " AND SYS_DATE = ( SELECT MAX(SYS_DATE) AS SYS_DATE ";
                        SQL = SQL + ComNum.VBLF + "                  FROM  MED_PMPA.OPD_SLIP  ";
                        SQL = SQL + ComNum.VBLF + "                  WHERE PATIENT_NO = '" + po.ptNo + "' ";
                        SQL = SQL + ComNum.VBLF + "                  AND DEPT_CODE = '" + po.medDeptCd + "'";
                        SQL = SQL + ComNum.VBLF + "                  AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(po.medFrDate, "D", "-"), "D");
                        SQL = SQL + ComNum.VBLF + "                  AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입')) ";
                        SQL = SQL + ComNum.VBLF + " AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입') ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }

                        strINTIME = VB.Replace(dt.Rows[0]["S_TIME"].ToString().Trim(), ":", "");


                        // 미종결
                        if (strINDATE == "" || strINDATE == null)
                        {
                            dt.Dispose();
                            dt = null;
                            return true;
                        }

                        // 외래24시간 체크
                        if (Convert.ToInt32(ComFunc.TimeDiffMin(ComFunc.FormatStrToDateEx(strINDATE, "D", "-") + " " + ComFunc.FormatStrToDateEx(strINTIME, "M", ":"),
                                                    ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":"))) < 1440)
                        {
                            dt.Dispose();
                            dt = null;
                            return true;
                        }
                        else
                        {
                            dt.Dispose();
                            dt = null;
                            rtnVal = false;
                        }
                    }
                    else
                    {
                        // 외래는 예약환자 미리 차팅하는 경우가 있음.
                        dt.Dispose();
                        dt = null;
                        //ComFunc.MsgBox("외래방문 내역이 없습니다.." + ComNum.VBLF + "수정할 수 없습니다.");
                        return true;
                    }
                }
            }

            // 입원
            if ((po.inOutCls == "I") && (isAuthCheck == true))
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM MED_OCS.INPATIENT_BY_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + po.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND IN_OR_OUT = '1' ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                // 재원환자
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PATID, CLINCODE, INDATE, INTIME, CLASS, OUTDATE, NVL(OUTTIME,0) OUTTIME FROM " + ComNum.DB_EMR + "EMR_TREATT ";
                SQL = SQL + ComNum.VBLF + " WHERE PATID = '" + po.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND CLASS = 'I' ";
                SQL = SQL + ComNum.VBLF + "     AND INDATE = " + po.medFrDate;
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }


                if (dt.Rows.Count > 0)
                {
                    // 재원환자
                    if (Convert.ToInt32(VB.Replace(dt.Rows[0]["OUTTIME"].ToString().Trim(), ":", "")) == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return true;
                    }
                    // 퇴원24시간 체크
                    else
                    {
                        if (Convert.ToInt32(ComFunc.TimeDiffMin(ComFunc.FormatStrToDateEx(dt.Rows[0]["OUTDATE"].ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(dt.Rows[0]["OUTTIME"].ToString().Trim(), "M", ":"),
                                                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":"))) < 1440)
                        {
                            dt.Dispose();
                            dt = null;
                            return true;
                        }
                        else
                        {
                            dt.Dispose();
                            dt = null;
                            rtnVal = false;
                        }
                    }
                }
            }
            
            // 2016-07-28 임시
            //if (clsCommon.gstrEXENAME == "MHEMRAK.EXE" || clsCommon.gstrEXENAME == "MHEMRPTDG.EXE" || clsCommon.gstrEXENAME == "MHEMRINJECTDG.EXE")
            //{
            //    rtnVal = true;
            //}

            //대출이 미비기록정리용인 경우
            SQL = "";
            SQL = "SELECT RENTNO, USERID, CLINCODE, RENTCODE, RENTNAME, RENTDT, RENTSDT, SEQ, PATID, RENTEDT , RENTAUTO, RENTREMARK,  CDATE, RENTHOPEDT, BANNABDATE";
            SQL = SQL + ComNum.VBLF + "FROM MED_EMR.RENTT@HAN_OCS ";
            SQL = SQL + ComNum.VBLF + "WHERE USERID = '" + strUSEID + "' ";
            SQL = SQL + ComNum.VBLF + "    AND PATID = '" + po.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "    AND RENTCODE = '002'";
            SQL = SQL + ComNum.VBLF + "    AND (RENTEDT IS NULL OR RENTEDT > TO_CHAR(SYSDATE,'YYYYMMDD'))";
            SQL = SQL + ComNum.VBLF + "    AND BANNABYN = 'N'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                return true;
            }

            dt.Dispose();
            dt = null;

            if (rtnVal == false)
            {
                ComFunc.MsgBox((po.inOutCls == "I" ? "퇴원" : "내원") + " 24시간이 경과한 환자입니다." + ComNum.VBLF + "작성을 원하실 경우 의무기록실로 문의하시기 바랍니다.");
            }

            return rtnVal;
        }

        /// <summary>
        /// 서식지 변경이 가능한지 여부
        /// </summary>
        /// <param name="strEMRNO"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool IsChangeAuth(PsmhDb pDbCon, string strEMRNO, string strUSEID)
        {
            //인증기간 풀기
            //bool rtnVal = false;
            ////return true;////인증기간 풀기

            //string SQL = "";
            //string SqlErr = ""; //에러문 받는 변수
            //DataTable dt = null;
            

            //SQL = "";
            //SQL = "SELECT ACPNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            //SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEMRNO;
            //SQL = SQL + ComNum.VBLF + "    AND CHARTUSEID = '" + strUSEID + "'";

            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return rtnVal;
            //}
            //if (dt.Rows.Count > 0)
            //{
            //    rtnVal = true;
            //}
            //else
            //{
            //    //ComFunc.MsgBox("수정, 삭제할 권한이 없습니다.");
            //}
            //dt.Dispose();
            //dt = null;
            return CanModify(pDbCon, strEMRNO).Equals("YES");

            ////전체 락 풀기
            //string strPRNTYN = "0";
            //string strACPNO = "";
            //string strPTNO = "";
            //string strCLASS = "";
            //string strINDATE = "";
            //string strINTIME = "";
            //string strOUTDATE = "";
            //string strOUTTIME = "";
            //string strDEPT = "";
            //bool isAuthCheck = false;
            //SQL = "";
            //SQL = "SELECT ACPNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            //SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + strEMRNO;

            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return false;
            //}
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    Cursor.Current = Cursors.Default;
            //    isAuthCheck = false;
            //}
            //else
            //{
            //    strACPNO = dt.Rows[0]["ACPNO"].ToString().Trim();
            //    dt.Dispose();
            //    dt = null;

            //    isAuthCheck = false;
            //}

            //SQL = "";
            //SQL = "SELECT * FROM " + ComNum.DB_EMR + "AEMRCHARTLOCK";
            //SQL = SQL + ComNum.VBLF + "    WHERE ACPNO = '" + strACPNO + "' ";
            //SQL = SQL + ComNum.VBLF + "        AND TRIM(STARTDATE || STARTTIME) <= '" + ComQuery.CurrentDateTime(pDbCon, "A") + "' ";
            //SQL = SQL + ComNum.VBLF + "        AND TRIM(ENDDATE || ENDTIME) >= '" + ComQuery.CurrentDateTime(pDbCon, "A") + "' ";

            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return false;
            //}
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    Cursor.Current = Cursors.Default;
            //    isAuthCheck = false;
            //}
            //else
            //{
            //    dt.Dispose();
            //    dt = null;

            //    return true;
            //}
            ////--전체 락 풀기

            ////2016-07-27 외래,입원 24시간 이후 수정차단 여부 
            //SQL = "";
            //SQL = SQL + ComNum.VBLF + "            SELECT BASVAL FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            //SQL = SQL + ComNum.VBLF + "			              WHERE BSNSCLS = '의무기록실'	 ";
            //SQL = SQL + ComNum.VBLF + "			              AND UNITCLS = '차트수정관리'	 ";
            //SQL = SQL + ComNum.VBLF + "			              AND BASCD = 'BLOCK24H' 	 ";
            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return false;
            //}

            //if (dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    Cursor.Current = Cursors.Default;
            //    return false;
            //}

            //if (VB.Left(dt.Rows[0]["BASVAL"].ToString().Trim(), 1) == "1")
            //{
            //    isAuthCheck = true;
            //}
            //else
            //{
            //    isAuthCheck = false;
            //}

            //dt.Dispose();
            //dt = null;

            ////2016-08-09 24시간 수정 예외처리
            //SQL = "";
            //SQL = SQL + ComNum.VBLF + " SELECT C.BASCD, BASNAME, BASVAL ";
            //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST M, " + ComNum.DB_EMR + "AEMRBASCD C ";
            //SQL = SQL + ComNum.VBLF + " WHERE (M.FORMNO||'') = (C.BASCD||'') ";
            //SQL = SQL + ComNum.VBLF + " AND C.BSNSCLS = '의무기록실' ";
            //SQL = SQL + ComNum.VBLF + " AND C.UNITCLS = '차트수정관리' ";
            //SQL = SQL + ComNum.VBLF + " AND C.BASVAL = '1' ";
            //SQL = SQL + ComNum.VBLF + " AND M.EMRNO = '" + strEMRNO + "' ";
            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return false;
            //}

            //if (dt.Rows.Count > 0)
            //{
            //    isAuthCheck = false;
            //}

            //Cursor.Current = Cursors.Default;

            //dt.Dispose();
            //dt = null;

            //try
            //{
            //    string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

            //    SQL = "";
            //    SQL = SQL + ComNum.VBLF + "SELECT C.ACPNO, C.PTNO, T.CLASS, T.INDATE, T.CLINCODE, C.CHARTUSEID, C.PRNTYN, T.DOCCODE ";
            //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
            //    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "EMR_TREATT T ";
            //    SQL = SQL + ComNum.VBLF + "    ON C.ACPNO = T.TREATNO ";
            //    SQL = SQL + ComNum.VBLF + "WHERE C.EMRNO = " + strEMRNO;
            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //        return false;
            //    }
            //    if (dt.Rows.Count == 0)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        ComFunc.MsgBox("작성내역이 없습니다." + ComNum.VBLF + "수정할 수 없습니다.");
            //        return false;
            //    }

            //    strPRNTYN = VB.Val(dt.Rows[0]["PRNTYN"].ToString().Trim()).ToString();

            //    strACPNO = dt.Rows[0]["ACPNO"].ToString().Trim();
            //    strPTNO = dt.Rows[0]["PTNO"].ToString().Trim();
            //    strCLASS = dt.Rows[0]["CLASS"].ToString().Trim();
            //    strINDATE = dt.Rows[0]["INDATE"].ToString().Trim();
            //    strDEPT = dt.Rows[0]["CLINCODE"].ToString().Trim();

            //    // 작성자 체크
            //    if (dt.Rows[0]["CHARTUSEID"].ToString().Trim() == strUSEID)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        rtnVal = true;
            //    }
            //    else if (dt.Rows[0]["DOCCODE"].ToString().Trim() == strUSEID)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        rtnVal = true;
            //    }
            //    else
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        ComFunc.MsgBox("작성자가 다릅니다." + ComNum.VBLF + "수정할 수 없습니다.");
            //        return false;
            //    }

            //    // 복사신청 체크
            //    SQL = " SELECT M.REQID, M.REQDATE, ";
            //    SQL = SQL + ComNum.VBLF + "    U.USENAME ";
            //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRPRTREQMST M ";
            //    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRPRTREQDTL D ";
            //    SQL = SQL + ComNum.VBLF + "    ON D.PRTREQSEQ = M.PRTREQSEQ ";
            //    SQL = SQL + ComNum.VBLF + "    AND D.DELCLS = '0'";
            //    SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
            //    SQL = SQL + ComNum.VBLF + "    ON U.USEID = M.REQID ";
            //    SQL = SQL + ComNum.VBLF + "WHERE D.EMRNO = " + strEMRNO;
            //    SQL = SQL + ComNum.VBLF + "    AND M.DELCLS = '0'";
            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //        return false;
            //    }

            //    if (dt.Rows.Count > 0)
            //    {
            //        string strReqNm = dt.Rows[0]["USENAME"].ToString().Trim();
            //        string strReqDt = ComFunc.FormatStrToDate(dt.Rows[0]["REQDATE"].ToString().Trim(), "D");
            //        dt.Dispose();
            //        dt = null;
            //        ComFunc.MsgBox(strReqNm + "님이 " + strReqDt + "에 복사 신청을 한 상태입니다." + ComNum.VBLF + "수정할 수 없습니다."
            //                                                + ComNum.VBLF + ComNum.VBLF + "복사 신청을 취소후 수정 하십시요.");
            //        return false;
            //    }

            //    if (strPRNTYN == "1")
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        ComFunc.MsgBox("원외출력된 기록지는 수정하실수 없습니다." + ComNum.VBLF + "수정을 원하실 경우 의무기록실로 문의하기 바랍니다.");

            //        return false;
            //    }

            //    string strCurDate = ComQuery.CurrentDateTime(pDbCon, "D");

            //    // 외래
            //    if ((strCLASS == "O") && (isAuthCheck == true))
            //    {
            //        // 외래당일 체크
            //        if (strINDATE == strCurDate)
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            if (strDEPT == "EM")
            //            {
            //                SQL = "";
            //                SQL = SQL + ComNum.VBLF + " SELECT GB_SPC FROM MED_PMPA.OPD_MAST ";
            //                SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
            //                SQL = SQL + ComNum.VBLF + " AND ACT_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
            //                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //                if (SqlErr != "")
            //                {
            //                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //                    return false;
            //                }

            //                if (dt.Rows.Count > 0)
            //                {
            //                    if (dt.Rows[0]["GB_SPC"].ToString().Trim() != "*")
            //                    {
            //                        return true;
            //                    }
            //                }
            //            }


            //            SQL = "";
            //            SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(MAX(SYS_DATE),'YYYYMMDD') AS S_DATE, MAX(SYS_TIME) AS S_TIME ";
            //            SQL = SQL + ComNum.VBLF + " FROM  MED_PMPA.OPD_SLIP ";
            //            SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
            //            SQL = SQL + ComNum.VBLF + " AND DEPT_CODE = '" + strDEPT + "'";
            //            SQL = SQL + ComNum.VBLF + " AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
            //            SQL = SQL + ComNum.VBLF + " AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입') ";

            //            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //            if (SqlErr != "")
            //            {
            //                ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //                return false;
            //            }
            //            if (dt.Rows.Count > 0)
            //            {
            //                strOUTDATE = dt.Rows[0]["S_DATE"].ToString().Trim();

            //                SQL = "";
            //                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(MAX(SYS_DATE),'YYYYMMDD') AS S_DATE, MAX(SYS_TIME) AS S_TIME ";
            //                SQL = SQL + ComNum.VBLF + " FROM  MED_PMPA.OPD_SLIP ";
            //                SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
            //                SQL = SQL + ComNum.VBLF + " AND DEPT_CODE = '" + strDEPT + "'";
            //                SQL = SQL + ComNum.VBLF + " AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
            //                SQL = SQL + ComNum.VBLF + " AND SYS_DATE = ( SELECT MAX(SYS_DATE) AS SYS_DATE ";
            //                SQL = SQL + ComNum.VBLF + "                  FROM  MED_PMPA.OPD_SLIP  ";
            //                SQL = SQL + ComNum.VBLF + "                  WHERE PATIENT_NO = '" + strPTNO + "' ";
            //                SQL = SQL + ComNum.VBLF + "                  AND DEPT_CODE = '" + strDEPT + "'";
            //                SQL = SQL + ComNum.VBLF + "                  AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
            //                SQL = SQL + ComNum.VBLF + "                  AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입')) ";
            //                SQL = SQL + ComNum.VBLF + " AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입') ";

            //                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //                if (SqlErr != "")
            //                {
            //                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //                    return false;
            //                }

            //                strOUTTIME = VB.Replace(dt.Rows[0]["S_TIME"].ToString().Trim(), ":", "");

            //                // 미종결
            //                if (strOUTDATE == "" || strOUTDATE == null)
            //                {
            //                    dt.Dispose();
            //                    dt = null;
            //                    return true;
            //                }

            //                // 외래24시간 체크
            //                if (Convert.ToInt32(ComFunc.TimeDiffMin(ComFunc.FormatStrToDateEx(strOUTDATE, "D", "-") + " " + ComFunc.FormatStrToDateEx(strOUTTIME, "M", ":"),
            //                                            ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":"))) < 1440)
            //                {
            //                    dt.Dispose();
            //                    dt = null;
            //                    return true;
            //                }
            //                else
            //                {
            //                    dt.Dispose();
            //                    dt = null;
            //                    rtnVal = false;
            //                }
            //            }
            //            else
            //            {
            //                // 외래는 예약환자 미리 차팅하는 경우가 있음.
            //                dt.Dispose();
            //                dt = null;
            //                //ComFunc.MsgBox("외래방문 내역이 없습니다.." + ComNum.VBLF + "수정할 수 없습니다.");
            //                return true;
            //            }
            //        }
            //    }

            //    // 입원
            //    if ((strCLASS == "I") && (isAuthCheck == true))
            //    {
            //        SQL = "";
            //        SQL = SQL + ComNum.VBLF + " SELECT * FROM MED_OCS.INPATIENT_BY_WARD ";
            //        SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
            //        SQL = SQL + ComNum.VBLF + " AND ENTER_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
            //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //        if (SqlErr != "")
            //        {
            //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //            return false;
            //        }

            //        // 재원환자
            //        if (dt.Rows.Count > 0)
            //        {
            //            dt.Dispose();
            //            dt = null;
            //            return true;
            //        }

            //        SQL = "";
            //        SQL = SQL + ComNum.VBLF + " SELECT PATID, CLINCODE, INDATE, INTIME, CLASS, OUTDATE, NVL(OUTTIME,0) OUTTIME FROM " + ComNum.DB_EMR + "EMR_TREATT ";
            //        SQL = SQL + ComNum.VBLF + " WHERE PATID = '" + strPTNO + "' ";
            //        SQL = SQL + ComNum.VBLF + "     AND CLASS = 'I' ";
            //        SQL = SQL + ComNum.VBLF + "     AND INDATE = " + strINDATE;
            //        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //        if (SqlErr != "")
            //        {
            //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //            return false;
            //        }


            //        if (dt.Rows.Count > 0)
            //        {
            //            // 재원환자
            //            if (Convert.ToInt32(VB.Replace(dt.Rows[0]["OUTTIME"].ToString().Trim(), ":", "")) == 0)
            //            {
            //                dt.Dispose();
            //                dt = null;
            //                return true;
            //            }
            //            // 퇴원24시간 체크
            //            else
            //            {
            //                if (Convert.ToInt32(ComFunc.TimeDiffMin(ComFunc.FormatStrToDateEx(dt.Rows[0]["OUTDATE"].ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(dt.Rows[0]["OUTTIME"].ToString().Trim(), "M", ":"),
            //                                                ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":"))) < 1440)
            //                {
            //                    dt.Dispose();
            //                    dt = null;
            //                    return true;
            //                }
            //                else
            //                {
            //                    dt.Dispose();
            //                    dt = null;
            //                    rtnVal = false;
            //                }
            //            }
            //        }
            //    }

            //    //대출이 미비기록정리용인 경우
            //    SQL = "";
            //    SQL = "SELECT RENTNO, USERID, CLINCODE, RENTCODE, RENTNAME, RENTDT, RENTSDT, SEQ, PATID, RENTEDT , RENTAUTO, RENTREMARK,  CDATE, RENTHOPEDT, BANNABDATE";
            //    SQL = SQL + ComNum.VBLF + "FROM MED_EMR.RENTT@HAN_OCS ";
            //    SQL = SQL + ComNum.VBLF + "WHERE USERID = '" + strUSEID + "' ";
            //    SQL = SQL + ComNum.VBLF + "    AND PATID = '" + strPTNO + "'";
            //    SQL = SQL + ComNum.VBLF + "    AND RENTCODE = '002'";
            //    SQL = SQL + ComNum.VBLF + "    AND (RENTEDT IS NULL OR RENTEDT > TO_CHAR(SYSDATE,'YYYYMMDD'))";
            //    SQL = SQL + ComNum.VBLF + "    AND BANNABYN = 'N'";

            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //        return false;
            //    }
            //    if (dt.Rows.Count > 0)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        return true;
            //    }

            //    dt.Dispose();
            //    dt = null;

            //    if (rtnVal == false)
            //    {
            //        ComFunc.MsgBox((strCLASS == "I" ? "퇴원" : "내원") + " 24시간이 경과한 환자입니다." + ComNum.VBLF + "수정을 원하실 경우 의무기록실로 수정요청을 하기 바랍니다.");
            //    }

            //    return rtnVal;
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //    return false;
            //}
        }

        /// <summary>
        /// 서식지 변경이 가능한지 여부 Old
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEMRNO"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool IsChangeAuthOld(PsmhDb pDbCon, string strEMRNO, string strUSEID)
        {
            //인증기간 풀기
            //bool rtnVal = false;
            ////return true;////인증기간 풀기

            //string SQL = "";
            //string SqlErr = ""; //에러문 받는 변수
            //DataTable dt = null;


            //SQL = "";
            //SQL = "SELECT EMRNO FROM " + ComNum.DB_EMR + "EMRXMLMST";
            //SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEMRNO;
            //SQL = SQL + ComNum.VBLF + "    AND USEID = '" + strUSEID + "'";

            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return rtnVal;
            //}
            //if (dt.Rows.Count > 0)
            //{
            //    rtnVal = true;
            //}
            //else
            //{
            //    //ComFunc.MsgBox("수정, 삭제할 권한이 없습니다.");
            //}
            //dt.Dispose();
            //dt = null;
            return CanModify(pDbCon, strEMRNO).Equals("YES");
        }

        /// <summary>
        /// 차트 출력 권한이 있는지 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEMRNO"></param>
        /// <param name="strUSEID"></param>
        /// <returns></returns>
        public static bool IsChangeAuthCopy(PsmhDb pDbCon, string strEMRNO, string strUSEID)
        {
            bool rtnVal = false;
            
            //2016-07-28 보험심사팀 저장막기
            if (clsType.User.DeptCode == "IRR")
            {
                ComFunc.MsgBox("현재 열람 권한만 있습니다." + ComNum.VBLF + "수정을 원하실 경우 의무기록실로 수정요청을 하기 바랍니다.");
                return false;
            }

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strPRNTYN = "0";
            string strACPNO = "";
            string strPTNO = "";
            string strCLASS = "";
            string strINDATE = "";
            //string strINTIME = "";
            string strOUTDATE = "";
            string strOUTTIME = "";
            string strDEPT = "";

            bool isAuthCheck = false;

            //2016-12-13
            //전체 락 풀기
            SQL = "";
            SQL = "SELECT ACPNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + strEMRNO;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                isAuthCheck = false;
            }
            else
            {
                strACPNO = dt.Rows[0]["ACPNO"].ToString().Trim();

                dt.Dispose();
                dt = null;

                isAuthCheck = false;
            }

            SQL = "";
            SQL = "SELECT * FROM " + ComNum.DB_EMR + "AEMRCHARTLOCK";
            SQL = SQL + ComNum.VBLF + "    WHERE ACPNO = '" + strACPNO + "' ";
            SQL = SQL + ComNum.VBLF + "        AND TRIM(STARTDATE || STARTTIME) <= '" + ComQuery.CurrentDateTime(pDbCon, "A") + "' ";
            SQL = SQL + ComNum.VBLF + "        AND TRIM(ENDDATE || ENDTIME) >= '" + ComQuery.CurrentDateTime(pDbCon, "A") + "' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                isAuthCheck = false;
            }
            else
            {
                dt.Dispose();
                dt = null;

                return true;
            }
            //--전체 락 풀기

            //2016-07-27 외래,입원 24시간 이후 수정차단 여부 
            SQL = "";
            SQL = SQL + ComNum.VBLF + "            SELECT BASVAL FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "			              WHERE BSNSCLS = '의무기록실'	 ";
            SQL = SQL + ComNum.VBLF + "			              AND UNITCLS = '차트수정관리'	 ";
            SQL = SQL + ComNum.VBLF + "			              AND BASCD = 'BLOCK24H' 	 ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }

            if (VB.Left(dt.Rows[0]["BASVAL"].ToString().Trim(), 1) == "1")
            {
                isAuthCheck = true;
            }
            else
            {
                isAuthCheck = false;
            }

            //2016-08-09 24시간 수정 예외처리
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT C.BASCD, BASNAME, BASVAL ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST M, " + ComNum.DB_EMR + "AEMRBASCD C ";
            SQL = SQL + ComNum.VBLF + " WHERE (M.FORMNO||'') = (C.BASCD||'') ";
            SQL = SQL + ComNum.VBLF + " AND C.BSNSCLS = '의무기록실' ";
            SQL = SQL + ComNum.VBLF + " AND C.UNITCLS = '차트수정관리' ";
            SQL = SQL + ComNum.VBLF + " AND C.BASVAL = '1' ";
            SQL = SQL + ComNum.VBLF + " AND M.EMRNO = '" + strEMRNO + "' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                isAuthCheck = false;
            }

            Cursor.Current = Cursors.Default;
            dt.Dispose();
            dt = null;


            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT C.ACPNO, C.PTNO, T.CLASS, T.INDATE, T.CLINCODE, C.CHARTUSEID, C.PRNTYN, T.DOCCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "EMR_TREATT T ";
                SQL = SQL + ComNum.VBLF + "    ON C.ACPNO = T.TREATNO ";
                SQL = SQL + ComNum.VBLF + "WHERE C.EMRNO = " + strEMRNO;
                //SQL = SQL + ComNum.VBLF + "    AND C.CHARTUSEID = '" + strUSEID + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("작성내역이 없습니다." + ComNum.VBLF + "수정할 수 없습니다.");
                    return false;
                }

                strPRNTYN = VB.Val(dt.Rows[0]["PRNTYN"].ToString().Trim()).ToString();

                strACPNO = dt.Rows[0]["ACPNO"].ToString().Trim();
                strPTNO = dt.Rows[0]["PTNO"].ToString().Trim();
                strCLASS = dt.Rows[0]["CLASS"].ToString().Trim();
                strINDATE = dt.Rows[0]["INDATE"].ToString().Trim();
                strDEPT = dt.Rows[0]["CLINCODE"].ToString().Trim();

                //if (dt.Rows[0]["CHARTUSEID"].ToString().Trim() == strUSEID)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    rtnVal = true;
                //}
                //else 
                //if (dt.Rows[0]["DOCCODE"].ToString().Trim() == strUSEID)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    rtnVal = true;
                //}
                //else
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("작성자가 다릅니다." + ComNum.VBLF + "수정할 수 없습니다.");
                //    return false;
                //}

                // New Lock 해제 된 것인지 파악을 한다.
                //SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT C.CHARTUSEID ";
                //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
                //SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTLOCKDTL L ";
                //SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = L.EMRNO ";
                //SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTLOCKMST M ";
                //SQL = SQL + ComNum.VBLF + "    ON L.LOCKNO = M.LOCKNO ";
                //SQL = SQL + ComNum.VBLF + "WHERE C.EMRNO = " + strEMRNO;
                //SQL = SQL + ComNum.VBLF + "    AND  " + strCurDateTime + " >= (M.STARTDATE || M.STARTTIME) ";
                //SQL = SQL + ComNum.VBLF + "    AND  " + strCurDateTime + " <= (M.ENDDATE || M.ENDTIME) ";
                //SQL = SQL + ComNum.VBLF + "    AND  M.MEDDRCD = '" + clsType.User.IdNumber + "'";
                //SQL = SQL + ComNum.VBLF + "    AND  M.DELYN = '0'";

                //dt = clsDB.GetDataTableREx(SQL);

                //if (dt == null)
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    return false;
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    dt.Dispose();
                //    dt = null;

                //    return true;
                //}


                // 복사신청 체크
                SQL = " SELECT M.REQID, M.REQDATE, ";
                SQL = SQL + ComNum.VBLF + "    U.USENAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRPRTREQMST M ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRPRTREQDTL D ";
                SQL = SQL + ComNum.VBLF + "    ON D.PRTREQSEQ = M.PRTREQSEQ ";
                SQL = SQL + ComNum.VBLF + "    AND D.DELCLS = '0'";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
                SQL = SQL + ComNum.VBLF + "    ON U.USEID = M.REQID ";
                SQL = SQL + ComNum.VBLF + "WHERE D.EMRNO = " + strEMRNO;
                SQL = SQL + ComNum.VBLF + "    AND M.DELCLS = '0'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    string strReqNm = dt.Rows[0]["USENAME"].ToString().Trim();
                    string strReqDt = ComFunc.FormatStrToDate(dt.Rows[0]["REQDATE"].ToString().Trim(), "D");
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox(strReqNm + "님이 " + strReqDt + "에 복사 신청을 한 상태입니다." + ComNum.VBLF + "수정할 수 없습니다."
                                                            + ComNum.VBLF + ComNum.VBLF + "복사 신청을 취소후 수정 하십시요.");
                    return false;
                }

                if (strPRNTYN == "1")
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("원외출력된 기록지는 수정하실수 없습니다." + ComNum.VBLF + "수정을 원하실 경우 의무기록실로 문의하기 바랍니다.");

                    return false;
                }

                string strCurDate = ComQuery.CurrentDateTime(pDbCon, "D");

                // 외래
                if ((strCLASS == "O") && (isAuthCheck == true))
                {
                    // 외래당일 체크
                    if (strINDATE == strCurDate)
                    {
                        return true;
                    }
                    else
                    {
                        if (strDEPT == "EM")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT GB_SPC FROM MED_PMPA.OPD_MAST ";
                            SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
                            SQL = SQL + ComNum.VBLF + " AND ACT_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return false;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["GB_SPC"].ToString().Trim() != "*")
                                {
                                    return true;
                                }
                            }
                        }
                        
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(MAX(SYS_DATE),'YYYYMMDD') AS S_DATE, MAX(SYS_TIME) AS S_TIME ";
                        SQL = SQL + ComNum.VBLF + " FROM  MED_PMPA.OPD_SLIP ";
                        SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + " AND DEPT_CODE = '" + strDEPT + "'";
                        SQL = SQL + ComNum.VBLF + " AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
                        SQL = SQL + ComNum.VBLF + " AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입') ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strOUTDATE = dt.Rows[0]["S_DATE"].ToString().Trim();

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(MAX(SYS_DATE),'YYYYMMDD') AS S_DATE, MAX(SYS_TIME) AS S_TIME ";
                            SQL = SQL + ComNum.VBLF + " FROM  MED_PMPA.OPD_SLIP ";
                            SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
                            SQL = SQL + ComNum.VBLF + " AND DEPT_CODE = '" + strDEPT + "'";
                            SQL = SQL + ComNum.VBLF + " AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
                            SQL = SQL + ComNum.VBLF + " AND SYS_DATE = ( SELECT MAX(SYS_DATE) AS SYS_DATE ";
                            SQL = SQL + ComNum.VBLF + "                  FROM  MED_PMPA.OPD_SLIP  ";
                            SQL = SQL + ComNum.VBLF + "                  WHERE PATIENT_NO = '" + strPTNO + "' ";
                            SQL = SQL + ComNum.VBLF + "                  AND DEPT_CODE = '" + strDEPT + "'";
                            SQL = SQL + ComNum.VBLF + "                  AND B_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
                            SQL = SQL + ComNum.VBLF + "                  AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입')) ";
                            SQL = SQL + ComNum.VBLF + " AND (TRIM(SEND_YYMM) IS NULL OR REMARK = '당입') ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return false;
                            }

                            strOUTTIME = VB.Replace(dt.Rows[0]["S_TIME"].ToString().Trim(), ":", "");

                            // 미종결
                            if (strOUTDATE == "" || strOUTDATE == null)
                            {
                                dt.Dispose();
                                dt = null;
                                return true;
                            }

                            // 외래24시간 체크
                            if (Convert.ToInt32(ComFunc.TimeDiffMin(ComFunc.FormatStrToDateEx(strOUTDATE, "D", "-") + " " + ComFunc.FormatStrToDateEx(strOUTTIME, "M", ":"),
                                                        ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":"))) < 1440)
                            {
                                dt.Dispose();
                                dt = null;
                                return true;
                            }
                            else
                            {
                                dt.Dispose();
                                dt = null;
                                rtnVal = false;
                            }
                        }
                        else
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("외래방문 내역이 없습니다.." + ComNum.VBLF + "수정할 수 없습니다.");
                            rtnVal = false;
                        }
                    }
                }

                // 입원
                if ((strCLASS == "I") && (isAuthCheck == true))
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM MED_OCS.INPATIENT_BY_WARD ";
                    SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + " AND ENTER_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDateEx(strINDATE, "D", "-"), "D");
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }

                    // 재원환자
                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return true;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT PATID, CLINCODE, INDATE, INTIME, CLASS, OUTDATE, NVL(OUTTIME,0) OUTTIME FROM " + ComNum.DB_EMR + "EMR_TREATT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PATID = '" + strPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND CLASS = 'I' ";
                    SQL = SQL + ComNum.VBLF + "     AND INDATE = " + strINDATE;
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }


                    if (dt.Rows.Count > 0)
                    {
                        // 재원환자
                        if (Convert.ToInt32(VB.Replace(dt.Rows[0]["OUTTIME"].ToString().Trim(), ":", "")) == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            return true;
                        }
                        // 퇴원24시간 체크
                        else
                        {
                            if (Convert.ToInt32(ComFunc.TimeDiffMin(ComFunc.FormatStrToDateEx(dt.Rows[0]["OUTDATE"].ToString().Trim(), "D", "-") + " " + ComFunc.FormatStrToDateEx(dt.Rows[0]["OUTTIME"].ToString().Trim(), "M", ":"),
                                                            ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":"))) < 1440)
                            {
                                dt.Dispose();
                                dt = null;
                                return true;
                            }
                            else
                            {
                                dt.Dispose();
                                dt = null;
                                rtnVal = false;
                            }
                        }
                    }
                }
                
                // 2016-07-28 임시
                //if (clsCommon.gstrEXENAME == "MHEMRAK.EXE" || clsCommon.gstrEXENAME == "MHEMRPTDG.EXE" || clsCommon.gstrEXENAME == "MHEMRINJECTDG.EXE")
                //{
                //    rtnVal = true;
                //}

                //대출이 미비기록정리용인 경우
                SQL = "";
                SQL = "SELECT RENTNO, USERID, CLINCODE, RENTCODE, RENTNAME, RENTDT, RENTSDT, SEQ, PATID, RENTEDT , RENTAUTO, RENTREMARK,  CDATE, RENTHOPEDT, BANNABDATE";
                SQL = SQL + ComNum.VBLF + "FROM MED_EMR.RENTT@HAN_OCS ";
                SQL = SQL + ComNum.VBLF + "WHERE USERID = '" + strUSEID + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PATID = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND RENTCODE = '002'";
                SQL = SQL + ComNum.VBLF + "    AND (RENTEDT IS NULL OR RENTEDT > TO_CHAR(SYSDATE,'YYYYMMDD'))";
                SQL = SQL + ComNum.VBLF + "    AND BANNABYN = 'N'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }

                dt.Dispose();
                dt = null;

                if (rtnVal == false)
                {
                    ComFunc.MsgBox((strCLASS == "I" ? "퇴원" : "내원") + " 24시간이 경과한 환자입니다." + ComNum.VBLF + "수정을 원하실 경우 의무기록실로 수정요청을 하기 바랍니다.");
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 사본발급여부를 가지고 온다.
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static bool GetPrnYnInfo(PsmhDb pDbCon, string strEmrNo)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //Lock 해제 된 것인지 파악을 한다.
            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT C.CHARTUSEID ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTLOCK L ";
            SQL = SQL + ComNum.VBLF + "    ON C.ACPNO = L.ACPNO ";
            SQL = SQL + ComNum.VBLF + "    AND L.DELYN = '0' ";
            SQL = SQL + ComNum.VBLF + "WHERE C.EMRNO = " + strEmrNo;
            SQL = SQL + ComNum.VBLF + "    AND  " + strCurDateTime + " >= (L.STARTDATE || L.STARTTIME) ";
            SQL = SQL + ComNum.VBLF + "    AND  " + strCurDateTime + " <= (L.ENDDATE || L.ENDTIME) ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            dt.Dispose();
            dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT PRNTYN ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
            SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = '" + strEmrNo + "'";
            SQL = SQL + ComNum.VBLF + "       AND PRNTYN = '1' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = true;
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 출력정보 저장
        /// </summary>
        /// <param name="Spd">스프래드 Sheet</param>
        /// <param name="strRowCol">방향</param>
        /// <param name="intChk">체크 Row or Col</param>
        /// <param name="intEmrNo">EMRNO Row or Col</param>
        /// <param name="intStartNo">data의 시작위치</param>
        /// <param name="strCurDateTime">현재 날짜 시간</param>
        /// <returns></returns>
        public static bool SaveEmrXmlPrnYn(PsmhDb pDbCon, FarPoint.Win.Spread.SheetView Spd, string strRowCol, int intChk, int intEmrNo, int intStartNo, string strCurDateTime)
        {
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);
            try
            {
                string strPrintDate = VB.Left(strCurDateTime, 8);
                string strPrintTime = VB.Right(strCurDateTime, 6);

                int i = 0;
                if (strRowCol == "COL")
                {
                    for (i = intStartNo; i < Spd.ColumnCount; i++)
                    {
                        if (VB.IsNumeric(Spd.Cells[intEmrNo, i].Text.Trim()) == false) continue;

                        if (intChk == -1)
                        {
                            if (clsEmrQuery.SaveEmrPrintHis(pDbCon, Spd.Cells[intEmrNo, i].Text.Trim(), clsFormPrint.mstrPRINTFLAG, "00",
                                                strPrintDate, strPrintTime, clsType.User.IdNumber, "0", "0") == false)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(Spd.Cells[intChk, i].Value) == true)
                            {
                                if (clsEmrQuery.SaveEmrPrintHis(pDbCon, Spd.Cells[intEmrNo, i].Text.Trim(), clsFormPrint.mstrPRINTFLAG, "00",
                                                    strPrintDate, strPrintTime, clsType.User.IdNumber, "0", "0") == false)
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (i = intStartNo; i < Spd.RowCount; i++)
                    {
                        if (VB.Val(Spd.Cells[i, intEmrNo].Text.Trim()) == 0) continue;
                        if (intChk == -1)
                        {
                            if (clsEmrQuery.SaveEmrPrintHis(pDbCon, Spd.Cells[i, intEmrNo].Text.Trim(), clsFormPrint.mstrPRINTFLAG, "00",
                                                    strPrintDate, strPrintTime, clsType.User.IdNumber, "0", "0") == false)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(Spd.Cells[i, intChk].Value) == true)
                            {
                                if (clsEmrQuery.SaveEmrPrintHis(pDbCon, Spd.Cells[i, intEmrNo].Text.Trim(), clsFormPrint.mstrPRINTFLAG, "00",
                                                    strPrintDate, strPrintTime, clsType.User.IdNumber, "0", "0") == false)
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                    }
                }

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
        /// 정형화 서식의 출력여부를 저장한다
        /// </summary>
        /// <param name="strEMRNO"></param>
        /// <param name="strSCANNO"></param>
        /// <returns></returns>
        public static bool SaveEmrXmlPrnYnForm(PsmhDb pDbCon, string strEMRNO, string strSCANNO)
        {
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strPrintDate = VB.Left(strCurDateTime, 8);
                string strPrintTime = VB.Right(strCurDateTime, 6);

                if (clsEmrQuery.SaveEmrPrintHis(pDbCon, strEMRNO, clsFormPrint.mstrPRINTFLAG, "00",
                                                    strPrintDate, strPrintTime, clsType.User.IdNumber, "0", strSCANNO) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

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
        /// 플로우 차트 저장
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCHARTUSEID"></param>
        /// <param name="strCOMPUSEID"></param>
        /// <param name="strSAVEGB"></param>
        /// <param name="strSAVECERT"></param>
        /// <param name="strFORMGB"></param>
        /// <param name="arryITEMCD"></param>
        /// <param name="arryITEMNO"></param>
        /// <param name="arryITEMINDEX"></param>
        /// <param name="arryITEMTYPE"></param>
        /// <param name="arryITEMVALUE"></param>
        /// <param name="arryDSPSEQ"></param>
        /// <param name="arryITEMVALUE1"></param>
        /// <returns></returns>
        public static double SaveFlowChart(PsmhDb pDbCon, EmrPatient po,
                                string strFormNo, string strUpdateNo, string strEmrNo, string strChartDate, string strChartTime,
                                string strCHARTUSEID, string strCOMPUSEID, string strSAVEGB, string strSAVECERT, string strFORMGB,
                                string[] arryITEMCD, string[] arryITEMNO, string[] arryITEMINDEX, string[] arryITEMTYPE, string[] arryITEMVALUE, string[] arryDSPSEQ, string[] arryITEMVALUE1)
        {
            double rtnVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            //int intRowAffected = 0; //변경된 Row 받는 변수
            double dblEmrHisNo = 0;
            double dblEmrNoNew = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                
                //dblEmrHisNo = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMSTHIS");
                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                if (VB.Val(strEmrNo) > 0)
                {
                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(pDbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", "", strCHARTUSEID);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion
                    dblEmrNoNew = VB.Val(strEmrNo);
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = "";

                if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, po, strFormNo, strUpdateNo,
                                    strChartDate, strChartTime,
                                    strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                                    strCurDate, strCurTime, strSaveFlag) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL = SQL + "\r\n" + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL = SQL + "\r\n" + " :ITEMCD, ";   //ITEMCD
                SQL = SQL + "\r\n" + " :ITEMNO, "; //ITEMNO
                SQL = SQL + "\r\n" + " :ITEMINDEX, "; //ITEMINDEX
                SQL = SQL + "\r\n" + " :ITEMTYPE, ";   //ITEMTYPE
                SQL = SQL + "\r\n" + " :ITEMVALUE, ";   //ITEMVALUE
                SQL = SQL + "\r\n" + " :DSPSEQ, ";   //DSPSEQ
                SQL = SQL + "\r\n" + " :ITEMVALUE1, ";   //ITEMVALUE
                SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteChartRow(pDbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                #endregion //CHARTROW

                rtnVal = dblEmrNoNew;

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 플로우 차트 저장 (INPUSEID, INPDATE, INPTIME 추가)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCHARTUSEID"></param>
        /// <param name="strCOMPUSEID"></param>
        /// <param name="strSAVEGB"></param>
        /// <param name="strSAVECERT"></param>
        /// <param name="strFORMGB"></param>
        /// <param name="arryITEMCD"></param>
        /// <param name="arryITEMNO"></param>
        /// <param name="arryITEMINDEX"></param>
        /// <param name="arryITEMTYPE"></param>
        /// <param name="arryITEMVALUE"></param>
        /// <param name="arryDSPSEQ"></param>
        /// <param name="arryITEMVALUE1"></param>
        /// <param name="arryINPUSEID"></param>
        /// <param name="arryINPDATE"></param>
        /// <param name="arryINPTIME"></param>
        /// <returns></returns>
        public static double SaveFlowChartEx(PsmhDb pDbCon, EmrPatient po,
                                string strFormNo, string strUpdateNo, string strEmrNo, string strChartDate, string strChartTime,
                                string strCHARTUSEID, string strCOMPUSEID, string strSAVEGB, string strSAVECERT, string strFORMGB,
                                string[] arryITEMCD, string[] arryITEMNO, string[] arryITEMINDEX, string[] arryITEMTYPE,
                                string[] arryITEMVALUE, string[] arryDSPSEQ, string[] arryITEMVALUE1,
                                string[] arryINPUSEID, string[] arryINPDATE, string[] arryINPTIME)
        {
            double rtnVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            double dblEmrHisNo = 0;
            double dblEmrNoNew = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                if (VB.Val(strEmrNo) > 0)
                {
                    #region //과거기록 백업
                    SqlErr = clsEmrQuery.SaveChartMastHis(pDbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", "", strCHARTUSEID);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion
                    dblEmrNoNew = VB.Val(strEmrNo);
                }
                else
                {
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //저장 CHRATMAST
                string strSaveFlag = "";

                if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, po, strFormNo, strUpdateNo,
                                    strChartDate, strChartTime,
                                    strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                                    strCurDate, strCurTime, strSaveFlag) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                #endregion //저장 CHRATMAST

                #region //CHARTROW
                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL = SQL + "\r\n" + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL = SQL + "\r\n" + " :ITEMCD, ";   //ITEMCD
                SQL = SQL + "\r\n" + " :ITEMNO, "; //ITEMNO
                SQL = SQL + "\r\n" + " :ITEMINDEX, "; //ITEMINDEX
                SQL = SQL + "\r\n" + " :ITEMTYPE, ";   //ITEMTYPE
                SQL = SQL + "\r\n" + " :ITEMVALUE, ";   //ITEMVALUE
                SQL = SQL + "\r\n" + " :DSPSEQ, ";   //DSPSEQ
                SQL = SQL + "\r\n" + " :ITEMVALUE1, ";   //ITEMVALUE
                SQL += ComNum.VBLF + " :INPUSEID,";   //INPUSEID
                SQL += ComNum.VBLF + " :INPDATE, ";   //INPDATE
                SQL += ComNum.VBLF + " :INPTIME ";   //INPTIME
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteChartRowEx(clsDB.DbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE,
                    arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1, arryINPUSEID, arryINPDATE, arryINPTIME);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                #endregion //CHARTROW

                rtnVal = dblEmrNoNew;

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }


        /// <summary>
        /// Batch 전장 인증
        /// </summary>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="strCurDateTime"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strITEMNO"></param>
        /// <param name="strITEMINDEX"></param>
        /// <param name="strITEMTYPE"></param>
        /// <param name="strITEMVALUE"></param>
        /// <param name="strDSPSEQ"></param>
        /// <param name="strITEMVALUE1"></param>
        /// <returns></returns>
        public static bool SaveDataAEMRCHARTCERTYbatch(double dblEmrNoNew, string strCurDateTime,
                                        string[] strITEMCD, string[] strITEMNO, string[] strITEMINDEX, string[] strITEMTYPE, string[] strITEMVALUE, string[] strDSPSEQ, string[] strITEMVALUE1)
        {
            bool rtnVal = false;
            //string strXML = "";
            return rtnVal;

            //try
            //{

            //    strXML = clsXML.SaveDataToXmlBatch(strITEMCD, strITEMNO, strITEMINDEX, strITEMTYPE, strITEMVALUE, strITEMVALUE1);

            //    //서명을 한다.
            //    string strCertKey = clsCerti.certDN.Trim();
            //    string strCertText = "";
            //    if (clsCerti.fCertiDg == null)
            //    {
            //        clsCerti.GetCert();
            //        clsCerti.ChangeCert();
            //        if (clsCerti.certDN.Trim() == "")
            //        {
            //            ComFunc.MsgBox("전자서명 인증서 로밍 실패로 처리할수 없습니다.다시 확인후 처리하십시요.");
            //            rtnVal = false;
            //            return rtnVal;
            //        }

            //        clsCerti.fCertiDg.EMRDoctor.useUI = 0;
            //        if (clsCerti.fCertiDg.EMRDoctor.SignData(strXML) != 0)
            //        {
            //            ComFunc.MsgBox("전자서명 인증서 로밍 실패로 처리할수 없습니다.다시 확인후 처리하십시요.");
            //            rtnVal = false;
            //            return rtnVal;
            //        }
            //        else
            //        {
            //            strCertText = clsCerti.fCertiDg.EMRDoctor.OutData;
            //        }
            //    }
            //    else
            //    {
            //        clsCerti.fCertiDg.EMRDoctor.useUI = 0;
            //        if (clsCerti.fCertiDg.EMRDoctor.SignData(strXML) != 0)
            //        {
            //            ComFunc.MsgBox("전자서명 인증서 로밍 실패로 처리할수 없습니다.다시 확인후 처리하십시요.");
            //            rtnVal = false;
            //            return rtnVal;
            //        }
            //        else
            //        {
            //            strCertText = clsCerti.fCertiDg.EMRDoctor.OutData;
            //        }
            //    }

            //    if (strCertText == "")
            //    {
            //        //ComFunc.MsgBox("전자서명 인증서 로밍 실패로 처리할수 없습니다.다시 확인후 처리하십시요.");
            //        rtnVal = false;
            //        return rtnVal;
            //    }

            //    if (clsDB.ExecuteChartCerty("", dblEmrNoNew.ToString(), clsType.User.IdNumber, clsType.User.strEmrPassWord, VB.Left(strCurDateTime, 8), VB.Right(strCurDateTime, 6), strCertKey, strCertText) == false)
            //    {
            //        return rtnVal;
            //    }

            //    rtnVal = true;

            //    return rtnVal;
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //    return rtnVal;
            //}
        }

        /// <summary>
        /// Chart 저장
        /// </summary>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="ChartForm"></param>
        /// <returns></returns>
        public static double SaveChartMst(PsmhDb pDbCon, EmrPatient po, Control ChartForm, bool isSpcPanel, Control pControl,
                                string strFormNo, string strUpdateNo, string strEmrNo, string strChartDate, string strChartTime,
                                string strCHARTUSEID, string strCOMPUSEID, string strSAVEGB, string strSAVECERT, string strFORMGB, string strSaveFlag, FarPoint.Win.Spread.FpSpread SpdWrite = null, string mDirection = "")
        {

            double dblEmrNoNew = 0;
            //string strEmrNoTR = "";
            //Control[] conTREmrNo = null;

            if (strEmrNo == "0")
            {
                if (isAuth24Hour(pDbCon, po, strFormNo, strCHARTUSEID) == false)
                {
                    return 0;
                }
            }

            // 기록지 저장
            dblEmrNoNew = SaveEmrChart(pDbCon, po, ChartForm, isSpcPanel, pControl,
                                                    strFormNo, strUpdateNo, strEmrNo, strChartDate, strChartTime,
                                                    strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB, strSaveFlag, SpdWrite, mDirection);

            #region //전자인증 하기
            if (strSAVECERT == "1")
            {
                //if (System.Diagnostics.Debugger.IsAttached == false)
                //{
                    bool blnCert = true;
                    if (dblEmrNoNew > 0)
                    {
                        string strUseId = strCHARTUSEID.To<int>().ToString("00000");
                        if (strSaveFlag.Equals("SAVE"))
                        {
                            #region 코딩 관련 EMR 작성시간, 작성자 가져오기
                            OracleDataReader dataReader = null;

                            string SQL = string.Empty;
                            SQL = "SELECT CHARTUSEID, WRITEDATE, WRITETIME";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + dblEmrNoNew;

                            string SqlErr = clsDB.GetAdoRs(ref dataReader, SQL, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return dblEmrNoNew;
                            }

                            if (dataReader.HasRows && dataReader.Read())
                            {
                                strUseId = dataReader.GetValue(0).ToString().Trim().To<int>().ToString("00000");
                            }

                            #endregion
                        }

                        if (clsCertWork.Cert_Check(clsDB.DbCon, strUseId) == true)
                        {
                            blnCert = SaveEmrCert(pDbCon, dblEmrNoNew);
                        }
                    }
                //}
            }

            #endregion

            #region //추후 필요시 코딩을 살려서 적용한다
            //if (dblEmrNoNew != 0)
            //{
            //    conTREmrNo = ChartForm.Controls.Find("lblEmrNoTR", true);

            //    if (conTREmrNo != null)
            //    {
            //        if (conTREmrNo.Length == 1)
            //        {
            //            if (conTREmrNo[0] is Label)
            //            {
            //                strEmrNoTR = ((Label)conTREmrNo[0]).Text.Trim();
            //            }
            //        }
            //    }

            //    AemrMapping(pDbCon, po, strFormNo, strEmrNo, Convert.ToString(Convert.ToInt32(dblEmrNoNew)), strEmrNoTR);

            //    if (AddSignCheck(pDbCon, strEmrNo) == true)
            //    {
            //        UpDateAddSign(pDbCon, strEmrNo, dblEmrNoNew);
            //    }
            //}
            #endregion

            #region //이미지 변환 //포항성모는 사용안함
            ////사용자 정의에 폼 확대 축소를 하지 않는 경우만 이미지 변환을 한다
            //if (clsEmrPublic.isEMRFORMZOOM == "1")
            //{
            //    return dblEmrNoNew;
            //}

            //if (dblEmrNoNew == 0)
            //{
            //    return dblEmrNoNew;
            //}

            //try
            //{
            //    string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
            //    string strCurDate = VB.Left(strCurDateTime, 8);
            //    string strCurTime = VB.Right(strCurDateTime, 6);

            //    string SQL = "";
            //    string SqlErr = ""; //에러문 받는 변수
            //    DataTable dt = null;
            //    ////정형화 서식중 이미지 변환을 하는 기록지는 이미지를 변환한다.
            //    ////확대된 경우는 이미지가 이상하게 되어 버린다.
            //    string strSUMMARYGB = "0";

            //    SQL = "";
            //    SQL = SQL + ComNum.VBLF + "SELECT F.CONVIMAGE, F.SUMMARYGB FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            //    SQL = SQL + ComNum.VBLF + "WHERE  F.FORMNO = " + strFormNo;
            //    SQL = SQL + ComNum.VBLF + "    AND F.UPDATENO = " + strUpdateNo;
            //    SQL = SQL + ComNum.VBLF + "    AND F.CONVIMAGE = '1'";
            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            //        return dblEmrNoNew;
            //    }
            //    if (dt.Rows.Count > 0)
            //    {
            //        strSUMMARYGB = dt.Rows[0]["SUMMARYGB"].ToString().Trim();

            //        dt.Dispose();
            //        dt = null;

            //        clsEmrPublic.isEMRFORMZOOMINIT = "0";
            //        if (clsEmrPublic.frmEmrChartToImageX == null)
            //        {
            //            clsEmrPublic.frmEmrChartToImageX = new frmEmrChartToImage();
            //            clsEmrPublic.frmEmrChartToImageX.Show();
            //            clsEmrPublic.frmEmrChartToImageX.Visible = false;
            //        }

            //        if (clsEmrPublic.frmEmrChartToImageX.ConChartToImage(strFormNo, strUpdateNo, po, dblEmrNoNew.ToString(), strChartDate, strCurDate) == false)
            //        {
            //        }
            //        if (clsEmrPublic.frmEmrChartToImageX != null)
            //        {
            //            clsEmrPublic.frmEmrChartToImageX = null;
            //        }
            //        clsEmrQuery.GetFormZoom(pDbCon);
            //    }
            //    else
            //    {
            //        dt.Dispose();
            //        dt = null;
            //    }
            //}
            //catch
            //{
            //    clsEmrQuery.GetFormZoom(pDbCon);
            //}
            #endregion //이미지변환

            return dblEmrNoNew;
        }

        /// <summary>
        /// EMR 전자 인증을 한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <returns></returns>
        public static bool SaveEmrCert(PsmhDb pDbCon, double dblEmrNoNew, bool Trans = true)
        {
            DataTable dt = null;
            int i = 0;
            string SQL = "";
            string SqlErr = "";

            double nCertno = 0;
            string strPTNO = string.Empty;
            string strDRSABUN = string.Empty;
            string strEMRDATA = string.Empty;
            string strSIGNDATA = string.Empty;
            string strHASHDATA = string.Empty;
            string strCERTDATA = string.Empty;
            string strSignData = string.Empty;
            string strROWID = string.Empty;

            ComFunc.ReadSysDate(pDbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");

            if (Trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT C.PTNO, C.FORMNO, C.UPDATENO, C.EMRNO, C.CHARTUSEID, C.ROWID, WRITEDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + " WHERE C.EMRNO = " + dblEmrNoNew;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                strPTNO = dt.Rows[0]["PTNO"].ToString().Trim();
                strEMRDATA = GetNewEmrJsonData(clsDB.DbCon, dt.Rows[i]["FORMNO"].ToString().Trim(), dt.Rows[i]["EMRNO"].ToString().Trim());
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                strDRSABUN = dt.Rows[0]["CHARTUSEID"].ToString().Trim().To<int>().ToString("00000");

                dt.Dispose();
                dt = null;

                if (string.IsNullOrEmpty(strEMRDATA) == true)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (strPTNO.Length == 8 && strPTNO.Substring(0, 6).Equals("810000"))
                {
                    if (Trans)
                    {
                        clsDB.setCommitTran(pDbCon);
                    }
                    return true;
                }

                // 전자인증 시퀀스 생성
                SQL = " SELECT " + ComNum.DB_PMPA + "SEQ_CERTPOOL.NEXTVAL  NVAL FROM DUAL  ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    clsDB.SaveSqlErrLog("함수명 : " + "SaveEmrCert" + ComNum.VBLF + SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    nCertno = Double.Parse(dt.Rows[0]["NVAL"].ToString());
                }
                dt.Dispose();
                dt = null;

                // 전자인증
                //if (clsCertWork.CertWorkEx(pDbCon, clsType.User.Sabun.Trim(), strEMRDATA, ref strHASHDATA, ref strCERTDATA) == true)
                //{
                //    if (Trans)
                //    {
                //        clsDB.setRollbackTran(pDbCon);
                //    }
                //    return false;
                //}

                // 전자인증
                if (clsCertWork.CertWorkBacth(pDbCon, strDRSABUN, CERTDATE, clsCertWork.AEMRCHARTMST, strPTNO, strEMRDATA, ref strHASHDATA, ref strSignData, ref strCERTDATA, ref nCertno) != "OK")
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_EMR, clsCertWork.AEMRCHARTMST, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strROWID) == false)
                {
                    if (Trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    return false;
                }

                if (Trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Trans)
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                
                return false;
            }
        }

        public static string GetNewEmrJsonData(PsmhDb pDbCon, string strFormNo, string strEmrNo)
        {
            string rtnVal = "";
            OracleDataReader reader = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string quote = "\"";

            try
            {
                if (strFormNo.Equals("1568"))
                {
                    // 1568 : 마취기록지
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNO, \r";
                    SQL += "    '{' || '\"ITEMCD\"' || ':\"' || B.TITLE || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.CONTROLVALUE || '\"}' AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRANCHART B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                    SQL += ") \r";
                    SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNO \r";
                }
                else if (strFormNo.Equals("965") || strFormNo.Equals("2049") || strFormNo.Equals("2137"))
                {
                    // 965 : 간호기록지
                    // 2049 : 응급 간호기록지(ER)
                    // 2137 : 회복실 간호기록지(RE)
                    SQL = "";
                    SQL += "SELECT 'WARDCODE : ' || WARDCODE || ' | ' || \r";
                    SQL += "       'ROOMCODE : ' || ROOMCODE || ' | ' || \r";
                    SQL += "       'PROBLEMCODE : ' || PROBLEMCODE || ' | ' || \r";
                    SQL += "       'PROBLEM : ' || PROBLEM || ' | ' || \r";
                    SQL += "       'TYPE : ' || TYPE || ' | ' || \r";
                    SQL += "       'NRRECODE : ' || NRRECODE AS JSON \r";
                    SQL += " FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRNURSRECORD B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                }
                else if (strFormNo.Equals("1575"))
                {
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNO, \r";
                    SQL += "    TO_CLOB('{' || '\"ITEMCD\"' || ':\"' || B.ITEMCD || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.ITEMVALUE || '\", ' || '\"ITEMVALUE1\"' || ':\"' || B.ITEMVALUE1 || '\", ITEMVALUE2\"' || ':\"' || B.ITEMVALUE2 || '\"}') AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "  AND B.ITEMCD > CHR(0) \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                    SQL += "ORDER BY B.DSPSEQ \r";
                    SQL += ") \r";
                    SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNO \r";
                }
                else
                {
                    #region 4천바이트 넘는게 있는지 확인
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS(                                                                                     \r";
                    SQL += "SELECT  LENGTHB(B.ITEMVALUE) SIZE1                                                                      \r";
                    SQL += "    ,   LENGTHB(B.ITEMVALUE1) SIZE2                                                                     \r";
                    SQL += "    ,   LENGTHB(B.ITEMVALUE2) SIZE3                                                                     \r";
                    SQL += "    ,   B.ITEMCD                                                                                        \r";
                    SQL += "    ,   B.ITEMVALUE                                                                                     \r";
                    SQL += "    ,   B.ITEMVALUE1                                                                                    \r";
                    SQL += "    ,   B.ITEMVALUE2                                                                                    \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A                                                                          \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B                                                                    \r";
                    SQL += "   ON A.EMRNO = B.EMRNO                                                                                 \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS                                                                           \r";
                    SQL += "  AND B.ITEMCD > CHR(0)                                                                                 \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "                                                                        \r";
                    SQL += ")                                                                                                       \r";
                    SQL += "  SELECT                                                                                                \r";
                    SQL += "          ITEMCD                                                                                        \r";
                    SQL += "    ,     ITEMVALUE                                                                                     \r";
                    SQL += "    ,     ITEMVALUE1                                                                                    \r";
                    SQL += "    ,     ITEMVALUE2                                                                                    \r";
                    SQL += "    FROM TEMP_TABLE                                                                                     \r";
                    SQL += "   WHERE (NVL(SIZE1, 0) + NVL(SIZE2, 0)  + NVL(SIZE3, 0)) >= 3500                                       \r";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }
                    #endregion


                    if (reader.HasRows)
                    {
                        StringBuilder builder = new StringBuilder();
                        while (reader.Read())
                        {
                            builder.Append("{").Append(quote).Append("ITEMCD").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(0).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(1).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE1").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(2).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE2").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(3).ToString().Trim()).Append(quote).Append("}");
                        }

                        rtnVal = builder.ToString().Trim();

                    }
                    else
                    {
                        reader.Dispose();
                        reader = null;

                        SQL = "";
                        SQL += "WITH TEMP_TABLE AS( \r";
                        SQL += "SELECT \r";
                        SQL += "    A.EMRNO, \r";
                        SQL += "    TO_CLOB('{' || '\"ITEMCD\"' || ':\"' || B.ITEMCD || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.ITEMVALUE || '\"}') AS ITEM \r";
                        SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                        SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B \r";
                        SQL += "   ON A.EMRNO = B.EMRNO \r";
                        SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                        SQL += "  AND B.ITEMCD > CHR(0) \r";
                        SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                        SQL += "ORDER BY B.DSPSEQ \r";
                        SQL += ") \r";
                        SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                        SQL += "  FROM TEMP_TABLE \r";
                        SQL += " GROUP BY EMRNO \r";
                    }
                }

                if (rtnVal.IsNullOrEmpty())
                {
                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        //rtnVal = dt.Rows[0]["JSON"].ToString().Trim();
                        rtnVal = reader.GetValue(0).ToString().Trim();
                        //rtnVal = rtnVal.Substring(0, rtnVal.Length > 3000 ? 3000 : rtnVal.Length);
                    }
                }

                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
                
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }
        /// <summary>
        /// 현재 사용 안함
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="dblEmrNoNew"></param>
        private static void UpDateAddSign(PsmhDb pDbCon, string strEmrNo, double dblEmrNoNew)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_EMR + "AEMRDRSIGNADD        ";
                SQL = SQL + ComNum.VBLF + "SET    EMRNO  = " + dblEmrNoNew + "     ";
                SQL = SQL + ComNum.VBLF + "WHERE  EMRNO  = " + strEmrNo + "         ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 현재 사용 안함
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        private static bool AddSignCheck(PsmhDb pDbCon, string strEmrNo)
        {
            bool bolReturn = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = "SELECT *          ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRDRSIGNADD        ";
            SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo + "          ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return bolReturn;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return bolReturn;
            }

            bolReturn = true;

            dt.Dispose();
            dt = null;

            return bolReturn;
        }

        /// <summary>
        /// 기록지 내용을 백업한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <param name="strFlag"></param>
        /// <returns></returns>
        public static string SaveChartMastHis(PsmhDb pDbCon, string strEmrNo, double dblEmrHisNo, string strCurDate, string strCurTime, string strFlag, string strSaveFlag, string strCHARTUSEID)
        {
            string rtnVal = "OK";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;


            try
            {
                if (string.IsNullOrWhiteSpace(strCHARTUSEID))
                {
                    strCHARTUSEID = clsType.User.IdNumber;
                }
                //AEMRCHARTMST
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTMSTHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ACPNO,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, UPDATENO, CHARTDATE,  ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME, CHARTUSEID, WRITEDATE,  ";
                SQL = SQL + ComNum.VBLF + "    WRITETIME, COMPUSEID, COMPDATE,  ";
                SQL = SQL + ComNum.VBLF + "    COMPTIME, PRNTYN, SAVEGB,  ";
                SQL = SQL + ComNum.VBLF + "    SAVECERT, FORMGB, PTNO, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD,  ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, OPDATE, OPDEGREE,  ";
                SQL = SQL + ComNum.VBLF + "    OP_DEPT, DEPTCDNOW, DRCDNOW,  ";
                SQL = SQL + ComNum.VBLF + "    ROOM_NO, ACPNOOUT, CURDEPT,  ";
                SQL = SQL + ComNum.VBLF + "    CURGRADE, CODEUSEID, CODEDATE, CODETIME,  CERTNO, CERTDATE, ";
                SQL = SQL + ComNum.VBLF + "    DCEMRNOHIS, DCUSEID,  ";
                SQL = SQL + ComNum.VBLF + "    DCDATE, DCTIME )";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, ACPNO,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, UPDATENO, CHARTDATE, ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME, CHARTUSEID, WRITEDATE,  ";
                SQL = SQL + ComNum.VBLF + "    WRITETIME, COMPUSEID, COMPDATE, ";
                SQL = SQL + ComNum.VBLF + "    COMPTIME, PRNTYN, SAVEGB, ";
                SQL = SQL + ComNum.VBLF + "    SAVECERT, FORMGB, PTNO,  ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD,  ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, OPDATE, OPDEGREE,  ";
                SQL = SQL + ComNum.VBLF + "    OP_DEPT, DEPTCDNOW, DRCDNOW,  ";
                SQL = SQL + ComNum.VBLF + "    ROOM_NO, ACPNOOUT, CURDEPT,  ";
                SQL = SQL + ComNum.VBLF + "    CURGRADE, CODEUSEID, CODEDATE, CODETIME, CERTNO, CERTDATE, ";
                SQL = SQL + ComNum.VBLF + dblEmrHisNo + " AS DCEMRNOHIS, ";
                if (strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + "      CHARTUSEID,";
                }
                else
                {
                    //SQL = SQL + ComNum.VBLF + "    '" + clsType.User.IdNumber + "',";   //DCUSEID
                    SQL = SQL + ComNum.VBLF + "    '" + strCHARTUSEID + "',";   //DCUSEID
                }
                SQL = SQL + ComNum.VBLF + "    '" + strCurDate + "',";   //DCDATE
                SQL = SQL + ComNum.VBLF + "    '" + strCurTime + "' ";   //DCTIME
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                //AEMRCHARTROW
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROWHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUTGB, INPUSEID, INPDATE, INPTIME, ITEMVALUE2)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUTGB, INPUSEID, INPDATE, INPTIME, ITEMVALUE2";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTROW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                //SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + dblEmrHisNoOld;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTROW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                //SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + dblEmrHisNoOld;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                //AEMRCHARTDRAW
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTDRAWHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, FORMNO, UPDATENO, DRAW, FILENAME, IMAGE, IMAGENAME, ITEMNAME)";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, FORMNO, UPDATENO, DRAW, FILENAME, IMAGE, IMAGENAME, ITEMNAME";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTDRAW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTDRAW";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                //간호기록

                //AEMRNURSRECORD
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRNURSRECORDHIS ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, WARDCODE, ROOMCODE, PROBLEMCODE, PROBLEM, TYPE, NRRECODE) ";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, WARDCODE, ROOMCODE, PROBLEMCODE, PROBLEM, TYPE, NRRECODE ";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRNURSRECORD ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRNURSRECORD";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    rtnVal = SqlErr;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                rtnVal = ex.Message;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 실제 EMR 저장하는 루틴
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="po"></param>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCHARTUSEID"></param>
        /// <param name="strCOMPUSEID"></param>
        /// <param name="strSAVEGB"></param>
        /// <param name="strSAVECERT"></param>
        /// <param name="strFORMGB"></param>
        /// <param name="strSaveFlag"></param>
        /// <param name="SpdWrite"></param>
        /// <param name="mDirection"></param>
        /// <returns></returns>
        public static double SaveEmrChart(PsmhDb pDbCon, EmrPatient po, Control ChartForm, bool isSpcPanel, Control pControl,
                                string strFormNo, string strUpdateNo, string strEmrNo, string strChartDate, string strChartTime,
                                string strCHARTUSEID, string strCOMPUSEID, string strSAVEGB, string strSAVECERT, string strFORMGB, string strSaveFlag, FarPoint.Win.Spread.FpSpread SpdWrite = null, string mDirection = "")
        {

            if (strSaveFlag.Equals("SAVE") && VB.Val(strEmrNo) == 0)
            {
                //ComFunc.MsgBoxEx(ChartForm as Form, "신규 작성은 저장 할 수 없습니다", "에러");
                return 0;
            }

            double rtnVal = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수
            double dblEmrHisNo = 0; //무저건 발생한다
            double dblEmrNoNew = 0;

            Dictionary<string, object> param = new Dictionary<string, object>();

            //---------------------------
            // EMRNO가 있을 경우 EMRHISNO만 증가를 시킨다
            //---------------------------

         

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                if (strChartTime.Length < 6)
                {
                    strChartTime = ComFunc.RPAD(strChartTime, 6, "0");
                }

                //dblEmrHisNo = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMSTHIS");
                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                string strUseId = string.Empty;
                string strWriteDate = string.Empty;
                string strWriteTime = string.Empty;
                string strJobFold = Path.Combine(clsEmrType.EmrSvrInfo.EmrClient, "EmrImageTmp\\New");
                string strSearchWord = string.Concat(strFormNo, "_", strUpdateNo, "_");

                if (VB.Val(strEmrNo) > 0)
                {
                    if (strSaveFlag.Equals("SAVE"))
                    {
                        #region 코딩 관련 EMR 작성시간, 작성자 가져오기
                        OracleDataReader dataReader = null;

                        SQL = "SELECT CHARTUSEID, WRITEDATE, WRITETIME";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                        SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                        SqlErr = clsDB.GetAdoRs(ref dataReader, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dataReader.HasRows && dataReader.Read())
                        {
                            strUseId = dataReader.GetValue(0).ToString().Trim();
                            strWriteDate = dataReader.GetValue(1).ToString().Trim();
                            strWriteTime = dataReader.GetValue(2).ToString().Trim();
                        }

                        #endregion
                    }

                    #region //과거기록 백업
                    SqlErr = SaveChartMastHis(pDbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", strSaveFlag, strCHARTUSEID);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion

                    strSearchWord = string.Concat(strEmrNo, "_");
                    strJobFold = Path.Combine(clsEmrType.EmrSvrInfo.EmrClient, "EmrImageTmp\\Update");
                    dblEmrNoNew = VB.Val(strEmrNo);
                }
                else
                {
                    //dblEmrNoNew = ComQuery.GetSequencesNo(pDbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                #region //접수마스터에 업데이트 한다. TEXT_EMRYN = '1'

                //SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT F.GRPFORMNO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
                //SQL = SQL + ComNum.VBLF + "WHERE  F.GRPFORMNO IN (1000,1001)";
                //SQL = SQL + ComNum.VBLF + "    AND F.FORMNO = " + strFormNo;
                //SQL = SQL + ComNum.VBLF + "    AND F.FORMNO <> 336 ";
                //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                //if (SqlErr != "")
                //{
                //    clsDB.setRollbackTran(pDbCon);
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //    return rtnVal;
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    string strSS = "1";
                //    if (strSAVEGB == "1")
                //    {
                //        strSS = "2";
                //    }
                //    else
                //    {
                //        strSS = "1";
                //    }
                //    SQL = "";
                //    SQL = SQL + ComNum.VBLF + "UPDATE  MED_PMPA.OPD_MAST ";
                //    SQL = SQL + ComNum.VBLF + "        SET TEXT_EMRYN = '" + strSS + "'";
                //    SQL = SQL + ComNum.VBLF + "    WHERE ACT_DATE = TO_DATE('" + ComFunc.FormatStrToDate(po.medFrDate, "D") + "','YYYY-MM-DD')";
                //    SQL = SQL + ComNum.VBLF + "    AND PATIENT_NO = '" + po.ptNo + "'";
                //    SQL = SQL + ComNum.VBLF + "    AND TRIM(DEPT_CODE) = '" + po.medDeptCd + "'";
                //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                //    if (SqlErr != "")
                //    {
                //        clsDB.setRollbackTran(pDbCon);
                //        ComFunc.MsgBox(SqlErr);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //        Cursor.Current = Cursors.Default;
                //        return rtnVal;
                //    }
                //}
                //dt.Dispose();
                //dt = null;
                #endregion

                #region //저장 CHRATMAST
                string strSaveId = strSaveFlag.Equals("SAVE") ? strUseId : strCHARTUSEID;
                if (SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, po, strFormNo, strUpdateNo, 
                                    strChartDate, strChartTime,
                                    strSaveId, strSaveId, strSAVEGB, strSAVECERT, strFORMGB,
                                    strCurDate, strCurTime, strSaveFlag) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                #endregion //저장 CHRATMAST

                #region //AEMRCHARTROW
                if (SpdWrite != null)
                {
                    if (SaveDataAEMRCHARTROWSpd(pDbCon, dblEmrNoNew, dblEmrHisNo, strFormNo, strUpdateNo, SpdWrite, mDirection) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                else
                {
                    #region //  이미지 및 어노테이션 정보 저장
                    DirectoryInfo directory = new DirectoryInfo(strJobFold);
                    DirectoryInfo[] dirs = directory.GetDirectories(string.Concat(strSearchWord, "*"));
                    foreach (DirectoryInfo dir in dirs)
                    {
                        if (dir.Name.LastIndexOf("BaseImage") > -1)
                        {
                            continue;
                        }

                        string itemName = string.Empty;

                        if (VB.Val(strEmrNo) > 0)
                        {
                            itemName = dir.Name.Replace(string.Concat(strEmrNo, "_"), "");
                        }
                        else
                        {
                            itemName = dir.Name.Replace(string.Concat(strFormNo, "_", strUpdateNo, "_"), "");
                        }

                        FileInfo[] files = dir.GetFiles("*.jpg");
                        foreach (FileInfo imageFile in files)
                        {
                            FileInfo dtlFile = new FileInfo(imageFile.FullName.Replace(".jpg", ".dtl"));
                            byte[] dtlBytes = null;
                            byte[] imgBytes = null;

                            if (File.Exists(dtlFile.FullName))
                            {
                                dtlBytes = File.ReadAllBytes(dtlFile.FullName);
                            }

                            if (File.Exists(imageFile.FullName))
                            {
                                imgBytes = File.ReadAllBytes(imageFile.FullName);
                            }

                            SQL = "";
                            SQL += "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTDRAW (  \r\n";
                            SQL += "    EMRNO                               \r\n";
                            SQL += "  , EMRNOHIS                            \r\n";
                            SQL += "  , FORMNO                              \r\n";
                            SQL += "  , UPDATENO                            \r\n";
                            SQL += "  , DRAW                                \r\n";
                            SQL += "  , FILENAME                            \r\n";
                            SQL += "  , IMAGE                               \r\n";
                            SQL += "  , IMAGENAME                           \r\n";
                            SQL += "  , ITEMNAME                            \r\n";
                            SQL += ") VALUES (                              \r\n";
                            SQL += "    :EMRNO                              \r\n";
                            SQL += "  , :EMRNOHIS                           \r\n";
                            SQL += "  , :FORMNO                             \r\n";
                            SQL += "  , :UPDATENO                           \r\n";
                            SQL += "  , :DRAW                               \r\n";
                            SQL += "  , :FILENAME                           \r\n";
                            SQL += "  , :IMAGE                              \r\n";
                            SQL += "  , :IMAGENAME                          \r\n";
                            SQL += "  , :ITEMNAME                           \r\n";
                            SQL += ")                                       \r\n";

                            param = new Dictionary<string, object>();
                            param.Add("EMRNO", dblEmrNoNew);
                            param.Add("EMRNOHIS", dblEmrHisNo);
                            param.Add("FORMNO", strFormNo);
                            param.Add("UPDATENO", strUpdateNo);
                            if (dtlBytes != null)
                            {
                                param.Add("DRAW", Convert.ToBase64String(dtlBytes));
                            }
                            else
                            {
                                param.Add("DRAW", dtlBytes);
                            }

                            param.Add("FILENAME", dtlFile.Name);
                            if (imgBytes != null)
                            {
                                param.Add("IMAGE", Convert.ToBase64String(imgBytes));
                            }
                            else
                            {
                                param.Add("DRAW", imgBytes);
                            }


                            param.Add("IMAGENAME", imageFile.Name);
                            param.Add("ITEMNAME", itemName);

                            clsDB.ExecuteBlob(SQL, param, ref intRowAffected, pDbCon);
                            imageFile.Delete();
                            dtlFile.Delete();
                        }
                    }

                    #endregion //  이미지 및 어노테이션 정보 저장

                    #region //저장 AEMRCHARTROW
                     if (SaveDataAEMRCHARTROW(pDbCon, ChartForm, false, null, dblEmrNoNew, dblEmrHisNo) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion //저장 AEMRCHARTROW

                    //포항성모 이상부터 사용안함 : 지우지 마시요
                    //if (SaveDataAEMRCHARTIAMGE(pDbCon, ChartForm, false, null, strFormNo, strUpdateNo, dblEmrHisNo, VB.Val(strEmrNo)) == false)
                    //{
                    //    clsDB.setRollbackTran(pDbCon);
                    //    ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                    //    Cursor.Current = Cursors.Default;
                    //    return rtnVal;
                    //}
                }
                #endregion //AEMRCHARTROW

                rtnVal = dblEmrNoNew;

                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// ChartMst에만 등록
        /// </summary>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCHARTUSEID"></param>
        /// <param name="strCOMPUSEID"></param>
        /// <param name="strSAVEGB"></param>
        /// <param name="strFORMGB"></param>
        /// <returns></returns>
        public static bool SaveChartMstOnly(PsmhDb pDbCon, double dblEmrNoNew, double dblEmrHisNo, EmrPatient po,
                                string strFormNo, string strUpdateNo, string strChartDate, string strChartTime,
                                string strCHARTUSEID, string strCOMPUSEID, string strSAVEGB, string strSAVECERT, string strFORMGB,
                                string strCurDate, string strCurTime, string strSaveFlag)
        {
            bool rtnVal = true;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (strChartTime.Length < 6)
            {
                strChartTime = strChartTime.PadRight(6, '0');
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTMST ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ACPNO,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNO, UPDATENO, CHARTDATE,  ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME, CHARTUSEID, WRITEDATE,  ";
                SQL = SQL + ComNum.VBLF + "    WRITETIME, COMPUSEID, COMPDATE,  ";
                SQL = SQL + ComNum.VBLF + "    COMPTIME, PRNTYN, SAVEGB,  ";
                SQL = SQL + ComNum.VBLF + "    SAVECERT, FORMGB, PTNO, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, MEDFRDATE, MEDFRTIME,  ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, MEDENDTIME, MEDDEPTCD,  ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, OPDATE, OPDEGREE,  ";
                SQL = SQL + ComNum.VBLF + "    OP_DEPT, DEPTCDNOW, DRCDNOW,  ";
                SQL = SQL + ComNum.VBLF + "    ROOM_NO, ACPNOOUT, CURDEPT, CURGRADE, ";
                SQL = SQL + ComNum.VBLF + "    CODEUSEID, CODEDATE, CODETIME) ";
                SQL = SQL + ComNum.VBLF + " VALUES (";
                SQL = SQL + ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                SQL = SQL + ComNum.VBLF + po.acpNo + ",";   //ACPNO  po.acpNo
                SQL = SQL + ComNum.VBLF + strFormNo + ","; //FORMNO
                SQL = SQL + ComNum.VBLF + strUpdateNo + ",";   //UPDATENO
                SQL = SQL + ComNum.VBLF + "'" + strChartDate + "',";   //CHARTDATE
                SQL = SQL + ComNum.VBLF + "'" + strChartTime.Replace(" ", "") + "',";   //CHARTTIME
                SQL = SQL + ComNum.VBLF + "'" + strCHARTUSEID + "',";   //CHARTUSEID
                SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";   //WRITEDATE
                SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";   //WRITETIME
                if (strSAVEGB == "1")
                {
                    SQL = SQL + ComNum.VBLF + "'" + strCOMPUSEID + "',";   //COMPUSEID
                    SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";   //COMPDATE
                    SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";   //COMPTIME
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "'',";   //COMPUSEID
                    SQL = SQL + ComNum.VBLF + "'',";   //COMPDATE
                    SQL = SQL + ComNum.VBLF + "'',";   //COMPTIME
                }
                //if (VB.Left(strCHARTUSEID, 1) == "A") //교수 ID일 경우는 확인 사인을 박는다.
                //{
                //    SQL = SQL + ComNum.VBLF + "'" + strCHARTUSEID + "',";   //COMPUSEID
                //    SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";   //COMPDATE
                //    SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";   //COMPTIME
                //}
                //else
                //{
                //    SQL = SQL + ComNum.VBLF + "'',";   //COMPUSEID
                //    SQL = SQL + ComNum.VBLF + "'',";   //COMPDATE
                //    SQL = SQL + ComNum.VBLF + "'',";   //COMPTIME
                //}

                SQL = SQL + ComNum.VBLF + "'',";   //PRNTYN
                SQL = SQL + ComNum.VBLF + "'" + strSAVEGB + "',";   //SAVEGB
                SQL = SQL + ComNum.VBLF + "'" + strSAVECERT + "',";   //SAVECERT
                SQL = SQL + ComNum.VBLF + "'" + strFORMGB + "',";   //FORMGB
                SQL = SQL + ComNum.VBLF + "'" + po.ptNo + "',";   //PTNO    po.ptNo
                SQL = SQL + ComNum.VBLF + "'" + po.inOutCls + "',";   //INOUTCLS   
                SQL = SQL + ComNum.VBLF + "'" + po.medFrDate + "',";   //MEDFRDATE   
                SQL = SQL + ComNum.VBLF + "'" + po.medFrTime + "',";   //MEDFRTIME   
                SQL = SQL + ComNum.VBLF + "'" + po.medEndDate + "',";   //MEDENDDATE   
                SQL = SQL + ComNum.VBLF + "'" + po.medEndTime + "',";   //MEDENDTIME   
                SQL = SQL + ComNum.VBLF + "'" + po.medDeptCd + "',";   //MEDDEPTCD   
                SQL = SQL + ComNum.VBLF + "'" + po.medDrCd + "',";   //MEDDRCD 
                if (po.opdate != "")
                {
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + po.opdate + "','YYYY-MM-DD') ,";   //OPDATE    po.OPDATE
                    SQL = SQL + ComNum.VBLF + "'" + po.opdegree + "',";   //OPDEGREE    
                    SQL = SQL + ComNum.VBLF + "'" + po.opdept + "' , ";   //OP_DEPT   
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "'' ,";   //OPDATE    po.OPDATE
                    SQL = SQL + ComNum.VBLF + "'',";   //OPDEGREE    
                    SQL = SQL + ComNum.VBLF + "'' , ";   //OP_DEPT   
                }

                SQL = SQL + ComNum.VBLF + "'" + po.nowdeptcd + "',";   //DEPTCDNOW    
                SQL = SQL + ComNum.VBLF + "'" + po.nowdrcd + "',";   //DRCDNOW    
                SQL = SQL + ComNum.VBLF + "'" + po.nowroomno + "', ";   //ROOM_NO  
                SQL = SQL + ComNum.VBLF + " 0, ";   //ACPNOOUT  


                //2017-03-31 작성당시 과 및 그레이트 의사 전공의 간호사 간호조무사 등 
                if (po.cur_Dept != "" && po.cur_Dept != null)
                {
                    SQL = SQL + ComNum.VBLF + " '" + po.cur_Dept + "'  , ";   //인턴은 입력시 들어오게  작성당시 과 .부서 병동 등    
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " '" + clsType.User.DeptCode + "',   ";   //작성당시 과 .부서 병동 등    
                }
                SQL = SQL + ComNum.VBLF + "     '" + "" + "'   ";   //clsType.User.strJobRght GRADE 의사 간호사 전공의  등     

                if(strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + ",'" + clsType.User.IdNumber + "'";   //CODEUSEID
                    SQL = SQL + ComNum.VBLF + ",'" + strCurDate + "'";   //CODEDATE
                    SQL = SQL + ComNum.VBLF + ",'" + strCurTime + "'";   //CODETIME
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + ",'', '', ''";
                }
                
                SQL = SQL + ComNum.VBLF + " )  ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                string strMiBiCd = string.Empty;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "EMRMIBI";
                SQL = SQL + ComNum.VBLF + "SET";
                SQL = SQL + ComNum.VBLF + " WRITEDATE = '" + strCurDate + "',";
                SQL = SQL + ComNum.VBLF + " WRITETIME = '" + strCurTime + "'";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + po.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + po.medFrDate + "' ";
                if (strSaveFlag == "SAVE")
                {
                    SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + VB.Val(strCHARTUSEID) + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND MEDDRCD = '" + clsType.User.IdNumber + "' ";
                }

                SQL = SQL + ComNum.VBLF + "  AND MIBICLS = 1";
                SQL = SQL + ComNum.VBLF + "  AND MIBIGRP = '" + clsEmrChart.GetEmrGrp(clsDB.DbCon, strFormNo, ref strMiBiCd) + "'";
                SQL = SQL + ComNum.VBLF + "  AND MIBICD <> 'A13'"; //입퇴원요약지 삭제 아이템
                SQL = SQL + ComNum.VBLF + "  AND MIBICD <> 'C10'"; //입원기록지 삭제  아이템

                if (strMiBiCd.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND MIBICD IN(" + strMiBiCd + ")";
                }

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                #region 입퇴원요약지 검수 관련 데이터 정리
                if (strFormNo.Equals("1647"))
                {
                    SQL = "INSERT INTO KOSMOS_EMR.EMRXML_COMPLETE_HISTORY(";
                    SQL += ComNum.VBLF + " EMRNO, EMRNOHIS, CDATE, CSABUN, DELDATE, DELSABUN, MEDFRDATE, PTNO, INDATE) ";
                    SQL += ComNum.VBLF + " SELECT EMRNO, EMRNOHIS, CDATE, CSABUN, SYSDATE, " + clsType.User.IdNumber + ", MEDFRDATE, PTNO, INDATE";
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_COMPLETE ";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNoNew;

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr.Length > 0)
                    {
                        //clsDB.setRollbackTran(pDbCon);
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return rtnVal;
                    }

                    SQL = " DELETE KOSMOS_EMR.EMRXML_COMPLETE ";
                    SQL += ComNum.VBLF + "WHERE EMRNO = " + dblEmrNoNew;

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr.Length > 0)
                    {
                        //clsDB.setRollbackTran(pDbCon);
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return rtnVal;
                    }
                }
                #endregion

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// ChartImage에만 저장.
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="dblEmrNoOld"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strITEMVALUE"></param>
        /// <returns></returns>
        public static bool SaveChartImageOnly(PsmhDb pDbCon, string strFormNo, string strUpdateNo, double dblEmrNoNew, double dblEmrNoOld,
                        string strITEMCD, string strITEMVALUE)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strIMGSVR = "";
            string strIMGPATH1 = "";
            string strIMGPATH2 = "";
            string strIMGPATH = "";

            try
            {
                string strCurDate = ComQuery.CurrentDateTime(pDbCon, "D"); //2015\05\123.jpg
                strIMGPATH1 = VB.Left(strCurDate, 4);
                strIMGPATH2 = VB.Mid(strCurDate, 5, 2);
                strIMGPATH = strIMGPATH1 + "/" + strIMGPATH2;

                DataTable dt = null;
                dt = clsEmrQuery.GetBBasCd(pDbCon, "기록지관리", "이미지PATH", strCurDate, "", "");

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                strIMGSVR = dt.Rows[0]["BASCD"].ToString().Trim();
                string ServerAddress = dt.Rows[0]["BASNAME"].ToString().Trim();
                string strServerPath = dt.Rows[0]["BASEXNAME"].ToString().Trim();
                //string ServerPort = "21";
                string UserName = dt.Rows[0]["REMARK1"].ToString().Trim();
                string Password = dt.Rows[0]["REMARK2"].ToString().Trim();
                string HomePath = dt.Rows[0]["VFLAG1"].ToString().Trim();
                dt.Dispose();
                dt = null;

                //저장
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTIMAGE ";
                SQL = SQL + ComNum.VBLF + "    (EMRNO       ,ITEMCD       ,IMAGENO      ,IMGSVR    ,IMGPATH   , IMGEXTENSION )";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + ComNum.VBLF + "'" + strITEMCD + "',";   //ITEMCD
                SQL = SQL + ComNum.VBLF + "'" + strITEMVALUE + "',"; //IMAGENO
                SQL = SQL + ComNum.VBLF + "'" + strIMGSVR + "',";   //IMGSVR
                SQL = SQL + ComNum.VBLF + "'" + strIMGPATH + "',";   //IMGPATH
                SQL = SQL + ComNum.VBLF + "'" + "jpg" + "'";   //IMGEXTENSION
                SQL = SQL + ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                string strFoldJob = "";
                string strFoldBase = "";

                clsEmrFunc.CheckImageJobFold(ref strFoldJob, ref strFoldBase, strFormNo, strUpdateNo, dblEmrNoOld.ToString(), strITEMCD);

                //파일이 있는지 확인하고
                if (File.Exists(strFoldJob + "\\" + strITEMVALUE + ".jpg") == false)
                {
                    return rtnVal;
                }

                ////FTP저장
                //ftpConnection1.ServerAddress = ServerAddress;
                //ftpConnection1.ServerPort = (int)VB.Val(ServerPort);
                //ftpConnection1.UserName = UserName;
                //ftpConnection1.Password = Password;
                //ftpConnection1.Connect();
                //string directoryX = ftpConnection1.ServerDirectory;
                //string directory = strServerPath;
                //ftpConnection1.ChangeWorkingDirectory(directory);
                ////폴더를 만들고
                //if (ftpConnection1.DirectoryExists(strIMGPATH1) == false) ftpConnection1.CreateDirectory(strIMGPATH1);
                //ftpConnection1.ChangeWorkingDirectory(strIMGPATH1);

                //if (ftpConnection1.DirectoryExists(strIMGPATH2) == false) ftpConnection1.CreateDirectory(strIMGPATH2);
                //ftpConnection1.ChangeWorkingDirectory(strIMGPATH2);
                ////파일 업로드
                //ftpConnection1.UploadFile(strFoldJob + "\\" + strITEMVALUE + ".jpg", strITEMVALUE + ".jpg");
                //if (File.Exists(strFoldJob + "\\" + strITEMVALUE + ".dgr") == true)
                //{
                //    ftpConnection1.UploadFile(strFoldJob + "\\" + strITEMVALUE + ".dgr", strITEMVALUE + ".dgr");
                //}

                //clsWinScp.ConWinScp("Ftp", ServerAddress, UserName, Password, HomePath, "");
                //if (clsWinScp.WinScpDirExit(strServerPath + "/" + strIMGPATH) == false)
                //{
                //    clsWinScp.gWinScp.CreateDirectory(strServerPath + "/" + strIMGPATH);
                //}
                //clsWinScp.gTrsResult = clsWinScp.gWinScp.PutFiles(strFoldJob + "\\" + strITEMVALUE + ".jpg", strServerPath + "/" + strIMGPATH + "/" + strITEMVALUE + ".jpg", false, clsWinScp.gTrsOptions);
                //if (File.Exists(strFoldJob + "\\" + strITEMVALUE + ".dgr") == true)
                //{
                //    clsWinScp.gTrsResult = clsWinScp.gWinScp.PutFiles(strFoldJob + "\\" + strITEMVALUE + ".dgr", strServerPath + "/" + strIMGPATH + "/" + strITEMVALUE + ".dgr", false, clsWinScp.gTrsOptions);
                //}
               

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// AEMRCHARTROW에 data를 저장한다.
        /// </summary>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <returns></returns>
        public static bool SaveDataAEMRCHARTROW(PsmhDb pDbCon, Control ChartForm, bool isSpcPanel, Control pControl, 
                double dblEmrNoNew, double dblEmrHisNo)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";

            string strITEMCD     = string.Empty;
            string strITEMNO     = string.Empty;
            string strITEMINDEX  = string.Empty;
            string strITEMTYPE   = string.Empty;
            string strITEMVALUE  = string.Empty;
            string strITEMVALUE1 = string.Empty;
            string strITEMVALUE2 = string.Empty;
            string strDSPSEQ = "0";

            bool bolIsItem = false;

            string[] arryEMRNO = null;
            string[] arryEMRNOHIS = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryITEMVALUE2 = null;
            string[] arryDSPSEQ = null;

            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        ComFunc.MsgBox("선택된 컨테이너가 존재하지 않습니다.");
                        return rtnVal;
                    }
                }

                Control[] controls = null;

                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(ChartForm);
                }

                foreach (Control objControl in controls)
                {
                    if (ComFunc.IsVisible(objControl, isSpcPanel, pControl) == true) //숨긴패널 안에 컨트롤은 저장하지 않는다.
                    {
                        if (objControl.Name == "IG00249" || objControl.Name == "I0000031497_0" || objControl.Name == "I0000031497_1"
                            || objControl.Name == "I0000031498_0" || objControl.Name == "I0000031498_1")
                        {
                            continue; //싸인 패널은 제외한다
                        }
                        strITEMCD = "";
                        strITEMNO = "";
                        strITEMINDEX = "-1";
                        strITEMTYPE = "";
                        strITEMVALUE = "";
                        strITEMVALUE1 = "";
                        strITEMVALUE2 = "";
                        strDSPSEQ = "0";

                        bolIsItem = false;

                        strITEMINDEX = clsXML.IsArryCon(objControl);
                        if (strITEMINDEX == "") strITEMINDEX = "-1";

                        strITEMCD = objControl.Name;
                        strITEMNO = strITEMCD;

                        if ((objControl is FarPoint.Win.Spread.FpSpread) == false)
                        {
                            if (VB.InStr(strITEMCD, "_") > 0)
                            {
                                string[] strParams = VB.Split(strITEMCD.Trim(), "_", -1);
                                strITEMNO = strParams[0];
                            }
                        }

                        if (strITEMNO != "txtMedFrDate" && strITEMNO != "txtMedFrTime" && strITEMNO != "")
                        {
                            //DateTimePicker(DTPicker)
                            if (objControl is DateTimePicker)
                            {
                                strITEMTYPE = "DATE";
                                bolIsItem = true;
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd");
                                }
                            }
                            //TextBox
                            if (objControl is TextBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "TEXT";

                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        string strITEMVALUETOT = objControl.Text.Trim().Replace("'", "`");
                                        int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                                        int intLen = 3999;
                                        if (intLenTot > 3999)
                                        {
                                            string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                                            if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                                            {
                                                intLen = 3998;
                                            }
                                            strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                                            if (intLenTot > 8000)
                                            {
                                                strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, 3999);
                                                if (VB.Right(strITEMVALUE1, 1) == "\r" || VB.Right(strITEMVALUE1, 1) == "?")
                                                {
                                                    strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUE1, 0, 3998);
                                                    intLen += 3998;
                                                }

                                                strITEMVALUE2 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - (intLen));
                                                if (VB.Right(strITEMVALUE2, 1) == "\r" || VB.Right(strITEMVALUE2, 1) == "?")
                                                {
                                                    strITEMVALUE2 = ComFunc.GetMidStr(strITEMVALUE2, 0, 3998);
                                                }
                                            }
                                            else
                                            {
                                                strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                                            }
                                        }
                                        else
                                        {
                                            strITEMVALUE = strITEMVALUETOT;
                                        }
                                    }
                                }
                                else
                                {
                                    string strITEMVALUETOT = objControl.Text.Trim().Replace("'", "`");
                                    int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                                    int intLen = 3999;
                                    if (intLenTot > 3999)
                                    {
                                        string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                                        if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                                        {
                                            intLen = 3998;
                                        }
                                        strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                                        if (intLenTot > 8000)
                                        {
                                            strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, 3999);
                                            if (VB.Right(strITEMVALUE1, 1) == "\r" || VB.Right(strITEMVALUE1, 1) == "?")
                                            {
                                                strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUE1, 0, 3998);
                                                intLen += 3998;
                                            }
                                            else
                                            {
                                                intLen += 3999;
                                            }

                                            strITEMVALUE2 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - (intLen));
                                            if (VB.Right(strITEMVALUE2, 1) == "\r" || VB.Right(strITEMVALUE2, 1) == "?")
                                            {
                                                strITEMVALUE2 = ComFunc.GetMidStr(strITEMVALUE2, 0, 3998);
                                            }
                                        }
                                        else
                                        {
                                            strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                                        }
                                    }
                                    else
                                    {
                                        strITEMVALUE = strITEMVALUETOT;
                                    }
                                }
                            }
                            //ComboBox
                            if (objControl is ComboBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "COMBO";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = objControl.Text.Trim().Replace("'", "`");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = objControl.Text.Trim().Replace("'", "`");
                                }
                            }
                            //CheckBox
                            if (objControl is CheckBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "CHECK";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = (((CheckBox)objControl).Checked == true ? "1" : "0");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = (((CheckBox)objControl).Checked == true ? "1" : "0");
                                }
                            }
                            //RadioButton(OptionButton)
                            if (objControl is RadioButton)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "RADIO";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = (((RadioButton)objControl).Checked == true ? "1" : "0");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = (((RadioButton)objControl).Checked == true ? "1" : "0");
                                }
                            }

                            if (objControl is PictureBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "IMAGE";
                                strITEMVALUE = "";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        if (((PictureBox)objControl).Tag != null)
                                        {
                                            strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (((PictureBox)objControl).Tag != null)
                                    {
                                        strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                    }
                                }
                            }

                            //fpSpread
                            //if (objControl is FarPoint.Win.Spread.FpSpread)
                            //{
                            //    strITEMINDEX = "";
                            //    if (strITEMCD != "ssMacroWord")
                            //    {
                            //        if (isSpcPanel == true)
                            //        {
                            //            if (IsParentRow(objControl, pControl) == true)
                            //            {
                            //                FarPoint.Win.Spread.FpSpread ssSpd = null;
                            //                ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                            //                strXML = strXML + "<" + strITEMCD + " Type=\"fpSpreadSheet\"" + " Index=\"" + strITEMINDEX + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strITEMCD + ">";
                            //                for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                            //                {
                            //                    for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                            //                    {
                            //                        strXML = strXML + SaveSpreadData(ssSpd, i, j);
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            FarPoint.Win.Spread.FpSpread ssSpd = null;
                            //            ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                            //            strXML = strXML + "<" + strITEMCD + " Type=\"fpSpreadSheet\"" + " Index=\"" + strITEMINDEX + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strITEMCD + ">";
                            //            for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                            //            {
                            //                for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                            //                {
                            //                    strXML = strXML + SaveSpreadData(ssSpd, i, j);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }

                        if (bolIsItem == true)
                        {

                            if (arryEMRNO == null)
                            {
                                arryEMRNO = new string[0];
                                arryEMRNOHIS = new string[0];
                                arryITEMCD = new string[0];
                                arryITEMNO = new string[0];
                                arryITEMINDEX = new string[0];
                                arryITEMTYPE = new string[0];
                                arryITEMVALUE = new string[0];
                                arryITEMVALUE1 = new string[0];
                                arryITEMVALUE2 = new string[0];
                                arryDSPSEQ = new string[0];
                            }

                            Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                            Array.Resize<string>(ref arryEMRNOHIS, arryEMRNOHIS.Length + 1);
                            Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                            Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                            Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                            Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                            Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                            Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                            Array.Resize<string>(ref arryITEMVALUE2, arryITEMVALUE2.Length + 1);
                            Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                            arryEMRNO[arryEMRNO.Length - 1] = dblEmrNoNew.ToString();
                            arryEMRNOHIS[arryEMRNOHIS.Length - 1] = dblEmrHisNo.ToString();
                            arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                            arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                            arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                            arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                            arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                            arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                            arryITEMVALUE2[arryEMRNO.Length - 1] = strITEMVALUE2;
                            arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;

                        }
                    }
                }

                if (arryEMRNO == null) return rtnVal;

                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME, ITEMVALUE2 )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL = SQL + "\r\n" + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL = SQL + "\r\n" + " :ITEMCD, ";   //ITEMCD
                SQL = SQL + "\r\n" + " :ITEMNO, "; //ITEMNO
                SQL = SQL + "\r\n" + " :ITEMINDEX, "; //ITEMINDEX
                SQL = SQL + "\r\n" + " :ITEMTYPE, ";   //ITEMTYPE
                SQL = SQL + "\r\n" + " :ITEMVALUE, ";   //ITEMVALUE
                SQL = SQL + "\r\n" + " :DSPSEQ, ";   //DSPSEQ
                SQL = SQL + "\r\n" + " :ITEMVALUE1, ";   //ITEMVALUE1
                SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS'), ";   //INPTIME
                SQL = SQL + "\r\n" + " :ITEMVALUE2 ";   //ITEMVALUE2
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteChartRow(pDbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1, arryITEMVALUE2);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public double SaveAEMRUSERCHARTFOR()
        {
            return 0;
        }

        public static double SaveAEMRUSERCHARTFORMROW(PsmhDb pDbCon, Control ChartForm, bool isSpcPanel, Control pControl, double dblEmrNoNew)
        {
            return 0;
        }


        /// <summary>
        /// 기록지별 사용자 상용 템플릿 문구 저장 ROW
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="dblMACRONO"></param>
        /// <param name="TRS"></param>
        /// <returns></returns>
        public static bool SaveDataAEMRUSERCHARTFORMROW(PsmhDb pDbCon, Control ChartForm, bool isSpcPanel, Control pControl, double dblMACRONO)
        {
            bool rtnVal = false;
            string SQL = "";

            string strITEMCD = "";
            string strITEMNO = "";
            string strITEMINDEX = "";
            string strITEMTYPE = "";
            string strITEMVALUE = "";
            string strITEMVALUE1 = "";
            string strDSPSEQ = "0";

            bool bolIsItem = false;

            string[] arryMACRONO = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryDSPSEQ = null;

            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        ComFunc.MsgBox("선택된 컨테이너가 존재하지 않습니다.");
                        return rtnVal;
                    }
                }

                Control[] controls = null;

                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(ChartForm);
                }

                foreach (Control objControl in controls)
                {
                    if (ComFunc.IsVisible(objControl, isSpcPanel, pControl) == true) //숨긴패널 안에 컨트롤은 저장하지 않는다.
                    {
                        //if (objControl.Name == "IG00249" || objControl.Name == "I0000031497_0" || objControl.Name == "I0000031497_1"
                        //    || objControl.Name == "I0000031498_0" || objControl.Name == "I0000031498_1")
                        //{
                        //    continue; //싸인 패널은 제외한다
                        //}
                        strITEMCD = "";
                        strITEMNO = "";
                        strITEMINDEX = "-1";
                        strITEMTYPE = "";
                        strITEMVALUE = "";
                        strITEMVALUE1 = "";
                        strDSPSEQ = "0";

                        bolIsItem = false;

                        strITEMINDEX = clsXML.IsArryCon(objControl);
                        if (strITEMINDEX == "") strITEMINDEX = "-1";

                        strITEMCD = objControl.Name;
                        strITEMNO = strITEMCD;

                        if ((objControl is FarPoint.Win.Spread.FpSpread) == false)
                        {
                            if (VB.InStr(strITEMCD, "_") > 0)
                            {
                                string[] strParams = VB.Split(strITEMCD.Trim(), "_", -1);
                                strITEMNO = strParams[0];
                            }
                        }

                        if (strITEMNO != "txtMedFrDate" && strITEMNO != "txtMedFrTime" && strITEMNO != "")
                        {
                            //DateTimePicker(DTPicker)
                            if (objControl is DateTimePicker)
                            {
                                strITEMTYPE = "DATE";
                                bolIsItem = true;
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = VB.Format(((DateTimePicker)objControl).Value, "yyyy-MM-dd");
                                }
                            }
                            //TextBox
                            if (objControl is TextBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "TEXT";

                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        string strITEMVALUETOT = objControl.Text.Trim().Replace("'", "`");
                                        int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                                        int intLen = 3999;
                                        if (intLenTot > 3999)
                                        {
                                            string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                                            if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                                            {
                                                intLen = 3998;
                                            }
                                            strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                                            strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                                        }
                                        else
                                        {
                                            strITEMVALUE = strITEMVALUETOT;
                                        }
                                    }
                                }
                                else
                                {
                                    string strITEMVALUETOT = objControl.Text.Trim().Replace("'", "`");
                                    int intLenTot = (int)ComFunc.GetWordByByte(strITEMVALUETOT);
                                    int intLen = 3999;
                                    if (intLenTot > 3999)
                                    {
                                        string strTmp0 = ComFunc.GetMidStr(strITEMVALUETOT, 0, 3999);
                                        if (VB.Right(strTmp0, 1) == "\r" || VB.Right(strTmp0, 1) == "?")
                                        {
                                            intLen = 3998;
                                        }
                                        strITEMVALUE = ComFunc.GetMidStr(strITEMVALUETOT, 0, intLen);
                                        strITEMVALUE1 = ComFunc.GetMidStr(strITEMVALUETOT, intLen, intLenTot - intLen);
                                    }
                                    else
                                    {
                                        strITEMVALUE = strITEMVALUETOT;
                                    }
                                }
                            }
                            //ComboBox
                            if (objControl is ComboBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "COMBO";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = objControl.Text.Trim().Replace("'", "`");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = objControl.Text.Trim().Replace("'", "`");
                                }
                            }
                            //CheckBox
                            if (objControl is CheckBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "CHECK";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = (((CheckBox)objControl).Checked == true ? "1" : "0");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = (((CheckBox)objControl).Checked == true ? "1" : "0");
                                }
                            }
                            //RadioButton(OptionButton)
                            if (objControl is RadioButton)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "RADIO";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        strITEMVALUE = (((RadioButton)objControl).Checked == true ? "1" : "0");
                                    }
                                }
                                else
                                {
                                    strITEMVALUE = (((RadioButton)objControl).Checked == true ? "1" : "0");
                                }
                            }

                            if (objControl is PictureBox)
                            {
                                bolIsItem = true;
                                strITEMTYPE = "IMAGE";
                                strITEMVALUE = "";
                                if (isSpcPanel == true)
                                {
                                    if (clsXML.IsParent(objControl, pControl) == true)
                                    {
                                        if (((PictureBox)objControl).Tag != null)
                                        {
                                            strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (((PictureBox)objControl).Tag != null)
                                    {
                                        strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                    }
                                }
                            }

                            //fpSpread
                            //if (objControl is FarPoint.Win.Spread.FpSpread)
                            //{
                            //    strITEMINDEX = "";
                            //    if (strITEMCD != "ssMacroWord")
                            //    {
                            //        if (isSpcPanel == true)
                            //        {
                            //            if (IsParentRow(objControl, pControl) == true)
                            //            {
                            //                FarPoint.Win.Spread.FpSpread ssSpd = null;
                            //                ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                            //                strXML = strXML + "<" + strITEMCD + " Type=\"fpSpreadSheet\"" + " Index=\"" + strITEMINDEX + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strITEMCD + ">";
                            //                for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                            //                {
                            //                    for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                            //                    {
                            //                        strXML = strXML + SaveSpreadData(ssSpd, i, j);
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            FarPoint.Win.Spread.FpSpread ssSpd = null;
                            //            ssSpd = (FarPoint.Win.Spread.FpSpread)objControl;

                            //            strXML = strXML + "<" + strITEMCD + " Type=\"fpSpreadSheet\"" + " Index=\"" + strITEMINDEX + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strITEMCD + ">";
                            //            for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
                            //            {
                            //                for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                            //                {
                            //                    strXML = strXML + SaveSpreadData(ssSpd, i, j);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }

                        if (bolIsItem == true)
                        {

                            if (arryMACRONO == null)
                            {
                                arryMACRONO = new string[0];
                                arryITEMCD = new string[0];
                                arryITEMNO = new string[0];
                                arryITEMINDEX = new string[0];
                                arryITEMTYPE = new string[0];
                                arryITEMVALUE = new string[0];
                                arryITEMVALUE1 = new string[0];
                                arryDSPSEQ = new string[0];
                            }

                            Array.Resize<string>(ref arryMACRONO, arryMACRONO.Length + 1);
                            Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                            Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                            Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                            Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                            Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                            Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                            Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                            arryMACRONO[arryMACRONO.Length - 1] = dblMACRONO.ToString();
                            arryITEMCD[arryMACRONO.Length - 1] = strITEMCD;
                            arryITEMNO[arryMACRONO.Length - 1] = strITEMNO;
                            arryITEMINDEX[arryMACRONO.Length - 1] = strITEMINDEX;
                            arryITEMTYPE[arryMACRONO.Length - 1] = strITEMTYPE;
                            arryITEMVALUE[arryMACRONO.Length - 1] = strITEMVALUE;
                            arryITEMVALUE1[arryMACRONO.Length - 1] = strITEMVALUE1;
                            arryDSPSEQ[arryMACRONO.Length - 1] = strDSPSEQ;
                            
                        }
                    }
                }

                if (arryMACRONO == null) return rtnVal;

                OracleCommand cmd = null;
                
                cmd = pDbCon.Con.CreateCommand();

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRUSERCHARTROW ";
                SQL = SQL + ComNum.VBLF + "    (MACRONO       ,ITEMCD       ,ITEMNO      ,ITEMINDEX    ,ITEMTYPE   , ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + dblMACRONO + ",";    //MACRONO
                SQL = SQL + ComNum.VBLF + " :ITEMCD,";   //ITEMCD
                SQL = SQL + ComNum.VBLF + " :ITEMNO,"; //ITEMNO
                SQL = SQL + ComNum.VBLF + " :ITEMINDEX,"; //ITEMINDEX
                SQL = SQL + ComNum.VBLF + " :ITEMTYPE,";   //ITEMTYPE
                SQL = SQL + ComNum.VBLF + " :ITEMVALUE,";   //ITEMVALUE
                SQL = SQL + ComNum.VBLF + " :DSPSEQ,";   //DSPSEQ
                SQL = SQL + ComNum.VBLF + " :ITEMVALUE1";   //ITEMVALUE
                SQL = SQL + ComNum.VBLF + ")"; 


                cmd.CommandText = SQL;
                cmd.CommandTimeout = 60;

                if (pDbCon.Trs != null)
                {
                    cmd.Transaction = pDbCon.Trs;
                }

                cmd.Parameters.Add(":ITEMCD", OracleDbType.Varchar2, 15);
                cmd.Parameters.Add(":ITEMNO", OracleDbType.Varchar2, 11);
                cmd.Parameters.Add(":ITEMINDEX", OracleDbType.Int64);
                cmd.Parameters.Add(":ITEMTYPE", OracleDbType.Varchar2, 10);
                cmd.Parameters.Add(":ITEMVALUE", OracleDbType.Varchar2, 4000);
                cmd.Parameters.Add(":DSPSEQ", OracleDbType.Int32);
                cmd.Parameters.Add(":ITEMVALUE1", OracleDbType.Varchar2, 4000);
                cmd.Prepare();

                for (int i = 0; i < arryITEMCD.Length; i++)
                {
                    cmd.Parameters[":ITEMCD"].Value = arryITEMCD[i];
                    cmd.Parameters[":ITEMNO"].Value = arryITEMNO[i];
                    cmd.Parameters[":ITEMINDEX"].Value = arryITEMINDEX[i];
                    cmd.Parameters[":ITEMTYPE"].Value = arryITEMTYPE[i];
                    cmd.Parameters[":ITEMVALUE"].Value = arryITEMVALUE[i];
                    cmd.Parameters[":DSPSEQ"].Value = arryDSPSEQ[i];
                    cmd.Parameters[":ITEMVALUE1"].Value = arryITEMVALUE1[i];
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
                cmd = null;

                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 요약된 서식을 저장한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="TRS"></param>
        /// <returns></returns>
        static bool SaveDataAEMRCHARTFORM(PsmhDb pDbCon, Control ChartForm, bool isSpcPanel, Control pControl, double dblEmrNoNew)
        {
            bool rtnVal = false;
            string SQL = "";

            string[] arryEMRNO = null;
            string[] arryCONTROLNAME = null;
            string[] arryCONTROTYPE = null;
            string[] arryCONTROLPARENT = null;
            string[] arryLOCATIONX = null;
            string[] arryLOCATIONY = null;
            string[] arrySIZEWIDTH = null;
            string[] arrySIZEHEIGHT = null;
            string[] arryTAG = null;
            string[] arryFRONTBACK = null;
            string[] arryCHILDINDEX = null;
            string[] arryBACKCOLOR = null;
            string[] arryFORECOLOR = null;
            string[] arryBOARDSTYLE = null;
            string[] arryDOCK = null;
            string[] arryENABLED = null;
            string[] arryVISIBLED = null;
            string[] arryTEXT = null;
            string[] arryFONTS = null;
            string[] arryMULTILINE = null;
            string[] arrySCROLLBARS = null;
            string[] arryTEXTALIGN = null;
            string[] arryIMAGE = null;
            string[] arryIMAGESIZEMODE = null;
            string[] arryCHECKED = null;

            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        ComFunc.MsgBox("선택된 컨테이너가 존재하지 않습니다.");
                        return rtnVal;
                    }
                }

                Control[] controls = null;

                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(ChartForm);
                }

                foreach (Control p in controls)
                {
                    string strCONTROLNAME = p.Name;
                    string strCONTROTYPE = p.GetType().ToString();
                    string strCONTROLPARENT = p.Parent.Name;

                    string strLOCATIONX = p.Location.X.ToString();
                    string strLOCATIONY = p.Location.Y.ToString();
                    string strSIZEWIDTH = p.Width.ToString();
                    string strSIZEHEIGHT = p.Height.ToString();
                    string strTAG = "";
                    string strCHILDINDEX = "";
                    string strBACKCOLOR = p.BackColor.Name;
                    string strFORECOLOR = p.ForeColor.Name;
                    string strBOARDSTYLE = "";
                    string strDOCK = p.Dock.ToString();
                    string strENABLED = p.Enabled.ToString();
                    string strVISIBLED = p.Visible.ToString();
                    string strTEXT = "";
                    string strFONTS = "";
                    string strMULTILINE = "";
                    string strSCROLLBARS = "";
                    string strTEXTALIGN = "";
                    string strIMAGE = "";
                    string strIMAGESIZEMODE = "";
                    string strCHECKED = "";

                    if (p is FarPoint.Win.Spread.FpSpread)
                    {
                        continue;
                    }
                    if (p.Name == "usFormTopMenu")
                    {
                        continue;
                    }
                    if (p.Parent.Name == "usFormTopMenu")
                    {
                        continue;
                    }
                    if (p.Name == "panTopMenu")
                    {
                        continue;
                    }
                    if (p.Name == "panChart")
                    {
                        continue;
                    }
                    if ((p is mtsPanel15.mPanel) || (p is Panel) || (p is GroupBox) || (p is PictureBox) || (p is TextBox) || (p is ComboBox) || (p is CheckBox) || (p is RadioButton) || (p is Label))
                    {
                        if ((p is mtsPanel15.mPanel) || (p is Panel) || (p is GroupBox) || (p is TextBox))
                        {
                            if (p is mtsPanel15.mPanel) strBOARDSTYLE = ((mtsPanel15.mPanel)p).BorderStyle.ToString();
                            if (p is Panel) strBOARDSTYLE = ((Panel)p).BorderStyle.ToString();
                            if (p is TextBox) strBOARDSTYLE = ((TextBox)p).BorderStyle.ToString();
                        }
                        if ((p is PictureBox) || (p is TextBox) || (p is ComboBox) || (p is CheckBox) || (p is RadioButton))
                        {
                            if (p is PictureBox) strTAG = (((PictureBox)p).Tag != null ? ((PictureBox)p).Tag.ToString() : "");
                            if (p is TextBox) strTAG = (((TextBox)p).Tag != null ? ((TextBox)p).Tag.ToString() : "");
                            if (p is ComboBox) strTAG = (((ComboBox)p).Tag != null ? ((ComboBox)p).Tag.ToString() : "");
                            if (p is CheckBox) strTAG = (((CheckBox)p).Tag != null ? ((CheckBox)p).Tag.ToString() : "");
                            if (p is RadioButton) strTAG = (((RadioButton)p).Tag != null ? ((RadioButton)p).Tag.ToString() : "");
                        }

                        if (p.Parent != null)
                        {
                            if (p.Parent is mtsPanel15.mPanel) strCHILDINDEX = ((mtsPanel15.mPanel)p.Parent).Controls.GetChildIndex(p).ToString();
                            if (p.Parent is Panel) strCHILDINDEX = ((Panel)p.Parent).Controls.GetChildIndex(p).ToString();
                            if (p.Parent is GroupBox) strCHILDINDEX = ((GroupBox)p.Parent).Controls.GetChildIndex(p).ToString();
                        }

                        if (p is PictureBox) strBOARDSTYLE = ((PictureBox)p).BorderStyle.ToString();
                        if (p is TextBox) strBOARDSTYLE = ((TextBox)p).BorderStyle.ToString();
                        if (p is Label) strBOARDSTYLE = ((Label)p).BorderStyle.ToString();

                        if (p is PictureBox)
                        {
                            strIMAGE = "";
                            strIMAGESIZEMODE = ((PictureBox)p).SizeMode.ToString();
                        }
                        else
                        {
                            strTEXT = p.Text.ToString();
                            strFONTS = FontToString(p.Font);
                        }

                        if (p is TextBox)
                        {
                            strMULTILINE = ((TextBox)p).Multiline.ToString();
                            strSCROLLBARS = ((TextBox)p).Multiline.ToString();
                            strTEXTALIGN = ((TextBox)p).TextAlign.ToString();
                        }

                        if (p is CheckBox)
                        {
                            strCHECKED = ((CheckBox)p).Checked.ToString();
                        }
                        if (p is RadioButton)
                        {
                            strCHECKED = ((RadioButton)p).Checked.ToString();
                        }

                        if (arryEMRNO == null)
                        {
                            arryEMRNO = new string[0];
                            arryCONTROLNAME = new string[0];
                            arryCONTROTYPE = new string[0];
                            arryCONTROLPARENT = new string[0];
                            arryLOCATIONX = new string[0];
                            arryLOCATIONY = new string[0];
                            arrySIZEWIDTH = new string[0];
                            arrySIZEHEIGHT = new string[0];
                            arryTAG = new string[0];
                            arryFRONTBACK = new string[0];
                            arryCHILDINDEX = new string[0];
                            arryBACKCOLOR = new string[0];
                            arryFORECOLOR = new string[0];
                            arryBOARDSTYLE = new string[0];
                            arryDOCK = new string[0];
                            arryENABLED = new string[0];
                            arryVISIBLED = new string[0];
                            arryTEXT = new string[0];
                            arryFONTS = new string[0];
                            arryMULTILINE = new string[0];
                            arrySCROLLBARS = new string[0];
                            arryTEXTALIGN = new string[0];
                            arryIMAGE = new string[0];
                            arryIMAGESIZEMODE = new string[0];
                            arryCHECKED = new string[0];
                        }

                        Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                        Array.Resize<string>(ref arryCONTROLNAME, arryCONTROLNAME.Length + 1);
                        Array.Resize<string>(ref arryCONTROTYPE, arryCONTROTYPE.Length + 1);
                        Array.Resize<string>(ref arryCONTROLPARENT, arryCONTROLPARENT.Length + 1);
                        Array.Resize<string>(ref arryLOCATIONX, arryLOCATIONX.Length + 1);
                        Array.Resize<string>(ref arryLOCATIONY, arryLOCATIONY.Length + 1);
                        Array.Resize<string>(ref arrySIZEWIDTH, arrySIZEWIDTH.Length + 1);
                        Array.Resize<string>(ref arrySIZEHEIGHT, arrySIZEHEIGHT.Length + 1);
                        Array.Resize<string>(ref arryTAG, arryTAG.Length + 1);
                        Array.Resize<string>(ref arryFRONTBACK, arryFRONTBACK.Length + 1);
                        Array.Resize<string>(ref arryCHILDINDEX, arryCHILDINDEX.Length + 1);
                        Array.Resize<string>(ref arryBACKCOLOR, arryBACKCOLOR.Length + 1);
                        Array.Resize<string>(ref arryFORECOLOR, arryFORECOLOR.Length + 1);
                        Array.Resize<string>(ref arryBOARDSTYLE, arryBOARDSTYLE.Length + 1);
                        Array.Resize<string>(ref arryDOCK, arryDOCK.Length + 1);
                        Array.Resize<string>(ref arryENABLED, arryENABLED.Length + 1);
                        Array.Resize<string>(ref arryVISIBLED, arryVISIBLED.Length + 1);
                        Array.Resize<string>(ref arryTEXT, arryTEXT.Length + 1);
                        Array.Resize<string>(ref arryFONTS, arryFONTS.Length + 1);
                        Array.Resize<string>(ref arryMULTILINE, arryMULTILINE.Length + 1);
                        Array.Resize<string>(ref arrySCROLLBARS, arrySCROLLBARS.Length + 1);
                        Array.Resize<string>(ref arryTEXTALIGN, arryTEXTALIGN.Length + 1);
                        Array.Resize<string>(ref arryIMAGE, arryIMAGE.Length + 1);
                        Array.Resize<string>(ref arryIMAGESIZEMODE, arryIMAGESIZEMODE.Length + 1);
                        Array.Resize<string>(ref arryCHECKED, arryCHECKED.Length + 1);

                        arryEMRNO[arryEMRNO.Length - 1] = dblEmrNoNew.ToString();
                        arryCONTROLNAME[arryCONTROLNAME.Length - 1] = strCONTROLNAME;
                        arryCONTROTYPE[arryCONTROTYPE.Length - 1] = strCONTROTYPE;
                        arryCONTROLPARENT[arryCONTROLPARENT.Length - 1] = strCONTROLPARENT;
                        arryLOCATIONX[arryLOCATIONX.Length - 1] = strLOCATIONX;
                        arryLOCATIONY[arryLOCATIONY.Length - 1] = strLOCATIONY;
                        arrySIZEWIDTH[arrySIZEWIDTH.Length - 1] = strSIZEWIDTH;
                        arrySIZEHEIGHT[arrySIZEHEIGHT.Length - 1] = strSIZEHEIGHT;
                        arryTAG[arryTAG.Length - 1] = strTAG;
                        arryCHILDINDEX[arryCHILDINDEX.Length - 1] = strCHILDINDEX;
                        arryBACKCOLOR[arryBACKCOLOR.Length - 1] = strBACKCOLOR;
                        arryFORECOLOR[arryFORECOLOR.Length - 1] = strFORECOLOR;
                        arryBOARDSTYLE[arryBOARDSTYLE.Length - 1] = strBOARDSTYLE;
                        arryDOCK[arryDOCK.Length - 1] = strDOCK;
                        arryENABLED[arryENABLED.Length - 1] = strENABLED;
                        arryVISIBLED[arryVISIBLED.Length - 1] = strVISIBLED;
                        arryTEXT[arryTEXT.Length - 1] = strTEXT;
                        arryFONTS[arryFONTS.Length - 1] = strFONTS;
                        arryMULTILINE[arryMULTILINE.Length - 1] = strMULTILINE;
                        arrySCROLLBARS[arrySCROLLBARS.Length - 1] = strSCROLLBARS;
                        arryTEXTALIGN[arryTEXTALIGN.Length - 1] = strTEXTALIGN;
                        arryIMAGE[arryIMAGE.Length - 1] = strIMAGE;
                        arryIMAGESIZEMODE[arryIMAGESIZEMODE.Length - 1] = strIMAGESIZEMODE;
                        arryCHECKED[arryCHECKED.Length - 1] = strCHECKED;
                    }
                }

                //

                if (arryEMRNO == null) return rtnVal;

                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTFORM";
                SQL = SQL + ComNum.VBLF + "    (EMRNO,CONTROLNAME,CONTROTYPE,CONTROLPARENT,LOCATIONX,LOCATIONY,";
                SQL = SQL + ComNum.VBLF + "    SIZEWIDTH,SIZEHEIGHT,TAG,CHILDINDEX, BACKCOLOR,FORECOLOR,BOARDSTYLE,DOCK,ENABLED,VISIBLED,TEXT,";
                SQL = SQL + ComNum.VBLF + "    FONTS,MULTILINE,SCROLLBARS,TEXTALIGN,IMAGE,IMAGESIZEMODE,CHECKED)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + ComNum.VBLF + " ?,"; //CONTROLNAME    ,
                SQL = SQL + ComNum.VBLF + " ?,"; //CONTROTYPE    ,
                SQL = SQL + ComNum.VBLF + " ?,"; //CONTROLPARENT  ,
                SQL = SQL + ComNum.VBLF + " ?,"; //LOCATIONX      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //LOCATIONY      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //SIZEWIDTH      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //SIZEHEIGHT      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //TAG      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //CHILDINDEX      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //BACKCOLOR      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //FORECOLOR      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //BOARDSTYLE     ,
                SQL = SQL + ComNum.VBLF + " ?,"; //DOCK           ,
                SQL = SQL + ComNum.VBLF + " ?,"; //ENABLED        ,
                SQL = SQL + ComNum.VBLF + " ?,"; //VISIBLED        ,
                SQL = SQL + ComNum.VBLF + " ?,"; //TEXT           ,
                SQL = SQL + ComNum.VBLF + " ?,"; //FONTS          ,
                SQL = SQL + ComNum.VBLF + " ?,"; //MULTILINE      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //SCROLLBARS     ,
                SQL = SQL + ComNum.VBLF + " ?,"; //TEXTALIGN      ,
                SQL = SQL + ComNum.VBLF + " ?,"; //IMAGE          ,
                SQL = SQL + ComNum.VBLF + " ?,"; //IMAGESIZEMODE  ,
                SQL = SQL + ComNum.VBLF + " ?"; //CHECKED  ,
                SQL = SQL + ComNum.VBLF + " )";

                //if (clsDB.ExecuteChartForm(SQL, dblEmrNoNew,
                //        arryCONTROLNAME, arryCONTROTYPE, arryCONTROLPARENT, arryLOCATIONX, arryLOCATIONY,
                //        arrySIZEWIDTH, arrySIZEHEIGHT, arryTAG, arryCHILDINDEX, arryBACKCOLOR,
                //        arryFORECOLOR, arryBOARDSTYLE, arryDOCK, arryENABLED, arryVISIBLED, arryTEXT, arryFONTS,
                //        arryMULTILINE, arrySCROLLBARS, arryTEXTALIGN, arryIMAGE, arryIMAGESIZEMODE, arryCHECKED,
                //        TRS.Trans) == false)
                //{
                //    return rtnVal;
                //}

                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private static string FontToString(Font font)
        {
            return font.FontFamily.Name + ":" + font.Size + ":" + (int)font.Style;
        }

        /// <summary>
        /// AEMRCHARTROW에 data를 저장한다.(Spread 형식)
        /// </summary>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="SpdWrite"></param>
        /// <param name="mDirection"></param>
        /// <returns></returns>
        public static bool SaveDataAEMRCHARTROWSpd(PsmhDb pDbCon, double dblEmrNoNew, double dblEmrHisNo, string strFormNo, string strUpdateNo, FarPoint.Win.Spread.FpSpread SpdWrite, string mDirection)
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";

            string strITEMCD = "";
            string strITEMNO = "";
            string strITEMINDEX = "";
            string strITEMTYPE = "";
            string strITEMVALUE = "";
            string strITEMVALUE1 = "";
            string strDSPSEQ = "";

            //bool bolIsItem = false;

            string[] arryEMRNO = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryDSPSEQ = null;

            try
            {
                strITEMCD = "";
                strITEMNO = "";
                strITEMINDEX = "-1";
                strITEMTYPE = "";
                strITEMVALUE = "";
                strITEMVALUE1 = "";
                strDSPSEQ = "0";

                //int intMaxValue = 0;
                //string strTYPE = "";
                //string strVALUE = "";

                #region //Flow Sheet 정보를 가지고 온다
                EmrForm pForm = null;
                pForm = clsEmrChart.ClearEmrForm();
                pForm = clsEmrChart.SerEmrFormInfo(pDbCon, strFormNo, strUpdateNo);
                string pFLOWGB = "";
                int intFLOWITEMCNT = 0;
                int intFLOWHEADCNT = 0;
                int intFLOWINPUTSIZE = 0;
                FormFlowSheet[] pFormFlowSheet = null;
                FormFlowSheetHead[,] pFormFlowSheetHead = null;
                FormDesignQuery.GetSetDate_AEMRFLOWXML(strFormNo, strUpdateNo, ref pFLOWGB, ref intFLOWITEMCNT, ref intFLOWHEADCNT, ref intFLOWINPUTSIZE, ref pFormFlowSheet, ref pFormFlowSheetHead);
                #endregion //Flow Sheet 정보를 가지고 온다

                for (i = 0; i < pFormFlowSheet.Length; i++)
                {
                    strITEMCD = pFormFlowSheet[i].ItemCode;
                    if (strITEMCD.IndexOf("_") > -1)
                    {
                        strITEMNO = strITEMCD.Split('_')[0];
                        strITEMINDEX = strITEMCD.Split('_')[1];
                    }
                    else
                    {
                        strITEMNO = strITEMCD;
                        strITEMINDEX = "0";
                    }
                    strITEMTYPE = pFormFlowSheet[i].CellType;

                    if (strITEMTYPE == "CheckBoxCellType")
                    {
                        strITEMTYPE = "CHECK";
                    }
                    else if (strITEMTYPE == "ComboBoxCellType")
                    {
                        strITEMTYPE = "COMBO";
                    }
                    else if (strITEMTYPE == "TextCellType")
                    {
                        strITEMTYPE = "TEXT";
                    }

                    if (pFormFlowSheet[i].CellType == "CheckBoxCellType")
                    {
                        if (pFLOWGB == "COL")
                        {
                            strITEMVALUE = (SpdWrite.ActiveSheet.Cells[0, i].Text.Trim().Equals("True") ? "1" : "0");
                        }
                        else
                        {
                            strITEMVALUE = (SpdWrite.ActiveSheet.Cells[i, 0].Text.Trim().Equals("True") ? "1" : "0");
                        }
                    }
                    else
                    {
                        if (pFLOWGB == "COL")
                        {
                            strITEMVALUE = SpdWrite.ActiveSheet.Cells[0, i].Text.Trim();
                        }
                        else
                        {
                            strITEMVALUE = SpdWrite.ActiveSheet.Cells[i, 0].Text.Trim();
                        }
                    }
                    strITEMVALUE = strITEMVALUE.Replace("'", "");
                    strDSPSEQ = (i + 1).ToString();

                    if (arryEMRNO == null)
                    {
                        arryEMRNO = new string[0];
                        arryITEMCD = new string[0];
                        arryITEMNO = new string[0];
                        arryITEMINDEX = new string[0];
                        arryITEMTYPE = new string[0];
                        arryITEMVALUE = new string[0];
                        arryITEMVALUE1 = new string[0];
                        arryDSPSEQ = new string[0];
                    }
                    Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                    Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                    Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                    Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                    Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                    Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                    Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                    Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                    arryEMRNO[arryEMRNO.Length - 1] = dblEmrNoNew.ToString();
                    arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                    arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                    arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                    arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                    arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                    arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                    arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
                }

                #region //Old
                //if (mDirection == "ROW")
                //{
                //    intMaxValue = SpdWrite.ActiveSheet.RowCount;
                //}
                //else
                //{
                //    intMaxValue = SpdWrite.ActiveSheet.ColumnCount;
                //}

                //for (i = 0; i < intMaxValue; i++)
                //{
                //    strDSPSEQ = i.ToString();

                //    if (mDirection == "ROW")
                //    {
                //        strITEMCD = SpdWrite.ActiveSheet.Cells[i, 0].Text.Trim();
                //        strTYPE = SpdWrite.ActiveSheet.Cells[i, SpdWrite.ActiveSheet.ColumnCount - 1].CellType.ToString();
                //        strVALUE = SpdWrite.ActiveSheet.Cells[i, SpdWrite.ActiveSheet.ColumnCount - 1].Text.Trim();
                //    }
                //    else
                //    {
                //        strITEMCD = SpdWrite.ActiveSheet.Cells[0, i].Text.Trim();
                //        strTYPE = SpdWrite.ActiveSheet.Cells[SpdWrite.ActiveSheet.RowCount - 1, i].CellType.ToString();
                //        strVALUE = SpdWrite.ActiveSheet.Cells[SpdWrite.ActiveSheet.RowCount - 1, i].Text.Trim();
                //    }
                //    if (VB.InStr(strITEMCD, "_") > 0)
                //    {
                //        strITEMNO = VB.Split(strITEMCD, "_")[0];
                //        strITEMINDEX = VB.Split(strITEMCD, "_")[1];
                //    }
                //    else
                //    {
                //        strITEMNO = strITEMCD;
                //        strITEMINDEX = "0";
                //    }

                //    if (strTYPE == "CheckBoxCellType")
                //    {
                //        strITEMTYPE = "CHECK";
                //        if (strVALUE.ToUpper() == "TRUE")
                //        {
                //            strITEMVALUE = "1";
                //        }
                //        else
                //        {
                //            strITEMVALUE = "0";
                //        }
                //    }
                //    else if (strTYPE == "ComboBoxCellType")
                //    {
                //        strITEMTYPE = "COMBO";
                //        strITEMVALUE = strVALUE.Replace("'", "`");
                //    }
                //    else  //나머지 TEXTBOX
                //    {
                //        strITEMTYPE = "TEXT";
                //        strITEMVALUE = strVALUE.Replace("'", "`");
                //    }

                //    if (bolIsItem == true)
                //    {
                //        if (arryEMRNO == null)
                //        {
                //            arryEMRNO = new string[0];
                //            arryITEMCD = new string[0];
                //            arryITEMNO = new string[0];
                //            arryITEMINDEX = new string[0];
                //            arryITEMTYPE = new string[0];
                //            arryITEMVALUE = new string[0];
                //            arryITEMVALUE1 = new string[0];
                //            arryDSPSEQ = new string[0];
                //        }
                //        Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                //        Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                //        Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                //        Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                //        Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                //        Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                //        Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                //        Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                //        arryEMRNO[arryEMRNO.Length - 1] = dblEmrNoNew.ToString();
                //        arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                //        arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                //        arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                //        arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                //        arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                //        arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                //        arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
                //    }
                //}
                #endregion //Old

                if (arryEMRNO == null) return rtnVal;

                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL = SQL + "\r\n" + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL = SQL + "\r\n" + " :ITEMCD, ";   //ITEMCD
                SQL = SQL + "\r\n" + " :ITEMNO, "; //ITEMNO
                SQL = SQL + "\r\n" + " :ITEMINDEX, "; //ITEMINDEX
                SQL = SQL + "\r\n" + " :ITEMTYPE, ";   //ITEMTYPE
                SQL = SQL + "\r\n" + " :ITEMVALUE, ";   //ITEMVALUE
                SQL = SQL + "\r\n" + " :DSPSEQ, ";   //DSPSEQ
                SQL = SQL + "\r\n" + " :ITEMVALUE1, ";   //ITEMVALUE
                SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";   //INPUSEID
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'YYYYMMDD'), ";   //INPDATE
                SQL += ComNum.VBLF + "TO_CHAR(SYSDATE,'HH24MISS') ";   //INPTIME
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteChartRow(pDbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// XML 형식을 저장을 한다 = 동국대 병원은 사용안함
        /// </summary>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="SpdWrite"></param>
        /// <returns></returns>
        private static bool SaveDataAEMRCHARTXML(PsmhDb pDbCon, Control ChartForm, bool isSpcPanel, Control pControl, double dblEmrNoNew, FarPoint.Win.Spread.FpSpread SpdWrite = null)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strXML = "";

            try
            {
                if (SpdWrite != null)
                {
                    strXML = clsXMLOld.SaveDataToXmlSpd(SpdWrite);
                }
                else
                {
                    clsEmrType.EmrXmlImage[] pEmrXmlImage = new clsEmrType.EmrXmlImage[0];
                    strXML = clsXML.SaveDataToXmlOld(ChartForm, false, ChartForm, ref pEmrXmlImage, null);
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,CHARTXML)";
                SQL = SQL + ComNum.VBLF + "VALUES (";
                SQL = SQL + ComNum.VBLF + "      " + dblEmrNoNew + ",";
                SQL = SQL + ComNum.VBLF + "      ?)";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// Chart Image를 저장한다.
        /// </summary>
        /// <param name="ChartForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="dblEmrNoNew"></param>
        /// <returns></returns>
        public static bool SaveDataAEMRCHARTIAMGE(PsmhDb pDbCon, Control ChartForm, bool isSpcPanel, Control pControl, 
                                string strFormNo, string strUpdateNo, double dblEmrHisNo, double dblEmrNo, 
                                clsTrans TRS = null)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strITEMCD = "";
            string strITEMNO = "";
            string strITEMINDEX = "";
            string strITEMVALUE = "";
            string strIMGSVR = "";
            string strIMGPATH1 = "";
            string strIMGPATH2 = "";
            string strIMGPATH = "";

            Ftpedt FtpedtX = null;

            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        ComFunc.MsgBox("선택된 컨테이너가 존재하지 않습니다.");
                        return rtnVal;
                    }
                }

                string strCurDate = ComQuery.CurrentDateTime(pDbCon, "D"); //2015\05\123.jpg
                strIMGPATH1 = VB.Left(strCurDate, 4);
                strIMGPATH2 = VB.Mid(strCurDate, 5, 2);
                strIMGPATH = strIMGPATH1 + "/" + strIMGPATH2;

                DataTable dt = null;
                dt = clsEmrQuery.GetBBasCd(pDbCon, "기록지관리", "이미지PATH", strCurDate, "", "");

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("Data가 존재하지 않습니다.");
                    return rtnVal;
                }

                strIMGSVR = dt.Rows[0]["BASCD"].ToString().Trim();
                string ServerAddress = dt.Rows[0]["BASNAME"].ToString().Trim();
                string strServerPath = dt.Rows[0]["BASEXNAME"].ToString().Trim();
                string UserName = dt.Rows[0]["REMARK1"].ToString().Trim();
                string Password = dt.Rows[0]["REMARK2"].ToString().Trim();
                string HomePath = dt.Rows[0]["VFLAG1"].ToString().Trim();

                dt.Dispose();
                dt = null;

                Control[] controls = null;
                
                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(ChartForm);
                }

                foreach (Control objControl in controls)
                {
                    if (objControl.Name == "IG00249" || objControl.Name == "I0000031497_0" || objControl.Name == "I0000031497_1"
                            || objControl.Name == "I0000031498_0" || objControl.Name == "I0000031498_1")
                    {
                        continue; //싸인 패널은 제외한다
                    }

                    strITEMCD = "";
                    strITEMNO = "";
                    strITEMINDEX = "-1";
                    strITEMVALUE = "";

                    strITEMINDEX = clsXML.IsArryCon(objControl);
                    if (strITEMINDEX == "") strITEMINDEX = "-1";

                    strITEMCD = objControl.Name;
                    strITEMNO = strITEMCD;

                    if ((objControl is FarPoint.Win.Spread.FpSpread) == false)
                    {
                        if (VB.InStr(strITEMCD, "_") > 0)
                        {
                            string[] strParams = VB.Split(VB.Trim(strITEMCD), "_", -1);
                            strITEMNO = strParams[0];
                        }
                    }

                    if (strITEMNO != "txtMedFrDate" && strITEMNO != "txtMedFrTime" && strITEMNO != "")
                    {
                        if (objControl is PictureBox)
                        {
                            strITEMVALUE = "";
                            if (isSpcPanel == true)
                            {
                                if (clsXML.IsParent(objControl, pControl) == true)
                                {
                                    if (((PictureBox)objControl).Tag != null)
                                    {
                                        strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (((PictureBox)objControl).Tag != null)
                                {
                                    strITEMVALUE = ((PictureBox)objControl).Tag.ToString();
                                }
                            }

                            if (strITEMVALUE != "")
                            {
                                //저장
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTIMAGE ";
                                SQL = SQL + ComNum.VBLF + "    (EMRNO , EMRNOHIS  ,ITEMCD       ,IMAGENO      ,IMGSVR    ,IMGPATH   , IMGEXTENSION )";
                                SQL = SQL + ComNum.VBLF + "VALUES (";
                                SQL = SQL + ComNum.VBLF + dblEmrNo.ToString() + ",";    //EMRNO
                                SQL = SQL + ComNum.VBLF + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                                SQL = SQL + ComNum.VBLF + "'" + strITEMCD + "',";   //ITEMCD
                                SQL = SQL + ComNum.VBLF + "'" + strITEMVALUE + "',"; //IMAGENO
                                SQL = SQL + ComNum.VBLF + "'" + strIMGSVR + "',";   //IMGSVR
                                SQL = SQL + ComNum.VBLF + "'" + strIMGPATH + "',";   //IMGPATH
                                SQL = SQL + ComNum.VBLF + "'" + "jpg" + "'";   //IMGEXTENSION
                                SQL = SQL + ComNum.VBLF + ")";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                string strFoldJob = "";
                                string strFoldBase = "";

                                clsEmrFunc.CheckImageJobFold(ref strFoldJob, ref strFoldBase, strFormNo, strUpdateNo, dblEmrHisNo.ToString(), strITEMCD);

                                //파일이 있는지 확인하고
                                if (File.Exists(strFoldJob + "\\" + strITEMVALUE + ".jpg") == false)
                                {
                                    rtnVal = true; //없으면 에러이기는 하지만 없을 수는 없다
                                    return rtnVal;
                                }

                                //FTP저장
                                FtpedtX = new Ftpedt();
                                //FtpedtX.FtpUploadEx("192.168.100.35", "pcnfs", "pcnfs1", strFoldJob, strITEMVALUE, strServerPath,
                                //                    strIMGPATH1 + "/" + strIMGPATH2 , "jpg/dgr");
                                FtpedtX = null;
                                
                            }
                        }
                    }
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                FtpedtX = null;
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 기록지를 이미지로 변환후 내용을 저장한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ChartForm"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strCurDate"></param>
        /// <param name="TRS"></param>
        /// <returns></returns>
        public static bool SaveDataConvertToImage(PsmhDb pDbCon, Control ChartForm, EmrPatient po, string strEmrNo, string strFormNo, string strUpdateNo, 
                                string strChartDate, string strCurDate)
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                string strFilePath = clsType.SvrInfo.strClient + "\\FormToImage\\";
                if (Directory.Exists(strFilePath) == false)
                {
                    Directory.CreateDirectory(strFilePath);
                }

                clsFormPrint.mstrCONVIMAGEPATH = strFilePath;

                int intChartCnt = ((EmrChartForm)ChartForm).ToImageFormMsg("0");
                //int intChartCnt = clsFormPrint.PrintToTifFileLong(strFormNo, strUpdateNo, po, strEmrNo, ChartForm, "C");

                string strIMGPATH = VB.Left(strChartDate, 4) + "/" + VB.Mid(strChartDate, 5, 2) + "/" + VB.Right(strChartDate, 2);

                if (intChartCnt > 0)
                {
                    DataTable dt = null;
                    dt = clsEmrQuery.GetBBasCd(pDbCon, "기록지관리", "CHARTIMAGE", strCurDate, "", "");

                    if (dt == null)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }

                    string strIMGSVR = dt.Rows[0]["BASCD"].ToString().Trim();
                    string ServerAddress = dt.Rows[0]["BASNAME"].ToString().Trim();
                    string strServerPath = dt.Rows[0]["BASEXNAME"].ToString().Trim();
                    //string ServerPort = "21";
                    string UserName = dt.Rows[0]["REMARK1"].ToString().Trim();
                    string Password = dt.Rows[0]["REMARK2"].ToString().Trim();
                    string HomePath = dt.Rows[0]["VFLAG1"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                    //clsWinScp.ConWinScp("Ftp", ServerAddress, UserName, Password, HomePath, "");

                    string strTiff = strFilePath + strEmrNo + ".tif";
                    string[] arryFILENAME = clsTiff.ConvertTiffToPng(strTiff);
                    string strFILENAME = "";

                    for (i = 0; i < arryFILENAME.Length; i++)
                    {
                        strFILENAME = arryFILENAME[i].Replace(strFilePath, "");
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTCONVIMAGE";
                        SQL = SQL + ComNum.VBLF + "(EMRNO, IMAGENO, IMGSVR, IMGPATH, FILENAME, IMGEXTENSION) ";
                        SQL = SQL + ComNum.VBLF + "VALUES (";
                        SQL = SQL + ComNum.VBLF + "    " + VB.Val(strEmrNo) + ",";
                        SQL = SQL + ComNum.VBLF + "    " + (i + 1).ToString() + ",";
                        SQL = SQL + ComNum.VBLF + "    '" + strIMGSVR + "',";
                        SQL = SQL + ComNum.VBLF + "    '" + strIMGPATH + "',";
                        SQL = SQL + ComNum.VBLF + "    '" + strFILENAME + "',";
                        SQL = SQL + ComNum.VBLF + "    '" + "png" + "'";
                        SQL = SQL + ComNum.VBLF + ")";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }

                        if (File.Exists(strFilePath + strFILENAME) == true)
                        {
                            //clsWinScp.gTrsResult = clsWinScp.gWinScp.PutFiles(strFilePath + strFILENAME, strServerPath + "/" + strIMGPATH + "/" + strFILENAME, false, clsWinScp.gTrsOptions);
                            //if (clsWinScp.gTrsResult == null)
                            //{
                            //    return rtnVal;
                            //}
                            Application.DoEvents();
                            File.Delete(strFilePath + strFILENAME);

                        }
                    }
                    Application.DoEvents();
                    File.Delete(strTiff);
                }

                clsFormPrint.mstrCONVIMAGEPATH = "";
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public static bool LoadChartImage(string strITEMVALUE)
        {
            bool rtnVal = false;

            try
            {


                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
       
        /// <summary>
        /// 기록지별 기본 이미지를 표시한다.
        /// </summary>
        /// <param name="objParent"></param>
        /// <param name="strFORMNO"></param>
        /// <param name="strUPDATENO"></param>
        public static void SetFormInitImage(PsmhDb pDbCon, Control objParent, string strFORMNO, string strUPDATENO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Control[] controls = ComFunc.GetAllControls(objParent);

            try
            {
                foreach (Control ctl in controls)
                {
                    if (ctl is PictureBox)
                    {
                        string strCONTROLID = ((PictureBox)ctl).Name;

                        SQL = " SELECT ";
                        SQL = SQL + ComNum.VBLF + "        A.FORMNO, A.UPDATENO, A.CONTROLID, A.GRPTYPE, A.GRPGB, A.USEGB, A.IMAGENO, B.IMAGEEXT";
                        SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRFORMIMAGE A";
                        SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASIMAGE B";
                        SQL = SQL + ComNum.VBLF + "        ON A.IMAGENO = B.IMAGENO";
                        SQL = SQL + ComNum.VBLF + "    WHERE A.FORMNO = " + strFORMNO;
                        SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = " + strUPDATENO;
                        SQL = SQL + ComNum.VBLF + "        AND A.CONTROLID = '" + strCONTROLID + "'";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            string mstrFoldJob = @"\EmrImageTmp\";
                            string strImageName = dt.Rows[0]["IMAGEEXT"].ToString().Trim();

                            //clsWinScp.ConWinScp("Ftp", clsType.gSvrInfo.strServerIp, clsType.gSvrInfo.strUser, clsType.gSvrInfo.strPasswd, clsType.gSvrInfo.strSvrHomePath, "");
                            //clsWinScp.gTrsResult = clsWinScp.gWinScp.GetFiles(clsType.gSvrInfo.strServerPath + "/EmrImage/BaseImage/" + strImageName, clsType.gSvrInfo.strClient + mstrFoldJob + strImageName, false, clsWinScp.gTrsOptions);

                            //이미지 사이즈 조정후 로드
                            int intWidth = ((PictureBox)ctl).Width;
                            int intHeight = ((PictureBox)ctl).Height;

                            Bitmap image1 = (Bitmap)Image.FromFile(clsType.SvrInfo.strClient + mstrFoldJob + strImageName, true);
                            Bitmap newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                            Graphics graphics_1 = Graphics.FromImage(newImage);
                            graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                            graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                            graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                            ((PictureBox)ctl).Image = newImage;
                            image1.Dispose();
                            image1 = null;

                            File.Delete(clsType.SvrInfo.strClient + mstrFoldJob + strImageName);
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 개별 배경이미지를 로드한다.
        /// </summary>
        /// <param name="strFORMNO"></param>
        /// <param name="strUPDATENO"></param>
        /// <param name="ctl"></param>
        public static void SetFormInitImageEx(PsmhDb pDbCon, string strFORMNO, string strUPDATENO, Control ctl)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                string strCONTROLID = ((PictureBox)ctl).Name;

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "        A.FORMNO, A.UPDATENO, A.CONTROLID, A.GRPTYPE, A.GRPGB, A.USEGB, A.IMAGENO, B.IMAGEEXT";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRFORMIMAGE A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASIMAGE B";
                SQL = SQL + ComNum.VBLF + "        ON A.IMAGENO = B.IMAGENO";
                SQL = SQL + ComNum.VBLF + "    WHERE A.FORMNO = " + strFORMNO;
                SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = " + strUPDATENO;
                SQL = SQL + ComNum.VBLF + "        AND A.CONTROLID = '" + strCONTROLID + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    string mstrFoldJob = @"\EmrImageTmp\";
                    string strImageName = dt.Rows[0]["IMAGEEXT"].ToString().Trim();
                    
                    //clsWinScp.ConWinScp("Ftp", clsType.gSvrInfo.strServerIp, clsType.gSvrInfo.strUser, clsType.gSvrInfo.strPasswd, clsType.gSvrInfo.strSvrHomePath, "");
                    //clsWinScp.gTrsResult = clsWinScp.gWinScp.GetFiles(clsType.gSvrInfo.strServerPath + "/EmrImage/BaseImage/" + strImageName, clsType.gSvrInfo.strClient + mstrFoldJob + strImageName, false, clsWinScp.gTrsOptions);

                    //이미지 사이즈 조정후 로드
                    int intWidth = ((PictureBox)ctl).Width;
                    int intHeight = ((PictureBox)ctl).Height;

                    Bitmap image1 = (Bitmap)Image.FromFile(clsType.SvrInfo.strClient + mstrFoldJob + strImageName, true);
                    Bitmap newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                    Graphics graphics_1 = Graphics.FromImage(newImage);
                    graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                    graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                    graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                    ((PictureBox)ctl).Image = newImage;
                    image1.Dispose();
                    image1 = null;

                    File.Delete(clsType.SvrInfo.strClient + mstrFoldJob + strImageName);
                    
                }
                dt.Dispose();
                dt = null;

            }
            catch
            {
            }
        }

        /// <summary>
        /// 기록지별 기록지 코드를 가지고 온다.
        /// </summary>
        public static void FindChart(PsmhDb pDbCon)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strBsnsCls = "기록지종류";
            //string strUnitCls = "입원기록, 경과기록, 수술기록, 퇴원기록, 전과기록";

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT UNITCLS, BASCD, BASNAME, BASEXNAME, BASVAL";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRBASCD ";
            SQL = SQL + ComNum.VBLF + "  WHERE BSNSCLS = '" + strBsnsCls + "'";
            SQL = SQL + ComNum.VBLF + "  ORDER BY BASCD ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["UNITCLS"].ToString().Trim())
                {
                    case "입원기록":
                        clsEmrPublic.AdmFormNo = dt.Rows[i]["BASNAME"].ToString().Trim();
                        break;
                    case "경과기록":
                        clsEmrPublic.ProgFormNo = dt.Rows[i]["BASNAME"].ToString().Trim();
                        break;
                    case "수술기록":
                        clsEmrPublic.OpFormNo = dt.Rows[i]["BASNAME"].ToString().Trim();
                        break;
                    case "퇴원기록":
                        clsEmrPublic.DishFormNo = dt.Rows[i]["BASNAME"].ToString().Trim();
                        break;
                    case "전과기록":
                        clsEmrPublic.TrsDptFormNo = dt.Rows[i]["BASNAME"].ToString().Trim();
                        break;
                }
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 내원내역을 가지고 온다.
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strFlag"></param>
        /// <param name="strSubQuery"></param>
        /// <returns></returns>
        public static DataTable GetEmrAcpInfo(PsmhDb pDbCon, string strPtNo, string strFlag, string strSubQuery = "", string strSubQuery2 = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    T.PATID AS PTNO, ";
            SQL = SQL + ComNum.VBLF + "    P.PATIENT_NAME AS PTNAME,  ";
            SQL = SQL + ComNum.VBLF + "    p.JUMIN1 AS SSNO1, ";
            SQL = SQL + ComNum.VBLF + "    p.JUMIN2 AS SSNO2, ";
            SQL = SQL + ComNum.VBLF + "    P.TEL AS TEL, ";
            SQL = SQL + ComNum.VBLF + "    P.TEL AS CELPHON, ";
            SQL = SQL + ComNum.VBLF + "    '' AS ZIPCD, ";
            SQL = SQL + ComNum.VBLF + "    '' AS ADDR, ";
            SQL = SQL + ComNum.VBLF + "    '' AS BIRTHDATE, ";
            SQL = SQL + ComNum.VBLF + "    '' AS ADDRESS, ";
            SQL = SQL + ComNum.VBLF + "    '' AS ZIPCDLOAD, ";
            SQL = SQL + ComNum.VBLF + "    '' AS ADDRLOAD, ";
            SQL = SQL + ComNum.VBLF + "    '' AS ADDRESSLOAD, ";
            SQL = SQL + ComNum.VBLF + "    TREATNO AS ACPNO, ";
            // 2017-02-10 소아과 성장곡선 표시
            SQL = SQL + ComNum.VBLF + "	(SELECT FORMNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
            SQL = SQL + ComNum.VBLF + "     WHERE FORMNO = '847' ";
            SQL = SQL + ComNum.VBLF + "     AND ACPNO = TREATNO) FRM847, ";

            SQL = SQL + ComNum.VBLF + "    T.CLASS AS INOUTCLS, ";
            SQL = SQL + ComNum.VBLF + "    T.INDATE AS MEDFRDATE, ";
            SQL = SQL + ComNum.VBLF + "    T.INTIME AS MEDFRTIME, ";
            SQL = SQL + ComNum.VBLF + "    T.OUTDATE AS MEDENDDATE, ";
            SQL = SQL + ComNum.VBLF + "    '' AS MEDENDTIME, ";
            SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDEXDATE, ";
            SQL = SQL + ComNum.VBLF + "    T.CLINCODE AS MEDDEPTCD, ";
            SQL = SQL + ComNum.VBLF + "    T.DOCCODE AS MEDDRCD, ";
            SQL = SQL + ComNum.VBLF + "    '0' AS CNCLYN, ";
            SQL = SQL + ComNum.VBLF + "    D.DEPTKORNAME AS MEDDEPTKORNAME, ";
            SQL = SQL + ComNum.VBLF + "    U.USENAME AS MEDDRNAME, ";
            SQL = SQL + ComNum.VBLF + "    '' AS WARD, ";
            SQL = SQL + ComNum.VBLF + "    '' AS ROOM, ";
            SQL = SQL + ComNum.VBLF + "    '' AS REMARK ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT T ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT P ";
            SQL = SQL + ComNum.VBLF + "    ON P.PATIENT_NO = T.PATID ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWMEDDEPT D ";
            SQL = SQL + ComNum.VBLF + "    ON TRIM(D.MEDDEPTCD) = TRIM(T.CLINCODE) ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
            SQL = SQL + ComNum.VBLF + "    ON U.USEID = T.DOCCODE ";
            if (strFlag == "IPD") //입원인 경우
            {
                SQL = SQL + ComNum.VBLF + "WHERE T.PATID = '" + strPtNo + "'  AND T.CLINCODE <> '99' AND T.INDATE  > '20150101'  ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE T.PATID = '" + strPtNo + "'   ";
            }

            SQL = SQL + strSubQuery;
            if (strFlag == "N") //A:전체,  N:작성내역만
            {
                SQL = SQL + ComNum.VBLF + " AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRCHARTMST C  ";
                SQL = SQL + ComNum.VBLF + "                WHERE C.PTNO = T.PATID  ";
                SQL = SQL + ComNum.VBLF + "                AND C.ACPNO = T.TREATNO) ";
            }
            SQL = SQL + strSubQuery2;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 내용을 스프래드에 표시한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="p"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="ssWrite"></param>
        /// <param name="dtMedFrDate"></param>
        /// <param name="txtMedFrTime"></param>
        /// <param name="mbtnSave"></param>
        /// <param name="mbtnSaveTemp"></param>
        /// <param name="mbtnDelete"></param>
        /// <param name="strDirection"></param>
        /// <param name="strOption"></param>
        public static void QuerySpdData(PsmhDb pDbCon, EmrPatient p, string strFormNo, string strEmrNo, FarPoint.Win.Spread.FpSpread ssWrite,
                                                DateTimePicker dtMedFrDate, ComboBox txtMedFrTime, Button mbtnSave, Button mbtnSaveTemp, Button mbtnDelete,
                                                string strDirection, string strOption)
        {
            string strCHARTDATE = "";
            string strCHARTTIME = "";
            string strUSEID = "";
            string strPRNYN = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            if (strOption == "H")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO = ( SELECT MAX(H.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST H ";
                SQL = SQL + ComNum.VBLF + "                        WHERE  H.PTNO = '" + p.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "                        AND H.FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "                        AND H.CHARTDATE <= TO_CHAR(SYSDATE,'YYYYMMDD') )";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO = " + VB.Val(strEmrNo);
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            strCHARTDATE = (dt.Rows[0]["CHARTDATE"].ToString() + "").Trim();
            strCHARTTIME = (dt.Rows[0]["CHARTTIME"].ToString() + "").Trim();
            strUSEID = (dt.Rows[0]["CHARTUSEID"].ToString() + "").Trim();
            strPRNYN = (dt.Rows[0]["PRNTYN"].ToString() + "").Trim();
            strEmrNo = (dt.Rows[0]["EMRNO"].ToString() + "").Trim();

            if (strDirection == "H")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < ssWrite.ActiveSheet.RowCount; j++)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[j, 0].Text.Trim())
                        {
                            clsSpread.SetSpdValue(ssWrite.ActiveSheet, j, ssWrite.ActiveSheet.ColumnCount - 1, dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < ssWrite.ActiveSheet.ColumnCount; j++)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[0, j].Text.Trim())
                        {
                            clsSpread.SetSpdValue(ssWrite.ActiveSheet, ssWrite.ActiveSheet.RowCount - 1, j, dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;

            if (strOption != "H")
            {
                dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strCHARTDATE, "D"));
                txtMedFrTime.Text = ComFunc.FormatStrToDate(strCHARTTIME, "T");

                dtMedFrDate.Enabled = false;

                if (clsType.User.IdNumber != strUSEID)
                {
                    mbtnSave.Visible = false;
                    mbtnDelete.Visible = false;
                }
                else
                {
                    if (strPRNYN == "1")
                    {
                        mbtnSave.Visible = false;
                        mbtnDelete.Visible = false;
                    }
                    else
                    {
                        mbtnSave.Visible = true;
                        mbtnDelete.Visible = true;
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }



        public static string ChartTotalCount(PsmhDb pDbCon, EmrPatient pAcp, string strFormNo, string strUpdateNo, string strFrDate, string strEndDate)
        {
            //int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return string.Empty;

            SQL = SQL + ComNum.VBLF + "SELECT COUNT(*) AS CNT";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRXML C ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "AEMRFLOWXML FX ";
            SQL = SQL + ComNum.VBLF + "          ON C.FORMNO = FX.FORMNO ";
            SQL = SQL + ComNum.VBLF + "         AND C.UPDATENO = FX.UPDATENO ";
            SQL = SQL + ComNum.VBLF + " WHERE C.PTNO = '" + pAcp.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND C.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "   AND C.CHARTDATE >= '" + strFrDate + "' ";
            SQL = SQL + ComNum.VBLF + "   AND C.CHARTDATE <= '" + strEndDate + "' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return string.Empty;
            }

            return dt.Rows[0]["CNT"].ToString();

        }

        /// <summary>
        /// 내원 내역 입력한 구분 리스트를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="pForm"></param>
        public static DataTable QueryCboGbnList(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm, bool dblSearch = false)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return null;

            SQL.AppendLine("SELECT B.ITEMVALUE");
            SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
            SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
            SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
            if (dblSearch == false)
            {
                SQL.AppendLine("    AND B.ITEMNO IN ('I0000034115', 'I0000024733') -- 욕창위치, 상처구분");
            }
            else
            {
                SQL.AppendLine("    AND B.ITEMNO IN ('I0000034121') -- 상처부위");
            }
            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
            SQL.AppendLine("     ON U.USERID = C.CHARTUSEID");
            SQL.AppendLine("WHERE C.FORMNO   = " + pForm.FmFORMNO);
            SQL.AppendLine("  AND C.UPDATENO = " + pForm.FmUPDATENO);
            SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "'");
            SQL.AppendLine("  AND C.PTNO = '" + pAcp.ptNo + "'");
            SQL.AppendLine("  AND B.ITEMVALUE IS NOT NULL");
            SQL.AppendLine("GROUP BY B.ITEMVALUE");

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 가장 최근 내용을 가져와서 입력란에 뿌려준다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="pForm"></param>
        public static DataTable QuerySpdLastList(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return null;

            #region NEW
            SQL.AppendLine("SELECT ROWNUM AS RNUM,");
            SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
            SQL.AppendLine("                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,");
            SQL.AppendLine("                U.NAME AS USENAME");
            SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
            SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
            SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
            SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
            SQL.AppendLine("    AND B.ITEMCD > CHR(0)");
            //SQL.AppendLine("    AND B.ITEMCD LIKE '%_" + cboItem.Tag.ToString().Split('_')[1] + "'");
            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
            SQL.AppendLine("     ON U.USERID = C.CHARTUSEID");
            #endregion
            SQL.AppendLine("  WHERE C.EMRNO = ");
            SQL.AppendLine("  (");
            SQL.AppendLine("  SELECT MAX(EMRNO)");
            SQL.AppendLine("    FROM " + ComNum.DB_EMR + "AEMRCHARTMST");
            SQL.AppendLine("   WHERE MEDFRDATE = '" + pAcp.medFrDate + "'");
            SQL.AppendLine("     AND PTNO = '" + pAcp.ptNo + "'");
            SQL.AppendLine("     AND FORMNO = " + pForm.FmFORMNO);
            SQL.AppendLine("     AND UPDATENO = " + pForm.FmUPDATENO);
            SQL.AppendLine("     AND (CHARTDATE || RPAD(CHARTTIME, 6, '0')) = ");
            SQL.AppendLine("         (");
            SQL.AppendLine("         SELECT MAX(CHARTDATE || RPAD(CHARTTIME, 6, '0'))");
            SQL.AppendLine("           FROM " + ComNum.DB_EMR + "AEMRCHARTMST");
            SQL.AppendLine("          WHERE MEDFRDATE = '" + pAcp.medFrDate + "'");
            SQL.AppendLine("            AND PTNO = '" + pAcp.ptNo  + "'");
            SQL.AppendLine("            AND FORMNO = " + pForm.FmFORMNO);
            SQL.AppendLine("            AND UPDATENO = " + pForm.FmUPDATENO);
            SQL.AppendLine("         )");
            SQL.AppendLine("   )");
            SQL.AppendLine("ORDER BY (TRIM(C.CHARTDATE) || TRIM(C.CHARTTIME)), C.EMRNO");

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }


        /// <summary>
        /// 내용을 스프래드에 표시한다(상처, 욕창간호 기록지)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="pForm"></param>
        /// <param name="chkAsc"></param>
        public static DataTable QuerySpdLastList(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm, string strItem, string strItem2, bool chkAsc = true)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return null;

            #region NEW
            SQL.AppendLine("WITH ITEM_DATA AS ");
            SQL.AppendLine("(");
            SQL.AppendLine("SELECT ITEMVALUE, MAX(CHARTDATE || CHARTTIME) CHARTDATETIME, MAX(C.EMRNO) EMRNO");
            SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
            SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
            SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
            SQL.AppendLine("    AND B.ITEMNO IN ('I0000034115', 'I0000024733') ");
            SQL.AppendLine("    AND B.ITEMVALUE IN (" + strItem + ")");

            if (strItem2.NotEmpty())
            {
                SQL.AppendLine("  AND EXISTS ");
                SQL.AppendLine("  (");
                SQL.AppendLine("    SELECT 1");
                SQL.AppendLine("      FROM KOSMOS_EMR.AEMRCHARTROW");
                SQL.AppendLine("     WHERE EMRNO = C.EMRNO");
                SQL.AppendLine("       AND EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("       AND ITEMNO IN ('I0000034121') ");
                SQL.AppendLine("       AND ITEMVALUE IN (" + strItem2 + ")");
                SQL.AppendLine("  )");
            }

            if (pAcp.inOutCls == "I")
            {
                SQL.AppendLine("WHERE C.MEDFRDATE = '" + pAcp.medFrDate + "'");
                SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                SQL.AppendLine("  AND C.PTNO      = '" + pAcp.ptNo + "'");
            }
            else
            {
                SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "'");
                SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
            }

            SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
            SQL.AppendLine("  AND C.UPDATENO = " + pForm.FmUPDATENO);
            SQL.AppendLine("GROUP BY B.ITEMVALUE");
            SQL.AppendLine(")");
            #endregion

            #region NEW
            SQL.AppendLine("SELECT ROWNUM AS RNUM,");
            SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
            SQL.AppendLine("                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,");
            SQL.AppendLine("                U.NAME AS USENAME");
            SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
            SQL.AppendLine("     , (SELECT ITEMNUMBER FROM KOSMOS_EMR.AEMRFLOWXML WHERE FORMNO = C.FORMNO AND UPDATENO = C.UPDATENO  AND ITEMNO = B.ITEMCD) AS SEQNO");
            SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
            SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
            SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
            SQL.AppendLine("    AND B.ITEMNO > CHR(0)");

            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
            SQL.AppendLine("     ON U.USERID = C.CHARTUSEID");

            if (pAcp.inOutCls == "I")
            {
                SQL.AppendLine("WHERE C.MEDFRDATE = '" + pAcp.medFrDate + "'");
                SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                SQL.AppendLine("  AND C.PTNO      = '" + pAcp.ptNo + "'");
            }
            else
            {
                SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "'");
                SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
            }

            SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
            SQL.AppendLine("  AND C.UPDATENO = " + pForm.FmUPDATENO);
            SQL.AppendLine("  AND ((C.CHARTDATE || C.CHARTTIME)) IN ");
            SQL.AppendLine("  (");
            SQL.AppendLine("    SELECT CHARTDATETIME");
            SQL.AppendLine("      FROM ITEM_DATA");
            SQL.AppendLine("  )");
            SQL.AppendLine("  AND EXISTS ");
            SQL.AppendLine("  (");
            SQL.AppendLine("    SELECT 1");
            SQL.AppendLine("      FROM KOSMOS_EMR.AEMRCHARTROW");
            SQL.AppendLine("     WHERE EMRNO = C.EMRNO");
            SQL.AppendLine("       AND EMRNOHIS = C.EMRNOHIS");
            SQL.AppendLine("       AND ITEMNO IN ('I0000034115', 'I0000024733') ");
            SQL.AppendLine("       AND ITEMVALUE IN (" + strItem + ")");
            SQL.AppendLine("  )");

            if (strItem2.NotEmpty())
            {
                SQL.AppendLine("  AND EXISTS ");
                SQL.AppendLine("  (");
                SQL.AppendLine("    SELECT 1");
                SQL.AppendLine("      FROM KOSMOS_EMR.AEMRCHARTROW");
                SQL.AppendLine("     WHERE EMRNO = C.EMRNO");
                SQL.AppendLine("       AND EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("       AND ITEMNO IN ('I0000034121') ");
                SQL.AppendLine("       AND ITEMVALUE IN (" + strItem2 + ")");
                SQL.AppendLine("  )");
            }
            #endregion

            if (chkAsc == true)
            {
                SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE), TRIM(C.CHARTTIME), C.EMRNO, SEQNO");
            }
            else
            {
                SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE) DESC, TRIM(C.CHARTTIME) DESC, C.EMRNO DESC, SEQNO");
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내용을 스프래드에 표시한다(상처, 욕창간호 기록지)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="pForm"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="strInOutCls"></param>
        /// <param name="strSearchItem"></param>
        /// <param name="ViewNpChart">정신과 여부</param>
        /// <param name="chkAsc"></param>
        public static DataTable QuerySpdList(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm, string strFrDate, string strEndDate,
            string strInOutCls, string strItem, string strItem2, bool ViewNpChart, bool chkAsc = true)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return null;

            #region NEW
            SQL.AppendLine("SELECT ROWNUM AS RNUM,");
            SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
            SQL.AppendLine("                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,");
            SQL.AppendLine("                U.NAME AS USENAME");
            SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
            SQL.AppendLine("     , (SELECT ITEMNUMBER FROM KOSMOS_EMR.AEMRFLOWXML WHERE FORMNO = C.FORMNO AND UPDATENO = C.UPDATENO  AND ITEMNO = B.ITEMCD) AS SEQNO");
            SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
            SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
            SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
            SQL.AppendLine("    AND B.ITEMNO > CHR(0)");

            SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
            SQL.AppendLine("     ON U.USERID = C.CHARTUSEID");

            if (pAcp.inOutCls == "I")
            {
                SQL.AppendLine("WHERE C.MEDFRDATE = '" + pAcp.medFrDate + "'");
                SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                SQL.AppendLine("  AND C.PTNO      = '" + pAcp.ptNo + "'");
            }
            else
            {
                SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "'");
                SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
            }

            SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
            SQL.AppendLine("  AND C.UPDATENO = " + pForm.FmUPDATENO);
            SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "'");
            SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "'   ");
            #endregion


            if (ViewNpChart == false)
            {
                SQL.AppendLine("  AND C.MEDDEPTCD <> 'NP'");
            }

            if (strItem.NotEmpty())
            {
                SQL.AppendLine("  AND EXISTS");
                SQL.AppendLine("  (");
                SQL.AppendLine("  SELECT 1");
                SQL.AppendLine("    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A");
                SQL.AppendLine("      INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
                SQL.AppendLine("         ON A.EMRNO = B.EMRNO");
                SQL.AppendLine("        AND A.EMRNOHIS = A.EMRNOHIS");
                SQL.AppendLine("        AND B.ITEMNO IN ('I0000034115', 'I0000024733', 'I0000034121') ");
                SQL.AppendLine("        AND B.ITEMVALUE IN (" + strItem + ")");
                SQL.AppendLine("   WHERE A.EMRNO = C.EMRNO");
                SQL.AppendLine("  )");
            }

            if (strItem2.NotEmpty())
            {
                SQL.AppendLine("  AND EXISTS");
                SQL.AppendLine("  (");
                SQL.AppendLine("  SELECT 1");
                SQL.AppendLine("    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A");
                SQL.AppendLine("      INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
                SQL.AppendLine("         ON A.EMRNO = B.EMRNO");
                SQL.AppendLine("        AND A.EMRNOHIS = A.EMRNOHIS");
                SQL.AppendLine("        AND B.ITEMNO IN ('I0000034121') ");
                SQL.AppendLine("        AND B.ITEMVALUE IN (" + strItem2 + ")");
                SQL.AppendLine("   WHERE A.EMRNO = C.EMRNO");
                SQL.AppendLine("  )");
            }

            if (chkAsc == true)
            {
                SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE), TRIM(C.CHARTTIME), C.EMRNO, SEQNO");
            }
            else
            {
                SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE) DESC, TRIM(C.CHARTTIME) DESC, C.EMRNO DESC, SEQNO");
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }


        /// <summary>
        /// 내용을 스프래드에 표시한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="p"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="ViewNpChart">정신과 여부</param>
        /// <param name="chkAsc"></param>
        public static DataTable QuerySpdList(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm, string strFrDate, string strEndDate,
            string strInOutCls, bool ViewNpChart,  bool chkAsc = true)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return null;

            SQL.AppendLine("SELECT ROWNUM AS RNUM,");

            if (pForm.FmOLDGB == 1)
            {
                #region XML
                if (pForm.FmFORMNO == 963 || pForm.FmFORMNO == 1232)
                {
                    SQL.AppendLine("     C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.USEID AS CHARTUSEID, C.WRITEDATE, C.WRITETIME,  -- C.CHARTXML,  ");
                    SQL.AppendLine("     CASE WHEN f.FORMNO = 963 THEN EXTRACT(chartxml, '//ta1')");
                    SQL.AppendLine("     END  ITEMVALUE, ");
                    SQL.AppendLine("     CASE WHEN F.FORMNO = 1232 THEN  (SELECT EMRIMAGEMERGE FROM  kosmos_emr.EMRXMLIMAGES WHERE EMRNO = C.EMRNO) ELSE NULL");
                    SQL.AppendLine("     END  IMAGEVALUE, ");
                    SQL.AppendLine("     CASE WHEN EXISTS(SELECT 1");
                    SQL.AppendLine("       FROM " + ComNum.DB_EMR + "EMRPRTREQ ");
                    SQL.AppendLine("       WHERE EMRNO = C.EMRNO ");
                    SQL.AppendLine("         AND SCANYN  = 'T'");
                    SQL.AppendLine("         AND PRINTYN = 'Y'");
                    SQL.AppendLine("     ) THEN '사 본' ELSE NULL END PRNTYN");
                    SQL.AppendLine("     ,(SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS USENAME,");
                    SQL.AppendLine("     CASE WHEN F.FORMNO = 963 THEN 'TextCellType'  ELSE 'ImageCellType' END ItemType,");
                    SQL.AppendLine("     CASE WHEN F.FORMNO = 963 THEN 'ta1'  ELSE 'im1' END AS ITEMCD, 'progress' AS ITEMNAME, F.FORMNO");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "EMRXML C ");
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ");
                    SQL.AppendLine("   ON C.FORMNO = F.FORMNO ");
                    SQL.AppendLine("  AND F.UPDATENO = " + pForm.FmUPDATENO);
           
                    SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");

                    if (pAcp.inOutCls.Equals("O"))
                    {
                        SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "' ");
                        SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "' ");
                    }
                    else
                    {
                        SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
                        SQL.AppendLine("  AND C.INOUTCLS = 'I'");
                    }

                    SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                }               
                else
                {
                    SQL.AppendLine("     C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.USEID AS CHARTUSEID, C.WRITEDATE, C.WRITETIME, C.FORMNO, --C.CHARTXML,  ");
                    SQL.AppendLine("     EXTRACTVALUE(C.CHARTXML,'//' || FX.ITEMNO) AS ITEMVALUE,");
                    SQL.AppendLine("     CASE WHEN EXISTS(SELECT 1");
                    SQL.AppendLine("       FROM " + ComNum.DB_EMR + "EMRPRTREQ ");
                    SQL.AppendLine("       WHERE EMRNO = C.EMRNO ");
                    SQL.AppendLine("         AND SCANYN  = 'T'");
                    SQL.AppendLine("         AND PRINTYN = 'Y'");
                    SQL.AppendLine("     ) THEN '사 본' ELSE NULL END PRNTYN");
                    SQL.AppendLine("     ,(SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS USENAME,");
                    SQL.AppendLine("     FX.ITEMNO AS ITEMCD, FX.ITEMNAME, FX.CELLTYPE AS ITEMTYPE ");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "EMRXML C ");
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ");
                    SQL.AppendLine("   ON C.FORMNO = F.FORMNO ");
                    SQL.AppendLine("  AND F.UPDATENO = " + pForm.FmUPDATENO);
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRFLOWXML FX ");
                    SQL.AppendLine("   ON F.FORMNO = FX.FORMNO ");
                    SQL.AppendLine("  AND F.UPDATENO = FX.UPDATENO ");
                    SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                }

                if (pAcp.inOutCls == "O")
                {
                    if (pAcp.medDeptCd == "RA" || (pAcp.medDeptCd == "MD" && (pAcp.medDrCd == "1107" || pAcp.medDrCd == "1125")))
                    {
                        SQL.AppendLine("  AND C.MEDDEPTCD = 'MD'");
                        SQL.AppendLine("  AND C.MEDDRCD IN ('1107','1125')");
                    }
                    else
                    {
                        if (pAcp.medDeptCd != "HD")
                        {
                            SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("  AND C.MEDDRCD NOT IN ('1107','1125')");
                        }
                    }

                    SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "' ");
                    SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "' ");
                    //ER 간호기록일 경우(ER에 2일 이상 있는경우 있어서 수정)
                    if (pForm.FmFORMNO == 2049)
                    {
                        SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
                    }
                }
                else
                {
                    SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
                    SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "' ");
                    SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "' ");

                    if (strInOutCls == "O")
                    {
                        SQL.AppendLine("  AND C.INOUTCLS IN ('O') ");
                    }
                    else
                    {
                        SQL.AppendLine("  AND C.INOUTCLS IN ('I') ");
                    }
                }

                if (pAcp.inOutCls == "O" && pAcp.medDeptCd == "HD")
                {
                    SQL.AppendLine("  AND C.INOUTCLS IN ('O','I') ");
                }
                else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "HD")
                {
                    SQL.AppendLine("  AND C.INOUTCLS IN ('O') ");
                }

                #endregion
            }
            else
            {
                #region NEW
                //mstrFormNo == "2135" || mstrFormNo == "1935" || mstrFormNo == "2431" || mstrFormNo == "1969" || mstrFormNo == "2201"
                if (pForm.FmFORMNO == 1232)
                {
                    SQL.AppendLine("     C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME");
                    SQL.AppendLine("     , (SELECT IMAGE FROM  kosmos_emr.AEMRCHARTDRAW WHERE EMRNO = C.EMRNO AND FORMNO = 1232) as IMAGEVALUE");
                    SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN ");
                    SQL.AppendLine("     , U.NAME AS USENAME");
                    SQL.AppendLine("     , 'ImageCellType' AS ItemType");
                    SQL.AppendLine("     , 'I0000029770' AS ITEMCD, 'progress' AS ITEMNAME, C.FORMNO");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                    SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
                }
                //투약기록지
                else if (pForm.FmFORMNO == 1796)
                {
                    SQL.AppendLine(" C.FORMNO, C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME");
                    SQL.AppendLine("     , B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE");
                    SQL.AppendLine("     , U.NAME AS USENAME");
                    SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN ");
                    SQL.AppendLine("     , (SELECT BUN FROM KOSMOS_OCS.OCS_ORDERCODE WHERE TRIM(SUCODE) = B2.ITEMVALUE AND ROWNUM = 1) AS BUN");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
                    SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                    SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                    SQL.AppendLine("    AND B.ITEMCD > CHR(0)");
                    //SQL.AppendLine("   INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B2 -- 처방코드");
                    SQL.AppendLine("   left outer JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B2 -- 처방코드");
                    SQL.AppendLine("     ON B2.EMRNO = C.EMRNO");
                    SQL.AppendLine("    AND B2.EMRNOHIS = C.EMRNOHIS");
                    SQL.AppendLine("    AND B2.ITEMCD  = 'I0000037685'");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                    SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
                }

                else if (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 2135 || pForm.FmFORMNO == 1935 ||
                         pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || pForm.FmFORMNO == 2201)
                    //임상관찰, 회복실, Angio
                    ////진정 환자 평가, 응급실 SPECIAL WATCH RECORD, 인공신장실 V/S
                {
                    #region //Old
                    //SQL.AppendLine("    C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN, ");
                    //SQL.AppendLine("    BB.BASNAME AS BGRPNAME,                                                                            ");
                    //SQL.AppendLine("    BG.BASNAME AS GRPNAME,                                                                             ");
                    //SQL.AppendLine("    R.ITEMCD,                                                                                          ");
                    //SQL.AppendLine("    BI.BASNAME AS ITEMNAME,                                                                            ");
                    //SQL.AppendLine("    R.ITEMVALUE || R.ITEMVALUE1 AS ITEMVALUE,                                                          ");
                    //SQL.AppendLine("	U.NAME  AS USENAME                                                                                 ");
                    //SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C                                                                         ");
                    //SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "EMR_USERT U                                                                      ");
                    //SQL.AppendLine("   ON C.CHARTUSEID = U.USERID                                                                         ");
                    //SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R                                                                   ");
                    //SQL.AppendLine("   ON C.EMRNO = R.EMRNO                                                                               ");
                    //SQL.AppendLine("  AND C.EMRNOHIS = R.EMRNOHIS                                                                        ");
                    //SQL.AppendLine("  AND R.ITEMVALUE > CHR(0)");
                    //SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BI                                                                     ");
                    //SQL.AppendLine("   ON R.ITEMCD = BI.BASCD                                                                             ");
                    //SQL.AppendLine("  AND BI.BSNSCLS = '기록지관리'                                                                        ");
                    //SQL.AppendLine("  AND BI.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')                                       ");
                    //SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG                                                                     ");
                    //SQL.AppendLine("   ON BI.VFLAG1 = BG.BASCD                                                                            ");
                    //SQL.AppendLine("  AND BG.BSNSCLS = '기록지관리'                                                                        ");
                    //SQL.AppendLine("  AND BG.UNITCLS IN ('임상관찰그룹', '섭취배설그룹', '특수치료그룹', '기본간호그룹')                          ");
                    //SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB                                                                     ");
                    //SQL.AppendLine("   ON BG.VFLAG1 = BB.BASCD                                                                            ");
                    //SQL.AppendLine("  AND BB.BSNSCLS = '기록지관리'                                                                        ");
                    //SQL.AppendLine("  AND BB.UNITCLS = '임상관찰그룹정렬'                                                                   ");
                    #endregion

                    SQL.AppendLine("    C.FORMNO, C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.CHARTUSEID,         ");
                    SQL.AppendLine("	 C.WRITEDATE, C.WRITETIME,                                          ");
                    SQL.AppendLine("    DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN,                            ");
                    SQL.AppendLine("    B.BGRPNAME,                                                        ");
                    SQL.AppendLine("    B.GRPNAME,                                                         ");
                    SQL.AppendLine("    R.ITEMCD,                                                          ");
                    SQL.AppendLine("    B.ITEMNAME,                                                        ");
                    SQL.AppendLine("    R.ITEMVALUE || R.ITEMVALUE1 AS ITEMVALUE,                          ");
                    SQL.AppendLine("    U.NAME  AS USENAME                                                 ");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C                                         ");
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "EMR_USERT U                                      ");
                    SQL.AppendLine("   ON C.CHARTUSEID = U.USERID                                          ");
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R                                   ");
                    SQL.AppendLine("   ON C.EMRNO = R.EMRNO                                                ");
                    SQL.AppendLine("  AND C.EMRNOHIS = R.EMRNOHIS                                          ");
                    SQL.AppendLine("  AND R.ITEMVALUE > CHR(0)                                             ");
                    SQL.AppendLine("INNER JOIN (                                                           ");
                    SQL.AppendLine("    SELECT                                                             ");
                    SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                    SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                    SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                    SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                    SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                    SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                    SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                    SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                    SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                    SQL.AppendLine("        AND BB.UNITCLS IN ('임상관찰그룹')                                  ");
                    SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                    SQL.AppendLine("        AND B.UNITCLS IN ('임상관찰')                                     ");
                    SQL.AppendLine("    UNION ALL                                                          ");
                    SQL.AppendLine("    SELECT                                                             ");
                    SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                    SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                    SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                    SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                    SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                    SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                    SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                    SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                    SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                    SQL.AppendLine("        AND BB.UNITCLS IN ('섭취배설그룹')                                  ");
                    SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                    SQL.AppendLine("        AND B.UNITCLS IN ('섭취배설')                                     ");
                    SQL.AppendLine("    UNION ALL                                                          ");
                    SQL.AppendLine("    SELECT                                                             ");
                    SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                    SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                    SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                    SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                    SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                    SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                    SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                    SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                    SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                    SQL.AppendLine("        AND BB.UNITCLS IN ('특수치료그룹')                                  ");
                    SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                    SQL.AppendLine("        AND B.UNITCLS IN ('특수치료')                                     ");
                    SQL.AppendLine("    UNION ALL                                                          ");
                    SQL.AppendLine("    SELECT                                                             ");
                    SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                    SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                    SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                    SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                    SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                    SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                    SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                    SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                    SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                    SQL.AppendLine("        AND BB.UNITCLS IN ('기본간호그룹')                                  ");
                    SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                    SQL.AppendLine("        AND B.UNITCLS IN ('기본간호')                                     ");
                    SQL.AppendLine("    ) B                                                                ");
                    SQL.AppendLine("    ON R.ITEMCD = B.ITEMCD                                             ");
                }
                else if (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049)//간호기록
                {
                    #region 이전 기록지
                    SQL.AppendLine("      C.FORMNO, C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.USEID AS CHARTUSEID, C.WRITEDATE, C.WRITETIME, --C.CHARTXML,  ");
                    SQL.AppendLine("     '' AS ITEMCD, '' AS ITEMNO, '' AS ITEMINDEX, '' AS ITEMTYPE , EXTRACTVALUE(C.CHARTXML,'//ta2') AS ITEMVALUE,");
                    SQL.AppendLine("     CASE WHEN C.FORMNO <> 2049 THEN EXTRACTVALUE(C.CHARTXML,'//ta3') END WARDCODE,");
                    SQL.AppendLine("     CASE WHEN C.FORMNO <> 2049 THEN EXTRACTVALUE(C.CHARTXML,'//ta4') END ROOMCODE,");
                    SQL.AppendLine("     '' AS PROBLEMCODE,");
                    SQL.AppendLine("     CASE WHEN C.FORMNO = 965 THEN EXTRACTVALUE(C.CHARTXML,'//ta1') END PROBLEM,");
                    SQL.AppendLine("     CASE WHEN C.FORMNO = 965 THEN EXTRACTVALUE(C.CHARTXML,'//it1') END TYPE,");
                    if (pForm.FmFORMNO == 2049)
                    {
                        SQL.AppendLine("     (SELECT NAME FROM " + ComNum.DB_PMPA + "BAS_PASS WHERE IDNUMBER = TO_NUMBER(C.USEID) AND PROGRAMID = '        ') AS USENAME,");
                    }
                    else
                    {
                        SQL.AppendLine("     (SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS USENAME,");
                    }
                    SQL.AppendLine("     CASE WHEN EXISTS(SELECT 1");
                    SQL.AppendLine("       FROM " + ComNum.DB_EMR + "EMRPRTREQ ");
                    SQL.AppendLine("       WHERE EMRNO = C.EMRNO ");
                    SQL.AppendLine("         AND SCANYN  = 'T'");
                    SQL.AppendLine("         AND PRINTYN = 'Y'");
                    SQL.AppendLine("     ) THEN '사 본' ELSE NULL END PRNTYN");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "EMRXML C ");
                    SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" || (pAcp.medDeptCd == "MD" && (pAcp.medDrCd == "1107" || pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("  AND C.MEDDEPTCD = 'MD'");
                            SQL.AppendLine("  AND C.MEDDRCD IN ('1107','1125')");
                        }
                        else
                        {
                            if (pAcp.medDeptCd != "HD")
                            {
                                SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                                SQL.AppendLine("  AND C.MEDDRCD NOT IN ('1107','1125')");
                            }
                        }

                        SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "' ");
                        SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "' ");
                        //ER 간호기록일 경우(ER에 2일 이상 있는경우 있어서 수정)
                        if (pForm.FmFORMNO == 2049)
                        {
                            SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
                        }
                    }
                    else
                    {
                        SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
                        SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "' ");
                        SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "' ");

                        if (strInOutCls == "O")
                        {
                            SQL.AppendLine("  AND C.INOUTCLS IN ('O') ");
                        }
                        else
                        {
                            SQL.AppendLine("  AND C.INOUTCLS IN ('I') ");
                        }
                    }

                    if (pAcp.inOutCls == "O" && pAcp.medDeptCd == "HD")
                    {
                        SQL.AppendLine("  AND C.INOUTCLS IN ('O','I') ");
                    }
                    else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "HD")
                    {
                        SQL.AppendLine("  AND C.INOUTCLS IN ('O') ");
                    }
                    #endregion


                    SQL.AppendLine("UNION ALL");
                    SQL.AppendLine("SELECT ROWNUM AS RNUM,");
                    SQL.AppendLine(" C.FORMNO, C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
                    SQL.AppendLine("    '' AS ITEMCD, '' AS ITEMNO, '' AS ITEMINDEX, '' AS ITEMTYPE , B.NRRECODE AS ITEMVALUE,");
                    SQL.AppendLine("    B.WARDCODE, B.ROOMCODE, B.PROBLEMCODE, B.PROBLEM, B.TYPE,");
                    if (pForm.FmFORMNO == 2049)
                    {
                        SQL.AppendLine("     (SELECT NAME FROM " + ComNum.DB_PMPA + "BAS_PASS WHERE IDNUMBER = TO_NUMBER(C.CHARTUSEID) AND PROGRAMID = '        ') AS USENAME");
                    }
                    else
                    {
                        SQL.AppendLine("     (SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE USERID = C.CHARTUSEID ) AS USENAME");
                    }

                    //SQL.AppendLine("    U.NAME AS USENAME");
                    SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRNURSRECORD B");
                    SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                    SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                    //SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                    //SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
                }
                else if (pForm.FmFORMNO == 1575) //간호활동
                {
                    SQL.AppendLine(" '1575' AS FORMNO, C.EMRNO, C.EMRNOHIS, C.CHARTUSEID, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.WRITEDATE, C.WRITETIME, DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN,");
                    SQL.AppendLine( "    R.ITEMCD, R.ITEMNO, R.ITEMINDEX, R.ITEMTYPE, (R.ITEMVALUE || R.ITEMVALUE1) AS ITEMVALUE,  ");
                    SQL.AppendLine("     B.BASEXNAME AS BGRPNAME, B.BASNAME AS GRPNAME, U.NAME AS USENAME  ");
                    SQL.AppendLine( "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST C  ");
                    SQL.AppendLine( "  INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R  ");
                    SQL.AppendLine( "     ON C.EMRNO = R.EMRNO  ");
                    SQL.AppendLine( "    AND C.EMRNOHIS = R.EMRNOHIS ");
                    SQL.AppendLine( "  INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B ");
                    SQL.AppendLine( "     ON R.ITEMCD = B.BASCD ");
                    SQL.AppendLine( "    AND B.BSNSCLS = '기록지관리' ");
                    SQL.AppendLine( "    AND B.UNITCLS = '간호활동항목' ");
                    SQL.AppendLine( "  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U  ");
                    SQL.AppendLine( "     ON U.USERID = C.CHARTUSEID  ");
                }
                else
                {
                    if (pForm.FmFORMNO == 963)
                    {
                        SQL.AppendLine("     C.FORMNO, C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.USEID AS CHARTUSEID, C.WRITEDATE, C.WRITETIME  -- C.CHARTXML,  ");
                        SQL.AppendLine("     ,'I0000000981' AS ITEMCD, 'I0000000981' AS ITEMNO, -1 AS ITEMINDEX, 'TEXT' AS ITEMTYPE ");
                        SQL.AppendLine("     ,EXTRACT(C.CHARTXML, '//ta1').GETCLOBVAL()  AS ITEMVALUE");
                        SQL.AppendLine("     ,(SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS USENAME");
                        SQL.AppendLine("     ,CASE WHEN EXISTS(SELECT 1");
                        SQL.AppendLine("       FROM " + ComNum.DB_EMR + "EMRPRTREQ ");
                        SQL.AppendLine("       WHERE EMRNO = C.EMRNO ");
                        SQL.AppendLine("         AND SCANYN  = 'T'");
                        SQL.AppendLine("         AND PRINTYN = 'Y'");
                        SQL.AppendLine("     ) THEN '사 본' ELSE NULL END PRNTYN");
                        SQL.AppendLine("     ,'OLD' AS GBN");
                        SQL.AppendLine("FROM " + ComNum.DB_EMR + "EMRXML C ");
                        SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                        SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);

                        if (pAcp.inOutCls.Equals("O"))
                        {
                            SQL.AppendLine("  AND C.INOUTCLS = 'O'");
                            SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "' ");
                            SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "' ");
                        }
                        else
                        {
                            SQL.AppendLine("  AND C.INOUTCLS = 'I'");
                            SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
                            SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "' ");
                            SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "' ");
                        }


                        SQL.AppendLine("UNION ALL");
                        SQL.AppendLine("SELECT ROWNUM AS RNUM,");
                        SQL.AppendLine("    C.FORMNO, C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
                        SQL.AppendLine("    B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, TO_CLOB(B.ITEMVALUE) AS ITEMVALUE,");
                        SQL.AppendLine("    U.NAME AS USENAME");
                        SQL.AppendLine("    , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
                        SQL.AppendLine("    ,'NEW' AS GBN");
                        SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
                        SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
                        SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                        SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                        SQL.AppendLine("    AND B.ITEMCD > CHR(0)");
                        SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                        SQL.AppendLine("     ON U.USERID = C.CHARTUSEID");
                    }
                    else
                    {
                        SQL.AppendLine(" C.FORMNO, C.EMRNO, TRIM(C.CHARTDATE) CHARTDATE, TRIM(C.CHARTTIME) CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
                        SQL.AppendLine("                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,");
                        SQL.AppendLine("                U.NAME AS USENAME");
                        SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
                        SQL.AppendLine("     ,'NEW' AS GBN");
                        SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
                        SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
                        SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                        SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                        SQL.AppendLine("    AND B.ITEMCD > CHR(0)");
                        SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                        SQL.AppendLine("     ON U.USERID = C.CHARTUSEID");
                    }
                   
                }

                if (pAcp.inOutCls == "I")
                {
                    SQL.AppendLine("WHERE C.MEDFRDATE = '" + pAcp.medFrDate + "'");
                    //SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                    SQL.AppendLine("  AND C.PTNO      = '" + pAcp.ptNo + "'");
                    SQL.AppendLine("  AND C.INOUTCLS = 'I'");
                }
                else
                {
                    SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "'");
                    if (pAcp.medDeptCd != "HD")
                    {
                        SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                        SQL.AppendLine("  AND C.MEDDRCD NOT IN ('1107','1125')");
                    }
                    //ER 간호기록일 경우(ER에 2일 이상 있는경우 있어서 수정)
                    if (pForm.FmFORMNO == 2049)
                    {
                        SQL.AppendLine("  AND C.MEDFRDATE = '" + pAcp.medFrDate + "' ");
                    }

                    SQL.AppendLine("  AND C.INOUTCLS = 'O'");
                }

                SQL.AppendLine( "  AND C.FORMNO = " + pForm.FmFORMNO);
                SQL.AppendLine( "  AND C.UPDATENO = " + pForm.FmUPDATENO);
                SQL.AppendLine( "  AND C.CHARTDATE >= '" + strFrDate + "'");
                SQL.AppendLine( "  AND C.CHARTDATE <= '" + strEndDate + "'   ");
                #endregion
            }

            if (ViewNpChart == false)
            {
                SQL.AppendLine("  AND C.MEDDEPTCD <> 'NP'");
            }

            if (pForm.FmFORMNO == 963 || pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049)
            {
                if (chkAsc == true)
                {
                    SQL.AppendLine("ORDER BY CHARTDATE, CHARTTIME, EMRNO");
                }
                else
                {
                    SQL.AppendLine("ORDER BY CHARTDATE DESC, CHARTTIME DESC, EMRNO DESC");
                }
            }
            else
            {
                if (chkAsc == true)
                {
                    SQL.AppendLine("ORDER BY CHARTDATE, CHARTTIME, C.EMRNO");
                }
                else
                {
                    SQL.AppendLine("ORDER BY CHARTDATE DESC, CHARTTIME DESC, C.EMRNO DESC");
                }
            }

      

            if  (pForm.FmOLDGB != 1 &&
                (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 2135 || pForm.FmFORMNO == 1935 ||
                 pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || pForm.FmFORMNO == 2201))
            {
                //SQL.AppendLine(", BB.DISSEQNO, BG.DISSEQNO, BI.NFLAG1");
                SQL.AppendLine("         , B.SORT1, B.SORT2, B.SORT3, B.SORT4                          ");
            }
            else if (pForm.FmFORMNO == 1575 && pForm.FmOLDGB != 1)
            {
                SQL.AppendLine(", BGRPNAME, B.NFLAG1, B.NFLAG2, R.DSPSEQ  ");
            }


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }



        /// <summary>
        /// 수정 내역을 표시한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="pForm"></param>
        /// <param name="HisNo"></param>
        /// <param name="chkAsc"></param>
        public static DataTable QueryHisSpdList(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm, string EmrNo, bool chkAsc = true)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return null;

            SQL.AppendLine("SELECT ROWNUM AS RNUM,");

         
            #region NEW
            //mstrFormNo == "2135" || mstrFormNo == "1935" || mstrFormNo == "2431" || mstrFormNo == "1969" || mstrFormNo == "2201"
            if (pForm.FmFORMNO == 1232)
            {
                SQL.AppendLine("     C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME");
                SQL.AppendLine("     , (SELECT IMAGE FROM  kosmos_emr.AEMRCHARTDRAWHIS WHERE EMRNO = C.EMRNO AND EMRNOHIS = C.EMRNOHIS AND FORMNO = 1232) as IMAGEVALUE");
                SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN ");
                SQL.AppendLine("     , U.NAME AS USENAME");
                SQL.AppendLine("     , 'ImageCellType' AS ItemType");
                SQL.AppendLine("     , 'I0000029770' AS ITEMCD, 'progress' AS ITEMNAME, C.FORMNO");
                SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS C ");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
            }
            //투약기록지
            else if (pForm.FmFORMNO == 1796)
            {
                SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME");
                SQL.AppendLine("     , B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE");
                SQL.AppendLine("     , U.NAME AS USENAME");
                SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN ");
                SQL.AppendLine("     , (SELECT BUN FROM KOSMOS_OCS.OCS_ORDERCODE WHERE TRIM(SUCODE) = B2.ITEMVALUE AND ROWNUM = 1) AS BUN");
                SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS C ");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROWHIS B");
                SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("    AND B.ITEMCD > CHR(0)");
                SQL.AppendLine("   INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROWHIS B2 -- 처방코드");
                SQL.AppendLine("     ON B2.EMRNO = C.EMRNO");
                SQL.AppendLine("    AND B2.EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("    AND B2.ITEMCD  = 'I0000037685'");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
            }

            else if (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 2135 || pForm.FmFORMNO == 1935 ||
                        pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || pForm.FmFORMNO == 2201)
            //임상관찰, 회복실, Angio
            ////진정 환자 평가, 응급실 SPECIAL WATCH RECORD, 인공신장실 V/S
            {

                SQL.AppendLine("    C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID,         ");
                SQL.AppendLine("	 C.WRITEDATE, C.WRITETIME,                                          ");
                SQL.AppendLine("    DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN,                            ");
                SQL.AppendLine("    B.BGRPNAME,                                                        ");
                SQL.AppendLine("    B.GRPNAME,                                                         ");
                SQL.AppendLine("    R.ITEMCD,                                                          ");
                SQL.AppendLine("    B.ITEMNAME,                                                        ");
                SQL.AppendLine("    R.ITEMVALUE || R.ITEMVALUE1 AS ITEMVALUE,                          ");
                SQL.AppendLine("    U.NAME  AS USENAME                                                 ");
                SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS C                                         ");
                SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "EMR_USERT U                                      ");
                SQL.AppendLine("   ON C.CHARTUSEID = U.USERID                                          ");
                SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROWHIS R                                   ");
                SQL.AppendLine("   ON C.EMRNO = R.EMRNO                                                ");
                SQL.AppendLine("  AND C.EMRNOHIS = R.EMRNOHIS                                          ");
                SQL.AppendLine("  AND R.ITEMVALUE > CHR(0)                                             ");
                SQL.AppendLine("INNER JOIN (                                                           ");
                SQL.AppendLine("    SELECT                                                             ");
                SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                SQL.AppendLine("        AND BB.UNITCLS IN ('임상관찰그룹')                                  ");
                SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                SQL.AppendLine("        AND B.UNITCLS IN ('임상관찰')                                     ");
                SQL.AppendLine("    UNION ALL                                                          ");
                SQL.AppendLine("    SELECT                                                             ");
                SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                SQL.AppendLine("        AND BB.UNITCLS IN ('섭취배설그룹')                                  ");
                SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                SQL.AppendLine("        AND B.UNITCLS IN ('섭취배설')                                     ");
                SQL.AppendLine("    UNION ALL                                                          ");
                SQL.AppendLine("    SELECT                                                             ");
                SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                SQL.AppendLine("        AND BB.UNITCLS IN ('특수치료그룹')                                  ");
                SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                SQL.AppendLine("        AND B.UNITCLS IN ('특수치료')                                     ");
                SQL.AppendLine("    UNION ALL                                                          ");
                SQL.AppendLine("    SELECT                                                             ");
                SQL.AppendLine("        B.BASCD AS ITEMCD, B.BASNAME AS ITEMNAME,                      ");
                SQL.AppendLine("        B.BASEXNAME AS GRPNAME, BB.BASNAME AS BGRPNAME,                ");
                SQL.AppendLine("        BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2,                      ");
                SQL.AppendLine("        B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4,                        ");
                SQL.AppendLine("        B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6                       ");
                SQL.AppendLine("    FROM  " + ComNum.DB_EMR + "AEMRBASCD B                                       ");
                SQL.AppendLine("    INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD BB                                ");
                SQL.AppendLine("        ON B.VFLAG1 = BB.BASCD                                         ");
                SQL.AppendLine("        AND BB.BSNSCLS = '기록지관리'                                      ");
                SQL.AppendLine("        AND BB.UNITCLS IN ('기본간호그룹')                                  ");
                SQL.AppendLine("    WHERE B.BSNSCLS = '기록지관리'                                         ");
                SQL.AppendLine("        AND B.UNITCLS IN ('기본간호')                                     ");
                SQL.AppendLine("    ) B                                                                ");
                SQL.AppendLine("    ON R.ITEMCD = B.ITEMCD                                             ");
            }
            else if (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049)//간호기록
            {
                SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
                SQL.AppendLine("    '' AS ITEMCD, '' AS ITEMNO, '' AS ITEMINDEX, '' AS ITEMTYPE , B.NRRECODE AS ITEMVALUE,");
                SQL.AppendLine("    B.WARDCODE, B.ROOMCODE, B.PROBLEMCODE, B.PROBLEM, B.TYPE,");
                SQL.AppendLine("    U.NAME AS USENAME");
                SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
                SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS C");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRNURSRECORDHIS B");
                SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
            }
            else if (pForm.FmFORMNO == 1575) //간호활동
            {
                SQL.AppendLine(" '1575' AS FORMNO, C.EMRNO, C.EMRNOHIS, C.CHARTUSEID, C.CHARTDATE, C.CHARTTIME, C.WRITEDATE, C.WRITETIME, DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN,");
                SQL.AppendLine("    R.ITEMCD, R.ITEMNO, R.ITEMINDEX, R.ITEMTYPE, (R.ITEMVALUE || R.ITEMVALUE1) AS ITEMVALUE,  ");
                SQL.AppendLine("     B.BASEXNAME AS BGRPNAME, B.BASNAME AS GRPNAME, U.NAME AS USENAME  ");
                SQL.AppendLine("FROM  " + ComNum.DB_EMR + "AEMRCHARTMSTHIS C  ");
                SQL.AppendLine("  INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROWHIS R  ");
                SQL.AppendLine("     ON C.EMRNO = R.EMRNO  ");
                SQL.AppendLine("    AND C.EMRNOHIS = R.EMRNOHIS ");
                SQL.AppendLine("  INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B ");
                SQL.AppendLine("     ON R.ITEMCD = B.BASCD ");
                SQL.AppendLine("    AND B.BSNSCLS = '기록지관리' ");
                SQL.AppendLine("    AND B.UNITCLS = '간호활동항목' ");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U  ");
                SQL.AppendLine("     ON U.USERID = C.CHARTUSEID  ");
            }
            else
            {
                SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
                SQL.AppendLine("                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,");
                SQL.AppendLine("                U.NAME AS USENAME");
                SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN");
                SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS C");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROWHIS B");
                SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("    AND B.ITEMCD > CHR(0)");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                SQL.AppendLine("     ON U.USERID = C.CHARTUSEID");
            }

            SQL.AppendLine("WHERE C.EMRNO = " + EmrNo);
            SQL.AppendLine("  AND C.EMRNOHIS > 0");
            #endregion

            if (chkAsc == true)
            {
                SQL.AppendLine("ORDER BY C.WRITEDATE, C.WRITETIME, C.EMRNO");
            }
            else
            {
                SQL.AppendLine("ORDER BY C.WRITEDATE DESC, C.WRITETIME DESC, C.EMRNO DESC");
            }

            if  (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 2135 || pForm.FmFORMNO == 1935 ||
                 pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || pForm.FmFORMNO == 2201)
            {
                //SQL.AppendLine(", BB.DISSEQNO, BG.DISSEQNO, BI.NFLAG1");
                SQL.AppendLine("         , B.SORT1, B.SORT2, B.SORT3, B.SORT4                          ");
            }
            else if (pForm.FmFORMNO == 1575 && pForm.FmOLDGB != 1)
            {
                SQL.AppendLine(",B.NFLAG1, B.NFLAG2, R.DSPSEQ  ");
            }


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 내용을 스프래드에 표시한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="p"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="ViewNpChart">정신과 여부</param>
        /// <param name="chkAsc"></param>
        /// <param name="strSearchCode">투약기록지 검색용</param>
        public static DataTable QuerySpdListTuYak(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm, string strFrDate, string strEndDate,
            string strInOutCls, bool ViewNpChart, bool chkAsc = true, string strSearchCode = "")
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pAcp == null) return null;

            SQL.AppendLine("SELECT ROWNUM AS RNUM,");

            if (pForm.FmFORMNO == 1796)
            {
                SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME");
                SQL.AppendLine("     , B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE");
                SQL.AppendLine("     , U.NAME AS USENAME");
                SQL.AppendLine("     , DECODE(C.PRNTYN, 'Y', '사 본') AS PRNTYN ");
                SQL.AppendLine("     , (SELECT BUN FROM KOSMOS_OCS.OCS_ORDERCODE WHERE TRIM(SUCODE) = B2.ITEMVALUE AND ROWNUM = 1) AS BUN");
                SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ");
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
                SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                SQL.AppendLine("    AND B.ITEMCD > CHR(0)");
                SQL.AppendLine("   INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B2 -- 처방코드");
                SQL.AppendLine("     ON B2.EMRNO = C.EMRNO");
                SQL.AppendLine("    AND B2.EMRNOHIS = C.EMRNOHIS");
                //SQL.AppendLine("    AND B2.ITEMCD  = 'I0000037685'");
                SQL.AppendLine("    AND B2.ITEMCD  IN ('I0000006552','I0000037685')");
                if (!string.IsNullOrWhiteSpace(strSearchCode))
                {
                    SQL.AppendLine("    AND UPPER(B2.ITEMVALUE) LIKE '%" + strSearchCode.ToUpper() + "%'");
                    
                }
                SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
            }

            if (pAcp.inOutCls == "I")
            {
                SQL.AppendLine("WHERE C.MEDFRDATE = '" + pAcp.medFrDate + "'");
                SQL.AppendLine("  AND C.PTNO      = '" + pAcp.ptNo + "'");
                SQL.AppendLine("  AND C.INOUTCLS = 'I'");
            }
            else
            {
                SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "'");
                if (pAcp.medDeptCd != "HD")
                {
                    SQL.AppendLine("  AND C.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                    SQL.AppendLine("  AND C.MEDDRCD NOT IN ('1107','1125')");
                }

                SQL.AppendLine("  AND C.INOUTCLS = 'O'");
                SQL.AppendLine("  AND C.CHARTDATE >= '" + strFrDate + "'");
                SQL.AppendLine("  AND C.CHARTDATE <= '" + strEndDate + "'   ");
            }

            SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
            SQL.AppendLine("  AND C.UPDATENO = " + pForm.FmUPDATENO);
    

            if (ViewNpChart == false)
            {
                SQL.AppendLine("  AND C.MEDDEPTCD <> 'NP'");
            }

            if (chkAsc == true)
            {
                SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE), TRIM(C.CHARTTIME), C.EMRNO");
            }
            else
            {
                SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE) DESC, TRIM(C.CHARTTIME) DESC, C.EMRNO DESC");
            }


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }


        /// <summary>
        /// 내용을 스프래드에 표시한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="p"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="ssWrite"></param>
        /// <param name="ssList"></param>
        /// <param name="strDirection"></param>
        /// <param name="intHeadRow"></param>
        /// <param name="intHeadCol"></param>
        /// <param name="chkDesc"></param>
        public static void QuerySpdList(PsmhDb pDbCon, EmrPatient pAcp, string strFormNo, string strUpdateNo, string strFrDate, string strEndDate,
                                        //FarPoint.Win.Spread.FpSpread ssWrite, 
                                        FarPoint.Win.Spread.FpSpread ssList,
                                        string strDirection, int intHeadRow, int intHeadCol, FormFlowSheet[] mFormFlowSheet, bool chkDesc = true)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strDirection == "ROW")
            {
                ssList.ActiveSheet.ColumnCount = 0;
            }
            else
            {
                ssList.ActiveSheet.RowCount = 0;
            }

            if (pAcp == null) return;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.USEID AS CHARTUSEID, C.CHARTXML,  ";
            SQL = SQL + ComNum.VBLF + "     EXTRACTVALUE(C.CHARTXML,'//' || FX.ITEMNO) AS ITEMVALUE, '' AS PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "     U.USENAME ,  ";
            SQL = SQL + ComNum.VBLF + "     FX.ITEMNO AS ITEMCD, FX.ITEMNAME, FX.CELLTYPE AS ITEMTYPE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML C ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
            SQL = SQL + ComNum.VBLF + "    ON C.USEID = U.USEID ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "    ON C.FORMNO = F.FORMNO ";
            SQL = SQL + ComNum.VBLF + "    AND F.UPDATENO = " + strUpdateNo;
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRFLOWXML FX ";
            SQL = SQL + ComNum.VBLF + "    ON F.FORMNO = FX.FORMNO ";
            SQL = SQL + ComNum.VBLF + "    AND F.UPDATENO = FX.UPDATENO ";
            SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + pAcp.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "	AND C.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "	AND C.CHARTDATE >= '" + strFrDate + "' ";
            SQL = SQL + ComNum.VBLF + "	AND C.CHARTDATE <= '" + strEndDate + "' ";
            //SQL = SQL + ComNum.VBLF + "	AND C.EMRNO = 93760352 ";

            //if (chkDesc == true)
            //{
            //    SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) DESC, FX.ITEMNUMBER ";
            //}
            //else
            //{
            //    SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) , FX.ITEMNUMBER ";
            //}

            if (chkDesc == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY C.CHARTDATE DESC, C.CHARTTIME DESC, C.EMRNO DESC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY C.CHARTDATE, C.CHARTTIME, C.EMRNO";
            }


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            //int intStart = 3;
            string strEMRNO = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    if (strDirection == "ROW")
                    {
                        ssList.ActiveSheet.ColumnCount = ssList.ActiveSheet.ColumnCount + 1;
                        if ((int)ssList.ActiveSheet.Columns[ssList.ActiveSheet.ColumnCount - 1].Width < 80)
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, 80);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, (int)ssList.ActiveSheet.Columns[ssList.ActiveSheet.ColumnCount - 1].Width);
                        }

                    }
                    else
                    {
                        ssList.ActiveSheet.RowCount = ssList.ActiveSheet.RowCount + 1;
                        if ((int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height < 22)
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, 22);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, (int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height);
                        }
                    }
                }

                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                string strCHARTDATE = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");

                if (strDirection == "ROW")
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 0, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, strCHARTDATE, false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == mFormFlowSheet[j].ItemCode.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssList.ActiveSheet.Cells[2 + j, ssList.ActiveSheet.ColumnCount - 1].VerticalAlignment,
                                            ssList.ActiveSheet.Cells[2 + j, ssList.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            false);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 3, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 2, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }
                else
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 0, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, strCHARTDATE, false);
                    //clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == mFormFlowSheet[j].ItemCode.Trim()) //이부분 수정 
                        {
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 2 + j, clsXML.GetTypeEx(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 2 + j].VerticalAlignment,
                                            ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 2 + j].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            true);
                            ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height =
                            (int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].GetPreferredHeight() + 10;
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 4, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 3, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);

                }

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            ////////////int intStart = 3;
            ////////////string strEMRNO = "";

            ////////////for (i = 0; i < dt.Rows.Count; i++)
            ////////////{
            ////////////    if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
            ////////////    {
            ////////////        if (strDirection == "ROW")
            ////////////        {
            ////////////            ssList.ActiveSheet.ColumnCount = ssList.ActiveSheet.ColumnCount + 1;
            ////////////            if ((int)ssList.ActiveSheet.Columns[ssList.ActiveSheet.ColumnCount - 1].Width < 80)
            ////////////            {
            ////////////                ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, 80);
            ////////////            }
            ////////////            else
            ////////////            {
            ////////////                ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, (int)ssList.ActiveSheet.Columns[ssList.ActiveSheet.ColumnCount - 1].Width);
            ////////////            }

            ////////////        }
            ////////////        else
            ////////////        {
            ////////////            ssList.ActiveSheet.RowCount = ssList.ActiveSheet.RowCount + 1;
            ////////////            if ((int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height < 22)
            ////////////            {
            ////////////                ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, 22);
            ////////////            }
            ////////////            else
            ////////////            {
            ////////////                ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, (int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height);
            ////////////            }
            ////////////        }
            ////////////    }
            ////////////    strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

            ////////////    string strCHARTDATE = VB.Mid(dt.Rows[i]["CHARTDATE"].ToString(), 3, 2) + "/" + VB.Mid(dt.Rows[i]["CHARTDATE"].ToString(), 5, 2) + "/" + VB.Mid(dt.Rows[i]["CHARTDATE"].ToString(), 7, 2);

            ////////////    if (strDirection == "ROW")
            ////////////    {
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, 0, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, strCHARTDATE, false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
            ////////////        //for (int j = 0; j < ssWrite.ActiveSheet.RowCount; j++) //이부분 수정 
            ////////////        //{
            ////////////        //    if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[j, 0].Text.Trim()) //이부분 수정 
            ////////////        //    {
            ////////////        //        //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
            ////////////        //        clsSpread.SetTypeAndValue(ssList.ActiveSheet, 4 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
            ////////////        //                        ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].VerticalAlignment,
            ////////////        //                        ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
            ////////////        //                        dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
            ////////////        //                        false);
            ////////////        //    }
            ////////////        //}
            ////////////        for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
            ////////////        {
            ////////////            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == mFormFlowSheet[j].ItemCode.Trim()) //이부분 수정 
            ////////////            {
            ////////////                clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
            ////////////                                ssList.ActiveSheet.Cells[3 + j, ssList.ActiveSheet.ColumnCount - 1].VerticalAlignment,
            ////////////                                ssList.ActiveSheet.Cells[3 + j, ssList.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
            ////////////                                dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
            ////////////                                false);
            ////////////            }
            ////////////        }
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 3, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 2, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
            ////////////    }
            ////////////    else
            ////////////    {
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 0, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, strCHARTDATE, false);
            ////////////        //clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
            ////////////        //for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
            ////////////        //{
            ////////////        //    if (dt.Rows[i]["ITEMCD"].ToString().Trim() == mFormFlowSheet[j].ItemCode.Trim()) //이부분 수정 
            ////////////        //    {
            ////////////        //        //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
            ////////////        //        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 3 + j, clsXML.GetTypeEx(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
            ////////////        //                        ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1,  j].VerticalAlignment,
            ////////////        //                        ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1,  j].HorizontalAlignment,
            ////////////        //                        dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
            ////////////        //                        true);
            ////////////        //        ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height =
            ////////////        //        (int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].GetPreferredHeight() + 10;
            ////////////        //    }
            ////////////        //}
            ////////////        for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
            ////////////        {
            ////////////            if (dt.Rows[i]["ITEMCD"].ToString().Trim() == mFormFlowSheet[j].ItemCode.Trim()) //이부분 수정 
            ////////////            {
            ////////////                //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
            ////////////                clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 3 + j, clsXML.GetTypeEx(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
            ////////////                                ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 3 + j].VerticalAlignment,
            ////////////                                ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 3 + j].HorizontalAlignment,
            ////////////                                dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
            ////////////                                true);
            ////////////                ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height =
            ////////////                (int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].GetPreferredHeight() + 10;
            ////////////            }
            ////////////        }
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 3, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
            ////////////        clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);

            ////////////    }

            ////////////}

            ////////////dt.Dispose();
            ////////////dt = null;
            ////////////Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// 내용을 스프래드에 표시한다(PRTREQNO)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strPrtSeq"></param>
        /// <param name="ViewNpChart">정신과 여부</param>
        /// <param name="chkAsc"></param>
        public static DataTable QuerySpdList2(PsmhDb pDbCon, EmrPatient pAcp, EmrForm pForm, string mstrDeptCode, string strEmrNo, string strPrtSeq, bool ViewNpChart, bool chkAsc = true)
        {
            StringBuilder SQL = new StringBuilder();
            DataTable dt = null;
            DateTime dtpSys = ComQuery.CurrentDateTime(pDbCon, "S").To<DateTime>();

            Cursor.Current = Cursors.WaitCursor;

            SQL.AppendLine("SELECT ");
            if (pForm.FmOLDGB == 1)
            {
                #region XML
                if (pForm.FmFORMNO == 963 || pForm.FmFORMNO == 1232)
                {
                    SQL.AppendLine("     C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.USEID AS CHARTUSEID, C.WRITEDATE, C.WRITETIME,  -- C.CHARTXML,  ");
                    SQL.AppendLine("     CASE WHEN f.FORMNO = 963 THEN EXTRACTVALUE(C.CHARTXML,'//ta1') END ITEMVALUE, ");
                    SQL.AppendLine("     CASE WHEN F.FORMNO = 1232 THEN  (SELECT EMRIMAGEMERGE FROM  kosmos_emr.EMRXMLIMAGES WHERE EMRNO = C.EMRNO) ELSE NULL");
                    SQL.AppendLine("     END  IMAGEVALUE, ");
                    SQL.AppendLine("     '' AS PRNTYN, ");
                    SQL.AppendLine("     (SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS USENAME,");
                    SQL.AppendLine("     CASE WHEN F.FORMNO = 963 THEN 'TextCellType'  ELSE 'ImageCellType' END ItemType,");
                    SQL.AppendLine("     CASE WHEN F.FORMNO = 963 THEN 'ta1' ELSE 'im1' END ITEMCD, 'progress' AS ITEMNAME, F.FORMNO");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "EMRXML C ");
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ");
                    SQL.AppendLine("   ON C.FORMNO = F.FORMNO ");
                    SQL.AppendLine("  AND F.UPDATENO = " + pForm.FmUPDATENO);
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "EMRPRTREQ E");
                    SQL.AppendLine("   ON C.EMRNO = E.EMRNO");
                    SQL.AppendLine("  AND E.PRTREQNO = " + strPrtSeq);
                    SQL.AppendLine("  AND E.SCANYN = 'T'");
                    SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                }
                else
                {
                   SQL.AppendLine("     C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.USEID AS CHARTUSEID, C.WRITEDATE, C.WRITETIME, C.FORMNO,  -- C.CHARTXML,  ");
                   SQL.AppendLine("     EXTRACTVALUE(C.CHARTXML,'//' || FX.ITEMNO) AS ITEMVALUE, '' AS PRNTYN, ");
                    //SQL.AppendLine("     U.USENAME ,  ");
                    //SQL.AppendLine("     (SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE LPAD(USERID, 5, '0') = LPAD(C.USEID, 5, '0')) AS USENAME,");
                   SQL.AppendLine("     (SELECT NAME FROM " + ComNum.DB_EMR + "EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS USENAME,");
                   SQL.AppendLine("     FX.ITEMNO AS ITEMCD, FX.ITEMNAME, FX.CELLTYPE AS ITEMTYPE ");
                   SQL.AppendLine("FROM " + ComNum.DB_EMR + "EMRXML C ");
                   //SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ");
                   //SQL.AppendLine("    ON LPAD(C.USEID, 5, '0') = LPAD(U.USEID, 5, '0')");
                    //SQL.AppendLine("   ON C.USEID = U.USEID ");
                   SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ");
                   SQL.AppendLine("   ON C.FORMNO = F.FORMNO ");
                   SQL.AppendLine("  AND F.UPDATENO = " + pForm.FmUPDATENO);
                   SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRFLOWXML FX ");
                   SQL.AppendLine("   ON F.FORMNO = FX.FORMNO ");
                   SQL.AppendLine("  AND F.UPDATENO = FX.UPDATENO ");
                   SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "EMRPRTREQ E");
                   SQL.AppendLine("   ON C.EMRNO = E.EMRNO");
                   SQL.AppendLine("  AND E.PRTREQNO = " + strPrtSeq);
                   SQL.AppendLine("  AND E.SCANYN = 'T'");
                   SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                   SQL.AppendLine("	 AND C.FORMNO = " + pForm.FmFORMNO);
                   SQL.AppendLine("	 AND C.MEDDEPTCD = '" + mstrDeptCode +  "'");
                }
                #endregion
            }
            else
            {
                #region NEW
                //mstrFormNo == "2135" || mstrFormNo == "1935" || mstrFormNo == "2431" || mstrFormNo == "1969" || mstrFormNo == "2201"
                if (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 2135 || pForm.FmFORMNO == 1935 ||
                    pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || pForm.FmFORMNO == 2201)
                //임상관찰, 회복실, Angio
                ////진정 환자 평가, 응급실 SPECIAL WATCH RECORD, 인공신장실 V/S
                {
                    #region 이전
                    if (dtpSys.Date < ("2020-09-22").To<DateTime>())
                    {
                        SQL.AppendLine("  C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, '' AS PRNTYN, ");
                        SQL.AppendLine("    BB.BASNAME AS BGRPNAME,                                                                            ");
                        SQL.AppendLine("    BG.BASNAME AS GRPNAME,                                                                             ");
                        SQL.AppendLine("    R.ITEMCD,                                                                                          ");
                        SQL.AppendLine("    BI.BASNAME AS ITEMNAME,                                                                            ");
                        SQL.AppendLine("    R.ITEMVALUE || R.ITEMVALUE1 AS ITEMVALUE,                                                          ");
                        SQL.AppendLine("	U.NAME AS USENAME                                                                                             ");
                        SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C                                                                         ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "EMR_USERT U                                                                      ");
                        SQL.AppendLine("   ON C.CHARTUSEID = U.USERID                                                                         ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R                                                                   ");
                        SQL.AppendLine("   ON C.EMRNO = R.EMRNO                                                                               ");
                        SQL.AppendLine("  AND C.EMRNOHIS = R.EMRNOHIS                                                                        ");
                        SQL.AppendLine("  AND R.ITEMVALUE > CHR(0)");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BI                                                                     ");
                        SQL.AppendLine("   ON R.ITEMCD = BI.BASCD                                                                             ");
                        SQL.AppendLine("  AND BI.BSNSCLS = '기록지관리'                                                                        ");
                        SQL.AppendLine("  AND BI.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')                                       ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG                                                                     ");
                        SQL.AppendLine("   ON BI.VFLAG1 = BG.BASCD                                                                            ");
                        SQL.AppendLine("  AND BG.BSNSCLS = '기록지관리'                                                                        ");
                        SQL.AppendLine("  AND BG.UNITCLS IN ('임상관찰그룹', '섭취배설그룹', '특수치료그룹', '기본간호그룹')                          ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB                                                                     ");
                        SQL.AppendLine("   ON BG.VFLAG1 = BB.BASCD                                                                            ");
                        SQL.AppendLine("  AND BB.BSNSCLS = '기록지관리'                                                                        ");
                        SQL.AppendLine("  AND BB.UNITCLS = '임상관찰그룹정렬'                                                                   ");
                        SQL.AppendLine("INNER JOIN  " + ComNum.DB_EMR + "EMRPRTREQ P");
                        SQL.AppendLine("   ON  P.PRTREQNO = " + strPrtSeq);
                        SQL.AppendLine("  AND  P.EMRNO = R.EMRNO");
                        SQL.AppendLine("  AND  P.SCANYN = 'T'");
                        SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                        SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                        SQL.AppendLine("  AND C.MEDDEPTCD = '" + mstrDeptCode + "'");
                    }
                    #endregion
                    #region 신규
                    else
                    {
                        SQL.Clear();
                        SQL.AppendLine("WITH M AS ");
                        SQL.AppendLine("(");
                        SQL.AppendLine(" SELECT");
                        SQL.AppendLine("    C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, '' AS PRNTYN, ");
                        SQL.AppendLine("    BB.BASNAME AS BGRPNAME,                                                                            ");
                        SQL.AppendLine("    BG.BASNAME AS GRPNAME,                                                                             ");
                        SQL.AppendLine("    R.ITEMCD,                                                                                          ");
                        SQL.AppendLine("    BI.BASNAME AS ITEMNAME,                                                                            ");
                        SQL.AppendLine("    R.ITEMVALUE || R.ITEMVALUE1 AS ITEMVALUE,                                                          ");
                        SQL.AppendLine("	U.NAME AS USENAME                                                                                             ");
                        SQL.AppendLine(",   BB.DISSEQNO AS DISSEQNO1, BG.DISSEQNO AS DISSEQNO2, BI.NFLAG1                                                                                            ");
                        SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C                                                                         ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "EMR_USERT U                                                                      ");
                        SQL.AppendLine("   ON C.CHARTUSEID = U.USERID                                                                         ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R                                                                   ");
                        SQL.AppendLine("   ON C.EMRNO = R.EMRNO                                                                               ");
                        SQL.AppendLine("  AND C.EMRNOHIS = R.EMRNOHIS                                                                        ");
                        SQL.AppendLine("  AND R.ITEMVALUE > CHR(0)");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BI                                                                     ");
                        SQL.AppendLine("   ON R.ITEMCD = BI.BASCD                                                                             ");
                        SQL.AppendLine("  AND BI.BSNSCLS = '기록지관리'                                                                        ");
                        SQL.AppendLine("  AND BI.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')                                       ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG                                                                     ");
                        SQL.AppendLine("   ON BI.VFLAG1 = BG.BASCD                                                                            ");
                        SQL.AppendLine("  AND BG.BSNSCLS = '기록지관리'                                                                        ");
                        SQL.AppendLine("  AND BG.UNITCLS IN ('임상관찰그룹', '섭취배설그룹', '특수치료그룹', '기본간호그룹')                          ");
                        SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB                                                                     ");
                        SQL.AppendLine("   ON BG.VFLAG1 = BB.BASCD                                                                            ");
                        SQL.AppendLine("  AND BB.BSNSCLS = '기록지관리'                                                                        ");
                        SQL.AppendLine("  AND BB.UNITCLS = '임상관찰그룹정렬'                                                                   ");
                        SQL.AppendLine("INNER JOIN  " + ComNum.DB_EMR + "EMRPRTREQ P");
                        SQL.AppendLine("   ON  P.PRTREQNO = " + strPrtSeq);
                        SQL.AppendLine("  AND  P.EMRNO = R.EMRNO");
                        SQL.AppendLine("  AND  P.SCANYN = 'T'");
                        SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                        SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                        SQL.AppendLine("  AND C.MEDDEPTCD = '" + mstrDeptCode + "'");
                        SQL.AppendLine("  AND R.ITEMCD NOT IN ('I0000022324') --기존 임상관찰 수혈 제외                                                     ");
                        SQL.AppendLine("UNION ALL --수혈기록지에서 수혈량 가저오기                                                                              ");
                        SQL.AppendLine("SELECT     A.FORMNO                                                                                           ");
                        SQL.AppendLine("         , 0 AS EMRNO                                                                                            ");
                        SQL.AppendLine("         , (REPLACE(REPLACE((SELECT ITEMVALUE                                                                 ");
                        SQL.AppendLine("                                  FROM KOSMOS_EMR.AEMRCHARTROW                                                ");
                        SQL.AppendLine("                                 WHERE EMRNO = A.EMRNO                                                        ");
                        SQL.AppendLine("                                   AND EMRNOHIS = A.EMRNOHIS                                                        ");
                        SQL.AppendLine("                                   AND ITEMNO = 'I0000037490'),'-',''),'/','')) AS CHARTDATE--수혈종료일자        ");
                        SQL.AppendLine("         , REPLACE((SELECT ITEMVALUE                                                                          ");
                        SQL.AppendLine("                      FROM KOSMOS_EMR.AEMRCHARTROW                                                            ");
                        SQL.AppendLine("                     WHERE EMRNO = A.EMRNO                                                                    ");
                        SQL.AppendLine("                       AND EMRNOHIS = A.EMRNOHIS                                                        ");
                        SQL.AppendLine("                       AND ITEMNO = 'I0000037491'),':','')AS CHARTTIME  --수혈종료시간                            ");
                        SQL.AppendLine("         , A.CHARTUSEID                                                                                       ");
                        SQL.AppendLine("         , A.WRITEDATE                                                                                        ");
                        SQL.AppendLine("         , A.WRITETIME                                                                                        ");
                        SQL.AppendLine("         , '' AS PRNTYN                                                                                       ");
                        SQL.AppendLine("         , BB.BASNAME AS BGRPNAME                                                                             ");
                        SQL.AppendLine("         , BG.BASNAME AS GRPNAME                                                                              ");
                        SQL.AppendLine("         , 'I0000022324' AS ITEMCD                                                                            ");
                        SQL.AppendLine("         , BI.BASNAME AS ITEMNAME                                                                             ");
                        SQL.AppendLine("         , B.ITEMVALUE || B.ITEMVALUE1 AS ITEMVALUE                                                           ");
                        SQL.AppendLine("         , (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE A.CHARTUSEID = USERID ) AS USENAME                    ");
                        SQL.AppendLine("         , BB.DISSEQNO AS DISSEQNO1                                                                           ");
                        SQL.AppendLine("         , BG.DISSEQNO AS DISSEQNO2                                                                           ");
                        SQL.AppendLine("         , BI.NFLAG1                                                                                          ");
                        SQL.AppendLine("      FROM KOSMOS_EMR.AEMRCHARTMST A                                                                          ");
                        SQL.AppendLine("     INNER JOIN  KOSMOS_EMR.AEMRCHARTROW B                                                                    ");
                        SQL.AppendLine("        ON A.EMRNO = B.EMRNO                                                                                  ");
                        SQL.AppendLine("       AND A.EMRNOHIS = B.EMRNOHIS                                                        ");
                        SQL.AppendLine("     INNER JOIN KOSMOS_EMR.AEMRBASCD BI                                                                       ");
                        SQL.AppendLine("        ON 'I0000022324' = BI.BASCD                                                                           ");
                        SQL.AppendLine("       AND BI.BSNSCLS = '기록지관리'                                                                             ");
                        SQL.AppendLine("       AND BI.UNITCLS IN ('섭취배설')                                                 ");
                        SQL.AppendLine("     INNER JOIN KOSMOS_EMR.AEMRBASCD BG                                                                       ");
                        SQL.AppendLine("        ON BI.VFLAG1 = BG.BASCD                                                                               ");
                        SQL.AppendLine("       AND BG.BSNSCLS = '기록지관리'                                                                             ");
                        SQL.AppendLine("       AND BG.UNITCLS IN ('섭취배설그룹')                                       ");
                        SQL.AppendLine("     INNER JOIN KOSMOS_EMR.AEMRBASCD BB                                                                       ");
                        SQL.AppendLine("        ON BG.VFLAG1 = BB.BASCD                                                                               ");
                        SQL.AppendLine("       AND BB.BSNSCLS = '기록지관리'                                                                             ");
                        SQL.AppendLine("       AND BB.UNITCLS = '임상관찰그룹정렬'                                                                          ");
                        SQL.AppendLine("     WHERE A.PTNO  = '" + pAcp.ptNo + "' ");
                        SQL.AppendLine("       AND A.ACPNO = " + pAcp.acpNo);
                        SQL.AppendLine("       AND A.FORMNO IN (1965, 3535)                                                                        ");
                        SQL.AppendLine("       AND B.ITEMNO = 'I0000013528'                                                                           ");
                        SQL.AppendLine(")");

                        SQL.AppendLine("SELECT FORMNO, EMRNO, CHARTDATE, CHARTTIME, CHARTUSEID, WRITEDATE, WRITETIME, PRNTYN, BGRPNAME, GRPNAME, ITEMCD, ITEMNAME, ITEMVALUE, USENAME, DISSEQNO1, DISSEQNO2, NFLAG1																						   ");
                        SQL.AppendLine("FROM M                                                                                                       ");
                        SQL.AppendLine(" WHERE ITEMCD NOT IN ('I0000030622','I0000030623', 'I0000022324') --기존 섭취배설 기록 삭제                                         ");
                        SQL.AppendLine(" UNION ALL --섭취계산                                                                                               ");
                        SQL.AppendLine(" SELECT A.FORMNO                                                                                             ");
                        SQL.AppendLine("      , A.EMRNO                                                                                              ");
                        SQL.AppendLine("      , A.CHARTDATE                                                                                          ");
                        SQL.AppendLine("      , A.CHARTTIME                                                                                          ");
                        SQL.AppendLine("      , '' AS CHARTUSEID                                                                                     ");
                        SQL.AppendLine("      , '' AS WRITEDATE                                                                                      ");
                        SQL.AppendLine("      , '' AS WRITETIME                                                                                      ");
                        SQL.AppendLine("      , '' AS PRNTYN                                                                                         ");
                        SQL.AppendLine("      , '섭취배설' AS BGRPNAME                                                                                  ");
                        SQL.AppendLine("      , '총섭취량' AS GRPNAME                                                                                   ");
                        SQL.AppendLine("      , 'I0000030622' AS ITEMCD                                                                              ");
                        SQL.AppendLine("      , '총섭취량' AS ITEMNAME                                                                                  ");
                        SQL.AppendLine("      , TO_CHAR(SUM(A.ITEMVALUE)) AS ITEMVALUE                                                               ");
                        SQL.AppendLine("      , '' AS USENAME, 2 AS DISSEQNO1, 5555 AS DISSEQNO2, 0 AS NFLAG1                                        ");
                        SQL.AppendLine("  FROM M A                                                                                                   ");
                        SQL.AppendLine(" INNER JOIN KOSMOS_EMR.AEMRBASCD B                                                                           ");
                        SQL.AppendLine("    ON A.ITEMCD = B.BASCD                                                                                    ");
                        SQL.AppendLine("   AND A.CHARTDATE >= B.APLFRDATE                                                                            ");
                        SQL.AppendLine("   AND A.CHARTDATE < B.APLENDDATE                                                                            ");
                        SQL.AppendLine(" WHERE B.BSNSCLS = '기록지관리'                                                                                  ");
                        SQL.AppendLine("   AND B.UNITCLS = '섭취배설'                                                                                   ");
                        SQL.AppendLine("   AND B.VFLAG3 = '01.섭취'                                                                                    ");
                        SQL.AppendLine("   AND A.ITEMVALUE IS NOT NULL                                                                               ");
                        SQL.AppendLine(@"   AND REGEXP_LIKE(A.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"); //소수점까지 체크가능
                        SQL.AppendLine("GROUP BY A.FORMNO, A.EMRNO, A.CHARTDATE, A.CHARTTIME                                                         ");
                        SQL.AppendLine(" UNION ALL -- 베설 계산                                                                                             ");
                        SQL.AppendLine(" SELECT A.FORMNO                                                                                             ");
                        SQL.AppendLine("      , A.EMRNO                                                                                              ");
                        SQL.AppendLine("      , A.CHARTDATE                                                                                          ");
                        SQL.AppendLine("      , A.CHARTTIME                                                                                          ");
                        SQL.AppendLine("      , '' AS CHARTUSEID                                                                                     ");
                        SQL.AppendLine("      , '' AS WRITEDATE                                                                                      ");
                        SQL.AppendLine("      , '' AS WRITETIME                                                                                      ");
                        SQL.AppendLine("      , '' AS PRNTYN                                                                                         ");
                        SQL.AppendLine("      , '섭취배설' AS BGRPNAME                                                                                  ");
                        SQL.AppendLine("      , '총배설량' AS GRPNAME                                                                                   ");
                        SQL.AppendLine("      , 'I0000030623' AS ITEMCD                                                                              ");
                        SQL.AppendLine("      , '총배설량' AS ITEMNAME                                                                                  ");
                        SQL.AppendLine("      , TO_CHAR(SUM(A.ITEMVALUE)) AS ITEMVALUE                                                               ");
                        SQL.AppendLine("      , '' AS USENAME, 2 AS DISSEQNO1, 8003 AS DISSEQNO2, 0 AS NFLAG1                                        ");
                        SQL.AppendLine("  FROM M A                                                                                                   ");
                        SQL.AppendLine(" INNER JOIN KOSMOS_EMR.AEMRBASCD B                                                                           ");
                        SQL.AppendLine("    ON A.ITEMCD = B.BASCD                                                                                    ");
                        SQL.AppendLine("   AND A.CHARTDATE >= B.APLFRDATE                                                                            ");
                        SQL.AppendLine("   AND A.CHARTDATE < B.APLENDDATE                                                                            ");
                        SQL.AppendLine(" WHERE B.BSNSCLS = '기록지관리'                                                                                  ");
                        SQL.AppendLine("   AND B.UNITCLS = '섭취배설'                                                                                   ");
                        SQL.AppendLine("   AND B.VFLAG3 = '11.배설'                                                                                    ");
                        SQL.AppendLine("   AND A.ITEMVALUE IS NOT NULL                                                                               ");
                        SQL.AppendLine(@"   AND REGEXP_LIKE(A.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"); //소수점까지 체크가능
                        SQL.AppendLine("GROUP BY A.FORMNO, A.EMRNO, A.CHARTDATE, A.CHARTTIME                                                         ");

                    }
                    #endregion

                }
                else if (pForm.FmFORMNO == 1575) //간호활동
                {
                    SQL.AppendLine(" '1575' AS FORMNO, C.EMRNO, C.EMRNOHIS, C.CHARTUSEID, C.CHARTDATE, C.CHARTTIME, C.WRITEDATE, C.WRITETIME, '' AS PRNTYN, ");
                    SQL.AppendLine("    R.ITEMCD, R.ITEMNO, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE || R.ITEMVALUE1 AS ITEMVALUE,  ");
                    SQL.AppendLine("    B.BASEXNAME AS BGRPNAME, B.BASNAME AS GRPNAME, U.NAME AS USENAME  ");
                    SQL.AppendLine("FROM  " + ComNum.DB_EMR + "AEMRCHARTMST C  ");
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R  ");
                    SQL.AppendLine("   ON C.EMRNO = R.EMRNO  ");
                    SQL.AppendLine("  AND C.EMRNOHIS = R.EMRNOHIS ");
                    SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B ");
                    SQL.AppendLine("   ON R.ITEMCD = B.BASCD ");
                    SQL.AppendLine("  AND B.BSNSCLS = '기록지관리' ");
                    SQL.AppendLine("  AND B.UNITCLS = '간호활동항목' ");
                    SQL.AppendLine("INNER JOIN  " + ComNum.DB_EMR + "EMRPRTREQ P");
                    SQL.AppendLine("   ON  P.PRTREQNO = " + strPrtSeq);
                    SQL.AppendLine("  AND  P.EMRNO = R.EMRNO");
                    SQL.AppendLine("  AND  P.SCANYN = 'T'");
                    SQL.AppendLine(" LEFT OUTER JOIN  " + ComNum.DB_EMR + "EMR_USERT U  ");
                    SQL.AppendLine("   ON U.USERID = C.CHARTUSEID  ");
                    SQL.AppendLine("WHERE C.ACPNO = " + pAcp.acpNo);
                    SQL.AppendLine("  AND C.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                    SQL.AppendLine("  AND C.MEDDEPTCD = '" + mstrDeptCode + "'");
                }
                else if (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2147 || pForm.FmFORMNO == 2049)//간호기록
                {
                    SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, --A.PRNTYN, ");
                    SQL.AppendLine("    '' AS ITEMCD, '' AS ITEMNO, '' AS ITEMINDEX, '' AS ITEMTYPE , B.NRRECODE AS ITEMVALUE,");
                    SQL.AppendLine("    B.WARDCODE, B.ROOMCODE, B.PROBLEMCODE, B.PROBLEM, B.TYPE,");
                    SQL.AppendLine("    U.NAME AS USENAME");
                    SQL.AppendLine("     , '' PRNTYN");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRNURSRECORD B");
                    SQL.AppendLine("     ON B.EMRNO = C.EMRNO");
                    SQL.AppendLine("    AND B.EMRNOHIS = C.EMRNOHIS");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                    SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMRPRTREQ P");
                    SQL.AppendLine("     ON  P.PRTREQNO = " + strPrtSeq);
                    SQL.AppendLine("    AND  P.EMRNO = B.EMRNO");
                    SQL.AppendLine("    AND  P.SCANYN = 'T'");
                    SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                    SQL.AppendLine("  AND C.MEDDEPTCD = '" + mstrDeptCode + "'");
                }
                else
                {
                    SQL.AppendLine(" C.FORMNO, C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.WRITEDATE, C.WRITETIME, '' AS PRNTYN, ");
                    SQL.AppendLine("                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,");
                    SQL.AppendLine("                U.NAME AS USENAME, '' AS BUN");
                    SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B");
                    SQL.AppendLine("     ON  B.EMRNO = C.EMRNO");
                    SQL.AppendLine("    AND  B.EMRNOHIS = C.EMRNOHIS");
                    SQL.AppendLine("    AND  B.ITEMCD > CHR(0)");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMRPRTREQ P");
                    SQL.AppendLine("     ON  P.PRTREQNO = " + strPrtSeq);
                    SQL.AppendLine("    AND  P.EMRNO = B.EMRNO");
                    SQL.AppendLine("    AND  P.SCANYN = 'T'");
                    SQL.AppendLine("  INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT U");
                    SQL.AppendLine("     ON  U.USERID = C.CHARTUSEID");
                    SQL.AppendLine("WHERE C.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("  AND C.FORMNO = " + pForm.FmFORMNO);
                    SQL.AppendLine("  AND C.MEDDEPTCD = '" + mstrDeptCode + "'");

                }
                #endregion
            }

            if (chkAsc == true)
            {
                if (dtpSys.Date < ("2020-09-22").To<DateTime>())
                {
                    SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE), TRIM(C.CHARTTIME), C.EMRNO");
                }
                else
                {
                    SQL.AppendLine("ORDER BY TRIM(CHARTDATE), TRIM(CHARTTIME), EMRNO");
                }
            }
            else
            {
                if (dtpSys.Date < ("2020-09-22").To<DateTime>())
                {
                    SQL.AppendLine("ORDER BY TRIM(C.CHARTDATE) DESC, TRIM(C.CHARTTIME) DESC, C.EMRNO DESC");
                }
                else
                {
                    SQL.AppendLine("ORDER BY CHARTDATE, CHARTTIME, EMRNO");
                }
            }
         

            if (pForm.FmOLDGB != 1 &&
                (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 2135 || pForm.FmFORMNO == 1935 ||
                 pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || pForm.FmFORMNO == 2201))
            {
                if (dtpSys.Date < ("2020-09-22").To<DateTime>())
                {
                    SQL.AppendLine(", BB.DISSEQNO, BG.DISSEQNO, BI.NFLAG1");
                }
                else
                {
                    SQL.AppendLine(", DISSEQNO1, DISSEQNO2, NFLAG1");
                }
            }
            else if (pForm.FmFORMNO == 1575 && pForm.FmOLDGB != 1)
            {
                SQL.AppendLine(",B.NFLAG1, B.NFLAG2, R.DSPSEQ  ");
            }

            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }

            return dt;
        }



        /// <summary>
        /// 내용을 스프래드에 표시한다(PRTREQNO)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strPrtSeq"></param>
        /// <param name="ssWrite"></param>
        /// <param name="ssList"></param>
        /// <param name="strDirection"></param>
        /// <param name="intHeadRow"></param>
        /// <param name="intHeadCol"></param>
        /// <param name="mFormFlowSheet"></param>
        /// <param name="chkDesc"></param>
        public static void QuerySpdList2(PsmhDb pDbCon, EmrPatient pAcp, string strFormNo, string strEmrNo, string strUpdateNo, string strPrtSeq,
                                            //FarPoint.Win.Spread.FpSpread ssWrite, 
                                            FarPoint.Win.Spread.FpSpread ssList,
                                            string strDirection, int intHeadRow, int intHeadCol, FormFlowSheet[] mFormFlowSheet, bool chkDesc = true)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strDirection == "ROW")
            {
                ssList.ActiveSheet.ColumnCount = 0;
            }
            else
            {
                ssList.ActiveSheet.RowCount = 0;
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL.AppendLine( "SELECT ");
            SQL.AppendLine( "     C.EMRNO, C.CHARTDATE, C.CHARTTIME, C.USEID AS CHARTUSEID, C.CHARTXML,  ");
            SQL.AppendLine( "     EXTRACTVALUE(C.CHARTXML,'//' || FX.ITEMNO) AS ITEMVALUE, '' AS PRNTYN, ");
            SQL.AppendLine( "     U.USENAME ,  ");
            SQL.AppendLine( "     FX.ITEMNO AS ITEMCD, FX.ITEMNAME, FX.CELLTYPE AS ITEMTYPE ");
            SQL.AppendLine( "FROM " + ComNum.DB_EMR + "EMRXML C ");
            SQL.AppendLine( "INNER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ");
            SQL.AppendLine( "    ON C.USEID = U.USEID ");
            SQL.AppendLine( "INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F ");
            SQL.AppendLine( "    ON C.FORMNO = F.FORMNO ");
            SQL.AppendLine( "    AND F.UPDATENO = " + strUpdateNo);
            SQL.AppendLine( "INNER JOIN " + ComNum.DB_EMR + "AEMRFLOWXML FX ");
            SQL.AppendLine( "    ON F.FORMNO = FX.FORMNO ");
            SQL.AppendLine( "    AND F.UPDATENO = FX.UPDATENO ");
            SQL.AppendLine( "INNER JOIN " + ComNum.DB_EMR + "EMRPRTREQ E");
            SQL.AppendLine( "    ON C.EMRNO = E.EMRNO");
            SQL.AppendLine( "    AND E.PRTREQNO = " + strPrtSeq);
            SQL.AppendLine( "    AND E.SCANYN = 'T'");
            SQL.AppendLine( "WHERE C.PTNO = '" + pAcp.ptNo + "' ");
            SQL.AppendLine("	AND C.FORMNO = " + strFormNo);

            if (chkDesc == true)
            {
                SQL.AppendLine("ORDER BY C.CHARTDATE DESC, C.CHARTTIME DESC, C.EMRNO DESC");
            }
            else
            {
                SQL.AppendLine("ORDER BY C.CHARTDATE, C.CHARTTIME, C.EMRNO");
            }


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            //int intStart = 3;
            string strEMRNO = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    if (strDirection == "ROW")
                    {
                        ssList.ActiveSheet.ColumnCount = ssList.ActiveSheet.ColumnCount + 1;
                        if ((int)ssList.ActiveSheet.Columns[ssList.ActiveSheet.ColumnCount - 1].Width < 80)
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, 80);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, (int)ssList.ActiveSheet.Columns[ssList.ActiveSheet.ColumnCount - 1].Width);
                        }

                    }
                    else
                    {
                        ssList.ActiveSheet.RowCount = ssList.ActiveSheet.RowCount + 1;
                        if ((int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height < 22)
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, 22);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, (int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height);
                        }
                    }
                }

                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                string strCHARTDATE = VB.Val( dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");

                if (strDirection == "ROW")
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 0, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, strCHARTDATE, false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == mFormFlowSheet[j].ItemCode.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssList.ActiveSheet.Cells[2 + j, ssList.ActiveSheet.ColumnCount - 1].VerticalAlignment,
                                            ssList.ActiveSheet.Cells[2 + j, ssList.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            false);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 3, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 2, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }
                else
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 0, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, strCHARTDATE, false);
                    //clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == mFormFlowSheet[j].ItemCode.Trim()) //이부분 수정 
                        {
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 2 + j, clsXML.GetTypeEx(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 2 + j].VerticalAlignment,
                                            ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 2 + j].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            true);
                            ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].Height =
                            (int)ssList.ActiveSheet.Rows[ssList.ActiveSheet.RowCount - 1].GetPreferredHeight() + 10;
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 4, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 3, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);

                }

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        public static void QuerySpdListNew(PsmhDb pDbCon, EmrPatient p, string strFormNo, string strUpdateNo, string strFrDate, string strEndDate,
                                            FarPoint.Win.Spread.FpSpread ssWrite, FarPoint.Win.Spread.FpSpread ssList,
                                            string strDirection, int intHeadRow, int intHeadCol, bool chkDesc = true)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strDirection == "ROW")
            {
                ssList.ActiveSheet.ColumnCount = intHeadCol;
            }
            else
            {
                ssList.ActiveSheet.RowCount = intHeadRow;
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + strUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + strFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + strEndDate + "'   ";

            //AND  B.EMRNO <> '10025131'

            if (chkDesc == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE DESC , A.CHARTTIME DESC , A.EMRNO, B.DSPSEQ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, B.DSPSEQ";
            }
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    if (strDirection == "ROW")
                    {
                        ssList.ActiveSheet.ColumnCount = ssList.ActiveSheet.ColumnCount + 1;
                        if ((int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width < 80)
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, 80);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, (int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width);
                        }

                    }
                    else
                    {
                        ssList.ActiveSheet.RowCount = ssList.ActiveSheet.RowCount + 1;
                        if ((int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height < 22)
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, 22);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, (int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height);
                        }
                    }
                }
                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                if (strDirection == "ROW")
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 0, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.RowCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[j, 0].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, 4 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            false);
                            //vital, BST 이상점수 빨간색
                            switch (dt.Rows[i]["ITEMCD"].ToString().Trim())
                            {
                                case "I0000019079":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 90) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 200))
                                    {
                                        ssList.ActiveSheet.Cells[4 + j, ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                                    }
                                    break;
                                case "I0000002018":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 100) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 140))
                                    {
                                        ssList.ActiveSheet.Cells[4 + j, ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                                    }
                                    break;
                                case "I0000001765":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 60) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 90))
                                    {
                                        ssList.ActiveSheet.Cells[4 + j, ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                                    }
                                    break;
                                case "I0000001811":
                                case "I0000000416":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 36.3) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 37.4))
                                    {
                                        ssList.ActiveSheet.Cells[4 + j, ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                                    }
                                    break;
                            }
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }
                else
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.ColumnCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[0, j].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 4 + j, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            false);
                            //vital, BST 이상점수 빨간색
                            switch (dt.Rows[i]["ITEMCD"].ToString().Trim())
                            {
                                case "I0000019079":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 90) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 200))
                                    {
                                        ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 4 + j].ForeColor = Color.Red;
                                    }
                                    break;
                                case "I0000002018":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 100) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 140))
                                    {
                                        ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 4 + j].ForeColor = Color.Red;
                                    }
                                    break;
                                case "I0000001765":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 60) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 90))
                                    {
                                        ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 4 + j].ForeColor = Color.Red;
                                    }
                                    break;
                                case "I0000001811":
                                case "I0000000416":
                                    if ((VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) < 36.3) || (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 37.4))
                                    {
                                        ssList.ActiveSheet.Cells[ssList.ActiveSheet.RowCount - 1, 4 + j].ForeColor = Color.Red;
                                    }
                                    break;
                            }
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 내용을 스프래드에 표시한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="p"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="ssWrite"></param>
        /// <param name="ssList"></param>
        /// <param name="strDirection"></param>
        /// <param name="intHeadRow"></param>
        /// <param name="intHeadCol"></param>
        /// <param name="chkDesc"></param>
        /// <param name="MultiLine"></param>
        public static void QuerySpdListEx(PsmhDb pDbCon, EmrPatient p, string strFormNo, string strUpdateNo, string strFrDate, string strEndDate,
                                            FarPoint.Win.Spread.FpSpread ssWrite, FarPoint.Win.Spread.FpSpread ssList,
                                            string strDirection, int intHeadRow, int intHeadCol, bool chkDesc = true, bool MultiLine = false)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strDirection == "H")
            {
                ssList.ActiveSheet.ColumnCount = intHeadCol;
            }
            else
            {
                ssList.ActiveSheet.RowCount = intHeadRow;
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + strUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + strFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + strEndDate + "'";

            if (chkDesc == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE DESC , A.CHARTTIME DESC , A.EMRNO, B.DSPSEQ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, B.DSPSEQ";
            }
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    if (strDirection == "H")
                    {
                        ssList.ActiveSheet.ColumnCount = ssList.ActiveSheet.ColumnCount + 1;
                        if ((int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width < 80)
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, 80);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, (int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width);
                        }

                    }
                    else
                    {
                        ssList.ActiveSheet.RowCount = ssList.ActiveSheet.RowCount + 1;
                        if ((int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height < 22)
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, 22);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, (int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height);
                        }
                    }
                }
                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                if (strDirection == "H")
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 0, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.RowCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[j, 0].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, 4 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            MultiLine);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }
                else
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.ColumnCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[0, j].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 4 + j, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            MultiLine);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        public static void QuerySpdListForm(PsmhDb pDbCon, EmrPatient p, string strFormNo, string strUpdateNo, string strFrDate, string strEndDate,
                                            FarPoint.Win.Spread.FpSpread ssWrite, FarPoint.Win.Spread.FpSpread ssList,
                                            string strDirection, int intHeadRow, int intHeadCol, bool chkDesc = true)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strDirection == "H")
            {
                ssList.ActiveSheet.ColumnCount = intHeadCol;
            }
            else
            {
                ssList.ActiveSheet.RowCount = intHeadRow;
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + strUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + strFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + strEndDate + "'";

            if (chkDesc == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE DESC , A.CHARTTIME DESC , A.EMRNO, B.DSPSEQ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, B.DSPSEQ";
            }
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    if (strDirection == "H")
                    {
                        ssList.ActiveSheet.ColumnCount = ssList.ActiveSheet.ColumnCount + 1;
                        if ((int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width < 80)
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, 80);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, (int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width);
                        }

                    }
                    else
                    {
                        ssList.ActiveSheet.RowCount = ssList.ActiveSheet.RowCount + 1;
                        if ((int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height < 22)
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, 22);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, (int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height);
                        }
                    }
                }
                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                if (strDirection == "H")
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 0, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.RowCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[j, 0].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, 4 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            false);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }
                else
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.ColumnCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[0, j].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 4 + j, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            false);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        public static void QuerySpdListFormEx(PsmhDb pDbCon, EmrPatient p, string strFormNo, string strUpdateNo, string strFrDate, string strEndDate,
                                            FarPoint.Win.Spread.FpSpread ssWrite, FarPoint.Win.Spread.FpSpread ssList,
                                            string strDirection, int intHeadRow, int intHeadCol, bool chkDesc = true, bool MultiLine = false)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strDirection == "H")
            {
                ssList.ActiveSheet.ColumnCount = intHeadCol;
            }
            else
            {
                ssList.ActiveSheet.RowCount = intHeadRow;
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            if (p.inOutCls == "I")
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + p.acpNo + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + strFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = " + strUpdateNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + strFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + strEndDate + "'";

            if (chkDesc == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE DESC , A.CHARTTIME DESC , A.EMRNO, B.DSPSEQ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO, B.DSPSEQ";
            }
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    if (strDirection == "H")
                    {
                        ssList.ActiveSheet.ColumnCount = ssList.ActiveSheet.ColumnCount + 1;
                        if ((int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width < 80)
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, 80);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetColumnWidth(ssList.ActiveSheet.ColumnCount - 1, (int)ssWrite.ActiveSheet.Columns[ssWrite.ActiveSheet.ColumnCount - 1].Width);
                        }

                    }
                    else
                    {
                        ssList.ActiveSheet.RowCount = ssList.ActiveSheet.RowCount + 1;
                        if ((int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height < 22)
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, 22);
                        }
                        else
                        {
                            ssList.ActiveSheet.SetRowHeight(ssList.ActiveSheet.RowCount - 1, (int)ssWrite.ActiveSheet.Rows[ssWrite.ActiveSheet.RowCount - 1].Height);
                        }
                    }
                }
                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                if (strDirection == "H")
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 0, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.RowCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[j, 0].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, 4 + j, ssList.ActiveSheet.ColumnCount - 1, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[j, ssWrite.ActiveSheet.ColumnCount - 1].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            MultiLine);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 3, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 2, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }
                else
                {
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 0, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["USENAME"].ToString() + "").Trim(), false);
                    for (int j = 0; j < ssWrite.ActiveSheet.ColumnCount; j++) //이부분 수정 
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == ssWrite.ActiveSheet.Cells[0, j].Text.Trim()) //이부분 수정 
                        {
                            //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                            clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 4 + j, clsXML.GetType(dt.Rows[i]["ITEMTYPE"].ToString().Trim()), true,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].VerticalAlignment,
                                            ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.RowCount - 1, j].HorizontalAlignment,
                                            dt.Rows[i]["ITEMVALUE"].ToString().Trim(),
                                            MultiLine);
                        }
                    }

                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 3, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["EMRNO"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 2, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["CHARTUSEID"].ToString() + "").Trim(), false);
                    clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1, "TypeText", true, clsSpread.VAlign_C, clsSpread.HAlign_C, (dt.Rows[i]["PRNTYN"].ToString() + "").Trim(), false);
                }

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        public static void SetWriteSpdHide(FarPoint.Win.Spread.FpSpread ssWrite, string strDirection, int intHeadRow, int intHeadCol)
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            int i = 0;
            if (strDirection == "H")
            {
                for (i = 0; i < ssWrite.ActiveSheet.RowCount; i++)
                {
                    if (ssWrite.ActiveSheet.Cells[i, intHeadCol].CellType == TypeCheckBox)
                    {
                        ssWrite.ActiveSheet.Cells[i, intHeadCol].Value = false;
                    }
                    else
                    {
                        ssWrite.ActiveSheet.Cells[i, intHeadCol].Text = "";
                    }

                }
            }
            else
            {
                for (i = 0; i < ssWrite.ActiveSheet.ColumnCount; i++)
                {
                    if (ssWrite.ActiveSheet.Cells[intHeadRow, i].CellType == TypeCheckBox)
                    {
                        ssWrite.ActiveSheet.Cells[intHeadRow, i].Value = false;
                    }
                    else
                    {
                        ssWrite.ActiveSheet.Cells[intHeadRow, i].Text = "";
                    }
                }
            }
        }

        public static void GetFormZoom(PsmhDb pDbCon)
        {
            DataTable dt = null;
            //optEMRFORMZOOM01 폼 확대 축소
            clsEmrPublic.isEMRFORMZOOM = "0";
            if (clsEmrPublic.isEMRFORMZOOMINIT == "1") return;

            dt = clsEmrQuery.GetEmrUserOption(pDbCon, clsType.User.IdNumber, "EMROPTION", "EMRFORMZOOM");
            if (dt != null)
            {
                if (dt.Rows.Count != 0)
                {
                    clsEmrPublic.isEMRFORMZOOM = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                }
                dt.Dispose();
                dt = null;
            }
        }

        public static string GetInChartOneEmrNo(PsmhDb pDbCon, string strAcpNo, EmrForm f)
        {
            string rtnVal = "0";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + strAcpNo;
            SQL = SQL + ComNum.VBLF + "    AND FORMNO = " + f.FmFORMNO;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            rtnVal = Convert.ToString(VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim()));
            dt.Dispose();
            dt = null;

            return rtnVal;
        }


        /// <summary>
        /// 수정 가능한지 여부
        /// </summary>
        /// <returns></returns>
        public static bool DischEditYn(PsmhDb pDbCon)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT AMANAGE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRAUTHORITY A ";
            SQL = SQL + ComNum.VBLF + "    ON A.USEID = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND A.AMANAGE = '1'";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '권한관리'";
            SQL = SQL + ComNum.VBLF + "       AND B.UNITCLS = 'TOP' ";
            SQL = SQL + ComNum.VBLF + "       AND B.BASEXNAME = '의무기록실' ";
            SQL = SQL + ComNum.VBLF + "       AND B.BASCD = '" + clsType.User.IdNumber + "' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

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
            dt.Dispose();
            dt = null;

            //if (clsCommon.gstrEXENAME == "MHEMRJOBDG.EXE" || clsCommon.gstrEXENAME == "MHEMRMAINDG.EXE")
            //{
            //    rtnVal = true;
            //}

            return rtnVal;
        }

        public static void ClearSpcScore(Control frm)
        {
            Control[] tx = null;
            Control obj = null;

            //낙상
            tx = frm.Controls.Find("lblFallDown", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = false;
                }
            }

            //욕창
            tx = frm.Controls.Find("lblYakchang", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = false;
                }
            }


            //불량영양
            tx = frm.Controls.Find("lblNutrition", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = false;
                }
            }


            //항응고제
            tx = frm.Controls.Find("lblAnticoagulant", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = false;
                }
            }

            //CPR 거부
            tx = frm.Controls.Find("lblDNR", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = true;
                    ((Label)obj).ForeColor = Color.Gray;
                }
            }

            //비밀보장
            tx = frm.Controls.Find("lblSECURE", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = true;
                    ((Label)obj).ForeColor = Color.Gray;
                }
            }

            //임산부
            tx = frm.Controls.Find("lblPregN", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = true;
                    ((Label)obj).ForeColor = Color.Gray;
                }
            }

            //화재
            tx = frm.Controls.Find("lblFire", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = true;
                    ((Label)obj).ForeColor = Color.Gray;
                    ((Label)obj).Text = "화재";
                }
            }

            //혈액
            tx = frm.Controls.Find("lblBlood", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Label)tx[0];
                    ((Label)obj).Visible = false;
                    ((Label)obj).Text = "";
                }
            }

            // 감염
            tx = frm.Controls.Find("lblInfect", true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    obj = (Button)tx[0];
                    ((Button)obj).Visible = false;
                    ((Button)obj).BackgroundImage = null;
                }
            }
        }

        /// <summary>
        /// 차트 작성일자를 세팅한다.
        /// </summary>
        public static void SetChartDateFlowSheet(PsmhDb pDbCon, string strEmrNo, string strFormNo, string strUpdateNo, DateTimePicker dtpStart, DateTimePicker dtpEnd)
        {
            if (VB.Val(strEmrNo) == 0) return;

            EmrForm fView = clsEmrChart.ClearEmrForm();
            fView = clsEmrChart.SerEmrFormInfo(pDbCon, strFormNo, strUpdateNo);

            if (fView.FmVIEWGROUP != 1) return;

            string strCHARTDATE = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO = " + VB.Val(strEmrNo);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

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
            strCHARTDATE = (dt.Rows[0]["CHARTDATE"].ToString() + "").Trim();

            dt.Dispose();
            dt = null;
            dtpStart.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strCHARTDATE, "D"));
            if (dtpEnd != null)
            {
                dtpEnd.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strCHARTDATE, "D"));
            }
        }

        /// <summary>
        /// 수술기록지 작성 번호 가지고 오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strFormNo"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetOpEmrNo(PsmhDb pDbCon, string strFormNo, EmrPatient p)
        {
            string rtnVal = "0";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //당일 차팅 내역이 있는지 조회를 한다.
            SQL = "SELECT MAX(EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST ";
            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + VB.Val(p.acpNo);
            SQL = SQL + ComNum.VBLF + "      AND CHARTDATE = '" + p.opdate.Replace("-", "") + "'";
            SQL = SQL + ComNum.VBLF + "      AND FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "      AND OPDATE = TO_DATE('" + p.opdate + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "      AND OPDEGREE = " + p.opdegree + "";
            SQL = SQL + ComNum.VBLF + "      AND OP_DEPT = '" + p.opdept + "'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = (VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim())).ToString();
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 퇴원요약지 작성 여부 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strACPNO"></param>
        /// <returns></returns>
        public static bool GetDishCompInfo(PsmhDb pDbCon, string strACPNO)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //당일 차팅 내역이 있는지 조회를 한다.
            SQL = "";
            SQL = "SELECT * FROM " + ComNum.DB_EMR + "ADSCHCOMP        ";
            SQL = SQL + ComNum.VBLF + "WHERE  ACPNO     = " + strACPNO + "        ";
            SQL = SQL + ComNum.VBLF + "    AND    COMPGB    = 'Y' ";
            SQL = SQL + ComNum.VBLF + "    AND    CNCLYN    = '0' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = true;
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 전입/전출 등 매핑이 필요한 기록지 처리
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="po"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strNewEmrNo"></param>
        /// <param name="strEmrNoTR"></param>
        private static void AemrMapping(PsmhDb pDbCon, EmrPatient po, string strFormNo, string strEmrNo, string strNewEmrNo, string strEmrNoTR)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strFromToGubun = "";
            string strFORMGB = "";

            DataTable dt = null;

            SQL = "";
            SQL = "SELECT BASCD, REMARK1, REMARK2     ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD        ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'         ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '매핑기록지'       ";
            SQL = SQL + ComNum.VBLF + "    AND (REMARK1 = '" + strFormNo + "' OR REMARK2 = '" + strFormNo + "')        ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

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

            strFORMGB = dt.Rows[0]["BASCD"].ToString().Trim();

            if (dt.Rows[0]["REMARK1"].ToString().Trim() == strFormNo.Trim())
            {
                strFromToGubun = "FROMEMRNO";
            }
            else if (dt.Rows[0]["REMARK2"].ToString().Trim() == strFormNo.Trim())
            {
                strFromToGubun = "TOEMRNO";
            }

            dt.Dispose();
            dt = null;

            if (strFromToGubun == "")
            {
                return;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                if (VB.Val(strEmrNo) > 0)
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_EMR + "AEMRMAPPING      ";
                    SQL = SQL + ComNum.VBLF + "SET " + strFromToGubun + " = '" + strNewEmrNo + "'      ";
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = '" + po.acpNo + "'        ";

                    if (strFromToGubun == "TOEMRNO")
                    {
                        SQL = SQL + ComNum.VBLF + "AND FROMEMRNO = '" + strEmrNoTR + "'        ";
                    }
                    else
                    {
                        //2017-01-10 매핑안되는문제 있어서 분리시킴
                        SQL = SQL + ComNum.VBLF + "AND " + strFromToGubun + " = '" + strEmrNo + "'        ";
                    }
                    SQL = SQL + ComNum.VBLF + "AND FORMGB = '" + strFORMGB + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }
                }
                else
                {
                    if (strFromToGubun == "FROMEMRNO")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRMAPPING     ";
                        SQL = SQL + ComNum.VBLF + "   (ACPNO, FORMGB, " + strFromToGubun + ")     ";
                        SQL = SQL + ComNum.VBLF + " VALUES     ";
                        SQL = SQL + ComNum.VBLF + "   (" + po.acpNo + ",      ";
                        SQL = SQL + ComNum.VBLF + "    '" + strFORMGB + "',      ";
                        SQL = SQL + ComNum.VBLF + "    " + strNewEmrNo + ")      ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_EMR + "AEMRMAPPING      ";
                        SQL = SQL + ComNum.VBLF + "SET " + strFromToGubun + " = '" + strNewEmrNo + "'      ";
                        SQL = SQL + ComNum.VBLF + "WHERE ACPNO = '" + po.acpNo + "'        ";
                        SQL = SQL + ComNum.VBLF + "AND FROMEMRNO = '" + strEmrNoTR + "'        ";
                        SQL = SQL + ComNum.VBLF + "AND FORMGB = '" + strFORMGB + "' ";

                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 현재 환자가 내원/재원중인지
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmPat(PsmhDb pDbCon, string strPTNO, string strEMRNO, string strCurDate)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            if (strPTNO == "" || strEMRNO == "")
            {
                return true; //원래는 에러임
            }

            if (strPTNO == "")
            {
                SQL = "";
                SQL = "SELECT PTNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE  EMRNO = " + strEMRNO + "        ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return true;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = true;
                }
                strPTNO = dt.Rows[0]["PTNO"].ToString().Trim();
                dt.Dispose();
                dt = null;
            }

            if (strCurDate == "")
            {
                strCurDate = ComQuery.CurrentDateTime(pDbCon, "D");
            }

            //외래 내원내역
            SQL = "";
            SQL = "SELECT CLASS, PATID, INDATE FROM " + ComNum.DB_EMR + "EMR_TREATT";
            SQL = SQL + ComNum.VBLF + "WHERE  PATID = '" + strPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "    AND    CLASS    = 'O' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                SQL = "";
                SQL = "SELECT CLASS, PATID, INDATE FROM " + ComNum.DB_EMR + "EMR_TREATT";
                SQL = SQL + ComNum.VBLF + "WHERE  PATID = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "    AND    CLASS    = 'I' ";
                SQL = SQL + ComNum.VBLF + "    AND    ( ( OUTDATE IS NULL AND INDATE <= '" + strCurDate + "' ) ";
                SQL = SQL + ComNum.VBLF + "            OR     ( OUTDATE IS NOT NULL AND INDATE <= '" + strCurDate + "' AND OUTDATE >= '" + strCurDate + "' ) ) ";
                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt1.Rows.Count > 0)
                {
                    rtnVal = true;
                }
                dt1.Dispose();
                dt1 = null;
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 환자 진료정보 접근 사유 입력
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strEMRNO"></param>
        /// <param name="strCurDate"></param>
        /// <returns></returns>
        public static bool CheckChartViewComp(PsmhDb pDbCon, string strAcpNo, string strEmrNo)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strCHARTDATE = "";

            string strPATID = "";
            DataTable dt = null;
            DataTable dt1 = null;

            if (VB.Val(strAcpNo) == 0)
            {
                SQL = "";
                SQL = "SELECT ACPNO , CHARTDATE   FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE  EMRNO = " + strEmrNo + "        ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return true;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }
                strAcpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                strCHARTDATE = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }

            //현재 진료과를 비교를 한다.
            SQL = "";
            SQL = "SELECT CLASS, PATID, INDATE, CLINCODE ,DOCCODE  FROM " + ComNum.DB_EMR + "EMR_TREATT";
            SQL = SQL + ComNum.VBLF + "WHERE  TREATNO = " + VB.Val(strAcpNo);
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return false;
            }

            string strMedDeptCd = dt.Rows[0]["CLINCODE"].ToString().Trim();

            //2016-07-19 추가 함  의사코드 
            string strMedDeptCode = dt.Rows[0]["DOCCODE"].ToString().Trim();
            string strIPDATE = dt.Rows[0]["INDATE"].ToString().Trim();

            strPATID = dt.Rows[0]["PATID"].ToString().Trim();

            // 2017-01-18 외래당일 진료의 또는 입원환자 담당교수는 다풀어줌
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM MED_PMPA.OPD_MAST ";
            SQL = SQL + ComNum.VBLF + " WHERE ACT_DATE = TRUNC(SYSDATE) ";
            SQL = SQL + ComNum.VBLF + "   AND PATIENT_NO = '" + strPATID + "'  ";
            SQL = SQL + ComNum.VBLF + "   AND DEPT_CODE = '" + clsType.User.DeptCode + "'  ";
            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt1.Rows.Count == 0)
            {
                dt1.Dispose();
                dt1 = null;
            }
            else
            {
                dt1.Dispose();
                dt1 = null;
                return true;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM MED_OCS.IPD_PATIENT_IN_WARD ";
            SQL = SQL + ComNum.VBLF + " WHERE IN_OR_OUT = '1'  ";
            SQL = SQL + ComNum.VBLF + "   AND PATIENT_NO = '" + strPATID + "'  ";
            SQL = SQL + ComNum.VBLF + "   AND MEDDEPTCD = '" + clsType.User.DeptCode + "'  ";
            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt1.Rows.Count == 0)
            {
                dt1.Dispose();
                dt1 = null;
            }
            else
            {
                dt1.Dispose();
                dt1 = null;
                return true;
            }
            
            if (dt.Rows[0]["CLASS"].ToString().Trim() == "I")
            {
                // 컨설터 적용 인경우엔  과와 같으면 바로 패스 하장 
                SQL = "";
                SQL = "SELECT *  FROM  MED_OCS.IPD_CONSULTNOPAPER ";
                SQL = SQL + ComNum.VBLF + " WHERE  PATIENT_NO  =  '" + strPATID + "' AND  Inpt_Stat IN (  'A'  ,'C' )  AND  OPDIPD = 'I'    ";
                SQL = SQL + ComNum.VBLF + "  AND  TO_CHAR(IP_DATE,'YYYYMMDD')    >= '" + strIPDATE + "' ";
                SQL = SQL + ComNum.VBLF + "  AND  CON_DEPTCD  =  '" + clsType.User.DeptCode + "'  ";
                SQL = SQL + ComNum.VBLF + "  AND  REQ_DEPTCD  = '" + strMedDeptCd.Trim() + "' "; //  AND  IP_DATE    >= " + ComFunc.ConvOraToDate(strIPDATE, "D");
                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                }
                else
                {
                    dt1.Dispose();
                    dt1 = null;
                    return true;
                }
            }

            dt.Dispose();
            dt = null;

            // ///////////////////////
            // 2016-07-19 나중에 손바야 한다  실제로 오늘 진료인 경우만 처리 해야 하지 않나 
            // 2016-07-28  최규천 응급실도 진료 목적임으로 다 풀어주자 실행 파일명 MHEREMRDG.EXE"
            // 외래 부분은 우선 응급실에 확인 후에 풀어 줌 보류
            //if (strMedDeptCd == "EM" || clsCommon.gstrEXENAME == "MHEREMRDG.EXE")
            //{
            //    return true;
            //}

            if (clsType.User.DeptCode == strMedDeptCd)
            {
                return true;
            }
            else
            {

                // 2016-07-19 보안 
                // 현재 진료과를 비교를 한다.
                SQL = "  ";
                SQL = "SELECT SECURE_YN  FROM " + ComNum.DB_PMPA + "BAS_PATIENT  ";
                SQL = SQL + ComNum.VBLF + " WHERE  PATIENT_NO  = '" + strPATID + "'  ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }

                string strPV = dt.Rows[0]["SECURE_YN"].ToString().Trim();

                dt.Dispose();
                dt = null;
                ///////////////////////////////////////

                if (strPV == "1")
                {
                    ComFunc.MsgBox("P.V신청 환자는 차트 조회가 불가능 합니다.");
                    return false;
                }

                if (strMedDeptCd == "UR" || strMedDeptCd == "NP" || strMedDeptCd == "OB")
                {
                    SQL = " SELECT * FROM MED_PMPA.OPD_MAST ";
                    SQL = SQL + ComNum.VBLF + " WHERE PATIENT_NO = '" + strPATID + "'  ";
                    SQL = SQL + ComNum.VBLF + " AND ACT_DATE = TRUNC(SYSDATE) ";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count >= 1)
                    {
                        dt.Dispose();
                        dt = null;
                        return true;
                    }

                    ComFunc.MsgBox("민감정보임으로 타과에서는 챠트 조회가 불가능 합니다.");
                    return false;
                }
                else
                {
                    //if (clsType.User.strDeptCd == "ED" || clsType.User.strDeptCd == "PU" || clsType.User.strDeptCd == "GI" || clsType.User.strDeptCd == "HO" || clsType.User.strDeptCd == "KH" || clsType.User.strDeptCd == "CV" || clsType.User.strDeptCd == "IM")
                    //{
                    //    if (strMedDeptCd == "ED" || strMedDeptCd == "PU" || strMedDeptCd == "GI" || strMedDeptCd == "HO" || strMedDeptCd == "KH" || strMedDeptCd == "CV" || strMedDeptCd == "IM")
                    //    {
                    //        return true;
                    //    }

                    //}
                }
            }

            SQL = "";
            SQL = "SELECT ACPNO, REQUSEID FROM " + ComNum.DB_EMR + "AEMRCHARTREQADM ";
            SQL = SQL + ComNum.VBLF + "WHERE  ACPNO = " + VB.Val(strAcpNo);
            SQL = SQL + ComNum.VBLF + "    AND  REQUSEID = '" + clsType.User.IdNumber + "'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                return true;
            }
            dt.Dispose();
            dt = null;

            EmrPatient tmpP = clsEmrChart.SetEmrPatInfo(pDbCon, strAcpNo);

            //frmChartViewReq frm = new frmChartViewReq(tmpP);
            //frm.TopMost = true;
            //frm.ShowDialog();

            SQL = "";
            SQL = "SELECT ACPNO, REQUSEID FROM " + ComNum.DB_EMR + "AEMRCHARTREQADM ";
            SQL = SQL + ComNum.VBLF + "WHERE  ACPNO = " + VB.Val(strAcpNo);
            SQL = SQL + ComNum.VBLF + "    AND  REQUSEID = '" + clsType.User.IdNumber + "'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                return true;
            }
            dt.Dispose();
            dt = null;
            ComFunc.MsgBox("환자 진료정보 접근 사유 입력를 입력해 주십시요.");
            return rtnVal;
        }

        /// <summary>
        /// EMR 사용자 옵션 가지고 오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <returns></returns>
        public static DataTable GetEmrUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strAplDate = "";

            strAplDate = ComQuery.CurrentDateTime(pDbCon, "D");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM";
            SQL = SQL + ComNum.VBLF + "    " + ComNum.DB_EMR + "ABUSEROPTION";
            SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + strUseId + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }
            return dt;
        }

        /// <summary>
        /// 진료 및 EMR 관련 사용자 정의를 저장한다.
        /// </summary>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <param name="strOPTVALUE"></param>
        /// <returns></returns>
        public static bool SetEmrUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB, string strOPTVALUE)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strCurDateTime = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            

            try
            {
                strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + strUseId + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "    (USEID, OPTCD, OPTGB, OPTVALUE, WRITEDATE, WRITETIME)";
                SQL = SQL + ComNum.VBLF + "  VALUES(";
                SQL = SQL + ComNum.VBLF + "      '" + strUseId + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTCD + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// EMR 관련 사용자 정의를 저장한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <param name="strOPTVALUE"></param>
        /// <param name="strVALUE"></param>
        /// <returns></returns>
        public static bool SetEmrUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB, string strOPTVALUE, string strVALUE)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strCurDateTime = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);
            
            try
            {
                strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + strUseId + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "ABUSEROPTION";
                SQL = SQL + ComNum.VBLF + "    (USEID, OPTCD, OPTGB, OPTVALUE, VALUE, WRITEDATE, WRITETIME)";
                SQL = SQL + ComNum.VBLF + "  VALUES(";
                SQL = SQL + ComNum.VBLF + "      '" + strUseId + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTCD + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 진료의 정보를 가지고 온다
        /// </summary>
        /// <param name="strSubSql"></param>
        /// <returns></returns>
        public static DataTable GetMedDrInfo(PsmhDb pDbCon, string strSubSql = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "  DRCODE, ";
            SQL = SQL + ComNum.VBLF + "  DRNAME,";
            SQL = SQL + ComNum.VBLF + "  DEPTCODE,";
            SQL = SQL + ComNum.VBLF + "  GRADE,";
            SQL = SQL + ComNum.VBLF + "  DOCCODE,";
            SQL = SQL + ComNum.VBLF + "  SORT";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DOCTOR ";
            SQL = SQL + ComNum.VBLF + strSubSql;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("OCS_DOCTOR 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 진료과 정보를 가지고 온다
        /// </summary>
        /// <param name="strSubSql">서브쿼리문(조건 등)</param>
        /// <returns></returns>
        public static DataTable GetMedDeptInfo(PsmhDb pDbCon, string strSubSql = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "SELECT MEDDEPTCD, DEPTKORNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "VIEWBMEDDEPT ";
            SQL = SQL + ComNum.VBLF + strSubSql;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("VIEWBMEDDEPT 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 재원환자 여부 체크
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        public static DataTable GetIpdPtInfo(PsmhDb pDbCon, string strPtNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT   * ";
            SQL = SQL + ComNum.VBLF + "     FROM   MED_OCS.INPATIENT_BY_WARD ";
            SQL = SQL + ComNum.VBLF + "     WHERE PATIENT_NO = '" + strPtNo + "' "; ;
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("INPATIENT_BY_WARD 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 사용중인 기록지의 이전서식을 제외한 최고 버젼을 가지고 온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pFORMNO"></param>
        /// <returns></returns>
        public static double GetMaxNewUpdateNo(PsmhDb pDbCon, double pFORMNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            double rtnVal = -1;

            SQL = " SELECT   MAX(UPDATENO) AS UPDATENO";
            SQL = SQL + ComNum.VBLF + "FROM   " + ComNum.DB_EMR + "AEMRFORM ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + pFORMNO.ToString() + " ";
            SQL = SQL + ComNum.VBLF + "  AND OLDGB = '0'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("GetMaxUpdateNo 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                return -1;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return -1;
            }

            rtnVal = VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 사용중인 기록지의 최고 버젼을 가지고 온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pFORMNO"></param>
        /// <returns></returns>
        public static double GetMaxUpdateNo(PsmhDb pDbCon, double pFORMNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            double rtnVal = -1;

            SQL = " SELECT   MAX(UPDATENO) AS UPDATENO";
            SQL = SQL + ComNum.VBLF + "FROM   " + ComNum.DB_EMR + "AEMRFORM ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + pFORMNO.ToString() + " ";
            SQL = SQL + ComNum.VBLF + "  AND USECHECK = 1 ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("GetMaxUpdateNo 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                return -1;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return -1;
            }

            rtnVal = VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 신규기록지중에 최근 기록지 번호를 읽는다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="dblFormNo"></param>
        /// <returns></returns>
        public static double GetNewFormMaxUpdateNo(PsmhDb pDbCon, double dblFormNo)
        {
            double rtnVal = 1;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT MAX(UPDATENO) AS UPDATENO ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL += ComNum.VBLF + "WHERE FORMNO = " + dblFormNo;
                //SQL += ComNum.VBLF + "    AND USECHECK = 1 ";
                SQL += ComNum.VBLF + "    AND OLDGB = '0' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                rtnVal = VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 외래 접수 정보의 의사 코드를 가지고 온다.
        /// mhemrview / frmTextEmrHistory / ReadDeptDoctor
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pFORMNO"></param>
        /// <returns></returns>
        public static string ReadDeptDoctor(PsmhDb pDbCon, string ArgBDate, string ArgDeptCode, string argPTNO)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string rtnVal = "";

            SQL = " SELECT DRCODE FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL = SQL + ComNum.VBLF + "WHERE BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + ArgDeptCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND PANO = '" + argPTNO + "' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("OPD_MASTER 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                return "";
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return "";
            }

            rtnVal = dt.Rows[0]["DRCODE"].ToString().Trim();
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// 당일 중복으로 차트 복사 신청을 했는지 조회
        /// mhemrview / frmTextEmrHistory / READ_DUPLICATE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argEMRNO"></param>
        /// <param name="argREQDATE"></param>
        /// <param name="argGBN"></param>
        /// <param name="argPRTOPTION"></param>
        /// <returns></returns>
        public static bool READ_DUPLICATE(PsmhDb pDbCon, string argPTNO, string argEMRNO, string argREQDATE, string argGBN, string argPRTOPTION)
        {
            string SQL  = string.Empty;
            string SqlErr = string.Empty; ; //에러문 받는 변수
            DataTable dt = null;
            bool rtnVal = false;

            if (argGBN == "O")
            {
                SQL = " SELECT EMRNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRPRTREQ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND REQDATE = '" + argREQDATE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PRTOPTION = '" + argPRTOPTION + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("EMRPRTREQ 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }
                string[] strTEMP = VB.Split(argPRTOPTION, "^");

                string strDate = VB.Left(strTEMP[4], 4) + "-" + VB.Mid(strTEMP[4], 5, 2) + "-" + VB.Right(strTEMP[4], 2);
                string strDept = strTEMP[2];
                dt.Dispose();

                ComFunc.MsgBox("★오더일 : " + strDate + "  ★진료과 : " + strDept + ComNum.VBLF + ComNum.VBLF + "  해당일자, 진료과 오더는 이미 복사신청을 하였습니다.");

                rtnVal = true;
            }
            else
            {
                SQL = " SELECT EMRNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRPRTREQ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND REQDATE = '" + argREQDATE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND EMRNO = " + argEMRNO;
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("EMRPRTREQ 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                dt.Dispose();

                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <returns></returns>
        public static string EmrGetUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strAplDate = "";

            string rtnVal = "";

            strAplDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "   OPTCD, OPTGB, OPTVALUE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRUSEROPTION";
            SQL = SQL + ComNum.VBLF + "WHERE USEID = '" + strUseId + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
            SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            rtnVal = dt.Rows[0]["OPTVALUE"].ToString().Trim();
            dt.Dispose();
            dt = null;
            return rtnVal;
        }
        

        /// <summary>
        /// 투약기록지 작성 쿼리
        /// </summary>
        /// <param name="spd">작성 스프레드</param>
        /// <param name="dblEmrNo">EMRNO</param>
        /// <param name="ChartDate">작성날짜</param>
        /// <param name="ChartTime">작성시간</param>
        /// <param name="emrPatient"></param>
        /// <returns></returns>
        public static bool Insert_EMR_TuYak(FarPoint.Win.Spread.FpSpread spd, string ChartDate, string ChartTime, EmrPatient emrPatient)
        {
            StringBuilder SQL = new StringBuilder();
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                double dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");

                SQL.AppendLine(" INSERT INTO " + ComNum.DB_EMR + "EMRXML_" + "TUYAK" + " (");
                SQL.AppendLine(" EMRNO, FORMNO, USEID, CHARTDATE, CHARTTIME, ");
                SQL.AppendLine(" ACPNO, PTNO, INOUTCLS, MEDFRDATE, MEDFRTIME, ");
                SQL.AppendLine(" MEDENDDATE, MEDENDTIME, MEDDEPTCD, MEDDRCD, MIBICHECK, ");
                SQL.AppendLine(" WRITEDATE, WRITETIME, IT1, IT2, ");
                SQL.AppendLine(" IT3, IT4, IT5, IT6, IT7, ");
                SQL.AppendLine(" IT8, IT9, IT10) VALUES ( ");
                SQL.AppendLine(dblEmrNo + ", 1796, '" + clsType.User.IdNumber + "','" + ChartDate + "','" + ChartTime + "',");
                SQL.AppendLine("0, '" + emrPatient.ptNo + "','" + emrPatient.inOutCls + "','" + emrPatient.medFrDate + "','" + emrPatient.medFrTime + "',");
                SQL.AppendLine("'" + emrPatient.medEndDate + "','" + emrPatient.medEndTime + "','" + emrPatient.medDeptCd + "','" + emrPatient.medDrCd + "','0', ");
                SQL.AppendLine("TO_CHAR(SYSDATE, 'YYYYMMDD'), TO_CHAR(SYSDATE, 'HH24MISS'), '" + spd.ActiveSheet.Cells[0, 2].Text.Trim().Replace("'", "`") + "','" + spd.ActiveSheet.Cells[0, 3].Text.Trim().Replace("'", "`") + "', ");
                SQL.AppendLine("'" + spd.ActiveSheet.Cells[0, 4].Text.Trim().Replace("'", "`") + "','" + spd.ActiveSheet.Cells[0, 5].Text.Trim().Replace("'", "`") + "','" + spd.ActiveSheet.Cells[0, 6].Text.Trim().Replace("'", "`") + "','" + spd.ActiveSheet.Cells[0, 7].Text.Trim().Replace("'", "`") + "','" +
                    spd.ActiveSheet.Cells[0, 8].Text.Trim().Replace("'", "`") + "', ");
                SQL.AppendLine("'" + spd.ActiveSheet.Cells[0, 9].Text.Trim().Replace("'", "`") + "','" + spd.ActiveSheet.Cells[0, 10].Text.Trim().Replace("'", "`") + "','" + spd.ActiveSheet.Cells[0, 11].Text.Trim().Replace("'", "`") + "')");


                string SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }


        /// <summary>
        /// 기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
        /// </summary>
        /// <param name="strEmrNo">EmrNo</param>
        /// <returns></returns>
        public static bool DeleteEmrXmlData(string strEmrNo)
        {
            StringBuilder SQL = new StringBuilder();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'" + ComNum.DB_EMR + "EMRXMLHISTORY_HISTORYNO_SEQ
                double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "EMRXMLHISNO");
                int intRowAffected = 0;

                string strFormNo = string.Empty;

                OracleDataReader reader = null;

                SQL.AppendLine(" SELECT FORMNO ");
                SQL.AppendLine(" FROM " + ComNum.DB_EMR + "EMRXMLMST ");
                SQL.AppendLine(" WHERE EMRNO = " + strEmrNo);

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    return false;
                }

                if (reader.HasRows && reader.Read())
                {
                    strFormNo = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

                if (strFormNo.Length == 0)
                {
                    SQL.Clear();
                    SQL.AppendLine(" SELECT FORMNO ");
                    SQL.AppendLine(" FROM " + ComNum.DB_EMR + "EMRXML_TUYAK ");
                    SQL.AppendLine(" WHERE EMRNO = " + strEmrNo);

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                        return false;
                    }

                    strFormNo = reader.HasRows && reader.Read() ? "1796" : string.Empty;

                    reader.Dispose();
                }


                if (strFormNo == "1796")
                {
                    SQL.Clear();
                    SQL.AppendLine(" INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY_TUYAK");
                    SQL.AppendLine("      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,");
                    SQL.AppendLine("      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,");
                    SQL.AppendLine("      WRITEDATE,WRITETIME,HISTORYWRITEDATE,HISTORYWRITETIME, ");
                    SQL.AppendLine("      DELUSEID,CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10)");
                    SQL.AppendLine(" SELECT  " + dblEmrHisNo + ",");
                    SQL.AppendLine("      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,");
                    SQL.AppendLine("      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,");
                    SQL.AppendLine("      WRITEDATE,WRITETIME, ");
                    SQL.AppendLine("      TO_CHAR(SYSDATE,'YYYYMMDD') , TO_CHAR(SYSDATE,'HH24MMSS') ,  ");
                    SQL.AppendLine("       '" + clsType.User.Sabun + "',CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10");
                    SQL.AppendLine(" FROM " + ComNum.DB_EMR + "EMRXML_TUYAK");
                    SQL.AppendLine("  WHERE EMRNO = " + VB.Val(strEmrNo));

                    SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }


                    SQL.Clear();
                    SQL.AppendLine(" DELETE FROM " + ComNum.DB_EMR + "EMRXML_TUYAK");
                    SQL.AppendLine("  WHERE EMRNO = " + VB.Val(strEmrNo));

                    SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }



                SQL.Clear();
                SQL.AppendLine(" INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY");
                SQL.AppendLine("      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,");
                SQL.AppendLine("      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,");
                SQL.AppendLine("      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME, UPDATENO,");
                SQL.AppendLine("      DELUSEID,CERTNO)");
                SQL.AppendLine(" SELECT  " + dblEmrHisNo + ",");
                SQL.AppendLine("      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,");
                SQL.AppendLine("      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,");
                SQL.AppendLine("      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, ");
                SQL.AppendLine("      TO_CHAR(SYSDATE,'YYYYMMDD') , TO_CHAR(SYSDATE,'HH24MMSS') , UPDATENO, ");
                SQL.AppendLine("       '" + clsType.User.Sabun + "',CERTNO");
                SQL.AppendLine(" FROM " + ComNum.DB_EMR + "EMRXML");
                SQL.AppendLine("  WHERE EMRNO = " + strEmrNo);

                SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }


                SQL.Clear();
                SQL.AppendLine("INSERT INTO " + ComNum.DB_EMR + "EMRXMLMST_HISTORY");
                SQL.AppendLine("      ( EMRNO, PTNO, GBEMR, ");
                SQL.AppendLine("      FORMNO, USEID, CHARTDATE, ");
                SQL.AppendLine("      CHARTTIME, INOUTCLS, MEDFRDATE,");
                SQL.AppendLine("      MEDFRTIME, MEDENDDATE, MEDENDTIME, ");
                SQL.AppendLine("      MEDDEPTCD, MEDDRCD, WRITEDATE, ");
                SQL.AppendLine("      WRITETIME )");
                SQL.AppendLine("SELECT ");
                SQL.AppendLine("      EMRNO, PTNO, GBEMR, ");
                SQL.AppendLine("      FORMNO, USEID, CHARTDATE, ");
                SQL.AppendLine("      CHARTTIME, INOUTCLS, MEDFRDATE,");
                SQL.AppendLine("      MEDFRTIME, MEDENDDATE, MEDENDTIME, ");
                SQL.AppendLine("      MEDDEPTCD, MEDDRCD, WRITEDATE, ");
                SQL.AppendLine("      WRITETIME ");
                SQL.AppendLine(" FROM " + ComNum.DB_EMR + "EMRXMLMST ");
                SQL.AppendLine("  WHERE EMRNO = " + strEmrNo);

                SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }


                SQL.Clear();
                SQL.AppendLine(" DELETE FROM " + ComNum.DB_EMR + "EMRXML");
                SQL.AppendLine("  WHERE EMRNO = " + strEmrNo);

                SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL.Clear();
                SQL.AppendLine(" DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST");
                SQL.AppendLine("  WHERE EMRNO = " + strEmrNo);

                SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// 기록지 저장
        /// </summary>
        /// <param name="strEmrNo">EmrNo</param>
        /// <param name="strFormNo">폼번호</param>
        /// <param name="strUseId">작성자 아이디</param>
        /// <param name="strChartDate">작성날짜</param>
        /// <param name="strChartTime">작성시간</param>
        /// <param name="strAcpNo">Acpno</param>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strInOutCls">외래,입원 구분</param>
        /// <param name="strMedFrDate">입원날짜</param>
        /// <param name="strMedFrTime">입원시간</param>
        /// <param name="strMedEndDate">퇴원날짜</param>
        /// <param name="strMedEndTime">퇴원시간</param>
        /// <param name="strMedDeptCd">과</param>
        /// <param name="strMedDrCd">의사</param>
        /// <param name="strXML">XML Data</param>
        /// <param name="strUPDATENO">기록지 최종 업데이트번호</param>
        /// <returns></returns>
        public static bool SaveEmrXmlData(string strEmrNo, string strFormNo,
                            string strChartDate, string strChartTime, string strAcpNo,
                            string strPtNo, string strInOutCls, string strMedFrDate,
                            string strMedFrTime, string strMedEndDate, string strMedEndTime,
                            string strMedDeptCd, string strMedDrCd, string strXML,
                            string strUPDATENO = "1"
                            )
        {
            bool rtnVal = false;

            string writeDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string writeTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");
            StringBuilder SQL = new StringBuilder();
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL.AppendLine(" INSERT INTO " + ComNum.DB_EMR + "EMRXMLMST");
                SQL.AppendLine("      ( ");
                SQL.AppendLine("      EMRNO, PTNO, GBEMR, ");
                SQL.AppendLine("      FORMNO, USEID, CHARTDATE, CHARTTIME, ");
                SQL.AppendLine("      INOUTCLS, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, ");
                SQL.AppendLine("      MEDDEPTCD, MEDDRCD, WRITEDATE, WRITETIME");
                SQL.AppendLine("      ) ");
                SQL.AppendLine("      VALUES (");
                SQL.AppendLine("      " + strEmrNo.Trim() + ",");
                SQL.AppendLine("      '" + strPtNo.Trim() + "',");
                SQL.AppendLine("      '1' ,");
                SQL.AppendLine("      " + strFormNo + ",");
                SQL.AppendLine("      '" + VB.Val(clsType.User.IdNumber) + "',");
                SQL.AppendLine("      '" + strChartDate.Trim() + "',");
                SQL.AppendLine("      '" + strChartTime.Trim() + "',");
                SQL.AppendLine("      '" + strInOutCls.Trim() + "',");
                SQL.AppendLine("      '" + strMedFrDate.Trim() + "',");
                SQL.AppendLine("      '" + strMedFrTime.Trim() + "',");
                SQL.AppendLine("      '" + strMedEndDate.Trim() + "',");
                SQL.AppendLine("      '" + strMedEndTime.Trim() + "',");
                SQL.AppendLine("      '" + strMedDeptCd.Trim() + "',");
                SQL.AppendLine("      '" + strMedDrCd.Trim() + "',");
                SQL.AppendLine("      '" + writeDate.Trim() + "',");
                SQL.AppendLine("      '" + writeTime.Trim() + "'");
                SQL.AppendLine("      )");

                string sqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
                if (sqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL.Clear();
                SQL.AppendLine(" INSERT INTO " + ComNum.DB_EMR + "EMRXML");
                SQL.AppendLine("      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,");
                SQL.AppendLine("      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,");
                SQL.AppendLine("      WRITEDATE,WRITETIME,CHARTXML,UPDATENO) ");
                SQL.AppendLine("      VALUES (");
                SQL.AppendLine("      " + strEmrNo.Trim() + ",");
                SQL.AppendLine("      " + strFormNo.Trim() + ",");
                SQL.AppendLine("      '" + VB.Val(clsType.User.IdNumber) + "',");
                SQL.AppendLine("      '" + strChartDate.Trim() + "',");
                SQL.AppendLine("      '" + strChartTime.Trim() + "',");
                SQL.AppendLine("      '" + strAcpNo.Trim() + "',");
                SQL.AppendLine("      '" + strPtNo.Trim() + "',");
                SQL.AppendLine("      '" + strInOutCls.Trim() + "',");
                SQL.AppendLine("      '" + strMedFrDate.Trim() + "',");
                SQL.AppendLine("      '" + strMedFrTime.Trim() + "',");
                SQL.AppendLine("      '" + strMedEndDate.Trim() + "',");
                SQL.AppendLine("      '" + strMedEndTime.Trim() + "',");
                SQL.AppendLine("      '" + strMedDeptCd.Trim() + "',");
                SQL.AppendLine("      '" + strMedDrCd.Trim() + "',");
                SQL.AppendLine("      '" + writeDate.Trim() + "',");
                SQL.AppendLine("      '" + writeTime.Trim() + "',");
                SQL.AppendLine("      :1,");
                SQL.AppendLine("      '" + strUPDATENO.Trim() + "')");

                sqlErr = clsDB.ExecuteXmlQuery(SQL.ToString().Trim(), strXML, ref RowAffected, clsDB.DbCon);
                if(sqlErr.Length > 0 || RowAffected == 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception e)
            {
                clsDB.SaveSqlErrLog(e.Message, SQL.ToString().Trim(), clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                return rtnVal;
            }
        }


        /// <summary>
        /// 이전 기록지 저장 방법
        /// </summary>
        /// <param name="strEmrNo">EmrNo</param>
        /// <param name="strFormNo">폼번호</param>
        /// <param name="strUseId">작성자 아이디</param>
        /// <param name="strChartDate">작성날짜</param>
        /// <param name="strChartTime">작성시간</param>
        /// <param name="strAcpNo">Acpno</param>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strInOutCls">외래,입원 구분</param>
        /// <param name="strMedFrDate">입원날짜</param>
        /// <param name="strMedFrTime">입원시간</param>
        /// <param name="strMedEndDate">퇴원날짜</param>
        /// <param name="strMedEndTime">퇴원시간</param>
        /// <param name="strMedDeptCd">과</param>
        /// <param name="strMedDrCd">의사</param>
        /// <param name="strXML">XML Data</param>
        /// <param name="strUPDATENO">기록지 최종 업데이트번호</param>
        /// <returns></returns>
        public static bool OldSaveEmrXmlData(string strEmrNo, string strFormNo, string strUseId,
                            string strChartDate, string strChartTime, string strAcpNo,
                            string strPtNo, string strInOutCls, string strMedFrDate,
                            string strMedFrTime, string strMedEndDate, string strMedEndTime,
                            string strMedDeptCd, string strMedDrCd, string strXML,
                            string strUPDATENO = "1"
                            )
        {
            bool rtnVal = false;

            int Result = 0;
            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "" + ComNum.DB_EMR + "XMLINSRT3";
            cmd.CommandType = CommandType.StoredProcedure;

            string writeDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            string writeTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            try
            {
                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, strEmrNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, VB.Val(strFormNo), ParameterDirection.Input);
                cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, strUseId, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strChartDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, strChartTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, strAcpNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPtNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, strInOutCls, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strMedFrDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, strMedFrTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, strMedEndDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, strMedEndTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strMedDeptCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strMedDrCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, writeDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, writeTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, strUPDATENO, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTXML", OracleDbType.Clob, strXML.Length, strXML, ParameterDirection.Input);

                Result = cmd.ExecuteNonQuery();

                rtnVal = true;
                return rtnVal;
            }
            catch
            {
                return rtnVal;
            }
        }

        /// <summary>
        /// 서식지에 있는 이미지를 조회 한다.
        /// </summary>
        /// <param name="formNo"></param>
        /// <returns></returns>
        public static DataTable GetFormImages(string formNo, string updateNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL += "SELECT FORMNO                                           \r\n";
            SQL += "     , UPDATENO                                         \r\n";
            SQL += "     , CONTROLNAME                                      \r\n";
            SQL += "     , IMAGE                                            \r\n";
            SQL += "  FROM " + ComNum.DB_EMR + "AEMRFORMXMLIMAGE                      \r\n";
            SQL += " WHERE FORMNO   = '" + formNo + "'                      \r\n";
            SQL += "   AND UPDATENO = '" + updateNo + "'                    \r\n";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("AEMRFORMXMLIMAGE 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                return null;
            }

            return dt;
        }



        public static double SaveNurseRecord(PsmhDb pDbCon, EmrPatient po, string strFormNo, string strUpdateNo, string strEmrNo, string strChartDate, string strChartTime,
                                string strCHARTUSEID, string strCOMPUSEID, string strSAVEGB, string strSAVECERT, string strFORMGB, string strSaveFlag,
                                string strProblem, string strType, string strNrRecode, string strProblemCode, string strWard, string strRoomCode, bool trans = true)
        {
            double rtnVal = 0;
            //string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            double dblEmrHisNo = 0; //무저건 발생한다
            double dblEmrNoNew = 0;

            if (strEmrNo == "0")
            {
                if (isAuth24Hour(pDbCon, po, strFormNo, strCHARTUSEID) == false)
                {
                    return 0;
                }
            }

            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            if (trans)
            {
                clsDB.setBeginTran(pDbCon);
            }

            try
            {
                // EMRNOHIS 시퀀스 생성                
                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                if (VB.Val(strEmrNo) > 0)
                {
                    //// AEMRCHARTMST 백업 및 삭제
                    SqlErr = SaveChartMastHis(pDbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", strSaveFlag, strCHARTUSEID);
                    if (SqlErr != "OK")
                    {
                        if (trans)
                        {
                            clsDB.setRollbackTran(pDbCon);
                        }
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    dblEmrNoNew = VB.Val(strEmrNo);
                }
                else
                {
                    // EMRNO 시퀀스 생성
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                if (strSAVECERT.Equals("0"))
                {
                    strSAVECERT = "1";
                }

                // AEMRCHARTMST 저장
                if (clsEmrQuery.SaveChartMstOnly(pDbCon, dblEmrNoNew, dblEmrHisNo, po, strFormNo, strUpdateNo, strChartDate, strChartTime,
                    strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB, strCurDate, strCurTime, strSaveFlag) == false)
                {
                    if (trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    ComFunc.MsgBox("AEMRCHARTMST 저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //if (VB.Val(strEmrNo) > 0)
                //{
                //    //// AEMRNURSRECORD 백업 및 삭제
                //    SqlErr = SaveNurNurseRecordHis(pDbCon, strEmrNo);
                //    if (SqlErr != "OK")
                //    {
                //        clsDB.setRollbackTran(pDbCon);
                //        ComFunc.MsgBox(SqlErr);
                //        Cursor.Current = Cursors.Default;
                //        return rtnVal;
                //    }
                //}

                // AEMRNURSRECORD 저장
                if (SaveNurNurseRecord(pDbCon, strWard, strRoomCode, dblEmrNoNew, dblEmrHisNo, strProblem, strType, strNrRecode, strProblemCode) == false)
                {
                    if (trans)
                    {
                        clsDB.setRollbackTran(pDbCon);
                    }
                    ComFunc.MsgBox("AEMRNURSRECORD 저장 중 에러가 발생했습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (trans)
                {
                    clsDB.setCommitTran(pDbCon);
                }
                Cursor.Current = Cursors.Default;

                #region //전자인증 하기
                //if (strSAVECERT == "1")
                //{
                    if (System.Diagnostics.Debugger.IsAttached == false)
                    {
                        bool blnCert = true;
                        if (dblEmrNoNew > 0)
                        {
                            if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                            {
                                blnCert = SaveEmrCert(pDbCon, dblEmrNoNew, trans);
                            }
                        }
                    }
                //}

                #endregion

                return dblEmrNoNew;
            }
            catch (Exception ex)
            {
                if (trans)
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return 0;
            }

            

            #region //추후 필요시 코딩을 살려서 적용한다
            //if (dblEmrNoNew != 0)
            //{
            //    conTREmrNo = ChartForm.Controls.Find("lblEmrNoTR", true);

            //    if (conTREmrNo != null)
            //    {
            //        if (conTREmrNo.Length == 1)
            //        {
            //            if (conTREmrNo[0] is Label)
            //            {
            //                strEmrNoTR = ((Label)conTREmrNo[0]).Text.Trim();
            //            }
            //        }
            //    }

            //    AemrMapping(pDbCon, po, strFormNo, strEmrNo, Convert.ToString(Convert.ToInt32(dblEmrNoNew)), strEmrNoTR);

            //    if (AddSignCheck(pDbCon, strEmrNo) == true)
            //    {
            //        UpDateAddSign(pDbCon, strEmrNo, dblEmrNoNew);
            //    }
            //}
            #endregion
        }

        public static bool DeleteNurseRecord(PsmhDb pDbCon, string strEmrNo)
        {
            bool rtnVal = false;
            //string SQL = "";
            string SqlErr = string.Empty; //에러문 받는 변수

            double dblEmrHisNo = 0; //무저건 발생한다
            //double dblEmrNoNew = 0;

            if (strEmrNo == "0")
            {
                return rtnVal;
            }

            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            clsDB.setBeginTran(pDbCon);

            try
            {
                // EMRNOHIS 시퀀스 생성                
                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                // AEMRCHARTMST 백업 및 삭제
                SqlErr = SaveChartMastHis(pDbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", "", clsType.User.IdNumber);
                if (SqlErr != "OK")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //// AEMRNURSRECORD 백업 및 삭제
                //SqlErr = SaveNurNurseRecordHis(pDbCon, strEmrNo);
                //if (SqlErr != "OK")
                //{
                //    clsDB.setRollbackTran(pDbCon);
                //    ComFunc.MsgBox(SqlErr);
                //    Cursor.Current = Cursors.Default;
                //    return rtnVal;
                //}

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public static bool SaveNurNurseRecord(PsmhDb pDbCon, string strWard, string strRoomCode, double dblEmrNo, double dblEmrNoHis, string strProblem, string strType, string strNrRecode, string strProblemCode)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRNURSRECORD ";
                SQL = SQL + ComNum.VBLF + "( ";
                SQL = SQL + ComNum.VBLF + "     EMRNO, EMRNOHIS, WARDCODE, ROOMCODE, PROBLEMCODE, PROBLEM, TYPE, NRRECODE";
                SQL = SQL + ComNum.VBLF + ") VALUES (";
                SQL = SQL + ComNum.VBLF + "  " + dblEmrNo + ",";
                SQL = SQL + ComNum.VBLF + "  " + dblEmrNoHis + ",";
                SQL = SQL + ComNum.VBLF + " '" + strWard + "',";
                SQL = SQL + ComNum.VBLF + " '" + strRoomCode + "',";
                SQL = SQL + ComNum.VBLF + " '" + strProblemCode + "',";
                SQL = SQL + ComNum.VBLF + " '" + strProblem + "',";
                SQL = SQL + ComNum.VBLF + " '" + strType + "',";
                SQL = SQL + ComNum.VBLF + " '" + strNrRecode + "')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        //public static string SaveNurNurseRecordHis(PsmhDb pDbCon, string strEmrNo)
        //{
        //    string rtnVal = "OK";
        //    string SQL = "";
        //    string SqlErr = ""; //에러문 받는 변수
        //    int intRowAffected = 0;

        //    try
        //    {
        //        //AEMRNURSRECORD
        //        SQL = "";
        //        SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRNURSRECORDHIS ";
        //        SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, WARDCODE, ROOMCODE, PROBLEMCODE, PROBLEM, TYPE, NRRECODE) ";
        //        SQL = SQL + ComNum.VBLF + "SELECT ";
        //        SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, WARDCODE, ROOMCODE, PROBLEMCODE, PROBLEM, TYPE, NRRECODE ";
        //        SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRNURSRECORD ";
        //        SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;

        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //        if (SqlErr != "")
        //        {
        //            rtnVal = SqlErr;
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return rtnVal;
        //        }
        //        SQL = "";
        //        SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRNURSRECORD";
        //        SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo;

        //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
        //        if (SqlErr != "")
        //        {
        //            rtnVal = SqlErr;
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return rtnVal;
        //        }

        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        rtnVal = SqlErr;
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return rtnVal;
        //    }
        //}

        public static bool Ins_OCS_IORDER_BST(EmrPatient AcpEmr, string argBDate, string MEASURE_DT = "", string GstrWardCode = "") 
        {
            #region 변수선언 
            bool rtnVal = true;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            int nSeq = 0;
            
            if (GstrWardCode == "ER")
            {
                AcpEmr.ward = "ER";
                AcpEmr.room = "100";
            }
            double cContents = 0;
            double cBContents = 0;
            double cRealNal = 1;
            double cNal = 1;
            double cGbDiv = 1;
            double nPRN_Unit = 0;
            double nPRN_Max = 0;
            string strTemp = "";
            long nOrderNo = 0;
            long nVOrderno = 1;

            string cSabun = "";
            string cRealQty = "1";
            string cQty = "1";            
            string cDosCode = "";
            string cGbInfo = "";
            string cGbSelf = " ";
            string cGbSpc = " ";
            string cGbNgt = " ";
            string cGbEr = " ";
            string cGbPRN = " ";
            string cGbBoth = "0";
            string cGbAct = " ";

            string cGbTFlag = " ";
            string cGbSend = " ";
            string cGbPosition = " ";
            string cGbStatus = " ";
            string cNurseID = clsType.User.Sabun;
            string cWardCode = AcpEmr.ward;
            string cRoomCode = AcpEmr.room;
            string cBi = AcpEmr.bi;
            string cRemark = "";
            string cActDate = clsPublic.GstrActDate;
            string cGbGroup = " ";
            string cGbPort = " ";
            string cOrderSite = clsPublic.GstrDeptCode.Trim();
            string cMulti = " ";
            string cMultiRemark = " ";
            string cDUR = "";
            string cMayak = "";
            string cPowder = "";
            string cSedation = "";
            string cMayakRemark = "";
            string cGbSPC_NO = "";
            string strGbTax = "";
            string strGbVerb = "N";
            string strVerbal_Doc = "";
            string strPrnRemark = "";
            string strPRN_Gbn = "";
            string strPRN_SDate = "";
            string strPRN_EDate = "";
            string strPowder_Sayu = "";
            string strASA = "";
            string strPRN_DosCode = "";
            string strPRN_Term = "";
            string strPRN_Notify = "";
            string strPRN_Unit = "";
            string strNurseER = "";
            string strPChasu = "";
            string strSUBUL_WARD = "";
            string strHighRiskGBN = "";
            string strER24 = "";
            string strGSADD = "";
            string cSuCode = "C3710";
            string cBun = "580";
            string strPRNORDSEQ = "";
            string strInWard = "";
            string strInId = "";
            string strInValue = "";

            string strEIChk = "";
            string strEIDeptcode = "";
            string strEIWardcode = "";
            string strEIRoomcode = "";


            string cOrderCode = "C3710"; //당분간 A26A로 고정 
            PsmhDb pDbCon = null;
            ComFunc CF = new ComFunc();
            clsExamMsg ExamMsg = new clsExamMsg();
            #endregion

            #region 시퀀스번호를 읽어온다
            SQL = "";
            SQL += ComNum.VBLF + "SELECT MAX(SEQNO) SEQNO";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND BDATE = TO_DATE('" + argBDate + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND PTNO = '" + AcpEmr.ptNo + "'";
            SQL += ComNum.VBLF + "  AND SEQNO <> '999'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("Ins_OCS_IORDER_BST " + "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                rtnVal = false;
                return rtnVal;
            }

            if(dt.Rows.Count > 0)
            {
                nSeq = Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim())) + 1;
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region 오더번호 생성
            SQL = "";
            SQL = "SELECT KOSMOS_OCS.SEQ_ORDERNO.NextVal NNEXTVAL FROM DUAL";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                rtnVal = false;
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                nOrderNo = (long)VB.Val(dt.Rows[0]["NNEXTVAL"].ToString());                
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region 검체번호 생성

            string s = string.Empty;
            string sDate = string.Empty;
            DataTable d = null;

            SQL = "SELECT  " + ComNum.DB_PMPA + "SEQ_EXAMSPECNO.NEXTVAL SPECNO FROM DUAL";


            SqlErr = clsDB.GetDataTableREx(ref d, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            if (d.Rows.Count > 0)
            {
                sDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                //string.Format("{0:00}"                    
                s = sDate.Substring(2, 2) + sDate.Substring(5, 2) + sDate.Substring(8, 2) + string.Format("{0:0000}", Convert.ToInt32(d.Rows[0]["SPECNO"].ToString()));
            }

            d.Dispose();
            d = null;
            #endregion

            #region 결과값을 읽어온다
            SQL = "";
            SQL += ComNum.VBLF + "SELECT NURSE_ID, VALUE, WARD";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_INTERFACE_BST";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND MEASURE_DT = '" + MEASURE_DT + "'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("Ins_OCS_IORDER_BST " + "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                rtnVal = false;
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                strInWard = dt.Rows[0]["WARD"].ToString().Trim();
                strInId = dt.Rows[0]["NURSE_ID"].ToString().Trim();
                strInValue = dt.Rows[0]["VALUE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region 인슐린 시작날짜를 읽어온다            

            SQL = "";
            SQL += ComNum.VBLF + "SELECT PRN_INS_SDATE ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND BDATE = TO_DATE('" + argBDate + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND PTNO = '" + AcpEmr.ptNo + "'";
            SQL += ComNum.VBLF + "  AND PRN_INS_SDATE IS NOT NULL";
            SQL += ComNum.VBLF + "ORDER BY PRN_INS_SDATE DESC";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("Ins_OCS_IORDER_BST " + "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                rtnVal = false;
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                strPRN_SDate = VB.Left(dt.Rows[0]["PRN_INS_SDATE"].ToString().Trim(), 10);
                nPRN_Unit = 1;
            }

            dt.Dispose();
            dt = null;

            #endregion

            #region 의사사번을 읽어온다            

            SQL = "";
            SQL += ComNum.VBLF + "SELECT SABUN ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND DRCODE = '" + AcpEmr.medDrCd  + "'";            
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("Ins_OCS_IORDER_BST " + "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                rtnVal = false;
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                cSabun = dt.Rows[0]["SABUN"].ToString().Trim();                
            }

            dt.Dispose();
            dt = null;

            #endregion

            if (string.IsNullOrEmpty(cOrderSite) == true)
            {
                cOrderSite = " ";
            }


            #region ER일 경우 저장시 입원환자 체크    
            if (GstrWardCode == "ER")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DEPTCODE, WARDCODE, ROOMCODE ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND PANO = '" + AcpEmr.ptNo + "'";
                SQL += ComNum.VBLF + "  AND INDATE < SYSDATE";
                SQL += ComNum.VBLF + "  AND OUTDATE IS NULL";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("Ins_OCS_IORDER_BST " + "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    rtnVal = false;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strEIChk = "Y";
                    strEIDeptcode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strEIWardcode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    strEIRoomcode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            #endregion

            #region INSERT 쿼리

            //SQL = "";
            //SQL += " INSERT INTO KOSMOS_OCS.OCS_IORDER                                          \r";
            //SQL += "        (Ptno, BDate,     Seqno,     DeptCode                               \r";
            //SQL += "      , DrCode,  StaffID,    Slipno,    OrderCode, SuCode,  Bun             \r";
            //SQL += "      , GbOrder, Contents,   BContents, RealQty,   Qty,     RealNal         \r";
            //SQL += "      , Nal,     DosCode,    GbInfo,    GbSelf,    GbSpc,   GbNgt           \r";
            //SQL += "      , GbER,    GbPRN,      GbDiv,     GbBoth,    GbAct,   GbTFlag         \r";
            //SQL += "      , GbSend,  GbPosition, GbStatus,  NurseID,   EntDate, WardCode        \r";
            //SQL += "      , RoomCode, Bi,        OrderNo,   Remark                              \r";
            //SQL += "      , ActDate, GbGroup,    GbPort,    OrderSite, Multi, MultiRemark       \r";
            //SQL += "      , Dur, GBPICKUP, PICKUPSABUN, PICKUPDATE , GBIOE, MAYAK, POWDER, SEDATION, CONSULT     \r";
            //SQL += "      , MayakRemark, VER, IP, GBSPC_NO,ENTDATE2                             \r";
            //SQL += "      , GbTax,GbVerb,Verbal,V_ORDERNO,PRN_REMARK,PRN_INS_GBN,PRN_INS_UNIT   \r";
            //SQL += "      , PRN_INS_SDATE,PRN_INS_EDATE,PRN_INS_Max,Powder_Sayu,ASA             \r";
            //SQL += "      , PRN_DOSCODE, PRN_TERM, PRN_NOTIFY,PRN_UNIT,AirSht, PCHASU           \r";
            //SQL += "      , SUBUL_WARD, HIGHRISK, ER24, GSADD                                   \r";
            //SQL += "      , CORDERCODE, CSUCODE, CBUN, PRN_ORDSEQ)                              \r";
            //SQL += " VALUES                                                                     \r";
            //SQL += "        ( '" + AcpEmr.ptNo + "', TO_DATE('" + argBDate + "','YYYY-MM-DD')   \r";

            //if(AcpEmr.medDeptCd != "" && AcpEmr.medDeptCd != null)
            //{
            //    SQL += "      , " + nSeq + ",     '" + AcpEmr.medDeptCd + "'                     \r";
            //}
            //else
            //{
            //    SQL += "      , " + nSeq + ",     '" + clsType.User.DeptCode + "'               \r";
            //}            

            //SQL += "      , '" + cSabun + "',  '" + AcpEmr.medDrCd + "',    '0010'   \r";
            //SQL += "      , '" + cOrderCode + "','C3710',     '58'                              \r";
            //SQL += "      , ' ',   " + cContents + ",     " + cBContents + "                    \r";
            //SQL += "      , '" + cRealQty + "',   " + cQty + ",          " + cRealNal + "       \r";
            //SQL += "      , " + cNal + ",       '" + cDosCode + "',    '" + cGbInfo + "'        \r";
            //SQL += "      , '" + cGbSelf + "',   '" + cGbSpc + "',      '" + cGbNgt + "'        \r";
            //SQL += "      , '" + cGbEr + "',     '" + cGbPRN + "',       " + cGbDiv + "         \r";
            //if (GstrWardCode == "ER")
            //{
            //    SQL += "      , '" + cGbBoth + "',   '*',      '" + cGbTFlag + "'      \r";
            //}
            //else
            //{
            //    SQL += "      , '" + cGbBoth + "',   '" + cGbAct + "',      '" + cGbTFlag + "'      \r";
            //}
            //SQL += "      , '" + cGbSend + "',   '" + cGbPosition + "', '" + cGbStatus + "'     \r";
            //SQL += "      , '" + cNurseID + "',  SysDate,   '" + cWardCode + "'                 \r";
            //SQL += "      , '" + cRoomCode + "', '" + cBi + "', " + nOrderNo + ", '" + cRemark + "'\r";
            //SQL += "      , TO_DATE('" + argBDate + "','YYYY-MM-DD'),   '" + cGbGroup + "'      \r";
            //SQL += "      , '" + cGbPort + "',   '" + cOrderSite + "' , '" + cMulti + "'        \r";
            //SQL += "      , '" + cMultiRemark + "', '" + cDUR + "'                              \r";         
            //if(GstrWardCode == "ER")
            //{
            //    SQL += "      , '*', '', '', 'E'                       \r";
            //}
            //else
            //{
            //    SQL += "      , '*', '" + clsType.User.IdNumber + "', SYSDATE, ''                       \r";
            //}
            //SQL += "      , '" + cMayak + "' , '" + cPowder + "', '" + cSedation + "'           \r";
            //SQL += "      , ''                                                                  \r";            
            //SQL += "      , '" + cMayakRemark + "', '', '" + clsPublic.GstrIpAddress + "' , '" + cGbSPC_NO + "',SYSDATE                 \r";
            //SQL += "      , '" + strGbTax + "','" + strGbVerb + "','" + strVerbal_Doc + "'," + nVOrderno + ",'" + strPrnRemark + "'     \r";
            //SQL += "      , '" + strPRN_Gbn + "'," + nPRN_Unit + ", TO_DATE('" + strPRN_SDate + "','YYYY-MM-DD')                        \r";
            //SQL += "      , TO_DATE('" + strPRN_EDate + "','YYYY-MM-DD') ," + nPRN_Max + ",'" + strPowder_Sayu + "','" + strASA + "'    \r";
            //SQL += "      , '" + strPRN_DosCode + "','" + strPRN_Term + "','" + strPRN_Notify + "' ,'" + strPRN_Unit + "'               \r";
            //SQL += "      , '" + strNurseER + "','" + strPChasu + "','" + strSUBUL_WARD + "','" + strHighRiskGBN + "','" + strER24 + "','" + strGSADD + "'\r";
            //SQL += "      , '" + cOrderCode + "','" + cSuCode + "','" + cBun + "', '" + strPRNORDSEQ + "')                              \r";

            //ER 응급실 입원관련 수정.
            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.OCS_IORDER                                          \r";
            SQL += "        (Ptno, BDate,     Seqno,     DeptCode                               \r";
            SQL += "      , DrCode,  StaffID,    Slipno,    OrderCode, SuCode,  Bun             \r";
            SQL += "      , GbOrder, Contents,   BContents, RealQty,   Qty,     RealNal         \r";
            SQL += "      , Nal,     DosCode,    GbInfo,    GbSelf,    GbSpc,   GbNgt           \r";
            SQL += "      , GbER,    GbPRN,      GbDiv,     GbBoth,    GbAct,   GbTFlag         \r";
            SQL += "      , GbSend,  GbPosition, GbStatus,  NurseID,   EntDate, WardCode        \r";
            SQL += "      , RoomCode, Bi,        OrderNo,   Remark                              \r";
            SQL += "      , ActDate, GbGroup,    GbPort,    OrderSite, Multi, MultiRemark       \r";
            SQL += "      , Dur, GBPICKUP, PICKUPSABUN, PICKUPDATE , GBIOE, MAYAK, POWDER, SEDATION, CONSULT     \r";
            SQL += "      , MayakRemark, VER, IP, GBSPC_NO,ENTDATE2                             \r";
            SQL += "      , GbTax,GbVerb,Verbal,V_ORDERNO,PRN_REMARK,PRN_INS_GBN,PRN_INS_UNIT   \r";
            SQL += "      , PRN_INS_SDATE,PRN_INS_EDATE,PRN_INS_Max,Powder_Sayu,ASA             \r";
            SQL += "      , PRN_DOSCODE, PRN_TERM, PRN_NOTIFY,PRN_UNIT,AirSht, PCHASU           \r";
            SQL += "      , SUBUL_WARD, HIGHRISK, ER24, GSADD                                   \r";
            SQL += "      , CORDERCODE, CSUCODE, CBUN, PRN_ORDSEQ, GBSEND_OORDER, AUTO_SEND)                              \r";
            SQL += " VALUES                                                                     \r";
            SQL += "        ( '" + AcpEmr.ptNo + "', TO_DATE('" + argBDate + "','YYYY-MM-DD')   \r";

            if (AcpEmr.medDeptCd != "" && AcpEmr.medDeptCd != null && string.IsNullOrEmpty(strEIChk) == true)
            {
                SQL += "      , " + nSeq + ",     '" + AcpEmr.medDeptCd + "'                     \r";
            }
            else if (strEIChk == "Y")
            {
                SQL += "      , " + nSeq + ",     '" + strEIDeptcode + "'               \r";
            }
            else
            {
                SQL += "      , " + nSeq + ",     '" + clsType.User.DeptCode + "'               \r";
            }

            SQL += "      , '" + cSabun + "',  '" + AcpEmr.medDrCd + "',    'A6'   \r";
            SQL += "      , '" + cOrderCode + "','C3710',     '58'                              \r";


            if (GstrWardCode == "ER" && strEIChk == "Y")
            {
                SQL += "      , 'M',   " + cContents + ",     " + cBContents + "                    \r";
            }
            else
            {
                SQL += "      , ' ',   " + cContents + ",     " + cBContents + "                    \r";
            }

            SQL += "      , '" + cRealQty + "',   " + cQty + ",          " + cRealNal + "       \r";
            SQL += "      , " + cNal + ",       '" + cDosCode + "',    '" + cGbInfo + "'        \r";
            SQL += "      , '" + cGbSelf + "',   '" + cGbSpc + "',      '" + cGbNgt + "'        \r";
            SQL += "      , '" + cGbEr + "',     '" + cGbPRN + "',       " + cGbDiv + "         \r";

            if (GstrWardCode == "ER" && string.IsNullOrEmpty(strEIChk) == true)
            {
                SQL += "      , '" + cGbBoth + "',   '*',      '" + cGbTFlag + "', '" + cGbSend + "'      \r";
            }
            else if (GstrWardCode == "ER" && strEIChk == "Y")
            {
                SQL += "      , '" + cGbBoth + "',   '" + cGbAct + "',      '" + cGbTFlag + "', '*'      \r";
            }
            else
            {
                SQL += "      , '" + cGbBoth + "',   '" + cGbAct + "',      '" + cGbTFlag + "', '" + cGbSend + "'      \r";
            }

            SQL += "      ,   '" + cGbPosition + "', '" + cGbStatus + "'     \r";
            SQL += "      , '" + cNurseID + "',  SysDate                 \r";

            if (GstrWardCode == "ER" && strEIChk == "Y")
            {
                SQL += "      ,   '" + strEIWardcode + "', '" + strEIRoomcode + "', '" + cBi + "', " + nOrderNo + ", '" + cRemark + "'\r";
            }
            else
            {
                SQL += "      ,   '" + cWardCode + "', '" + cRoomCode + "', '" + cBi + "', " + nOrderNo + ", '" + cRemark + "'\r";
            }

            SQL += "      , TO_DATE('" + argBDate + "','YYYY-MM-DD'),   '" + cGbGroup + "'      \r";

            if (GstrWardCode == "ER" && strEIChk == "Y")
            {
                SQL += "      , '" + cGbPort + "',   'ERO' , '" + cMulti + "'        \r";
            }
            else
            {
                SQL += "      , '" + cGbPort + "',   '" + cOrderSite + "' , '" + cMulti + "'        \r";
            }

            SQL += "      , '" + cMultiRemark + "', '" + cDUR + "'                              \r";

            if (GstrWardCode == "ER")
            {
                if (strEIChk == "Y")
                {
                    SQL += "      , '*', '#', SYSDATE, 'EI'                       \r";
                }
                else
                {
                    SQL += "      , '*', '', '', 'E'                       \r";
                }
            }
            else
            {
                SQL += "      , '*', '" + clsType.User.IdNumber + "', SYSDATE, ''                       \r";
            }

            SQL += "      , '" + cMayak + "' , '" + cPowder + "', '" + cSedation + "'           \r";
            SQL += "      , ''                                                                  \r";
            SQL += "      , '" + cMayakRemark + "', '', '" + clsPublic.GstrIpAddress + "' , '" + cGbSPC_NO + "',SYSDATE                 \r";
            SQL += "      , '" + strGbTax + "','" + strGbVerb + "','" + strVerbal_Doc + "'," + nVOrderno + ",'" + strPrnRemark + "'     \r";
            SQL += "      , '" + strPRN_Gbn + "'," + nPRN_Unit + ", TO_DATE('" + strPRN_SDate + "','YYYY-MM-DD')                        \r";
            SQL += "      , TO_DATE('" + strPRN_EDate + "','YYYY-MM-DD') ," + nPRN_Max + ",'" + strPowder_Sayu + "','" + strASA + "'    \r";
            SQL += "      , '" + strPRN_DosCode + "','" + strPRN_Term + "','" + strPRN_Notify + "' ,'" + strPRN_Unit + "'               \r";
            SQL += "      , '" + strNurseER + "','" + strPChasu + "','" + strSUBUL_WARD + "','" + strHighRiskGBN + "','" + strER24 + "','" + strGSADD + "'\r";
            SQL += "      , '" + cOrderCode + "','" + cSuCode + "','" + cBun + "', '" + strPRNORDSEQ + "'                              \r";

            if (GstrWardCode == "ER" && strEIChk == "Y")
            {
                SQL += "      , 'Y',   '2')         \r";
            }
            else
            {
                SQL += "      , '',   '' )        \r";
            }

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr + " 처방 저장시 오류가 발생되었습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                rtnVal = false;
                return rtnVal;
            }

            #endregion

            #region EXBST INSERT 쿼리

            if(GstrWardCode == "HD")
            {
                SQL = "";
                SQL += " INSERT INTO KOSMOS_OCS.EXAM_EXBST                                                                       \r";
                SQL += "        (ACTDATE, WARDCODE, PANO, SNAME                                                                  \r";
                SQL += "      , BDATE, DEPTCODE, QTY, SCODE)                                                                     \r";
                SQL += " VALUES                                                                                                  \r";
                SQL += "        ( TO_DATE('" + argBDate + "', 'YYYY-MM-DD'), 'HD'                             \r";
                SQL += "      , '" + AcpEmr.ptNo + "', '" + AcpEmr.ptName + "'                                                   \r";
                SQL += "      , TO_DATE('" + argBDate + "', 'YYYY-MM-DD'), 'HD', '1'                     \r";
                SQL += "      , '1'  )          \r";
            }
            else
            {
                SQL = "";
                SQL += " INSERT INTO KOSMOS_OCS.EXAM_EXBST                                                                       \r";
                SQL += "        (ACTDATE, WARDCODE, PANO, SNAME                                                                  \r";
                SQL += "      , BDATE, DEPTCODE, QTY, SCODE)                                                                     \r";
                SQL += " VALUES                                                                                                  \r";
                SQL += "        ( TO_DATE('" + argBDate + "', 'YYYY-MM-DD'), '" + AcpEmr.ward + "'                             \r";
                SQL += "      , '" + AcpEmr.ptNo + "', '" + AcpEmr.ptName + "'                                                   \r";
                SQL += "      , TO_DATE('" + argBDate + "', 'YYYY-MM-DD'), '" + AcpEmr.medDeptCd + "', '1'                     \r";
                SQL += "      , '1'  )          \r";
            }

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr + "BST 불출 저장시 오류가 발생되었습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                rtnVal = false;
                return rtnVal;
            }

            #endregion

            #region EXAM_ORDER INSERT 쿼리

            string FMGunbun2 = "";
            if (AcpEmr.sex == "남")
            {
                FMGunbun2 = "M";
            }
            else if (AcpEmr.sex == "여")
            {
                FMGunbun2 = "F";
            }
            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_ORDER                                                                                                                                               \r";
            SQL += "        (IPDOPD, BDATE, ACTDATE, PANO, BI, SNAME                                                                                                                                 \r";
            SQL += "      , AGE, SEX, DEPTCODE, DRCODE, WARD, ROOM                                                                                                                                   \r";
            SQL += "      , MASTERCODE, QTY, STRT, SPECNO, ORDERNO                                                                                                                                   \r";
            SQL += "      , ORDERDATE, SENDDATE, JDATE, UPPS, UPDT  )                                                                                                                                \r";
            SQL += " VALUES                                                                                                                                                                          \r";
            if(GstrWardCode == "ER")
            {
                SQL += "        ( 'O', TO_DATE('" + argBDate + "','YYYY-MM-DD'), TO_DATE('" + argBDate + "','YYYY-MM-DD'), '" + AcpEmr.ptNo + "', '" + AcpEmr.bi + "', '" + AcpEmr.ptName + "'           \r";
            }
            else
            {
                SQL += "        ( 'I', TO_DATE('" + argBDate + "','YYYY-MM-DD'), TO_DATE('" + argBDate + "','YYYY-MM-DD'), '" + AcpEmr.ptNo + "', '" + AcpEmr.bi + "', '" + AcpEmr.ptName + "'           \r";
            }

            SQL += "      , '" + AcpEmr.age + "', '" + FMGunbun2 + "'                                                                                                                                \r";
            SQL += "      , '" + AcpEmr.medDeptCd + "', '" + AcpEmr.medDrCd + "', '" + AcpEmr.ward + "', '" + AcpEmr.room + "'                                                                     \r";
            SQL += "      , 'CR59', '1'                                                                                                                                                              \r";
            SQL += "      , 'R', '" + s + "', '" + nOrderNo + "', SYSDATE, SYSDATE, SYSDATE                                                                                                          \r";
            SQL += "      , '" + clsType.User.IdNumber + "', SYSDATE )                                                                                                                               \r";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr + "검체 오더 저장시 오류가 발생되었습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                rtnVal = false;
                return rtnVal;
            }

            #endregion

            #region SPECMST INSERT 쿼리

            string FMGunbun = "";
            if(AcpEmr.sex == "남")
            {
                FMGunbun = "M";
            }
            else if(AcpEmr.sex == "여")
            {
                FMGunbun = "F";
            }
            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_SPECMST                                                                 \r";
            SQL += "        (SPECNO, PANO, BI, SNAME                                                                     \r";
            SQL += "      , IPDOPD, AGE, SEX, DEPTCODE, WARD, ROOM                                                       \r";
            SQL += "      , DRCODE, STRT, WORKSTS, SPECCODE, TUBE, BDATE                                                 \r";
            SQL += "      , BLOODDATE, RECEIVEDATE, RESULTDATE, STATUS, EMR, ORDERDATE, SENDDATE, GB_GWAEXAM             \r";
            SQL += "      , ORDERNO, GB_GWAEXAM2, WPART_SUB, INPS, INPT_DT, UPPS, UPDT)                                   \r";
            SQL += " VALUES                                                                                              \r";
            SQL += "        ( '" + s + "', '" + AcpEmr.ptNo + "', '" + AcpEmr.bi + "', '" + AcpEmr.ptName + "'           \r";
            if(GstrWardCode == "ER")
            {
                SQL += "      , 'O', '" + AcpEmr.age + "', '" + FMGunbun + "'                                              \r";
            }
            else
            {
                SQL += "      , 'I', '" + AcpEmr.age + "', '" + FMGunbun + "'                                              \r";
            }
            SQL += "      , '" + AcpEmr.medDeptCd + "', '" + AcpEmr.ward + "', '" + AcpEmr.room + "'                     \r";
            SQL += "      , '" + AcpEmr.medDrCd + "', 'R', 'C'                                                           \r";
            SQL += "      , '150', '092', TO_DATE('" + argBDate + "','YYYY-MM-DD')                                       \r";
            SQL += "      , SYSDATE, SYSDATE, SYSDATE, '05', '0'                                                         \r";
            SQL += "      , SYSDATE, SYSDATE, 'Y', '" + nOrderNo + "', '" + cWardCode + "', '1'                          \r";
            SQL += "      , '" + clsType.User.IdNumber + "', SYSDATE, " + clsType.User.IdNumber + ", SYSDATE  )          \r";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr + " 검체 저장시 오류가 발생되었습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                rtnVal = false;
                return rtnVal;
            }

            #endregion

            #region RESULTC INSERT 쿼리
            strTemp = Regex.Replace(VB.Left(strInValue, 4), @"[^.,0-9]", "");
            
            
            if (Convert.ToDouble(strTemp) <= 30 || Convert.ToDouble(strTemp) >= 600)
            {
                SQL = "";
                SQL += " INSERT INTO KOSMOS_OCS.EXAM_RESULTC                                                                 \r";
                SQL += "        (SPECNO, RESULTWS, EQUCODE, SEQNO, PANO                                                      \r";
                SQL += "      ,  MASTERCODE, SUBCODE, STATUS, RESULT, RESULTDATE, RESULTSABUN, REFER, UNIT                          \r";
                SQL += "      , EQUCODE_INTER, CV, INPS, INPT_DT, UPPS, UPDT)                                                     \r";
                SQL += " VALUES                                                                                              \r";
                SQL += "        ( '" + s + "', 'C', '004', '001', '" + AcpEmr.ptNo + "'                                      \r";
                if(Convert.ToDouble(strTemp) <= 30)
                {
                    SQL += "      ,  'CR59', 'CR59', 'V', '" + strInValue + "', SYSDATE , '" + strInId + "','L', 'mg/dL'         \r";
                }
                else
                {
                    SQL += "      ,  'CR59', 'CR59', 'V', '" + strInValue + "', SYSDATE , '" + strInId + "','H', 'mg/dL'         \r";
                }
                SQL += "      ,  '" + strInWard + "', 'C', '" + clsType.User.IdNumber + "', SYSDATE, '" + clsType.User.IdNumber + "', SYSDATE  )\r";
            }
            else
            {
                SQL = "";
                SQL += " INSERT INTO KOSMOS_OCS.EXAM_RESULTC                                                                 \r";
                SQL += "        (SPECNO, RESULTWS, EQUCODE, SEQNO, PANO                                                      \r";
                SQL += "      ,  MASTERCODE, SUBCODE, STATUS, RESULT, RESULTDATE, RESULTSABUN, UNIT                          \r";
                SQL += "      , EQUCODE_INTER, INPS, INPT_DT, UPPS, UPDT)                                                     \r";
                SQL += " VALUES                                                                                              \r";
                SQL += "        ( '" + s + "', 'C', '004', '001', '" + AcpEmr.ptNo + "'                                      \r";
                SQL += "      ,  'CR59', 'CR59', 'V', '" + strInValue + "', SYSDATE , '" + strInId + "', 'mg/dL'         \r";
                SQL += "      ,  '" + strInWard + "', '" + clsType.User.IdNumber + "', SYSDATE, '" + clsType.User.IdNumber + "', SYSDATE  )\r";
            }

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr + " 검체 결과 저장시 오류가 발생되었습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                rtnVal = false;
                return rtnVal;
            }

            if (Convert.ToDouble(strTemp) <= 30 || Convert.ToDouble(strTemp) >= 600)
            {
                SqlErr = ExamMsg.Exam_SMS_SEND_CV(clsDB.DbCon, s, "", "", false);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    //clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr + " 검체 CV 문자 발송 오류가 발생되었습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);//에러로그 저장                        
                    rtnVal = false;
                    return rtnVal;
                }
            }
            #endregion

            return rtnVal;
        }

       
    }
}

