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
    public class HicGroupcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupcodeRepository()
        {
        }

        public IList<HIC_GROUPCODE> FindCodeIn(string argSql)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Code,Name,YName ");
            parameter.AppendSql("  FROM HIC_GROUPCODE ");
            parameter.AppendSql("  WHERE 1 = 1 ");
            parameter.AppendSql("   AND Code IN (" + argSql + ") ");
            parameter.AppendSql("   AND SWLICENSE=:SWLICENSE ");
            parameter.AppendSql(" ORDER BY Code ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public IList<HIC_GROUPCODE> GetListByAll(string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT CODE,NAME,HANG,JONG,GBSELECT,GBSUGA,SDATE, GBAM, GBSELF,");
            parameter.AppendSql("        EXAMNAME, GBPRINT, UCODE, YNAME,DELDATE, REMARK,GBDENT,REEXAM,GBSUNAP,");
            parameter.AppendSql("        GBNOTADDPAN,GBSANGDAM,GBGUBUN1, ENTDATE, ROWID AS RID ");
            parameter.AppendSql("   FROM HIC_GROUPCODE ");
            parameter.AppendSql("  WHERE 1 = 1 ");
            if (!argJong.Equals("**"))
            {
                parameter.AppendSql("    AND JONG =:JONG ");
            }
            parameter.AppendSql("    AND DELDATE IS NULL ");
            parameter.AppendSql("    AND SWLICENSE=:SWLICENSE ");
            parameter.AppendSql("  ORDER BY Code ");

            parameter.Add("JONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public List<HIC_GROUPCODE> GetHalinListByItems(string argSDate, string argJong, List<string> lstCode, string argTECGubun, int[] fnAm, string argTemp, string arghCan4)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT CODE,NAME,GBSELECT,GBSUGA,GBAM ");
            parameter.AppendSql("   FROM HIC_GROUPCODE ");
            parameter.AppendSql("  WHERE 1 = 1                                                      ");
            parameter.AppendSql("    AND (DELDATE IS NULL OR DELDATE > TO_DATE(:SDATE, 'YYYY-MM-DD')) ");
            parameter.AppendSql("    AND (SDATE IS NULL OR SDATE <= TO_DATE(:SDATE, 'YYYY-MM-DD'))    ");
            if (argJong == "11")
            {
                parameter.AppendSql("    AND GBSELECT = 'N'                         ");    
            }
            
            parameter.AppendSql("   AND  JONG =:JONG                            ");

            if (argTECGubun == "B")
            {
                parameter.AppendSql("    AND TRIM(CODE) NOT IN ( SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "HIC_BCODE ");
                parameter.AppendSql("                             WHERE Gubun ='HIC_의료급여생애기본검사제외' )         ");
            }

            if (argJong == "11")
            {
                if (!lstCode.IsNullOrEmpty() && lstCode.Count > 0)
                {
                    parameter.AppendSql(" OR CODE IN (:CODE)       ");
                }
            }

            if (argTemp == "OK")
            {
                parameter.AppendSql("    AND GBAM IS NOT NULL ");
                parameter.AppendSql("    AND (CODE = '" + argJong.Replace("'", "") + "01' ");
                for (int i = 0; i < fnAm.Length; i++)
                {
                    if (fnAm[i] > 0 && i != 4 && i != 3)
                    {
                        parameter.AppendSql(" OR SUBSTR(GBAM, " + fnAm[i].To<string>() + ", 1) = '1' ");
                    }
                }

                if (fnAm[4] > 0)
                {
                    if (arghCan4.Contains("부담") || arghCan4.Contains("대상"))
                    {
                        //TODO : 하드코딩 없앨것
                        parameter.AppendSql(" OR CODE = '3115' OR CODE = '3512' ");
                    }
                    else
                    {
                        //TODO : 하드코딩 없앨것
                        parameter.AppendSql(" OR ( CODE NOT IN ('3115','3512') AND SUBSTR(GBAM, " + fnAm[4].To<string>() + ", 1) = '1') ");
                    }
                }

                if (fnAm[3] > 0)
                {
                    parameter.AppendSql(" OR CODE = '3116' OR CODE = '3511' ");
                }

                parameter.AppendSql(" )                     ");
                parameter.AppendSql(" AND SUBSTR(CODE, 1, 1) NOT IN ('9')                       ");
            }

            if (argJong == "11")
            {
                //TODO : 하드코딩 없앨것
                parameter.AppendSql("  AND CODE NOT IN ('3101','3501','1155','1152') ");                
            }
            else
            {
                parameter.AppendSql("  AND GBSELECT = 'N'                           ");
                //TODO : 하드코딩 없앨것
                parameter.AppendSql("  AND CODE NOT IN ('3101','3501','1155','1152') ");
            }

            parameter.AppendSql("  ORDER BY HANG,CODE                               ");

            parameter.Add("SDATE", argSDate);
            parameter.Add("JONG", argJong);

            if (!lstCode.IsNullOrEmpty() && lstCode.Count > 0)
            {
                parameter.AddInStatement("CODE", lstCode);
            }
            

            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        

        public int DeletebyRowId(string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_GROUPCODE       ");
            parameter.AppendSql(" WHERE ROWID       =:RID               ");

            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_GROUPCODE> GetItembyJong(string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, HANG, JONG, GBSELECT, NAME, UCODE, GBSUGA     ");
            parameter.AppendSql("  FROM ADMIN.HIC_GROUPCODE                           ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            if (!strJong.IsNullOrEmpty() && strJong != "**")
            {
                parameter.AppendSql("   AND JONG =:JONG                                     ");
            }

            if (!strJong.IsNullOrEmpty() && strJong != "**")
            {
                parameter.Add("JONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public string GetGbAmByCode(string argGroupCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBAM                                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_GROUPCODE                         ");
            parameter.AppendSql(" WHERE 1 = 1                                             ");
            parameter.AppendSql("   AND CODE =:CODE                                       ");

            parameter.Add("CODE", argGroupCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_GROUPCODE> GetItembyJepDateGjJong(string argGJepDate, string argGjJong, string argGubun, List<string> argUCodesSql, List<string> argCodesSql, string strGjJong_Gubun1, string strGjJong_Gubun2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, UCODE, GBSELECT, GBSUGA, GBSELF                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_GROUPCODE                                               ");
            parameter.AppendSql(" WHERE (DELDATE IS NULL OR DELDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD'))          ");
            parameter.AppendSql("   AND (SDATE IS NULL OR SDATE <= TO_DATE(:JEPDATE, 'YYYY-MM-DD'))             ");
            //2019-05-09
            //검진종류별 기본검사
            parameter.AppendSql("   AND ((GBSELECT = 'N'                                                        ");

            parameter.AppendSql("   AND JONG = :JONG)                                                           ");
       
            //2018-01-01
            if (argGubun == "A" || argGubun.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE NOT IN ('1155')                                                ");
            }
            if (argGubun == "B")
            {
                parameter.AppendSql("   AND CODE NOT IN ('1152')                                                ");
                parameter.AppendSql("   AND TRIM(CODE) NOT IN (SELECT TRIM(CODE)                                ");
                parameter.AppendSql("                            FROM ADMIN.HIC_BCODE                     ");
                parameter.AppendSql("                           WHERE GUBUN = 'HIC_의료급여생애기본검사제외' )  ");
            }
            if (!argUCodesSql.IsNullOrEmpty() && argUCodesSql.Count > 0)
            {
                parameter.AppendSql("    OR (HANG = 'C' AND UCODE IN (:UCODE))                                  ");
            }
            if (!argCodesSql.IsNullOrEmpty() && argCodesSql.Count > 0)
            {
                parameter.AppendSql("    OR (CODE IN (:CODE))                                                   ");
            }
            parameter.AppendSql("       )                                                                       ");
            if (strGjJong_Gubun1 == "1")
            {
                parameter.AppendSql("    OR CODE = '2302'                                                       ");
            }
            if (strGjJong_Gubun2 == "1")
            {
                parameter.AppendSql("    OR CODE = 'J224'                                                       ");
            }
            parameter.AppendSql(" ORDER BY HANG, CODE                                                           ");

            parameter.Add("JEPDATE", argGJepDate);
            parameter.Add("JONG", argGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            if (!argUCodesSql.IsNullOrEmpty() && argUCodesSql.Count > 0)
            {
                parameter.AddInStatement("UCODE", argUCodesSql);
            }
            if (!argCodesSql.IsNullOrEmpty() && argCodesSql.Count > 0)
            {
                parameter.AddInStatement("CODE", argCodesSql, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public List<HIC_GROUPCODE> GetItembySDate(string argGJepDate, List<string> argUCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, UCODE, GBSELECT, GBSUGA, GBSELF                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_GROUPCODE                                       ");
            parameter.AppendSql(" WHERE (DELDATE IS NULL OR DELDATE > TO_DATE(:JEPDATE, 'YYYY-MM-DD'))  ");
            parameter.AppendSql("   AND (SDATE IS NULL OR SDATE    <= TO_DATE(:JEPDATE, 'YYYY-MM-DD'))  ");
            if (!argUCodes.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE IN (:CODE)                                             ");
            }
            parameter.AppendSql(" ORDER BY HANG, CODE                                                   ");

            parameter.Add("JEPDATE", argGJepDate);
            if (!argUCodes.IsNullOrEmpty())
            {
                parameter.AddInStatement("CODE", argUCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public List<HIC_GROUPCODE> GetHangCode(List<string> strJong, string strGbSelectYN)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Hang,Code,Name FROM HIC_GroupCode               ");
            parameter.AppendSql(" WHERE Hang <> 'M'                                     ");
            if (strGbSelectYN == "Y")
            {
                parameter.AppendSql("   AND GbSelect = 'Y'                              ");
            }
            parameter.AppendSql("   AND (DelDate IS NULL OR DelDate >TRUNC(SYSDATE))    ");
            parameter.AppendSql("   AND (JONG IN (:JONG)                                ");
            parameter.AppendSql("    OR  Hang IN ('H','I') )                            "); //특수선택,공통선택
            parameter.AppendSql(" ORDER BY Name                                         ");

            parameter.AddInStatement("JONG", strJong);

            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public List<HIC_GROUPCODE> GetListByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, UCODE, GBSELECT, GBSUGA, GBAM, HANG ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_GROUPCODE             ");
            parameter.AppendSql("  WHERE 1 = 1                                          ");
            parameter.AppendSql("   AND CODE =:CODE                                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                 ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            
            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public string GetNameByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_GROUPCODE                         ");
            parameter.AppendSql(" WHERE 1 = 1                                             ");
            parameter.AppendSql("   AND CODE =:CODE                                       ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public IList<HIC_GROUPCODE> GetItemByLikeName(string argName = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HANG, CODE, NAME                                                        ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_GROUPCODE                                     ");
            parameter.AppendSql("  WHERE 1 = 1                                                                  ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE >TRUNC(SYSDATE))                            ");
            parameter.AppendSql("   AND (Jong IN ('21','22','23','24','24','25','26','32') OR Hang IN ('H','I'))"); //특수선택,공통선택
            if (!argName.IsNullOrEmpty())
            {
                parameter.AppendSql("  AND NAME LIKE :NAME                                                      ");
            }
            parameter.AppendSql(" ORDER BY NAME                                                                 ");

            if (!argName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", argName);
            }
                
            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }

        public int Delete(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_GROUPCODE       ");
            parameter.AppendSql("   SET DELDATE     = TRUNC(SYSDATE)    ");
            parameter.AppendSql("      ,ENTDATE     = SYSDATE           ");
            parameter.AppendSql("      ,ENTSABUN    = :ENTSABUN         ");
            parameter.AppendSql(" WHERE ROWID       = :RID              ");

            #region Query 변수대입
            parameter.Add("ENTSABUN",   clsType.User.IdNumber);
            parameter.Add("RID",        argRowid);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpDate(HIC_GROUPCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_GROUPCODE   ");
            parameter.AppendSql("   SET CODE		= :CODE		    ");
            parameter.AppendSql("     , NAME		= :NAME		    ");
            parameter.AppendSql("     , HANG		= :HANG		    ");
            parameter.AppendSql("     , JONG		= :JONG		    ");
            parameter.AppendSql("     , GBSELECT	= :GBSELECT	    ");
            parameter.AppendSql("     , GBSUGA		= :GBSUGA		");
            parameter.AppendSql("     , GBAM		= :GBAM		    ");
            parameter.AppendSql("     , YNAME		= :YNAME		");
            parameter.AppendSql("     , UCODE		= :UCODE		");
            parameter.AppendSql("     , SDATE		= :SDATE		");
            parameter.AppendSql("     , DELDATE 	= :DELDATE 	    ");
            parameter.AppendSql("     , ENTDATE		=  SYSDATE		");
            parameter.AppendSql("     , JOBSABUN	= :JOBSABUN		");
            parameter.AppendSql("     , GBSELF		= :GBSELF		");
            parameter.AppendSql("     , EXAMNAME	= :EXAMNAME	    ");
            parameter.AppendSql("     , GBPRINT		= :GBPRINT		");
            parameter.AppendSql("     , REMARK		= :REMARK		");
            parameter.AppendSql("     , GBDENT		= :GBDENT		");
            parameter.AppendSql("     , REEXAM		= :REEXAM		");
            parameter.AppendSql("     , GBSUNAP		= :GBSUNAP		");
            parameter.AppendSql("     , GBSANGDAM	= :GBSANGDAM	");
            parameter.AppendSql("     , GBNOTADDPAN	= :GBNOTADDPAN	");
            parameter.AppendSql("     , GBGUBUN1 	= :GBGUBUN1 	");
            parameter.AppendSql(" WHERE ROWID       = :RID          ");

            #region Query 변수대입
            parameter.Add("CODE",       item.CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME",       item.NAME);
            parameter.Add("HANG",       item.HANG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JONG",       item.JONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSELECT",   item.GBSELECT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSUGA",     item.GBSUGA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBAM",       item.GBAM);
            parameter.Add("YNAME",      item.YNAME);
            parameter.Add("UCODE",      item.UCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE",      item.SDATE);
            parameter.Add("DELDATE",    item.DELDATE);
            parameter.Add("GBSELF",     item.GBSELF);
            parameter.Add("EXAMNAME",   item.EXAMNAME);
            parameter.Add("GBPRINT",    item.GBPRINT);
            parameter.Add("REMARK",     item.REMARK);
            parameter.Add("GBDENT",     item.GBDENT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REEXAM",     item.REEXAM);
            parameter.Add("GBSUNAP",    item.GBSUNAP, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSANGDAM",  item.GBSANGDAM);
            parameter.Add("GBNOTADDPAN",item.GBNOTADDPAN);
            parameter.Add("GBGUBUN1",   item.GBGUBUN1);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_GROUPCODE item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HIC_GROUPCODE (                                                ");
            parameter.AppendSql("       CODE,HANG,NAME,JONG,GBSELECT,GBSUGA,UCODE,YNAME,SDATE,DELDATE,ENTDATE           ");
            parameter.AppendSql("      ,ENTSABUN,GBAM,GBSELF,REMARK,GBDENT,EXAMNAME,GBPRINT,REEXAM,GBSUNAP              ");
            parameter.AppendSql("      ,GBNOTADDPAN,GBSANGDAM, GBGUBUN1 )                                               ");
            parameter.AppendSql("VALUES (                                                                               ");
            parameter.AppendSql("      :CODE,:HANG,:NAME,:JONG,:GBSELECT,:GBSUGA,:UCODE,:YNAME,:SDATE,:DELDATE,SYSDATE  ");
            parameter.AppendSql("     ,:ENTSABUN,:GBAM,:GBSELF,:REMARK,:GBDENT,:EXAMNAME,:GBPRINT,:REEXAM,:GBSUNAP      ");
            parameter.AppendSql("     ,:GBNOTADDPAN,:GBSANGDAM,:GBGUBUN1 )                                              ");

            #region Query 변수대입
            parameter.Add("CODE",       item.CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("HANG",       item.HANG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME",       item.NAME);
            parameter.Add("JONG",       item.JONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSELECT",   item.GBSELECT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSUGA",     item.GBSUGA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("UCODE",      item.UCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("YNAME",      item.YNAME);
            parameter.Add("SDATE",      item.SDATE);
            parameter.Add("DELDATE",    item.DELDATE);
            parameter.Add("ENTSABUN",   clsType.User.IdNumber);
            parameter.Add("GBAM",       item.GBAM);
            parameter.Add("GBSELF",     item.GBSELF);
            parameter.Add("REMARK",     item.REMARK);
            parameter.Add("GBDENT",     item.GBDENT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXAMNAME",   item.EXAMNAME);
            parameter.Add("GBPRINT",    item.GBPRINT);
            parameter.Add("REEXAM",     item.REEXAM);
            parameter.Add("GBSUNAP",    item.GBSUNAP, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBNOTADDPAN",item.GBNOTADDPAN);
            parameter.Add("GBSANGDAM",  item.GBSANGDAM);
            parameter.Add("GBGUBUN1",   item.GBGUBUN1);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public HIC_GROUPCODE GetItemByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,HANG,JONG,GBSELECT,GBSUGA,SDATE, GBAM, GBSELF, EXAMNAME, GBPRINT, UCODE, YNAME             ");
            parameter.AppendSql("      ,DELDATE, REMARK,GBDENT,REEXAM,GBSUNAP,GBNOTADDPAN,GBSANGDAM,GBGUBUN1, ENTDATE, ROWID AS RID   ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_GROUPCODE                                                                                         ");
            parameter.AppendSql("  WHERE 1 = 1                                                                                                                      ");
            parameter.AppendSql("    AND CODE = :CODE                                                                                                               ");
            parameter.AppendSql("    AND DELDATE IS NULL                                                                                                            ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            

            return ExecuteReaderSingle<HIC_GROUPCODE>(parameter);
        }

        public string Read_Group_Name(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_GROUPCODE               ");
            parameter.AppendSql(" WHERE CODE = :CODE                            ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_GROUPCODE> GetHicItemByUCodes(List<string> argUCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HANG, CODE, UCODE, NAME                                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_GROUPCODE                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND UCODE IN (:UCODE)                                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                     ");
            parameter.AppendSql("   ORDER BY HANG,CODE                                                      ");

            parameter.AddInStatement("UCODE", argUCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_GROUPCODE>(parameter);
        }
    }
}
