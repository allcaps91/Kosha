namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComBase.Controls;

    /// <summary>
    /// 
    /// </summary>
    public class HicResultExCodeRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicResultExCodeRepository()
        {
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID      ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit    ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                            ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                            ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                   ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                                   ");
            parameter.AppendSql("   AND b.PART   = '5'                                                      ");
            parameter.AppendSql("   AND a.RESULT IS NULL                                                    ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoOrderbyPanjengPartExCode(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.PART, a.WRTNO, a.EXCODE,b.HNAME,a.RESULT,a.RESCODE,a.PANJENG,a.ROWID  ");
            parameter.AppendSql("     , b.MIN_M,b.MAX_M,b.MIN_F,b.MAX_F,b.RESULTTYPE,b.GBCODEUSE, b.UNIT        ");
            parameter.AppendSql("     , b.XRAYCODE,b.ENDOGUBUN2,b.ENDOGUBUN3,b.ENDOGUBUN4,b.ENDOGUBUN5          ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                                ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                       ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                    ");
            parameter.AppendSql(" ORDER BY a.PANJENG, b.PART, a.EXCODE                                          ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        

        public List<HIC_RESULT_EXCODE> GetItemNoActingbyWrtNo(long fnWRTNO, string strResult)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.Excode Code,b.HName,a.Result,a.ResCode,a.Panjeng                      ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName,B.HEASORT, a.SangDam   ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                                ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                                ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                                       ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                                    ");
            parameter.AppendSql("   AND b.PART   != '5'                                                                         ");
            if (strResult == "Y")
            {
                parameter.AppendSql(" ORDER BY a.Panjeng , b.HeaSORT, b.GBSORT                                                  ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY b.HeaSORT,b.GBSORT                                                               ");
            }

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(string fstrGubun, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            if (fstrGubun == "HIC")
            {
                parameter.AppendSql("SELECT b.part, a.WRTNO, a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID      ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit,b.HEAPART  ");
                parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                                    ");
                parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
                parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
                parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
                //parameter.AppendSql(" ORDER BY a.PANJENG, b.PART, a.EXCODE                                              ");
                parameter.AppendSql(" ORDER BY a.PART, b.GBSORT, a.EXCODE                                               ");
            }
            else
            {
                parameter.AppendSql("SELECT b.part, a.WRTNO, a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID      ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit,b.HEAPART  ");
                parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                    ");
                parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
                parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
                parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
                //parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                       ");
                parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                       ");
            }

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemHeaNoActingbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Part,a.ExCode,b.Excode Code,b.HName,a.Result,a.ResCode,a.Panjeng          ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName,B.HEASORT  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                    ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
            //parameter.AppendSql(" ORDER BY b.HEASORT, a.ExCode                                                      ");            
            parameter.AppendSql(" ORDER BY b.HEASORT, b.GBSORT, a.EXCODE                                               ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetHeaEndoExListByWrtno(long nHeaWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.ENDOGUBUN1, b.ENDOGUBUN2, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                                               ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                               ");
            parameter.AppendSql("   AND (b.ENDOGUBUN2='Y' OR b.ENDOGUBUN3='Y' OR b.ENDOGUBUN4='Y' OR b.ENDOGUBUN5='Y')  ");

            parameter.Add("WRTNO", nHeaWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetHNamebyWrtNo(string strWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.HNAME   ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND B.ENTPART IN('A')                                                   ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                  ");
            parameter.AppendSql(" ORDER BY b.SortA,  A.PART, a.ExCode                                       ");

            parameter.Add("WRTNO", strWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemHeabyWrtNoSort(long fnWrtNo, string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.RESULT, b.PART, B.CODE, B.HNAME                                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND b.PART != :PART                                                     ");
            parameter.AppendSql("   AND a.Result IS NULL                                                    ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                  ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("PART", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetCodeNamebyWrtNoNotInExCode(long argWRTNO, List<string> strExCodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.Code, b.HName                                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b  ");
            parameter.AppendSql(" WHERE a.ExCode=b.Code(+)                                  ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                    ");
            parameter.AppendSql("   AND a.RESULT IS NULL                                    ");
            //2019-09-26 ('A170','A172','A173') 3가지코드 김재관계장님 요청으로 추가
            parameter.AppendSql("   AND a.EXCODE NOT IN (:EXCODE)                           ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.AddInStatement("EXCODE", strExCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemCounselCanbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,b.ResCode,a.Panjeng,a.ROWID AS RID ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse            ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND a.ExCode NOT IN ('ZE47','TX20')                                     "); //위수면내시경
            parameter.AppendSql("   AND b.Part <> '5'                                                       ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                ");
            parameter.AppendSql(" ORDER BY b.ExSort                                                         ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNotExCode(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,b.ResCode,a.Panjeng,a.ROWID                            ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                                                ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                                ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND a.ExCode NOT IN ('ZE47', 'TX20')                                                        ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                                    ");
            parameter.AppendSql("   AND b.Part <> '5'                                                                           "); //액팅코드 제외            
            parameter.AppendSql(" ORDER BY b.SortA, A.PART, a.ExCode                                                            ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemHeabyWrtNoSort(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.Excode Code,b.HName,a.Result,a.ResCode,a.Panjeng, a.ROWID AS RID      ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName,B.HEASORT,a.SangDam    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                                ");
            parameter.AppendSql("     , ADMIN.Hic_EXCODE b                                                                ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                                    ");
            parameter.AppendSql("   AND b.Part <> '5'                                                                           "); //액팅코드 제외            
            parameter.AppendSql(" ORDER BY a.Panjeng , b.HeaSORT, b.gbsort, a.ExCode                                            ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemNotEmptybyWrtNo(string strWrtNo, string strGubun = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID                                    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit,b.ExSort                        ");
            if (strGubun == "HIC")
            {
                parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                                                    ");
            }
            else
            {
                parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                                    ");
            }
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                                ");
            parameter.AppendSql("   AND (b.PANDISP IS NULL OR B.PANDISP = 'N')                                                          ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                                            ");
            parameter.AppendSql("   AND b.Part <> '9'                                                                                   ");
            parameter.AppendSql("   AND (a.Result is null or trim(a.Result) = '')                                                       ");
            //2018-08-27(결과값 미입력 항목 추가)
            parameter.AppendSql("   AND a.ExCode NOT IN ('A142','A154','TR11','TZ08','TZ09','TZ72','TZ85','TZ12','ZE76','ZE77','TZ69')  ");
            parameter.AppendSql(" ORDER BY a.Panjeng,b.ExSort,b.Part,a.ExCode                                                           ");

            parameter.Add("WRTNO", strWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoResultHea(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.HeaSORT,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng                 ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName        ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                       ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                    ");
            parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                   ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetHeaEndoExListByWrtnoIN(List<long> lstHeaWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.ENDOGUBUN1, b.ENDOGUBUN2, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO IN (:WRTNO)                                                             ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                                               ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                               ");
            parameter.AppendSql("   AND (b.ENDOGUBUN2='Y' OR b.ENDOGUBUN3='Y' OR b.ENDOGUBUN4='Y' OR b.ENDOGUBUN5='Y')  ");

            parameter.AddInStatement("WRTNO", lstHeaWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemHeabyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.HeaSORT,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng             ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                  ");
            parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                               ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo1WrtNo2PaNo(long fnWrtno1, long fnWrtno2, long nPaNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng                    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                                        ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                        ");
            parameter.AppendSql("     , ADMIN.HIC_JEPSU  c                                                        ");
            parameter.AppendSql(" WHERE c.PANO = :PANO                                                                  ");
            if (fnWrtno1 > 0 && fnWrtno2 > 0)
            {
                parameter.AppendSql("   AND (c.WRTNO = :WRTNO1 OR c.WRTNO = :WRTNO2)                                    ");
            }
            else if (fnWrtno1 > 0 && fnWrtno2 == 0)
            {
                parameter.AppendSql("   AND c.WRTNO = :WRTNO1                                                           ");
            }
            else
            {
                parameter.AppendSql("   AND c.WRTNO = :WRTNO2)                                                          ");
            }
            parameter.AppendSql("   AND c.WRTNO  = a.WRTNO(+)                                                           ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                            ");
            parameter.AppendSql(" ORDER BY a.Panjeng, a.WRTNO, a.Part, a.ExCode                                         ");

            parameter.Add("WRTNO1", fnWrtno1);
            if (fnWrtno2 > 0)
            {
                parameter.Add("WRTNO2", fnWrtno2);
            }
            parameter.Add("PANO", nPaNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public HIC_RESULT_EXCODE GetHicExCodeGroupCodebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ExCode, a.GroupCode, b.HName, b.EName, b.YName, b.XRayCode                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                            ");
            parameter.AppendSql("   AND b.Code IN ('A142','TX13','TX14','TX11','A213','TX16','A211')                    ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_RESULT_EXCODE>(parameter);
        }

        public HIC_RESULT_EXCODE GetExCodeGroupCodebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ExCode, a.GroupCode, b.HName, b.EName, b.YName, b.XRayCode                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                            ");
            parameter.AppendSql("   AND b.Code IN ('A142','TX13','TX14','TX11','A213','TX16','A211')                    ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetListByWrtno(long argWrtno, string argBuse)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE,b.XRAYCODE,b.XNAME,b.XREMARK,b.XJONG,b.XSUBCODE,b.XORDERCODE       ");
            parameter.AppendSql("      ,b.SENDBUSE1,b.SENDBUSE2                                                     ");
            if (argBuse == "TO")
            {
                parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b                      ");
            }
            else
            {
                parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                      ");
            }
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
            parameter.AppendSql("   AND b.XORDERCODE IS NOT NULL                                                    ");
            parameter.AppendSql("   AND b.XRAYCODE IS NOT NULL                                                      ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                           ");
            //심전도실에 별도로 오더를 전달을 함
            if (argBuse == "TO")
            {
                parameter.AppendSql("   AND b.CODE NOT IN ('TX84')                                                  ");
            }
            else
            {
                parameter.AppendSql("   AND b.CODE NOT IN ('TZ17','TX84','TX68')                                    ");
            }


            parameter.AppendSql("   GROUP BY a.EXCODE,b.XRAYCODE,b.XNAME,b.XREMARK,b.XJONG,b.XSUBCODE,b.XORDERCODE,b.SENDBUSE1,b.SENDBUSE2  ");
            parameter.AppendSql("   ORDER BY b.XJONG, A.EXCODE                                                                              ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetCountbyPaNoJepDateWrtNoEntPart(long pano, string sysdate, long nWrtNo, string eNTPART, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Active FROM ADMIN.HIC_RESULT A, ADMIN.HIC_EXCODE B          ");
            if (strJong == "Y")
            {
                parameter.AppendSql(" WHERE A.WRTNO IN (                                                        ");
                parameter.AppendSql("              SELECT WRTNO FROM ADMIN.HIC_JEPSU                      ");
                parameter.AppendSql("               WHERE PANO = :PANO                                          ");
                parameter.AppendSql("                 AND JEPDATE = TO_DATE(:JEPDATE, 'YYYY-MM-DD')             ");
                parameter.AppendSql("                 AND DELDATE IS NULL )                                     ");
            }
            else
            {
                parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                    ");
            }
            parameter.AppendSql("   AND A.EXCODE = B.CODE                                                       ");
            parameter.AppendSql("   AND B.ENTPART = :ENTPART                                                    ");
            parameter.AppendSql("   AND (a.Active = '' or a.Active is null)                                     ");

            if (strJong == "Y")
            {
                parameter.Add("PANO", pano);
                parameter.Add("JEPDATE", sysdate);
            }
            else
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            parameter.Add("ENTPART", eNTPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetHicEndoExListByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.ENDOGUBUN1, b.ENDOGUBUN2, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5  ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                                               ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                               ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetEntPartByWRTNO(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.ENTPART                                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b  ");
            parameter.AppendSql(" WHERE a.WRTNO =:WRTNO                                     ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                ");
            parameter.AppendSql("   AND b.ENTPART NOT IN (' ','Z')                          ");
            parameter.AppendSql(" GROUP by b.ENTPART                                        ");
            parameter.AppendSql(" ORDER by b.ENTPART                                        ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetHicEndoExListByWrtnoIN(List<long> lstHicWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.ENDOGUBUN1, b.ENDOGUBUN2, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5  ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO IN (:WRTNO)                                                             ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                                               ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                               ");
            parameter.AppendSql("   AND (b.ENDOGUBUN2='Y' OR b.ENDOGUBUN3='Y' OR b.ENDOGUBUN4='Y' OR b.ENDOGUBUN5='Y')  ");

            parameter.AddInStatement("WRTNO", lstHicWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCodeSpc(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID AS RID   ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse        ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND a.ExCode IN('TI01','TI02','TR11','TH01','TH02')                 ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                              ");
            parameter.AppendSql(" ORDER BY b.Part, a.ExCode                                             ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoInExCodes(long fnWRTNO, List<string> strNewExCode, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "HIC")
            {
                parameter.AppendSql("SELECT a.part, a.excode, b.hname, b.yname, a.RESULT, a.rescode, a.panjeng      ");
                parameter.AppendSql("     , a.ROWID, b.min_m, b.max_m, b.min_f, b.max_f, b.resulttype               ");
                parameter.AppendSql("     , b.gbcodeuse, b.unit, b.min_m, b.max_m, b.min_f, b.max_f                 ");
                parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                      ");
                parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                        ");
                parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                    ");
                parameter.AppendSql("   AND a.EXCODE IN  (:EXCODE)                                                  ");
                parameter.AppendSql(" ORDER BY a.Part,a.ExCode                                                      ");
            }
            else if (strGubun == "HEA")
            {
                parameter.AppendSql("SELECT a.Part,a.ExCode,b.Excode Code,b.HName,a.Result,a.ResCode,a.Panjeng      ");
                parameter.AppendSql("     , a.ROWID,a.ExCode,a.Result,a.Panjeng,c.GbUse                             ");
                parameter.AppendSql("     , b.gbcodeuse, b.unit                                                     ");
                parameter.AppendSql("     , c.Min_M,c.Min_M1,c.Min_M2,c.Min_M3,c.Max_M,c.Max_M1,c.Max_M2,c.Max_M3   ");
                parameter.AppendSql("     , c.Min_F,c.Min_F1,c.Min_F2,c.Min_F3,c.Max_F,c.Max_F1,c.Max_F2,c.Max_F3   ");
                parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b                      ");
                parameter.AppendSql("     , ADMIN.HIC_YakCode c                                               ");
                parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                        ");
                parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                    ");
                parameter.AppendSql("   AND a.ExCode = c.Code(+)                                                    ");
                parameter.AppendSql("   AND a.EXCODE IN  (:EXCODE)                                                  ");
                parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                   ");
            }
            else if (strGubun == "HEA_OLD")
            {
                parameter.AppendSql("SELECT b.HeaSORT,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng                 ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName        ");
                parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b                      ");
                parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                        ");
                parameter.AppendSql("   AND a.EXCODE IN  (:EXCODE)                                                  ");
                parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                      ");
                parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                   ");
            }

            parameter.Add("WRTNO", fnWRTNO);
            parameter.AddInStatement("EXCODE", strNewExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetUrineItembyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ExCode                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a            ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b            ");
            parameter.AppendSql(" WHERE a.WRTNO   = :WRTNO                  ");
            parameter.AppendSql("   AND a.ExCode  = b.Code(+)               ");
            parameter.AppendSql("   AND b.GbUline = 'Y'                     "); //소변컵라벨 인쇄 여부

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNotInPart(long fnWrtNo, string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.part, a.excode, b.hname, b.yname, a.RESULT, a.rescode, a.panjeng  ");
            parameter.AppendSql("     , a.ROWID, b.min_m, b.max_m, b.min_f, b.max_f, b.resulttype           ");
            parameter.AppendSql("     , b.gbcodeuse, b.unit, b.min_m, b.max_m, b.min_f, b.max_f             ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND a.ExCode = b.Code                                                   ");
            parameter.AppendSql("   AND a.PART != :PART                                                     ");
            parameter.AppendSql(" ORDER BY a.Part,a.ExCode                                                  ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("PART", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public int GetCountbyWrtNoNotPart9(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b      ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                        ");
            parameter.AppendSql("   AND a.RESULT IS NULL                                        ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                    ");
            parameter.AppendSql("   AND b.PART <> '9'                                           "); //금액코드 항목 제외
            parameter.AppendSql("   AND b.GBRESEMPTY = 'Y'                                      "); //검사결과 입력하지 않아도 되는 항목                                      ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoPartNot9(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.RESULT, a.excode, b.HNAME            ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                ");
            parameter.AppendSql("   AND a.Result IS NULL                                                    ");
            parameter.AppendSql("   AND b.Part <> '9'                                                       ");
            parameter.AppendSql(" ORDER BY b.SortA, A.PART, a.ExCode                                        ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoInExCodePart(long fnWRTNO, List<string> strNewExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.part, a.excode, b.hname, b.yname, a.RESULT, a.rescode, a.panjeng  ");
            parameter.AppendSql("     , a.ROWID, b.min_m, b.max_m, b.min_f, b.max_f, b.resulttype           ");
            parameter.AppendSql("     , b.gbcodeuse, b.unit, b.min_m, b.max_m, b.min_f, b.max_f             ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                            ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                            ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                ");
            parameter.AppendSql("   AND a.EXCODE NOT IN  (:EXCODE)                                          ");
            parameter.AppendSql("   AND b.Part <> '5'                                                       ");
            parameter.AppendSql(" ORDER BY b.SortA, A.PART, a.ExCode                                        ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.AddInStatement("EXCODE", strNewExCode);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemCounselbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,DECODE(NVL(a.Panjeng, ''), ' ', '', a.Panjeng) AS PANJENG,a.ROWID AS RID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse        ");
            parameter.AppendSql("     , b.Unit, b.ExSort, b.GBAUTOSEND, a.GROUPCODE                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                              ");
            parameter.AppendSql("   AND b.Part <> '9'                                                   ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                               ");
            parameter.AppendSql("   AND (b.PANDISP IS NULL OR B.PANDISP = 'N')                          ");
            parameter.AppendSql("   AND B.GBRESVIEW = 'Y'                                               ");
            //parameter.AppendSql(" ORDER BY DECODE(NVL(a.Panjeng, ''), ' ', '', a.Panjeng) DESC, b.ExSort, b.Part, a.ExCode          ");
            //parameter.AppendSql(" ORDER BY b.ExSort, b.Part, a.ExCode          ");
            parameter.AppendSql(" ORDER BY b.Part, b.Exsort, a.ExCode                            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItemCounselbyWrtNo(List<long> fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,DECODE(NVL(a.Panjeng, ''), ' ', '', a.Panjeng) AS PANJENG,a.ROWID AS RID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse        ");
            parameter.AppendSql("     , b.Unit, b.ExSort, b.GBAUTOSEND, a.GROUPCODE                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO IN (:WRTNO)                                             ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                              ");
            parameter.AppendSql("   AND b.Part <> '9'                                                   ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                               ");
            parameter.AppendSql("   AND (b.PANDISP IS NULL OR B.PANDISP = 'N')                          ");
            parameter.AppendSql(" ORDER BY b.ExSort, b.Part, a.ExCode          ");

            parameter.AddInStatement("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoInExCode(long fnWRTNO, List<string> g37_DOCT_ENTCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID AS RID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse        ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            if (g37_DOCT_ENTCODE.Count > 0)
            {
                parameter.AppendSql("   AND a.EXCODE IN (:EXCODE)                                       ");
            }
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                              ");
            parameter.AppendSql("   AND (b.PANDISP IS NULL OR B.PANDISP = 'N')                          ");
            parameter.AppendSql(" ORDER BY a.ExCode                                                     ");

            parameter.Add("WRTNO", fnWRTNO);
            if (g37_DOCT_ENTCODE.Count > 0)
            {
                parameter.AddInStatement("EXCODE", g37_DOCT_ENTCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCodeCheckNew(long fnWRTNO, List<string> fstrPartExam, string strChkNew)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID RID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,a.ENTSABUN ");
            parameter.AppendSql("     , b.GBAUTOSEND                                                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            if (fstrPartExam.Count > 0 && fstrPartExam[0] != "전체")
            {
                parameter.AppendSql("   AND a.EXCODE IN (:EXCODE)                                           ");
            }
            if (strChkNew == "1")
            {
                parameter.AppendSql("   AND a.Result IS NULL                                                ");
            }
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                  ");
            parameter.AppendSql("   AND b.GBRESVIEW ='Y'                                                    ");
            //parameter.AppendSql(" ORDER BY b.Part,a.ExCode                                                  ");     
            //parameter.AppendSql(" ORDER BY b.Part, b.Heasort, b.Gbsort, a.ExCode                            ");
            //parameter.AppendSql(" ORDER BY b.Part,  b.Heasort, b.ENTPART, b.Gbsort, a.ExCode                ");
            parameter.AppendSql(" ORDER BY b.Part, b.Exsort, a.ExCode                            ");

            parameter.Add("WRTNO", fnWRTNO);
            if (fstrPartExam.Count > 0 && fstrPartExam[0] != "전체")
            {
                parameter.AddInStatement("EXCODE", fstrPartExam, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }


        public List<HIC_RESULT_EXCODE> GetItemByWrtNoNotInExCode(long fnWRTNO, List<string> fstrExcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT a.WRTNO,a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng       ");
            parameter.AppendSql(" , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit         ");
            parameter.AppendSql(" FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                   ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            if (fstrExcode.Count > 0 )
            {
                parameter.AppendSql(" AND a.EXCODE NOT IN (:EXCODE)                                             ");
            }
            
            parameter.AppendSql(" AND a.Result NOT IN ('미실시','.')                                         ");
            parameter.AppendSql(" AND a.ExCode=b.Code(+)                                                    ");
            parameter.AppendSql(" AND b.Part <> '5'                                                         ");    
            parameter.AppendSql(" ORDER BY a.Part,a.ExCode                                                  ");

            parameter.Add("WRTNO", fnWRTNO);
            if (fstrExcode.Count > 0)
            {
                parameter.AddInStatement("EXCODE", fstrExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
                

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }


        public List<HIC_RESULT_EXCODE> GetItembyWrtNoPart(long fnWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse        ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("  AND b.PART <> '5'                                                    ");
            parameter.AppendSql("  AND a.ExCode = b.Code(+)                                             ");
            parameter.AppendSql("ORDER BY a.Part,a.ExCode                                               ");

            parameter.Add("WRTNO", fnWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoPaNo(long fnWrtno1, long fnWrtno2, long fnWRTNO, long fnPano, string fstrGjChasu)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng                ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit             ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b, ADMIN.HIC_JEPSU c ");
            parameter.AppendSql(" WHERE c.PANO = :PANO                                                              ");
            if (fstrGjChasu == "2")
            {
                parameter.AppendSql("   AND c.WRTNO IN (:WRTNO1, :WRTNO2)                                           ");
            }
            else
            {
                parameter.AppendSql("   AND c.WRTNO = :WRTNO                                                        ");
            }
            parameter.AppendSql("   AND c.WRTNO=a.WRTNO(+)                                                          ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                          ");
            parameter.AppendSql(" ORDER BY a.Panjeng,a.WRTNO,a.Part,a.ExCode                                        ");
            
            if (fstrGjChasu == "2")
            {
                parameter.Add("WRTNO1", fnWrtno1);
                parameter.Add("WRTNO2", fnWrtno2);
            }
            else
            {
                parameter.Add("WRTNO", fnWRTNO);
            }
            parameter.Add("PANO", fnPano);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyOnlyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                            ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyOnlyWrtNoSort(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                            ");
            parameter.AppendSql("   ORDER BY A.PART, A.EXCODE                                           ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCodeNotIn(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND a.ExCode NOT IN ('A139','A999')                                 ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                            ");
            parameter.AppendSql(" ORDER BY a.Panjeng,b.Part,a.ExCode                                    ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetXrayItembyWrtNoExCode(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID AS RID   ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse        ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND a.ExCode IN ('A142','A154','A215')                              ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                              ");
            parameter.AppendSql("   AND (b.PANDISP IS NULL OR B.PANDISP = 'N')                          ");
            parameter.AppendSql(" ORDER BY a.ExCode                                                     ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoExCode(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.YName,a.Result,b.GbCodeUse,b.ResCode              ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b  ");
            parameter.AppendSql(" WHERE a.ExCode = b.Code(+)                                ");
            parameter.AppendSql("   AND a.Panjeng = 'R'                                     ");
            parameter.AppendSql("   AND a.Excode IN ('A108','A109','A122','A108')           ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                    ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyPaNoGjYear(long nPano, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.Part,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID, A.WRTNO           ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit,b.ExSort        ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO IN (SELECT WRTNO                                                        ");
            parameter.AppendSql("                     FROM ADMIN.HIC_JEPSU                                        ");
            parameter.AppendSql("                    WHERE PANO = :PANO AND GJYEAR = :GJYEAR                            ");
            parameter.AppendSql("                    AND GJJONG IN ('11','16','23','28'))                                ");
            parameter.AppendSql("   AND (b.PANDISP IS NULL OR B.PANDISP = 'N')                                          ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                            ");
            parameter.AppendSql("   AND b.Part <> '9'                                                                   ");
            parameter.AppendSql(" ORDER BY a.Panjeng,b.ExSort,b.Part,a.ExCode                                           ");

            parameter.Add("PANO", nPano);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo_First(long fnWrtno, string strGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,b.YName,a.Result,a.ResCode,a.Panjeng,a.ROWID            ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                              ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND a.Result <> '.'                                                                 ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                            ");
            parameter.AppendSql("   AND a.ExCode IN (SELECT ExCode FROM ADMIN.HIC_SPC_SogenExam WHERE SOGENCODE = :SOGENCODE) ");           
            parameter.AppendSql(" ORDER BY a.Part,a.ExCode                                                              ");

            parameter.Add("WRTNO", fnWrtno);
            parameter.Add("SOGENCODE", strGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoSogenCode(long fnWRTNO, string argGbn, List<string> strNewExCode, string argJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,b.YName,a.Result,a.ResCode,a.Panjeng,a.ROWID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit         ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                      ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                        ");
            parameter.AppendSql("   AND a.Result IS NOT NULL                                                    ");
            parameter.AppendSql("   AND a.Result <> '.'                                                         ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                    ");
            if (argJob == "특수")
            {
                parameter.AppendSql("   AND a.ExCode IN (SELECT ExCode FROM ADMIN.HIC_SPC_SogenExam WHERE SOGENCODE = :SOGENCODE) ");
            }
            else if (argJob == "D1D2")
            {
                parameter.AppendSql("   AND a.PANJENG IN ('B','C','R')                                          "); //비정상
                parameter.AppendSql("   AND a.EXCODE IN  (:EXCODE)                                              ");
            }
            else
            {
                if (argGbn == "04") //고지혈증
                {
                    parameter.AppendSql("   AND (b.GBPANBUN2 = :GBPANBUN2 OR a.ExCode IN ('C404','C405'))       ");
                }
                else
                {
                    parameter.AppendSql("   AND b.GBPANBUN2 = :GBPANBUN2                                        ");
                }
            }
            parameter.AppendSql(" ORDER BY a.Part,a.ExCode                                                      ");

            parameter.Add("WRTNO", fnWRTNO);
            if (argJob == "특수")
            {
                parameter.Add("SOGENCODE", argGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (argJob != "특수" && argJob != "D1D2")
            {
                parameter.Add("GBPANBUN2", argGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (argJob == "D1D2")
            {
                parameter.AddInStatement("EXCODE", strNewExCode);
            }

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNewExCode(long fnWRTNO, List<string> strNewExCode, string strPanjeng = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,b.YName,a.Result,a.ResCode,a.Panjeng,a.ROWID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit         ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                      ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                       ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                    ");
            if (strPanjeng == "")
            {
                parameter.AppendSql("   AND a.Panjeng IN('B','C','R')                                           "); //비정상
            }
            if (strNewExCode.Count > 0)
            {
                parameter.AppendSql("   AND a.ExCode IN (:EXCODE)                                               ");
            }
            parameter.AppendSql(" ORDER BY a.Part,a.ExCode                                                      ");

            parameter.Add("WRTNO", fnWRTNO);
            if (strNewExCode.Count > 0)
            {
                parameter.AddInStatement("EXCODE", strNewExCode);
            }

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoGbn(long fnWRTNO, string argGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Part,a.ExCode,b.HName,b.YName,a.Result,a.ResCode,a.Panjeng,a.ROWID    ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.Unit         ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a, ADMIN.HIC_EXCODE b                      ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                       ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                    ");
            if (argGbn == "04") //고지혈증
            {
                parameter.AppendSql("   AND (b.GBPANBUN2 = :GBPANBUN2                                           ");
                parameter.AppendSql("    OR a.ExCode IN ('C404','C405'))                                        ");
            }
            else if (argGbn == "12")
            {
                parameter.AppendSql("   AND b.GBPANBUN2 = '999'                                                 ");
            }
            else if (argGbn == "11")
            {
                parameter.AppendSql("   AND b.GBPANBUN2 = '12'                                                  ");
            }
            else
            {
                parameter.AppendSql("   AND b.GBPANBUN2 = :GBPANBUN2                                            ");
            }
            parameter.AppendSql(" ORDER BY a.Part,a.ExCode                                                      ");

            parameter.Add("WRTNO", fnWRTNO);
            if (argGbn != "11" && argGbn != "12")
            {
                parameter.Add("GBPANBUN2", argGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetResultbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, a.RESULT                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a            ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b            ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                    ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                ");
            parameter.AppendSql("   AND b.PART <> '5'                       ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoNoActing(long fnWRTNO, string strHeaSORT)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.HeaSORT, a.ExCode, b.HName, a.Result, b.ResCode, a.Panjeng, a.ROWID AS RID ");
            parameter.AppendSql("     , b.Min_M, b.Max_M, b.Min_F, b.Max_F, b.ResultType, b.GbCodeUse, a.SangDam    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                    ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                            ");
            parameter.AppendSql("   AND a.ExCode = b.Code(+)                                                        ");
            parameter.AppendSql("   AND b.Part <> '5'                                                               "); //액팅코드 제외
            if (strHeaSORT != "*")
            {
                parameter.AppendSql("   AND SUBSTR(b.HEASORT, 1, 1) = :HEASORT                                      ");
            }
            parameter.AppendSql(" ORDER BY b.HeaSORT, b.GBSORT                                                      ");

            parameter.Add("WRTNO", fnWRTNO);
            if (strHeaSORT != "*")
            {
                parameter.Add("HEASORT", strHeaSORT, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public int GetCountbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('x') CNT              ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESULT a    ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b    ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO           ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)        ");
            parameter.AppendSql("   AND b.PART   = '5'              ");
            parameter.AppendSql("   AND b.ENDOGUBUN3 = 'Y'          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNo_Stomach(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.ENDOGUBUN3, b.ENDOGUBUN4, b.ENDOGUBUN5                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                            ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                            ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                   ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                                   ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                   ");
            parameter.AppendSql("   AND (b.ENDOGUBUN3 = 'Y' OR b.ENDOGUBUN4 = 'Y' OR b.ENDOGUBUN5 = 'Y')    ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetItembyWrtNoResult(long fnWRTNO, string strGubun = "")
        {
            MParameter parameter = CreateParameter();
            if (strGubun == "HEA")
            {
                parameter.AppendSql("SELECT b.HEASORT, a.EXCODE, b.HNAME, a.RESULT, a.RESCODE, a.PANJENG                ");
                parameter.AppendSql("     , b.min_m, b.max_m, b.min_f, b.max_f, b.RESULTTYPE, b.GBCODEUSE               ");
                parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                    ");
                parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
                parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
                parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
                parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                       ");
            }
            else
            {
                parameter.AppendSql("SELECT a.Part, a.ExCode, b.HName, a.Result, a.ResCode, a.Panjeng, B.HEASORT        ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName, b.UNIT    ");
                parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                                    ");
                parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
                parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
                parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
                parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                       ");
            }

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }

        public List<HIC_RESULT_EXCODE> GetExcodeResultListByWrtno(long argWrtno, string argSex)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT A.EXCODE, A.RESULT                     ");
            if(argSex == "M")
            {
                parameter.AppendSql(" ,MIN_M AS DMIN, MAX_M AS DMAX             ");
            }
            else
            {
                parameter.AppendSql(" ,MIN_F AS DMIN, MAX_F AS DMAX             ");
            }

            parameter.AppendSql("  FROM HIC_RESULT A, HIC_EXCODE B              ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                        ");
            parameter.AppendSql(" AND A.RESULT IS NOT NULL                      ");
            parameter.AppendSql(" AND A.EXCODE = B.CODE(+)                      ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<HIC_RESULT_EXCODE>(parameter);
        }
    }
}
