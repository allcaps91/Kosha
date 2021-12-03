namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    
    /// <summary>
    /// 
    /// </summary>
    public class SelectJeplistRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public SelectJeplistRepository()
        {
        }
        
        public List<Select_JepList> Read_Sangdam_View4(string SDATE, string PART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Wrtno, b.Sname,b.Gjjong, C.NAME                           ");
            parameter.AppendSql("     , decode(b.AMPM2, '2', '오후', '') AMPM2                      ");
            parameter.AppendSql("  From ADMIN.HEA_RESULT a                                    ");
            parameter.AppendSql("     , ADMIN.HEA_JEPSU  b                                    ");
            parameter.AppendSql("     , ADMIN.HEA_EXJONG c                                    ");
            parameter.AppendSql(" WHERE a.Wrtno = b.Wrtno                                           ");
            parameter.AppendSql("   AND a.Result IS NULL                                            ");
            parameter.AppendSql("   AND b.SDATE = TO_DATE(:SDATE,'YYYY-MM-DD')                      ");
            parameter.AppendSql("   AND b.DELDATE IS  NULL                                          ");
            parameter.AppendSql("   AND b.GBSTS NOT IN ('0','D')                                    ");
            parameter.AppendSql("   AND a.ActPart = :ACTPART                                        ");
            parameter.AppendSql("   AND a.ExCode <> 'A103'                                          "); //비만도제외
            parameter.AppendSql("   AND (a.Active is Null OR a.Active = ' ')                        ");
            parameter.AppendSql("   AND B.GJJONG = C.CODE(+)                                        ");
            parameter.AppendSql(" GROUP By a.Wrtno, b.Sname, b.Gjjong, C.NAME, b.AMPM2              ");
            parameter.AppendSql(" ORDER By a.Wrtno                                                  ");

            parameter.Add("SDATE", SDATE);
            parameter.Add("ACTPART", PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<Select_JepList>(parameter);
        }

        public List<Select_JepList> Read_Sangdam_View4_Hic(string SDATE, string PART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Wrtno, b.Sname,b.Gjjong, C.NAME                           ");
            parameter.AppendSql("     , '' AMPM2, d.PTNO, d.SEX                                     ");
            parameter.AppendSql("  From ADMIN.HIC_RESULT  a                                   ");
            parameter.AppendSql("     , ADMIN.HIC_JEPSU   b                                   ");
            parameter.AppendSql("     , ADMIN.HIC_EXJONG  c                                   ");
            parameter.AppendSql("     , ADMIN.HIC_PATIENT d                                   ");
            parameter.AppendSql(" WHERE a.Wrtno = b.Wrtno                                           ");
            parameter.AppendSql("   AND b.JEPDATE = TO_DATE(:SDATE,'YYYY-MM-DD')                    ");
            parameter.AppendSql("   AND b.DELDATE IS  NULL                                          ");
            parameter.AppendSql("   AND a.PART = :ACTPART                                           ");
            parameter.AppendSql("   AND b.jonggumyn <> '1'                                          ");
            parameter.AppendSql("   AND (a.Active is Null OR a.Active = ' ')                        ");
            parameter.AppendSql("   AND b.GJJONG = c.CODE(+)                                        ");
            parameter.AppendSql("   AND b.PANO = d.PANO(+)                                          ");
            parameter.AppendSql(" GROUP By a.Wrtno, b.Sname, b.Gjjong, C.NAME, d.PTNO, d.SEX        ");
            parameter.AppendSql(" ORDER By a.Wrtno                                                  ");

            parameter.Add("SDATE", SDATE);
            parameter.Add("ACTPART", PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<Select_JepList>(parameter);
        }
    }
}
