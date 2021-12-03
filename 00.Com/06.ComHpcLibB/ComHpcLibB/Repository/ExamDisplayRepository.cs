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
    public class ExamDisplayRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public ExamDisplayRepository()
        {
        }
        
        public List<EXAM_DISPLAY> Read_ExamList(long WRTNO, List<string> ENTPART, string strGubun, List<long> AllWrtNo)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT b.HeaSORT,a.ExCode,b.HName,a.Result,b.ResCode,a.Panjeng,a.ROWID RID ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,a.SangDam  ");
                parameter.AppendSql("     , a.ACTPART, b.ENTPART, b.HEAPART                                     ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE b                                            ");
                parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
                if (ENTPART.To<string>() != "W")
                {
                    parameter.AppendSql("   AND B.HEAPART IN (:ENTPART)                                         "); //신체계측 부분만 가져옴
                }
                parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                  ");
                parameter.AppendSql(" ORDER BY b.HeaSORT, b.GBSORT, a.ExCode                                    ");
            }
            else
            {
                parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,b.ResCode,a.Panjeng,a.ROWID RID    ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse            ");
                parameter.AppendSql("     , '' ACTPART, b.ENTPART, b.HEAPART                                    ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                            ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE b                                            ");
                parameter.AppendSql(" WHERE 1 = 1                                                               ");
                if (AllWrtNo.Count > 0)
                {
                    parameter.AppendSql("   AND a.WRTNO IN (:WRTNO)                                             ");
                }
                if (ENTPART[0] != "W")
                {
                    parameter.AppendSql("   AND B.ENTPART IN (:ENTPART)                                         "); //신체계측 부분만 가져옴
                }
                parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                  ");
                parameter.AppendSql(" ORDER BY a.PART, b.gbsort, a.ExCode                                       ");
            }
            if (strGubun == "1")
            {
                parameter.Add("WRTNO", WRTNO);
            }
            else
            {
                if (AllWrtNo.Count > 0)
                {
                    parameter.AddInStatement("WRTNO", AllWrtNo);
                }
            }
            parameter.AddInStatement("ENTPART", ENTPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<EXAM_DISPLAY>(parameter);
        }

        
        public List<EXAM_DISPLAY> Read_Result(long WRTNO, string[] EXCODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.ExCode,b.HName,a.Result           ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE b            ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                    ");            
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                ");
            parameter.AppendSql("   AND a.ExCode IN (:EXCODE)               ");

            parameter.Add("WRTNO", WRTNO);
            parameter.AddInStatement("EXCODE", EXCODE);

            return ExecuteReader<EXAM_DISPLAY>(parameter);
        }
    }
}
