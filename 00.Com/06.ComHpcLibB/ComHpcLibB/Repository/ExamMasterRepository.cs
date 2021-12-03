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
    public class ExamMasterRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ExamMasterRepository()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gstrRetValue"></param>
        /// <returns></returns>
        /// <seealso cref="HaMain.bas> READ_Exam_Name"/>
        public EXAM_MASTER FindExamName(string gstrRetValue)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ExamFName                           ");
            parameter.AppendSql("  FROM " + ComNum.DB_MED + "EXAM_MASTER    ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND MasterCode =:MasterCode            ");
           
            parameter.Add("MasterCode", gstrRetValue.Trim());

            return ExecuteReaderSingle<EXAM_MASTER>(parameter);
        }

        public List<EXAM_MASTER> GetHeaBarcodeBySunalDtl(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXAMCODE, m.MASTERCODE, m.EXAMNAME, m.SPECCODE, m.TUBECODE, COUNT(a.EXAMCODE) AS CNT  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_BARCODE a                                                               ");
            parameter.AppendSql("      ,KOSMOS_OCS.EXAM_MASTER m                                                                ");
            parameter.AppendSql(" WHERE a.GROUPCODE IN (                                                                        ");
            parameter.AppendSql("       SELECT Code FROM KOSMOS_PMPA.HEA_SUNAPDTL WHERE WRTNO =:WRTNO)                          ");
            parameter.AppendSql("   AND a.EXAMCODE = m.MASTERCODE(+)                                                            ");
            parameter.AppendSql(" GROUP BY a.EXAMCODE,m.MASTERCODE,m.EXAMNAME,m.SPECCODE,m.TUBECODE                             ");
            parameter.AppendSql(" ORDER BY a.EXAMCODE,m.MASTERCODE,m.EXAMNAME,m.SPECCODE,m.TUBECODE                             ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<EXAM_MASTER>(parameter);
        }

        public EXAM_MASTER GetItemsByMasterCode(string argMasterCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WSCODE1,WSCODE1POS,TUBECODE,SPECCODE,RESULTIN                             ");
            parameter.AppendSql("      ,MOTHER,UNITCODE,EQUCODE1,SERIES, PIECE, GBTLA, MASTERCODE                 ");
            parameter.AppendSql("      ,KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_응급실검사', MASTERCODE) AS CHKGWA ");
            parameter.AppendSql("      ,KOSMOS_OCS.FC_WSGRP(WSCODE1) AS WSGRP_TITLE                               ");
            parameter.AppendSql("  FROM " + ComNum.DB_MED + "EXAM_MASTER                                          ");
            parameter.AppendSql(" WHERE 1 = 1                                                                     ");
            parameter.AppendSql("   AND MASTERCODE =:MASTERCODE                                                  ");

            parameter.Add("MASTERCODE", argMasterCode);

            return ExecuteReaderSingle<EXAM_MASTER>(parameter);
        }

        public List<EXAM_MASTER> GetMasterCodebyWsCode1(string sWsCode1Fr, string sWsCode1To)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MASTERCODE                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_MASTER      ");
            parameter.AppendSql(" WHERE WsCode1 > :MASTERCODEFR     ");
            parameter.AppendSql("   AND WsCode1 < :MASTERCODETO     ");

            parameter.Add("MASTERCODEFR", sWsCode1Fr.Trim());
            parameter.Add("MASTERCODETO", sWsCode1To.Trim());

            return ExecuteReader<EXAM_MASTER>(parameter);
        }

        public string GetExamNamebyMasterCode(string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TRIM(EXAMNAME) EXAMNAME             ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_MASTER              ");
            parameter.AppendSql(" WHERE MASTERCODE = :MASTERCODE            ");

            parameter.Add("MASTERCODE", argCode.Trim());

            return ExecuteScalar<string>(parameter);
        }
    }
}
