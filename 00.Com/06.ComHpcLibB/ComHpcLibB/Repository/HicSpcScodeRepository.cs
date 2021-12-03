namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcScodeRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcScodeRepository()
        {
        }

        public string Read_Spc_Scode_Name(string argCode)
        {   
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_Spc_SCODE_NEW   ");
            parameter.AppendSql(" WHERE CODE = :CODE                ");

            parameter.Add("CODE", argCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_SPC_SCODE GetItembyCode(string argSogen)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DECODE(PANJENG,'1','A','2','B','3','C1','4','C2','5','D1','6','D2','7','R','8','U') PANJENG ");
            parameter.AppendSql("      ,PANJENG AS PANJENG2, DBUN,ADMIN.FC_HIC_CODE_NM('19', DBUN) AS DBUN_NM, NAME, CODE     ");
            parameter.AppendSql("      ,MCODE, ADMIN.FC_HIC_MCODE_NM(MCODE) AS MCODE_NM, GCODE, SORT                          ");
            parameter.AppendSql("      ,GBAUTOPAN,SOGENREMARK,JOCHIREMARK,WORKYN,SAHUCODE,REEXAM,AUTOPANGBN                         ");
            parameter.AppendSql("      ,TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE, ROWID AS RID                                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_SCODE_NEW                                   ");
            parameter.AppendSql(" WHERE CODE = :CODE                                                ");

            parameter.Add("CODE", argSogen, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_SPC_SCODE>(parameter);
        }

        public HIC_SPC_SCODE GetItemByMCodeSogen(string fstrCode, string fstrMCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DBUN,ADMIN.FC_HIC_CODE_NM('19', DBUN) AS DBUN_NM, NAME, CODE  ");
            parameter.AppendSql("      ,MCODE, ADMIN.FC_HIC_MCODE_NM(MCODE) AS MCODE_NM, GCODE, SORT  ");
            parameter.AppendSql("      ,GBAUTOPAN,SOGENREMARK,JOCHIREMARK,WORKYN,SAHUCODE,REEXAM,AUTOPANGBN ");
            parameter.AppendSql("      ,TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE, ROWID AS RID, PANJENG       ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_SCODE_NEW                                           ");
            parameter.AppendSql(" WHERE MCODE = :MCODE                                                      ");
            parameter.AppendSql("   AND CODE = :CODE                                                        ");
            parameter.AppendSql("   AND AUTOPANGBN = '2'                                                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                     ");

            parameter.Add("MCODE", fstrMCode);
            parameter.Add("CODE", fstrCode);

            return ExecuteReaderSingle<HIC_SPC_SCODE>(parameter);
        }

        public HIC_SPC_SCODE GetItemByMCodePanjeng(string strPan, string fstrMCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DBUN,ADMIN.FC_HIC_CODE_NM('19', DBUN) AS DBUN_NM, NAME, CODE  ");
            parameter.AppendSql("      ,MCODE, ADMIN.FC_HIC_MCODE_NM(MCODE) AS MCODE_NM, GCODE, SORT  ");
            parameter.AppendSql("      ,GBAUTOPAN,SOGENREMARK,JOCHIREMARK,WORKYN,SAHUCODE,REEXAM,AUTOPANGBN ");
            parameter.AppendSql("      ,TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE, ROWID AS RID                ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_SCODE_NEW                                           ");
            parameter.AppendSql(" WHERE MCODE = :MCODE                                                      ");
            parameter.AppendSql("   AND PANJENG = :PANJENG                                                  ");
            parameter.AppendSql("   AND AUTOPANGBN = '1'                                                    ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                     ");

            parameter.Add("MCODE", fstrMCode);
            parameter.Add("PANJENG", strPan);

            return ExecuteReaderSingle<HIC_SPC_SCODE>(parameter);
        }

        public List<HIC_SPC_SCODE> GetListByCodeName(string strName, string strRetValue = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_SCODE_NEW           ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            if (!strRetValue.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PANJENG = :PANJENG              ");
            }
            parameter.AppendSql("   AND DELDATE IS NULL                     ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND NAME LIKE :NAME                 ");
            }
            parameter.AppendSql("   AND ROWNUM <= 500                       ");
            parameter.AppendSql(" ORDER BY Sort,CODE                        ");

            if (!strRetValue.IsNullOrEmpty())
            {
                parameter.Add("PANJENG", strRetValue, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HIC_SPC_SCODE>(parameter);
        }

        public int Delete(string argRid, DateTime? argDelDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_SPC_SCODE_NEW       ");
            parameter.AppendSql("   SET DELDATE  = :DELDATE             ");
            parameter.AppendSql("      ,ENTSABUN =:ENTSABUN             ");
            parameter.AppendSql(" WHERE ROWID    =:RID                  ");

            parameter.Add("DELDATE", argDelDate);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);
            parameter.Add("RID", argRid);

            return ExecuteNonQuery(parameter);
        }

        public int Update(HIC_SPC_SCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_SPC_SCODE_NEW  ");
            parameter.AppendSql("   SET CODE        =:CODE         ");
            parameter.AppendSql("      ,PANJENG     =:PANJENG      ");
            parameter.AppendSql("      ,DBUN        =:DBUN         ");
            parameter.AppendSql("      ,NAME        =:NAME         ");
            parameter.AppendSql("      ,MCODE       =:MCODE        ");
            parameter.AppendSql("      ,ENTDATE     =SYSDATE       ");
            parameter.AppendSql("      ,ENTSABUN    =:ENTSABUN     ");
            parameter.AppendSql("      ,GCODE       =:GCODE        ");
            parameter.AppendSql("      ,SOGENREMARK =:SOGENREMARK  ");
            parameter.AppendSql("      ,JOCHIREMARK =:JOCHIREMARK  ");
            parameter.AppendSql("      ,WORKYN      =:WORKYN       ");
            parameter.AppendSql("      ,SAHUCODE    =:SAHUCODE     ");
            parameter.AppendSql("      ,REEXAM      =:REEXAM       ");
            parameter.AppendSql("      ,GBAUTOPAN   =:GBAUTOPAN    ");
            parameter.AppendSql("      ,AUTOPANGBN  =:AUTOPANGBN   ");
            parameter.AppendSql("      ,SORT        =:SORT         ");
            parameter.AppendSql(" WHERE ROWID       =:RID          ");

            parameter.Add("CODE",        item.CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENG",     item.PANJENG);
            parameter.Add("DBUN",        item.DBUN); 
            parameter.Add("NAME",        item.NAME); 
            parameter.Add("MCODE",       item.MCODE);
            parameter.Add("ENTSABUN",    item.ENTSABUN);
            parameter.Add("GCODE",       item.GCODE);
            parameter.Add("SOGENREMARK", item.SOGENREMARK);
            parameter.Add("JOCHIREMARK", item.JOCHIREMARK);
            parameter.Add("WORKYN",      item.WORKYN);
            parameter.Add("SAHUCODE",    item.SAHUCODE);
            parameter.Add("REEXAM",      item.REEXAM);
            parameter.Add("GBAUTOPAN",   item.GBAUTOPAN);
            parameter.Add("AUTOPANGBN",  item.AUTOPANGBN);
            parameter.Add("SORT",        item.SORT);
            parameter.Add("RID",         item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_SPC_SCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_SPC_SCODE_NEW (                                    ");
            parameter.AppendSql("   CODE,PANJENG,DBUN,NAME,MCODE,ENTDATE,ENTSABUN,GCODE,SOGENREMARK         ");
            parameter.AppendSql("  ,JOCHIREMARK, WORKYN, SAHUCODE, REEXAM, GBAUTOPAN, SORT,AUTOPANGBN       ");
            parameter.AppendSql(" ) VALUES (                                                                ");
            parameter.AppendSql("  :CODE,:PANJENG,:DBUN,:NAME,:MCODE,SYSDATE,:ENTSABUN,:GCODE,:SOGENREMARK  ");
            parameter.AppendSql(" ,:JOCHIREMARK,:WORKYN,:SAHUCODE,:REEXAM,:GBAUTOPAN,:SORT,:AUTOPANGBN  )   ");

            parameter.Add("CODE",        item.CODE);
            parameter.Add("PANJENG",     item.PANJENG);
            parameter.Add("DBUN",        item.DBUN);
            parameter.Add("NAME",        item.NAME);
            parameter.Add("MCODE",       item.MCODE);
            parameter.Add("ENTSABUN",    clsType.User.IdNumber);
            parameter.Add("GCODE",       item.GCODE);
            parameter.Add("SOGENREMARK", item.SOGENREMARK);
            parameter.Add("JOCHIREMARK", item.JOCHIREMARK);
            parameter.Add("WORKYN",      item.WORKYN);
            parameter.Add("SAHUCODE",    item.SAHUCODE);
            parameter.Add("REEXAM",      item.REEXAM);
            parameter.Add("GBAUTOPAN",   item.GBAUTOPAN);
            parameter.Add("SORT",        item.SORT);
            parameter.Add("AUTOPANGBN",  item.AUTOPANGBN);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SPC_SCODE> FindAll(string strPan, bool bDel)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DECODE(PANJENG,'1','A','2','B','3','C1','4','C2','5','D1','6','D2','7','R','8','U','9','CN','A','DN') PANJENG ");
            parameter.AppendSql("      ,DBUN,ADMIN.FC_HIC_CODE_NM('19', DBUN) AS DBUN_NM, NAME, CODE                          ");
            parameter.AppendSql("      ,MCODE, ADMIN.FC_HIC_MCODE_NM(MCODE) AS MCODE_NM, GCODE, SORT                          ");
            parameter.AppendSql("      ,GBAUTOPAN,SOGENREMARK,JOCHIREMARK,WORKYN,SAHUCODE,REEXAM                                    ");
            parameter.AppendSql("      ,DECODE(AUTOPANGBN,'1','판정','2','소견','') AUTOPANGBN                                      ");
            parameter.AppendSql("      ,DELDATE, ROWID AS RID                                                                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_SCODE_NEW                                                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                       ");
            if (strPan != "*" && !strPan.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND PANJENG = :PANJENG                                          ");
            }

            if (!bDel)
            {
                parameter.AppendSql("   AND DELDATE IS NULL                                         ");
            }
            parameter.AppendSql(" ORDER BY CODE                                                     ");

            if (strPan != "*" && !strPan.IsNullOrEmpty())
            {
                parameter.Add("PANJENG", strPan, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            
            return ExecuteReader<HIC_SPC_SCODE>(parameter);
        }

        public List<HIC_SPC_SCODE> GetCodeNameby()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_SCODE_NEW   ");
            parameter.AppendSql(" WHERE DELDATE IS NULL             ");
            parameter.AppendSql("   AND CODE IS NOT NULL            ");
            parameter.AppendSql("   AND NAME IS NOT NULL            ");
            parameter.AppendSql(" ORDER BY NAME                     ");

            return ExecuteReader<HIC_SPC_SCODE>(parameter);
        }
    }
}
