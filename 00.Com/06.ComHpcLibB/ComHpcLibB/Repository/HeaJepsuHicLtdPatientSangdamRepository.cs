namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuHicLtdPatientSangdamRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuHicLtdPatientSangdamRepository()
        {
        }

        public List<HEA_JEPSU_HIC_LTD_PATIENT_SANGDAM> GetItembyEntTimeGubun(string idNumber, string sJob, string strFrDate, string strToDate, string strWToDate, string strLtd, string strSort, long nLicenceNo, string argSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.SDate,'YYYY-MM-DD') SDate, a.GjJong, a.GbSTS, a.SangDamOut, A.GBEKG   ");
            parameter.AppendSql("     , b.Name LtdName, a.SangDamGbn, a.SangDamTel, a.SangDam_One, c.SABUN, a.DrSabun, a.DrName         ");
            parameter.AppendSql("     , a.NrSabun,a.SangDamYN,SangDamNot,TO_CHAR(a.IDate,'YYYY-MM-DD') IDate                            ");
            parameter.AppendSql("     , TO_CHAR(a.RecvDate,'YYYY-MM-DD') RecvDate                                                       ");
            parameter.AppendSql("     , TO_CHAR(a.WebPrintReq,'YYYY-MM-DD') WebPrintReq                                                 ");
            if (sJob == "1")
            {
                parameter.AppendSql("     , TO_CHAR(a.MailDate,'YYYY-MM-DD') MailDate,d.WaitNo,DeCode(d.GbCall,'','1','Y','2') GbCall,d.GUBUN       ");
                parameter.AppendSql("  FROM ADMIN.HEA_JEPSU        a                                                              ");
                parameter.AppendSql("     , ADMIN.HIC_LTD          b                                                              ");
                parameter.AppendSql("     , ADMIN.HIC_PATIENT      c                                                              ");
                parameter.AppendSql("     , ADMIN.HEA_SANGDAM_WAIT d                                                              ");
                parameter.AppendSql(" WHERE d.ENTTIME >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                  ");
                parameter.AppendSql("   AND d.ENTTIME <= TO_DATE(:W_TODATE,'YYYY-MM-DD')                                                ");
                if (idNumber == "32158") //이주령
                {
                    parameter.AppendSql("    AND d.GUBUN = '12'                                                                         ");
                }
                else if (idNumber == "34626") //이상엽
                {
                    parameter.AppendSql("    AND d.GUBUN = '13'                                                                         ");
                }
                else if (idNumber == "39444")   //주철효
                {
                    parameter.AppendSql("    AND d.GUBUN = '13'                                                                         ");
                }
                else if (idNumber == "54075")  //이지은
                {
                    parameter.AppendSql("    AND d.GUBUN = '14'                                                                         ");
                }
                parameter.AppendSql("    AND (d.GbCall<>'Y' OR d.GbCall IS NULL)                                                        ");
            }
            else
            {
                parameter.AppendSql("     , TO_CHAR(a.MAILDATE,'YYYY-MM-DD') MAILDATE                                                   ");
                parameter.AppendSql("  FROM ADMIN.HEA_JEPSU   a                                                                   ");
                parameter.AppendSql("     , ADMIN.HIC_LTD     b                                                                   ");
                parameter.AppendSql("     , ADMIN.HIC_PATIENT c                                                                   ");
                parameter.AppendSql(" WHERE a.SDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                   ");
                parameter.AppendSql("   AND a.SDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                   ");
                if (nLicenceNo > 0)
                {
                    parameter.AppendSql("   AND a.NRSABUN > 0                                                                           ");
                }
                parameter.AppendSql("   AND a.GbSTS NOT IN ('1','2')                                                                    ");
            }

            if (sJob == "1")
            {
                parameter.AppendSql("  AND a.WRTNO = d.WRTNO                                                                            ");
            }
            else
            {
                if (sJob == "3")
                {
                    parameter.AppendSql("   AND (a.DRSABUN = 0 or a.DRSABUN ='' or a.DRSABUN IS NULL )                                  ");
                }
                else if (sJob == "4")
                {
                    parameter.AppendSql("   AND a.DRSABUN > 0                                                                           ");
                }
            }

            parameter.AppendSql("   AND a.GbSTS NOT IN ('0','D')                                                                        ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                               ");

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                 ");
            }

            if (strLtd != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                        ");
            }
            parameter.AppendSql("   AND a.PANO = c.PANO(+)                                                                              ");
            parameter.AppendSql("   AND a.LTDCODE = b.CODE(+)                                                                           ");
            if (sJob == "1")
            {
                parameter.AppendSql(" ORDER BY GBCALL, d.WAITNO ASC                                                                     ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY a.SDATE, a.SNAME, a.GJJONG                                                           ");
            }

            parameter.Add("FRDATE", strFrDate);
            
            if (sJob == "1")
            {
                parameter.Add("W_TODATE", strWToDate);
            }
            else
            {
                parameter.Add("TODATE", strToDate);
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSName);
            }

            if (strLtd != "")
            {
                parameter.Add("LTDCODE", strLtd);
            }
            

            return ExecuteReader<HEA_JEPSU_HIC_LTD_PATIENT_SANGDAM>(parameter);
        }
    }
}
