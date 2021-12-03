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
    public class HicJepsuPatientSchoolRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientSchoolRepository()
        {
        }

        public HIC_JEPSU_PATIENT_SCHOOL GetItembyJepDateWrtNo(string strFrDate, string strToDate, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT C.WRTNO, C.JUMIN, C.SNAME, C.SDATE, C.RDATE, C.SEX, C.GBN, C.CLASS, C.BAN           ");
            parameter.AppendSql("     , C.BUN, C.LTDCODE, C.DMUN1, C.DMUN2, C.DMUN3, C.DMUN4, C.DMUN5, C.DMUN6, C.DMUN7     ");
            parameter.AppendSql("     , C.DMUN8, C.DMUN9, C.DMUN10, C.DMUNREMARK, C.DPAN1, C.DPAN2, C.DPAN3, C.DPAN4        ");
            parameter.AppendSql("     , C.DPAN5, C.DPAN6, C.DPAN7, C.DPAN8, C.DPAN9, C.DPAN10, C.DPAN11, C.DPANSOGEN        ");
            parameter.AppendSql("     , C.DPANJOCHI, C.DPANDRNO, C.PMUNA1, C.PMUNA2, C.PMUNA3                               ");
            parameter.AppendSql("     , C.PMUNA4, C.PMUNA5, C.PMUNA6, C.PMUNA7, C.PMUNB1, C.PMUNB2, C.PMUNB3, C.PMUNB4      ");
            parameter.AppendSql("     , C.PMUNB5, C.PMUNB6, C.PMUNB7, C.PMUNC1, C.PMUNC2                                    ");
            parameter.AppendSql("     , C.PMUNC3, C.PMUNC4, C.PMUNC5, C.PMUNC6, C.PMUNC7, C.PMUND1                          ");
            parameter.AppendSql("     , C.PMUND2, C.PMUND3, C.PMUND4, C.PMUND5, C.PMUND6, C.PMUND7, C.PMUND8                ");
            parameter.AppendSql("     , C.PMUNREMARK1, C.PMUNREMARK2, C.PPANA1, C.PPANA2, C.PPANA3                          ");
            parameter.AppendSql("     , C.PPANA4, C.PPANB1, C.PPANB2, C.PPANC1, C.PPANC2, C.PPANC3                          ");
            parameter.AppendSql("     , C.PPANC4, C.PPANC5, C.PPANC6, C.PPANC7, C.PPANC8, C.PPANC9, C.PPAND1, C.PPAND2      ");
            parameter.AppendSql("     , C.PPAND3, C.PPAND4, C.PPAND5, C.PPAND6, C.PPANE1                                    ");
            parameter.AppendSql("     , C.PPANE2, C.PPANE3, C.PPANE4, C.PPANF1, C.PPANF2, C.PPANF3                          ");
            parameter.AppendSql("     , C.PPANF4, C.PPANF5, C.PPANF6, C.PPANG1, C.PPANH1, C.PPANJ1, C.PPANK1, C.PPANK2      ");
            parameter.AppendSql("     , C.PPANK3, C.PPANK4, C.PPANDRNO, C.ENTTIME                                           ");
            parameter.AppendSql("     , C.ENTSABUN, C.GBDNTPRT, C.GBPANPRT, C.GBPAN, C.PMUND9, C.PPANREMARK1                ");
            parameter.AppendSql("     , C.PPANREMARK2, C.GBMIRPRINT, C.SANGDAM, C.DPANDATE, C.DPAN12, C.DPAN13              ");
            parameter.AppendSql("     , C.TONGBODATE, C.PRTSABUN, C.JUMIN2, C.LTDCODE2                                      ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.WRTNO  = :WRTNO                                                                   ");
            parameter.AppendSql("   AND a.GjJong = '56'                                                                     "); //학생신검만
            parameter.AppendSql("   AND a.Pano   = b.Pano(+)                                                                ");
            parameter.AppendSql("   AND a.WRTNO  = c.WRTNO(+)                                                               ");
            parameter.AppendSql(" ORDER BY a.JepDate,a.SName                                                                ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDateGroup(string argDate1, string argDate2, string argLtdCode, string argClass)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT c.Class,c.Ban,c.Bun,a.SName,DECODE(SUBSTR(a.Sex,1,1),'M','남','F','여') Sex1 ,c.PPanRemark1,c.PPanRemark2,c.PPanDrno    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c                                        ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                             ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                             ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                                                    ");
            parameter.AppendSql("   AND c.CLASS   = :CLASS                                                                                                      ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                                                       ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                                                    ");
            parameter.AppendSql("   AND a.GjJong='56'                                                                                                           ");    //학생신검만
            parameter.AppendSql("   AND c.PPanDrno IS NOT NULL                                                                                                  ");    //판정된것만
            parameter.AppendSql("   AND c.Rdate IS NOT NULL                                                                                                     ");    //판정된것만
            parameter.AppendSql("   AND c.GbPanPrt ='Y'                                                                                                         ");    //인쇄된것
            parameter.AppendSql("   AND c.GbPan ='3'                                                                                                            ");    //질환의심
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                                                        ");
            parameter.AppendSql("   AND a.WRTNO=c.WRTNO(+)                                                                                                      ");
            parameter.AppendSql(" GROUP BY c.Class,c.Ban,c.Bun,a.SName,a.Sex,c.PPanRemark1,c.PPanRemark2,c.PPanDrno                                             ");
            parameter.AppendSql(" ORDER BY c.Class,c.Ban,c.Bun,a.SName,a.Sex,c.PPanRemark1,c.PPanRemark2,c.PPanDrno                                             ");

            parameter.Add("FRDATE", argDate1);
            parameter.Add("TODATE", argDate2);
            parameter.Add("LTDCODE", argLtdCode);
            parameter.Add("CLASS", argClass);

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public HIC_JEPSU_PATIENT_SCHOOL GetItembyJepDateSingle(string strFrDate, string strToDate, long nLtdCode1, string strClass)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LtdCode,c.Class,MAX(a.JepDate) MaxDate,MIN(a.JepDate) MinDate                     ");
            parameter.AppendSql("     , COUNT(a.LtdCode) CNT                                                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.GjJong='56'                                                                       "); //학생신검만
            parameter.AppendSql("   AND c.PPanDrno IS NOT NULL                                                              "); //판정된것만
            parameter.AppendSql("   AND c.Rdate IS NOT NULL                                                                 "); //판정된것만
            parameter.AppendSql("   AND c.GbPanPrt ='Y'                                                                     "); //인쇄된것
            parameter.AppendSql("   AND c.GbPan ='3'                                                                        "); //질환의심만                                                                    "); //인쇄된것            
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            parameter.AppendSql("   AND c.CLASS   = :CLASS                                                                  ");
            parameter.AppendSql("   AND a.Pano  = b.Pano(+)                                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");
            parameter.AppendSql(" GROUP BY a.LtdCode,c.Class                                                                ");
            parameter.AppendSql(" ORDER BY a.LtdCode,c.Class                                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("LTDCODE", nLtdCode1);
            parameter.Add("CLASS", strClass);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItemCntbyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LtdCode,c.Class,MAX(a.JepDate) MaxDate,MIN(a.JepDate) MinDate                     ");
            parameter.AppendSql("     , COUNT(a.LtdCode) CNT                                                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                "); 
            parameter.AppendSql("   AND a.GjJong='56'                                                                       "); //학생신검만
            parameter.AppendSql("   AND c.PPanDrno IS NOT NULL                                                              "); //판정된것만
            parameter.AppendSql("   AND c.Rdate IS NOT NULL                                                                 "); //판정된것만
            parameter.AppendSql("   AND c.GbPanPrt ='Y'                                                                     "); //인쇄된것
            parameter.AppendSql("   AND c.GbPan ='3'                                                                        "); //질환의심만                                                                    "); //인쇄된것            
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }
            parameter.AppendSql("   AND a.Pano  = b.Pano(+)                                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");
            parameter.AppendSql(" GROUP BY a.LtdCode,c.Class                                                                ");
            parameter.AppendSql(" ORDER BY a.LtdCode,c.Class                                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public HIC_JEPSU_PATIENT_SCHOOL GetMinMaxDate(string strFrDate, string strToDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(MAX(a.JepDate), 'YYYY-MM-DD') MaxDate                                       ");
            parameter.AppendSql("     , TO_CHAR(MIN(a.JepDate), 'YYYY-MM-DD') MinDate                                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.GjJong='56'                                                                       "); //학생신검만
            parameter.AppendSql("   AND c.GbPanPrt ='Y'                                                                     "); //인쇄된것            
            parameter.AppendSql("   AND c.PPanDrno IS NOT NULL                                                              "); //판정된것만
            parameter.AppendSql("   AND c.Rdate IS NOT NULL                                                                 "); //판정된것만
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }
            parameter.AppendSql("   AND a.Pano  = b.Pano(+)                                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDate(string argDate1, string argDate2, string argLtdCode, string argClass)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SEX, C.WRTNO, C.JUMIN, C.SNAME, C.SDATE, C.RDATE, C.SEX, C.GBN, C.CLASS, C.BAN    ");
            parameter.AppendSql("     , DECODE(SUBSTR(a.Sex, 1, 1), 'M', '남', 'F', '여') Sex1                              ");  
            parameter.AppendSql("     , C.BUN, C.LTDCODE, C.DMUN1, C.DMUN2, C.DMUN3, C.DMUN4, C.DMUN5, C.DMUN6, C.DMUN7     ");
            parameter.AppendSql("     , C.DMUN8, C.DMUN9, C.DMUN10, C.DMUNREMARK, C.DPAN1, C.DPAN2, C.DPAN3, C.DPAN4        ");
            parameter.AppendSql("     , C.DPAN5, C.DPAN6, C.DPAN7, C.DPAN8, C.DPAN9, C.DPAN10, C.DPAN11, C.DPANSOGEN        ");
            parameter.AppendSql("     , C.DPANJOCHI, C.DPANDRNO, C.PMUNA1, C.PMUNA2, C.PMUNA3                               ");
            parameter.AppendSql("     , C.PMUNA4, C.PMUNA5, C.PMUNA6, C.PMUNA7, C.PMUNB1, C.PMUNB2, C.PMUNB3, C.PMUNB4      ");
            parameter.AppendSql("     , C.PMUNB5, C.PMUNB6, C.PMUNB7, C.PMUNC1, C.PMUNC2                                    ");
            parameter.AppendSql("     , C.PMUNC3, C.PMUNC4, C.PMUNC5, C.PMUNC6, C.PMUNC7, C.PMUND1                          ");
            parameter.AppendSql("     , C.PMUND2, C.PMUND3, C.PMUND4, C.PMUND5, C.PMUND6, C.PMUND7, C.PMUND8                ");
            parameter.AppendSql("     , C.PMUNREMARK1, C.PMUNREMARK2, C.PPANA1, C.PPANA2, C.PPANA3                          ");
            parameter.AppendSql("     , C.PPANA4, C.PPANB1, C.PPANB2, C.PPANC1, C.PPANC2, C.PPANC3                          ");
            parameter.AppendSql("     , C.PPANC4, C.PPANC5, C.PPANC6, C.PPANC7, C.PPANC8, C.PPANC9, C.PPAND1, C.PPAND2      ");
            parameter.AppendSql("     , C.PPAND3, C.PPAND4, C.PPAND5, C.PPAND6, C.PPANE1                                    ");
            parameter.AppendSql("     , C.PPANE2, C.PPANE3, C.PPANE4, C.PPANF1, C.PPANF2, C.PPANF3                          ");
            parameter.AppendSql("     , C.PPANF4, C.PPANF5, C.PPANF6, C.PPANG1, C.PPANH1, C.PPANJ1, C.PPANK1, C.PPANK2      ");
            parameter.AppendSql("     , C.PPANK3, C.PPANK4, C.PPANDRNO, C.ENTTIME                                           ");
            parameter.AppendSql("     , C.ENTSABUN, C.GBDNTPRT, C.GBPANPRT, C.GBPAN, C.PMUND9, C.PPANREMARK1                ");
            parameter.AppendSql("     , C.PPANREMARK2, C.GBMIRPRINT, C.SANGDAM, C.DPANDATE, C.DPAN12, C.DPAN13              ");
            parameter.AppendSql("     , C.TONGBODATE, C.PRTSABUN, C.JUMIN2, C.LTDCODE2                                      ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            parameter.AppendSql("   AND c.CLASS   = :CLASS                                                                  ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.GjJong='56'                                                                       "); //학생신검만
            if (argClass == "2" || argClass == "3" || argClass == "5" || argClass == "6")
            {
                parameter.AppendSql("   AND c.DPanDrno IS NOT NULL                                                          ");       //판정된것만
            }
            else
            {
                parameter.AppendSql("   AND c.PPanDrno IS NOT NULL                                                          ");       //판정된것만
                parameter.AppendSql("   AND c.Rdate IS NOT NULL                                                             ");       //판정된것만
            }
            parameter.AppendSql("   AND c.GbPanPrt ='Y'                                                                     "); //인쇄된것
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                    ");
            parameter.AppendSql("   AND a.WRTNO=c.WRTNO(+)                                                                  ");
            parameter.AppendSql(" ORDER BY c.Class,a.Sex                                                                    ");

            parameter.Add("FRDATE", argDate1);
            parameter.Add("TODATE", argDate2);
            parameter.Add("LTDCODE", argLtdCode);
            parameter.Add("CLASS", argClass);

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDatePrtCnt(string strFrDate, string strToDate, string strPrt, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LtdCode,c.Class,MAX(a.JepDate) MaxDate,MIN(a.JepDate) MinDate                     ");
            parameter.AppendSql("     , COUNT(a.LtdCode) CNT                                                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.GjJong='56'                                                                       "); //학생신검만
            parameter.AppendSql("   AND c.GbPanPrt ='Y'                                                                     "); //인쇄된것
            if (strPrt == "1")
            {
                parameter.AppendSql("   AND ( c.GbMirPrint IS NULL OR c.GbMirPrint ='' )                                    ");
            }
            else if (strPrt == "2")
            {
                parameter.AppendSql("   AND c.GbMirPrint ='Y'                                                               ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }
            parameter.AppendSql("   AND a.Pano  = b.Pano(+)                                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");
            parameter.AppendSql(" GROUP BY a.LtdCode,c.Class                                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetSexbyJepDate(string strFrDate, string strToDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SEX                                                                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.GjJong = '56'                                                                     ");    //학생신검만
            parameter.AppendSql("   AND c.PPanDrno IS NOT NULL                                                              ");    //판정된것만
            parameter.AppendSql("   AND c.Rdate IS NOT NULL                                                                 ");    //판정된것만
            parameter.AppendSql("   AND c.GbPanPrt = 'Y'                                                                    ");    //인쇄된것
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }
            parameter.AppendSql("   AND a.Pano  = b.Pano(+)                                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");
            parameter.AppendSql(" GROUP BY a.Sex                                                                            ");

            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDateLtdCode(string strFrDate, string strToDate, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.Pano,a.SName,a.Sex,a.Age                  ");
            parameter.AppendSql("     , a.WRTNO,a.LtdCode,b.Jumin2                                                          ");
            parameter.AppendSql("     , C.JUMIN, C.SDATE, C.RDATE, C.GBN, C.CLASS, C.BAN                                    ");
            parameter.AppendSql("     , C.BUN, C.DMUN1, C.DMUN2, C.DMUN3, C.DMUN4, C.DMUN5, C.DMUN6, C.DMUN7                ");
            parameter.AppendSql("     , C.DMUN8, C.DMUN9, C.DMUN10, C.DMUNREMARK, C.DPAN1, C.DPAN2, C.DPAN3, C.DPAN4        ");
            parameter.AppendSql("     , C.DPAN5, C.DPAN6, C.DPAN7, C.DPAN8, C.DPAN9, C.DPAN10, C.DPAN11, C.DPANSOGEN        ");
            parameter.AppendSql("     , C.DPANJOCHI, C.DPANDRNO, C.PMUNA1, C.PMUNA2, C.PMUNA3                               ");
            parameter.AppendSql("     , C.PMUNA4, C.PMUNA5, C.PMUNA6, C.PMUNA7, C.PMUNB1, C.PMUNB2, C.PMUNB3, C.PMUNB4      ");
            parameter.AppendSql("     , C.PMUNB5, C.PMUNB6, C.PMUNB7, C.PMUNC1, C.PMUNC2                                    ");
            parameter.AppendSql("     , C.PMUNC3, C.PMUNC4, C.PMUNC5, C.PMUNC6, C.PMUNC7, C.PMUND1                          ");
            parameter.AppendSql("     , C.PMUND2, C.PMUND3, C.PMUND4, C.PMUND5, C.PMUND6, C.PMUND7, C.PMUND8                ");
            parameter.AppendSql("     , C.PMUNREMARK1, C.PMUNREMARK2, C.PPANA1, C.PPANA2, C.PPANA3                          ");
            parameter.AppendSql("     , C.PPANA4, C.PPANB1, C.PPANB2, C.PPANC1, C.PPANC2, C.PPANC3                          ");
            parameter.AppendSql("     , C.PPANC4, C.PPANC5, C.PPANC6, C.PPANC7, C.PPANC8, C.PPANC9, C.PPAND1, C.PPAND2      ");
            parameter.AppendSql("     , C.PPAND3, C.PPAND4, C.PPAND5, C.PPAND6, C.PPANE1                                    ");
            parameter.AppendSql("     , C.PPANE2, C.PPANE3, C.PPANE4, C.PPANF1, C.PPANF2, C.PPANF3                          ");
            parameter.AppendSql("     , C.PPANF4, C.PPANF5, C.PPANF6, C.PPANG1, C.PPANH1, C.PPANJ1, C.PPANK1, C.PPANK2      ");
            parameter.AppendSql("     , C.PPANK3, C.PPANK4, C.PPANDRNO, C.ENTTIME                                           ");
            parameter.AppendSql("     , C.ENTSABUN, C.GBDNTPRT, C.GBPANPRT, C.GBPAN, C.PMUND9, C.PPANREMARK1                ");
            parameter.AppendSql("     , C.PPANREMARK2, C.GBMIRPRINT, C.SANGDAM, C.DPANDATE, C.DPAN12, C.DPAN13              ");
            parameter.AppendSql("     , C.TONGBODATE, C.PRTSABUN, C.LTDCODE2                                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.GjJong='56'                                                                       "); //학생신검만
            parameter.AppendSql("   AND c.PPanDrno IS NOT NULL                                                              "); //판정된것만
            parameter.AppendSql("   AND c.Rdate IS NOT NULL                                                                 "); //판정된것만
            parameter.AppendSql("   AND c.GbPanPrt ='Y'                                                                     "); //판정된것만            
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }            
            parameter.AppendSql("   AND a.Pano  = b.Pano(+)                                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");
            parameter.AppendSql(" ORDER BY c.Class, c.Ban, c.Bun                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }

        public List<HIC_JEPSU_PATIENT_SCHOOL> GetItembyJepDate(string strFrDate, string strToDate, string strChkRePrint, string strSName, long nLtdCode, string strClass, string strBan)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.PANO, a.SNAME, a.SEX, a.AGE              ");
            parameter.AppendSql("     , a.WRTNO,a.LTDCODE, b.JUMIN2, c.CLASS, c.BAN, c.BUN, c.ROWID                         ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_PATIENT b, ADMIN.HIC_SCHOOL_NEW c    ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                   ");
            parameter.AppendSql("   AND a.GBSTS NOT IN ('D')                                                                ");
            parameter.AppendSql("   AND a.GJJONG = '56'                                                                     "); //학생신검만
            parameter.AppendSql("   AND c.RDATE IS NOT NULL                                                                 "); //판정된것만
            if (strChkRePrint == "1")
            {
                parameter.AppendSql("   AND c.GbDntPrt = 'Y'                                                                 "); //인쇄된것
            }
            else
            {
                parameter.AppendSql("   AND ( c.GBDNTPRT IS NULL OR c.GBDNTPRT ='')                                          "); //인쇄안된것
            }
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                             ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            }
            if (strClass != "*전체" && !strClass.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND c.CLASS = :CLASS                                                                ");
            }
            if (strBan != "*전체" && !strBan.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND c.BAN = :BAN                                                                    ");
            }

            parameter.AppendSql("   AND a.PANO  = b.PANO(+)                                                                 ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO(+)                                                                ");
            parameter.AppendSql(" ORDER BY c.CLASS, c.BAN, c.BUN, a.SNAME                                                   ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", strSName);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strClass != "*전체" && !strClass.IsNullOrEmpty())
            {
                parameter.Add("CLASS", strClass);
            }
            if (strBan != "*전체" && !strBan.IsNullOrEmpty())
            {
                parameter.Add("BAN", strBan);
            }

            return ExecuteReader<HIC_JEPSU_PATIENT_SCHOOL>(parameter);
        }
    }
}
