namespace HC.Core.Repository
{
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Model;
    using HC.Core.Service;

    /// <summary>
    /// 일반검진 수검자
    /// </summary>
    public class HealthCarePatientRepository : BaseRepository
    {
        public PatientModel FindOne(long siteId, string pano, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT A.PANO , D.PTNO, A.SNAME AS NAME, D.JUMIN2 as JUMIN,  A.TEL, to_char(A.IpsaDate, 'YYYY-MM-DD') IPSADATE, A.BuseName AS DEPT   ");
            parameter.AppendSql("   FROM HIC_JEPSU A   ");
            parameter.AppendSql("   INNER JOIN HC_SITE_VIEW C   ");
            parameter.AppendSql("   ON A.LTDCODE = C.ID   ");
            parameter.AppendSql("   RIGHT OUTER JOIN HIC_PATIENT D   ");
            parameter.AppendSql("   ON A.PANO = D.PANO   ");
            parameter.AppendSql("   WHERE C.ID = :SITEID   ");
            parameter.AppendSql("   AND A.JepDate >= TO_DATE(:startDate, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND A.JepDate <= TO_DATE(:endDate, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND A.PANO = :PANO   ");
            parameter.AppendSql("   AND A.DELDATE IS NULL   ");
            
            parameter.AppendSql("   GROUP BY  A.PANO, D.PTNO, A.SNAME, D.JUMIN2, A.TEL, to_char(A.IpsaDate, 'YYYY-MM-DD'),  A.BuseName   ");

            parameter.Add("SITEID", siteId);
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("PANO", pano);

            return ExecuteReaderSingle<PatientModel>(parameter);
        }

        public List<PatientModel> FindAll(long siteId, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT A.PANO , D.PTNO, A.SNAME AS NAME, D.JUMIN2 as JUMIN,  A.TEL, to_char(A.IpsaDate, 'YYYY-MM-DD') IPSADATE, A.BuseName AS DEPT   ");
            parameter.AppendSql("   FROM HIC_JEPSU A   ");
            parameter.AppendSql("   INNER JOIN HC_SITE_VIEW C   ");
            parameter.AppendSql("   ON A.LTDCODE = C.ID   ");
            parameter.AppendSql("   RIGHT OUTER JOIN HIC_PATIENT D   ");
            parameter.AppendSql("   ON A.PANO = D.PANO   ");
            parameter.AppendSql("   WHERE C.ID = :SITEID   ");
            parameter.AppendSql("   AND A.JepDate >= TO_DATE(:startDate, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND A.JepDate <= TO_DATE(:endDate, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND A.DELDATE IS NULL   ");
            parameter.AppendSql("   GROUP BY  A.PANO, D.PTNO, A.SNAME, D.JUMIN2, A.TEL, to_char(A.IpsaDate, 'YYYY-MM-DD'),  A.BuseName   ");

            parameter.Add("SITEID", siteId);
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);

            return ExecuteReader<PatientModel>(parameter);
        }
        /// <summary>
        /// 일반검진 판정완료된 수검자 목록
        /// 1차 검사에서 a101(키), a102(몸무게) 결과 없는 사람은 제외함
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="year"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<HealthCarePatientModel> FindAllComplete(long siteId, string year, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,b.Jumin2,TO_CHAR(a.JepDate, 'YYYY-MM-DD') JepDate,a.Sex,a.Age ");
            parameter.AppendSql("  FROM HIC_JEPSU a ");
            parameter.AppendSql("       RIGHT OUTER JOIN HIC_PATIENT b ");
            parameter.AppendSql("             ON a.pano = b.pano ");
            parameter.AppendSql("       LEFT  OUTER JOIN HIC_RESULT C ");
            parameter.AppendSql("             ON A.WRTNO = C.WRTNO ");
            parameter.AppendSql("             AND C.EXCODE IN('A101','A102') ");
            parameter.AppendSql(" WHERE a.JepDate  >= TO_DATE( :startDate , 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE( :endDate, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND a.DelDate IS NULL ");
            parameter.AppendSql("   AND a.GjYear = :year ");
            parameter.AppendSql("   AND a.LtdCode = :siteId ");
            parameter.AppendSql("   AND a.GjJong IN('11','21','22','23','24','25','26','30','41') ");
            parameter.AppendSql("   AND a.PanjengDate IS NOT NULL "); // 판정완료
            parameter.AppendSql("  GROUP BY a.WRTNO ,a.SNAME, Jumin2, TO_CHAR(a.JepDate, 'YYYY-MM-DD'), a.Sex, a.age ");
            parameter.AppendSql("  ORDER BY a.SName,a.Age ");

            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("year", year);
            parameter.Add("siteId", siteId);

            return ExecuteReader<HealthCarePatientModel>(parameter);
        }
        /// <summary>
        /// 종합검진 수검자 자료(판정완료)
        /// 1차 검사에서 a101(키), a102(몸무게) 결과 없는 사람은 제외함
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="year"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<HealthCarePatientModel> FindAllToalComplete(long siteId, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,b.Jumin2,TO_CHAR(a.SDate, 'YYYY-MM-DD')  as JEPDATE ,a.Sex,a.Age ");
            parameter.AppendSql("  FROM HEA_JEPSU a ");
            parameter.AppendSql("       RIGHT OUTER JOIN HIC_PATIENT b ");
            parameter.AppendSql("             ON a.pano = b.pano ");
            parameter.AppendSql("       LEFT  OUTER JOIN HEA_RESULT C ");
            parameter.AppendSql("             ON A.WRTNO = C.WRTNO ");
            parameter.AppendSql("             AND C.EXCODE IN('A101','A102') ");
            parameter.AppendSql(" WHERE a.SDate  >= TO_DATE( :startDate , 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND a.SDate <= TO_DATE( :endDate, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND a.DelDate IS NULL ");
            parameter.AppendSql("   AND a.LtdCode =  :siteId ");
            parameter.AppendSql("   AND a.GjJong IN('11','21','22','23','24','25','26','30','41') ");
            parameter.AppendSql("   AND a.DrSabun > 0 "); // 판정완료
            parameter.AppendSql(" GROUP BY a.WRTNO ,a.SNAME, Jumin2, TO_CHAR(a.SDate, 'YYYY-MM-DD'), a.Sex, a.age ");
            parameter.AppendSql(" ORDER BY a.SName,a.Age ");

            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("siteId", siteId);

            return ExecuteReader<HealthCarePatientModel>(parameter);
        }
    }
}

