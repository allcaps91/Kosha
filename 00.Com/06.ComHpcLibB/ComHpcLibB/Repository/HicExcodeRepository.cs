
namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicExcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicExcodeRepository()
        {
        }

        public List<HIC_EXCODE> FindAll(bool fbDel, bool fbSpc, bool fbSend, string fstrKey, string strGbSuga = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,HNAME,ENAME,YNAME,UNIT,EXCODE,SUCODE,GCODE,RESCODE,RESCODE2,PART,BRESULT           ");
            parameter.AppendSql("      ,DECODE(RESULTTYPE,'1','숫자','2','문자','3','공용','4','2줄','문자') STRRESTYPE         ");
            parameter.AppendSql("      ,RESULTTYPE,RESULTTYPE2,GBCODEUSE,GBAUTOSEND,TONGBUN                                     ");
            parameter.AppendSql("      ,AMT1,AMT2,AMT3,AMT4,AMT5,TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE                            ");
            parameter.AppendSql("      ,OLDAMT1,OLDAMT2,OLDAMT3,OLDAMT4,OLDAMT5                                                 ");
            parameter.AppendSql("      ,CHKSUGA1,CHKSUGA2,CHKSUGA3,CHKSUGA4,CHKSUGA5                                            ");
            parameter.AppendSql("      ,MIN_M,MIN_MB,MIN_MR,MAX_M,MAX_MB,MAX_MR,MIN_F,MIN_FB,MIN_FR,MAX_F,MAX_FB,MAX_FR         ");
            parameter.AppendSql("      ,DELDATE,HEASORT,ENTPART,ENTDATE,ENTSABUN                                                ");
            parameter.AppendSql("      ,GBRESEMPTY,GBPANBUN1,GBPANBUN2,XRAYCODE, BUCODE, SORTA, PANDISP, EXSORT                 ");
            parameter.AppendSql("      ,HEAPART,HAROOM,ENDOGUBUN1,ENDOGUBUN2,ENDOGUBUN3,ENDOGUBUN4,ENDOGUBUN5,ENDOSCOPE         ");
            parameter.AppendSql("      ,XNAME,XREMARK,XJONG,XSUBCODE,XORDERCODE,GOTO,EXAMBUN,EXAMFRTO,GBSPCEXAM,GBULINE         ");
            parameter.AppendSql("      ,SENDBUSE1,SENDBUSE2,GBSUGA                                                              ");
            parameter.AppendSql("      ,TO_CHAR(SUDATE2,'YYYY-MM-DD') SUDATE2,TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3             ");
            parameter.AppendSql("      ,TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5             ");
            parameter.AppendSql("      ,GAMT1,SAMT1,JAMT1,IAMT1,OAMT1                                                           ");
            parameter.AppendSql("      ,GAMT2,SAMT2,JAMT2,IAMT2,OAMT2                                                           ");
            parameter.AppendSql("      ,GAMT3,SAMT3,JAMT3,IAMT3,OAMT3                                                           ");
            parameter.AppendSql("      ,ROWID AS RID                                                                            ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_EXCODE                                                        ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
           
            if (fbDel == false)     //삭제코드 
            {
                parameter.AppendSql("  AND DELDATE IS NULL                                       ");
            }
            else
            {
                parameter.AppendSql("  AND DELDATE IS NOT NULL                                   ");
            }

            if (fbSpc == true)      //특수검진코드
            {
                parameter.AppendSql("  AND GBSPCEXAM = 'Y'                                   ");
            }

            if (fbSend == true)     //오더전달
            {
                parameter.AppendSql("  AND XORDERCODE > ' '                                ");
            }

            if (!string.IsNullOrEmpty(fstrKey))
            {
                parameter.AppendSql("  AND HName LIKE :Key                             ");
            }

            if (!string.IsNullOrEmpty(strGbSuga) && strGbSuga != "*")
            {
                parameter.AppendSql("  AND GBSUGA = :GBSUGA                             ");
            }

            parameter.AppendSql(" ORDER BY SortA, Code                                  ");
            
            if (!string.IsNullOrEmpty(fstrKey))
            {
                parameter.AddLikeStatement("Key", fstrKey.Trim());
            }

            if (!string.IsNullOrEmpty(strGbSuga) && strGbSuga != "*")
            {
                parameter.Add("GBSUGA", strGbSuga);
            }

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public HIC_EXCODE GetHNameYNamebyCode(string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HNAME, YNAME, ENAME     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", strExCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetCodeHNamebyPartHName(string strGubun, string strView)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,HNAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE DELDATE IS NULL         ");
            if (!strGubun.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PART = :PART        ");
            }
            if (!strView.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND HNAME LIKE :HNAME   ");
            }
            parameter.AppendSql(" ORDER BY CODE                 ");

            if (!strGubun.IsNullOrEmpty())
            {
                parameter.Add("PART", strGubun, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            if (!strView.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("HNAME", strView);
            }

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetEndoGbnCodeList()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,ENDOGUBUN3,ENDOGUBUN4,ENDOGUBUN5                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE                                  ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                                         ");
            parameter.AppendSql("   AND (ENDOGUBUN3='Y' OR ENDOGUBUN4='Y' OR ENDOGUBUN5='Y')    ");

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetSugaListByChkSuga(string strChkSuga, string strSuCode, string strExamCode, string strSuDateCode, bool bDel, string argSearch)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, HNAME, SUCODE, EXCODE, ROWID AS RID                       ");
            parameter.AppendSql("       , CASE WHEN KOSMOS_PMPA.FC_BAS_SUT_SUGBA(SUCODE) = '1' THEN     ");
            parameter.AppendSql("                   CASE WHEN KOSMOS_PMPA.FC_BAS_SUT_SUGBE(SUCODE) > '0' THEN TRUNC(FC_BAS_SUT_BAMT(SUCODE) * 1.25 , -1)  ");
            parameter.AppendSql("                        ELSE FC_BAS_SUT_BAMT(SUCODE) END               ");
            parameter.AppendSql("              ELSE FC_BAS_SUT_BAMT(SUCODE) END AS BAMT                 ");
            parameter.AppendSql("       , KOSMOS_PMPA.FC_BAS_SUT_SUDATE(SUCODE) AS BSUDATE              ");
            parameter.AppendSql("       , KOSMOS_PMPA.FC_BAS_SUT_SUGBA(SUCODE)  AS SUGBA                ");
            parameter.AppendSql("       , KOSMOS_PMPA.FC_BAS_SUT_SUGBE(SUCODE)  AS SUGBE                ");
            parameter.AppendSql("       , AMT1,AMT2,AMT3,AMT4,AMT5                                      ");
            parameter.AppendSql("       , KOSMOS_PMPA.FC_CHK_BAS_SUT_DELDATE(SUCODE) AS ERR1            ");
            parameter.AppendSql("       , KOSMOS_PMPA.FC_CHK_EXAMCODE_DEL(SUCODE) AS ERR2               ");
            parameter.AppendSql("       , TO_CHAR(SUDATE, 'YYYY-MM-DD') AS SUDATE                       ");
            parameter.AppendSql("       , TO_CHAR(DELDATE, 'YYYY-MM-DD') AS DELDATE                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE                                          ");
            parameter.AppendSql(" WHERE 1 = 1                                                           ");
            if (!bDel)
            {
                parameter.AppendSql("  AND DELDATE IS NULL ");
            }

            if (!argSearch.IsNullOrEmpty()) { parameter.AppendSql("  AND HNAME LIKE :HNAME              "); }

            switch (strChkSuga)
            {
                case "HEA": parameter.AppendSql(" AND CHKSUGA1 = 'Y' "); break;
                case "HIC": parameter.AppendSql(" AND CHKSUGA2 = 'Y' "); break;
                case "SPC": parameter.AppendSql(" AND CHKSUGA3 = 'Y' "); break;
                case "MED": parameter.AppendSql(" AND CHKSUGA4 = 'Y' "); break;
                case "TMP": parameter.AppendSql(" AND CHKSUGA5 = 'Y' "); break;
                default:
                    break;
            }

            if (!strSuCode.IsNullOrEmpty()) { parameter.AppendSql("  AND SUCODE IS NOT NULL                             "); }
            if (!strExamCode.IsNullOrEmpty()) { parameter.AppendSql("  AND EXCODE IS NOT NULL                           "); }
            if (!strSuDateCode.IsNullOrEmpty()) { parameter.AppendSql("  AND SUDATE < TO_DATE(:SUDATE, 'YYYY-MM-DD')    "); }

            if (!argSearch.IsNullOrEmpty()) { parameter.AddLikeStatement("HNAME", argSearch); }
            if (!strSuDateCode.IsNullOrEmpty()) { parameter.Add("SUDATE", strSuDateCode); }

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetHistoryByCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ENTDATE, ENTSABUN, KOSMOS_OCS.FC_INSA_MST_KORNAME(ENTSABUN) AS JOBNAME  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE_HIS                                              ");
            parameter.AppendSql(" WHERE CODE =:CODE                                                             ");
            parameter.AppendSql(" ORDER BY ENTDATE DESC                                                         ");

            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public void SelectInsertHicExCodeByRid(string fstrRowid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_EXCODE_HIS (                                                                  ");
            parameter.AppendSql("        CODE,HNAME,ENAME,YNAME,UNIT,EXCODE,SUCODE,GCODE,RESCODE,TONGBUN,PART                               ");
            parameter.AppendSql("       ,BRESULT,RESULTTYPE,GBCODEUSE,GBAUTOSEND,MIN_M,MIN_MB,MIN_MR,MAX_M,MAX_MB,MAX_MR                    ");
            parameter.AppendSql("       ,MIN_F,MIN_FB,MIN_FR,MAX_F,MAX_FB,MAX_FR,AMT1,AMT2,AMT3,AMT4,AMT5,SUDATE,OLDAMT1                    ");
            parameter.AppendSql("       ,OLDAMT2,OLDAMT3,OLDAMT4,OLDAMT5,DELDATE,ENTDATE,ENTSABUN,RESBOHUM1,RESBOHUM2                       ");
            parameter.AppendSql("       ,RESCANCER,GBGAM,GBRESULT,AMT6,OLDAMT6,HEASORT,ENTPART,GBRESEMPTY,GBPANBUN1                         ");
            parameter.AppendSql("       ,GBPANBUN2 , XRAYCODE, BUCODE, SORTA, PANDISP, EXSORT, ACTTIME,HEAPART,HAROOM,GOTO,GBSORT )         ");
            parameter.AppendSql(" SELECT CODE,HNAME,ENAME,YNAME,UNIT,EXCODE,SUCODE,GCODE,RESCODE,TONGBUN,PART                               ");
            parameter.AppendSql("       ,BRESULT,RESULTTYPE,GBCODEUSE,GBAUTOSEND,MIN_M,MIN_MB,MIN_MR,MAX_M,MAX_MB,MAX_MR                    ");
            parameter.AppendSql("       ,MIN_F,MIN_FB,MIN_FR,MAX_F,MAX_FB,MAX_FR,AMT1,AMT2,AMT3,AMT4,AMT5,SUDATE,OLDAMT1                    ");
            parameter.AppendSql("       ,OLDAMT2,OLDAMT3,OLDAMT4,OLDAMT5,DELDATE,SYSDATE," + clsType.User.IdNumber + ",RESBOHUM1,RESBOHUM2  ");
            parameter.AppendSql("       ,RESCANCER,GBGAM,GBRESULT,AMT6,OLDAMT6,HEASORT,ENTPART,GBRESEMPTY,GBPANBUN1                         ");
            parameter.AppendSql("       ,GBPANBUN2 , XRAYCODE, BUCODE, SORTA, PANDISP, EXSORT, ACTTIME,HEAPART,HAROOM,GOTO,GBSORT           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE                                                                              ");
            parameter.AppendSql(" WHERE ROWID =:RID                                                                                         ");

            parameter.Add("RID", fstrRowid);

            ExecuteNonQuery(parameter);
        }

        public string GetRoombyHeaPart(string strEntPart)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT unique HAROOM               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE      ");
            parameter.AppendSql(" WHERE DELDATE IS NULL             ");
            parameter.AppendSql("   AND trim(HAROOM) IS NOT NULL    ");
            parameter.AppendSql("   AND HEAPART = :HEAPART          ");

            parameter.Add("HEAPART", strEntPart, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_EXCODE> GetCodebyHeaSort(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,HNAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE DELDATE IS NULL         ");
            if (!argCode.IsNullOrEmpty() && argCode != "0")
            {
                parameter.AppendSql("   AND SUBSTR(HEASORT,1,1) = :HEASORT            ");
            }

            parameter.AppendSql(" ORDER BY CODE                 ");

            if (!argCode.IsNullOrEmpty())
            {
                parameter.Add("HEASORT", VB.Left(argCode, 1));
            }

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public HIC_EXCODE GetCodeEndoGubun3byExCode(string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, ENDOGUBUN3        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE CODE       = :CODE      ");
            parameter.AppendSql("   AND ENDOGUBUN3 = 'Y'        ");

            parameter.Add("CODE", strExCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXCODE>(parameter);
        }

        public int UpDateGbSuga(string argExCode, string argGbSuga)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql("   SET GBSUGA = :GBSUGA        ");            
            parameter.AppendSql(" WHERE CODE  = :CODE           ");

            #region Query 변수대입
            parameter.Add("GBSUGA", argGbSuga, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", argExCode, Oracle.DataAccess.Client.OracleDbType.Char);

            #endregion
            return ExecuteNonQuery(parameter);
        }

        public List<HIC_EXCODE> GetCodebyPart(string argPart)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,HNAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE DELDATE IS NULL         ");
            if (!argPart.IsNullOrEmpty() && argPart != "*")
            {
                parameter.AppendSql("   AND PART = :PART            ");
            }
           
            parameter.AppendSql(" ORDER BY CODE                 ");

            if (!argPart.IsNullOrEmpty())
            {
                parameter.Add("PART", argPart, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetCodebyEntPart(string strPart)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE ENTPART = :ENTPART      ");
            parameter.AppendSql("   AND DelDate IS NULL         ");
            parameter.AppendSql(" ORDER BY Code                 ");

            parameter.Add("ENTPART", strPart, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public string GetHNmaebyCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HNAME                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetResCodebyCode(string strHicCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESCODE                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");
            parameter.AppendSql("   AND GBAUTOSEND='Y'          "); //결과 자동전송
            parameter.AppendSql("   AND DELDATE IS NULL         ");

            parameter.Add("CODE", strHicCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_EXCODE> GetListByExCode(string argExamCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,HNAME,ENAME,YNAME,UNIT,EXCODE,SUCODE,GCODE,RESCODE,TONGBUN,PART                        ");
            parameter.AppendSql("      ,BRESULT, RESULTTYPE, GBCODEUSE, GBAUTOSEND, MIN_M, MIN_MB, MIN_MR, MAX_M, MAX_MB, MAX_MR    ");
            parameter.AppendSql("      ,MIN_F, MIN_FB, MIN_FR, MAX_F, MAX_FB, MAX_FR, AMT1, AMT2, AMT3, AMT4, AMT5, SUDATE, OLDAMT1 ");
            parameter.AppendSql("      ,OLDAMT2, OLDAMT3, OLDAMT4, OLDAMT5, DELDATE, ENTDATE, ENTSABUN, RESBOHUM1, RESBOHUM2        ");
            parameter.AppendSql("      ,RESCANCER, GBGAM, GBRESULT, AMT6, OLDAMT6, HEASORT, ENTPART, GBRESEMPTY, GBPANBUN1          ");
            parameter.AppendSql("      ,GBPANBUN2, XRAYCODE, BUCODE, SORTA, PANDISP, EXSORT, ACTTIME, HEAPART, HAROOM, ENDOGUBUN1   ");
            parameter.AppendSql("      ,ENDOGUBUN2, ENDOGUBUN3, ENDOGUBUN4, ENDOGUBUN5, ENDOSCOPE, XNAME, XREMARK, XJONG            ");
            parameter.AppendSql("      ,XSUBCODE, XORDERCODE, GBSANGDAM, HEA_PRTGBN, GOTO, EXAMBUN, EXAMFRTO, GBSPCEXAM, SENDBUSE1  ");
            parameter.AppendSql("      ,SENDBUSE2, GBEKGSEND, GBSUGA, GBULINE                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE                                                                      ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                       ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                                             ");
            parameter.AppendSql("   AND EXCODE =:EXCODE                                                                             ");
            parameter.AppendSql(" ORDER BY CODE                                                                                     ");

            parameter.Add("EXCODE", argExamCode, Oracle.DataAccess.Client.OracleDbType.Char);
            
            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetCodeHNamebyPartHname(string strPart, string strName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, HNAME             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE DELDATE IS NULL         ");
            if (!strPart.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PART = :PART      ");
            }
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND HNAME LIKE :HNAME       ");
            }
            parameter.AppendSql("   ORDER BY CODE       ");


            if (!strPart.IsNullOrEmpty())
            {
                parameter.Add("PART", strPart, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("HNAME", strName.Trim());
            }

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetItembyFind(string strTemp, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, HNAME, UNIT           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE      ");
            if (strTemp != "")
            {
                if (strGubun == "1")
                {
                    parameter.AppendSql(" WHERE HNAME LIKE :TEMP    ");
                }
                else if (strGubun == "2")
                {
                    parameter.AppendSql(" WHERE CODE LIKE :TEMP     ");
                }
            }
            parameter.AppendSql(" ORDER BY HNAME                    ");

            parameter.AddLikeStatement("TEMP", strTemp.Trim());

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetCodeAll()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE      ");

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetCodeEndoScope()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE              ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                     ");
            parameter.AppendSql("   AND (ENDOGUBUN3='Y' OR  ENDOGUBUN5='Y') "); //위수면내시경 대상자만

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public List<HIC_EXCODE> GetEntPartbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.ENTPART                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a            ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE b            ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                    ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                ");
            parameter.AppendSql("   AND b.ENTPART NOT IN (' ','Z')          "); //입력조가 없거나 'Z'인 경우 제외]
            parameter.AppendSql(" Group by b.ENTPART                        ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public string GetResultTypebyCode(string strCODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULTTYPE                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", strCODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_EXCODE> GetReferencebyCodeList(List<string> rEFER_CHANGE_CODELIST)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Code, MIN_M, MAX_M, MIN_F, MAX_F, Unit  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE                  ");
            parameter.AppendSql(" WHERE CODE IN (:CODE)                         ");

            parameter.AddInStatement("CODE", rEFER_CHANGE_CODELIST);

            return ExecuteReader<HIC_EXCODE>(parameter);
        }

        public string Read_XrayCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT XRAYCODE                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXCODE  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_EXCODE FindOne(string fCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,HName,EName,YName,Unit,ExCode,SuCode,GCode,ResCode,Part,BResult                    ");
            parameter.AppendSql("      ,DECODE(ResultType,'1','숫자','2','문자','3','공용','4','2줄','문자') STRRESTYPE         ");
            //parameter.AppendSql("      ,DECODE(ResultType2,'1','숫자','2','문자','3','공용','4','2줄','문자') STRRESTYPE2       ");
            parameter.AppendSql("      ,ResultType,ResultType2,GbCodeUse,GbAutoSend,TongBun                                     ");
            parameter.AppendSql("      ,AMT1,AMT2,AMT3,AMT4,AMT5,TO_CHAR(SuDate,'YYYY-MM-DD') SUDATE,OLDAMT1,OLDAMT2,OLDAMT3,OLDAMT4,OLDAMT5                 ");
            parameter.AppendSql("      ,CHKSUGA1,CHKSUGA2,CHKSUGA3,CHKSUGA4,CHKSUGA5                                            ");
            parameter.AppendSql("      ,Min_M,Min_MB,Min_MR,Max_M,Max_MB,Max_MR,Min_F,Min_FB,Min_FR,Max_F,Max_FB,Max_FR         ");
            parameter.AppendSql("      ,DelDate,HeaSORT,EntPart,EntDate,EntSabun                                                ");
            parameter.AppendSql("      ,GbResEmpty,GbPanBun1,GbPanBun2,XRayCode, BUCODE, SORTA, PANDISP, EXSORT                 ");
            parameter.AppendSql("      ,HeaPart,HaRoom,EndoGubun1,EndoGubun2,EndoGubun3,EndoGubun4,EndoGubun5,EndoScope         ");
            parameter.AppendSql("      ,XNAME,XREMARK,XJONG,XSUBCODE,XORDERCODE,Goto,ExamBun,ExamFrTo,GbSpcExam,GbUline         ");
            parameter.AppendSql("      ,SendBuse1,SendBuse2,ResCode2                                                            ");
            parameter.AppendSql("      ,TO_CHAR(SuDate2,'YYYY-MM-DD') SUDATE2,TO_CHAR(SuDate3,'YYYY-MM-DD') SUDATE3             ");
            parameter.AppendSql("      ,TO_CHAR(SuDate4,'YYYY-MM-DD') SUDATE4,TO_CHAR(SuDate5,'YYYY-MM-DD') SUDATE5             ");
            parameter.AppendSql("      ,JAMT1,GAMT1,SAMT1,OAMT1,IAMT1                                                           ");
            parameter.AppendSql("      ,JAMT2,GAMT2,SAMT2,OAMT2,IAMT2                                                           ");
            parameter.AppendSql("      ,JAMT3,GAMT3,SAMT3,OAMT3,IAMT3                                                           ");
            parameter.AppendSql("      ,ROWID AS RID                                                                            ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_EXCODE                                                        ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                   ");
            if (!string.IsNullOrEmpty(fCode))
            {
                parameter.AppendSql("  AND CODE =:Code                                       ");
            }

            if (!string.IsNullOrEmpty(fCode))
            {
                parameter.Add("Code", fCode.Trim(), Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<HIC_EXCODE>(parameter);
        }

        public int Delete(HIC_EXCODE list)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_EXCODE      ");
            parameter.AppendSql("   SET DelDate     =SYSDATE        ");
            parameter.AppendSql(" WHERE ROWID       = :RID          ");

            #region Query 변수대입
            parameter.Add("RID", list.RID);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int Update(HIC_EXCODE list)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_EXCODE      ");
            parameter.AppendSql("   SET HName       =:HName        ");
            parameter.AppendSql("     , EName       =:EName        ");
            parameter.AppendSql("     , YName       =:YName        ");
            parameter.AppendSql("     , Unit        =:Unit         ");
            parameter.AppendSql("     , TongBun     =:TongBun      ");
            parameter.AppendSql("     , ExCode      =:ExCode       ");
            parameter.AppendSql("     , SuCode      =:SuCode       ");
            parameter.AppendSql("     , GCode       =:GCode        ");
            parameter.AppendSql("     , ResCode     =:ResCode      ");
            parameter.AppendSql("     , Part        =:Part         ");
            parameter.AppendSql("     , EntPart     =:EntPart      ");
            parameter.AppendSql("     , EntSabun    =:EntSabun     ");
            parameter.AppendSql("     , BResult     =:BResult      ");
            parameter.AppendSql("     , Min_M       =:Min_M        ");
            parameter.AppendSql("     , Min_MB      =:Min_MB       ");
            parameter.AppendSql("     , Min_MR      =:Min_MR       ");
            parameter.AppendSql("     , Max_M       =:Max_M        ");
            parameter.AppendSql("     , Max_MB      =:Max_MB       ");
            parameter.AppendSql("     , Max_MR      =:Max_MR       ");
            parameter.AppendSql("     , Min_F       =:Min_F        ");
            parameter.AppendSql("     , Min_FB      =:Min_FB       ");
            parameter.AppendSql("     , Min_FR      =:Min_FR       ");
            parameter.AppendSql("     , Max_F       =:Max_F        ");
            parameter.AppendSql("     , Max_FB      =:Max_FB       ");
            parameter.AppendSql("     , Max_FR      =:Max_FR       ");
            parameter.AppendSql("     , ResultType  =:ResultType   ");
            parameter.AppendSql("     , GbCodeUse   =:GbCodeUse    ");
            parameter.AppendSql("     , GbAutoSend  =:GbAutoSend   ");
            if (!list.SUDATE.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SuDate  = TO_DATE(:SuDate, 'YYYY-MM-DD') ");
            }
            parameter.AppendSql("     , HeaSORT     =:HeaSORT      ");
            parameter.AppendSql("     , GbResEmpty  =:GbResEmpty   ");
            parameter.AppendSql("     , GbPanBun1   =:GbPanBun1    ");
            parameter.AppendSql("     , GbPanBun2   =:GbPanBun2    ");
            parameter.AppendSql("     , XRayCode    =:XRayCode     ");
            parameter.AppendSql("     , EntDate     =SYSDATE       ");
            parameter.AppendSql("     , BUCODE      =:BUCODE       ");
            parameter.AppendSql("     , SORTA       =:SORTA        ");
            parameter.AppendSql("     , PANDISP     =:PANDISP      ");
            parameter.AppendSql("     , EXSORT      =:EXSORT       ");
            parameter.AppendSql("     , HeaPart     =:HeaPart      ");
            parameter.AppendSql("     , HaRoom      =:HaRoom       ");
            parameter.AppendSql("     , EndoGubun1  =:EndoGubun1   ");
            parameter.AppendSql("     , EndoGubun2  =:EndoGubun2   ");
            parameter.AppendSql("     , EndoGubun3  =:EndoGubun3   ");
            parameter.AppendSql("     , EndoGubun4  =:EndoGubun4   ");
            parameter.AppendSql("     , EndoGubun5  =:EndoGubun5   ");
            parameter.AppendSql("     , EndoScope   =:EndoScope    ");
            parameter.AppendSql("     , XNAME       =:XNAME        ");
            parameter.AppendSql("     , XREMARK     =:XREMARK      ");
            parameter.AppendSql("     , XJONG       =:XJONG        ");
            parameter.AppendSql("     , XSUBCODE    =:XSUBCODE     ");
            parameter.AppendSql("     , XORDERCODE  =:XORDERCODE   ");
            parameter.AppendSql("     , GbUline     =:GbUline      ");
            parameter.AppendSql("     , SendBuse1   =:SendBuse1    ");
            parameter.AppendSql("     , SendBuse2   =:SendBuse2    ");
            parameter.AppendSql("     , GOTO        =:GOTO         ");
            parameter.AppendSql("     , EXAMBUN     =:EXAMBUN      ");
            parameter.AppendSql("     , GbSpcExam   =:GbSpcExam    ");
            parameter.AppendSql("     , EXAMFRTO    =:EXAMFRTO     ");
            parameter.AppendSql("     , CHKSUGA1    =:CHKSUGA1     ");
            parameter.AppendSql("     , CHKSUGA2    =:CHKSUGA2     ");
            parameter.AppendSql("     , CHKSUGA3    =:CHKSUGA3     ");
            parameter.AppendSql("     , CHKSUGA4    =:CHKSUGA4     ");
            parameter.AppendSql("     , CHKSUGA5    =:CHKSUGA5     ");
            parameter.AppendSql("     , Amt1        =:Amt1         ");
            parameter.AppendSql("     , Amt2        =:Amt2         ");
            parameter.AppendSql("     , Amt3        =:Amt3         ");
            parameter.AppendSql("     , Amt4        =:Amt4         ");
            parameter.AppendSql("     , Amt5        =:Amt5         ");
            parameter.AppendSql("     , OldAmt1     =:OldAmt1      ");
            parameter.AppendSql("     , OldAmt2     =:OldAmt2      ");
            parameter.AppendSql("     , OldAmt3     =:OldAmt3      ");
            parameter.AppendSql("     , OldAmt4     =:OldAmt4      ");
            parameter.AppendSql("     , OldAmt5     =:OldAmt5      ");
            parameter.AppendSql("     , GAmt1       =:GAmt1        ");
            parameter.AppendSql("     , SAmt1       =:SAmt1        ");
            parameter.AppendSql("     , JAmt1       =:JAmt1        ");
            parameter.AppendSql("     , IAmt1       =:IAmt1        ");
            parameter.AppendSql("     , OAmt1       =:OAmt1        ");
            if (!list.SUDATE2.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SuDate2 = TO_DATE(:SuDate2, 'YYYY-MM-DD') ");
            }
            parameter.AppendSql("     , GAmt2       =:GAmt2        ");
            parameter.AppendSql("     , SAmt2       =:SAmt2        ");
            parameter.AppendSql("     , JAmt2       =:JAmt2        ");
            parameter.AppendSql("     , IAmt2       =:IAmt2        ");
            parameter.AppendSql("     , OAmt2       =:OAmt2        ");
            if (!list.SUDATE3.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SuDate3 = TO_DATE(:SuDate3, 'YYYY-MM-DD') ");
            }
            parameter.AppendSql("     , GAmt3       =:GAmt3        ");
            parameter.AppendSql("     , SAmt3       =:SAmt3        ");
            parameter.AppendSql("     , JAmt3       =:JAmt3        ");
            parameter.AppendSql("     , IAmt3       =:IAmt3        ");
            parameter.AppendSql("     , OAmt3       =:OAmt3        ");
            if (!list.SUDATE4.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SuDate4 = TO_DATE(:SuDate4, 'YYYY-MM-DD') ");
            }
            if (!list.SUDATE5.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SuDate5 = TO_DATE(:SuDate5, 'YYYY-MM-DD') ");
            }

            parameter.AppendSql(" WHERE ROWID       =:RID          ");

            #region Query 변수대입
            parameter.Add("HName",      list.HNAME);
            parameter.Add("EName",      list.ENAME);
            parameter.Add("YName",      list.YNAME);
            parameter.Add("Unit",       list.UNIT);
            parameter.Add("TongBun",    list.TONGBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("ExCode",     list.EXCODE);
            parameter.Add("SuCode",     list.SUCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GCode",      list.GCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("ResCode",    list.RESCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("Part",       list.PART);
            parameter.Add("EntPart",    list.ENTPART, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EntSabun",   list.ENTSABUN);
            parameter.Add("BResult",    list.BRESULT);
            parameter.Add("Min_M",      list.MIN_M);
            parameter.Add("Min_MB",     list.MIN_MB);
            parameter.Add("Min_MR",     list.MIN_MR);
            parameter.Add("Max_M",      list.MAX_M);
            parameter.Add("Max_MB",     list.MAX_MB);
            parameter.Add("Max_MR",     list.MAX_MR);
            parameter.Add("Min_F",      list.MIN_F);
            parameter.Add("Min_FB",     list.MIN_FB);
            parameter.Add("Min_FR",     list.MIN_FR);
            parameter.Add("Max_F",      list.MAX_F);
            parameter.Add("Max_FB",     list.MAX_FB);
            parameter.Add("Max_FR",     list.MAX_FR);
            parameter.Add("ResultType", list.RESULTTYPE);
            parameter.Add("GbCodeUse",  list.GBCODEUSE);
            parameter.Add("GbAutoSend", list.GBAUTOSEND);
            if (!list.SUDATE.IsNullOrEmpty())
            {
                parameter.Add("SuDate", list.SUDATE);
            }
            parameter.Add("HeaSORT",    list.HEASORT);
            parameter.Add("GbResEmpty", list.GBRESEMPTY);
            parameter.Add("GbPanBun1",  list.GBPANBUN1);
            parameter.Add("GbPanBun2",  list.GBPANBUN2);
            parameter.Add("XRayCode",   list.XRAYCODE);
            parameter.Add("BUCODE",     list.BUCODE);
            parameter.Add("SORTA",      list.SORTA);
            parameter.Add("PANDISP",    list.PANDISP);
            parameter.Add("EXSORT",     list.EXSORT);
            parameter.Add("HeaPart",    list.HEAPART);
            parameter.Add("HaRoom",     list.HAROOM);
            parameter.Add("EndoGubun1", list.ENDOGUBUN1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EndoGubun2", list.ENDOGUBUN2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EndoGubun3", list.ENDOGUBUN3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EndoGubun4", list.ENDOGUBUN4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EndoGubun5", list.ENDOGUBUN5, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("EndoScope",  list.ENDOSCOPE);
            parameter.Add("XNAME",      list.XNAME);
            parameter.Add("XREMARK",    list.XREMARK);
            parameter.Add("XJONG",      list.XJONG);
            parameter.Add("XSUBCODE",   list.XSUBCODE);
            parameter.Add("XORDERCODE", list.XORDERCODE);
            parameter.Add("GbUline",    list.GBULINE);
            parameter.Add("SendBuse1",  list.SENDBUSE1);
            parameter.Add("SendBuse2",  list.SENDBUSE2);
            parameter.Add("GOTO",       list.GOTO);
            parameter.Add("EXAMBUN",    list.EXAMBUN);
            parameter.Add("GbSpcExam",  list.GBSPCEXAM);
            parameter.Add("EXAMFRTO",   list.EXAMFRTO);
            parameter.Add("CHKSUGA1",   list.CHKSUGA1);
            parameter.Add("CHKSUGA2",   list.CHKSUGA2);
            parameter.Add("CHKSUGA3",   list.CHKSUGA3);
            parameter.Add("CHKSUGA4",   list.CHKSUGA4);
            parameter.Add("CHKSUGA5",   list.CHKSUGA5);
            parameter.Add("Amt1",       list.AMT1);
            parameter.Add("Amt2",       list.AMT2);
            parameter.Add("Amt3",       list.AMT3);
            parameter.Add("Amt4",       list.AMT4);
            parameter.Add("Amt5",       list.AMT5);
            parameter.Add("OldAmt1",    list.OLDAMT1);
            parameter.Add("OldAmt2",    list.OLDAMT2);
            parameter.Add("OldAmt3",    list.OLDAMT3);
            parameter.Add("OldAmt4",    list.OLDAMT4);
            parameter.Add("OldAmt5",    list.OLDAMT5);
            parameter.Add("GAmt1",      list.GAMT1);
            parameter.Add("SAmt1",      list.SAMT1);
            parameter.Add("JAmt1",      list.JAMT1);
            parameter.Add("IAmt1",      list.IAMT1);
            parameter.Add("OAmt1",      list.OAMT1);
            if (!list.SUDATE2.IsNullOrEmpty())
            { 
                parameter.Add("SuDate2",    list.SUDATE2);
            }
            parameter.Add("GAmt2",      list.GAMT2);
            parameter.Add("SAmt2",      list.SAMT2);
            parameter.Add("JAmt2",      list.JAMT2);
            parameter.Add("IAmt2",      list.IAMT2);
            parameter.Add("OAmt2",      list.OAMT2);
            if (!list.SUDATE3.IsNullOrEmpty())
            { 
                parameter.Add("SuDate3",    list.SUDATE3);
            }
            parameter.Add("GAmt3",      list.GAMT3);
            parameter.Add("SAmt3",      list.SAMT3);
            parameter.Add("JAmt3",      list.JAMT3);
            parameter.Add("IAmt3",      list.IAMT3);
            parameter.Add("OAmt3",      list.OAMT3);
            if (!list.SUDATE4.IsNullOrEmpty())
            { 
                parameter.Add("SuDate4",    list.SUDATE4);
            }
            if (!list.SUDATE5.IsNullOrEmpty())
            { 
                parameter.Add("SuDate5",    list.SUDATE5);
            }
            parameter.Add("RID",        list.RID);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_EXCODE item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_EXCODE (");
            parameter.AppendSql("       CODE,HName,EName,YName,Unit,ExCode,SuCode,GCode,ResCode,Part,BResult,ResultType             ");
            parameter.AppendSql("     , GbCodeUse,GbAutoSend,TongBun, SuDate                                                        ");
            parameter.AppendSql("     , Min_M,Min_MB,Min_MR,Max_M,Max_MB,Max_MR,Min_F,Min_FB,Min_FR,Max_F,Max_FB,Max_FR             ");
            parameter.AppendSql("     , DelDate,HeaSORT,EntPart,EntDate,EntSabun                                                    ");
            parameter.AppendSql("     , GbResEmpty,GbPanBun1,GbPanBun2,XRayCode, BUCODE, SORTA, PANDISP, EXSORT                     ");
            parameter.AppendSql("     , HeaPart,HaRoom,EndoGubun1,EndoGubun2,EndoGubun3,EndoGubun4,EndoGubun5,EndoScope             ");
            parameter.AppendSql("     , XNAME,XREMARK,XJONG,XSUBCODE,XORDERCODE,Goto,ExamBun,ExamFrTo,GbSpcExam,GbUline             ");
            parameter.AppendSql("     , SendBuse1,SendBuse2,CHKSUGA1,CHKSUGA2,CHKSUGA3,CHKSUGA4,CHKSUGA5                            ");
            parameter.AppendSql("     , Amt1,Amt2,Amt3,Amt4,Amt5,OldAmt1,OldAmt2,OldAmt3,OldAmt4,OldAmt5                            ");
            parameter.AppendSql(") VALUES (                                                                                         ");
            parameter.AppendSql("       :CODE,:HName,:EName,:YName,:Unit,:ExCode,:SuCode,:GCode,:ResCode,:Part,:BResult,:ResultType ");
            parameter.AppendSql("     , :GbCodeUse,:GbAutoSend,:TongBun, :SuDate                                                    ");
            parameter.AppendSql("     , :Min_M,:Min_MB,:Min_MR,:Max_M,:Max_MB,:Max_MR,:Min_F,:Min_FB,:Min_FR,:Max_F,:Max_FB,:Max_FR ");
            parameter.AppendSql("     , :DelDate,:HeaSORT,:EntPart,:EntDate,:EntSabun,:GbResEmpty,:GbPanBun1,:GbPanBun2             ");
            parameter.AppendSql("     , :XRayCode,:BUCODE,:SORTA,:PANDISP,:EXSORT                                                   ");
            parameter.AppendSql("     , :HeaPart,:HaRoom,:EndoGubun1,:EndoGubun2,:EndoGubun3,:EndoGubun4,:EndoGubun5,:EndoScope     ");
            parameter.AppendSql("     , :XNAME,:XREMARK,:XJONG,:XSUBCODE,:XORDERCODE,:Goto,:ExamBun,:ExamFrTo,:GbSpcExam,:GbUline   ");
            parameter.AppendSql("     , :SendBuse1,:SendBuse2,:CHKSUGA1,:CHKSUGA2,:CHKSUGA3,:CHKSUGA4,:CHKSUGA5                     ");
            parameter.AppendSql("     , :Amt1,:Amt2,:Amt3,:Amt4,:Amt5,:OldAmt1,:OldAmt2,:OldAmt3,:OldAmt4,:OldAmt5                  ");
            parameter.AppendSql(")");

            #region Query 변수대입
            parameter.Add("CODE",       item.CODE);
            parameter.Add("HName",      item.HNAME);
            parameter.Add("EName",      item.ENAME);
            parameter.Add("YName",      item.YNAME);
            parameter.Add("Unit",       item.UNIT);
            parameter.Add("ExCode",     item.EXCODE);
            parameter.Add("SuCode",     item.SUCODE);
            parameter.Add("GCode",      item.GCODE);
            parameter.Add("ResCode",    item.RESCODE);
            parameter.Add("Part",       item.PART);
            parameter.Add("BResult",    item.BRESULT);
            parameter.Add("ResultType", item.RESULTTYPE);
            parameter.Add("GbCodeUse",  item.GBCODEUSE);
            parameter.Add("GbAutoSend", item.GBAUTOSEND);
            parameter.Add("TongBun",    item.TONGBUN);
            parameter.Add("SuDate",     item.SUDATE);
            parameter.Add("Min_M",      item.MIN_M);
            parameter.Add("Min_MB",     item.MIN_MB);
            parameter.Add("Min_MR",     item.MIN_MR);
            parameter.Add("Max_M",      item.MAX_M);
            parameter.Add("Max_MB",     item.MAX_MB);
            parameter.Add("Max_MR",     item.MAX_MR);
            parameter.Add("Min_F",      item.MIN_F);
            parameter.Add("Min_FB",     item.MIN_FB);
            parameter.Add("Min_FR",     item.MIN_FR);
            parameter.Add("Max_F",      item.MAX_F);
            parameter.Add("Max_FB",     item.MAX_FB);
            parameter.Add("Max_FR",     item.MAX_FR);
            parameter.Add("DelDate",    item.DELDATE);
            parameter.Add("HeaSORT",    item.HEASORT);
            parameter.Add("EntPart",    item.ENTPART);
            parameter.Add("EntDate",    item.ENTDATE);
            parameter.Add("EntSabun",   item.ENTSABUN);
            parameter.Add("GbResEmpty", item.GBRESEMPTY);
            parameter.Add("GbPanBun1",  item.GBPANBUN1);
            parameter.Add("GbPanBun2",  item.GBPANBUN2);
            parameter.Add("XRayCode",   item.XRAYCODE);
            parameter.Add("BUCODE",     item.BUCODE);
            parameter.Add("SORTA",      item.SORTA);
            parameter.Add("PANDISP",    item.PANDISP);
            parameter.Add("EXSORT",     item.EXSORT);
            parameter.Add("HeaPart",    item.HEAPART);
            parameter.Add("HaRoom",     item.HAROOM);
            parameter.Add("EndoGubun1", item.ENDOGUBUN1);
            parameter.Add("EndoGubun2", item.ENDOGUBUN2);
            parameter.Add("EndoGubun3", item.ENDOGUBUN3);
            parameter.Add("EndoGubun4", item.ENDOGUBUN4);
            parameter.Add("EndoGubun5", item.ENDOGUBUN5);
            parameter.Add("EndoScope",  item.ENDOSCOPE);
            parameter.Add("XNAME",      item.XNAME);
            parameter.Add("XREMARK",    item.XREMARK);
            parameter.Add("XJONG",      item.XJONG);
            parameter.Add("XSUBCODE",   item.XSUBCODE);
            parameter.Add("XORDERCODE", item.XORDERCODE);
            parameter.Add("Goto",       item.GOTO);
            parameter.Add("ExamBun",    item.EXAMBUN);
            parameter.Add("ExamFrTo",   item.EXAMFRTO);
            parameter.Add("GbSpcExam",  item.GBSPCEXAM);
            parameter.Add("GbUline",    item.GBULINE);
            parameter.Add("SendBuse1",  item.SENDBUSE1);
            parameter.Add("SendBuse2",  item.SENDBUSE2);
            parameter.Add("CHKSUGA1",   item.CHKSUGA1);
            parameter.Add("CHKSUGA2",   item.CHKSUGA2);
            parameter.Add("CHKSUGA3",   item.CHKSUGA3);
            parameter.Add("CHKSUGA4",   item.CHKSUGA4);
            parameter.Add("CHKSUGA5",   item.CHKSUGA5);
            parameter.Add("Amt1",       item.AMT1);
            parameter.Add("Amt2",       item.AMT2);
            parameter.Add("Amt3",       item.AMT3);
            parameter.Add("Amt4",       item.AMT4);
            parameter.Add("Amt5",       item.AMT5);
            parameter.Add("OldAmt1",    item.OLDAMT1);
            parameter.Add("OldAmt2",    item.OLDAMT2);
            parameter.Add("OldAmt3",    item.OLDAMT3);
            parameter.Add("OldAmt4",    item.OLDAMT4);
            parameter.Add("OldAmt5",    item.OLDAMT5);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpdateAmt(HIC_EXCODE list)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_EXCODE                      ");
            parameter.AppendSql("   SET SUDATE = TO_DATE(:SUDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("     , AMT1    =:AMT1                              ");
            parameter.AppendSql("     , AMT2    =:AMT2                              ");
            parameter.AppendSql("     , AMT3    =:AMT3                              ");
            parameter.AppendSql("     , AMT4    =:AMT4                              ");
            parameter.AppendSql("     , AMT5    =:AMT5                              ");

            if (!list.SUDATE2.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SUDATE2 = TO_DATE(:SUDATE2, 'YYYY-MM-DD')   ");
            }

            parameter.AppendSql("     , OLDAMT1 =:OLDAMT1                           ");
            parameter.AppendSql("     , OLDAMT2 =:OLDAMT2                           ");
            parameter.AppendSql("     , OLDAMT3 =:OLDAMT3                           ");
            parameter.AppendSql("     , OLDAMT4 =:OLDAMT4                           ");
            parameter.AppendSql("     , OLDAMT5 =:OLDAMT5                           ");

            if (!list.SUDATE3.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SUDATE3 = TO_DATE(:SUDATE3, 'YYYY-MM-DD')   ");    
            }

            parameter.AppendSql("     , JAMT1 =:JAMT1                               ");
            parameter.AppendSql("     , GAMT1 =:GAMT1                               ");
            parameter.AppendSql("     , SAMT1 =:SAMT1                               ");
            parameter.AppendSql("     , OAMT1 =:OAMT1                               ");
            parameter.AppendSql("     , IAMT1 =:IAMT1                               ");

            if (!list.SUDATE4.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SUDATE4 = TO_DATE(:SUDATE4, 'YYYY-MM-DD')   ");
            }

            parameter.AppendSql("     , JAMT2 =:JAMT2                               ");
            parameter.AppendSql("     , GAMT2 =:GAMT2                               ");
            parameter.AppendSql("     , SAMT2 =:SAMT2                               ");
            parameter.AppendSql("     , OAMT2 =:OAMT2                               ");
            parameter.AppendSql("     , IAMT2 =:IAMT2                               ");

            if (!list.SUDATE5.IsNullOrEmpty())
            {
                parameter.AppendSql("     , SUDATE5 = TO_DATE(:SUDATE5, 'YYYY-MM-DD')   ");
            }

            parameter.AppendSql("     , JAMT3 =:JAMT3                               ");
            parameter.AppendSql("     , GAMT3 =:GAMT3                               ");
            parameter.AppendSql("     , SAMT3 =:SAMT3                               ");
            parameter.AppendSql("     , OAMT3 =:OAMT3                               ");
            parameter.AppendSql("     , IAMT3 =:IAMT3                               ");
            parameter.AppendSql("     , ENTSABUN =:ENTSABUN                               ");
            parameter.AppendSql("     , ENTDATE =SYSDATE                               ");
            parameter.AppendSql(" WHERE CODE  = :CODE                               ");

            #region Query 변수대입
            parameter.Add("SUDATE",  list.SUDATE);
            parameter.Add("AMT1",    list.AMT1);
            parameter.Add("AMT2", list.AMT2);
            parameter.Add("AMT3", list.AMT3);
            parameter.Add("AMT4", list.AMT4);
            parameter.Add("AMT5", list.AMT5);

            if (!list.SUDATE2.IsNullOrEmpty())
            {
                parameter.Add("SUDATE2", list.SUDATE2);    
            }

            parameter.Add("OLDAMT1", list.OLDAMT1);
            parameter.Add("OLDAMT2", list.OLDAMT2);
            parameter.Add("OLDAMT3", list.OLDAMT3);
            parameter.Add("OLDAMT4", list.OLDAMT4);
            parameter.Add("OLDAMT5", list.OLDAMT5);

            if (!list.SUDATE3.IsNullOrEmpty())
            {
                parameter.Add("SUDATE3", list.SUDATE3);                
            }

            parameter.Add("JAMT1", list.JAMT1);
            parameter.Add("GAMT1", list.GAMT1);
            parameter.Add("SAMT1", list.SAMT1);
            parameter.Add("OAMT1", list.OAMT1);
            parameter.Add("IAMT1", list.IAMT1);

            if (!list.SUDATE4.IsNullOrEmpty())
            {
                parameter.Add("SUDATE4", list.SUDATE4);    
            }

            parameter.Add("JAMT2", list.JAMT2);
            parameter.Add("GAMT2", list.GAMT2);
            parameter.Add("SAMT2", list.SAMT2);
            parameter.Add("OAMT2", list.OAMT2);
            parameter.Add("IAMT2", list.IAMT2);

            if (!list.SUDATE5.IsNullOrEmpty())
            {
                parameter.Add("SUDATE5", list.SUDATE5);
            }

            parameter.Add("JAMT3", list.JAMT3);
            parameter.Add("GAMT3", list.GAMT3);
            parameter.Add("SAMT3", list.SAMT3);
            parameter.Add("OAMT3", list.OAMT3);
            parameter.Add("IAMT3", list.IAMT3);

            parameter.Add("CODE",    list.CODE);

            parameter.Add("ENTSABUN", list.ENTSABUN);

            #endregion
            return ExecuteNonQuery(parameter);
        }

        public HIC_EXCODE Read_HaRoom(string HEAPART)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HAROOM                              ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_EXCODE    ");
            parameter.AppendSql(" WHERE HEAPART = :HEAPART                  ");
            parameter.AppendSql(" GROUP BY HAROOM                           ");
            parameter.AppendSql(" ORDER BY HAROOM                           ");

            parameter.Add("HEAPART", HEAPART, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXCODE>(parameter);
        }

        public HIC_EXCODE Read_HcRoom(string ENTPART)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HCROOM                              ");
            parameter.AppendSql("  FROM " + ComNum.DB_PMPA + "HIC_EXCODE    ");
            parameter.AppendSql(" WHERE ENTPART = :ENTPART                  ");
            parameter.AppendSql(" GROUP BY HCROOM                           ");
            parameter.AppendSql(" ORDER BY HCROOM                           ");

            parameter.Add("ENTPART", ENTPART, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXCODE>(parameter);
        }

        public HIC_EXCODE GetEndoGubunbyCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ENDOGUBUN1,ENDOGUBUN2,ENDOGUBUN3,ENDOGUBUN4,ENDOGUBUN5                  ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_EXCODE                                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql(" AND CODE =:Code                                                               ");
            parameter.AppendSql(" AND (ENDOGUBUN2='Y' OR ENDOGUBUN3='Y' OR ENDOGUBUN4='Y' OR ENDOGUBUN5='Y')    ");

            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXCODE>(parameter);
        }

        public HIC_EXCODE GetGotobyCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GOTO                                                                    ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_EXCODE                                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql(" AND CODE =:Code                                                               ");
            parameter.AppendSql(" AND DELDATE IS NULL                                                           ");

            parameter.Add("CODE", argCode, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXCODE>(parameter);
        }

    }
}
