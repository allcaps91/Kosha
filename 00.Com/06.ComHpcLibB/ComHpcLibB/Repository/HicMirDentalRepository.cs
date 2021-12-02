namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    /// <summary>
    /// 
    /// </summary>
    public class HicMirDentalRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirDentalRepository()
        {
        }

        public HIC_MIR_DENTAL GetMirDentalByWRTNO(long ArgWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MIRNO,YEAR,GUBUN,JOHAP,KIHO,JEPNO,JEPQTY,TAMT,JEPDATE                                               ");
            parameter.AppendSql(" ,IPGUMDATE,IPGUMAMT,BUILDCNT,BUILDSABUN,GBERRCHK, GbJohap                                                 ");
            parameter.AppendSql(" ,TO_CHAR(FrDate,'YYYY-MM-DD') FrDate                                                                      ");
            parameter.AppendSql(" ,TO_CHAR(ToDate,'YYYY-MM-DD') ToDate                                                                      ");
            parameter.AppendSql(" ,TO_CHAR(BuildDate,'YYYY-MM-DD') BuildDate,Life_Gbn,ROWID                                                 ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_MIR_DENTAL                                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql(" AND WRTNO  = :WRTNO                                                                                       ");

            parameter.Add("WRTNO", ArgWRTNO);

            return ExecuteReaderSingle<HIC_MIR_DENTAL>(parameter);
        }

        public HIC_MIR_DENTAL GetItembyMirno(long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIRNO,YEAR,GUBUN,JOHAP,KIHO,JEPNO,JEPQTY,TAMT,JEPDATE       ");
            parameter.AppendSql("     , IPGUMDATE,IPGUMAMT,BUILDCNT,BUILDSABUN,GBERRCHK, GbJohap    ");
            parameter.AppendSql("     , TO_CHAR(FrDate,'YYYY-MM-DD') FrDate                         ");
            parameter.AppendSql("     , TO_CHAR(ToDate,'YYYY-MM-DD') ToDate                         ");
            parameter.AppendSql("     , TO_CHAR(BuildDate,'YYYY-MM-DD') BuildDate,Life_Gbn,ROWID    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_DENTAL                                  ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                              ");

            parameter.Add("MIRNO", argMirno);

            return ExecuteReaderSingle<HIC_MIR_DENTAL>(parameter);
        }

        public int UpdateMirNoOldMirNobyMirNo(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_DENTAL SET                  ");
            parameter.AppendSql("       MIRNO    = 0                                    ");
            parameter.AppendSql("     , OLDMIRNO = :MIRNO                               ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                               ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyMirNo(string strErrChk, long nCnt, long huQty, long nTotAmt, long fnMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_DENTAL SET                  ");
            parameter.AppendSql("       GBERRCHk = :GBERRCHk                            ");
            parameter.AppendSql("     , BUILDCNT = :BUILDCNT                            ");
            parameter.AppendSql("     , HUQTY    = :HUQTY                               ");
            parameter.AppendSql("     , TAMT     = :TAMT                                ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                               ");

            parameter.Add("GBERRCHk", strErrChk, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("BUILDCNT", nCnt);
            parameter.Add("HUQTY", huQty);
            parameter.Add("TAMT", nTotAmt);
            parameter.Add("MIRNO", fnMirNo);

            return ExecuteNonQuery(parameter);
        }

        public int InsertAll(long nMirno, string strYear, string strJohap, long FnTotCNT, string strGbJohap, string argFDate, string argTDate, string strkiho, string strChasu, string IdNumber, string strMirGbn, string strLife_Gbn, long FnHuCnt)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_MIR_DENTAL                                         ");
            parameter.AppendSql("        (MIRNO,YEAR,GUBUN,JOHAP, JEPQTY, GBJOHAP,FRDATE,TODATE                 ");
            parameter.AppendSql("      , KIHO,BUILDCNT,BUILDDATE,CHASU,BUILDSABUN,MIRGBN,LIFE_GBN,HUQTY)        ");
            parameter.AppendSql(" VALUES                                                                        ");
            parameter.AppendSql("        (:MIRNO,:YEAR,:GUBUN,:JOHAP, :JEPQTY, :GBJOHAP                         ");
            parameter.AppendSql("      , TO_DATE(:FRDATE, 'yyyy-mm-dd'), TO_DATE(:TODATE, 'yyyy-mm-dd')         ");
            parameter.AppendSql("      , :KIHO,:BUILDCNT,SYSDATE,:CHASU,:BUILDSABUN,:MIRGBN,:LIFE_GBN,:HUQTY)   ");

            parameter.Add("MIRNO", nMirno);
            parameter.Add("YEAR", strYear, Oracle.DataAccess.Client.OracleDbType.Char);
            switch (strJohap)
            {
                case "사업장":
                    parameter.Add("GUBUN", "6");
                    parameter.Add("JOHAP", "K");
                    break;
                case "공무원":
                    parameter.Add("GUBUN", "6");
                    parameter.Add("JOHAP", "G");
                    break;
                case "성인병":
                    parameter.Add("GUBUN", "6");
                    parameter.Add("JOHAP", "J");
                    break;
                case "통합":
                    parameter.Add("GUBUN", "6");
                    parameter.Add("JOHAP", "T");
                    break;
                default:
                    break;
            }
            parameter.Add("JEPQTY", FnTotCNT);
            parameter.Add("GBJOHAP", strGbJohap, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);
            parameter.Add("KIHO", strkiho, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("BUILDCNT", FnTotCNT);
            parameter.Add("CHASU", strChasu, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("BUILDSABUN", IdNumber);
            parameter.Add("MIRGBN", strMirGbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LIFE_GBN", strLife_Gbn, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("HUQTY", FnHuCnt);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateJepNoFileNameJepDatebyMirNo(string strJepNo, string strFileName, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_DENTAL SET                  ");
            parameter.AppendSql("       JEPNO    = :JEPNO                               ");
            parameter.AppendSql("     , FILENAME = :FILENAME                            ");
            parameter.AppendSql("     , JEPDATE  = TRUNC(SYSDATE)                       ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                               ");

            parameter.Add("JEPNO", strJepNo);
            parameter.Add("FILENAME", strFileName);
            parameter.Add("MIRNO", argMirno);

            return ExecuteNonQuery(parameter);
        }

        public HIC_MIR_DENTAL GetChasubyMirNo(long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CHASU                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_DENTAL              ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                          ");

            parameter.Add("MIRNO", argMirno);

            return ExecuteReaderSingle<HIC_MIR_DENTAL>(parameter);
        }

        public List<HIC_MIR_DENTAL> GetItembyMirnoYear(string strMirNo, string strYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIRNO,YEAR,GUBUN,JOHAP,KIHO,JEPNO,JEPQTY,HuQty,TAMT,JEPDATE,DELDATE,IPGUMDATE   ");
            parameter.AppendSql("     , IpGumAmt , BuildCnt, BuildDate, BuildSabun, GbErrChk, FileName                  ");
            parameter.AppendSql("     , TO_CHAR(FRDATE,'YYYY.MM.DD') FRDATE,TO_CHAR(TODATE,'YYYY.MM.DD') TODATE         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_DENTAL                                                      ");
            if (!strMirNo.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE MIRNO = :MIRNO                                                              ");
            }
            parameter.AppendSql("   AND YEAR = :YEAR                                                                    ");

            if (!strMirNo.IsNullOrEmpty())
            {
                parameter.Add("MIRNO", strMirNo);
            }
            parameter.Add("YEAR", strYear);

            return ExecuteReader<HIC_MIR_DENTAL>(parameter);
        }
    }
}
