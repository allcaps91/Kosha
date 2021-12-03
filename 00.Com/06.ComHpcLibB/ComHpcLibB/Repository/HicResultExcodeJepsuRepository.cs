namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultExcodeJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultExcodeJepsuRepository()
        {
        }

        public List<HIC_RESULT_EXCODE_JEPSU> GetItembyWrtNo(string fstrGjChasu, long fnPaNo, long fnWrtno, long fnWrtno1, long fnWrtno2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng                    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b, ADMIN.HIC_JEPSU c     ");
            parameter.AppendSql(" WHERE c.PANO = :PANO                                                                  ");
            if (fstrGjChasu == "2")
            {
                parameter.AppendSql("   AND c.WRTNO IN (:WRTNO1, :WRTNO2)                                               ");
            }
            else
            {
                parameter.AppendSql("   AND c.WRTNO = :WRTNO                                                            ");
            }
            parameter.AppendSql("   AND c.WRTNO = a.WRTNO(+)                                                            ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                            ");
            parameter.AppendSql(" ORDER BY a.Panjeng,a.WRTNO,a.Part,a.ExCode                                            ");

            parameter.Add("PANO", fnPaNo);             
            if (fstrGjChasu == "2")
            {
                parameter.Add("WRTNO1", fnWrtno1);
                parameter.Add("WRTNO2", fnWrtno2);
            }
            else
            {
                parameter.Add("WRTNO", fnWrtno);
            }

            return ExecuteReader<HIC_RESULT_EXCODE_JEPSU>(parameter);
        }

        public List<HIC_RESULT_EXCODE_JEPSU> GetItembyOnlyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ExCode,b.ENDOGUBUN1,b.ENDOGUBUN2,b.ENDOGUBUN3,b.ENDOGUBUN4,b.ENDOGUBUN5, C.PTNO, C.JEPDATE    ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b, ADMIN.HIC_JEPSU C                     ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                                ");
            parameter.AppendSql("   AND a.WRTNO = C.WRTNO                                                                               ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                                                               ");
            parameter.AppendSql("   AND b.Deldate IS NULL                                                                               ");
            parameter.AppendSql("   AND (b.ENDOGUBUN2='Y' OR b.ENDOGUBUN3='Y' OR b.ENDOGUBUN4='Y' OR b.ENDOGUBUN5='Y')                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE_JEPSU>(parameter);
        }

        public List<HIC_RESULT_EXCODE_JEPSU> GetItemByWrtNo1(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT  GjYear,b.ExCode,b.ResCode,b.Result,a.Sex,c.HName,c.GbCodeUse,c.ResultType,c.Unit              ");
            parameter.AppendSql(" ,c.Min_M,c.Max_M,c.Min_F,c.Max_F                                                                      ");
            parameter.AppendSql(" FROM HIC_JEPSU a,HIC_RESULT b, HIC_EXCODE c                                                           ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                                ");
            parameter.AppendSql(" AND a.WRTNO = b.WRTNO(+)                                                                              ");
            parameter.AppendSql(" AND b.ExCode=c.Code(+)                                                                                ");
            parameter.AppendSql(" AND b.Result <> '미실시'                                                                               ");
            parameter.AppendSql(" AND b.Result <> '.'                                                                                   ");
            parameter.AppendSql(" AND c.HName NOT LIKE '%(ACT)'                                                                         ");
            parameter.AppendSql(" ORDER BY b.Part,b.ExCode                                                                              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE_JEPSU>(parameter);
        }

        public List<HIC_RESULT_EXCODE_JEPSU> GetItemByWrtNo2(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT  A.GJYEAR,B.EXCODE,C.HNAME,B.RESCODE,B.RESULT,C.UNIT,C.EXAMBUN,C.EXAMFRTO  ");
            parameter.AppendSql(" FROM HIC_JEPSU A,HIC_RESULT B, HIC_EXCODE C                                       ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql(" AND A.WRTNO = B.WRTNO(+)                                                          ");
            parameter.AppendSql(" AND B.RESULT NOT IN ('미실시','.')                                                 ");
            parameter.AppendSql(" AND B.EXCODE NOT IN ('A215')                                                      ");
            parameter.AppendSql(" AND B.EXCODE = C.CODE                                                             ");
            parameter.AppendSql(" AND B.RESULT NOT IN ('미실시','.')                                                 ");
            parameter.AppendSql(" AND C.PART NOT IN ('5','9')                                                       ");
            parameter.AppendSql(" ORDER BY C.EXAMBUN,C.HEASORT                                                      ");


            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE_JEPSU>(parameter);
        }
    }
}
