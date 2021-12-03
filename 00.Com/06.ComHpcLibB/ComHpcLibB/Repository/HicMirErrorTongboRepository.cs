namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicMirErrorTongboRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirErrorTongboRepository()
        {
        }

        public List<HIC_MIR_ERROR_TONGBO> GetListByItems(long ArgWRTNO, string ArgFDATE, string ArgTDATE, string ArgGUBUN, string ArgSNAME)
        {

            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO, TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate,b.SName,b.Sex,b.Age,b.GjJong,b.JobSabun        ");
            parameter.AppendSql(" ,a.Gubun,a.Remark,a.DamName,TO_CHAR(a.TongboDate,'YYYY-MM-DD') TongboDate                             ");
            parameter.AppendSql(" ,TO_CHAR(a.OkDate,'YYYY-MM-DD') OkDate,a.Gubun,a.ActMemo,a.ROWID, b.ptno, b.PRTSABUN                  ");
            parameter.AppendSql(" FROM ADMIN.HIC_MIR_ERROR_TONGBO a,ADMIN.HIC_JEPSU b                                       ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                           ");
            parameter.AppendSql(" AND A.WRTNO = B.WRTNO                                                                                 ");
            
            if (ArgGUBUN =="1")
            {
                parameter.AppendSql("   AND A.TONGBODATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                               ");
                parameter.AppendSql("   AND A.TONGBODATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                               ");
            }
            else if (ArgGUBUN =="3")
            {
                parameter.AppendSql("   AND A.TONGBODATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                               ");
                parameter.AppendSql("   AND A.TONGBODATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                               ");
                parameter.AppendSql("   AND A.OKDATE IS NULL                                                                            ");
            }
            else
            {
                parameter.AppendSql("   AND A.OKDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                   ");
                parameter.AppendSql("   AND A.OKDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                                   ");
            }

            if (ArgWRTNO > 0)
            {
                parameter.AppendSql("   AND A.WRTNO = :WRTNO                                                                            ");
            }

            if (!ArgSNAME.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.SNAME = :SNAME                                                                            ");
            }
           
            parameter.Add("FDATE", ArgFDATE);
            parameter.Add("TDATE", ArgTDATE);
            if (ArgWRTNO > 0)
            {
                parameter.Add("WRTNO", ArgWRTNO);
            }

            if (!ArgSNAME.IsNullOrEmpty())
            {
                parameter.Add("SNAME", ArgSNAME);
            }

            return ExecuteReader<HIC_MIR_ERROR_TONGBO>(parameter);
        }

        public int Insert(long nWRTNO, long nMirno, string strSName, string strSex, long nAge, string strGubun, string strRemark)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_MIR_ERROR_TONGBO                               ");            
            parameter.AppendSql("       (WRTNO, TONGBODATE ,MIRNO, SNAME, SEX, AGE, GUBUN,REMARK)           ");
            parameter.AppendSql("VALUES                                                                     ");
            parameter.AppendSql("       (:WRTNO, TRUNC(SYSDATE) ,:MIRNO, :SNAME, :SEX,                      ");
            parameter.AppendSql("       :AGE, :GUBUN, :REMARK)                                              ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("MIRNO", nMirno);
            parameter.Add("SNAME", strSName);
            parameter.Add("SEX", strSex, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", nAge);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REMARK", strRemark);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNoGubunRemark(long nWRTNO, string strGubun, string strRemark)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_MIR_ERROR_TONGBO                                ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                 ");
            parameter.AppendSql("   AND GUBUN  = :GUBUN                                                 ");
            parameter.AppendSql("   AND REMARK = :REMARK                                                ");
            parameter.AppendSql("   AND OKDATE IS NULL                                                  ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GUBUN", strGubun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("REMARK", strRemark);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyMirNo(long fnMirNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_MIR_ERROR                                       ");
            parameter.AppendSql(" WHERE MIRNO = :MIRNO                                                  ");

            parameter.Add("MIRNO", fnMirNo);

            return ExecuteScalar<int>(parameter);
        }

        public int UPDATE_OKDATE(string strCHK, string strMEMO1, string strROWID, string strJOBNAME)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_MIR_ERROR_TONGBO                ");
            if (strCHK == "True")
            {
                parameter.AppendSql("   SET OKDATE     = SYSDATE                        ");
            }
            else
            {
                parameter.AppendSql("   SET OKDATE     = ''                             ");
            }
            
            parameter.AppendSql("     , DAMNAME   = :DAMNAME                            ");
            parameter.AppendSql("     , ACTMEMO     = :MEMO1                            ");
            parameter.AppendSql(" WHERE ROWID      = :RID                               ");

            parameter.Add("DAMNAME", strJOBNAME);
            parameter.Add("MEMO1", strMEMO1);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);

        }
    }

}
