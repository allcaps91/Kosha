namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSjPanoRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSjPanoRepository()
        {
        }

        public int UPdateGbDelbyLtdCode(string strYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SJ_PANO ");
            parameter.AppendSql("   SET GbDel   = 'Y'           ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR       ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE      ");

            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SJ_PANO GetItembyGjYearLtdCodeWrtNo(string strYear, long nLtdCode, long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, BUSE, PANJENG    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SJ_PANO     ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO            ");
            parameter.AppendSql("   AND GJYEAR  = :GJYEAR           ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE          ");

            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SJ_PANO>(parameter);
        }

        public int UpdatebyRowId(string strBuse, string strChukResult, string strPanjeng, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SJ_PANO     ");
            parameter.AppendSql("       BUSE       = :BUSE          ");
            parameter.AppendSql("     , CHUKRESULT = :CHUKRESULT    ");
            parameter.AppendSql("     , PANJENG    = :PANJENG       ");
            parameter.AppendSql(" WHERE ROWID      = :RID           ");

            parameter.Add("BUSE", strBuse);
            parameter.Add("CHUKRESULT", strChukResult);
            parameter.Add("PANJENG", strPanjeng);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengGbDelbyRowId(HIC_SJ_PANO item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SJ_PANO ");
            parameter.AppendSql("   SET PANJENG = :PANJENG      ");
            parameter.AppendSql("     , GbDel   = 'Y'           ");
            parameter.AppendSql(" WHERE ROWID   = :RID          ");

            //parameter.Add("GJYEAR", item.GJYEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENG", item.PANJENG);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_SJ_PANO item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SJ_PANO                                        ");
            parameter.AppendSql("       (GJYEAR, LTDCODE, WRTNO, BUSE, CHUKRESULT, PANJENG, GBDEL)          ");
            parameter.AppendSql("VALUES                                                                     "); 
            parameter.AppendSql("       (:GJYEAR, :LTDCODE, :WRTNO, :BUSE, :CHUKRESULT, :PANJENG, :GBDEL)   ");

            parameter.Add("GJYEAR", item.GJYEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("BUSE", item.BUSE);
            parameter.Add("CHUKRESULT", item.CHUKRESULT);
            parameter.Add("PANJENG", item.PANJENG);
            parameter.Add("GBDEL", item.GBDEL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
    }
}
