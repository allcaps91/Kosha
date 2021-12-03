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
    public class HicJepsuResSpecialLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResSpecialLtdRepository()
        {
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjYearGjBangi(string strFrDate, string strToDate, string strGjYear, string strBangi, string strJob, string strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.PANO, a.SNAME, TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE   ");
            parameter.AppendSql("     , a.GJJONG, a.GJCHASU, a.UCODES, a.GJYEAR, a.GJBANGI, a.SEX           ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU       a                                       ");
            parameter.AppendSql("     , ADMIN.HIC_RES_SPECIAL b                                       ");
            parameter.AppendSql("     , ADMIN.HIC_LTD         c                                       ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                          ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                   ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                ");
            parameter.AppendSql("   AND a.UCODES IS NOT NULL                                                "); //취급물질이 있는 사람만
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                  ");
            parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                ");
            if (strJob == "1") //특수
            {
                parameter.AppendSql("   AND a.GJJONG IN ('11','12','14','23','41')                          ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("   AND a.GJJONG IN ('22','24','30')                                    ");
            }
            else
            {
                parameter.AppendSql("   AND a.GJJONG  IN ('69')                                             ");
            }
            //특수검진인지 점검
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");
            parameter.AppendSql("   AND b.PANJENGDRNO IS NOT NULL                                           "); //판정완료된것만 읽음
            parameter.AppendSql("   AND a.LTDCODE = c.CODE(+)                                               ");
            if (strLtdCode != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                            ");
            }
            parameter.AppendSql(" GROUP BY c.NAME, a.LTDCODE                                                ");
            parameter.AppendSql(" ORDER BY c.NAME, a.LTDCODE                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_LTD>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjYear(string strFrDate, string strToDate, string strRdoChk, string strRdoBook, string strYear, long nLtdCode, string sSort = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LtdCode,a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate, a.Pano                           ");
            parameter.AppendSql("     , a.Sname, a.Age, a.GjJong, a.GjChasu,a.UCodes, a.SExams                                      ");
            parameter.AppendSql("     , a.Mirno1,a.Mirno2,a.Mirno3,a.Pano,a.HEMSNO                                                  ");
            parameter.AppendSql("     , b.GbOHMS,TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate,a.GjYear                           ");
            parameter.AppendSql("     , b.SUCHUPYN, C.GBGUKGO, a.ROWID                                                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_SPECIAL b, ADMIN.HIC_LTD C               ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO                                                                           ");
            parameter.AppendSql("   AND A.LTDCODE = C.CODE                                                                          ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                                          ");
            if (strRdoChk == "0" || strRdoChk == "1")
            {
                parameter.AppendSql("   AND a.GjJong NOT IN ('21','22','24','27','29','30','49','51','50')                          ");
            }
            else if (strRdoChk == "2")
            {
                parameter.AppendSql("   AND a.GjJong IN ('24')                                                                      ");
            }
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                           ");
            parameter.AppendSql("   AND (a.HEMSNO IS NULL OR a.HEMSNO = 0 )                                                         ");
            parameter.AppendSql("   AND a.HEMSMIRSAYU IS NULL                                                                       ");
            parameter.AppendSql("   AND a.UCODES IS NOT NULL                                                                        ");
            parameter.AppendSql("   AND b.PANJENGDRNO IS NOT NULL                                                                   ");
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                    ");
            }
            if (strRdoChk == "0" || strRdoChk == "2")
            {
                parameter.AppendSql("   AND C.GBGUKGO = 'Y'                                                                         ");
            }
            else if (strRdoChk == "1")
            {
                parameter.AppendSql("   AND C.GBGUKGO = 'N'                                                                         ");
            }
            if (strRdoBook == "0")
            {
                parameter.AppendSql("   AND b.SUCHUPYN = 'Y'                                                                        ");
            }
            else
            {
                parameter.AppendSql("   AND (b.SUCHUPYN <> 'Y' or b.SUCHUPYN is null)                                               ");
            }
            if (sSort.IsNullOrEmpty())
            {
                parameter.AppendSql("ORDER BY a.LTDCODE, a.PANO, a.WRTNO                                                            ");
            }
            else
            {
                parameter.AppendSql("ORDER BY a.JEPDATE                                                                             ");
            }

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_LTD>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjYearGjJongLtdCodeWrtNo(string strFDate, string strTDate, string strYear, long nLtdCode, long nWrtNo, string strJong, string strDel, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') JEPDATE                                                     ");
            parameter.AppendSql("     , a.WRTNO, a.SNAME, a.AGE, a.GJJONG, a.GJCHASU, a.LTDCODE, a.HEMSMIRSAYU , B.SUCHUPYN         ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_SPECIAL b, ADMIN.HIC_LTD c               ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                                                           ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                                          ");
            parameter.AppendSql("   AND a.GJJONG NOT IN ('21','22','27','29','30','49')                                             ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                           ");
            parameter.AppendSql("   AND a.UCODES IS NOT NULL                                                                        ");
            parameter.AppendSql("   AND b.PANJENGDRNO IS NOT NULL                                                                   ");
            parameter.AppendSql("   AND A.LTDCODE = C.CODE                                                                          ");
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                    ");
            }
            if (nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            }
            if (!strJong.IsNullOrEmpty() && strJong != "**")
            {
                parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                      ");
            }
            if (strJong == "24")
            {
                parameter.AppendSql("  AND C.GBGUKGO = 'Y'                                                                          ");
            }
            if (strDel == "1")
            {
                parameter.AppendSql("   AND (a.HEMSNO = 0 OR a.HEMSNO IS NULL)                                                      ");
            }
            else if (strDel == "0") //제외
            {
                parameter.AppendSql("   AND a.HEMSNO = 1                                                                            ");
            }
            if (strSort == "0")
            {
                parameter.AppendSql(" ORDER BY a.JepDate,a.SName                                                                    ");
            }
            else if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY a.SName,a.JepDate                                                                    ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.Wrtno,a.SName,a.JepDate                                                            ");
            }
            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!strJong.IsNullOrEmpty() && strJong != "**")
            {
                parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            if (nWrtNo.IsNullOrEmpty() && nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_LTD>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetSpcItemCountbyJepDateGjYearGjBangi(string strFrDate, string strToDate, string fstrGjYear, string fstrGjBangi, string strJob, string strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT c.Name,a.LtdCode,COUNT(*) CNT                                                   ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MinDate                                    ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.JepDate,'YYYY-MM-DD')) MaxDate                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_SPECIAL b, ADMIN.HIC_LTD c   ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.LtdCode IS NOT NULL                                                           ");
            parameter.AppendSql("   AND a.UCodes IS NOT NULL                                                            ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                              ");
            if (fstrGjBangi != "" && fstrGjBangi != "*")
            {
                switch (fstrGjBangi)
                {
                    case "상반기":
                        parameter.AppendSql("   AND a.GjBangi = '1'                                                     ");
                        break;
                    case "하반기":
                        parameter.AppendSql("   AND a.GjBangi = '2'                                                     ");
                        break;
                    default:
                        break;
                }
            }
            if (strJob == "0")  //특수
            {
                parameter.AppendSql("   AND a.Gjjong IN ('11','12','14','41','42','23')                                 ");
            }
            else if (strJob == "1") //채용
            {
                parameter.AppendSql("   AND a.Gjjong IN ('22','24','30')                                                ");
            }
            else
            {
                parameter.AppendSql("   AND a.Gjjong  IN ('69')                                                         ");
            }
            //특수검진인지 점검
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                              ");
            parameter.AppendSql("   AND b.PanjengDrno IS NOT NULL                                                       ");
            //회사명
            parameter.AppendSql("   AND a.LtdCode=c.Code(+)                                                             ");
            if (strLtdCode != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            parameter.AppendSql(" GROUP BY c.Name,a.LtdCode                                                             ");
            parameter.AppendSql(" ORDER BY c.Name,a.LtdCode                                                             ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GJYEAR", fstrGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (fstrGjBangi != "" && fstrGjBangi != "*")
            {
                parameter.Add("GJBANGI", fstrGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_LTD>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItembyJepDateGjBangiLtdCode(string strFrDate, string strToDate, long nLtdCode, string strJob, string fstrGjBangi, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Pano,a.Sex,a.UCodes,a.GjChasu,a.GjJong,a.GjBangi                      ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SECOND_Flag                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_SPECIAL b, ADMIN.HIC_LTD c   ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                               ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                            ");
            parameter.AppendSql("   AND a.UCODES IS NOT NULL                                                            ");

            if (strGjYear != "")
            {
                parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                              ");
            }
            
            if (fstrGjBangi != "" && fstrGjBangi != "전체")
            {
                parameter.AppendSql("    AND a.GJBANGI = :GJBANGI                                                       ");
            }
            if (strJob == "0")    //특수
            {
                parameter.AppendSql("   AND a.GJJONG IN ('11','12','14','23','41','42')                                 ");
            }
            else if (strJob == "`") //배치전
            {
                parameter.AppendSql("   AND a.GJJONG IN ('22','24','30')                                                ");
            }
            else if (strJob == "1") //수시
            {
                parameter.AppendSql("   AND a.GJJONG IN ('25')                                                          ");
            }
            else                               //임시
            {
                parameter.AppendSql("   AND a.GJJONG  IN ('26')                                                         ");
            }
            //특수검진인지 점검
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                            ");
            parameter.AppendSql("   AND b.PanjengDrno IS NOT NULL                                                       "); //판정완료
            //회사명
            parameter.AppendSql("   AND a.LtdCode = c.Code(+)                                                           ");
            parameter.AppendSql(" ORDER BY a.WRTNO ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            if (strGjYear != "")
            {
                parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            
            if (fstrGjBangi != "" && fstrGjBangi != "전체")
            {
                parameter.Add("GJBANGI", fstrGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_LTD>(parameter);
        }

        public List<HIC_JEPSU_RES_SPECIAL_LTD> GetItemCountbyJepDateGjYearGjBangi(string strFrDate, string strToDate, string strGjYear, string strBangi, string strJob, string strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT c.NAME,a.LTDCODE,COUNT(*) CNT                                                   ");
            parameter.AppendSql("     , MIN(TO_CHAR(a.JEPDATE,'YYYY-MM-DD')) MINDATE                                    ");
            parameter.AppendSql("     , MAX(TO_CHAR(a.JEPDATE,'YYYY-MM-DD')) MAXDATE                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a, ADMIN.HIC_RES_SPECIAL b, ADMIN.HIC_LTD c   ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                               ");
            parameter.AppendSql("   AND a.LTDCODE IS NOT NULL                                                           ");
            parameter.AppendSql("   AND a.UCODES IS NOT NULL                                                            ");
            if(strGjYear!="")
            {
                parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                                                              ");
            }
            
            
            if (strBangi != "" && strBangi != "전체")
            {
                parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                        ");
            }
            if (strJob == "1")  //특수
            {
                parameter.AppendSql("   AND a.GJJONG IN ('11','12','14','23','41','42')                                 ");
            }
            else if (strJob == "2") //배치전
            {
                parameter.AppendSql("   AND a.GJJONG IN ('22','24','30')                                                ");
            }
            else if (strJob == "3") //수시
            {
                parameter.AppendSql("   AND a.GJJONG IN ('25')                                                          ");
            }
            else//임시
            {
                parameter.AppendSql("   AND a.GJJONG  IN ('26')                                                         ");
            }
            //특수검진인지 점검
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                            ");
            parameter.AppendSql("   AND b.PANJENGDRNO IS NOT NULL                                                       ");
            //회사명
            parameter.AppendSql("   AND a.LTDCODE = c.CODE(+)                                                           ");
            if (strLtdCode != "")
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            parameter.AppendSql(" GROUP BY c.NAME, a.LTDCODE                                                            ");
            parameter.AppendSql(" ORDER BY c.NAME, a.LTDCODE                                                            ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            if (strGjYear != "")
            {
                parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            
            if (strBangi != "" && strBangi != "전체")
            {
                parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (strLtdCode != "")
            {
                parameter.Add("LTDCODE", strLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_LTD>(parameter);
        }
    }
}
