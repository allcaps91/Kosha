namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HicCharttransHisRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicCharttransHisRepository()
        {
        }

        public int Insert(HIC_CHARTTRANS_HIS item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CHARTTRANS_HIS                                                 ");
            parameter.AppendSql(" (WRTNO,TRDATE,SNAME,GJJONG,TRLIST,EntTime,EntSabun,RecvSabun,RecvTime,BackupTime)         ");
            parameter.AppendSql(" SELECT WRTNO,TRDATE,SNAME,GJJONG,TRLIST,EntTime,EntSabun,RecvSabun,RecvTime,SYSDATE       ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_CHARTTRANS                                                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                      ");

            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int InsertDel(HIC_CHARTTRANS_HIS item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_CHARTTRANS_HIS                                                 ");
            parameter.AppendSql(" (WRTNO,TRDATE,SNAME,GJJONG,TRLIST,EntTime,EntSabun,RecvSabun,RecvTime,BackupTime,GBDEL)   ");
            parameter.AppendSql(" SELECT WRTNO,TRDATE,SNAME,GJJONG,TRLIST,EntTime,EntSabun,RecvSabun,RecvTime,SYSDATE,'Y'   ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_CHARTTRANS                                                           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                      ");

            parameter.Add("WRTNO", item.WRTNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
