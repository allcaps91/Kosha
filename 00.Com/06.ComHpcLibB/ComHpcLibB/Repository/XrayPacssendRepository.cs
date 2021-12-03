namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class XrayPacssendRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public XrayPacssendRepository()
        {
        }

        public void InsertData(XRAY_PACSSEND item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.XRAY_PACSSEND (                                            ");
            parameter.AppendSql("       ENTDATE,PACSNO,SENDGBN,PANO,SNAME                                           ");
            parameter.AppendSql("      ,SEX,AGE,IPDOPD,DEPTCODE,DRCODE,XJONG,XSUBCODE                               ");
            parameter.AppendSql("      ,XCODE,ORDERCODE,SEEKDATE,REMARK,XRAYROOM,XRAYNAME                           ");

            parameter.AppendSql(" ) VALUES (                                                                        ");
            parameter.AppendSql("       SYSDATE,:PACSNO,:SENDGBN,:PANO,:SNAME                                       ");
            parameter.AppendSql("      ,:SEX,:AGE,:IPDOPD,:DEPTCODE,:DRCODE,:XJONG,:XSUBCODE                        ");
            parameter.AppendSql("      ,:XCODE,:ORDERCODE,:SEEKDATE,:REMARK,:XRAYROOM,:XRAYNAME                     ");
            parameter.AppendSql(" )                                                                                 ");

            parameter.Add("PACSNO", item.PACSNO);
            parameter.Add("SENDGBN", item.SENDGBN);
            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("IPDOPD", item.IPDOPD);
            parameter.Add("DEPTCODE", item.DEPTCODE);
            parameter.Add("DRCODE", item.DRCODE);
            parameter.Add("XJONG", item.XJONG);
            parameter.Add("XSUBCODE", item.XSUBCODE);
            parameter.Add("XCODE", item.XCODE);
            parameter.Add("ORDERCODE", item.ORDERCODE);
            parameter.Add("SEEKDATE", item.SEEKDATE);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("XRAYROOM", item.XRAYROOM);
            parameter.Add("XRAYNAME", item.XRAYNAME);

            ExecuteNonQuery(parameter);
        }
    }
}
