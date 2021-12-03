namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicPendingRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public HicPendingRepository()
        {
        }

        public HIC_PENDING GetIetmbyWrtNo(long fnWrtNo, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, EXAMNAME, SAYU   ");
            parameter.AppendSql("  FROM ADMIN.HIC_PENDING     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND GUBUN = :GUBUN              ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("GUBUN", strGubun);

            return ExecuteReaderSingle<HIC_PENDING>(parameter);
        }

        public int DeletebyRowId(string fstrROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM ADMIN.HIC_PENDING    ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Update(HIC_PENDING item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_PENDING SET ");
            parameter.AppendSql("       EXAMNAME = :EXAMNAME        ");
            parameter.AppendSql("     , SAYU     = :SAYU            ");
            parameter.AppendSql(" WHERE ROWID    = :RID             ");
            //parameter.AppendSql("   AND GUBUN    = :GUBUN           ");

            parameter.Add("EXAMNAME", item.EXAMNAME);
            parameter.Add("SAYU", item.SAYU);
            parameter.Add("RID", item.RID);
            //parameter.Add("GUBUN", item.GUBUN);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_PENDING item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HIC_PENDING                                        ");
            parameter.AppendSql("       (JEPDATE, WRTNO, EXAMNAME, SAYU, ENTDATE, ENTSABUN, GUBUN)          ");
            parameter.AppendSql("VALUES                                                                     ");
            parameter.AppendSql("       (:JEPDATE, :WRTNO, :EXAMNAME, :SAYU, SYSDATE, :ENTSABUN, :GUBUN)    ");

            parameter.Add("JEPDATE", item.JEPDATE);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("EXAMNAME", item.EXAMNAME);
            parameter.Add("SAYU", item.SAYU);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("GUBUN", item.GUBUN);

            return ExecuteNonQuery(parameter);
        }
    }
}
