namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanhisRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanhisRepository()
        {
        }

        public int GetCountbyWrtNoMCodePyoJanggi(long fnWrtNo, string strMCode, string strPyoJangi)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANHIS              ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO                      ");
            parameter.AppendSql("   AND MCODE     = :MCODE                      ");
            parameter.AppendSql("   AND PYOJANGGI = :PYOJANGGI                  ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("MCODE", strMCode);
            parameter.Add("PYOJANGGI", strPyoJangi);            

            return ExecuteScalar<int>(parameter);
        }

        public HIC_SPC_PANHIS GetItembyWrtNoMCodePyojanggi(long fnWrtNo, string strMCode, string strPyoJangi)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE,MCODE,PANJENG     ");
            parameter.AppendSql("     , SOGENCODE,JOCHICODE,WORKYN,SAHUCODE,MSYMCODE,MBUN,UCODE         ");
            parameter.AppendSql("     , SOGENREMARK,JOCHIREMARK,REEXAM,WRTNO,ROWID AS RID               ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANHIS                                      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                                ");
            parameter.AppendSql("   AND MCODE = :MCODE                                                  ");
            parameter.AppendSql("   AND PYOJANGGI = :PYOJANGGI                                          ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("MCODE", strMCode);
            parameter.Add("PYOJANGGI", strPyoJangi);

            return ExecuteReaderSingle<HIC_SPC_PANHIS>(parameter);
        }

        public int GetPanRbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(DECODE(Panjeng,'7',1,0)) PAN_R          ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANHIS                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANHIS                  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND MCODE NOT IN ('ZZZ')                        ");
            parameter.AppendSql("   AND PANJENG = '7'                               "); //R판정
            parameter.AppendSql("   AND DELDATE IS NULL                             ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int DeletebyJepDate(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_SPC_PANHIS                                      ");
            parameter.AppendSql(" WHERE Panjeng = '7'                                                   ");
            parameter.AppendSql("   AND WRTNO IN (SELECT WRTNO FROM ADMIN.HIC_SPC_PANJENG         ");
            parameter.AppendSql("                  WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql("                    AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql("                    AND Panjeng = '8'                                  "); //U판정
            parameter.AppendSql("                    AND SOGENREMARK = '기간내 미실시'                  ");
            parameter.AppendSql("                )                                                      ");    

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteNonQuery(parameter);
        }

        public int DeleteByRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_SPC_PANHIS  ");
            parameter.AppendSql(" WHERE ROWID = :ROWID              ");

            parameter.Add("ROWID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_SPC_PANHIS GetItembyRowId(string fstrPROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE   ");
            parameter.AppendSql("     , JOCHICODE,WORKYN,SAHUCODE,SAHUREMARK,MSYMCODE,MBUN,UCODE,SOGENREMARK,JOCHIREMARK    ");
            parameter.AppendSql("     , REEXAM,ORCODE,ORSAYUCODE                                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANHIS                                                          ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                        ");

            parameter.Add("RID", fstrPROWID);

            return ExecuteReaderSingle<HIC_SPC_PANHIS>(parameter);
        }

        public List<HIC_SPC_PANHIS> GetItembyWrtNoRowId(long fnWrtNo, List<string> strList)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE,MCODE,PANJENG     ");
            parameter.AppendSql("     , SOGENCODE,JOCHICODE,WORKYN,SAHUCODE,MSYMCODE,MBUN,UCODE         ");
            parameter.AppendSql("     , SOGENREMARK,JOCHIREMARK,REEXAM,WRTNO,ROWID AS RID                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANHIS                                      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");
            parameter.AppendSql("   AND ROWID IN (:RID)                                                 ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.AddInStatement("RID", strList);

            return ExecuteReader<HIC_SPC_PANHIS>(parameter);
        }

        public List<HIC_SPC_PANHIS> GetMCodeRowIdbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MCODE, ROWID AS RID                           ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANHIS              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')        ");
            parameter.AppendSql(" ORDER BY MCODE                                ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_SPC_PANHIS>(parameter);
        }

        public int Insert(string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_SPC_PANHIS                                                     ");
            parameter.AppendSql("       (WRTNO,JEPDATE,LTDCODE,PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE              ");
            parameter.AppendSql("     , JOCHICODE,WORKYN,SAHUCODE,MSYMCODE,MBUN,UCODE,SOGENREMARK,JOCHIREMARK,REEXAM        ");
            parameter.AppendSql("     , ENTSABUN,ENTTIME,DELDATE,ORCODE,ORSAYUCODE,SAHUREMARK,GJCHASU,LTDCODE2,PYOJANGGI)   ");
            parameter.AppendSql("SELECT WRTNO                                                                               ");
            parameter.AppendSql("     , JEPDATE,LTDCODE,PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE                     ");
            parameter.AppendSql("     , JOCHICODE,WORKYN,SAHUCODE,MSYMCODE,MBUN,UCODE,SOGENREMARK,JOCHIREMARK,REEXAM        ");
            parameter.AppendSql("     , ENTSABUN,ENTTIME,DELDATE,ORCODE,ORSAYUCODE,SAHUREMARK,GJCHASU,LTDCODE2,PYOJANGGI    ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANJENG                                                         ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                        ");

            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }
    }
}
