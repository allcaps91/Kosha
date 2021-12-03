namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class EtcAlimTalkRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public EtcAlimTalkRepository()
        {
        }

        public int Insert(ETC_ALIMTALK item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO ADMIN.ETC_ALIMTALK                                                          ");
            parameter.AppendSql("        (JOBDATE, SENDFLAG, SENDTYPE, TEMPCD, ENTSABUN, ENTDATE, PANO, SNAME                   ");
            parameter.AppendSql("      , HPHONE, LTDNAME, DEPTNAME, DRNAME, RDATE, RETTEL, SENDUID, WRTNO, GJNAME, QUESTLINK)   ");
            parameter.AppendSql(" VALUES                                                                                        ");
            parameter.AppendSql("        (SYSDATE, :SENDFLAG, :SENDTYPE, :TEMPCD, :ENTSABUN, SYSDATE, :PANO, :SNAME             ");
            parameter.AppendSql("      , :HPHONE, :LTDNAME, :DEPTNAME, :DRNAME, TO_DATE(:RDATE, 'YYYY-MM-DD HH24:MI'), :RETTEL  ");
            parameter.AppendSql("      , :SENDUID, :WRTNO, :GJNAME, :QUESTLINK)                                                 ");

            parameter.Add("SENDFLAG", item.SENDFLAG);
            parameter.Add("SENDTYPE", item.SENDTYPE);
            parameter.Add("TEMPCD", item.TEMPCD);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("PANO", item.PANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("LTDNAME", item.LTDNAME);
            parameter.Add("DEPTNAME", item.DEPTNAME);
            parameter.Add("DRNAME", item.DRNAME);
            parameter.Add("RDATE", item.RDATE);
            parameter.Add("RETTEL", item.RETTEL);
            parameter.Add("SENDUID", item.SENDUID);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("GJNAME", item.GJNAME);
            parameter.Add("QUESTLINK", item.QUESTLINK);

            return ExecuteNonQuery(parameter);
        }

        public void UpDateDelDateByPanoRDateTmpCD(long argPano, string argSDate, string argTmpCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE ADMIN.ETC_ALIMTALK               ");
            parameter.AppendSql("    SET DELDATE = SYSDATE                      ");
            parameter.AppendSql("  WHERE PANO = :PANO                           ");
            parameter.AppendSql("    AND RDATE >= TO_DATE(:RDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("    AND TEMPCD =:TEMPCD                        ");
            parameter.AppendSql("    AND DELDATE IS NULL                        ");

            parameter.Add("PANO", argPano);
            parameter.Add("RDATE", argSDate);
            parameter.Add("TEMPCD", argTmpCD);

            ExecuteNonQuery(parameter);
        }

        public List<ETC_ALIMTALK> GetItembyJobDate(string strFrDate, string strToDate, string strDeptName, string strTempCd, string strReportCode, string strSName, string strPhone)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JOBDATE,'YYYY-MM-DD HH24:MI') JOBDATE, PANO, DEPTNAME, SNAME                                        ");
            parameter.AppendSql("     , HPHONE, ENTSABUN, REPORT_CODE, TEMPCD, RETTEL, DECODE(REPORT_TYPE, 'AT','알림톡','SM','SMS') REPORT_TYPE    ");
            parameter.AppendSql("     , TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') RDATE, RESEND_REPORT_CODE, QUESTLINK                                    ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK                                                                                    ");
            parameter.AppendSql("  WHERE TRUNC(JOBDATE) >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                           ");
            parameter.AppendSql("    AND TRUNC(JOBDATE) <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                           ");
            if (!strDeptName.IsNullOrEmpty() && strDeptName != "**")
            {
                parameter.AppendSql("    AND DEPTNAME = :DEPTNAME                                                                                   ");
            }
            if (!strTempCd.IsNullOrEmpty() && strTempCd != "**")
            {
                parameter.AppendSql("    AND TEMPCD = :TEMPCD                                                                                       ");
            }
            if (strReportCode == "1")
            {
                parameter.AppendSql("    AND REPORT_CODE = '0000'                                                                                   ");
            }
            else if (strReportCode == "2")
            {
                parameter.AppendSql("    AND REPORT_CODE != '0000'                                                                                  ");
            }
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("    AND SNAME LIKE :SNAME                                                                                      ");
            }
            if (!strPhone.IsNullOrEmpty())
            {
                parameter.AppendSql("    AND HPHONE LIKE :HPHONE                                                                                    ");
            }
            parameter.AppendSql(" ORDER BY REQUEST_DATE                                                                                             ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strDeptName.IsNullOrEmpty() && strDeptName != "**")
            {
                parameter.Add("DEPTNAME", strDeptName);
            }
            if (!strTempCd.IsNullOrEmpty() && strTempCd != "**")
            {
                parameter.Add("TEMPCD", strTempCd);
            }
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }
            if (!strPhone.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("HPHONE", strPhone);
            }

            return ExecuteReader<ETC_ALIMTALK>(parameter);
        }

        public List<ETC_ALIMTALK> GetItembySendFlag(string strSendFlag)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RDate, 'YYYY-MM-DD HH24:MI') RDATE, TEMPCD, ENTSABUN, ENTDATE       ");
            parameter.AppendSql("     , PANO, SNAME, Hphone, LtdName, DEPTNAME, DrName, RDate, RetTel, SendUID      ");
            parameter.AppendSql("     , GjName, QUESTTITLE, QUESTMSG, QUESTLINK, SENDTYPE                           ");
            parameter.AppendSql("  From ADMIN.ETC_ALIMTALK                                                    ");
            parameter.AppendSql(" Where JOBDATE >= TRUNC(SYSDATE)                                                   ");
            parameter.AppendSql("   AND (WebSend IS NULL OR WebSend = '')                                           ");
            parameter.AppendSql("   AND SENDFLAG = :SENDFLAG                                                        ");

            parameter.Add("SENDFLAG", strSendFlag);

            return ExecuteReader<ETC_ALIMTALK>(parameter);
        }

        public ETC_ALIMTALK GetRDatebyJobDatePaNoHPhoneTempCd(string strFDate, string strTDate, string strPANO, string strTel, string strTempCD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDate           ");
            parameter.AppendSql("  FROM ADMIN.ETC_ALIMTALK                            ");
            parameter.AppendSql(" WHERE JOBDATE >= TO_DATE(:FRDATE,  'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND JOBDATE <= TO_DATE(:TODATE', 'YYYY-MM-DD HH24:MI')  ");
            //등록번호가 공백이 아닐경우
            if (strPANO.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PANO = :PANO                                    ");
            }
            parameter.AppendSql("   AND HPHONE = :HPHONE                                    ");
            parameter.AppendSql("   AND TEMPCD = :TEMPCD                                    "); //종검예약자
            parameter.AppendSql("   AND DELDATE IS NULL                                     ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (strPANO.IsNullOrEmpty())
            {
                parameter.Add("PANO", strPANO);
            }
            parameter.Add("HPHONE", strTel);
            parameter.Add("TEMPCD", strTempCD);

            return ExecuteReaderSingle<ETC_ALIMTALK>(parameter);
        }
    }
}
