namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class XrayResultnewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public XrayResultnewRepository()
        {
        }

        public List<XRAY_RESULTNEW> GetResultNewbyPano(string strPtNo, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, DEPTCODE, XCODE, RESULT               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                  ");
            parameter.AppendSql(" WHERE PANO  = :PANO                               ");
            parameter.AppendSql("   AND SEEKDATE = TO_DATE(:SEEKDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND XCODE = 'HC341'                             ");
            parameter.AppendSql(" GROUP BY PANO, DEPTCODE, XCODE, RESULT            ");

            parameter.Add("PANO", strPtNo);
            parameter.Add("SEEKDATE", argDate);

            return ExecuteReader<XRAY_RESULTNEW>(parameter);
        }

        public List<XRAY_RESULTNEW> GetItembySeekDate(string strDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,TO_CHAR(SEEKDATE,'YYYY-MM-DD') SEEKDATE, XJONG, XCODE, RESULT, RESULT1 ");
            parameter.AppendSql("     , TO_CHAR(READDATE,'YYYY-MM-DD') READDATE                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                                                  ");
            parameter.AppendSql(" WHERE SEEKDATE = TO_DATE(:SEEKDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                                     ");
            parameter.AppendSql("   AND (APPROVE = 'Y' OR APPROVE IS NULL)                                          ");
            parameter.AppendSql(" ORDER BY Pano                                                                     ");

            parameter.Add("SEEKDATE", strDate);

            return ExecuteReader<XRAY_RESULTNEW>(parameter);
        }

        public List<XRAY_RESULTNEW> GetItembyPaNoSeekDateXCode(string strPtNo, string argBDate, string strXCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, DEPTCODE, XCODE, RESULT                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                                                  ");
            parameter.AppendSql(" WHERE PANO  = :PANO                                                               ");
            parameter.AppendSql("   AND SEEKDATE = TO_DATE(:SEEKDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND XCODE = :XCODE                                                              ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                                                          ");
            parameter.AppendSql(" GROUP BY PANO, DEPTCODE, XCODE, RESULT                                            ");

            parameter.Add("PANO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SEEKDATE", argBDate);
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<XRAY_RESULTNEW>(parameter);
        }

        public XRAY_RESULTNEW GetXDrCode1byPaNoSeekDateGroup(string fstrPano, string fstrJepDate, string[] strXCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT XDRCODE1                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                      ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SEEKDATE = TO_DATE(:SEEKDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND XCODE IN (:XCODE)                               ");
            parameter.AppendSql("   AND READTIME IS NOT NULL                            ");
            parameter.AppendSql(" GROUP BY XDRCODE1                                     ");

            parameter.Add("PANO", fstrPano);
            parameter.Add("SEEKDATE", fstrJepDate);
            parameter.AddInStatement("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<XRAY_RESULTNEW>(parameter);
        }

        public XRAY_RESULTNEW GetXDrCode1byPaNoSeekDate(string fstrPano, string fstrJepDate, string[] strXCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT XDRCODE1                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                      ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SEEKDATE = TO_DATE(:SEEKDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND XCODE IN (:XCODE)                               ");
            parameter.AppendSql("   AND READTIME IS NOT NULL                            ");

            parameter.Add("PANO", fstrPano);
            parameter.Add("SEEKDATE", fstrJepDate);
            parameter.AddInStatement("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<XRAY_RESULTNEW>(parameter);
        }

        public List<XRAY_RESULTNEW> GetItembyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT XJong,XDrCode1,XCode,XName,Result,TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                                                  ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                                ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                                                             ");
            parameter.AppendSql("   AND TRUNC(SeekDate)=TO_DATE(:SEEKDATE, 'YYYY-MM-DD')                            ");

            parameter.Add("PANO", fstrPtno);
            parameter.Add("SEEKDATE", fstrJepDate);

            return ExecuteReader<XRAY_RESULTNEW>(parameter);
        }

        public List<XRAY_RESULTNEW> GetItembyXCode(string strXCode, string strDeptCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,TO_CHAR(SEEKDATE,'YYYY-MM-DD') SEEKDATE,PANHIC ");
            parameter.AppendSql("     , RESULT                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                          ");
            parameter.AppendSql("WHERE ReadDate >= TRUNC(SYSDATE-14)                        ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                                ");
            parameter.AppendSql("   AND XCODE    = :XCODE                                   ");
            parameter.AppendSql("   AND PANHIC IS NOT NULL                                  ");

            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<XRAY_RESULTNEW>(parameter);
        }

        public List<XRAY_RESULTNEW> GetItembyPaNoSeekDateXCode1(string strPtNo, string argBDate, string strXCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEEKDATE, RESULT, RESULT1                               ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.XRAY_RESULTNEW                               ");
            parameter.AppendSql(" WHERE PANO  = :PANO                                           ");
            parameter.AppendSql(" AND SEEKDATE >= TO_DATE(:SEEKDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql(" AND XCODE = :XCODE                                            ");
            parameter.AppendSql(" ORDER BY SEEKDATE                                             ");

            parameter.Add("PANO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SEEKDATE", argBDate);
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<XRAY_RESULTNEW>(parameter);
        }
    }
}
