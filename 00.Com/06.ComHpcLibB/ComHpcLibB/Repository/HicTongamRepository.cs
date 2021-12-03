namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicTongamRepository : BaseRepository
    {
       
        public HicTongamRepository()
        {
        }
        public List<HIC_TONGAM> GetCancerCount(string strSdate, string strEdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT  TO_CHAR(TDate,'YYYYMM') YYMM, SUM(CNT1) CNT1, SUM(CNT2) CNT2, SUM(CNT3) CNT3, SUM(CNT4) CNT4, SUM(CNT5) CNT5, SUM(CNT6) CNT6, SUM(CNT7) CNT7 ");
            parameter.AppendSql("   FROM ADMIN.HIC_TONGAM");

            parameter.AppendSql("   WHERE       TDate >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     TDate <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     GJJONG IN ('31', '35')  ");
            parameter.AppendSql("   GROUP BY TO_CHAR(TDate,'YYYYMM')  ");

            parameter.Add("FDATE", strSdate);
            parameter.Add("TDATE", strEdate);

            return ExecuteReader<HIC_TONGAM>(parameter);
        }

        public List<HIC_TONGAM> GetCancerData(string dtpFDate, string dtpTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT  GJJONG ,SUM(CNT1) CNT1,SUM(CNT2) CNT2, SUM(CNT3) CNT3, SUM(CNT4) CNT4, SUM(CNT5) CNT5, SUM(CNT6) CNT6 ,SUM(CNT7) CNT7 ");
            parameter.AppendSql("   FROM ADMIN.HIC_TONGAM");

            parameter.AppendSql("   WHERE       TDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     TDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("       AND     GJJONG IN ('31','35')  ");
            parameter.AppendSql("   GROUP BY    GJJONG  ");

            parameter.Add("FDATE", dtpFDate);
            parameter.Add("TDATE", dtpTDate);

            return ExecuteReader<HIC_TONGAM>(parameter);
        }

        public List<HIC_TONGAM> GetCancerTong(string dtpFDate, string dtpTDate, string cboJONG)
        {
            MParameter parameter = CreateParameter();
            

            parameter.AppendSql("   SELECT  GJJONG , SUM(CNT1) CNT1, SUM(CNT2) CNT2, SUM(CNT3) CNT3, SUM(CNT4) CNT4, SUM(CNT5) CNT5, SUM(CNT6) CNT6, SUM(CNT1+CNT2+CNT3+CNT4+CNT5+CNT6) SUBTOT, SUM(JEPCNT) JEPCNT ");

            parameter.AppendSql("   FROM  ADMIN.HIC_TONGAM");

            parameter.AppendSql("   WHERE TDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("          AND TDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')  ");
           

            if (cboJONG != "00")
            {
                parameter.AppendSql("          AND GJJONG = :CBOJONG  ");
            }

            parameter.AppendSql("   GROUP BY GJJONG  ");

            parameter.Add("TDATE", dtpTDate);
            parameter.Add("FDATE", dtpFDate);

            if (cboJONG != "00")
            {
                parameter.Add("CBOJONG", cboJONG);
            }

            return ExecuteReader<HIC_TONGAM>(parameter);
        }
    }
}
