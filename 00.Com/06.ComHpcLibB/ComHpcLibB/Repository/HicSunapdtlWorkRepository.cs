namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSunapdtlWorkRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSunapdtlWorkRepository()
        {
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SUNAPDTL_WORK SET       ");
            parameter.AppendSql("       PANO = :PANO                            ");
            parameter.AppendSql(" WHERE PANO IN (SELECT PANO                    ");
            parameter.AppendSql("                  FROM KOSMOS_PMPA.HIC_PATIENT ");
            parameter.AppendSql("                 WHERE JUMIN2 = :JUMIN2        ");
            parameter.AppendSql("                   AND PANO <> :PANO)          ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("JUMIN2", argJumin2);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SUNAPDTL_WORK> GetItembyPaNoSuDateGjJong(string fstrPANO, string argGJepdate, string argGjJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, GBSELF, AMT                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_WORK                           ");
            parameter.AppendSql(" WHERE PANO   = :PANO                                          ");
            parameter.AppendSql("   AND SUDATE = TO_DATE(:SUDATE, 'YYYY-MM-DD')                 ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                                        ");
            parameter.AppendSql(" ORDER BY Code                                                 ");

            parameter.Add("PANO", fstrPANO);
            parameter.Add("SUDATE", argGJepdate);
            parameter.Add("GJJONG", argGjJong);

            return ExecuteReader<HIC_SUNAPDTL_WORK>(parameter);
        }

        public HIC_SUNAPDTL_WORK GetExCodeHNamebyPaNoSudate(string strPANO, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.EXCODE, c.HNAME                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_WORK a                                 ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GROUPEXAM     b                                 ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_EXCODE        c                                 ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                  ");
            parameter.AppendSql("   AND a.SUDATE = TO_DATE(:SUDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql("   AND a.CODE   = b.GROUPCODE(+)                                       ");
            parameter.AppendSql("   AND b.EXCODE = c.CODE(+)                                            ");
            parameter.AppendSql("   AND b.EXCODE IN ('A142','TX13','TX14','TX11','A213','TX16','A211')  ");

            parameter.Add("PANO", strPANO);
            parameter.Add("SUDATE", strJepDate);

            return ExecuteReaderSingle<HIC_SUNAPDTL_WORK>(parameter);
        }

        public void DeletebyPaNoSuDate2(HIC_JEPSU_WORK hJW)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAPDTL_WORK                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SUDATE = TO_DATE(:SUDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND GJJONG =:GJJONG                                 ");

            parameter.Add("PANO", hJW.PANO);
            parameter.Add("SUDATE", hJW.JEPDATE);
            parameter.Add("GJJONG", hJW.GJJONG);

            ExecuteNonQuery(parameter);
        }

        public void DeletebyPaNoSuDate(HIC_JEPSU_WORK iHJW)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAPDTL_WORK                   ");
            parameter.AppendSql(" WHERE PANO = :PANO                                    ");
            parameter.AppendSql("   AND SUDATE = TO_DATE(:SUDATE, 'YYYY-MM-DD')         ");
            parameter.AppendSql("   AND GJJONG IN ('16','17','18','28','44','45','46')  ");

            parameter.Add("PANO", iHJW.PANO);
            parameter.Add("SUDATE", iHJW.JEPDATE);

            ExecuteNonQuery(parameter);
        }

        public int GetCountbyPaNo(string argPaNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_WORK A                         ");
            parameter.AppendSql("     , KOSMOS_PMPA.HIC_GROUPCODE     B                         ");
            parameter.AppendSql(" WHERE A.CODE = b.CODE                                         ");
            parameter.AppendSql("   AND B.GBGUBUN1 IS NOT NULL                                  ");
            parameter.AppendSql("   AND PANO = :PANO                                            ");

            parameter.Add("PANO", argPaNo);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountSunapDtlWorkbyPanoJong(long argPano, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SUNAPDTL_WORK                           ");
            parameter.AppendSql(" WHERE PANO   = :PANO                                          ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                                        ");
            parameter.AppendSql(" ORDER BY CODE                                                 ");

            parameter.Add("PANO", argPano);
            parameter.Add("GJJONG", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public void InsertData(long argPano, string argJong, READ_SUNAP_ITEM rSuInfo, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAPDTL_WORK       ");
            parameter.AppendSql("     ( WRTNO,CODE,UCODE,AMT,GBSELF,SUDATE,PANO,GJJONG )       ");
            parameter.AppendSql(" VALUES                                    ");
            parameter.AppendSql("     ( 0,:CODE,:UCODE,:AMT,:GBSELF,TO_DATE(:SUDATE,'YYYY-MM-DD'),:PANO,:GJJONG )  ");

            
            parameter.Add("CODE", rSuInfo.GRPCODE);
            parameter.Add("UCODE", rSuInfo.UCODE);
            parameter.Add("AMT", rSuInfo.AMT);
            parameter.Add("GBSELF", rSuInfo.GBSELF);
            parameter.Add("SUDATE", argDate);
            parameter.Add("PANO", argPano);
            parameter.Add("GJJONG", argJong);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAllByPano(long argPano, string argJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAPDTL_WORK   ");
            parameter.AppendSql(" WHERE PANO = :PANO                    ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                ");

            parameter.Add("PANO", argPano);
            parameter.Add("GJJONG", argJong);

            ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_SUNAPDTL_WORK item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SUNAPDTL_WORK                                                      ");
            parameter.AppendSql("       (WRTNO, CODE, UCODE, AMT, GBSELF, SUDATE, PANO, GJJONG)                                 ");
            parameter.AppendSql("VALUES                                                                                         ");
            parameter.AppendSql("       (:WRTNO, :CODE, :UCODE, :AMT, :GBSELF, TO_DATE(:SUDATE, 'YYYY-MM-DD'), :PANO, :GJJONG)  ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("CODE", item.CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("UCODE", item.UCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AMT", item.AMT);
            parameter.Add("GBSELF", item.GBSELF);
            parameter.Add("SUDATE", item.SUDATE);
            parameter.Add("PANO", item.PANO);
            parameter.Add("GJJONG", item.GJJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int DeletebyPaNoGjJong(long argPaNo, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_SUNAPDTL_WORK       ");
            parameter.AppendSql(" WHERE PANO   = :PANO                      ");
            parameter.AppendSql("   AND GJJONG = :GJJONG                    ");

            parameter.Add("PANO", argPaNo);
            parameter.Add("GJJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
    }
}
