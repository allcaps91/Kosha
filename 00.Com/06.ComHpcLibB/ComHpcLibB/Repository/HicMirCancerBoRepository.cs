namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicMirCancerBoRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirCancerBoRepository()
        {
        }

        public HIC_MIR_CANCER_BO GetMirCancerBoByWRTNO(long ArgWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MIRNO,YEAR,GUBUN,JOHAP,KIHO,JEPNO,JEPQTY,TAMT,JEPDATE                                   ");
            parameter.AppendSql("     , INWON01,INWON02,INWON03,INWON04,INWON05,INWON06,INWON07,INWON08                         ");
            parameter.AppendSql("     , INWON09,INWON10,INWON11,INWON12,INWON13,INWON14,INWON15,INWON16,INWON17,INWON18         ");
            parameter.AppendSql("     , INWON19,INWON20,INWON21,INWON22,INWON23,INWON24,INWON25,INWON26,INWON27,INWON28         ");
            parameter.AppendSql("     , SangDam1,SangDam2,SangDam3,SangDam4,SangDam5,SangDam6                                   ");
            parameter.AppendSql("     , IPGUMDATE,IPGUMAMT,BUILDCNT,BUILDSABUN,GBERRCHK, GbJohap,MirGbn                         ");
            parameter.AppendSql("     , TO_CHAR(FrDate,'YYYY.MM.DD') FrDate                                                     ");
            parameter.AppendSql("     , TO_CHAR(ToDate,'YYYY.MM.DD') ToDate                                                     ");
            parameter.AppendSql("     , TO_CHAR(BuildDate,'YYYY-MM-DD') BuildDate,FileName,Life_Gbn,ROWID                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_MIR_CANCER_BO                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            parameter.AppendSql("   AND WRTNO  = :WRTNO                                                                         ");

            parameter.Add("WRTNO", ArgWRTNO);

            return ExecuteReaderSingle<HIC_MIR_CANCER_BO>(parameter);
        }

        public int GetTamtbyMirNo(long mIRNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TAMT FROM ADMIN.HIC_MIR_CANCER_BO                                             ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                                                      ");

            parameter.Add("WRTNO", mIRNO);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateNhicNobyMirNo(string strnHicNo, long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_CANCER_BO SET   ");
            parameter.AppendSql("       NHICNO = :NHICNO                    ");
            parameter.AppendSql(" WHERE MIRNO  = :MIRNO                     ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyMirNo(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                                                      ");
            parameter.AppendSql("  FROM ADMIN.HIC_MIR_CANCER_BO                                                       ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                                                      ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateMirNoOldMirNobyMirNo(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_CANCER_BO SET   ");
            parameter.AppendSql("       MIRNO    = 0                        ");
            parameter.AppendSql("     , OLDMIRNO = :MIRNO                   ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                   ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public HIC_MIR_CANCER_BO GetItembyMirno(long argMirno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIRNO,YEAR,GUBUN,JOHAP,KIHO,JEPNO,JEPQTY,TAMT,JEPDATE                               ");
            parameter.AppendSql("     , INWON01,INWON02,INWON03,INWON04,INWON05,INWON06,INWON07,INWON08                     ");
            parameter.AppendSql("     , INWON09,INWON10,INWON11,INWON12,INWON13,INWON14,INWON15,INWON16,INWON17,INWON18     ");
            parameter.AppendSql("     , INWON19,INWON20,INWON21,INWON22,INWON23,INWON24,INWON25,INWON26,INWON27,INWON28     ");
            parameter.AppendSql("     , SangDam1,SangDam2,SangDam3,SangDam4,SangDam5,SangDam6                               ");
            parameter.AppendSql("     , IPGUMDATE,IPGUMAMT,BUILDCNT,BUILDSABUN,GBERRCHK, GbJohap,MirGbn                     ");
            parameter.AppendSql("     , TO_CHAR(FrDate,'YYYY.MM.DD') FrDate                                                 ");
            parameter.AppendSql("     , TO_CHAR(ToDate,'YYYY.MM.DD') ToDate                                                 ");
            parameter.AppendSql("     , TO_CHAR(BuildDate,'YYYY-MM-DD') BuildDate,FileName,Life_Gbn,ROWID                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_MIR_CANCER_BO                                                       ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                                                      ");

            parameter.Add("WRTNO", argMirno);

            return ExecuteReaderSingle<HIC_MIR_CANCER_BO>(parameter);
        }

        public int UpdateJepQtyBuildCntGbErrChkbyMirNo(long nCnt, string strErrChk, long fnMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_CANCER_BO SET   ");
            parameter.AppendSql("     , JEPQTY    = :JEPQTY                 ");
            parameter.AppendSql("     , BUILDCNT  = :BUILDCNT               ");
            parameter.AppendSql("     , GBERRCHK  = :GBERRCHK               ");
            parameter.AppendSql(" WHERE MIRNO     = :MIRNO                  ");

            parameter.Add("JEPQTY", nCnt);
            parameter.Add("BUILDCNT", nCnt);
            parameter.Add("GBERRCHK ", strErrChk);
            parameter.Add("MIRNO", fnMirNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyMirNo(HIC_MIR_CANCER_BO item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_CANCER_BO SET   ");
            parameter.AppendSql("       GBERRCHK  = :GBERRCHK               ");
            parameter.AppendSql("     , INWON01   = :INWON01                ");
            parameter.AppendSql("     , INWON02   = :INWON02                ");
            parameter.AppendSql("     , INWON03   = :INWON03                ");
            parameter.AppendSql("     , INWON04   = :INWON04                ");
            parameter.AppendSql("     , INWON05   = :INWON05                ");
            parameter.AppendSql("     , INWON06   = :INWON06                ");
            parameter.AppendSql("     , INWON07   = :INWON07                ");
            parameter.AppendSql("     , INWON08   = :INWON08                ");
            parameter.AppendSql("     , INWON09   = :INWON09                ");
            parameter.AppendSql("     , INWON10   = :INWON10                ");
            parameter.AppendSql("     , INWON11   = :INWON11                ");
            parameter.AppendSql("     , INWON12   = :INWON12                ");
            parameter.AppendSql("     , INWON13   = :INWON13                ");
            parameter.AppendSql("     , INWON14   = :INWON14                ");
            parameter.AppendSql("     , INWON15   = :INWON15                ");
            parameter.AppendSql("     , INWON16   = :INWON16                ");
            parameter.AppendSql("     , INWON17   = :INWON17                ");
            parameter.AppendSql("     , INWON18   = :INWON18                ");
            parameter.AppendSql("     , INWON19   = :INWON19                ");
            parameter.AppendSql("     , INWON20   = :INWON20                ");
            parameter.AppendSql("     , INWON21   = :INWON21                ");
            parameter.AppendSql("     , INWON22   = :INWON22                ");
            parameter.AppendSql("     , INWON23   = :INWON23                ");
            parameter.AppendSql("     , INWON24   = :INWON24                ");
            parameter.AppendSql("     , INWON25   = :INWON25                ");
            parameter.AppendSql("     , INWON26   = :INWON26                ");
            parameter.AppendSql("     , INWON27   = :INWON27                ");
            parameter.AppendSql("     , INWON28   = :INWON28                ");
            parameter.AppendSql("     , JEPQTY    = :JEPQTY                 ");
            parameter.AppendSql("     , BUILDCNT  = :BUILDCNT               ");
            parameter.AppendSql("     , SAANGDAM1 = :SANGDAM1               ");
            parameter.AppendSql("     , SAANGDAM2 = :SANGDAM2               ");
            parameter.AppendSql("     , SAANGDAM3 = :SANGDAM3               ");
            parameter.AppendSql("     , SAANGDAM4 = :SANGDAM4               ");
            parameter.AppendSql("     , SAANGDAM5 = :SANGDAM5               ");
            parameter.AppendSql(" WHERE MIRNO     = :MIRNO                  ");

            parameter.Add("GBERRCHK", item.GBERRCHK);
            parameter.Add("INWON01", item.INWON01);
            parameter.Add("INWON02", item.INWON02);
            parameter.Add("INWON03", item.INWON03);
            parameter.Add("INWON04", item.INWON04);
            parameter.Add("INWON05", item.INWON05);
            parameter.Add("INWON06", item.INWON06);
            parameter.Add("INWON07", item.INWON07);
            parameter.Add("INWON08", item.INWON08);
            parameter.Add("INWON09", item.INWON09);
            parameter.Add("INWON10", item.INWON10);
            parameter.Add("INWON11", item.INWON11);
            parameter.Add("INWON12", item.INWON12);
            parameter.Add("INWON13", item.INWON13);
            parameter.Add("INWON14", item.INWON14);
            parameter.Add("INWON15", item.INWON15);
            parameter.Add("INWON16", item.INWON16);
            parameter.Add("INWON17", item.INWON17);
            parameter.Add("INWON18", item.INWON18);
            parameter.Add("INWON19", item.INWON19);
            parameter.Add("INWON20", item.INWON20);
            parameter.Add("INWON21", item.INWON21);
            parameter.Add("INWON22", item.INWON22);
            parameter.Add("INWON23", item.INWON23);
            parameter.Add("INWON24", item.INWON24);
            parameter.Add("INWON25", item.INWON25);
            parameter.Add("INWON26", item.INWON26);
            parameter.Add("INWON27", item.INWON27);
            parameter.Add("INWON28", item.INWON28);
            parameter.Add("JEPQTY", item.JEPQTY);
            parameter.Add("BUILDCNT", item.BUILDCNT);
            parameter.Add("SANGDAM1", item.SANGDAM1);
            parameter.Add("SANGDAM2", item.SANGDAM2);
            parameter.Add("SANGDAM3", item.SANGDAM3);
            parameter.Add("SANGDAM4", item.SANGDAM4);
            parameter.Add("SANGDAM5", item.SANGDAM5);
            parameter.Add("MIRNO", item.MIRNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateJepNoFileNameJepDatebyMirNo(string strJepNo, string strFileName, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_CANCER_BO SET       ");
            parameter.AppendSql("       JEPNO    = :JEPNO                       ");
            parameter.AppendSql("       FILENAME = :FILENAME                    ");
            parameter.AppendSql("       JEPDATE  = TRUNC(SYSDATE)               ");
            parameter.AppendSql(" WHERE MIRNO    = :MIRNO                       ");

            parameter.Add("JEPNO", strJepNo);
            parameter.Add("FILENAME", strFileName);
            parameter.Add("MIRNO", argMirno);

            return ExecuteNonQuery(parameter);
        }

        public int InsertAll(long nMirNo, string strYear, string strjik, string strFDate, string strTDate, string strkiho, int fnTotCNT, string idNumber, string strMirGbn, string strLife_Gbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_MIR_CANCER_BO                                                          ");
            parameter.AppendSql("       (MIRNO,JEPQTY,YEAR,GUBUN,JOHAP,FRDATE,TODATE                                                ");
            parameter.AppendSql("     , KIHO,BUILDCNT,BUILDDATE,BUILDSABUN,MIRGBN,LIFE_GBN)                                         ");
            parameter.AppendSql("VALUES                                                                                             ");
            parameter.AppendSql("       (:MIRNO,:JEPQTY,:YEAR,:GUBUN,:JOHAP,TO_DATE(:FRDATE, 'YYYYY-MM-DD')                         ");
            parameter.AppendSql("     , TO_DATE(:TODATE, 'YYYYY-MM-DD'), :KIHO,:BUILDCNT,SYSDATE,:BUILDSABUN,:MIRGBN,:LIFE_GBN)     ");

            parameter.Add("MIRNO", nMirNo);
            parameter.Add("JEPQTY", fnTotCNT);
            parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN", "4", Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JOHAP", strjik, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("KIHO", strkiho, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BUILDCNT", fnTotCNT);
            parameter.Add("BUILDSABUN", idNumber);
            parameter.Add("MIRGBN", strMirGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LIFE_GBN", strLife_Gbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateTamtbyMirNo(long nTotAmt, string strMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_CANCER_BO SET       ");
            parameter.AppendSql("       TOTAMT  = :TOTAMT                       ");
            parameter.AppendSql(" WHERE MIRNO   = :MIRNO                        ");

            parameter.Add("TOTAMT", nTotAmt);
            parameter.Add("MIRNO", strMirNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateAll(HIC_MIR_CANCER_BO item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_CANCER_BO SET       ");
            parameter.AppendSql("       INWON01 = :INWON01                      ");
            parameter.AppendSql("     , INWON02 = :INWON02                      ");
            parameter.AppendSql("     , INWON03 = :INWON03                      ");
            parameter.AppendSql("     , INWON04 = :INWON04                      ");
            parameter.AppendSql("     , INWON05 = :INWON05                      ");
            parameter.AppendSql("     , INWON06 = :INWON06                      ");
            parameter.AppendSql("     , INWON07 = :INWON07                      ");
            parameter.AppendSql("     , INWON11 = :INWON11                      ");
            parameter.AppendSql("     , INWON12 = :INWON12                      ");
            parameter.AppendSql("     , INWON14 = :INWON14                      ");
            parameter.AppendSql("     , INWON15 = :INWON15                      ");
            parameter.AppendSql("     , INWON16 = :INWON16                      ");
            parameter.AppendSql("     , INWON17 = :INWON17                      ");
            parameter.AppendSql("     , INWON18 = :INWON18                      ");
            parameter.AppendSql("     , INWON19 = :INWON19                      ");
            parameter.AppendSql("     , INWON20 = :INWON20                      ");
            parameter.AppendSql("     , INWON21 = :INWON21                      ");
            parameter.AppendSql("     , INWON22 = :INWON22                      ");
            parameter.AppendSql("     , INWON23 = :INWON23                      ");
            parameter.AppendSql("     , INWON26 = :INWON26                      ");
            parameter.AppendSql(" WHERE MIRNO   = :MIRNO                        ");

            parameter.Add("INWON01", item.INWON01);
            parameter.Add("INWON02", item.INWON02);
            parameter.Add("INWON03", item.INWON03);
            parameter.Add("INWON04", item.INWON04);
            parameter.Add("INWON05", item.INWON05);
            parameter.Add("INWON06", item.INWON06);
            parameter.Add("INWON07", item.INWON07);
            parameter.Add("INWON11", item.INWON11);
            parameter.Add("INWON12", item.INWON12);
            parameter.Add("INWON14", item.INWON14);
            parameter.Add("INWON15", item.INWON15);
            parameter.Add("INWON16", item.INWON16);
            parameter.Add("INWON17", item.INWON17);
            parameter.Add("INWON18", item.INWON18);
            parameter.Add("INWON19", item.INWON19);
            parameter.Add("INWON20", item.INWON20);
            parameter.Add("INWON21", item.INWON21);
            parameter.Add("INWON22", item.INWON22);
            parameter.Add("INWON23", item.INWON23);
            parameter.Add("INWON26", item.INWON26);
            parameter.Add("MIRNO", item.MIRNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
