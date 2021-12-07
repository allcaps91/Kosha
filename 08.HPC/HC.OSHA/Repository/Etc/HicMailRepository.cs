namespace HC.OSHA.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC_Core.Service;
    
    /// <summary>
    /// 
    /// </summary>
    public class HicMailRepository : BaseRepository
    {
        internal int Insert(HIC_OSHA_MAIL_SEND item)
        {
            if(item.ID == 0)
            {
                item.ID = GetSequenceNextVal("HIC_OSHA_MAIL_SEND_SEQ");
            }

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_MAIL_SEND (");
            parameter.AppendSql("    ID                          ");
            parameter.AppendSql("  , SITE_ID                     ");
            parameter.AppendSql("  , SEND_DATE                   ");
            parameter.AppendSql("  , SEND_USER                   ");
            parameter.AppendSql("  , SEND_TYPE                   ");
            parameter.AppendSql("  , CREATED                     ");
            parameter.AppendSql("  , CREATEDUSER                 ");
            parameter.AppendSql("  , WRTNO,                      ");
            parameter.AppendSql("  , SWLICENSE                   ");
            parameter.AppendSql(") VALUES (                      ");
            parameter.AppendSql("    :ID                         ");
            parameter.AppendSql("  , :SITE_ID                    ");
            parameter.AppendSql("  , SYSDATE                     ");
            parameter.AppendSql("  , :SEND_USER                  ");
            parameter.AppendSql("  , :SEND_TYPE                  ");
            parameter.AppendSql("  , SYSTIMESTAMP                ");
            parameter.AppendSql("  , :CREATEDUSER                ");
            parameter.AppendSql("  , :WRTNO,                     ");
            parameter.AppendSql("  , :SWLICENSE                  ");
            parameter.AppendSql(")                               ");

            parameter.Add("ID", item.ID);
            parameter.Add("SITE_ID", item.SITE_ID);
            parameter.Add("SEND_USER", item.SEND_USER);
            parameter.Add("SEND_TYPE", item.SEND_TYPE);
            parameter.Add("CREATEDUSER", item.SEND_USER);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            DataSyncService.Instance.Insert("HIC_OSHA_MAIL_SEND", item.ID);

            return ExecuteNonQuery(parameter);
        }

        internal List<HIC_OSHA_MAIL_SEND> FindBySiteIdAndType(long siteId, string type)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID                                  ");
            parameter.AppendSql("     , A.SITE_ID                             ");
            parameter.AppendSql("     , A.SEND_DATE                           ");
            parameter.AppendSql("     , A.SEND_USER                           ");
            parameter.AppendSql("     , A.SEND_TYPE                           ");
            parameter.AppendSql("     , B.NAME AS USERNAME                    ");
            parameter.AppendSql("  FROM HIC_OSHA_MAIL_SEND A                  ");
            parameter.AppendSql("  LEFT OUTER JOIN HIC_USERS B                 ");
            parameter.AppendSql("               ON A.SEND_USER = TRIM(B.USERID)");
            parameter.AppendSql(" WHERE SITE_ID      = :SITE_ID               ");
            parameter.AppendSql("   AND SEND_TYPE    = :SEND_TYPE             ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE1             ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE2             ");
            parameter.AppendSql("ORDER BY SEND_DATE DESC                      ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("SEND_TYPE", type);
            parameter.Add("SWLICENSE1", clsType.HosInfo.SwLicense);
            parameter.Add("SWLICENSE2", clsType.HosInfo.SwLicense);
            return ExecuteReader<HIC_OSHA_MAIL_SEND>(parameter);
        }

        internal HIC_OSHA_MAIL_SEND FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID                    ");
            parameter.AppendSql("     , A.SITE_ID               ");
            parameter.AppendSql("     , A.SEND_DATE             ");
            parameter.AppendSql("     , A.SEND_USER             ");
            parameter.AppendSql("     , A.SEND_TYPE             ");
            parameter.AppendSql("  FROM HIC_OSHA_MAIL_SEND A    ");
            parameter.AppendSql(" WHERE ID = :ID                ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE   ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HIC_OSHA_MAIL_SEND>(parameter);
        }

        internal HIC_OSHA_MAIL_SEND InsertAndSelect(HIC_OSHA_MAIL_SEND saved)
        {
            saved.ID = GetSequenceNextVal("HIC_OSHA_MAIL_SEND_SEQ");
            Insert(saved);
            return FindOne(saved.ID);
        }
    }
}
