
namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;

    public class ComHpcLibBRepository : BaseRepository
    {
        public ComHpcLibBRepository()
        {
        }

        public string Read_JikWonName(string SABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT trim(KORNAME) KORNAME   ");
            parameter.AppendSql("  FROM KOSMOS_ADM.INSA_MST     ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", SABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);

        }

        public string Seq_PacsNo()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEQ_PACSNO.NEXTVAL NextPacsNo FROM DUAL ");

            return ExecuteScalar<string>(parameter);
        }

        public string Read_SName(string JUMIN1, string JUMIN3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT  ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1        ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN3        ");

            parameter.Add("JUMIN1", JUMIN1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", JUMIN3);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Insa_Mst(string SABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KORNAME             ");
            parameter.AppendSql("  FROM KOSMOS_ADM.INSA_MST ");
            parameter.AppendSql(" WHERE SABUN = :SABUN      ");

            parameter.Add("SABUN", SABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public COMHPC GetPatientInfoByGubunTable(string argTable, string fstrKey)
        {
            MParameter parameter = CreateParameter();

            if (argTable == "BAS_PATIENT")
            {
                parameter.AppendSql("SELECT SNAME, JUMIN1, JUMIN3, TEL, HPHONE      ");
            }
            else
            {
                parameter.AppendSql("SELECT SNAME, JUMIN2, TEL, HPHONE      ");
            }

            parameter.AppendSql("  FROM KOSMOS_PMPA." + argTable + "    ");

            if (argTable == "HIC_CANCER_RESV2")
            {
                parameter.AppendSql(" WHERE ROWID = :KEYVALUE                    ");
            }
            else
            {
                parameter.AppendSql(" WHERE PANO = :KEYVALUE                    ");
            }

            parameter.Add("KEYVALUE", fstrKey);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<EMR_CONSENT> GetEmrNoByPtnoDate(string fstrPtno, string fstrFDate, string fstrTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT F.FORMNO                                    ");
            parameter.AppendSql("   , MAX(F.UPDATENO) AS UPDATENO                   ");
            parameter.AppendSql("   , MAX(F.FORMNAME) AS FORMNAME                   ");
            parameter.AppendSql("   , MAX(C.CREATEDUSER) AS CREATEDUSER             ");
            parameter.AppendSql("   , MAX(C.CREATED) AS CREATED                     ");
            parameter.AppendSql("   , MAX(C.ID) AS ID                               ");
            parameter.AppendSql("  FROM KOSMOS_EMR.AEMRFORM F                       ");
            parameter.AppendSql("    INNER JOIN KOSMOS_EMR.AEASFORMCONTENT B        ");
            parameter.AppendSql("       ON F.FORMNO = B.FORMNO                      ");
            parameter.AppendSql("     AND F.UPDATENO = B.UPDATENO                   ");
            parameter.AppendSql("   INNER JOIN KOSMOS_EMR.AEASFORMDATA C            ");
            parameter.AppendSql("         ON B.ID = C.EASFORMCONTENT                ");
            parameter.AppendSql(" WHERE C.PTNO = :PTNO                              ");
            parameter.AppendSql("   AND C.CREATED >= TO_DATE(:FCREATED, 'YYYY-MM-DD')             ");
            parameter.AppendSql("   AND C.CREATED <= TO_DATE(:TCREATED, 'YYYY-MM-DD')             ");
            parameter.AppendSql("   AND C.ISDELETED = 'N'                           ");
            parameter.AppendSql("   AND C.INOUTCLS = 'O'                            ");
            parameter.AppendSql("   AND F.FORMNO IN (3166, 3257, 3261, 3415, 3553, 3554, 3555, 3556, 3557, 3558) ");
            parameter.AppendSql(" GROUP BY F.FORMNO, F.FORMNAME                     ");

            parameter.Add("PTNO", fstrPtno);
            parameter.Add("FCREATED", fstrFDate);
            parameter.Add("TCREATED", fstrTDate);

            return ExecuteReader<EMR_CONSENT>(parameter);
        }

        public List<COMHPC> GetListChukMstByLtdCode(long argLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.LTDCODE, A.CHKYEAR AS GJYEAR, A.LTDSEQNO");
            parameter.AppendSql("     , DECODE(A.BANGI, '1', '상반기', '2', '하반기', '') AS BANGI");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CHUKDTL_PLAN_CNT(A.WRTNO) AS COUNT1 ");
            parameter.AppendSql("     , '0' AS COUNT2 ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CHUKDTL_UCODE_CNT(A.WRTNO, '1') AS COUNT3 ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_CHUKDTL_UCODE_CNT(A.WRTNO, '2') AS COUNT4 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND A.LTDCODE =:LTDCODE");
            parameter.AppendSql("   AND A.DELDATE IS NULL");
            parameter.AppendSql(" GROUP BY A.LTDCODE, A.CHKYEAR, A.LTDSEQNO, A.BANGI,A.WRTNO");

            parameter.Add("LTDCODE", argLtdCode);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetListByDate(string argFDate, string argTDate, string argSName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.PTNO, A.SNAME, A.AGE, 'TO' AS DEPTCODE                            ");
            parameter.AppendSql("     , TO_CHAR(A.SDATE,'YYYY-MM-DD') JEPDATE                               ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HEA_JEPSU A, KOSMOS_PMPA.HIC_CONSENT B                   ");
            parameter.AppendSql(" WHERE A.PTNO = B.PTNO                                                     ");
            parameter.AppendSql(" AND A.SDATE = B.SDATE                                                     ");
            parameter.AppendSql(" AND A.SDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                             ");
            parameter.AppendSql(" AND A.SDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                             ");
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.SNAME LIKE :SNAME                                             ");
            }
            parameter.AppendSql(" AND A.DELDATE IS NULL                                                     ");
            parameter.AppendSql(" AND B.EMRNO IS NOT NULL                                                   ");
            parameter.AppendSql(" GROUP BY A.PTNO, A.SNAME, A.AGE, A.SDATE                                  ");
            //일반검진 추가
            parameter.AppendSql(" UNION                                                                     ");
            parameter.AppendSql("SELECT A.PTNO, A.SNAME, A.AGE, 'HR' AS DEPTCODE                            ");
            parameter.AppendSql("     , TO_CHAR(A.JEPDATE,'YYYY-MM-DD') JEPDATE                             ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_JEPSU A, KOSMOS_PMPA.HIC_CONSENT B                   ");
            parameter.AppendSql(" WHERE A.PTNO = B.PTNO                                                     ");
            parameter.AppendSql(" AND  A.JEPDATE = B.SDATE                                                  ");
            parameter.AppendSql(" AND A.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql(" AND A.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                           ");
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.SNAME LIKE :SNAME                                             ");
            }
            parameter.AppendSql(" AND A.DELDATE IS NULL                                                     ");
            parameter.AppendSql(" AND B.EMRNO IS NOT NULL                                                   ");
            parameter.AppendSql(" GROUP BY A.PTNO, A.SNAME, A.AGE, A.JEPDATE                                ");
            parameter.AppendSql(" ORDER BY SName                                                            ");

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSName);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetListTableLikeJumin()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT OWNER, TABLE_NAME AS TBLNAME, COLUMN_NAME AS COLNAME      ");
            parameter.AppendSql("  FROM ALL_TAB_COLUMNS              ");
            parameter.AppendSql(" WHERE COLUMN_NAME LIKE '%JUMIN%'   ");
            parameter.AppendSql("   AND SUBSTR(TABLE_NAME, 1, 4) != 'VIEW'  ");

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> CheckJuminNoTable(string oWNER, string tBLNAME, string cOLNAME)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT " + cOLNAME + " AS JUMIN      ");
            parameter.AppendSql("  FROM " + oWNER + "." + tBLNAME + " ");
            parameter.AppendSql(" WHERE " + cOLNAME + " IS NOT NULL   ");
            parameter.AppendSql("   AND LENGTH(" + " TRIM(" + cOLNAME + ")" + ") = 13   ");
            parameter.AppendSql("   AND SUBSTR(" + cOLNAME + ", 8, 1) != '*'   ");
            parameter.AppendSql("   AND ROWNUM <= 100   ");

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetHeaCodeOcsOOrderbyDeptCodePtNo(string argDeptCode, string argPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_CODE  A                 ");
            parameter.AppendSql("     , KOSMOS_OCS.OCS_OORDER B                 ");
            parameter.AppendSql(" WHERE A.GUBUN      = '18'                     ");
            parameter.AppendSql("   AND B.DEPTCODE   = :DEPTCODE                ");
            parameter.AppendSql("   AND B.PTNO       = :PTNO                    ");
            parameter.AppendSql("   AND TRIM(A.NAME) = TRIM(B.ORDERCODE)        ");
            parameter.AppendSql("   AND B.BDATE      = TRUNC(SYSDATE)           ");

            parameter.Add("DEPTCODE", argDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PTNO", argPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public COMHPC Read_Ocs_Doctor_DrCode(string argDrCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRNAME, DRBUNHO, DRCODE ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE DRCODE = :DRCODE          ");

            parameter.Add("DRCODE", argDrCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public long GetSpcMirNobyDual()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HIC_SPCMIRNO.NEXTVAL SPCMIRNO FROM DUAL     ");

            return ExecuteScalar<long>(parameter);
        }

        public void SelectInsertTableHicJepsu()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_PATIENT                ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HIC_PATIENT@PSMH         ");
            parameter.AppendSql(" WHERE PANO NOT IN ( SELECT PANO FROM HIC_PATIENT) ");

            ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetDeptName()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DEPTCODE, DEPTNAMEK                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_CLINICDEPT              ");
            parameter.AppendSql(" WHERE PRINTRANKING < 39                       ");
            parameter.AppendSql("   AND DEPTCODE NOT IN ('HC', 'II', 'R6')      ");
            parameter.AppendSql(" ORDER BY PRINTRANKING                         ");

            return ExecuteReader<COMHPC>(parameter);
        }

        public void SelectInsertTableHicTable(string argTable, string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO " + argTable + "                               ");
            parameter.AppendSql("SELECT * FROM " + argTable + "@PSMH                        ");
            parameter.AppendSql(" WHERE WRTNO IN (                                          ");
            parameter.AppendSql("       SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU@PSMH        ");
            parameter.AppendSql("        WHERE JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("          AND DELDATE IS NULL                              ");
            parameter.AppendSql(" )                                                         ");

            parameter.Add("JEPDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetBasPatPoscoListByExamres15(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.PANO AS PTNO, TO_CHAR(A.EXAMRES15,'HH24:MI') RDATE           ");
            parameter.AppendSql("     , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PANO) SNAME   ");
            parameter.AppendSql("     , '' GBEXAM, '' EXCODE, A.JDATE AS SDATE, 0 WRTNO, '뇌혈류초음파' EXAMNAME    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT_POSCO A                         ");
            parameter.AppendSql(" WHERE A.EXAMRES15 >= TO_DATE(:SDATE,'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND A.EXAMRES15 <  TO_DATE(:SDATE,'YYYY-MM-DD')  + 0.999999   ");
            parameter.AppendSql(" ORDER BY A.JDATE, A.SNAME                                         ");

            parameter.Add("SDATE", strDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetBillNo()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_ADM.SEQ_TRBEBILL.NEXTVAL SEQNO FROM DUAL    ");
            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetLtdInwon2byLtdCodeYear(string strOldData, string strYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT INWON011,INWON012                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_LTDINWON2               ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            if (!strOldData.IsNullOrEmpty() && strOldData != "0")
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                  ");
            }
            parameter.AppendSql("   AND YEAR = :YEAR                            ");

            if (!strOldData.IsNullOrEmpty() && strOldData != "0")
            {
                parameter.Add("LTDCODE", strOldData);
            }
            parameter.Add("YEAR", strYear);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<COMHPC> GetExpenseItembyJepDateJohap(string strJong, string strFDate, string strTDate, string strSabun, string str직역구분, string strLife)
        {
            MParameter parameter = CreateParameter();

            if (strJong == "*")
            {
                parameter.AppendSql("SELECT '1' AS GUBUN, MIRNO, YEAR, JOHAP,JEPNO, JEPQTY                                                                      ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE                                          ");
                parameter.AppendSql("      , FileName , TAmt, ONE_TAmt, TWO_TAmt, NhicNo                                                                        ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_BOHUM                                                                                          ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                          ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "Y")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                             ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT , ONE_TAMT, TWO_TAMT,NhicNo                     ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" Union All                                                                                                                 ");
                parameter.AppendSql(" SELECT '2'AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, LTDCODE                                                              ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT, 0 ONE_TAmt              ");
                parameter.AppendSql("      , 0 TWO_TAmt, NhicNo                                                                                                 ");
                parameter.AppendSql("   FROM KOSMOs_pmpa.HIC_MIR_DENTAL                                                                                         ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                          ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                             ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY,FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                   ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" Union All                                                                                                                 ");
                parameter.AppendSql(" SELECT '3'AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, LTDCODE                                                              ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT,0 ONE_TAmt, 0 TWO_TAmt   ");
                parameter.AppendSql("      , NhicNo                                                                                                             ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_CANCER                                                                                         ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JOHAP NOT IN ('X')                                                                                                 ");
                parameter.AppendSql("    AND (GBBOGUN NOT IN ('Y','E') OR GBBOGUN IS NULL )                                                                     ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BuildSabun = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn = '' )                                                                            ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                  ");
                parameter.AppendSql(" HAVING MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" UNION ALL                                                                                                                 ");
                parameter.AppendSql(" SELECT '4'AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, LTDCODE                                                              ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT,0 ONE_TAmt               ");
                parameter.AppendSql("      , 0 TWO_TAmt,NhicNo                                                                                                  ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_CANCER_BO                                                                                      ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                                           ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                                           ");
                parameter.AppendSql("    AND Johap  NOT IN ('X')                                                                                                ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                             ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                  ");
                parameter.AppendSql(" HAVING MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" UNION ALL                                                                                                                 ");
                parameter.AppendSql(" SELECT '5' AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, LTDCODE                                                             ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT,0 ONE_TAmt               ");
                parameter.AppendSql("      , 0 TWO_TAmt,NhicNo                                                                                                  ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_CANCER                                                                                         ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JOHAP  IN ('X')                                                                                                    ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                             ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                  ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" ORDER BY 1                                                                                                                ");
            }
            else if (strJong == "1")
            {
                parameter.AppendSql("SELECT '1' AS GUBUN, MIRNO, YEAR, JOHAP,JEPNO, JEPQTY                                                                      ");
                parameter.AppendSql("     , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE                                           ");
                parameter.AppendSql("     , FileName , TAmt, ONE_TAmt, TWO_TAmt,NhicNo                                                                          ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_BOHUM                                                                                           ");
                parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                           ");
                parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                           ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                             ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT , ONE_TAMT, TWO_TAMT,NhicNo                     ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" ORDER BY 1                                                                                                                ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql(" SELECT '2'AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY                                                                       ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT, 0 ONE_TAMT              ");
                parameter.AppendSql("      , 0 TWO_TAMT,NhicNo                                                                                                  ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_DENTAL                                                                                         ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                                           ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                                           ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                             ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                  ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" ORDER BY 1                                                                                                                ");
            }
            else if (strJong == "4")
            {
                parameter.AppendSql(" SELECT '3'AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY                                                                       ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT,0 ONE_TAMT               ");
                parameter.AppendSql("      , 0 TWO_TAMT,NhicNo                                                                                                  ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_CANCER                                                                                         ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND (GBBOGUN NOT IN ('Y','E') OR GBBOGUN IS NULL )                                                                     ");
                parameter.AppendSql("    AND JOHAP NOT IN ('X')                                                                                                 ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("   AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                             ");
                }
                else
                {
                    parameter.AppendSql("   AND Life_Gbn = 'Y'                                                                                                  ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                  ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" ORDER BY 1                                                                                                                ");
            }
            else if (strJong == "5")
            {
                parameter.AppendSql(" SELECT '4'AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY                                                                       ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT,0 ONE_TAMT               ");
                parameter.AppendSql("      , 0 TWO_TAMT,NhicNo                                                                                                  ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_CANCER_BO                                                                                      ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                          ");
                parameter.AppendSql("    AND NhicNo IS NOT NULL                                                                                                 ");
                if (strSabun != "")
                {
                    parameter.AppendSql("   AND BUILDSABUN = :SABUN                                                                                             ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("    AND JOHAP = :JOHAP                                                                                                 ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("    AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                            ");
                }
                else
                {
                    parameter.AppendSql("    AND Life_Gbn = 'Y'                                                                                                 ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                  ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" ORDER BY 1                                                                                                                ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql(" SELECT '5'AS GUBUN,MIRNO, YEAR, JOHAP,JEPNO, JEPQTY                                                                       ");
                parameter.AppendSql("      , TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE,  TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, FILENAME, TAMT,0 ONE_TAMT               ");
                parameter.AppendSql("      , 0 TWO_TAMT,NhicNo                                                                                                  ");
                parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_CANCER                                                                                         ");
                parameter.AppendSql("  WHERE JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                                                           ");
                parameter.AppendSql("    AND JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                                                           ");
                parameter.AppendSql("    AND JOHAP IN ('X')                                                                                                     ");
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("    AND BUILDSABUN = :SABUN                                                                                            ");
                }
                if (str직역구분 != "*")
                {
                    parameter.AppendSql("   AND JOHAP = :JOHAP                                                                                                  ");
                }
                if (strLife == "")
                {
                    parameter.AppendSql("    AND (Life_Gbn IS NULL OR Life_Gbn ='' )                                                                            ");
                }
                else
                {
                    parameter.AppendSql("    AND Life_Gbn = 'Y'                                                                                                 ");
                }
                parameter.AppendSql(" GROUP BY MIRNO, YEAR, JOHAP,JEPNO, JEPQTY, FRDATE, TODATE ,FILENAME, TAMT, 11, 12,NhicNo                                  ");
                parameter.AppendSql(" Having MIRNO > 0                                                                                                          ");
                parameter.AppendSql(" ORDER BY 1                                                                                                                ");
            }

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            if (!strSabun.IsNullOrEmpty())
            {
                parameter.Add("SABUN", strSabun);
            }
            if (str직역구분 != "*")
            {
                parameter.Add("JOHAP", str직역구분);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public int InsertBill(COMHPC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO      KOSMOS_ADM.TRB_ITEMLIST (                                                                     ");

            parameter.AppendSql("                 SEQID,DETAILSEQID,ITEMDATE,ITEMNAME,ITEMTYPE,ITEMQTY,ITEMPRICE,ITEMSUPPRICE                   ");
            parameter.AppendSql("               , ITEMTAX,ITEMREMARKS                                                                           ");

            parameter.AppendSql("                 ) VALUES (                                                                                    ");

            parameter.AppendSql("                 :SEQID,:DETAILSEQID,:ITEMDATE,:ITEMNAME,:ITEMTYPE,:ITEMQTY,:ITEMPRICE,:ITEMSUPPRICE           ");
            parameter.AppendSql("               , :ITEMTAX,:ITEMREMARKS )                                                                       ");


            parameter.Add("SEQID", item.SEQID);
            parameter.Add("DETAILSEQID", item.DETAILSEQID);
            parameter.Add("ITEMDATE", item.ITEMDATE);
            parameter.Add("ITEMNAME", item.ITEMNAME);
            parameter.Add("ITEMTYPE", item.ITEMTYPE);
            parameter.Add("ITEMQTY", item.ITEMQTY);
            parameter.Add("ITEMPRICE", item.ITEMPRICE);
            parameter.Add("ITEMSUPPRICE", item.ITEMSUPPRICE);
            parameter.Add("ITEMTAX", item.ITEMTAX);
            parameter.Add("ITEMREMARKS", item.ITEMREMARKS);

            return ExecuteNonQuery(parameter);
        }

        public int SendBill(COMHPC item, string cboChungu, string cboMJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO      KOSMOS_ADM.TRB_SALEEBILL (                                                                    ");

            parameter.AppendSql("                 RESSEQ,DOCTYPE,DOCCODE,EBILLKIND,CUSTOMS,TAXSNUM,DOCATTR,PUBDATE                              ");
            parameter.AppendSql("               , SYSTEMCODE,PUBTYPE,PUBFORM,MEMID,MEMDEPTNAME,MEMNAME                                          ");
            parameter.AppendSql("               , EMAIL,TEL,COREGNO,COTAXREGNO,CONAME,COCEO,COADDR,COBIZTYPE,COBIZSUB,RECMEMID                  ");
            parameter.AppendSql("               , RECMEMDEPTNAME,RECMEMNAME,RECEMAIL,RECTEL,RECCOREGNOTYPE,RECCOREGNO,RECCOTAXREGNO             ");
            parameter.AppendSql("               , RECCONAME,RECCOCEO,RECCOADDR,RECCOBIZTYPE,RECCOBIZSUB                                         ");
            parameter.AppendSql("               , SUPPRICE,TAX,CASH,CHEQUE,BILL,OUTSTAND,PUBKIND,SMS,REMARKS                                    ");

            parameter.AppendSql("                 ) VALUES (                                                                                    ");

            parameter.AppendSql("                 :RESSEQ,:DOCTYPE,:DOCCODE,:EBILLKIND,:CUSTOMS,:TAXSNUM,:DOCATTR,:PUBDATE                      ");
            parameter.AppendSql("               , :SYSTEMCODE,:PUBTYPE,:PUBFORM,:MEMID,:MEMDEPTNAME,:MEMNAME                                    ");
            parameter.AppendSql("               , :EMAIL,:TEL,:COREGNO,:COTAXREGNO,:CONAME,:COCEO,:COADDR,:COBIZTYPE,:COBIZSUB,:RECMEMID        ");
            parameter.AppendSql("               , :RECMEMDEPTNAME,:RECMEMNAME,:RECEMAIL,:RECTEL,:RECCOREGNOTYPE,:RECCOREGNO,:RECCOTAXREGNO      ");
            parameter.AppendSql("               , :RECCONAME,:RECCOCEO,:RECCOADDR,:RECCOBIZTYPE,:RECCOBIZSUB                                    ");
            parameter.AppendSql("               , :SUPPRICE,:TAX,:CASH,:CHEQUE,:BILL,:OUTSTAND,:PUBKIND,:SMS,:REMARKS)                          ");

            parameter.Add("RESSEQ", item.RESSEQ);
            parameter.Add("DOCTYPE", item.DOCTYPE);
            parameter.Add("DOCCODE", item.DOCCODE);
            parameter.Add("EBILLKIND", item.EBILLKIND);
            parameter.Add("CUSTOMS", item.CUSTOMS);
            parameter.Add("TAXSNUM", item.TAXSNUM);
            parameter.Add("DOCATTR", item.DOCATTR);
            parameter.Add("PUBDATE", item.PUBDATE);

            parameter.Add("SYSTEMCODE", item.SYSTEMCODE);
            parameter.Add("PUBTYPE", item.PUBTYPE);
            switch (cboChungu)
            {
                case "1":
                case "2":
                case "3":
                    parameter.Add("PUBFORM", item.PUBFORM);
                    break;
                default:
                    parameter.Add("PUBFORM", item.PUBFORM2);
                    break;
            }
            parameter.Add("MEMID", item.MEMID);

            switch (cboChungu)
            {
                case "1":
                    parameter.Add("MEMDEPTNAME", item.MEMDEPTNAME);
                    parameter.Add("MEMNAME", item.MEMNAME);
                    parameter.Add("EMAIL", item.EMAIL);
                    parameter.Add("TEL", item.TEL);
                    break;
                case "2":
                    parameter.Add("MEMDEPTNAME", item.MEMDEPTNAME2);
                    parameter.Add("MEMNAME", item.MEMNAME);
                    parameter.Add("EMAIL", item.EMAIL);
                    parameter.Add("TEL", item.TEL2);
                    break;
                case "3":
                    parameter.Add("MEMDEPTNAME", item.MEMDEPTNAME3);
                    parameter.Add("MEMNAME", item.MEMNAME);
                    parameter.Add("EMAIL", item.EMAIL);
                    parameter.Add("TEL", item.TEL2);
                    break;
                case "4":
                    parameter.Add("MEMDEPTNAME", item.MEMDEPTNAME4);
                    parameter.Add("MEMNAME", item.MEMNAME);
                    parameter.Add("EMAIL", item.EMAIL);
                    parameter.Add("TEL", item.TEL3);
                    break;
                default:
                    parameter.Add("MEMDEPTNAME", item.MEMDEPTNAME);
                    parameter.Add("MEMNAME", item.MEMNAME);
                    parameter.Add("EMAIL", item.EMAIL);
                    parameter.Add("TEL", item.TEL);
                    break;
            }

            parameter.Add("COREGNO", item.COREGNO);
            parameter.Add("COTAXREGNO", item.COTAXREGNO);
            parameter.Add("CONAME", item.CONAME);
            parameter.Add("COCEO", item.COCEO);
            parameter.Add("COADDR", item.COADDR);
            parameter.Add("COBIZTYPE", item.COBIZTYPE);
            parameter.Add("COBIZSUB", item.COBIZSUB);
            parameter.Add("RECMEMID", item.RECMEMID);

            parameter.Add("RECMEMDEPTNAME", item.RECMEMDEPTNAME);
            parameter.Add("RECMEMNAME", item.RECMEMNAME);
            parameter.Add("RECEMAIL", item.RECEMAIL);
            parameter.Add("RECTEL", item.RECTEL);
            parameter.Add("RECCOREGNOTYPE", item.RECCOREGNOTYPE);
            parameter.Add("RECCOREGNO", item.RECCOREGNO);
            parameter.Add("RECCOTAXREGNO", item.RECCOTAXREGNO);

            parameter.Add("RECCONAME", item.RECCONAME);
            parameter.Add("RECCOCEO", item.RECCOCEO);
            parameter.Add("RECCOADDR", item.RECCOADDR);
            parameter.Add("RECCOBIZTYPE", item.RECCOBIZTYPE);
            parameter.Add("RECCOBIZSUB", item.RECCOBIZSUB);

            parameter.Add("SUPPRICE", item.SUPPRICE);
            parameter.Add("TAX", item.TAX);

            switch (cboChungu)
            {
                case "1":
                    parameter.Add("CASH", item.CASH);
                    parameter.Add("CHEQUE", "0");
                    parameter.Add("BILL", "0");
                    parameter.Add("OUTSTAND", "0");
                    break;
                case "2":
                    parameter.Add("CASH", "0");
                    parameter.Add("CHEQUE", item.CHEQUE);
                    parameter.Add("BILL", "0");
                    parameter.Add("OUTSTAND", "0");
                    break;
                case "3":
                    parameter.Add("CASH", "0");
                    parameter.Add("CHEQUE", "0");
                    parameter.Add("BILL", item.BILL);
                    parameter.Add("OUTSTAND", "0");
                    break;
                case "4":
                    parameter.Add("CASH", "");
                    parameter.Add("CHEQUE", "0");
                    parameter.Add("BILL", "0");
                    parameter.Add("OUTSTAND", item.OUTSTAND);
                    break;
            }

            parameter.Add("PUBKIND", item.PUBKIND);
            parameter.Add("SMS", item.SMS);
            parameter.Add("REMARKS", item.REMARKS);

            return ExecuteNonQuery(parameter);
        }

        public string GetGjjongListByPtnoJepDate(string strPtno, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.FC_HIC_JEPSUJONG(:PTNO, :SDATE) JEPSUJONG   ");
            parameter.AppendSql("  FROM DUAL                                                    ");

            parameter.Add("PTNO", strPtno);
            parameter.Add("SDATE", strJepDate);

            return ExecuteScalar<string>(parameter);
        }

        public long GetFormNoByFormCD(string frmCD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FORMNO FROM KOSMOS_PMPA.HIC_CONSENT_FORM    ");
            parameter.AppendSql(" WHERE FORMCODE = :FORMCODE                        ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("FORMCODE", frmCD);

            return ExecuteScalar<long>(parameter);
        }

        public long GetFormUpDateNoByFormCD(long frmNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT UPDATENO FROM KOSMOS_EMR.AEMRFORM   ");
            parameter.AppendSql(" WHERE FORMNO = :FORMNO                    ");
            parameter.AppendSql("   AND USECHECK = 1                        ");

            parameter.Add("FORMNO", frmNo);

            return ExecuteScalar<long>(parameter);
        }

        public int InsertMirHems(long argMirNo, string argYear, long argLtdCode, string argMinDate, string argMaxDate, int argCnt1, long argSabun, int argCnt2, string argOHMS, string argBook, int argCnt3, string argGubun1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_MIR_HEMS (                                                             ");
            parameter.AppendSql("       HEMSNO, YEAR, LTDCODE, FRDATE, TODATE                                                       ");
            parameter.AppendSql("     , BUILDCNT, BUILDDATE, BUILDSABUN, SEQNO, GBOHMS, SUCHUPYN, JOBSEQ, GBCHK1 )                  ");
            parameter.AppendSql("VALUES                                                                                             ");
            parameter.AppendSql("       (:HEMSNO, :YEAR, :LTDCODE, TO_DATE(:FRDATE, 'YYYY-MM-DD'), TO_DATE(:TODATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , :BUILDCNT, SYSDATE, :BUILDSABUN, :SEQNO, :GBOHMS, :SUCHUPYN, :JOBSEQ, :GBCHK1 )             ");

            parameter.Add("HEMSNO", argMirNo);
            parameter.Add("YEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", argLtdCode);
            parameter.Add("FRDATE", argMinDate);
            parameter.Add("TODATE", argMaxDate);
            parameter.Add("BUILDCNT", argCnt1);
            parameter.Add("BUILDSABUN", argSabun);
            parameter.Add("SEQNO", argCnt2);
            parameter.Add("GBOHMS", argOHMS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCHUPYN", argBook, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JOBSEQ", argCnt3);
            parameter.Add("GBCHK1", argGubun1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetMirHemsSeqNobyBuildDate(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(SEQNo) AS SEQNO                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_HEMS                        ");
            parameter.AppendSql(" WHERE BUILDDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND BUILDDATE  < TO_DATE(:TODATE, 'YYYY-MM-DD')     ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteScalar<COMHPC>(parameter);
        }

        public COMHPC GetSunapAmtbyJepDateMirNo(string strGubun, string strFDate, string strTDate, long nMirNo, string strLife)
        {
            MParameter parameter = CreateParameter();

            if (strGubun != "4")
            {
                parameter.AppendSql("SELECT SUM(JOHAPAMT) AMT ,c.Cnt , d.DenCnt                                                 ");
            }
            else if (strGubun == "4" || strGubun == "5")
            {
                parameter.AppendSql("SELECT SUM(BOGENAMT) AMT ,c.Cnt , d.DenCnt                                                 ");
            }
            else
            {
                parameter.AppendSql("SELECT SUM(LTDAMT) AMT ,c.Cnt , d.DenCnt                                                   ");
            }
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAP                                                                   ");
            parameter.AppendSql("     , (SELECT COUNT(GJJONG) CNT FROM KOSMOS_PMPA.HIC_JEPSU                                    ");
            if (strGubun == "1")
            {
                parameter.AppendSql("          WHERE MIRNO1 = :MIRNO                                                            ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("          WHERE MIRNO2 = :MIRNO                                                            ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("          WHERE MIRNO3 = :MIRNO                                                            ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("          WHERE MIRNO4 = :MIRNO                                                            ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("          WHERE MIRNO5 = :MIRNO                                                            ");
            }
            parameter.AppendSql("            AND JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql("            AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                      ");
            if (strLife == "")
            {
                parameter.AppendSql("            AND DELDATE IS NULL AND GJJONG IN ('11','12','13','14' ) ) c ,                 ");
            }
            else
            {
                parameter.AppendSql("            AND DELDATE IS NULL AND GJJONG IN ('41','42','43' ) ) c ,                      ");
            }

            parameter.AppendSql("        (SELECT COUNT(EXCODE) DENCNT FROM KOSMOS_PMPA.HIC_RESULT                               ");
            parameter.AppendSql("          WHERE WRTNO IN ( SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU                             ");
            if (strGubun == "1")
            {
                parameter.AppendSql("                             WHERE MIRNO1 = :MIRNO                                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("                             WHERE MIRNO2 = :MIRNO                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("                             WHERE MIRNO3 = :MIRNO                                         ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("                             WHERE MIRNO4 = :MIRNO                                         ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("                             WHERE MIRNO5 = :MIRNO                                         ");
            }
            parameter.AppendSql("                               AND JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("                               AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                   ");
            if (strLife == "")
            {
                parameter.AppendSql("                               AND DELDATE IS NULL   ) AND EXCODE IN ('ZD00','ZD01') ) d   ");
            }
            else
            {
                parameter.AppendSql("                               AND DELDATE IS NULL   ) AND EXCODE IN ('ZD00','ZD01') ) d   ");
            }
            parameter.AppendSql(" WHERE WRTNO IN (                                                                              ");
            if (strGubun == "1")
            {
                parameter.AppendSql("                   SELECT WRTNO FROM HIC_JEPSU WHERE MIRNO1 = :MIRNO                       ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("                   SELECT WRTNO FROM HIC_JEPSU WHERE MIRNO2 = :MIRNO                       ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("                   SELECT WRTNO FROM HIC_JEPSU WHERE MIRNO3 = :MIRNO                       ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("                   SELECT WRTNO FROM HIC_JEPSU WHERE MIRNO4 = :MIRNO                       ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("                   SELECT WRTNO FROM HIC_JEPSU WHERE MIRNO5 = :MIRNO                       ");
            }
            parameter.AppendSql("                      AND DELDATE IS NULL                                                      ");
            parameter.AppendSql("                      AND JepDate >=TO_DATE(:FRDATE,'YYYY-MM-DD')                              ");
            parameter.AppendSql("                      AND JepDate <=TO_DATE(:TODATE,'YYYY-MM-DD')                              ");
            parameter.AppendSql("                  )                                                                            ");
            parameter.AppendSql(" GROUP BY c.Cnt, d.DenCnt                                                                      ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);
            parameter.Add("MIRNO", nMirNo);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public void SelectInsertTableHicJepsu(string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_JEPSU                  ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HIC_JEPSU@PSMH           ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("JEPDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public string GetIEMunjinDateByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MUNDATE                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE PTNO=:PTNO                      ");
            parameter.AppendSql(" ORDER BY MUNDATE DESC                 ");

            parameter.Add("PTNO", argPtno);

            return ExecuteScalar<string>(parameter);
        }

        public void SelectInsertTableHea1(string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_JEPSU              ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HEA_JEPSU@PSMH       ");
            parameter.AppendSql(" WHERE SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')  ");

            parameter.Add("SDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public void SelectInsertTableHea6(string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESV_EXAM                   ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HEA_RESV_EXAM@PSMH            ");
            parameter.AppendSql(" WHERE SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')          ");

            parameter.Add("SDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public void SelectInsertTableHea5(string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESV_SET                   ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HEA_RESV_SET@PSMH            ");
            parameter.AppendSql(" WHERE SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')          ");

            parameter.Add("SDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public void SelectInsertTableHea4(string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SUNAPDTL                     ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HEA_SUNAPDTL@PSMH              ");
            parameter.AppendSql(" WHERE WRTNO IN (                                      ");
            parameter.AppendSql("       SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU@PSMH    ");
            parameter.AppendSql("        WHERE SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" )                                                     ");

            parameter.Add("SDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetMirItembyYearMirGbn(string strYear, string strMirGbn, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MirNo,Johap,GbJohap,BuildCNT,GbErrChk,Kiho,BuildSabun,Tamt                      ");
            parameter.AppendSql("     , JepNo,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,MirGbn,Chasu,Life_Gbn               ");
            parameter.AppendSql("     , TO_CHAR(FrDate,'YYYY-MM-DD') FrDate                                             ");
            parameter.AppendSql("     , TO_CHAR(ToDate,'YYYY-MM-DD') ToDate                                             ");
            parameter.AppendSql("     , (SELECT TAMT BOTAMT                                                             ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HIC_MIR_CANCER_BO                                           ");
            parameter.AppendSql("         WHERE MIRNO = a.MIRNO) BOTAMT                                                 ");
            switch (strJong)
            {
                case "1":
                    parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_BOHUM a                                            "); //건강보험 1,2차
                    break;
                case "3":
                    parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_DENTAL a                                           "); //구강검사
                    break;
                case "4":
                case "E":
                    parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_CANCER a                                           "); //암검진,의료급여암
                    break;
                default:
                    break;
            }
            parameter.AppendSql(" WHERE YEAR = :YEAR                                                                    ");
            parameter.AppendSql("   AND MIRNO <> 0                                                                      ");
            if (strJong == "4")
            {
                parameter.AppendSql("   AND Johap NOT IN ('X')                                                          ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql("   AND Johap = 'X'                                                                 ");
            }
            parameter.AppendSql("   AND JEPDATE IS NULL                                                                 ");
            parameter.AppendSql("   AND MIRGBN = :MIRGBN                                                                "); //추가청구
            parameter.AppendSql(" ORDER BY MirNo                                                                        ");

            parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MIRGBN", strMirGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetJepNobyYear(string strGubun, string strYear, string strJepNo)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT MAX(JEPNO) JEPNO                                        ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_BOHUM                               ");
                parameter.AppendSql(" WHERE YEAR = :YEAR                                            ");
                parameter.AppendSql("   AND JEPNO LIKE :JEPNO                                       ");
                parameter.AppendSql("   AND MIRNO > 0                                               ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT MAX(JEPNO) JEPNO                                        ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_DENTAL                              ");
                parameter.AppendSql(" WHERE YEAR = :YEAR                                            ");
                parameter.AppendSql("   AND JEPNO LIKE :JEPNO                                       ");
                parameter.AppendSql("   AND MIRNO > 0                                               ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("SELECT MAX(JEPNO) JEPNO                                        ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_CANCER                              ");
                parameter.AppendSql(" WHERE YEAR = :YEAR                                            ");
                parameter.AppendSql("   AND JEPNO LIKE :JEPNO                                       ");
                parameter.AppendSql("   AND MIRNO > 0                                               ");
            }

            parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPNO", strJepNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetListWombAnatMstByPtno(string argPtno, string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.ROWID AS RID, A.GBIO             ");
            parameter.AppendSql("     , B.ORDERNAME, A.DEPTCODE, A.ANATNO, A.CREMARK1 AS REMARK                 ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST A                                               ");
            parameter.AppendSql("     , KOSMOS_OCS.OCS_ORDERCODE B                                              ");
            parameter.AppendSql("     , KOSMOS_OCS.EXAM_SPECMST C                                               ");
            parameter.AppendSql(" WHERE a.PTNO = :PTNO                                                          ");
            parameter.AppendSql("   AND A.BDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                ");
            parameter.AppendSql("   AND A.BDATE <  TO_DATE(:TDATE, 'YYYY-MM-DD')                                ");
            parameter.AppendSql("   AND A.SPECNO = C.SPECNO                                                     ");
            parameter.AppendSql("   AND A.DEPTCODE = 'HR'                                                       ");
            parameter.AppendSql("   AND A.ORDERCODE = B.ORDERCODE                                               ");
            parameter.AppendSql("   AND (SUBSTR(A.ANATNO,1,1) IN ('C','P') OR SUBSTR(A.ANATNO,1,2) IN ('AC'))   ");
            parameter.AppendSql("   AND ORDERNAME LIKE 'Pap%'                                                   ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public void SelectInsertTableHea3(string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SUNAP                     ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HEA_SUNAP@PSMH              ");
            parameter.AppendSql(" WHERE WRTNO IN (                                      ");
            parameter.AppendSql("       SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU@PSMH    ");
            parameter.AppendSql("        WHERE SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" )                                                     ");

            parameter.Add("SDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public void SelectInsertTableHea2(string argJobDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESULT                     ");
            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HEA_RESULT@PSMH              ");
            parameter.AppendSql(" WHERE WRTNO IN (                                      ");
            parameter.AppendSql("       SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU@PSMH    ");
            parameter.AppendSql("        WHERE SDATE >= TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" )                                                     ");

            parameter.Add("SDATE", argJobDate);

            ExecuteNonQuery(parameter);
        }

        public int DeleteHicMirErrorbyMirNo(long nMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" DELETE KOSMOS_PMPA.HIC_MIR_ERROR                      ");
            parameter.AppendSql("  WHERE MIRNO = :MIRNO                                 ");

            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public void SelectInsertTable(string argTable, string strDate = "", string strColumn = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO " + argTable + "           ");
            parameter.AppendSql("SELECT * FROM " + argTable + "@PSMH    ");
            if (!strDate.IsNullOrEmpty() && !strColumn.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE " + strColumn + ">=TO_DATE(:SDATE, 'YYYY-MM-DD')   ");
            }

            if (!strDate.IsNullOrEmpty() && !strColumn.IsNullOrEmpty())
            {
                parameter.Add("SDATE", strDate);
            }

            ExecuteNonQuery(parameter);
        }

        public void TruncateTable(string argTable)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("TRUNCATE TABLE " + argTable + "        ");

            ExecuteNonQuery(parameter);
        }

        public List<COMHPC> chkEtcXrayOrderByWrtno(long argWrtno, string argBuse)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.ROWID AS RID        ");
            if (argBuse == "TO")
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT a                    ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                    ");
            }

            parameter.AppendSql("    , KOSMOS_PMPA.HIC_EXCODE b                     ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                            ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                           ");
            parameter.AppendSql("   AND b.XJONG = '1'                               ");  //'방사선 단순촬영
            parameter.AppendSql("   AND b.DELDATE IS NULL                           ");

            if (argBuse == "TO")
            {
                parameter.AppendSql("  AND (b.SENDBUSE1 IS NULL OR b.SENDBUSE1=' ')   ");
            }
            else
            {
                parameter.AppendSql("  AND (b.SENDBUSE2 IS NULL OR b.SENDBUSE2=' ')   ");
            }

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetCancerFlagbyPatIdSerialNo(string strPtNo, string strXrayno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CANCELFLAG,ROWID                ");
            parameter.AppendSql("  FROM PACS.DORDER                     ");
            parameter.AppendSql(" WHERE PATID     = :PATID              ");
            parameter.AppendSql("   AND ORDERFROM = 'HR'                ");
            parameter.AppendSql("   AND SERIALNO  = :SERIALNO           ");

            parameter.Add("PATID", strPtNo);
            parameter.Add("SERIALNO", strXrayno);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int GetPacsOrderCountbyPatIdAcdessionNoExamDate(string strPtNo, string strXrayno, string strExamDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM PACS.XRAY_PACS_ORDER                ");
            parameter.AppendSql(" WHERE PATID = :PATID                      ");
            parameter.AppendSql("   AND ACDESSIONNO = :ACDESSIONNO          ");
            parameter.AppendSql("   AND EXAMDATE = :EXAMDATE                ");

            parameter.Add("PATID", strPtNo);
            parameter.Add("ACDESSIONNO", strXrayno);
            parameter.Add("EXAMDATE", strExamDate);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateMirBuildCntbyMirNo(long nMirno, long nCnt, long nCnt1, string strGubun)
        {

            MParameter parameter = CreateParameter();
            if (strGubun == "1")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_BOHUM SET                       ");
                parameter.AppendSql("       BUILDCNT = :BUILDCNT                                ");
                parameter.AppendSql("     , ONE_QTY  = :ONE_QTY                                 ");
                parameter.AppendSql("     , TWO_QTY  = :TWO_QTY                                 ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_DENTAL SET                      ");
                parameter.AppendSql("       BUILDCNT = :BUILDCNT                                ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_CANCER SET                      ");
                parameter.AppendSql("       BUILDCNT = :BUILDCNT                                ");
            }
            else
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_CANCER SET                      ");
                parameter.AppendSql("       BUILDCNT = :BUILDCNT                                ");
            }

            parameter.AppendSql("     , JEPQTY   = :JEPQTY                                      ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                          ");

            if (strGubun == "1")
            {
                parameter.Add("ONE_QTY", nCnt1);
                parameter.Add("TWO_QTY", nCnt1);
            }
            parameter.Add("MIRNO", nMirno);
            parameter.Add("BUILDCNT", nCnt);
            parameter.Add("JEPQTY", nCnt);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetPanjengDatebyWrtNo(string strJong, string strChasu, long wRTNO)
        {
            MParameter parameter = CreateParameter();
            if (strJong == "1") //건강검진
            {
                parameter.AppendSql("SELECT SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE    ");
                if (strChasu == "1")
                {
                    parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                          ");
                }
                else
                {
                    parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM2                          ");
                }

            }
            else if (strJong == "3")    //구강검사
            {
                parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE           ");
                parameter.AppendSql("     , T_PANJENG1,RES_RESULT,RES_JOCHI                         ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL                              ");
            }
            else                        //암검진
            {
                parameter.AppendSql("SELECT TO_CHAR(GUNDATE,'YYYY-MM-DD') PANJENGDATE               ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW                              ");
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int InsertHicMirError(long argMirNo, long argWrtNo, string argGubun, string fstrSname, string fstrSex, long fnAge, string strRemark)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_MIR_ERROR                                      ");
            parameter.AppendSql("       (MIRNO, WRTNO, GUBUN, SNAME, SEX, AGE, REMARK)          ");
            parameter.AppendSql("VALUES                                                         ");
            parameter.AppendSql("       (:MIRNO, :WRTNO, :GUBUN, :SNAME, :SEX, :AGE, :REMARK)   ");

            parameter.Add("MIRNO", argMirNo);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("GUBUN", argGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", fstrSname);
            parameter.Add("SEX", fstrSex, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", fnAge);
            parameter.Add("REMARK", strRemark);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetItembyYearLtdCodeSabunMirNoJepDate(string strJong, string strYear, long nLtdCode, string strSabun, long nMirNo, string strJob, string strFDate, string strTDate, string strJepDate, string strMirGbn)
        {
            MParameter parameter = CreateParameter();
            if (strJong == "1" || strJong == "3")
            {
                parameter.AppendSql("SELECT MirNo,Johap,GbJohap,BuildCNT,GbErrChk,Kiho,JepQty,BuildSabun,Tamt                       ");
                parameter.AppendSql("     , JepNo,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate, FileName,MirGbn,Chasu,Life_Gbn,nHicNo      ");
                parameter.AppendSql("     , TO_CHAR(FrDate,'YYYY-MM-DD') FrDate                                                     ");
                parameter.AppendSql("     , TO_CHAR(ToDate,'YYYY-MM-DD') ToDate                                                     ");
                switch (strJong)
                {
                    case "1":
                        parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_BOHUM                                                      ");        //건강보험 1,2차
                        break;
                    case "3":
                        parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_MIR_DENTAL                                                     ");       //구강검사
                        break;
                    default:
                        break;
                }
                parameter.AppendSql(" WHERE YEAR = :YEAR                                                                            ");
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("    AND LTDCODE = :LTDCODE                                                                 ");
                }
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND BUILDSABUN = :BUILDSABUN                                                            ");
                }
                if (!nMirNo.IsNullOrEmpty() && nMirNo != 0)
                {
                    parameter.AppendSql("   AND MIRNO = :MIRNO                                                                      ");
                }
                parameter.AppendSql("   AND MIRGBN = :MIRGBN                                                                        ");   //추가청구
                parameter.AppendSql("   AND Mirno <> 0                                                                              ");
                if (strJob == "0")
                {
                    parameter.AppendSql("   AND JepDate IS NULL                                                                     ");
                    parameter.AppendSql("   AND GbErrChk ='Y'                                                                       ");
                    parameter.AppendSql(" ORDER BY MirNo                                                                            ");
                }
                else
                {
                    parameter.AppendSql("   AND JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                           ");
                    parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                           ");
                    parameter.AppendSql("   AND JepDate IS NOT NULL                                                                 ");
                    parameter.AppendSql("   AND GbErrChk = 'Y'                                                                      ");
                    parameter.AppendSql(" ORDER BY JepDate DESC,MirNo                                                               ");
                }
            }
            else
            {
                parameter.AppendSql("SELECT a.MirNo,a.Johap,a.GbJohap,a.BuildCNT,a.GbErrChk,a.Kiho,a.JepQty,a.BuildSabun,a.Tamt         ");
                parameter.AppendSql("     , a.JepNo,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate, a.FileName,MirGbn,Chasu,Life_Gbn,a.nHicNo  ");
                parameter.AppendSql("     , TO_CHAR(a.FrDate,'YYYY-MM-DD') FrDate                                                       ");
                parameter.AppendSql("     , TO_CHAR(a.ToDate,'YYYY-MM-DD') ToDate                                                       ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_CANCER a, KOSMOS_PMPA.HIC_JEPSU b                                       "); //암검진
                parameter.AppendSql(" WHERE a.YEAR = :YEAR                                                                              ");
                if (strJong == "4")
                {
                    parameter.AppendSql("   AND a.Mirno = b.Mirno3                                                                      ");
                    parameter.AppendSql("   AND (b.MileageAm <> 'Y' OR b.MileageAm IS NULL)                                             "); //공단암
                }
                else if (strJong == "E")  //의료급여암
                {
                    parameter.AppendSql("   AND a.Mirno = b.Mirno5                                                                      ");
                }
                parameter.AppendSql("   AND Mirno <> 0                                                                                  ");
                parameter.AppendSql("   AND MIRGBN = :MIRGBN                                                                            ");  //추가청구
                if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
                {
                    parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                                    ");
                }
                if (!strSabun.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND a.BUILDSABUN = :BUILDSABUN                                                              ");
                }
                if (!nMirNo.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND MIRNO = :MIRNO                                                                          ");
                }
                if (strJob == "0")
                {
                    parameter.AppendSql("   AND a.JepDate IS NULL                                                                       ");
                }
                else
                {
                    parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                              ");
                    parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')                                              ");
                    parameter.AppendSql("   AND a.JEPDATE IS NOT NULL                                                                   ");
                }
                parameter.AppendSql("   AND b.JEPDATE >= TO_DATE(:JEPDATE, 'YYYY-MM-DD')                                                ");
                parameter.AppendSql("   AND a.GbErrChk = 'Y'                                                                            ");
                parameter.AppendSql(" GROUP BY a.MirNo,a.Johap,a.GbJohap,a.BuildCNT,a.GbErrChk,a.Kiho,a.JepQty,a.BuildSabun,a.Tamt      ");
                parameter.AppendSql("     , a.JepNo,a.JepDate, a.FileName,MirGbn,Chasu,Life_Gbn, a.FrDate , a.ToDate,a.nHicNo           ");
            }

            parameter.Add("MIRGBN", strMirGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strJong == "1" || strJong == "3")
            {
                parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            else
            {
                parameter.Add("JEPDATE", strJepDate);
            }
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (!strSabun.IsNullOrEmpty())
            {
                parameter.Add("BUILDSABUN", strSabun);
            }
            if (!nMirNo.IsNullOrEmpty() && nMirNo != 0)
            {
                parameter.Add("MIRNO", nMirNo);
            }
            if (strJob != "0")
            {
                parameter.Add("FRDATE", strFDate);
                parameter.Add("TODATE", strTDate);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetOcsIllsbyPtNoDeptCodeIllCode(string strPtNo, string strDeptCode, string strIllCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_OILLS            ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE            ");
            parameter.AppendSql("   AND BDATE    = TRUNC(SYSDATE)       ");
            parameter.AppendSql("   AND ILLCODE  = :ILLCODE             ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ILLCODE", strIllCode);

            return ExecuteScalar<int>(parameter);
        }

        public int UpDatePacsDOrderCancelFlagbyRowId(string strCancelFlag, string rOWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE PACS.DORDER SET             ");
            parameter.AppendSql("       CANCELFLAG = :CANCELFLAG    ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("CANCELFLAG", strCancelFlag, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteOcsOrderTrans(string strPtNo, string idNumber)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_OORDER                                           ");
            parameter.AppendSql(" WHERE DEPTCODE IN ('HR','TO')                                         ");
            parameter.AppendSql("   AND BDATE = TRUNC(SYSDATE)                                          ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                    ");
            parameter.AppendSql("   AND SABUN = :SABUN                                                  ");
            parameter.AppendSql("   AND ORDERCODE NOT IN ('A-PO12GA','A-BASCAM','N-PTD25', 'A-ANE12G')  ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SABUN", idNumber);

            return ExecuteNonQuery(parameter);
        }

        public string Read_Ocs_Doctor(long SABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", SABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public void DeletePdfSendByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_PDF_SEND      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public COMHPC GetPanjengInfoByTableWrtno(string strTable, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE, PANJENGDRNO  ");
            if (strTable != "HIC_SPC_PANJENG")
            {
                parameter.AppendSql("     , TO_CHAR(TONGBODATE,'YYYY-MM-DD') TONGBODATE, TONGBOGBN      ");
            }
            parameter.AppendSql("  FROM KOSMOS_PMPA." + strTable + "");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int DeleteDojangbySabun(string fstrSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_DOJANG      ");
            parameter.AppendSql(" WHERE SABUN = :SABUN              ");

            parameter.Add("SABUN", fstrSabun);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetBaseCode(string strBusiness)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CodeName_Ex, Remark1, Min(Code) Code        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_BASECODE                    ");
            parameter.AppendSql(" WHERE BUSINESS = :BUSINESS                        ");
            parameter.AppendSql("   AND GbUse = '0'                                 ");
            parameter.AppendSql(" GROUP BY CodeName_Ex, Remark1                     ");
            parameter.AppendSql(" ORDER BY Remark1                                  ");

            parameter.Add("BUSINESS", strBusiness, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetOcsOorderbyPtnoOrderCode(string argPtNo, string argOrderCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_OORDER                       ");
            parameter.AppendSql(" WHERE PTNO      = :PTNO                           ");
            parameter.AppendSql("   AND ORDERCODE = :ORDERCODE                      ");
            parameter.AppendSql("   AND BDATE     = TRUNC(SYSDATE)                  ");

            parameter.Add("PTNO", argPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORDERCODE", argOrderCode);

            return ExecuteScalar<int>(parameter);
        }

        public List<COMHPC> GetItemHicDojangbySabun(string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN, LICENSE, IMAGE       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOJANG      ");
            if (strSabun != "")
            {
                parameter.AppendSql(" WHERE SABUN = :SABUN          ");
            }

            if (strSabun != "")
            {
                parameter.Add("SABUN", strSabun.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public int InsertHicDojang(string strSabun, long nLicense)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_DOJANG    ");
            parameter.AppendSql("        (SABUN, LICENSE)               ");
            parameter.AppendSql(" VALUES                                ");
            parameter.AppendSql("        (:SABUN, :LICENSE)             ");

            parameter.Add("SABUN", strSabun);
            parameter.Add("LICENSE", nLicense);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetSuNapCodebyPtNo(string pTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT B.CODE FROM KOSMOS_PMPA.HEA_JEPSU A, KOSMOS_PMPA.HEA_SUNAPDTL B     ");
            parameter.AppendSql(" WHERE A.PTNO  = :PTNO                                                     ");
            parameter.AppendSql("   AND A.WRTNO = B.WRTNO                                                   ");
            parameter.AppendSql("   AND A.SDATE = TRUNC(SYSDATE)                                            ");
            parameter.AppendSql("   AND B.CODE IN ('Z1130', 'Z1131','Z1132')                                ");

            parameter.Add("PTNO", pTNO.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int InsertHicIEMunjinSendReq(string argPtno, string argForm, long argMunID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_IE_MUNJIN_SENDREQ     ");
            parameter.AppendSql("        (PTNO, RECVFORM, IDNO)                     ");
            parameter.AppendSql(" VALUES                                            ");
            parameter.AppendSql("        (:PTNO, :RECVFORM, :IDNO)                  ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("RECVFORM", argForm);
            parameter.Add("IDNO", argMunID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateHicDojangImage(string fstrSabun, byte[] bImage)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_DOJANG SET      ");
            parameter.AppendSql("       IMAGE = :IMAGE                  ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                  ");

            parameter.Add("SABUN", fstrSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("IMAGE", bImage, Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw);

            return ExecuteNonQuery(parameter);
        }

        public string GetHeaSRevRemarkBySDate(string argCurDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT REMARK                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SREV                    ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");

            parameter.Add("SDATE", argCurDate);

            return ExecuteScalar<string>(parameter);
        }

        public List<COMHPC> GetListJepListByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT X.WRTNO, X.GJYEAR, X.SDATE, X.GJJONG, X.JONGNAME                    ");
            parameter.AppendSql("      ,DECODE(X.GBN, 1, '일반', 2, '종검', '') AS GBN                      ");
            parameter.AppendSql("  FROM (                                                                   ");
            parameter.AppendSql("        SELECT WRTNO, GJYEAR                                               ");
            parameter.AppendSql("              ,TO_CHAR(JEPDATE, 'YYYY-MM-DD') AS SDATE                     ");
            parameter.AppendSql("              ,GJJONG, KOSMOS_PMPA.FC_HIC_GJJONG_NAME(GJJONG, UCODES) AS JONGNAME  ");
            parameter.AppendSql("              ,1 AS GBN                                                    ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HIC_JEPSU                                       ");
            parameter.AppendSql("         WHERE 1 = 1                                                       ");
            parameter.AppendSql("           AND PTNO = :PTNO                                                ");
            parameter.AppendSql("           AND DELDATE IS NULL                                             ");
            parameter.AppendSql("         UNION ALL                                                         ");
            parameter.AppendSql("        SELECT WRTNO, TO_CHAR(SDATE, 'YYYY') AS GJYEAR                     ");
            parameter.AppendSql("              ,TO_CHAR(SDATE, 'YYYY-MM-DD') AS SDATE                       ");
            parameter.AppendSql("              ,GJJONG, KOSMOS_PMPA.FC_HEA_GJJONG_NAME(GJJONG) AS JONGNAME  ");
            parameter.AppendSql("              ,2 AS GBN                                                    ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HEA_JEPSU                                       ");
            parameter.AppendSql("         WHERE 1 = 1                                                       ");
            parameter.AppendSql("           AND PTNO = :PTNO                                                ");
            parameter.AppendSql("           AND DELDATE IS NULL                                             ");
            parameter.AppendSql("       ) X                                                                 ");
            parameter.AppendSql(" ORDER By X.SDATE DESC, X.GBN                                              ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetInsaMstBuseBySabun(string argSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT BUSE                                ");
            parameter.AppendSql("  FROM KOSMOS_ADM.INSA_MST                 ");
            parameter.AppendSql(" WHERE SABUN =:SABUN                       ");

            parameter.Add("SABUN", argSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int DeleteHicSpecMstWorkbyRowId(string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HIC_SPECMST_WORK           ");
            parameter.AppendSql(" WHERE ROWID = :RID                                ");

            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetPrtDatabyHicSpecMstWork()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PRTDATA, ROWID                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPECMST_WORK    ");

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetImagebySabun(string fstrSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT IMAGE                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOJANG      ");
            parameter.AppendSql(" WHERE SABUN = :SABUN              ");

            parameter.Add("SABUN", fstrSabun);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public string GetLastDay4ByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MIN(TO_CHAR(SDATE,'YYYY-MM-DD')) JDATE  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                         ");
            parameter.AppendSql("   AND PTNO = :PTNO                            ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<COMHPC> GetSabunNameHicDojangbyLicense()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN, ROWID                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOJANG      ");
            parameter.AppendSql(" WHERE License > 0                 ");
            parameter.AppendSql("   AND License < 20                ");
            parameter.AppendSql(" ORDER BY SABUN                    ");

            return ExecuteReader<COMHPC>(parameter);
        }

        public int UpdateMirJepDatebyMirNo(long nMirNo, string strJong)
        {
            MParameter parameter = CreateParameter();

            if (strJong == "1")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_BOHUM SET       ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_DENTAL SET      ");
            }
            else if (strJong == "4")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_CANCER SET      ");
            }
            parameter.AppendSql("       JEPDATE = ''                            ");
            parameter.AppendSql(" WHERE MIRNO   = :MIRNO                        ");

            parameter.Add("MIRNO", nMirNo);

            return ExecuteNonQuery(parameter);
        }

        public string SelHicPrivacy_Accept(string argYear, string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ENTDATE                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PRIVACY_ACCEPT      ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND PTNO = :PTNO                        ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR                    ");

            #region Query 변수대입
            parameter.Add("PTNO", argPtno);
            parameter.Add("GJYEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            #endregion

            return ExecuteScalar<string>(parameter);
        }

        public int InsertHicSpecmstWork(string prdata)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SPECMST_WORK   ");
            parameter.AppendSql("       (JOBDATE, PRTDATA, GBPRT, GBPRINT)  ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("      (SYSDATE, :PRTDATA, 'N', '')         ");

            parameter.Add("PRTDATA", prdata);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetSabunbyNurse(string strBuse, string strJikjong, string strSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SABUN, KORNAME                                              ");
            parameter.AppendSql("  FROM KOSMOS_ADM.INSA_MST                                         ");
            parameter.AppendSql(" WHERE BUSE = :BUSE                                                ");
            parameter.AppendSql("   AND JIKJONG = :JIKJONG                                          ");
            parameter.AppendSql("   AND (ToiDay IS NULL OR ToiDay >= TO_DATE(:SDATE,'YYYY-MM-DD'))  ");
            parameter.AppendSql(" ORDER BY KORNAME, SABUN                                           ");

            parameter.Add("BUSE", strBuse, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SDATE", strSDate);
            parameter.Add("JIKJONG", strJikjong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<COMHPC>(parameter);
        }

        public long GetNewPanobySeq()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQ_PANO.NEXTVAL NEXTVAL FROM DUAL    ");

            return ExecuteScalar<long>(parameter);
        }

        public void UpDatePacsDOrderCancelFlag(string argPano, string argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE PACS.DORDER                 ");
            parameter.AppendSql("   SET CANCELFLAG = '0'            ");
            parameter.AppendSql(" WHERE PATID = :PATID              ");
            parameter.AppendSql("   AND ORDERFROM = 'HR'            ");
            parameter.AppendSql("   AND SERIALNO = :SERIALNO        ");
            parameter.AppendSql("   AND CANCELFLAG = '1'            ");

            parameter.Add("PATID", argPano);
            parameter.Add("SERIALNO", argXrayNo);

            ExecuteNonQuery(parameter);
        }

        public int UpdateNHicNobyMirNo(string strnHicNo, long nMirno, string strJong)
        {
            MParameter parameter = CreateParameter();

            if (strJong == "1")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_BOHUM SET       ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_DENTAL SET      ");
            }
            else
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_MIR_CANCER SET      ");
            }
            parameter.AppendSql("       NHICNO = :NHICNO                        ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                          ");

            parameter.Add("NHICNO", strnHicNo);
            parameter.Add("MIRNO", nMirno);

            return ExecuteNonQuery(parameter);
        }

        public string GetJumin3byMyen_Bunho(string argSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUMIN3 FROMKOSMOS_ADM.INSA_MST      ");
            parameter.AppendSql(" WHERE MYEN_BUNHO = :MYEN_BUNHO            ");

            parameter.Add("MYEN_BUNHO", argSabun);

            return ExecuteScalar<string>(parameter);
        }

        public string GetPacsDOrderRowidByPanoXrayNo(string argPano, string argXrayNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM PACS.DORDER              ");
            parameter.AppendSql(" WHERE PATID = :PATID                      ");
            parameter.AppendSql("   AND ORDERFROM = 'HR'                    ");
            parameter.AppendSql("   AND SERIALNO = :SERIALNO                ");

            parameter.Add("PATID", argPano);
            parameter.Add("SERIALNO", argXrayNo);

            return ExecuteScalar<string>(parameter);
        }

        public long HicNew_XrayNo_Create()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HIC_XRAYNO.NEXTVAL FROM DUAL    ");

            return ExecuteScalar<long>(parameter);
        }

        public long HicNew_XrayNo_Create_Chul()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HIC_XRAYNO_CHUL.NEXTVAL FROM DUAL    ");

            return ExecuteScalar<long>(parameter);
        }

        public COMHPC GetIpdMasterBCodebyPaNo(string strDrno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT REPLACE(b.NAME, '격리', '^') NAME                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.IPD_NEW_MASTER a                            ");
            parameter.AppendSql("     , KOSMOS_PMPA.BAS_BCODE      b                            ");
            parameter.AppendSql(" WHERE OUTDATE IS NULL                                         ");
            parameter.AppendSql("   AND BEDNUM = CODE(+)                                        ");
            parameter.AppendSql("   AND a.PANO = :PANO                                          ");
            parameter.AppendSql("   AND a.OUTDATE IS NULL  AND GUBUN(+) = 'NUR_ICU_침상번호'    ");

            parameter.Add("PANO", strDrno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetExamPatientbyPaNo(string strDrno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SODIUM , HEPARIN                        ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_PATIENT                 ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");
            parameter.AppendSql("   AND (SODIUM ='*'  OR HEPARIN ='*')          ");

            parameter.Add("PANO", strDrno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetOpdMasterbyPaNoDeptCodeBDate(string strPano, string strDep, string strBDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ERPATIENT as ERPAT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.OPD_MASTER                          ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                            ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')           ");

            parameter.Add("PANO", strPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", strDep, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDate);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetInfectMasterbyExNamePaNo(string strExName, string strDrno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                           ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_INFECT_MASTER   ");
            parameter.AppendSql(" WHERE (EXNAME LIKE :EXNAME            ");
            parameter.AppendSql("   AND PANO = :PANO                    ");

            parameter.AddLikeStatement("EXNAME", strExName);
            parameter.Add("PANO", strDrno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<COMHPC>(parameter);
        }

        public string GetRowIdOcsHyangbyPtNoSuCodeBDate(string argPtNo, string argDrug, object argBDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.OCS_HYANG                     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                                    ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                             ");

            parameter.Add("PTNO", argPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", argDrug, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argBDate);

            return ExecuteScalar<string>(parameter);
        }
        public string GetRowIdOcsMayakbyPtNoSuCodeBDate(string argPtNo, string argDrug, object argBDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.OCS_HYANG                     ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                        ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                                    ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                             ");

            parameter.Add("PTNO", argPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", argDrug, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argBDate);

            return ExecuteScalar<string>(parameter);
        }

        public int InsertOcsHyang(string strPtNo, string strSName, string strBI, string strWardCode, string strBDate, string strDeptCode,
                                  string strDRSabun, string strIO, string strSuCode, string strQty, double nRealQty, int nNal, string strDosCode,
                                  string strRemark1, string strRemark2, string strSex, long nAge, string strJumin, string strJumin3, string strJuso,
                                  double nEntQty)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_HYANG                                       ");
            parameter.AppendSql("       (PTNO,SNAME,BI,WARDCODE,BDATE,ENTDATE                           ");
            parameter.AppendSql("     , DEPTCODE, DRSABUN, IO, SUCODE, QTY                              ");
            parameter.AppendSql("     , REALQTY, NAL, DOSCODE, REMARK1, REMARK2                         ");
            parameter.AppendSql("     , SEX, AGE, JUMIN, JUMIN3, JUSO, ENTQTY, MDATE)                   ");
            parameter.AppendSql("VALUES                                                                 ");
            parameter.AppendSql("       (:PTNO, :SNAME,:BI,:WARDCODE,:BDATE,SYSDATE                     ");
            parameter.AppendSql("     , :DEPTCODE, :DRSABUN, :IO, :SUCODE, :QTY                         ");
            parameter.AppendSql("     , :REALQTY, :NAL, :DOSCODE, :REMARK1, :REMARK2                    ");
            parameter.AppendSql("     , :SEX, :AGE, :JUMIN, :JUMIN3, :JUSO, :ENTQTY, TRUNC(SYSDATE))    ");

            parameter.Add("PTNO", strPtNo);
            parameter.Add("SNAME", strSName);
            parameter.Add("BI", strBI);
            parameter.Add("WARDCODE", strWardCode);
            parameter.Add("BDATE", strBDate);
            parameter.Add("DEPTCODE", strDeptCode);
            parameter.Add("DRSABUN", strDRSabun);
            parameter.Add("IO", strIO);
            parameter.Add("SUCODE", strSuCode);
            parameter.Add("QTY", strQty);
            parameter.Add("REALQTY", nRealQty);
            parameter.Add("NAL", nNal);
            parameter.Add("DOSCODE", strDosCode);
            parameter.Add("REMARK1", strRemark1);
            parameter.Add("REMARK2", strRemark2);
            parameter.Add("SEX", strSex);
            parameter.Add("AGE", nAge);
            parameter.Add("JUMIN", strJumin);
            parameter.Add("JUMIN3", strJumin3);
            parameter.Add("JUSO", strJuso);
            parameter.Add("ENTQTY", nEntQty);

            return ExecuteNonQuery(parameter);
        }
        public int InsertOcsMayak(string strPtNo, string strSName, string strBI, string strWardCode, string strDeptCode,
                                  string strDRSabun, string strIO, string strSuCode, string strQty, double nRealQty, int nNal, string strDosCode,
                                  string strRemark1, string strRemark2, string strSex, long nAge, string strJumin, string strJumin3, string strJuso,
                                  double nEntQty, string strBdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_MAYAK                                                       ");
            parameter.AppendSql("       (PTNO,SNAME,BI,WARDCODE,ENTDATE                                                 ");
            parameter.AppendSql("     , DEPTCODE, DRSABUN, IO, SUCODE, QTY                                              ");
            parameter.AppendSql("     , REALQTY, NAL, DOSCODE, REMARK1, REMARK2                                         ");
            parameter.AppendSql("     , SEX, AGE, JUMIN3, JUMIN, JUSO,BDATE_R,ENTQTY, BDATE)                            ");
            parameter.AppendSql("VALUES                                                                                 ");
            parameter.AppendSql("       (:PTNO, :SNAME,:BI,:WARDCODE,SYSDATE                                            ");
            parameter.AppendSql("     , :DEPTCODE, :DRSABUN, :IO, :SUCODE, :QTY                                         ");
            parameter.AppendSql("     , :REALQTY, :NAL, :DOSCODE, :REMARK1, :REMARK2                                    ");
            parameter.AppendSql("     , :SEX, :AGE, :JUMIN3, :JUMIN, :JUSO, TRUNC(SYSDATE) ,:ENTQTY, TO_DATE(:BDATE,'YYYY-MM-DD'))      ");

            parameter.Add("PTNO", strPtNo);
            parameter.Add("SNAME", strSName);
            parameter.Add("BI", strBI);
            parameter.Add("WARDCODE", strWardCode);
            parameter.Add("DEPTCODE", strDeptCode);
            parameter.Add("DRSABUN", strDRSabun);
            parameter.Add("IO", strIO);
            parameter.Add("SUCODE", strSuCode);
            parameter.Add("QTY", strQty);
            parameter.Add("REALQTY", nRealQty);
            parameter.Add("NAL", nNal);
            parameter.Add("DOSCODE", strDosCode);
            parameter.Add("REMARK1", strRemark1);
            parameter.Add("REMARK2", strRemark2);
            parameter.Add("SEX", strSex);
            parameter.Add("AGE", nAge);
            parameter.Add("JUMIN3", strJumin3);
            parameter.Add("JUMIN", strJumin);
            parameter.Add("JUSO", strJuso);
            parameter.Add("ENTQTY", nEntQty);
            parameter.Add("BDATE", strBdate);

            return ExecuteNonQuery(parameter);
        }

        public int InsertOcsHyangSelect(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_HYANG (                                                         ");
            parameter.AppendSql("       PTNO,SNAME,BI,WARDCODE,ROOMCODE,BDATE,ENTDATE,DEPTCODE,DRSABUN,IO,SUCODE            ");
            parameter.AppendSql("     , QTY,REALQTY,NAL,DOSCODE,REMARK1,REMARK2,SEQNO,PRINT,ORDERNO,BANBUN,BANREMARK        ");
            parameter.AppendSql("     , NO1,JEGO,GUBUN,GBSORT,DRCODE,IWOLQTY,ILLCODE,NRSABUN,POWDER,SEX,AGE,JUMIN,JUSO      ");
            parameter.AppendSql("     , CERTNO , ENTQTY, ORDERSITE, JDATE_ENDO, ENDO_QTY, MDate, JUMIN3 , CHASU )           ");
            parameter.AppendSql(" SELECT PTNO,SNAME,BI,WARDCODE,ROOMCODE,BDATE,ENTDATE,DEPTCODE,DRSABUN,IO,SUCODE           ");
            parameter.AppendSql("     , QTY,REALQTY,NAL*-1,DOSCODE,REMARK1,REMARK2,SEQNO,PRINT,ORDERNO,BANBUN,BANREMARK     ");
            parameter.AppendSql("     , NO1,JEGO,GUBUN,GBSORT,DRCODE,IWOLQTY,ILLCODE,NRSABUN,POWDER,SEX,AGE,JUMIN,JUSO      ");
            parameter.AppendSql("     , CERTNO , ENTQTY, ORDERSITE, JDATE_ENDO, ENDO_QTY, MDate, JUMIN3, CHASU              ");
            parameter.AppendSql("  From KOSMOS_OCS.OCS_HYANG                                                                ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                        ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }
        public int InsertOcsMayakSelect(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_MAAYK (                                                         ");
            parameter.AppendSql(" PTNO,SNAME,BI,WARDCODE,ROOMCODE,BDATE,ENTDATE,DEPTCODE,DRSABUN,IO,SUCODE,                 ");
            parameter.AppendSql(" QTY,REALQTY,NAL,DOSCODE,REMARK1,REMARK2,SEQNO,PRINT,ORDERNO,BANBUN,BANREMARK,             ");
            parameter.AppendSql(" NO1,JEGO,GUBUN,GBSORT,DRCODE,IWOLQTY,ILLCODE,NRSABUN,POWDER,SEX,AGE,JUMIN,JUSO,           ");
            parameter.AppendSql(" CERTNO , BDATE_R, ENTQTY, ORDERSITE, JDATE_ENDO, ENTDATE2, MDate, JUMIN3)                 ");
            parameter.AppendSql(" SELECT PTNO,SNAME,BI,WARDCODE,ROOMCODE,BDATE,ENTDATE,DEPTCODE,DRSABUN,IO,SUCODE,          ");
            parameter.AppendSql(" QTY,REALQTY,NAL*-1,DOSCODE,REMARK1,REMARK2,SEQNO,PRINT,ORDERNO,BANBUN,BANREMARK,          ");
            parameter.AppendSql(" NO1,JEGO,GUBUN,GBSORT,DRCODE,IWOLQTY,ILLCODE,NRSABUN,POWDER,SEX,AGE,JUMIN,JUSO,           ");
            parameter.AppendSql(" CERTNO , BDATE_R, ENTQTY, ORDERSITE, JDATE_ENDO, ENTDATE2, MDate, JUMIN3                  ");
            parameter.AppendSql("  From KOSMOS_OCS.OCS_MAAYK                                                                ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                        ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateOcsHyang(string strDeptCode, string strDRSABUN, double nQty, double nSQty, double nRealQty, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.OCS_HYANG SET            ");
            parameter.AppendSql("       DEPTCODE = :DEPTCODE                ");
            parameter.AppendSql("     , DRSABUN  = :DRSABUN                 ");
            parameter.AppendSql("     , QTY      = :QTY                     ");
            parameter.AppendSql("     , ENTQTY   = :ENTQTY                  ");
            parameter.AppendSql("     , REALQTY  = :REALQTY                 ");
            parameter.AppendSql("     , ENTDATE  = SYSDATE                  ");
            parameter.AppendSql("     , MDATE    = TRUNC(SYSDATE)           ");
            parameter.AppendSql(" WHERE ROWID    = :RID                     ");

            parameter.Add("DEPTCODE", strDeptCode);
            parameter.Add("DRSABUN", strDRSABUN);
            parameter.Add("QTY", nQty);
            parameter.Add("ENTQTY", nSQty);
            parameter.Add("REALQTY", nRealQty);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }
        public int UpdateOcsMayak(string strDeptCode, string strDRSABUN, double nQty, double nSQty, double nRealQty, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.OCS_HYANG SET            ");
            parameter.AppendSql("       DEPTCODE = :DEPTCODE                ");
            parameter.AppendSql("     , DRSABUN  = :DRSABUN                 ");
            parameter.AppendSql("     , QTY      = :QTY                     ");
            parameter.AppendSql("     , ENTQTY   = :ENTQTY                  ");
            parameter.AppendSql("     , REALQTY  = :REALQTY                 ");
            parameter.AppendSql("     , ENTDATE  = SYSDATE                  ");
            parameter.AppendSql(" WHERE ROWID    = :RID                     ");

            parameter.Add("DEPTCODE", strDeptCode);
            parameter.Add("DRSABUN", strDRSABUN);
            parameter.Add("QTY", nQty);
            parameter.Add("ENTQTY", nSQty);
            parameter.Add("REALQTY", nRealQty);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetHicMirErrorCountbyMirNo(long fnMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM HIC_MIR_ERROR WHERE MIRNO = :MIRNO   ");

            parameter.Add("MIRNO", fnMirNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public long GetHicMIrNobySeq()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HIC_MIRNO.NEXTVAL HICMIRNO FROM DUAL");

            return ExecuteScalar<long>(parameter);
        }

        public string GetPatientAreabyPaNo(string strDrno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JINAME                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT A, KOSMOS_PMPA.BAS_AREA B   ");
            parameter.AppendSql(" WHERE A.PANO = :PANO                                      ");
            parameter.AppendSql("   AND A.JICODE = B.JICODE                                 ");
            parameter.AppendSql("   AND (A.JICODE = '63' OR A.JICODE >='76')                ");

            parameter.Add("PANO", strDrno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetABObyPaNo(string strDrno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ABO                             ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_BLOOD_MASTER    ");
            parameter.AppendSql(" WHERE PANO = :PANO                    ");

            parameter.Add("PANO", strDrno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<COMHPC> GetExamMasterResultCbySpecNo(string strSpecNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT M.VolumeCode, M.BCODEPRINT, R.Subcode   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_MASTER  M               ");
            parameter.AppendSql("     , KOSMOS_OCS.EXAM_RESULTC R               ");
            parameter.AppendSql(" WHERE R.SPECNO = :SPECNO                      ");
            parameter.AppendSql("   AND R.SUBCODE = M.MASTERCODE(+)             ");

            parameter.Add("SPECNO", strSpecNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetJepsuWrokPatientCountbyJumin2GjYearGjJong(string argJumin, string argGjYear, string argGjJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK  a           ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT     b           ");
            parameter.AppendSql(" WHERE a.PANO   = b.PANO                       ");
            parameter.AppendSql("   AND b.JUMIN2 = :JUMIN2                      ");
            parameter.AppendSql("   AND a.GJYEAR = :GJYEAR                      ");
            parameter.AppendSql("   AND a.GJJONG = :GJJONG                      ");

            parameter.Add("JUMIN2", argJumin);
            parameter.Add("GJYEAR", argGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJJONG", argGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<COMHPC> GetCountHicMirErrorbyMirNo(long fnMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MIRNO, WRTNO, SNAME, SEX, AGE, GUBUN, REMARK    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_ERROR                       ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                  ");

            parameter.Add("PTNO", fnMirNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetMunDatebyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MUNDATE                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                    ");
            parameter.AppendSql(" ORDER BY MUNDATE DESC                 ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetPtNobyViewId(long fnMunID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_IE_MUNJIN_VIEW              ");
            parameter.AppendSql(" WHERE VIEWID = :VIEWID                            ");

            parameter.Add("VIEWID", fnMunID);

            return ExecuteScalar<string>(parameter);
        }

        public int DeleteHicIEMunjinNewbyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.HIC_IE_MUNJIN_NEW         ");
            parameter.AppendSql(" WHERE ROWID = :RID                                ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertHicIEMunjinDel(string strROWID, string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_IE_MUNJIN_DEL                                                              ");
            parameter.AppendSql("       (DELDATE,DELSABUN,WRTNO,MUNDATE,SNAME,BIRTH,HPHONE,PTNO,RECVFORM,GBMUN1             ");
            parameter.AppendSql("     , GBMUN2,GBMUN3,GBMUN4,GBMUN5,AGE,SEX,LTDNAME,TEL,GBSCHOOL,CLASS,BAN,BUN,MUNJINRES    ");
            parameter.AppendSql("     , INJEKDATA,GBDBSEND,WRTNO1,WRTNO2,WRTNO3,WRTNO4,WRTNO5,GBWEBHTML)                    ");
            parameter.AppendSql("SELECT SYSDATE, :SABUN, WRTNO,MUNDATE,SNAME,BIRTH,HPHONE,PTNO,RECVFORM,GBMUN1              ");
            parameter.AppendSql("     , GBMUN2,GBMUN3,GBMUN4,GBMUN5,AGE,SEX,LTDNAME,TEL,GBSCHOOL,CLASS,BAN,BUN,MUNJINRES    ");
            parameter.AppendSql("     , INJEKDATA,GBDBSEND,WRTNO1,WRTNO2,WRTNO3,WRTNO4,WRTNO5,GBWEBHTML                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_IE_MUNJIN_NEW                                                       ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                        ");

            parameter.Add("SABUN", strSabun);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetExamResultCMasterbySpecNo(string strSpecCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT M.GBTLASORT, M.MasterCode,M.BCodeName,COUNT(R.MasterCode) CNT   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.Exam_ResultC R, KOSMOS_OCS.Exam_Master M             ");
            parameter.AppendSql(" WHERE R.SPECNO = :SPECNO                                              ");
            parameter.AppendSql("   AND R.MasterCode = R.SubCode                                        ");
            parameter.AppendSql("   AND R.MasterCode = M.MasterCode(+)                                  ");
            parameter.AppendSql("   AND M.BCodePrint > 0                                                ");
            parameter.AppendSql(" GROUP BY M.GBTLASORT, M.MasterCode,M.BCodeName                        ");
            parameter.AppendSql(" ORDER BY M.GBTLASORT DESC, M.MasterCode,M.BCodeName                   ");

            parameter.Add("SPECNO", strSpecCode);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetExamResultCMasterNotBarCodebySpecNo(string strSpecNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT M.MASTERCODE, M.BCODENAME                                       ");
            parameter.AppendSql("  FROM KOSMOS_OCS.Exam_ResultC R, KOSMOS_OCS.Exam_Master M             ");
            parameter.AppendSql(" WHERE R.SPECNO = :SPECNO                                              ");
            parameter.AppendSql("   AND R.MasterCode = R.SubCode                                        ");
            parameter.AppendSql("   AND R.MasterCode = M.MasterCode(+)                                  ");
            parameter.AppendSql("   AND M.BCodePrint = 0                                                ");
            parameter.AppendSql(" ORDER BY M.GBTLASORT, M.MasterCode,M.BCodeName                        ");

            parameter.Add("SPECNO", strSpecNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetHicSjMstLtdbyGjYear(string strGjYear, string strFrDate, string strToDate, string strViewLtd)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JEPDATE, a.LTDCODE, b.NAME  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SJ_MST a, KOSMOS_PMPA.HIC_LTD b             ");
            parameter.AppendSql(" WHERE a.GJYEAR = :GJYEAR                                          ");
            parameter.AppendSql("   AND a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.LtdCode = b.Code(+)                                       ");
            if (!strViewLtd.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.NAME LIKE :NAME                                       ");
            }
            parameter.AppendSql(" ORDER BY b.Name                                                   ");

            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (!strViewLtd.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strViewLtd);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetRowidBySRev(string argCurDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SREV                    ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");

            parameter.Add("SDATE", argCurDate);

            return ExecuteScalar<string>(parameter);
        }

        public int InsertSRev(string argCurDate, string argRemark)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SREV ( SDATE, REMARK, CONV )   ");
            parameter.AppendSql("       VALUES (:SDATE, :REMARK, 'Y' )                      ");

            parameter.Add("SDATE", argCurDate);
            parameter.Add("REMARK", argRemark);

            return ExecuteNonQuery(parameter);
        }

        public int UpDateSRev(string argRowid, string argRemark)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_SREV        ");
            parameter.AppendSql("   SET REMARK = :REMARK            ");
            parameter.AppendSql("      ,CONV = 'Y'                  ");
            parameter.AppendSql(" WHERE ROWID = :RID                ");

            parameter.Add("REMARK", argRemark);
            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetItembyOpdMaster(string pTNO, string jEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT DEPTCODE, PANO PTNO, SNAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.OPD_MASTER                      ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND ACTDATE = TO_DATE(:ACTDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DeptCode NOT IN ('HR')                      ");

            parameter.Add("PANO", pTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ACTDATE", jEPDATE);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetEndoJupMstbyPtnoRDate(string strPtNo, string strFrDate, string strToDate, string strGbJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST                      ");
            parameter.AppendSql(" WHERE PTNO = :PANO                                ");
            parameter.AppendSql("   AND RDATE >=TO_DATE(:FRDATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql("   AND RDATE < TO_DATE(:TODATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                              ");     //위
            parameter.AppendSql("   AND GBSUNAP IN ('1','7')                        ");     //접수만-취소제외
            parameter.AppendSql("   AND ResultDate IS NOT NULL                      ");

            parameter.Add("PANO", strPtNo);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("GBJOB", strGbJob, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetExam_SpecMstCountbyPtNo(string strPtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_SPECMST                     ");
            parameter.AppendSql(" WHERE PANO     = :PANO                            ");
            parameter.AppendSql("   AND BDATE   >= TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                             ");
            parameter.AppendSql("   AND STATUS   = '05'                             "); //완료체크
            parameter.AppendSql("   AND SPECCODE = '084'                            ");
            parameter.AppendSql("   AND TUBE     = '041'                            ");

            parameter.Add("PANO", strPtNo);
            parameter.Add("BDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public COMHPC GetCancerNewbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBSTOMACH,GBLIVER,GBRECTUM,GBBREAST,GbWomb, GBLUNG                          ");
            parameter.AppendSql("     , NEW_WOMAN32,NEW_WOMAN33,NEW_WOMAN34,NEW_WOMAN35,NEW_WOMAN36, NEW_WOMAN37    ");
            parameter.AppendSql("     , TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate,PrtSabun                        ");
            parameter.AppendSql("     , TO_CHAR(S_PANJENGDATE,'YYYY-MM-DD') S_PANJENGDATE                           ");
            parameter.AppendSql("     , TO_CHAR(C_PANJENGDATE,'YYYY-MM-DD') C_PANJENGDATE                           ");
            parameter.AppendSql("     , TO_CHAR(L_PANJENGDATE,'YYYY-MM-DD') L_PANJENGDATE                           ");
            parameter.AppendSql("     , TO_CHAR(B_PANJENGDATE,'YYYY-MM-DD') B_PANJENGDATE                           ");
            parameter.AppendSql("     , TO_CHAR(W_PANJENGDATE,'YYYY-MM-DD') W_PANJENGDATE                           ");
            parameter.AppendSql("     , TO_CHAR(L_PANJENGDATE1,'YYYY-MM-DD') L_PANJENGDATE1                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CANCER_NEW                                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public string GetRowIdbyPaNoGjYearGjJong(string strPANO, string strYear, string strGjjong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU_WORK  ");
            parameter.AppendSql(" WHERE PANO   = :PANO              ");
            parameter.AppendSql("   AND GJYEAR = :GJYEAR            ");
            parameter.AppendSql("   AND GJJONG = :GJJONG            ");

            parameter.Add("PANO", strPANO);
            parameter.Add("GJYEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJJONG", strGjjong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public COMHPC GetJinGbnbyWrtNo(long nWRTNO, string strTable)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(b.ENTDATE,'YYYY-MM-DD') ENTDATE,b.ENTSABUN, B.GBPRINT   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a,KOSMOS_PMPA." + strTable + " b          ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int InsertSangdamUseLog(string idNumber, string sProgID, string sIPAddress, string strPtno, long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_PROGRAM_USE_CNT                        ");
            parameter.AppendSql("       (USERID, PTNO, PROGRAMID, INPDATE, IPADDRESS, WRTNO)        ");
            parameter.AppendSql(" VALUES                                                            ");
            parameter.AppendSql("       (:USERID, :PTNO, :PROGRAMID, SYSDATE, :IPADDRESS, :WRTNO)   ");

            parameter.Add("USERID", idNumber);
            parameter.Add("PROGRAMID", sProgID);
            parameter.Add("IPADDRESS", sIPAddress);
            parameter.Add("PTNO", strPtno);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetResDentalbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PanjengDrno, TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate,'0' PrtSabun     ");
            parameter.AppendSql("     , TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_DENTAL                                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int DeleteOcsOorders(string strPtNo, List<string> lstSuCode, string strBdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_OORDER                       ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DeptCode = 'HR'                             ");
            parameter.AppendSql("   AND SUCODE   IN (:SUCODE)                       ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBdate);
            parameter.AddInStatement("SUCODE", lstSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetSchoolNewbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PPanDrno AS PanjengDrno, TO_CHAR(RDate,'YYYY-MM-DD') PanjengDate,PrtSabun   ");
            parameter.AppendSql("     , TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate, DPanDrno                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SCHOOL_NEW                                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<COMHPC> GetHicIEMunjinItembyEntDateLtdName(string strFrDate, string strToDate, string strLtdName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUMIN2,SNAME, TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE, GJJONG, SEX, AGE, LTDNAME, MAILCODE        ");
            parameter.AppendSql("     , TEL,HPHONE,EMAIL,JUSO1,JUSO2,CLASS,BAN,BUN                                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_IE_MUNJIN                                                                       ");
            parameter.AppendSql(" WHERE ENTDATE > =TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                       ");
            parameter.AppendSql("   AND ENTDATE < =TO_DATE(:TODATE, 'YYYY-MM-DD')                                                       ");
            if (!strLtdName.IsNullOrEmpty() && strLtdName != "전체")
            {
                parameter.AppendSql("   AND LTDNAME LIKE ':LTDNAME                                                                      ");
            }
            parameter.AppendSql(" ORDER BY Sname,Jumin2,entdate                                                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("LTDNAME", strLtdName);

            return ExecuteReader<COMHPC>(parameter);
        }

        public void DelHeaGamCodeHicByPanoSDate(long fnPano, string strSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_GAMCODE_HIC                     ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')           ");

            parameter.Add("PANO", fnPano);
            parameter.Add("SDATE", strSDate);

            ExecuteNonQuery(parameter);
        }

        public void InsertWebPrintReqHea(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_WEBPRINTREQ_HIS (WRTNO,JEPDATE,PTNO,SNAME,GJJONG,WEBPRINTREQ,ENTDATE,ENTSABUN) ");
            parameter.AppendSql(" SELECT WRTNO,SDATE,PTNO,SNAME,'83',WEBPRINTREQ,SYSDATE,JOBSABUN                           ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HEA_JEPSU                                                                  ");
            parameter.AppendSql("  WHERE WRTNO= :WRTNO                                                                          ");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public string ChkEndoExcuteByPtnoBDate(string argPtno, string argSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.ENDO_JUPMST           ");
            parameter.AppendSql(" WHERE PTNO =:PTNO                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND PACSUID IS NOT NULL                         ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argSDate);

            return ExecuteScalar<string>(parameter);
        }

        public void DelOpdMasterByPanoBDateDept(string argPtno, string argBDate, string argDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM KOSMOS_PMPA.OPD_MASTER          ");
            parameter.AppendSql(" WHERE PANO= :PTNO                          ");
            parameter.AppendSql("   AND BDATE= TO_DATE(:BDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DEPTCODE= :DEPTCODE                  ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("BDATE", argBDate);
            parameter.Add("DEPTCODE", argDept);

            ExecuteNonQuery(parameter);
        }

        public string ChkXrayExcuteByPtnoBDate(string argPtno, string argSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_PMPA.XRAY_DETAIL          ");
            parameter.AppendSql(" WHERE PANO =:PANO                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND PACS_END IS NOT NULL                        ");
            parameter.AppendSql("   AND PACSSTUDYID IS NOT NULL                     ");

            parameter.Add("PANO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argSDate);

            return ExecuteScalar<string>(parameter);
        }

        public string GetInsaMstHTelBySabun(string argSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HTEL FROM KOSMOS_ADM.INSA_MST               ");
            parameter.AppendSql(" WHERE SABUN =:SABUN                               ");
            parameter.AppendSql("   AND (TOIDAY IS NULL OR TOIDAY >= TRUNC(SYSDATE))");
            parameter.AppendSql("   AND HTEL IS NOT NULL                            ");

            parameter.Add("SABUN", argSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertHeaGamCodeHic(clsHcType.HaMain_HEA_GAMCODE_INFO haGam)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_GAMCODE_HIC (                                          ");
            parameter.AppendSql("       SNAME,PANO,SDATE,ACTDATE,GCODE,GCODENAME,BURATE,AMT,GAMAMT,ENTSABUN,ENTDATE ");
            parameter.AppendSql(" ) VALUES (                                                                        ");
            parameter.AppendSql("      :SNAME,:PANO,TO_DATE(:SDATE, 'YYYY-MM-DD'), TO_DATE(:ACTDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("     ,:GCODE,:GCODENAME,:BURATE,:AMT,:GAMAMT,:ENTSABUN,SYSDATE)                    ");

            parameter.Add("SNAME", haGam.sName);
            parameter.Add("PANO", haGam.Pano);
            parameter.Add("SDATE", haGam.sDate);
            parameter.Add("ACTDATE", haGam.ActDate);
            parameter.Add("GCODE", haGam.GCode);
            parameter.Add("GCODENAME", haGam.GCodeName);
            parameter.Add("BURATE", haGam.BuRate);
            parameter.Add("AMT", haGam.AMT);
            parameter.Add("GAMAMT", haGam.GamAmt);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }

        public string GetHeaGamcodeHicByPano(long fnPano, string argSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GAMCODE_HIC             ");
            parameter.AppendSql(" WHERE PANO   = :PANO                          ");
            parameter.AppendSql("   AND SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");

            parameter.Add("PANO", fnPano);
            parameter.Add("SDATE", argSDate);

            return ExecuteScalar<string>(parameter);
        }

        public COMHPC GetGViewLtdByLtdCode(string argLtdCode, string argYear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, REG_DATE, DATA1, ROWID, YEAR    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_READING                     ");
            parameter.AppendSql(" WHERE CODE = :CODE                                ");
            parameter.AppendSql("   AND DEL_DATE IS NULL                            ");
            parameter.AppendSql("   AND YEAR = :YEAR                                ");
            parameter.AppendSql(" ORDER BY YEAR DESC                                ");

            parameter.Add("CODE", argLtdCode);
            parameter.Add("YEAR", argYear);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetTicketInfoByTicketNo(long nTicket)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT BSAWON,BNAME,REMARK     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_TICKET  ");
            parameter.AppendSql(" WHERE NO = :NO                ");

            parameter.Add("NO", nTicket);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int InsertHicSpecmstWork(string prdata, string strGbPrt)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SPECMST_WORK                   ");
            parameter.AppendSql("       (JOBDATE, PRTDATA, GBPRT)                           ");
            parameter.AppendSql("VALUES                                                     ");
            parameter.AppendSql("       (SYSDATE, :PRTDATA, :GBPRT)                         ");

            parameter.Add("PRTDATA", prdata);
            parameter.Add("GBPRT", strGbPrt, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountOcsOillsbyPtnoBDate(string strPtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_OILLS                            ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                    ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                                 ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE,'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND ILLCODE = 'Z018'                                ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public int InsertOcsOills(string strPtNo, string strJepDate, string strDeptCode, int nSeqNo, string strillCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_OILLS                                   ");
            parameter.AppendSql("       (PTNO, BDATE, DEPTCODE, SEQNO, ILLCODE, ENTDATE)            ");
            parameter.AppendSql("VALUES                                                             ");
            parameter.AppendSql("       (:PTNO, TO_DATE(:BDATE, 'YYYY-MM-DD'), :DEPTCODE, :SEQNO    ");
            parameter.AppendSql("     , :ILLCODE, SYSDATE)                                          ");

            parameter.Add("PTNO", strPtNo);
            parameter.Add("BDATE", strJepDate);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SEQNO", nSeqNo);
            parameter.Add("ILLCODE", strillCode);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetDeptNamebyDeptCode(string strDeptCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TOGO || ' ' || DeptNameK DEPTNAMEK, DeptNameK DeptNameS         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_CLINICDEPT A, KOSMOS_PMPA.BAS_CLINICDEPT_TOGO B ");
            parameter.AppendSql(" WHERE A.DEPTCODE(+) = B.DEPTCODE                                      ");
            parameter.AppendSql("   AND A.DEPTCODE = :DEPTCODE                                          ");

            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public void InsertRowByWrtnoTable(long argWrtno, string argTable)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO " + argTable + " ( WRTNO ) VALUES ( :WRTNO ) ");

            parameter.Add("WRTNO", argWrtno);

            ExecuteNonQuery(parameter);
        }

        public HIC_SUNAP chkSunapAudio(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, SUM(SUNAPAMT) SUNAPAMT FROM KOSMOS_PMPA.HIC_SUNAP                ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                              ");
            parameter.AppendSql("       SELECT a.WRTNO FROM KOSMOS_PMPA.HIC_JEPSU a                             ");
            parameter.AppendSql("                          ,KOSMOS_PMPA.HIC_RESULT b                            ");
            parameter.AppendSql("        WHERE a.WRTNO = :WRTNO                                                 ");
            parameter.AppendSql("          AND a.GJCHASU <> '2'                                                 ");
            parameter.AppendSql("          AND a.WRTNO = b.WRTNO                                                ");
            parameter.AppendSql("          AND SUBSTR(b.EXCODE, 1, 3) IN ('TH5','TH6','TH7','TH8')  ");
            parameter.AppendSql("       )                                                                       ");
            parameter.AppendSql(" GROUP BY WRTNO                                                                ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);
        }
        public HIC_SUNAP chkSunapAudio1(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, SUM(SUNAPAMT) SUNAPAMT FROM KOSMOS_PMPA.HIC_SUNAP                ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                              ");
            parameter.AppendSql("       SELECT a.WRTNO FROM KOSMOS_PMPA.HIC_JEPSU a                             ");
            parameter.AppendSql("                          ,KOSMOS_PMPA.HIC_RESULT b                            ");
            parameter.AppendSql("        WHERE a.WRTNO = :WRTNO                                                 ");
            parameter.AppendSql("          AND a.GJCHASU <> '2'                                                 ");
            parameter.AppendSql("          AND a.WRTNO = b.WRTNO                                                ");
            parameter.AppendSql("          AND SUBSTR(b.EXCODE, 1, 3) IN ('TH1','TH2')  ");
            parameter.AppendSql("       )                                                                       ");
            parameter.AppendSql(" GROUP BY WRTNO                                                                ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_SUNAP>(parameter);
        }

        public string GetHicPrivacyAccept(string argPtno, string argFormCode, string argDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.ROWID AS RID                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT a               ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_CONSENT b               ");
            parameter.AppendSql(" WHERE a.PTNO = b.PTNO                         ");
            parameter.AppendSql("   AND a.PTNO = :PTNO                          ");
            //parameter.AppendSql("   AND a.GBPRIVACY_NEW IS NULL                 ");
            parameter.AppendSql("   AND b.DEPTCODE =:DEPTCODE                   ");
            parameter.AppendSql("   AND b.FORMCODE = :FORMCODE                  ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("DEPTCODE", argDept);
            parameter.Add("FORMCODE", argFormCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertNurFallScaleOpd(string argPtno, string argDept, long argJobSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.NUR_FALL_SCALE_OPD (              ");
            parameter.AppendSql("       PANO,ACTDATE,ENTSABUN,ENTDATE,ENTGUBUN              ");
            parameter.AppendSql(" ) VALUES (                                                ");
            parameter.AppendSql("      :PANO,TRUNC(SYSDATE),:ENTSABUN,SYSDATE,:ENTGUBUN )    ");

            parameter.Add("PANO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", argJobSabun);
            parameter.Add("ENTGUBUN", argDept);

            ExecuteNonQuery(parameter);
        }

        public int GetCountNurFallScaleOpd(string argPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(PANO) CNT FROM KOSMOS_PMPA.NUR_FALL_SCALE_OPD    ");
            parameter.AppendSql(" WHERE ACTDATE = TRUNC(SYSDATE)                            ");
            parameter.AppendSql("   AND PANO = :PANO                                        ");

            parameter.Add("PANO", argPano);


            return ExecuteScalar<int>(parameter);
        }

        public void InsertWebPrintReqHic(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_WEBPRINTREQ_HIS (WRTNO,JEPDATE,PTNO,SNAME,GJJONG,WEBPRINTREQ,ENTDATE,ENTSABUN) ");
            parameter.AppendSql(" SELECT WRTNO,JEPDATE,PTNO,SNAME,GJJONG,WEBPRINTREQ,SYSDATE,JOBSABUN                           ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_JEPSU                                                                  ");
            parameter.AppendSql("  WHERE WRTNO= :WRTNO                                                                          ");

            parameter.Add("WRTNO", argWrtno);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByTableWRTNO(long argWrtno, string argTable)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM " + argTable + "     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteScalar<string>(parameter);
        }

        public List<COMHPC> GetOpdReservedNewPatientbyDate3(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Pano,a.DeptCode,a.DrCode,TO_CHAR(a.DATE3,'YYYY-MM-DD HH24:MI') YDate  ");
            parameter.AppendSql("     , b.SName,b.HPhone, a.SMSBUILD,b.GB_VIP, b.GB_VIP_REMARK                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.OPD_RESERVED_NEW a, KOSMOS_PMPA.BAS_PATIENT b               ");
            parameter.AppendSql(" WHERE a.DATE3 >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND a.DATE3 <  TO_DATE(:TODATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND TRANSDATE IS NULL                                                       ");
            parameter.AppendSql("   AND RETDATE IS NULL                                                         ");
            parameter.AppendSql("   AND b.GB_VIP IS NOT NULL                                                    ");
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                        ");
            parameter.AppendSql(" ORDER BY a.Pano,a.DeptCode                                                    ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int UpdateOpdReservedNewbyPaNoDate3DeptCodeDrCode(string strPANO, string strTime, string strDeptCode, string strDRCODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE OPD_RESERVED_NEW SET                            ");
            parameter.AppendSql("       SMSBUILD = 'Y'                                  ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND DATE3 = TO_DATE(:DATE3, 'YYYY-MM-DD HH24:MI')   ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                            ");
            parameter.AppendSql("   AND DRCODE = :DRCODE                                ");

            parameter.Add("PANO", strPANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DATE3", strTime);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DRCODE", strDRCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int GetPoscoreservedbyPaNoExamres(string strPANO, string strFDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT_POSCO                           ");
            parameter.AppendSql(" WHERE PANO = :PANO                                            ");
            parameter.AppendSql("   AND (TRUNC(EXAMRES1) = TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("    OR TRUNC(EXAMRES2) = TO_DATE(:FRDATE, 'YYYY-MM-DD')        ");
            parameter.AppendSql("    OR TRUNC(EXAMRES3) = TO_DATE(:FRDATE, 'YYYY-MM-DD')        ");
            parameter.AppendSql("    OR TRUNC(EXAMRES4) = TO_DATE(:FRDATE, 'YYYY-MM-DD')        ");
            parameter.AppendSql("    OR TRUNC(EXAMRES6) = TO_DATE(:FRDATE, 'YYYY-MM-DD')        ");
            parameter.AppendSql("    OR TRUNC(EXAMRES7) = TO_DATE(:FRDATE, 'YYYY-MM-DD')        ");
            parameter.AppendSql("    OR TRUNC(EXAMRES8) = TO_DATE(:FRDATE, 'YYYY-MM-DD')        ");
            parameter.AppendSql("    OR TRUNC(EXAMRES9) = TO_DATE(:FRDATE, 'YYYY-MM-DD')        ");
            parameter.AppendSql("    OR TRUNC(EXAMRES10) = TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("    OR TRUNC(EXAMRES11) = TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("    OR TRUNC(EXAMRES12) = TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("    OR TRUNC(EXAMRES13) = TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("    OR TRUNC(EXAMRES14) = TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("    OR TRUNC(EXAMRES15) = TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("    OR TRUNC(EXAMRES16) = TO_DATE(:FRDATE, 'YYYY-MM-DD'))      ");

            parameter.Add("PANO", strPANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<COMHPC> GetIpdNewMasterbyPaNo(string strPANO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO, INDATE, OUTDATE                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.IPD_NEW_MASTER                  ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')  "); //입원중인 환자만 조회

            parameter.Add("PANO", strPANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetSmsbyDate(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Pano, a.DeptCode, a.DrCode, TO_CHAR(a.DATE3,'YYYY-MM-DD HH24:MI') YDate           ");
            parameter.AppendSql("     , b.SName, b.HPhone, a.SMSBUILD, c.DRNAME                                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.OPD_RESERVED_NEW a                                                      ");
            parameter.AppendSql("     , KOSMOS_PMPA.BAS_PATIENT      b                                                      ");
            parameter.AppendSql("     , KOSMOS_PMPA.BAS_DOCTOR       c                                                      ");
            parameter.AppendSql(" WHERE a.DATE3 >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                           ");
            parameter.AppendSql("   AND a.DATE3 <  TO_DATE(:TODATE, 'YYYY-MM-DD')                                           ");
            parameter.AppendSql("   AND TRANSDATE IS NULL                                                                   ");
            parameter.AppendSql("   AND RETDATE IS NULL                                                                     ");
            parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                    ");
            parameter.AppendSql("   AND a.DRCODE = c.DRCODE                                                                 ");
            parameter.AppendSql("   AND (b.GbSMS <> 'X' or b.GbSMS is null)                                                 ");  //동의안한분을 제외한 모두에게 발송
            parameter.AppendSql("   AND A.PANO NOT IN(                                                                      ");
            parameter.AppendSql("                     SELECT PANO                                                           ");
            parameter.AppendSql("                       FROM KOSMOS_PMPA.NUR_STD_DEATH                                      ");
            parameter.AppendSql("                      WHERE ACTDATE >= TRUNC(SYSDATE)-180                                  "); //6개월이내 병원에서 사망환자 제외
            parameter.AppendSql("                    )                                                                      ");
            //======================================================================
            //예약시 문자 날아가지 않도록 외래에서 체크
            parameter.AppendSql("   AND NOT EXISTS (                                                                        ");
            parameter.AppendSql("                    SELECT * FROM KOSMOS_PMPA.ETC_SMS_RESNOTSEND SUB                       ");
            parameter.AppendSql("                      WHERE A.PANO = SUB.PTNO                                              ");
            parameter.AppendSql("                        AND A.DATE3 = SUB.RDATE                                            ");
            parameter.AppendSql("                        AND A.DEPTCODE = SUB.DEPTCODE                                      ");
            parameter.AppendSql("                  )                                                                        ");
            //======================================================================                     
            parameter.AppendSql("   AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019')                       ");
            parameter.AppendSql(" ORDER BY a.Pano,a.DeptCode                                                                ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public void UpDateHicLtdPanoByItem(string argDate, string argYear, string argGjJong, long argLtdCode, long argPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_LTDPANO SET                ");
            parameter.AppendSql("        GJDATE = TO_DATE(:GJDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("  WHERE YEAR = :YEAR                               ");
            parameter.AppendSql("    AND JONG = :JONG                               ");
            parameter.AppendSql("    AND LTDCODE = :LTDCODE                         ");
            parameter.AppendSql("    AND PANO = :PANO                               ");

            parameter.Add("GJDATE", argDate);
            parameter.Add("YEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JONG", argGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", argLtdCode);
            parameter.Add("PANO", argPano);

            ExecuteNonQuery(parameter);
        }

        public int GetEtcJupMstbyPtnoBDate(string strPtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT IMAGE                                   ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DEPTCODE = 'TO'                         ");
            parameter.AppendSql("   AND GUBUN = '1'                             "); //EKG
            parameter.AppendSql("   AND GBJOB NOT IN ('9')                      ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public COMHPC GetTablebyWrtNo(string strTable, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PanjengDrno, TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate  ");
            parameter.AppendSql("     , TO_CHAR(TongboDate,'YYYY-MM-DD') TongboDate                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA." + strTable + "                                ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetTableJoinbyWrtNo(long nWRTNO, string strTable)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.PanjengDrno, TO_CHAR(a.PanjengDate,'YYYY-MM-DD') PanjengDate      ");
            parameter.AppendSql("     , TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate, b.PrtSabun           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA." + strTable + " b             ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public void InsertPrivacyAccept(string argYear, string argPtno, string argSName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_PRIVACY_ACCEPT             ");
            parameter.AppendSql("       (GJYEAR,PTNO,SNAME,ENTDATE)                     ");
            parameter.AppendSql("VALUES                                                 ");
            parameter.AppendSql("       (:GJYEAR, :PTNO, :SNAME, TRUNC(SYSDATE) )       ");

            parameter.Add("GJYEAR", argYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", argSName);

            ExecuteNonQuery(parameter);
        }

        public COMHPC GetSeq_Pano()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SEQ_PANO.NEXTVAL NEXTVAL FROM DUAL      ");

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public HIC_SPC_PANJENG GetItembyRowId(string fstrPROWID, string fstrSpcTable)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE   ");
            parameter.AppendSql("     , JOCHICODE,WORKYN,SAHUCODE,SAHUREMARK,MSYMCODE,MBUN,UCODE,SOGENREMARK,JOCHIREMARK    ");
            parameter.AppendSql("     , REEXAM,ORCODE,ORSAYUCODE                                                            ");
            if (fstrSpcTable == "H")
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANHIS                                                      ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                                     ");
            }
            parameter.AppendSql(" WHERE ROWID = :RID                                                                        ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE = '')                                                   ");

            parameter.Add("RID", fstrPROWID);

            return ExecuteReaderSingle<HIC_SPC_PANJENG>(parameter);
        }

        public int InsertHicJongChange(COMHPC item1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_JONG_CHANGE                    ");
            parameter.AppendSql("       (JOBTIME, JEPDATE, SNAME, OLD_WRTNO, OLD_GJJONG     ");
            parameter.AppendSql("     , NEW_WRTNO,NEW_GJJONG,JOBSABUN)                      ");
            parameter.AppendSql("VALUES                                                     ");
            parameter.AppendSql("       (SYSDATE, TO_DATE(:JEPDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("     , :SNAME, :OLD_WRTNO, :OLD_GJJONG                     ");
            parameter.AppendSql("     , :NEW_WRTNO, :NEW_GJJONG, :JOBSABUN)                 ");

            parameter.Add("JEPDATE", item1.JEPDATE);
            parameter.Add("SNAME", item1.SNAME);
            parameter.Add("OLD_WRTNO", item1.OLD_WRTNO);
            parameter.Add("OLD_GJJONG", item1.OLD_GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("NEW_WRTNO", item1.NEW_WRTNO);
            parameter.Add("NEW_GJJONG", item1.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JOBSABUN", item1.JOBSABUN);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetJongChangebyJobTime(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(JobTime,'YYYY-MM-DD HH24:MI') JobTime                           ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,SName,OLD_WRTNO,OLD_GJJONG        ");
            parameter.AppendSql("     , NEW_WRTNO,NEW_GJJONG,JobSabun                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JONG_CHANGE                                             ");
            parameter.AppendSql(" WHERE JobTime >= TO_DATE(:FRDATE,'YYYY-MM-DD')                                ");
            parameter.AppendSql("   AND JobTime <= TO_DATE(:TODATE,'YYYY-MM-DD HH24:MI')                        ");
            parameter.AppendSql(" ORDER BY JobTime DESC                                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int UpdateCARD_APPROV_CENTERHWrtNobyPanoHwrtNo(long nWrtNo, string fstrPtno, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.CARD_APPROV_CENTER SET  ");
            parameter.AppendSql("       HWRTNO = :HWRTNO                    ");
            parameter.AppendSql(" WHERE PANO   = :PANO                      ");
            parameter.AppendSql("   AND HWRTNO = :FWRTNO                    ");

            parameter.Add("HWRTNO", nWrtNo);
            parameter.Add("PANO", fstrPtno);
            parameter.Add("FWRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int GetXrayDetailbyPanoSeekDateXCode(string strPtNo, string strFrDate, string strToDate, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL                         ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SEEKDATE >=TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND SEEKDATE < TO_DATE(:TODATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND XCODE = :XCODE                                  ");
            parameter.AppendSql("   AND XSENDDATE IS NOT NULL                           ");

            parameter.Add("PANO", strPtNo);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.Add("XCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCoutnbyDojangSabun(string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOJANG                      ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                              ");

            parameter.Add("SABUN", strSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<COMHPC> GetItembyPtNo(string fstrPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,A.Image_Gbn     ");
            parameter.AppendSql("     , A.AGE, A.SEX, A.GBIO, A.DEPTCODE,A.GbFTP,B.ORDERNAME, A.ROWID, A.IMAGE , C.DRNAME       ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST A, KOSMOS_OCS.OCS_ORDERCODE B, KOSMOS_PMPA.BAS_DOCTOR C           ");
            parameter.AppendSql(" WHERE PTNO       = :PTNO                                                                      ");
            parameter.AppendSql("   AND A.DEPTCODE = 'TO'                                                                       ");
            parameter.AppendSql("   AND A.GUBUN    = '1'                                                                        "); //EKG
            parameter.AppendSql("   AND A.GBJOB NOT IN ('9')                                                                    ");
            parameter.AppendSql("   AND A.DRCODE   = C.DRCODE(+)                                                                ");
            //'1미접수 2.예약 3.접수 9.취소)
            parameter.AppendSql("   AND A.ORDERCODE= B.ORDERCODE(+)                                                             ");
            parameter.AppendSql("   AND a.BDATE <= TRUNC(SYSDATE)                                                               "); //index 태우기위해사용
            parameter.AppendSql(" ORDER BY a.RDate DESC                                                                         ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetEtcJupMstbyPtNoOrderCodeNotFilePath(string strPtNo, string strOrderCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST       ");
            parameter.AppendSql(" WHERE PTNO      = :PTNO           ");
            parameter.AppendSql("   AND BDate     = TRUNC(SYSDATE)  ");
            parameter.AppendSql("   AND ORDERCODE = :ORDERCODE      ");
            parameter.AppendSql("   AND DEPTCODE  = 'TO'            ");
            parameter.AppendSql("   AND FilePath IS NOT NULL        ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORDERCODE", strOrderCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetEtcJupMstbyPtNoOrderCode(string strPtNo, string strOrderCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("  AND BDate=TRUNC(SYSDATE)                         ");
            parameter.AppendSql("  AND ORDERCODE = :ORDERCODE                       ");
            parameter.AppendSql("  AND DEPTCODE = 'TO'                              ");
            parameter.AppendSql("  AND (CDate IS NOT NULL OR StartDate IS NOT NULL) ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORDERCODE", strOrderCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public IList<COMHPC> GetPanoSNameByJepsuResvExam(string argFDate, string argTDate)
        {

            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO, SNAME                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:RFTIME, 'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND GBSTS != 'D'                            ");
            parameter.AppendSql(" UNION                                         ");
            parameter.AppendSql("SELECT PANO, SNAME                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESV_EXAM               ");
            parameter.AppendSql(" WHERE RTIME >= TO_DATE(:RFTIME,'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND RTIME <  TO_DATE(:RTTIME,'YYYY-MM-DD')  ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");

            parameter.Add("RFTIME", argFDate);
            parameter.Add("RTTIME", argTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetXray_DetailbyPtNoXCode(string strPtNo, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL             ");
            parameter.AppendSql(" WHERE PANO = :PANO                        ");
            parameter.AppendSql("   AND TRUNC(SEEKDATE) = TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND DeptCode IN ('TO','HR')             ");
            parameter.AppendSql("   AND PacsNo IS NOT NULL                  ");
            parameter.AppendSql("   AND XSendDate IS NOT NULL               ");
            parameter.AppendSql("   AND XCODE IN (:XCODE)                   ");
            //parameter.AppendSql("   AND XCODE = :XCODE                      ");

            parameter.Add("PANO", strPtNo);
            parameter.AddInStatement("XCODE", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            //parameter.Add("XCODE", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateHicSpcPanjengPanHis(string FstrSpcTable, COMHPC item2)
        {
            MParameter parameter = CreateParameter();

            if (FstrSpcTable == "P")
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG SET     ");
            }
            else
            {
                parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANHIS SET      ");

            }
            parameter.AppendSql("       PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql("     , PANJENG     = :PANJENG                              ");
            parameter.AppendSql("     , SOGENCODE   = :SOGENCODE                            ");
            parameter.AppendSql("     , JOCHICODE   = :JOCHICODE                            ");
            parameter.AppendSql("     , WORKYN      = :WORKYN                               ");
            parameter.AppendSql("     , SAHUCODE    = :SAHUCODE                             ");
            parameter.AppendSql("     , SAHUREMARK  = :SAHUREMARK                           ");
            parameter.AppendSql("     , UCODE       = :UCODE                                ");
            parameter.AppendSql("     , SOGENREMARK = :SOGENREMARK                          ");
            parameter.AppendSql("     , JOCHIREMARK = :JOCHIREMARK                          ");
            parameter.AppendSql("     , REEXAM      = :REEXAM                               ");
            parameter.AppendSql("     , ORCODE      = :ORCODE                               ");
            parameter.AppendSql("     , ORSAYUCODE  = :ORSAYUCODE                           ");
            parameter.AppendSql("     , PYOJANGGI   = :PYOJANGGI                            ");
            parameter.AppendSql("     , GJCHASU     = :GJCHASU                              ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN                             ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE                               ");
            parameter.AppendSql(" WHERE ROWID       = :RID                                  ");

            //parameter.Add("WRTNO", item2.WRTNO);
            parameter.Add("PANJENGDATE", item2.PANJENGDATE);
            parameter.Add("PANJENGDRNO", item2.PANJENGDRNO);
            parameter.Add("PANJENG", item2.PANJENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SOGENCODE", item2.SOGENCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JOCHICODE", item2.JOCHICODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WORKYN", item2.WORKYN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SAHUCODE", item2.SAHUCODE);
            parameter.Add("SAHUREMARK", item2.SAHUREMARK);
            parameter.Add("UCODE", item2.UCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SOGENREMARK", item2.SOGENREMARK);
            parameter.Add("JOCHIREMARK", item2.JOCHIREMARK);
            parameter.Add("REEXAM", item2.REEXAM);
            parameter.Add("ORCODE", item2.ORCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORSAYUCODE", item2.ORSAYUCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PYOJANGGI", item2.PYOJANGGI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJCHASU", item2.GJCHASU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", item2.ENTSABUN);
            parameter.Add("RID", item2.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int GetEtc_JupMstbyPtNoOrderCode(string strPtNo, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST               ");
            parameter.AppendSql("WHERE TRUNC(BDATE) = TRUNC(SYSDATE)        ");
            parameter.AppendSql("   AND PTNO = :PTNO                        ");
            parameter.AppendSql("   AND DeptCode IN ('TO','HR')             ");
            parameter.AppendSql("   AND GbJob = '3'                         ");
            parameter.AppendSql("   AND ORDERCODE = :ORDERCODE              ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORDERCODE", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetEtc_JupMstbyPtNoOrderCodeStartDate(string strPtNo, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST               ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                        ");
            parameter.AppendSql("   AND DeptCode IN ('TO','HR')             ");
            parameter.AppendSql("   AND ORDERCODE IN (:ORDERCODE)           ");
            parameter.AppendSql("   AND StartDate IS NOT NULL               ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("ORDERCODE", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetEndo_JupMstCountbyPtNoGbJob(string strPtNo, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST              ");
            parameter.AppendSql(" WHERE TRUNC(RDATE) = TRUNC(SYSDATE)       ");
            parameter.AppendSql("   AND PTNO = :PTNO                        ");
            parameter.AppendSql("   AND DEPTCODE IN ('TO','HR')             ");
            parameter.AppendSql("   AND RESULTDATE IS NOT NULL              ");
            parameter.AppendSql("   AND GBJOB IN (:MASTERCODE)              ");
            parameter.AppendSql("   AND GBSUNAP IN ('1', '7')               ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("MASTERCODE", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetItemOpdMasterbyPaNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.OPD_MASTER      ");
            parameter.AppendSql(" WHERE ActDate  = TRUNC(SYSDATE)   ");
            parameter.AppendSql("   AND DeptCode = 'TO'             ");
            parameter.AppendSql("   AND PANO     = :PANO            ");

            parameter.Add("PANO", strPtNo);

            return ExecuteScalar<int>(parameter);
        }

        public int GetExam_XrayDetailCountbyPtNoXCode(string strPtNo, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL             ");
            parameter.AppendSql(" WHERE a.BDate         = TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND TRUNC(SEEKDATE) = TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND DeptCode IN ('TO','HR')             ");
            parameter.AppendSql("   AND PacsNo IS NOT NULL                  ");
            parameter.AppendSql("   AND XSendDate IS NOT NULL               ");
            parameter.AppendSql("   AND b.MASTERCODE IN (:MASTERCODE)       ");

            parameter.Add("PANO", strPtNo);
            parameter.AddInStatement("MASTERCODE", strACT_SET_Data);

            return ExecuteScalar<int>(parameter);
        }

        public int GetExam_SpecMstCountbyPtNoMasterCode(string strPtNo, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_SPECMST a, KOSMOS_OCS.EXAM_RESULTC b    ");
            parameter.AppendSql(" WHERE a.BDate = TRUNC(SYSDATE)                                ");
            parameter.AppendSql("   AND a.PANO  = :PANO                                         ");
            parameter.AppendSql("   AND a.DeptCode IN ('TO','HR')                               ");
            parameter.AppendSql("   AND a.ReceiveDate IS NOT NULL                               ");
            parameter.AppendSql("   AND a.Cancel IS NULL                                        ");
            parameter.AppendSql("   AND a.SpecNo = b.SpecNo(+)                                  ");
            parameter.AppendSql("   AND b.MASTERCODE IN (:MASTERCODE)                           ");
            //parameter.AppendSql("   AND b.MASTERCODE = :MASTERCODE                              ");

            parameter.Add("PANO", strPtNo);
            parameter.AddInStatement("MASTERCODE", strACT_SET_Data);
            //parameter.Add("MASTERCODE", strACT_SET_Data);

            return ExecuteScalar<int>(parameter);
        }

        public string GetHeaSRevBySDate(string argCurDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT REMARK                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SREV                    ");
            parameter.AppendSql(" WHERE SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')   ");

            parameter.Add("SDATE", argCurDate);

            return ExecuteScalar<string>(parameter);
        }

        public int ComDeleteByUseType(string argTable, string argColumn, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA." + argTable + "    ");
            parameter.AppendSql(" WHERE " + argColumn + "= :COL         ");

            parameter.Add("COL", argCode);

            return ExecuteNonQuery(parameter);
        }

        public int GetSignImagebyHicDojangSabun(string strSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOJANG          ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                  ");

            parameter.Add("SABUN", strSabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int HicSPcMcodeExamInsert(object argMCodem, string argExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SPC_MCODEEXAM (MCODE, EXCODE)  ");
            parameter.AppendSql(" VALUES (:MCODE, :EXCODE )                                 ");

            parameter.Add("MCODE", argMCodem, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", argExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetExCodeHNameByMCode(object strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.EXCODE, b.HNAME               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_MCODEEXAM a ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE b        ");
            parameter.AppendSql(" WHERE a.MCODE=:MCODE                  ");
            parameter.AppendSql("   AND a.EXCODE=b.CODE(+)              ");
            parameter.AppendSql(" ORDER BY a.EXCODE                     ");

            parameter.Add("MCODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetItemHicXMunjinbyWrtNo(long fnWrtno1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,JinGbn, XP1,XPJONG,XPLACE,XREMARK,XTERM,XTERM1,XMUCH,XJUNGSAN, MUN1,JUNGSAN1, MunDrNo     ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate, PANJENG    ");
            parameter.AppendSql("     , PanjengDrno, JUNGSAN2 , JUNGSAN3, STS ,Pan, Sogen, Jochi                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_X_MUNJIN                                                                        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                                  ");

            parameter.Add("WRTNO", fnWrtno1);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int UpdateHic_X_Munjin(long fnWRTNO, string strOK)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_X_MUNJIN SET    ");
            if (strOK == "OK")
            {
                parameter.AppendSql("       STS = 'Y'                   ");
            }
            else
            {
                parameter.AppendSql("       STS = ''                    ");
            }
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetItembyYearLtdCodeBangi(string fstrGjYear, string fstrLtdCode, string fstrGjBangi)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM KOSMOS_PMPA.HIC_LTDINWON2    ");
            parameter.AppendSql(" WHERE YEAR    = :YEAR                     ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                  ");
            parameter.AppendSql("   AND BANGI   = :BANGI                    ");

            parameter.Add("YEAR", fstrGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", fstrLtdCode);
            parameter.Add("BANGI", fstrGjBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetHic_X_MunjinbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,JinGbn, XP1,XPJONG,XPLACE,XREMARK,XTERM,XTERM1,XMUCH,XJUNGSAN, MUN1,JUNGSAN1, MunDrNo     ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate, PANJENG    ");
            parameter.AppendSql("     , PanjengDrno, JUNGSAN2 , JUNGSAN3, STS ,Pan, Sogen, Jochi,WorkYN,SahuCode,SANGDAM                ");
            parameter.AppendSql("     , JILBYUNG,BLOOD1,BLOOD2,BLOOD3,SKIN1,SKIN2,SKIN3,NERVOUS1,EYE1,EYE2,CANCER1,GAJOK                ");
            parameter.AppendSql("     , BLOOD , NERVOUS2, CANCER2, SYMPTON, JIKJONG1, JIKJONG2, JIKJONG3,REEXAM                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_X_MUNJIN                                                                        ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                                                 ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<COMHPC> GetListSpecNobyHicNo(string argPano, long argHicno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SPECNO                     ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_SPECMST    ");
            parameter.AppendSql(" WHERE PANO  = :PANO             ");
            parameter.AppendSql("   AND HICNO = :HICNO            ");

            parameter.Add("PANO", argPano);
            parameter.Add("HICNO", argHicno);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC ReadFirstPanDrno(long pANO, string gJYEAR, string gJJONG, string jEPDATE, string[] strJongSQL)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, a.PanjengDrNo AS DRNO1, b.PanjengDrNo AS DRNO2 ");
            switch (gJJONG)
            {
                case "27":
                case "28":
                case "29":
                case "33": parameter.AppendSql("   FROM HIC_JEPSU a, HIC_RES_SPECIAL b   "); break;
                case "50": parameter.AppendSql("   FROM HIC_JEPSU a, HIC_X_MUNJIN b      "); break;
                default: parameter.AppendSql("   FROM HIC_JEPSU a, HIC_RES_BOHUM1 b    "); break;
            }
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND a.PANO   =:PANO                                 ");
            parameter.AppendSql("   AND a.GJYEAR =:GJYEAR                               ");
            parameter.AppendSql("   AND a.GJJONG IN ( :GJJONG )                         ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                               ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:JEPDATE,'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                            ");
            parameter.AppendSql(" ORDER By a.WRTNO DESC                                 ");

            parameter.Add("PANO", pANO);
            parameter.Add("GJYEAR", gJYEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("GJJONG", strJongSQL, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", jEPDATE);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetConclusionConFDr1byHisOrderId(string strXrayno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CONCLUSION, CONFDR1             ");
            parameter.AppendSql("  FROM PACS.XRAY_PACS_SREPORT          ");
            parameter.AppendSql(" WHERE HISORDERID = :HISORDERID        ");
            parameter.AppendSql("   AND READSTAT = 'C'                  ");
            parameter.AppendSql(" ORDER BY WORKTIME DESC                ");

            parameter.Add("HISORDERID", strXrayno);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public string GetSeekDatebyPaNoSeekDate(string fstrPano, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(SEEKDATE, 'YYYYMM') AS SEEKDATE                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL                                         ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                    ");
            parameter.AppendSql("   AND GBRESERVED = '7'                                                ");
            parameter.AppendSql("   AND XJONG = '4'                                                     ");
            parameter.AppendSql("   AND PACSSTUDYID IS NOT NULL                                         ");
            parameter.AppendSql("   AND GBEND = '1'                                                     ");
            parameter.AppendSql("   AND SEEKDATE < TO_DATE(:SEEKDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND XCODE IN ( SELECT XCODE From KOSMOS_PMPA.XRAY_CODE              ");
            parameter.AppendSql("                   WHERE XNAME LIKE '%CT Chest%')                      ");
            parameter.AppendSql(" ORDER BY SEEKDATE DESC                                                ");

            parameter.Add("PANO", fstrPano);
            parameter.Add("SEEKDATE", fstrJepDate);

            return ExecuteScalar<string>(parameter);
        }

        public List<COMHPC> GetItembyJepDate(string strFrDate, string strToDate, string strJob, string strExamMeterial, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,b.GjJong,b.Pano,b.SName,b.LtdCode,b.Sex,b.Age,b.BuseName                    ");
            parameter.AppendSql("     , b.Pano,a.ExCode,d.HName,c.Result,a.Remark,e.Jumin                                   ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate                                             ");
            parameter.AppendSql("     , TO_CHAR(a.BloodDate,'YYYY-MM-DD') BloodDate                                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT_H827 a, KOSMOS_PMPA.HIC_JEPSU b, KOSMOS_PMPA.HIC_RESULT c    ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_ExCode d, KOSMOS_PMPA.HIC_PATIENT e                                 ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                         ");
            if (strJob == "1") //채혈대상자
            {
                parameter.AppendSql("   AND a.BloodDate IS NULL                                                             ");
            }
            else if (strJob == "2") //채혈완료
            {
                parameter.AppendSql("   AND a.BloodDate IS NOT NULL                                                         ");
            }
            if (strExamMeterial != "전체" && !strExamMeterial.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND a.EXCODE = :EXCODE                                                              ");
            }
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND b.LTDCODE = :LTDCODE                                                            ");
            }
            parameter.AppendSql("   AND b.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.WRTNO  = c.WRTNO(+)                                                               ");
            parameter.AppendSql("   AND b.Pano   = e.Pano                                                                   ");
            parameter.AppendSql("   AND a.ExCode = c.ExCode(+)                                                              ");
            parameter.AppendSql("   AND a.ExCode = d.Code(+)                                                                ");
            parameter.AppendSql(" ORDER BY b.LtdCode,b.SName                                                                ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (strExamMeterial != "전체" && !strExamMeterial.IsNullOrEmpty())
            {
                parameter.Add("EXCODE", strExamMeterial, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetHolyDay(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT HOLYDAY FROM KOSMOS_PMPA.BAS_JOB            ");
            parameter.AppendSql(" WHERE JOBDATE = TO_DATE(:JOBDATE,'YYYY-MM-DD')    ");

            parameter.Add("JOBDATE", strDate);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRowIdOpdMasterbyPanoBDate(string strPTNO, string strBDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID FROM KOSMOS_PMPA.OPD_MASTER       ");
            parameter.AppendSql(" WHERE PANO  = :PANO                               ");
            parameter.AppendSql("   AND BDATE = TO_Date(:BDATE,'YYYY-MM-DD')        ");
            parameter.AppendSql("   AND DEPTCODE='HR'                               ");

            parameter.Add("PANO", strPTNO);
            parameter.Add("BDATE", strBDATE);

            return ExecuteScalar<string>(parameter);
        }

        public int DeleteHicSjMstbyRowId(string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SJ_MST    ");
            parameter.AppendSql(" WHERE ROWID    = :RID           ");

            parameter.Add("RID", fstrROWID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertOpdMaster(COMHPC item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.OPD_MASTER                                            ");
            parameter.AppendSql("        (ACTDATE, PANO, DEPTCODE, BI, SNAME, SEX,AGE, JICODE, DRCODE           ");
            parameter.AppendSql("      , RESERVED, CHOJAE, GBGAMEK,GBSPC, JIN, SINGU,PART,JTIME,BDATE           ");
            parameter.AppendSql("      , EMR,GBUSE,MKSJIN)                                                      ");
            parameter.AppendSql(" VALUES (                                                                      ");
            parameter.AppendSql("        TRUNC(SYSDATE), :PANO, :DEPTCODE, :BI, :SNAME, :SEX                    ");
            parameter.AppendSql("      , :AGE, :JICODE, :DRCODE, :RESERVED, :CHOJAE, :GBGAMEK, :GBSPC, :JIN     ");
            parameter.AppendSql("      , :SINGU, :PART, SYSDATE, TO_DATE(:BDATE, 'YYYY-MM-DD')                  ");
            parameter.AppendSql("      , :EMR, :GBUSE, :MKSJIN)                                                 ");

            parameter.Add("PANO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", item.DEPTCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BI", item.BI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("JICODE", item.JICODE);
            parameter.Add("DRCODE", item.DRCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESERVED", item.RESERVED, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CHOJAE", item.CHOJAE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBGAMEK", item.GBGAMEK, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSPC", item.GBSPC, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JIN", item.JIN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SINGU", item.SINGU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PART", item.PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("EMR", item.EMR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBUSE", item.GBUSE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MKSJIN", item.MKSJIN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetRowIdQtyOcsOrderbyPtNoBDateSuCode(string strPTNO, string strBDATE, string strSuCode, string strDeptCode)
        {

            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID, Qty FROM KOSMOS_OCS.OCS_OORDER   ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                        ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                          ");

            parameter.Add("PTNO", strPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDATE);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int DeleteHicSjJindanbyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SJ_JINDAN   ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR           ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE          ");

            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteNonQuery(parameter);
        }

        public int InsertOcsOrder(COMHPC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_OORDER                                                                      ");
            parameter.AppendSql("       (PTNO, BDATE, DEPTCODE, SEQNO, ORDERCODE, SUCODE, BUN, SLIPNO, REALQTY, QTY, NAL, GBDIV         ");
            parameter.AppendSql("      , DOSCODE, GBBOTH, GBINFO, GBER, GBSELF, GBSPC, BI, DRCODE, REMARK, ENTDATE, GBSUNAP, TUYAKNO    ");
            parameter.AppendSql("      , ORDERNO, MULTI, MULTIREMARK, DUR, RESV, SCODESAYU, SCODEREMARK, GBSEND,SABUN,CORDERCODE        ");
            parameter.AppendSql("      ,  CSUCODE, CBUN, IP)                                                                            ");
            parameter.AppendSql("VALUES                                                                                                 ");
            parameter.AppendSql("       (:PTNO, TO_DATE(:BDATE, 'YYYY-MM-DD'), :DEPTCODE, :SEQNO, :ORDERCODE, :SUCODE, :BUN, :SLIPNO    ");
            parameter.AppendSql("      , :REALQTY, :QTY, :NAL, :GBDIV, :DOSCODE, :GBBOTH, :GBINFO, :GBER, :GBSELF, :GBSPC, :BI, :DRCODE ");
            parameter.AppendSql("      , :REMARK, SYSDATE, :GBSUNAP, :TUYAKNO, KOSMOS_OCS.SEQ_ORDERNO.NEXTVAL, :MULTI, :MULTIREMARK     ");
            parameter.AppendSql("      , :DUR, :RESV, :SCODESAYU, :SCODEREMARK, :GBSEND, :SABUN,:CORDERCODE, :CSUCODE, :CBUN, :IP)      ");

            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("DEPTCODE", item.DEPTCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SEQNO", item.SEQNO);
            parameter.Add("ORDERCODE", item.ORDERCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", item.SUCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BUN", item.BUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SLIPNO", item.SLIPNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REALQTY", item.REALQTY, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("QTY", item.QTY);
            parameter.Add("NAL", item.NAL);
            parameter.Add("GBDIV", item.GBDIV);
            parameter.Add("DOSCODE", item.DOSCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBBOTH", item.GBBOTH, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBINFO", item.GBINFO);
            parameter.Add("GBER", item.GBER, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSELF", item.GBSELF, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBSPC", item.GBSPC, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BI", item.BI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DRCODE", item.DRCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("GBSUNAP", item.GBSUNAP, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("TUYAKNO", item.TUYAKNO);
            parameter.Add("MULTI", item.MULTI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MULTIREMARK", item.MULTIREMARK);
            parameter.Add("DUR", item.DUR);
            parameter.Add("RESV", item.RESV, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SCODESAYU", item.SCODESAYU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SCODEREMARK", item.SCODEREMARK);
            parameter.Add("GBSEND", item.GBSEND, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SABUN", item.SABUN);
            parameter.Add("CORDERCODE", item.CORDERCODE);
            parameter.Add("CSUCODE", item.CSUCODE);
            parameter.Add("CBUN", item.CBUN);
            parameter.Add("IP", item.IP);

            return ExecuteNonQuery(parameter);
        }

        public int InsertOIlls(COMHPC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_OILLS                                                       ");
            parameter.AppendSql("       (PTNO, BDATE, DEPTCODE, SEQNO, ILLCODE, ENTDATE )                               ");
            parameter.AppendSql("VALUES                                                                                 ");
            parameter.AppendSql("       (:PTNO, TO_DATE(:BDATE, 'YYYY-MM-DD'), :DEPTCODE, :SEQNO, :ILLCODE, SYSDATE)    ");

            parameter.Add("PTNO", item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("DEPTCODE", item.DEPTCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SEQNO", item.SEQNO);
            parameter.Add("ILLCODE", item.ILLCODE);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPtNoBDate(string strPtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT FROM KOSMOS_OCS.OCS_OILLS    ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                             ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE,'YYYY-MM-DD')     ");
            parameter.AppendSql("   AND ILLCODE  = 'Z018'                           ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountOrderbyPtnoJepDate(string strPtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT FROM KOSMOS_OCS.OCS_OORDER   ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND ORDERCODE IN('00440110', '00440120')        ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateOcsOrder(long nQty, string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.OCS_OORDER SET   ");
            parameter.AppendSql("       QTY     = :QTY              ");
            parameter.AppendSql("     , REALQTY = :QTY              ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            parameter.Add("QTY", nQty);
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetRowIdQtybyOcsOrder(string strPtNo, string strBDATE, string strSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID, QTY FROM KOSMOS_OCS.OCS_OORDER       ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                             ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                          ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDATE);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int GetCountbyPaNoBDate(string strPtNo, string strBDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT FROM KOSMOS_PMPA.OPD_MASTER   ");
            parameter.AppendSql(" WHERE PANO     = :PANO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                             ");

            parameter.Add("PANO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDATE);

            return ExecuteScalar<int>(parameter);
        }

        public int DeleteOcsOorder(string strPtNo, string strSuCode, string strBdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_OORDER                       ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DeptCode = 'HR'                             ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                          ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBdate);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateOcsHyang(COMHPC item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_OCS.OCS_MAYAK SET   ");
            parameter.AppendSql("        WARDCODE =:WARDCODE        ");
            parameter.AppendSql("      , QTY      =:QTY             ");
            parameter.AppendSql("      , REALQTY  =:REALQTY         ");
            parameter.AppendSql("      , DRSABUN  =:DRSABUN         ");
            parameter.AppendSql("      , ORDERNO  =:ORDERNO         ");
            parameter.AppendSql("      , DOSCODE  =:DOSCODE         ");
            parameter.AppendSql("      , CERTNO   =:CERTNO          ");
            parameter.AppendSql("  WHERE ROWID   = :RID             ");

            parameter.Add("WARDCODE", item.WARDCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("QTY", item.QTY, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("DRSABUN", item.DRSABUN);
            parameter.Add("ORDERNO", item.ORDERNO);
            parameter.Add("DOSCODE", item.DOSCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CERTNO", item.CERTNO);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSelectOcsHyang(COMHPC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_HYANG                                                                                   ");
            parameter.AppendSql("       (PTNO,SNAME,BI,WARDCODE,BDATE,ENTDATE,DEPTCODE,DRSABUN,IO,SUCODE,QTY,REALQTY,NAL,DOSCODE                    ");
            parameter.AppendSql("     , ORDERNO, REMARK1, REMARK2,SEQNO,PRINT,SEX,AGE,JUMIN,JUSO,JUMIN3)                                            ");
            parameter.AppendSql("SELECT PTNO, SNAME, :BI, :WARDCODE, BDATE, ENTDATE, DEPTCODE, DRSABUN, :IO, SUCODE, :QTY, :REALQTY, :NAL, :DOSCODE ");
            parameter.AppendSql("     , :ORDERNO, :REMARK1, :REMARK2, '', '', SEX, AGE, JUMIN, :JUSO, JUMIN2                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                                                               ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                                                ");

            parameter.Add("BI", item.BI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WARDCODE", item.WARDCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("IO", item.IO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("QTY", item.QTY, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("NAL", item.NAL);
            parameter.Add("DOSCODE", item.DOSCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORDERNO", item.ORDERNO);
            parameter.Add("REMARK1", item.REMARK1);
            parameter.Add("REMARK2", item.REMARK2);
            parameter.Add("JUSO", item.JUSO);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateOcsMayak(COMHPC item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_OCS.OCS_MAYAK SET   ");
            parameter.AppendSql("        WARDCODE =:WARDCODE        ");
            parameter.AppendSql("      , QTY      =:QTY             ");
            parameter.AppendSql("      , REALQTY  =:REALQTY         ");
            parameter.AppendSql("      , DRSABUN  =:DRSABUN         ");
            parameter.AppendSql("      , ORDERNO  =:ORDERNO         ");
            parameter.AppendSql("      , DOSCODE  =:DOSCODE         ");
            parameter.AppendSql("      , CERTNO   =:CERTNO          ");
            parameter.AppendSql("  WHERE ROWID   = :ROWID           ");

            parameter.Add("WARDCODE", item.WARDCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("QTY", item.QTY, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("DRSABUN", item.DRSABUN);
            parameter.Add("ORDERNO", item.ORDERNO);
            parameter.Add("DOSCODE", item.DOSCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CERTNO", item.CERTNO);
            parameter.Add("ROWID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSelectOcsMayak(COMHPC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_MAYAK                                                                                   ");
            parameter.AppendSql("       (PTNO,SNAME,BI,WARDCODE,BDATE,ENTDATE,DEPTCODE,DRSABUN,IO,SUCODE,QTY,REALQTY,NAL,DOSCODE                    ");
            parameter.AppendSql("     , ORDERNO, REMARK1, REMARK2,SEQNO,PRINT,SEX,AGE,JUMIN,JUSO,JUMIN3)                                            ");
            parameter.AppendSql("SELECT PTNO, SNAME, :BI, :WARDCODE, BDATE, ENTDATE, DEPTCODE, DRSABUN, :IO, SUCODE, :QTY, :REALQTY, :NAL, :DOSCODE ");
            parameter.AppendSql("     , :ORDERNO, :REMARK1, :REMARK2, '', '', SEX, AGE, JUMIN, :JUSO, JUMIN2                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                                                               ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                                                ");

            parameter.Add("BI", item.BI, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WARDCODE", item.WARDCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("IO", item.IO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("QTY", item.QTY, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("NAL", item.NAL);
            parameter.Add("DOSCODE", item.DOSCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORDERNO", item.ORDERNO);
            parameter.Add("REMARK1", item.REMARK1);
            parameter.Add("REMARK2", item.REMARK2);
            parameter.Add("JUSO", item.JUSO);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetRowIdOcsHyangbyPtNo(string strPTNO, string strBDATE, string strSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID FROM KOSMOS_OCS.OCS_HYANG         ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                             ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                          ");

            parameter.Add("PTNO", strPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDATE);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public COMHPC GetRowIdOcsMayakbyPtNo(string strPTNO, string strBDATE, string strSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.OCS_MAYAK             ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = 'HR'                             ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                          ");

            parameter.Add("PTNO", strPTNO);
            parameter.Add("BDATE", strBDATE);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public double GetOrderNoOcsOrderbyPtno(string strPTNO, string strBDATE, string strSuCode, string strDeptCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ORDERNO FROM KOSMOS_OCS.OCS_OORDER          ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                        ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                          ");

            parameter.Add("PTNO", strPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strBDATE);
            parameter.Add("SUCODE", strSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", strDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<double>(parameter);
        }

        public int UpdateQtybyRowId(double nQty, string rOWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_OCS.OCS_OORDER SET  ");
            parameter.AppendSql("        QTY         = :QTY         ");
            parameter.AppendSql("      , REALQTY     = :QTY         ");
            parameter.AppendSql("  WHERE ROWID       = :RID         ");

            parameter.Add("QTY", nQty);
            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetItembyJepDate(string strFrDate, string strToDate, long nWrtNo, long nLtdCode, string strSort)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.SDate,'YYYYMMDD') SDate                   ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYYMMDD') JepDate,a.PanjengGbn          ");
            parameter.AppendSql("     , a.Sabun,a.Sogen,a.Gbn,a.Remark,b.PanjengDrno                ");
            parameter.AppendSql("     , b.GjJong,b.SName,b.LtdCode,b.Sex,b.Age,c.Tel,c.HPhone       ");
            parameter.AppendSql("     , FC_HC_PATIENT_JUMINNO(b.PTNO) JUMIN, B.TONGBODATE           ");
            parameter.AppendSql("  FROM HIC_RES_SAHUSANGDAM a,HIC_JEPSU b,HIC_PATIENT c             ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND a.Gbn <> '4'                                                "); //제외
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                          ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                           ");
            if (nWrtNo != 0)
            {
                parameter.AppendSql("   AND b.WRTNO = :WRTNO                                        ");
            }
            else if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND b.LTDCODE = :LTDCODE                                    ");
            }
            parameter.AppendSql("   AND b.Pano = c.Pano(+)                                          ");
            if (strSort == "1")
            {
                parameter.AppendSql(" ORDER BY b.SName, a.WRTNO,a.JepDate                           ");
            }
            else if (strSort == "2")
            {
                parameter.AppendSql(" ORDER BY a.JepDate,b.SName,a.WRTNO                            ");
            }
            else if (strSort == "3")
            {
                parameter.AppendSql(" ORDER BY b.LtdCode,b.SName,a.WRTNO                            ");
            }

            if (nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int DeleteHicSjPanobyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SJ_PANO     ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR           ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE          ");

            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetItembySpcPanjengJepsu(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,SUM(DECODE(a.Panjeng,'5',1,0)) D1,SUM(DECODE(a.Panjeng,'6',1,0)) D2     ");
            parameter.AppendSql("     , SUM(DECODE(a.Panjeng,'A',1,0)) DN                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG a, KOSMOS_PMPA.HIC_JEPSU b                          ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND a.PanjengDate IS NOT NULL                                                       ");
            parameter.AppendSql("   AND a.Panjeng IN ('5','6','A')                                                      ");
            parameter.AppendSql("   AND a.MCode <> 'ZZZ'                                                                ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                            ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND b.GjJong IN ('11','14','16','19','23','28','41','44')                           ");
            parameter.AppendSql(" GROUP BY a.WRTNO                                                                      ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<COMHPC> GetJobDatebyBasJob(string strFDate, string strTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JobDate,'DD') ILJA FROM KOSMOS_PMPA.BAS_JOB ");
            parameter.AppendSql(" WHERE JOBDATE >= TO_DATE(:FRDATE,'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND JOBDATE <= TO_DATE(:TODATE,'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND HolyDay = '*'                                       ");

            parameter.Add("FRDATE", strFDate);
            parameter.Add("TODATE", strTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItembySpcPanjengJepsuPatient(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.JepDate,'YYYYMMDD') JepDate,a.Panjeng,a.PanjengDrno               ");
            parameter.AppendSql("     , a.SogenRemark,b.GjJong,b.SName,b.LtdCode,b.Sex,b.Age,c.Tel,c.HPhone                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG a, KOSMOS_PMPA.HIC_JEPSU b, KOSMOS_PMPA.HIC_PATIENT c   ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                    ");
            parameter.AppendSql("   AND a.PanjengDate IS NOT NULL                                                           ");
            parameter.AppendSql("   AND a.Panjeng IN ('5','6','A')                                                          ");
            parameter.AppendSql("   AND a.MCode <> 'ZZZ'                                                                    ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                                   ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                ");
            parameter.AppendSql("   AND b.Pano = c.Pano(+)                                                                  ");
            parameter.AppendSql(" ORDER BY a.WRTNO,a.Panjeng DESC                                                           ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetXP1byWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,JinGbn, XP1,XPJONG,XPLACE,XREMARK,XTERM,XTERM1,XMUCH,XJUNGSAN, MUN1,JUNGSAN1, MunDrNo                 ");
            parameter.AppendSql("     , TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate, PANJENG, PanjengDrno   ");
            parameter.AppendSql("     , JUNGSAN2 , JUNGSAN3, STS ,Pan, Sogen, Jochi,WorkYN,SahuCode,SANGDAM                                         ");
            parameter.AppendSql("     , JILBYUNG,BLOOD1,BLOOD2,BLOOD3,SKIN1,SKIN2,SKIN3,NERVOUS1,EYE1,EYE2,CANCER1,GAJOK                            ");
            parameter.AppendSql("     , BLOOD , NERVOUS2, CANCER2, SYMPTON, JIKJONG1, JIKJONG2, JIKJONG3,REEXAM                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_X_MUNJIN                                                                                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<COMHPC> GetWrtNOCntGrpNo(string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MST.WRTNO, SUM(1) CNT, B.GRPNO                          ");
            parameter.AppendSql("  FROM (                                                       ");
            parameter.AppendSql("        SELECT WRTNO, EXCODE                                   ");
            parameter.AppendSql("          From KOSMOS_PMPA.HEA_AUTOPAN_LOGIC                   ");
            parameter.AppendSql("         UNION ALL                                             ");
            parameter.AppendSql("        SELECT WRTNO, EXCODE                                   ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_EXAM              ");
            parameter.AppendSql("         UNION ALL                                             ");
            parameter.AppendSql("        SELECT WRTNO, EXCODE1 EXCODE                           ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC              ");
            parameter.AppendSql("         UNION ALL                                             ");
            parameter.AppendSql("        SELECT WRTNO, EXCODE2 EXCODE                           ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC              ");
            parameter.AppendSql("        ) MST                                                  ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_AUTOPAN B                               ");
            parameter.AppendSql(" WHERE EXISTS (                                                ");
            parameter.AppendSql("               SELECT * FROM KOSMOS_PMPA.HIC_RESULT SUB        ");
            parameter.AppendSql("                WHERE MST.EXCODE = SUB.EXCODE                  ");
            parameter.AppendSql("                  AND SUB.WRTNO = :WRTNO                       ");
            parameter.AppendSql("              )                                                ");
            parameter.AppendSql("   AND MST.WRTNO = B.WRTNO                                     ");
            parameter.AppendSql(" GROUP BY MST.WRTNO, B.GRPNO                                   ");
            parameter.AppendSql(" ORDER BY CNT DESC                                             ");

            parameter.Add("WRTNO", argJepNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetMunjinNightbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, ITEM1_DATA, ITEM1_JEMSU          ");
            parameter.AppendSql("     , ITEM1_PANJENG, ITEM2_DATA, ITEM2_JEMSU  ");
            parameter.AppendSql("     , ITEM2_PANJENG, ITEM3_DATA, ITEM3_JEMSU  ");
            parameter.AppendSql("     , ITEM3_PANJENG, ENTDATE, ENTSABUN        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MUNJIN_NIGHT            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetCytologybyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,a.GbIO,a.OrderCode,b.OrderName,a.DeptCode,a.Anatno  ");
            parameter.AppendSql("     , a.Result1,a.Result2,a.ROWID                                                             ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ANATMST a, KOSMOS_OCS.OCS_ORDERCODE b  ,KOSMOS_OCS.EXAM_SPECMST c       ");
            parameter.AppendSql(" WHERE a.PTNO     = :PTNO                                                                      ");
            parameter.AppendSql("   AND a.BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("   AND a.DeptCode IN ('HR','TO')                                                               ");
            parameter.AppendSql("   AND SUBSTR(a.AnatNo,1,1) IN ('C','P')                                                       ");
            parameter.AppendSql("   AND a.OrderCode = b.OrderCode(+)                                                            ");
            parameter.AppendSql("   AND a.SpecNo = c.SpecNo(+)                                                                  ");
            parameter.AppendSql("   AND (b.SendDept <> 'N' OR b.SendDept IS NULL)                                               ");
            parameter.AppendSql(" ORDER BY a.BDate DESC                                                                         ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrJepDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public long GetEmrNobyPtNoFormNo(string fstrPtno, long nFormNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EMRNO FROM KOSMOS_EMR.EMRXML    ");
            parameter.AppendSql(" WHERE PTNO   = :PTNO                  ");
            parameter.AppendSql("   AND FORMNO = :FORMNO                ");
            parameter.AppendSql("   AND MedDeptCd IN ('HR','TO')        ");
            parameter.AppendSql("   AND CHARTDATE = :CHARTDATE          ");

            parameter.Add("PTNO", fstrPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("FORMNO", nFormNo);
            parameter.Add("CHARTDATE", strJepDate);

            return ExecuteScalar<long>(parameter);
        }

        public List<COMHPC> GetItemHicDojang(string strSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SABUN, LICENSE, IMAGE       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOJANG      ");
            if (strSabun != "")
            {
                parameter.AppendSql(" WHERE TRIM(LICENSE) = :SABUN  ");
            }

            if (strSabun != "")
            {
                parameter.Add("SABUN", strSabun.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetEntDatebyHicPrivacyAccept(string argPTNO, string argYEAR)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(ENTDATE,'YYYYMMDD') ENTDATE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PRIVACY_ACCEPT          ");
            parameter.AppendSql(" WHERE PTNO   = :PTNO                          ");
            parameter.AppendSql(" WHERE GJYEAR = :GJYEAR                        ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJYEAR", argYEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetExamAnataMst(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DECODE(SUBSTR(b.hrremark1,9,1)  ,'1','1',DECODE(SUBSTR(b.hrremark1,10,1),'1','2', '0' )) GBCHECK    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_OCS.EXAM_ANATMST b                                                  ");
            parameter.AppendSql(" WHERE a.PtNo = b.PtNo                                                                                     ");
            parameter.AppendSql("   AND a.JEPDATE=b.BDATE                                                                                   ");
            parameter.AppendSql("   AND b.GbJob IN ('V')                                                                                    ");
            parameter.AppendSql("   AND SUBSTR(b.AnatNo,1,1) ='P'                                                                           ");
            parameter.AppendSql("   AND b.ORDERCODE ='PAP`S'                                                                                ");
            parameter.AppendSql("   AND b.DEPTCODE IN ('HR','TO')                                                                           ");
            parameter.AppendSql("   AND a.WRTNO =                                                                                           ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int SaveHicXMunjinbyWrtNo(long nWrtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("MERGE INTO KOSMOS_PMPA.HIC_X_MUNJIN a                  ");
            parameter.AppendSql("using dual d                                           ");
            parameter.AppendSql("   on (a.WRTNO     = :WRTNO)                           ");
            parameter.AppendSql(" when matched then                                     ");
            parameter.AppendSql("  update set                                           ");
            parameter.AppendSql("         JEPDATE   = TO_DATE(:JEPDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("    when not matched then                              ");
            parameter.AppendSql("  insert                                               ");
            parameter.AppendSql("         (WRTNO                                        ");
            parameter.AppendSql("        , JEPDATE)                                     ");
            parameter.AppendSql(" VALUES                                                ");
            parameter.AppendSql("         (:WRTNO                                       ");
            parameter.AppendSql("        , TO_DATE(:JEPDATE, 'YYYY-MM-DD'))             ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetItembySahuSangdamWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,TO_CHAR(a.SDate,'YYYYMMDD') SDate                                               ");
            parameter.AppendSql("     , TO_CHAR(a.JepDate,'YYYYMMDD') JepDate, a.PanjengGbn                                     ");
            parameter.AppendSql("     , a.SABUN, a.Sogen, a.Gbn, a.Remark, b.PanjengDrno                                        ");
            parameter.AppendSql("     , b.GjJong, b.SName, b.LtdCode, b.Sex, b.Age, c.Tel, c.HPhone                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_SAHUSANGDAM a, KOSMOS_PMPA.HIC_JEPSU b, KOSMOS_PMPA.HIC_PATIENT c   ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND a.PANJENGDATE IS NOT NULL                                                               ");
            parameter.AppendSql("   AND a.PANJENG IN ('5','6','A')                                                              ");
            parameter.AppendSql("   AND a.MCODE <> 'ZZZ'                                                                        ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                       ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND b.PANO  = c.PANO(+)                                                                     ");
            parameter.AppendSql(" ORDER BY a.WRTNO, a.PANJENG DESC                                                              ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int UpdateHic_X_MunjinResult(COMHPC item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_X_MUNJIN SET                       ");
            parameter.AppendSql("        STS         = :STS                                 ");
            parameter.AppendSql("      , PAN         = :PAN                                 ");
            parameter.AppendSql("      , SOGEN       = :SOGEN                               ");
            parameter.AppendSql("      , WORKYN      = :WORKYN                              ");
            parameter.AppendSql("      , SAHUCODE    = :SAHUCODE                            ");
            parameter.AppendSql("      , PANJENG     = :PANJENG                             ");
            parameter.AppendSql("      , PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("      , REEXAM      = :REEXAM                              ");
            parameter.AppendSql("      , PANJENGDRNO = :PANJENGDRNO                         ");
            parameter.AppendSql("  WHERE WRTNO       = :WRTNO                               ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("STS", item.STS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PAN", item.PAN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SOGEN", item.SOGEN);
            parameter.Add("WORKYN", item.WORKYN);
            parameter.Add("SAHUCODE", item.SAHUCODE);
            parameter.Add("PANJENG", item.PANJENG);
            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("REEXAM", item.REEXAM);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateHic_X_MunjinbyWrtNo(COMHPC item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_X_MUNJIN SET   ");
            parameter.AppendSql("      , JINGBN   = :JINGBN             ");
            parameter.AppendSql("      , XP1      = :XP1                ");
            parameter.AppendSql("      , XPJONG   = :XPJONG             ");
            parameter.AppendSql("      , XPLACE   = :XPLACE             ");
            parameter.AppendSql("      , XREMARK  = :XREMARK            ");
            parameter.AppendSql("      , XMUCH    = :XMUCH              ");
            parameter.AppendSql("      , XTERM    = :XTERM              ");
            parameter.AppendSql("      , XTERM1   = :XTERM1             ");
            parameter.AppendSql("      , XJUNGSAN = :XJUNGSAN           ");
            parameter.AppendSql("      , MUN1     = :MUN1               ");
            parameter.AppendSql("      , JUNGSAN1 = :JUNGSAN1           ");
            parameter.AppendSql("      , JUNGSAN2 = :JUNGSAN2           ");
            parameter.AppendSql("      , JUNGSAN3 = :JUNGSAN3           ");
            parameter.AppendSql("      , MUNDRNO  = :MUNDRNO            ");
            parameter.AppendSql("  WHERE WRTNO    = :WRTNO              ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("STS", item.STS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PAN", item.PAN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SOGEN", item.SOGEN);
            parameter.Add("WORKYN", item.WORKYN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SAHUCODE", item.SAHUCODE);
            parameter.Add("PANJENG", item.PANJENG);
            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("REEXAM", item.REEXAM);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetItembyHicSjPanoJepsuPatient(string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.Buse,b.SName,c.Jumin,b.UCodes,a.ChukResult,a.Panjeng,b.GjJong ");
            parameter.AppendSql("     , TO_CHAR(JepDate, 'MM/DD') JepDate, a.ROWID RID                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SJ_Pano a                                               ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU   b                                               ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT c                                               ");
            parameter.AppendSql(" WHERE a.GJYEAR  = :GJYEAR                                                     ");
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                ");
            }
            parameter.AppendSql("   AND a.GbDel IS NULL                                                         ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                      ");
            parameter.AppendSql("   AND b.Pano=c.Pano(+)                                                        ");
            parameter.AppendSql(" ORDER BY b.SName,c.Jumin                                                      ");

            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetHICsJPanoJepsubyGjYearLtdCode(string strGjYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Buse,b.UCodes,a.ChukResult,COUNT(*) CNT           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SJ_Pano a, KOSMOS_PMPA.HIC_JEPSU b  ");
            parameter.AppendSql(" WHERE a.GJYEAR  = :GJYEAR                                 ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                ");
            parameter.AppendSql("   AND a.GbDel IS NULL                                     ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                  ");
            parameter.AppendSql(" GROUP BY a.Buse, b.UCodes, a.ChukResult                   ");
            parameter.AppendSql(" ORDER BY a.Buse, b.UCodes, a.ChukResult                   ");

            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItembyHicMirHemsLtd(string strYear, string strViewLtd)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.LTDCODE, b.NAME, COUNT(*) CNT                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_MIR_HEMS a, KOSMOS_PMPA.HIC_LTD b   ");
            parameter.AppendSql(" WHERE a.YEAR = :YEAR                                      ");
            parameter.AppendSql("   AND a.LTDCODE = b.CODE(+)                               ");
            if (strViewLtd != "")
            {
                parameter.AppendSql("   AND b.NAME LIKE :NAME                               ");
            }
            parameter.AppendSql(" GROUP BY a.LTDCODE, b.NAME                                ");
            parameter.AppendSql(" ORDER BY b.NAME                                           ");

            parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (strViewLtd != "")
            {
                parameter.AddLikeStatement("NAME", strViewLtd);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItembyPanjengWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,SUM(DECODE(a.Panjeng,'5',1,0)) D1,SUM(DECODE(a.Panjeng,'6',1,0)) D2     ");
            parameter.AppendSql("     , SUM(DECODE(a.Panjeng,'A',1,0)) DN                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG a, KOSMOS_PMPAHIC_JEPSU b                           ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                ");
            parameter.AppendSql("   AND a.PanjengDate IS NOT NULL                                                       ");
            parameter.AppendSql("   AND a.Panjeng IN ('5','6','A')                                                      ");
            parameter.AppendSql("   AND a.MCode <> 'ZZZ'                                                                ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                            ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND b.GjJong IN ('11','14','16','19','23','28','41','44')                           ");
            parameter.AppendSql(" GROUP BY a.WRTNO                                                                      ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int UpdateHicOmrInfobyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_OMRINFO SET        ");
            parameter.AppendSql("        SendOK = SYSDATE                   ");
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO                    ");
            parameter.AppendSql("    AND FormCode = '12001'                 ");
            parameter.AppendSql("    AND SendOK IS NULL                     ");
            parameter.AppendSql("    AND SendTime IS NOT NULL               ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetListMasterCodeByHicWrtno(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT m.MasterCode               ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_RESULT a   ");
            parameter.AppendSql("       ,KOSMOS_PMPA.HIC_EXCODE b   ");
            parameter.AppendSql("       ,KOSMOS_OCS.EXAM_MASTER m   ");
            parameter.AppendSql("  WHERE 1 = 1                      ");
            parameter.AppendSql("    AND a.WRTNO =:WRTNO            ");
            parameter.AppendSql("    AND a.EXCODE = b.CODE(+)       ");
            parameter.AppendSql("    AND b.GBAUTOSEND='Y'           ");   //자동전송코드
            parameter.AppendSql("    AND b.EXCODE = m.MASTERCODE(+) ");
            parameter.AppendSql("  ORDER BY m.MASTERCODE            ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetCountSpcPanjengbyLtdCode(string fstrDate1, string fstrDate2, long ltdCode, string strJob, string strGjYear, string strBangi)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('x') CNT                                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_SPC_PANJENG b                      ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                        ");
            parameter.AppendSql("   AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                           ");
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            parameter.AppendSql("   AND b.PANJENG IN ('3','4','5','6','7','8')                                      ");
            if (strJob == "1") //특수
            {
                parameter.AppendSql("   AND a.Gjjong IN ('11','12','14','23')                                       ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("   AND a.Gjjong IN ('22','24','30')                                            ");
            }
            else
            {
                parameter.AppendSql("   AND a.Gjjong  IN ('69')                                                     ");
            }
            parameter.AppendSql("   AND b.PANJENGDRNO > 0                                                           ");
            parameter.AppendSql(" GROUP BY A.PANO                                                                   ");
            parameter.AppendSql(" Union                                                                             ");
            parameter.AppendSql("SELECT a.PANO                                                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_BOHUM1 b                       ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                        ");
            parameter.AppendSql("   AND a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                 ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                           ");
            if (strJob == "1") //특수
            {
                parameter.AppendSql("   AND a.Gjjong IN ('11','12','14','23')                                       ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql(" AND a.Gjjong IN ('22','24','30')                                              ");
            }
            else
            {
                parameter.AppendSql(" AND a.Gjjong  IN ('69')                                                       ");
            }
            parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            parameter.AppendSql("   AND ( b.PANJENGC1 ='1' OR  b.PANJENGC2 ='1' OR  b.PANJENGC3 = '1'               ");
            parameter.AppendSql("    OR b.PANJENGC4 ='1' OR  b.PANJENGC5 ='1'                                       ");
            parameter.AppendSql("    OR b.PANJENGD11 <> '' OR b.PANJENGD12 <> '' OR b.PANJENGD13 <> ''              ");
            parameter.AppendSql("    OR b.PANJENGD21 ='1' OR b.PANJENGD22 ='1' OR b.PANJENGD23 ='1'                 ");
            parameter.AppendSql("    OR b.PANJENGR1='1' OR b.PANJENGR2='1' OR b.PANJENGR3='1'                       ");
            parameter.AppendSql("    OR b.PANJENGR4='1' OR b.PANJENGR5='1'                                          ");
            parameter.AppendSql("    OR b.PANJENGR6='1' OR b.PANJENGR7='1' OR b.PANJENGR8='1'                       ");
            parameter.AppendSql("    OR b.PANJENGR9='1' OR b.PANJENGR10='1' OR b.PANJENGR11='1' )                   ");
            parameter.AppendSql("   AND b.PANJENGDRNO > 0                                                           ");
            parameter.AppendSql("   AND a.WRTNO IN (SELECT a.WRTNO                                                  ");
            parameter.AppendSql("                     FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_RES_SPECIAL b   ");
            parameter.AppendSql("                    WHERE a.WRTNO = b.WRTNO(+)                                     ");
            parameter.AppendSql("                      AND a.JepDate >= TO_DATE(:FRDATE,'YYYY-MM-DD')               ");
            parameter.AppendSql("                      AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("                      AND a.DelDate IS NULL                                        ");
            parameter.AppendSql("                      AND a.LTDCODE = :LTDCODE                                     ");
            parameter.AppendSql("                      AND a.UCodes IS NOT NULL                                     "); //취급물질이 있는 사람만
            parameter.AppendSql("                      AND a.GJYEAR = :GJYEAR                                       ");
            parameter.AppendSql("   AND a.GJBANGI = :GJBANGI                                                        ");
            if (strJob == "1") //특수
            {
                parameter.AppendSql("                  AND a.Gjjong IN ('11','12','14','23')                        ");
            }
            else if (strJob == "2") //채용
            {
                parameter.AppendSql("                  AND a.Gjjong IN ('22','24','30')                             ");
            }
            else
            {
                parameter.AppendSql("                  AND a.Gjjong  IN ('69')                                      ");
            }
            parameter.AppendSql("                       )                                                           ");
            parameter.AppendSql("   GROUP BY a.PANO                                                                 ");

            parameter.Add("TODATE", fstrDate1);
            parameter.Add("FRDATE", fstrDate2);
            parameter.Add("LTDCODE", ltdCode);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GJBANGI", strBangi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<COMHPC> GetXrayDetailPacsNobySDate(string text, string strSDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PACSNO                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL                         ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND XCODE IN ('GR2101A','GR2101')                   ");
            parameter.AppendSql("   AND SEEKDATE >= TO_DATE(:SEEKDATE, 'YYYY-MM-DD')    ");

            parameter.Add("PANO", text);
            parameter.Add("SEEKDATE", strSDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetJepNamebySuCode(string argSuCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JEPNAME                                     ");
            parameter.AppendSql("  FROM KOSMOS_ADM.DRUG_JEP                         ");
            parameter.AppendSql(" WHERE JEPCODE IN (                                ");
            parameter.AppendSql("                   SELECT JEPCODE                  ");
            parameter.AppendSql("                     FROM KOSMOS_ADM.DRUG_CONVRATE ");
            parameter.AppendSql("                    WHERE SUNEXT = :SUNEXT)        ");

            parameter.Add("SUNEXT", argSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int DeletebyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_MAILSEND    ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetHicResPftbyWrtNo(long fnWRTNO, long fnWrtno2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FVC_PRED,FEV10_PRED,FEV1_FVC_MEAS           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_PFT                     ");
            if (fnWRTNO > 0)
            {
                parameter.AppendSql(" WHERE WRTNO IN (:WRTNO, :WRTNO2)              ");
            }
            else
            {
                parameter.AppendSql("WHERE WRTNO = :WRTNO                           ");
            }

            parameter.Add("WRTNO", fnWRTNO);
            if (fnWRTNO > 0)
            {
                parameter.Add("WRTNO2", fnWrtno2);
            }

            return ExecuteReader<COMHPC>(parameter);
        }

        public int UpdateHicResBohum1(long nDrNo, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_RES_BOHUM1 SET     ");
            parameter.AppendSql("        MUNJINDRNO = :MUNJINDRNO           ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                     ");

            parameter.Add("MUNJINDRNO", nDrNo);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetHeaResPftbyPtNoExDate(string strPtNo, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FVC_PRED, FEV10_PRED, FEV1_FVC_MEAS         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RES_PFT                     ");
            parameter.AppendSql(" WHERE PTNO   = :PTNO                              ");
            parameter.AppendSql("   AND EXDATE = TO_DATE(:EXDATE, 'YYYY-MM-DD')     ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXDATE", strJepDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetTypebyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT c.TYPE                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU     a                         ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_SUNAPDTL  b                         ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_GROUPCODE c                         ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                ");
            parameter.AppendSql("   AND b.CODE  = c.CODE                                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                    ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                   ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int UpdateHicXMunjin(long nDrNo, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_X_MUNJIN SET   ");
            parameter.AppendSql("        MUNDRNO = :MUNDRNO             ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                 ");

            parameter.Add("MUNDRNO", nDrNo);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteHicHyangApprove(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" DELETE KOSMOS_PMPA.HIC_HYANG_APPROVE  ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                 ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateHicSangdamNew(long nDrNo, long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HIC_SANGDAM_NEW SET    ");
            parameter.AppendSql("        SANGDAMDRNO = :SANGDAMDRNO         ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                     ");

            parameter.Add("SANGDAMDRNO", nDrNo);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int InsertBasPatientByHicPatient(BAS_PATIENT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.BAS_PATIENT (                                             ");
            parameter.AppendSql("        PANO, SNAME, SEX, JUMIN1, JUMIN2, STARTDATE, LASTDATE                      ");
            parameter.AppendSql("       ,ZIPCODE1, ZIPCODE2, JUSO, JICODE, TEL, HPHONE, EMBPRT                      ");
            parameter.AppendSql("       ,BI, PNAME, GWANGE, KIHO, GKIHO, DEPTCODE, DRCODE                           ");
            parameter.AppendSql("       ,GBSPC, GBGAMEK, BOHUN, REMARK, SABUN, BUNUP, BIRTH, GBBIRTH                ");
            parameter.AppendSql("       ,EMAIL, GBINFOR, GBJUSO, GBSMS, HPHONE2, JUMIN3, BUILDNO)                   ");
            parameter.AppendSql(" VALUES (                                                                          ");
            parameter.AppendSql("        :PANO, :SNAME, :SEX, :JUMIN1, :JUMIN2, TO_DATE(:STARTDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("       , TO_DATE(:LASTDATE, 'YYYY-MM-DD')                                          ");
            parameter.AppendSql("       ,:ZIPCODE1, :ZIPCODE2, :JUSO, :JICODE, :TEL, :HPHONE, :EMBPRT               ");
            parameter.AppendSql("       ,:BI, :PNAME, :GWANGE, :KIHO, :GKIHO, :DEPTCODE, :DRCODE                    ");
            parameter.AppendSql("       ,:GBSPC, :GBGAMEK, :BOHUN, :REMARK, :SABUN, :BUNUP, :BIRTH, :GBBIRTH        ");
            parameter.AppendSql("       ,:EMAIL, :GBINFOR, :GBJUSO, :GBSMS, :HPHONE2, :JUMIN3, :BUILDNO)            ");

            #region Query 변수대입
            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("JUMIN1", item.JUMIN1);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("STARTDATE", item.STARTDATE);
            parameter.Add("LASTDATE", item.LASTDATE);
            parameter.Add("ZIPCODE1", item.ZIPCODE1);
            parameter.Add("ZIPCODE2", item.ZIPCODE2);
            parameter.Add("JUSO", item.JUSO);
            parameter.Add("JICODE", item.JICODE);
            parameter.Add("TEL", item.TEL);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("EMBPRT", item.EMBPRT);
            parameter.Add("BI", item.BI);
            parameter.Add("PNAME", item.PNAME);
            parameter.Add("GWANGE", item.GWANGE);
            parameter.Add("KIHO", item.KIHO);
            parameter.Add("GKIHO", item.GKIHO);
            parameter.Add("DEPTCODE", item.DEPTCODE);
            parameter.Add("DRCODE", item.DRCODE);
            parameter.Add("GBSPC", item.GBSPC);
            parameter.Add("GBGAMEK", item.GBGAMEK);
            parameter.Add("BOHUN", item.BOHUN);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("SABUN", item.SABUN);
            parameter.Add("BUNUP", item.BUNUP);
            parameter.Add("BIRTH", item.BIRTH);
            parameter.Add("GBBIRTH", item.GBBIRTH);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("GBINFOR", item.GBINFOR);
            parameter.Add("GBJUSO", item.GBJUSO);
            parameter.Add("GBSMS", item.GBSMS);
            parameter.Add("HPHONE2", item.HPHONE2);
            parameter.Add("JUMIN3", item.JUMIN3);
            parameter.Add("BUILDNO", item.BUILDNO);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public long SpecNo()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_EXAMSPECNO.NEXTVAL SPECNO FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }

        /// <summary>
        /// 출장검진 검체번호 7000부터 시작
        /// </summary>
        /// <returns></returns>
        public long SpecNo_HicChul()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_EXAMSPECNO_HICCHUL.NEXTVAL SPECNO FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }

        public List<COMHPC> GetCountAutoPanLogicExamByLogic(string argJepNo, string argSeqNo, string argWrtNo, string argLogic)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.EXCODE, A.LOGIC                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_EXAM A, KOSMOS_PMPA.HEA_RESULT B  ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                             ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND A.LOGIC = :LOGIC                                                ");

            parameter.Add("JEPNO", argJepNo);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqNo);
            parameter.Add("LOGIC", argLogic);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetCountAutoPanLogicExamByJepNo(string argJepNo, string argSeqNo, string argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_EXAM A, KOSMOS_PMPA.HEA_RESULT B  ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                             ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                ");
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                       ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                ");

            parameter.Add("JEPNO", argJepNo);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public int GetExetcLogicbyWrtNo(string argWrtNo, string argSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_EXAM                                          ");
            parameter.AppendSql(" WHERE EXETC IN ('혈압약 복용 중','당뇨병 치료 중','흡연','고혈압 치료 중','음주') ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                                                              ");
            parameter.AppendSql("   AND SEQNO = :SEQNO                                                              ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqNo);

            return ExecuteScalar<int>(parameter);
        }

        public List<COMHPC> GetAutopanLogicResultbyWrtNo_forth(string argJepNo, string argWrtNo, string argSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.SEX, A.EXCODE1, A.EXCODE2, B.RETVAL1, A.CALC, C.RETVAL2, A.LOGIC, A.RESULTVALUE   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC A,                                               ");
            parameter.AppendSql(" (SELECT EXCODE1, RESULTVALUE RETVAL1                                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC A, KOSMOS_PMPA.HEA_RESULT B                      ");
            parameter.AppendSql(" WHERE a.EXCODE1 = b.EXCODE                                                                ");
            parameter.AppendSql("    AND B.WRTNO = :JEPNO                                                                   ");
            parameter.AppendSql("    AND A.SEQNO = :SEQNO                                                                   ");
            parameter.AppendSql("    AND B.RESULT NOT IN ('.','0')                                                          ");
            parameter.AppendSql("    AND A.WRTNO = :WRTNO) B,                                                               ");
            parameter.AppendSql("  (SELECT EXCODE2, RESULTVALUE RETVAL2                                                     ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC A, KOSMOS_PMPA.HEA_RESULT B                       ");
            parameter.AppendSql(" WHERE a.EXCODE2 = b.EXCODE                                                                ");
            parameter.AppendSql("    AND B.WRTNO = :JEPNO                                                                   ");
            parameter.AppendSql("    AND A.SEQNO = :SEQNO                                                                   ");
            parameter.AppendSql("    AND B.RESULT NOT IN ('.','0')                                                          ");
            parameter.AppendSql("    AND A.WRTNO = :WRTNO) C                                                                ");
            parameter.AppendSql(" WHERE a.EXCODE1 = b.EXCODE1                                                               ");
            parameter.AppendSql("    AND A.EXCODE2 = C.EXCODE2(+)                                                           ");
            parameter.AppendSql("    AND A.WRTNO = :WRTNO                                                                   ");
            parameter.AppendSql("    AND A.SEQNO = :SEQNO                                                                   ");

            parameter.Add("JEPNO", argJepNo);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetAutopanLogicResultbyWrtNo_Third(string argJepNo, string argWrtNo, string argSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.EXCODE, A.GUBUN, A.SEX, B.RESULT, LOGIC, RESULTVALUE      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC A, KOSMOS_PMPA.HEA_RESULT B   ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                            ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                         ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                            ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                            ");
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                   ");
            parameter.AppendSql("   AND A.GUBUN = '2'                                               ");

            parameter.Add("JEPNO", argJepNo);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetAutopanLogicResultbyWrtNo_Second(string argJepNo, string argWrtNo, string argSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.EXCODE, A.GUBUN, A.SEX, B.RESULT, LOGIC, RESULTVALUE              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC A, KOSMOS_PMPA.HEA_RESULT B           ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                                    ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                                 ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                    ");
            parameter.AppendSql("   AND A.GUBUN = '1'                                                       ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                                    ");
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                           ");
            parameter.AppendSql("   AND A.EXCODE IN (                                                       ");
            parameter.AppendSql("                     SELECT EXCODE                                         ");
            parameter.AppendSql("                      FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC                   ");
            parameter.AppendSql("                     WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("                        AND SEQNO = :SEQNO                                 ");
            parameter.AppendSql("                     GROUP BY EXCODE                                       ");
            parameter.AppendSql("                     HAVING SUM(1) = 2                                     ");
            parameter.AppendSql("                    )                                                      ");
            parameter.AppendSql(" ORDER BY A.EXCODE, DECODE(LOGIC, '<', 1, '<=', 2, '>', 3, '>=', 4) ASC    ");

            parameter.Add("JEPNO", argJepNo);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetAutopanLogicResultbyWrtNo(string argJepNo, string argWrtNo, string argSeqNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.EXCODE, A.GUBUN, A.SEX, B.RESULT, LOGIC, RESULTVALUE      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC A, KOSMOS_PMPA.HEA_RESULT B   ");
            parameter.AppendSql(" WHERE b.WRTNO = :JEPNO                                            ");
            parameter.AppendSql("   AND A.EXCODE = B.EXCODE                                         ");
            parameter.AppendSql("   AND A.WRTNO = :WRTNO                                            ");
            parameter.AppendSql("   AND A.GUBUN = '1'                                               ");
            parameter.AppendSql("   AND A.SEQNO = :SEQNO                                            ");
            parameter.AppendSql("   AND B.RESULT NOT IN ('.','0')                                   ");
            parameter.AppendSql("   AND A.EXCODE IN (                                               ");
            parameter.AppendSql("                     SELECT EXCODE                                 ");
            parameter.AppendSql("                      FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC           ");
            parameter.AppendSql("                     WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("                        AND SEQNO = :SEQNO                         ");
            parameter.AppendSql("                     GROUP BY EXCODE                               ");
            parameter.AppendSql("                     HAVING SUM(1) < 2 OR SUM(1) > 2               ");
            parameter.AppendSql("                    )                                              ");

            parameter.Add("JEPNO", argJepNo);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SEQNO", argSeqNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetWrtNoSeqNobyWrtNo(string strWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, SEQNO                                    ");
            parameter.AppendSql("  FROM (                                               ");
            parameter.AppendSql("        SELECT WRTNO, SEQNO                            ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC           ");
            parameter.AppendSql("         WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("         UNION ALL                                     ");
            parameter.AppendSql("        SELECT WRTNO, SEQNO                            ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC      ");
            parameter.AppendSql("         WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("         UNION ALL                                     ");
            parameter.AppendSql("        SELECT WRTNO, SEQNO                            ");
            parameter.AppendSql("          FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_EXAM      ");
            parameter.AppendSql("         WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("        )                                              ");
            parameter.AppendSql(" GROUP BY WRTNO, SEQNO                                 ");
            parameter.AppendSql(" ORDER BY WRTNO, SEQNO ASC                             ");

            parameter.Add("WRTNO", strWrtNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetSumGrpNobyWrtNo(string argJepNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MST.WRTNO, SUM(1) CNT, B.GRPNO                                                  ");
            parameter.AppendSql("  FROM (                                                                               ");
            parameter.AppendSql("         SELECT WRTNO, EXCODE                                                          ");
            parameter.AppendSql("           FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC                                          ");
            parameter.AppendSql("          UNION ALL                                                                    ");
            parameter.AppendSql("         SELECT WRTNO, EXCODE                                                          ");
            parameter.AppendSql("           FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_EXAM                                     ");
            parameter.AppendSql("          UNION ALL                                                                    ");
            parameter.AppendSql("         SELECT WRTNO, EXCODE1 EXCODE                                                  ");
            parameter.AppendSql("           FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC                                     ");
            parameter.AppendSql("          UNION ALL                                                                    ");
            parameter.AppendSql("         SELECT WRTNO, EXCODE2 EXCODE                                                  ");
            parameter.AppendSql("           FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC_CALC                                     ");
            parameter.AppendSql("       ) MST                                                                           ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_AUTOPAN B                                                       ");
            parameter.AppendSql(" WHERE EXISTS (                                                                        ");
            parameter.AppendSql("                SELECT * FROM KOSMOS_PMPA.HEA_RESULT SUB                               ");
            parameter.AppendSql("                 WHERE MST.EXCODE = SUB.EXCODE                                         ");
            parameter.AppendSql("                   AND SUB.WRTNO = :WRTNO                                              ");
            parameter.AppendSql("              )                                                                        ");
            parameter.AppendSql("   AND MST.WRTNO = B.WRTNO                                                             ");
            parameter.AppendSql(" GROUP BY MST.WRTNO, B.GRPNO                                                           ");
            parameter.AppendSql(" ORDER BY CNT DESC                                                                     ");

            parameter.Add("WRTNO", argJepNo);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<ENDO_RESULT_JUPMST> GetItembyJDate(string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.Remark1, A.Remark2, A.Remark3, A.Remark4, A.Remark5, A.Remark6, A.PicXY               ");
            parameter.AppendSql("     , b.GBPRE_1,b.GBPRE_2,b.GBPRE_21,b.GBPRE_22,b.GBPRE_3,b.GBCON_1,b.GBCON_2,b.GBCON_21      ");
            parameter.AppendSql("     , b.GBCON_22,b.GBCON_3,b.GBCON_31,b.GBCON_32,b.GBCON_4,b.GBCON_41,b.GBCON_42,b.GBPRO_1    ");
            parameter.AppendSql("     , b.GBPRO_2,b.GBPRE_31, a.Remark6_2,a.Remark6_3,a.Remark,b.Ptno,b.Gb_Clean                ");
            parameter.AppendSql("     , TO_CHAR(b.JDATE,'YYYY-MM-DD') JDATE                                                     ");
            parameter.AppendSql("     , TO_CHAR(b.RDATE,'YYYY-MM-DD HH24:MI') RDATE                                             ");
            parameter.AppendSql("     , TO_CHAR(b.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE,b.GbNew,b.GbJob,a.REMARK          ");
            parameter.AppendSql("     , b.RESULTDRCODE, b.PRO_RUT                                                               ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_RESULT a, KOSMOS_OCS.ENDO_JUPMST b                                      ");
            parameter.AppendSql(" WHERE b.JDATE = TO_DATE(:JDATE,'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND a.SEQNO = b.SEQNO                                                                       ");
            parameter.AppendSql("   AND b.DeptCode = 'TO'                                                                       "); //종합검진만
            parameter.AppendSql("   AND b.GbSunap IN ('1','7')                                                                  ");
            parameter.AppendSql("   AND b.ResultSend = 'Y'                                                                      ");

            parameter.Add("JDATE", strDate);

            return ExecuteReader<ENDO_RESULT_JUPMST>(parameter);
        }

        public List<ENDO_RESULT_JUPMST> GetItembyJDateDeptPtno(string strDate, string strPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.Remark1, A.Remark2, A.Remark3, A.Remark4, A.Remark5, A.Remark6, A.PicXY               ");
            parameter.AppendSql("     , b.GBPRE_1,b.GBPRE_2,b.GBPRE_21,b.GBPRE_22,b.GBPRE_3,b.GBCON_1,b.GBCON_2,b.GBCON_21      ");
            parameter.AppendSql("     , b.GBCON_22,b.GBCON_3,b.GBCON_31,b.GBCON_32,b.GBCON_4,b.GBCON_41,b.GBCON_42,b.GBPRO_1    ");
            parameter.AppendSql("     , b.GBPRO_2,b.GBPRE_31, a.Remark6_2,a.Remark6_3,a.Remark,b.Ptno,b.Gb_Clean                ");
            parameter.AppendSql("     , TO_CHAR(b.JDATE,'YYYY-MM-DD') JDATE                                                     ");
            parameter.AppendSql("     , TO_CHAR(b.RDATE,'YYYY-MM-DD HH24:MI') RDATE                                             ");
            parameter.AppendSql("     , TO_CHAR(b.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE,b.GbNew,b.GbJob,a.REMARK          ");
            parameter.AppendSql("     , b.RESULTDRCODE, b.PRO_RUT                                                               ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_RESULT a, KOSMOS_OCS.ENDO_JUPMST b                                      ");
            parameter.AppendSql(" WHERE b.JDATE = TO_DATE(:JDATE,'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND a.SEQNO = b.SEQNO                                                                       ");
            parameter.AppendSql("   AND b.DEPTCODE IN ('HR','TO')                                                               ");
            parameter.AppendSql("   AND b.PTNO = :PTNO                                                                          ");
            parameter.AppendSql("   AND b.GbSunap IN ('1','7')                                                                  ");
            parameter.AppendSql("   AND b.ResultSend = 'Y'                                                                      ");

            parameter.Add("JDATE", strDate);
            parameter.Add("PTNO", strPtno);

            return ExecuteReader<ENDO_RESULT_JUPMST>(parameter);
        }

        public int DeletebyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" DELETE KOSMOS_PMPA.HEA_MEMO   ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertHeaMemo(long fnWRTNO, string strMemo, string idNumber, string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HEA_MEMO (                ");
            parameter.AppendSql("        WRTNO,MEMO,ENTTIME,JOBSABUN,PTNO)          ");
            parameter.AppendSql(" VALUES (                                          ");
            parameter.AppendSql("        :WRTNO, :MEMO, SYSDATE, :JOBSABUN, :PTNO)  ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("MEMO", strMemo);
            parameter.Add("JOBSABUN", idNumber);
            parameter.Add("PTNO", argPtno);

            return ExecuteNonQuery(parameter);
        }

        public COMHPC GetJusobyWrtnoPtnoSname(long nWRTNO, string strPtNo, string strSname, string strDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUSO1, JUSO2                    ");
            if (strDept == "HR")
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU       ");
                parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PATIENT     ");
                parameter.AppendSql(" WHERE PTNO = :PTNO                ");
            }
            parameter.AppendSql("   AND SNAME = :SNAME                  ");

            if (nWRTNO != 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }

            if (nWRTNO != 0)
            {
                parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (nWRTNO != 0)
            {
                parameter.Add("SNAME", strSname);
            }

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int GetFlagbyXrayNo(string strXrayno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X')                      ");
            parameter.AppendSql("  FROM PACS.XRAY_PACS_ORDER            ");
            parameter.AppendSql(" WHERE ACDESSIONNO = :ACDESSIONNO      ");
            parameter.AppendSql("   AND FALG <> 'Y'                     ");

            parameter.Add("ACDESSIONNO", strXrayno);

            return ExecuteScalar<int>(parameter);
        }

        public long GetSeqNo(string argOracleUser, string argSEQ_Name)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT " + argOracleUser + "." + argSEQ_Name + ".NEXTVAL WRTNO_SEQ FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }

        public int GetEtcJupMstbyPaNo(string fstrPano, string fstrBDate, string fstrDeptCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST                           ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                    ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')           ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                            ");
            parameter.AppendSql("   AND ORDERCODE = 'STRESS'                            ");
            parameter.AppendSql("   AND GUBUN='18'                                      ");

            parameter.Add("PTNO", fstrPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", fstrBDate);
            parameter.Add("DEPTCODE", fstrDeptCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public string GetMunjinResByIEMunNo(long nMunjinNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_IE_MUNJIN_NEW   ");
            parameter.AppendSql(" WHERE WRTNO = :IEMUNNO                ");

            parameter.Add("IEMUNNO", nMunjinNo);

            return ExecuteScalar<string>(parameter);
        }

        /// <summary>
        /// 검진종류 이름 가져오기
        /// </summary>
        /// <param name="argGB"></param>
        /// <param name="gJJONG"></param>
        /// <returns></returns>
        public string GetJongNameByCode(string argGB, string gJJONG)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME  ");
            if (argGB.Equals("HIC"))
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG                   ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXJONG                   ");
            }
            parameter.AppendSql(" WHERE 1 = 1                         ");
            parameter.AppendSql("   AND CODE = :CODE                            ");

            parameter.Add("CODE", gJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        /// <summary>
        /// 종합검진 횟수
        /// </summary>
        /// <param name="argPtno"></param>
        /// <returns></returns>
        public string GetCountHeaByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(PANO) CNT  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                         ");
            parameter.AppendSql("   AND PTNO = :PTNO                            ");
            parameter.AppendSql("   AND GBSTS != '0'                            ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        /// <summary>
        /// 최종검진일자(종검)
        /// </summary>
        /// <param name="argPtno"></param>
        /// <returns></returns>
        public string GetLastDay3ByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(TO_CHAR(SDATE,'YYYY-MM-DD')) JDATE  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                   ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                         ");
            parameter.AppendSql("   AND PTNO = :PTNO                            ");
            parameter.AppendSql("   AND GBSTS != '0'                            ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        /// <summary>
        /// 최종검진일자(암)
        /// </summary>
        /// <param name="argPtno"></param>
        /// <returns></returns>
        public string GetLastDay2ByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(TO_CHAR(JEPDATE,'YYYY-MM-DD')) JDATE    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GJJONG = '31'                               ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        /// <summary>
        /// 최종검진일자(일반)
        /// </summary>
        /// <param name="argPtno"></param>
        /// <returns></returns>
        public string GetLastDay1ByPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(TO_CHAR(JEPDATE,'YYYY-MM-DD')) JDATE    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE DELDATE IS NULL                             ");
            parameter.AppendSql("   AND GJJONG <> '31'                              ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetLicencebyRoom(string strRoom)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT LICENCE                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOCTOR      ");
            parameter.AppendSql(" WHERE Reday IS NULL               ");
            parameter.AppendSql("   AND ROOM = :ROOM                ");

            parameter.Add("ROOM", strRoom, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int GetHeaSangdamWaitbyWrtNo(long nWRTNO, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM HEA_SANGDAM_WAIT            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND GUBUN IN(:GUBUN)            ");
            parameter.AppendSql("   AND GBCALL = 'Y'                ");
            parameter.AppendSql("   AND CALLTIME >= TRUNC(SYSDATE)  ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetHicResultbyWrtNo(long nWrtNo, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND ExCode IN(:EXCODE)          ");
            parameter.AppendSql("   AND Result IS NOT NULL          ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetEtcJupMstbyPaNo(string strPtno, string strOrderCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ETC_JUPMST               ");
            parameter.AppendSql(" WHERE TRUNC(BDate) = TRUNC(SYSDATE)       ");
            parameter.AppendSql("   AND PTNO = :PTNO                        ");
            parameter.AppendSql("   AND DEPTCODE IN('TO', 'HR')             ");
            parameter.AppendSql("   AND GBJOB = '3'                         ");
            parameter.AppendSql("   AND ORDERCODE IN(:ORDERCODE)            ");

            parameter.Add("PTNO", strPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ORDERCODE", strOrderCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetEndobyPaNo(string strPtno, string strGbJob)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST              ");
            parameter.AppendSql(" WHERE TRUNC(RDate) = TRUNC(SYSDATE)       ");
            parameter.AppendSql("   AND PTNO = :PTNO                        ");
            parameter.AppendSql("   AND DeptCode IN('TO', 'HR')             ");
            parameter.AppendSql("   AND ResultDate IS NOT NULL              ");
            parameter.AppendSql("   AND GBJOB IN(:GBJOB)                    "); //위,대장구분
            parameter.AppendSql("   AND GbSunap IN('1', '7')                ");

            parameter.Add("PTNO", strPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBJOB", strGbJob, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetXRaybyPaNo(string strPtno, string strXCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL             ");
            parameter.AppendSql(" WHERE PANO = :PANO                        ");
            parameter.AppendSql("   AND TRUNC(SeekDate) = TRUNC(SYSDATE)    ");
            parameter.AppendSql("   AND DEPTCODE IN('TO', 'HR')             ");
            parameter.AppendSql("   AND PACSNO IS NOT NULL                  ");
            parameter.AppendSql("   AND XSENDDATE IS NOT NULL               ");
            parameter.AppendSql("   AND XCODE IN(:XCODE)                    ");

            parameter.Add("PANO", strPtno);
            parameter.Add("XCODE", strXCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetSpecNobyPtNo(string strPtno, string strMasterCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.SpecNo                        ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_SPECMST a       ");
            parameter.AppendSql("     , KOSMOS_OCS.EXAM_RESULTC b       ");
            parameter.AppendSql(" WHERE a.BDate = TRUNC(SYSDATE)        ");
            parameter.AppendSql("   AND a.Pano = :PANO                  ");
            parameter.AppendSql("   AND a.DeptCode IN('TO', 'HR')       ");
            parameter.AppendSql("   AND a.ReceiveDate IS NOT NULL       ");
            parameter.AppendSql("   AND a.Cancel IS NULL                ");
            parameter.AppendSql("   AND a.SpecNo = b.SpecNo(+)          ");
            parameter.AppendSql("   AND b.MASTERCODE IN(:MASTERCODE)    ");

            parameter.Add("PANO", strPtno);
            parameter.Add("MASTERCODE", strMasterCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int InsertHicPatient(HIC_PATIENT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_PATIENT (                                     ");
            parameter.AppendSql("        PANO,SNAME,SEX,JUMIN,JUMIN2,MAILCODE,JUSO1,JUSO2,TEL,HPHONE,EMAIL  ");
            parameter.AppendSql("       ,GKIHO,BUILDNO,PTNO                                                 ");
            parameter.AppendSql("       ,LTDCODE                                                            ");
            parameter.AppendSql(" ) VALUES (                                                                ");
            parameter.AppendSql("        :PANO,:SNAME,:SEX,:JUMIN,:JUMIN2,:MAILCODE,:JUSO1,:JUSO2,:TEL      ");
            parameter.AppendSql("       ,:HPHONE,:EMAIL,:GKIHO,:BUILDNO,:PTNO,:LTDCODE                  )   ");

            #region Query 변수대입
            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("MAILCODE", item.MAILCODE);
            parameter.Add("JUSO1", item.JUSO1);
            parameter.Add("JUSO2", item.JUSO2);
            parameter.Add("TEL", item.TEL);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("GKIHO", item.GKIHO);
            parameter.Add("BUILDNO", item.BUILDNO);
            parameter.Add("PTNO", item.PTNO);
            parameter.Add("LTDCODE", item.LTDCODE);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public long Seq_GetHicPatientNo()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KOSMOS_PMPA.SEQ_HICPANO.NEXTVAL HICPANO FROM DUAL ");

            return ExecuteScalar<long>(parameter);
        }

        public string Seq_GetBasPatientNo()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEQ_PANO.NEXTVAL NEXTVAL FROM DUAL ");

            return ExecuteScalar<string>(parameter);
        }

        public int InsertEndoChart(string argPTNO, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_OCS.ENDO_CHART ( PTNO, BDATE, RDATE  ) ");
            parameter.AppendSql(" VALUES (  ");
            parameter.AppendSql("          :PTNO, TO_DATE(:BDATE, 'YYYY-MM-DD'), SYSDATE ) ");

            #region Query 변수대입
            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argDate);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public IList<Dictionary<string, object>> GetItemHicResPft(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FVC_PRED,FEV10_PRED,FEV1_FVC_MEAS       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_PFT                 ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                          ");

            #region Query 변수대입
            parameter.Add("WRTNO", argWRTNO);
            #endregion

            return ExecuteReader<Dictionary<string, object>>(parameter);
        }

        public Dictionary<string, object> GetOneEndoMstByPtno(string argPtno, string argBDate, string argRDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBCON_1, GBCON_2, GBCON_21, GBCON_22,GBCON_3, GBCON_31  ");
            parameter.AppendSql("      ,GBCON_32,GBCON_4, GBCON_41, GBCON_42                    ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST                                  ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:RDATE, 'YYYY-MM-DD') - 0.99999        ");
            parameter.AppendSql("   AND RDATE <= TO_DATE(:RDATE, 'YYYY-MM-DD') + 0.99999        ");
            parameter.AppendSql("   AND GBSUNAP <> '*'                                          ");
            parameter.AppendSql("   AND (GBCON_2 ='Y' OR GBCON_3='Y' OR GBCON_4 ='Y' )          ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argBDate);
            parameter.Add("RDATE", argRDate);

            return ExecuteReaderSingle(parameter);
        }

        public IList<Dictionary<string, object>> GetListEndoMstByPtno(string argPtno, string argBDate, string argRDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBCON_1, GBCON_2, GBCON_21, GBCON_22,GBCON_3, GBCON_31  ");
            parameter.AppendSql("      ,GBCON_32,GBCON_4, GBCON_41, GBCON_42                    ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_JUPMST                                  ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                            ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                   ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:RDATE, 'YYYY-MM-DD') - 0.99999        ");
            parameter.AppendSql("   AND RDATE <= TO_DATE(:RDATE, 'YYYY-MM-DD') + 0.99999        ");
            parameter.AppendSql("   AND GBSUNAP <> '*'                                          ");
            parameter.AppendSql("   AND (GBCON_2 ='Y' OR GBCON_3='Y' OR GBCON_4 ='Y' )          ");

            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argBDate);
            parameter.Add("RDATE", argRDate);

            return ExecuteReader<Dictionary<string, object>>(parameter);
        }

        public int InsertSpcPanhisBySelSpcPan(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_SPC_PANHIS (                                                          ");
            parameter.AppendSql("        WRTNO,JEPDATE,LTDCODE,PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE                      ");
            parameter.AppendSql("       ,JOCHICODE, WORKYN, SAHUCODE, MSYMCODE, MBUN, UCODE, SOGENREMARK, JOCHIREMARK, REEXAM       ");
            parameter.AppendSql("       ,ENTSABUN, ENTTIME, DELDATE, ORCODE, ORSAYUCODE, SAHUREMARK, GJCHASU, LTDCODE2, PYOJANGGI ) ");
            parameter.AppendSql(" SELECT WRTNO,JEPDATE,LTDCODE,PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE                      ");
            parameter.AppendSql("       ,JOCHICODE, WORKYN, SAHUCODE, MSYMCODE, MBUN, UCODE, SOGENREMARK, JOCHIREMARK, REEXAM       ");
            parameter.AppendSql("       ,ENTSABUN, ENTTIME, DELDATE, ORCODE, ORSAYUCODE, SAHUREMARK, GJCHASU, LTDCODE2, PYOJANGGI   ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                                                ");
            parameter.AppendSql("  WHERE 1 = 1                                                                                      ");
            parameter.AppendSql("    AND WRTNO = :WRTNO                                                                             ");
            parameter.AppendSql("    AND GJCHASU = '1'                                                                              ");
            parameter.AppendSql("    AND PANJENG = '7'                                                                              ");
            parameter.AppendSql("    AND (DELDATE IS NULL OR DELDATE = '')                                                          ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteSpcPanHis(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" DELETE KOSMOS_PMPA.HIC_SPC_PANHIS WHERE WRTNO = :WRTNO ");

            #region Query 변수대입
            parameter.Add("WRTNO", argWRTNO);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public string GetRowidEndoChartByPtno(string argPTNO, string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                               ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_CHART                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE,'YYYY-MM-DD')    ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argDate);

            return ExecuteScalar<string>(parameter);
        }

        public int InsertOpdMasterByHicJepsu(COMHPC item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.OPD_MASTER (                                  ");
            parameter.AppendSql("        ActDate, Pano, DeptCode, Bi, Sname, Sex,Age, JiCode, DrCode,   ");
            parameter.AppendSql("        Reserved, Chojae, GbGamek,GbSpc, Jin, Singu,Part,Jtime,BDate,  ");
            parameter.AppendSql("        EMR,GbUse,MksJin                                               ");
            parameter.AppendSql(") VALUES (                                                             ");
            parameter.AppendSql("       TO_DATE(:ActDate,'YYYY-MM-DD'),:Pano,:DeptCode,:Bi,:Sname,:Sex,:Age,:JiCode,:DrCode   ");
            parameter.AppendSql("      ,:Reserved,:Chojae,:GbGamek,:GbSpc,:Jin,:Singu,:Part,SYSDATE     ");
            parameter.AppendSql("      ,TO_DATE(:BDate,'YYYY-MM-DD'),:EMR,:GbUse,:MksJin )                                    ");

            #region Query 변수대입
            parameter.Add("ActDate", item.JEPDATE);
            parameter.Add("Pano", item.PTNO);
            parameter.Add("DeptCode", item.DEPTCODE);
            parameter.Add("Bi", "51");
            parameter.Add("Sname", item.SNAME);
            parameter.Add("Sex", item.SEX);
            parameter.Add("Age", item.AGE);
            parameter.Add("JiCode", "");
            parameter.Add("DrCode", item.DRCODE);
            parameter.Add("Reserved", "0");
            parameter.Add("Chojae", "1");
            parameter.Add("GbGamek", "00");
            parameter.Add("GbSpc", "0");
            parameter.Add("Jin", "D");
            parameter.Add("Singu", "0");
            parameter.Add("Part", item.JOBSABUN.To<string>("111"));
            parameter.Add("BDate", item.JEPDATE);
            parameter.Add("EMR", "0");
            parameter.Add("GbUse", "Y");
            parameter.Add("MksJin", "D");
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public string ChkOpdMaster(string argPTNO, string argDate, string argDept)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.OPD_MASTER                  ");
            parameter.AppendSql(" WHERE PANO  = :PANO                           ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   ");
            if (argDept != "")
            {
                parameter.AppendSql("   AND DEPTCODE = :DEPTCODE              ");
            }

            parameter.Add("PANO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argDate);
            if (argDept != "")
            {
                parameter.Add("DEPTCODE", argDept);
            }

            return ExecuteScalar<string>(parameter);
        }

        public int InsertByWrtno(long argWRTNO, string argTable)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA." + argTable + " ( WRTNO ) VALUES ( :WRTNO) ");

            #region Query 변수대입
            parameter.Add("WRTNO", argWRTNO);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public string GetTableRowidByWrtno(long argWRTNO, string argTable)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA." + argTable + "    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRowIdbyXrayNo(string argXrayNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT     ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO                ");
            parameter.AppendSql("   AND EXID IS NULL                    ");

            parameter.Add("XRAYNO", argXrayNo);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Exid_Name(string argXrayNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.NAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_XRAY_RESULT A   ");
            parameter.AppendSql("     , KOSMOS_PMPA.BAS_BCODE       B   ");
            parameter.AppendSql(" WHERE XRAYNO = :XRAYNO                ");
            parameter.AppendSql("   AND B.GUBUN = 'HIC_촬영기사'        ");
            parameter.AppendSql("   AND A.EXID = B.CODE                 ");

            parameter.Add("XRAYNO", argXrayNo);

            return ExecuteScalar<string>(parameter);
        }

        public COMHPC Read_Ocs_Doctor_All(long argSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRNAME, DRBUNHO, DRCODE ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", argSabun);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public string Read_WebPrt_Log(string strSendNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_WEBPRT_LOG      ");
            parameter.AppendSql(" WHERE SENDNO = :SENDNO                ");
            parameter.AppendSql("   AND SmsTime>= TRUNC(SYSDATE - 100)  ");

            parameter.Add("SENDNO", strSendNo);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_HolyDay(string argDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT HOLYDAY                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_JOB                         ");
            parameter.AppendSql(" WHERE JOBDATE = TO_DATE(:JOBDATE, 'yyyy-MM-dd')   ");

            parameter.Add("JOBDATE", argDate);

            return ExecuteScalar<string>(parameter);
        }

        public string ReadHicDoctor(long argSABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRNAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE SABUN =:SABUN           ");

            parameter.Add("SABUN", argSABUN);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Hic_Doctor_Name(string argSABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT KORNAME                 ");
            parameter.AppendSql("  FROM KOSMOS_ADM.INSA_MST     ");
            parameter.AppendSql(" WHERE SABUN =:SABUN           ");

            parameter.Add("SABUN", argSABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Hic_Doctor_Name_byLicence(string argLicence)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRNAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_DOCTOR  ");
            parameter.AppendSql(" WHERE LICENCE = :LICENCE      ");
            parameter.AppendSql("   AND REDAY IS NULL           ");

            parameter.Add("LICENCE", argLicence);

            return ExecuteScalar<string>(parameter);
        }

        public Dictionary<string, object> SelMunjin(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT T66_INJECT, TMUN0013, T66_FALL                                          ");
            parameter.AppendSql("      ,T_STAT21,T_STAT22,T_STAT31,T_STAT32,T_STAT41,T_STAT42,T_STAT61,T_STAT62 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RES_BOHUM1                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                                                          ");

            #region Query 변수대입
            parameter.Add("WRTNO", argWRTNO);
            #endregion
            return ExecuteReaderSingle(parameter);
        }

        public string Read_DrCode(string SABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", SABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_GunTae(string SABUN, string YEAR)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUNTAE                  ");
            parameter.AppendSql("  FROM KOSMOS_ADM.INSA_MSTH    ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");
            parameter.AppendSql("   AND YEAR  = :YEAR           ");

            parameter.Add("SABUN", SABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("YEAR", YEAR, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_ToisaDay(string SABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(ToiDay,'YYYY-MM-DD') ToiDay     ");
            parameter.AppendSql("  FROM KOSMOS_ADM.INSA_MST                     ");
            parameter.AppendSql(" WHERE SABUN = :SABUN                          ");

            parameter.Add("SABUN", SABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_JisaName(string JISA)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Name                ");
            // ADM 에 HIC_CODE 테이블 존재하지 않음으로 KOSMOS_PMPA로 변경 / 2021.06.21 심명섭
            //parameter.AppendSql("  FROM KOSMOS_ADM.HIC_CODE ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_CODE ");
            parameter.AppendSql(" WHERE GUBUN = '21'        ");
            parameter.AppendSql("   AND CODE  = :CODE       ");

            parameter.Add("CODE", JISA, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string SunapDtlChk(long argWRTNO, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND CODE = :CODE                ");
            parameter.AppendSql("   AND GBSELF IN ('01', '1')       ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public IDictionary<string, object> ReadJepMunjinSatus(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.GjJong,a.GbMunjin1,a.GbMunjin2,a.GbMunjin3    ");
            parameter.AppendSql("      ,a.UCodes,a.GbDental,b.GbMunjin                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                         ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXJONG b                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                ");
            parameter.AppendSql("   AND a.GJJONG =b.CODE(+)                             ");

            #region Query 변수대입
            parameter.Add("WRTNO", argWRTNO);
            #endregion
            return ExecuteReaderSingle(parameter);
        }

        public int Date_Count_HoliDay(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(JOBDATE) AS CNT FROM KOSMOS_PMPA.BAS_JOB  ");
            parameter.AppendSql(" WHERE JOBDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND JOBDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND HOLYDAY = '*'                                   ");

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);

            return ExecuteScalar<int>(parameter);
        }

        public int Date_ILSU(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_DATE(:FRDATE, 'YYYY-MM-DD') -                ");
            parameter.AppendSql("       TO_DATE(:TODATE, 'YYYY-MM-DD') Gigan FROM DUAL  ");

            parameter.Add("FRDATE", argFDate);
            parameter.Add("TODATE", argTDate);

            return ExecuteScalar<int>(parameter);
        }

        public int DeleteOcsMayak(string strPtNo, string strSuCode, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_MAYAK                        ");
            parameter.AppendSql(" WHERE PTNO    = :PTNO                             ");
            if (!strSuCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SUCODE   = :SUCODE                      ");
            }
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE IN('HR', 'TO')                     ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!strSuCode.IsNullOrEmpty())
            {
                parameter.Add("SUCODE", strSuCode.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("BDATE", strDate);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteOcsOrder(string strPtNo, string strSuCode, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_OORDER                       ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            if (!strSuCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SUCODE   = :SUCODE                      ");
            }
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE IN('HR', 'TO')                     ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!strSuCode.IsNullOrEmpty())
            {
                parameter.Add("SUCODE", strSuCode.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("BDATE", strDate);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteOcsHyang(string strPtNo, string strSuCode, string strDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_HYANG                        ");
            parameter.AppendSql(" WHERE PTNO     = :PTNO                            ");
            parameter.AppendSql("   AND SUCODE   = :SUCODE                          ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND DEPTCODE IN('HR')                           ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", strSuCode.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", strDate);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetHeaExamListByItems(string argFDate, string argTDate, string argGbExam)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE, WRTNO, SNAME, GJJONG,PTNO    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND SDATE >= TO_DATE(:SFDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND SDATE <= TO_DATE(:STDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");

            if (argGbExam.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GBSTS IN ('1', '2')                                     ");
                parameter.AppendSql("   AND (GBEXAM IS NULL OR GBEXAM='N')                          ");
            }
            else
            {
                parameter.AppendSql("   AND GBSTS != 'D'                                            ");
                parameter.AppendSql("   AND GBEXAM = 'Y'                                            ");
            }

            parameter.Add("SFDATE", argFDate);
            parameter.Add("STDATE", argTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetHicExamListByItems(string argFDate, string argTDate, string argGbExam, bool gbHicChul)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JEPDATE, 'YYYY-MM-DD') AS JEPDATE, WRTNO, SNAME, GJJONG             ");
            parameter.AppendSql("     , JONGGUMYN, PTNO                                                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND JEPDATE >= TO_DATE(:SFDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:STDATE, 'YYYY-MM-DD')                                   ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                             ");
            parameter.AppendSql("   AND GJJONG NOT IN ('55')                                                        ");

            if (argGbExam.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND (GBEXAM IS NULL OR GBEXAM='N')                                          ");
            }
            else
            {
                parameter.AppendSql("   AND GBEXAM = 'Y'                                                            ");
            }

            if (gbHicChul)
            {
                parameter.AppendSql("   AND GBCHUL='Y'                                                              ");
            }

            parameter.AppendSql(" ORDER BY SNAME,WRTNO                                                              ");

            parameter.Add("SFDATE", argFDate);
            parameter.Add("STDATE", argTDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public string GetHoliDayByCurrentDay(string argCurMonth)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT HOLYDAY                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_JOB                         ");
            parameter.AppendSql(" WHERE JOBDATE =TO_DATE(:JOBDATE, 'YYYY-MM-DD')    ");

            parameter.Add("JOBDATE", argCurMonth);

            return ExecuteScalar<string>(parameter);
        }

        public string MagamSelect(string argBdate, string argPtno, string argSucode)
        {
            MParameter parameter = CreateParameter();

            if (VB.Left(argSucode, 2) == "N-")
            {
                parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.OCS_MAYAK ");
            }
            else
            {
                parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.OCS_HYANG ");
            }

            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                     ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }



        public int MagamUpdate(long argQty, string argRealQty, double argEntqty, string argROWID, string argSucode)
        {
            MParameter parameter = CreateParameter();


            if (VB.Left(argSucode, 2) == "N-")
            {
                parameter.AppendSql("UPDATE KOSMOS_OCS.OCS_MAYAK SET        ");
            }
            else
            {
                parameter.AppendSql("UPDATE KOSMOS_OCS.OCS_HYANG SET        ");
            }

            parameter.AppendSql("       QTY = :QTY                      ");
            parameter.AppendSql("       ,REALQTY = :REALQTY             ");
            parameter.AppendSql("       ,ENTQTY = :ENTQTY             ");
            parameter.AppendSql(" WHERE ROWID    = :RID               ");

            parameter.Add("QTY", argQty);
            parameter.Add("REALQTY", argRealQty);
            parameter.Add("ENTQTY", argEntqty);
            parameter.Add("RID", argROWID);

            return ExecuteNonQuery(parameter);
        }



        public int MagamInsert(COMHPC item)
        {
            MParameter parameter = CreateParameter();


            if (VB.Left(item.SUCODE, 2) == "N-")
            {
                parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_MAYAK                    ");
            }
            else
            {
                parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_HYANG                    ");
            }

            parameter.AppendSql("       (PTNO, SNAME, BI, WARDCODE, BDATE, ENTDATE, DEPTCODE                        ");
            parameter.AppendSql("      , DRSABUN, IO, SUCODE, QTY, REALQTY, NAL, DOSCODE, REMARK1, REMARK2          ");
            parameter.AppendSql("      , NRSABUN, SEX, AGE, JUMIN, JUSO, ENTQTY, JUMIN3, CHASU)                     ");
            parameter.AppendSql(" SELECT PTNO, SNAME, '51', DEPTCODE, BDATE, ENTDATE, DEPTCODE                      ");
            parameter.AppendSql("      , DRSABUN, 'O', SUCODE, :QTY, :REALQTY, '1', '920299', '검사용', 'Pain'       ");
            parameter.AppendSql("      , NRSABUN, SEX, AGE, JUMIN, JUSO, ENTQTY2, JUMIN2, CHASU                     ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                              ");
            parameter.AppendSql("  WHERE ROWID = :RID                                                               ");

            parameter.Add("QTY", item.QTY);
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("RID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int OcsMayakDelDateByBdatePtnoSnameSucodes(string argBdate, string argPtno, string argSname, List<string> argSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_MAYAK       ");
            parameter.AppendSql(" WHERE BDATE    = TO_DATE(:BDATE , 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND PTNO     = :PTNO                ");
            parameter.AppendSql("   AND SNAME    = :SNAME               ");
            if (!argSucode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SUCODE NOT IN (:SUCODE)         ");
            }
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')         ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", argSname, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!argSucode.IsNullOrEmpty())
            {
                parameter.AddInStatement("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }
        public int OcsHyangDelDateByBdatePtnoSnameSucodes(string argBdate, string argPtno, string argSname, List<string> argSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_HYANG       ");
            parameter.AppendSql(" WHERE BDATE    = TO_DATE(:BDATE , 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND PTNO     = :PTNO                ");
            parameter.AppendSql("   AND SNAME    = :SNAME               ");
            if (!argSucode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SUCODE NOT IN (:SUCODE)         ");
            }
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')         ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SNAME", argSname, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!argSucode.IsNullOrEmpty())
            {
                parameter.AddInStatement("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public int DeleteOcsDrug(string argBdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_OCS.OCS_DRUG                         ");
            parameter.AppendSql(" WHERE BUILDDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql(" AND WARDCODE = 'TO'                               ");
            parameter.AppendSql(" AND GBN2 IN ('2','3')                             ");

            parameter.Add("BDATE", argBdate);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetItemByJdateWardGbn(string argJdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(MAX(JDATE), 'YYYY-MM-DD') JDATE, SUCODE             ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DRUG_SET                                     ");
            parameter.AppendSql(" WHERE JDATE <= TO_DATE(:JDATE ,'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND WARDCODE = 'TO'                                             ");
            parameter.AppendSql("   AND GBN = '2'                                                   ");
            parameter.AppendSql("   GROUP BY SUCODE                                                 ");

            parameter.Add("JDATE", argJdate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetQtySucodeOcsDrugSet(string argJdate, string argSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT QTY, SUCODE                                 ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DRUG_SET                     ");
            parameter.AppendSql(" WHERE JDATE <= TO_DATE(:JDATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql("   AND WARDCODE = 'TO'                             ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                            ");
            parameter.AppendSql("   ORDER BY JDATE DESC                             ");

            parameter.Add("JDATE", argJdate);
            parameter.Add("SUCODE", argSucode);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int InsertOcsDrug(COMHPC item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_DRUG                                                                                    ");
            parameter.AppendSql("       (BDATE,GBN,GBN2,SUCODE,WARDCODE,QTY,REALQTY,NAL,BUILDDATE)                                                  ");
            parameter.AppendSql("      VALUES(                                                                                                      ");
            parameter.AppendSql(" TO_DATE(:BDATE, 'YYYY-MM-DD'), '2', '3', :SUCODE, 'TO', :QTY, :REALQTY, '1', TO_DATE(:BUILDDATE ,'YYYY-MM-DD'))   ");

            parameter.Add("BDATE", item.BDATE);
            parameter.Add("SUCODE", item.SUCODE);
            parameter.Add("QTY", item.QTY);
            parameter.Add("REALQTY", item.REALQTY);
            parameter.Add("BUILDDATE", item.BUILDDATE);

            return ExecuteNonQuery(parameter);
        }

        public int InsertOcsDrug1(long argQty, double argEntqty, string argROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_DRUG                                                        ");
            parameter.AppendSql("       (NO1, BDATE, GBN, GBN2, PTNO, SNAME, DEPTCODE, DRSABUN, IO                      ");
            parameter.AppendSql("      , WARDCODE, ROOMCODE, SUCODE, QTY, REALQTY, NAL, DOSCODE, ORDERNO, ENTDATE       ");
            parameter.AppendSql("      , BUILDDATE,MDATE,SUCODER,ENTQTY2)                                               ");
            parameter.AppendSql(" SELECT '1', BDATE, '2', '2' , PTNO, SNAME, DEPTCODE, DRSABUN, 'O'                     ");
            parameter.AppendSql("      ,'TO', '', SUCODE, :QTY, :ENTQTY, '1','920299','',ENTDATE                        ");
            parameter.AppendSql("      , BDATE, BDATE,SUCODE, ENTQTY2                                                   ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_HYANG_APPROVE                                                  ");
            parameter.AppendSql("  WHERE ROWID = :RID                                                                   ");

            parameter.Add("QTY", argQty);
            parameter.Add("ENTQTY", argEntqty);
            parameter.Add("RID", argROWID);

            return ExecuteNonQuery(parameter);
        }
        public int InsertOcsDrug2(string argBdate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.OCS_DRUG                                                        ");
            parameter.AppendSql("       (NO1,BDATE,GBN,GBN2,WARDCODE,SUCODE,QTY,NAL                                     ");
            parameter.AppendSql("      , ENTDATE,BUILDDATE)                                                             ");
            parameter.AppendSql(" SELECT NO1, TO_DATE(:BDATE, 'YYYY-MM-DD'), '2', '1', 'TO', SUCODE, SUM(QTY*NAL), '1'  ");
            parameter.AppendSql("      , SYSDATE, TO_DATE(:BDATE, 'YYYY-MM-DD')                                         ");
            parameter.AppendSql("   FROM KOSMOS_OCS.OCS_DRUG                                                            ");
            parameter.AppendSql("  WHERE BUILDDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                      ");
            parameter.AppendSql("  AND GBN2 = '2'                                                                       ");
            parameter.AppendSql("  AND NO1 = '1'                                                                        ");
            parameter.AppendSql("  AND WARDCODE = 'TO'                                                                  ");
            parameter.AppendSql("  GROUP BY NO1, BDATE, GBN, SUCODE                                                     ");

            parameter.Add("BDATE", argBdate);

            return ExecuteNonQuery(parameter);
        }

        public List<COMHPC> GetCountOrdReq(string argFDATE, string argTDATE, List<string> argDeptcode, List<string> argJepcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(REQDATE, 'YYYYMM') REQDATE, DEPTCODE, JEPCODE, SUM(OQTY) OQTY   ");
            parameter.AppendSql("  FROM KOSMOS_ADM.ORD_REQ                                                      ");
            parameter.AppendSql(" WHERE REQDATE >= TO_DATE(:FDATE ,'YYYY-MM-DD')                                ");
            parameter.AppendSql(" AND REQDATE <= TO_DATE(:TDATE ,'YYYY-MM-DD')                                  ");
            parameter.AppendSql(" AND DEPTCODE IN (:DEPTCODE)                                                   ");
            parameter.AppendSql(" AND JEPCODE IN (:JEPCODE)                                                     ");
            parameter.AppendSql(" AND GBEND = '1'                                                               ");
            parameter.AppendSql(" GROUP BY TO_CHAR(REQDATE,'YYYYMM'), DEPTCODE, JEPCODE                         ");

            parameter.Add("FDATE", argFDATE);
            parameter.Add("TDATE", argTDATE);
            parameter.AddInStatement("DEPTCODE", argDeptcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("JEPCODE", argJepcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItembyBalDate(string argFDATE, string argTDATE, string argJong, string argJob)
        {
            MParameter parameter = CreateParameter();

            //1
            if(argJong == "*" || argJong == "1")
            {
                parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                parameter.AppendSql(" ,a.GjJong,a.LtdCode,a.UCodes,a.GbSTS,TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate          ");
                parameter.AppendSql(" ,b.PanjengDrNo, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate,b.PrtSabun,'1' GjBun            ");
                parameter.AppendSql(" FROM HIC_JEPSU A,HIC_RES_BOHUM1 b                                                             ");
                parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
                parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                  ");
                parameter.AppendSql(" AND a.DelDate IS NULL                                                                         ");
                parameter.AppendSql(" AND a.GjJong IN ('11','12','13','14','41','42','43','69')                                     ");
                parameter.AppendSql(" AND a.UCodes IS NULL                                                                          ");
                parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                        ");
                if (argJob == "2")
                {
                    //미판정
                    parameter.AppendSql(" AND b.WRTNO > 0                       ");
                    parameter.AppendSql(" AND b.PanjengDate IS NULL             ");
                }
                else if (argJob == "3")
                {
                    //미발송
                    parameter.AppendSql(" AND b.PanjengDate IS NOT NULL         ");
                    parameter.AppendSql(" AND b.TongboDate IS NULL              ");
                }
                else
                {
                    parameter.AppendSql(" AND b.PanjengDrno > 0                 ");
                    parameter.AppendSql(" AND b.TongboDate IS NOT NULL          ");
                }
            }

            if (argJong == "*" || argJong == "2")
            {
                if(!parameter.SQL.IsNullOrEmpty())
                {
                    parameter.AppendSql(" UNION ALL                                  ");
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }
                else
                {
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }

                parameter.AppendSql(" ,a.GjJong,a.LtdCode,a.UCodes,a.GbSTS,TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate          ");
                parameter.AppendSql(" ,b.PanjengDrNo, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate,b.PrtSabun,'2' GjBun            ");
                parameter.AppendSql(" FROM HIC_JEPSU A,HIC_RES_BOHUM2 b                                                             ");
                parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
                parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                   ");
                parameter.AppendSql(" AND a.DelDate IS NULL                                                                         ");
                parameter.AppendSql(" AND a.GjJong IN ('16','17','18','19','44','45','46')                                          ");
                parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                        ");

                if (argJob == "2")
                {
                    //미판정
                    parameter.AppendSql(" AND b.WRTNO > 0                       ");
                    parameter.AppendSql(" AND b.PanjengDate IS NULL             ");
                }
                else if (argJob == "3")
                {
                    //미발송
                    parameter.AppendSql(" AND b.PanjengDate IS NOT NULL         ");
                    parameter.AppendSql(" AND b.TongboDate IS NULL              ");
                }
                else
                {
                    parameter.AppendSql(" AND b.PanjengDrno > 0                 ");
                    parameter.AppendSql(" AND b.TongboDate IS NOT NULL          ");
                }


            }

            if (argJong == "*" || argJong == "3")
            {

                if (!parameter.SQL.IsNullOrEmpty())
                {
                    parameter.AppendSql(" UNION ALL                                  ");
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }
                else
                {
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }

                parameter.AppendSql(" ,a.GjJong,a.LtdCode,a.UCodes,a.GbSTS,TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate          ");
                parameter.AppendSql(" ,b.PanjengDrNo, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate,b.PrtSabun,'3' GjBun            ");
                parameter.AppendSql(" FROM HIC_JEPSU A, HIC_RES_SPECIAL b                                                           ");
                parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
                parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                  ");
                parameter.AppendSql(" AND a.DelDate IS NULL                                                                         ");
                parameter.AppendSql(" AND a.GJJONG IN ('11','12','14','16','17','19','23','28','41','42','44','45')                 ");
                parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                        ");

                if (argJob == "2")
                {
                    //미판정
                    parameter.AppendSql(" AND b.WRTNO > 0                       ");
                    parameter.AppendSql(" AND b.PanjengDate IS NULL             ");
                }
                else if (argJob == "3")
                {
                    //미발송
                    parameter.AppendSql(" AND b.PanjengDate IS NOT NULL         ");
                    parameter.AppendSql(" AND b.TongboDate IS NULL              ");
                }
                else
                {
                    parameter.AppendSql(" AND b.PanjengDrno > 0                 ");
                    parameter.AppendSql(" AND b.TongboDate IS NOT NULL          ");
                }

            }

            if (argJong == "*" || argJong == "4")
            {
                if (!parameter.SQL.IsNullOrEmpty())
                {
                    parameter.AppendSql(" UNION ALL                                  ");
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }
                else
                {
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }

                parameter.AppendSql(" ,a.GjJong,a.LtdCode,a.UCodes,a.GbSTS,TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate          ");
                parameter.AppendSql(" ,b.PanjengDrNo, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate, 0 PrtSabun,'4' GjBun           ");
                parameter.AppendSql(" FROM HIC_JEPSU A, HIC_RES_DENTAL b                                                            ");
                parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
                parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                   ");
                parameter.AppendSql(" AND a.DelDate IS NULL                                                                         ");
                parameter.AppendSql(" AND a.GjJong IN ('11','12','13','14','41','42','43','69')                                     ");
                parameter.AppendSql(" AND a.GbDental='Y                                                                             ");
                parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                        ");

                if (argJob == "2")
                {
                    //미판정
                    parameter.AppendSql(" AND b.WRTNO > 0                       ");
                    parameter.AppendSql(" AND b.PanjengDate IS NULL             ");
                }
                else if (argJob == "3")
                {
                    //미발송
                    parameter.AppendSql(" AND b.PanjengDate IS NOT NULL         ");
                    parameter.AppendSql(" AND b.TongboDate IS NULL              ");
                }
                else
                {
                    parameter.AppendSql(" AND b.PanjengDrno > 0                 ");
                    parameter.AppendSql(" AND b.TongboDate IS NOT NULL          ");
                }

            }


            if (argJong == "*" || argJong == "5")
            {

                if (!parameter.SQL.IsNullOrEmpty())
                {
                    parameter.AppendSql(" UNION ALL                                  ");
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }
                else
                {
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }

                parameter.AppendSql(" ,a.GjJong,a.LtdCode,a.UCodes,a.GbSTS,TO_CHAR(a.PanjengDate,'YYYY-MM-DD') PanjengDate          ");
                parameter.AppendSql(" ,a.PanjengDrNo, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate,a.PrtSabun,'5' GjBun            ");
                parameter.AppendSql(" FROM HIC_JEPSU A, HIC_CANCER_NEW b                                                            ");
                parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
                parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                  ");
                parameter.AppendSql(" AND a.DelDate IS NULL                                                                         ");
                parameter.AppendSql(" AND a.GjJong IN ('31','35')                                                                   ");
                parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                        ");

                if (argJob == "2")
                {
                    //미판정
                    parameter.AppendSql(" AND a.WRTNO > 0                       ");
                    parameter.AppendSql(" AND b.PanjengDate IS NULL             ");
                }
                else if (argJob == "3")
                {
                    //미발송
                    parameter.AppendSql(" AND a.PanjengDate IS NOT NULL         ");
                    parameter.AppendSql(" AND b.TongboDate IS NULL              ");
                }
                else
                {
                    parameter.AppendSql(" AND a.PanjengDrno > 0                 ");
                    parameter.AppendSql(" AND b.TongboDate IS NOT NULL          ");
                }

            }



            if (argJong == "*" || argJong == "6")
            {

                if (!parameter.SQL.IsNullOrEmpty())
                {
                    parameter.AppendSql(" UNION ALL                                  ");
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }
                else
                {
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }

                parameter.AppendSql(" ,a.GjJong,a.LtdCode,a.UCodes,a.GbSTS,TO_CHAR(b.RDATE,'YYYY-MM-DD') PanjengDate                ");
                parameter.AppendSql(" ,PPanDrno PanjengDrNo, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate, 0 PrtSabun,'6' GjBun           ");
                parameter.AppendSql(" FROM HIC_JEPSU A, HIC_SCHOOL_NEW b                                                            ");
                parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
                parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                   ");
                parameter.AppendSql(" AND a.DelDate IS NULL                                                                         ");
                parameter.AppendSql(" AND a.GjJong IN ('56')                                                                        ");
                parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                        ");

                if (argJob == "2")
                {
                    //미판정
                    parameter.AppendSql(" AND b.WRTNO > 0                           ");
                    parameter.AppendSql(" AND (b.PPanDrno=0 OR b.PPanDrno IS NULL)  ");
                }
                else if (argJob == "3")
                {
                    //미발송
                    parameter.AppendSql(" AND b.PPanDrno > 0                        ");
                    parameter.AppendSql(" AND b.TongboDate IS NULL                  ");
                }
                else
                {
                    parameter.AppendSql(" AND b.PPanDrno > 0                        ");
                    parameter.AppendSql("AND b.TongboDate IS NOT NULL               ");
                }

            }


            if (argJong == "*" || argJong == "7")
            {

                if (!parameter.SQL.IsNullOrEmpty())
                {
                    parameter.AppendSql(" UNION ALL                                  ");
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }
                else
                {
                    parameter.AppendSql(" SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.SName,a.Sex,a.Age,a.WRTNO                    ");
                }

                parameter.AppendSql(" ,a.GjJong,a.LtdCode,a.UCodes,a.GbSTS,TO_CHAR(b.PanjengDate,'YYYY-MM-DD') PanjengDate          ");
                parameter.AppendSql(" ,b.PanjengDrNo, TO_CHAR(b.TongboDate,'YYYY-MM-DD') TongboDate,b.PrtSabun,'7' GJBUN            ");
                parameter.AppendSql(" FROM HIC_JEPSU A, HIC_RES_ETC b                                                            ");
                parameter.AppendSql(" WHERE a.JepDate>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
                parameter.AppendSql(" AND a.JepDate<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                  ");
                parameter.AppendSql(" AND a.DelDate IS NULL                                                                         ");
                parameter.AppendSql(" AND a.GjJong NOT IN ('11','12','13','14','16','17','18','19','23','28','31','35','41','42','43','44','45','46','56','69') ");
                parameter.AppendSql(" AND a.UCodes IS NULL                                                                          ");                parameter.AppendSql(" AND a.WRTNO=b.WRTNO(+)                                                                        ");

                if (argJob == "2")
                {
                    //미판정
                    parameter.AppendSql(" AND b.WRTNO > 0                       ");
                    parameter.AppendSql(" AND b.PanjengDate IS NULL             ");
                }
                else if (argJob == "3")
                {
                    //미발송
                    parameter.AppendSql(" AND b.PanjengDate IS NOT NULL         ");
                    parameter.AppendSql(" AND b.TongboDate IS NULL              ");
                }
                else
                {
                    parameter.AppendSql(" AND b.PanjengDrno > 0                 ");
                    parameter.AppendSql(" AND b.TongboDate IS NOT NULL          ");
                }

            }

            parameter.AppendSql(" ORDER BY 1,2,5                                ");

            parameter.Add("FDATE", argFDATE);
            parameter.Add("TDATE", argTDATE);



            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItemByBDate(string argBDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.PTNO, A.SNAME, A.DEPTCODE, A.DRSABUN, A.SUCODE, B.ORDERNO, A.ROWID AS RID     ");
            parameter.AppendSql(" FROM KOSMOS_OCS.OCS_HYANG a, KOSMOS_OCS.OCS_OORDER B WHERE                            ");
            parameter.AppendSql(" A.DEPTCODE IN ('HR','TO')                                                             ");
            parameter.AppendSql(" AND A.BDATE >= TO_DATE(:BDATE,'YYYY-MM-DD')                                           ");
            parameter.AppendSql(" AND A.ORDERNO IS NULL                                                                 ");
            parameter.AppendSql(" AND A.PTNO = B.PTNO                                                                   ");
            parameter.AppendSql(" AND A.BDATE = B.BDATE                                                                 ");
            parameter.AppendSql(" AND TRIM(A.SUCODE) = TRIM(B.SUCODE)                                                   ");
            parameter.AppendSql(" ORDER BY A.BDATE, A.PTNO                                                              ");

            parameter.Add("BDATE", argBDATE);

            return ExecuteReader<COMHPC>(parameter);
        }


        public int UpdateHyangOrderNo(string argOrderNo, string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE UPDATE KOSMOS_OCS.OCS_HYANG SET     ");
            parameter.AppendSql("       ORDERNO = :ORDERNO                  ");
            parameter.AppendSql(" WHERE ROWID    = :RID                     ");

            parameter.Add("ORDERNO", argOrderNo);
            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }
        public List<COMHPC> GetPoscoCount(string argBDate)
        {
            MParameter parameter = CreateParameter();


            parameter.AppendSql(" SELECT PANO AS PTNO, SNAME, EXAMRES1, EXAMRES2, EXAMRES3, EXAMRES7, HIC           ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.BAS_PATIENT_POSCO                                        ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql(" AND(TRUNC(EXAMRES1) = TO_DATE(:BDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql(" OR TRUNC(EXAMRES2) = TO_DATE(:BDATE, 'YYYY-MM-DD')                        ");
            parameter.AppendSql(" OR TRUNC(EXAMRES3) = TO_DATE(:BDATE, 'YYYY-MM-DD')                        ");
            parameter.AppendSql(" OR TRUNC(EXAMRES7) = TO_DATE(:BDATE, 'YYYY-MM-DD'))                       ");

            parameter.Add("BDATE", argBDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC GetResultByPtnoBdate(string argPtno, string argFDate, string argTDate, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,A.DEPTCODE,A.RESULT1,A.RESULT2         ");
            parameter.AppendSql(" FROM KOSMOS_OCS.EXAM_ANATMST A, KOSMOS_OCS.EXAM_SPECMST C                         ");
            parameter.AppendSql(" WHERE A.PTNO = :PTNO                                                              ");
            parameter.AppendSql(" AND A.BDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                       ");
            parameter.AppendSql(" AND A.BDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                       ");
            parameter.AppendSql(" AND A.SPECNO = C.SPECNO                                                           ");
            if (argGubun == "위")
            {
                parameter.AppendSql(" AND (SUBSTR(A.ORDERCODE,1,1) = 'E' OR SUBSTR(A.ORDERCODE,1,1) = 'G')          ");
            }
            else if (argGubun == "대장")
            {
                parameter.AppendSql("  AND SUBSTR(a.OrderCode,1,1) = 'R'                                            ");
            }

            parameter.AppendSql(" AND SUBSTR(A.ANATNO,1,1) = 'S'                                                    ");
            parameter.AppendSql(" AND A.GBJOB ='V'                                                                  ");
            parameter.AppendSql(" AND A.DEPTCODE = 'TO'                                                             ");
            parameter.AppendSql(" ORDER BY A.BDATE DESC                                                             ");



            parameter.Add("PTNO", argPtno);
            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }
        public COMHPC GetItemByXrayResult(string argPtno, string argFDate, string argTDate, string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,A.DEPTCODE,B.RESULT,B.RESULT1      ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.XRAY_DETAIL A, KOSMOS_PMPA.XRAY_RESULTNEW B                  ");
            parameter.AppendSql(" WHERE A.PANO = :PTNO                                                          ");
            parameter.AppendSql(" AND A.BDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                   ");
            parameter.AppendSql(" AND A.BDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                   ");
            parameter.AppendSql(" AND a.EXINFO = b.WRTNO                                                        ");
            parameter.AppendSql(" AND a.XCODE = :CODE                                                           ");
            parameter.AppendSql(" AND a.DEPTCODE = 'TO'                                                         ");
            parameter.AppendSql(" ORDER BY a.BDate DESC                                                         ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("CODE", argCode);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public int UpdateHyangOrderNo1(string argOrderNo, string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.OCS_HYANG SET     ");
            //parameter.AppendSql("       DEPTCODE = 'TO'                     ");
            parameter.AppendSql("        WARDCODE = 'EN'                   ");
            parameter.AppendSql("       , ORDERNO = :ORDERNO                ");
            parameter.AppendSql("       , CERTNO = ''                       ");
            parameter.AppendSql(" WHERE ROWID = :RID                        ");

            parameter.Add("ORDERNO", argOrderNo);
            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateMayakOrderNo1(string argOrderNo, string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.OCS_MAYAK SET     ");
            parameter.AppendSql("       DEPTCODE = 'TO'                     ");
            parameter.AppendSql("       , WARDCODE = 'EN'                   ");
            parameter.AppendSql("       , ORDERNO = :ORDERNO                ");
            parameter.AppendSql("       , CERTNO = ''                       ");
            parameter.AppendSql(" WHERE ROWID = :RID                        ");

            parameter.Add("ORDERNO", argOrderNo);
            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public string GetRidOcsMayak(string argBdate, string argPtno, string argSucode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.OCS_MAYAK             ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                     ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRidOcsHyang(string argBdate, string argPtno, string argSucode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_OCS.OCS_HYANG             ");
            parameter.AppendSql(" WHERE BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND PTNO = :PTNO                                ");
            parameter.AppendSql("   AND SUCODE = :SUCODE                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                     ");

            parameter.Add("BDATE", argBdate);
            parameter.Add("PTNO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public COMHPC GetEndoJupmstByRdateDeptCodeGbSunap(string argRDATE, string argJob, string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, ORDERCODE, RDATE, RESULTDATE, RESULTDRCODE, GBSUNAP, GBJOB, DEPTCODE  ");
            parameter.AppendSql(" FROM KOSMOS_OCS.ENDO_JUPMST                                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                                       ");
            parameter.AppendSql("   AND RDATE >= TO_DATE(:RDATE, 'YYYY-MM-DD') - 0.99999                            ");
            parameter.AppendSql("   AND RDATE <= TO_DATE(:RDATE, 'YYYY-MM-DD') + 0.99999                            ");
            parameter.AppendSql("   AND DEPTCODE IN ('HR','TO')                                                     ");
            parameter.AppendSql("   AND GBSUNAP = '7'                                                               ");
            parameter.AppendSql("   AND GBJOB = :GBJOB                                                              ");
            parameter.AppendSql("   AND PTNO = :PTNO                                                                ");
            parameter.AppendSql("   AND RESULTDRCODE IS NOT NULL                                                    ");

            parameter.Add("RDATE", argRDATE);
            parameter.Add("GBJOB", argJob);
            parameter.Add("PTNO", argPtno);
            
            return ExecuteReaderSingle <COMHPC>(parameter);
        }
    }
}
