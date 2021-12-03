namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class XrayResultnewDrRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public XrayResultnewDrRepository()
        {
        }

        public List<XRAY_RESULTNEW_DR> GetItembyXCodeDeptCode(string strXCode, string strDeptCode, int nDay)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(READDATE,'YYYY-MM-DD') RDATE, PANO, RESULT, RESULT1 ");
            parameter.AppendSql("  FROM ADMIN.XRAY_RESULTNEW_DR                               ");
            parameter.AppendSql(" WHERE READDATE >= TRUNC(SYSDATE - :DAY)                           ");
            parameter.AppendSql("   AND READDATE <= TRUNC(SYSDATE)                                  ");
            parameter.AppendSql("   AND XCODE    = :XCODE                                           ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                                        ");
            parameter.AppendSql(" ORDER BY READDATE DESC, PANO                                      ");

            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DAY", nDay);

            return ExecuteReader<XRAY_RESULTNEW_DR>(parameter);
        }

        public List<XRAY_RESULTNEW_DR> GetItembyPaNoReadDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(ReadDate,'YYYY-MM-DD') RDate,Pano,Result,Result1    ");
            parameter.AppendSql("  FROM ADMIN.XRAY_RESULTNEW_DR                               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                ");
            parameter.AppendSql("   AND TRUNC(READDATE)=TO_DATE(:READDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                                     ");
            parameter.AppendSql("   AND XCODE = 'US24'                                              ");
            parameter.AppendSql(" ORDER BY ReadDate DESC,Pano                                       ");

            parameter.Add("PANO", fstrPtno);
            parameter.Add("READDATE", fstrJepDate);

            return ExecuteReader<XRAY_RESULTNEW_DR>(parameter);
        }
    }
}
