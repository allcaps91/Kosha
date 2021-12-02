namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaSiteRepository : BaseRepository
    {
        public HC_OSHA_SITE FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_SITE WHERE ID = :ID ");
            parameter.Add("ID", id);
            return ExecuteReaderSingle<HC_OSHA_SITE>(parameter);

        }

        /// <summary>
        /// 미수형성시 원청으로 청구할 것인지 여부를 설정
        /// </summary>
        /// <param name="id"></param>
        public void UpdateCharge(long siteId, string ISPARENTCHARGE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE   ");
            parameter.AppendSql("SET ISPARENTCHARGE = :ISPARENTCHARGE , MODIFIED = SYSTIMESTAMP, MODIFIEDUSER  = :MODIFIEDUSER    ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.Add("ID", siteId);
            parameter.Add("ISPARENTCHARGE", ISPARENTCHARGE);
            parameter.Add("MODIFIEDUSER", clsType.User.Sabun);

            ExecuteNonQuery(parameter);
        }
        public void UpdateQuarterCharge(long siteId, string ISQUARTERCHARGE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE   ");
            parameter.AppendSql("SET ISQUARTERCHARGE = :ISQUARTERCHARGE , MODIFIED = SYSTIMESTAMP, MODIFIEDUSER  = :MODIFIEDUSER   ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.Add("ID", siteId);
            parameter.Add("ISQUARTERCHARGE", ISQUARTERCHARGE);
            parameter.Add("MODIFIEDUSER", clsType.User.Sabun);

            ExecuteNonQuery(parameter);
        }

        public void InactiveOsha(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE       ");
            parameter.AppendSql("SET ISACTIVE = 'N', MODIFIED = SYSTIMESTAMP, MODIFIEDUSER  = :MODIFIEDUSER        ");
            parameter.AppendSql("WHERE ID = :ID             ");
            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", clsType.User.Sabun);
           
            ExecuteNonQuery(parameter);

        }
        public void ActiveOsha(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE       ");
            parameter.AppendSql("SET ISACTIVE = 'Y', ISPARENTCHARGE = 'Y', MODIFIED = SYSTIMESTAMP, MODIFIEDUSER  = :MODIFIEDUSER        ");
            parameter.AppendSql("WHERE ID = :ID             ");
            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", clsType.User.Sabun);

            ExecuteNonQuery(parameter);

        }
        public void Insert(HC_SITE_VIEW view)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_SITE");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , ISACTIVE");
            parameter.AppendSql("  , TASKNAME");
            parameter.AppendSql("  , PARENTSITE_ID");
            parameter.AppendSql("  , HASCHILD");
            parameter.AppendSql("  , ISPARENTCHARGE");
            parameter.AppendSql("  , ISQUARTERCHARGE");
            parameter.AppendSql("  , MODIFIED");
            parameter.AppendSql("  , MODIFIEDUSER");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , 'Y'");
            parameter.AppendSql("  , :TASKNAME");
            parameter.AppendSql("  , 0"  );
            parameter.AppendSql("  , 'N' ");
            parameter.AppendSql("  , 'Y' ");
            parameter.AppendSql("  , 'N' ");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :MODIFIEDUSER");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql(") ");

            parameter.Add("ID", view.ID);
            parameter.Add("TASKNAME", "사업장 등록");
            parameter.Add("MODIFIEDUSER", clsType.User.Sabun);
            parameter.Add("CREATEDUSER", clsType.User.Sabun);


            ExecuteNonQuery(parameter);
        }

        public void UpdateParentSite(long siteId, long parentSiteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE            ");
            parameter.AppendSql("SET ");
            parameter.AppendSql("    PARENTSITE_ID = :PARENTSITE_ID");
            parameter.AppendSql("    ,ISPARENTCHARGE = 'N'         ");
            parameter.AppendSql("   ,MODIFIED = SYSTIMESTAMP");
            parameter.AppendSql("   ,MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("WHERE ID = :ID");

            parameter.Add("ID", siteId);
            parameter.Add("PARENTSITE_ID", parentSiteId);
            parameter.Add("MODIFIEDUSER", clsType.User.Sabun);
            ExecuteNonQuery(parameter);


            parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE     ");
            parameter.AppendSql("SET ");
            parameter.AppendSql("    HASCHILD = 'Y'       ");
            parameter.AppendSql("WHERE ID = :PARENTSITE_ID");
            
            parameter.Add("PARENTSITE_ID", parentSiteId);

            ExecuteNonQuery(parameter);
        }
        public void UpdateCandelParentSite(long siteId, long parentSiteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE ");
            parameter.AppendSql("SET ");
            parameter.AppendSql("    HASCHILD = 'N'  ");
            parameter.AppendSql("  , PARENTSITE_ID = null ");
            parameter.AppendSql("  , ISPARENTCHARGE ='Y' ");
            parameter.AppendSql("WHERE ID = :ID ");

            parameter.Add("ID", siteId);

            ExecuteNonQuery(parameter);

            parameter = CreateParameter();
            parameter.AppendSql("SELECT count(*) FROM HIC_OSHA_SITE       ");     
            parameter.AppendSql("WHERE PARENTSITE_ID = :PARENTSITE_ID ");
            parameter.Add("PARENTSITE_ID", parentSiteId);

            long count = ExecuteScalar<long>(parameter);
            if(count <= 0)
            {
                //하청사업장이 없으면 
                parameter = CreateParameter();
                parameter.AppendSql("UPDATE HIC_OSHA_SITE    ");
                parameter.AppendSql("SET ");
                parameter.AppendSql("    HASCHILD = 'N'       ");
                parameter.AppendSql("WHERE ID = :PARENTSITE_ID");

                parameter.Add("PARENTSITE_ID", parentSiteId);

                ExecuteNonQuery(parameter);
            }


            
        }

        public int GetMisuTaxUpdate(MISU_TAX item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   UPDATE MISU_TAX SET ");

            parameter.AppendSql("   PUMMOK = :PUMMOK,                     ");
            parameter.AppendSql("   LTDCODE = :LTDCODE,                   ");
            parameter.AppendSql("   DAEPYONAME = :DAEPYONAME,                 ");
            parameter.AppendSql("   UPTAE = :UPTAE,                       ");
            parameter.AppendSql("   JONGMOK = :JONGMOK,                   ");
            parameter.AppendSql("   LTDNO = :LTDNO,                      ");
            parameter.AppendSql("   LTDJUSO = :LTDJUSO,                      ");
            parameter.AppendSql("   LTDNAME = :LTDNAME,                    ");
            parameter.AppendSql("   ENTSABUN = :ENTSABUN                  ");
            parameter.AppendSql("   WHERE MISUNO= :MISUNO                 ");

            
            parameter.Add("PUMMOK",             item.PUMMOK         );
            parameter.Add("LTDCODE",            item.LTDCODE        );
            parameter.Add("UPTAE",              item.UPTAE          );
            parameter.Add("DAEPYONAME",         item.DAEPYONAME     );
            parameter.Add("JONGMOK",            item.JONGMOK        );
            parameter.Add("LTDNO",              item.LTDNO          );
            parameter.Add("LTDJUSO",            item.LTDJUSO        );
            parameter.Add("LTDNAME",            item.LTDNAME, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("ENTSABUN",           item.ENTSABUN       );
            parameter.Add("MISUNO",             item.MISUNO         );

            return ExecuteNonQuery(parameter);
        }
    }
}
