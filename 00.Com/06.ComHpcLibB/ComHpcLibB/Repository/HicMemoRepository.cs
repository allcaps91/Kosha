namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicMemoRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMemoRepository()
        {
        }

        public List<HIC_MEMO> GetItembyPaNo(long nPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MEMO,TO_CHAR(ENTTIME,'YYYY-MM-DD HH24:MI') ENTTIME,PANO,PTNO        ");
            parameter.AppendSql("      ,JOBSABUN,KOSMOS_OCS.FC_INSA_MST_KORNAME(JOBSABUN) AS JOBNAME        ");
            parameter.AppendSql("      ,ROWID AS RID                                                        ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_MEMO                                                ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                        ");
            parameter.AppendSql("   AND Memo IS NOT NULL                                                    ");
            parameter.AppendSql("   AND DelDate IS NULL                                                     ");
            parameter.AppendSql(" ORDER BY ENTTIME DESC                                                     ");

            parameter.Add("PANO", nPano);
            
            return ExecuteReader<HIC_MEMO>(parameter);
        }

        public int InsertPanoMemoJobsabun(HIC_MEMO item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_MEMO           ");
            parameter.AppendSql("       (PANO, MEMO, ENTTIME, JOBSABUN, PTNO)     ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:PANO, :MEMO, SYSDATE, :JOBSABUN, :PTNO)  ");

            parameter.Add("PANO", item.PANO);
            parameter.Add("MEMO", item.MEMO);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("PTNO", item.PTNO);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteData(string argRowid, string strGbn)
        {
            MParameter parameter = CreateParameter();

            if (strGbn == "종검")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_MEMO SET    ");
            }
            else
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MEMO SET    ");
            }
            
            parameter.AppendSql("       DELDATE = SYSDATE           ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_MEMO> GetHeaItembyPaNo(string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT '종검' AS JOBGBN, MEMO, TO_CHAR(ENTTIME, 'YYYY-MM-DD HH24:MI') ENTTIME, JOBSABUN   ");
            parameter.AppendSql("       ,KOSMOS_OCS.FC_BAS_USER(JOBSABUN) AS JOBNAME,PTNO,ROWID AS RID    ");
            parameter.AppendSql("   From KOSMOS_PMPA.HEA_MEMO                                             ");
            parameter.AppendSql("  WHERE PTNO = :PTNO                                                     ");
            parameter.AppendSql("    AND DELDATE IS NULL                                                     ");
            parameter.AppendSql("  ORDER By ENTTIME DESC                                                  ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<HIC_MEMO>(parameter);
        }

        public List<HIC_MEMO> GetHicItembyPaNo(string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT '일반' AS JOBGBN, MEMO, TO_CHAR(ENTTIME, 'YYYY-MM-DD HH24:MI') ENTTIME, JOBSABUN   ");
            parameter.AppendSql("       ,KOSMOS_OCS.FC_BAS_USER(JOBSABUN) AS JOBNAME,PTNO,ROWID AS RID    ");
            parameter.AppendSql("   From KOSMOS_PMPA.HIC_MEMO                                             ");
            parameter.AppendSql("  WHERE PTNO = :PTNO                                                     ");
            parameter.AppendSql("    AND DELDATE IS NULL                                                  ");
            parameter.AppendSql("  ORDER By ENTTIME DESC                                                  ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<HIC_MEMO>(parameter);
        }

        public int Insert(HIC_MEMO item)
        {
            MParameter parameter = CreateParameter();

            if (item.JOBGBN == "종검")
            {
                parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_MEMO                           ");
                parameter.AppendSql("       (WRTNO, MEMO, ENTTIME, JOBSABUN, PTNO, GUBUN1)      ");
                parameter.AppendSql("VALUES                                                     ");
                parameter.AppendSql("       (:WRTNO, :MEMO, SYSDATE, :JOBSABUN, :PTNO, :GUBUN)  ");

                parameter.Add("WRTNO", item.WRTNO);
                parameter.Add("MEMO", item.MEMO);
                parameter.Add("JOBSABUN", item.JOBSABUN);
                parameter.Add("PTNO", item.PTNO);
                parameter.Add("GUBUN", item.GUBUN1);
            }
            else
            {
                parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_MEMO                           ");
                parameter.AppendSql("       (PANO, MEMO, ENTTIME, JOBSABUN, PTNO, GUBUN1)       ");
                parameter.AppendSql("VALUES                                                     ");
                parameter.AppendSql("       (:PANO, :MEMO, SYSDATE, :JOBSABUN, :PTNO, :GUBUN)   ");

                parameter.Add("PANO", item.PANO);
                parameter.Add("MEMO", item.MEMO);
                parameter.Add("JOBSABUN", item.JOBSABUN);
                parameter.Add("PTNO", item.PTNO);
                parameter.Add("GUBUN", item.GUBUN1);
            }
            
            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyRowId(HIC_MEMO item)
        {
            MParameter parameter = CreateParameter();

            if (item.JOBGBN == "종검")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_MEMO SET    ");
            }
            else
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MEMO SET    ");
            }

            parameter.AppendSql("       DELDATE = SYSDATE           ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        /// <summary>
        /// TODO : 종검 메모 HEA_MEMO 임시 JOIN 함 (HIC_MEMO 로 Data 이관작업 후 HIC_MEMO 로 통일할것
        /// </summary>
        /// <param name="argPtno"></param>
        /// <returns></returns>
        public List<HIC_MEMO> GetItembyPaNo(string argPtno, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT X.JOBGBN, X.MEMO, X.ENTTIME, X.JOBSABUN, X.JOBNAME, X.PTNO, X.RID       ");
            parameter.AppendSql("  FROM (                                                                       ");

            parameter.AppendSql("       SELECT '일반' AS JOBGBN, MEMO, TO_CHAR(ENTTIME, 'YYYY-MM-DD HH24:MI') ENTTIME, JOBSABUN   ");
            parameter.AppendSql("             ,KOSMOS_OCS.FC_BAS_USER(JOBSABUN) AS JOBNAME,PTNO,ROWID AS RID    ");
            parameter.AppendSql("         From KOSMOS_PMPA.HIC_MEMO                                             ");
            parameter.AppendSql("        WHERE PTNO = :PTNO                                                     ");
            parameter.AppendSql("          AND DELDATE IS NULL                                                  ");
            if (argGubun =="D")
            {
                parameter.AppendSql("          AND GUBUN1 = 'Y'                                                  ");
            }
            else if (argGubun == "N")
            {
                parameter.AppendSql("          AND GUBUN1 IS NULL                                                ");
            }

            parameter.AppendSql(" UNION ALL                                                                     ");

            parameter.AppendSql("       SELECT '종검' AS JOBGBN, MEMO, TO_CHAR(ENTTIME, 'YYYY-MM-DD HH24:MI') ENTTIME, JOBSABUN   ");
            parameter.AppendSql("             ,KOSMOS_OCS.FC_BAS_USER(JOBSABUN) AS JOBNAME,PTNO,ROWID AS RID    ");
            parameter.AppendSql("         From KOSMOS_PMPA.HEA_MEMO                                             ");
            parameter.AppendSql("        WHERE PTNO = :PTNO                                                     ");
            parameter.AppendSql("          AND DELDATE IS NULL                                                  ");
            if (argGubun == "D")
            {
                parameter.AppendSql("          AND GUBUN1 = 'Y'                                                  ");
            }
            else if (argGubun == "N")
            {
                parameter.AppendSql("          AND GUBUN1 IS NULL                                                ");
            }

            parameter.AppendSql("       ) X                                                                     ");

            parameter.AppendSql(" ORDER BY X.ENTTIME DESC                                                       ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<HIC_MEMO>(parameter);
        }
    }
}
