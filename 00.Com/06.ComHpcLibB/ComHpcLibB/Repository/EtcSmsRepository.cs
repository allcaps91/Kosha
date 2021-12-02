namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class EtcSmsRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public EtcSmsRepository()
        {
        }

        public List<ETC_SMS> GetListByBigoGubun(string argSmsLtdCode, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JOBDATE                                             ");
            parameter.AppendSql("      ,BIGO, SENDMSG, ROWID AS RID                         ");
            parameter.AppendSql("      ,CASE SENDTIME WHEN NULL THEN ''                     ");
            parameter.AppendSql("                     ELSE '¡Û' END ISSEND                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.ETC_SMS                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            parameter.AppendSql("   AND BIGO =:BIGO                                         ");
            parameter.AppendSql("   AND GUBUN =:GUBUN                                       ");

            parameter.Add("BIGO", argSmsLtdCode);
            parameter.Add("GUBUN", argGubun);

            return ExecuteReader<ETC_SMS>(parameter);
        }

        public int InsertAll(ETC_SMS item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.ETC_SMS (                                                          ");
            parameter.AppendSql("       (JOBDATE, PANO, SNAME, HPHONE, GUBUN                                                ");
            parameter.AppendSql("      , DEPTCODE, DRCODE, RTIME, RETTEL, SENDTIME, SENDMSG)                                ");
            parameter.AppendSql("VALUES                                                                                     ");
            parameter.AppendSql("       (TO_DATE(:JOBDATE, 'YYYY-MM-DD HH24:MI'), :PANO, :SNAME, :HPHONE, :GUBUN            ");
            parameter.AppendSql("      , :DEPTCODE, :DRCODE, TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI'), :RETTEL, '', :SENDMSG)  ");

            parameter.Add("JOBDATE", item.JOBDATE);
            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("DEPTCODE", item.DEPTCODE);
            parameter.Add("DRCODE", item.DRCODE);
            parameter.Add("RTIME", item.RTIME);
            parameter.Add("RETTEL", item.RETTEL);
            parameter.Add("SENDTIME", item.SENDTIME);
            parameter.Add("SENDMSG", item.SENDMSG);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyRTimePanoDeptcode(string strTime, string strPANO, string strDeptCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT FROM ETC_SMS                           ");
            parameter.AppendSql(" WHERE TRUNC(RTIME)=TO_DATE(:RTIME, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND PANO        = :PANO                                 ");
            parameter.AppendSql("   AND DEPTCODE    = :DEPTCODE                             ");
            parameter.AppendSql("   AND TRIM(Gubun) = '53'                                  ");

            parameter.Add("RTIME", strTime);
            parameter.Add("PANO", strPANO);
            parameter.Add("DEPTCODE", strDeptCode);

            return ExecuteScalar<int>(parameter);
        }

        public int Insert(ETC_SMS eSMS)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.ETC_SMS (                                                  ");
            parameter.AppendSql("       JOBDATE, HPHONE, GUBUN, RETTEL, SENDMSG, ENTSABUN, ENTDATE, BIGO            ");
            parameter.AppendSql(" ) VALUES (                                                                        ");
            parameter.AppendSql("      :JOBDATE, :HPHONE, :GUBUN, :RETTEL, :SENDMSG, :ENTSABUN, SYSDATE, :BIGO )    ");

            parameter.Add("JOBDATE", eSMS.JOBDATE);
            parameter.Add("HPHONE", eSMS.HPHONE);
            parameter.Add("GUBUN", eSMS.GUBUN);
            parameter.Add("RETTEL", eSMS.RETTEL);
            parameter.Add("SENDMSG", eSMS.SENDMSG);
            parameter.Add("ENTSABUN", eSMS.ENTSABUN);
            parameter.Add("BIGO", eSMS.BIGO);

            return ExecuteNonQuery(parameter);
        }
    }
}
