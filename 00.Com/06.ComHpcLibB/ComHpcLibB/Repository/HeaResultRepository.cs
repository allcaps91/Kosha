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
    public class HeaResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResultRepository()
        {
        }

        public int UpdateResultbyWrtNoExCode(string argResult, string strResCode, string idNumber, long argWrtNo, string argExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT          ");
            parameter.AppendSql("   SET RESULT   = :RESULT              ");
            parameter.AppendSql("     , RESCODE  = :RESCODE             ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE              ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                  ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE              ");

            parameter.Add("RESULT", argResult);
            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("EXCODE", argExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_RESULT> GetExCodeResult(long fnHeaWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXCODE, RESULT                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND Result IS NOT NULL              ");

            parameter.Add("WRTNO", fnHeaWRTNO);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public string GetRowidByOneExcodeWrtno(string argExcode, long argwRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID            ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO         ");
            parameter.AppendSql("   AND EXCODE = :EXCODE        ");

            parameter.Add("WRTNO", argwRTNO);
            parameter.Add("EXCODE", argExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateResultActivebyWrtNoExCode(string strResult, string strActive, string idNumber, long fnWRTNO, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT          ");
            parameter.AppendSql("   SET RESULT   = :RESULT              ");
            parameter.AppendSql("     , ACTIVE   = :ACTIVE              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = :SYSDATE             ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE              ");
            parameter.AppendSql("   AND RESULT IS NULL                  ");

            parameter.Add("RESULT", strResult);
            parameter.Add("ACTIVE", strActive, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", fnWRTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateResultActivebyWrtNoExCode(string strResult, string strActive, long fnWRTNO, string strExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT          ");
            parameter.AppendSql("   SET RESULT   = :RESULT              ");
            parameter.AppendSql("     , ACTIVE   = :ACTIVE              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN            ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE             ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE              ");

            parameter.Add("RESULT", strResult);
            parameter.Add("ACTIVE", strActive, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", clsType.User.IdNumber.To<long>());
            parameter.Add("WRTNO", fnWRTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int Result_History_Insert(string idNumber, string strResult, string strROWID, string strExCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HEA_RESULT_HIS                                     ");
            parameter.AppendSql("       (ENTTIME, ENTSABUN, WRTNO, EXCODE, RESULT)                          ");
            parameter.AppendSql("SELECT SYSDATE, :SABUN, WRTNO, EXCODE, RESULT                              ");
            parameter.AppendSql("  From ADMIN.HEA_RESULT                                              ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                        ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                                                  ");
            parameter.AppendSql("   AND RESULT <> :RESULT                                                   ");
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE = :EXCODE                                                ");
            }

            parameter.Add("SABUN", idNumber);
            parameter.Add("RESULT", strResult);
            parameter.Add("RID", strROWID);
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNoHaRoom(long argWrtNo, string strRoom)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT A    ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE B    ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO            ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)        ");
            if (!strRoom.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND Trim(B.HAROOM) = :HAROOM    ");
            }

            parameter.Add("WRTNO", argWrtNo);
            if (!strRoom.IsNullOrEmpty())
            {
                parameter.Add("HAROOM", strRoom);
            }

            return ExecuteScalar<int>(parameter);
        }

        public List<HEA_RESULT> GetListByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.ROWID AS RID, a.EXCODE, a.RESULT, a.PANJENG       ");
            parameter.AppendSql("     , b.MIN_M, b.MAX_M, b.MIN_F, b.MAX_F                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                            ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                            ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                    ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public string GetActiveByWrtnoExCode(long wRTNO, string eXCODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ACTIVE                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO            ");
            parameter.AppendSql("   AND EXCODE = :EXCODE          ");

            parameter.Add("WRTNO", wRTNO);
            parameter.Add("EXCODE", eXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HEA_RESULT GetResultBYWrtnoExCodeIN(long nWRTNO, List<string> strExcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)     ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("EXCODE", strExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESULT>(parameter);
        }

        public int UpdateSangdambyWrtnoExCode(long fnWRTNO, string strExam, string strSangDam)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT SET  ");
            parameter.AppendSql("       SANGDAM = :SANGDAM          ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO            ");
            parameter.AppendSql("   AND EXCODE  = :EXCODE           ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("SANGDAM", strSangDam);
            parameter.Add("EXCODE", strExam, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpDateResultReadTimeByItemExCodeIN(HEA_RESULT item, List<string> strExcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT SET                      ");
            parameter.AppendSql("       RESULT = :RESULT                                ");
            parameter.AppendSql("     , ACTIVE = :ACTIVE                                ");
            if (!item.READTIME.IsNullOrEmpty())
            {
                parameter.AppendSql("     , READTIME = TO_DATE(:READTIME, 'YYYY-MM-DD') ");
            }
            parameter.AppendSql("     , ENTTIME  = SYSDATE                              ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                             ");

            parameter.Add("RESULT", item.RESULT);
            parameter.Add("ACTIVE", item.ACTIVE);

            if (!item.READTIME.IsNullOrEmpty())
            {
                parameter.Add("READTIME", item.READTIME);
            }
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.AddInStatement("EXCODE", strExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpDateResultPanjengByRid(string strNewPan, string argRid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT        ");
            parameter.AppendSql("   SET PANJENG   = :PANJENG          ");
            parameter.AppendSql(" WHERE ROWID    = :RID               ");

            parameter.Add("PANJENG", strNewPan);
            parameter.Add("RID", argRid);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_RESULT> GetExCodeResultbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE,Result           ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> GetExCodeResultbyWrtNo1(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE,Result           ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql(" AND RESULT IS NOT NULL        ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public HEA_RESULT GetEntSabunbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(ENTTIME), ENTSABUN  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND PART <> '5'             ");
            parameter.AppendSql("   AND ENTTIME IS NOT NULL     ");
            parameter.AppendSql(" GROUP BY ENTSABUN             ");
            parameter.AppendSql(" ORDER BY MAX(ENTTIME) DESC    ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReaderSingle<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> Read_ResultAct(long fnWRTNO, string hEAPART)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT                                                          "); 
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT A, ADMIN.HIC_EXCODE B              ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                ");
            parameter.AppendSql("   AND A.Part = '5'                                                    "); //ACT코드만
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                            ");
            parameter.AppendSql("   AND Trim(B.HEAPART) = :HEAPART                                      ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("HEAPART", hEAPART);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public int GetCountbyWrtNo(long argWrtNo, string strRoom)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a        ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b        ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)            ");
            parameter.AppendSql("   AND A.Active = 'Y'                  ");
            parameter.AppendSql("   AND Trim(B.HeaPART) IN(SELECT TRIM(HEAPART) FROM ADMIN.HIC_EXCODE WHERE HAROOM = :HAROOM)  ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("HAROOM", strRoom);

            return ExecuteScalar<int>(parameter);
        }

        public string IsActiveByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND ACTIVE IS NOT NULL              ");
            parameter.AppendSql("   AND PART NOT IN ('5')               ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_RESULT> GetListByWrtnoExCodeIN(long fnWRTNO, List<string> lstExCD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXCODE, RESULT                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)             ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.AddInStatement("EXCODE", lstExCD, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public HEA_RESULT GetOneItemByExCodeWrtno(string argExCode, long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,EXCODE,PART,RESCODE,PANJENG,RESULT,SANGDAM    ");
            parameter.AppendSql("      ,ACTIVE, ACTPART, ENTSABUN, ENTTIME, READTIME, ROWID AS RID        ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT                              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("   AND EXCODE =:EXCODE                                     ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("EXCODE", argExCode);

            return ExecuteReaderSingle<HEA_RESULT>(parameter);
        }

        public void DeleteByRowid(string argRid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE ADMIN.HEA_RESULT  WHERE ROWID = :RID  ");

            parameter.Add("RID", argRid);

            ExecuteNonQuery(parameter);
        }

        public void InsertDelSelectbyRowid(string argRid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HEA_RESULT_HIS                         ");
            parameter.AppendSql("     ( WRTNO,EXCODE,RESULT,ACTIVE,GUBUN,ENTSABUN,ENTTIME )     ");
            parameter.AppendSql(" SELECT WRTNO,EXCODE,RESULT,ACTIVE,'D',:ENTSABUN,SYSDATE      ");
            parameter.AppendSql("   FROM ADMIN.HEA_RESULT                                                         ");
            parameter.AppendSql("  WHERE ROWID = :RID                                                                ");

            parameter.Add("RID", argRid);
            parameter.Add("ENTSABUN", clsType.User.IdNumber.To<long>());

            ExecuteNonQuery(parameter);
        }

        public List<HEA_RESULT> GetAllByWrtNo(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, RESCODE, SANGDAM, ACTIVE, ACTPART, PANJENG   ");
            parameter.AppendSql("     , EXCODE, RESULT, ROWID AS RID                        ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT                              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public string GetRowidByWrtnoExCodeIN(long wRTNO, List<string> lstExCodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID                ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO             ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODES)        ");

            parameter.Add("WRTNO", wRTNO);
            parameter.AddInStatement("EXCODES", lstExCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_RESULT> Read_Result(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT                          ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND EXCODE IN ('TZ08','A902','A903','A920') ");
            parameter.AppendSql("   AND Result IS NOT NULL                      ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HIC_RESULT> Read_Result_Acitve(long nWrtNo, List<string> fstrPartG)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, ACTIVE       ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND ACTPART IN (:ACTPART)   ");

            parameter.Add("WRTNO", nWrtNo);
            if (!fstrPartG.IsNullOrEmpty())
            {
                parameter.AddInStatement("ACTPART", fstrPartG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_RESULT>(parameter);
        }

        public int Result_Update(string strResult, string strPanjeng, string strResCode, string idNumber, string strRowId, string strExCode = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            if (!strPanjeng.IsNullOrEmpty())
            {
                parameter.AppendSql("     , PANJENG  = :PANJENG                 ");
            }
            parameter.AppendSql("     , RESCODE  = :RESCODE                     ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            if (!strResult.IsNullOrEmpty())
            {
                parameter.AppendSql("     , ACTIVE = 'Y'                        ");
            }
            else
            {
                parameter.AppendSql("     , ACTIVE = ''                         ");
            }
            parameter.AppendSql(" WHERE ROWID = :RID                            ");
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE = :EXCODE                    ");
            }

            parameter.Add("RESULT", strResult);
            if (!strPanjeng.IsNullOrEmpty())
            {
                parameter.Add("PANJENG", strPanjeng, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strRowId); 
            if (!strExCode.IsNullOrEmpty())
            {
                parameter.Add("EXCODE", strExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_RESULT> Get_Results(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE,RESULT                           ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> GetItembyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT RESULT, EXCODE, SANGDAM         ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("  AND (RESULT IS NULL OR RESULT = '')  ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> Read_Active(long fnWRTNO, string hEAPART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ACTIVE                                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT A                ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE B                ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND A.EXCODE NOT IN ('A101','A102','A103')  "); //ACT코드만
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                    ");
            if (hEAPART != "")
            {
                parameter.AppendSql("   AND Trim(B.HeaPART) = :HEAPART          ");
            }
            parameter.AppendSql("   AND(a.Active = '' or a.Active is null)      ");

            parameter.Add("WRTNO", fnWRTNO);
            if (hEAPART != "")
            {
                parameter.Add("HEAPART", hEAPART.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> Read_Result2(long fnWRTNO, string hEAPART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Result                                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT A                ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE B                ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND A.Part = '5'                            "); //ACT코드만
            parameter.AppendSql("   AND WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND A.Result IS NULL                        "); 
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                    ");
            parameter.AppendSql("   AND Trim(B.HeaPART) = :HEAPART              ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("HEAPART", hEAPART.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> Read_Result(long fnWRTNO, string hEAPART)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Result                          ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT A        ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE B        ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql("   AND WRTNO    = :WRTNO               ");
            parameter.AppendSql("   AND A.Part   = '5'                  "); //ACT코드만
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)            ");
            parameter.AppendSql("   AND Trim(B.HeaPART) = :HEAPART      ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("HEAPART", hEAPART.Trim(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> GetExCodeResultbyWrtNoExCode(long argHcWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT                      ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND EXCODE IN ('A101','A102')           ");

            parameter.Add("WRTNO", argHcWRTNO);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> GetActiveExCodebyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Active, a.EXCODE                                      ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a, ADMIN.HIC_EXCODE b      ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                        ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE                                       ");
            parameter.AppendSql("   AND b.Part = '1'                                            "); //신체계측구분

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public List<HEA_RESULT> GetExCodebyWrtNo_All(long wRTNO, List<string> lstInstr)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT EXCODE, RESULT                                  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT                          ");
            parameter.AppendSql("  WHERE WRTNO = :WRTNO                                 ");
            parameter.AppendSql("    AND EXCODE IN (:EXCODE)                            ");
            parameter.AppendSql("  ORDER By EXCODE                                      ");

            parameter.Add("WRTNO", wRTNO);
            parameter.AddInStatement("EXCODE", lstInstr, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_RESULT>(parameter);
        }

        public int GetCountbyPtNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT FROM HEA_RESULT                              ");
            parameter.AppendSql(" WHERE WRTNO IN (SELECT WRTNO FROM ADMIN.HEA_JEPSU           ");
            parameter.AppendSql("                  WHERE PTNO = :PTNO                               ");
            parameter.AppendSql("                    AND SDate=TRUNC(SYSDATE)                       ");
            parameter.AppendSql("                    AND DelDate IS NULL)                           ");
            parameter.AppendSql("   AND EXCODE IN('TX20', 'TX64', 'TX41')                           ");

            parameter.Add("PTNO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public void InSert(HEA_RESULT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HEA_RESULT (                               ");
            parameter.AppendSql("       WRTNO,EXCODE,PART,RESCODE,PANJENG,RESULT,ACTPART            ");
            parameter.AppendSql(" ) VALUES (                                                        ");
            parameter.AppendSql("       :WRTNO,:EXCODE,:PART,:RESCODE,:PANJENG,:RESULT,:ACTPART )   ");

            parameter.Add("WRTNO",      item.WRTNO); 
            parameter.Add("EXCODE",     item.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PART",       item.PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE",    item.RESCODE);
            parameter.Add("PANJENG",    item.PANJENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESULT",     item.RESULT);
            parameter.Add("ACTPART",    item.ACTPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            ExecuteNonQuery(parameter);
        }

        public void UpDatePartResCodeByExCodeWrtno(HEA_RESULT hRES)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT          ");
            parameter.AppendSql("   SET RESCODE = :RESCODE              ");
            parameter.AppendSql("     , ACTPART = :ACTPART              ");
            parameter.AppendSql("     , PART    = :PART                 ");
            parameter.AppendSql(" WHERE EXCODE  = :EXCODE               ");
            parameter.AppendSql("   AND WRTNO   = :WRTNO                ");

            parameter.Add("RESCODE", hRES.RESCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ACTPART", hRES.ACTPART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PART", hRES.PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("EXCODE", hRES.EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", hRES.WRTNO);

            ExecuteNonQuery(parameter);
        }

        public long GetCountByWrtnoInExcodeIn(List<long> lstHeaWrtno, List<string> lstExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*) CNT                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO IN (:WRTNO)               ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)             ");

            parameter.AddInStatement("WRTNO", lstHeaWrtno);
            parameter.AddInStatement("EXCODE", lstExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<long>(parameter);
        }

        public string ChkExamByWrtnoExCode(long nWRTNO, string argExCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND EXCODE =:EXCODE                 ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", argExCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyWrtNoExCode(long nWRTNO, List<string> strACT_SET_Data)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND EXCODE IN (:EXCODE)                 ");
            //parameter.AppendSql("   AND EXCODE = :EXCODE                    ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                  ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("EXCODE", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            //parameter.Add("EXCODE", strACT_SET_Data, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }


        public List<HEA_RESULT> GetListByWrtnoGubun(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.ACTPART                       ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT A        ");
            parameter.AppendSql("     , ADMIN.HEA_CODE   B        ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                ");
            parameter.AppendSql("   AND TRIM(A.ACTPART) = B.NAME(+)     ");
            parameter.AppendSql("   AND A.ACTPART IS NOT NULL           ");
            parameter.AppendSql("   AND B.GUBUN = '12'                  ");
            parameter.AppendSql(" GROUP BY A.ACTPART, B.CODE            ");
            parameter.AppendSql(" ORDER BY B.CODE                       ");

            parameter.Add("WRTNO", argWrtno);
            

            return ExecuteReader<HEA_RESULT>(parameter);
        }


        public HEA_RESULT Read_Result_YN(long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID, EXCODE, RESULT   ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND EXCODE = :EXCODE            ");
            //parameter.AppendSql("   AND Result <> '.'               ");

            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_RESULT>(parameter);
        }

        		
		public int Result_Update2(double RESULT, long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE                      ");

            parameter.Add("RESULT", RESULT);
            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }


        public int UpdateResCode(string strResCode, string strResult, long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_RESULT SET  ");
            parameter.AppendSql("       RESCODE  = :RESCODE         ");
            parameter.AppendSql("     , RESULT   = :RESULT          ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO           ");
            parameter.AppendSql("   AND EXCODE   = 'A151'           "); //심전도

            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESULT", strResult);
            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
