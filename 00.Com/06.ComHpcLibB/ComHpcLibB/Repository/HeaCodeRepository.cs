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
    public class HeaCodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaCodeRepository()
        {
        }

        public List<HEA_CODE> GetGroupNameByGubun(string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME, GUBUN2                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN             ");
            parameter.AppendSql(" GROUP BY NAME, GUBUN2             ");
            parameter.AppendSql(" ORDER BY GUBUN2                   ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public string GetItemByGubun1(string argExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN1  = :GUBUN1           ");

            parameter.Add("GUBUN1", argExCode);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_CODE> GetItemByGubun(string argExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN             ");

            parameter.Add("GUBUN", argExCode);

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public List<HEA_CODE> Hea_Part_Jepsu(string strGubun)
        {
            MParameter parameter = CreateParameter();

            //parameter.AppendSql("SELECT '**' AS CODE, '선택안함' as Name, TO_NUMBER('0') SORT   ");
            //parameter.AppendSql("  FROM DUAL                                                    ");
            //parameter.AppendSql(" UNION ALL                                                     ");
            parameter.AppendSql("SELECT CODE, NAME, TO_NUMBER(NVL(SORT, '999')) SORT, ROWID RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE                                    ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                          ");
            parameter.AppendSql(" ORDER BY TO_NUMBER(NVL(SORT, '999')), CODE                    ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public string GetCodeByGubun(string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN             ");
            parameter.AppendSql("   AND (GBDEL='' OR GBDEL IS NULL) ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_CODE> GetAllbyGubunCode(string strCODE, string strJong, string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME, GUBUN1                                                    ");
            parameter.AppendSql("     , (SELECT CASE WHEN COUNT('X') > 0 THEN '' ELSE 'OK' END ORDYN    ");
            parameter.AppendSql("          FROM KOSMOS_OCS.OCS_OORDER                                   ");
            parameter.AppendSql("         WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("           AND ORDERCODE = :CODE                                       ");
            parameter.AppendSql("           AND BDATE = TRUNC(SYSDATE) ) AS ORDYN                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE                                            ");
            parameter.AppendSql(" WHERE GUBUN = '18'                                                    ");
            if (!strJong.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND (CODE = :CODE OR CODE = :JONG)                      ");
            }
            else
            {
                parameter.AppendSql("   AND CODE = :CODE                                        ");
            }
            parameter.AppendSql(" ORDER BY NAME                                                 ");

            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            if (!strJong.IsNullOrEmpty())
            {
                parameter.Add("JONG", strJong, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("PTNO", strPtNo);

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public string GetNameByGubunGubun2(string argGubun, string argGubun2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN             ");
            parameter.AppendSql("   AND GUBUN2 = :GUBUN2            ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GUBUN2", argGubun2);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_CODE> FindOne(string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TRIM(CODE) AS CODE, NAME, GUBUN1                ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HEA_CODE                  ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");

            if (!string.IsNullOrEmpty(argGubun))
            {
                parameter.AppendSql("  AND GUBUN = :GUBUN                               ");
            }
            parameter.AppendSql(" ORDER BY Code                                         ");

            if (!string.IsNullOrEmpty(argGubun))
            {
                parameter.Add("GUBUN", argGubun.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public string GetGubun2ByNameGubun(string argGbName, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GUBUN2                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN             ");
            parameter.AppendSql("   AND NAME  = :NAME               ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", argGbName);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateSortbyRowId(string cODE, string rID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_CODE SET    ");
            parameter.AppendSql("       SORT    = :SORT             ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("SORT", cODE);
            parameter.Add("RID", rID);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_CODE> GetListByGubunName(string argGubun, string argName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME,CODE,GUBUN1,GUBUN2,GUBUN3,ROWID AS RID             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                         ");
            parameter.AppendSql("   AND NAME  = :NAME                                           ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("NAME", argName);

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public List<HEA_CODE> GetNameByGubun(string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME,CODE,GUBUN1,GUBUN2,GUBUN3,ROWID AS RID             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                         ");

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public string GetGubun2ByGubunCode(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GUBUN2                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN             ");
            parameter.AppendSql("   AND CODE  = :CODE               ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetNameByGubunCode(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN             ");
            parameter.AppendSql("   AND CODE  = :CODE               ");

            parameter.Add("GUBUN", argGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_CODE> GetListByCodeName(string strGubun, string strName)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "51")
            {
                parameter.AppendSql("SELECT CODE,NAME                                   ");
                parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HEA_MCODE             ");
                parameter.AppendSql(" WHERE 1 = 1                                       ");
                if (!strName.IsNullOrEmpty())
                {
                    parameter.AppendSql("  AND NAME LIKE :NAME                          ");
                }
                else
                {
                    parameter.AppendSql("  AND NAME IS NOT NULL                         ");
                }

                parameter.AppendSql(" ORDER BY CODE                                     ");
            }
            else if (strGubun == "SA")
            {
                //parameter.AppendSql("SELECT SABUN AS CODE, KORNAME AS NAME              ");
                //parameter.AppendSql("     , KOSMOS_PMPA.FC_BAS_BUSENAME(BUSE) AS GUBUN1 ");
                //parameter.AppendSql("  FROM " + ComNum.DB_ERP + "INSA_MST               ");
                //parameter.AppendSql(" WHERE 1 = 1                                       ");
                //if (!strName.IsNullOrEmpty())
                //{
                //    parameter.AppendSql("  AND KORNAME LIKE :NAME                       ");
                //}
                //else
                //{
                //    parameter.AppendSql("  AND KORNAME IS NOT NULL                      ");
                //}

                //parameter.AppendSql("  AND JAEGU ='0'                                   ");

                //parameter.AppendSql(" ORDER BY KORNAME                                  ");

                parameter.AppendSql("SELECT EMP_ID AS CODE, EMP_NM AS NAME                  ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_BAS_BUSENAME(DEPT_CD) AS GUBUN1  ");
                parameter.AppendSql("  FROM KOSMOS_ERP.HR_EMP_BASIS                         ");
                parameter.AppendSql(" WHERE 1 = 1                                           ");
                if (!strName.IsNullOrEmpty())
                {
                    parameter.AppendSql("  AND EMP_NM LIKE :NAME                            ");
                }
                else
                {
                    parameter.AppendSql("  AND EMP_NM IS NOT NULL                           ");
                }

                parameter.AppendSql("  AND EMPL_GB ='W'                                     ");

                parameter.AppendSql(" ORDER BY EMP_NM                                       ");

            }
            else
            {
                parameter.AppendSql("SELECT CODE,NAME                                   ");
                parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HEA_CODE              ");
                parameter.AppendSql(" WHERE 1 = 1                                       ");
                parameter.AppendSql("   AND GUBUN = :GUBUN                              ");

                if (!strName.IsNullOrEmpty())
                {
                    parameter.AppendSql("  AND NAME LIKE :NAME                          ");
                }

                parameter.AppendSql(" ORDER BY CODE                                     ");
            }

            if (strGubun != "51" && strGubun != "SA")
            {
                parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            }
                
            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public string GetGubun1ByCode(string argExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GUBUN1                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND CODE =:CODE                 ");
            parameter.AppendSql("   AND GUBUN = '13'                ");

            parameter.Add("CODE", argExCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_CODE> GetItemByGubunGroupBy(string argGbn, List<string> argExams, string argViewGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME,GUBUN1,GUBUN2                      ");

            //if (argViewGbn == "CAL")
            //{
            //    parameter.AppendSql("SELECT NAME,GUBUN1,GUBUN2                      ");
            //}
            //else
            //{
            //    parameter.AppendSql("SELECT DECODE(NAME, '수면위내시경','위내시경','수면대장내시경','대장내시경',NAME) NAME   ");
            //    parameter.AppendSql("      ,DECODE(GUBUN1,'GFS(수면)','GFS','Colon(수면)','Colon',GUBUN1) GUBUN1, GUBUN2      ");
            //}

            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE                                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                             ");
            if (!argExams.IsNullOrEmpty() && argExams.Count > 0)
            {
                parameter.AppendSql("   AND GUBUN2 IN (:EXAMS)                  ");
            }
            parameter.AppendSql("   AND GUBUN  = :GUBUN                         ");

            parameter.AppendSql(" GROUP By NAME, GUBUN1, GUBUN2        ");

            //if (argViewGbn == "CAL")
            //{
            //    parameter.AppendSql(" GROUP By NAME, GUBUN1, GUBUN2        ");
            //}
            //else
            //{
            //    parameter.AppendSql(" GROUP By DECODE(NAME, '수면위내시경','위내시경','수면대장내시경','대장내시경',NAME)   ");
            //    parameter.AppendSql("         ,DECODE(GUBUN1,'GFS(수면)','GFS','Colon(수면)','Colon',GUBUN1), GUBUN2        ");
            //}
            
            parameter.AppendSql(" ORDER By GUBUN2                               ");

            if (!argExams.IsNullOrEmpty() && argExams.Count > 0)
            {
                parameter.AddInStatement("EXAMS", argExams);
            }

            parameter.Add("GUBUN", argGbn, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_CODE>(parameter);
        }

        public HEA_CODE GetItemByActPart(string argActPart)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE        ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND CODE =:ACTPART              ");
            parameter.AppendSql("   AND GUBUN = '11'                ");

            parameter.Add("ACTPART", argActPart, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_CODE>(parameter);
        }

        public long GetCodebyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE            ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND (GbDel = '' or GbDel IS NULL)   ");

            parameter.Add("GUBUN", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }
    }
}
