namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicScodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicScodeRepository()
        {
        }

        public List<HIC_SCODE> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE,NAME,PANJENG,CHASU,SCODE, JCODE,ROWID AS RID ");
            parameter.AppendSql("  FROM ADMIN.HIC_SCODE                       ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");            
            parameter.AppendSql(" ORDER BY CODE                                     ");

            return ExecuteReader<HIC_SCODE>(parameter);
        }

        public void Delete(HIC_SCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_SCODE               ");
            parameter.AppendSql(" WHERE ROWID =:RID                         ");

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Update(HIC_SCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_SCODE  ");
            parameter.AppendSql("   SET CODE     =:CODE        ");
            parameter.AppendSql("      ,NAME     =:NAME        ");
            parameter.AppendSql("      ,PANJENG  =:PANJENG     ");
            parameter.AppendSql("      ,CHASU    =:CHASU       ");
            parameter.AppendSql("      ,SCODE    =:SCODE       ");
            parameter.AppendSql("      ,JCODE    =:JCODE       ");
            parameter.AppendSql("      ,ENTSABUN =ENTSABUN     ");
            parameter.AppendSql("      ,ENTTIME  =SYSDATE      ");
            parameter.AppendSql(" WHERE ROWID    =:RID         ");

            parameter.Add("CODE",      code.CODE);
            parameter.Add("NAME",      code.NAME);
            parameter.Add("PANJENG",   code.PANJENG);
            parameter.Add("CHASU",     code.CHASU);
            parameter.Add("SCODE",     code.SCODE);
            parameter.Add("JCODE",     code.JCODE);
            parameter.Add("ENTSABUN",  clsType.User.IdNumber);
            parameter.Add("RID",       code.RID);

            ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_SCODE code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT ADMIN.HIC_SCODE (                                             ");
            parameter.AppendSql("   CODE,NAME,PANJENG,CHASU,SCODE,JCODE,ENTSABUN,ENTDATE                    ");
            parameter.AppendSql(" ) VALUES (                                                                ");
            parameter.AppendSql("  :CODE,:NAME,:PANJENG,:CHASU,:SCODE,:JCODE,:ENTSABUN,SYSDATE )            ");

            parameter.Add("CODE",     code.CODE);
            parameter.Add("NAME",     code.NAME);
            parameter.Add("PANJENG",  code.PANJENG);
            parameter.Add("CHASU",    code.CHASU);
            parameter.Add("SCODE",    code.SCODE);
            parameter.Add("JCODE",    code.JCODE);
            parameter.Add("ENTSABUN", clsType.User.IdNumber);

            ExecuteNonQuery(parameter);
        }
    }
}
