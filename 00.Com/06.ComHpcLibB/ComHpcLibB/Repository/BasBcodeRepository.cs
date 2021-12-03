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
    public class BasBcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasBcodeRepository()
        {
        }

        public BAS_BCODE GetAllByGubunCode(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                       ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql(" AND CODE = :CODE              ");

            parameter.Add("GUBUN", argGubun);
            parameter.Add("CODE", argCode);

            return ExecuteReaderSingle<BAS_BCODE>(parameter);
        }
        public BAS_BCODE GetAllByGubunCode1(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                       ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN  = :GUBUN         ");
            parameter.AppendSql(" AND GUBUN2 = :CODE              ");

            parameter.Add("GUBUN", argGubun);
            parameter.Add("CODE", argCode);

            return ExecuteReaderSingle<BAS_BCODE>(parameter);
        }

        public List<BAS_BCODE> GetCodeNamebyBCode(string strGubun, string strJobSabun = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND DELDATE IS NULL         ");
            if (!strJobSabun.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND CODE = :CODE        ");
            }
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("GUBUN", strGubun);
            if (!strJobSabun.IsNullOrEmpty())
            {
                parameter.Add("CODE", strJobSabun);
            }

            return ExecuteReader<BAS_BCODE>(parameter);
        }

        public List<BAS_BCODE> GetItembyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("GUBUN", strGubun);

            return ExecuteReader<BAS_BCODE>(parameter);
        }

        public BAS_GAMF Read_Gam_Opd(string JUMIN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GAMCODE,  GAMMESSAGE                    ");
            parameter.AppendSql("     , TO_CHAR(GamEnd, 'YYYY-MM-DD') GamEnd    ");
            parameter.AppendSql("  FROM ADMIN.BAS_GAMF                    ");
            parameter.AppendSql(" WHERE GamJumin3 = :JUMIN                      ");

            parameter.Add("JUMIN", JUMIN);

            return ExecuteReaderSingle<BAS_GAMF>(parameter);
        }

        public List<BAS_BCODE> GetListByCodeIn(string argGubun, string[] strCodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                                  ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND GUBUN =:GUBUN                               ");
            parameter.AppendSql("   AND CODE IN (:CODE)                            ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE>TRUNC(SYSDATE)) ");
            parameter.AppendSql(" ORDER BY CODE                                     ");

            parameter.Add("GUBUN", argGubun);
            parameter.AddInStatement("CODE", strCodes);

            return ExecuteReader<BAS_BCODE>(parameter);
        }

        public List<BAS_BCODE> GetCodeNamebyBGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                                  ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND GUBUN =:GUBUN                               ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE>TRUNC(SYSDATE)) ");
            parameter.AppendSql(" ORDER BY CODE                                     ");

            parameter.Add("GUBUN", strGubun);

            return ExecuteReader<BAS_BCODE>(parameter);
        }

        public List<BAS_BCODE> GetCodeNamebyBGubunCode(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                                  ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                              ");
            parameter.AppendSql("   AND CODE  = :CODE                               ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCode);
            

            return ExecuteReader<BAS_BCODE>(parameter);
        }

        public List<BAS_BCODE> GetListHicExamMstByGubun(string argGbn, string argSex, int argAge, string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME, PART, SORT, CNT, GUBUN2                         ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND GUBUN ='HIC_EXAM_MST'                                       ");
            parameter.AppendSql("   AND SUBSTR(CODE, 1, 1) = :GUBUN                                 ");
            parameter.AppendSql("   AND (PART ='*' OR PART = :SEX)                                  ");
            parameter.AppendSql("   AND SORT =:AGE                                                  ");
            parameter.AppendSql("   AND (JDATE IS NULL OR JDATE<=TO_DATE(:JEPDATE,'YYYY-MM-DD'))    ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                            ");
            parameter.AppendSql(" GROUP BY CODE, NAME, PART, SORT, CNT, GUBUN2                      ");
            parameter.AppendSql(" UNION                                                             ");
            parameter.AppendSql("SELECT CODE, NAME, PART, SORT, CNT, GUBUN2                         ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND GUBUN ='HIC_EXAM_MST'                                       ");
            parameter.AppendSql("   AND CODE IN (                                                   ");
            parameter.AppendSql("       SELECT SUBSTR(CODE, 1, 8) AS CODE                           ");
            parameter.AppendSql("         FROM ADMIN.BAS_BCODE                                ");
            parameter.AppendSql("        WHERE 1 = 1                                                ");
            parameter.AppendSql("          AND GUBUN ='HIC_EXAM_MST_SUB'                            ");
            parameter.AppendSql("          AND SUBSTR(CODE, 1, 1) = :GUBUN                          ");
            parameter.AppendSql("          AND (PART ='*' OR PART = :SEX)                           ");
            parameter.AppendSql("          AND SORT =:AGE                                           ");
            parameter.AppendSql(" )                                                                 ");
            parameter.AppendSql("   AND (JDATE IS NULL OR JDATE<=TO_DATE(:JEPDATE,'YYYY-MM-DD'))    ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                            ");
            parameter.AppendSql(" GROUP BY CODE, NAME, PART, SORT, CNT, GUBUN2                      ");
            parameter.AppendSql(" ORDER BY CODE,SORT                                                ");

            parameter.Add("GUBUN",   argGbn);
            parameter.Add("SEX",     argSex);
            parameter.Add("AGE",     argAge);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<BAS_BCODE>(parameter);
        }

        public List<BAS_BCODE> FindAllByGubun(string strGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,TO_CHAR(JDATE,'YYYY-MM-DD') JDATE         ");
            parameter.AppendSql("      ,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE,ROWID AS RID  ");
            parameter.AppendSql("      ,SORT,PART,CNT,GUBUN2                                ");
            parameter.AppendSql("  FROM ADMIN.BAS_BCODE                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND GUBUN =:GUBUN                               ");
            parameter.AppendSql(" ORDER BY CODE                                     ");

            parameter.Add("GUBUN", strGbn);

            return ExecuteReader<BAS_BCODE>(parameter);
        }

        public void Delete(BAS_BCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.BAS_BCODE                 ");
            parameter.AppendSql(" WHERE ROWID =:RID                            ");
            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Update(BAS_BCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.BAS_BCODE                       ");
            parameter.AppendSql("   SET CODE     =:CODE                             ");
            parameter.AppendSql("      ,NAME     =:NAME                             ");
            parameter.AppendSql("      ,GUBUN2   =:GUBUN2                           ");
            parameter.AppendSql("      ,SORT     =:SORT                             ");
            parameter.AppendSql("      ,PART     =:PART                             ");
            parameter.AppendSql("      ,CNT      =:CNT                              ");
            parameter.AppendSql("      ,JDATE    =TO_DATE(:JDATE,'YYYY-MM-DD')      ");
            parameter.AppendSql("      ,DELDATE  =TO_DATE(:DELDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("      ,ENTSABUN =ENTSABUN                          ");
            parameter.AppendSql("      ,ENTTIME  =SYSDATE                           ");
            parameter.AppendSql(" WHERE ROWID    =:RID                              ");

            parameter.Add("CODE",       code.CODE);
            parameter.Add("NAME",       code.NAME);
            parameter.Add("GUBUN2",     code.GUBUN2);
            parameter.Add("SORT",       code.SORT);
            parameter.Add("PART",       code.PART);
            parameter.Add("CNT",        code.CNT);
            parameter.Add("JDATE",      code.JDATE);
            parameter.Add("DELDATE",    code.DELDATE);
            parameter.Add("ENTSABUN",   clsType.User.IdNumber);
            parameter.Add("RID",        code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Insert(BAS_BCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT ADMIN.BAS_BCODE (                                                     ");
            parameter.AppendSql("   GUBUN,CODE,NAME,GUBUN2,SORT,PART,CNT,JDATE,DELDATE,ENTSABUN,ENTDATE             ");
            parameter.AppendSql(" ) VALUES (                                                                        ");
            parameter.AppendSql("  :GUBUN,:CODE,:NAME,:GUBUN2,:SORT,:PART,:CNT,TO_CHAR(:JDATE,'YYYY-MM-DD')         ");
            parameter.AppendSql(" ,TO_CHAR(:DELDATE,'YYYY-MM-DD'),:ENTSABUN,SYSDATE )                               ");

            parameter.Add("GUBUN",      code.GUBUN);
            parameter.Add("CODE",       code.CODE);
            parameter.Add("NAME",       code.NAME);
            parameter.Add("GUBUN2",     code.GUBUN2);
            parameter.Add("SORT",       code.SORT);
            parameter.Add("PART",       code.PART);
            parameter.Add("CNT",        code.CNT);
            parameter.Add("JDATE",      code.JDATE);
            parameter.Add("DELDATE",    code.DELDATE);
            parameter.Add("ENTSABUN",   clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }
    }
}
