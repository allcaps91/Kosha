namespace HC_Main.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC_Main.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvMemoRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvMemoRepository()
        {
        }

        public List<HEA_RESV_MEMO> GetListByAll()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SNAME,EXAMENAME,TEL,ENTSABUN,CONFIRM,ROWID,SANGDAM ");
            parameter.AppendSql("       ,TO_CHAR(SDATE,'YYYY-MM-DD') SDATE                  ");
            parameter.AppendSql("       ,TO_CHAR(SUBDATE,'YYYY-MM-DD') SUBDATE              ");
            parameter.AppendSql("       ,TO_CHAR(ENTTIME,'YYYY-MM-DD HH24:MI') ENTTIME      ");
            parameter.AppendSql("       ,ROWID AS RID                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_MEMO                           ");
            parameter.AppendSql(" ORDER BY CONFIRM DESC, SDATE DESC, SNAME, SUBDATE         ");

            return ExecuteReader<HEA_RESV_MEMO>(parameter);
        }

        public void Insert(HEA_RESV_MEMO code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HEA_RESV_MEMO (                                       ");
            parameter.AppendSql("       SDATE,SNAME,EXAMENAME,SANGDAM,TEL,ENTTIME,ENTSABUN,CONFIRM              ");
            if (!code.SUBDATE.IsNullOrEmpty())
            {
                parameter.AppendSql("       ,SUBDATE                                                            ");
            }
            parameter.AppendSql(" ) VALUES (                                                                    ");
            parameter.AppendSql("       :SDATE,:SNAME,:EXAMENAME,:SANGDAM,:TEL,SYSDATE,:ENTSABUN,:CONFIRM       ");
            if (!code.SUBDATE.IsNullOrEmpty())
            {
                parameter.AppendSql("       ,TO_DATE(:SUBDATE, 'YYYY-MM-DD')                                    ");
            }
            parameter.AppendSql(" )                                                                             ");

            parameter.Add("SDATE",      code.SDATE.Substring(0, 10));
            parameter.Add("SNAME",      code.SNAME);
            parameter.Add("EXAMENAME",  code.EXAMENAME);
            parameter.Add("SANGDAM",    code.SANGDAM);
            parameter.Add("TEL",        code.TEL);
            parameter.Add("ENTSABUN",   clsType.User.IdNumber.To<long>());
            parameter.Add("CONFIRM",    code.CONFIRM);
            if (!code.SUBDATE.IsNullOrEmpty())
            {
                parameter.Add("SUBDATE", code.SUBDATE);
            }

            ExecuteNonQuery(parameter);
        }

        public void Update(HEA_RESV_MEMO code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESV_MEMO                   ");
            parameter.AppendSql("   SET SDATE     =TO_DATE(:SDATE,'YYYY-MM-DD')     ");
            parameter.AppendSql("      ,SNAME     =:SNAME                           ");
            parameter.AppendSql("      ,SUBDATE   =TO_DATE(:SUBDATE,'YYYY-MM-DD')   ");
            parameter.AppendSql("      ,TEL       =:TEL                             ");
            parameter.AppendSql("      ,EXAMENAME =:EXAMENAME                       ");
            parameter.AppendSql("      ,SANGDAM   =:SANGDAM                         ");
            parameter.AppendSql("      ,CONFIRM   =:CONFIRM                         ");
            parameter.AppendSql(" WHERE ROWID =:RID                                 ");

            parameter.Add("SDATE", code.SDATE.Substring(0, 10));
            parameter.Add("SNAME", code.SNAME);
            parameter.Add("SUBDATE", code.SUBDATE.Substring(0, 10));
            parameter.Add("TEL", code.TEL);
            parameter.Add("EXAMENAME", code.EXAMENAME);
            parameter.Add("SANGDAM", code.SANGDAM);
            parameter.Add("CONFIRM", code.CONFIRM);
            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HEA_RESV_MEMO code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_RESV_MEMO WHERE ROWID =:RID ");

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }
    }
}
