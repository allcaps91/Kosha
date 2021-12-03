namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicCancerNewJepsuPatientRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicCancerNewJepsuPatientRepository()
        {

        }

        public List<HIC_CANCER_NEW_JEPSU_PATIENT> GetItembyJepdateGubun(string strFdate, string strTdate, string strName, string strLtdcode, string strSort, string strRePrint, long nWrtno, string strCert)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT c.Pano, c.Sname, b.Juso1||'  '||b.Juso2 as juso,a.GbStomach, a.GbLiver, a.GbRectum, a.GbBreast,a.GbWomb        ");
            parameter.AppendSql(" ,substr(c.Jumin,1,6)||'-'||substr(c.Jumin,7,7) as Jumin, c.Sabun , b.GjJong  ,b.GjJong, b.LtdCode                     ");
            parameter.AppendSql(" ,TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate ,TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate , b.Age, b.Sex , b.Wrtno      ");
            parameter.AppendSql(" ,b.UCodes , TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate2,c.HPhone,c.EMail                                           ");
            parameter.AppendSql(" ,TO_CHAR(b.BalDate,'YYYY-MM-DD') BalDate,b.WebPrintReq,b.PTno                                                         ");
            parameter.AppendSql(" FROM HIC_CANCER_NEW a, HIC_JEPSU b , HIC_PATIENT c                                                                    ");
            parameter.AppendSql(" WHERE a.GunDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                                         ");
            parameter.AppendSql(" AND a.GunDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                                           ");
            parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                                                ");
            parameter.AppendSql(" AND b.PanjengDrNo>0                                                                                                   ");
            if (strCert.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND b.WebPrintReq IS NULL                                                                                         ");
            }
            else
            {
                parameter.AppendSql(" AND b.WebPrintReq IS NOT NULL                                                                                     ");
            }   
            parameter.AppendSql(" AND b.GjYear>='2018'                                                                                                  ");
            parameter.AppendSql(" AND b.Pano = c.Pano(+)                                                                                                ");
            parameter.AppendSql(" AND B.GJJONG IN ('31','35')                                                                                           ");

            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.SNAME LIKE :SNAME           ");
            }
            if (!strLtdcode.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND a.LTDCODE = :LTDCODE          ");
            }

            if (nWrtno > 0)
            {
                parameter.AppendSql(" AND B.WRTNO = :WRTNO              ");
            }

            if (strRePrint =="Y")
            {
                parameter.AppendSql(" AND A. GBPRINT = 'Y'                                      ");
            }
            else
            {
                parameter.AppendSql(" AND (a.GbPrint IS NULL OR a.GbPrint<>'Y')                 ");
            }

            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY b.LtdCode,b.SName,b.JepDate ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY b.SName,b.JepDate,b.LtdCode ");
            }
            else if (strSort == "3")
            {
                parameter.AppendSql(" ORDER BY b.JepDate,b.LtdCode,b.SName ");
            }

            parameter.Add("FDATE", strFdate);
            parameter.Add("TDATE", strTdate);

            if (!strName.IsNullOrEmpty())
            {
                parameter.Add("NAME", strName);
            }
            if (!strLtdcode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdcode);
            }

            if (nWrtno > 0)
            {
                parameter.Add("WRTNO", nWrtno);
            }

            return ExecuteReader<HIC_CANCER_NEW_JEPSU_PATIENT>(parameter);


        }


        public HIC_CANCER_NEW_JEPSU_PATIENT GetIetmbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.*, b.UCodes,b.SExams,b.SName,b.Sex,b.Age,b.Sabun,b.GjYear,b.Juso1,b.Juso2                             ");
            parameter.AppendSql(" ,TO_CHAR(b.IpsaDate,'YYYY-MM-DD') IpsaDate,b.Tel,b.LtdCode,b.BuseName                                         ");
            parameter.AppendSql(" ,b.JikJong,TO_CHAR(b.BuseIpsa,'YYYY-MM-DD') JenipDate                                                         ");
            parameter.AppendSql(" ,TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate, b.Kiho, b.MailCode,b.Gubdaesang                                     ");
            parameter.AppendSql(" ,TO_CHAR(a.GunDate,'YYYY-MM-DD') GunDate, TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate                       ");
            parameter.AppendSql(" ,TO_CHAR(a.S_PanjengDate,'YYYY-MM-DD') S_PanjengDate, TO_CHAR(a.C_PanjengDate,'YYYY-MM-DD') C_PanjengDate     ");
            parameter.AppendSql(" ,TO_CHAR(a.L_PanjengDate,'YYYY-MM-DD') L_PanjengDate, TO_CHAR(a.B_PanjengDate,'YYYY-MM-DD') B_PanjengDate     ");
            parameter.AppendSql(" ,TO_CHAR(a.W_PanjengDate,'YYYY-MM-DD') W_PanjengDate                                                          ");
            parameter.AppendSql(" ,a.GbStomach, a.GbLiver, a.GbRectum, a.GbBreast,a.GbWomb                                                      ");
            parameter.AppendSql(" ,b.Pano,c.Jumin, c.Jisa, B.GBJUSO                                                                             ");
            parameter.AppendSql(" FROM HIC_CANCER_NEW a, HIC_JEPSU b, HIC_PATIENT c                                                             ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                                        ");
            parameter.AppendSql(" AND a.WRTNO = b.WRTNO(+)                                                                                      ");
            parameter.AppendSql(" AND b.Pano  = c.Pano(+)                                                                                       ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_CANCER_NEW_JEPSU_PATIENT>(parameter);
        }



    }
}
