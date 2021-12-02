namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class ExamAnatmstRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ExamAnatmstRepository()
        {
        }

        public EXAM_ANATMST GetItembyRowId(string argRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, ORDERCODE, GBIO, SPECNO, ANATNO, DEPTCODE, ORDERNO    ");
            parameter.AppendSql("     , DRCODE, TO_CHAR(BDATE, 'YYYY-MM-DD') BDATE                  ");
            parameter.AppendSql("     , REMARK1, REMARK2, REMARK3, REMARK4                          ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST                                     ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                ");

            parameter.Add("RID", argRowId);

            return ExecuteReaderSingle<EXAM_ANATMST>(parameter);
        }

        public EXAM_ANATMST GetCountbyPaNo(string rECEIVEDATE, string strPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST                 ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND PTNO  = :PTNO                           ");

            parameter.Add("BDATE", rECEIVEDATE);
            parameter.Add("PTNO", strPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReaderSingle<EXAM_ANATMST>(parameter);
        }

        public List<EXAM_ANATMST> GetItembyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDate,Ptno,OrderCode,Result1,Result2,Remark5    ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST                                                     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                                ");
            parameter.AppendSql("   AND TRUNC(BDATE) = TO_DATE(:BDATE, 'YYYY-MM-DD')                                ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                                                     ");
            parameter.AppendSql("   AND GbJob = 'V'                                                                 ");
            parameter.AppendSql("   AND ResultDate IS NOT NULL                                                      ");
            parameter.AppendSql("   AND SUBSTR(AnatNo,1,1) NOT IN ('C','P')                                         ");
            parameter.AppendSql(" ORDER BY BDate,Ptno                                                               ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);

            return ExecuteReader<EXAM_ANATMST>(parameter);
        }

        public EXAM_ANATMST GetItembyPtNoJepDateOrder(string fstrPtno, string fstrJepDate, List<string> fstrMasterCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, RESULTSABUN, HRREMARK5                                                ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST                                                     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                                ");
            parameter.AppendSql("   AND TRUNC(BDATE) = TO_DATE(:BDATE, 'YYYY-MM-DD')                                ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                                                     ");
            parameter.AppendSql("   AND GbJob = 'V'                                                                 ");
            parameter.AppendSql("   AND ResultDate IS NOT NULL                                                      ");
            parameter.AppendSql("   AND MASTERCODE IN (:MASTERCODE)                                                 ");
            parameter.AppendSql(" ORDER BY BDate,Ptno                                                               ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);
            parameter.AddInStatement("MASTERCODE", fstrMasterCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<EXAM_ANATMST>(parameter);
        }



        public EXAM_ANATMST GetItemBySpecnoAnatno(string argSpecNo, string argAnatNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ORDERCODE, MASTERCODE, HRREMARK1, RESULT1   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST                     ");
            parameter.AppendSql(" WHERE SPECNO = :SPECNO                            ");
            parameter.AppendSql("   AND ANATNO  = :ANATNO                           ");
            parameter.AppendSql("   AND GBJOB ='V'                                  ");

            parameter.Add("SPECNO", argSpecNo);
            parameter.Add("ANATNO", argAnatNO);

            return ExecuteReaderSingle<EXAM_ANATMST>(parameter);
        }

        public List<EXAM_ANATMST> GetItembyOrderCodeGbJob()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, PTNO, RESULT1, RESULT2   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST                                     ");
            parameter.AppendSql(" WHERE BDATE >= TRUNC(SYSDATE - 7)                                 "); 
            parameter.AppendSql("   AND BDATE <= TRUNC(SYSDATE)                                     ");
            parameter.AppendSql("   AND ORDERCODE IN ('R2003','R2007','R2010','R371002')   ");
            parameter.AppendSql("   AND DEPTCODE = 'TO'                                             ");
            parameter.AppendSql("   AND GBJOB = 'V'                                                 ");
            parameter.AppendSql("   AND RESULTDATE IS NOT NULL                                      ");
            parameter.AppendSql(" ORDER BY BDATE, PTNO                                              ");

            return ExecuteReader<EXAM_ANATMST>(parameter);
        }

        public List<EXAM_ANATMST> GetItembyOrderCode()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, PTNO, RESULT1, RESULT2   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST                                     ");
            parameter.AppendSql(" WHERE BDate >= TRUNC(SYSDATE - 7)                                 ");
            parameter.AppendSql("   AND BDate <= TRUNC(SYSDATE)                                     ");
            parameter.AppendSql("   AND ORDERCODE IN ('R2001','R2008','R2009','R45001')    ");
            parameter.AppendSql("   AND DeptCode = 'TO'                                             ");
            parameter.AppendSql("   AND GbJob = 'V'                                                 ");
            parameter.AppendSql("   AND ResultDate IS NOT NULL                                      ");
            parameter.AppendSql(" ORDER BY BDate,Ptno                                               ");

            return ExecuteReader<EXAM_ANATMST>(parameter);
        }

        public int Insert(EXAM_ANATMST item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.EXAM_ANATMST                    ");
            parameter.AppendSql("       (PTNO, BDATE, ORDERCODE, GBIO, REMARK1, REMARK2 ");
            parameter.AppendSql("     , REMARK3, REMARK4, DEPTCODE, DRCODE)             ");
            parameter.AppendSql("VALUES                                                 ");
            parameter.AppendSql("       (:PTNO, TO_DATE(:BDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("     , :ORDERCODE, :GBIO, :REMARK1, :REMARK2           ");
            parameter.AppendSql("     , :REMARK3, :REMARK4, :DEPTCODE, :DRCODE)         ");

            parameter.Add("PTNO", item.PTNO);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("ORDERCODE", item.ORDERCODE.Trim());
            parameter.Add("GBIO", item.GBIO.Trim());
            parameter.Add("REMARK1", item.REMARK1.Trim());
            parameter.Add("REMARK2", item.REMARK2.Trim());
            parameter.Add("REMARK3", item.REMARK3.Trim());
            parameter.Add("REMARK4", item.REMARK4.Trim());
            parameter.Add("DEPTCODE", item.DEPTCODE.Trim());
            parameter.Add("DRCODE", item.DRCODE.Trim());

            return ExecuteNonQuery(parameter);
        }

        public int Update(EXAM_ANATMST item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.EXAM_ANATMST ");
            parameter.AppendSql("   SET REMARK1 = :REMARK1      ");
            parameter.AppendSql("     , REMARK2 = :REMARK2      ");
            parameter.AppendSql("     , REMARK3 = :REMARK3      ");
            parameter.AppendSql("     , REMARK4 = :REMARK4      ");
            parameter.AppendSql(" WHERE ROWID   = :RID          ");

            parameter.Add("REMARK1", item.REMARK1.Trim());
            parameter.Add("REMARK2", item.REMARK2.Trim());
            parameter.Add("REMARK3", item.REMARK3.Trim());
            parameter.Add("REMARK4", item.REMARK4.Trim());
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }
    }
}
