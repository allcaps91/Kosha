namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicJinGbnRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJinGbnRepository()
        {
        }

        public string GetItembyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_JIN_GBN ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int Update(HIC_JIN_GBN item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_JIN_GBN SET     ");
            parameter.AppendSql("       GUBUN       = :GUBUN            ");
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO      ");
            parameter.AppendSql("     , ENTDATE     = SYSDATE           ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN         ");
            parameter.AppendSql("  WHERE ROWID      = :RID              ");

            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int PanJengDrNoUpdate(HIC_JIN_GBN item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_JIN_GBN SET     ");
            parameter.AppendSql("       PANJENGDRNO = :PANJENGDRNO      ");
            parameter.AppendSql("     , ENTDATE     = SYSDATE           ");
            parameter.AppendSql("  WHERE ROWID      = :RID              ");

            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int PrintUpdate(HIC_JIN_GBN item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_JIN_GBN SET     ");
            parameter.AppendSql("       GBPRINT       = 'Y'             ");
            parameter.AppendSql("  WHERE WRTNO      = :WRTNO            ");

            parameter.Add("WRTNO", item.WRTNO);


            return ExecuteNonQuery(parameter);
        }

        public HIC_JIN_GBN GetItemByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(b.ENTDATE,'YYYY-MM-DD') ENTDATE,b.ENTSABUN, B.GBPRINT   ");
            parameter.AppendSql(" , B.GUBUN, B.WRTNO                                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a , ADMIN.HIC_JIN_GBN b             ");
            parameter.AppendSql(" WHERE a.WRTNO   = :WRTNO                                              ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                            ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_JIN_GBN>(parameter);
        }

        public int Insert(HIC_JIN_GBN item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_JIN_GBN                        ");
            parameter.AppendSql("       (WRTNO, GUBUN, PANJENGDRNO, ENTDATE, ENTSABUN)      ");
            parameter.AppendSql("VALUES                                                     ");
            parameter.AppendSql("       (:WRTNO, :GUBUN, :PANJENGDRNO, SYSDATE, :ENTSABUN)  ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("GUBUN", item.GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }
    }
}
