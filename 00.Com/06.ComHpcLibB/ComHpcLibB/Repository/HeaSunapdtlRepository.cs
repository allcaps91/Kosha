namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComBase.Controls;

    /// <summary>
    /// 
    /// </summary>
    public class HeaSunapdtlRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public HeaSunapdtlRepository()
        {
        }

        public List<HEA_SUNAPDTL> Read_YName(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT YNAME                           ");
            parameter.AppendSql("     , ROWID RID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPCODE       ");
            parameter.AppendSql(" WHERE CODE IN (                       ");
            parameter.AppendSql("                SELECT CODE            ");
            parameter.AppendSql("                  FROM HEA_SUNAPDTL    ");
            parameter.AppendSql("                 WHERE WRTNO = :WRTNO  ");
            parameter.AppendSql("                   AND JONG = '**'     ");
            parameter.AppendSql("               )                       ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReader<HEA_SUNAPDTL>(parameter);
        }

        public List<HEA_SUNAPDTL> GetListByWRTNO(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, CODE, CODENAME, AMT, GBSELF, GBHALIN ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HEA_SUNAPDTL>(parameter);
        }

        public string GetRowidByWrtnoCodeIN(long nWRTNO, List<string> lstExCodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL           ");
            parameter.AppendSql(" WHERE 1 = 1                              ");
            parameter.AppendSql("   AND WRTNO = :WRTNO                     ");
            parameter.AppendSql("   AND CODE IN (:CODE)                    ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.AddInStatement("CODE", lstExCodes);

            return ExecuteScalar<string>(parameter);
        }

        public string GetMainSunapDtlCodeNameByWrtno(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.CODENAME                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL a      ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_GROUPCODE b     ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                ");
            parameter.AppendSql("   AND a.CODE = b.CODE                 ");
            parameter.AppendSql("   AND b.GBSELECT = 'N'                ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertDataHis(long argWrtno, READ_SUNAP_ITEM rSuInfo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SUNAPDTL_HIS                                   ");
            parameter.AppendSql("     ( GBDEL,WRTNO,CODE,AMT,GBSELF,GBHALIN,CODENAME,ENTSABUN,ENTTIME, BONINAMT, LTDAMT )     ");
            parameter.AppendSql(" VALUES                                                                    ");
            parameter.AppendSql("     ( :GBDEL,:WRTNO,:CODE,:AMT,:GBSELF,:GBHALIN,:CODENAME,:ENTSABUN,SYSDATE,:BONINAMT,:LTDAMT )    ");

            parameter.Add("GBDEL",  rSuInfo.ISDELTE);
            parameter.Add("WRTNO",  argWrtno);            
            parameter.Add("CODE",   rSuInfo.GRPCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("AMT",    rSuInfo.AMT);
            parameter.Add("GBSELF", rSuInfo.BURATE);
            parameter.Add("GBHALIN", rSuInfo.GBHALIN);
            parameter.Add("CODENAME", rSuInfo.GRPNAME);
            parameter.Add("ENTSABUN", clsType.User.IdNumber.To<long>(0));
            parameter.Add("BONINAMT", rSuInfo.BONINAMT);
            parameter.Add("LTDAMT", rSuInfo.LTDAMT);

            ExecuteNonQuery(parameter);
        }

        public void InsertData(long argWrtno, READ_SUNAP_ITEM rSuInfo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SUNAPDTL       ");
            parameter.AppendSql("     ( WRTNO,CODE,AMT,GBSELF,GBHALIN,CODENAME, BONINAMT, LTDAMT )       ");
            parameter.AppendSql(" VALUES                                    ");
            parameter.AppendSql("     ( :WRTNO,:CODE,:AMT,:GBSELF,:GBHALIN,:CODENAME,:BONINAMT,:LTDAMT )  ");

            parameter.Add("WRTNO",  argWrtno);
            parameter.Add("CODE",   rSuInfo.GRPCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("AMT",    rSuInfo.AMT);
            parameter.Add("GBSELF", rSuInfo.GBSELF);
            parameter.Add("GBHALIN", rSuInfo.GBHALIN);
            parameter.Add("CODENAME", rSuInfo.GRPNAME);
            parameter.Add("BONINAMT", rSuInfo.BONINAMT);
            parameter.Add("LTDAMT", rSuInfo.LTDAMT);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAllByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWrtno);

            ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNo(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            parameter.Add("WRTNO", argWrtno);

            return ExecuteScalar<int>(parameter);
        }

        public string GetCodeTypeByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.TYPE                                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL a                          ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_GROUPCODE b                         ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                    ");
            parameter.AppendSql("   AND a.CODE = b.CODE(+)                                  ");
            parameter.AppendSql("   AND b.GBSELECT = 'N'                                    ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_SUNAPDTL> GetSunapListByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.CODE,b.Amt,a.GbSelf,b.Name GrName,a.GbHalin       ");
            parameter.AppendSql("     , a.CodeName as GamName,a.Amt as GamAmt, b.BuRate     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL a                          ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_GROUPCODE b                         ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                    ");
            parameter.AppendSql("   AND a.CODE = b.CODE(+)                                  ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HEA_SUNAPDTL>(parameter);
        }

        public long GetSumAmtByWRTNO(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(AMT) AMT                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                   ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public string CheckVipByWRTNOLikeCodeName(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODENAME                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL                                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");
            parameter.AppendSql("   AND (CODENAME LIKE '%골드검진%' OR CODENAME LIKE '%VIP검진%' )  ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_SUNAPDTL> GetNamebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.CODE, b.YNAME                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_SUNAPDTL  a     ");
            parameter.AppendSql("     , KOSMOS_PMPA.HEA_GROUPCODE b     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND a.CODE >= 'Z0000'               ");
            parameter.AppendSql("   AND a.CODE = b.CODE(+)              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HEA_SUNAPDTL>(parameter);
        }
    }
}
