
namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComBase.Controls;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicXMunjinJepsuPatientRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicXMunjinJepsuPatientRepository()
        {
        }


        public List<HIC_X_MUNJIN_JEPSU_PATIENT> GetListByItems(string argFDate, string argTDate, string argSname, string argWrtno, string argLtdcode, string argRePrt, string argSort, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT B.PANO, B.SNAME, B.JUSO1||'  '||B.JUSO2 AS JUSO                                                        ");
            parameter.AppendSql(" ,SUBSTR(C.JUMIN,1,6)||'-'||SUBSTR(C.JUMIN,7,7) AS JUMIN, C.SABUN , B.GJJONG                                    ");
            parameter.AppendSql(" ,B.LTDCODE, TO_CHAR(B.JEPDATE,'YYYY-MM-DD') JEPDATE , B.AGE, B.SEX , B.WRTNO,UCODES                            ");
            parameter.AppendSql(" ,TO_CHAR(A.PANJENGDATE,'YYYY-MM-DD') PANJENGDATE, TO_CHAR(A.TONGBODATE,'YYYY-MM-DD') TONGBODATE                ");
            parameter.AppendSql(" FROM HIC_X_MUNJIN A,HIC_JEPSU B , HIC_PATIENT C                                                               ");
            parameter.AppendSql(" WHERE A.JEPDATE>=TO_DATE(:FDATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("   AND A.JEPDATE<=TO_DATE(:TDATE, 'YYYY-MM-DD')                                                                ");
            parameter.AppendSql("   AND A.WRTNO=B.WRTNO(+)                                                                                      ");
            parameter.AppendSql("   AND A.PANJENGDATE IS NOT NULL                                                                               ");
            parameter.AppendSql("   AND A.STS ='Y'                                                                                              ");
            parameter.AppendSql("   AND B.PANO = C.PANO(+)                                                                                      ");


            if (argGubun == "PAPER")
            {
                parameter.AppendSql("   AND B.WEBPRINTREQ IS NULL                                                                                   ");
                if (argRePrt == "OK")
                {
                    parameter.AppendSql("AND A.GBPRINT='Y'                                          ");
                }
                else
                {
                    parameter.AppendSql("AND (A.GBPRINT IS NULL OR A.GBPRINT<>'Y')                  ");
                }
            }
            else if (argGubun == "PDF")
            {
                parameter.AppendSql("   AND B.WEBPRINTREQ IS NOT NULL                                                                               ");
                if (argRePrt == "OK")
                {
                    parameter.AppendSql("AND B.WEBPRINTSEND IS NULL                                 ");
                }
                else
                {
                    parameter.AppendSql("AND B.WEBPRINTSEND IS NOT NULL                             ");
                }
            }


            if (!argSname.IsNullOrEmpty()) { parameter.AppendSql("AND B.SNAME LIKE :SNAME       "); }
            if (!argSname.IsNullOrEmpty()) { parameter.AppendSql("AND B.WRTNO = :WRTNO          "); }
            if (!argLtdcode.IsNullOrEmpty()) { parameter.AppendSql("AND B.LTDCODE = :LTDCODE    "); }

            parameter.AppendSql("   ORDER BY b.SName                                                                                            ");
               

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            if (!argSname.IsNullOrEmpty()) { parameter.AddLikeStatement("SNAME", argSname); }
            if (!argSname.IsNullOrEmpty()) { parameter.Add("WRTNO", argWrtno); }
            if (!argLtdcode.IsNullOrEmpty()) { parameter.Add("LTDCODE", argLtdcode); }

            return ExecuteReader<HIC_X_MUNJIN_JEPSU_PATIENT>(parameter);
        }

        public HIC_X_MUNJIN_JEPSU_PATIENT GetAllItemsByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT B.UCODES,B.SNAME,B.SEX,B.AGE,C.SABUN,B.JUSO1,B.JUSO2, B.GJJONG                                 ");
            parameter.AppendSql(" , TO_CHAR(B.IPSADATE,'YYYY-MM-DD') IPSADATE,B.TEL,B.LTDCODE,B.BUSENAME                                ");
            parameter.AppendSql(" , B.JIKJONG,TO_CHAR(B.JEPDATE,'YYYY-MM-DD') JEPDATE,A.PANJENG,A.PANJENGDRNO                           ");
            parameter.AppendSql(" , A.WRTNO,TO_CHAR(A.PANJENGDATE,'YYYY-MM-DD') PANJENGDATE, A.PAN, A.SOGEN, A.JOCHI                    ");
            parameter.AppendSql(" , B.PANO,B.PTNO,C.JUMIN, C.JISA, B.MAILCODE ,B.GJJONG,B.SEX , A.JINGBN ,A.XTERM1 ,A.MUN1              ");
            parameter.AppendSql(" , A.XP1,A.XPJONG,A.XPLACE,A.XREMARK,A.XTERM,A.XMUCH,A.XJUNGSAN,A.JUNGSAN1,A.JUNGSAN2                  ");
            parameter.AppendSql(" , A.JUNGSAN3,A.WORKYN,A.SAHUCODE,A.JILBYUNG,A.BLOOD1,A.BLOOD2,A.BLOOD3,A.SKIN1,A.SKIN2,A.SKIN3        ");
            parameter.AppendSql(" , A.NERVOUS1,A.EYE1,A.EYE2,A.CANCER1,A.GAJOK,C.JUMIN2                                                 ");
            parameter.AppendSql(" , A.BLOOD ,A.NERVOUS2,A.CANCER2,A.SYMPTON,A.JIKJONG1,A.JIKJONG2,A.JIKJONG3,A.REEXAM                   ");
            parameter.AppendSql(" FROM HIC_X_MUNJIN A, HIC_JEPSU B, HIC_PATIENT C                                                       ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                                                ");
            parameter.AppendSql("  AND A.WRTNO = B.WRTNO(+)                                                                             ");
            parameter.AppendSql("  AND B.PANO  = C.PANO(+)                                                                              ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReaderSingle<HIC_X_MUNJIN_JEPSU_PATIENT>(parameter);
        }
    }
}
