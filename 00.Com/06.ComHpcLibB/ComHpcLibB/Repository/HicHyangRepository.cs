namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicHyangRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicHyangRepository()
        {
        }

        public int UpdatebyRowId(string rOWID, long nWrtNo, string strSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_HYANG SET       ");
            parameter.AppendSql("       DELDATE = SYSDATE               ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                 ");
            if (!strSuCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SUCODE   = :SUCODE          ");
            }
            parameter.AppendSql("   AND DELDATE IS NULL                 ");

            parameter.Add("WRTNO", nWrtNo);
            if (!strSuCode.IsNullOrEmpty())
            {
                parameter.Add("SUCODE", strSuCode.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public int UpdateQtybyRowId(string argRowid, long argQty, string argRealQty, double argEntQty, double argEntQty2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_HYANG SET       ");
            parameter.AppendSql("       QTY = :QTY                      ");
            parameter.AppendSql("       ,REALQTY = :REALQTY             ");
            parameter.AppendSql("       ,ENTQTY2 = :ENTQTY2             ");
            parameter.AppendSql("       ,ENTQTY  = :ENTQTY              ");
            parameter.AppendSql(" WHERE ROWID    = :RID                 ");

            parameter.Add("RID", argRowid);
            parameter.Add("QTY", argQty); 
            parameter.Add("REALQTY", argRealQty);
            parameter.Add("ENTQTY2", argEntQty2);
            parameter.Add("ENTQTY", argEntQty);


            return ExecuteNonQuery(parameter);
        }

        public int UpdateQtybyRowId(double nQty, string sRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_HYANG SET       ");
            parameter.AppendSql("       DELDATE = ''                    ");
            parameter.AppendSql("     , QTY     = :QTY                  ");
            parameter.AppendSql("     , REALQTY = :REALQTY              ");
            parameter.AppendSql(" WHERE ROWID   = :RID                  ");

            parameter.Add("QTY", nQty);
            parameter.Add("REALQTY", nQty);
            parameter.Add("RID", sRowId);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateDelDatebyWrtNo(long argWrtNo, string strSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_HYANG SET       ");
            parameter.AppendSql("       DELDATE  = SYSDATE              ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                 ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE              ");
            //parameter.AppendSql("   AND DELDATE IS NULL                 ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SUCODE", strSuCode.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSelectbyWorId(HIC_HYANG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_HYANG                                                  ");
            parameter.AppendSql("       (PTNO, WRTNO, SNAME, JONG, BI, WARDCODE, BDATE, SDATE, ENTDATE              ");
            parameter.AppendSql("      , DEPTCODE, DRSABUN, IO, SUCODE, QTY, REALQTY, NAL, DOSCODE                  ");
            parameter.AppendSql("      , REMARK1, REMARK2, SEX, AGE, JUMIN, JUSO, ENTQTY, ENTQTY2, JUMIN2, CHASU)   ");
            parameter.AppendSql(" SELECT PTNO, WRTNO, SNAME, JONG, '51', DEPTCODE, BDATE, SDATE,  ENTDATE           ");
            parameter.AppendSql("      , DEPTCODE, DRSABUN, 'O', SUCODE, :QTY, :REALQTY, '1', '920299'              ");
            parameter.AppendSql("      , '°Ë»ç¿ë', 'Pain', SEX, AGE, JUMIN, JUSO, ENTQTY, ENTQTY2, JUMIN2, CHASU     ");
            parameter.AppendSql("   FROM ADMIN.HIC_HYANG_APPROVE                                              ");
            parameter.AppendSql("  WHERE ROWID = :RID                                                               ");

            parameter.Add("QTY", item.QTY); 
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_HYANG GetRowIdbyWrtNoSuCode(long argWrtNo, string strSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID FROM ADMIN.HIC_HYANG        ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                         ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                      ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_HYANG>(parameter);
        }

        public long GetSeqNobyBDate(string strSysDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(SeqNo) + 1 SEQNO                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_HYANG                   ");
            parameter.AppendSql(" WHERE BDATE >= TO_DATE(:BDATE, 'YYYY-MM-DD')  ");

            parameter.Add("BDATE", strSysDate);

            return ExecuteScalar<long>(parameter);
        }

        public string GetRowIdByItems(string argBdate, long argWrtno, string argPtno, string argSname, string argSucode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID FROM ADMIN.HIC_HYANG     ");
            parameter.AppendSql(" WHERE BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND SNAME = :SNAME                              ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                            ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", argSname);
            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateDelDateByWrtnoPtnoSnameSucodes(long argWrtNo, string argPtno, string argSname, List<string> argSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_HYANG SET       ");
            parameter.AppendSql("       DELDATE  = SYSDATE              ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND PTNO     = :PTNO                ");
            parameter.AppendSql("   AND SNAME    = :SNAME               ");
            parameter.AppendSql("   AND SNAME    = :SNAME               ");
            if (!argSucode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SUCODE NOT IN (:SUCODE)         ");
            }
            parameter.AppendSql("   AND DELDATE IS NULL                 ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("PTNO", argPtno); 
            parameter.Add("SNAME", argSname);
            if (!argSucode.IsNullOrEmpty())
            {
                parameter.AddInStatement("SUCODE", argSucode);
            }
                
            return ExecuteNonQuery(parameter);
        }

        
    }
}
