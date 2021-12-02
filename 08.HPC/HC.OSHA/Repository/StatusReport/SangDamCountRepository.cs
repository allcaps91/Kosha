using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC_Core.Service;
using HC_OSHA.Model.StatussReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HC_OSHA.Repository.StatusReport
{
    public class SangDamCountRepository : BaseRepository
    {
        /// <summary>
        /// 상담자 일반질병 건수
        /// </summary>
        /// <param name="WRTNO"></param>
        /// <returns></returns>
        public SangDamGeneralCountModel FindOne(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT SUM(DECODE(PANJENGU1,'1','1','0')) PANJENGU1, SUM(DECODE(PANJENGR3, '1', '1', '0')) PANJENGR3,    ");
            parameter.AppendSql("SUM(DECODE(PANJENGU2, '1', '1', '0')) PANJENGU2, SUM(DECODE(PANJENGR6, '1', '1', '0')) PANJENGR6,    ");
            parameter.AppendSql("SUM(DECODE(PANJENGU3, '1', '1', '0')) PANJENGU3, SUM(DECODE(PANJENGR4, '1', '1', '0')) PANJENGR4,    ");
            parameter.AppendSql("SUM(DECODE(PANJENGR6, '1', '1', '0')) 간장질환C,    ");
            parameter.AppendSql("SUM(DECODE(PANJENGR1, '1', '1', '0')) ETC_C1,SUM(DECODE(PANJENGR2, '1', '1', '0')) ETC_C2,    ");
            parameter.AppendSql("SUM(DECODE(PANJENGR5, '1', '1', '0')) ETC_C3,SUM(DECODE(PANJENGR7, '1', '1', '0')) ETC_C4,SUM(DECODE(PANJENGR8, '1', '1', '0')) ETC_C5,    ");
            parameter.AppendSql("SUM(DECODE(PANJENGR9, '1', '1', '0')) ETC_C6,SUM(DECODE(PANJENGR12, '1', '1', '0')) ETC_C7,SUM(DECODE(PANJENGR11, '1', '1', '0')) ETC_C8,SUM(DECODE(PANJENGR10, '1', '1', '0')) ETC_C9,    ");
            parameter.AppendSql("SUM(DECODE(PANJENGU4, '1', '1', '0')) ETC_D1    ");
            parameter.AppendSql("FROM KOSMOS_PMPA.HIC_RES_BOHUM1 WHERE WRTNO = :WRTNO; ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReaderSingle<SangDamGeneralCountModel>(parameter);
        }

        public List<SangDamDnCnCountModel> FindCnDnCount(long id)
        {
              MParameter parameter = CreateParameter();
              parameter.AppendSql("              SELECT AA.WORKER_ID, SUM(DECODE(PANJEONG, 'CN', '1', '0')) CNCOUNT, SUM(DECODE(PANJEONG, 'DN', '1', '0')) DNCOUNT FROM HIC_OSHA_PANJEONG AA        ");
              parameter.AppendSql("  INNER JOIN(                                                                                                                                                    ");
              parameter.AppendSql("  SELECT D.WORKER_ID, MAX(D.YEAR) AS YEAR FROM HIC_OSHA_REPORT_DOCTOR A                                                                                          ");
              parameter.AppendSql("  INNER JOIN HIC_OSHA_HEALTHCHECK B                                                                                                                              ");
              parameter.AppendSql("  ON A.ID = B.REPORT_ID                                                                                                                                          ");
              parameter.AppendSql("  INNER JOIN HC_SITE_WORKER_VIEW C                                                                                                                               ");
              parameter.AppendSql("  ON B.WORKER_ID = C.ID                                                                                                                                          ");
              parameter.AppendSql("  INNER JOIN HIC_OSHA_PANJEONG D                                                                                                                                 ");
              parameter.AppendSql("  ON B.WORKER_ID = D.WORKER_ID                                                                                                                                   ");
              parameter.AppendSql("  WHERE A.ID = :id                                                                                                                                               ");
              parameter.AppendSql("  AND B.ISDELETED = 'N'                                                                                                                                          ");
              parameter.AppendSql("  GROUP BY D.WORKER_ID                                                                                                                                           ");
              parameter.AppendSql("  ) BB                                                                                                                                                           ");
              parameter.AppendSql("  ON AA.WORKER_ID = BB.WORKER_ID                                                                                                                                 ");
              parameter.AppendSql("  AND AA.YEAR = BB.YEAR                                                                                                                                          ");
              parameter.AppendSql("  GROUP BY AA.WORKER_ID                                                                                                                                          ");


            parameter.Add("ID", id);

            return ExecuteReader<SangDamDnCnCountModel>(parameter);
        }

        public List<SandDamTongbun> FindSpecialReport(long id,string JEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("               SELECT B.TONGBUN,    ");
            parameter.AppendSql("      SUM(DECODE(PANJENG, '5', '1', '0')) D1,  SUM(DECODE(PANJENG, '6', '1', '0')) D2  ,  SUM(DECODE(PANJENG, '3', '1', '0')) C1, SUM(DECODE(PANJENG, '7', '1', '0')) 미판정1, SUM(DECODE(PANJENG, '9', '1', '0')) 미판정2, SUM(DECODE(PANJENG, 'A', '1', '0')) 미판정3   ");
            parameter.AppendSql("      FROM KOSMOS_PMPA.HIC_SPC_PANJENG A   ");
            parameter.AppendSql("      RIGHT OUTER JOIN KOSMOS_PMPA.HIC_MCODE B   ");
            parameter.AppendSql("      ON A.MCODE = B.CODE   ");
            parameter.AppendSql("      INNER JOIN   ");
            parameter.AppendSql("      (");
            parameter.AppendSql("      SELECT MAX(D.WRTNO) as WRTNO, B.ID, B.WORKER_ID, C.PANO FROM HIC_OSHA_REPORT_DOCTOR A   ");
            parameter.AppendSql("      INNER JOIN HIC_OSHA_HEALTHCHECK B   ");
            parameter.AppendSql("      ON A.ID = B.REPORT_ID   ");
            parameter.AppendSql("      INNER JOIN HC_SITE_WORKER_VIEW C   ");
            parameter.AppendSql("      ON B.WORKER_ID = C.ID   ");
            parameter.AppendSql("      INNER JOIN HIC_JEPSU D   ");
            parameter.AppendSql("      ON C.PANO = D.PANO   ");
            parameter.AppendSql("      WHERE A.ID = :ID   ");
            parameter.AppendSql("   AND D.JEPDATE >= :JEPDATE   ");
            parameter.AppendSql("   AND B.ISDELETED = 'N'   ");
            parameter.AppendSql("   AND D.deldate is NUll   ");
            parameter.AppendSql("   AND D.ltdcode is not null   ");
            parameter.AppendSql("   AND D.GBINWON IN('21','22','23','31','32','64','65','66','67','68')   ");
            parameter.AppendSql("   AND D.GjJong IN('11','12','14','41','42','23')     ");
            parameter.AppendSql("      GROUP BY B.ID, B.WORKER_ID, C.PANO   ");
            parameter.AppendSql("      ) C   ");
            parameter.AppendSql("      ON A.WRTNO = C.WRTNO   ");
            parameter.AppendSql("      WHERE A.DELDATE IS NULL   ");
            parameter.AppendSql("      AND B.TongBun <> '16'   ");
            parameter.AppendSql("      GROUP BY B.TONGBUN   ");
            parameter.AppendSql("      ORDER BY B.TONGBUN   ");

            parameter.Add("ID", id);
            parameter.Add("JEPDATE", JEPDATE);

            return ExecuteReader<SandDamTongbun>(parameter);
        }

        /// <summary>
        /// 상태보고서 의사 일반 질병 건수 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDoctor"></param>
        /// <returns></returns>
        public SangDamGeneralCountModel FindByReport(long id, bool isDoctor, string JEPDATE)
        {
            string tableName = "HIC_OSHA_REPORT_DOCTOR";
            if (!isDoctor)
            {
                tableName = "HIC_OSHA_REPORT_NURSE";
            }

            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(DECODE(PANJENGU1,'1','1','0'))       AS PANJENGU1                      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR3, '1', '1', '0'))    AS PANJENGR3                      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGU2, '1', '1', '0'))    AS PANJENGU2                      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR6, '1', '1', '0'))    AS PANJENGR6                      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGU3, '1', '1', '0'))    AS PANJENGU3                      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR4, '1', '1', '0'))    AS PANJENGR4                      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR5, '1','1','0'))      AS PANJENGR5                      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR1, '1', '1', '0'))    AS ETC_C1                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR2, '1', '1', '0'))    AS ETC_C2                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR5, '1', '1', '0'))    AS ETC_C3                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR7, '1', '1', '0'))    AS ETC_C4                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR8, '1', '1', '0'))    AS ETC_C5                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR9, '1', '1', '0'))    AS ETC_C6                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR12, '1', '1', '0'))   AS ETC_C7                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR11, '1', '1', '0'))   AS ETC_C8                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGR10, '1', '1', '0'))   AS ETC_C9                         ");
            parameter.AppendSql("     , SUM(DECODE(PANJENGU4, '1', '1', '0'))    AS ETC_D1                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1 A                                               ");
            parameter.AppendSql("  INNER JOIN                                                                      ");
            parameter.AppendSql("  (                                                                               ");
            parameter.AppendSql("        SELECT MAX(D.WRTNO) AS WRTNO                                              ");
            parameter.AppendSql("             , B.ID                                                               ");
            parameter.AppendSql("             , B.WORKER_ID                                                        ");
            parameter.AppendSql("             , C.PANO                                                             ");
            parameter.AppendSql("          FROM " + tableName + " A                                                 ");
            parameter.AppendSql("          INNER JOIN HIC_OSHA_HEALTHCHECK B                                       ");
            parameter.AppendSql("                  ON A.ID = B.REPORT_ID                                           ");
            parameter.AppendSql("          INNER JOIN HC_SITE_WORKER_VIEW C                                        ");
            parameter.AppendSql("                  ON B.WORKER_ID = C.ID                                           ");
            parameter.AppendSql("          INNER JOIN HIC_JEPSU D                                                  ");
            parameter.AppendSql("                  ON C.PANO = D.PANO                                              ");
            parameter.AppendSql("         WHERE A.ID         = :ID                                                 ");
            parameter.AppendSql("           AND D.JEPDATE    >= :JEPDATE                                           ");
            parameter.AppendSql("           AND B.ISDELETED  = 'N'                                                 ");
            parameter.AppendSql("           AND D.DELDATE    IS NULL                                               ");
            parameter.AppendSql("           AND D.LTDCODE    IS NOT NULL                                           ");
            parameter.AppendSql("           AND D.GBINWON    IN ('21','22','23','31','32','64','65','66','67','68')");
            parameter.AppendSql("           AND D.GJJONG     IN ('11','12','14','41','42','23')                    ");
            parameter.AppendSql("        GROUP BY B.ID, B.WORKER_ID, C.PANO                                        ");
            parameter.AppendSql("  ) B                                                                             ");
            parameter.AppendSql("  ON A.WRTNO = B.WRTNO                                                            ");

            parameter.Add("ID", id);
            parameter.Add("JEPDATE", JEPDATE);

            SangDamGeneralCountModel model = ExecuteReaderSingle<SangDamGeneralCountModel>(parameter);

            parameter = CreateParameter();
            parameter.AppendSql("     select count(*) from (    SELECT COUNT(B.WORKER_ID) as COUNT FROM HIC_OSHA_REPORT_DOCTOR A  ");
            parameter.AppendSql(" INNER JOIN HIC_OSHA_HEALTHCHECK B      ");
            parameter.AppendSql(" ON A.ID = B.REPORT_ID                  ");
            parameter.AppendSql(" WHERE B.REPORT_ID = :ID                 ");
            parameter.AppendSql(" AND B.ISDELETED = 'N'                   ");
            parameter.AppendSql(" AND B.ISDOCTOR = 'Y'                   ");
            parameter.AppendSql(" AND A.ISDELETED = 'N'                   ");
            parameter.AppendSql(" GROUP BY B.WORKER_ID           )           ");
            parameter.Add("ID", id);
            long count = ExecuteScalar<long>(parameter);
            model.SANGDAMCOUNT = count;

            return model;
        }

        public List<HealthCheckDto> FindSangDamExamCount(long reportId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT WORKER_ID, bpl, bpr, bst, dan, WEIGHT, BMI, ALCHOL, SMOKE FROM HIC_OSHA_HEALTHCHECK A  ");
            parameter.AppendSql(" WHERE REPORT_ID = :ID                 ");
            parameter.AppendSql(" AND ISDELETED = 'N'                   ");
            parameter.AppendSql(" AND ISDOCTOR = 'Y'                   ");
            return ExecuteReader<HealthCheckDto>(parameter);
        }

        /// <summary>
        /// 의사 혈압측정 간이검사 건수
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public SangDamExamCountModel FindSangDamExamCount2(long reportId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT sum(decode(bpl,null, 0, 1)) as BpCount ,  sum(decode(bst, null, 0, 1)) AS BstCount, sum(decode(dan, null, 0, 1)) AS DanCount, sum(decode(BMI, null, 0, 1)) as BMICount ");
            parameter.AppendSql(" FROM HIC_OSHA_HEALTHCHECK ");
            parameter.AppendSql(" WHERE REPORT_ID = :ID                 ");
            parameter.AppendSql(" AND ISDELETED = 'N'                   ");
            parameter.AppendSql(" AND ISDOCTOR = 'Y'                   ");
            parameter.Add("ID", reportId); 
            return ExecuteReaderSingle<SangDamExamCountModel>(parameter);
        }
        public List<SangDamOutExamCountModel> FindSangDamOutExamCount(long reportId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT NAME, EXAM ");
            parameter.AppendSql(" FROM HIC_OSHA_HEALTHCHECK ");
            parameter.AppendSql(" WHERE REPORT_ID = :ID                 ");
            parameter.AppendSql(" AND ISDELETED = 'N'                   ");
            parameter.AppendSql(" AND ISDOCTOR = 'Y'                   ");
            parameter.AppendSql(" AND EXAM IS NOT NULL                   ");
            parameter.Add("ID", reportId);

            return ExecuteReader<SangDamOutExamCountModel>(parameter);
        }

        public List<SangDamVitalCountModel> FindSangDamVital(long reportId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("            SELECT COUNT(*) COUNT FROM HIC_OSHA_HEALTHCHECK        "); 
            parameter.AppendSql("WHERE REPORT_ID = :ID                                              ");
            parameter.AppendSql("AND ISDELETED = 'N'                                                ");
            parameter.AppendSql("UNION ALL                                                          ");
            parameter.AppendSql("SELECT COUNT(*) BPCOUNT FROM HIC_OSHA_HEALTHCHECK                  ");
            parameter.AppendSql("WHERE REPORT_ID =:ID                                              ");
            parameter.AppendSql("AND ISDELETED = 'N'                                                ");
            parameter.AppendSql("AND(BPL IS NOT NULL OR BPR IS NOT NULL)                            ");
            parameter.AppendSql("UNION ALL                                                          ");
            parameter.AppendSql("SELECT COUNT(*) BSTCOUNT FROM HIC_OSHA_HEALTHCHECK                 ");
            parameter.AppendSql("WHERE REPORT_ID =:ID                                               ");
            parameter.AppendSql("AND ISDELETED = 'N'                                                ");
            parameter.AppendSql("AND BST IS NOT NULL                                                ");
            parameter.AppendSql("UNION ALL                                                          ");
            parameter.AppendSql("SELECT COUNT(*) DANCOUNT FROM HIC_OSHA_HEALTHCHECK                 ");
            parameter.AppendSql("WHERE REPORT_ID = :ID                                               ");
            parameter.AppendSql("AND ISDELETED = 'N'                                                ");
            parameter.AppendSql("AND DAN IS NOT NULL                                                ");
            parameter.AppendSql("UNION ALL                                                          ");
            parameter.AppendSql("SELECT COUNT(*) BMICOUNT FROM HIC_OSHA_HEALTHCHECK                 ");
            parameter.AppendSql("WHERE REPORT_ID =:ID                                              ");
            parameter.AppendSql("AND ISDELETED = 'N'                                                ");
            parameter.AppendSql("AND BMI IS NOT NULL"                               );
            parameter.Add("ID", reportId);
            return ExecuteReader<SangDamVitalCountModel>(parameter);
        }
 
    }
}
