namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuLtdResBohum1Repository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuLtdResBohum1Repository()
        {
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateGjYearGjBangi(string strFrDate, string strToDate, string strGjYear, string strGjBangi, string strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.NAME, a.LTDCODE                                                               ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.JEPDATE,'YYYY-MM-DD')) MINDATE                                    ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.JEPDATE,'YYYY-MM-DD')) MAXDATE                                    ");
            parameter.AppendSql("     , COUNT(c.PANJENG) SUM                                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_LTD b, ADMIN.HIC_RES_BOHUM1 c    ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.Wrtno = c.Wrtno(+)                                                            ");
            parameter.AppendSql("   AND a.GjJong IN ('11','12','14','41','42')                                          ");
            parameter.AppendSql("   AND a.GjChasu = '1'                                                                 ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.GbInwon in ('21','22','23','31','32','64','65','66','67','68')                ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                                           ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                              ");            
            switch (strGjBangi)
            {
                case "상반기":
                    parameter.AppendSql("   AND a.GjBangi = '1'                                                         ");
                    break;
                case "하반기":
                    parameter.AppendSql("   AND a.GjBangi = '2'                                                         ");
                    break;
                default:
                    break;
            }
            parameter.AppendSql("   AND a.LtdCode = b.Code(+)                                                           ");

            if (strLtdCode != "")
            {
                parameter.AppendSql("   AND a.LtdCode = :LTDCODE                                                        ");
            }
            parameter.AppendSql(" GROUP BY b.Name, a.LtdCode                                                            ");
            parameter.AppendSql("HAVING COUNT(c.PANJENG) > 0                                                            "); 
            parameter.AppendSql(" ORDER BY b.Name, a.LtdCode                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyGjJongPanjengDate(string strFDate, string strTDate, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.GJYEAR, C.JUMIN2, A.SNAME, TO_CHAR(A.JEPDATE,'YYYYMMDD') JEPDATE, B.PANJENGR3, B.PANJENGR6    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU A, ADMIN.HIC_RES_BOHUM1 B, ADMIN.HIC_PATIENT C                ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                                                               ");
            parameter.AppendSql("   AND A.PANO = C.PANO                                                                                 ");
            parameter.AppendSql("   AND (PANJENGR3 = '1' OR PANJENGR6 = '1')                                                            ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                                                                                ");
            parameter.AppendSql("   AND A.PANJENGDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND A.PANJENGDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND A.PANO > '1000'                                                                                 ");
            parameter.AppendSql(" ORDER BY A.WRTNO                                                                                      ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetPanjengPatListbyJepDate(string fstrJob, string fstrFDate, string fstrTDate, bool fbSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT /*+ INDEX(ADMIN.HIC_JEPSU INX_HICJEPSU2) */ a.WRTNO,a.SNAME                                   ");
            parameter.AppendSql("     , TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.GJCHASU,a.GJJONG,a.GBSTS,a.LTDCODE, a.ERFLAG             ");
            parameter.AppendSql("     , a.PTNO, a.UCODES,a.SEX,a.AGE,b.PANJENGDRNO,b.MUNJINDRNO, a.GBCHUL                                   ");
            parameter.AppendSql("     , ADMIN.FC_HIC_GJJONG_NAME(a.GJJONG, a.UCODES) EXNAME                                           ");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME, a.JONGGUMYN                                          ");
            parameter.AppendSql("     , ADMIN.FC_HIC_DOCTOR_NAME(b.PANJENGDRNO) PANDRNAME                                             ");
            parameter.AppendSql("     , DECODE(b.Panjeng, '1','A','2','B','4','R1','5','R2','6','D1','7','D2','8','D','') Panjeng           ");
            parameter.AppendSql("     , DECODE(a.GBSTS, '0','N','1','N','Y') GBSTSNM                                                        ");
            parameter.AppendSql("     , a.SANGDAMDRNO,b.PANJENGDRNO,a.GBAUTOPAN, a.PANO, a.IEMUNNO                                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU      a                                                                        ");
            parameter.AppendSql("     , ADMIN.HIC_RES_BOHUM1 b                                                                        ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                         ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                                   ");
            parameter.AppendSql("   AND a.GJCHASU = '1'                                                                                     ");
            parameter.AppendSql("   AND a.GJJONG IN ('11','12','13','14','41','42','43')                                                    ");
            parameter.AppendSql("   AND a.SANGDAMDRNO > 0                                                                                   "); //상담완료
            parameter.AppendSql("   AND (a.GBDENTONLY IS NULL OR a.GBDENTONLY != 'Y')                                                       "); //구강검진만 한 경우 제외
            parameter.AppendSql("   AND a.GBSTS > '1'                                                                                       ");
            parameter.AppendSql("   AND a.GBMUNJIN1 != 'N'                                                                                  "); //결과입력완료 및 문진입력
            parameter.AppendSql("   AND b.WRTNO > 0                                                                                         ");

            if (fstrJob == "N")
            {
                parameter.AppendSql("   AND (b.GBPANJENG IS NULL OR b.GBPANJENG <> 'Y')                                                     ");
            }
            else if (fstrJob == "Y")
            {
                parameter.AppendSql("   AND b.GBPANJENG = 'Y'                                                                               ");
            }

            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                                ");

            if (fbSort)
            {
                parameter.AppendSql(" ORDER BY a.JEPDATE,a.GJJONG                                                                           ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY a.SNAME,a.GJJONG                                                                             ");
            }

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno, string argChasu)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.Kiho,a.GjJong,a.JikGbn,a.Sname,a.Sex,a.Age,a.GjChasu                           ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,b.PanjengDrno                                           ");
            parameter.AppendSql("     , TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate                                                 ");
            parameter.AppendSql("     , TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate                                                   ");
            if (argChasu != "Y")
            {
                parameter.AppendSql("  FROM  ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b                                      ");
            }
            else
            {
                parameter.AppendSql("  FROM  ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM2 b                                      ");
            }
            parameter.AppendSql(" WHERE a.Wrtno = b.Wrtno                                                                               ");
            parameter.AppendSql("   AND a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                      ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                      ");
            parameter.AppendSql("   AND a.MIRNO1 = :MIRNO                                                                               ");

            parameter.Add("FRDATE", argFrDate);
            parameter.Add("TODATE", argToDate);
            parameter.Add("MIRNO", argMirno);

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyMunjinDrNo(long gnHicLicense, string b02_PANJENG_DRNO, long nMunjinDrNo, string idNumber)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.SName,a.Age,a.Sex,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate      ");
            parameter.AppendSql("     , a.GjJong,a.SANGDAMDRNO,b.MunjinDrno,a.PTno, b.PANJENGDRNO, A.SExams             ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, HIC_RES_BOHUM1ADMIN. b                           ");
            parameter.AppendSql(" WHERE a.JepDate>=TRUNC(SYSDATE-14)                                                    ");
            parameter.AppendSql("   AND a.JepDate>=TRUNC(SYSDATE-3)                                                     ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               "); //접수취소는 제외
            parameter.AppendSql("   AND a.GjJong IN ('11','12','13','14')                                               ");
            parameter.AppendSql("   AND a.GbSts >= '2'                                                                  "); //결과 입력완료
            parameter.AppendSql("   AND a.PanjengDate IS NULL                                                           "); //미판정
            parameter.AppendSql("   AND a.UCodes IS NULL                                                                "); //특수는 제외
            parameter.AppendSql("   AND (a.GbAutoPan IS NULL OR a.GbAutoPan='')                                         "); //자동판정 미점검만
            parameter.AppendSql("   AND (a.Pano = 999 OR a.GbMunjin1 <> 'N')                                            "); //결과입력완료 및 문진입력"
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                            ");
            parameter.AppendSql("   AND b.MunjinDrno > 0                                                                ");
            if (gnHicLicense > 0)
            {
                //김중구소장님은 전체의사 조회가 가능하게 수정
                if (b02_PANJENG_DRNO == "ALL")
                {
                    if (nMunjinDrNo != 0)
                    {
                        parameter.AppendSql("   AND b.MUNJINDRNO = :MUNJINDRNO                                          ");
                    }
                }
                else
                {
                    if (nMunjinDrNo != 0)
                    {
                        parameter.AppendSql("   AND b.MUNJINDRNO = :MUNJINDRNO                                          ");
                    }
                    else if (gnHicLicense == 32158 || gnHicLicense == 36531 || gnHicLicense == 39444)
                    {
                        parameter.AppendSql("   AND b.MUNJINDRNO IN (71735, 71797, 61439)                               "); //원소준과장,이주령,주철효과장
                    }
                    else
                    {
                        parameter.AppendSql("   AND b.MunjinDrno NOT IN (71735, 71797, 61439)                           "); //원소준과장,이주령,주철효과장 제외
                        parameter.AppendSql("   AND b.MUNJINDRNO = :B02_PANJENG_DRNO                                    ");
                    }
                }
            }

            parameter.Add("MUNJINDRNO", nMunjinDrNo);
            parameter.Add("b02_PANJENG_DRNO", b02_PANJENG_DRNO);

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyGjYearGjjong(string strGjYear, string strGjJong, long nLtdCode, string strJob, List<string> strFirstGjJong, List<string> strDAT, string strChkFirst1, string strChkFirst2, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SName,a.Age,a.Sex,TO_CHAR(a.JEPDate,'YYYY-MM-DD') JEPDate,a.GjJong    ");
            parameter.AppendSql("     , a.LtdCode,a.BuseName,a.GjChasu,a.Pano,a.ChungguYN,DentChungguYN                 ");
            parameter.AppendSql("     , a.GjYear,a.GjBangi,a.GbSTS                                                      ");
            parameter.AppendSql("     , b.Panjeng, b.Panjengb1, b.Panjengb2, b.Panjengb3                                ");
            parameter.AppendSql("     , b.Panjengb4,b.Panjengb5,b.Panjengb6,b.Panjengb7,b.Panjengb8                     ");
            parameter.AppendSql("     , b.Panjengr1,b.Panjengr2,b.Panjengr3,b.Panjengr4,b.Panjengr5                     ");
            parameter.AppendSql("     , b.Panjengr6,b.Panjengr7,b.Panjengr8,b.Panjengr9                                 ");
            parameter.AppendSql("     , TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate                                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b                           ");
            parameter.AppendSql(" WHERE a.GJYEAR = :GJYEAR                                                              ");
            parameter.AppendSql("   AND a.GbSTS <> 'D'                                                                  ");//접수취소(삭제)
            if (!nLtdCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            if (strGjJong == "**")
            {
                parameter.AppendSql("   AND a.GJJONG IN (:FIRSTGJJONG)                                                  ");
            }
            else
            {
                parameter.AppendSql("   AND a.GJJONG  = :GJJONG                                                         ");
            }
            if (strJob == "4" || strJob == "5" || strJob == "6")
            {
                parameter.AppendSql("   AND b.Panjeng IN('3','5')                                                       ");
            }
            if (strDAT != null)
            {
                parameter.AppendSql("   AND b.PANJENG IN (:PANJENG)                                                     ");
            }
            switch (strChkFirst1)
            {
                case "1":
                    parameter.AppendSql("   AND PANJENGB1 = '1'                                                         ");
                    break;
                case "2":
                    parameter.AppendSql("   AND PANJENGB2 = '1'                                                         ");
                    break;
                case "3":
                    parameter.AppendSql("   AND PANJENGB3 = '1'                                                         ");
                    break;
                case "4":
                    parameter.AppendSql("   AND PANJENGB4 = '1'                                                         ");
                    break;
                case "5":
                    parameter.AppendSql("   AND PANJENGB5 = '1'                                                         ");
                    break;
                case "6":
                    parameter.AppendSql("   AND PANJENGB6 = '1'                                                         ");
                    break;
                case "7":
                    parameter.AppendSql("   AND PANJENGB7 = '1'                                                         ");
                    break;
                case "8":
                    parameter.AppendSql("   AND PANJENGB8 = '1'                                                         ");
                    break;
                default:
                    break;
            }
            switch (strChkFirst2)
            {
                case "01":
                    parameter.AppendSql("   AND PANJENGR1 = '1'                                                         ");
                    break;
                case "02":
                    parameter.AppendSql("   AND PANJENGR2 = '1'                                                         ");
                    break;
                case "03":
                    parameter.AppendSql("   AND PANJENGR3 = '1'                                                         ");
                    break;
                case "04":
                    parameter.AppendSql("   AND PANJENGR4 = '1'                                                         ");
                    break;
                case "05":
                    parameter.AppendSql("   AND PANJENGR5 = '1'                                                         ");
                    break;
                case "06":
                    parameter.AppendSql("   AND PANJENGR6 = '1'                                                         ");
                    break;
                case "07":
                    parameter.AppendSql("   AND PANJENGR7 = '1'                                                         ");
                    break;
                case "08":
                    parameter.AppendSql("   AND PANJENGR8 = '1'                                                         ");
                    break;
                case "09":
                    parameter.AppendSql("   AND PANJENGR9 = '1'                                                         ");
                    break;
                case "10":
                    parameter.AppendSql("   AND PANJENGR10 = '1'                                                        ");
                    break;
                case "11":
                    parameter.AppendSql("   AND PANJENGR11 = '1'                                                        ");
                    break;
                default:
                    break;
            }
            if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.SName,a.JEPDate,a.GjJong                                               ");
            }
            else if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.JEPDate,a.SName,a.GjJong                                               ");
            }

            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);            
            if (strGjJong == "**")
            {
                parameter.AddInStatement("GJJONG", strFirstGjJong);
            }
            else
            {
                parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!nLtdCode.IsNullOrEmpty())
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strDAT != null)
            {
                parameter.Add("PANJENG", strDAT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public long GetCountbyJepDateLtdCodeGjYearGjBangi_New(string strFDate, string strTDate, string fstrLtdCode, string strYear, string strBangi)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b               ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                   ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                ");
            parameter.AppendSql("   AND a.GbInwon IN ('21','22','23','31','32','64','65','66','67','68')    ");
            if (strYear != "")
            {
                parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                              ");
            }
            if (strBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                            ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");
            parameter.AppendSql("   AND b.Panjengdrno > 0                                                   ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (strYear != "")
            {
                parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (fstrLtdCode != "")
            {
                parameter.Add("LTDCODE", fstrLtdCode);
            }

            return ExecuteScalar<long>(parameter);
        }

        public long GetCountbyJepDateLtdCodeGjYearGjBangi(string strFDate, string strTDate, string fstrLtdCode, string strYear, string strBangi)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b   ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:JEPDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                       ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                    ");
            parameter.AppendSql("   AND a.GbInwon IN ('31','67')                                ");
            parameter.AppendSql("   AND a.UCodes IS NULL                                        ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                      ");
            parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                    ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                    ");
            parameter.AppendSql("   AND b.Panjengdrno > 0                                       ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (fstrLtdCode != "")
            {
                parameter.Add("LTDCODE", fstrLtdCode);
            }

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateGjYearLtdCode(string fstrFDate, string fstrTDate, string strYear, string strBangi, string fstrLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.WRTNO WRTNO1,a.Pano,a.SEX,a.Age,a.JikGbn,a.SName                                  ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.GjJong,GbInwon                            ");
            parameter.AppendSql("     , TO_CHAR(a.IpsaDate,'YYYY-MM-DD') IpsaDate2,a.GjYear,a.GjBangi                       ");
            //parameter.AppendSql("     , b.*                                                                                 ");
            parameter.AppendSql("     , b.WRTNO, b.HEIGHT, b.WEIGHT, b.BIMAN, b.EYE_L, b.EYE_R, b.EAR_L, b.EAR_R            ");
            parameter.AppendSql("     , b.BLOOD_H, b.BLOOD_L, b.URINE1, b.URINE2, b.URINE3, b.URINE4, b.BLOOD1              ");
            parameter.AppendSql("     , b.BLOOD2, b.BLOOD3, b.BLOOD4, b.BLOOD5, b.BLOOD6, b.LIVER1, b.LIVER2, b.LIVER3      ");
            parameter.AppendSql("     , b.XRAYGBN, b.XRAYRES, b.EKG, b.CYTO1, b.CYTO2, b.EXAMFLAG, b.SICK11, b.SICK12       ");
            parameter.AppendSql("     , b.SICK13, b.SICK21, b.SICK22, b.SICK23, b.SICK31, b.SICK32, b.SICK33, b.GAJOK1      ");
            parameter.AppendSql("     , b.GAJOK2, b.GAJOK3, b.GAJOK4, b.GAJOK5, b.GAJOK6, b.ROSICK                          ");
            parameter.AppendSql("     , b.ROSICKNAME, b.SIKSENG, b.DRINK1, b.DRINK2, b.SMOKING1, b.SMOKING2, b.SMOKING3     ");
            parameter.AppendSql("     , b.SPORTS, b.ANNOUNE, b.WOMAN1, b.WOMAN2, b.WOMAN3, b.MUNJINFLAG, b.MUNJINENTDATE    ");
            parameter.AppendSql("     , b.MUNJINENTSABUN, b.OLDBYENG, b.OLDBYENG1, b.OLDBYENG2                              ");
            parameter.AppendSql("     , b.OLDBYENG3, b.OLDBYENG4, b.OLDBYENG5, b.OLDBYENG6, b.OLDBYENG7, b.HABIT, b.HABIT1  ");
            parameter.AppendSql("     , b.HABIT2, b.HABIT3, b.HABIT4, b.HABIT5, b.JINCHAL1, b.JINCHAL2, b.PANJENG           ");
            parameter.AppendSql("     , b.PANJENGB1, b.PANJENGB2, b.PANJENGB3, b.PANJENGB4, b.PANJENGB5                     ");
            parameter.AppendSql("     , b.PANJENGB6, b.PANJENGB7, b.PANJENGB8, b.PANJENGR1, b.PANJENGR2, b.PANJENGR3        ");
            parameter.AppendSql("     , b.PANJENGR4, b.PANJENGR5, b.PANJENGR6, b.PANJENGR7, b.PANJENGR8, b.PANJENGR9        ");
            parameter.AppendSql("     , b.PANJENGR10, b.PANJENGR11, b.PANJENGETC, b.PANJENGDATE, b.GUNDATE                  ");
            parameter.AppendSql("     , b.TONGBOGBN, b.TONGBODATE, b.PANJENGDRNO, b.SOGEN, b.XRAYNO, b.IPSADATE, b.WOMB01   ");
            parameter.AppendSql("     , b.WOMB02, b.WOMB03, b.WOMB04, b.WOMB05, b.WOMB06, b.WOMB07, b.WOMB08, b.WOMB09      ");
            parameter.AppendSql("     , b.WOMB10, b.WOMB11, b.OLDBYENGNAME, b.GBPRINT, b.MUNJINDRNO                         ");
            parameter.AppendSql("     , b.JINREMARK, b.GBPANJENG, b.PANJENGB9, b.PANJENGB_ETC, b.PANJENGR_ETC, b.ADDSOGEN   ");
            parameter.AppendSql("     , b.SMOKING4, b.SMOKING5, b.PANJENGB_ETC_DTL, b.WAIST, b.T_STAT01, b.T_STAT02         ");
            parameter.AppendSql("     , b.T_STAT11, b.T_STAT12, b.T_STAT21, b.T_STAT22, b.T_STAT31                          ");
            parameter.AppendSql("     , b.T_STAT32, b.T_STAT41, b.T_STAT42, b.T_STAT51, b.T_STAT52, b.T_GAJOK1, b.T_GAJOK2  ");
            parameter.AppendSql("     , b.T_GAJOK3, b.T_GAJOK4, b.T_GAJOK5, b.T_BLIVER, b.T_SMOKE1, b.T_SMOKE2, b.T_SMOKE3  ");
            parameter.AppendSql("     , b.T_SMOKE4, b.T_SMOKE5, b.T_DRINK1, b.T_DRINK2, b.T_ACTIVE1                         ");
            parameter.AppendSql("     , b.T_ACTIVE2, b.T_ACTIVE3, b.T40_FEEL1, b.T40_FEEL2, b.T40_FEEL3, b.T40_FEEL4        ");
            parameter.AppendSql("     , b.T66_INJECT, b.T66_STAT1, b.T66_STAT2, b.T66_STAT3, b.T66_STAT4, b.T66_STAT5       ");
            parameter.AppendSql("     , b.T66_STAT6, b.T66_FEEL1, b.T66_FEEL2, b.T66_FEEL3, b.T66_MEMORY1                   ");
            parameter.AppendSql("     , b.T66_MEMORY2, b.T66_MEMORY3, b.T66_MEMORY4, b.T66_MEMORY5, b.T66_FALL, b.T66_URO   ");
            parameter.AppendSql("     , b.PANJENGC1, b.PANJENGC2, b.PANJENGC3, b.PANJENGC4, b.PANJENGD11, b.PANJENGD12      ");
            parameter.AppendSql("     , b.PANJENGD13, b.PANJENGD21, b.PANJENGD22, b.PANJENGD23, b.PANJENGSAHU               ");
            parameter.AppendSql("     , b.PANJENGC5, b.SANGDAM, b.SANGDAM2, b.GBSIKSA, b.T40_FEEL, b.T66_STAT, b.FOOT1      ");
            parameter.AppendSql("     , b.FOOT2, b.BALANCE, b.OSTEO, b.LIFESOGEN, b.T_STAT61, b.T_STAT62, b.PANJENGU1       ");
            parameter.AppendSql("     , b.PANJENGU2, b.PANJENGU3, b.PANJENGU4, b.PANJENGSAHU2, b.PANJENGSAHU3               ");
            parameter.AppendSql("     , b.WORKYN, b.PRTSABUN, b.T_STAT71, b.T_STAT72, b.SOGENB, b.GBGONGHU, b.TMUN0001      ");
            parameter.AppendSql("     , b.TMUN0002, b.TMUN0003, b.TMUN0004, b.TMUN0005, b.TMUN0006, b.TMUN0007, b.TMUN0008  ");
            parameter.AppendSql("     , b.TMUN0009, b.TMUN0010, b.TMUN0011, b.TMUN0012, b.TMUN0013                          ");
            parameter.AppendSql("     , b.TMUN0014, b.TMUN0015, b.TMUN0016, b.TMUN0017, b.TMUN0018, b.TMUN0019, b.TMUN0020  ");
            parameter.AppendSql("     , b.TMUN0021, b.TMUN0022, b.TMUN0023, b.TMUN0024, b.TMUN0025, b.TMUN0026, b.TMUN0027  ");
            parameter.AppendSql("     , b.TMUN0028, b.TMUN0029, b.TMUN0030, b.TMUN0031, b.TMUN0032                          ");
            parameter.AppendSql("     , b.TMUN0033, b.TMUN0034, b.TMUN0035, b.TMUN0036, b.TMUN0037, b.TMUN0038, b.TMUN0039  ");
            parameter.AppendSql("     , b.TMUN0040, b.TMUN0041, b.TMUN0042, b.TMUN0043, b.TMUN0044, b.TMUN0045, b.TMUN0046  ");
            parameter.AppendSql("     , b.TMUN0047, b.TMUN0048, b.TMUN0049, b.TMUN0050, b.TMUN0051                          ");
            parameter.AppendSql("     , b.TMUN0052, b.TMUN0053, b.TMUN0054, b.TMUN0055, b.TMUN0056, b.TMUN0057, b.TMUN0058  ");
            parameter.AppendSql("     , b.TMUN0059, b.TMUN0060, b.TMUN0061, b.TMUN0062, b.TMUN0063, b.TMUN0064, b.TMUN0065  ");
            parameter.AppendSql("     , b.TMUN0066, b.TMUN0067, b.TMUN0068, b.TMUN0069, b.TMUN0070                          ");
            parameter.AppendSql("     , b.TMUN0071, b.TMUN0072, b.TMUN0073, b.TMUN0074, b.TMUN0075, b.TMUN0076, b.TMUN0077  ");
            parameter.AppendSql("     , b.TMUN0078, b.TMUN0079, b.TMUN0080, b.TMUN0081, b.TMUN0082, b.TMUN0083, b.TMUN0084  ");
            parameter.AppendSql("     , b.TMUN0085, b.TMUN0086, b.TMUN0087, b.TMUN0088, b.TMUN0089                          ");
            parameter.AppendSql("     , b.TMUN0090, b.TMUN0091, b.TMUN0092, b.TMUN0093, b.TMUN0094, b.TMUN0095, b.PANJENGC  ");
            parameter.AppendSql("     , b.PANJENGD, b.SLIP_SMOKE, b.SLIP_DRINK, b.SLIP_ACTIVE, b.SLIP_FOOD, b.SLIP_BIMAN    ");
            parameter.AppendSql("     , b.SOGENC, b.SOGEND, b.SLIP_PHQ, b.SLIP_KDSQ, b.SLIP_OLDMAN                          ");
            parameter.AppendSql("     , b.SLIP_LIFESOGEN1, b.SLIP_LIFESOGEN2, b.PANJENGB10, b.PANJENGR12, b.TMUN0096        ");
            parameter.AppendSql("     , b.TMUN0097, b.TMUN0098, b.TMUN0099, b.TMUN0100, b.TMUN0101, b.TMUN0102, b.TMUN0103  ");
            parameter.AppendSql("     , b.TMUN0104, b.SIM_RESULT1, b.SIM_RESULT2, b.SIM_RESULT3                             ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b                               ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'yyyy-mm-dd')                                         ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'yyyy-mm-dd')                                         ");
            parameter.AppendSql("   AND a.GJCHASU = '1'                                                                     "); //1차만
            if (strYear != "")
            {
                parameter.AppendSql("   AND a.GJYEAR  = :GJYEAR                                                             ");
            }
            if (strBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                            ");
            }
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            //parameter.AppendSql("   AND a.GbInwon IN ('31','67')                                                            ");
            parameter.AppendSql("   AND a.GbInwon IN ('21', '22', '23', '31', '32', '64', '65', '66', '67', '68')           ");
            //parameter.AppendSql("   AND a.UCodes IS NULL                                                                    ");
            parameter.AppendSql(" ORDER BY a.JepDate,b.WRTNO                                                                ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            if (strYear != "")
            {
                parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strBangi != "")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (fstrLtdCode != "")
            {
                parameter.Add("LTDCODE", fstrLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDate()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.SangdamDrno,b.MunjinDrno                      ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b   ");
            parameter.AppendSql( "WHERE a.JepDate>=TRUNC(SYSDATE-100)                           ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                       ");
            parameter.AppendSql("   AND a.GjJong IN ('11','12','13','14','41','42','43')        ");
            parameter.AppendSql("   AND a.PanjengDate IS NULL                                   ");
            parameter.AppendSql("   AND a.SangdamDrno > 0                                       ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                      ");
            parameter.AppendSql("   AND b.WRTNO > 0                                             ");
            parameter.AppendSql("   AND (b.MunjinDrno IS NULL OR b.MunjinDrno=0)                ");
            parameter.AppendSql(" ORDER BY a.SangdamDrno,a.WRTNO                                ");
            
            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyPaNoJepDate(long argPano, string argJepDate, string strRowNum = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, b.PANJENG         ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b               ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                      ");
            parameter.AppendSql("   AND a.JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                         ");
            parameter.AppendSql("   AND a.GbSTS <> 'D'                                                      "); //접수취소(삭제)는 제외
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                  ");
            parameter.AppendSql("   AND b.Panjeng IS NOT NULL                                               ");
            if (strRowNum == "")
            {
                parameter.AppendSql("   AND ROWNUM < 3                                                      ");
            }
            parameter.AppendSql(" ORDER BY a.JepDate DESC                                                   ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyPaNoJepDateGjJong(long argPano, string argJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,b.Panjeng   ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b       ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                              ");
            parameter.AppendSql("   AND a.JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.GbSTS <> 'D'                                              "); //접수취소(삭제)는 제외
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                          ");
            parameter.AppendSql("   AND a.GJJONG IN ('51', '50')                                    ");
            parameter.AppendSql(" ORDER BY a.JEPDATE DESC                                           ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argJepDate);

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyPaNoJepDateGjYear(long argPano, string argJepDate, string argGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,b.Panjeng   ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_BOHUM1 b       ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                              ");
            parameter.AppendSql("   AND a.JEPDATE < TO_DATE(:JEPDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.GJYEAR < :GJYEAR                                          ");
            parameter.AppendSql("   AND a.GbSTS <> 'D'                                              "); //접수취소(삭제)는 제외
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                          ");
            parameter.AppendSql("   AND b.Panjeng IS NOT NULL                                       ");
            parameter.AppendSql(" ORDER BY a.JepDate DESC                                           ");

            parameter.Add("PANO", argPano);
            parameter.Add("JEPDATE", argJepDate);
            parameter.Add("GJYEAR", argGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public List<HIC_JEPSU_LTD_RES_BOHUM1> GetItembyJepDateGjYearGjBangi_GenSpc(string strFrDate, string strToDate, string strGjYear, string strGjBangi, string strJob, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.Name, a.Ltdcode, COUNT(*) CNT                                                 ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MinDate                                    ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MaxDate                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_LTD b                                  ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE,'YYYY-MM-DD')                                      ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                                           ");
            //parameter.AppendSql("   AND a.UCodes IS NOT NULL                                                            ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                              ");
            parameter.AppendSql("   AND a.PANJENGDRNO IS NOT NULL                                                       ");
            if (strGjBangi != "")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                        ");
            }
            if (strJob == "1")
            {
                parameter.AppendSql("   AND a.GjJong IN ('11','12','14','41','42','23')                                 ");
            }
            else if (strJob == "2")
            {
                parameter.AppendSql("   AND a.Gjjong IN('22','24','30')                                                 ");
            }
            else
            {
                parameter.AppendSql("   AND a.GJJONG IN ('69')                                                          ");
            }
            
            parameter.AppendSql("   AND a.LTDCODE = b.CODE(+)                                                           ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            else
            {
                //parameter.AppendSql("   AND a.LtdCode IN (SELECT LtdCode FROM HIC_BOGENLTD                              ");
                //parameter.AppendSql("                      WHERE GeDate <= TO_DATE(:GJYEAR || '- 01-01','YYYY-MM-DD')   ");
                //parameter.AppendSql("                        AND (DelDate IS NULL                                       ");
                //parameter.AppendSql("                         OR DelDate>=TO_DATE(:GJYEAR || '- 01-01','YYYY-MM-DD'))   ");
                //parameter.AppendSql("                      GROUP BY LtdCode)                                            ");
                parameter.AppendSql("  AND a.LtdCode IN ( SELECT ID FROM HIC_OSHA_SITE )                                  ");

            }
            parameter.AppendSql(" GROUP BY b.NAME, a.LTDCODE                                                            ");
            parameter.AppendSql(" ORDER BY b.NAME, a.LTDCODE                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strGjBangi != "")
            {
                parameter.Add("GJBANGI", strGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_LTD_RES_BOHUM1>(parameter);
        }

        public HIC_RES_BOHUM1 GetItembyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Habit1,Habit2,Habit3,Habit4,Habit5      ");
            parameter.AppendSql("  FROM ADMIN.HIC_RES_BOHUM1              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                        ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_RES_BOHUM1>(parameter);
        }
    }
}
