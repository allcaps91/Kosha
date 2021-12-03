namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC.OSHA.Model;
    using HC_Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class IncomeRepository : BaseRepository
    {

        public List<IncomeModel> FindAll(long siteId, string startDate, string endDate, bool isHistory)
        {
          
            MParameter parameter = CreateParameter();
           // parameter.AppendSql("SELECT TO_CHAR(B.CREATED,'YYYY-MM-DD') AS CREATED,  C.ID AS SITEID,  C.NAME AS SITENAME, TO_CHAR(A.VISITDATETIME, 'YYYY-MM-DD') AS VISITDATE, VISITUSERNAME, SUM(B.WORKERCOUNT) AS WORKERCOUNT, B.UNITPRICE, SUM(b.totalprice) AS TOTALPRICE, B.ISDELETED         ");
            parameter.AppendSql("SELECT TO_CHAR(B.CREATED,'YYYY-MM-DD') AS CREATED,  C.ID AS SITEID,  C.NAME AS SITENAME, TO_CHAR(A.VISITDATETIME, 'YYYY-MM-DD') AS VISITDATE, VISITUSERNAME, B.WORKERCOUNT AS WORKERCOUNT, B.UNITPRICE, b.totalprice AS TOTALPRICE, B.ISDELETED         ");

            parameter.AppendSql("FROM HIC_OSHA_VISIT  A         ");
            parameter.AppendSql("INNER JOIN HIC_OSHA_VISIT_PRICE B         ");
            parameter.AppendSql("ON A.ID = B.VISIT_ID         ");
            parameter.AppendSql("INNER JOIN HC_SITE_VIEW C         ");
            parameter.AppendSql("ON A.SITE_ID = C.ID         ");
            parameter.AppendSql("WHERE 1=1         ");
            parameter.AppendSql("AND A.ISPRECHARGE = 'N'        ");

            if (!isHistory)
            {
                parameter.AppendSql("AND A.ISDELETED = 'N'         ");
                parameter.AppendSql("AND B.ISDELETED = 'N' ");
            }
            

            if (siteId > 0)
            {
                parameter.AppendSql("AND C.ID = :SITEID ");

            }
   
            //parameter.AppendSql("AND B.CREATED >= TO_TIMESTAMP(:STARTDATE, 'YYYY-MM-DD HH24:MI:SS')         ");
            //parameter.AppendSql("AND B.CREATED <= TO_TIMESTAMP(:ENDDATE, 'YYYY-MM-DD HH24:MI:SS')         ");
            parameter.AppendSql("AND  TO_CHAR(B.CREATED,'YYYY-MM-DD') >= :STARTDATE         ");
            parameter.AppendSql("AND  TO_CHAR(B.CREATED,'YYYY-MM-DD') <= :ENDDATE         ");
        //    parameter.AppendSql("GROUP BY TO_CHAR(B.CREATED, 'YYYY-MM-DD'), C.NAME, C.ID, TO_CHAR(A.VISITDATETIME, 'YYYY-MM-DD'), VISITUSERNAME, B.UNITPRICE ,B.ISDELETED        ");
            parameter.AppendSql("ORDER BY TO_CHAR(B.CREATED, 'YYYY-MM-DD'), C.ID, C.NAME , B.ID     ");

            parameter.Add("STARTDATE", startDate);
            parameter.Add("ENDDATE", endDate);

            if (siteId > 0)
            {
                parameter.Add("SITEID", siteId);

            }
            return ExecuteReader<IncomeModel>(parameter);
        }
    }
}
