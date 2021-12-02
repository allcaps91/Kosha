namespace HC.Core.Repository
{
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Model;
    using HC.Core.Service;

    public class ExResultRepository : BaseRepository
    {
        /// <summary>
        /// 접수번호별 일반검진 검사결과 
        /// </summary>
        /// <param name="WRTNO"></param>
        /// <returns></returns>
        public List<ExResult_WRTNO_Model> FindByWrtNo(long WRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT AA.*,                                                         ");
            parameter.AppendSql("        CASE                                                            ");
            parameter.AppendSql("            WHEN AA.RESULT_NAME IS NULL THEN AA.RESULT                  ");
            parameter.AppendSql("            WHEN AA.RESULT_NAME IS NOT NULL THEN AA.RESULT_NAME        ");
            parameter.AppendSql("        END AS STATUS                                                       ");
            parameter.AppendSql("   FROM (                                                                   ");
            parameter.AppendSql("            SELECT A.EXCODE, A.RESULT, A.RESCODE,                           ");
            parameter.AppendSql("            (SELECT NAME FROM HIC_RESCODE WHERE GUBUN = trim(a.RESCODE) AND CODE = trim(a.RESULT)) as RESULT_NAME   ");
            parameter.AppendSql("            FROM HIC_RESULT A                                                                        ");
            parameter.AppendSql("            WHERE WRTNO = :WRTNO                                                                   ");
            parameter.AppendSql("   ) AA                                                                                                 ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReader<ExResult_WRTNO_Model>(parameter);
        }

        /// <summary>
        /// 접수번호별 종합검진 검사결과 
        /// </summary>
        /// <param name="WRTNO"></param>
        /// <returns></returns>
        public List<ExResult_WRTNO_Model> FindTotalByWrtNo(long WRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT AA.*,                                                         ");
            parameter.AppendSql("        CASE                                                            ");
            parameter.AppendSql("            WHEN AA.RESULT_NAME IS NULL THEN AA.RESULT                  ");
            parameter.AppendSql("            WHEN AA.RESULT_NAME IS NOT NULL THEN AA.RESULT_NAME        ");
            parameter.AppendSql("        END AS STATUS                                                       ");
            parameter.AppendSql("   FROM (                                                                   ");
            parameter.AppendSql("            SELECT A.EXCODE, A.RESULT, A.RESCODE,                           ");
            parameter.AppendSql("            (SELECT NAME FROM HIC_RESCODE WHERE GUBUN = trim(a.RESCODE) AND CODE = trim(a.RESULT)) as RESULT_NAME   ");
            parameter.AppendSql("            FROM HEA_RESULT A                                                                        ");
            parameter.AppendSql("            WHERE WRTNO = :WRTNO                                                                   ");
            parameter.AppendSql("   ) AA                                                                                                 ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReader<ExResult_WRTNO_Model>(parameter);
        }
    }
}
