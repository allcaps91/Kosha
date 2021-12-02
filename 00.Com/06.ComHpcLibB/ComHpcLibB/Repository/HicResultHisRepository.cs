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
    public class HicResultHisRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HicResultHisRepository()
        {
        }

        public int Result_History_Insert(string SABUN, string RESULT, string ROWID, string EXCODE = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RESULT_HIS                                     ");
            parameter.AppendSql("       (JOBTIME, JOBSABUN, WRTNO, EXCODE, RESULT_OLD, RESULT_New)          ");
            parameter.AppendSql("SELECT SYSDATE, :SABUN, WRTNO, EXCODE, RESULT, :RESULT                     ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RESULT                                              ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                        ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                                                  ");
            parameter.AppendSql("   AND RESULT <> :RESULT                                                   ");
            if (!EXCODE.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE = :EXCODE                                                ");
            }

            parameter.Add("SABUN", SABUN);
            parameter.Add("RESULT", RESULT);
            parameter.Add("RID", ROWID);
            if (!EXCODE.IsNullOrEmpty())
            {
                parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public int Result_Update(string RESULT, string PANJENG, string RESCODE, string ENTSABUN, string strRowId, string EXCODE = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET              ");
            parameter.AppendSql("       Result   = :RESULT                      ");
            parameter.AppendSql("     , Panjeng  = :PANJENG                     ");
            parameter.AppendSql("     , RESCODE  = :RESCODE                     ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            if (!RESULT.IsNullOrEmpty())
            {
                parameter.AppendSql("     , Active = 'Y'                        ");
            }
            else
            {
                parameter.AppendSql("     , Active = ''                         ");
            }
            parameter.AppendSql(" WHERE ROWID = :RID                            ");
            //if (!EXCODE.IsNullOrEmpty())
            //{
            //    parameter.AppendSql("   AND EXCODE = :EXCODE                    ");
            //}

            parameter.Add("RESULT", RESULT);
            parameter.Add("PANJENG", PANJENG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE", RESCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", ENTSABUN);
            //if (!EXCODE.IsNullOrEmpty())
            //{
            //    parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            //}
            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNoExCode_Hea(string strResult, string idNumber, long nWrtNo, string strCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE                      ");

            parameter.Add("RESULT", strResult.Replace("'", "`"));
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("EXCODE", strCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyRowId_Hea(string strResult, string strPanjeng, string strResCode, string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            parameter.AppendSql("     , PANJENG  = :PANJENG                     ");
            parameter.AppendSql("     , RESCODE  = :RESCODE                     ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            parameter.AppendSql(" WHERE ROWID    = :RID                         ");

            parameter.Add("RESULT", strResult.Replace("'", "`"));
            parameter.Add("PANJENG", strPanjeng, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Result_History_Insert_Hea(string SABUN, string RESULT, string ROWID, string EXCODE = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESULT_HISTORY                                 ");
            parameter.AppendSql("       (JOBTIME, JOBSABUN, WRTNO, EXCODE, RESULT_OLD, RESULT_NEW)          ");
            parameter.AppendSql("SELECT SYSDATE, :SABUN, WRTNO, EXCODE, RESULT, :RESULT                     ");
            parameter.AppendSql("  From KOSMOS_PMPA.HEA_RESULT                                              ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                        ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                                                  ");
            parameter.AppendSql("   AND RESULT <> :RESULT                                                   ");
            if (!EXCODE.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND EXCODE = :EXCODE                                                ");
            }

            parameter.Add("SABUN", SABUN);
            parameter.Add("RESULT", RESULT);
            parameter.Add("RID", ROWID);
            if (!EXCODE.IsNullOrEmpty())
            {
                parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyWrtNoExCode(string strResult, string idNumber, long nWrtNo, string strCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE                      ");

            parameter.Add("RESULT", strResult.Replace("'", "`"));
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("EXCODE", strCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int UpdatebyRowId(string strResult, string strPanjeng, string strResCode, string idNumber, string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            parameter.AppendSql("     , PANJENG  = :PANJENG                     ");
            parameter.AppendSql("     , RESCODE  = :RESCODE                     ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            parameter.AppendSql(" WHERE ROWID    = :RID                         ");

            parameter.Add("RESULT", strResult.Replace("'", "`"));
            parameter.Add("PANJENG", strPanjeng, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("RESCODE", strResCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN", idNumber);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Result_History_Insert2(string SABUN, string RESULT, long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_RESULT_HISTORY                                 ");
            parameter.AppendSql("       (JOBTIME,JOBSABUN,WRTNO,EXCODE,RESULT_OLD,RESULT_NEW)               ");
            parameter.AppendSql("SELECT SYSDATE, :SABUN, WRTNO, EXCODE, RESULT, replace(:RESULT, ''', '`')  ");
            parameter.AppendSql("  From KOSMOS_PMPA.HIC_RESULT                                              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");
            parameter.AppendSql("   AND EXCODE = :EXCODE                                                    ");
            parameter.AppendSql("   AND RESULT IS NOT NULL                                                  ");
            parameter.AppendSql("   AND Result != :RESULT                                                   ");

            parameter.Add("SABUN", SABUN);
            parameter.Add("RESULT", RESULT);
            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char); 

            return ExecuteNonQuery(parameter);
        }

        public int Result_History_Update(string RESULT, string ENTSABUN, long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET              ");
            parameter.AppendSql("       RESULT   = replace(:RESULT, ''', '`')   ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN                    ");
            parameter.AppendSql("     , ENTTIME  = SYSDATE                      ");
            parameter.AppendSql("     , ACTIVE   = 'Y'                          ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE                      ");

            parameter.Add("RESULT", RESULT);
            parameter.Add("ENTSABUN", ENTSABUN);
            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int Result_History_Update2(double RESULT, long WRTNO, string EXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_RESULT SET              ");
            parameter.AppendSql("       RESULT   = :RESULT                      ");
            parameter.AppendSql(" WHERE WRTNO    = :WRTNO                       ");
            parameter.AppendSql("   AND EXCODE   = :EXCODE                      ");

            parameter.Add("RESULT", RESULT);
            parameter.Add("WRTNO", WRTNO);
            parameter.Add("EXCODE", EXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
    }
}
