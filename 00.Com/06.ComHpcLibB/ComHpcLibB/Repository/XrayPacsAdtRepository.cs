namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class XrayPacsAdtRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public XrayPacsAdtRepository()
        {
        }

        public int Insert(XRAY_PACS_ADT item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO PACS.XRAY_PACS_ADT(                                                ");
            parameter.AppendSql(" QUEUEID,FLAG,WORKTIME,PATID,EVENTTYPE                                         ");
            parameter.AppendSql(" ,BIRTHDAY, DEPT,ATTENDDOCT1,PATNAME,PATTYPE,PERSONALID,SEX)                   ");
            parameter.AppendSql("VALUES                                                                         ");
            parameter.AppendSql(" (:QUEUEID, 'N', SYSDATE, :PATID, 'A04'                                        ");
            parameter.AppendSql(" ,:BIRTHDAY,'HR','',:PATNAME,'0',:PERSONALID, :SEX)                            ");

            parameter.Add("QUEUEID", item.QUEUEID);
            parameter.Add("PATID", item.PATID);
            parameter.Add("BIRTHDAY", item.BIRTHDAY);
            parameter.Add("PATNAME", item.PATNAME);
            parameter.Add("PERSONALID", item.PERSONALID);
            parameter.Add("SEX", item.SEX);

            return ExecuteNonQuery(parameter);
        }

        public XRAY_PACS_ADT GetItembyPATID(string strPANO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PATID                                       ");
            parameter.AppendSql("  FROM PACS.XRAY_PACS_ADT                          ");
            parameter.AppendSql(" WHERE 1=1                                         ");
            parameter.AppendSql("   AND PATID = :PANO                             ");

            parameter.Add("PANO", strPANO);

            return ExecuteReaderSingle<XRAY_PACS_ADT>(parameter);
        }

    }
}
