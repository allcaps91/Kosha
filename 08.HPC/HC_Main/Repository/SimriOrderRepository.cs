namespace HC_Main.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using HC_Main.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class SimriOrderRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public SimriOrderRepository()
        {
        }

        public string GetRowidByPanoBDate(string argPtno, string argBDate, double nQty)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.SIMRI_ORDER                     ");
            parameter.AppendSql(" WHERE PANO    = :PANO                             ");
            parameter.AppendSql("   AND BDATE   = TO_DATE(:BDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND SUNEXT  = 'ZA44'                            ");
            parameter.AppendSql("   AND QTY     =:QTY                               ");

            parameter.Add("PANO", argPtno);
            parameter.Add("BDATE", argBDate);
            parameter.Add("QTY", nQty);

            return ExecuteScalar<string>(parameter);
        }

        public void DeleteByRowid(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.SIMRI_ORDER WHERE ROWID =:RID    ");

            parameter.Add("RID", argRowid);

            ExecuteNonQuery(parameter);
        }

        public void InsertDataFromHic(HIC_JEPSU item, double nQty)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.SIMRI_ORDER (                                      ");
            parameter.AppendSql("       PANO,ACTDATE,SNAME,IPDOPD,BDATE,SEX,AGE                             ");
            parameter.AppendSql("      ,DEPTCODE,DRCODE,SUNEXT,QTY,SUNAPTIME,SUNAPSABUN                     ");
            parameter.AppendSql(" ) VALUES (                                                                ");
            parameter.AppendSql("       :PANO, TRUNC(SYSDATE), :SNAME, 'O', TO_DATE(:BDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("      ,:SEX, :AGE, 'HR', '7101', 'ZA44', :QTY, SYSDATE, :SUNAPSABUN        ");
            parameter.AppendSql(" )                                                                         ");

            parameter.Add("PANO", item.PTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("BDATE", item.JEPDATE);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("QTY", nQty);
            parameter.Add("SUNAPSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }

        public void UpDateQtyByRowid(double nQty, string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.SIMRI_ORDER SET QTY = :QTY  ");
            parameter.AppendSql(" WHERE ROWID =:ROWID                           ");

            parameter.Add("QTY", nQty);
            parameter.Add("ROWID", argRowid);

            ExecuteNonQuery(parameter);
        }
    }
}
