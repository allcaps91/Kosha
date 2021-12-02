namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvExamPatientRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvExamPatientRepository()
        {
        }

        public List<HEA_RESV_EXAM_PATIENT> GetItembyRTime(string strBDateFr, string strBDateTo, long nSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, a.PANO, a.SNAME, b.SEX                     ");
            parameter.AppendSql("     , b.PTNO, b.JUSO1 || b.JUSO2 JUSO                                                 ");
            parameter.AppendSql("     , b.JUMIN2, DECODE(a.AMPM, 'A', '1', 'P', '2', '1') as AMPM2                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM a                                                     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT   b                                                     ");
            parameter.AppendSql(" WHERE a.RTIME >= TO_DATE(:SDATEFR, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.RTIME <  TO_DATE(:SDATETO, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.EXCODE IN('TX20', 'TX32', 'TX41', 'TX64')                                     ");
            if (nSabun != 36540)
            {
                parameter.AppendSql("   AND a.Pano <> 999                                                               ");
            }
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                               ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                                              ");
            parameter.AppendSql(" GROUP By a.SDATE, a.PANO, a.SNAME, b.SEX, b.PTNO, b.JUSO1, b.JUSO2, b.JUMIN2, a.AMPM  ");
            parameter.AppendSql(" ORDER BY a.AMPM, a.SNAME                                                              ");  

            parameter.Add("SDATEFR", strBDateFr);
            parameter.Add("SDATETO", strBDateTo);

            return ExecuteReader<HEA_RESV_EXAM_PATIENT>(parameter);
        }

        public List<HEA_RESV_EXAM_PATIENT> GetListByLtdCode(long argLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.LTDCODE, TO_CHAR(a.RTIME,'YYYY-MM-DD') SDATE, GBEXAM  ");
            parameter.AppendSql("     , SUM(DECODE(a.AMPM, 'A', 1, 1)) AS AMCNT                 ");
            parameter.AppendSql("     , SUM(DECODE(a.AMPM, 'A', 0, 1)) AS PMCNT                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM a                             ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT   b                             ");
            parameter.AppendSql(" WHERE a.RTIME >= TRUNC(SYSDATE)                               ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND a.PANO = b.PANO(+)                                      ");
            parameter.AppendSql("   AND b.LTDCODE =:LTDCODE                                     ");
            parameter.AppendSql(" GROUP By b.LTDCODE, TO_CHAR(a.RTIME, 'YYYY-MM-DD'), GBEXAM    ");

            parameter.Add("LTDCODE", argLtdCode);

            return ExecuteReader<HEA_RESV_EXAM_PATIENT>(parameter);
        }

        public HEA_RESV_EXAM_PATIENT GetItembyPtnoExam(string argPtno, string argExam)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM A,        ");
            parameter.AppendSql(" KOSMOS_PMPA.HIC_PATIENT B                 ");                     
            parameter.AppendSql(" WHERE A.PANO = B.PANO                     ");
            parameter.AppendSql(" AND B.Ptno= :PTNO                         ");
            parameter.AppendSql(" AND A.RTIME >=TRUNC(SYSDATE)              ");
            parameter.AppendSql(" AND A.RTIME < TRUNC(SYSDATE)+1            ");
            parameter.AppendSql(" AND A.DelDate IS NULL                     ");
            parameter.AppendSql(" AND A.GBEXAM = :GBEXAM                    ");
            parameter.AppendSql(" AND RTIME <> TRUNC(SYSDATE)               ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("GBEXAM", argExam);

            return ExecuteReaderSingle<HEA_RESV_EXAM_PATIENT>(parameter);
        }

    }
}
