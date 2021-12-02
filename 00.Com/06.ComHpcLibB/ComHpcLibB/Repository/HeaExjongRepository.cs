namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaExjongRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaExjongRepository()
        {
        }

        public List<HEA_EXJONG> Read_Hea_ExJong()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXJONG  ");
            parameter.AppendSql(" ORDER BY CODE                 ");

            return ExecuteReader<HEA_EXJONG>(parameter);
        }

        public string Read_ExJong_Name(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXJONG  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int Insert(HEA_EXJONG item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HEA_EXJONG (                              ");
            parameter.AppendSql("       CODE,NAME,YNAME,BUN,BURATE,BUCHANGE,REMARK,GBINWON          ");
            parameter.AppendSql(") VALUES (                                                         ");
            parameter.AppendSql("       :CODE,:NAME,:YNAME,:BUN,:BURATE,:BUCHANGE,:REMARK,:GBINWON  ");
            parameter.AppendSql(")");

            #region Query 변수대입
            parameter.Add("CODE",       item.CODE);
            parameter.Add("Name",       item.NAME);
            parameter.Add("Bun",        item.YNAME);
            parameter.Add("Chasu",      item.BUN);
            parameter.Add("BuRate",     item.BURATE);
            parameter.Add("BuChange",   item.BUCHANGE);
            parameter.Add("GbSuga",     item.REMARK);
            parameter.Add("Chasu",      item.GBINWON);
           
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public string Read_Hea_ExJong_Name(string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXJONG      ");
            parameter.AppendSql(" WHERE CODE =:CODE                 ");

            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetBuRateByGjJong(string argJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT BURATE                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXJONG      ");
            parameter.AppendSql(" WHERE 1 = 1                       ");
            parameter.AppendSql("   AND CODE =:CODE                 ");

            parameter.Add("CODE", argJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int Delete(HEA_EXJONG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_EXJONG      ");
            parameter.AppendSql(" WHERE ROWID       = :RID          ");

            #region Query 변수대입
            parameter.Add("RID", item.RID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int UpDate(HEA_EXJONG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_EXJONG      ");
            parameter.AppendSql("   SET Name        =:Name          ");
            parameter.AppendSql("       Bun         =:Bun           ");
            parameter.AppendSql("       BuRate      =:BuRate        ");
            parameter.AppendSql("       BuChange    =:BuChange      ");            
            parameter.AppendSql("       GbInwon     =:GbInwon       ");
            parameter.AppendSql("       Remark      =:Remark        ");
            parameter.AppendSql("       GbNhic      =:GbNhic        ");
            parameter.AppendSql("       EntDate     =:EntDate       ");
            parameter.AppendSql("       EntSabun    =:EntSabun      ");
            parameter.AppendSql(" WHERE ROWID       =:RID          ");

            #region Query 변수대입
            parameter.Add("Name",       item.NAME);
            parameter.Add("Bun",        item.BUN);
            parameter.Add("BuRate",     item.BURATE);
            parameter.Add("BuChange",   item.BUCHANGE);
            parameter.Add("GbInwon",    item.GBINWON);
            parameter.Add("Remark",     item.REMARK);   
            parameter.Add("EntDate",    item.ENTDATE);
            parameter.Add("EntSabun",   item.ENTSABUN);
            parameter.Add("RID",        item.RID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_EXJONG> FindAll()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,NAME,YNAME,BUN,BURATE,BUCHANGE,REMARK,GBINWON,ROWID AS RID     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXJONG                                              ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");

            return ExecuteReader<HEA_EXJONG>(parameter);
        }

        public HEA_EXJONG Read_ExJong_CodeName(string v)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,YNAME,BUN,BURATE,BUCHANGE,REMARK,GBINWON,ROWID AS RID ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXJONG                                          ");
            parameter.AppendSql(" WHERE 1 = 1                                                           ");
            parameter.AppendSql("   AND Code =:Code                                                    ");

            parameter.Add("Code", v, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HEA_EXJONG>(parameter);
        }
    }
}
