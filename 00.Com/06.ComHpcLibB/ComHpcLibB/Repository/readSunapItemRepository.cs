
namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    public class ReadSunapItemRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadSunapItemRepository() 
        {
        }

        public List<READ_SUNAP_ITEM> GetListByGWrtno(long nGWRTNO, string argIdx = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT X.JEPBUN, X.GJNAME, X.GRPCODE, X.GRPNAME, X.AMT, X.GBSELF                           ");
            parameter.AppendSql("  FROM (                                                                                   ");
            parameter.AppendSql("         SELECT A.JEPBUN, KOSMOS_PMPA.FC_HIC_GJJONG_NAME(A.GJJONG, A.UCODES) AS GJNAME     ");
            parameter.AppendSql("              , B.CODE AS GRPCODE, KOSMOS_PMPA.FC_HIC_GROUPCODE_NAME(B.CODE) AS GRPNAME    ");
            parameter.AppendSql("              , B.AMT, B.GBSELF                                                            ");
            parameter.AppendSql("           FROM KOSMOS_PMPA.HIC_SUNAPDTL B,                                                ");
            parameter.AppendSql("                KOSMOS_PMPA.HIC_JEPSU A                                                    ");
            parameter.AppendSql("          WHERE 1 = 1                                                                      ");
            parameter.AppendSql("            AND B.GWRTNO = :GWRTNO                                                         ");
            parameter.AppendSql("            AND B.WRTNO  = A.WRTNO                                                         ");

            parameter.AppendSql("          UNION ALL                                                                        ");

            parameter.AppendSql("         SELECT '4' AS JEPBUN, KOSMOS_PMPA.FC_HEA_GJJONG_NAME(A.GJJONG) AS GJNAME          ");
            parameter.AppendSql("              , B.CODE AS GRPCODE, KOSMOS_PMPA.FC_HEA_GROUPCODE_NAME(B.CODE) AS GRPNAME    ");
            parameter.AppendSql("              , B.AMT, B.GBSELF                                                            ");
            parameter.AppendSql("           FROM KOSMOS_PMPA.HEA_SUNAPDTL B,                                                ");
            parameter.AppendSql("                KOSMOS_PMPA.HEA_JEPSU A                                                    ");
            parameter.AppendSql("          WHERE 1 = 1                                                                      ");
            parameter.AppendSql("            AND B.GWRTNO = :GWRTNO                                                         ");
            parameter.AppendSql("            AND B.WRTNO  = A.WRTNO                                                         ");
            parameter.AppendSql("  ) X                                                                                      ");

            if (!argIdx.IsNullOrEmpty())
            {
                parameter.AppendSql("  WHERE X.JEPBUN = :JEPBUN                                                              ");
            }

            parameter.AppendSql(" ORDER BY X.JEPBUN, X.GRPCODE                                                              ");

            parameter.Add("GWRTNO", nGWRTNO);

            if (!argIdx.IsNullOrEmpty())
            {
                parameter.Add("JEPBUN", argIdx);
            }

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }

        public List<READ_SUNAP_ITEM> GetHeaGrpCodeListByNameLike(string argWard, string argName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE AS GRPCODE, NAME AS GRPNAME, AMT           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE                       ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");
            if (argWard == "전체")
            {
                if (!argName.IsNullOrEmpty())
                {
                    parameter.AppendSql(" AND NAME LIKE :NAME                               ");
                }
            }
            else
            {
                if (argWard == "추가")
                {
                    parameter.AppendSql("   AND (GBSELECT = 'Y' OR SUBSTR(CODE,1,1) = 'Z')  ");
                    if (!argName.IsNullOrEmpty())
                    {
                        parameter.AppendSql(" AND NAME LIKE :NAME                           ");
                    }
                }
                else if (argWard == "조직+CLO")
                {
                    parameter.AppendSql("   AND (NAME LIKE '%조직%' OR NAME LIKE '%CLO%'  ");
                }
                else if (argWard == "BMD")
                {
                    parameter.AppendSql("   AND (NAME LIKE '%BMD%' OR Name LIKE '%골밀도%'  ");
                }
                else
                {
                    parameter.AppendSql("   AND (NAME LIKE :WARD                           ");
                }

                if (argName.IsNullOrEmpty() && argWard != "추가")
                {
                    parameter.AppendSql(" )                          ");
                }
                else
                {
                    if ((argWard != "추가"))
                    {
                        parameter.AppendSql(" OR NAME LIKE :NAME )                          ");
                    }

                }
            }

            parameter.AppendSql(" ORDER BY NAME                           ");


            if (argWard == "전체")
            {
                if (!argName.IsNullOrEmpty())
                {
                    parameter.AddLikeStatement("NAME", argName);
                }
            }
            else
            {
                if (argWard == "추가" && !argName.IsNullOrEmpty())
                {
                    parameter.AddLikeStatement("NAME", argName);
                }
                else if (argWard == "조직+CLO")
                {

                }
                else if (argWard == "BMD")
                {

                }
                else
                {
                    parameter.AddLikeStatement("WARD", argWard);
                }

                if (!argName.IsNullOrEmpty() && argWard != "추가")
                {
                    parameter.AddLikeStatement("NAME", argName);
                }
            }

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }

        public List<READ_SUNAP_ITEM> GetHicSunapHisInfoByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE AS GRPCODE, AMT, GBSELF, ROWID AS RID          ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HIC_GROUPCODE_NAME(CODE) AS GRPNAME  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_HIS                        ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }

        public READ_SUNAP_ITEM GetHeaItemByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT  CODE AS GRPCODE, NAME AS GRPNAME, GBSELECT, ROWID AS RID, AMT              ");
            parameter.AppendSql("     , OLDAMT, TO_CHAR(SUDATE, 'YYYY-MM-DD') SUDATE, ENDOCODE, BURATE AS GBSELF    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE                                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND CODE = :CODE                                                                ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<READ_SUNAP_ITEM>(parameter);
        }

        public READ_SUNAP_ITEM GetHicItemByCode(string argCode, string argGbn = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE AS GRPCODE, NAME AS GRPNAME, UCODE, GBSELECT, GBSUGA, GBAM     ");
            parameter.AppendSql("      ,REMARK, LPAD(GBSELF, 2, '0') GBSELF, ROWID AS RID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPCODE                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE))                       ");
            parameter.AppendSql("   AND (SDATE IS NULL OR SDATE <= TRUNC(SYSDATE))                          ");
            if (argGbn.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE = :CODE                                                        ");
            }
            else
            {
                parameter.AppendSql("   AND UCODE = :UCODE                                                        ");
            }

            parameter.AppendSql(" ORDER BY HANG,CODE                                                        ");

            if (argGbn.IsNullOrEmpty())
            {
                parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            else
            {
                parameter.Add("UCODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<READ_SUNAP_ITEM>(parameter);
        }

        public List<READ_SUNAP_ITEM> GetListHeaGrpCodeByJong(string argJong, string argDate, long nLtdCode, string argSex)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE AS GRPCODE, NAME AS GRPNAME, GBSELECT, ROWID AS RID, AMT               ");
            parameter.AppendSql("     , OLDAMT, TO_CHAR(SUDATE, 'YYYY-MM-DD') SUDATE, ENDOCODE, BURATE AS GBSELF    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE                                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE > TO_DATE(:DELDATE, 'YYYY-MM-DD'))              ");
            parameter.AppendSql("   AND (SDATE IS NULL OR SDATE <= TO_DATE(:SDATE, 'YYYY-MM-DD'))                   ");

            if (string.Compare(argJong, "21") >= 0 && string.Compare(argJong, "29") <= 0)       //회사종검
            {
                parameter.AppendSql("   AND JONG >= '21'                                    ");
                parameter.AppendSql("   AND JONG <= '29'                                    ");
                parameter.AppendSql("   AND GBSEX IN ('*', :SEX)                                            ");
            }
            else if (argJong == "43" || argJong == "44" || argJong == "69")        //효도검진 / 청소년검진 / 회사추가종검
            {
                parameter.AppendSql("   AND JONG =:JONG                                                     ");
                parameter.AppendSql("   AND GBSEX IN ('*', :SEX)                                            ");
            }
            else
            {
                parameter.AppendSql("   AND JONG=:JONG                                                      ");
                parameter.AppendSql("   AND GBSEX =:SEX                                                     ");
            }

            if (argJong != "11" && argJong != "12" && nLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE =:LTDCODE                                                ");
            }

            parameter.AppendSql(" ORDER BY CODE                                                        ");

            parameter.Add("DELDATE", argDate);
            parameter.Add("SDATE", argDate);

            if (string.Compare(argJong, "21") >= 0 && string.Compare(argJong, "29") <= 0)       //회사종검
            {
                parameter.Add("SEX", argSex, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            else if (argJong == "43" || argJong == "44" || argJong == "69")        //효도검진 / 청소년검진 / 회사추가종검
            {
                parameter.Add("JONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                parameter.Add("SEX", argSex, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            else
            {
                parameter.Add("JONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                parameter.Add("SEX", argSex, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (argJong != "11" && argJong != "12" && nLtdCode > 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }

        public List<READ_SUNAP_ITEM> GetListHicGrpCodeByJong(string argJong, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE AS GRPCODE, NAME AS GRPNAME, UCODE, GBSELECT, GBSUGA, GBAM     ");
            parameter.AppendSql("      ,REMARK ,GBSELF, ROWID AS RID, HANG     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPCODE                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE > TO_DATE(:DELDATE, 'YYYY-MM-DD'))      ");
            parameter.AppendSql("   AND (SDATE IS NULL OR SDATE <= TO_DATE(:SDATE, 'YYYY-MM-DD'))           ");
            if (argJong != "56")
            {
                parameter.AppendSql("   AND GBSELECT='N'                                                        ");
            }
            parameter.AppendSql("   AND JONG = :JONG                                                        ");
            parameter.AppendSql(" ORDER BY HANG,CODE                                                        ");

            parameter.Add("DELDATE", argDate);
            parameter.Add("SDATE", argDate);
            parameter.Add("JONG", argJong);

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }

        public List<READ_SUNAP_ITEM> GetHicSunapWorkInfoByWrtno(long argPano, string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.CODE AS GRPCODE, KOSMOS_PMPA.FC_HIC_GROUPCODE_NAME(a.CODE) AS GRPNAME ");
            parameter.AppendSql("     , FC_HIC_GROUPCODE_UCODE(a.CODE) AS UCODE, HANG                           ");
            parameter.AppendSql("     , a.UCODE, a.AMT, a.GBSELF, a.ROWID AS RID, b.GBSELECT                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_WORK a                                         ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_GROUPCODE b                                             ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql("   AND a.CODE   = b.CODE                                                       ");
            parameter.AppendSql("   AND a.PANO   = :PANO                                                        ");
            parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                      ");
            parameter.AppendSql(" ORDER BY a.CODE                                                               ");

            parameter.Add("PANO", argPano);
            parameter.Add("GJJONG", argJong);

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }

        public List<READ_SUNAP_ITEM> GetHeaSunapInfoByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT B.CODE AS GRPCODE, B.CODENAME AS GRPNAME, B.AMT, C.BURATE           ");
            parameter.AppendSql("     , B.GBSELF, B.ROWID AS RID, C.GBSELECT                                ");
            parameter.AppendSql("     , DECODE(B.GBHALIN, '1', 'Y', 'N') GBHALIN                            ");
            parameter.AppendSql("     , B.BONINAMT, B.LTDAMT                                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL B                                          ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HEA_GROUPCODE C                                         ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND B.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND B.CODE  = C.CODE(+)                                                 ");
            parameter.AppendSql(" ORDER BY B.CODE                                                           ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }

        public List<READ_SUNAP_ITEM> GetHicSunapInfoByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.FC_HIC_GJJONG_NAME(A.GJJONG, A.UCODES) AS GJNAME, B.CODE AS GRPCODE  ");
            //parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_GROUPCODE_NAME(B.CODE) AS GRPNAME     ");
            //parameter.AppendSql("     , FC_HIC_GROUPCODE_UCODE(B.CODE) AS UCODE, C.GBSELECT                         ");
            parameter.AppendSql("     , C.NAME AS GRPNAME, C.UCODE, C.GBSELECT , C.HANG                             ");
            parameter.AppendSql("     , B.AMT, LPAD(B.GBSELF, 2,'0') GBSELF, B.ROWID AS RID                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A                                                     ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_SUNAPDTL B                                                  ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_GROUPCODE C                                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql("   AND A.WRTNO  = B.WRTNO                                                          ");
            parameter.AppendSql("   AND B.CODE  = C.CODE                                                            ");
            parameter.AppendSql(" ORDER BY C.HANG, B.CODE                                                           ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<READ_SUNAP_ITEM>(parameter);
        }
    }
}
