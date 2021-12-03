namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicMisuDetailRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuDetailRepository()
        {
        }

        public int InsertList(HIC_MISU_DETAIL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO            HIC_MISU_DETAIL (WRTNO,MISUNO,GUBUN,GJJONG,LTDCODE,MISUJONG,BDATE,ENTDATE,ENTJOBSABUN                ");
            parameter.AppendSql("                     , MISUAMT,IPGUMAMT,GAMAMT,BANAMT,SAKAMT,HALAMT,ETCAMT,JANAMT,DAMNAME,REMARK                            ");
            parameter.AppendSql("                       ) VALUES (                                                                                           ");
            parameter.AppendSql("                       :WRTNO, :MISUNO, :GUBUN, :GJJONG, :LTDCODE, :MISUJONG, TO_DATE(:BDATE,'YYYY-MM-DD')                  ");
            parameter.AppendSql("                     , SYSDATE, :ENTJONSABUN, :MISUAMT, :IPGUMANT, :GAMAMT, :BANAMT, :SAKAMT, :HALAMT, :ETCAMT, :JANAMT     ");
            parameter.AppendSql("                     , :DAMNAME, :REMARK)                                                                                    ");

            parameter.Add("WRTNO",                  item.WRTNO);
            parameter.Add("MISUNO",                 item.MISUNO);
            parameter.Add("GUBUN",                  item.GUBUN);
            parameter.Add("GJJONG",                 item.GJJONG);
            parameter.Add("LTDCODE",                item.LTDCODE);
            parameter.Add("MISUJONG",               item.MISUJONG);
            parameter.Add("BDATE",                  item.BDATE);
            parameter.Add("ENTJONSABUN",            item.ENTJOBSABUN);
            parameter.Add("MISUAMT",                item.MISUAMT);
            parameter.Add("IPGUMANT",               item.IPGUMAMT);
            parameter.Add("GAMAMT",                 item.GAMAMT);
            parameter.Add("BANAMT",                 item.BANAMT);
            parameter.Add("SAKAMT",                 item.SAKAMT);
            parameter.Add("HALAMT",                 item.HALAMT);
            parameter.Add("ETCAMT",                 item.ETCAMT);
            parameter.Add("JANAMT",                 item.JANAMT);
            parameter.Add("DAMNAME",                item.DAMNAME);
            parameter.Add("REMARK",                 item.REMARK);

            return ExecuteNonQuery(parameter);
        }

        public int GongDanInsert(HIC_MISU_DETAIL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO            HIC_MISU_DETAIL (WRTNO,MISUNO,GUBUN,GJJONG,LTDCODE,MISUJONG,BDATE,ENTDATE,ENTJOBSABUN                ");
            parameter.AppendSql("                     , MISUAMT,IPGUMAMT,GAMAMT,BANAMT,SAKAMT,HALAMT,ETCAMT,JANAMT,DAMNAME,REMARK                            ");
            parameter.AppendSql("                       ) VALUES (                                                                                           ");
            parameter.AppendSql("                       :WRTNO, :MISUNO, :GUBUN, :GJJONG, :LTDCODE, :MISUJONG, TO_DATE(:BDATE,'YYYY-MM-DD')                  ");
            parameter.AppendSql("                     , SYSDATE, :ENTJONSABUN, :MISUAMT, :IPGUMANT, :GAMAMT, :BANAMT, :SAKAMT, :HALAMT, :ETCAMT, :JANAMT     ");
            parameter.AppendSql("                     , :DAMNAME, :REMARK)                                                                                    ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("MISUNO", item.MISUNO);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("GJJONG", item.GJJONG);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("MISUJONG", item.MISUJONG);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("ENTJONSABUN", item.ENTJOBSABUN);
            parameter.Add("MISUAMT", item.MISUAMT);
            parameter.Add("IPGUMANT", item.IPGUMAMT);
            parameter.Add("GAMAMT", item.GAMAMT);
            parameter.Add("BANAMT", item.BANAMT);
            parameter.Add("SAKAMT", item.SAKAMT);
            parameter.Add("HALAMT", item.HALAMT);
            parameter.Add("ETCAMT", item.ETCAMT);
            parameter.Add("JANAMT", item.JANAMT);
            parameter.Add("DAMNAME", item.DAMNAME);
            parameter.Add("REMARK", item.REMARK);

            return ExecuteNonQuery(parameter);
        }
    }
}
