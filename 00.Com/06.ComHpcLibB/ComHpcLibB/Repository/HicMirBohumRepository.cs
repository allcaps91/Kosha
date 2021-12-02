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
    public class HicMirBohumRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirBohumRepository()
        {
        }

        public HIC_MIR_BOHUM GetItemByMirno(long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIRNO,YEAR,GUBUN,JOHAP,GBJOHAP,KIHO,JEPNO,JEPQTY,TAMT,JEPDATE,ONE_QTY,ONE_TAMT          ");
            parameter.AppendSql("     , ONE_INWON011,ONE_INWON012,ONE_INWON021,ONE_INWON022,ONE_INWON031,ONE_INWON032           ");
            parameter.AppendSql("     , ONE_INWON041,ONE_INWON042,ONE_INWON051,ONE_INWON052,ONE_INWON061,ONE_INWON062           ");
            parameter.AppendSql("     , ONE_INWON071,ONE_INWON072,ONE_INWON081,ONE_INWON082,ONE_INWON091,ONE_INWON092           ");
            parameter.AppendSql("     , ONE_INWON101,ONE_INWON102,ONE_INWON111,ONE_INWON112,ONE_INWON121,ONE_INWON122           ");
            parameter.AppendSql("     , TWO_QTY,TWO_TAMT,TWO_INWON01,TWO_INWON02,TWO_INWON03,TWO_INWON04,TWO_INWON05            ");
            parameter.AppendSql("     , TWO_INWON06,TWO_INWON07,TWO_INWON08,TWO_INWON09,TWO_INWON10,TWO_INWON11                 ");
            parameter.AppendSql("     , TWO_INWON12,TWO_INWON13,TWO_INWON14,TWO_INWON15,IPGUMDATE,IPGUMAMT,ROWID, CHASU         ");
            parameter.AppendSql("     , TWO_INWON21,TWO_INWON22,TWO_INWON23,TWO_INWON24,TWO_INWON25,TWO_INWON26                 ");
            parameter.AppendSql("     , TWO_INWON27,TWO_INWON28,TWO_INWON29,TWO_INWON30                                         ");
            parameter.AppendSql("     , TWO_INWON31,TWO_INWON32,TWO_INWON33,TWO_INWON34                                         ");
            parameter.AppendSql("     , TWO_INWON35,TWO_INWON36,TWO_INWON37,TWO_INWON38,TWO_INWON39,TWO_INWON40,TWO_INWON41     ");
            parameter.AppendSql("     , BUILDSABUN,BUILDCNT,GBERRCHK,Life_Gbn                                                   ");
            parameter.AppendSql("     , TO_CHAR(FrDate,'YYYY-MM-DD') FrDate                                                     ");
            parameter.AppendSql("     , TO_CHAR(ToDate,'YYYY-MM-DD') ToDate                                                     ");
            parameter.AppendSql("     , TO_CHAR(BuildDate,'YYYY-MM-DD') BuildDate                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_BOHUM                                                               ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                                                          ");

            parameter.Add("MIRNO", argMirno);

            return ExecuteReaderSingle<HIC_MIR_BOHUM>(parameter);
        }

        public int UpdatebyMirNo(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_BOHUM SET               ");
            parameter.AppendSql("       MIRNO = 0                                   ");
            parameter.AppendSql("     , OLDMIRNO = :MIRNO                           ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                           ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyRowId(HIC_MIR_BOHUM item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_BOHUM SET               ");
            parameter.AppendSql("       ONE_INWON011 = :ONE_INWON011                ");
            parameter.AppendSql("     , ONE_INWON012 = :ONE_INWON012                ");
            parameter.AppendSql("     , ONE_INWON021 = :ONE_INWON021                ");
            parameter.AppendSql("     , ONE_INWON022 = :ONE_INWON022                ");
            parameter.AppendSql("     , ONE_INWON031 = :ONE_INWON031                ");
            parameter.AppendSql("     , ONE_INWON032 = :ONE_INWON032                ");
            parameter.AppendSql("     , ONE_INWON041 = :ONE_INWON041                ");
            parameter.AppendSql("     , ONE_INWON042 = :ONE_INWON042                ");
            parameter.AppendSql("     , ONE_INWON051 = :ONE_INWON051                ");
            parameter.AppendSql("     , ONE_INWON052 = :ONE_INWON052                ");
            parameter.AppendSql("     , ONE_INWON061 = :ONE_INWON061                ");
            parameter.AppendSql("     , ONE_INWON062 = :ONE_INWON062                ");
            parameter.AppendSql("     , ONE_INWON071 = :ONE_INWON071                ");
            parameter.AppendSql("     , ONE_INWON072 = :ONE_INWON072                ");
            parameter.AppendSql("     , ONE_INWON081 = :ONE_INWON081                ");
            parameter.AppendSql("     , ONE_INWON082 = :ONE_INWON082                ");
            parameter.AppendSql("     , ONE_INWON091 = :ONE_INWON091                ");
            parameter.AppendSql("     , ONE_INWON092 = :ONE_INWON092                ");
            parameter.AppendSql("     , ONE_INWON101 = :ONE_INWON101                ");
            parameter.AppendSql("     , ONE_INWON102 = :ONE_INWON102                ");
            parameter.AppendSql("     , ONE_INWON111 = :ONE_INWON111                ");
            parameter.AppendSql("     , ONE_INWON112 = :ONE_INWON112                ");
            parameter.AppendSql("     , ONE_INWON013 = :ONE_INWON013                ");
            parameter.AppendSql("     , GBERRCHK     = :GBERRCHK                    ");
            parameter.AppendSql("     , TWO_INWON01  = :TWO_INWON01                 ");
            parameter.AppendSql("     , TWO_INWON02  = :TWO_INWON02                 ");
            parameter.AppendSql("     , TWO_INWON03  = :TWO_INWON03                 ");
            parameter.AppendSql("     , TWO_INWON04  = :TWO_INWON04                 ");
            parameter.AppendSql("     , TWO_INWON05  = :TWO_INWON05                 ");
            parameter.AppendSql("     , TWO_INWON06  = :TWO_INWON06                 ");
            parameter.AppendSql("     , TWO_INWON07  = :TWO_INWON07                 ");
            parameter.AppendSql("     , TWO_INWON08  = :TWO_INWON08                 ");
            parameter.AppendSql("     , TWO_INWON09  = :TWO_INWON09                 ");
            parameter.AppendSql("     , TWO_INWON10  = :TWO_INWON10                 ");
            parameter.AppendSql("     , TWO_INWON11  = :TWO_INWON11                 ");
            parameter.AppendSql("     , TWO_INWON12  = :TWO_INWON12                 ");
            parameter.AppendSql("     , TWO_INWON16  = :TWO_INWON16                 ");
            parameter.AppendSql(" WHERE ROWID        = :RID                         ");

            parameter.Add("ONE_INWON011", item.ONE_INWON011);
            parameter.Add("ONE_INWON012", item.ONE_INWON012);
            parameter.Add("ONE_INWON021", item.ONE_INWON021);
            parameter.Add("ONE_INWON022", item.ONE_INWON022);
            parameter.Add("ONE_INWON031", item.ONE_INWON031);
            parameter.Add("ONE_INWON032", item.ONE_INWON032);
            parameter.Add("ONE_INWON041", item.ONE_INWON041);
            parameter.Add("ONE_INWON042", item.ONE_INWON042);
            parameter.Add("ONE_INWON051", item.ONE_INWON051);
            parameter.Add("ONE_INWON052", item.ONE_INWON052);
            parameter.Add("ONE_INWON061", item.ONE_INWON061);
            parameter.Add("ONE_INWON062", item.ONE_INWON062);
            parameter.Add("ONE_INWON071", item.ONE_INWON071);
            parameter.Add("ONE_INWON072", item.ONE_INWON072);
            parameter.Add("ONE_INWON081", item.ONE_INWON081);
            parameter.Add("ONE_INWON082", item.ONE_INWON082);
            parameter.Add("ONE_INWON091", item.ONE_INWON091);
            parameter.Add("ONE_INWON092", item.ONE_INWON092);
            parameter.Add("ONE_INWON101", item.ONE_INWON101);
            parameter.Add("ONE_INWON102", item.ONE_INWON102);
            parameter.Add("ONE_INWON111", item.ONE_INWON111);
            parameter.Add("ONE_INWON112", item.ONE_INWON112);
            parameter.Add("ONE_INWON013", item.ONE_INWON013);
            parameter.Add("GBERRCHK    ", item.GBERRCHK, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("TWO_INWON01 ", item.TWO_INWON01 );
            parameter.Add("TWO_INWON02 ", item.TWO_INWON02 );
            parameter.Add("TWO_INWON03 ", item.TWO_INWON03 );
            parameter.Add("TWO_INWON04 ", item.TWO_INWON04 );
            parameter.Add("TWO_INWON05 ", item.TWO_INWON05 );
            parameter.Add("TWO_INWON06 ", item.TWO_INWON06 );
            parameter.Add("TWO_INWON07 ", item.TWO_INWON07 );
            parameter.Add("TWO_INWON08 ", item.TWO_INWON08 );
            parameter.Add("TWO_INWON09 ", item.TWO_INWON09 );
            parameter.Add("TWO_INWON10 ", item.TWO_INWON10 );
            parameter.Add("TWO_INWON11 ", item.TWO_INWON11 );
            parameter.Add("TWO_INWON12 ", item.TWO_INWON12 );
            parameter.Add("TWO_INWON16 ", item.TWO_INWON16);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateJepNoFileNameJepDatebyMirNo(string strJepNo, string strFileName, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_BOHUM SET               ");
            parameter.AppendSql("       JEPNO    = :JEPNO                           ");
            parameter.AppendSql("     , FILENAME = :FILENAME                        ");
            parameter.AppendSql("     , JEPDATE  = TRUNC(SYSDATE)                   ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                           ");

            parameter.Add("JEPNO", strJepNo);
            parameter.Add("FILENAME", strFileName);
            parameter.Add("MIRNO", argMirno);

            return ExecuteNonQuery(parameter);
        }

        public int InsertAll(long nMirno, string argYear, string strJohap, string strGbJohap, string argFDate, string argTDate, long nTotCNT, long nCnt1, long nCnt2, string strChasu, string argSabun, string strMirGbn, string strLife_Gbn, string strkiho)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_MIR_BOHUM                                                          ");
            parameter.AppendSql("       (MIRNO, YEAR, GUBUN, JOHAP, GBJOHAP, FRDATE, TODATE                                     ");
            parameter.AppendSql("     , KIHO,JEPQTY,ONE_QTY,TWO_QTY,BUILDCNT,BUILDDATE,CHASU,BUILDSABUN,MIRGBN,LIFE_GBN)        ");
            parameter.AppendSql("VALUES                                                                                         ");
            parameter.AppendSql("       (:MIRNO, :YEAR, :GUBUN, :JOHAP, :GBJOHAP, TO_DATE(:FRDATE, 'YYYY-MM-DD')                ");
            parameter.AppendSql("     , TO_DATE(:TODATE, 'YYYY-MM-DD'), :KIHO, :JEPQTY, :ONE_QTY, :TWO_QTY                      ");
            parameter.AppendSql("     , :BUILDCNT, SYSDATE, :CHASU, :BUILDSABUN, :MIRGBN, :LIFE_GBN)                            ");

            parameter.Add("MIRNO", nMirno);
            parameter.Add("YEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            switch (strJohap)
            {
                case "사업장":
                    parameter.Add("GUBUN", "1", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    parameter.Add("JOHAP", "K", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    break;
                case "공무원":
                    parameter.Add("GUBUN", "1", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    parameter.Add("JOHAP", "G", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    break;
                case "성인병":
                    parameter.Add("GUBUN", "1", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    parameter.Add("JOHAP", "J", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    break;
                case "통합":
                    parameter.Add("GUBUN", "1", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    parameter.Add("JOHAP", "T", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    break;
                default:
                    break;
            }

            parameter.Add("GBJOHAP", strGbJohap, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);
            parameter.Add("KIHO", strkiho, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPQTY", nTotCNT);
            parameter.Add("ONE_QTY", nCnt1);
            parameter.Add("TWO_QTY", nCnt2);
            parameter.Add("BUILDCNT", nTotCNT);
            parameter.Add("CHASU", strChasu, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BUILDSABUN", argSabun);
            parameter.Add("MIRGBN", strMirGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LIFE_GBN", strLife_Gbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateTamtOntTamtTowTamtbyMirNo(long nTotAmt, long nOneAmt, long nTwoAmt, string strMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_BOHUM SET       ");
            parameter.AppendSql("       TAMT     = :TAMT                    ");
            parameter.AppendSql("     , ONE_TAMT = :ONE_TAMT                ");
            parameter.AppendSql("     , TWO_TAMT = :TWO_TAMT                ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                   ");

            parameter.Add("TAMT", nTotAmt);
            parameter.Add("ONE_TAMT", nOneAmt);
            parameter.Add("TWO_TAMT", nTwoAmt);
            parameter.Add("MIRNO", strMirNo);

            return ExecuteNonQuery(parameter);
        }

        public HIC_MIR_BOHUM GetTamtbyMirNo(string strMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TAMT                                                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_BOHUM                                                               ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                                                          ");

            parameter.Add("MIRNO", strMirNo);

            return ExecuteReaderSingle<HIC_MIR_BOHUM>(parameter);
        }

        public List<HIC_MIR_BOHUM> GetItembyMirNoLtdCodeYear(long argMirNo, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIRNO,YEAR,GUBUN,JOHAP,GBJOHAP,KIHO,JEPNO,JEPQTY,TAMT,JEPDATE,ONE_QTY,ONE_TAMT  ");
            parameter.AppendSql("     , ONE_INWON011,ONE_INWON012,ONE_INWON021,ONE_INWON022,ONE_INWON031,ONE_INWON032   ");
            parameter.AppendSql("     , ONE_INWON041,ONE_INWON042,ONE_INWON051,ONE_INWON052,ONE_INWON061,ONE_INWON062   ");
            parameter.AppendSql("     , ONE_INWON071,ONE_INWON072,ONE_INWON081,ONE_INWON082,ONE_INWON091,ONE_INWON092   ");
            parameter.AppendSql("     , ONE_INWON101,ONE_INWON102,ONE_INWON111,ONE_INWON112,ONE_INWON013                ");
            parameter.AppendSql("     , TWO_QTY,TWO_TAMT,TWO_INWON01,TWO_INWON02,TWO_INWON03,TWO_INWON04,TWO_INWON05    ");
            parameter.AppendSql("     , TWO_INWON06,TWO_INWON07,TWO_INWON08,TWO_INWON09,TWO_INWON10,TWO_INWON11         ");
            parameter.AppendSql("     , TWO_INWON12,TWO_INWON16, IPGUMDATE,IPGUMAMT,BUILDDATE                           ");
            parameter.AppendSql("     , BuildSabun , BuildCnt, GbErrChk, FileName, CHASU, MIRGBN                        ");
            parameter.AppendSql("     , BUILDDATE, FileName                                                             ");
            parameter.AppendSql("     , TO_CHAR(FRDATE,'YYYY.MM.DD') FRDATE,TO_CHAR(TODATE,'YYYY.MM.DD') TODATE         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_BOHUM                                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                                           ");
            parameter.AppendSql("   AND Johap NOT IN ('J')                                                              ");
            if (!argMirNo.IsNullOrEmpty() && argMirNo != 0)
            {
                parameter.AppendSql("   AND MIRNO = :MIRNO                                                              ");
            }
            parameter.AppendSql("   AND YEAR = :YEAR                                                                    ");

            if (!argMirNo.IsNullOrEmpty() && argMirNo != 0)
            {
                parameter.Add("MIRNO", argMirNo);
            }
            parameter.Add("YEAR", argYear);

            return ExecuteReader<HIC_MIR_BOHUM>(parameter);
        }

        public string GetJepNobyMirNo(long nMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JEPNO                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_BOHUM       ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                  ");

            parameter.Add("MIRNO", nMirNo);

            return ExecuteScalar<string>(parameter);
        }
    }
}
