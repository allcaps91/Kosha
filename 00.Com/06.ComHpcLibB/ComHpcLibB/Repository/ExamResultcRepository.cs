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
    public class ExamResultcRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ExamResultcRepository()
        {
        }

        public string ChkExcuteExamByPanoCode(string argPano, string argSpecNo, string argMasterCode, string argSubCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_RESULTC  ");
            parameter.AppendSql(" WHERE PANO =:PANO     ");
            if (!argSubCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND ( MASTERCODE IN (:SUBCODE) OR MASTERCODE =:MASTERCODE )                ");
            }
            else
            {
                parameter.AppendSql("   AND MASTERCODE =:MASTERCODE               ");
            }
            parameter.AppendSql("   AND SPECNO IN (:SPECNO)               ");

            parameter.Add("PANO", argPano);
            parameter.Add("MASTERCODE", argMasterCode);
            
            if (!argSubCode.IsNullOrEmpty())
            {
                parameter.Add("SUBCODE", argSubCode);
            }
            parameter.Add("SPECNO", argSpecNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRowidByMstCodeSpecNoIN(string strPtno, string strMasterCode, List<string> lstSpecno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                       ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_RESULTC     ");
            parameter.AppendSql(" WHERE PANO =:PANO                 ");
            parameter.AppendSql("   AND MASTERCODE =:MASTERCODE     ");
            parameter.AppendSql("   AND SPECNO IN (:SPECNO)         ");

            parameter.Add("PANO", strPtno);
            parameter.Add("MASTERCODE", strMasterCode);
            parameter.AddInStatement("SPECNO", lstSpecno);

            return ExecuteScalar<string>(parameter);
        }

        public List<EXAM_RESULTC> GetMasterCodebySpecNoMasterCode(string sSpecNo, string sMasterCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MASTERCODE                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_RESULTC     ");
            parameter.AppendSql(" WHERE SPECNO = :SPECNO            ");
            parameter.AppendSql("   AND MASTERCODE =:MASTERCODE     ");

            parameter.Add("MASTERCODE", sMasterCode);
            parameter.Add("SPECNO", sSpecNo);

            return ExecuteReader<EXAM_RESULTC>(parameter);
        }

        public List<EXAM_RESULTC> GetABODataBySpecNo(long argWRTNO, string fstrBDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUBCODE,RESULT                              ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_RESULTC                     ");
            parameter.AppendSql(" WHERE SPECNO IN (                                 ");
            parameter.AppendSql("       SELECT SPECNO FROM KOSMOS_OCS.EXAM_SPECMST  ");
            parameter.AppendSql("        WHERE HICNO=:HICNO                         ");
            parameter.AppendSql("          AND BDATE=TO_DATE(:BDATE,'YYYY-MM-DD')   ");
            parameter.AppendSql("          AND STATUS IN ('05')                     ");
            parameter.AppendSql("          AND BI='61'                              ");
            parameter.AppendSql("       )                                           ");
            parameter.AppendSql("   AND SUBCODE IN ('BB01','BB05')                  ");
            parameter.AppendSql("   AND STATUS='V'                                  ");
            parameter.AppendSql(" GROUP BY SUBCODE,RESULT                           ");

            parameter.Add("HICNO", argWRTNO);
            parameter.Add("BDATE", fstrBDate);

            return ExecuteReader<EXAM_RESULTC>(parameter);
        }

        public List<EXAM_RESULTC> GetListBySpecno(string argSpecNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT R.SEQNO, R.SUBCODE, R.RESULT,M.EXAMNAME ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_RESULTC R               ");
            parameter.AppendSql("      ,KOSMOS_OCS.EXAM_MASTER M                ");
            parameter.AppendSql(" WHERE R.SPECNO = :SPECNO                      ");
            parameter.AppendSql("   AND R.STATUS = 'V'                          ");     //검사실에서 확인한것만
            parameter.AppendSql("   AND R.RESULTWS NOT IN ('A','T')             ");     //세포,조직은 제외
            parameter.AppendSql("   AND R.SUBCODE = M.MASTERCODE(+)             ");
            parameter.AppendSql(" ORDER BY R.SEQNO                              ");

            parameter.Add("SPECNO", argSpecNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<EXAM_RESULTC>(parameter);
        }

        public int InsertData(EXAM_RESULTC item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_OCS.EXAM_RESULTC (                                             ");
            parameter.AppendSql("        SPECNO,RESULTWS,EQUCODE,SEQNO,PANO,MASTERCODE,SUBCODE,UNIT,STATUS          ");
            parameter.AppendSql(" ) VALUES (                                                                        ");
            parameter.AppendSql("       :SPECNO,:RESULTWS,:EQUCODE,:SEQNO,:PANO,:MASTERCODE,:SUBCODE,:UNIT,:STATUS  ");

            #region Query 변수대입
            parameter.Add("SPECNO",     item.SPECNO);
            parameter.Add("RESULTWS",   item.RESULTWS); 
            parameter.Add("EQUCODE",    item.EQUCODE);  
            parameter.Add("SEQNO",      item.SEQNO);    
            parameter.Add("PANO",       item.PANO);
            parameter.Add("MASTERCODE", item.MASTERCODE);
            parameter.Add("SUBCODE",    item.SUBCODE);  
            parameter.Add("UNIT",       item.UNIT);     
            parameter.Add("STATUS",     item.STATUS);   
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public EXAM_RESULTC GetResultbyPTNO_RESULT_B형간염(long ArgWRTNO, string ArgPTNO,  string ArgExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT B.RESULT FROM KOSMOS_OCS.EXAM_SPECMST A, KOSMOS_OCS.EXAM_RESULTC B         ");
            parameter.AppendSql(" WHERE A.PANO = :WRTNO                                                             ");
            parameter.AppendSql(" AND B.RESULT IS NOT NULL                                                          ");
            parameter.AppendSql(" AND A.SPECNO = B.SPECNO                                                           ");
            parameter.AppendSql(" AND A.PANO = B.PANO                                                               ");
            parameter.AppendSql(" AND B.MASTERCODE = :ArgExCode                                                     ");
            parameter.AppendSql(" AND B.STATUS = 'V'                                                                ");
            parameter.AppendSql(" ORDER BY A.BDATE DESC                                                             ");


            parameter.Add("WRTNO", ArgWRTNO);
            parameter.Add("PTNO", ArgPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", ArgExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);


            return ExecuteReaderSingle<EXAM_RESULTC>(parameter);
        }
    }
}
