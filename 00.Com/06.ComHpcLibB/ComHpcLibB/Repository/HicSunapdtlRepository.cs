namespace ComHpcLibB.Repository
{
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicSunapdtlRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicSunapdtlRepository()
        {
        }

        public List<HIC_SUNAPDTL> Read_UCode(string PTNO, string JEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT UCode                                                       ");
            parameter.AppendSql("     , ROWID RID                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                                    ");
            parameter.AppendSql(" WHERE WRTNO IN (                                                  ");
            parameter.AppendSql("                SELECT WRTNO                                       ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_JEPSU                       ");
            parameter.AppendSql("                 WHERE PTNO = :PTNO                                ");
            parameter.AppendSql("                   AND JEPDATE = TO_DATE(:JEPDATE,'YYYY-MM-DD')    ");
            parameter.AppendSql("                   AND DELDATE IS NULL                             ");
            parameter.AppendSql("               )                                                   ");
            parameter.AppendSql("   AND UCode IN ('L06','L07')                                      ");

            parameter.Add("PTNO", PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JEPDATE", JEPDATE);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public int GetCountbyWrtNoCode(long fnWRTNO, string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND CODE  = :CODE               ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNOInCode(long argWrtNo, List<string> g36_NIGHT_CODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");
            parameter.AppendSql("   AND CODE IN (:CODE)                                 ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.AddInStatement("CODE", g36_NIGHT_CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_SUNAPDTL GetRowIdbyWrtNoCode(long wRTNO, List<string> g36_NIGHT_CODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID, WRTNO                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            if (g36_NIGHT_CODE.Count > 0)
            {
                parameter.AppendSql("   AND CODE IN (:CODE)                 ");
            }

            parameter.Add("WRTNO", wRTNO);
            if (g36_NIGHT_CODE.Count > 0)
            {
                parameter.AddInStatement("CODE", g36_NIGHT_CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReaderSingle<HIC_SUNAPDTL>(parameter);
        }

        public int GetCountbyWrtnoInHcCode(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(CODE) FROM KOSMOS_PMPA.HIC_SUNAPDTL               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                          ");
            parameter.AppendSql("   AND CODE IN ( SELECT CODE FROM HIC_CODE WHERE GUBUN ='98' ) ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteScalar<int>(parameter);
        }

        public int GetMaxHistorySeqNoByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(SEQ)  AS MSEQ FROM KOSMOS_PMPA.HIC_SUNAPDTL_HIS     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                          ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteScalar<int>(parameter);
        }

        public void InsertData(HIC_SUNAPDTL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAPDTL       ");
            parameter.AppendSql("     ( WRTNO,CODE,UCODE,AMT,GBSELF )       ");
            parameter.AppendSql(" VALUES                                    ");
            parameter.AppendSql("     ( :WRTNO,:CODE,:UCODE,:AMT,:GBSELF )  ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("CODE", item.CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("UCODE", item.UCODE.To<string>(""), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AMT", item.AMT);
            parameter.Add("GBSELF", item.GBSELF);

            ExecuteNonQuery(parameter);
        }

        public HIC_SUNAPDTL GetCountbyCodeMirNo(string strCode, string strJong, long nMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                            ");
            parameter.AppendSql(" WHERE CODE = :CODE                                        ");
            parameter.AppendSql("   AND WRTNO IN (SELECT WRTNO                              ");
            parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU              ");
            parameter.AppendSql("                  WHERE DELDATE IS NULL                    ");
            parameter.AppendSql("                    AND GBDENTAL = 'Y'                     ");
            if (strJong == "1")
            {
                parameter.AppendSql("                    AND MIRNO1 = :MIRNO                 ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("                    AND MIRNO2 = :MIRNO                 ");
            }
            else if (strJong == "4")
            {
                parameter.AppendSql("                    AND MIRNO3 = :MIRNO                 ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql("                    AND MIRNO5 = :MIRNO                 ");
            }
            parameter.AppendSql("                )                                           ");

            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MIRNO", nMirNo);

            return ExecuteReaderSingle<HIC_SUNAPDTL>(parameter);
        }

        public List<HIC_SUNAPDTL> GetAll(long nWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT     *                                        ");
            parameter.AppendSql("FROM       KOSMOS_PMPA.HIC_SUNAPDTL                 ");
            parameter.AppendSql(" WHERE     WRTNO = :WRTNO                           ");
            parameter.AppendSql("   AND     CODE = '1157'                             ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public List<HIC_SUNAPDTL> GetDentalList(long nWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT     *                                        ");
            parameter.AppendSql("FROM       KOSMOS_PMPA.HIC_SUNAPDTL                 ");
            parameter.AppendSql(" WHERE     WRTNO = :WRTNO                           "); 
            parameter.AppendSql("   AND     CODE = '1157'                             ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public long GetCountbyInWrtNoJepDate(string strGubun, string strFDate, string strTDate, long nMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                            ");
            parameter.AppendSql(" WHERE CODE = '1118'                                       "); //구강 토.공휴일 가산
            parameter.AppendSql("   AND WRTNO IN (                                          ");
            if (strGubun == "1")
            {
                parameter.AppendSql("                  SELECT WRTNO                         ");
                parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU          ");
                parameter.AppendSql("                  WHERE MIRNO1 = :MIRNO                ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("                  SELECT WRTNO                         ");
                parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU          ");
                parameter.AppendSql("                  WHERE MIRNO2 = :MIRNO                ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("                  SELECT WRTNO                         ");
                parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU          ");
                parameter.AppendSql("                  WHERE MIRNO3 = :MIRNO                ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("                  SELECT WRTNO                         ");
                parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU          ");
                parameter.AppendSql("                  WHERE MIRNO4 = :MIRNO                ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("                  SELECT WRTNO                         ");
                parameter.AppendSql("                   FROM KOSMOS_PMPA.HIC_JEPSU          ");
                parameter.AppendSql("                  WHERE MIRNO5 = :MIRNO                ");
            }
            parameter.AppendSql("                )                                          ");

            parameter.Add("MIRNO", nMirNo);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_SUNAPDTL GetCount(string strJong, long nMirNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                                        ");
            parameter.AppendSql(" WHERE CODE = '1118'                                                   ");
            parameter.AppendSql("   AND WRTNO IN (                                                      ");
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

            parameter.Add("MIRNO", nMirNo);

            return ExecuteReaderSingle<HIC_SUNAPDTL>(parameter);
        }

        public int GetCountbyWrtNoGbSelf(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND GBSELF = '7'                ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNoCodeGbSelf(long argWRTNO, string strCode, List<string> strGbSelf)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("   AND CODE = :CODE                                        ");
            parameter.AppendSql("   AND GBSELF IN (:GBSELF)                                 ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("GBSELF", strGbSelf, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_SUNAPDTL> GetCodebyWrtNoCode(long nWRTNO1, List<string> strCodeList1)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, CODE, GBSELF                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO1                                     ");
            parameter.AppendSql("   AND CODE IN (:CODE)                                     ");

            parameter.Add("WRTNO1", nWRTNO1);
            parameter.AddInStatement("CODE", strCodeList1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public List<HIC_SUNAPDTL> GetWrtNoCodeGbSelfbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, CODE, GBSELF                                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                      ");
            parameter.AppendSql("   AND CODE IN ('1165','1166','1163','1167','1160','J193') ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public int GetCountbyWrtno(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND CODE = '3169'               ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_SUNAPDTL> GetCodebyWrtNo(long nLifeWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, WRTNO                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO =:WRTNO               ");

            parameter.Add("WRTNO", nLifeWrtNo);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public List<HIC_SUNAPDTL> GetCodebyWrtNo(List<long> nLifeWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, WRTNO                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO IN (:WRTNO)           ");

            parameter.AddInStatement("WRTNO", nLifeWrtNo);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public int GetCountbyWrtNoInCode(long nWRTNO, string[] strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND CODE IN (:CODE)             ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("CODE", strCode);

            return ExecuteScalar<int>(parameter);
        }

        public void InsertSelectBySunapDtlWork2(long nWRTNO, HIC_JEPSU_WORK hJW)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAPDTL                   ");
            parameter.AppendSql("     ( WRTNO,CODE,UCODE,AMT,GBSELF )                   ");
            parameter.AppendSql("SELECT :WRTNO, CODE, UCODE, AMT, GBSELF                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_WORK                   ");
            parameter.AppendSql(" WHERE PANO =:PANO                                     ");
            parameter.AppendSql("   AND SUDATE = TO_DATE(:SUDATE,'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GJJONG =:GJJONG  ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("PANO", hJW.PANO);
            parameter.Add("SUDATE", hJW.JEPDATE);
            parameter.Add("GJJONG", hJW.GJJONG);

            ExecuteNonQuery(parameter);
        }

        public void InsertSelectBySunapDtlWork(long wRTNO, HIC_JEPSU_WORK iHJW)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAPDTL                   ");
            parameter.AppendSql("     ( WRTNO,CODE,UCODE,AMT,GBSELF )                   ");
            parameter.AppendSql("SELECT :WRTNO, CODE, UCODE, AMT, GBSELF                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_WORK                   ");
            parameter.AppendSql(" WHERE PANO =:PANO                                     ");
            parameter.AppendSql("   AND SUDATE = TO_DATE(:SUDATE,'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND GJJONG IN ('16','17','18','28','44','45','46')  ");

            parameter.Add("WRTNO", wRTNO);
            parameter.Add("PANO", iHJW.PANO);
            parameter.Add("SUDATE", iHJW.JEPDATE);
            
            ExecuteNonQuery(parameter);
        }

        public List<HIC_SUNAPDTL> GetGbSangdambyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT  a.Code,b.GbSangdam                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL  a                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPCODE b                             ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                        ");
            parameter.AppendSql("   AND a.Code = b.Code(+)                                      ");
            parameter.AppendSql("   AND b.GbSangdam IS NOT NULL                                 ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public string GetRowidbyWrtNoCodeIN(long argWrtno, List<string> argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID FROM KOSMOS_PMPA.HIC_SUNAPDTL  ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND CODE IN (:CODE)                             ");

            parameter.Add("WRTNO", argWrtno);
            parameter.AddInStatement("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertDataHis(long argWrtno, READ_SUNAP_ITEM rSuInfo, int nSeq)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAPDTL_HIS                   ");
            parameter.AppendSql("     ( WRTNO,GUBUN,CODE,UCODE,AMT,GBSELF,SEQ )             ");
            parameter.AppendSql(" VALUES                                                    ");
            parameter.AppendSql("     ( :WRTNO,:GUBUN,:CODE,:UCODE,:AMT,:GBSELF,:SEQ )      ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("GUBUN", rSuInfo.RowStatus == RowStatus.Delete ? "1" : "");
            parameter.Add("CODE", rSuInfo.GRPCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("UCODE", rSuInfo.UCODE.To<string>(""), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AMT", rSuInfo.AMT);
            parameter.Add("GBSELF", rSuInfo.GBSELF);
            parameter.Add("SEQ", nSeq);

            ExecuteNonQuery(parameter);
        }

        public void InsertData(long argWrtno, READ_SUNAP_ITEM rSuInfo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAPDTL       ");
            parameter.AppendSql("     ( WRTNO,CODE,UCODE,AMT,GBSELF )       ");
            parameter.AppendSql(" VALUES                                    ");
            parameter.AppendSql("     ( :WRTNO,:CODE,:UCODE,:AMT,:GBSELF )  ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("CODE", rSuInfo.GRPCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("UCODE", rSuInfo.UCODE.To<string>(""), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AMT", rSuInfo.AMT);
            parameter.Add("GBSELF", rSuInfo.GBSELF);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAllByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWrtno);

            ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNoInCode(long nWrtNo, List<string> strCodes)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL            ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");
            parameter.AppendSql("   AND CODE NOT IN (:CODE)                 ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.AddInStatement("CODE", strCodes);

            return ExecuteScalar<int>(parameter);
        }

        public int UpdateCodebyWrtNoCode(string strNew_Group, long nWrtNo, string strOLD_Group)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAPDTL SET   ");
            parameter.AppendSql("       CODE  = :NEW_CODE           ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND CODE  = :OLD_CODE           ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("NEW_CODE", strNew_Group);
            parameter.Add("OLD_CODE", strOLD_Group);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SUNAPDTL> GetItembyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, CODE, UCODE, AMT, GBSELF, GWRTNO     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND Code IN ('1163','1167','1168')              ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public string GetGbSelfByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBSELF FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND CODE  = '1160'                          ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_SUNAPDTL GetCodebyWrtNo(long nWrtNo, string strGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            if (strGbn == "2")
            {
                parameter.AppendSql("   AND CODE NOT IN ('9540')                    ");
            }
            else if (strGbn == "3")
            {
                parameter.AppendSql("   CODE IN ('9540')                            ");
            }

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReaderSingle<HIC_SUNAPDTL>(parameter);
        }

        public List<HIC_SUNAPDTL> GetAllbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO, CODE, UCODE, AMT, GBSELF, GWRTNO ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL                ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public int GetRowIdbyWrtNo(long nWRTNO, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID FROM KOSMOS_PMPA.HIC_SUNAPDTL     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND CODE  = :CODE                           ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT count('X') cnt              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCount(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT count('X') cnt              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL  a ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GROUPCODE b ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND a.CODE = b.CODE             ");
            parameter.AppendSql("   AND b.DELDATE IS NULL           ");
            parameter.AppendSql("   AND b.HANG IN ('A','B')         ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_SUNAPDTL> GetNamebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.CODE, b.NAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL  a     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GROUPCODE b     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND a.CODE >= 'Z0000'               ");
            parameter.AppendSql("   AND a.CODE = b.CODE(+)              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        public HIC_SUNAPDTL GetSunapDtlbyWrtNo(long nWrtNo, List<string> strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND CODE  IN (:CODE)            ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.AddInStatement("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SUNAPDTL>(parameter);
        }

        public List<HIC_SUNAPDTL> Read_GbSelf(long nWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GbSelf,SUM(AMT) AMT         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");
            parameter.AppendSql(" GROUP BY GbSelf                   ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReader<HIC_SUNAPDTL>(parameter);
        }

        //챠트인계등록
        public int GetCountbyWrtNoNotInCode(long nWrtNo, List<string> strCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                            ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_SUNAPDTL                 ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql(" AND CODE NOT IN (:CODE)                       ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.AddInStatement("CODE", strCODE);

            return ExecuteScalar<int>(parameter);
        }

    }
}
