namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicResultRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicResultRepository()
        {
        }

        public List<HIC_RESULT> FindAll(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, GROUPCODE, PART                  ");
            parameter.AppendSql("     , EXCODE, RESCODE, RESULT                 ");
            parameter.AppendSql("     , PANJENG, OPER_DEPT, OPER_DCT            ");
            parameter.AppendSql("     , ACTIVE, ENTSABUN, ENTTIME, ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql("  WHERE 1 = 1                                  ");

            if (nWrtNo != 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                      ");                
            }
            parameter.AppendSql("  ORDER BY GROUPCODE, EXCODE                   ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> FindAll(List<long> nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, GROUPCODE, PART                  ");
            parameter.AppendSql("     , EXCODE, RESCODE, RESULT                 ");
            parameter.AppendSql("     , PANJENG, OPER_DEPT, OPER_DCT            ");
            parameter.AppendSql("     , ACTIVE, ENTSABUN, ENTTIME, ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql("  WHERE 1 = 1                                  ");
            parameter.AppendSql("    AND WRTNO IN (:WRTNO)                      ");
            parameter.AppendSql("  ORDER BY GROUPCODE, EXCODE                   ");

            parameter.AddInStatement("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int PanUpDate(string argPan, string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql("   SET PANJENG     = :PANJENG      ");
            parameter.AppendSql(" WHERE ROWID       = :RID          ");

            #region Query 변수대입
            parameter.Add("PANJENG", argPan, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RID", rOWID);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public string Read_Sangdam_Acting(long wrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE = 'A999'         ");

            parameter.Add("WRTNO", wrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_CervicalCancer(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DECODE(SUBSTR(b.hrremark1, 9, 1)                                            "); 
            parameter.AppendSql("              , '1','1',DECODE(SUBSTR(b.hrremark1, 10, 1), '1', '2', '0')) GBCHECK ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   a                                                   ");
            parameter.AppendSql("     , KOSMOS_OCS.EXAM_ANATMST b                                                   ");
            parameter.AppendSql(" WHERE a.WRTNO    = :WRTNO                                                         ");
            parameter.AppendSql("   AND a.PtNo     = b.PtNo                                                         ");
            parameter.AppendSql("   AND a.JEPDATE  = b.BDATE                                                        ");
            parameter.AppendSql("   AND b.GbJob IN ('V')                                                            ");
            parameter.AppendSql("   AND SUBSTR(b.AnatNo, 1, 1) = 'P'                                                ");
            parameter.AppendSql("   AND b.ORDERCODE = 'PAP`S'                                                       ");
            parameter.AppendSql("   AND b.DEPTCODE  = 'HR'                                                          ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatebyRowId(HIC_RESULT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = :RESULT          ");
            parameter.AppendSql("     , PANJENG  = :PANJENG         ");
            parameter.AppendSql("     , RESCODE  = :RESCODE         ");
            parameter.AppendSql(" WHERE ROWID    = :RID             ");

            parameter.Add("RESULT", item.RESULT);
            parameter.Add("PANJENG", item.PANJENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE", item.RESCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResultPanjengbyRowId(string strResult, string strNewPan, string strResCode, string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = :RESULT          ");
            parameter.AppendSql("     , PANJENG  = :PANJENG         ");
            parameter.AppendSql("     , RESCODE  = :RESCODE         ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN        ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql(" WHERE ROWID    = :RID             ");

            parameter.Add("RESULT", strResult);
            parameter.Add("PANJENG", strNewPan, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetCount(string strJong, long nMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                                          ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                      ");
            parameter.AppendSql("                 SELECT Wrtno                                          ");
            parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU                          ");
            parameter.AppendSql("                  WHERE DELDATE IS NULL                                ");
            parameter.AppendSql("                    AND GBDENTAL = 'Y'                                 ");
            if (strJong == "1")
            {
                parameter.AppendSql("                       AND MIRNO1 = :MIRNO                         ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("                       AND MIRNO2 = :MIRNO                         ");
            }
            else if (strJong == "4")
            {
                parameter.AppendSql("                       AND MIRNO3 = :MIRNO                         ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql("                       AND MIRNO5 = :MIRNO                         ");
            }
            parameter.AppendSql("                 )                                                     ");
            parameter.AppendSql("   AND EXCODE IN ('ZD00','ZD01')                                       ");

            parameter.Add("MIRNO", nMirNo);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public void Update_Reset_Acting(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = ''               ");
            parameter.AppendSql("     , ACTIVE  = ''                ");
            parameter.AppendSql("     , PANJENG  = ''               ");
            parameter.AppendSql("     , ENTSABUN = 0                ");
            parameter.AppendSql("     , ENTTIME  = ''               ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = 'A999'           ");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_RESULT> GetWaitCountByPart(string argEntPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,a.PART,a.ACTIVE                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_JEPSU b                 ");
            parameter.AppendSql(" WHERE a.WRTNO   = b.WRTNO                     ");
            parameter.AppendSql("   AND b.JEPDATE = TRUNC(SYSDATE)              ");
            parameter.AppendSql("   AND a.PART =:PART                           ");
            parameter.AppendSql("   AND ( a.ACTIVE IS NULL OR a.ACTIVE =' ' )   ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                       ");
            parameter.AppendSql("   AND b.GBCHUL != 'Y'                         ");
            parameter.AppendSql(" GROUP BY a.WRTNO,a.PART,a.ACTIVE              ");

            parameter.Add("PART", argEntPart);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetRowIdRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetExCodebyWrtNo_All(long wRTNO, string[] strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT, GROUPCODE                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                          ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                 ");
            parameter.AppendSql("    AND EXCODE IN (:EXCODE)                            ");
            parameter.AppendSql("    AND(RESULT IS NOT NULL OR RESULT <> '.')           ");
            parameter.AppendSql("  ORDER By EXCODE                                      ");

            parameter.Add("WRTNO", wRTNO);
            parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetOnlyExCodebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT              ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                     ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int Update_ResultbyWrtNoExCode(long argWrtNo, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            if (argJong == "")
            {
                parameter.AppendSql("       RESULT = '.'            "); //정상
            }
            else
            {
                parameter.AppendSql("       RESULT = '01'           "); //정상
            }
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");
            if (argJong == "1")
            {
                parameter.AppendSql("   AND EXCODE  = 'A141'        "); //간촬
            }
            else if (argJong == "2")
            {
                parameter.AppendSql("   AND EXCODE  = 'A142'        "); //직촬
            }
            else if (argJong == "3")
            {
                parameter.AppendSql("   AND EXCODE  = 'A142'        "); //직촬(분진)
            }
            else if (argJong == "")
            {
                parameter.AppendSql("   AND EXCODE  = 'A154'        "); //흉부촬영결과
            }

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNoCode(long nWRTNO, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");
            parameter.AppendSql("   AND EXCODE = :EXCODE            ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtno1(long fnWrtno1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ExCode,Result               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fnWrtno1);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public string GetRowidByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID FROM KOSMOS_PMPA.HIC_RESULT   ");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO                       ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                  ");
            parameter.AppendSql("   AND EXCODE <> 'A215'                    ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtNoExCode(long argHcWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND EXCODE IN ('A101','A102')           ");

            parameter.Add("WRTNO", argHcWRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateResultEntSabunbyWrtNoExCode(string strResult, long nWRTNO, long nSabun, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = :RESULT          ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN        ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE          "); //TR11

            parameter.Add("RESULT", strResult);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("ENTSABUN", nSabun);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int InsertSelectbyWrtNo(long nWrtNo, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RESULT     ");
            parameter.AppendSql("       (WRTNO,GROUPCODE,PART,EXCODE,RESCODE,RESULT,PANJENG,OPER_DEPT,OPER_DCT              ");
            parameter.AppendSql(" SELECT :WRTNO,GROUPCODE,PART,EXCODE,RESCODE,RESULT,PANJENG,OPER_DEPT,OPER_DCT             ");
            parameter.AppendSql("      , ACTIVE , ENTSABUN, ENTTIME                                                         ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_RESULT                                                             ");
            parameter.AppendSql("  WHERE WRTNO = :FWRTNO                                                                    ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RESULT> GetExcodebyExCode(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ExCode FROM KOSMOS_PMPA.HEA_RESULT                                                          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND ( ExCode IN ('TX53','TX54')                                                                 ");
            parameter.AppendSql("    OR  ExCode IN ( SELECT CODE FROM KOSMOS_PMPA.HIC_EXCODE WHERE SUBSTR(XRAYCODE,1,2) ='HA')      ");
            parameter.AppendSql("    OR  ExCode IN ( SELECT CODE FROM KOSMOS_PMPA.HIC_EXCODE WHERE SUBSTR(XRAYCODE,1,2) ='MR')  )   ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateResultbyRowId(HIC_RESULT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = :RESULT          ");
            parameter.AppendSql("     , PANJENG  = :PANJENG         ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN        ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql(" WHERE ROWID    = :RID             ");

            parameter.Add("RESULT", item.RESULT);
            parameter.Add("PANJENG", item.PANJENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResultbyWrtNo(string idNumber, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       ENTSABUN = :ENTSABUN        ");
            parameter.AppendSql("     , RESULT   = '.'              ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   IN('ZD00', 'ZD01') ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("ENTSABUN", idNumber);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateActiveResultbyWrtNoExCode(string idNumber, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       ACTIVE   = 'Y'              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN        ");
            parameter.AppendSql("     , RESULT   = '01'             ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = 'ZD99'           ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("ENTSABUN", idNumber);

            return ExecuteNonQuery(parameter);
        }

        public string GetResultOnlybyWrtNoExCode(long nWRTNO, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE = :EXCODE        ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteScalar<string>(parameter);
        }

        public int GetExCodebyWrtNoExCode(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND EXCODE = 'A142'         ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_RESULT> GetAllByWrtNo(long fnWRTNO)
        {   
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, EXCODE, PART, RESCODE, PANJENG, RESULT   ");
            parameter.AppendSql("     , OPER_DEPT, OPER_DCT, ACTIVE, GROUPCODE          ");
            parameter.AppendSql("     , ENTSABUN, ENTTIME, ROWID AS RID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateEntSabunbyWrtNo(long nSabun, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       ACTIVE   = 'Y'              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN        ");
            parameter.AppendSql("     , RESULT   = '01'             ");
            parameter.AppendSql("     , ENTTime  = SYSDATE          ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = 'A999'           ");

            parameter.Add("ENTSABUN", nSabun);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int Update_Hic_Result_Complete(string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = '01'             ");
            parameter.AppendSql("     , PANJENG  = 'B'              ");
            parameter.AppendSql("     , ACTIVE   = 'Y'              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN        ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql(" WHERE ROWID    = :RID             ");

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateGroupCodebyWrtNoCode(string strNew_Group, string strNew_ExCode, long nWrtNo, string strOLD_Group, string strOld_ExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET      ");
            parameter.AppendSql("       GROUPCODE = :NEW_GROUPCODE      ");
            if (!strNew_ExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("     , EXCODE    = :NEW_EXCODE     ");
            }
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND GROUPCODE    = :OLD_GROUP       ");
            if (!strOld_ExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE    = :OLD_EXCODE     ");
            }

            parameter.Add("NEW_GROUPCODE", strNew_Group, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!strNew_ExCode.IsNullOrEmpty())
            {
                parameter.Add("NEW_EXCODE", strNew_ExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("OLD_GROUP", strOLD_Group, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (!strOld_ExCode.IsNullOrEmpty())
            {
                parameter.Add("OLD_EXCODE", strOld_ExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public string GetResultbyExCode(long wRTNO, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Result FROM KOSMOS_PMPA.HIC_RESULT                                                          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND Result IS NOT NULL                                                                          ");
            if (strJong == "간촬")
            {
                parameter.AppendSql("   AND ExCode ='A141'                                                                          ");
            }
            else if (strJong == "직촬")
            {
                parameter.AppendSql("   AND ExCode ='A142'                                                                          ");
            }           

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_RESULT GetExCodeResultbyWrtNoInExCode(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE,RESULT FROM KOSMOS_PMPA.HIC_RESULT                                                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND RESULT IS NULL                                                                              ");
            //parameter.AppendSql("   AND EXCODE IN ('ZD00','ZD01','LU38','LU54','A291','TX20','TZ45','AA08','A297','A298','A169')    ");
            parameter.AppendSql("   AND EXCODE IN ('ZD01','LU38','LU54','A291','TX20','TZ45','AA08','A297','A298','A169')    ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int UpdateResultPanjengResCodeActivebyRowId(string strResult, string strNewPan, string strResCode, string idNumber, string sSysDate, string strROWID)        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                                                                      ");
            parameter.AppendSql("       RESULT   = :RESULT                                                                              ");
            parameter.AppendSql("     , PANJENG  = :PANJENG                                                                             ");
            parameter.AppendSql("     , RESCODE  = :RESCODE                                                                             ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                                                                            ");
            parameter.AppendSql("     , ENTTIME  = TO_DATE(:ENTTIME, 'YYYY-MM-DD HH24:MI')                                              ");
            if (!strResult.IsNullOrEmpty())
            {
                parameter.AppendSql("     , ACTIVE   = 'Y'                                                                              ");
            }
            else
            {
                parameter.AppendSql("     , ACTIVE   = ''                                                                               ");
            }
            
            parameter.AppendSql(" WHERE ROWID    = :RID                                                                                 ");

            parameter.Add("RESULT", strResult);
            parameter.Add("PANJENG", strNewPan, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("ENTTIME", sSysDate);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RESULT> GetItembyOnlyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, GROUPCODE, PART                  ");
            parameter.AppendSql("     , EXCODE, RESCODE, RESULT                 ");
            parameter.AppendSql("     , PANJENG, OPER_DEPT, OPER_DCT            ");
            parameter.AppendSql("     , ACTIVE, ENTSABUN, ENTTIME               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                         ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int GetCountbyWrtNoInExCode(long nWRTNO, List<string> strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_RESULT> GetCorrectedEyeSightbyWrtNoExCode(long nWRTNO, List<string> strCodeList2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("EXCODE", strCodeList2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetExCodeREsultbyWrtNoExCodeNotLike(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE NOT LIKE 'TH%'       ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetWrtNoExCodeResultbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, EXCODE, RESULT  FROM HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND EXCODE IN ('A131','A132','A258','A259')     ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateChangeItembyWrtNoExCode(long fnWrtNo, string strExCode, string strResult)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                                                                      ");
            parameter.AppendSql("       RESULT   = :RESULT                                                                              ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                                                               ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE                                                                              ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("RESULT", strResult);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNoExCodeTX07(long nWrtNo, string strResult, string idNumber)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                                                                      ");
            parameter.AppendSql("       RESULT   = :RESULT                                                                              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                                                                            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                                                                              ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                                                                                  ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                                                               ");
            parameter.AppendSql("   AND EXCODE   = 'TX07'                                                                               ");

            parameter.Add("RESULT", strResult);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatePointbyWrtNoExCode(string idNumber, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET          ");
            parameter.AppendSql("       ENTSABUN = :ENTSABUN                ");
            parameter.AppendSql("     , RESULT   = '.'                      ");
            parameter.AppendSql("     , ENTTime  = SYSDATE                  ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");
            parameter.AppendSql("   AND EXCODE   = 'ZD00'                   ");

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpDate_Audio_Auto(HIC_RESULT item1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql("   SET RESULT     = :RESULT        ");
            parameter.AppendSql("      ,ENTSABUN   = :ENTSABUN      ");
            parameter.AppendSql("      ,ENTTIME    = SYSDATE        ");
            parameter.AppendSql("      ,ACTIVE     = :ACTIVE        ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE     = :EXCODE        ");

            parameter.Add("RESULT", item1.RESULT);
            parameter.Add("ENTSABUN", item1.ENTSABUN);
            parameter.Add("WRTNO", item1.WRTNO);
            parameter.Add("ACTIVE", item1.ACTIVE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", item1.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetCountbyWrtNoPart(long fnWRTNO, string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT FROM KOSMOS_PMPA.HIC_RESULT                                                          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND PART  = :PART                                                                               ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("PART", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int GetCountbyWrtNoExCodeNoResult(long nWrtNo, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT FROM KOSMOS_PMPA.HIC_RESULT                                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND RESULT IS NULL                                                                              ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                                                                            ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateResultEntSabunEntTimeActivebyWrtNoExCode(long nWrtNo, string idNumber)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                                                                          ");
            parameter.AppendSql("       RESULT = '01'                                                                                       ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                                                                                ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                                                                                  ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                                                                                      ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                                                                   ");
            //parameter.AppendSql("   AND EXCODE IN('A135','A136','A899','A902')                                                              ");
            parameter.AppendSql("   AND EXCODE IN('A135','A136','A902')                                                              ");

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountByWrtnoInExCode(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT FROM KOSMOS_PMPA.HIC_RESULT                                                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
            parameter.AppendSql("   AND RESULT IS NULL                                                                              ");
            //parameter.AppendSql("   AND EXCODE IN ('A135','A136','A899','A902')                                                     ");
            parameter.AppendSql("   AND EXCODE IN ('A135','A136','A902')                                                     ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateTeethbyWrtNoExCode(long nWrtNo, string idNumber)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                                                                          ");
            parameter.AppendSql("       RESULT = '.'                                                                                        ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                                                                                ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                                                                                  ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                                                                                      ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                                                                   ");
            //parameter.AppendSql("   AND EXCODE IN('ZD00', 'ZD01', 'LU38', 'LU54', 'A291', 'TX20', 'TZ45', 'AA08', 'A297', 'A298', 'A169')   ");
            parameter.AppendSql("   AND EXCODE IN('ZD01', 'LU38', 'LU54', 'A291', 'TX20', 'TZ45', 'AA08', 'A297', 'A298', 'A169')   ");

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", nWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResultActiveEntSabunEntTimebyRowId(string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET      ");
            parameter.AppendSql("       RESULT   ='01'                  ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                  ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE              ");
            parameter.AppendSql(" WHERE ROWID    = :RID                 ");

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNoExCodeNoResult(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('x') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND EXCODE IN ('A121','A123','A124','A282')     ");
            parameter.AppendSql("   AND Result IS NOT NULL                          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public long GetCountByWrtnoInExCodeIn(List<long> lstHicWrtno, List<string> lstExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('x') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO IN (:WRTNO)              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");

            parameter.Add("WRTNO", lstHicWrtno);
            parameter.AddInStatement("EXCODE", lstExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_RESULT> GetSpcResByPtnoSDate(string strPtno, string strSDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT, EXCODE                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                                      ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                  ");
            parameter.AppendSql("       SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU WHERE PTNO =:PTNO   ");
            parameter.AppendSql("          AND JEPDATE = TO_DATE(:SDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("          AND DELDATE IS NULL                                      ");
            parameter.AppendSql(" )                                                                 ");
            parameter.AppendSql("   AND EXCODE IN ('MU11','MU15','MU12','MU74','LM10','TR11','TH12','TH22','TZ08','A899','A902','A992','A993','A803') ");

            parameter.Add("PTNO", strPtno);
            parameter.Add("SDATE", strSDate);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateActivebyPartWrtNo(string strPart, long argWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET      ");
            parameter.AppendSql("       ACTIVE = ' '                    ");
            parameter.AppendSql(" WHERE PART   = :PART                  ");
            parameter.AppendSql("   AND WRTNO  = :WRTNO                 ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("PART", strPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetResultbyWrtNoPart(long fnWRTNO, string strPart)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT FROM KOSMOS_PMPA.HIC_RESULT ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                     ");
            parameter.AppendSql("   AND PART  = :PART                      ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("PART", strPart);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int UpdateResCode(string strResCode, string strResult, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESCODE  = :RESCODE         ");
            parameter.AppendSql("     , RESULT   = :RESULT          ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = 'A151'           "); //심전도

            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESULT", strResult);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string Chk_Simli_ExCode(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE = 'TZ17'             ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_RESULT> GetListByWrtnoCodeIN(long argWrtno, List<string> lstExcode, string argDept)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT, EXCODE              ");
            if (argDept == "TO")
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT      ");
            }
            else
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            }
            
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");

            parameter.Add("WRTNO", argWrtno);
            parameter.AddInStatement("EXCODE", lstExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public HIC_RESULT GetResultRowIdbyWrtNoExCode(long fnWRTNO, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT, EXCODE              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");
            parameter.AppendSql("   AND EXCODE = :EXCODE            ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int UpdateItembyRowId(string strResult, string strNewPan, string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET              ");
            parameter.AppendSql("       RESULT  = :RESULT                       ");
            parameter.AppendSql("     , PANJENG = :PANJENG                      ");
            parameter.AppendSql(" WHERE ROWID   = :RID                          ");

            parameter.Add("RESULT", strResult);
            parameter.Add("PANJENG", strNewPan, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public void InsertData(HIC_RESULT hRES)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RESULT (           ");
            parameter.AppendSql("       WRTNO,GROUPCODE,PART,EXCODE,RESCODE     ");
            parameter.AppendSql(" ) VALUES (                                    ");
            parameter.AppendSql("      :WRTNO,:GROUPCODE,:PART,:EXCODE,:RESCODE ");
            parameter.AppendSql(" )  ");

            parameter.Add("WRTNO", hRES.WRTNO);
            parameter.Add("GROUPCODE", hRES.GROUPCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PART", hRES.PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", hRES.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE", hRES.RESCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            ExecuteNonQuery(parameter);
        }

        public void DeleteByRowid(string argRid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_RESULT  WHERE ROWID = :RID  ");            

            parameter.Add("RID", argRid);

            ExecuteNonQuery(parameter);
        }

        public void InsertDelSelectbyRowid(string argRid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RESULT_DEL                                                 ");
            parameter.AppendSql("     ( WRTNO,GROUPCODE,PART,EXCODE,RESCODE,RESULT,PANJENG,ACTIVE,DELSABUN,DELTIME )    ");
            parameter.AppendSql(" SELECT WRTNO,GROUPCODE,PART,EXCODE,RESCODE,RESULT,PANJENG,ACTIVE,:DELSABUN,SYSDATE    ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_RESULT                                                         ");
            parameter.AppendSql("  WHERE ROWID = :RID                                                                ");

            parameter.Add("RID", argRid);
            parameter.Add("DELSABUN", clsType.User.IdNumber.To<long>());

            ExecuteNonQuery(parameter);
        }

        public int GetCntbyWrtNoExCode(long fnWrtNo, string[] strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('x') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyPtNo(string sPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('x') CNT                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                              ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO FROM HIC_JEPSU               ");
            parameter.AppendSql("                  WHERE PTNO = :PTNO                       ");
            parameter.AppendSql("                    AND JEPDATE = TRUNC(SYSDATE)           ");
            parameter.AppendSql("                    AND DELDATE IS NULL)                   ");
            parameter.AppendSql("   AND EXCODE IN ('TX20', 'TX64', 'TX41')                  ");

            parameter.Add("PTNO", sPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateResultActivebyWrtNoExCode(long fnWRTNO, string strInsomniaMun, string idNumber, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET      ");
            parameter.AppendSql("       RESULT   = :RESULT              ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                  ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE              ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE              ");

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("RESULT", strInsomniaMun);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RESULT> GetResultExCodebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT, EXCODE              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateActivebyWrtNoExCode(string idNumber, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET          ");
            parameter.AppendSql("       ACTIVE   = 'Y'                      ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                ");
            parameter.AppendSql("     , RESULT   = '01'                     ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                  ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                   ");
            parameter.AppendSql("   AND EXCODE  IN('ZD99','ZD01','ZD09')    ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("ENTSABUN", idNumber);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RESULT> GetExCodeLoopbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateBimanbyWrtNoExCode(string strBiman, string idNumber, long nWRTNO, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET      ");
            parameter.AppendSql("       RESULT   = :RESULT              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE              ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE              ");

            parameter.Add("RESULT", strBiman);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RESULT> GetResultbyWrtNoExCodeList(long nWRTNO, List<string> sExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");
            parameter.AppendSql(" ORDER BY ExCode DESC              ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("EXCODE", sExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetResultbyWrtNoExCode(long fnWRTNO, List<string> sExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");
            parameter.AppendSql(" ORDER BY ExCode                   ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.AddInStatement("EXCODE", sExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpdateActiveResultbyWrtNo(string idNumber, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET      ");
            parameter.AppendSql("       ACTIVE   = 'Y'                  ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE              ");
            parameter.AppendSql("     , RESULT   = '01'                 ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND EXCODE   = 'A999'               "); 

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetActivebyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ACTIVE                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE = 'A999'         ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public string GetRowidStomachByWrtno(long argwRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT              ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");
            parameter.AppendSql("   AND EXCODE IN ('TX20', 'TX23', 'TX41')  ");

            parameter.Add("WRTNO", argwRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRowidColonByWrtno(long argwRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT              ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");
            parameter.AppendSql("   AND EXCODE IN ('TX32', 'TX64', 'TX41')  ");

            parameter.Add("WRTNO", argwRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_RESULT> GetExCodeResultbyOnlyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT, EXCODE                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int Update_Auto_Result_Hea(HIC_RESULT item, string strTemp)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT                                                  ");
            parameter.AppendSql("   SET RESULT     = :RESULT                                                    ");
            if (strTemp != "3")
            {
                parameter.AppendSql("     , PANJENG    = :PANJENG                                               ");
            }
            parameter.AppendSql("     , ENTTIME    = SYSDATE                                                    ");
            parameter.AppendSql("     , ENTSABUN   = :ENTSABUN                                                  ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO                                                     ");
            if (strTemp == "1")
            {
                parameter.AppendSql("   AND (RESULT IS NULL OR RESULT IN ('01','02','03','04','08','09','10'))  ");
            }
            else if (strTemp == "2")
            {
                parameter.AppendSql("   AND RESULT IS NULL                                                      ");
            }
            parameter.AppendSql("   AND EXCODE = :EXCODE                                                        ");

            parameter.Add("RESULT", item.RESULT);
            if (strTemp != "3")
            {
                parameter.Add("PANJENG", item.PANJENG);
            }
            parameter.Add("ENTSABUN", item.ENTSABUN);            
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("EXCODE", item.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetJepDate(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(TO_CHAR(JEPDATE,'YYYY-MM-DD')) MAXDATE  ");
            parameter.AppendSql("     , MIN(TO_CHAR(JEPDATE,'YYYY-MM-DD')) MINDATE  ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   And JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND LTDCODE  = :LTDCODE                         ");
            parameter.AppendSql("   AND DelDate IS NULL                             ");
            parameter.AppendSql("   AND GJYEAR   = :GJYEAR                          ");

            parameter.Add("FRDATE", fstrFDate);
            parameter.Add("TODATE", fstrTDate);
            parameter.Add("LTDCODE", fstrLtdCode);
            parameter.Add("GJYEAR", strGjYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int UpdatebyWrtNoExCode(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = '01'             ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql("     , ENTSABUN = '222'            ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = 'TR11'           ");
            parameter.AppendSql("   AND RESULT IS NULL              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public string GetAllByWrtNo(string argWrtNo, string argJepNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(TO_CHAR(JEPDATE,'YYYY-MM-DD')) MAXDATE          ");
            parameter.AppendSql("     , MIN(TO_CHAR(JEPDATE,'YYYY-MM-DD')) MINDATE          ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_JEPSU                               ");
            parameter.AppendSql(" WHERE EXCODE IN (                                         ");
            parameter.AppendSql("                   SELECT EXCODE                           ");
            parameter.AppendSql("                     FROM KOSMOS_PMPA.HEA_AUTOPAN_LOGIC    ");
            parameter.AppendSql("                    WHERE WRTNO = :WRTNO                   ");
            parameter.AppendSql("                    GROUP BY EXCODE                        ");
            parameter.AppendSql("                 )                                         ");
            parameter.AppendSql("   AND WRTNO = :WRTNO2                                     ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("WRTNO2", argJepNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatebyWrtNoSabunPano(string idNumber, string strJepDate, long nPano, string strResult, string[] strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                                  ");
            parameter.AppendSql("       RESULT    = :RESULT                                         "); //정상A
            parameter.AppendSql("     , ENTSABUN  = :ENTSABUN                                       ");
            parameter.AppendSql("     , ENTTIME   = SYSDATE                                         ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU           ");
            parameter.AppendSql("                  WHERE JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')   ");
            parameter.AppendSql("                    AND PANO = :PANO                               "); 
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                                         ");
            parameter.AppendSql("   AND Result IS NULL                                              ");

            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("JEPDATE", strJepDate);
            parameter.Add("PANO", nPano);
            parameter.Add("RESULT", strResult);
            parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResultbyWrtNoExCode(string strResult, long nWRTNO, long nSabun, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       RESULT   = :RESULT          ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE          ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN        "); 
            if (strResult.Trim() != "")
            {
                parameter.AppendSql("     , ACTIVE = 'Y'            ");
            }
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE          "); //TR11

            parameter.Add("RESULT", strResult);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("ENTSABUN", nSabun);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNoExCode(long nSabun, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                  ");
            parameter.AppendSql("       RESULT   = :RESULT                          ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                          ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                        ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                           ");
            parameter.AppendSql("   AND EXCODE   IN ('ZD00','LU38','LU54','A291')   ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("ENTSABUN", nSabun);

            return ExecuteNonQuery(parameter);
        }

        public int GetItembyWrtNoExCode(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT FROM HIC_RESULT              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND RESULT IS NULL                              ");
            parameter.AppendSql("   AND EXCODE IN ('ZD00','LU38','LU54','A291')     ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetEntSabunbyWrtNoExCode(long wRTNO, string strPart, string strChkNew, List<string> fstrPartExam)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT FROM HIC_RESULT        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            if (strPart != "*")
            {
                parameter.AppendSql("  AND EXCODE IN (:EXCODE)              ");
            }
            if (strChkNew == "1")
            {
                parameter.AppendSql("   AND Result IS NULL                  ");
            }

            parameter.Add("WRTNO", wRTNO);
            parameter.AddInStatement("EXCODE", fstrPartExam, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public string GetRowidByOneExcodeWrtno(string argExcode, long argwRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE = :EXCODE        ");

            parameter.Add("WRTNO", argwRTNO);
            parameter.Add("EXCODE", argExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRowidByOneExcodePtnoJepdate(string argExcode, string argPTNO, string argJEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                ");
            parameter.AppendSql("              SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU              ");
            parameter.AppendSql("               WHERE PTNO = :PTNO                                  ");
            parameter.AppendSql("                 AND JEPDATE = TO_DATE(:JEPDATE , 'YYYY-MM-DD')    ");
            parameter.AppendSql("                 AND DELDATE IS NULL )                             ");
            parameter.AppendSql("   AND EXCODE = :EXCODE        ");

            parameter.Add("PTNO", argPTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", argJEPDATE);
            parameter.Add("EXCODE", argExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateSangdambyWrtno(long fnWRTNO, string strExam, string strSangDam)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULT SET  ");
            if (strSangDam == "Y")
            {
                parameter.AppendSql("       SANGDAM = 'N'           ");
            }
            else
            {
                parameter.AppendSql("       SANGDAM = 'Y'           ");
            }
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO            ");
            parameter.AppendSql("   AND EXCODE  = :EXCODE           ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("EXCODE", strExam, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_RESULT> GetItembyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT, EXCODE, RESCODE         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("  AND RESULT <> '미실시'                ");
            parameter.AppendSql("  ORDER BY EXCODE                      ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public HIC_RESULT GetResultbyJepsu(string strPano, string strExCode, int nDay)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT,ROWID AS RID FROM KOSMOS_PMPA.HEA_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU   ");
            parameter.AppendSql("                  WHERE PTNO = :PTNO                       ");
            parameter.AppendSql("                    AND SDATE >= TRUNC(SYSDATE - :DAY)     ");
            parameter.AppendSql("                    AND DELDATE IS NULL)                   ");
            parameter.AppendSql("  AND EXCODE = :EXCODE                                     ");

            parameter.Add("PTNO", strPano);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DAY", nDay);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int GetCountbyWrtNoExCode(string argWrtNo, string argJepNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT                  ");
            parameter.AppendSql(" WHERE EXCODE IN (                             ");
            parameter.AppendSql("                  SELECT EXCODE                ");
            parameter.AppendSql("                    FROM HEA_AUTOPAN_LOGIC     ");
            parameter.AppendSql("                   WHERE WRTNO = :WRTNO        ");
            parameter.AppendSql("                   GROUP BY EXCODE             ");
            parameter.AppendSql("                  )                            ");
            parameter.AppendSql("   AND WRTNO = :JEPNO                          ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("JEPNO", argJepNo);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdatebyWrtNo(string argReadTime, string argResult, long nWRTNO, string[] strCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                          ");
            parameter.AppendSql("       READTIME = TO_DATE(:READTIME, 'YYYY-MM-DD HH24:MI') ");
            parameter.AppendSql("     , RESULT   = :RESULT                                  ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                                   ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                                 ");

            parameter.Add("READTIME", argReadTime);
            parameter.Add("RESULT", argResult);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", strCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetResultByWrtNoExCode(long nWRTNO, string[] strCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("EXCODE", strCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public HIC_RESULT GetResultRowIdbyPtNo(string strPano, string[] strCodes, int nDay, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT, ROWID AS RID                                           ");
            
            if (strGubun == "HA")   //종검
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT                                  ");
                parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HEA_JEPSU   ");
                parameter.AppendSql("                    WHERE SDATE >= TRUNC(SYSDATE - :DAY)       ");
            }
            else if (strGubun == "HC")  //일검
            {
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                                  ");
                parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU   ");
                parameter.AppendSql("                    WHERE JEPDATE >= TRUNC(SYSDATE - :DAY)       ");
            }
            parameter.AppendSql("                  AND PTNO = :PTNO                           ");
            //parameter.AppendSql("                  WHERE PTNO = :PTNO                           ");
            //parameter.AppendSql("                    AND JEPDATE >= TRUNC(SYSDATE - :DAY)       ");
            parameter.AppendSql("                    AND DELDATE IS NULL)                       ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                                     ");

            parameter.Add("PTNO", strPano, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("EXCODE", strCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DAY", nDay);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int UpdatebyRowId(string strResult, string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULT SET  ");
            parameter.AppendSql("       RESULT = :RESULT            ");
            parameter.AppendSql(" WHERE ROWID  = :RID               ");

            parameter.Add("RESULT", strResult);
            parameter.Add("RID", rOWID);

            return ExecuteNonQuery(parameter);
        }

        public int Update_ResultbyWrtNo(HIC_RESULT item, List<string> strExCode, string argReadDate = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET                      ");
            parameter.AppendSql("       RESULT = :RESULT                                ");
            parameter.AppendSql("     , ACTIVE = 'Y'                                    ");
            if (argReadDate != "")
            {
                parameter.AppendSql("     , READTIME = TO_DATE(:READTIME, 'YYYY-MM-DD') ");
            }
            parameter.AppendSql("     , ENTTIME  = SYSDATE                              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                             ");

            parameter.Add("RESULT", item.RESULT);
            if (argReadDate != "")
            {
                parameter.Add("READTIME", argReadDate);
            }
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetResultbyWrtNo(long nWRTNO, List<string> strExcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)     ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("EXCODE", strExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int UpdateSangdambyWrtnoExCode(long fnWRTNO, string strExam, string strSangdam)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET  ");
            parameter.AppendSql("       SANGDAM  = :SANGDAM        ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE          ");

            parameter.Add("SANGDAM", strSangdam);
            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("EXCODE", strExam, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetEntSabunbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(ENTTIME), ENTSABUN  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND PART <> '5'             ");
            parameter.AppendSql("   AND ENTTIME IS NOT NULL     ");
            parameter.AppendSql(" GROUP BY ENTSABUN             ");
            parameter.AppendSql(" ORDER BY MAX(ENTTIME) DESC    ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtNoExCode(long wRTNO, List<string> strCodeList)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)         ");
            parameter.AppendSql(" ORDER BY EXCODE                   ");

            parameter.Add("WRTNO", wRTNO);
            parameter.AddInStatement("EXCODE", strCodeList, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public HIC_RESULT GetResultRowIdbyExCode(long fnWRTNO, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID, RESULT               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND RESULT IS NULL              ");
            parameter.AppendSql("   AND EXCODE = :EXCODE            ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public int Update_Auto_Result_Hic(HIC_RESULT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT          ");
            parameter.AppendSql("   SET RESULT     = :RESULT            ");
            parameter.AppendSql("     , PANJENG    = :PANJENG           ");
            parameter.AppendSql("     , ENTSABUN   = :ENTSABUN          ");
            parameter.AppendSql("     , ENTTIME    = SYSDATE            ");
            parameter.AppendSql(" WHERE ROWID      = :RID               ");

            parameter.Add("RESULT", item.RESULT);
            parameter.Add("PANJENG", item.PANJENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_RESULT GetResultRowIdbyWrtNo(long fnWRTNO, string strNewCode, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID, RESULT       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO        ");
            parameter.AppendSql("    AND EXCODE = :EXCODE       ");
            if (strExCode != "C203" && strExCode != "C204" && strExCode != "A108" && strExCode != "A109")
            {
                parameter.AppendSql("    AND Result IS NULL     ");
            }

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("EXCODE", strNewCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtNo(long fnWRTNO, string strExCode = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                      ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                             ");
            if (strExCode == "")
            {
                parameter.AppendSql("    AND Result IS NOT NULL                     ");
            }
            else
            {
                parameter.AppendSql("    AND ExCode IN ('A282','H805','A121','A283')");
            }

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int Update_Hic_Result(string strResult, long idNumber, long fnWRTNO, string[] strExamList)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT          ");
            parameter.AppendSql("   SET RESULT     = :RESULT            ");
            parameter.AppendSql("      ,ENTSABUN   = :ENTSABUN          ");
            parameter.AppendSql("      ,ENTTIME    = SYSDATE            ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO             ");
            parameter.AppendSql("   AND EXCODE    IN (:EXCODE)          ");
            parameter.AppendSql("   AND(RESULT = '' OR RESULT IS NULL)  ");

            #region Query 변수대입
            parameter.Add("RESULT", strResult);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", fnWRTNO);
            if (!strExamList.IsNullOrEmpty())
            {
                parameter.AddInStatement("EXCODE", strExamList, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNo(long nWrtNo, string[] strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('x') CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO     ");
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE IN (:EXCODE) ");
            }

            parameter.Add("WRTNO", nWrtNo);
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNoNotIn(long nWrtNo, string[] strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('x') CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO     ");
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE NOT IN (:EXCODE) ");
            }

            parameter.Add("WRTNO", nWrtNo);
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNoExcodes(long nWrtNo, string[] strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('x') CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO     ");
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE NOT IN (:EXCODE) ");
            }

            parameter.Add("WRTNO", nWrtNo);
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNoExCodeChkNew(long nWrtNo, string strPart, string strChkNew, List<string> strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('x') CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO     ");
            if (strPart != "*" && strPart != "")
            {
                parameter.AppendSql("   AND EXCODE IN (:EXCODE) ");
            }
            if (strChkNew == "1")
            {
                parameter.AppendSql("   AND RESULT IS NULL      ");
            }


            parameter.Add("WRTNO", nWrtNo);
            if (strPart != "*" && strPart != "")
            {
                parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<int>(parameter);
        }

        public string GetResultByWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE = 'A990'         ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int Read_Result_Acitve_Status(long nWrtNo, string sysdate, long pano, string strEntPart, string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('x') CNT                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT A                                        "); 
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE B                                        ");
            if (strJong == "Y")
            {
                parameter.AppendSql(" WHERE A.WRTNO IN (                                                ");
                parameter.AppendSql("              SELECT WRTNO FROM KOSMOS_PMPA.HIC_JEPSU              ");
                parameter.AppendSql("               WHERE PANO = :PANO                                  ");
                parameter.AppendSql("                 AND JEPDATE = TO_DATE(:JEPDATE , 'YYYY-MM-DD')    ");
                parameter.AppendSql("                 AND DELDATE IS NULL )                             ");
            }
            else
            {
                parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                            ");
            }
            parameter.AppendSql("   AND A.EXCODE = B.CODE                                               ");
            parameter.AppendSql("   AND B. ENTPART = :ENTPART                                           ");
            parameter.AppendSql("   AND (a.Active = '' or a.Active is null)                             ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("JEPDATE", sysdate);
            parameter.Add("PANO", pano);
            parameter.Add("ENTPART", strEntPart, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int Update_Auto_Result(string strActValue, long nSabun, long fnWRTNO, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT          ");
            parameter.AppendSql("   SET RESULT     = :RESULT            ");
            parameter.AppendSql("     , ENTSABUN   = :ENTSABUN          ");
            parameter.AppendSql("     , ACTIVE     = 'Y'                ");
            parameter.AppendSql("     , ENTTIME    = SYSDATE            ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO             ");
            parameter.AppendSql("   AND ROWID      = :RID               ");            
            parameter.AppendSql("   AND(RESULT = '' OR RESULT IS NULL)  "); //액팅처리안된것만

            parameter.Add("RESULT", strActValue);
            parameter.Add("ENTSABUN", nSabun);
            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Update_Result_ChulAutoFlag(HIC_RESULT item, string[] strCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql("   SET RESULT     = :RESULT    ");
            parameter.AppendSql("     , ENTSABUN   = :ENTSABUN  ");
            parameter.AppendSql("     , ACTIVE     = :ACTIVE    ");
            parameter.AppendSql("     , ENTTIME    = SYSDATE    ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO     ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)     ");

            parameter.Add("RESULT", item.RESULT);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("ACTIVE", item.ACTIVE);
            parameter.AddInStatement("EXCODE", strCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int GetResultCount_ChulAutFlag(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND RESULT IS NULL                          ");
            parameter.AppendSql("   AND EXCODE IN ('A135','A136','A899','A902') ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int Update_Result_Flag(long nSabun, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT          ");
            parameter.AppendSql("   SET RESULT     = '.'                ");
            parameter.AppendSql("     , ENTSABUN   = :ENTSABUN          ");
            parameter.AppendSql("     , ACTIVE     = 'Y'                ");
            parameter.AppendSql("     , ENTTIME    = SYSDATE            ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO             ");
            parameter.AppendSql("   AND EXCODE IN ('ZD00','ZD01','LU38','LU54','A291','TX20','TZ45','AA08','A297','A298','A169')  "); 

            parameter.Add("ENTSABUN", nSabun);
            parameter.Add("WRTNO", nWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int GetExCodebyWrtNo_Second(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                        ");
            parameter.AppendSql("   AND SUBSTR(ExCode,1,3) IN ('TH1','TH2')     ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetExCodebyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU  a                ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_RESULT b                ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                        ");
            parameter.AppendSql("   AND a.GjChasu <> '2'                        "); //2차는 제외
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                    ");
            parameter.AppendSql("   AND SUBSTR(b.ExCode,1,3) IN ('TH5','TH6','TH7','TH8') ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetResultCount_ACT(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM HIC_RESULT                      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND ExCode IN ('A101','A102')       ");
            parameter.AppendSql("   AND Result IS NOT NULL              ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public int Update_ResultbyRowId(string strXrayNo, long idNumber, string sRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT          ");
            parameter.AppendSql("   SET RESULT     = :RESULT            ");
            parameter.AppendSql("     , ENTSABUN   = :ENTSABUN          ");
            parameter.AppendSql("     , ENTTIME    = SYSDATE            ");
            parameter.AppendSql(" WHERE ROWID      = :RID               ");
            
            parameter.Add("RESULT", strXrayNo);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", sRowId);
            
            return ExecuteNonQuery(parameter);
        }

        public string GetXrayNoByWrtno(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE = 'A215'         ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public IList<HIC_RESULT> GetResultByWrtnoExCodes(long argWRTNO, string[] strCodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ExCode,Result            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT   ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO          ");
            parameter.AppendSql("   AND EXCODE IN ( :EXCODE )    ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.AddInStatement("EXCODE", strCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public IList<HIC_RESULT> GetResultByWrtnosExCodes(string[] strWrtno, string[] strCodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ExCode,Result            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT   ");
            parameter.AppendSql(" WHERE WRTNO  IN ( :WRTNO )     ");
            parameter.AppendSql("   AND EXCODE IN ( :EXCODE )    ");
           
            parameter.AddInStatement("WRTNO", strWrtno);
            parameter.AddInStatement("EXCODE", strCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int UpDate(HIC_RESULT item3)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql("   SET RESULT     = :RESULT        ");
            parameter.AppendSql("      ,PANJENG    = :PANJENG       ");
            parameter.AppendSql("      ,RESCODE    = :RESCODE       ");
            parameter.AppendSql("      ,ENTSABUN   = :ENTSABUN      ");
            parameter.AppendSql("      ,ENTTIME    = SYSDATE        ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE     = :EXCODE        ");

            #region Query 변수대입
            parameter.Add("RESULT",     item3.RESULT);
            parameter.Add("PANJENG",    item3.PANJENG);
            parameter.Add("RESCODE",    item3.RESCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN",   item3.ENTSABUN);
            parameter.Add("WRTNO",      item3.WRTNO);
            parameter.Add("EXCODE",     item3.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public string Read_ExCode(long argWrtNo, List<string> strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                             ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteScalar<string>(parameter);
        }

        public HIC_RESULT Read_ExCode2(long argWrtNo, string[] strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                             ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.AddInStatement("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> Read_Result(string PTNO, string JEPDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ExCode,Result                                                                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                                                                                          ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO                                                                                          ");
            parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU                                                                          ");
            parameter.AppendSql("                  WHERE PTNO = :PTNO                                                                                   ");
            parameter.AppendSql("                    AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')                                                       ");
            parameter.AppendSql("                    AND DELDATE IS NULL)                                                                               ");
            parameter.AppendSql("  AND EXCODE IN ('MU11','MU15','MU12','MU74','LM10','TR11','TH12','TH22','TZ08','A899','A902','A903','A920','TH51')    ");

            parameter.Add("PTNO", PTNO);
            parameter.Add("JEPDATE", JEPDATE);
            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> Read_Result2(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ExCode,Result                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND EXCODE IN ('TZ08','A902','A903','A920') ");
            parameter.AppendSql("   AND Result IS NOT NULL                      ");

            parameter.Add("WRTNO", WRTNO);
            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public HIC_RESULT Read_Result3(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT, ROWID RID       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND PART = '7'              ");

            parameter.Add("WRTNO", WRTNO);
            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> Read_Result_Acitve(long WRTNO, List<string> ACTPART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, ACTIVE       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            if (!ACTPART.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND ACTPART IN (:ACTPART)");
            }

            parameter.Add("WRTNO", WRTNO);
            if (!ACTPART.IsNullOrEmpty())
            {
                parameter.AddInStatement("ACTPART", ACTPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public HIC_RESULT Read_Result_YN(long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, EXCODE, RESULT   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE = :EXCODE            ");
            parameter.AppendSql("   AND Result <> '.'               ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public List<HIC_RESULT> Read_Result(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, ACTIVE                                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT a                                                ");
            parameter.AppendSql("     , KOSMOS_PMPA.Hic_EXCODE b                                                ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                          ");
            parameter.AppendSql("   AND a.ExCode=b.Code(+)                                                      ");
            parameter.AppendSql("   AND a.ExCode IN ('TH11','TH12','TH13','TH15','TH21','TH22','TH23','TH25')   ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public HIC_RESULT Read_Result_YN(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, EXCODE, RESULT   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND ExCode = 'ZE13'             ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public HIC_RESULT Read_Result_Data(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT,RESCODE          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE EXCODE = :GUBUN         ");

            parameter.Add("GUBUN", CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }


        public IList<HIC_RESULT> Read_Result_All(long argWRTNO, string strGubun = "", string[] argCodes = null)
        {
            MParameter parameter = CreateParameter();
            if (strGubun.IsNullOrEmpty() || strGubun == "HIC")
            {
                parameter.AppendSql("SELECT WRTNO, GROUPCODE, PART                  ");
                parameter.AppendSql("     , EXCODE, RESCODE, RESULT                 ");
                parameter.AppendSql("     , PANJENG, OPER_DEPT, OPER_DCT            ");
                parameter.AppendSql("     , ACTIVE, ENTSABUN, ENTTIME               ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
                parameter.AppendSql("  WHERE 1 = 1                                  ");
                parameter.AppendSql("    AND WRTNO = :WRTNO                         ");

                if (!argCodes.IsNullOrEmpty())
                {
                    parameter.AppendSql("    AND EXCODE IN ( :CODES )               ");
                }

                parameter.AppendSql(" ORDER BY EXCODE DESC                          ");
            }
            else if (strGubun == "HEA" || strGubun == "HAPAN")
            {
                parameter.AppendSql("SELECT EXCODE, RESULT                          ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT                  ");
                parameter.AppendSql("  WHERE WRTNO = :WRTNO                         ");

                if (!argCodes.IsNullOrEmpty())
                {
                    parameter.AppendSql("    AND EXCODE IN ( :CODES )               ");
                }
            }

            parameter.Add("WRTNO", argWRTNO);
            if (!argCodes.IsNullOrEmpty())
            {
                parameter.AddInStatement("CODES", argCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int Chk_NonExcute_Result_Count(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                                      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                             ");
            parameter.AppendSql("   AND RESULT IS NULL                                              ");
            parameter.AppendSql("   AND EXCODE NOT IN (                                             ");
            parameter.AppendSql("                       SELECT CODE FROM KOSMOS_PMPA.HIC_EXCODE     ");     //결과입력 점검안하는 코드는 제외
            parameter.AppendSql("                        WHERE DELDATE IS NULL                      ");
            parameter.AppendSql("                          AND GBRESEMPTY = 'N'                     ");
            parameter.AppendSql("                          AND PART NOT IN ('5','9')                ");     //금액코드 제외 + 액팅코드 추가(2021-07-07)
            parameter.AppendSql("                     )                                             ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetResultCount(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                 ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                        ");
            parameter.AppendSql("   AND GjChasu <> '2'                        "); //2차는 제외
            parameter.AppendSql("   AND SUBSTR(ExCode,1,3) IN ('TH5','TH6','TH7','TH8') ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetResultCount_Flag(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND RESULT IS NULL                          ");
            parameter.AppendSql("   AND EXCODE IN ('ZD00','ZD01','LU38','LU54','A291','TX20','TZ45','AA08','A297','A298','A169') ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_RESULT GetResultCount_Blood(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                        ");
            parameter.AppendSql("   AND ExCode IN ('A121','A123','A124','A282') "); 
            parameter.AppendSql("   AND Result IS NOT NULL                      ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<HIC_RESULT>(parameter);
        }

        public int GetResultCount_BMD(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND RESULT IS NULL                          ");
            parameter.AppendSql("   AND EXCODE IN ('TX07')                      ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_RESULT> Read_Result_ (long WRTNO, string ACTPART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, ACTIVE       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND ACTPART IN (:ACTPART)   ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("ACTPART", ACTPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int Update_ResultbyWrtNo_Auto(HIC_RESULT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT      ");
            parameter.AppendSql("   SET RESULT     = :RESULT        ");
            parameter.AppendSql("     , ENTSABUN   = :ENTSABUN      ");
            parameter.AppendSql("     , ENTTIME    = SYSDATE        ");
            parameter.AppendSql(" WHERE WRTNO      = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE     = :EXCODE        ");

            parameter.Add("RESULT", item.RESULT);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("EXCODE", item.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
        public HIC_RESULT GetExCodebyWrtNo_RESULT1(long ArgWRTNO, string ArgExCode, long ArgPP, string ArgGBN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, RESULT, RESCODE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                                  ");
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO                                        ");
            parameter.AppendSql("    AND EXCODE = :EXCODE                                       ");
            parameter.AppendSql("    AND RESULT NOT IN ('미실시','.')                           ");

            parameter.Add("WRTNO", ArgWRTNO);
            parameter.Add("EXCODE", ArgExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public HIC_RESULT GetExCodebyWrtNo_RESULT2(long ArgWRTNO, string ArgExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.WRTNO,A.RESULT,A.RESCODE                             ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_RESULT   A                             ");
            parameter.AppendSql("      , KOSMOS_PMPA.HIC_SUNAPDTL B                             ");
            parameter.AppendSql("  WHERE A.WRTNO  = :WRTNO                                      ");
            parameter.AppendSql("    AND A.EXCODE = :EXCODE                                     ");
            parameter.AppendSql("    AND A.GROUPCODE = B.CODE(+)                                ");
            parameter.AppendSql("    AND B.WRTNO= :WRTNO                                        ");
            parameter.AppendSql("    AND B.GBSELF NOT IN ('2','3')                              ");
            parameter.AppendSql("    AND A.RESULT NOT IN ('미실시','.')                         ");

            parameter.Add("WRTNO", ArgWRTNO);
            parameter.Add("EXCODE", ArgExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);


            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public HIC_RESULT GetExCodebyWrtNo_RESULT3(long ArgWRTNO, string ArgExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, RESULT, RESCODE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT                                  ");
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO                                        ");
            parameter.AppendSql("    AND EXCODE = :EXCODE                                       ");
            parameter.AppendSql("    AND RESULT NOT IN ('미실시','.')                           ");

            parameter.Add("WRTNO", ArgWRTNO);
            parameter.Add("EXCODE", ArgExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        public HIC_RESULT GetExCodebyWrtNo_ExcodeYN(long ArgWRTNO, string ArgExCode, string ArgGUBUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT WRTNO,RESULT,RESCODE                                       ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HIC_RESULT                                     ");
            parameter.AppendSql("  WHERE 1=1                                                        ");
            parameter.AppendSql("    AND A.WRTNO = :WRTNO                                           ");
            parameter.AppendSql("    AND A.ExCode = :EXCODE                                         ");
            if (ArgGUBUN == "2")
            {
                parameter.AppendSql("    AND RESULT IS NOT NULL                                     ");
            }
            parameter.Add("WRTNO", ArgWRTNO);
            parameter.Add("EXCODE", ArgExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_RESULT>(parameter);
        }

        //인계차트 내시경 있는지 점검
        public int GetEndoCountbyWrtNoExCode(long nWrtno, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*) CNT                            ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HEA_RESULT                   ");
            parameter.AppendSql(" WHERE 1=1                                     ");
            parameter.AppendSql(" AND WRTNO = :WRTNO                            ");
            parameter.AppendSql(" AND EXCODE = :EXCODE                          ");

            parameter.Add("WRTNO", nWrtno);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int> (parameter);
        }
    }
}