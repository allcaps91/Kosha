namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicResEtcRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResEtcRepository()
        {
        }

        public HIC_RES_ETC GetItembyWrtNo(long fnWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENGDRNO, TO_CHAR(PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE     ");
            parameter.AppendSql("     , TO_CHAR(GUNDATE, 'YYYY-MM-DD') GUNDATE                          ");
            parameter.AppendSql("     , GBPANJENG, GBPRINT, SOGEN                                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_RES_ETC                                         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                                                  ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReaderSingle<HIC_RES_ETC>(parameter);
        }

        public int SaveHicResEtc(long nWrtNo, string strJepDate, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("MERGE INTO ADMIN.HIC_RES_ETC a                   ");
            parameter.AppendSql("using dual d                                           ");
            parameter.AppendSql("   on (a.WRTNO     = :WRTNO                            ");
            parameter.AppendSql("  and  a.GUBUN     = :GUBUN)                           ");
            parameter.AppendSql(" when matched then                                     ");
            parameter.AppendSql("  update set                                           ");
            parameter.AppendSql("         GUNDATE   = TO_DATE(:GUNDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("       , GUBUN     = :GUBUN                            ");
            parameter.AppendSql("    when not matched then                              ");
            parameter.AppendSql("  insert                                               ");
            parameter.AppendSql("         (WRTNO                                        ");
            parameter.AppendSql("        , GUNDATE                                      "); 
            parameter.AppendSql("        , GUBUN)                                       ");
            parameter.AppendSql(" VALUES                                                ");
            parameter.AppendSql("         (:WRTNO                                       ");
            parameter.AppendSql("        , TO_DATE(:GUNDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("        , :GUBUN)                                      ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("GUNDATE", strJepDate);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNoGubun(string strGbPanjeng, long fnWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_RES_ETC SET     ");
            parameter.AppendSql("       GBPANJENG   = :GBPANJENG        ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO             ");
            parameter.AppendSql("   AND GUBUN      = :GUBUN             ");

            parameter.Add("GBPANJENG", strGbPanjeng);
            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNo(HIC_RES_ETC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_RES_ETC SET                         ");
            parameter.AppendSql("       GBPANJENG   = :GBPANJENG                            ");
            parameter.AppendSql("     , SOGEN       = :SOGEN                                ");
            parameter.AppendSql("     , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          "); 
            parameter.AppendSql("  WHERE WRTNO      = :WRTNO                                ");
            parameter.AppendSql("    AND GUBUN      = :GUBUN                                ");

            parameter.Add("GBPANJENG", item.GBPANJENG);
            parameter.Add("SOGEN", item.SOGEN);
            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("GUBUN", item.GUBUN); 

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNoGubun(HIC_RES_ETC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_RES_ETC SET                         ");
            parameter.AppendSql("       GBPANJENG   = :GBPANJENG                            ");
            parameter.AppendSql("     , PAN         = :PAN                                  ");
            parameter.AppendSql("     , SOGEN       = :SOGEN                                ");
            parameter.AppendSql("     , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql("     , SOGENREMARK = :SOGENREMARK                          ");
            parameter.AppendSql("     , JOCHI       = :JOCHI                                ");
            parameter.AppendSql("     , WORKYN      = :WORKYN                               ");
            parameter.AppendSql("     , SAHUCODE    = :SAHUCODE                             ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                                ");
            parameter.AppendSql("   AND GUBUN       = :GUBUN                                ");

            parameter.Add("GBPANJENG", item.GBPANJENG);
            parameter.Add("PAN", item.PAN);
            parameter.Add("SOGEN", item.SOGEN);
            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("SOGENREMARK", item.SOGENREMARK);
            parameter.Add("JOCHI", item.JOCHI);
            parameter.Add("WORKYN", item.WORKYN);
            parameter.Add("SAHUCODE", item.SAHUCODE);
            parameter.Add("GUBUN", item.GUBUN);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNo(long fnWRTNO, string strOK, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_RES_ETC SET ");
            
            if (strOK == "OK")
            {
                parameter.AppendSql("       GBPANJENG = 'Y'         ");
            }
            else
            {
                parameter.AppendSql("       GBPANJENG = ''          ");
            }
            parameter.AppendSql("  WHERE WRTNO = :WRTNO             ");
            parameter.AppendSql("    AND GUBUN = :GUBUN             ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteNonQuery(parameter);
        }

        public int UpdateByWrtnoGubun(long argWrtno, long argJobSabun, string argTongbodate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_RES_ETC SET                 ");
            parameter.AppendSql(" GBPRINT = 'Y'                                     ");
            parameter.AppendSql(" ,PRTSABUN = :JOBSABUN                             ");
            parameter.AppendSql(" ,TONGBODATE = TO_DATE(:TONGBODATE,'YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO                         ");
            parameter.AppendSql("   AND GUBUN      = :GUBUN                         ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("JOBSABUN", argJobSabun);
            parameter.Add("TONGBODATE", argTongbodate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(long argWRTNO, string argJepDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_RES_ETC                        ");
            parameter.AppendSql("       (WRTNO, GUNDATE, GUBUN)                             ");
            parameter.AppendSql("VALUES                                                     ");
            parameter.AppendSql("       (:WRTNO, TO_DATE(:JEPDATE, 'YYYY-MM-DD'), :GUBUN)   ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("JEPDATE", argJepDate);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
    }
}
