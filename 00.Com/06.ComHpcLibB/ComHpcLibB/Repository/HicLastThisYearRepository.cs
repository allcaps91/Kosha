namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicLastThisYearRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicLastThisYearRepository()
        {
        }

        public List<COMHPC> GetStatistics(string strSdate, string strEdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT TO_CHAR(JEPDATE,'YYYYMM') YYMM, GBINWON  GjJong, GJCHASU CHASU, COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_JEPSU");
            parameter.AppendSql("   WHERE JEPDATE >= TO_DATE(:SDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND JEPDATE <= TO_DATE(:EDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND DelDate IS NULL  ");
            parameter.AppendSql("       AND GBSTS NOT IN ( '0','D')  ");
            parameter.AppendSql("       AND GBINWON NOT IN ('57')  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(JEPDATE,'YYYYMM'),GBINWON, GJCHASU  ");

            parameter.AppendSql("   UNION ALL ");
            parameter.AppendSql("   SELECT TO_CHAR(CDate,'YYYYMM') YYMM, DECODE(GBKUKGO,'Y','72','71') GJJONG, '1' CHASU, COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_CHUKDTL");
            parameter.AppendSql("   WHERE CDate >= TO_DATE(:SDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND CDate <= TO_DATE(:EDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(CDate,'YYYYMM'), GBKUKGO  ");

            parameter.AppendSql("   UNION ALL ");
            parameter.AppendSql("   SELECT TO_CHAR(BDate,'YYYYMM') YYMM, DECODE(GBKUKGO,'Y','82','81') GJJONG,'1' CHASU, COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HIC_BOGENMST");
            parameter.AppendSql("   WHERE BDate >= TO_DATE(:SDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND BDate <= TO_DATE(:EDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND DelDate IS NULL  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(BDate,'YYYYMM'), GBKUKGO  ");

            parameter.AppendSql("   UNION ALL ");
            parameter.AppendSql("   SELECT TO_CHAR(SDATE,'YYYYMM') YYMM,DECODE(SUBSTR(GJJONG,1,1) , '2' ,'92','91') GJJONG, '1'  CHASU,COUNT(*) JEPCNT ");
            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "HEA_JEPSU");
            parameter.AppendSql("   WHERE SDate >= TO_DATE(:SDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND SDate <= TO_DATE(:EDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND GBSTS NOT IN ('0','D')  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(SDATE,'YYYYMM'), DECODE(SUBSTR(GJJONG, 1, 1) , '2','92', '91')  ");

            parameter.Add("SDATE", strSdate);
            parameter.Add("EDATE", strEdate);

            return ExecuteReader<COMHPC>(parameter);
        }
    }
}