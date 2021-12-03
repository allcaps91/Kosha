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
    public class HcPanjengPatlistSchoolRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HcPanjengPatlistSchoolRepository()
        {
        }

        public List<HC_PANJENG_PATLIST_SCHOOL> GetPanjengPatListbyJepDate(PAN_PATLIST_SEARCH sItem)
        {
            MParameter parameter = CreateParameter();
            //HIC_SCHOOL_NEW 학년, 반, 번호는 HIC_JEPSU 테이블 이용 c.Class,c.Ban,c.Bun
            //HIC_JEPSU 초,중,고 구분 이용 > GBN 컬럼
            //HIC_SANGDAM_NEW, HIC_PATIENT 제거
            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.PANO, a.SNAME, a.SEX, a.AGE, a.WRTNO, a.LTDCODE, a.PTNO      ");
            parameter.AppendSql("     , a.CLASS, a.BAN, a.BUN, TO_CHAR(b.RDATE,'YYYY-MM-DD') RDATE, b.GBPAN, b.ROWID RID, a.SANGDAMDRNO         ");
            parameter.AppendSql("     , a.GJJONG                                                                                                ");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME                                                           ");
            parameter.AppendSql("     , ADMIN.FC_BAS_PATIENT_JUMINNO(a.PTNO) JUMIN                                                         ");
            parameter.AppendSql("     , a.GWRTNO                                                                                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU       a    --건강증진센타 접수 마스타                                             ");
            parameter.AppendSql("     , ADMIN.HIC_SCHOOL_NEW  b    --2006년 학생신체검사 new 테이블                                       ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                             ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                             ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                                       ");
            parameter.AppendSql("   AND a.GBSTS > '1'                                                                                           "); //결과입력 완료
            parameter.AppendSql("   AND a.GJJONG IN('56', '59')                                                                                 "); //학생신검만(56 : 학생 / 59 : 학교밖)
            //구강만 있는 학년은 안보이게 요청
            parameter.AppendSql("   AND a.CLASS NOT IN (2, 3, 5, 6)                                                                             ");
            if (!sItem.SNAME.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                                 ");
            }
            if (sItem.LTDCODE != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                ");
            }

            if (sItem.SABUN > 0)
            {
                parameter.AppendSql("   AND a.SANGDAMDRNO = :SANGDAMDRNO                                                                        ");                
            }

            if (sItem.JOB == "1")
            {
                parameter.AppendSql("   AND b.RDATE IS NULL                                                                                     ");
                parameter.AppendSql("   AND b.GBPAN IS NULL                                                                                     ");
            }
            else
            {
                parameter.AppendSql("   AND b.RDATE IS NOT NULL                                                                                 ");
                parameter.AppendSql("   AND b.GBPAN IS NOT NULL                                                                                 ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                                    ");
            parameter.AppendSql(" ORDER BY a.LTDCODE, a.SNAME, a.CLASS, a.BAN, a.BUN                                                            ");


            parameter.Add("FRDATE", sItem.FRDATE);
            parameter.Add("TODATE", sItem.TODATE);
            if (!sItem.SNAME.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", sItem.SNAME);
            }
            if (sItem.LTDCODE != 0)
            {
                parameter.Add("LTDCODE", sItem.LTDCODE);
            }
            if (sItem.JONG != "**" && !sItem.JONG.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", sItem.JONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            //if (sItem.PAN != "**" && !sItem.PAN.IsNullOrEmpty())
            //{
            //    parameter.Add("FIRSTPANDRNO", sItem.PAN);
            //    parameter.Add("SANGDAMDRNO", sItem.PAN);
            //    parameter.Add("PANJENGDRNO", sItem.PAN);
            //}
            //if (!sItem.PANJENGDRNO.IsNullOrEmpty())
            //{
            //    parameter.Add("SANGDAMDRNO", sItem.PANJENGDRNO);
            //}

            if (sItem.SABUN > 0)
            {
                parameter.Add("SANGDAMDRNO", sItem.SABUN);
            }

            return ExecuteReader<HC_PANJENG_PATLIST_SCHOOL>(parameter);
        }
    }
}
