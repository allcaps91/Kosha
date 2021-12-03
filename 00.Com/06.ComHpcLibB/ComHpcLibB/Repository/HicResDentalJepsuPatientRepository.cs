namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicResDentalJepsuPatientRepository : BaseRepository
    {


        public HicResDentalJepsuPatientRepository()
        {
        }

        public HIC_RES_DENTAL_JEPSU_PATEINT GetItemByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT b.SName,b.Sex,b.Age,c.Sabun,b.Juso1,b.Juso2,c.Jumin2,b.LtdCode,b.GbChul,b.GjYear,c.MailCode                                                ");
            parameter.AppendSql(" ,TO_CHAR(b.JepDate, 'YYYY-MM-DD') JepDate, TO_CHAR(a.PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE                                                       ");
            parameter.AppendSql(" ,TO_CHAR(a.TONGBODATE, 'YYYY-MM-DD') TONGBODATE ,a.TONGBOGBN,a.PANJENGDRNO,a.T_HABIT1,a.T_HABIT2,a.T_HABIT3                                       ");
            parameter.AppendSql(" ,a.T_HABIT4,a.T_STAT1,a.T_STAT2,a.T_STAT3,a.T_STAT4,a.T_STAT5,a.T_STAT6,a.T_FUNCTION1,a.T_FUNCTION2                                               ");
            parameter.AppendSql(" ,a.T_FUNCTION3,a.T_FUNCTION4,a.T_FUNCTION5,a.T_JILBYUNG1,a.T_PAN1,a.T_PAN2,a.T_PAN3,a.T_PAN4,a.T_PAN5,a.T_PAN6                                    ");
            parameter.AppendSql(" ,a.T_PAN7,a.T_PAN8, a.T_PAN9,a.T_PAN10,a.T_PAN11,a.T_PAN_ETC,a.T40_PAN1,a.T40_PAN2,a.T40_PAN3                                                     ");
            parameter.AppendSql(" ,a.T40_PAN4,a.T40_PAN5,a.T40_PAN6,a.T_PANJENG1,a.T_PANJENG2,a.T_PANJENG3,a.T_PANJENG4, a.T_PANJENG5, a.T_PANJENG6                                 ");
            parameter.AppendSql(" ,a.T_PANJENG7,a.T_PANJENG8,a.T_PANJENG9,a.T_PANJENG10, ,a.RES_Munjin,a.RES_RESULT,a.RES_Jochi,a.Sangdam                                           ");
            parameter.AppendSql(" ,a.T_PANJENG_ETC,a.T_PANJENG_SOGEN,a.GbPrint, A.T40_PAN1_NEW, A.T40_PAN2_NEW, A.T40_PAN3_NEW, A.T40_PAN4_NEW, A.T40_PAN5_NEW, A.T40_PAN6_NEW      ");
            parameter.AppendSql(" FROM HIC_RES_DENTAL a, HIC_JEPSU b, HIC_PATIENT c                                                                                                 ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                                                                            ");
            parameter.AppendSql(" AND a.WRTNO = b.WRTNO(+)                                                                                                                          ");
            parameter.AppendSql(" AND b.Pano = c.Pano(+)                                                                                                                            ");
            parameter.AppendSql(" AND a.PANJENGDATE IS NOT NULL                                                                                                                     ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_RES_DENTAL_JEPSU_PATEINT>(parameter);
        }

        public HIC_RES_DENTAL_JEPSU_PATEINT GetItembyMirNoMirNo(long argMirno, long argWRTNO, string argJepaAte)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,a.GjChasu               ");
            parameter.AppendSql("      , a.SECOND_Flag,TO_CHAR(a.SECOND_Date,'YYYY-MM-DD') Second_Date, b.Tel,b.Hphone, b.Email     ");
            parameter.AppendSql("      , b.Jumin2, a.Kiho, a.Gkiho ,c.*                                                             ");
            parameter.AppendSql("   FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_RES_DENTAL c           ");
            parameter.AppendSql("  WHERE a.JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                               ");
            parameter.AppendSql("    AND a.MIRNO2 = :MIRNO                                                                          ");
            parameter.AppendSql("    AND a.Pano = b.Pano                                                                            ");
            parameter.AppendSql("    AND a.Wrtno = c.Wrtno                                                                          ");
            parameter.AppendSql("    AND c.PanjengDrNo > 0                                                                          ");
            parameter.AppendSql("    AND a.WRTNO = :WRTNO                                                                           ");
            parameter.AppendSql("  ORDER BY a.LtdCode,a.WRTNO                                                                       ");

            parameter.Add("MIRNO", argMirno);
            parameter.Add("JEPDATE", argJepaAte);
            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReaderSingle<HIC_RES_DENTAL_JEPSU_PATEINT>(parameter);
        }



        public List<HIC_RES_DENTAL_JEPSU_PATEINT> GetItemByPandateSnameLtdGubun(string argFDate, string argTDate, string argSname, string argLtdCode, string argRePrint, string argSort)
        {
            MParameter parameter = CreateParameter();                                                    
            parameter.AppendSql(" SELECT b.Pano, b.Sname, c.Juso1||'  '||c.Juso2 as juso,b.BuseName                                     ");
            parameter.AppendSql(" ,substr(c.Jumin,1,6)||'-'||substr(c.Jumin,7,7) as Jumin, c.Sabun , b.GjJong,c.Sabun                   ");
            parameter.AppendSql(" ,b.GjYear, b.LtdCode, TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate , b.Age, b.Sex , b.Wrtno, b.UCodes      ");
            parameter.AppendSql(" ,TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate                                                        ");
            parameter.AppendSql(" ,TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate2                                                       ");
            parameter.AppendSql(" ,TO_CHAR(b.BalDate,'YYYY-MM-DD') BalDate,b.GbMunjin1,c.HPhone,c.EMail                                 ");
            parameter.AppendSql("  FROM HIC_RES_DENTAL a,HIC_JEPSU b , HIC_PATIENT c                                                    ");
            parameter.AppendSql(" WHERE a.PanjengDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND a.PanjengDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                                              ");
            parameter.AppendSql("   AND b.GbMunjin2='Y'                                                                                 ");
            parameter.AppendSql("   AND b.GbDental='Y'                                                                                  ");
            parameter.AppendSql("   AND b.Pano = c.Pano(+)                                                                              ");

            if (argRePrint == "OK")
            {
                parameter.AppendSql("   AND a.GbPrint='Y' ");
            }
            else
            {
                parameter.AppendSql("  AND (a.GbPrint IS NULL OR a.GbPrint<>'Y') ");
            }

            if (!argSname.IsNullOrEmpty())
            {
                parameter.AppendSql("  AND b.SName LIKE :SNAME ");
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("  AND b.LTDCODE = :LTDCODE ");
            }

            if (argSort == "1")

            {
                parameter.AppendSql("  ORDER BY b.LtdCode,b.SName ");
            }

            else if (argSort =="2")
            {
                parameter.AppendSql("  ORDER BY b.SName,b.JepDate ");
            }
            else
            {
                parameter.AppendSql("  ORDER BY b.JepDate,b.SName ");
            }

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            if (!argSname.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSname);
            }
            if (!argLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", argLtdCode);
            }

            return ExecuteReader<HIC_RES_DENTAL_JEPSU_PATEINT>(parameter);
        }
    }
}
