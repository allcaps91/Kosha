
namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    public class ExamDisplayNewRepository : BaseRepository
    {
        public ExamDisplayNewRepository()
        {
        }

        /// <summary>
        /// Result & ExCode Join 표준 Query. 접수, 판정, 액팅, 결과 다방면에서 많이 사용됨.
        /// 아래 쿼리를 공통쿼리로 지향합니다. 
        /// 필요한 컬럼이 있으면 추가해서 사용하도록 합니다.
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="strExamList"></param>
        /// <returns></returns>
        public IList<EXAM_DISPLAY_NEW> GetItemsInResultExCode(long argWRTNO, string[] strExamList = null)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.HeaSORT,a.ExCode,b.HName,a.Result,b.ResCode,a.Panjeng,a.ROWID     ");
            parameter.AppendSql("      ,b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse            ");
            parameter.AppendSql("      ,a.Part,b.Unit,a.ExCode                                              ");
            parameter.AppendSql("      ,b.ENDOGUBUN2,b.ENDOGUBUN3,b.ENDOGUBUN4,b.ENDOGUBUN5                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE b                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND a.WRTNO =:WRTNO                                                     "); 
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                ");
            if (!strExamList.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.ExCode IN ( :EXCODE )                                   ");
            }
            
            parameter.AppendSql(" ORDER BY a.Part, a.ExCode                                                 ");

            parameter.Add("WRTNO", argWRTNO);
            if (!strExamList.IsNullOrEmpty())
            { 
                parameter.AddInStatement("EXCODE", strExamList);
            }

            return ExecuteReader<EXAM_DISPLAY_NEW>(parameter);
        }

        public IList<EXAM_DISPLAY_NEW> GetItemsInResultHaExCode(long argWRTNO, string[] strExamList = null)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.HeaSORT,a.ExCode,b.HName,a.Result,b.ResCode,a.Panjeng,a.ROWID     ");
            parameter.AppendSql("      ,b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse            ");
            parameter.AppendSql("      ,a.Part,b.Unit,a.ExCode                                              ");
            parameter.AppendSql("      ,b.ENDOGUBUN2,b.ENDOGUBUN3,b.ENDOGUBUN4,b.ENDOGUBUN5                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE b                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND a.WRTNO =:WRTNO                                                     ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                ");
            if (!strExamList.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.ExCode IN ( :EXCODE )                                   ");
            }

            parameter.AppendSql(" ORDER BY a.Part, a.ExCode                                                 ");

            parameter.Add("WRTNO", argWRTNO);
            if (!strExamList.IsNullOrEmpty())
            {
                parameter.AddInStatement("EXCODE", strExamList);
            }

            return ExecuteReader<EXAM_DISPLAY_NEW>(parameter);
        }

        public long GetWrtnoForBloodActing(long argPano, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.WRTNO                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT B                    ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND A.PANO    =:PANO                           ");
            parameter.AppendSql("   AND A.JEPDATE =TO_DATE(:JEPDATE,'YYYY-MM-DD') ");
            parameter.AppendSql("   AND A.WRTNO   =  B.WRTNO                        ");
            parameter.AppendSql("   AND A.GJJONG  = '62'                            ");
            parameter.AppendSql("   AND B.EXCODE  = 'A135'                          ");
            parameter.AppendSql("   AND A.DELDATE IS NULL                           ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argDate);

            return ExecuteScalar<long>(parameter);
        }

        public EXAM_DISPLAY_NEW GetItemForBloodActing(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT B.RESULT, B.ACTIVE                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU A                 ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT B                ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND A.WRTNO   =:WRTNO                      ");
            parameter.AppendSql("   AND A.WRTNO   =  B.WRTNO                    ");
            parameter.AppendSql("   AND A.GJJONG NOT IN ('31', '62')            ");
            parameter.AppendSql("   AND B.EXCODE  = 'A135'                      ");
            parameter.AppendSql("   AND A.DELDATE IS NULL                       ");

            parameter.Add("WRTNO", argWrtno);
            
            return ExecuteReaderSingle<EXAM_DISPLAY_NEW>(parameter);
        }

        /// <summary>
        /// Jepsu & Result Join 표준 Query. 
        /// 해당 접수별 검사항목 조회
        /// </summary>
        /// <param name="ptno"></param>
        /// <param name="jepDate"></param>
        /// <param name="strExcodes"></param>
        /// <returns></returns>
        public IList<EXAM_DISPLAY_NEW> GetItemsInJepsuResult(string ptno, string jepDate, string[] strExcodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.ExCode,b.Result                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                         ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND a.WRTNO   =  b.WRTNO(+)                         ");
            parameter.AppendSql("   AND a.PTNO    = :PTNO                               ");
            parameter.AppendSql("   AND a.JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                               ");

            if (!strExcodes.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.ExCode IN (:EXCODE)                       ");
            }

            parameter.Add("PTNO", ptno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", jepDate);

            if (!strExcodes.IsNullOrEmpty())
            {
                parameter.AddInStatement("EXCODE", strExcodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<EXAM_DISPLAY_NEW>(parameter);
        }
    }
}
