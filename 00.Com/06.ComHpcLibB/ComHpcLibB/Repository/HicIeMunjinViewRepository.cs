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
    public class HicIeMunjinViewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicIeMunjinViewRepository()
        {
        }

        public int GetAllbyViewKey(string fstrROWID, string gstrSysDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT COUNT('X') CNT                                 ");
            parameter.AppendSql("   FROM ADMIN.HIC_IE_MUNJIN_VIEW                 ");
            parameter.AppendSql("  WHERE VIEWKEY  = :VIEWKEY                            ");
            parameter.AppendSql("    AND VIEWDATE = :VIEWDATE                           ");

            parameter.Add("VIEWKEY", fstrROWID);
            parameter.Add("VIEWDATE", gstrSysDate);

            return ExecuteScalar<int>(parameter);
        }

        public string GetViewIdbyViewKey(string fstrROWID, string gstrSysDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT VIEWID                                         ");
            parameter.AppendSql("   FROM ADMIN.HIC_IE_MUNJIN_VIEW                 ");
            parameter.AppendSql("  WHERE VIEWKEY  = :VIEWKEY                            ");
            parameter.AppendSql("    AND VIEWDATE = :VIEWDATE                           ");
            parameter.AppendSql("    AND ViewID IS NOT NULL                             ");

            parameter.Add("VIEWKEY", fstrROWID);
            parameter.Add("VIEWDATE", gstrSysDate);

            return ExecuteScalar<string>(parameter);
        }

        public int Insert(string gstrSysDate, string fstrROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_IE_MUNJIN_VIEW     ");
            parameter.AppendSql("       (VIEWDATE                               ");
            if (!fstrROWID.IsNullOrEmpty())
            {
                parameter.AppendSql("       , VIEWKEY                           ");
            }
            parameter.AppendSql("       )                                       ");
            parameter.AppendSql("VALUES                                         ");
            parameter.AppendSql("       (:VIEWDATE                              ");
            if (!fstrROWID.IsNullOrEmpty())
            {
                parameter.AppendSql("       , :VIEWKEY                          ");
            }
            parameter.AppendSql("       )                                       ");

            parameter.Add("VIEWDATE", gstrSysDate);
            if (!fstrROWID.IsNullOrEmpty())
            {
                parameter.Add("VIEWKEY", fstrROWID);
            }

            return ExecuteNonQuery(parameter);
        }
    }
}
