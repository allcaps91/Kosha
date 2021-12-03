using ComBase; //기본 클래스
using ComBase.Controls;
using ComDbB; //DB연결
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ComEmrBase
{
    /// <summary>
    /// 서식지에 환자 정보 연동 함수 모음
    /// </summary>
    public class FormPatInfoFunc
    {
        /// <summary>
        /// ENDO 당일 검사 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="AcpEmr"></param>
        /// <returns></returns>

        public static string Set_FormPatInfo_ENDO_NAME(PsmhDb pDbCon, EmrPatient AcpEmr)
        {
            string rtnVal = string.Empty;


            try
            {
                OracleDataReader reader = null;
                string SQL = FormPatInfoQuery.Query_FormPatInfo_ENDO_NAME(AcpEmr.ptNo);
                string strBuNmae = string.Empty;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
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
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }
        /// <summary>
        /// 기초코드에 등록된 수가코드인지 확인.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="FormNo"></param>
        /// <param name="Itemcd"></param>
        /// <param name="OrderGbn"></param>
        /// <returns></returns>
        public static bool Set_FormPatInfo_ItemSugaMaaping(PsmhDb pDbCon, string FormNo, string Ward, string Itemcd, string Value, string OrderGbn = "")
        {
            bool rntVal = false;

            try
            {
                OracleDataReader reader = null;
                string SQL = FormPatInfoQuery.Query_FormPatInfo_ItemSugaMaaping(FormNo, Ward, Itemcd, Value, OrderGbn);
                string strBuNmae = string.Empty;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return rntVal;
                }

                if (reader.HasRows)
                {
                    rntVal = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rntVal;
        }


        public static bool Set_FormPatInfo_IsPoaException(PsmhDb pDbCon, string Code)
        {
            bool rntVal = false;

            try
            {
                OracleDataReader reader = null;
                string SQL = FormPatInfoQuery.Query_FormPatInfo_PoaException(Code);
                string strBuNmae = string.Empty;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return rntVal;
                }

                if (reader.HasRows)
                {
                    rntVal = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rntVal;
        }

        public static bool Set_FormPatInfo_IsBlood(PsmhDb pDbCon, string Pano, string Date)
        {
            bool rntVal = false;

            try
            {
                OracleDataReader reader = null;
                string SQL = FormPatInfoQuery.Query_FormPatInfo_IsBlood(Pano, Date);
                string strBuNmae = string.Empty;


                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return rntVal;
                }

                if (reader.HasRows)
                {
                    rntVal = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rntVal;
        }

        /// <summary>
        /// 소속 부서명 가져오기
        /// </summary>
        /// <returns></returns>
        public static string Set_FormPatInfo_SabunBuseName(PsmhDb pDbCon)
        {
            try
            {
                OracleDataReader reader = null;
                string SQL = FormPatInfoQuery.Query_FormPatInfo_SabunBuseName();
                string strBuNmae = string.Empty;


                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (reader.HasRows && reader.Read())
                {
                    strBuNmae = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                return strBuNmae;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 수혈 기록지 MAPPING 정보 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="EmrNo"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_BloodComponent(PsmhDb pDbCon, string EmrNo)
        {

            #region 변수
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_BloodComponent(EmrNo);
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            #endregion

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString();
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string Set_FormPatInfo_PTGetFormNo(PsmhDb pDbCon, string FormName)
        {

            #region 변수
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_PTGetFormNo(FormName);
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            #endregion

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString();
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 물리치료실 초기, 재평가 기록지 일자 경과 확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strBdate">치료일자</param>
        /// <param name="strPano">등록번호</param>
        /// <param name="strInOutCls">O: 외래, I: 입원</param>
        /// <param name="strDeptCode">과?</param>
        /// <param name="strFormName">작성된 폼이름</param>
        /// <returns></returns>
        public static bool Set_FormPatInfo_Is_PTReCord_Day(PsmhDb pDbCon, string strBdate,  string strPano, string strInOutCls, string strDeptCode, ref string strFormName)
        {
            strBdate = strBdate.Replace("-", "");
            OracleDataReader reader = null;
            bool rtnVal = false;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_Is_PTReCord_Day(strBdate, strPano, strInOutCls, strDeptCode);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        string strDate = reader.GetValue(0).ToString().Trim();
                        if (string.IsNullOrWhiteSpace(strDate) == false)
                        {
                            DateTime dtpSysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                            DateTime dtpMaxDate = DateTime.ParseExact(strDate, "yyyyMMdd", null);
                            TimeSpan timeSpan = (dtpSysDate - dtpMaxDate);
                            strFormName = reader.GetValue(1).ToString().Trim();
                            rtnVal = timeSpan.TotalDays > 30 && timeSpan.TotalDays <= 45;
                            if (rtnVal)
                                break;
                        }
                    }
                  
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
        /// 알레르기 항목에 자동 체크 후 해당 항목(음식, 약물항생제)에 뿌려준다.
        /// </summary>
        /// <param name="pDbCon"></param>
        public static Dictionary<string, string> Set_FormPatInfo_AUTO_ALLERGY(PsmhDb pDbCon, EmrPatient pAcp)
        {
            OracleDataReader reader = null;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_ALLERGY(pAcp);
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return keyValuePairs;
                }

                if (reader.HasRows)
                {
                    keyValuePairs.Add("I0000034276", "1");

                    Dictionary<string, string> keyAllergy = new Dictionary<string, string>();
                    keyAllergy.Add("005", "I0000034279_1");
                    keyAllergy.Add("003", "I0000034277_1");
                    keyAllergy.Add("100", "I0000035257_1");
                    while (reader.Read())
                    {
                        string rtnVal = string.Empty;

                        if (keyAllergy.TryGetValue(reader.GetValue(1).ToString().Trim(), out rtnVal))
                        {
                            if (keyValuePairs.ContainsKey(rtnVal) == false)
                            {
                                keyValuePairs.Add(rtnVal, "1");
                            }

                            string rtnText = string.Empty;
                            if (keyValuePairs.TryGetValue(rtnVal.Replace("_1", "_2"), out rtnText) == false)
                            {
                                keyValuePairs.Add(rtnVal.Replace("_1", "_2"), rtnText);
                            }
                            else
                            {
                                keyValuePairs[rtnVal.Replace("_1", "_2")] += reader.GetValue(0).ToString().Trim() + ", ";
                            }
                        }
                    }
                }
                reader.Dispose();

                return keyValuePairs;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
                return keyValuePairs;
            }
        }

        /// <summary>
        /// 중심정맥관 유지일 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp">환자정보</param>
        /// <param name="strInsertDay">삽입일</param>
        /// <param name="strMaintaintDay">유지일</param>
        public static void Set_FormPatInfo_CVCUseDay(PsmhDb pDbCon, EmrPatient pAcp, ref string strInsertDay, ref string strMaintaintDay)
        {
            #region 변수
            string SQL = FormPatInfoQuery.Query_FormPatInfo_CVCUseDay(pAcp);
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            #endregion

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strInsertDay = reader.GetValue(0).ToString().Trim();
                    if (VB.IsDate(strInsertDay) == false)
                    {
                        ComFunc.MsgBox("삽입일이 날짜 형식이 아닙니다. 다시 확인해주세요.");
                    }
                    else
                    {
                        double Day = (Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S")).Date - Convert.ToDateTime(strInsertDay).Date).TotalDays + 1;
                        strMaintaintDay = Day.ToString();
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

        }

        /// <summary>
        /// 인공호흡기 관련 폐렴에방  유지일 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp">환자정보</param>
        /// <param name="strInsertDay">삽입일</param>
        /// <param name="strMaintaintDay">유지일</param>
        /// <param name="strSize">Size</param>
        public static void Set_FormPatInfo_PneumoniaUseDay(PsmhDb pDbCon, EmrPatient pAcp, ref string strInsertDay, ref string strMaintaintDay, ref string strSize)
        {
            if (pAcp == null || pAcp != null && pAcp.ptNo.IsNullOrEmpty())
                return;

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            DateTime sysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
            #endregion

            try
            {

                #region 삽입일 가져오기
                SQL = FormPatInfoQuery.Query_FormPatInfo_PneumoniaInsertDay(pAcp);
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strInsertDay = reader.GetValue(0).ToString().Trim();
                    DateTime InsertDate = Convert.ToDateTime("9999-12-31");

                    if (!string.IsNullOrWhiteSpace(strInsertDay) && DateTime.TryParseExact(strInsertDay, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out InsertDate))
                    {
                        double Day = (sysDate.Date - InsertDate.Date).TotalDays + 1;
                        strMaintaintDay = Day.ToString();
                    }
                }

                reader.Dispose();
                #endregion


                #region Size 가져오기
                if (!string.IsNullOrWhiteSpace(strMaintaintDay))
                {
                    SQL = FormPatInfoQuery.Query_FormPatInfo_PneumoniaInsertSize(pAcp);
                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        strSize = reader.GetValue(0).ToString().Trim();
                    }

                    reader.Dispose();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 유치도뇨관 유지일 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp">환자정보</param>
        /// <param name="strInsertDay">삽입일</param>
        /// <param name="strMaintaintDay">유지일</param>
        /// <param name="strSize">Size</param>
        public static void Set_FormPatInfo_VentilatorUseDay(PsmhDb pDbCon, EmrPatient pAcp, ref string strInsertDay, ref string strMaintaintDay, ref string strSize)
        {
            if (pAcp == null || pAcp != null && pAcp.ptNo.IsNullOrEmpty())
                return;

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            DateTime delDate = Convert.ToDateTime("2020-01-01");
            DateTime sysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));
            #endregion

            try
            {
                #region 마지막 제거일 가져오기
                SQL = FormPatInfoQuery.Query_FormPatInfo_VentilatorDelDay(pAcp);

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    string strDelDateTime = reader.GetValue(0).ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(strDelDateTime) && DateTime.TryParseExact(strDelDateTime, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out delDate) == false)
                    {
                        delDate = Convert.ToDateTime("2000-01-01");
                        //reader.Dispose();
                        //ComFunc.MsgBox("제거일이 날짜 형식이 아닙니다.");
                        //return;
                    }
                }

                reader.Dispose();
                #endregion

                #region 삽입일 가져오기
                SQL = FormPatInfoQuery.Query_FormPatInfo_VentilatorInsertDay(pAcp);
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strInsertDay = reader.GetValue(0).ToString().Trim();
                    DateTime InsertDate = Convert.ToDateTime("9999-12-31");

                    if (!string.IsNullOrWhiteSpace(strInsertDay) && DateTime.TryParseExact(strInsertDay, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out InsertDate))
                    {
                        if (delDate <= InsertDate)
                        {
                            double Day = (sysDate.Date - InsertDate.Date).TotalDays + 1;
                            strMaintaintDay = Day.ToString();
                        }
                    }
                }

                reader.Dispose();
                #endregion
                

                #region Size 가져오기
                if (!string.IsNullOrWhiteSpace(strMaintaintDay))
                {
                    SQL = FormPatInfoQuery.Query_FormPatInfo_VentilatorInsertSize(pAcp);
                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        strSize = reader.GetValue(0).ToString().Trim();
                    }

                    reader.Dispose();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 내원기간안에 해당 기록지 마지막 작성일자
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp">환자정보</param>
        /// <param name="strFormNo">기록지 번호</param>
        public static string Set_FormPatInfo_MaxChartDate(PsmhDb pDbCon, EmrPatient pAcp, string strFormNo)
        {
            if (pAcp == null || pAcp != null && pAcp.ptNo.IsNullOrEmpty())
                return string.Empty;

            #region 변수
            string SQL = FormPatInfoQuery.Query_FormPatInfo_MaxChartDate(pAcp, strFormNo);
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            #endregion

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
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
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 내원기간안에 해당 기록지 작성했는지
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm">기록지 정보</param>
        /// <param name="pAcp">환자정보</param>
        public static bool Set_FormPatInfo_IsWrite(PsmhDb pDbCon, EmrForm pForm, EmrPatient pAcp)
        {
            if (pAcp == null || pAcp != null && pAcp.ptNo.IsNullOrEmpty())
                return false;

            #region 변수
            string SQL = FormPatInfoQuery.Query_FormPatInfo_IsWrite(pAcp, pForm);
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            bool rtnVal = false;
            #endregion

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// RSS 점수 계산 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp">환자정보</param>
        /// <param name="Score">중증도 점수</param>
        /// <param name="Warring">중등도 명칭</param>
        /// <param name="WarringColor">중증도 색상</param>
        public static void Set_FormPatInfo_RRSSCORE(PsmhDb pDbCon, EmrPatient pAcp, ref int Score, ref string Warring, ref Color WarringColor)
        {
            #region 변수
            string SQL = FormPatInfoQuery.Query_FormPatInfo_RRSSCORE(pAcp);
            string SqlErr = string.Empty;

            DataTable dt = null;
            #endregion

            try
            {
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                #region 점수 계산
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    double Val = 0;
                    #region 맥박 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000014815"))
                    {
                        Val = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                        if (Val <= 40 || Val >= 111 && Val <= 130)
                        {
                            Score += 2;
                        }
                        else if (Val >= 41 && Val <= 50 || Val >= 101 && Val <= 110)
                        {
                            Score += 1;
                        }
                        else if (Val >= 51 && Val <= 100)
                        {
                            Score += 0;
                        }
                        else if (Val >= 131)
                        {
                            Score += 3;
                        }
                    }
                    #endregion

                    #region 수축기 혈압 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000002018"))
                    {
                        Val = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                        if (Val <= 70)
                        {
                            Score += 3;
                        }
                        else if (Val >= 71 && Val <= 80 || Val >= 200)
                        {
                            Score += 2;
                        }
                        else if (Val >= 81 && Val <= 100)
                        {
                            Score += 1;
                        }
                        else if (Val >= 101 && Val <= 199)
                        {
                            Score += 0;
                        }
                    }
                    #endregion

                    #region 호흡수 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000002009"))
                    {
                        Val = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                        if (Val >= 30)
                        {
                            Score += 3;
                        }
                        else if (Val <= 8 || Val >= 21 && Val <= 29)
                        {
                            Score += 2;
                        }
                        else if (Val >= 15 && Val <= 20)
                        {
                            Score += 1;
                        }
                        else if (Val >= 9 && Val <= 14)
                        {
                            Score += 0;
                        }
                    }
                    #endregion

                    #region 체온 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000001811"))
                    {
                        Val = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                        if (Val <= 35)
                        {
                            Score += 2;
                        }
                        else if (Val >= 35.1 && Val <= 36.0 || Val >= 37.5)
                        {
                            Score += 1;
                        }
                        else if (Val >= 36.1 && Val <= 37.4)
                        {
                            Score += 0;
                        }
                    }
                    #endregion

                    #region 의식수준 점수(AVPU SCORE)
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000037778"))
                    {
                        switch (dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                        {
                            case "Alert": //정상
                                Score += 0;
                                break;
                            case "Verbal Response": //목소리 반응
                                Score += 1;
                                break;
                            case "Pain Response": // 통증 반응
                                Score += 2;
                                break;
                            case "Unconsciousness": //무반응
                                Score += 3;
                                break;
                        }
                    }
                    #endregion
                }
                #endregion

                #region 중증도 
                if (Score >= 0 && Score <= 4)
                {
                    Warring = "경증(저위험군)";
                }
                else if (Score >= 5 && Score <= 6)
                {
                    Warring = "중등증(중증도위험군)";
                }
                else if (Score >= 7)
                {
                    Warring = "중증(고위험군)";
                }
                //else if (Score >= 7)
                //{
                //    Warring = "최중증(고위험군)";
                //}
                #endregion

                #region 색상
                if (Score >= 0 && Score <= 4)
                {
                    WarringColor = Color.LightGreen;
                }
                else if (Score >= 5 && Score <= 6)
                {
                    WarringColor = Color.Orange;
                }
                else if (Score >= 7)
                {
                    WarringColor = Color.Red;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }



        /// <summary>
        /// 욕창 지표 조회 함수
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argDate"></param>
        /// <param name="argIPDNO"></param>
        /// <param name="argAge"></param>
        /// <param name="argWARD"></param>
        /// <param name="ArgDate2"></param>
        public static string READ_DETAIL_BRADEN(PsmhDb pDbCon, EmrPatient pAcp)
        {
            OracleDataReader dataReader = null;
            DataTable dt = null;
            string SQL = string.Empty;
            string sqlErr = string.Empty;

            string strBraden = string.Empty;
            string strOK = string.Empty;
            string strGUBUN = string.Empty;
            string strTOOL = string.Empty;

            double Total = 0;

            if (pAcp.ward.Equals("NR") || pAcp.ward.Equals("ND") || pAcp.ward.Equals("IQ"))
            {
                strGUBUN = "신생아";
                strTOOL = "생아욕창사정 도구표";
            }
            else if (VB.Val(pAcp.age) < 5)
            {
                strGUBUN = "소아";
                strTOOL = "소아욕창사정 도구표";
            }
            else
            {
                strGUBUN = string.Empty;
                strTOOL = "욕창사정 도구표";
            }

            if (strGUBUN.Length == 0)
            {
                SQL = FormPatInfoQuery.Query_FormPatInfo_DETAIL_BRADEN(pAcp);
                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (dt.Rows.Count > 0)
                {
                    double dAge = VB.Val(dt.Rows[0]["AGE"].ToString().Trim());
                    double dTotal = VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim());
                    Total = dTotal;

                    //if ((dAge >= 60 && dTotal <= 18) ||
                    //dAge < 60 && dTotal <= 16)
                    if (dTotal <= 18)
                    {
                        strBraden = "OK";
                    }
                }

                dt.Dispose();
            }
            else if (strGUBUN == "소아")
            {
                SQL = FormPatInfoQuery.Query_FormPatInfo_DETAIL_BRADEN2(pAcp);
                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (dataReader.HasRows && dataReader.Read())
                {
                    Total = VB.Val(dataReader.GetValue(0).ToString().Trim());
                    strOK = Total <= 16 ? "OK" : "";
                }

                dataReader.Dispose();
            }
            else if (strGUBUN == "신생아")
            {

                SQL = FormPatInfoQuery.Query_FormPatInfo_DETAIL_BRADEN_BABY(pAcp);
                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }


                if (dataReader.HasRows && dataReader.Read())
                {
                    Total = VB.Val(dataReader.GetValue(0).ToString().Trim());
                    strBraden = Total <= 20 ? "OK" : "";
                }

                dataReader.Dispose();
            }

            SQL = FormPatInfoQuery.Query_FormPatInfo_NUR_BRADEN_WARNING(pAcp);
            sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBox(sqlErr);
                return string.Empty;
            }

            string strBun = string.Empty;

            if (dt.Rows.Count > 0)
            {
                #region 데이터 넣기
                strBraden = "OK";

                if (dt.Rows[0]["WARD_ICU"].ToString().Trim() == "1")
                {
                    strBun += "중환자실 ";
                }

                if (dt.Rows[0]["GRADE_HIGH"].ToString().Trim() == "1")
                {
                    strBun += "중증도 분류 3군 이상 ";
                }

                if (dt.Rows[0]["PARAL"].ToString().Trim() == "1")
                {
                    strBun += "뇌, 척추 관련 마비 ";
                }

                if (dt.Rows[0]["NOT_MOVE"].ToString().Trim() == "1")
                {
                    strBun += "부종 ";
                }

                if (dt.Rows[0]["DIET_FAIL"].ToString().Trim() == "1")
                {
                    strBun += "영양불량 ";
                }

                if (dt.Rows[0]["NEED_PROTEIN"].ToString().Trim() == "1")
                {
                    strBun += "단백질 불량 ";
                }

                if (dt.Rows[0]["EDEMA"].ToString().Trim() == "1")
                {
                    strBun += "부동 ";
                }

                if (dt.Rows[0]["BRADEN"].ToString().Trim() == "1")
                {
                    strBun += "현재 욕창이 있는 환자 ";
                }
                #endregion
            }

            dt.Dispose();

            string strTEMP = string.Empty;
            if (strBraden == "OK")
            {
                if (Total > 0)
                {
                    strTEMP += " ★ 욕창점수 : " + Total + ComNum.VBLF;
                }

                if (strBun.Length > 0)
                {
                    strTEMP += strBun;
                }

            }

            strTEMP = string.IsNullOrWhiteSpace(strTEMP) ? "미해당" : strTEMP;

            return strTEMP;
        }

        /// <summary>
        /// 욕창 고위험군 표시
        /// </summary>
        /// <param name="pAcp"></param>
        /// <returns></returns>
        public static string READ_DETAIL_EVAL_BRADEN_NEW(PsmhDb pDbCon, EmrPatient pAcp)
        {
            string rtnVal = string.Empty;

            try
            {
                string strDrug = string.Empty;
                string strDrug2 = string.Empty;

                string strCAUSE = string.Empty;

                string SQL = string.Empty;
                StringBuilder strTemp = new StringBuilder();

                DataTable dt = null;

                SQL = FormPatInfoQuery.Query_FormPatInfo_READ_DETAIL_EVAL_BRADEN_NEW(pAcp);
                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (dt.Rows.Count > 0)
                {
                    #region 데이타 매핑
                    if (dt.Rows[0]["TRANSFOR"].ToString().Trim() == "1")
                    {
                        strTemp.Append("전동 시  ");
                    }

                    if (dt.Rows[0]["OP"].ToString().Trim() == "1")
                    {
                        strTemp.Append("수술/시술 ");
                    }

                    if (dt.Rows[0]["PAT_CHANGE"].ToString().Trim() == "1")
                    {

                        strTemp.Append("급격한 상태 변화 ");
                    }

                    if (dt.Rows[0]["YOK"].ToString().Trim() == "1")
                    {
                        strTemp.Append("욕창 발생 시 ");
                    }

                    if (dt.Rows[0]["PAT_CHANGE2"].ToString().Trim() == "1")
                    {
                        strTemp.Append("신체상태 악화 ");
                    }
                    #endregion
                }

                dt.Dispose();

                rtnVal = strTemp.ToString().Trim();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return string.IsNullOrWhiteSpace(rtnVal) ? "미해당" : rtnVal;
        }



        /// <summary>
        /// 낙상 고위험군 표시
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <returns></returns>
        public static string READ_DETAIL_FALL(PsmhDb pDbCon, EmrPatient pAcp)
        {
            try
            {
                string strFall = string.Empty;
                string strBraden = string.Empty;

                string strTOTAL = string.Empty;
                string strExam = string.Empty;
                string strCAUSE = string.Empty;
                string strDrug = string.Empty;

                string strWARD_C = string.Empty;
                string strAGE_C = string.Empty;

                string SQL = string.Empty;

                DataTable dt = null;

                switch (pAcp.ward)
                {
                    case "33":
                    case "35":
                        strFall = "OK";
                        strWARD_C = "중환자실 재원 환자";
                        break;

                    case "NR":
                    case "IQ":
                        strFall = "OK";
                        strWARD_C = "신생아실 재원 환자";
                        break;
                }

                double dAge = VB.Val(pAcp.age);
                if (dAge >= 70)
                {
                    strFall = "OK";
                    strAGE_C = "70세 이상 환자";
                }

                if (dAge < 7)
                {
                    strFall = "OK";
                    strAGE_C = "7세 미만 환자";
                }

                SQL = FormPatInfoQuery.Query_FormPatInfo_NUR_FALLMORSE_SCALE(pAcp);

                if (VB.Val(pAcp.age) < 18)
                {
                    SQL = FormPatInfoQuery.Query_FormPatInfo_NUR_FALLMORSE_SCALE18(pAcp);
                }

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (dt.Rows.Count > 0)
                {
                    strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                    strFall = "OK";
                }

                dt.Dispose();

                StringBuilder strTemp = new StringBuilder();

                SQL = FormPatInfoQuery.Query_FormPatInfo_NUR_FALL_WARNING(pAcp);
                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (dt.Rows.Count > 0)
                {
                    #region 데이터 입력
                    strFall = "OK";

                    strCAUSE = "";

                    if (strAGE_C.Length == 0 && dt.Rows[0]["WARNING1"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 70세이상 ");
                    }

                    if (dt.Rows[0]["WARNING2"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 보행장애 ");
                    }

                    if (dt.Rows[0]["WARNING3"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 혼미 ");
                    }

                    if (dt.Rows[0]["WARNING4"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 어지럼증 ");
                    }

                    strCAUSE = strTemp.ToString().Trim();

                    strTemp.Clear();
                    strDrug = "";

                    if (dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 진정제 ");
                    }
                    if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 수면제 ");
                    }
                    if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 향정신성약물 ");
                    }
                    if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 항우울제 ");
                    }
                    if (dt.Rows[0]["DRUG_05"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 완하제 ");
                    }
                    if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 이뇨제 ");
                    }
                    if (dt.Rows[0]["DRUG_07"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 진정약물 ");
                    }
                    if (dt.Rows[0]["DRUG_08"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ " + dt.Rows[0]["DRUG_08_ETC"].ToString().Trim());
                    }

                    strDrug = strTemp.ToString().Trim();
                    #endregion
                }

                dt.Dispose();


                if (strFall == "OK")
                {
                    #region 낙상점수
                    strTemp.Clear();
                    if (strTOTAL.Length > 0)
                    {
                        strTemp.AppendLine(" => 낙상점수 : " + VB.Val(strTOTAL) + "점 ");
                    }


                    if (strWARD_C.Length > 0)
                    {
                        strTemp.AppendLine(strWARD_C);
                    }


                    if (strAGE_C.Length > 0)
                    {
                        strTemp.AppendLine(strAGE_C);
                    }


                    if (strCAUSE.Length > 0)
                    {
                        strTemp.AppendLine(strCAUSE);
                    }


                    if (strDrug.Length > 0)
                    {
                        strTemp.AppendLine("-위험약물");
                        strTemp.AppendLine(strDrug);
                    }
                    #endregion
                }

                return strTemp.Length == 0 ? "미해당" : strTemp.ToString().Trim();

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 낙상 재평가 요인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strIpdNo"></param>
        /// <returns></returns>
        public static string READ_DETAIL_EVAL_FALL_NEW(PsmhDb pDbCon, EmrPatient pAcp)
        {
            string rtnVal = string.Empty;

            try
            {
                string strDrug = string.Empty;
                string strDrug2 = string.Empty;

                string strCAUSE = string.Empty;

                string SQL = string.Empty;
                StringBuilder strTemp = new StringBuilder();

                DataTable dt = null;

                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALL_EVAL ";
                SQL += ComNum.VBLF + " WHERE IPDNO = " + pAcp.acpNoIn;

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (dt.Rows.Count > 0)
                {
                    #region 데이타 매핑
                    if (dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "진정제 ";
                    }

                    if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "수면제 ";
                    }

                    if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "향정신성 약물 ";
                    }

                    if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "항우울제 ";
                    }

                    if (dt.Rows[0]["DRUG_05"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "완하제 ";
                    }

                    if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "이뇨제 ";
                    }

                    if (dt.Rows[0]["DRUG_07"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "진정약품 ";

                    }

                    if (dt.Rows[0]["DRUG_08"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "기타 ";
                    }

                    if (dt.Rows[0]["DRUG_08ETC"].ToString().Trim() == "1")
                    {
                        strDrug = strDrug + "(" + dt.Rows[0]["DRUG_08"].ToString().Trim() + ") ";
                    }

                    if (dt.Rows[0]["PAT_CHANGE"].ToString().Trim() == "1")
                    {
                        strTemp.Append("환자상태변화 ");
                    }

                    if (dt.Rows[0]["PAT_CHANGE2"].ToString().Trim() == "1")
                    {
                        strTemp.Append("주요상태변화:간성 혼수, 알콜 섬망, 발작 ");
                    }

                    if (dt.Rows[0]["TRANFOR"].ToString().Trim() == "1")
                    {
                        strTemp.Append("전동 ");
                    }

                    if (dt.Rows[0]["RELEX"].ToString().Trim() == "1")
                    {
                        strTemp.Append("진정 치료(검사) ");
                    }

                    if (dt.Rows[0]["OP"].ToString().Trim() == "1")
                    {
                        strTemp.Append("수술/시술 ");
                    }

                    if (dt.Rows[0]["FALL"].ToString().Trim() == "1")
                    {
                        strTemp.Append("낙상 발생 ");
                    }
                    #endregion
                }

                rtnVal = strTemp.ToString().Trim();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return string.IsNullOrWhiteSpace(rtnVal) ? "미해당" : rtnVal;
        }


        /// <summary>
        /// 간호사가 등록한 진단명 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient">환자정보</param>
        /// <returns></returns>
        public static string Set_FormPatInfo_NUR_DIAGNOSIS(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_NUR_DIAGNOSIS(emrPatient);

            try
            {
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
        /// PT오더 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient">환자정보</param>
        /// <param name="spd">스프레드(입력 안하면 데이터 유무만)</param>
        /// <returns></returns>
        public static bool Set_FormPatInfo_PTOrder(PsmhDb pDbCon, EmrPatient emrPatient, string OrderDate, FarPoint.Win.Spread.FpSpread spd = null)
        {
            OracleDataReader reader = null;
            bool rtnVal = false;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_PTOrder(emrPatient, OrderDate);


            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    if (spd != null)
                    {
                        spd.ActiveSheet.RowCount = 0;

                        while (reader.Read())
                        {
                            spd.ActiveSheet.RowCount += 1;
                            //ACTDATE
                            spd.ActiveSheet.Cells[spd.ActiveSheet.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
                            //SUCODE
                            spd.ActiveSheet.Cells[spd.ActiveSheet.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim();
                            //ORDERNAME
                            spd.ActiveSheet.Cells[spd.ActiveSheet.RowCount - 1, 2].Text = reader.GetValue(2).ToString().Trim();
                            //CTIME
                            spd.ActiveSheet.Cells[spd.ActiveSheet.RowCount - 1, 3].Text = reader.GetValue(3).ToString().Trim();
                        }
                    }

                    rtnVal = true;
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
        /// 신장, 체중 정보
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient">환자정보</param>
        /// <returns></returns>
        public static string Set_FormPatInfo_BodyInfo(PsmhDb pDbCon, EmrPatient emrPatient, string ItemCd)
        {
            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_BodyInfo(emrPatient, ItemCd);

            try
            {
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
        /// 물리치료실 경과기록지 치료시간 중복 체크 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient">환자정보</param>
        /// <param name="strPTTime">치료시간</param>
        /// <returns></returns>
        public static bool Set_FormPatInfo_Is_PTReCord_Write(PsmhDb pDbCon, EmrPatient emrPatient, string strPTTime)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return false;

            OracleDataReader reader = null;
            bool rtnVal = false;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_PT_ReCord_Write(emrPatient, strPTTime);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
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
        /// 환자 보험 유형 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strIpWonDate">병동 입원시간</param>
        /// <param name="strErFrDate">ER 출발시간</param>
        /// <returns></returns>
        public static string Set_FormPatInfo_BI(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_BI(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = clsVbfunc.GetBiName(reader.GetValue(0).ToString().Trim());
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
        /// DB로 시간 계산
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strIpWonDate">병동 입원시간</param>
        /// <param name="strErFrDate">ER 출발시간</param>
        /// <returns></returns>
        public static string Set_FormPatInfo_TIMECOMPARE(PsmhDb pDbCon, string strIpWonDate, string strErFrDate)
        {
            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = " SELECT (TO_DATE('" + strIpWonDate + "','yyyy-mm-dd HH24:MI') - TO_DATE('" + strErFrDate + "','yyyy-mm-dd HH24:MI')) * 60 * 24 nTime \r\n FROM DUAL";

            try
            {
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
        ///  응급실에서 병동 보낸 시간 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <returns></returns>

        public static string Set_FormPatInfo_ER_TRANS_TIME(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_ER_IPWON(emrPatient);

            try
            {
                #region ER입원 경유 체크
                string strERIpwon = string.Empty;
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

                #region ER 경유 했으면 날짜 반환
                SQL = FormPatInfoQuery.Query_FormPatInfo_ER_TRANS_TIME(emrPatient);
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
        /// 해당(이전 XML형식) 서식지에 해당하는 아이템값의 데이터를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="FormNo">기록지번호</param>
        /// <param name="ItemCd">아이템 번호</param>
        /// <returns></returns>
        public static string Set_FormPatInfo_BRADEN(PsmhDb pDbCon, EmrPatient emrPatient, string ChartDate)
        {
            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_BRADEN(emrPatient, ChartDate);

            try
            {
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
        /// 환자의 RO 상병 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_RODisease(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            StringBuilder rtnVal = new StringBuilder();
            
            string SQL = FormPatInfoQuery.Query_FormPatInfo_RODisease(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal.ToString().Trim();
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rtnVal.Append(reader.GetValue(1).ToString().Equals("*") ? "R/O " : "");
                        rtnVal.AppendLine(reader.GetValue(0).ToString());
                    }
                }
                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal.ToString().Trim();
        }



        /// <summary>
        /// 해당(이전 XML형식) 서식지에 해당하는 아이템값의 데이터를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="FormNo">기록지번호</param>
        /// <param name="ItemCd">아이템 번호</param>
        /// <returns></returns>
        public static string Set_FormPatInfo_OldGetItemValue(PsmhDb pDbCon, EmrPatient emrPatient, string FormNo, string ItemCd)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_OldGetItemValue(emrPatient, FormNo, ItemCd);

            try
            {
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
        /// 해당 서식지에 해당하는 아이템값의 데이터를 가져온다.
        /// 함수이름:서식지번회:아이템이름
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_GetItemValue(PsmhDb pDbCon, EmrPatient emrPatient, string Tag)
        {
            if (string.IsNullOrWhiteSpace(Tag.Substring(Tag.LastIndexOf(":"))))
                return string.Empty;

            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string[] Arr = Tag.Split(':');
            string FormNo = Arr[0];
            string ItemCd = Arr[1];
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_GetItemValue(emrPatient, FormNo, ItemCd);

            try
            {
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
        /// 환자의 당일 상병명 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_NowDisease(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_NowDisease(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    StringBuilder Diag = new StringBuilder();
                    while(reader.Read())
                    {
                        Diag.Append(reader.GetValue(0).ToString()).Append(",");
                    }
                    rtnVal = Diag.ToString().Trim();
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
        /// 병동 전화번호
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_Ward_CallNumber(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            if (string.IsNullOrWhiteSpace(emrPatient.room))
            {
                return string.Empty;
            }

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_Ward_CallNumber(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = string.Concat("054-260-", reader.GetValue(0).ToString());
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
        /// ER내원 후 병동 입원환자 출발시간 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_Er_Trans(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_Er_Trans(emrPatient);

            try
            {
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
        /// 환자의 사생활 보호를 UPDATE 한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static bool Set_FormPatInfo_Secret(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return false;

            bool rtnVal = false;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_Secret(emrPatient);

            clsDB.setBeginTran(pDbCon);
            try
            {
                int RowAffected = 0;
                string SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }

                clsDB.setCommitTran(pDbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
                return rtnVal;
            }
        }

        /// <summary>
        /// 알레르기 정보를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_ALLERGY(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            StringBuilder rtnVal = new StringBuilder();

            string SQL = FormPatInfoQuery.Query_FormPatInfo_ALLERGY(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal.ToString().Trim();
                }

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        rtnVal.Append(reader.GetValue(0).ToString().Trim() + ", ");
                    }
                }
                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal.ToString().Trim();
        }

        /// <summary>
        /// 입원기간내에 감염정보를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_INFECT(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            StringBuilder strINFECT = new StringBuilder();
            string SQL = FormPatInfoQuery.Query_FormPatInfo_INFECT(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(sqlErr);
                }

                if (reader.HasRows && reader.Read())
                {
                    string strInfect = reader.GetValue(0).ToString().Trim();
                    if (strInfect.Substring(0, 1).Equals("1"))
                    {
                        strINFECT.Append("혈액주의, ");
                    }
                    else if (strInfect.Substring(1, 1).Equals("1"))
                    {
                        strINFECT.Append("접촉주의, ");
                    }
                    else if (strInfect.Substring(2, 1).Equals("1"))
                    {
                        strINFECT.Append("접촉(격리)주의, ");
                    }
                    else if (strInfect.Substring(3, 1).Equals("1"))
                    {
                        strINFECT.Append("공기주의, ");
                    }
                    else if (strInfect.Substring(4, 1).Equals("1"))
                    {
                        strINFECT.Append("비말주의, ");
                    }
                    else if (strInfect.Substring(5, 1).Equals("1"))
                    {
                        strINFECT.Append("보호격리, ");
                    }
                    else if (strInfect.Substring(6, 1).Equals("1"))
                    {
                        strINFECT.Append("해외경유자");
                    }
                }
                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return strINFECT.ToString().Trim();
        }

        /// <summary>
        /// 가장 최근 BST값을 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_BST(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_BST(emrPatient);

            try
            {
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
        /// 입원 기간내에 수술, 시술명 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_OP_INFO(PsmhDb pDbCon, EmrPatient emrPatient, string GB)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_OP_IFNO(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(GB.Equals("진단") ? 0 : 1).ToString().Trim();
                }
                else
                {
                    if (GB.Equals("진단"))
                    {
                        SQL = FormPatInfoQuery.Query_FormPatInfo_NUR_DIAGNOSIS(emrPatient);
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
                    }
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
        /// 입원 기간내에 외래 예약한 경우 예약 시간 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strDrName"></param>
        /// <param name="strDeptCd"></param>
        public static string Set_FormPatInfo_OPD_RESERVED(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_OPD_RESERVED(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = string.Format("{0}/{1}", reader.GetValue(0).ToString().Trim(), Convert.ToDateTime(reader.GetValue(1).ToString().Trim()).ToString("yyyy-MM-dd HH:mm")).Replace(" ", "/");                                
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
        /// 입원정보, 환자 개인정보 등 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strDrName"></param>
        /// <param name="strDeptCd"></param>
        public static string Set_FormPatInfo_ADMISSION_DATE(PsmhDb pDbCon, EmrPatient emrPatient, string Type)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_Admission(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(3).ToString().Trim();
                    
                    if(Type.Equals("DATE"))
                    {
                        rtnVal = Convert.ToDateTime(rtnVal).ToShortDateString();
                    }
                    else
                    {
                        rtnVal = Convert.ToDateTime(rtnVal).ToString("HH:mm");
                    }

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
        /// 입원정보, 환자 개인정보 등 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strDrName"></param>
        /// <param name="strDeptCd"></param>
        public static string Set_FormPatInfo_JUSO(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_Admission(emrPatient);

            try
            {
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
        /// 입원정보, 환자 개인정보 등 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strDrName"></param>
        /// <param name="strDeptCd"></param>
        public static string Set_FormPatInfo_PHONE(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_Admission(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(2).ToString().Trim();

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
        /// 입원정보, 환자 개인정보 등 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strDrName"></param>
        /// <param name="strDeptCd"></param>
        public static string Set_FormPatInfo_Admission(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            OracleDataReader reader = null;
            string rtnVal = string.Empty;
            
            string SQL = FormPatInfoQuery.Query_FormPatInfo_Admission(emrPatient);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    string JUSO  = reader.GetValue(0).ToString().Trim();
                    string PHONE = reader.GetValue(1).ToString().Trim();
                    string PHONE2 = reader.GetValue(2).ToString().Trim();
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
        /// 입원기간 내에 전과 하였을경우 처음 입원한 과장 정보 가져오는 함수.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strDrName"></param>
        /// <param name="strDeptCd"></param>
        public static void GetInDrName(PsmhDb pDbCon, EmrPatient emrPatient, ref string strDrName, ref string strDeptCd)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return;

            OracleDataReader reader = null;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_GetInDrName(emrPatient, ComQuery.CurrentDateTime(pDbCon, "D"));

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strDeptCd = reader.GetValue(0).ToString().Trim();
                    strDrName = reader.GetValue(1).ToString().Trim();
                }
                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 해당 환자의 KTAS 중증도 분류 일시를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="Gubun">일자, 시간 각각 반환</param>
        /// <returns></returns>
        public static string Set_FormPatInfo_GetTriage(PsmhDb pDbCon, EmrPatient emrPatient, string Gubun)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_Triage(emrPatient);
            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(Gubun.Equals("일자") ? 0 : 1).ToString().Trim();
            }

            reader.Dispose();
            return rtnVal;
        }


        /// <summary>
        /// 입원기간 내에 입퇴원 요약지 갯수
        /// </summary>
        /// <param name="strPtno"></param>
        /// <returns></returns>
        public static double Set_FormPatInfo_DischargeRecordCnt(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return 0;

            double rtnVal = 0;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_DischargeSummary(emrPatient);
            OracleDataReader reader = null;


            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = 1;
            }

            reader.Dispose();
            return rtnVal;
        }


        /// <summary>
        /// ER환자의 가장 최근 접수시간 가져온다
        /// </summary>
        /// <param name="strPtno"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_ERInDate(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            if (emrPatient == null || emrPatient != null && emrPatient.ptNo.IsNullOrEmpty())
                return string.Empty;

            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_ERInDate(emrPatient);
            OracleDataReader reader = null;


            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if(reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();
            return rtnVal;
        }

        /// <summary>
        /// 환자의 주증상 및 현병력을 세팅한다
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_MainSymptoms(PsmhDb pDbCon, EmrForm pForm, EmrPatient emrPatient)
        {
            string rtnVal = string.Empty;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_MainSymptoms(emrPatient, pForm);
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
             
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count == 0 || dt.Rows.Count > 0 && dt.Rows[0]["chiefComplaint"].ToString().Replace("\r\n", "").Trim().Length == 0)
            {
                dt.Dispose();

                SQL = FormPatInfoQuery.Query_FormPatInfo_MainSymptoms2(emrPatient, pForm);

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
                    return rtnVal;
                }
            }
            
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["GBN"].ToString().Trim().Equals("OLD"))
                {
                    rtnVal = string.Format("{0}\r\n{1}", dt.Rows[0]["chiefComplaint"].ToString().Trim().Replace("\n", "\r\n"), dt.Rows[0]["presentIllness"].ToString().Trim().Replace("\n", "\r\n"));
                }
                else
                {
                    rtnVal = string.Format("{0}\r\n{1}", dt.Rows[0]["chiefComplaint"].ToString().Trim(), dt.Rows[0]["presentIllness"].ToString().Trim());
                }

                if (emrPatient.medDeptCd.Equals("EN") && dt.Columns.IndexOf("physicalEx") != -1)
                {
                    if (dt.Rows[0]["GBN"].ToString().Trim().Equals("OLD"))
                    {
                        rtnVal += "\r\n" + dt.Rows[0]["physicalEx"].ToString().Trim().Replace("\n", "\r\n");
                    }
                    else
                    {
                        rtnVal += "\r\n" + dt.Rows[0]["physicalEx"].ToString().Trim();
                    }
                }
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }


        /// <summary>
        /// 환자의 주진단을 세팅한다
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_MainDisease(PsmhDb pDbCon, string strPtno, string strInDate, string strOutDate, string pFlag)
        {
            string rtnVal =string.Empty;
            string SQL =string.Empty;
            string SqlErr =string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strOutDate == "99991231" || strOutDate == "99981231")
            {
                strOutDate =string.Empty; // ComQuery.CurrentDateTime(pDbCon, "D");
            }
            SQL = FormPatInfoQuery.Query_FormPatInfo_MainDisease(strPtno, strInDate, strOutDate);
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

            if (pFlag == "KOR")
            {
                rtnVal = ComFunc.RPAD(dt.Rows[0]["ILLCODED"].ToString().Trim(), 5, " ") + ":" + dt.Rows[0]["ILLNAMEK"].ToString().Trim();
            }
            else
            {
                rtnVal = ComFunc.RPAD(dt.Rows[0]["ILLCODED"].ToString().Trim(), 5, " ") + ":" + dt.Rows[0]["ILLNAMEE"].ToString().Trim();
            }
            
            dt.Dispose();
            dt = null;
            return rtnVal;
        }


        


        /// <summary>
        /// 환자의 부진단을 세팅한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <param name="TextName"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_SubDisease(PsmhDb pDbCon, string strPtno, string strInDate, string strOutDate, string TextName)
        {
            string rtnVal = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strOutDate == "99991231" || strOutDate == "99981231")
            {
                strOutDate =string.Empty; // ComQuery.CurrentDateTime(pDbCon, "D");
            }
            SQL = FormPatInfoQuery.Query_FormPatInfo_MainDisease(strPtno, strInDate, strOutDate);
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
                return rtnVal;
            }

            dt = dt.AsEnumerable().GroupBy(r => r.Field<string>("ILLCODED")).Select(s => s.First()).OrderBy(o => o.Field<string>("ORDDATE")).CopyToDataTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (TextName.Equals("di" + (i + 2)))
                {
                    rtnVal = Regex.Replace(dt.Rows[i]["ILLCODED"].ToString().Trim(), @"/([^a-zA-Z0-9])/", "") +  "  :" + dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                    break;
                }
            }

            dt.Dispose();
            return rtnVal;
        }


        /// <summary>
        /// 수술 및 처치명 세팅한다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_OpName(PsmhDb pDbCon, string strPtno, string strInDate, string strOutDate)
        {
            string rtnVal =string.Empty;
            string SQL =string.Empty;
            string SqlErr =string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strOutDate == "99991231" || strOutDate == "99981231")
            {
                strOutDate =string.Empty; // ComQuery.CurrentDateTime(pDbCon, "D");
            }
            SQL = FormPatInfoQuery.Query_FormPatInfo_OpName(strPtno, strInDate, strOutDate);
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1 )
                {
                    rtnVal = rtnVal + "■" + "수술명 : " + dt.Rows[i]["OPTITLE"].ToString().Trim() + " " + dt.Rows[i]["OPDATE"].ToString().Trim();
                }
                else
                {
                    rtnVal = rtnVal + "■" + "수술명 : " + dt.Rows[i]["OPTITLE"].ToString().Trim() + " " + dt.Rows[i]["OPDATE"].ToString().Trim()
                        + ComNum.VBLF;
                }
            }
            
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 전과정보 세팅
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_IpdTrans(PsmhDb pDbCon, string strPtno, string strInDate, string strOutDate)
        {
            string rtnVal =string.Empty;
            string SQL =string.Empty;
            string SqlErr =string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strOutDate == "99991231" || strOutDate == "99981231")
            {
                strOutDate =string.Empty; // ComQuery.CurrentDateTime(pDbCon, "D");
            }
            SQL = FormPatInfoQuery.Query_FormPatInfo_IpdTrans(strPtno, strInDate, strOutDate);
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                {
                    rtnVal = rtnVal + "■" + dt.Rows[i]["DRNAME"].ToString().Trim() + " " + dt.Rows[i]["TODEPT"].ToString().Trim() + " " + dt.Rows[i]["TRSDATE"].ToString().Trim();
                }
                else
                {
                    rtnVal = rtnVal + "■" + dt.Rows[i]["DRNAME"].ToString().Trim() + " " + dt.Rows[i]["TODEPT"].ToString().Trim() + " " + dt.Rows[i]["TRSDATE"].ToString().Trim()
                        + ComNum.VBLF;
                }
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 경과요약 및 검사결과 - (퇴원자 방사선소견  리스트) 세팅
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_TestResult(PsmhDb pDbCon, string strPtno, string strInDate, string strOutDate)
        {
            string rtnVal = string.Empty;
            string SQL =string.Empty;
            string SqlErr =string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strOutDate == "99991231" || strOutDate == "99981231")
            {
                strOutDate =string.Empty; // ComQuery.CurrentDateTime(pDbCon, "D");
            }
            SQL = FormPatInfoQuery.Query_FormPatInfo_TestResult(strPtno, strInDate, strOutDate);
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                {
                    rtnVal +=  dt.Rows[i]["XNAME"].ToString().Trim() + " " + dt.Rows[i]["RESULT"].ToString().Trim() + " " + dt.Rows[i]["READDATE"].ToString().Trim();
                }
                else
                {
                    rtnVal += dt.Rows[i]["XNAME"].ToString().Trim() + " " + dt.Rows[i]["RESULT"].ToString().Trim() + " " + dt.Rows[i]["READDATE"].ToString().Trim()
                        + ComNum.VBLF;
                }
            }

            rtnVal += VB.String(4, ComNum.VBLF);
            dt.Dispose();
            return rtnVal;
        }

        /// <summary>
        /// LAB 결과 세팅
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_LabResult(PsmhDb pDbCon, string strPtno, string strInDate, string strOutDate)
        {
            string rtnVal =string.Empty;
            string SQL =string.Empty;
            string SqlErr =string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strOutDate == "99991231" || strOutDate == "99981231")
            {
                strOutDate =string.Empty; // ComQuery.CurrentDateTime(pDbCon, "D");
            }
            SQL = FormPatInfoQuery.Query_FormPatInfo_LabResult(strPtno, strInDate, strOutDate);
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

            string strRESULTDATE =string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strRESULTDATE != dt.Rows[i]["RESULTDATE"].ToString().Trim())
                {
                    if (i ==0)
                    {
                        rtnVal = rtnVal + "■" + dt.Rows[i]["RESULTDATE"].ToString().Trim()
                            + ComNum.VBLF;
                        rtnVal = rtnVal + dt.Rows[i]["EXAMNAME"].ToString().Trim() + " " + dt.Rows[i]["Refer"].ToString().Trim() + " " + dt.Rows[i]["Result"].ToString().Trim();
                    }
                    else
                    {
                        rtnVal = rtnVal + ComNum.VBLF + "■" + dt.Rows[i]["RESULTDATE"].ToString().Trim()
                            + ComNum.VBLF;
                        rtnVal = rtnVal + dt.Rows[i]["EXAMNAME"].ToString().Trim() + " " + dt.Rows[i]["Refer"].ToString().Trim() + " " + dt.Rows[i]["Result"].ToString().Trim();
                    }
                }
                else
                {
                    rtnVal = rtnVal + "," + dt.Rows[i]["EXAMNAME"].ToString().Trim() + " " + dt.Rows[i]["Refer"].ToString().Trim() + " " + dt.Rows[i]["Result"].ToString().Trim();
                }
                strRESULTDATE = dt.Rows[i]["RESULTDATE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// LAB 결과 세팅
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="AcpEmr"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_LabResultNew(PsmhDb pDbCon, EmrPatient AcpEmr)
        {
            string rtnVal = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
     
            SQL = FormPatInfoQuery.Query_FormPatInfo_LabResultNew(AcpEmr);
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

            string strRESULTDATE = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strRESULTDATE != dt.Rows[i]["RESULTDATE"].ToString().Trim())
                {
                    if (i == 0)
                    {
                        rtnVal = rtnVal + "■" + dt.Rows[i]["RESULTDATE"].ToString().Trim()
                            + ComNum.VBLF;
                        rtnVal = rtnVal + dt.Rows[i]["EXAMNAME"].ToString().Trim() + " " + dt.Rows[i]["Refer"].ToString().Trim() + " " + dt.Rows[i]["Result"].ToString().Trim();
                    }
                    else
                    {
                        rtnVal = rtnVal + ComNum.VBLF + "■" + dt.Rows[i]["RESULTDATE"].ToString().Trim()
                            + ComNum.VBLF;
                        rtnVal = rtnVal + dt.Rows[i]["EXAMNAME"].ToString().Trim() + " " + dt.Rows[i]["Refer"].ToString().Trim() + " " + dt.Rows[i]["Result"].ToString().Trim();
                    }
                }
                else
                {
                    rtnVal = rtnVal + "," + dt.Rows[i]["EXAMNAME"].ToString().Trim() + " " + dt.Rows[i]["Refer"].ToString().Trim() + " " + dt.Rows[i]["Result"].ToString().Trim();
                }
                strRESULTDATE = dt.Rows[i]["RESULTDATE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        /// <summary>
        /// 퇴원약 세팅
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <param name="bKor"></param>
        /// <returns></returns>
        public static string Set_FormPatInfo_MedicineDischarge(PsmhDb pDbCon, string strPtno, string strInDate, string strOutDate, bool bKor = false)
        {
            string rtnVal =string.Empty;
            string SQL =string.Empty;
            string SqlErr =string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strOutDate == "99991231" || strOutDate == "99981231")
            {
                strOutDate =string.Empty; // ComQuery.CurrentDateTime(pDbCon, "D");
            }
            SQL = FormPatInfoQuery.Query_FormPatInfo_MedicineDischarge(strPtno, strInDate, strOutDate);
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                {
                    rtnVal = rtnVal + "■" + dt.Rows[i][bKor ? "SUNAMEK" : "ORDERNAMES"].ToString().Trim() + "  " + dt.Rows[i]["ORDERNAME"].ToString().Trim() + "  " + dt.Rows[i]["REALQTY"].ToString().Trim() + "      " + dt.Rows[i]["QTY"].ToString().Trim() + " " + dt.Rows[i]["REALNAL"].ToString().Trim();
                }
                else
                {
                    rtnVal = rtnVal + "■" + dt.Rows[i][bKor ? "SUNAMEK" : "ORDERNAMES"].ToString().Trim() + "  " + dt.Rows[i]["ORDERNAME"].ToString().Trim() + "  " + dt.Rows[i]["REALQTY"].ToString().Trim() + "      " + dt.Rows[i]["QTY"].ToString().Trim() + " " + dt.Rows[i]["REALNAL"].ToString().Trim()
                    + ComNum.VBLF;
                }
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }
    }
}
