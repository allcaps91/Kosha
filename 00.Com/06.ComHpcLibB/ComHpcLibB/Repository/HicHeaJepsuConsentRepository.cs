using System;
using System.Collections.Generic;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;

namespace ComHpcLibB.Repository
{
    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicHeaJepsuConsentRepository : BaseRepository
    {
        public HicHeaJepsuConsentRepository()
        {
        }

        public List<HIC_HEA_JEPSU_CONSENT> GetItembyJepDate(string strFrDate, string strToDate, string strSName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.PTno, a.SName, a.WRTNO, a.Sex, a.Age                      ");
            parameter.AppendSql("     , TO_CHAR(a.SDate,'YYYY-MM-DD') SDate                         ");
            parameter.AppendSql("     , MIN(b.ASA) Asa                                              ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D10',1,0)) D10                       ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D20',1,0)) D20                       ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D30',1,0)) D30                       ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D40',1,0)) D40                       ");
            parameter.AppendSql("     , B.DeptCode                                                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU a,ADMIN.HIC_CONSENT b           ");
            parameter.AppendSql(" WHERE B.SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND B.SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                   ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                     ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                        ");
            parameter.AppendSql("   AND b.DrSabun > 0                                               ");
            parameter.AppendSql("   AND b.Consent_Time IS NOT NULL                                  ");
            parameter.AppendSql("   AND B.FORMCODE IN('D10','D20','D30','D40')                      ");
            parameter.AppendSql(" GROUP BY a.WRTNO,a.PTno,a.SName,a.Sex,a.Age,a.SDate, B.DEPTCODE   ");
            //일반검진 추가
            parameter.AppendSql(" UNION ALL                                                         ");
            parameter.AppendSql("SELECT a.PTno,a.SName,a.WRTNO,a.Sex,a.Age                          ");
            parameter.AppendSql("     , TO_CHAR(a.JEPDATE,'YYYY-MM-DD') SDate                       ");
            parameter.AppendSql("     , MIN(b.ASA) Asa                                              ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D10',1,0)) D10                       ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D20',1,0)) D20                       ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D30',1,0)) D30                       ");
            parameter.AppendSql("     , SUM(DECODE(b.FormCode,'D40',1,0)) D40                       ");
            parameter.AppendSql("     , B.DeptCode                                                  ");
            parameter.AppendSql(" FROM ADMIN.HIC_JEPSU a,ADMIN.HIC_CONSENT b            ");
            parameter.AppendSql(" WHERE a.JEPDATE>=TO_DATE(:FRDATE,'YYYY-MM-DD')                    ");
            parameter.AppendSql("   AND a.JEPDATE<=TO_DATE(:TODATE,'YYYY-MM-DD')                    ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                     ");
            }
            parameter.AppendSql(" AND a.WRTNO = b.WRTNO(+)                                          ");
            parameter.AppendSql(" AND b.DrSabun > 0                                                 ");
            parameter.AppendSql(" AND b.Consent_Time IS NOT NULL                                    ");
            parameter.AppendSql(" AND B.FORMCODE IN('D10','D20','D30','D40')                        ");
            parameter.AppendSql(" GROUP BY a.WRTNO,a.PTno,a.SName,a.Sex,a.Age,a.JEPDATE, B.DEPTCODE ");
            parameter.AppendSql("ORDER BY SName,PTno                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }

            return ExecuteReader<HIC_HEA_JEPSU_CONSENT>(parameter);
        }
    }
}
