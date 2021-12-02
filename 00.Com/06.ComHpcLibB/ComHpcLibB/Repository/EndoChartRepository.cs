namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class EndoChartRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public EndoChartRepository()
        {
        }

        public int UpdatebyRowId(ENDO_CHART item3)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.ENDO_CHART SET                       ");
            parameter.AppendSql("       GUBUN         = :GUBUN                          ");
            parameter.AppendSql("     , GB_EGD1       = :GB_EGD1                        ");
            parameter.AppendSql("     , GB_EGD2       = :GB_EGD2                        ");
            parameter.AppendSql("     , GB_CFS1       = :GB_CFS1                        ");
            parameter.AppendSql("     , GB_CFS2       = :GB_CFS2                        ");
            parameter.AppendSql("     , GB_DIET       = :GB_DIET                        ");
            parameter.AppendSql("     , GB_STS        = :GB_STS                         ");
            parameter.AppendSql("     , GB_OLD        = :GB_OLD                         ");
            parameter.AppendSql("     , GB_OLD1       = :GB_OLD1                        ");
            parameter.AppendSql("     , GB_OLD2       = :GB_OLD2                        ");
            parameter.AppendSql("     , GB_OLD3       = :GB_OLD3                        ");
            parameter.AppendSql("     , GB_OLD4       = :GB_OLD4                        ");
            parameter.AppendSql("     , GB_OLD5       = :GB_OLD5                        ");
            parameter.AppendSql("     , GB_OLD6       = :GB_OLD6                        ");
            parameter.AppendSql("     , GB_OLD7       = :GB_OLD7                        ");
            parameter.AppendSql("     , GB_OLD8       = :GB_OLD8                        ");
            parameter.AppendSql("     , GB_OLD9       = :GB_OLD9                        ");
            parameter.AppendSql("     , GB_OLD10      = :GB_OLD10                       ");
            parameter.AppendSql("     , GB_OLD11      = :GB_OLD11                       ");
            parameter.AppendSql("     , GB_OLD12      = :GB_OLD12                       ");
            parameter.AppendSql("     , GB_OLD13      = :GB_OLD13                       ");
            parameter.AppendSql("     , GB_OLD13_1    = :GB_OLD13_1                     ");
            parameter.AppendSql("     , GB_OLD14      = :GB_OLD14                       ");
            parameter.AppendSql("     , GB_OLD15_1    = :GB_OLD15_1                     ");
            parameter.AppendSql("     , GB_DRUG       = :GB_DRUG                        ");
            parameter.AppendSql("     , GB_DRUG1      = :GB_DRUG1                       ");
            parameter.AppendSql("     , GB_DRUG2      = :GB_DRUG2                       ");
            parameter.AppendSql("     , GB_DRUG3      = :GB_DRUG3                       ");
            parameter.AppendSql("     , GB_DRUG4      = :GB_DRUG4                       ");
            parameter.AppendSql("     , GB_DRUG5      = :GB_DRUG5                       ");
            parameter.AppendSql("     , GB_DRUG6      = :GB_DRUG6                       ");
            parameter.AppendSql("     , GB_DRUG7      = :GB_DRUG7                       ");
            parameter.AppendSql("     , GB_DRUG8_1    = :GB_DRUG8_1                     ");
            parameter.AppendSql("     , GB_DRUG_STOP1 = :GB_DRUG_STOP1                  ");
            parameter.AppendSql("     , GB_DRUG_STOP2 = :GB_DRUG_STOP2                  ");
            parameter.AppendSql("     , GB_B_DRUG     = :GB_B_DRUG                      ");
            parameter.AppendSql("     , GB_B_DRUG1    = :GB_B_DRUG1                     ");
            parameter.AppendSql("     , GB_B_DRUG1_1  = :GB_B_DRUG1_1                   ");
            parameter.AppendSql("     , GB_BIGO       = :GB_BIGO                        ");
            parameter.AppendSql(" WHERE ROWID         = :RID                            ");

            parameter.Add("GUBUN", item3.GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_EGD1", item3.GB_EGD1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_EGD2", item3.GB_EGD2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_CFS1", item3.GB_CFS1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_CFS2", item3.GB_CFS2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DIET", item3.GB_DIET, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_STS", item3.GB_STS, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD", item3.GB_OLD, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD1", item3.GB_OLD1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD2", item3.GB_OLD2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD3", item3.GB_OLD3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD4", item3.GB_OLD4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD5", item3.GB_OLD5, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD6", item3.GB_OLD6, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD7", item3.GB_OLD7, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD8", item3.GB_OLD8, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD9", item3.GB_OLD9, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD10", item3.GB_OLD10, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD11", item3.GB_OLD11, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD12", item3.GB_OLD12, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD13", item3.GB_OLD13, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD13_1", item3.GB_OLD13_1);
            parameter.Add("GB_OLD14", item3.GB_OLD14, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_OLD15_1", item3.GB_OLD15_1);
            parameter.Add("GB_DRUG", item3.GB_DRUG, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG1", item3.GB_DRUG1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG2", item3.GB_DRUG2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG3", item3.GB_DRUG3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG4", item3.GB_DRUG4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG5", item3.GB_DRUG5, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG6", item3.GB_DRUG6, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG7", item3.GB_DRUG7, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_DRUG8_1", item3.GB_DRUG8_1);
            parameter.Add("GB_DRUG_STOP1", item3.GB_DRUG_STOP1);
            parameter.Add("GB_DRUG_STOP2", item3.GB_DRUG_STOP2);
            parameter.Add("GB_B_DRUG", item3.GB_B_DRUG, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_B_DRUG1", item3.GB_B_DRUG1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GB_B_DRUG1_1", item3.GB_B_DRUG1_1);
            parameter.Add("GB_BIGO", item3.GB_BIGO);
            parameter.Add("RID", item3.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(ENDO_CHART item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_OCS.ENDO_CHART              ");
            parameter.AppendSql("       (PTNO,BDATE,RDATE,GUBUN)                ");
            parameter.AppendSql("VALUES                                         ");
            parameter.AppendSql("       (:PTNO, TO_DATE(:BDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("      , TO_DATE(:RDATE, 'YYYY-MM-DD'), :GUBUN) ");

            parameter.Add("PTNO", item.PTNO, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("RDATE", item.RDATE);
            parameter.Add("GUBUN", item.GUBUN, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPtnoRDate(string argPtno, string argJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_CHART                   ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND RDATE = TO_DATE(:RDATE, 'YYYY-MM-DD')   ");

            parameter.Add("PTNO", argPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", argJepDate);

            return ExecuteScalar<int>(parameter);
        }

        public ENDO_CHART GetItembyRowId(string argEndoRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PTNO, GUBUN, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                ");
            parameter.AppendSql("     , R_DRNAME,GUBUN,GB_EGD1,GB_EGD2,GB_CFS1,GB_CFS2,GB_DIET, GB_STS,GB_STS1,GB_STS2                  ");
            parameter.AppendSql("     , GB_OLD,GB_OLD1,GB_OLD2,GB_OLD3,GB_OLD4,GB_OLD5,GB_OLD6,GB_OLD7,GB_OLD8,GB_OLD9,GB_OLD10         ");
            parameter.AppendSql("     , GB_OLD11,GB_OLD12,GB_OLD13,GB_OLD13_1,GB_OLD14,GB_OLD15_1                                       ");
            parameter.AppendSql("     , GB_DRUG,GB_DRUG1,GB_DRUG2,GB_DRUG3,GB_DRUG4,GB_DRUG5,GB_DRUG6,GB_DRUG7,GB_DRUG8_1               ");
            parameter.AppendSql("     , GB_DRUG_STOP1,GB_DRUG_STOP2,GB_B_DRUG,GB_B_DRUG1,GB_B_DRUG1_1,GB_BIGO                           ");
            parameter.AppendSql("  FROM KOSMOS_OCS.ENDO_CHART                                                                           ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                                    ");

            parameter.Add("RID", argEndoRowId);

            return ExecuteReaderSingle<ENDO_CHART>(parameter);
        }

        public string GetRowIdbyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID FROM KOSMOS_OCS.ENDO_CHART    ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                            ");
            parameter.AppendSql("   AND RDATE = TO_DATE(:RDATE, 'YYYY-MM-DD')   ");

            parameter.Add("PTNO", fstrPtno, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("RDATE", fstrJepDate);

            return ExecuteScalar<string>(parameter);
        }
    }
}
