namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicHyangApproveJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicHyangApproveJepsuRepository()
        {
        }

        public List<HIC_HYANG_APPROVE_JEPSU> GetItembyBDate(string strDate, string strSName, string strJob, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.Pano,a.SName,a.Age,a.Sex,a.SuCode,a.EntQty,a.EntQty2,a.Print,a.DrSabun    ");
            parameter.AppendSql("     , a.GbSite,a.DeptCode,a.PTno                                                  ");
            parameter.AppendSql("     , TO_CHAR(a.BDate,'YYYY-MM-DD') BDate                                         ");
            parameter.AppendSql("     , TO_CHAR(a.OcsSendTime,'YYYY-MM-DD HH24:MI') OcsSendTime                     ");
            parameter.AppendSql("     , TO_CHAR(a.ApproveTime,'YYYY-MM-DD HH24:MI') ApproveTime,a.ROWID AS RID      ");
            parameter.AppendSql("     , KOSMOS_OCS.FC_INSA_MST_KORNAME(a.DRSABUN) DRNAME                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE a, KOSMOS_PMPA.HEA_JEPSU b                    ");
            parameter.AppendSql(" WHERE a.BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql(" AND a.GBSITE = :GBSITE                                                            ");
            
            if (strJob == "1")
            {
                parameter.AppendSql("   AND a.ApproveTime IS NULL                                                   ");
                //parameter.AppendSql("   AND a.DeptCode = 'TO'                                                       ");
            }
            if (strGubun =="1")
            {
                parameter.AppendSql("   AND a.DeptCode IN ('HR','TO')                                               ");
            }
            else if(strGubun =="2")
            {
                parameter.AppendSql("   AND a.DeptCode IN ('HR','TO')                                               ");
            }

            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                          ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                           ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                                           ");
            if (!strSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.SNAME = :SNAME                                                        ");
            }
            parameter.AppendSql("ORDER BY a.BDate,a.Pano,a.SuCode                                                   ");

            parameter.Add("BDATE", strDate);
            if (!strSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", strSName);
            }

            parameter.Add("GBSITE", strGubun);
            

            return ExecuteReader<HIC_HYANG_APPROVE_JEPSU>(parameter);
        }
    }
}
