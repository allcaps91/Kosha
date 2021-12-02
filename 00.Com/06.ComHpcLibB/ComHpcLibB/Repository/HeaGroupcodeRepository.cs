namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaGroupcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGroupcodeRepository()
        {
        }

        
        public List<HEA_GROUPCODE> Read_Hea_GroupCode(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE       ");
            parameter.AppendSql(" WHERE CODE = :CODE                    ");

            parameter.Add("CODE", CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_GROUPCODE>(parameter);
        }

        public HEA_GROUPCODE GetYNamebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.YNAME                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL  a     ");            
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_GROUPCODE b     ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                ");
            parameter.AppendSql("   AND a.CODE = b.CODE(+)              ");
            parameter.AppendSql("   AND b.JONG IN ('11','12')           ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HEA_GROUPCODE>(parameter);
        }

        public int Delete(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_GROUPCODE       ");
            parameter.AppendSql("   SET DELDATE     = TRUNC(SYSDATE)    ");
            parameter.AppendSql("      ,ENTDATE     = SYSDATE           ");
            parameter.AppendSql("      ,ENTSABUN    =:ENTSABUN          ");
            parameter.AppendSql(" WHERE ROWID       =:RID               ");

            #region Query 변수대입
            parameter.Add("ENTSABUN",   clsType.User.IdNumber);
            parameter.Add("RID",        argRowid);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpDate(HEA_GROUPCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_GROUPCODE   ");
            parameter.AppendSql("   SET JONG	    =:JONG	        ");
            parameter.AppendSql("       NAME	    =:NAME	        ");
            parameter.AppendSql("       YNAME	    =:YNAME	        ");
            parameter.AppendSql("       GBSEX	    =:GBSEX	        ");
            parameter.AppendSql("       GBSELECT    =:GBSELECT      ");
            parameter.AppendSql("       BURATE	    =:BURATE	    ");
            parameter.AppendSql("       GBSELF	    =:GBSELF	    ");
            parameter.AppendSql("       GBGAM	    =:GBGAM	        ");
            parameter.AppendSql("       LTDCODE	    =:LTDCODE	    ");
            parameter.AppendSql("       SDATE	    =:SDATE	        ");
            parameter.AppendSql("       DELDATE	    =:DELDATE	    ");
            parameter.AppendSql("       AMT		    = AMT		    ");
            parameter.AppendSql("       SUDATE	    =:SUDATE	    ");
            parameter.AppendSql("       OLDAMT	    =:OLDAMT	    ");
            parameter.AppendSql("       TYPE	    =:TYPE	        ");
            parameter.AppendSql("       ENTDATE	    =:ENTDATE	    ");
            parameter.AppendSql("       GBPRINT	    =:GBPRINT	    ");
            parameter.AppendSql("       ENDOCODE    =:ENDOCODE      ");
            parameter.AppendSql("       EXAMNAME    =:EXAMNAME      ");
            parameter.AppendSql(" WHERE ROWID       =:RID           ");

            #region Query 변수대입
            parameter.Add("JONG",     item.JONG);
            parameter.Add("NAME",     item.NAME);
            parameter.Add("YNAME",    item.YNAME);
            parameter.Add("GBSEX",    item.GBSEX);
            parameter.Add("GBSELECT", item.GBSELECT);
            parameter.Add("BURATE",   item.BURATE);
            parameter.Add("GBSELF",   item.GBSELF);
            parameter.Add("GBGAM",    item.GBGAM);
            parameter.Add("LTDCODE",  item.LTDCODE);
            parameter.Add("SDATE",    item.SDATE);
            parameter.Add("DELDATE",  item.DELDATE);
            parameter.Add("AMT",      item.AMT);
            parameter.Add("SUDATE",   item.SUDATE);
            parameter.Add("OLDAMT",   item.OLDAMT);
            parameter.Add("TYPE",     item.TYPE);
            parameter.Add("ENTDATE",  item.ENTDATE);
            parameter.Add("GBPRINT",  item.GBPRINT);
            parameter.Add("ENDOCODE", item.ENDOCODE);
            parameter.Add("EXAMNAME", item.EXAMNAME);
            
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public string GetGbSelectByCode(string gRPCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBSELECT                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE     ");
            parameter.AppendSql(" WHERE CODE =:CODE                ");

            parameter.Add("CODE", gRPCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public long GetLongServiceWorkerTotalAmtByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(b.AMT) TOAMT                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL a              ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_GROUPCODE b             ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                        ");
            parameter.AppendSql("   AND a.CODE IN ('14576','14575')             ");
            parameter.AppendSql("   AND a.CODE = b.CODE                         ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                       ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public IList<HEA_GROUPCODE> GetListByItem(long nLtdCode, string strSex, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT CODE, NAME, AMT, OLDAMT                        ");
            parameter.AppendSql("       ,TO_CHAR(SUDATE, 'YYYY-MM-DD') SUDATE           ");
            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HEA_GROUPCODE            ");
            parameter.AppendSql("  WHERE 1 = 1                                          ");
            if (nLtdCode > 0)
            {
                parameter.AppendSql("    AND LTDCODE =:LTDCODE                          ");
                parameter.AppendSql("    AND SDATE  <= TO_DATE(:SDATE,'YYYY-MM-DD')     ");
            }
            else
            {
                parameter.AppendSql("    AND NAME LIKE :NAME                              ");
                parameter.AppendSql("    AND(LTDCODE IS NULL OR LTDCODE = 0)              ");
                parameter.AppendSql("    AND (NAME LIKE '%(남)%' OR NAME LIKE '%(여)%')   ");
            }
            if (!strSex.IsNullOrEmpty())
            {
                parameter.AppendSql("    AND GBSEX IN (:SEX, '*')                       ");
            }
            parameter.AppendSql("    AND (DELDATE IS NULL OR DELDATE > TO_DATE(:DELDATE,'YYYY-MM-DD'))  ");
            parameter.AppendSql("  ORDER BY CODE                                        ");

            if (nLtdCode > 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
                parameter.Add("SDATE", strDate);
            }
            else
            {
                parameter.AddLikeStatement("NAME", "<" + VB.Left(strDate, 4) + ">");
            }

            if (!strSex.IsNullOrEmpty())
            {
                parameter.Add("SEX", strSex);
            }

            parameter.Add("DELDATE", strDate);

            return ExecuteReader<HEA_GROUPCODE>(parameter);
        }

        public List<HEA_GROUPCODE> GetListByAll(string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT JONG,CODE,NAME                                 ");
            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HEA_GROUPCODE            ");
            parameter.AppendSql("  WHERE 1 = 1                                          ");
            if (!argJong.Equals("**"))
            {
                parameter.AppendSql("    AND JONG =:JONG                                ");
            }
            parameter.AppendSql("    AND DELDATE IS NULL                                ");
            parameter.AppendSql("  ORDER BY Code                                        ");

            parameter.Add("JONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_GROUPCODE>(parameter);
        }

        public int Insert(HEA_GROUPCODE item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HEA_GROUPCODE (                                       ");
            parameter.AppendSql("       CODE,JONG,NAME,YNAME,GBSEX,GBSELECT,BURATE,GBSELF,GBGAM,LTDCODE         ");
            parameter.AppendSql("      ,SDATE,DELDATE,AMT,SUDATE,OLDAMT,TYPE,ENTDATE,ENTSABUN,GBPRINT           ");
            parameter.AppendSql("      ,EXAMNAME,ENDOCODE                                                       ");
            parameter.AppendSql(") VALUES (                                                                     ");
            parameter.AppendSql("       CODE,:JONG,:NAME,:YNAME,:GBSEX,:GBSELECT,:BURATE,:GBSELF,:GBGAM,:LTDCODE");
            parameter.AppendSql("      ,:SDATE,:DELDATE,:AMT,:SUDATE,:OLDAMT,:TYPE,:ENTDATE,:ENTSABUN,:GBPRINT  ");
            parameter.AppendSql("      ,:EXAMNAME,:ENDOCODE                                                     ");
            parameter.AppendSql(")");

            #region Query 변수대입
            parameter.Add("CODE",       item.CODE);
            parameter.Add("JONG",       item.JONG);
            parameter.Add("NAME",       item.NAME);
            parameter.Add("YNAME",      item.YNAME);
            parameter.Add("GBSEX",      item.GBSEX);
            parameter.Add("GBSELECT",   item.GBSELECT);
            parameter.Add("BURATE",     item.BURATE);
            parameter.Add("GBSELF",     item.GBSELF);
            parameter.Add("GBGAM",      item.GBGAM);
            parameter.Add("LTDCODE",    item.LTDCODE);
            parameter.Add("SDATE",      item.SDATE);
            parameter.Add("DELDATE",    item.DELDATE);
            parameter.Add("AMT",        item.AMT);
            parameter.Add("SUDATE",     item.SUDATE);
            parameter.Add("OLDAMT",     item.OLDAMT);
            parameter.Add("TYPE",       item.TYPE);
            parameter.Add("ENTDATE",    item.ENTDATE);
            parameter.Add("ENTSABUN",   item.ENTSABUN);
            parameter.Add("GBPRINT",    item.GBPRINT);
            parameter.Add("EXAMNAME",   item.EXAMNAME);
            parameter.Add("ENDOCODE",   item.ENDOCODE);
            
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_GROUPCODE> GetListByJongLtdCode(string argJong, long argLtdCode, bool bDel)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,JONG,GBSELECT,NAME,GBSEX,BURATE,GBSELF,GBGAM               ");
            parameter.AppendSql("      ,DECODE(LTDCODE, '','****',LTDCODE) AS V_LTDCODE, LTDCODE        ");            
            parameter.AppendSql("      ,DECODE(LTDCODE, '','공통',KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE)) AS LTDNAME   ");
            parameter.AppendSql("      ,DELDATE, SDATE                              "); 
            parameter.AppendSql("      ,AMT,SUDATE,TYPE,OLDAMT,ROWID AS RID");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                           ");
            if (argJong.Equals("**"))
            {
                parameter.AppendSql("   AND JONG >='**'                         ");
            }
            else
            {
                parameter.AppendSql("   AND JONG =:JONG                         ");
            }

            if (argLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE=:LTDCODE                    ");
            }

            if (!bDel)
            {
                parameter.AppendSql("    AND DELDATE IS NULL                    ");
            }

            parameter.AppendSql("ORDER BY JONG,NAME,LTDCODE,CODE                ");

            parameter.AddLikeStatement("JONG", argJong);

            if (argLtdCode > 0)
            {
                parameter.AddLikeStatement("LTDCODE", argLtdCode);
            }
            

            return ExecuteReader<HEA_GROUPCODE>(parameter);
        }

        public HEA_GROUPCODE GetItemByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JONG,NAME,YNAME,GBSEX,GBSELECT,BURATE,GBSELF,GBGAM,LTDCODE                  ");
            parameter.AppendSql("      ,DECODE(LTDCODE, '','****',LTDCODE) AS V_LTDCODE                             ");
            parameter.AppendSql("      ,SDATE,DELDATE                                                               ");
            parameter.AppendSql("      ,AMT,SUDATE,OLDAMT,TYPE,GBPRINT,EXAMNAME                                     ");            
            parameter.AppendSql("      ,DECODE(LTDCODE, '','공통',KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE)) AS LTDNAME   ");
            parameter.AppendSql("      ,ENDOCODE,ROWID AS RID                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE                                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND CODE =:CODE                                                                 ");

            parameter.Add("CODE", argCode);

            return ExecuteReaderSingle<HEA_GROUPCODE>(parameter);
        }

        public HEA_GROUPCODE GetItemByCodeDeldate(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBPRINT, EXAMNAME                                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE                                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND CODE =:CODE                                                                 ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");

            parameter.Add("CODE", argCode);

            return ExecuteReaderSingle<HEA_GROUPCODE>(parameter);
        }

        public List<HEA_GROUPCODE> GetCodeNameByLikeName(string argName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE   ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND NAME LIKE :NAME             ");

            parameter.AddLikeStatement("NAME", argName);

            return ExecuteReader<HEA_GROUPCODE>(parameter);
        }

        public string GetJongByCode(string argGroupCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE                         ");
            parameter.AppendSql(" WHERE 1 = 1                                             ");
            parameter.AppendSql("   AND CODE =:CODE                                       ");
            //parameter.AppendSql("   AND JONG <> '**'                                      ");
            parameter.AppendSql("   AND GBSELECT = 'N'                                    ");

            parameter.Add("CODE", argGroupCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
    }
}
