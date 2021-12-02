namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultH827Repository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultH827Repository()
        {
        }

        public HIC_RESULT_H827 GetBloodDatebyWrtNoExCode(long nWRTNO, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(BloodDate,'YYYY-MM-DD') BloodDate   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT_H827                 ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                             ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                            ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT_H827>(parameter);
        }

        public int Update(HIC_RESULT_H827 item, string strBloodDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT_H827 SET                         ");
            parameter.AppendSql("       BLOODDATE = to_date(:BLOODDATE, 'yyyy-mm-dd')           ");
            if (strBloodDate == "")
            {
                parameter.AppendSql("     , ENTSABUN = '', ENTDATE = ''                         ");
            }
            else
            {
                parameter.AppendSql("     , ENTSABUN = :ENTSABUN                                ");
                parameter.AppendSql("     , ENTDATE  = SYSDATE                                  ");
            }
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                         ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                                        ");

            parameter.Add("BLOODDATE", item.BLOODDATE);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("EXCODE", item.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateRemarkbyWrtNo(string strRemark, long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT_H827 SET     ");
            parameter.AppendSql("       REMARK = :REMARK                    ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");

            parameter.Add("REMARK", strRemark);
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int Delete(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HIC_RESULT_H827                            ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU           ");
            parameter.AppendSql("                  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("                    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("                    AND DELDATE IS NOT NULL)                       ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_RESULT_H827 item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RESULT_H827                                        ");
            parameter.AppendSql("       (WRTNO, PANO, JEPDATE, EXCODE, BLOODDATE, REMARK, ENTSABUN, ENTDATE)    ");
            parameter.AppendSql("VALUES                                                                         ");
            parameter.AppendSql("       (:WRTNO,:PANO, to_date(:JEPDATE, 'yyyy-mm-dd'), :EXCODE                 ");
            parameter.AppendSql("     , to_date(:BLOODDATE, 'yyyy-mm-dd'), :REMARK, :ENTSABUN, SYSDATE)         ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("PANO", item.PANO);
            parameter.Add("JEPDATE", item.JEPDATE);
            parameter.Add("EXCODE", item.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BLOODDATE", item.BLOODDATE);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }
    }
}
