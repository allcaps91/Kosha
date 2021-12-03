namespace ComHpcLibB.Repository
{
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class BasPcconfigRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BAS_PCCONFIG> GetConfig(string IPADDRESS)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT IPADDRESS, GUBUN,   CODE,    VALUEV, VALUEN ");
            parameter.AppendSql("     , DISSEQNO,  INPDATE, INPTIME, DELGB          ");
            parameter.AppendSql("  FROM ADMIN.BAS_PCCONFIG                    ");
            parameter.AppendSql(" WHERE GUBUN = '검진센터계측항목'                  ");

            if (!string.IsNullOrEmpty(IPADDRESS))
            {
                parameter.AppendSql("   AND IPADDRESS = :IPADDRESS                  ");
            }
            parameter.AppendSql("  ORDER BY DISSEQNO, CODE                          ");

            parameter.Add("IPADDRESS", IPADDRESS);

            return ExecuteReader<BAS_PCCONFIG>(parameter);
        }

        public string GetConfig_Check(string iPADDRESS, string sGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT VALUEV                                      ");
            parameter.AppendSql("  FROM ADMIN.BAS_PCCONFIG                    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                              ");

            if (!string.IsNullOrEmpty(iPADDRESS))
            {
                parameter.AppendSql("   AND IPADDRESS = :IPADDRESS                  ");
            }
            parameter.AppendSql("  ORDER BY CODE                                    ");

            parameter.Add("IPADDRESS", iPADDRESS);
            parameter.Add("GUBUN", sGubun);

            return ExecuteScalar<string>(parameter);
        }

        public int GetConfig_Code(string sIpAddress)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.BAS_PCCONFIG   ");
            parameter.AppendSql(" WHERE IPADDRESS = :IPADDRESS     ");

            parameter.Add("IPADDRESS", sIpAddress);

            return ExecuteNonQuery(parameter);
        }

        public string GetConfig_Code(string gstrCOMIP, string sGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE                                        ");
            parameter.AppendSql("  FROM ADMIN.BAS_PCCONFIG                    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                              ");

            if (!string.IsNullOrEmpty(gstrCOMIP))
            {
                parameter.AppendSql("   AND IPADDRESS = :IPADDRESS                  ");
            }
            parameter.AppendSql("  ORDER BY CODE                                    ");

            parameter.Add("IPADDRESS", gstrCOMIP);
            parameter.Add("GUBUN", sGubun);

            return ExecuteScalar<string>(parameter);
        }

        public int Save_PcConfig(BAS_PCCONFIG item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("MERGE INTO ADMIN.BAS_PCCONFIG a  ");
            parameter.AppendSql("using dual d                           ");
            parameter.AppendSql("   on (a.GUBUN     = :GUBUN            ");
            parameter.AppendSql("  and  a.CODE      = :CODE             ");
            parameter.AppendSql("  and  a.IPADDRESS = :IPADDRESS)       ");
            parameter.AppendSql(" when matched then                     ");
            parameter.AppendSql("  update set                           ");
            parameter.AppendSql("          VALUEV    = :VALUEV          ");
            parameter.AppendSql("        , VALUEN    = :VALUEN          ");
            parameter.AppendSql("        , DISSEQNO  = :DISSEQNO        ");
            parameter.AppendSql("        , INPDATE   = :INPDATE         ");
            parameter.AppendSql("        , INPTIME   = :INPTIME         ");
            parameter.AppendSql("        , DELGB     = :DELGB           ");
            parameter.AppendSql("    when not matched then              ");
            parameter.AppendSql("  insert                               ");
            parameter.AppendSql("         (IPADDRESS                    ");
            parameter.AppendSql("        , GUBUN                        ");
            parameter.AppendSql("        , CODE                         ");
            parameter.AppendSql("        , VALUEV                       ");
            parameter.AppendSql("        , VALUEN                       ");
            parameter.AppendSql("        , DISSEQNO                     ");
            parameter.AppendSql("        , INPDATE                      ");
            parameter.AppendSql("        , INPTIME                      ");
            parameter.AppendSql("        , DELGB )                      ");
            parameter.AppendSql(" VALUES                                ");
            parameter.AppendSql("       (  :IPADDRESS                   ");
            parameter.AppendSql("        , :GUBUN                       ");
            parameter.AppendSql("        , :CODE                        ");
            parameter.AppendSql("        , :VALUEV                      ");
            parameter.AppendSql("        , :VALUEN                      ");
            parameter.AppendSql("        , :DISSEQNO                    ");
            parameter.AppendSql("        , :INPDATE                     ");
            parameter.AppendSql("        , :INPTIME                     ");
            parameter.AppendSql("        , :DELGB)                      ");

            parameter.Add("IPADDRESS", item.IPADDRESS); 
            parameter.Add("GUBUN",     item.GUBUN);
            parameter.Add("CODE",      item.CODE);
            parameter.Add("VALUEV",    item.VALUEV);
            parameter.Add("VALUEN",    item.VALUEN);
            parameter.Add("DISSEQNO",  item.DISSEQNO);
            parameter.Add("INPDATE",   item.INPDATE);
            parameter.Add("INPTIME",   item.INPTIME);
            parameter.Add("DELGB",     item.DELGB);

            return ExecuteNonQuery(parameter);
        }

        public int Save_PcConfig_Test(BAS_PCCONFIG item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.BAS_PCCONFIG   ");
            parameter.AppendSql("         (IPADDRESS                    ");
            parameter.AppendSql("        , GUBUN                        ");
            parameter.AppendSql("        , CODE                         ");
            parameter.AppendSql("        , VALUEV                       ");
            parameter.AppendSql("        , VALUEN                       ");
            parameter.AppendSql("        , DISSEQNO                     ");
            parameter.AppendSql("        , INPDATE                      ");
            parameter.AppendSql("        , INPTIME                      ");
            parameter.AppendSql("        , DELGB )                      ");
            parameter.AppendSql(" VALUES                                ");
            parameter.AppendSql("       (  :IPADDRESS                   ");
            parameter.AppendSql("        , :GUBUN                       ");
            parameter.AppendSql("        , :CODE                        ");
            parameter.AppendSql("        , :VALUEV                      ");
            parameter.AppendSql("        , :VALUEN                      ");
            parameter.AppendSql("        , :DISSEQNO                    ");
            parameter.AppendSql("        , :INPDATE                     ");
            parameter.AppendSql("        , :INPTIME                     ");
            parameter.AppendSql("        , :DELGB)                      ");

            parameter.Add("IPADDRESS", item.IPADDRESS);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("CODE", item.CODE);
            parameter.Add("VALUEV", item.VALUEV);
            parameter.Add("VALUEN", item.VALUEN);
            parameter.Add("DISSEQNO", item.DISSEQNO);
            parameter.Add("INPDATE", item.INPDATE);
            parameter.Add("INPTIME", item.INPTIME);
            parameter.Add("DELGB", item.DELGB);

            return ExecuteNonQuery(parameter);
        }

        public BAS_PCCONFIG GetConfig_PFTSN(string IPADDRESS)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT VALUEV                                      ");
            parameter.AppendSql("  FROM ADMIN.BAS_PCCONFIG                    ");
            parameter.AppendSql(" WHERE GUBUN = '폐활량장비S/N'                     ");

            if (!string.IsNullOrEmpty(IPADDRESS))
            {
                parameter.AppendSql("   AND IPADDRESS = :IPADDRESS                  ");
            }
            parameter.AppendSql("  ORDER BY CODE                                    ");

            parameter.Add("IPADDRESS", IPADDRESS);

            return ExecuteReaderSingle<BAS_PCCONFIG>(parameter);
        }

        public string GetConfig_CardGubun(string IPADDRESS)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT VALUEV                                      ");
            parameter.AppendSql("  FROM ADMIN.BAS_PCCONFIG                    ");
            parameter.AppendSql(" WHERE GUBUN = '카드구분'                     ");

            if (!string.IsNullOrEmpty(IPADDRESS))
            {
                parameter.AppendSql("   AND IPADDRESS = :IPADDRESS                  ");
            }

            parameter.Add("IPADDRESS", IPADDRESS);

            return ExecuteScalar<string>(parameter);
        }
    }
}
