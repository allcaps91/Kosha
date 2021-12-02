
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
    public class HicExjongRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicExjongRepository()
        {
        }

        public List<HIC_EXJONG> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,BUN,CHASU,BURATE,BUCHANGE,REMARK,ENTDATE,ENTSABUN,GBSUGA,GBMUNJIN      ");
            parameter.AppendSql("      ,MISUJONG, GBINWON, GBJIN, GBPRT1, GBPRT2, GBPRT3, GBPRT4, GBPRT5, GBPRT6, BUCODE ");
            parameter.AppendSql("      ,GBSANGDAM, GBNHIC, CODE2, Rowid                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                            ");
            parameter.AppendSql(" ORDER By CODE                                                                          ");

            return ExecuteReader<HIC_EXJONG>(parameter);
        }

        public List<HIC_EXJONG> GetItemListByBun(string v)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE 1 = 1                   ");
            parameter.AppendSql("   AND BUN =:BUN               ");
            parameter.AppendSql(" ORDER By CODE                 ");

            parameter.Add("BUN", v, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_EXJONG>(parameter);
        }

        public int Delete(HIC_EXJONG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE ROWID       = :RID          ");

            #region Query 변수대입
            parameter.Add("RID", item.ROWID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpDate(HIC_EXJONG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql("   SET Name        =:Name          ");
            parameter.AppendSql("      ,Bun         =:Bun           ");
            parameter.AppendSql("      ,Chasu       =:Chasu         ");
            parameter.AppendSql("      ,BuRate      =:BuRate        ");
            parameter.AppendSql("      ,BuChange    =:BuChange      ");
            parameter.AppendSql("      ,GbSuga      =:GbSuga        ");
            parameter.AppendSql("      ,GbMunjin    =:GbMunjin      ");
            parameter.AppendSql("      ,GbInwon     =:GbInwon       ");
            parameter.AppendSql("      ,GbJin       =:GbJin         ");
            parameter.AppendSql("      ,GbPrt1      =:GbPrt1        ");
            parameter.AppendSql("      ,GbPrt2      =:GbPrt2        ");
            parameter.AppendSql("      ,GbPrt3      =:GbPrt3        ");
            parameter.AppendSql("      ,GbPrt4      =:GbPrt4        ");
            parameter.AppendSql("      ,GbPrt5      =:GbPrt5        ");
            parameter.AppendSql("      ,GbPrt6      =:GbPrt6        ");
            parameter.AppendSql("      ,GbSangDam   =:GbSangDam     ");
            parameter.AppendSql("      ,BuCode      =:BuCode        ");
            parameter.AppendSql("      ,Remark      =:Remark        ");
            parameter.AppendSql("      ,GbNhic      =:GbNhic        ");
            parameter.AppendSql("      ,EntDate     =:EntDate       ");
            parameter.AppendSql("      ,EntSabun    =:EntSabun      ");
            parameter.AppendSql(" WHERE ROWID       =:RID          ");

            #region Query 변수대입
            parameter.Add("Name", item.NAME);
            parameter.Add("Bun", item.BUN);
            parameter.Add("Chasu", item.CHASU);
            parameter.Add("BuRate", item.BURATE);
            parameter.Add("BuChange", item.BUCHANGE);
            parameter.Add("GbSuga", item.GBSUGA);
            parameter.Add("GbMunjin", item.GBMUNJIN);
            parameter.Add("GbInwon", item.GBINWON);
            parameter.Add("GbJin", item.GBJIN);
            parameter.Add("GbPrt1", item.GBPRT1);
            parameter.Add("GbPrt2", item.GBPRT2);
            parameter.Add("GbPrt3", item.GBPRT3);
            parameter.Add("GbPrt4", item.GBPRT4);
            parameter.Add("GbPrt5", item.GBPRT5);
            parameter.Add("GbPrt6", item.GBPRT6);
            parameter.Add("GbSangDam", item.GBSANGDAM);
            parameter.Add("BuCode", item.BUCODE);
            parameter.Add("Remark", item.REMARK);
            parameter.Add("GbNhic", item.GBNHIC);
            parameter.Add("EntDate", item.ENTDATE);
            parameter.Add("EntSabun", item.ENTSABUN);
            parameter.Add("RID", item.ROWID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_EXJONG> GetCode(string fstrJong, string [] jong_1, string[] jong_2, string[] jong_3, string[] jong_4)
        {
            MParameter parameter = CreateParameter();


            parameter.AppendSql("SELECT     CODE                                                  ");
            parameter.AppendSql("FROM       KOSMOS_PMPA.HIC_EXJONG                                ");
            parameter.AppendSql("WHERE      MISUJONG = :JONG                                      ");
            parameter.AppendSql("   AND     CODE NOT IN (:JONG1, :JONG2, :JONG3, :JONG4)          ");


            parameter.Add("JONG", fstrJong);
            parameter.AddInStatement("JONG1", jong_1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("JONG2", jong_2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("JONG3", jong_3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("JONG4", jong_4, Oracle.DataAccess.Client.OracleDbType.Char);
            return ExecuteReader<HIC_EXJONG>(parameter);
        }

        public List<HIC_EXJONG> GetListByCodeName(string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG          ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE NAME LIKE :NAME             ");
            }
            else
            {
                parameter.AppendSql(" WHERE NAME IS NOT NULL            ");
            }
            parameter.AppendSql("   AND ROWNUM <= 500                   ");
            parameter.AppendSql(" ORDER BY CODE                         ");

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HIC_EXJONG>(parameter);
        }

        public string GetGbNhicByExJong(string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBNHIC                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", argJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetGbMunjinbyCode(string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBMUNJIN                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", argJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_EXJONG GetNamebyCode(string argGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE 1 = 1                   ");
            parameter.AppendSql("   AND CODE =:CODE             ");

            parameter.Add("CODE", argGjJong);

            return ExecuteScalar<HIC_EXJONG>(parameter);
        }

        public string GetBuRateByGjJong(string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT BURATE                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE 1 = 1                   ");
            parameter.AppendSql("   AND CODE =:CODE             ");

            parameter.Add("CODE", argJong);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_EXJONG> GetItemList()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE 1 = 1                   ");
            parameter.AppendSql("   AND CODE < '90'             ");     //추가 선택검사 영역
            parameter.AppendSql(" ORDER By CODE                 ");

            return ExecuteReader<HIC_EXJONG>(parameter);
        }

        public string GetBunByGjJong(string grgJONG)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT BUN                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", grgJONG, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
        public HIC_EXJONG GetItembyCode(string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBMUNJIN,GBSANGDAM,GBPRT1,GBPRT2,GBPRT3,GBPRT4,GBPRT5,GBPRT6    ");
            parameter.AppendSql("      ,BURATE,BUCHANGE,CHASU,GBINWON                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG                                          ");
            parameter.AppendSql(" WHERE CODE = :CODE                                                    ");

            parameter.Add("CODE", strJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXJONG>(parameter);
        }

        public string GetChasubyCode(string strJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Chasu                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", strJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetGbSangdambyCode(string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBSANGDAM                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", argJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetGbPrt5byCode(string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBPRT5                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", argJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_EXJONG> GetCodeName(List<string> strBun, string strChasu)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CHASU = :CHASU              ");
            parameter.AppendSql("   AND BUN IN (:InBUN)               ");

            parameter.Add("CHASU", strChasu, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.AddInStatement("InBUN", strBun, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_EXJONG>(parameter);
        }

        public int GetCount(string argGjJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG      ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");
            parameter.AppendSql("   AND NAME LIKE '%생애%'          ");

            parameter.Add("CODE", argGjJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public string GetGbMunjin(string argJONG)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBMUNJIN                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argJONG, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_GbSangdam(string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Code,Name               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", argJong, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_EXJONG Read_ExJong_CodeName(string v)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,BUN,CHASU,BURATE,BUCHANGE,REMARK,ENTDATE,ENTSABUN,GBSUGA,GBMUNJIN      ");
            parameter.AppendSql("     , MISUJONG, GBINWON, GBJIN, GBPRT1, GBPRT2, GBPRT3, GBPRT4, GBPRT5, GBPRT6, BUCODE ");
            parameter.AppendSql("     , GBSANGDAM, GBNHIC, CODE2, Rowid                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_ExJong                                                           ");
            parameter.AppendSql(" WHERE 1 = 1                                                                            ");
            parameter.AppendSql("   AND CODE = :CODE                                                                     ");

            parameter.Add("CODE", v, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_EXJONG>(parameter);
        }

        public int Insert(HIC_EXJONG item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HIC_EXJONG (                                                              ");
            parameter.AppendSql("       CODE,Name,Bun,Chasu,BuRate,BuChange,GbSuga                                                  ");
            parameter.AppendSql("     , GbMunjin,GbInwon,GbJin,GbPrt1,GbPrt2,GbPrt3,GbPrt4,GbPrt5,GbPrt6,GbSangDam                  ");
            parameter.AppendSql("     , Remark,BuCode,EntDate,EntSabun,GbNhic                                                       ");
            parameter.AppendSql(") VALUES (                                                                                         ");
            parameter.AppendSql("       :CODE,:Name,:Bun,:Chasu,:BuRate,:BuChange,:GbSuga                                           ");
            parameter.AppendSql("      ,:GbMunjin,:GbInwon,:GbJin,:GbPrt1,:GbPrt2,:GbPrt3,:GbPrt4,:GbPrt5,:GbPrt6,:GbSangDam        ");
            parameter.AppendSql("      ,:Remark,:BuCode,:EntDate,:EntSabun,:GbNhic                                                  ");
            parameter.AppendSql(")");

            #region Query 변수대입
            parameter.Add("CODE", item.CODE);
            parameter.Add("Name", item.NAME);
            parameter.Add("Bun", item.BUN);
            parameter.Add("Chasu", item.CHASU);
            parameter.Add("BuRate", item.BURATE);
            parameter.Add("BuChange", item.BUCHANGE);
            parameter.Add("GbSuga", item.GBSUGA);
            parameter.Add("Chasu", item.CHASU);
            parameter.Add("GbMunjin", item.GBMUNJIN);
            parameter.Add("GbInwon", item.GBINWON);
            parameter.Add("GbJin", item.GBJIN);
            parameter.Add("GbPrt1", item.GBPRT1);
            parameter.Add("GbPrt2", item.GBPRT2);
            parameter.Add("GbPrt3", item.GBPRT3);
            parameter.Add("GbPrt4", item.GBPRT4);
            parameter.Add("GbPrt5", item.GBPRT5);
            parameter.Add("GbPrt6", item.GBPRT6);
            parameter.Add("GbSangDam", item.GBSANGDAM);
            parameter.Add("Remark", item.REMARK);
            parameter.Add("BuCode", item.BUCODE);
            parameter.Add("EntDate", item.ENTDATE);
            parameter.Add("EntSabun", item.ENTSABUN);
            parameter.Add("GbNhic", item.GBNHIC);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public string Read_Hic_ExJong_Name(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", CODE, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_EXJONG> Read_ExJong_Add(bool chk = false)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Code,Name                                                   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXJONG                                      ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            if (chk == true)
            {
                parameter.AppendSql("   AND Code NOT IN ( SELECT TRIM(Code)                         ");
                parameter.AppendSql("                       FROM BAS_BCODE                          ");
                parameter.AppendSql("                      WHERE GUBUN ='HIC_2018_EXJONG'           ");
                parameter.AppendSql("                        AND (DELDATE IS NULL OR DELDATE ='') ) ");
            }
            parameter.AppendSql(" ORDER BY Code                                                     ");

            return ExecuteReader<HIC_EXJONG>(parameter);
        }
    }
}
