namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuResultRepository()
        {
        }

        public List<HEA_JEPSU_RESULT> GetItembySDate(string argBDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            if (argGubun == "HEA")
            {
                parameter.AppendSql("SELECT TO_CHAR(a.SDate,'YYYY-MM-DD') Jepdate                                              ");
                parameter.AppendSql("     , a.Ptno,a.Pano,a.Wrtno,b.ExCode,b.ResCode,b.Result,c.Code,c.GCode1,b.ROWID AS RID   ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HEA_RESULT b                                  ");
                parameter.AppendSql("     , (SELECT Code,GCode,GCode1 FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN ='A1') c           ");
                parameter.AppendSql(" WHERE a.WRTNO  = b.WRTNO(+)                                                              ");
                parameter.AppendSql("   AND b.EXCODE = c.GCODE                                                                 ");
                parameter.AppendSql("   AND a.SDATE  = TO_DATE(:SDATE ,'YYYY-MM-DD')                                           ");
                parameter.AppendSql("   AND a.GBSTS NOT IN ('0','D')                                                           ");
                parameter.AppendSql("   AND b.EXCODE IN (SELECT GCODE FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN ='A1')             "); //액팅할코드만
                parameter.AppendSql("   AND ( b.RESULT IS NULL OR b.RESULT = '')                                               "); //액팅처리안된것
                parameter.AppendSql(" ORDER BY a.WRTNO, b.ExCode                                                               ");
            }
            else if (argGubun == "HIC")
            {
                parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') Jepdate                                                     ");
                parameter.AppendSql("     , a.Ptno,a.Pano,a.Wrtno,a.XrayNo,b.ExCode,b.ResCode,b.Result,c.Code,c.GCode1,b.ROWID AS RID   ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RESULT b                                           ");
                parameter.AppendSql("     , (SELECT Code,GCode,GCode1 FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN ='A1') c                    ");
                parameter.AppendSql(" WHERE a.WRTNO   = b.WRTNO(+)                                                                      ");
                parameter.AppendSql("   AND b.EXCODE  = c.GCODE                                                                         ");
                parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:SDATE ,'YYYY-MM-DD')                                                   ");
                parameter.AppendSql("   AND a.GbChul = 'N'                                                                              ");
                parameter.AppendSql("   AND ( a.GBSTS IS NULL OR a.GBSTS <> 'D' )                                                       ");
                parameter.AppendSql("   AND b.EXCODE IN ( SELECT GCODE FROM KOSMOS_PMPA.HIC_CODE WHERE GUBUN ='A1')                     "); //액팅할코드만
                parameter.AppendSql("   AND ( b.RESULT IS NULL OR b.RESULT = '')                                                        "); //액팅처리안된것
                parameter.AppendSql(" ORDER BY a.WRTNO, b.ExCode                                                                        ");
            }

            parameter.Add("SDATE", argBDate);

            return ExecuteReader<HEA_JEPSU_RESULT>(parameter);
        }

        public List<HEA_JEPSU_RESULT> GetItembySDate(string strSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.PANO,a.WRTNO,a.SNAME,a.GJJONG,a.LTDCODE,a.ExamChange,a.GBDaily,a.AMPM2                        ");
            parameter.AppendSql("     , TO_CHAR(a.EntTime,'HH24:MI:SS') EntTime                                                         ");
            parameter.AppendSql("     , TO_CHAR(a.SDate,'YYYY-MM-DD') SDate                                                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HEA_RESULT b                                               ");
            parameter.AppendSql(" WHERE a.SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')                                                         ");
            parameter.AppendSql("   AND a.DELDATE IS  NULL                                                                              ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('0','D')                                                                        ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                                                               ");
            parameter.AppendSql(" GROUP BY a.SDate,a.PANO,a.WRTNO,a.SNAME,a.GJJONG,a.LTDCODE,a.EntTime,a.ExamChange,a.GBDaily,a.AMPM2   ");
            parameter.AppendSql(" ORDER By a.SName,a.Pano                                                                               "); //액팅할코드만

            parameter.Add("SDATE", strSDate);

            return ExecuteReader<HEA_JEPSU_RESULT>(parameter);
        }

        public List<HEA_JEPSU_RESULT> GetListBySDatePano(string argCurDate, long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.PANO, a.WRTNO, b.EXCODE,TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, a.AMPM2      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HEA_RESULT b                           ");
            parameter.AppendSql(" WHERE a.SDATE  = TO_DATE(:SDATE ,'YYYY-MM-DD')                                    ");
            parameter.AppendSql("   AND a.PANO = :PANO                                                              ");
            parameter.AppendSql("   AND a.WRTNO  = b.WRTNO(+)                                                       ");
            parameter.AppendSql("   AND b.EXCODE IN (SELECT CODE FROM KOSMOS_PMPA.HEA_CODE WHERE GUBUN ='13' GROUP By CODE)      "); //액팅할코드만
            
            parameter.Add("SDATE", argCurDate);
            parameter.Add("PANO", nPano);

            return ExecuteReader<HEA_JEPSU_RESULT>(parameter);
        }

        public List<HEA_JEPSU_RESULT> GetItembyPart(string strPart)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.PTno,b.ExCode                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU a, KOSMOS_PMPA.HEA_RESULT b   ");
            parameter.AppendSql(" WHERE a.SDate=TRUNC(SYSDATE)                              ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                   ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                ");
            parameter.AppendSql("   AND b.PART  = :PART                                     ");
            parameter.AppendSql("   AND b.Result IS NULL                                    ");

            parameter.Add("PART", strPart);

            return ExecuteReader<HEA_JEPSU_RESULT>(parameter);
        }

        public int UpdateResultActiveEntSabunbyRowIdWrtNo(string strActValue, string srActive, long idNumber, string strROWID, long nWRTNO, string argGubun)
        {
            MParameter parameter = CreateParameter();
            if (argGubun == "HEA")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULT SET      ");
            }
            else if (argGubun == "HIC")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET      ");
            }

            parameter.AppendSql("       RESULT   = :RESULT                  ");
            parameter.AppendSql("     , ACTIVE   = :ACTIVE                  ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                  ");
            parameter.AppendSql(" WHERE ROWID    = :RID                     ");
            parameter.AppendSql("   AND WRTNO    = :WRTNO                   ");
            parameter.AppendSql("   AND ( RESULT IS NULL OR RESULT ='' )    ");

            parameter.Add("RESULT", strActValue);
            parameter.Add("ACTIVE", srActive, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strROWID);
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
