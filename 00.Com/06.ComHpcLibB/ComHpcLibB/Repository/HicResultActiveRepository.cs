namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComBase.Controls;

    /// <summary>
    /// 
    /// </summary>
    public class HicResultActiveRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HicResultActiveRepository()
        {
        }
        
        public List<HIC_RESULT_ACTIVE> Read_Result(long WRTNO, string HEAPART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Result                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT A        ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE B        ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql("   AND WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND A.Part   = '5'                  "); //ACT코드만
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)            ");
            parameter.AppendSql("   AND Trim(B.HeaPART) = :HEAPART      ");

            parameter.Add("WRTNO", WRTNO); 
            parameter.Add("HEAPART", HEAPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_ACTIVE>(parameter);
        }

        public List<HIC_RESULT_ACTIVE> GetActivebyWrtno(long nWrtNo, string strBuse)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Active, a.EXCODE                  ");
            if (strBuse == "1") //종검
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT A        ");
            }
            else if (strBuse == "2")    //일반검진
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT A        ");
            }
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE B            ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                    ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                   ");
            parameter.AppendSql("   AND b.Part = '1'                        "); //신체계측구분

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT_ACTIVE>(parameter);
        }

        public string GetActiveByPanoEntPart(long nPANO, string argEntPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ACTIVE                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                    ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE b                    ");
            parameter.AppendSql(" WHERE a.WRTNO IN (                                ");
            parameter.AppendSql("       SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU     ");
            parameter.AppendSql("              WHERE PANO = :PANO                   ");
            parameter.AppendSql("                AND JEPDATE = TRUNC(SYSDATE)       ");
            parameter.AppendSql("                AND DELDATE IS NULL )              ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                           ");
            parameter.AppendSql("   AND b.ENTPART = :ENTPART                        ");
            //parameter.AppendSql("   AND (a.ACTIVE = '' OR a.ACTIVE IS NULL)         ");

            parameter.Add("PANO", nPANO);
            parameter.Add("ENTPART", argEntPart);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_RESULT_ACTIVE> Read_Active(long WRTNO, string HEAPART = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ACTIVE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT A                ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE B                ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND A.EXCODE NOT IN ('A101','A102','A103')  "); //ACT코드만
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                    ");
            if (HEAPART != "")
            {
                parameter.AppendSql("   AND Trim(B.HeaPART) = :HEAPART          ");
            }
            parameter.AppendSql("   AND(a.Active = '' or a.Active is null)      ");

            parameter.Add("WRTNO", WRTNO);
            if (HEAPART != "")
            {
                parameter.Add("HEAPART", HEAPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT_ACTIVE>(parameter);
        }

        public List<HIC_RESULT_ACTIVE> Read_Active_Hic(long WRTNO, long nPaNo, string ArgDate, string strJong, string ENTPART = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ACTIVE                                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT A                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE B                                            ");
            if (strJong == "Y")
            {
                parameter.AppendSql(" WHERE A.WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU             ");
                parameter.AppendSql("                    WHERE PANO = :PANO                                 ");
                parameter.AppendSql("                      AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')    ");
                parameter.AppendSql("                      AND deldate IS NULL                              ");
                parameter.AppendSql("                  )                                                    ");
            }
            else
            {
                parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                ");
            }
            parameter.AppendSql("   AND A.EXCODE = B.CODE                                                   ");  
            parameter.AppendSql("   AND (a.ACTIVE = '' OR a.ACTIVE IS NULL)                                 ");
            if (!ENTPART.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND B.ENTPART = :ENTPART                                            ");
            }
                   
            if (!ENTPART.IsNullOrEmpty())
            {
                parameter.Add("ENTPART", ENTPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strJong == "Y")
            {
                parameter.Add("PANO", nPaNo);
                parameter.Add("JEPDATE", ArgDate);
            }
            else
            {
                parameter.Add("WRTNO", WRTNO);
            }

            return ExecuteReader<HIC_RESULT_ACTIVE>(parameter);
        }

        public List<HIC_RESULT_ACTIVE> Read_Result2(long WRTNO, string HEAPART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Result                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT A                ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE B                ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND A.Result IS NULL                        "); //ACT코드만
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                    ");
            parameter.AppendSql("   AND Trim(B.HeaPART) = :HEAPART              ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("HEAPART", HEAPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_ACTIVE>(parameter);
        }
    }
}
