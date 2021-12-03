
namespace ComHpcLibB.Model
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicResBohum1JepsuPatientRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicResBohum1JepsuPatientRepository()
        {

        }

        public HIC_RES_BOHUM1_JEPSU_PATIENT GetItembyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT b.SExams,b.SName,c.Sabun,b.Juso1,b.Juso2,c.Jumin, b.MailCode                               ");
            parameter.AppendSql("      , TO_CHAR(b.IpsaDate,'YYYY-MM-DD') IpsaDate,b.Tel,b.LtdCode,c.BuseName,b.GbChul              ");
            parameter.AppendSql("      , a.Wrtno,a.SOGEN,a.SOGENB,a.SOGENC,a.SOGEND,a.TMUN0092,a.TMUN0093,a.TMUN0094,a.TMUN0095     ");
            parameter.AppendSql("      , a.PANJENG,a.PANJENGU1,a.PANJENGU2,a.PANJENGU3,a.PANJENGU4, B.GBJUSO                        ");
            parameter.AppendSql("   FROM ADMIN.HIC_RES_BOHUM1 a, ADMIN.HIC_JEPSU b, ADMIN.HIC_PATIENT c           ");
            parameter.AppendSql("  WHERE a.WRTNO = :WRTNO                                                                           ");
            parameter.AppendSql("    AND a.WRTNO = b.WRTNO(+)                                                                       ");
            parameter.AppendSql("    AND b.Pano  = c.Pano(+)                                                                        ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_RES_BOHUM1_JEPSU_PATIENT>(parameter);

        }

        public HIC_RES_BOHUM1_JEPSU_PATIENT GetItembyJepDateMirNoWrtNo(string sJepDate, long argMirno, long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu,a.Age,a.Ptno                              ");
            parameter.AppendSql("      , a.SECOND_Flag,TO_CHAR(a.SECOND_Date,'YYYY-MM-DD') Second_Date, a.GbChul,a.GbChul2, a.Juso1 || ' ' || a.Juso2  JusoAA   ");
            parameter.AppendSql("      , b.Jumin2, a.Kiho, a.Gkiho , TO_CHAR(c.PanjengDate,'YYYY-MM-DD') PanjengDate                                            ");
            parameter.AppendSql("      , TO_CHAR(c.TongboDate,'YYYY-MM-DD') TongboDate,a.SANGDAMDRNO, a.SANGDAMDATE, a.SEX, a.GBCHK1, a.GBCHK2                  ");
            parameter.AppendSql("      , c.WRTNO, c.HEIGHT, c.WEIGHT, c.BIMAN, c.EYE_L, c.EYE_R, c.EAR_L, c.EAR_R, c.BLOOD_H, c.BLOOD_L, c.URINE1               ");
            parameter.AppendSql("      , c.URINE2, c.URINE3, c.URINE4, c.BLOOD1, c.BLOOD2, c.BLOOD3, c.BLOOD4, c.BLOOD5, c.BLOOD6                               ");
            parameter.AppendSql("      , c.LIVER1, c.LIVER2, c.LIVER3, c.XRAYGBN, c.XRAYRES, c.EKG, c.CYTO1, c.CYTO2, c.EXAMFLAG                                ");
            parameter.AppendSql("      , c.SICK11, c.SICK12, c.SICK13, c.SICK21, c.SICK22, c.SICK23, c.SICK31, c.SICK32, c.SICK33                               ");
            parameter.AppendSql("      , c.GAJOK1, c.GAJOK2, c.GAJOK3, c.GAJOK4, c.GAJOK5, c.GAJOK6, c.ROSICK, c.ROSICKNAME, c.SIKSENG                          ");
            parameter.AppendSql("      , c.DRINK1, c.DRINK2, c.SMOKING1, c.SMOKING2, c.SMOKING3, c.SPORTS, c.ANNOUNE, c.WOMAN1, c.WOMAN2                        ");
            parameter.AppendSql("      , c.WOMAN3, c.MUNJINFLAG, c.MUNJINENTDATE, c.MUNJINENTSABUN, c.OLDBYENG, c.OLDBYENG1, c.OLDBYENG2                        ");
            parameter.AppendSql("      , c.OLDBYENG3, c.OLDBYENG4, c.OLDBYENG5, c.OLDBYENG6, c.OLDBYENG7, c.HABIT, c.HABIT1, c.HABIT2, c.HABIT3                 ");
            parameter.AppendSql("      , c.HABIT4, c.HABIT5, c.JINCHAL1, c.JINCHAL2, c.PANJENG, c.PANJENGB1, c.PANJENGB2, c.PANJENGB3, c.PANJENGB4              ");
            parameter.AppendSql("      , c.PANJENGB5, c.PANJENGB6, c.PANJENGB7, c.PANJENGB8, c.PANJENGR1, c.PANJENGR2, c.PANJENGR3, c.PANJENGR4                 ");
            parameter.AppendSql("      , c.PANJENGR5, c.PANJENGR6, c.PANJENGR7, c.PANJENGR8, c.PANJENGR9, c.PANJENGR10, c.PANJENGR11, c.PANJENGETC              ");
            parameter.AppendSql("      , c.PANJENGDATE, c.GUNDATE, c.TONGBOGBN, c.TONGBODATE, c.PANJENGDRNO, c.SOGEN, c.XRAYNO, c.IPSADATE, c.WOMB01            ");
            parameter.AppendSql("      , c.WOMB02, c.WOMB03, c.WOMB04, c.WOMB05, c.WOMB06, c.WOMB07, c.WOMB08, c.WOMB09, c.WOMB10, c.WOMB11, c.OLDBYENGNAME     ");
            parameter.AppendSql("      , c.GBPRINT, c.MUNJINDRNO, c.JINREMARK, c.GBPANJENG, c.PANJENGB9, c.PANJENGB_ETC, c.PANJENGR_ETC, c.ADDSOGEN             ");
            parameter.AppendSql("      , c.SMOKING4, c.SMOKING5, c.PANJENGB_ETC_DTL, c.WAIST, c.T_STAT01, c.T_STAT02, c.T_STAT11, c.T_STAT12, c.T_STAT21        ");
            parameter.AppendSql("      , c.T_STAT22, c.T_STAT31, c.T_STAT32, c.T_STAT41, c.T_STAT42, c.T_STAT51, c.T_STAT52, c.T_GAJOK1, c.T_GAJOK2             ");
            parameter.AppendSql("      , c.T_GAJOK3, c.T_GAJOK4, c.T_GAJOK5, c.T_BLIVER, c.T_SMOKE1, c.T_SMOKE2, c.T_SMOKE3, c.T_SMOKE4, c.T_SMOKE5             ");
            parameter.AppendSql("      , c.T_DRINK1, c.T_DRINK2, c.T_ACTIVE1, c.T_ACTIVE2, c.T_ACTIVE3, c.T40_FEEL1, c.T40_FEEL2, c.T40_FEEL3                   ");
            parameter.AppendSql("      , c.T40_FEEL4, c.T66_INJECT, c.T66_STAT1, c.T66_STAT2, c.T66_STAT3, c.T66_STAT4, c.T66_STAT5, c.T66_STAT6                ");
            parameter.AppendSql("      , c.T66_FEEL1, c.T66_FEEL2, c.T66_FEEL3, c.T66_MEMORY1, c.T66_MEMORY2, c.T66_MEMORY3, c.T66_MEMORY4                      ");
            parameter.AppendSql("      , c.T66_MEMORY5, c.T66_FALL, c.T66_URO, c.PANJENGC1, c.PANJENGC2, c.PANJENGC3, c.PANJENGC4, c.PANJENGD11                 ");
            parameter.AppendSql("      , c.PANJENGD12, c.PANJENGD13, c.PANJENGD21, c.PANJENGD22, c.PANJENGD23, c.PANJENGSAHU, c.PANJENGC5, c.SANGDAM            ");
            parameter.AppendSql("      , c.SANGDAM2, c.GBSIKSA, c.T40_FEEL, c.T66_STAT, c.FOOT1, c.FOOT2, c.BALANCE, c.OSTEO, c.LIFESOGEN, c.T_STAT61           ");
            parameter.AppendSql("      , c.T_STAT62, c.PANJENGU1, c.PANJENGU2, c.PANJENGU3, c.PANJENGU4, c.PANJENGSAHU2, c.PANJENGSAHU3, c.WORKYN               ");
            parameter.AppendSql("      , c.PRTSABUN, c.T_STAT71, c.T_STAT72, c.SOGENB, c.GBGONGHU, c.TMUN0001, c.TMUN0002, c.TMUN0003, c.TMUN0004               ");
            parameter.AppendSql("      , c.TMUN0005, c.TMUN0006, c.TMUN0007, c.TMUN0008, c.TMUN0009, c.TMUN0010, c.TMUN0011, c.TMUN0012, c.TMUN0013             ");
            parameter.AppendSql("      , c.TMUN0014, c.TMUN0015, c.TMUN0016, c.TMUN0017, c.TMUN0018, c.TMUN0019, c.TMUN0020, c.TMUN0021, c.TMUN0022             ");
            parameter.AppendSql("      , c.TMUN0023, c.TMUN0024, c.TMUN0025, c.TMUN0026, c.TMUN0027, c.TMUN0028, c.TMUN0029, c.TMUN0030, c.TMUN0031             ");
            parameter.AppendSql("      , c.TMUN0032, c.TMUN0033, c.TMUN0034, c.TMUN0035, c.TMUN0036, c.TMUN0037, c.TMUN0038, c.TMUN0039, c.TMUN0040             ");
            parameter.AppendSql("      , c.TMUN0041, c.TMUN0042, c.TMUN0043, c.TMUN0044, c.TMUN0045, c.TMUN0046, c.TMUN0047, c.TMUN0048, c.TMUN0049             ");
            parameter.AppendSql("      , c.TMUN0050, c.TMUN0051, c.TMUN0052, c.TMUN0053, c.TMUN0054, c.TMUN0055, c.TMUN0056, c.TMUN0057, c.TMUN0058             ");
            parameter.AppendSql("      , c.TMUN0059, c.TMUN0060, c.TMUN0061, c.TMUN0062, c.TMUN0063, c.TMUN0064, c.TMUN0065, c.TMUN0066, c.TMUN0067             ");
            parameter.AppendSql("      , c.TMUN0068, c.TMUN0069, c.TMUN0070, c.TMUN0071, c.TMUN0072, c.TMUN0073, c.TMUN0074, c.TMUN0075, c.TMUN0076             ");
            parameter.AppendSql("      , c.TMUN0077, c.TMUN0078, c.TMUN0079, c.TMUN0080, c.TMUN0081, c.TMUN0082, c.TMUN0083, c.TMUN0084, c.TMUN0085             ");
            parameter.AppendSql("      , c.TMUN0086, c.TMUN0087, c.TMUN0088, c.TMUN0089, c.TMUN0090, c.TMUN0091, c.TMUN0092, c.TMUN0093, c.TMUN0094             ");
            parameter.AppendSql("      , c.TMUN0095, c.PANJENGC, c.PANJENGD, c.SLIP_SMOKE, c.SLIP_DRINK, c.SLIP_ACTIVE, c.SLIP_FOOD, c.SLIP_BIMAN               ");
            parameter.AppendSql("      , c.SOGENC, c.SOGEND, c.SLIP_PHQ, c.SLIP_KDSQ, c.SLIP_OLDMAN, c.SLIP_LIFESOGEN1, c.SLIP_LIFESOGEN2                       ");
            parameter.AppendSql("      , c.PANJENGB10, c.PANJENGR12, c.TMUN0096, c.TMUN0097, c.TMUN0098, c.TMUN0099, c.TMUN0100, c.TMUN0101                     ");
            parameter.AppendSql("      , c.TMUN0102, c.TMUN0103, c.TMUN0104, c.SIM_RESULT1, c.SIM_RESULT2, c.SIM_RESULT3, c.TMUN0105, c.TMUN0106                ");
            parameter.AppendSql("      , c.TMUN0107, c.TMUN0108, c.TMUN0109, c.TMUN0110, c.TMUN0111, c.TMUN0112, c.TMUN0113, c.TMUN0114, c.TMUN0115             ");
            parameter.AppendSql("      , c.TMUN0116, c.TMUN0117, c.TMUN0118, c.TMUN0119, c.TMUN0120, c.TMUN0121, c.TMUN0122, c.TMUN0123, c.TMUN0124             ");
            parameter.AppendSql("   FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_RES_BOHUM1 c                                       ");
            parameter.AppendSql("  WHERE a.JEPDATE >= TO_DATE(:JEPDATE,'YYYY-MM-DD')                                                                            ");
            parameter.AppendSql("    AND a.MIRNO1 = :MIRNO                                                                                                      ");
            parameter.AppendSql("    AND a.Pano = b.Pano(+)                                                                                                     ");
            parameter.AppendSql("    AND a.Wrtno = c.Wrtno                                                                                                      ");
            parameter.AppendSql("    AND c.PanjengDrNo > 0                                                                                                      ");
            parameter.AppendSql("    AND A.WRTNO = :WRTNO                                                                                                       ");
            parameter.AppendSql("  ORDER BY a.Sname, a.JepDate                                                                                                  ");

            parameter.Add("JEPDATE", sJepDate);
            parameter.Add("MIRNO", argMirno);
            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_RES_BOHUM1_JEPSU_PATIENT>(parameter);
        }

        public List<HIC_RES_BOHUM1_JEPSU_PATIENT> GetItembyJepdateGubun(string strFdate, string strTdate, string strName, string strLtdcode, string strSort, string strRePrint, long nWrtno, string strCert)
        {
            MParameter parameter = CreateParameter();


            parameter.AppendSql(" SELECT b.Pano, b.Sname, b.Juso1||'  '||b.Juso2 as juso,b.BuseName                                         ");
            parameter.AppendSql(" ,substr(c.Jumin,1,6)||'-'||substr(c.Jumin,7,7) as Jumin, c.Sabun , b.GjJong,c.Sabun                       ");
            parameter.AppendSql(" ,b.GjJong, b.LtdCode, TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate , b.Age, b.Sex , b.Wrtno, b.UCodes          ");
            parameter.AppendSql(" ,TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate,TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate2             ");
            parameter.AppendSql(" ,TO_CHAR(a.Panjengdate,'YYYY-MM-DD') Panjengdate,c.HPhone,c.EMail,b.Ptno,b.GbDental,b.WebPrintReq         ");
            parameter.AppendSql(" FROM HIC_RES_BOHUM1 a,HIC_JEPSU b , HIC_PATIENT c                                                         ");
            parameter.AppendSql(" WHERE b.JepDate>=TO_DATE(:FDATE, 'YYYY-MM-DD')                                                            ");
            parameter.AppendSql(" AND b.JepDate<=TO_DATE(:TDATE, 'YYYY-MM-DD')                                                              ");
            parameter.AppendSql(" AND b.UCodes IS NULL                                                                                      ");
            if (strCert.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND b.WebPrintReq IS NULL                                                                             ");
            }
            else
            {
                parameter.AppendSql(" AND b.WebPrintReq IS NOT NULL                                                                         ");
            }
            
            parameter.AppendSql(" AND ((b.GbDental='Y' AND b.GbMunjin2='Y') OR (b.GbDental='N' OR b.GbDental IS NULL))                      ");
            parameter.AppendSql(" AND a.WRTNO=b.WRTNO                                                                                       ");
            parameter.AppendSql(" AND a.PanjengDate IS NOT NULL                                                                             ");
            parameter.AppendSql(" AND b.GjYear>='2021'                                                                                      ");
            parameter.AppendSql(" AND b.Deldate IS NULL                                                                                     ");
            parameter.AppendSql(" AND b.Pano = c.Pano(+)                                                                                    ");
            parameter.AppendSql(" AND b.GjJong IN ('11','12','13','14','41','42','43','69')                                                 ");
            
            if (strRePrint =="Y")
            {
                parameter.AppendSql(" AND A.GBPRINT = 'Y'               ");
            }
            else
            {
                parameter.AppendSql(" AND (A.GBPRINT IS NULL OR A.GBPRINT <> 'Y')               ");
            }

            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND B.SNAME LIKE :NAME            ");
            }
            if (!strLtdcode.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND B.LTDCODE = :LTDCODE          ");
            }
            if(nWrtno > 0)
            {
                parameter.AppendSql(" AND B.WRTNO = :WRTNO              ");
            }


            if(strSort == "1")
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
                parameter.AddLikeStatement("NAME", strName);
            }
            if (nWrtno > 0)
            {
                parameter.Add("WRTNO", nWrtno);
            }
                if (!strLtdcode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", strLtdcode);
            }

            return ExecuteReader<HIC_RES_BOHUM1_JEPSU_PATIENT>(parameter);


        }
    }
}
