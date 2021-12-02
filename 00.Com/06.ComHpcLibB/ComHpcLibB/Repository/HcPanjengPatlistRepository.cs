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
    public class HcPanjengPatlistRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HcPanjengPatlistRepository()
        {
        }

        public List<HC_PANJENG_PATLIST> GetPanjengPatListbyJepDate(PAN_PATLIST_SEARCH sItem)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT /*+ INDEX(KOSMOS_PMPA.HIC_JEPSU INX_HICJEPSU2) */ a.WRTNO, a.SNAME                                      ");
            parameter.AppendSql("     , TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.BURATE, a.GJCHASU, a.GJJONG, a.GBSTS, a.LTDCODE, a.ERFLAG    ");
            parameter.AppendSql("     , a.PTNO, a.UCODES, a.SEX, a.AGE, a.PANJENGDRNO, DECODE(a.GBCHUL, 'Y', 'Y', '') AS GBCHUL                 ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_GJJONG_NAME(a.GJJONG, a.UCODES) EXNAME                                               ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE) LTDNAME                                                           ");
            parameter.AppendSql("     , DECODE(a.JONGGUMYN, '1', 'Y', '') AS  JONGGUMYN                                                         ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_XRAYREAD_CHK(a.PTNO, TO_CHAR(a.JEPDATE, 'YYYY-MM-DD')) AS XREAD                      "); //미판독 여부 (미판독 : N)
            parameter.AppendSql("     , a.CLASS, a.BAN, a.BUN, a.GBADDPAN                                                                       ");
            if (sItem.JOB == "1")
            {
                parameter.AppendSql("     , KOSMOS_PMPA.FC_READ_DRNAME_SABUN(a.SANGDAMDRNO) AS DOCTOR                                           ");
            }
            else if (sItem.JOB == "2")
            {
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_DOCTOR_NAME(a.PANJENGDRNO) AS DOCTOR                                             ");
            }
            else
            {
                parameter.AppendSql("     , CASE WHEN a.PANJENGDRNO = 0 THEN KOSMOS_PMPA.FC_READ_DRNAME_SABUN(a.SANGDAMDRNO)                    ");
                parameter.AppendSql("            ELSE KOSMOS_PMPA.FC_HIC_DOCTOR_NAME(a.PANJENGDRNO) END AS DOCTOR                               ");
            }
            parameter.AppendSql("     , DECODE(a.GBSTS, '0','N','1','N','Y') GBSTSNM                                                            ");
            parameter.AppendSql("     , a.SANGDAMDRNO, a.GBAUTOPAN, a.PANO, a.IEMUNNO                                                           ");
            parameter.AppendSql("     , KOSMOS_OCS.FC_BAS_PATIENT_JUMINNO(a.PTNO) JUMIN                                                         ");
            parameter.AppendSql("     , KOSMOS_OCS.FC_HIC_PATIENT_JUMIN2(a.PTNO) JUMIN2                                                         ");
            parameter.AppendSql("     , a.GWRTNO                                                                                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                                                                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                                   ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                                       ");

            //접수번호 입력하여 선택시
            if (!sItem.WRTNO.IsNullOrEmpty() && sItem.WRTNO != 0)
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                                    ");
                parameter.AppendSql("   AND a.GBSTS NOT IN ('0','D')                                                                            ");
            }
            //수검자명 입력하여 선택시
            else if (sItem.SNAME.To<string>("").Trim() != "")
            {
                parameter.AppendSql("   AND a.SNAME LIKE :SNAME                                                                                 ");
                parameter.AppendSql("   AND a.GBSTS NOT IN ('0','D')                                                                            ");
                parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                         ");
                parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                         ");
                parameter.AppendSql("   AND a.GJJONG NOT IN ('31', '35')                                                        ");
            }
            //명단조회시
            else
            {
                parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                             ");
                parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                             ");

                //검진종류 분류 Query : GJJONG IN (' ... ')
                switch (sItem.JOBGUBUN)
                {
                    case "HIC":
                        parameter.AppendSql("   AND a.GJJONG IN ('11', '14')                                                                        ");
                        break;
                    case "SPC":
                        parameter.AppendSql("   AND a.GJJONG IN ('16', '23', '28')                                                                  ");
                        break;
                    case "REC":
                        parameter.AppendSql("   AND a.GJJONG IN ('21', '22', '24', '27', '29', '30', '32', '33')                                    ");
                        break;
                    case "ETC":
                        if (clsHcVariable.GnHicLicense > 0)
                        {
                            parameter.AppendSql("   AND ( a.GJJONG IN ('62', '50', '51','54') OR (a.GJJONG = '69' AND a.GBADDPAN = 'Y') )                 ");
                        }
                        else
                        {
                            parameter.AppendSql("   AND a.GJJONG IN ('62', '50', '51','54', '69')                                                         ");
                        }

                        break;
                    default:
                        break;
                }

                //사업장코드 
                if (sItem.LTDCODE != 0) { parameter.AppendSql("   AND a.LTDCODE = :LTDCODE "); }
                //검진종류 선택 
                if (sItem.JONG != "**" && !sItem.JONG.IsNullOrEmpty()) { parameter.AppendSql("   AND GJJONG = :GJJONG "); }
                ////수검자명 
                //if (!sItem.SNAME.IsNullOrEmpty()) { parameter.AppendSql("   AND SNAME LIKE :SNAME  "); }

                if (sItem.JOBGUBUN != "ETC")
                {
                    parameter.AppendSql("   AND a.SANGDAMDRNO > 0                                                                                   "); //상담완료
                }

                parameter.AppendSql("   AND a.GBSTS > '1'                                                                                           ");
                parameter.AppendSql("   AND a.WRTNO > 0                                                                                             ");
                parameter.AppendSql("   AND (a.GBDENTONLY IS NULL OR a.GBDENTONLY != 'Y')                                                           "); //구강검진만 한 경우 제외

                //판정유무는 HIC_JEPSU PANJENGDRNO 이용
                if (sItem.JOB == "1")  //미판정
                {
                    parameter.AppendSql("   AND (a.PANJENGDRNO IS NULL OR a.PANJENGDRNO = 0)      ");

                    //2016-04-20 이름을 입력하면 모두 표시함
                    if (sItem.SNAME.IsNullOrEmpty())
                    {
                        if (sItem.LICENSE_VIEW != "**")
                        {
                            parameter.AppendSql("   AND (FIRSTPANDRNO = :DRNO                                                         ");
                            parameter.AppendSql("       OR (FIRSTPANDRNO IS NULL AND (SELECT SANGDAMDRNO FROM KOSMOS_PMPA.HIC_SANGDAM_NEW WHERE WRTNO = A.WRTNO) = :DRNO)) ");
                        }
                    }
                }
                else if (sItem.JOB == "2") //판정
                {
                    parameter.AppendSql("   AND (a.PANJENGDRNO IS NOT NULL OR a.PANJENGDRNO != 0) ");
                    //자동판정만 검색
                    if (sItem.GBAUTOPAN == "Y") { parameter.AppendSql("   AND GBAUTOPAN = 'Y' "); }
                    //2016-04-20 이름을 입력하면 모두 표시함
                    if (sItem.SNAME.IsNullOrEmpty())
                    {
                        if (sItem.LICENSE_VIEW != "**")
                        {
                            parameter.AppendSql("   AND a.PANJENGDRNO = :DRNO   ");
                        }
                    }
                }

                //출장여부
                if (!sItem.CHUL.IsNullOrEmpty()) { parameter.AppendSql("   AND a.GBCHUL = :GBCHUL "); }
                //조회정렬 구분
                if (sItem.SORT == "0") { parameter.AppendSql(" ORDER BY a.GJJONG, a.LTDCODE, a.SNAME  "); }
                else if (sItem.SORT == "1") { parameter.AppendSql(" ORDER BY a.SNAME, a.JEPDATE, a.GJJONG  "); }
                else if (sItem.SORT == "2") { parameter.AppendSql(" ORDER BY a.JEPDATE, a.SNAME, a.GJJONG  "); }
                else { parameter.AppendSql(" ORDER BY KOSMOS_PMPA.FC_HIC_LTDNAME(a.LTDCODE), a.SNAME, a.JEPDATE, a.GJJONG "); }
            }

            if (!sItem.WRTNO.IsNullOrEmpty() && sItem.WRTNO != 0)
            {
                parameter.Add("WRTNO", sItem.WRTNO);
            }
            else if (sItem.SNAME.To<string>("").Trim() != "")
            {
                parameter.AddLikeStatement("SNAME", sItem.SNAME);
                parameter.Add("FRDATE", sItem.FRDATE);
                parameter.Add("TODATE", sItem.TODATE);
            }
            else
            {
                parameter.Add("FRDATE", sItem.FRDATE);
                parameter.Add("TODATE", sItem.TODATE);

                if (sItem.LTDCODE != 0) { parameter.Add("LTDCODE", sItem.LTDCODE); }
                if (sItem.JONG != "**" && !sItem.JONG.IsNullOrEmpty()) { parameter.Add("GJJONG", sItem.JONG, Oracle.DataAccess.Client.OracleDbType.Char); }

                if (sItem.JOB == "1")   //미판정
                {
                    if (sItem.LICENSE_VIEW != "**" && !sItem.LICENSE_VIEW.IsNullOrEmpty())
                    {
                        parameter.Add("DRNO", sItem.LICENSE_VIEW);
                    }
                }
                else if (sItem.JOB == "2")   //판정
                {
                    if (sItem.LICENSE_VIEW != "**" && !sItem.LICENSE_VIEW.IsNullOrEmpty())
                    {
                        parameter.Add("DRNO", sItem.LICENSE_VIEW);
                    }
                }

                if (!sItem.CHUL.IsNullOrEmpty())
                {
                    parameter.Add("GBCHUL", sItem.CHUL, Oracle.DataAccess.Client.OracleDbType.Char);
                }
            }

            return ExecuteReader<HC_PANJENG_PATLIST>(parameter);
        }
    }
}
