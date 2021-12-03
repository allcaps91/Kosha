namespace ComHpcLibB
{

    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicJepsuResEtcPatientRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResEtcPatientRepository()
        {
        }

        
        public List<HIC_JEPSU_RES_ETC_PATIENT> GetItembyJepDate(string strFrDate, string strToDate, long nLtdCode, string strSName, string strGbRe,string strGjjong, long nWrtno, string strCert )
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.PANO, B.SNAME, C.JUSO1||'  '||C.JUSO2 AS JUSO,UCODES                                      ");
            parameter.AppendSql(" ,SUBSTR(C.JUMIN,1,6)||'-'||SUBSTR(C.JUMIN,7,7) AS JUMIN, C.SABUN , B.GJJONG                       ");
            parameter.AppendSql(" ,B.GJJONG, B.LTDCODE, TO_CHAR(B.JEPDATE,'YYYY-MM-DD') JEPDATE , B.AGE, B.SEX , B.WRTNO            ");
            parameter.AppendSql(" ,TO_CHAR(A.PANJENGDATE,'YYYY-MM-DD') PANJENGDATE, TO_CHAR(A.TONGBODATE,'YYYY-MM-DD') TONGBODATE   ");
            parameter.AppendSql(" ,B.PTNO, B.WEBPRINTREQ                                                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_RES_ETC a                                                                   ");
            parameter.AppendSql("     , ADMIN.HIC_JEPSU b                                                                     ");
            parameter.AppendSql("     , ADMIN.HIC_PATIENT c                                                                   ");
            parameter.AppendSql(" WHERE 1=1                                                                                         ");
            parameter.AppendSql(" AND B.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                   ");
            parameter.AppendSql(" AND B.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                   ");
            if (strCert.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND b.WebPrintReq IS NULL                                                                             ");
            }
            else
            {
                parameter.AppendSql(" AND b.WebPrintReq IS NOT NULL                                                                         ");
            }
            parameter.AppendSql(" AND B.WRTNO = A.WRTNO(+)                                                                          ");
            parameter.AppendSql(" AND A.PANJENGDATE IS NOT NULL                                                                     ");
            parameter.AppendSql(" AND B.PANO = C.PANO(+)                                                                            ");
            parameter.AppendSql(" AND B.GJJONG = :GJJONG                                                                            "); //69종
            parameter.AppendSql(" AND B.DelDate IS NULL                                                                             ");

            if (strGbRe == "Y")
            {
                parameter.AppendSql(" AND A.GBPRINT = 'Y'                                                                           ");
            }
            else
            {
                parameter.AppendSql(" AND (A.GBPRINT IS NULL OR A.GBPRINT <> 'Y')                                                   ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND B.LTDCODE = :LTDCODE                                                                    ");
            }
            if (strSName != "")
            {
                parameter.AppendSql("   AND B.SNAME LIKE :SNAME                                                                     ");
            }
            if (nWrtno > 0)
            {
                parameter.AppendSql(" AND B.WRTNO = :WRTNO                                                                          ");
            }

            parameter.AppendSql(" ORDER BY B.SNAME , B.LTDCODE                                                                      ");


            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strSName != "")
            {
                parameter.Add("SNAME", strSName);
            }

            if (nWrtno > 0)
            {
                parameter.Add("WRTNO", nWrtno);
            }

            parameter.Add("GJJONG", strGjjong);
            

            return ExecuteReader<HIC_JEPSU_RES_ETC_PATIENT>(parameter);
        }

        public HIC_JEPSU_RES_ETC_PATIENT GetItemByWrtnoGubun(long nWrtno, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.UCodes,b.SExams,b.SName,b.Sex,b.Age,c.Sabun,b.Juso1,b.Juso2                                   ");
            parameter.AppendSql(" ,TO_CHAR(b.IpsaDate,'YYYY-MM-DD') IpsaDate,b.Tel,b.LtdCode,b.BuseName                                 ");
            parameter.AppendSql(" ,b.JikJong,TO_CHAR(b.BuseIpsa,'YYYY-MM-DD') JenipDate                                                 ");
            parameter.AppendSql(" ,TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate,a.Sogen,a.PanjengDrno                                        ");
            parameter.AppendSql(" ,a.Wrtno,TO_CHAR(a.PANJENGDATE,'YYYY-MM-DD') PANJENGDATE, TO_CHAR(a.GUNDATE,'YYYY-MM-DD') GUNDATE     ");
            parameter.AppendSql(" ,a.PAN, a.JOCHI, a.SOGENREMARK, a.WorkYN, a.SAHUCODE                                                  ");
            parameter.AppendSql(" ,b.Pano,b.Ptno,c.Jumin, c.Jisa, b.MailCode ,b.GjJong,b.Sex                                            ");
            parameter.AppendSql(" FROM HIC_RES_ETC a, HIC_JEPSU b, HIC_PATIENT c                                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                                ");
            parameter.AppendSql("  AND a.WRTNO = b.WRTNO(+)                                                                             ");
            parameter.AppendSql("  AND b.Pano  = c.Pano(+)                                                                              ");
            parameter.AppendSql("  AND A.GUBUN = :GUBUN                                                                                 ");

            parameter.Add("WRTNO", nWrtno);
            parameter.Add("GUBUN", strGubun);

            return ExecuteReaderSingle<HIC_JEPSU_RES_ETC_PATIENT>(parameter);
        }
    }
}
