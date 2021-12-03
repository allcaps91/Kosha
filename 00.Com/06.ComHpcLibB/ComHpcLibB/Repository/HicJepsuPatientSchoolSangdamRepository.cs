namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuPatientSchoolSangdamRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientSchoolSangdamRepository()
        {
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL_SANGDAM> GetItembyJepDate(HIC_JEPSU_PATIENT_SCHOOL_SANGDAM item, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.Pano,a.SName,a.Sex,a.Age,a.WRTNO,a.LtdCode,a.PTNO                     ");
            parameter.AppendSql("     , b.Jumin2,c.Class,c.Ban,c.Bun,TO_CHAR(c.RDate,'YYYY-MM-DD') RDate,c.GbPan,c.ROWID,d.SangDamDrno                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c, ADMIN.HIC_SANGDAM_NEW d ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                     ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                     ");
            parameter.AppendSql("   AND a.WRTNO = d.WRTNO                                                                                               ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                                               ");
            parameter.AppendSql("   AND a.GBSTS > '1'                                                                                                   ");//결과입력 완료
            parameter.AppendSql("   AND a.GjJong='56'                                                                                                   ");//학생신검만
            //2012-05-02  구강만 있는 학년은 안보이게 요청
            parameter.AppendSql("   AND a.Class NOT IN (2,3,5,6)                                                                                        ");
            if (!item.SNAME.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                                         ");
            }
            if (item.LTDCODE != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                        ");
            }
            if (item.SANGDAMDRNO > 0)
            {
                if (item.SANGDAMDRNO == 86651)  //정혜진 과장은 박지원과장 상담자 포함
                {
                    parameter.AppendSql("   AND ( d.SANGDAMDRNO = :SANGDAMDRNO OR d.SangDamDrno=84571 )                                                 ");
                }
                else if (item.SANGDAMDRNO == 22977) //김주호과장은 정해진,김태완과장외 모두 보이게
                {
                    parameter.AppendSql("   AND d.SangDamDrno NOT IN (86651,70686)                                                                      ");
                }
                else
                {
                    parameter.AppendSql("   AND d.SANGDAMDRNO = :SANGDAMDRNO                                                                            ");
                }
            }
            if (strGubun == "1")
            {
                parameter.AppendSql("   AND c.RDATE IS NULL                                                                                             ");
                parameter.AppendSql("   AND c.GBPAN IS NULL                                                                                             ");
            }
            else
            {
                parameter.AppendSql("   AND c.RDATE IS NOT NULL                                                                                         ");
                parameter.AppendSql("   AND c.GBPAN IS NOT NULL                                                                                         ");
            }
            parameter.AppendSql("   AND a.Pano  = b.Pano(+)                                                                                             ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                                            ");
            parameter.AppendSql(" ORDER BY a.LtdCode,a.SName,c.Class,c.Ban,c.Bun                                                                        ");

            parameter.Add("FRDATE", item.JEPFRDATE);
            parameter.Add("TODATE", item.JEPTODATE);

            if (!item.SNAME.IsNullOrEmpty())
            {
                parameter.Add("SNAME", item.SNAME);
            }
            if (item.LTDCODE != 0)
            {
                parameter.Add("LTDCODE", item.LTDCODE);
            }
            if (item.SANGDAMDRNO != 0)
            {
                parameter.Add("SANGDAMDRNO", item.SANGDAMDRNO);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL_SANGDAM>(parameter);
        }
    }
}
