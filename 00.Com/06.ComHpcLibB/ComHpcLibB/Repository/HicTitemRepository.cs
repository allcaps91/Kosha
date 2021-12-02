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
    public class HicTitemRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicTitemRepository()
        {
        }

        public List<HIC_TITEM> GetJumsuCodebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUMSU, CODE                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND Gubun = '13'                        ");
            parameter.AppendSql("   AND Jumsu IN (910,911,912,913,914,915)  ");
            parameter.AppendSql(" ORDER BY Jumsu                            ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public int GetCountbyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND GUBUN = '18'            ");
            parameter.AppendSql("   AND CODE >= '80902'         ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNoGubunCode(long fnWrtNo, string strGubun, string[] strCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE IN (:CODE)         ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("GUBUN", strGubun);
            parameter.AddInStatement("CODE", strCodes, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_TITEM GetRowIdbyGubun(int strGubun, string strCODE, long nWRTNO, long fnPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID FROM KOSMOS_PMPA.HIC_TITEM        ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                          ");
            parameter.AppendSql("   AND CODE  = :CODE                           ");
            if (nWRTNO > 0)
            {
                parameter.AppendSql("  AND WRTNO = :WRTNO                       ");
            }
            else
            {
                parameter.AppendSql("  AND PANO = :PANO                         ");
            }

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (nWRTNO > 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }
            else
            {
                parameter.Add("PANO", fnPano);
            }

            return ExecuteReaderSingle<HIC_TITEM>(parameter);
        }

        public int DeletebyWrtNoPaNo(long nWRTNO, long fnPano, string strGubun, int nJumsu)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_TITEM   ");
            if (nWRTNO > 0)
            {
                parameter.AppendSql(" WHERE WRTNO = :WRTNO      ");
            }
            else
            {
                parameter.AppendSql(" WHERE PANO = :PANO        ");
            }
            parameter.AppendSql("   AND GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND JUMSU > :JUMSU          ");

            if (nWRTNO > 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }
            else
            {
                parameter.Add("PANO", fnPano);
            }
            parameter.Add("GUBUN", strGubun);
            parameter.Add("JUMSU", nJumsu);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubunCode(long fnWrtNo, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, GUBUN, CODE, JUMSU               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                          ");
            if (strGubun == "11")
            {
                parameter.AppendSql("   AND CODE LIKE '101%'                    ");
            }
            parameter.AppendSql(" ORDER BY GUBUN, CODE                          ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("GUBUN", strGubun);

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubunJumsu2(long argWRTNO, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, GUBUN, CODE, JUMSU               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                          ");
            parameter.AppendSql("   AND JUMSU < 900                             ");
            parameter.AppendSql(" GROUP BY  WRTNO, GUBUN, CODE, JUMSU           ");
            parameter.AppendSql(" ORDER BY CODE                                 ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("GUBUN", argGubun);

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubun(long argWRTNO, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, GUBUN, CODE, JUMSU               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND GUBUN = :GUBUN                          ");
            parameter.AppendSql(" GROUP BY WRTNO, GUBUN, CODE, JUMSU            ");
            parameter.AppendSql(" ORDER BY CODE                                 ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("GUBUN", argGubun);

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubunJumsu(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, GUBUN, CODE, JUMSU, PANO         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND GUBUN = '13'                            ");
            parameter.AppendSql("   AND JUMSU >= 901                            ");
            parameter.AppendSql(" ORDER BY JUMSU                                ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public HIC_TITEM GetJumsubyGubunWrtNo(int i, long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(JUMSU) TOTAL        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND GUBUN = :GUBUN          ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GUBUN", i);

            return ExecuteReaderSingle<HIC_TITEM>(parameter);
        }

        public string GetRowIdRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_TITEM> GetRowIdbyGubunWrtNoPaNo(string strGubun, int nJumsu, long nWRTNO, long fnPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND JUMSU > :JUMSU          ");
            if (nWRTNO > 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO      ");
            }
            else
            {
                parameter.AppendSql("   AND PANO = :PANO        ");
            }

            parameter.Add("GUBUN", strGubun);
            parameter.Add("JUMSU", nJumsu);
            if (nWRTNO > 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }
            else
            {
                parameter.Add("PANO", fnPano);
            }

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public List<HIC_TITEM> GetRowId(long nWRTNO, long FnPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE Gubun = '13'            ");
            parameter.AppendSql("   AND JUMSU > 900             ");
            if (nWRTNO > 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO      ");
            }
            else
            {
                parameter.AppendSql("   AND PANO = :PANO        ");
            }

            if (nWRTNO > 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }
            else
            {
                parameter.Add("PANO", FnPano);
            }

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public int UpdateCodePaNobyWrtNoPaNo(string strCode, long fnPano, long nWRTNO, string strGubun, int nJumsu)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_TItem SET       ");
            parameter.AppendSql("       CODE  = :CODE                   ");
            parameter.AppendSql("     , PANO  = :PANO                   ");
            if (nWRTNO > 0)
            {
                parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            }
            else
            {
                parameter.AppendSql(" WHERE PANO = :PANO                ");
            }
            parameter.AppendSql("   AND GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND JUMSU = :JUMSU                  ");

            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANO", fnPano);
            if (nWRTNO > 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }
            parameter.Add("GUBUN", strGubun);
            parameter.Add("JUMSU", nJumsu);

            return ExecuteNonQuery(parameter);
        }

        public int Update(string strCODE, long nWRTNO, long fnPano, long nJumsu, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_TItem SET       ");
            parameter.AppendSql("       CODE  = :CODE                   ");
            parameter.AppendSql("     , WRTNO = :WRTNO                  ");
            parameter.AppendSql("     , PANO  = :PANO                   ");
            parameter.AppendSql("     , JUMSU = :JUMSU                  ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("CODE", strCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("PANO", fnPano);
            parameter.Add("JUMSU", nJumsu);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Delete(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");

            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(long nWRTNO, string strGubun, string strCODE, long nJumsu, long fnPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_TITEM                  ");
            parameter.AppendSql("       (WRTNO, GUBUN, CODE, JUMSU, PANO)           ");
            parameter.AppendSql("VALUES                                             ");
            parameter.AppendSql("       (:WRTNO, :GUBUN, :CODE, :JUMSU, :PANO)      ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMSU", nJumsu);
            parameter.Add("PANO", fnPano);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyWrtNoGuybun(long nWRTNO, int nGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND GUBUN = :GUBUN          ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("GUBUN", nGubun);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_TITEM> GetGubunCodeJumsubyWrtNo(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GUBUN, CODE, JUMSU      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND GUBUN > '16'            ");
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public List<HIC_TITEM> GetGubunCodeJumsubyWrtNo(long argWrtNo, string argGubun, long argJumsu, string sGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GUBUN, CODE, JUMSU      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");                        
            if (sGbn.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GUBUN < :GUBUN      ");
                parameter.AppendSql("   AND JUMSU < :JUMSU      ");
            }
            else    //운동, 비만
            {
                parameter.AppendSql("   AND GUBUN = :GUBUN      ");
                parameter.AppendSql("   AND JUMSU > :JUMSU      ");
            }
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("GUBUN", argGubun);
            parameter.Add("JUMSU", argJumsu);

            return ExecuteReader<HIC_TITEM>(parameter);
        }

        public long GetTotalbyGubunWrtNo(int nGubun, object fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(JUMSU) TOTAL        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND GUBUN = :GUBUN          ");

            parameter.Add("GUBUN", nGubun);
            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteScalar<long>(parameter);
        }

        public string GetCodebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO          ");
            parameter.AppendSql("   AND GUBUN = '13'            ");
            parameter.AppendSql("   AND CODE LIKE '318%'        ");
            parameter.AppendSql(" ORDER BY CODE                 ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_TITEM SET               ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public string GetItembyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_TITEM   ");
            parameter.AppendSql(" WHERE 1= 1                    ");
            parameter.AppendSql("   AND WRTNO = :WRTNO          ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }
    }
}
